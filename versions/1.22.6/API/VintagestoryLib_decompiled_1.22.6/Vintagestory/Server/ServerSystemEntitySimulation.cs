using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.Client.NoObf;
using Vintagestory.Common;

namespace Vintagestory.Server;

public class ServerSystemEntitySimulation : ServerSystem
{
	internal int trackingRangeSq = MagicNum.DefaultEntityTrackingRange * MagicNum.ServerChunkSize * MagicNum.DefaultEntityTrackingRange * MagicNum.ServerChunkSize;

	internal PhysicsManager physicsManager;

	private Dictionary<string, List<string>> deathMessagesCache;

	private int skippedDespawnTicks;

	public ServerSystemEntitySimulation(ServerMain server)
		: base(server)
	{
		server.RegisterGameTickListener(UpdateEvery1000ms, 1000);
		server.EventManager.OnGameWorldBeingSaved += EventManager_OnGameWorldBeingSaved;
		server.RegisterGameTickListener(UpdateEvery100ms, 100);
		server.clientAwarenessEvents[EnumClientAwarenessEvent.ChunkTransition].Add(OnPlayerLeaveChunk);
		server.PacketHandlers[17] = HandleEntityInteraction;
		server.PacketHandlers[12] = HandleSpecialKey;
		server.PacketHandlers[32] = HandleRuntimeSetting;
		server.EventManager.OnPlayerChat += EventManager_OnPlayerChat;
	}

	private void HandleRuntimeSetting(Packet_Client packet, ConnectedClient player)
	{
		player.Player.ImmersiveFpMode = packet.RuntimeSetting.ImmersiveFpMode > 0;
		player.Player.ItemCollectMode = packet.RuntimeSetting.ItemCollectMode;
	}

	private void EventManager_OnPlayerChat(IServerPlayer byPlayer, int channelId, ref string message, ref string data, BoolRef consumed)
	{
		if (byPlayer.HasPrivilege("staffentitlement"))
		{
			double[] color = GlobalConstants.playerColorByEntitlement["staff"];
			message = string.Format("<font color=\"" + VtmlUtil.toHexColor(color) + "\"><strong>" + Lang.Get("serverstaffchatprefix") + " {0}:</strong></font> {1}", byPlayer.PlayerName, message);
			return;
		}
		if (byPlayer.Entitlements.Count > 0)
		{
			Entitlement ent = byPlayer.Entitlements[0];
			if (GlobalConstants.playerColorByEntitlement.TryGetValue(ent.Code, out var color2))
			{
				message = string.Format("<font color=\"" + VtmlUtil.toHexColor(color2) + "\"><strong>{0}:</strong></font> {1}", byPlayer.PlayerName, message);
				return;
			}
		}
		message = $"<strong>{byPlayer.PlayerName}:</strong> {message}";
	}

	private void EventManager_OnGameWorldBeingSaved()
	{
		if (server.RunPhase != EnumServerRunPhase.Shutdown)
		{
			server.EventManager.defragLists();
		}
	}

	public override int GetUpdateInterval()
	{
		return 20;
	}

	public override void OnBeginModsAndConfigReady()
	{
		server.EventManager.OnPlayerRespawn += OnPlayerRespawn;
		new ShapeTesselatorManager(server).LoadEntityShapes(server.EntityTypes, server.api);
	}

	public override void OnPlayerJoin(ServerPlayer player)
	{
		physicsManager.ForceClientUpdateTick(player.client);
	}

	public override void OnPlayerDisconnect(ServerPlayer player)
	{
		physicsManager.ForceClientUpdateTick(player.client);
	}

	private void OnPlayerLeaveChunk(ClientStatistics clientStats)
	{
		physicsManager.ForceClientUpdateTick(clientStats.client);
	}

