using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.Server;

public class ServerSystemEntitySpawner : ServerSystem
{
	private bool multithreaded = true;

	private const int yWiggle = 3;

	private const int xzWiggle = 5;

	private const int chunksize = 32;

	private const int chunkRange = 3;

	private const float globalMultiplier = 1f;

	private int GraceTimerDayNotify = -1;

	private double GraceTimeUntilTotalDays = 5.0;

	private List<SpawnArea> spawnAreas = new List<SpawnArea>();

	private List<SpawnState> spawnStates = new List<SpawnState>();

	private HashSet<long> chunkColumnCoordsTmp = new HashSet<long>();

	private CollisionTester collisionTester = new CollisionTester();

	private Dictionary<AssetLocation, EntityProperties> entityTypesByCode = new Dictionary<AssetLocation, EntityProperties>();

	private Random rand = new Random();

	private ushort[] heightMap;

	private ICachingBlockAccessor cachingBlockAccessor;

	private float slowaccum;

	private bool first = true;

	private int SeaLevel;

	private int SkyHeight;

	private BlockPos spawnPosition = new BlockPos();

	private int[] shuffledY = new int[7] { -3, -2, -1, 0, 1, 2, 3 };

	private int surfaceMapTryID;

	private bool errorLoggedThisTick;

	public volatile bool paused;

	private ConcurrentQueue<EntitySpawnerResult> readyToSpawn = new ConcurrentQueue<EntitySpawnerResult>();

	private readonly BlockPos tmpPos = new BlockPos(0);

	public ServerSystemEntitySpawner(ServerMain server)
		: base(server)
	{
		multithreaded = !server.ReducedServerThreads;
	}

	public override void OnBeginModsAndConfigReady()
	{
		List<EntityProperties> entityTypes = server.EntityTypes;
		for (int i = 0; i < entityTypes.Count; i++)
		{
			EntityProperties entityType = entityTypes[i];
			entityTypesByCode[entityType.Code] = entityType;
		}
	}

	public override void OnBeginGameReady(SaveGame savegame)
	{
		base.OnBeginGameReady(savegame);
		cachingBlockAccessor = server.GetCachingBlockAccessor(synchronize: false, relight: false);
		server.RegisterGameTickListener(SpawnReadyMobs, 250);
		if (multithreaded)
		{
			Thread thread = TyronThreadPool.CreateDedicatedThread(new SpawnerOffthread(this).Start, "physicsManagerHelper");
			thread.IsBackground = true;
			thread.Priority = Thread.CurrentThread.Priority;
			server.Serverthreads.Add(thread);
		}
		if (savegame.IsNewWorld || !savegame.ModData.ContainsKey("graceTimeUntilTotalDays"))
		{
			string s = savegame.WorldConfiguration.GetString("graceTimer", "5");
			GraceTimeUntilTotalDays = 5.0;
			double.TryParse(s, out GraceTimeUntilTotalDays);
			if (!savegame.IsNewWorld && !savegame.ModData.ContainsKey("graceTimeUntilTotalDays"))
			{
				int dayspm = Math.Max(1, savegame.WorldConfiguration.GetAsInt("daysPerMonth", 12));
				double daysStart = 1f / 3f + (float)(dayspm * 4);
				GraceTimeUntilTotalDays += daysStart;
			}
			else
			{
				GraceTimeUntilTotalDays += server.Calendar.TotalDays;
			}
			savegame.ModData["graceTimeUntilTotalDays"] = SerializerUtil.Serialize(GraceTimeUntilTotalDays);
		}
		else
		{
			GraceTimeUntilTotalDays = SerializerUtil.Deserialize<double>(savegame.ModData["graceTimeUntilTotalDays"]);
		}
		Dictionary<AssetLocation, Block[]> searchCache = new Dictionary<AssetLocation, Block[]>();
		LCGRandom lcgrnd = new LCGRandom();
		lcgrnd.SetWorldSeed(server.World.Seed);
		foreach (EntityProperties type in server.EntityTypes)
		{
			(type.Server.SpawnConditions?.Runtime)?.Initialise(lcgrnd, server, type.Code.ToShortString(), searchCache);
		}
	}

