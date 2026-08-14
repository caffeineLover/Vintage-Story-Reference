using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.Common;
using Vintagestory.Common.Network.Packets;
using Vintagestory.Server.Systems;

namespace Vintagestory.Server;

public class PhysicsManager : LoadBalancedTask
{
	private class PhysicsOffthreadTasks
	{
		private readonly ConcurrentQueue<Action> queue = new ConcurrentQueue<Action>();

		private readonly PhysicsManager physicsManager;

		private bool idle;

		private bool busy;

		internal FastMemoryStream buffer = new FastMemoryStream();

		public PhysicsOffthreadTasks(PhysicsManager manager)
		{
			physicsManager = manager;
		}

		internal void Start()
		{
			physicsManager.server.EventManager.TriggerPhysicsThreadStart();
			while (!physicsManager.ShouldExit())
			{
				if (queue.IsEmpty)
				{
					try
					{
						idle = true;
						lock (this)
						{
							Monitor.Wait(this, 10);
						}
						idle = false;
					}
					catch (ThreadInterruptedException)
					{
					}
				}
				Action a;
				while (queue.TryDequeue(out a))
				{
					busy = true;
					if (physicsManager.ShouldExit())
					{
						break;
					}
					try
					{
						a();
					}
					catch (Exception e)
					{
						ServerMain.Logger.Error(e);
					}
				}
				busy = false;
			}
		}

		internal void QueueAsyncTask(Action a)
		{
			if (physicsManager.server.ReducedServerThreads)
			{
				a();
				return;
			}
			queue.Enqueue(a);
			if (!idle)
			{
				return;
			}
			lock (this)
			{
				Monitor.Pulse(this);
			}
		}

		internal bool Busy()
		{
			if (!busy)
			{
				return !queue.IsEmpty;
			}
			return true;
		}
	}

	private const int HiResTrackingRange = 2500;

	public const float AttributesToClientsInterval = 0.2f;

	private const int firstTick = 1;

	public readonly ConcurrentQueue<IPhysicsTickable> toAdd = new ConcurrentQueue<IPhysicsTickable>();

	public readonly ConcurrentQueue<IPhysicsTickable> toRemove = new ConcurrentQueue<IPhysicsTickable>();

	private const float tickInterval = 1f / 30f;

	private readonly ICoreServerAPI sapi;

	private readonly ServerUdpNetwork udpNetwork;

	public readonly IServerNetworkChannel AnimationsAndTagsChannel;

	private readonly ServerMain server;

	private readonly LoadBalancer loadBalancer;

	private int maxPhysicsThreads;

	private int ticksToDo;

	private readonly long listener;

	private float physicsTickAccum;

	private float attrUpdateAccum;

	private readonly List<IPhysicsTickable> tickables = new List<IPhysicsTickable>();

	private ServerSystemEntitySimulation es;

	private List<Packet_EntityAttributes> cliententitiesFullUpdate = new List<Packet_EntityAttributes>();

	private List<Packet_EntityAttributeUpdate> cliententitiesPartialUpdate = new List<Packet_EntityAttributeUpdate>();

	private List<Packet_EntityAttributes> cliententitiesDebugUpdate = new List<Packet_EntityAttributes>();

	private Dictionary<long, Packet_EntityAttributes>[] entitiesFullUpdate;

	private Dictionary<long, Packet_EntityAttributeUpdate>[] entitiesPartialUpdate;

	private Dictionary<long, Packet_EntityAttributes>[] entitiesDebugUpdate;

	private Dictionary<long, Packet_EntityPosition>[] entitiesPositionPackets;

	private Dictionary<long, AnimationPacket>[] entitiesAnimPackets;

	private ConcurrentDictionary<long, EntityTagPacket> entitiesTagPackets;

	private FastMemoryStream[] entitiesUpdateReusableBuffers;

	private double[] positions = new double[3];

	private readonly EntityDespawnData outofRangeDespawnData = new EntityDespawnData
	{
		Reason = EnumDespawnReason.OutOfRange
	};

	private List<long> alreadyTracked = new List<long>();

	private List<Entity> newlyTracked = new List<Entity>();

	private List<ConnectedClient> ClientList;

	private ConcurrentBag<Entity> stateChanges = new ConcurrentBag<Entity>();

	private PhysicsOffthreadTasks offthreadProcess;

	private int currentTick;

	private static float rateModifier = 1f;

	private float deltaT;