	public override void OnServerTick(float dt)
	{
		foreach (ConnectedClient value in server.Clients.Values)
		{
			ConnectedClient client = value;
			ServerPlayer player;
			if (client.IsPlayingClient)
			{
				player = client.Player;
				player.Entity.PreviousBlockSelection = player.Entity.BlockSelection?.Position.Copy();
				server.RayTraceForSelection(player, ref player.Entity.BlockSelection, ref player.Entity.EntitySelection, bFilter, eFilter);
				if (player.Entity.BlockSelection != null)
				{
					bool firstTick = player.Entity.PreviousBlockSelection == null || player.Entity.BlockSelection.Position != player.Entity.PreviousBlockSelection;
					server.BlockAccessor.GetBlock(player.Entity.BlockSelection.Position).OnBeingLookedAt(player, player.Entity.BlockSelection, firstTick);
				}
			}
			bool bFilter(BlockPos pos, Block block)
			{
				if (block != null && block.RenderPass == EnumChunkRenderPass.Meta)
				{
					return client.WorldData.RenderMetaBlocks;
				}
				return true;
			}
			bool eFilter(Entity e)
			{
				if (e.IsInteractable)
				{
					return e.EntityId != player.Entity.EntityId;
				}
				return false;
			}
		}
		TickEntities(dt);
		SendPlayerEntityDeaths();
	}

	private void UpdateEvery100ms(float t1)
	{
		SendEntityDespawns();
		int count = server.DelayedSpawnQueue.Count;
		if (count <= 0)
		{
			return;
		}
		ServerMain.FrameProfiler.Enter("spawningentities");
		int maxCount = MagicNum.MaxEntitySpawnsPerTick;
		if (count > maxCount * 3)
		{
			maxCount = count / 2;
		}
		count = Math.Min(count, maxCount);
		Entity entity;
		while (count-- > 0 && server.DelayedSpawnQueue.TryDequeue(out entity))
		{
			try
			{
				server.SpawnEntity(entity);
				ServerMain.FrameProfiler.Mark("spawning:", entity.Code);
			}
			catch (Exception e)
			{
				ServerMain.Logger.Error(e);
			}
		}
		ServerMain.FrameProfiler.Leave();
	}

	private void UpdateEvery1000ms(float dt)
	{
		foreach (Entity entity in server.LoadedEntities.Values)
		{
			long chunkindex3d = server.WorldMap.ChunkIndex3D(entity.Pos);
			if (entity.InChunkIndex3d != chunkindex3d)
			{
				ServerChunk oldChunk = server.WorldMap.GetServerChunk(entity.InChunkIndex3d);
				ServerChunk newChunk = server.WorldMap.GetServerChunk(chunkindex3d);
				if (newChunk != null)
				{
					oldChunk?.RemoveEntity(entity.EntityId);
					newChunk.AddEntity(entity);
					entity.InChunkIndex3d = chunkindex3d;
				}
			}
		}
	}

	private void TickEntities(float dt)
	{
		List<KeyValuePair<Entity, EntityDespawnData>> despawned = new List<KeyValuePair<Entity, EntityDespawnData>>();
		ServerMain.FrameProfiler.Enter("tickentities");
		foreach (Entity entity in server.LoadedEntities.Values)
		{
			if (!Dimensions.ShouldNotTick(entity.Pos, entity.Api))
			{
				try
				{
					entity.OnGameTick(dt);
				}
				catch (Exception e)
				{
					if (entity is EntityPlayer epl)
					{
						ServerMain.Logger.Warning("Exception while ticking player {0} ({1}) at {2}: ", epl.GetName(), epl.PlayerUID, epl.Pos);
						ServerMain.Logger.Error(e);
					}
					else
					{
						ServerMain.Logger.Warning("Exception while ticking entity {0} ({1}) at {2}: ", entity.GetName(), entity.Properties?.Class, entity.Pos);
						ServerMain.Logger.Error(e);
					}
				}
			}
			if (entity.ShouldDespawn)
			{
				despawned.Add(new KeyValuePair<Entity, EntityDespawnData>(entity, entity.DespawnReason));
			}
		}
		ServerMain.FrameProfiler.Enter("despawning");
		foreach (KeyValuePair<Entity, EntityDespawnData> val in despawned)
		{
			server.DespawnEntity(val.Key, val.Value);
			ServerMain.FrameProfiler.Mark("despawned-", val.Key.Code.Path);
		}
		ServerMain.FrameProfiler.Leave();
		ServerMain.FrameProfiler.Leave();
	}

