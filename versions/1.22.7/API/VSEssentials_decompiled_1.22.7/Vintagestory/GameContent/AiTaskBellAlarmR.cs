using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class AiTaskBellAlarmR : AiTaskBaseTargetableR
{
	protected int nextSpawnIntervalMs;

	protected List<Entity> spawnedEntities = new List<Entity>();

	protected float timeSinceLastSpawnSec;

	protected CollisionTester collisionTester = new CollisionTester();

	private AiTaskBellAlarmConfig Config => GetConfig<AiTaskBellAlarmConfig>();

	public AiTaskBellAlarmR(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig)
		: base(entity, taskConfig, aiConfig)
	{
		baseConfig = AiTaskBaseR.LoadConfig<AiTaskBellAlarmConfig>(entity, taskConfig, aiConfig);
	}

	public override bool ShouldExecute()
	{
		if (!PreconditionsSatisficed())
		{
			return false;
		}
		return SearchForTarget();
	}

	public override void StartExecute()
	{
		if (Config.RepeatSound != null)
		{
			(entity.Api as ICoreServerAPI)?.Network.BroadcastEntityPacket(entity.EntityId, 1025, SerializerUtil.Serialize(Config.RepeatSound));
		}
		nextSpawnIntervalMs = Config.SpawnIntervalMinMs + entity.World.Rand.Next(Config.SpawnIntervalMaxMs - Config.SpawnIntervalMinMs);
		base.StartExecute();
	}

	public override bool ContinueExecute(float dt)
	{
		if (!base.ContinueExecute(dt))
		{
			return false;
		}
		if (targetEntity == null)
		{
			return false;
		}
		timeSinceLastSpawnSec += dt;
		if (timeSinceLastSpawnSec * 1000f > (float)nextSpawnIntervalMs)
		{
			if (!(entity.Api is ICoreServerAPI api))
			{
				return false;
			}
			int numberOfPlayersInRange = entity.World.GetPlayersAround(entity.Pos.XYZ, Config.PlayerSpawnScaleRange, Config.PlayerSpawnScaleRange, (IPlayer player) => player.Entity.Alive).Length;
			float playerScaling = 1f + (float)(numberOfPlayersInRange - 1) * api.Server.Config.SpawnCapPlayerScaling * Config.PlayerScalingFactor;
			TrySpawnCreatures(GameMath.RoundRandom(base.Rand, (float)Config.SpawnMaxQuantity * playerScaling - 1f) + 1, Config.SpawnRange);
			nextSpawnIntervalMs = Config.SpawnIntervalMinMs + entity.World.Rand.Next(Config.SpawnIntervalMaxMs - Config.SpawnIntervalMinMs);
			timeSinceLastSpawnSec = 0f;
		}
		if (targetEntity.Pos.SquareDistanceTo(entity.Pos) > Config.MaxDistanceToTarget * Config.MaxDistanceToTarget)
		{
			return false;
		}
		return true;
	}

	public override void FinishExecute(bool cancelled)
	{
		(entity.Api as ICoreServerAPI)?.Network.BroadcastEntityPacket(entity.EntityId, 1026);
		base.FinishExecute(cancelled);
	}

	public override void OnEntityDespawn(EntityDespawnData reason)
	{
		(entity.Api as ICoreServerAPI)?.Network.BroadcastEntityPacket(entity.EntityId, 1026);
		base.OnEntityDespawn(reason);
	}

	protected virtual void TrySpawnCreatures(int maxQuantity, int range)
	{
		if (!(entity.Api is ICoreServerAPI api))
		{
			return;
		}
		FastVec3d centerPos = new FastVec3d(entity.Pos.X, entity.Pos.Y, entity.Pos.Z);
		Vec3d spawnPos = new Vec3d();
		BlockPos spawnPosi = new BlockPos(0);
		BlockPos spawnPosBuffer = new BlockPos(0);
		for (int index = 0; index < spawnedEntities.Count; index++)
		{
			if (spawnedEntities[index] == null || !spawnedEntities[index].Alive)
			{
				spawnedEntities.RemoveAt(index);
				index--;
			}
		}
		if (Config.EntitiesToSpawn.Length == 0 || spawnedEntities.Count > maxQuantity)
		{
			return;
		}
		int tries = 50;
		int spawned = 0;
		while (tries-- > 0 && spawned < 1)
		{
			int index2 = base.Rand.Next(Config.EntitiesToSpawn.Length);
			EntityProperties type = Config.EntitiesToSpawn[index2];
			int rndx = api.World.Rand.Next(2 * range) - range;
			int rndy = api.World.Rand.Next(2 * range) - range;
			int rndz = api.World.Rand.Next(2 * range) - range;
			spawnPos.Set((double)((int)centerPos.X + rndx) + 0.5, (double)((int)centerPos.Y + rndy) + 0.001, (double)((int)centerPos.Z + rndz) + 0.5);
			spawnPosi.Set((int)spawnPos.X, (int)spawnPos.Y, (int)spawnPos.Z);
			while (api.World.BlockAccessor.GetBlockBelow(spawnPosi).Id == 0 && spawnPos.Y > 0.0)
			{
				spawnPosi.Y--;
				spawnPos.Y -= 1.0;
			}
			spawnPosBuffer.Set((int)spawnPos.X, (int)spawnPos.Y, (int)spawnPos.Z);
			if (api.World.BlockAccessor.IsValidPos(spawnPosBuffer))
			{
				Cuboidf collisionBox = type.SpawnCollisionBox.OmniNotDownGrowBy(0.1f);
				if (!collisionTester.IsColliding(api.World.BlockAccessor, collisionBox, spawnPos, alsoCheckTouch: false))
				{
					DoSpawn(type, spawnPos, entity.HerdId);
					spawned++;
				}
			}
		}
	}

	protected virtual void DoSpawn(EntityProperties entityType, Vec3d spawnPosition, long herdId)
	{
		Entity entityToSpawn = entity.Api.ClassRegistry.CreateEntity(entityType);
		if (entityToSpawn is EntityAgent agentToSpawn)
		{
			agentToSpawn.HerdId = herdId;
		}
		entityToSpawn.Pos.SetPosWithDimension(spawnPosition);
		entityToSpawn.Pos.SetYaw((float)base.Rand.NextDouble() * ((float)Math.PI * 2f));
		entityToSpawn.PositionBeforeFalling.Set(entityToSpawn.Pos.X, entityToSpawn.Pos.Y, entityToSpawn.Pos.Z);
		entityToSpawn.Attributes.SetString("origin", Config.Origin);
		entity.World.SpawnEntity(entityToSpawn);
		spawnedEntities.Add(entityToSpawn);
	}

	protected override bool CheckDetectionRange(Entity target, double range)
	{
		if (!base.CheckDetectionRange(target, range))
		{
			return false;
		}
		if (!(target is EntityPlayer player))
		{
			return true;
		}
		double num = target.Pos.DistanceTo(entity.Pos.XYZ);
		bool usingHands = player.ServerControls.LeftMouseDown || player.ServerControls.RightMouseDown || player.ServerControls.HandUse != EnumHandInteract.None;
		bool moving = player.ServerControls.TriesToMove || player.ServerControls.Jump || !player.OnGround;
		bool num2 = !usingHands && !moving;
		bool quiet = player.ServerControls.Sneak && !player.ServerControls.Jump && player.OnGround && !usingHands;
		if (num2)
		{
			range *= (double)Config.SilentSoundRangeReduction;
		}
		else if (quiet)
		{
			range *= (double)Config.QuietSoundRangeReduction;
		}
		return num <= range;
	}

	protected override float GetSeekingRange()
	{
		float range = base.GetSeekingRange();
		if (!entity.GetBehavior<EntityBehaviorTaskAI>().TaskManager.IsTaskActive(Config.ListenAiTaskId))
		{
			range *= Config.NotListeningRangeReduction;
		}
		return range;
	}
}