	public PhysicsManager(ICoreServerAPI sapi, ServerUdpNetwork udpNetwork)
	{
		this.sapi = sapi;
		this.udpNetwork = udpNetwork;
		AnimationsAndTagsChannel = sapi.Network.RegisterChannel("EntityAnims");
		AnimationsAndTagsChannel.RegisterMessageType<AnimationPacket>().RegisterMessageType<BulkAnimationPacket>().RegisterMessageType<EntityTagPacket>();
		maxPhysicsThreads = Math.Clamp(MagicNum.MaxPhysicsThreads, 1, 8);
		if (sapi.Server.ReducedServerThreads)
		{
			maxPhysicsThreads = 1;
		}
		entitiesFullUpdate = new Dictionary<long, Packet_EntityAttributes>[maxPhysicsThreads];
		entitiesPartialUpdate = new Dictionary<long, Packet_EntityAttributeUpdate>[maxPhysicsThreads];
		entitiesDebugUpdate = new Dictionary<long, Packet_EntityAttributes>[maxPhysicsThreads];
		entitiesPositionPackets = new Dictionary<long, Packet_EntityPosition>[maxPhysicsThreads];
		entitiesAnimPackets = new Dictionary<long, AnimationPacket>[maxPhysicsThreads];
		entitiesTagPackets = new ConcurrentDictionary<long, EntityTagPacket>(maxPhysicsThreads, 32);
		entitiesUpdateReusableBuffers = new FastMemoryStream[maxPhysicsThreads];
		for (int i = 0; i < entitiesFullUpdate.Length; i++)
		{
			entitiesFullUpdate[i] = new Dictionary<long, Packet_EntityAttributes>();
		}
		for (int j = 0; j < entitiesPartialUpdate.Length; j++)
		{
			entitiesPartialUpdate[j] = new Dictionary<long, Packet_EntityAttributeUpdate>();
		}
		for (int k = 0; k < entitiesDebugUpdate.Length; k++)
		{
			entitiesDebugUpdate[k] = new Dictionary<long, Packet_EntityAttributes>();
		}
		for (int l = 0; l < entitiesPositionPackets.Length; l++)
		{
			entitiesPositionPackets[l] = new Dictionary<long, Packet_EntityPosition>();
		}
		for (int m = 0; m < entitiesAnimPackets.Length; m++)
		{
			entitiesAnimPackets[m] = new Dictionary<long, AnimationPacket>();
		}
		for (int n = 0; n < entitiesUpdateReusableBuffers.Length; n++)
		{
			entitiesUpdateReusableBuffers[n] = new FastMemoryStream();
		}
		server = sapi.World as ServerMain;
		loadBalancer = new LoadBalancer(this, ServerMain.Logger);
		loadBalancer.CreateDedicatedThreads(maxPhysicsThreads, "physicsManager", server.Serverthreads);
		offthreadProcess = new PhysicsOffthreadTasks(this);
		if (!server.ReducedServerThreads)
		{
			Thread thread = TyronThreadPool.CreateDedicatedThread(offthreadProcess.Start, "physicsManagerHelper");
			thread.IsBackground = true;
			thread.Priority = Thread.CurrentThread.Priority;
			server.Serverthreads.Add(thread);
		}
		listener = server.RegisterGameTickListener(ServerTick, 1);
		rateModifier = 1f;
		PhysicsBehaviorBase.InitServerMT(sapi);
	}

	public void Init()
	{
		es = server.Systems.First((ServerSystem s) => s is ServerSystemEntitySimulation) as ServerSystemEntitySimulation;
		es.physicsManager = this;
	}

	public void ForceClientUpdateTick(ConnectedClient client)
	{
		attrUpdateAccum = 0.2f;
	}

	public void ServerTick(float dt)
	{
		ServerMain.FrameProfiler.Enter("physicsmanager-servertick");
		IPhysicsTickable addable;
		while (!toAdd.IsEmpty && toAdd.TryDequeue(out addable))
		{
			if (addable != null && addable.Entity != null)
			{
				if (addable.Entity.ServerBehaviorsThreadsafe == null)
				{
					ServerMain.Logger.Warning("An entity " + addable.Entity.Code.ToShortString() + " failed to complete initialisation, will not be physics ticked.");
				}
				else
				{
					tickables.Add(addable);
				}
			}
		}
		IPhysicsTickable removable;
		while (!toRemove.IsEmpty && toRemove.TryDequeue(out removable))
		{
			if (removable != null)
			{
				tickables.Remove(removable);
			}
		}
		physicsTickAccum += dt;
		deltaT = dt;
		if (physicsTickAccum > 0.4f)
		{
			int skippedTicks = (int)((physicsTickAccum - 0.4f) / (1f / 30f));
			if (ServerMain.FrameProfiler.Enabled)
			{
				ServerMain.Logger.Warning("Over 400ms tick. Skipping {0} physics ticks.", skippedTicks);
			}
			physicsTickAccum %= 0.4f;
		}
		ServerMain.FrameProfiler.Mark("physicsmanager-preparation");
		ticksToDo = (int)(physicsTickAccum / (1f / 30f));
		physicsTickAccum -= (float)ticksToDo * (1f / 30f);
		long timeout = Environment.TickCount + 1000;
		while (offthreadProcess.Busy() && Environment.TickCount < timeout)
		{
			Thread.Sleep(0);
		}
		ServerMain.FrameProfiler.Mark("physicsmanager-waitingOnPreviousTick");
		BuildClientList(server.Clients.Values);
		attrUpdateAccum += dt;
		if (attrUpdateAccum >= 0.2f)
		{
			attrUpdateAccum = 0f;
		}
		ServerMain.FrameProfiler.Mark("physicsmanager-buildclientlist");
		int threadsWorking = 1;
		if (ticksToDo > 0)
		{
			stateChanges.Clear();
			if (tickables.Count > 800 && maxPhysicsThreads > 1)
			{
				threadsWorking = maxPhysicsThreads;
				loadBalancer.SynchroniseWorkToMainThread(this);
				loadBalancer.AwaitCompletionOnAllThreads(threadsWorking);
				ServerMain.FrameProfiler.Mark("physicsmanager-waitingForSlowestThread");
				if (attrUpdateAccum == 0f)
				{
					GatherUpdatePacketsFromAllThreads();
					ServerMain.FrameProfiler.Mark("physicsmanager-mergeThreadPackets");
				}
			}
			else
			{
				DoWork(0);
			}
			foreach (Entity entity in stateChanges)
			{
				ActiveStateChanged(entity);
			}
			stateChanges.Clear();
			currentTick += ticksToDo;
			float adjustedRate = (float)ticksToDo * (1f / 30f) * rateModifier;
			foreach (IPhysicsTickable tickable in tickables)
			{
				try
				{
					tickable.AfterPhysicsTick(adjustedRate);
				}
				catch (Exception e)
				{
					ServerMain.Logger.Error(e);
				}
			}
			ServerMain.FrameProfiler.Mark("physicsmanager-afterphysicstick");
		}
		SendPositionsForNonTickableEntities(attrUpdateAccum == 0f);
		ServerMain.FrameProfiler.Mark("physicsmanager-nontickables");
		if (attrUpdateAccum == 0f)
		{
			foreach (ConnectedClient client in ClientList)
			{
				UpdateTrackedEntityLists(client, threadsWorking);
			}
			ServerMain.FrameProfiler.Mark("physicsmanager-updatetrackedentitylists");
			SendTrackedEntitiesStateChanges();
			offthreadProcess.QueueAsyncTask(delegate
			{
				SendAttributesViaTCP(ClientList);
			});
		}
		Entity[] spawnsToSend = null;
		int spawnsCount = 0;
		lock (server.EntitySpawnSendQueue)
		{
			List<Entity> SendQueue = server.EntitySpawnSendQueue;
			spawnsCount = SendQueue.Count;
			if (spawnsCount > 0)
			{
				spawnsToSend = new Entity[spawnsCount];
				for (int i = 0; i < spawnsToSend.Length; i++)
				{
					Entity entity2 = SendQueue[i];
					if (!entity2.ShouldDespawn)
					{
						spawnsToSend[i] = entity2;
					}
					else
					{
						spawnsCount--;
					}
				}
				SendQueue.Clear();
			}
		}
		ServerMain.FrameProfiler.Mark("physicsmanager-sendspawnlockwaiting");
		if (spawnsCount > 0)
		{
			if (PrepareEntitySpawns(spawnsToSend, ClientList) > 0)
			{
				Dictionary<long, Packet_EntityPosition> entityPositionPackets = entitiesPositionPackets[0];
				entityPositionPackets.Clear();
				DoFirstPhysicsTicks(spawnsToSend, entityPositionPackets);
				ServerMain.spawnSendQueue++;
				offthreadProcess.QueueAsyncTask(delegate
				{
					SendEntitySpawns(spawnsToSend, ClientList, entityPositionPackets);
					ServerMain.spawnSendQueue--;
				});
			}
			ServerMain.FrameProfiler.Mark("physicsmanager-sendspawns");
		}
		ServerMain.FrameProfiler.Leave();
	}