	private void HandleEntityInteraction(Packet_Client packet, ConnectedClient client)
	{
		ServerPlayer player = client.Player;
		if (player.WorldData.CurrentGameMode == EnumGameMode.Spectator)
		{
			return;
		}
		Packet_EntityInteraction p = packet.EntityInteraction;
		Entity[] entitiesAround = server.GetEntitiesAround(player.Entity.Pos.XYZ, player.WorldData.PickingRange + 10f, player.WorldData.PickingRange + 10f, (Entity e) => e.EntityId == p.EntityId);
		if (entitiesAround == null || entitiesAround.Length == 0)
		{
			ServerMain.Logger.Debug("HandleEntityInteraction received from client " + client.PlayerName + " but no such entity found in his range!");
			return;
		}
		Entity entity = entitiesAround[0];
		Cuboidd cuboidd = entity.SelectionBox.ToDouble().Translate(entity.Pos.X, entity.Pos.Y, entity.Pos.Z);
		EntityPos pos = client.Entityplayer.Pos;
		ItemStack itemStack = client.Player.InventoryManager?.ActiveHotbarSlot?.Itemstack;
		float range = itemStack?.Collectible.GetAttackRange(itemStack) ?? GlobalConstants.DefaultAttackRange;
		double eyePosX = pos.X + client.Entityplayer.LocalEyePos.X;
		double eyePosY = pos.Y + client.Entityplayer.LocalEyePos.Y;
		double eyePosZ = pos.Z + client.Entityplayer.LocalEyePos.Z;
		bool isAttack = p.MouseButton == 0;
		if ((cuboidd.ShortestDistanceFrom(eyePosX, eyePosY, eyePosZ) > (double)(range * 2f) && isAttack) || (isAttack && (((!server.Config.AllowPvP || !player.HasPrivilege("attackplayers")) && entity is EntityPlayer) || (!player.HasPrivilege("attackcreatures") && entity is EntityAgent))) || (entity is EntityPlayer entityPlayer && !(entityPlayer.Player is IServerPlayer { ConnectionState: EnumClientState.Playing })))
		{
			return;
		}
		Vec3d hitPosition = new Vec3d(CollectibleNet.DeserializeDouble(p.HitX), CollectibleNet.DeserializeDouble(p.HitY), CollectibleNet.DeserializeDouble(p.HitZ));
		if (server.Config.AntiAbuse >= EnumProtectionLevel.Basic && !player.IsInInteractionRangeOf(entity))
		{
			ServerMain.Logger.Audit("{0} tried to interact with an entity out of range.", player.PlayerName);
			return;
		}
		if (p.EntityId != player.CurrentEntitySelection?.Entity?.EntityId)
		{
			player.Entity.EntitySelection = new EntitySelection
			{
				Entity = entity,
				SelectionBoxIndex = p.SelectionBoxIndex,
				Position = hitPosition
			};
		}
		else
		{
			player.CurrentEntitySelection.Position = hitPosition;
			player.CurrentEntitySelection.SelectionBoxIndex = p.SelectionBoxIndex;
		}
		EnumHandling handling = EnumHandling.PassThrough;
		server.EventManager.TriggerPlayerInteractEntity(entity, player, player.inventoryMgr.ActiveHotbarSlot, hitPosition, p.MouseButton, ref handling);
		if (handling == EnumHandling.PassThrough)
		{
			entity.OnInteract(player.Entity, player.InventoryManager.ActiveHotbarSlot, hitPosition, (p.MouseButton != 0) ? EnumInteractMode.Interact : EnumInteractMode.Attack);
		}
	}

	private void HandleSpecialKey(Packet_Client packet, ConnectedClient client)
	{
		int lives = server.SaveGameData?.WorldConfiguration.GetString("playerlives", "-1").ToInt(-1) ?? (-1);
		if (lives < 0 || lives > client.WorldData.Deaths)
		{
			ServerMain.Logger.VerboseDebug("Received respawn request from {0}", client.PlayerName);
			server.EventManager.TriggerPlayerRespawn(client.Player);
		}
		else
		{
			client.Player.SendMessage(GlobalConstants.CurrentChatGroup, "Cannot revive! All lives used up.", EnumChatType.CommandError);
		}
	}

