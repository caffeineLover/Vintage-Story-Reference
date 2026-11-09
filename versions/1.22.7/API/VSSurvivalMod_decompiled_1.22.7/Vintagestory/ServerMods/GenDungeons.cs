using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.Common.Collectible.Block;
using Vintagestory.GameContent;
using Vintagestory.ServerMods.NoObf;

namespace Vintagestory.ServerMods;

public class GenDungeons : ModStdWorldGen
{
	private ModSystemTiledDungeons tiledDungeonsSys;

	private IWorldGenBlockAccessor worldgenBlockAccessor;

	private ICoreServerAPI sapi;

	private LCGRandom rand;

	private Dictionary<long, List<DungeonPlaceTask>> dungeonPlaceTasksByRegion = new Dictionary<long, List<DungeonPlaceTask>>();

	private int regionSize;

	private BlockPos spawnPos;

	private GenStoryStructures storyStructSys;

	private GenBlockLayers genBlockLayers;

	private LCGRandom grassRand;

	public List<DungeonLocation> Dungeons = new List<DungeonLocation>();

	public bool DungeonsDirty;

	public override double ExecuteOrder()
	{
		return 0.12;
	}

	public override void StartServerSide(ICoreServerAPI api)
	{
		if (api.World.Config.GetAsString("loreContent", "true").ToBool(defaultValue: true))
		{
			sapi = api;
			base.StartServerSide(api);
			tiledDungeonsSys = api.ModLoader.GetModSystem<ModSystemTiledDungeons>();
			if (TerraGenConfig.DoDecorationPass)
			{
				api.Event.MapRegionGeneration(onMapRegionGen, "standard");
				api.Event.ChunkColumnGeneration(onChunkColumnGen, EnumWorldGenPass.TerrainFeatures, "standard");
				api.Event.GetWorldgenBlockAccessor(OnWorldGenBlockAccessor);
				api.Event.SaveGameLoaded += Event_SaveGameLoaded;
				api.Event.GameWorldSave += Event_GameWorldSave;
				api.Event.MapRegionUnloaded += Event_MapRegionUnloaded;
				api.Event.MapRegionLoaded += Event_MapRegionLoaded;
				api.Event.InitWorldGenerator(OnInitWorldGenerator, "standard");
				genBlockLayers = api.ModLoader.GetModSystem<GenBlockLayers>();
			}
		}
	}

	private void OnInitWorldGenerator()
	{
		tiledDungeonsSys.Init();
		PlayerSpawnPos df = sapi.WorldManager.SaveGame.DefaultSpawn;
		if (df != null)
		{
			spawnPos = new BlockPos(df.x, df.y.GetValueOrDefault(), df.z);
		}
		else
		{
			spawnPos = sapi.World.BlockAccessor.MapSize.AsBlockPos / 2;
		}
		storyStructSys = sapi.ModLoader.GetModSystem<GenStoryStructures>();
		grassRand = new LCGRandom(sapi.WorldManager.Seed);
	}

	private void OnWorldGenBlockAccessor(IChunkProviderThread chunkProvider)
	{
		worldgenBlockAccessor = chunkProvider.GetBlockAccessor(updateHeightmap: false);
		rand = new LCGRandom(sapi.WorldManager.Seed ^ 0x217F464FEL);
		regionSize = sapi.World.BlockAccessor.RegionSize;
	}

	private void Event_MapRegionLoaded(Vec2i mapCoord, IMapRegion region)
	{
		try
		{
			List<DungeonPlaceTask> tasks = region.GetModdata<List<DungeonPlaceTask>>("dungeonPlaceTasks");
			if (tasks != null)
			{
				dungeonPlaceTasksByRegion[MapRegionIndex2D(mapCoord.X, mapCoord.Y)] = tasks;
			}
		}
		catch
		{
		}
	}