	private void BuildClientList(ICollection<ConnectedClient> values)
	{
		List<ConnectedClient> clients = (ClientList = new List<ConnectedClient>());
		foreach (ConnectedClient client in values)
		{
			if (client.State.ConnectedOrPlaying() && client.Entityplayer != null)
			{
				clients.Add(client);
			}
		}
		if (this.positions.Length < clients.Count * 3)
		{
			this.positions = new double[clients.Count * 3];
		}
		double[] positions = this.positions;
		int k = 0;
		foreach (ConnectedClient client2 in clients)
		{
			EntityPos pos = client2.Position;
			positions[k] = pos.X;
			positions[k + 1] = pos.Y;
			positions[k + 2] = pos.Z;
			k += 3;
			if (client2.threadedTrackedEntities == null)
			{
				List<Entity>[] threadedTracked = (client2.threadedTrackedEntities = new List<Entity>[maxPhysicsThreads]);
				for (int i = 0; i < maxPhysicsThreads; i++)
				{
					threadedTracked[i] = new List<Entity>();
				}
			}
			else
			{
				List<Entity>[] threadedTracked2 = client2.threadedTrackedEntities;
				for (int j = 0; j < maxPhysicsThreads; j++)
				{
					threadedTracked2[j].Clear();
				}
			}
		}
	}

	public void UpdateTrackedEntitiesStates(ConnectedClient client)
	{
		List<ConnectedClient> clients = new List<ConnectedClient> { client };
		EntityPos pos = client.Position;
		positions[0] = pos.X;
		positions[1] = pos.Y;
		positions[2] = pos.Z;
		foreach (Entity entity in server.LoadedEntities.Values)
		{
			if (UpdateTrackedEntityState(entity, clients, 0))
			{
				ActiveStateChanged(entity);
			}
		}
		UpdateTrackedEntityLists(client, 1);
	}

	private bool UpdateTrackedEntityState(Entity entity, List<ConnectedClient> clients, int zeroBasedThreadNum)
	{
		EntityPos pos = entity.Pos;
		double x = pos.X;
		double y = pos.Y;
		double z = pos.Z;
		double minrangeSq = double.MaxValue;
		double simRangeSq = entity.SimulationRange * entity.SimulationRange;
		double trackRange = Math.Max(es.trackingRangeSq, simRangeSq);
		long entityId = entity.EntityId;
		long entityChunkIndex = entity.InChunkIndex3d;
		bool doTrackOutsideLoadedRange = entity.AllowOutsideLoadedRange;
		double[] positions = this.positions;
		int i = 0;
		foreach (ConnectedClient client in clients)
		{
			double num = x - positions[i];
			double dy = y - positions[i + 1];
			double dz = z - positions[i + 2];
			i += 3;
			double rangeSq = num * num + dz * dz + dy * dy;
			if (rangeSq < minrangeSq)
			{
				minrangeSq = rangeSq;
			}
			if (rangeSq < trackRange && (client.DidSendChunk(entityChunkIndex) || entityId == client.Player.Entity.EntityId || doTrackOutsideLoadedRange))
			{
				client.threadedTrackedEntities[zeroBasedThreadNum].Add(entity);
			}
		}
		if (minrangeSq < trackRange)
		{
			entity.IsTracked = (byte)((minrangeSq >= 2500.0) ? 1 : 2);
		}
		else
		{
			entity.IsTracked = 0;
			if (!(entity is EntityPlayer))
			{
				CompletePositionUpdate(entity);
			}
		}
		entity.NearestPlayerDistance = (float)Math.Sqrt(minrangeSq);
		if (!entity.AlwaysActive)
		{
			bool active = minrangeSq < simRangeSq;
			if (active != (entity.State == EnumEntityState.Active))
			{
				entity.State = ((!active) ? EnumEntityState.Inactive : EnumEntityState.Active);
				return true;
			}
		}
		return false;
	}