	private void OnPlayerRespawn(IServerPlayer player)
	{
		if (player.Entity == null || player.Entity.Alive)
		{
			ServerMain.Logger.VerboseDebug("Respawn key received but ignored. Cause: {0} || {1}", player.Entity == null, player.Entity.Alive);
			return;
		}
		FuzzyEntityPos pos = player.GetSpawnPosition(consumeSpawnUse: true);
		ConnectedClient client = server.Clients[player.ClientId];
		if (pos.UsesLeft >= 0)
		{
			if (pos.UsesLeft == 99)
			{
				player.SendLocalisedMessage(GlobalConstants.GeneralChatGroup, "playerrespawn-nocustomspawnset");
			}
			else if (pos.UsesLeft > 0)
			{
				player.SendLocalisedMessage(GlobalConstants.GeneralChatGroup, "You have re-emerged at your returning point. It will vanish after {0} more uses", pos.UsesLeft);
			}
			else if (pos.UsesLeft == 0)
			{
				player.SendLocalisedMessage(GlobalConstants.GeneralChatGroup, "You have re-emerged at your returning point, which has now vanished.");
			}
		}
		if (pos.Radius > 0f)
		{
			server.LocateRandomPosition(pos.XYZ, pos.Radius, 10, (BlockPos spawnpos) => ServerSystemSupplyChunks.AdjustForSaveSpawnSpot(server, spawnpos, player, server.rand.Value), delegate(BlockPos foundpos)
			{
				if (foundpos != null)
				{
					EntityPos entityPos = pos.Copy();
					entityPos.X = (double)foundpos.X + 0.5;
					entityPos.Y = foundpos.Y;
					entityPos.Z = (double)foundpos.Z + 0.5;
					teleport(client, entityPos);
				}
				else
				{
					teleport(client, pos);
				}
			});
		}
		else
		{
			teleport(client, pos);
		}
		ServerMain.Logger.VerboseDebug("Respawn key received. Teleporting player to spawn and reviving once chunks have loaded.");
	}

	private void teleport(ConnectedClient client, EntityPos targetPos)
	{
		EntityPlayer eplr = client.Player.Entity;
		eplr.TeleportTo(targetPos, delegate
		{
			eplr.Revive();
			server.ServerUdpNetwork.physicsManager.UpdateTrackedEntitiesStates(client);
		});
	}

	private void GenDeathMessagesCache()
	{
		deathMessagesCache = new Dictionary<string, List<string>>();
		foreach (KeyValuePair<string, string> allEntry in Lang.AvailableLanguages["en"].GetAllEntries())
		{
			AssetLocation loc = new AssetLocation(allEntry.Key);
			if (loc.PathStartsWith("deathmsg"))
			{
				List<string> parts = new List<string>(loc.Path.Split('-'));
				parts.RemoveAt(parts.Count - 1);
				string key = string.Join("-", parts);
				List<string> elems;
				if (deathMessagesCache.ContainsKey(key))
				{
					elems = deathMessagesCache[key];
				}
				else
				{
					elems = new List<string>();
					deathMessagesCache[key] = elems;
				}
				elems.Add(loc.Path);
			}
		}
	}

	private void SendPlayerEntityDeaths()
	{
		if (deathMessagesCache == null)
		{
			GenDeathMessagesCache();
		}
		List<ConnectedClient> deadClients = server.Clients.Values.Where((ConnectedClient connectedClient) => connectedClient.State.ConnectedOrPlaying() && !connectedClient.Entityplayer.Alive && connectedClient.Entityplayer.DeadNotify).ToList();
		if (deadClients.Count == 0)
		{
			return;
		}
		int lives = server.SaveGameData?.WorldConfiguration.GetString("playerlives", "-1").ToInt(-1) ?? (-1);
		foreach (ConnectedClient client in deadClients)
		{
			client.Entityplayer.DeadNotify = false;
			client.WorldData.Deaths++;
			server.EventManager.TriggerPlayerDeath(client.Player, client.Entityplayer.DeathReason);
			server.BroadcastPacket(new Packet_Server
			{
				Id = 45,
				PlayerDeath = new Packet_PlayerDeath
				{
					ClientId = client.Id,
					LivesLeft = ((lives < 0) ? (-1) : Math.Max(0, lives - client.WorldData.Deaths))
				}
			});
			DamageSource src = client.Entityplayer.DeathReason;
			bool num = !server.api.World.Config.GetBool("disableDeathMessages");
			string deathMessage = "";
			if (num)
			{
				deathMessage = GetDeathMessage(client, src);
				server.joinLeaveDeathMessage(deathMessage, EnumChatType.Notification, client.Player);
			}
			if (src?.GetCauseEntity() is EntityPlayer otherPlayer)
			{
				string creatureName = server.PlayerByUid(otherPlayer.PlayerUID).PlayerName;
				ServerMain.Logger.Audit(Lang.Get("{0} killed {1}, with item (if any): {2}", creatureName, client.PlayerName, otherPlayer.RightHandItemSlot?.Itemstack?.Collectible.Code));
			}
			else
			{
				ServerMain.Logger.Audit(Lang.Get("{0} died. Death message: {1}", client.PlayerName, deathMessage));
			}
		}
	}

