using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace Vintagestory.GameContent;

public class EntityPartitioning : ModSystem, IEntityPartitioning
{
	public delegate bool RangeTestDelegate(Entity e, double posX, double posY, double posZ, double radiuSq);

	public const int partitionsLength = 4;

	private const int gridSizeInBlocks = 8;

	private ICoreAPI api;

	private ICoreClientAPI capi;

	private ICoreServerAPI sapi;

	public Dictionary<long, EntityPartitionChunk> Partitions = new Dictionary<long, EntityPartitionChunk>();

	private const int chunkSize = 32;

	public double LargestTouchDistance;

	public override double ExecuteOrder()
	{
		return 0.0;
	}

	public override bool ShouldLoad(EnumAppSide side)
	{
		return true;
	}

	public override void Start(ICoreAPI api)
	{
		this.api = api;
		api.Event.PlayerDimensionChanged += Event_PlayerDimensionChanged;
	}

	private void Event_PlayerDimensionChanged(IPlayer byPlayer)
	{
		RePartitionPlayer(byPlayer.Entity);
	}

	public override void StartClientSide(ICoreClientAPI api)
	{
		capi = api;
		api.Event.RegisterGameTickListener(OnClientTick, 32);
	}

	public override void StartServerSide(ICoreServerAPI api)
	{
		sapi = api;
		api.Event.RegisterGameTickListener(OnServerTick, 32);
		api.Event.PlayerSwitchGameMode += OnSwitchedGameMode;
	}

	private void OnClientTick(float dt)
	{
		PartitionEntities(capi.World.LoadedEntities.Values);
	}

	private void OnServerTick(float dt)
	{
		PartitionEntities(((CachingConcurrentDictionary<long, Entity>)sapi.World.LoadedEntities).Values);
	}

	private void PartitionEntities(ICollection<Entity> entities)
	{
		long chunkMapSizeX = api.World.BlockAccessor.MapSizeX / 32;
		long chunkMapSizeZ = api.World.BlockAccessor.MapSizeZ / 32;
		double largestTouchDistance = 0.0;
		Dictionary<long, EntityPartitionChunk> Partitions = this.Partitions;
		Partitions.Clear();
		foreach (Entity entity in entities)
		{
			if (entity.IsCreature && entity.touchDistance > largestTouchDistance)
			{
				largestTouchDistance = entity.touchDistance;
			}
			EntityPos pos = entity.Pos;
			int x = (int)pos.X;
			int z = (int)pos.Z;
			int gridIndex = z / 8 % 4 * 4 + x / 8 % 4;
			if (gridIndex >= 0)
			{
				long nowInChunkIndex3d = MapUtil.Index3dL(x / 32, (int)pos.InternalY / 32, z / 32, chunkMapSizeX, chunkMapSizeZ);
				if (!Partitions.TryGetValue(nowInChunkIndex3d, out var partition))
				{
					partition = (Partitions[nowInChunkIndex3d] = new EntityPartitionChunk());
				}
				List<Entity> list = partition.Add(entity, gridIndex);
				if (entity is EntityPlayer ep)
				{
					ep.entityListForPartitioning = list;
				}
			}
		}
		LargestTouchDistance = largestTouchDistance;
	}

	public void RePartitionPlayer(EntityPlayer entity)
	{
		entity.entityListForPartitioning?.Remove(entity);
		PartitionEntities(new Entity[1] { entity });
	}

	private void OnSwitchedGameMode(IServerPlayer player)
	{
		RePartitionPlayer(player.Entity);
	}

	[Obsolete("In version 1.19.2 and later, this searches only entities which are Creatures, which is probably what the caller wants but you should specify EnumEntitySearchType explicitly")]
	public Entity GetNearestEntity(Vec3d position, double radius, ActionConsumable<Entity> matches = null)
	{
		return GetNearestEntity(position, radius, matches, EnumEntitySearchType.Creatures);
	}

	public Entity GetNearestInteractableEntity(Vec3d position, double radius, ActionConsumable<Entity> matches = null)
	{
		if (matches == null)
		{
			return GetNearestEntity(position, radius, (Entity e) => e.IsInteractable, EnumEntitySearchType.Creatures);
		}
		return GetNearestEntity(position, radius, (Entity e) => matches(e) && e.IsInteractable, EnumEntitySearchType.Creatures);
	}

	public Entity GetNearestEntity(Vec3d position, double radius, ActionConsumable<Entity> matches, EnumEntitySearchType searchType)
	{
		Entity nearestEntity = null;
		double radiusSq = radius * radius;
		double nearestDistanceSq = radiusSq;
		if (api is ICoreClientAPI)
		{
			WalkEntities(position.X, position.Y, position.Z, radius, delegate(Entity e)
			{
				if (matches(e))
				{
					double num = e.Pos.SquareDistanceTo(position);
					if (num < nearestDistanceSq)
					{
						nearestDistanceSq = num;
						nearestEntity = e;
					}
				}
				return true;
			}, null, searchType);
		}
		else
		{
			WalkEntities(position.X, position.Y, position.Z, radius, delegate(Entity e)
			{
				double num = e.Pos.SquareDistanceTo(position);
				if (num < nearestDistanceSq && matches(e))
				{
					nearestDistanceSq = num;
					nearestEntity = e;
				}
				return true;
			}, null, searchType);
		}
		return nearestEntity;
	}