	public override void Dispose()
	{
		cachingBlockAccessor?.Dispose();
		cachingBlockAccessor = null;
		readyToSpawn.Clear();
	}

	public override void OnServerPause()
	{
		paused = true;
	}

	public override void OnServerResume()
	{
		paused = false;
	}

	private void SpawnReadyMobs(float dt)
	{
		if (server.SaveGameData.EntitySpawning)
		{
			EntitySpawnerResult spawngroup;
			while (!readyToSpawn.IsEmpty && readyToSpawn.TryDequeue(out spawngroup))
			{
				spawngroup.Spawn(server, this);
			}
			PrepareForSpawning(dt);
		}
	}

	internal void PrepareForSpawning(float dt)
	{
		slowaccum += dt;
		int day = (int)server.Calendar.TotalDays;
		double daysLeft = GraceTimeUntilTotalDays - server.Calendar.TotalDays + 0.25;
		if (GraceTimerDayNotify != day && daysLeft >= 0.0)
		{
			GraceTimerDayNotify = day;
			if ((int)daysLeft > 1)
			{
				server.SendMessageToGeneral(Lang.Get("server-xdaysleft", (int)daysLeft), EnumChatType.Notification);
			}
			if ((int)daysLeft == 1)
			{
				server.SendMessageToGeneral(Lang.Get("server-1dayleft"), EnumChatType.Notification);
			}
			if ((int)daysLeft == 0)
			{
				server.SendMessageToGeneral(Lang.Get("server-monsterbegins"), EnumChatType.Notification);
			}
		}
		if (first || slowaccum > 10f)
		{
			LoadViableSpawnAreas();
			slowaccum = 0f;
			first = false;
		}
		if (!multithreaded)
		{
			FindMobSpawnPositions_offthread(dt);
		}
	}

	internal void FindMobSpawnPositions_offthread(float dt)
	{
		if (server.Clients.IsEmpty)
		{
			return;
		}
		ReloadSpawnStates_offthread();
		cachingBlockAccessor.Begin();
		SeaLevel = server.SeaLevel;
		SkyHeight = server.WorldMap.MapSizeY - SeaLevel;
		errorLoggedThisTick = false;
		List<SpawnState> spawnStates = this.spawnStates;
		List<SpawnArea> spawnAreas = this.spawnAreas;
		for (int i = 0; i < spawnAreas.Count; i++)
		{
			SpawnArea spawnArea = spawnAreas[i];
			for (int k = 0; k < spawnArea.ChunkColumnCoords.Length; k++)
			{
				long num = spawnArea.ChunkColumnCoords[k];
				int x = (int)num;
				int z = (int)(num >> 32);
				TrySpawnSomethingAt_offthread(x, spawnArea.chunkY, z, spawnArea.spawnCounts, spawnStates);
			}
		}
	}

	private void TrySpawnSomethingAt_offthread(int baseX, int baseY, int baseZ, Dictionary<AssetLocation, int> spawnCounts, List<SpawnState> spawnStates)
	{
		ServerWorldMap WorldMap = server.WorldMap;
		heightMap = WorldMap.GetMapChunk(baseX, baseZ)?.WorldGenTerrainHeightMap;
		if (heightMap == null)
		{
			return;
		}
		IWorldChunk[] chunkCol = new IWorldChunk[WorldMap.ChunkMapSizeY];
		bool anyNull = false;
		for (int cy = 0; cy < chunkCol.Length; cy++)
		{
			IWorldChunk chunk = (chunkCol[cy] = WorldMap.GetChunk(baseX, cy, baseZ));
			if (chunk == null)
			{
				anyNull = true;
				continue;
			}
			chunk.Unpack_ReadOnly();
			chunk.AcquireBlockReadLock();
		}
		try
		{
			if (!anyNull)
			{
				TrySpawnSomethingAt_offthrad(baseX, baseY, baseZ, spawnCounts, chunkCol, spawnStates);
			}
		}
		catch (Exception e)
		{
			if (!errorLoggedThisTick)
			{
				errorLoggedThisTick = true;
				server.World.Logger.Warning("Error when testing to spawn entities at " + baseX * 32 + "," + baseY * 32 + "," + baseZ * 32);
				server.World.Logger.Error(e);
			}
		}
		finally
		{
			for (int i = 0; i < chunkCol.Length; i++)
			{
				chunkCol[i]?.ReleaseBlockReadLock();
			}
		}
	}