	private void Event_GameWorldSave()
	{
		if (DungeonsDirty)
		{
			sapi.WorldManager.SaveGame.StoreData("dungeonLocations", SerializerUtil.Serialize(Dungeons));
			DungeonsDirty = false;
		}
	}

	private void Event_SaveGameLoaded()
	{
		List<DungeonLocation> strucs = sapi.WorldManager.SaveGame.GetData<List<DungeonLocation>>("dungeonLocations");
		if (strucs != null)
		{
			Dungeons.AddRange(strucs);
		}
	}

	private void Event_MapRegionUnloaded(Vec2i mapCoord, IMapRegion region)
	{
		if (dungeonPlaceTasksByRegion.TryGetValue(MapRegionIndex2D(mapCoord.X, mapCoord.Y), out List<DungeonPlaceTask> list) && list.Any((DungeonPlaceTask d) => d.IsDirty))
		{
			region.SetModdata("dungeonPlaceTasks", list);
		}
	}

	private void onMapRegionGen(IMapRegion mapRegion, int regionX, int regionZ, ITreeAttribute? chunkGenParams = null)
	{
		int size = sapi.WorldManager.RegionSize;
		LCGRandom mapRand = new LCGRandom(sapi.WorldManager.Seed ^ 0x217F464FEL);
		mapRand.InitPositionSeed(regionX * size, regionZ * size);
		long mapRegionIndex = MapRegionIndex2D(regionX, regionZ);
		if (dungeonPlaceTasksByRegion.ContainsKey(mapRegionIndex))
		{
			Dungeons.RemoveAll((DungeonLocation d) => d.Position.X / regionSize == regionX && d.Position.Z / regionSize == regionZ);
		}
		dungeonPlaceTasksByRegion[mapRegionIndex] = new List<DungeonPlaceTask>();
		List<TiledDungeon> dungeons = tiledDungeonsSys.Tcfg.Dungeons.Where((TiledDungeon d) => d.Worldgen).ToList();
		int[] dungeIndices = new int[dungeons.Count];
		for (int l = 0; l < dungeIndices.Length; l++)
		{
			dungeIndices[l] = l;
		}
		int seaLevel = sapi.World.SeaLevel;
		for (int i = 0; i < 12; i++)
		{
			int posx = regionX * size + mapRand.NextInt(size);
			int posz = regionZ * size + mapRand.NextInt(size);
			if (!SatisfiesMinDistance(posx, seaLevel, posz) || storyStructSys.Structures.values.Values.Any((StoryStructureLocation storyLoc) => storyLoc.CenterPos.HorDistanceSqTo(posx, posz) < (float)tiledDungeonsSys.Tcfg.StoryStructMinDistanceSq))
			{
				continue;
			}
			TiledDungeon dungeon = pickDungeon(dungeIndices, mapRand, dungeons);
			if (spawnPos.HorDistanceSqTo(posx, posz) < (float)(dungeon.MinSpawnDistance * dungeon.MinSpawnDistance) || IsInOcean(mapRegion, posx, posz) || !HasSuitableLandforms(dungeon, mapRegion, posx, posz))
			{
				continue;
			}
			int posy = seaLevel - dungeon.MaxDepth + mapRand.NextInt(dungeon.MaxDepth - dungeon.MinDepth);
			BlockPos dungeonStartPos = new BlockPos(posx, posy, posz);
			bool didGen = false;
			for (int j = 0; j < 5; j++)
			{
				DungeonPlaceTask placeTask = tiledDungeonsSys.TryPregenerateTiledDungeon(mapRand, dungeon, mapRegion.GeneratedStructures, dungeonStartPos, dungeon.MinTiles, dungeon.MaxTiles);
				if (placeTask != null)
				{
					sapi.Logger.Debug($"Dungeon {dungeon.Code} @: /tp ={posx} {posy} ={posz}");
					placeTask.IsDirty = true;
					dungeonPlaceTasksByRegion[mapRegionIndex].Add(placeTask);
					Dungeons.Add(new DungeonLocation(dungeonStartPos, placeTask.Code, placeTask.DungeonBoundaries));
					DungeonsDirty = true;
					mapRegion.AddGeneratedStructures(placeTask.GeneratedStructures);
					didGen = true;
					break;
				}
			}
			if (didGen)
			{
				break;
			}
		}
	}