	private void ActiveStateChanged(Entity entity)
	{
		entity.OnStateChanged(entity.State ^ EnumEntityState.Inactive);
	}

	private void CompletePositionUpdate(Entity entity)
	{
		entity.PreviousServerPos.SetFrom(entity.Pos);
		entity.IsTeleport = false;
	}

	private void UpdateTrackedEntityLists(ConnectedClient client, int threadCount)
	{
		List<Entity>[] threadedTracked = client.threadedTrackedEntities;
		List<Entity> threadedTracked2 = threadedTracked[0];
		HashSet<long> cte = client.TrackedEntities;
		List<long> alreadyTracked = this.alreadyTracked;
		alreadyTracked.EnsureCapacity(cte.Count);
		List<Entity> newlyTracked = this.newlyTracked;
		foreach (Entity e in threadedTracked2)
		{
			long id;
			if (cte.Remove(id = e.EntityId))
			{
				alreadyTracked.Add(id);
			}
			else
			{
				newlyTracked.Add(e);
			}
		}
		for (int i = 1; i < threadCount; i++)
		{
			foreach (Entity e2 in threadedTracked[i])
			{
				long id;
				if (cte.Remove(id = e2.EntityId))
				{
					alreadyTracked.Add(id);
				}
				else
				{
					newlyTracked.Add(e2);
				}
				threadedTracked2.Add(e2);
			}
			threadedTracked[i].Clear();
		}
		foreach (long untrackedId in cte)
		{
			client.entitiesNowOutOfRange.Add(new EntityDespawn
			{
				ForClientId = client.Id,
				DespawnData = outofRangeDespawnData,
				EntityId = untrackedId
			});
		}
		cte.Clear();
		cte.AddRange(alreadyTracked);
		alreadyTracked.Clear();
		foreach (Entity entity in newlyTracked)
		{
			if (cte.Count >= MagicNum.TrackedEntitiesPerClient)
			{
				break;
			}
			cte.Add(entity.EntityId);
			client.entitiesNowInRange.Add(new EntityInRange
			{
				ForClientId = client.Id,
				Entity = entity
			});
		}
		newlyTracked.Clear();
	}

	public void SendTrackedEntitiesStateChanges()
	{
		List<AnimationPacket> entityAnimPackets = new List<AnimationPacket>();
		FastMemoryStream ms = null;
		try
		{
			foreach (ConnectedClient client in ClientList)
			{
				if (client.entitiesNowInRange.Count > 0)
				{
					entityAnimPackets.Clear();
					if (ms == null)
					{
						ms = new FastMemoryStream();
					}
					foreach (EntityInRange nowInRange in client.entitiesNowInRange)
					{
						Entity entity = nowInRange.Entity;
						if (entity is EntityPlayer entityPlayer)
						{
							server.PlayersByUid.TryGetValue(entityPlayer.PlayerUID, out var value);
							if (value != null)
							{
								server.SendPacket(nowInRange.ForClientId, ((ServerWorldPlayerData)value.WorldData).ToPacketForOtherPlayers(value));
							}
						}
						ms.Reset();
						BinaryWriter writer = new BinaryWriter(ms);
						server.SendPacket(nowInRange.ForClientId, ServerPackets.GetFullEntityPacket(entity, ms, writer));
						if (entity.AnimManager != null)
						{
							entityAnimPackets.Add(new AnimationPacket(entity));
						}
					}
					BulkAnimationPacket bulkAnimationPacket = new BulkAnimationPacket
					{
						Packets = entityAnimPackets.ToArray()
					};
					AnimationsAndTagsChannel.SendPacket(bulkAnimationPacket, client.Player);
					client.entitiesNowInRange.Clear();
				}
				if (client.entitiesNowOutOfRange.Count > 0)
				{
					server.SendPacket(client.Id, ServerPackets.GetEntityDespawnPacket(client.entitiesNowOutOfRange));
					client.entitiesNowOutOfRange.Clear();
				}
			}
		}
		finally
		{
			ms?.Dispose();
		}
		ServerMain.FrameProfiler.Mark("physicsmanager-sendstatechanged");
	}

	public void GatherUpdatePacketsFromAllThreads()
	{
		Dictionary<long, Packet_EntityAttributes> eFullUpdate = entitiesFullUpdate[0];
		Dictionary<long, Packet_EntityAttributeUpdate> ePartialUpdate = entitiesPartialUpdate[0];
		Dictionary<long, Packet_EntityAttributes> eDebugUpdate = entitiesDebugUpdate[0];
		for (int i = 1; i < maxPhysicsThreads; i++)
		{
			eFullUpdate.AddRange(entitiesFullUpdate[i]);
			ePartialUpdate.AddRange(entitiesPartialUpdate[i]);
			eDebugUpdate.AddRange(entitiesDebugUpdate[i]);
			entitiesFullUpdate[i].Clear();
			entitiesPartialUpdate[i].Clear();
			entitiesDebugUpdate[i].Clear();
		}
	}