	private void TrySpawnSomethingAt_offthrad(int baseX, int baseY, int baseZ, Dictionary<AssetLocation, int> spawnCounts, IWorldChunk[] chunkCol, List<SpawnState> spawnStates)
	{
		BlockPos spawnPosition = this.spawnPosition;
		int mapsizey = server.WorldMap.MapSizeY;
		List<SpawnOppurtunity> spawnPositions = new List<SpawnOppurtunity>();
		int surfaceMapTryID = this.surfaceMapTryID++;
		int SeaLevel = this.SeaLevel;
		int SkyHeight = this.SkyHeight;
		ushort[] heightMap = this.heightMap;
		Random rand = this.rand;
		int[] shuffledY = this.shuffledY;
		shuffledY.Shuffle(rand);
		for (int yIndex = 0; yIndex < shuffledY.Length; yIndex++)
		{
			int startY = (baseY + shuffledY[yIndex]) * 32 + rand.Next(32);
			if (startY <= -3 || startY >= mapsizey + 3)
			{
				continue;
			}
			int startX;
			int startZ;
			spawnPosition.Set(startX = baseX * 32 + rand.Next(32), startY, startZ = baseZ * 32 + rand.Next(32));
			foreach (SpawnState spawnState in spawnStates)
			{
				RuntimeSpawnConditions sc = spawnState.ForType.Server.SpawnConditions.Runtime;
				if (spawnState.SpawnableAmountGlobal <= 0)
				{
					continue;
				}
				spawnCounts.TryGetValue(spawnState.ForType.Code, out var areaSpawned);
				if (areaSpawned > spawnState.SpawnCapScaledPerPlayer)
				{
					continue;
				}
				if (!sc.TryOnlySurface)
				{
					double y = startY + 3;
					double yRel = ((y > (double)SeaLevel) ? (1.0 + (y - (double)SeaLevel) / (double)SkyHeight) : (y / (double)SeaLevel));
					if ((double)sc.MinY > yRel)
					{
						continue;
					}
					y -= 6.0;
					yRel = ((y > (double)SeaLevel) ? (1.0 + (y - (double)SeaLevel) / (double)SkyHeight) : (y / (double)SeaLevel));
					if ((double)sc.MaxY < yRel)
					{
						continue;
					}
				}
				int tries = 10;
				while (spawnState.NextGroupSize <= 0 && tries-- > 0)
				{
					float val = sc.HerdSize.nextFloat();
					spawnState.NextGroupSize = (int)val + (((double)(val - (float)(int)val) > rand.NextDouble()) ? 1 : 0);
				}
				if (spawnState.NextGroupSize <= 0)
				{
					spawnState.NextGroupSize = -1;
					continue;
				}
				spawnPositions.Clear();
				int qSelfAndCompanions = spawnState.SelfAndCompanionProps.Length;
				EntityProperties typeToSpawn = spawnState.SelfAndCompanionProps[0];
				if (!server.EventManager.TriggerTrySpawnGroupNearOffthread(server.blockAccessor, typeToSpawn, spawnPosition))
				{
					continue;
				}
				tries = spawnState.NextGroupSize * 4 + 5;
				for (int i = 0; i < tries && spawnPositions.Count < spawnState.NextGroupSize; i++)
				{
					spawnPosition.X = randomWithinSameChunk(startX);
					spawnPosition.Z = randomWithinSameChunk(startZ);
					if (sc.TryOnlySurface)
					{
						int mapIndex = spawnPosition.Z % 32 * 32 + spawnPosition.X % 32;
						if (spawnState.surfaceMap == null)
						{
							spawnState.surfaceMap = new int[1024];
						}
						if (spawnState.surfaceMap[mapIndex] == surfaceMapTryID)
						{
							continue;
						}
						spawnState.surfaceMap[mapIndex] = surfaceMapTryID;
						spawnPosition.Y = heightMap[mapIndex] + 1;
					}
					else
					{
						spawnPosition.Y = Math.Max(1, startY + rand.Next(7) - 3);
					}
					if (spawnPosition.Y < 1 || spawnPosition.Y >= mapsizey)
					{
						i++;
						continue;
					}
					double yRel2 = ((spawnPosition.Y > SeaLevel) ? (1.0 + (double)(spawnPosition.Y - SeaLevel) / (double)SkyHeight) : ((double)spawnPosition.Y / (double)SeaLevel));
					if ((double)sc.MinY > yRel2 || (double)sc.MaxY < yRel2)
					{
						i++;
						continue;
					}
					if (spawnPositions.Count > 0 && qSelfAndCompanions > 1)
					{
						int rnd = 1 + rand.Next(qSelfAndCompanions - 1);
						typeToSpawn = spawnState.SelfAndCompanionProps[rnd];
					}
					Vec3d canSpawnPos = CanSpawnAt_offthread(typeToSpawn, spawnPosition, sc, chunkCol);
					if (canSpawnPos != null)
					{
						spawnPositions.Add(new SpawnOppurtunity
						{
							ForType = typeToSpawn,
							Pos = canSpawnPos
						});
					}
					if (spawnPositions.Count == 0)
					{
						i++;
					}
				}
				if (spawnPositions.Count >= spawnState.NextGroupSize)
				{
					EntitySpawnerResult toSpawn = new EntitySpawnerResult(new List<SpawnOppurtunity>(spawnPositions), spawnState);
					readyToSpawn.Enqueue(toSpawn);
					spawnState.SpawnableAmountGlobal -= spawnState.NextGroupSize;
					spawnState.NextGroupSize = -1;
				}
			}
		}
	}