	private static TiledDungeon pickDungeon(int[] dungeIndices, LCGRandom mapRand, List<TiledDungeon> dungeons)
	{
		TiledDungeon dungeon = null;
		dungeIndices.Shuffle(mapRand);
		float rndval = (float)mapRand.NextDouble() * dungeons.Sum((TiledDungeon d) => d.chance);
		for (int k = 0; k < dungeons.Count; k++)
		{
			dungeon = dungeons[dungeIndices[k]];
			rndval -= dungeon.chance;
			if (!(rndval > 0f))
			{
				return dungeon;
			}
		}
		return dungeon;
	}

	private bool SatisfiesMinDistance(int posx, int posy, int posz)
	{
		bool satisfiesDistance = true;
		foreach (DungeonLocation dungeon in Dungeons)
		{
			if (dungeon.Position.DistanceSqTo(posx, posy, posz) < (float)tiledDungeonsSys.Tcfg.MinDistanceSq)
			{
				satisfiesDistance = false;
				break;
			}
		}
		return satisfiesDistance;
	}

	private bool IsInOcean(IMapRegion mapRegion, int posx, int posz)
	{
		if (mapRegion.OceanMap != null && mapRegion.OceanMap.Data.Length != 0)
		{
			int regionChunkSize = regionSize / 32;
			int rlX = posx / 32 % regionChunkSize;
			int rlZ = posz / 32 % regionChunkSize;
			float oFac = (float)mapRegion.OceanMap.InnerSize / (float)regionChunkSize;
			int unpaddedInt = mapRegion.OceanMap.GetUnpaddedInt((int)((float)rlX * oFac), (int)((float)rlZ * oFac));
			int oceanUpRight = mapRegion.OceanMap.GetUnpaddedInt((int)((float)rlX * oFac + oFac), (int)((float)rlZ * oFac));
			int oceanBotLeft = mapRegion.OceanMap.GetUnpaddedInt((int)((float)rlX * oFac), (int)((float)rlZ * oFac + oFac));
			int oceanBotRight = mapRegion.OceanMap.GetUnpaddedInt((int)((float)rlX * oFac + oFac), (int)((float)rlZ * oFac + oFac));
			if (GameMath.BiLerp(unpaddedInt, oceanUpRight, oceanBotLeft, oceanBotRight, (float)(posx % 32) / 32f, (float)(posz % 32) / 32f) > 0f)
			{
				return true;
			}
		}
		return false;
	}

	private bool HasSuitableLandforms(TiledDungeon dungeon, IMapRegion mapRegion, int posx, int posz)
	{
		if (dungeon.RequiredLandform == null)
		{
			return true;
		}
		int noiseSizeLandform = mapRegion.LandformMap.InnerSize;
		LandformVariant[] landforms = NoiseLandforms.landforms.LandFormsByIndex;
		IntDataMap2D landformMap = mapRegion.LandformMap;
		LerpedWeightedIndex2DMap map = new LerpedWeightedIndex2DMap(landformMap.Data, mapRegion.LandformMap.Size, TerraGenConfig.landFormSmoothingRadius, landformMap.TopLeftPadding, landformMap.BottomRightPadding);
		bool valid = true;
		int sizeX = dungeon.Surface[0].SizeX;
		int sizeZ = dungeon.Surface[0].SizeZ;
		for (int x = -1; x < 2; x++)
		{
			for (int z = 0; z < 2; z++)
			{
				int tmpX = posx + x * (sizeX / 2);
				int tmpZ = posz + z * (sizeZ / 2);
				float posXInRegion = ((float)tmpX / (float)regionSize - (float)(tmpX / regionSize)) * (float)noiseSizeLandform;
				float posZInRegion = ((float)tmpZ / (float)regionSize - (float)(tmpZ / regionSize)) * (float)noiseSizeLandform;
				valid &= HasSuitableLandforms(dungeon, map, posXInRegion, posZInRegion, landforms);
			}
		}
		return valid;
	}