	public void SendAttributesViaTCP(List<ConnectedClient> clientList)
	{
		Dictionary<long, Packet_EntityAttributes> eFullUpdate = entitiesFullUpdate[0];
		Dictionary<long, Packet_EntityAttributeUpdate> ePartialUpdate = entitiesPartialUpdate[0];
		Dictionary<long, Packet_EntityAttributes> eDebugUpdate = entitiesDebugUpdate[0];
		List<Packet_EntityAttributes> cliententitiesFullUpdate = this.cliententitiesFullUpdate;
		List<Packet_EntityAttributeUpdate> cliententitiesPartialUpdate = this.cliententitiesPartialUpdate;
		List<Packet_EntityAttributes> cliententitiesDebugUpdate = this.cliententitiesDebugUpdate;
		bool debugMode = server.Config.EntityDebugMode;
		foreach (ConnectedClient client in clientList)
		{
			cliententitiesFullUpdate.Clear();
			cliententitiesPartialUpdate.Clear();
			cliententitiesDebugUpdate.Clear();
			try
			{
				try
				{
					foreach (Entity item in client.threadedTrackedEntities[0])
					{
						long entityId = item.EntityId;
						if (eFullUpdate.TryGetValue(entityId, out var pf))
						{
							cliententitiesFullUpdate.Add(pf);
						}
						if (ePartialUpdate.TryGetValue(entityId, out var pp))
						{
							cliententitiesPartialUpdate.Add(pp);
						}
						if (debugMode && eDebugUpdate.TryGetValue(entityId, out var pd))
						{
							cliententitiesDebugUpdate.Add(pd);
						}
					}
				}
				catch (InvalidOperationException)
				{
				}
				if (cliententitiesFullUpdate.Count > 0 || cliententitiesPartialUpdate.Count > 0)
				{
					server.SendPacket(client.Id, ServerPackets.GetBulkEntityAttributesPacket(cliententitiesFullUpdate, cliententitiesPartialUpdate));
				}
				if (cliententitiesDebugUpdate.Count > 0)
				{
					server.SendPacket(client.Id, ServerPackets.GetBulkEntityDebugAttributesPacket(cliententitiesDebugUpdate));
				}
			}
			finally
			{
				cliententitiesFullUpdate.Clear();
				cliententitiesPartialUpdate.Clear();
				cliententitiesDebugUpdate.Clear();
				client.threadedTrackedEntities[0].Clear();
			}
		}
		eFullUpdate.Clear();
		ePartialUpdate.Clear();
		eDebugUpdate.Clear();
	}

	public void BuildPositionPacket(Entity entity, bool forceUpdate, Dictionary<long, Packet_EntityPosition> entityPositionPackets, Dictionary<long, AnimationPacket> entityAnimPackets)
	{
		if (entity is EntityPlayer)
		{
			return;
		}
		EntityAgent entityAgent = entity as EntityAgent;
		if (entity.AnimManager != null && (entity.AnimManager.AnimationsDirty || entity.IsTeleport))
		{
			entityAnimPackets[entity.EntityId] = new AnimationPacket(entity);
			entity.AnimManager.AnimationsDirty = false;
		}
		if (forceUpdate || !entity.Pos.BasicallySameAs(entity.PreviousServerPos) || (entityAgent != null && entityAgent.Controls.Dirty) || entity.tagsDirty)
		{
			int tick = entity.Attributes.GetIntAndIncrement("tick");
			entityPositionPackets[entity.EntityId] = ServerPackets.getEntityPositionPacket(entity.Pos, entity, tick);
			if (entityAgent != null)
			{
				entityAgent.Controls.Dirty = false;
			}
			entity.tagsDirty = false;
		}
		CompletePositionUpdate(entity);
	}

	private EntityTagPacket BuildAttributesPackets(Entity entity, FastMemoryStream ms, bool debugMode, Dictionary<long, Packet_EntityAttributes> eFullUpdate, Dictionary<long, Packet_EntityAttributeUpdate> ePartialUpdate, Dictionary<long, Packet_EntityAttributes> eDebugUpdate)
	{
		EntityTagPacket result = null;
		SyncedTreeAttribute WatchedAttributes = entity.WatchedAttributes;
		if (WatchedAttributes.AllDirty)
		{
			ms.Reset();
			eFullUpdate[entity.EntityId] = ServerPackets.GetEntityPacket(ms, entity);
			entity.tagsDirty = false;
		}
		else
		{
			if (WatchedAttributes.PartialDirty)
			{
				ms.Reset();
				ePartialUpdate[entity.EntityId] = ServerPackets.GetEntityPartialAttributePacket(ms, entity);
			}
			if (entity.tagsDirty)
			{
				result = ServerPackets.GetEntityTagPacket(entity);
				entity.tagsDirty = false;
			}
		}
		if (debugMode && (entity.DebugAttributes.AllDirty || entity.DebugAttributes.PartialDirty))
		{
			ms.Reset();
			eDebugUpdate[entity.EntityId] = ServerPackets.GetEntityDebugAttributePacket(ms, entity);
		}
		WatchedAttributes.MarkClean();
		return result;
	}

