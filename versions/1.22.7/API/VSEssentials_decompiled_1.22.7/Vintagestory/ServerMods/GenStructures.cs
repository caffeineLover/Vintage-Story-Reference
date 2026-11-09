using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.ServerMods.NoObf;

namespace Vintagestory.ServerMods;

public class GenStructures : ModStdWorldGen, IBlockPatchModifier
{
	private ICoreServerAPI api;

	private int worldheight;

	private int regionChunkSize;

	private ushort[] heightmap;

	private int climateUpLeft;

	private int climateUpRight;

	private int climateBotLeft;

	private int climateBotRight;

	private int forestUpLeft;

	private int forestUpRight;

	private int forestBotLeft;

	private int forestBotRight;

	internal WorldGenStructuresConfig scfg;

	public WorldGenVillageConfig vcfg;

	private LCGRandom strucRand;

	private IWorldGenBlockAccessor worldgenBlockAccessor;

	private WorldGenStructure[] shuffledStructures;

	private Dictionary<string, WorldGenStructure[]> StoryStructures;

	private BlockPos spawnPos;

	private List<Cuboidi> treeAndShrubSupressingStructures = new List<Cuboidi>();

	private int chunkMapSizeY;

	public event PeventSchematicAtDelegate OnPreventSchematicPlaceAt;

	public override double ExecuteOrder()
	{
		return 0.3;
	}

	public override void StartServerSide(ICoreServerAPI api)
	{
		this.api = api;
		base.StartServerSide(api);
		if (TerraGenConfig.DoDecorationPass)
		{
			api.Event.InitWorldGenerator(initWorldGen, "standard");
			api.Event.InitWorldGenerator(InitSpecialBlocks, "superflat");
			api.Event.ChunkColumnGeneration(OnChunkColumnGen, EnumWorldGenPass.TerrainFeatures, "standard");
			api.Event.GetWorldgenBlockAccessor(OnWorldGenBlockAccessor);
			api.ModLoader.GetModSystem<GenStructuresPosPass>().handler = OnChunkColumnGenPostPass;
			api.ModLoader.GetModSystem<GenVegetationAndPatches>().RegisterPatchModifier(this);
		}
	}

	private void OnWorldGenBlockAccessor(IChunkProviderThread chunkProvider)
	{
		worldgenBlockAccessor = chunkProvider.GetBlockAccessor(updateHeightmap: false);
	}

	public bool WouldSchematicOverlapAt(IBlockAccessor blockAccessor, BlockPos pos, Cuboidi schematicLocation, string locationCode)
	{
		if (this.OnPreventSchematicPlaceAt != null)
		{
			Delegate[] invocationList = this.OnPreventSchematicPlaceAt.GetInvocationList();
			for (int i = 0; i < invocationList.Length; i++)
			{
				if (((PeventSchematicAtDelegate)invocationList[i])(blockAccessor, pos, schematicLocation, locationCode))
				{
					return true;
				}
			}
		}
		return false;
	}

	public void initWorldGen()
	{
		LoadGlobalConfig(api);
		InitSpecialBlocks();
		worldheight = api.WorldManager.MapSizeY;
		regionChunkSize = api.WorldManager.RegionSize / 32;
		chunkMapSizeY = api.WorldManager.MapSizeY / 32;
		strucRand = new LCGRandom(api.WorldManager.Seed + 1090);
		LoadStructures();
		LoadVillages();
		if (!api.World.Config.GetAsString("loreContent", "true").ToBool(defaultValue: true))
		{
			return;
		}
		WorldGenStoryStructuresConfig stcfg = (api.ModLoader.GetModSystem("Vintagestory.GameContent.GenStoryStructures") as IStoryStructureSystem)?.scfg;
		if (stcfg == null)
		{
			return;
		}
		StoryStructures = new Dictionary<string, WorldGenStructure[]>();
		WorldGenStoryStructure[] structures = stcfg.Structures;
		foreach (WorldGenStoryStructure storyStructure in structures)
		{
			string path = "worldgen/story/" + storyStructure.Code + "/structures.json";
			Dictionary<AssetLocation, WorldGenStructuresConfig> many = api.Assets.GetMany<WorldGenStructuresConfig>(api.Logger, path);
			List<WorldGenStructure> structures2 = new List<WorldGenStructure>();
			foreach (var (_, storyStructuresConfig) in many)
			{
				storyStructuresConfig.Init(api);
				structures2.AddRange(storyStructuresConfig.Structures);
			}
			if (structures2.Count > 0)
			{
				StoryStructures[storyStructure.Code] = structures2.ToArray();
			}
		}
		PlayerSpawnPos df = api.WorldManager.SaveGame.DefaultSpawn;
		if (df != null)
		{
			spawnPos = new BlockPos(df.x, df.y.GetValueOrDefault(), df.z);
		}
		else
		{
			spawnPos = api.World.BlockAccessor.MapSize.AsBlockPos / 2;
		}
	}