	private static bool HasSuitableLandforms(TiledDungeon dungeon, LerpedWeightedIndex2DMap map, float posXInRegion, float posZInRegion, LandformVariant[] landforms)
	{
		Dictionary<string, WeightedIndex> weightedIndices = new Dictionary<string, WeightedIndex>();
		WeightedIndex[] array = map[posXInRegion, posZInRegion];
		for (int i = 0; i < array.Length; i++)
		{
			WeightedIndex index = array[i];
			weightedIndices.Add(landforms[index.Index].Code.ToString(), index);
		}
		LandformConstraint[] requiredLandform = dungeon.RequiredLandform;
		foreach (LandformConstraint landform in requiredLandform)
		{
			WeightedIndex index3;
			if (landform.Type == "greater")
			{
				if (!weightedIndices.TryGetValue(landform.Code, out var index2) || !(index2.Weight > landform.Value))
				{
					return false;
				}
			}
			else if (landform.Type == "less" && weightedIndices.TryGetValue(landform.Code, out index3) && index3.Weight > landform.Value)
			{
				return false;
			}
		}
		return true;
	}

	public long MapRegionIndex2D(int regionX, int regionZ)
	{
		return (long)regionZ * (long)(sapi.WorldManager.MapSizeX / sapi.WorldManager.RegionSize) + regionX;
	}

