using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class AiTaskBellAlarm : AiTaskBase
{
	private string[] seekEntityCodesExact = new string[1] { "player" };

	private string[] seekEntityCodesBeginsWith = Array.Empty<string>();

	private int spawnRange;

	private float seekingRange = 12f;

	private EntityProperties[] spawnMobs;

	private Entity targetEntity;

	private AssetLocation repeatSoundLoc;

	private ICoreServerAPI sapi;

	private int spawnIntervalMsMin = 2000;

	private int spawnIntervalMsMax = 12000;

	private int spawnMaxQuantity = 5;

	private int nextSpawnIntervalMs;

	private List<Entity> spawnedEntities = new List<Entity>();

	private float spawnAccum;

	private CollisionTester collisionTester = new CollisionTester();

	public AiTaskBellAlarm(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig)
		: base(entity, taskConfig, aiConfig)
	{
		sapi = entity.World.Api as ICoreServerAPI;
		spawnRange = taskConfig["spawnRange"].AsInt(12);
		spawnIntervalMsMin = taskConfig["spawnIntervalMsMin"].AsInt(2500);
		spawnIntervalMsMax = taskConfig["spawnIntervalMsMax"].AsInt(12000);
		spawnMaxQuantity = taskConfig["spawnMaxQuantity"].AsInt(5);
		seekingRange = taskConfig["seekingRange"].AsFloat(12f);
		AssetLocation[]? array = taskConfig["spawnMobs"].AsObject(Array.Empty<AssetLocation>());
		List<EntityProperties> props = new List<EntityProperties>();
		AssetLocation[] array2 = array;
		foreach (AssetLocation val in array2)
		{
			EntityProperties etype = sapi.World.GetEntityType(val);
			if (etype == null)
			{
				sapi.World.Logger.Warning("AiTaskBellAlarm defined spawnmob {0}, but no such entity type found, will ignore.", val);
			}
			else
			{
				props.Add(etype);
			}
		}
		spawnMobs = props.ToArray();
		repeatSoundLoc = ((!taskConfig["repeatSound"].Exists) ? null : AssetLocation.Create(taskConfig["repeatSound"].AsString(), entity.Code.Domain).WithPathPrefixOnce("sounds/"));
		string[] codes = taskConfig["onNearbyEntityCodes"].AsArray(new string[1] { "player" });
		List<string> exact = new List<string>();
		List<string> beginswith = new List<string>();
		foreach (string code in codes)
		{
			if (code.EndsWith('*'))
			{
				beginswith.Add(code.Substring(0, code.Length - 1));
			}
			else
			{
				exact.Add(code);
			}
		}
		seekEntityCodesExact = exact.ToArray();
		seekEntityCodesBeginsWith = beginswith.ToArray();
		cooldownUntilTotalHours = entity.World.Calendar.TotalHours + MinCooldownHours + entity.World.Rand.NextDouble() * (MaxCooldownHours - MinCooldownHours);
	}

	protected override void SetDefaultValues()
	{
		base.SetDefaultValues();
		ExecutionChance = 0.05;
	}

	public override bool ShouldExecute()
	{
		if (entity.World.Rand.NextDouble() > ExecutionChance)
		{
			return false;
		}
		if (IsOnCooldown())
		{
			return false;
		}
		if (!PreconditionsSatisfied())
		{
			return false;
		}
		float range = seekingRange;
		bool listening = entity.GetBehavior<EntityBehaviorTaskAI>().TaskManager.IsTaskActive("listen");
		range = (listening ? (range * 1.25f) : (range / 3f));
		targetEntity = entity.World.GetNearestEntity(entity.Pos.XYZ, range, range, delegate(Entity e)
		{
			if (!e.Alive || e.EntityId == entity.EntityId)
			{
				return false;
			}
			if (e is EntityPlayer entityPlayer)
			{
				IPlayer player = entityPlayer.Player;
				if (player == null || player.WorldData.CurrentGameMode != EnumGameMode.Creative)
				{
					IPlayer player2 = (e as EntityPlayer).Player;
					if (player2 == null || player2.WorldData.CurrentGameMode != EnumGameMode.Spectator)
					{
						bool num = entityPlayer.ServerControls.TriesToMove || entityPlayer.ServerControls.LeftMouseDown || entityPlayer.ServerControls.RightMouseDown || entityPlayer.ServerControls.Jump || !entityPlayer.OnGround || entityPlayer.ServerControls.HandUse != EnumHandInteract.None;
						bool flag = entityPlayer.ServerControls.TriesToMove && !entityPlayer.ServerControls.LeftMouseDown && !entityPlayer.ServerControls.RightMouseDown && !entityPlayer.ServerControls.Jump && entityPlayer.OnGround && entityPlayer.ServerControls.HandUse == EnumHandInteract.None && entityPlayer.ServerControls.Sneak;
						if (!num)
						{
							if (entityPlayer.Pos.DistanceTo(entity.Pos.XYZ) >= (double)(3 - (listening ? 1 : 0)))
							{
								return false;
							}
						}
						else if (flag && entityPlayer.Pos.DistanceTo(entity.Pos.XYZ) >= (double)(6 - (listening ? 3 : 0)))
						{
							return false;
						}
						return true;
					}
				}
			}
			return false;
		});
		if (targetEntity != null)
		{
			return true;
		}
		return false;
	}

	public override void StartExecute()
	{
		sapi.Network.BroadcastEntityPacket(entity.EntityId, 1025, SerializerUtil.Serialize(repeatSoundLoc));
		nextSpawnIntervalMs = spawnIntervalMsMin + entity.World.Rand.Next(spawnIntervalMsMax - spawnIntervalMsMin);
		base.StartExecute();
	}

	public override bool ContinueExecute(float dt)
	{
		if (!IsInValidDayTimeHours(initialRandomness: false))
		{
			return false;
		}
		spawnAccum += dt;
		if (spawnAccum > (float)nextSpawnIntervalMs / 1000f)
		{
			float playerScaling = (float)sapi.World.GetPlayersAround(entity.Pos.XYZ, 15f, 10f, (IPlayer plr) => plr.Entity.Alive).Length * sapi.Server.Config.SpawnCapPlayerScaling;
			trySpawnCreatures(GameMath.RoundRandom(sapi.World.Rand, (float)spawnMaxQuantity * playerScaling), spawnRange);
			nextSpawnIntervalMs = spawnIntervalMsMin + entity.World.Rand.Next(spawnIntervalMsMax - spawnIntervalMsMin);
			spawnAccum = 0f;
		}
		if ((double)targetEntity.Pos.SquareDistanceTo(entity.Pos) > Math.Pow(seekingRange + 5f, 2.0))
		{
			return false;
		}
		return true;
	}

	public override void FinishExecute(bool cancelled)
	{
		sapi.Network.BroadcastEntityPacket(entity.EntityId, 1026);
		base.FinishExecute(cancelled);
	}

	public override void OnEntityDespawn(EntityDespawnData reason)
	{
		sapi.Network.BroadcastEntityPacket(entity.EntityId, 1026);
		base.OnEntityDespawn(reason);
	}

	private void trySpawnCreatures(int maxquantity, int range = 13)
	{
		Vec3d centerPos = entity.Pos.XYZ;
		Vec3d spawnPos = new Vec3d();
		BlockPos spawnPosi = new BlockPos(0);
		for (int i = 0; i < spawnedEntities.Count; i++)
		{
			if (spawnedEntities[i] == null || !spawnedEntities[i].Alive)
			{
				spawnedEntities.RemoveAt(i);
				i--;
			}
		}
		if (spawnedEntities.Count > maxquantity)
		{
			return;
		}
		int tries = 50;
		int spawned = 0;
		while (tries-- > 0 && spawned < 1)
		{
			int index = sapi.World.Rand.Next(spawnMobs.Length);
			EntityProperties type = spawnMobs[index];
			int rndx = sapi.World.Rand.Next(2 * range) - range;
			int rndy = sapi.World.Rand.Next(2 * range) - range;
			int rndz = sapi.World.Rand.Next(2 * range) - range;
			spawnPos.Set((double)((int)centerPos.X + rndx) + 0.5, (double)((int)centerPos.Y + rndy) + 0.001, (double)((int)centerPos.Z + rndz) + 0.5);
			spawnPosi.Set((int)spawnPos.X, (int)spawnPos.Y, (int)spawnPos.Z);
			while (sapi.World.BlockAccessor.GetBlockBelow(spawnPosi).Id == 0 && spawnPos.Y > 0.0)
			{
				spawnPosi.Y--;
				spawnPos.Y -= 1.0;
			}
			if (sapi.World.BlockAccessor.IsValidPos(spawnPosi))
			{
				Cuboidf collisionBox = type.SpawnCollisionBox.OmniNotDownGrowBy(0.1f);
				if (!collisionTester.IsColliding(sapi.World.BlockAccessor, collisionBox, spawnPos, alsoCheckTouch: false))
				{
					DoSpawn(type, spawnPos, 0L);
					spawned++;
				}
			}
		}
	}

	private void DoSpawn(EntityProperties entityType, Vec3d spawnPosition, long herdid)
	{
		Entity entity = sapi.ClassRegistry.CreateEntity(entityType);
		if (entity is EntityAgent agent)
		{
			agent.HerdId = herdid;
		}
		entity.Pos.SetPosWithDimension(spawnPosition);
		entity.Pos.SetYaw((float)sapi.World.Rand.NextDouble() * ((float)Math.PI * 2f));
		entity.PositionBeforeFalling.Set(entity.Pos.X, entity.Pos.Y, entity.Pos.Z);
		entity.Attributes.SetString("origin", "bellalarm");
		sapi.World.SpawnEntity(entity);
		spawnedEntities.Add(entity);
	}
}