	private void InitSpecialBlocks()
	{
		Block fillerBlock = api.World.BlockAccessor.GetBlock(new AssetLocation("meta-filler"));
		Block pathwayBlock = api.World.BlockAccessor.GetBlock(new AssetLocation("meta-pathway"));
		Block undergroundBlock = api.World.BlockAccessor.GetBlock(new AssetLocation("meta-underground"));
		Block block = api.World.BlockAccessor.GetBlock(new AssetLocation("meta-aboveground"));
		BlockSchematic.ConnectorBlockId = api.World.BlockAccessor.GetBlock(new AssetLocation("meta-connector")).Id;
		BlockSchematic.FillerBlockId = fillerBlock.Id;
		BlockSchematic.PathwayBlockId = pathwayBlock.Id;
		BlockSchematic.UndergroundBlockId = undergroundBlock.Id;
		BlockSchematic.AbovegroundBlockId = block.Id;
	}

	private void LoadStructures()
	{
		Dictionary<AssetLocation, WorldGenStructuresConfig> assets = api.Assets.GetMany<WorldGenStructuresConfig>(api.Logger, "worldgen/structures.json");
		scfg = new WorldGenStructuresConfig();
		scfg.ChanceMultiplier = assets.First((KeyValuePair<AssetLocation, WorldGenStructuresConfig> v) => v.Key.Domain == "game").Value.ChanceMultiplier;
		scfg.SchematicYOffsets = new Dictionary<string, int>();
		scfg.RocktypeRemapGroups = new Dictionary<string, Dictionary<AssetLocation, AssetLocation>>();
		List<WorldGenStructure> structures = new List<WorldGenStructure>();
		foreach (KeyValuePair<AssetLocation, WorldGenStructuresConfig> item in assets)
		{
			item.Deconstruct(out var key, out var value);
			WorldGenStructuresConfig conf = value;
			foreach (KeyValuePair<string, Dictionary<AssetLocation, AssetLocation>> remap in conf.RocktypeRemapGroups)
			{
				if (scfg.RocktypeRemapGroups.TryGetValue(remap.Key, out var remapGroup))
				{
					foreach (KeyValuePair<AssetLocation, AssetLocation> item2 in remap.Value)
					{
						item2.Deconstruct(out key, out var value2);
						AssetLocation source = key;
						AssetLocation target = value2;
						remapGroup.TryAdd(source, target);
					}
				}
				else
				{
					scfg.RocktypeRemapGroups.TryAdd(remap.Key, remap.Value);
				}
			}
			foreach (KeyValuePair<string, int> remap2 in conf.SchematicYOffsets)
			{
				scfg.SchematicYOffsets.TryAdd(remap2.Key, remap2.Value);
			}
			structures.AddRange(conf.Structures);
		}
		scfg.Structures = structures.ToArray();
		shuffledStructures = new WorldGenStructure[scfg.Structures.Length];
		scfg.Init(api);
	}

	private void LoadVillages()
	{
		Dictionary<AssetLocation, WorldGenVillageConfig> assets = api.Assets.GetMany<WorldGenVillageConfig>(api.Logger, "worldgen/villages.json");
		vcfg = new WorldGenVillageConfig();
		vcfg.ChanceMultiplier = assets.First((KeyValuePair<AssetLocation, WorldGenVillageConfig> v) => v.Key.Domain == "game").Value.ChanceMultiplier;
		List<WorldGenVillage> villages = new List<WorldGenVillage>();
		foreach (var (_, conf) in assets)
		{
			villages.AddRange(conf.VillageTypes);
		}
		vcfg.VillageTypes = villages.ToArray();
		vcfg.Init(api, scfg);
	}