	private bool onIsInRangeServer(Entity e, double posX, double posY, double posZ, double radiusSq)
	{
		EntityPos pos = e.Pos;
		double dx = pos.X - posX;
		double dy = pos.InternalY - posY;
		double dz = pos.Z - posZ;
		return dx * dx + dy * dy + dz * dz < radiusSq;
	}

	private bool onIsInRangeClient(Entity e, double posX, double posY, double posZ, double radiusSq)
	{
		EntityPos pos = e.Pos;
		double dx = pos.X - posX;
		double dy = pos.InternalY - posY;
		double dz = pos.Z - posZ;
		return dx * dx + dy * dy + dz * dz < radiusSq;
	}

	[Obsolete("In version 1.19.2 and later, this walks through Creature entities only, so recommended to call WalkEntityPartitions() specifying the type of search explicitly for clarity in the calling code")]
	public void WalkEntities(Vec3d centerPos, double radius, ActionConsumable<Entity> callback)
	{
		WalkEntities(centerPos, radius, callback, EnumEntitySearchType.Creatures);
	}

	[Obsolete("In version 1.19.2 and later, use WalkEntities specifying the searchtype (Creatures or Inanimate) explitly in the calling code.")]
	public void WalkInteractableEntities(Vec3d centerPos, double radius, ActionConsumable<Entity> callback)
	{
		WalkEntities(centerPos, radius, callback, EnumEntitySearchType.Creatures);
	}

	public void WalkEntities(Vec3d centerPos, double radius, ActionConsumable<Entity> callback, EnumEntitySearchType searchType)
	{
		if (api.Side == EnumAppSide.Client)
		{
			WalkEntities(centerPos.X, centerPos.Y, centerPos.Z, radius, callback, onIsInRangeClient, searchType);
		}
		else
		{
			WalkEntities(centerPos.X, centerPos.Y, centerPos.Z, radius, callback, onIsInRangeServer, searchType);
		}
	}

	public void WalkEntityPartitions(Vec3d centerPos, double radius, ActionConsumable<Entity> callback)
	{
		WalkEntities(centerPos.X, centerPos.Y, centerPos.Z, radius, callback, null, EnumEntitySearchType.Creatures);
	}

	public void WalkEntities(double centerPosX, double centerPosY, double centerPosZ, double radius, ActionConsumable<Entity> callback, RangeTestDelegate onRangeTest, EnumEntitySearchType searchType)
	{
		IBlockAccessor blockAccessor = api.World.BlockAccessor;
		long chunkMapSizeX = blockAccessor.MapSizeX / 32;
		long chunkMapSizeZ = blockAccessor.MapSizeZ / 32;
		int gridXMax = blockAccessor.MapSizeX / 8 - 1;
		int cyTop = blockAccessor.MapSizeY / 32 - 1;
		int gridZMax = blockAccessor.MapSizeZ / 8 - 1;
		int mingx = (int)GameMath.Clamp((centerPosX - radius) / 8.0, 0.0, gridXMax);
		int maxgx = (int)GameMath.Clamp((centerPosX + radius) / 8.0, 0.0, gridXMax);
		int num = (int)GameMath.Clamp((centerPosY - radius) / 32.0, 0.0, cyTop);
		int maxcy = (int)GameMath.Clamp((centerPosY + radius) / 32.0, 0.0, cyTop);
		int mingz = (int)GameMath.Clamp((centerPosZ - radius) / 8.0, 0.0, gridZMax);
		int maxgz = (int)GameMath.Clamp((centerPosZ + radius) / 8.0, 0.0, gridZMax);
		double radiusSq = radius * radius;
		Dictionary<long, EntityPartitionChunk> Partitions = this.Partitions;
		EntityPartitionChunk partitionChunk = null;
		long lastIndex3d = -1L;
		for (int cy = num; cy <= maxcy; cy++)
		{
			for (int gridZ = mingz; gridZ <= maxgz; gridZ++)
			{
				int cz = gridZ / 4;
				int indexBase = gridZ % 4 * 4;
				long index3d = MapUtil.Index3dL(0, cy, cz, chunkMapSizeX, chunkMapSizeZ);
				for (int gridX = mingx; gridX <= maxgx; gridX++)
				{
					if (index3d + gridX / 4 != lastIndex3d)
					{
						lastIndex3d = index3d + gridX / 4;
						Partitions.TryGetValue(lastIndex3d, out partitionChunk);
					}
					if (partitionChunk == null)
					{
						continue;
					}
					object obj;
					if (searchType != EnumEntitySearchType.Creatures)
					{
						List<Entity>[] inanimateEntities = partitionChunk.InanimateEntities;
						obj = ((inanimateEntities != null) ? inanimateEntities[indexBase + gridX % 4] : null);
					}
					else
					{
						obj = partitionChunk.Entities[indexBase + gridX % 4];
					}
					List<Entity> entities = (List<Entity>)obj;
					if (entities == null)
					{
						continue;
					}
					if (onRangeTest == null)
					{
						foreach (Entity entity in entities)
						{
							if (!callback(entity))
							{
								return;
							}
						}
						continue;
					}
					foreach (Entity entity2 in entities)
					{
						if (onRangeTest(entity2, centerPosX, centerPosY, centerPosZ, radiusSq) && !callback(entity2))
						{
							return;
						}
					}
				}
			}
		}
	}
}