	private void onChunkColumnGen(IChunkColumnGenerateRequest request)
	{
		int regionx = request.ChunkX * 32 / regionSize;
		int regionz = request.ChunkZ * 32 / regionSize;
		int posX = request.ChunkX * 32;
		int posZ = request.ChunkZ * 32;
		Cuboidi cuboid = new Cuboidi(posX, 0, posZ, posX + 32, sapi.World.BlockAccessor.MapSizeY, posZ + 32);
		IMapRegion region = request.Chunks[0].MapChunk.MapRegion;
		for (int dx = -1; dx <= 1; dx++)
		{
			for (int dz = -1; dz <= 1; dz++)
			{
				long mapRegionIndex2D = MapRegionIndex2D(regionx + dx, regionz + dz);
				if (!dungeonPlaceTasksByRegion.TryGetValue(mapRegionIndex2D, out List<DungeonPlaceTask> dungePlaceTasks))
				{
					continue;
				}
				foreach (DungeonPlaceTask placeTask in dungePlaceTasks)
				{
					if (!placeTask.DungeonBoundaries.Intersects(cuboid))
					{
						continue;
					}
					if (!tiledDungeonsSys.Tcfg.DungeonsByCode.TryGetValue(placeTask.Code, out TiledDungeon dungeon))
					{
						return;
					}
					Block rockBlock = null;
					if (placeTask.RockBlockCode == null)
					{
						int posY = placeTask.TilePlaceTasks[0].Pos.Y;
						int j = 0;
						while (rockBlock == null && j < 10)
						{
							Block block = worldgenBlockAccessor.GetBlockRaw(request.ChunkX * 32 + 15, posY, request.ChunkZ * 32 + 15, 1);
							if (block.BlockMaterial == EnumBlockMaterial.Stone)
							{
								rockBlock = block;
								placeTask.RockBlockCode = block.Code.ToString();
								placeTask.IsDirty = true;
								break;
							}
							posY -= 2;
							j++;
						}
					}
					else
					{
						rockBlock = worldgenBlockAccessor.GetBlock(placeTask.RockBlockCode);
					}
					generateDungeonPartial(region, dungeon, placeTask, request.Chunks, request.ChunkX, request.ChunkZ, rockBlock);
					if (!placeTask.StairsConnector.HasValue)
					{
						continue;
					}
					ConnectorMetaData connector = placeTask.StairsConnector.Value;
					ref BlockFacing facing = ref connector.Facing;
					if (facing == null)
					{
						facing = BlockFacing.ALLFACES[connector.FacingInt];
					}
					if (placeTask.SurfacePlaceTasks == null && dungeon.StairCase != null)
					{
						BlockPos pos = new BlockPos(connector.Position.X, connector.Position.Y, connector.Position.Z);
						int height = sapi.World.BlockAccessor.GetTerrainMapheightAt(pos);
						if (height == 0)
						{
							continue;
						}
						height += dungeon.surfaceYOffset;
						rand.InitPositionSeed(connector.Position.X, connector.Position.Y, connector.Position.Z);
						int[] tileIndices = new int[dungeon.StairCase.Tiles.Count];
						for (int i = 0; i < tileIndices.Length; i++)
						{
							tileIndices[i] = i;
						}
						DungeonGenWorkspace dgd = new DungeonGenWorkspace(dungeon.StairCase, 0, 0, new List<ConnectorMetaData>(), new List<TilePlaceTask>(), new List<GeneratedStructure>(), new List<GeneratedStructure>(), null);
						int currHeight = connector.Position.Y;
						List<TilePlaceTask> surfacePlaceTasks = new List<TilePlaceTask>();
						int tries = 40;
						ConnectorMetaData curConnector = connector;
						BlockSchematicPartial schematic = null;
						DungeonTile tile = null;
						while (tries-- > 0 && currHeight < height && curConnector.Facing != null)
						{
							FastVec3i? offsetPos = null;
							if (!pickStairTile(dgd, curConnector, tileIndices, ref schematic, ref tile, ref offsetPos))
							{
								continue;
							}
							if (currHeight == height)
							{
								break;
							}
							if (currHeight + schematic.SizeY < height)
							{
								currHeight += schematic.SizeY;
								FastVec3i fastPos = curConnector.Position.AddCopy(curConnector.Facing).Sub(offsetPos.Value);
								BlockPos startPos = new BlockPos(fastPos.X, fastPos.Y, fastPos.Z);
								surfacePlaceTasks.Add(new TilePlaceTask
								{
									Rotation = connector.Rotation,
									Pos = startPos,
									FileName = schematic.FromFile,
									TileCode = tile.Code,
									SizeX = schematic.SizeX,
									SizeY = schematic.SizeY,
									SizeZ = schematic.SizeZ
								});
								curConnector = schematic.Connectors.First((ConnectorMetaData p) => !p.ConnectsTo(curConnector)).Clone().Offset(fastPos);
								Cuboidi location = new Cuboidi(startPos, startPos.AddCopy(schematic.SizeX, schematic.SizeY, schematic.SizeZ));
								if (cuboid.Intersects(location))
								{
									region.AddGeneratedStructure(new GeneratedStructure
									{
										Code = schematic.FromFile.GetNameWithDomain(),
										Group = "stairs",
										Location = location,
										SuppressTreesAndShrubs = true,
										SuppressRivulets = true
									});
								}
							}
						}
						if (curConnector.Facing != null && dungeon.Surface != null)
						{
							schematic = dungeon.Surface[connector.Rotation];
							FastVec3i offset = schematic.Connectors.First((ConnectorMetaData p) => p.ConnectsTo(curConnector)).Position;
							FastVec3i fastPos2 = curConnector.Position.AddCopy(curConnector.Facing).Sub(offset);
							BlockPos startPos2 = new BlockPos(fastPos2.X, fastPos2.Y, fastPos2.Z);
							surfacePlaceTasks.Add(new TilePlaceTask
							{
								Rotation = connector.Rotation,
								Pos = startPos2,
								FileName = schematic.FromFile,
								TileCode = "surface",
								SizeX = schematic.SizeX,
								SizeY = schematic.SizeY,
								SizeZ = schematic.SizeZ
							});
							Cuboidi location2 = new Cuboidi(startPos2, startPos2.AddCopy(schematic.SizeX, schematic.SizeY, schematic.SizeZ));
							if (cuboid.Intersects(location2))
							{
								region.AddGeneratedStructure(new GeneratedStructure
								{
									Code = schematic.FromFile.GetNameWithDomain(),
									Group = "stairs",
									Location = location2,
									SuppressTreesAndShrubs = true,
									SuppressRivulets = true
								});
							}
						}
						placeTask.SurfacePlaceTasks = surfacePlaceTasks;
						placeTask.IsDirty = true;
					}
					if (placeTask.SurfacePlaceTasks != null)
					{
						generateDungeonSurfacePartial(request, region, dungeon, placeTask.SurfacePlaceTasks, request.Chunks, request.ChunkX, request.ChunkZ, rockBlock);
					}
				}
			}
		}
	}