	private void OnChunkColumnGenPostPass(IChunkColumnGenerateRequest request)
	{
		if (TerraGenConfig.GenerateStructures)
		{
			IStructureLocation structure = GetIntersectingStructure(request.ChunkX * 32 + 16, request.ChunkZ * 32 + 16, ModStdWorldGen.StructuresHashCode);
			IServerChunk[] chunks = request.Chunks;
			int chunkX = request.ChunkX;
			int chunkZ = request.ChunkZ;
			worldgenBlockAccessor.BeginColumn();
			IMapRegion region = chunks[0].MapChunk.MapRegion;
			DoGenStructures(region, chunkX, chunkZ, postPass: true, structure?.Code, request.ChunkGenParams);
			TryGenVillages(region, chunkX, chunkZ, postPass: true, request.ChunkGenParams);
		}
	}

	private void OnChunkColumnGen(IChunkColumnGenerateRequest request)
	{
		if (TerraGenConfig.GenerateStructures)
		{
			IStructureLocation structure = GetIntersectingStructure(request.ChunkX * 32 + 16, request.ChunkZ * 32 + 16, ModStdWorldGen.StructuresHashCode);
			IServerChunk[] chunks = request.Chunks;
			int chunkX = request.ChunkX;
			int chunkZ = request.ChunkZ;
			worldgenBlockAccessor.BeginColumn();
			IMapRegion region = chunks[0].MapChunk.MapRegion;
			IntDataMap2D climateMap = region.ClimateMap;
			IntDataMap2D forestMap = region.ForestMap;
			int rlX = chunkX % regionChunkSize;
			int rlZ = chunkZ % regionChunkSize;
			float facC = (float)climateMap.InnerSize / (float)regionChunkSize;
			climateUpLeft = climateMap.GetUnpaddedInt((int)((float)rlX * facC), (int)((float)rlZ * facC));
			climateUpRight = climateMap.GetUnpaddedInt((int)((float)rlX * facC + facC), (int)((float)rlZ * facC));
			climateBotLeft = climateMap.GetUnpaddedInt((int)((float)rlX * facC), (int)((float)rlZ * facC + facC));
			climateBotRight = climateMap.GetUnpaddedInt((int)((float)rlX * facC + facC), (int)((float)rlZ * facC + facC));
			_ = (float)forestMap.InnerSize / (float)regionChunkSize;
			forestUpLeft = forestMap.GetUnpaddedInt((int)((float)rlX * facC), (int)((float)rlZ * facC));
			forestUpRight = forestMap.GetUnpaddedInt((int)((float)rlX * facC + facC), (int)((float)rlZ * facC));
			forestBotLeft = forestMap.GetUnpaddedInt((int)((float)rlX * facC), (int)((float)rlZ * facC + facC));
			forestBotRight = forestMap.GetUnpaddedInt((int)((float)rlX * facC + facC), (int)((float)rlZ * facC + facC));
			heightmap = chunks[0].MapChunk.WorldGenTerrainHeightMap;
			DoGenStructures(region, chunkX, chunkZ, postPass: false, structure?.Code, request.ChunkGenParams);
			if (structure == null)
			{
				TryGenVillages(region, chunkX, chunkZ, postPass: false, request.ChunkGenParams);
			}
		}
	}