	private int randomWithinSameChunk(int x)
	{
		return (x & -32) + (x + rand.Next(11) - 5 + 32) % 32;
	}

	public Vec3d CanSpawnAt_offthread(EntityProperties type, BlockPos spawnPosition, RuntimeSpawnConditions sc, IWorldChunk[] chunkCol)
	{
		ServerWorldMap WorldMap = server.WorldMap;
		if (spawnPosition.Y <= 0 || spawnPosition.Y >= WorldMap.MapSizeY)
		{
			return null;
		}
		BlockPos tmpPos = this.tmpPos;
		IWorldAccessor worldAccessor = WorldMap.World;
		try
		{
			tmpPos.Set(spawnPosition);
			ClimateCondition climate;
			Block block;
			if (sc.TryOnlySurface)
			{
				climate = GetSuitableClimateTemperatureRainfall(WorldMap, sc);
				if (climate == null)
				{
					return null;
				}
				block = chunkCol[tmpPos.Y / 32].GetLocalBlockAtBlockPos_LockFree(worldAccessor, tmpPos);
				if (!sc.CanSpawnInside(block))
				{
					return null;
				}
				tmpPos.Y--;
				if (!chunkCol[tmpPos.Y / 32].GetLocalBlockAtBlockPos_LockFree(worldAccessor, tmpPos, 1).CanCreatureSpawnOn(cachingBlockAccessor, tmpPos, type, sc))
				{
					return null;
				}
			}
			else
			{
				block = chunkCol[tmpPos.Y / 32].GetLocalBlockAtBlockPos_LockFree(worldAccessor, tmpPos);
				bool canspawn = false;
				for (int i = 0; i < 5; i++)
				{
					if (--tmpPos.Y < 0)
					{
						break;
					}
					Block belowBlock = chunkCol[tmpPos.Y / 32].GetLocalBlockAtBlockPos_LockFree(worldAccessor, tmpPos);
					if (sc.CanSpawnInside(block) && belowBlock.CanCreatureSpawnOn(cachingBlockAccessor, tmpPos, type, sc))
					{
						canspawn = true;
						break;
					}
					spawnPosition.Y--;
					block = belowBlock;
				}
				if (!canspawn)
				{
					return null;
				}
				climate = GetSuitableClimateTemperatureRainfall(WorldMap, sc);
				if (climate == null)
				{
					return null;
				}
			}
			tmpPos.Y++;
			IMapRegion mapregion = WorldMap.GetMapRegion(tmpPos);
			WorldMap.AddWorldGenForestShrub(climate, mapregion, tmpPos);
			if (!sc.MatchesForestation(climate))
			{
				return null;
			}
			double yOffset = 1E-07;
			Cuboidf[] insideBlockBoxes = block.GetCollisionBoxes(server.BlockAccessor, tmpPos);
			if (insideBlockBoxes != null && insideBlockBoxes.Length != 0)
			{
				yOffset += (double)(insideBlockBoxes[0].MaxY % 1f);
			}
			Vec3d spawnPosition3d = new Vec3d((double)spawnPosition.X + 0.5, (double)spawnPosition.Y + yOffset, (double)spawnPosition.Z + 0.5);
			Cuboidf collisionBox = type.SpawnCollisionBox.OmniNotDownGrowBy(0.1f);
			if (collisionTester.IsColliding(server.BlockAccessor, collisionBox, spawnPosition3d, alsoCheckTouch: false))
			{
				return null;
			}
			IPlayer plr = server.NearestPlayer(spawnPosition3d.X, spawnPosition3d.Y, spawnPosition3d.Z);
			if (plr?.Entity != null && !plr.Entity.CanSpawnNearby(type, spawnPosition3d, sc))
			{
				return null;
			}
			if (sc.HabitatSize > 0 && (type.Habitat == EnumHabitat.Underwater || type.Habitat == EnumHabitat.Sea))
			{
				int hb = sc.HabitatSize;
				block = chunkCol[tmpPos.Y / 32].GetLocalBlockAtBlockPos_LockFree(worldAccessor, tmpPos.Set(spawnPosition3d).Add(BlockFacing.NORTH, hb));
				if (!sc.CanSpawnInside(block))
				{
					return null;
				}
				block = chunkCol[tmpPos.Y / 32].GetLocalBlockAtBlockPos_LockFree(worldAccessor, tmpPos.Set(spawnPosition3d).Add(BlockFacing.EAST, hb));
				if (!sc.CanSpawnInside(block))
				{
					return null;
				}
				block = chunkCol[tmpPos.Y / 32].GetLocalBlockAtBlockPos_LockFree(worldAccessor, tmpPos.Set(spawnPosition3d).Add(BlockFacing.WEST, hb));
				if (!sc.CanSpawnInside(block))
				{
					return null;
				}
				block = chunkCol[tmpPos.Y / 32].GetLocalBlockAtBlockPos_LockFree(worldAccessor, tmpPos.Set(spawnPosition3d).Add(BlockFacing.SOUTH, hb));
				if (!sc.CanSpawnInside(block))
				{
					return null;
				}
			}
			return spawnPosition3d;
		}
		catch (Exception e)
		{
			if (!errorLoggedThisTick)
			{
				errorLoggedThisTick = true;
				server.World.Logger.Warning("Error when testing to spawn entity {0} at position {1}, can report to dev team but otherwise should do no harm.", type.Code.ToShortString(), spawnPosition);
				server.World.Logger.Error(e);
			}
			return null;
		}
	}