	private bool pickStairTile(DungeonGenWorkspace dgd, ConnectorMetaData connector, int[] tileIndices, [NotNullWhen(true)] ref BlockSchematicPartial? schematic, [NotNullWhen(true)] ref DungeonTile? tile, [NotNullWhen(true)] ref FastVec3i? offsetPos)
	{
		tile = tiledDungeonsSys.dungeonGen.pickTile(dgd, tileIndices, rand, connector);
		if (tile != null)
		{
			tile.FreshShuffleIndices(rand);
			int len = tile.ResolvedSchematics.Length;
			for (int k = 0; k < len; k++)
			{
				BlockSchematicPartial[] schematicByRot = tile.ResolvedSchematics[tile.ShuffledIndices[k]];
				int startRot = connector.Rotation;
				if (connector.Facing.IsHorizontal)
				{
					startRot = rand.NextInt(4);
				}
				for (int i = 0; i < 4; i++)
				{
					int rot = (startRot + i) % 4;
					ConnectorMetaData path = schematicByRot[rot].Connectors.FirstOrDefault((ConnectorMetaData p) => p.ConnectsTo(connector));
					if (path.Valid)
					{
						offsetPos = path.Position;
						schematic = schematicByRot[rot];
						return true;
					}
				}
			}
		}
		return false;
	}