	private void DoGenStructures(IMapRegion region, int chunkX, int chunkZ, bool postPass, string locationCode, ITreeAttribute chunkGenParams = null)
	{
		if (locationCode != null)
		{
			if (!StoryStructures.TryGetValue(locationCode, out var storyStructures))
			{
				return;
			}
			shuffledStructures = new WorldGenStructure[storyStructures.Length];
			for (int i = 0; i < storyStructures.Length; i++)
			{
				shuffledStructures[i] = storyStructures[i];
			}
		}
		else
		{
			shuffledStructures = new WorldGenStructure[scfg.Structures.Length];
			for (int j = 0; j < shuffledStructures.Length; j++)
			{
				shuffledStructures[j] = scfg.Structures[j];
			}
		}
		BlockPos startPos = new BlockPos(0);
		ITreeAttribute chanceModTree = null;
		ITreeAttribute maxQuantityModTree = null;
		StoryStructureLocation location = null;
		if (chunkGenParams?["structureChanceModifier"] != null)
		{
			chanceModTree = chunkGenParams["structureChanceModifier"] as TreeAttribute;
		}
		if (chunkGenParams?["structureMaxCount"] != null)
		{
			maxQuantityModTree = chunkGenParams["structureMaxCount"] as TreeAttribute;
		}
		strucRand.InitPositionSeed(chunkX, chunkZ);
		shuffledStructures.Shuffle(strucRand);
		for (int k = 0; k < shuffledStructures.Length; k++)
		{
			WorldGenStructure struc = shuffledStructures[k];
			if (struc.PostPass != postPass)
			{
				continue;
			}
			float chance = struc.Chance * scfg.ChanceMultiplier;
			int toGenerate = 9999;
			if (chanceModTree != null)
			{
				chance *= chanceModTree.GetFloat(struc.Code);
			}
			if (maxQuantityModTree != null)
			{
				toGenerate = maxQuantityModTree.GetInt(struc.Code, 9999);
			}
			while (chance-- > strucRand.NextFloat() && toGenerate > 0)
			{
				int dx = strucRand.NextInt(32);
				int dz = strucRand.NextInt(32);
				int ySurface = heightmap[dz * 32 + dx];
				if (ySurface <= 0 || ySurface >= worldheight - 15)
				{
					continue;
				}
				if (struc.Placement == EnumStructurePlacement.Underground)
				{
					if (struc.Depth != null)
					{
						startPos.Set(chunkX * 32 + dx, ySurface - (int)struc.Depth.nextFloat(1f, strucRand), chunkZ * 32 + dz);
					}
					else
					{
						startPos.Set(chunkX * 32 + dx, 8 + strucRand.NextInt(Math.Max(1, ySurface - 8 - 5)), chunkZ * 32 + dz);
					}
				}
				else
				{
					startPos.Set(chunkX * 32 + dx, ySurface, chunkZ * 32 + dz);
				}
				if (startPos.Y <= 0 || !BlockSchematicStructure.SatisfiesMinSpawnDistance(struc.MinSpawnDistance, startPos, spawnPos))
				{
					continue;
				}
				if (locationCode != null)
				{
					location = GetIntersectingStructure(chunkX * 32 + 16, chunkZ * 32 + 16) as StoryStructureLocation;
					Dictionary<string, int> schematicsSpawned = location.SchematicsSpawned;
					if ((schematicsSpawned != null && schematicsSpawned.TryGetValue(struc.Group, out var spawnedSchematics) && spawnedSchematics >= struc.StoryLocationMaxAmount) || (struc.StoryMaxFromCenter != 0 && !startPos.InRangeHorizontally(location.CenterPos.X, location.CenterPos.Z, struc.StoryMaxFromCenter)))
					{
						continue;
					}
				}
				int forest = GameMath.BiLerpRgbColor((float)(startPos.X % 32) / 32f, (float)(startPos.Z % 32) / 32f, forestUpLeft, forestUpRight, forestBotLeft, forestBotRight);
				if (!struc.TryGenerate(worldgenBlockAccessor, api.World, startPos, climateUpLeft, climateUpRight, climateBotLeft, climateBotRight, forest, locationCode))
				{
					continue;
				}
				if (locationCode != null && location != null)
				{
					Dictionary<string, int> schematicsSpawned2 = location.SchematicsSpawned;
					if (schematicsSpawned2 != null && schematicsSpawned2.TryGetValue(struc.Group, out var spawnedSchematics2))
					{
						location.SchematicsSpawned[struc.Group] = spawnedSchematics2 + 1;
					}
					else
					{
						StoryStructureLocation storyStructureLocation = location;
						if (storyStructureLocation.SchematicsSpawned == null)
						{
							storyStructureLocation.SchematicsSpawned = new Dictionary<string, int>();
						}
						location.SchematicsSpawned[struc.Group] = 1;
					}
				}
				Cuboidi loc = struc.LastPlacedSchematicLocation;
				string code = ((struc.LastPlacedSchematic == null) ? "" : struc.LastPlacedSchematic.FromFile.GetNameWithDomain()) + "/" + struc.Code;
				region.AddGeneratedStructure(new GeneratedStructure
				{
					Code = code,
					Group = struc.Group,
					Location = loc.Clone(),
					SuppressTreesAndShrubs = struc.SuppressTrees,
					SuppressRivulets = struc.SuppressWaterfalls
				});
				if (struc.BuildProtected)
				{
					api.World.Claims.Add(new LandClaim
					{
						Areas = new List<Cuboidi> { loc.Clone() },
						Description = struc.BuildProtectionDesc,
						ProtectionLevel = struc.ProtectionLevel,
						LastKnownOwnerName = struc.BuildProtectionName,
						AllowUseEveryone = struc.AllowUseEveryone,
						AllowTraverseEveryone = struc.AllowTraverseEveryone
					});
				}
				toGenerate--;
			}
		}
	}