	private string GetDeathMessage(ConnectedClient client, DamageSource src)
	{
		if (src == null)
		{
			return Lang.Get("Player {0} died.", client.PlayerName);
		}
		Entity causeEntity = src.GetCauseEntity();
		if (causeEntity != null)
		{
			string ecode = "deathmsg-" + causeEntity.Code.Path.Replace("-", "");
			deathMessagesCache.TryGetValue(ecode, out var messages);
			if (messages != null && messages.Count > 0)
			{
				return Lang.Get(messages[server.rand.Value.Next(messages.Count)], client.PlayerName);
			}
			string creatureName = causeEntity.GetPrefixAndCreatureName();
			return Lang.Get("Player {0} got killed by {1}", client.PlayerName, creatureName);
		}
		string code = null;
		if (src.Source == EnumDamageSource.Explosion)
		{
			code = "deathmsg-explosion";
		}
		else if (src.Type == EnumDamageType.Hunger)
		{
			code = "deathmsg-hunger";
		}
		else if (src.Type == EnumDamageType.Fire)
		{
			code = "deathmsg-fire-block";
		}
		else if (src.Type == EnumDamageType.Electricity)
		{
			code = "deathmsg-electricity-block";
		}
		else if (src.Source == EnumDamageSource.Fall)
		{
			code = "deathmsg-fall";
		}
		if (code != null)
		{
			deathMessagesCache.TryGetValue(code, out var messages2);
			if (messages2 != null && messages2.Count > 0)
			{
				int variant = server.rand.Value.Next(messages2.Count);
				return Lang.Get(messages2[variant], client.PlayerName);
			}
		}
		return Lang.Get("Player {0} died.", client.PlayerName);
	}

	private void SendEntityDespawns()
	{
		if (server.EntityDespawnSendQueue.Count == 0)
		{
			return;
		}
		if (ServerMain.spawnSendQueue > 0 && skippedDespawnTicks < 15)
		{
			skippedDespawnTicks++;
			return;
		}
		skippedDespawnTicks = 0;
		Packet_EntityDespawn p = new Packet_EntityDespawn();
		Packet_Server packet = new Packet_Server
		{
			Id = 36,
			EntityDespawn = p
		};
		List<long> entityIds = new List<long>();
		List<int> despawnReasons = new List<int>();
		List<int> damageSource = new List<int>();
		foreach (ConnectedClient client in server.Clients.Values)
		{
			entityIds.Clear();
			despawnReasons.Clear();
			damageSource.Clear();
			foreach (KeyValuePair<Entity, EntityDespawnData> val in server.EntityDespawnSendQueue)
			{
				if (client.TrackedEntities.Contains(val.Key.EntityId))
				{
					entityIds.Add(val.Key.EntityId);
					despawnReasons.Add((int)(val.Value?.Reason ?? EnumDespawnReason.Death));
					damageSource.Add((int)((val.Value?.DamageSourceForDeath == null) ? EnumDamageSource.Unknown : val.Value.DamageSourceForDeath.Source));
				}
			}
			p.SetEntityId(entityIds.ToArray());
			p.SetDeathDamageSource(damageSource.ToArray());
			p.SetDespawnReason(despawnReasons.ToArray());
			server.SendPacket(client.Id, packet);
		}
		server.EntityDespawnSendQueue.Clear();
	}
}