	private void generateDungeonSurfacePartial(IChunkColumnGenerateRequest request, IMapRegion region, TiledDungeon dungeon, List<TilePlaceTask> placeTasks, IServerChunk[] chunks, int chunkX, int chunkZ, Block? rockBlock)
	{
		foreach (TilePlaceTask placeTask in placeTasks)
		{
			BlockSchematicPartial schematic = null;
			if (placeTask.TileCode == null || !dungeon.StairCase.TilesByCode.TryGetValue(placeTask.TileCode, out DungeonTile tile))
			{
				if (dungeon.Surface[placeTask.Rotation].FromFile == (AssetLocation)placeTask.FileName)
				{
					schematic = dungeon.Surface[placeTask.Rotation];
				}
			}
			else
			{
				int roomIndex = -1;
				for (int index = 0; index < tile.ResolvedSchematics.Length; index++)
				{
					if (!(tile.ResolvedSchematics[index][0].FromFile != (AssetLocation)placeTask.FileName))
					{
						roomIndex = index;
						break;
					}
				}
				if (roomIndex == -1)
				{
					sapi.Logger.Warning("Failed to find room: " + placeTask.FileName + ", maybe outdated? Will not generate");
					continue;
				}
				schematic = tile.ResolvedSchematics[roomIndex][placeTask.Rotation];
			}
			if (schematic == null)
			{
				continue;
			}
			if (schematic.PlacePartial(chunks, worldgenBlockAccessor, sapi.World, chunkX, chunkZ, placeTask.Pos, EnumReplaceMode.ReplaceAll, null, GlobalConfig.ReplaceMetaBlocks, resolveImports: true, dungeon.resolvedRockTypeRemaps, dungeon.replacewithblocklayersBlockids, rockBlock) > 0)
			{
				ModStdWorldGen.UpdateHeightmap(request, worldgenBlockAccessor);
				GenerateGrass(sapi, request, grassRand, genBlockLayers);
			}
			if (request.ChunkX == placeTask.Pos.X / 32 && request.ChunkZ == placeTask.Pos.Z / 32)
			{
				Cuboidi location = new Cuboidi(placeTask.Pos, placeTask.Pos.AddCopy(schematic.SizeX, schematic.SizeY, schematic.SizeZ));
				region.AddGeneratedStructure(new GeneratedStructure
				{
					Code = schematic.FromFile.GetNameWithDomain(),
					Group = dungeon.Code,
					Location = location,
					SuppressTreesAndShrubs = true,
					SuppressRivulets = true
				});
				if (dungeon.BuildProtected)
				{
					sapi.World.Claims.Add(new LandClaim
					{
						Areas = new List<Cuboidi> { location.Clone() },
						Description = dungeon.BuildProtectionDesc,
						ProtectionLevel = 10,
						LastKnownOwnerName = dungeon.BuildProtectionName,
						AllowUseEveryone = true
					});
				}
			}
		}
	}

	private void generateDungeonPartial(IMapRegion region, TiledDungeon dungeon, DungeonPlaceTask dungeonPlaceTask, IServerChunk[] chunks, int chunkX, int chunkZ, Block? rockBlock)
	{
		foreach (TilePlaceTask placeTask in dungeonPlaceTask.TilePlaceTasks)
		{
			if (!dungeon.TilesByCode.TryGetValue(placeTask.TileCode, out DungeonTile tile))
			{
				continue;
			}
			int roomIndex = -1;
			for (int index = 0; index < tile.ResolvedSchematics.Length; index++)
			{
				if (!(tile.ResolvedSchematics[index][0].FromFile != (AssetLocation)placeTask.FileName))
				{
					roomIndex = index;
					break;
				}
			}
			if (roomIndex == -1)
			{
				sapi.Logger.Warning("Failed to find room: " + placeTask.FileName + ", maybe outdated? Will not generate");
				continue;
			}
			BlockSchematicPartial schematic = tile.ResolvedSchematics[roomIndex][placeTask.Rotation];
			schematic.PlacePartial(chunks, worldgenBlockAccessor, sapi.World, chunkX, chunkZ, placeTask.Pos, EnumReplaceMode.ReplaceAll, null, GlobalConfig.ReplaceMetaBlocks, resolveImports: true, dungeon.resolvedRockTypeRemaps, dungeon.replacewithblocklayersBlockids, rockBlock);
			if (chunkX == placeTask.Pos.X / 32 && chunkZ == placeTask.Pos.Z / 32)
			{
				Cuboidi location = new Cuboidi(placeTask.Pos, placeTask.Pos.AddCopy(schematic.SizeX, schematic.SizeY, schematic.SizeZ));
				region.AddGeneratedStructure(new GeneratedStructure
				{
					Code = schematic.FromFile.GetNameWithDomain(),
					Group = dungeonPlaceTask.Code,
					Location = location,
					SuppressTreesAndShrubs = true,
					SuppressRivulets = true
				});
				if (dungeon.BuildProtected)
				{
					sapi.World.Claims.Add(new LandClaim
					{
						Areas = new List<Cuboidi> { location.Clone() },
						Description = dungeon.BuildProtectionDesc,
						ProtectionLevel = 10,
						LastKnownOwnerName = dungeon.BuildProtectionName,
						AllowUseEveryone = true
					});
				}
			}
		}
	}
}