	public void TryGenVillages(IMapRegion region, int chunkX, int chunkZ, bool postPass, ITreeAttribute chunkGenParams = null)
	{
		strucRand.InitPositionSeed(chunkX, chunkZ);
		for (int i = 0; i < vcfg.VillageTypes.Length; i++)
		{
			WorldGenVillage struc = vcfg.VillageTypes[i];
			if (struc.PostPass == postPass)
			{
				float chance = struc.Chance * vcfg.ChanceMultiplier;
				while (chance-- > strucRand.NextFloat())
				{
					GenVillage(worldgenBlockAccessor, region, struc, chunkX, chunkZ);
				}
			}
		}
	}

	public bool GenVillage(IBlockAccessor blockAccessor, IMapRegion region, WorldGenVillage struc, int chunkX, int chunkZ)
	{
		BlockPos pos = new BlockPos(0);
		int dx = 16;
		int dz = 16;
		int ySurface = heightmap[dz * 32 + dx];
		if (ySurface <= 0 || ySurface >= worldheight - 15)
		{
			return false;
		}
		pos.Set(chunkX * 32 + dx, ySurface, chunkZ * 32 + dz);
		return struc.TryGenerate(blockAccessor, api.World, pos, climateUpLeft, climateUpRight, climateBotLeft, climateBotRight, delegate(Cuboidi loc, BlockSchematicStructure schematic)
		{
			string code = ((schematic == null) ? "" : schematic.FromFile.GetNameWithDomain()) + "/" + struc.Code;
			region.AddGeneratedStructure(new GeneratedStructure
			{
				Code = code,
				Group = struc.Group,
				Location = loc.Clone()
			});
			if (struc.BuildProtected)
			{
				api.World.Claims.Add(new LandClaim
				{
					Areas = new List<Cuboidi> { loc.Clone() },
					Description = struc.BuildProtectionDesc,
					ProtectionLevel = struc.ProtectionLevel,
					LastKnownOwnerName = struc.BuildProtectionName,
					AllowUseEveryone = struc.AllowUseEveryone,
					AllowTraverseEveryone = struc.AllowTraverseEveryone
				});
			}
		}, spawnPos);
	}

	public bool PreventPlacementAt(int x, int z, int category)
	{
		if (category != ModStdWorldGen.ShrubsHashCode && category != ModStdWorldGen.TreesHashCode)
		{
			return false;
		}
		for (int i = 0; i < treeAndShrubSupressingStructures.Count; i++)
		{
			if (treeAndShrubSupressingStructures[i].Contains(x, z))
			{
				return true;
			}
		}
		return false;
	}

	public bool PreventPlacementBroadlyAt(int chunkX, int chunkZ)
	{
		BlockPos chunkStart = new BlockPos(0);
		BlockPos chunkEnd = new BlockPos(0);
		treeAndShrubSupressingStructures.Clear();
		api.World.BlockAccessor.WalkStructures(chunkStart.Set(chunkX * 32, 0, chunkZ * 32), chunkEnd.Set(chunkX * 32 + 32, chunkMapSizeY * 32, chunkZ * 32 + 32), delegate(GeneratedStructure struc)
		{
			if (struc.SuppressTreesAndShrubs)
			{
				treeAndShrubSupressingStructures.Add(struc.Location.Clone().GrowBy(1, 1, 1));
			}
		});
		return false;
	}

	public BlockPatchConfig GetPatchProviderAt(int chunkX, int chunkZ, ref EnumHandling handling)
	{
		return null;
	}
}