	public bool CheckCanSpawnAt(EntityProperties type, RuntimeSpawnConditions sc, BlockPos spawnPosition)
	{
		ServerWorldMap WorldMap = server.WorldMap;
		if (spawnPosition.Y <= 0 || spawnPosition.Y >= WorldMap.MapSizeY)
		{
			return false;
		}
		IBlockAccessor blockAccessor = WorldMap.World.BlockAccessor;
		try
		{
			Block block = blockAccessor.GetBlock(spawnPosition);
			if (!sc.CanSpawnInside(block))
			{
				return false;
			}
			spawnPosition.Y--;
			return blockAccessor.GetBlock(spawnPosition, 1).CanCreatureSpawnOn(blockAccessor, spawnPosition, type, sc);
		}
		catch (Exception e)
		{
			server.World.Logger.Warning("Error when re-testing to spawn entity {0} at position {1}, can report to dev team but otherwise should do no harm.", type.Code.ToShortString(), spawnPosition);
			server.World.Logger.Error(e);
			return false;
		}
	}

	private ClimateCondition GetSuitableClimateTemperatureRainfall(ServerWorldMap worldMap, RuntimeSpawnConditions sc)
	{
		int origY = tmpPos.Y;
		tmpPos.Y = (int)((double)server.seaLevel * 1.09);
		ClimateCondition climate = worldMap.getWorldGenClimateAt(tmpPos, temperatureRainfallOnly: true);
		if (climate == null)
		{
			tmpPos.Y = origY;
			return null;
		}
		if (sc.ClimateValueMode != EnumGetClimateMode.WorldGenValues)
		{
			worldMap.GetClimateAt(tmpPos, climate, sc.ClimateValueMode, server.Calendar.TotalDays);
		}
		tmpPos.Y = origY;
		if (!sc.MatchesClimate(climate))
		{
			return null;
		}
		return climate;
	}

