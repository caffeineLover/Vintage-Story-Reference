using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace Vintagestory.Server;

public class EntitySpawnerResult
{
	private List<SpawnOppurtunity> spawnPositions;

	private SpawnState spawnState;

	private int quantityToSpawn;

	public EntitySpawnerResult(List<SpawnOppurtunity> spawnPositions, SpawnState spawnState)
	{
		this.spawnPositions = spawnPositions;
		this.spawnState = spawnState;
		quantityToSpawn = spawnState.NextGroupSize;
	}

	public void Spawn(ServerMain server, ServerSystemEntitySpawner entitySpawner)
	{
		long herdid = server.GetNextHerdId();
		if (server.SpawnDebug)
		{
			SpawnOppurtunity firstPos = spawnPositions[0];
			ServerMain.Logger.Notification("Spawn {0}x {1} @{2}/{3}/{4}", spawnPositions.Count, firstPos.ForType.Code, (int)firstPos.Pos.X, (int)firstPos.Pos.Y, (int)firstPos.Pos.Z);
		}
		BlockPos tmpPos = new BlockPos(0);
		RuntimeSpawnConditions sc = spawnState.ForType.Server.SpawnConditions.Runtime;
		int totalSpawned = 0;
		foreach (SpawnOppurtunity so in spawnPositions)
		{
			if (quantityToSpawn-- <= 0)
			{
				break;
			}
			EntityProperties props = so.ForType;
			if (entitySpawner.CheckCanSpawnAt(props, sc, tmpPos.SetAndCorrectDimension(so.Pos)))
			{
				AssetLocation originalType = props.Code;
				if (server.EventManager.TriggerTrySpawnEntity(server.blockAccessor, ref props, so.Pos, herdid))
				{
					DoSpawn(server, props, so.Pos, herdid, originalType);
					totalSpawned++;
				}
			}
		}
		ServerMain.FrameProfiler.Mark(spawnState.profilerName);
		if (totalSpawned < quantityToSpawn)
		{
			spawnState.SpawnableAmountGlobal += quantityToSpawn - totalSpawned;
		}
	}

	private static void DoSpawn(ServerMain server, EntityProperties entityType, Vec3d spawnPosition, long herdid, AssetLocation originalType)
	{
		Entity entity = server.Api.ClassRegistry.CreateEntity(entityType);
		if (entity is EntityAgent agent)
		{
			agent.HerdId = herdid;
		}
		EntityPos serverPos = entity.Pos;
		serverPos.SetPosWithDimension(spawnPosition);
		serverPos.SetYaw((float)((IWorldAccessor)server).Rand.NextDouble() * ((float)Math.PI * 2f));
		entity.Pos.SetFrom(serverPos);
		entity.PositionBeforeFalling.Set(serverPos.X, serverPos.Y, serverPos.Z);
		if (entityType.Code != originalType)
		{
			entity.Attributes.SetString("originaltype", originalType.ToString());
		}
		entity.Attributes.SetString("origin", "entityspawner");
		server.DelayedSpawnQueue.Enqueue(entity);
	}
}