	public void SendPositionsAndAnimations(Dictionary<long, Packet_EntityPosition> entityPositionPackets, Dictionary<long, AnimationPacket> entityAnimPackets, int zeroBasedThreadNum, bool stateUpdateTick)
	{
		List<Packet_EntityPosition> positionUpdate = new List<Packet_EntityPosition>();
		List<AnimationPacket> animationUpdate = new List<AnimationPacket>();
		List<EntityTagPacket> tagUpdate = new List<EntityTagPacket>();
		ConcurrentDictionary<long, EntityTagPacket> entityTagPackets = entitiesTagPackets;
		foreach (ConnectedClient client in ClientList)
		{
			positionUpdate.Clear();
			animationUpdate.Clear();
			tagUpdate.Clear();
			if (stateUpdateTick)
			{
				List<Entity> TrackedEntities = client.threadedTrackedEntities[zeroBasedThreadNum];
				foreach (Entity item in TrackedEntities)
				{
					long id = item.EntityId;
					if (entityPositionPackets.TryGetValue(id, out var pu))
					{
						positionUpdate.Add(pu);
					}
					if (entityAnimPackets.TryGetValue(id, out var au))
					{
						animationUpdate.Add(au);
					}
					if (entityTagPackets.TryGetValue(id, out var tu))
					{
						tagUpdate.Add(tu);
					}
				}
				if (entityAnimPackets.TryGetValue(client.Entityplayer.EntityId, out var auOwn) && !TrackedEntities.Contains(client.Entityplayer))
				{
					animationUpdate.Add(auOwn);
				}
			}
			else
			{
				foreach (long id2 in client.TrackedEntities)
				{
					if (entityPositionPackets.TryGetValue(id2, out var pu2))
					{
						positionUpdate.Add(pu2);
					}
					if (entityAnimPackets.TryGetValue(id2, out var au2))
					{
						animationUpdate.Add(au2);
					}
					if (entityTagPackets.TryGetValue(id2, out var tu2))
					{
						tagUpdate.Add(tu2);
					}
				}
			}
			int positionsCount = positionUpdate.Count;
			if (positionsCount > 8 && !client.IsSinglePlayerClient && !client.FallBackToTcp)
			{
				for (int i = 0; i < positionsCount; i += 8)
				{
					Packet_EntityPosition[] chunk = new Packet_EntityPosition[Math.Min(8, positionsCount - i)];
					for (int j = 0; j < chunk.Length; j++)
					{
						chunk[j] = positionUpdate[i + j];
					}
					Packet_BulkEntityPosition bulkPositionPacket = new Packet_BulkEntityPosition();
					bulkPositionPacket.SetEntityPositions(chunk);
					udpNetwork.SendPacket_Threadsafe(client, bulkPositionPacket);
				}
			}
			else if (positionsCount > 0)
			{
				Packet_BulkEntityPosition bulkPositionPacket2 = new Packet_BulkEntityPosition();
				bulkPositionPacket2.SetEntityPositions(positionUpdate.ToArray());
				udpNetwork.SendPacket_Threadsafe(client, bulkPositionPacket2);
			}
			if (animationUpdate.Count > 0)
			{
				BulkAnimationPacket bulkAnimationPacket = new BulkAnimationPacket
				{
					Packets = animationUpdate.ToArray()
				};
				AnimationsAndTagsChannel.SendPacket(bulkAnimationPacket, client.Player);
			}
			if (tagUpdate.Count <= 0)
			{
				continue;
			}
			foreach (EntityTagPacket tagPacket in tagUpdate)
			{
				AnimationsAndTagsChannel.SendPacket(tagPacket, client.Player);
			}
		}
	}

	private int PrepareEntitySpawns(Entity[] spawnsToSend, List<ConnectedClient> clientList)
	{
		int squareDistance = MagicNum.DefaultEntityTrackingRange * MagicNum.ServerChunkSize * MagicNum.DefaultEntityTrackingRange * MagicNum.ServerChunkSize;
		double[] positions = this.positions;
		int clientCount = clientList.Count;
		foreach (ConnectedClient client in clientList)
		{
			client.EntitySpawnsToSend.Clear();
		}
		int numberInRange = 0;
		for (int j = 0; j < spawnsToSend.Length; j++)
		{
			Entity entity = spawnsToSend[j];
			if (entity == null)
			{
				continue;
			}
			EntityPos spawnedEntityPos = entity.Pos;
			long spawnedEntityId = entity.EntityId;
			bool inRange = false;
			for (int i = 0; i < positions.Length && i < clientCount; i += 3)
			{
				if (spawnedEntityPos.InRangeOf(positions[i], positions[i + 1], positions[i + 2], squareDistance))
				{
					ConnectedClient connectedClient = clientList[i / 3];
					connectedClient.TrackedEntities.Add(spawnedEntityId);
					connectedClient.EntitySpawnsToSend.Add(entity);
					inRange = true;
				}
			}
			if (inRange)
			{
				numberInRange++;
			}
			else
			{
				spawnsToSend[j] = null;
			}
		}
		return numberInRange;
	}

	public void SendEntitySpawns(Entity[] spawnsToSend, List<ConnectedClient> clientList, Dictionary<long, Packet_EntityPosition> entityPositionPackets)
	{
		FastMemoryStream ms = offthreadProcess.buffer;
		ms.Reset();
		try
		{
			foreach (ConnectedClient client in clientList)
			{
				if (client.EntitySpawnsToSend.Count <= 0)
				{
					continue;
				}
				server.SendPacket(client.Id, ServerPackets.GetEntitySpawnPacket(client.EntitySpawnsToSend, ms));
				foreach (Entity entity in client.EntitySpawnsToSend)
				{
					if (entityPositionPackets.TryGetValue(entity.EntityId, out var posPacket))
					{
						Packet_Server packet = new Packet_Server
						{
							Id = 80,
							EntityPosition = posPacket
						};
						server.SendPacket(client.Id, packet);
						entity.Pos.SetFromPacket(posPacket, entity, sanityCheck: false);
						entity.Attributes.SetInt("tick", 2);
					}
				}
			}
			foreach (Entity entity2 in spawnsToSend)
			{
				if (entity2 != null)
				{
					entity2.packet = null;
				}
			}
		}
		catch (Exception e)
		{
			ServerMain.Logger.Error(e);
		}
		entityPositionPackets.Clear();
	}

	private void DoFirstPhysicsTicks(Entity[] spawnsToSend, Dictionary<long, Packet_EntityPosition> entityPositionPackets)
	{
		float adjustedRate = 1f / 30f * rateModifier;
		EntityPos tmpPos = new EntityPos();
		foreach (Entity entity in spawnsToSend)
		{
			if (entity != null && !(entity is EntityPlayer))
			{
				tmpPos.SetFrom(entity.Pos);
				entityPositionPackets[entity.EntityId] = DoFirstPhysicsTick(entity, adjustedRate);
				entity.Pos.SetFrom(tmpPos);
			}
		}
	}