	private void LoadViableSpawnAreas()
	{
		List<SpawnArea> spawnAreas = new List<SpawnArea>();
		HashSet<long> chunkColumnCoordsTmp = this.chunkColumnCoordsTmp;
		ServerWorldMap worldMap = server.WorldMap;
		foreach (ConnectedClient client in server.Clients.Values)
		{
			if (!client.IsPlayingClient || client.Entityplayer == null)
			{
				continue;
			}
			EntityPos cpos = client.Entityplayer.Pos;
			if (cpos.Dimension != 0)
			{
				continue;
			}
			int chunkX = (int)cpos.X / 32;
			int chunkY = (int)cpos.Y / 32;
			int chunkZ = (int)cpos.Z / 32;
			SpawnArea area = new SpawnArea();
			area.chunkY = chunkY;
			chunkColumnCoordsTmp.Clear();
			for (int dx = -3; dx <= 3; dx++)
			{
				int x = chunkX + dx;
				for (int dz = -3; dz <= 3; dz++)
				{
					int z = chunkZ + dz;
					bool columnLoaded = false;
					for (int dy = Math.Max(-3, -chunkY); dy <= 3; dy++)
					{
						IWorldChunk chunk = worldMap.GetChunk(x, chunkY + dy, z);
						if (chunk == null)
						{
							continue;
						}
						columnLoaded = true;
						Entity[] chunkEntities = chunk.Entities;
						if (chunkEntities == null)
						{
							continue;
						}
						for (int i = 0; i < chunkEntities.Length; i++)
						{
							Entity e = chunkEntities[i];
							int cnt = 0;
							if (e != null)
							{
								area.spawnCounts.TryGetValue(e.Code, out cnt);
							}
							else if (i >= chunk.EntitiesCount)
							{
								break;
							}
							area.spawnCounts[e.Code] = cnt + 1;
						}
					}
					if (columnLoaded)
					{
						chunkColumnCoordsTmp.Add(((long)z << 32) + x);
					}
				}
			}
			if (chunkColumnCoordsTmp.Count > 0)
			{
				area.ChunkColumnCoords = chunkColumnCoordsTmp.ToArray();
				area.ChunkColumnCoords.Shuffle(rand);
				spawnAreas.Add(area);
			}
		}
		this.spawnAreas = spawnAreas;
	}

	internal void ReloadSpawnStates_offthread()
	{
		double daysLeft = GraceTimeUntilTotalDays - server.Calendar.TotalDays;
		Dictionary<AssetLocation, int> quantityLoaded = new Dictionary<AssetLocation, int>();
		foreach (Entity entity in server.LoadedEntities.Values)
		{
			if (!(entity.Code == null))
			{
				string es = entity.Attributes.GetString("originaltype");
				AssetLocation ecode = ((es == null) ? entity.Code : new AssetLocation(es));
				quantityLoaded.TryGetValue(ecode, out var beforeQuantity);
				quantityLoaded[ecode] = beforeQuantity + 1;
				QuantityByGroup maxqgrp = entity.Properties.Server.SpawnConditions?.Runtime?.MaxQuantityByGroup;
				if (maxqgrp != null && WildcardUtil.Match(maxqgrp.Code, ecode))
				{
					quantityLoaded.TryGetValue(maxqgrp.Code, out beforeQuantity);
					quantityLoaded[maxqgrp.Code] = beforeQuantity + 1;
				}
			}
		}
		List<SpawnState> newSpawnStates = new List<SpawnState>();
		Random rand = this.rand;
		int onlinePlayersCount = server.AllOnlinePlayers.Length;
		foreach (EntityProperties type in server.EntityTypes)
		{
			RuntimeSpawnConditions conds = type.Server.SpawnConditions?.Runtime;
			if (conds == null || conds.MaxQuantity == 0 || (daysLeft > 0.0 && conds.Group == "hostile") || rand.NextDouble() >= 1.0 * conds.Chance)
			{
				continue;
			}
			quantityLoaded.TryGetValue(type.Code, out var qNow);
			float spawnCapMul = 1f + Math.Max(0f, (float)(onlinePlayersCount - 1) * server.Config.SpawnCapPlayerScaling * conds.SpawnCapPlayerScaling);
			int spawnableAmount = (int)((float)conds.MaxQuantity * spawnCapMul - (float)qNow);
			if (conds.MaxQuantityByGroup != null)
			{
				quantityLoaded.TryGetValue(conds.MaxQuantityByGroup.Code, out var qNowGroup);
				spawnableAmount = Math.Min(spawnableAmount, (int)((float)conds.MaxQuantityByGroup.MaxQuantity * spawnCapMul) - qNowGroup);
			}
			if (spawnableAmount <= 0)
			{
				continue;
			}
			bool num = conds.Companions != null && conds.Companions.Length != 0;
			List<EntityProperties> selfAndCompanionsProps = new List<EntityProperties> { type };
			if (num)
			{
				AssetLocation[] companions = conds.Companions;
				for (int i = 0; i < companions.Length; i++)
				{
					if (entityTypesByCode.TryGetValue(companions[i], out var companionType))
					{
						selfAndCompanionsProps.Add(companionType);
					}
					else if (!conds.doneInitialLoad)
					{
						ServerMain.Logger.Warning("Entity with code {0} has defined a companion spawn {1}, but no such entity type found.", type.Code, conds.Companions[i]);
					}
				}
			}
			conds.doneInitialLoad = true;
			newSpawnStates.Add(new SpawnState
			{
				ForType = type,
				profilerName = "testspawn " + type.Code,
				SpawnableAmountGlobal = spawnableAmount,
				SpawnCapScaledPerPlayer = (int)((float)conds.MaxQuantity * spawnCapMul / (float)onlinePlayersCount),
				SelfAndCompanionProps = selfAndCompanionsProps.ToArray()
			});
		}
		spawnStates = newSpawnStates;
	}

	public bool ShouldExit()
	{
		if (!server.stopped)
		{
			return server.exitState.Exit;
		}
		return true;
	}
}