	private static Packet_EntityPosition DoFirstPhysicsTick(Entity entity, float adjustedRate)
	{
		foreach (EntityBehavior behavior in entity.SidedProperties.Behaviors)
		{
			if (behavior is IPhysicsTickable tickable)
			{
				tickable.Ticking = true;
				tickable.OnPhysicsTick(adjustedRate);
				tickable.OnPhysicsTick(adjustedRate);
				tickable.AfterPhysicsTick(adjustedRate);
				break;
			}
		}
		return ServerPackets.getEntityPositionPacket(entity.Pos, entity, 1);
	}

	internal void SendPrioritySpawn(Entity entity, ICollection<ConnectedClient> clientList)
	{
		Packet_Server spawnPacket = ServerPackets.GetEntitySpawnPacket(new List<Entity>(1) { entity });
		Packet_EntityPosition posPacket = DoFirstPhysicsTick(entity, 1f / 15f);
		Packet_Server packet2 = new Packet_Server
		{
			Id = 80,
			EntityPosition = posPacket
		};
		entity.Attributes.SetInt("tick", 2);
		int squareDistance = MagicNum.DefaultEntityTrackingRange * MagicNum.ServerChunkSize * MagicNum.DefaultEntityTrackingRange * MagicNum.ServerChunkSize;
		EntityPos entityPos = entity.Pos;
		foreach (ConnectedClient client in clientList)
		{
			if (client.State.ConnectedOrPlaying() && client.Entityplayer != null)
			{
				EntityPos clientPos = client.Entityplayer.Pos;
				if (entityPos.InRangeOf(clientPos, squareDistance))
				{
					client.TrackedEntities.Add(entity.EntityId);
					server.SendPacket(client.Id, spawnPacket);
					server.SendPacket(client.Id, packet2);
				}
			}
		}
	}

	public void DoWork(int threadNumber)
	{
		float adjustedRate = 1f / 30f * rateModifier;
		List<IPhysicsTickable> tickables = this.tickables;
		int count = tickables.Count;
		int activePhysicsThreadsCount = maxPhysicsThreads;
		if (threadNumber == 0)
		{
			threadNumber = 1;
			activePhysicsThreadsCount = 1;
		}
		int countoffset = 480;
		int startpos = countoffset + (count - countoffset) * (threadNumber - 1) / activePhysicsThreadsCount;
		int endpos = countoffset + (count - countoffset) * threadNumber / activePhysicsThreadsCount;
		if (threadNumber == 1)
		{
			startpos = 0;
		}
		Dictionary<long, Packet_EntityPosition> entityPositionPackets = entitiesPositionPackets[threadNumber - 1];
		Dictionary<long, AnimationPacket> entityAnimPackets = entitiesAnimPackets[threadNumber - 1];
		entityPositionPackets.Clear();
		entityAnimPackets.Clear();
		if (attrUpdateAccum == 0f)
		{
			List<ConnectedClient> ClientList = this.ClientList;
			for (int i = startpos; i < endpos; i++)
			{
				IPhysicsTickable tickable = tickables[i];
				if (UpdateTrackedEntityState(tickable.Entity, ClientList, threadNumber - 1))
				{
					if (threadNumber == 1)
					{
						ActiveStateChanged(tickable.Entity);
					}
					else
					{
						stateChanges.Add(tickable.Entity);
					}
				}
			}
		}
		FrameProfilerUtil frameProfiler = null;
		if (threadNumber == 1)
		{
			frameProfiler = ServerMain.FrameProfiler;
			if (frameProfiler == null)
			{
				throw new Exception("FrameProfiler on main thread was null - this should be impossible!");
			}
			if (!frameProfiler.Enabled)
			{
				frameProfiler = null;
			}
		}
		if (activePhysicsThreadsCount == 1)
		{
			frameProfiler?.Enter("entityphysics-mainthread (" + count + " entities, single-threaded) (" + ticksToDo + " physics ticks to do)");
		}
		else
		{
			frameProfiler?.Enter("entityphysics-mainthread (" + count + " entities across " + activePhysicsThreadsCount + " threads) (" + ticksToDo + " physics ticks to do)");
		}
		FastMemoryStream ms = entitiesUpdateReusableBuffers[threadNumber - 1];
		ms.Reset();
		bool debugMode = server.Config.EntityDebugMode;
		Dictionary<long, Packet_EntityAttributes> efu = null;
		Dictionary<long, Packet_EntityAttributeUpdate> epu = null;
		Dictionary<long, Packet_EntityAttributes> edu = null;
		if (attrUpdateAccum == 0f)
		{
			efu = entitiesFullUpdate[threadNumber - 1];
			epu = entitiesPartialUpdate[threadNumber - 1];
			edu = entitiesDebugUpdate[threadNumber - 1];
			efu.Clear();
			epu.Clear();
			edu.Clear();
		}
		try
		{
			int physicsTicknum = -1;
			float dt = deltaT;
			bool attrUpdateTick = attrUpdateAccum == 0f;
			while (++physicsTicknum < ticksToDo)
			{
				int doubleTick = (ticksToDo - physicsTicknum + 1) % 2;
				physicsTicknum += doubleTick;
				bool finalTick = physicsTicknum == ticksToDo - 1;
				bool firstTick = physicsTicknum == doubleTick;
				bool forceUpdate = (physicsTicknum + currentTick) % 30 <= doubleTick;
				for (int j = startpos; j < endpos; j++)
				{
					IPhysicsTickable tickable2 = tickables[j];
					Entity entity = tickable2.Entity;
					if (firstTick)
					{
						EntityBehavior[] serverBehaviors = entity.ServerBehaviorsThreadsafe;
						for (int k = 0; k < serverBehaviors.Length; k++)
						{
							serverBehaviors[k].OnGameTick(dt);
							frameProfiler?.Mark(serverBehaviors[k].ProfilerName);
						}
					}
					if (entity.IsTracked == 0)
					{
						if (finalTick)
						{
							entity.PositionTicked = true;
						}
						continue;
					}
					if (!((tickable2 as PhysicsBehaviorBase)?.mountableSupplier?.Controller is EntityPlayer { Alive: not false }))
					{
						tickable2.OnPhysicsTick(adjustedRate);
						if (frameProfiler == null)
						{
							if (doubleTick != 0)
							{
								tickable2.OnPhysicsTick(adjustedRate);
							}
							BuildPositionPacket(entity, forceUpdate, entityPositionPackets, entityAnimPackets);
						}
						else
						{
							frameProfiler.Mark("physicstick-oneentity");
							if (doubleTick != 0)
							{
								tickable2.OnPhysicsTick(adjustedRate);
								frameProfiler.Mark("physicstick-oneentity");
							}
							BuildPositionPacket(entity, forceUpdate, entityPositionPackets, entityAnimPackets);
							frameProfiler.Mark("physicstick-buildpospacket");
						}
					}
					if (!finalTick)
					{
						continue;
					}
					if (attrUpdateTick)
					{
						EntityTagPacket unsentTagPacket = BuildAttributesPackets(entity, ms, debugMode, efu, epu, edu);
						if (unsentTagPacket != null)
						{
							entitiesTagPackets[entity.EntityId] = unsentTagPacket;
						}
						frameProfiler?.Mark("physicstick-buildattrpacket");
					}
					entity.PositionTicked = true;
				}
				SendPositionsAndAnimations(entityPositionPackets, entityAnimPackets, threadNumber - 1, attrUpdateAccum == 0f);
				frameProfiler?.Mark("physicsmanager-udp");
			}
		}
		catch (Exception e)
		{
			ServerMain.Logger.Error("Error while enumerating tickables. Tickables total count is " + tickables.Count);
			ServerMain.Logger.Error(e);
		}
		entityPositionPackets.Clear();
		entityAnimPackets.Clear();
		frameProfiler?.Leave();
	}

	private void SendPositionsForNonTickableEntities(bool doBuildAttributes)
	{
		Dictionary<long, Packet_EntityPosition> entityPositionPackets = entitiesPositionPackets[0];
		Dictionary<long, AnimationPacket> entityAnimPackets = entitiesAnimPackets[0];
		IDictionary<long, EntityTagPacket> entityTagPackets = entitiesTagPackets;
		entityPositionPackets.Clear();
		entityAnimPackets.Clear();
		entityTagPackets.Clear();
		bool forceUpdate = currentTick % 30 < ticksToDo;
		Dictionary<long, Packet_EntityAttributes> eFullUpdate = null;
		Dictionary<long, Packet_EntityAttributeUpdate> ePartialUpdate = null;
		Dictionary<long, Packet_EntityAttributes> eDebugUpdate = null;
		bool debugMode = server.Config.EntityDebugMode;
		FastMemoryStream ms = null;
		if (doBuildAttributes)
		{
			eFullUpdate = entitiesFullUpdate[0];
			ePartialUpdate = entitiesPartialUpdate[0];
			eDebugUpdate = entitiesDebugUpdate[0];
			ms = new FastMemoryStream(64);
		}
		float dt = deltaT;
		FrameProfilerUtil frameProfiler = ServerMain.FrameProfiler;
		foreach (Entity entity in server.LoadedEntities.Values)
		{
			if (entity.PositionTicked)
			{
				entity.PositionTicked = false;
				continue;
			}
			EntityBehavior[] serverBehaviorsThreadsafe = entity.ServerBehaviorsThreadsafe;
			foreach (EntityBehavior behavior in serverBehaviorsThreadsafe)
			{
				behavior.OnGameTick(dt);
				frameProfiler.Mark(behavior.ProfilerName);
			}
			if (doBuildAttributes && UpdateTrackedEntityState(entity, ClientList, 0))
			{
				ActiveStateChanged(entity);
			}
			if (entity.IsTracked == 0)
			{
				continue;
			}
			if (ticksToDo > 0)
			{
				BuildPositionPacket(entity, forceUpdate, entityPositionPackets, entityAnimPackets);
			}
			if (doBuildAttributes)
			{
				EntityTagPacket tagPacket = BuildAttributesPackets(entity, ms, debugMode, eFullUpdate, ePartialUpdate, eDebugUpdate);
				if (tagPacket != null)
				{
					entitiesTagPackets[entity.EntityId] = tagPacket;
				}
				frameProfiler.Mark("physicstick-buildattrpacket");
			}
		}
		if (entityPositionPackets.Count + entityAnimPackets.Count + entityTagPackets.Count > 0)
		{
			SendPositionsAndAnimations(entityPositionPackets, entityAnimPackets, 0, doBuildAttributes);
		}
		entityPositionPackets.Clear();
		entityAnimPackets.Clear();
		entityTagPackets.Clear();
	}

	public bool ShouldExit()
	{
		if (!server.stopped)
		{
			return server.exitState.Exit;
		}
		return true;
	}

	public void HandleException(Exception e)
	{
		ServerMain.Logger.Error("Error thrown while ticking physics:\n{0}\n{1}", e.Message, e.StackTrace);
	}

	public void StartWorkerThread(int threadNum)
	{
		try
		{
			while (tickables.Count < 120)
			{
				if (ShouldExit())
				{
					return;
				}
				Thread.Sleep(15);
			}
		}
		catch (Exception)
		{
		}
		server.EventManager.TriggerPhysicsThreadStart();
		loadBalancer.WorkerThreadLoop(this, threadNum);
	}

	public void Dispose()
	{
		server?.UnregisterGameTickListener(listener);
		tickables.Clear();
	}
}
