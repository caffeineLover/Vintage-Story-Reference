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

public class GenVegetationAndPatches : ModStdWorldGen
{
	protected ICoreServerAPI sapi;

	protected LCGRandom rnd;

	protected IWorldGenBlockAccessor blockAccessor;

	protected WgenTreeSupplier treeSupplier;

	protected int worldheight;

	protected int regionChunkSize;

	protected IBlockPatchModifier[] patchMod = new IBlockPatchModifier[0];

	protected float forestMod;

	protected float shrubMod;

	protected int noiseSizeDensityMap;

	protected int regionSize;

	public Dictionary<string, int> RockBlockIdsByType;

	public BlockPatchConfig bpc;

	public Dictionary<string, MapLayerBase> blockPatchMapGens = new Dictionary<string, MapLayerBase>();

	protected const int subSeed = 87698;

	private ushort[] heightmap;

	private int forestUpLeft;

	private int forestUpRight;

	private int forestBotLeft;

	private int forestBotRight;

	private int shrubUpLeft;

	private int shrubUpRight;

	private int shrubBotLeft;

	private int shrubBotRight;

	private int climateUpLeft;

	private int climateUpRight;

	private int climateBotLeft;

	private int climateBotRight;

	private BlockPos tmpPos = new BlockPos(0);

	private bool preventBroadly;

	public override bool ShouldLoad(EnumAppSide side)
	{
		return side == EnumAppSide.Server;
	}

	public override double ExecuteOrder()
	{
		return 0.5;
	}

	public void RegisterPatchModifier(IBlockPatchModifier modifier)
	{
		patchMod = patchMod.Append(modifier);
	}

	public override void StartServerSide(ICoreServerAPI api)
	{
		sapi = api;
		if (TerraGenConfig.DoDecorationPass)
		{
			api.Event.InitWorldGenerator(initWorldGen, "standard");
			api.Event.InitWorldGenerator(initWorldGenForSuperflat, "superflat");
			api.Event.ChunkColumnGeneration(OnChunkColumnGen, EnumWorldGenPass.Vegetation, "standard");
			api.Event.MapRegionGeneration(OnMapRegionGen, "standard");
			api.Event.MapRegionGeneration(OnMapRegionGen, "superflat");
			api.Event.GetWorldgenBlockAccessor(OnWorldGenBlockAccessor);
		}
	}

	private void OnMapRegionGen(IMapRegion mapRegion, int regionX, int regionZ, ITreeAttribute chunkGenParams = null)
	{
		int noiseSize = sapi.WorldManager.RegionSize / TerraGenConfig.blockPatchesMapScale;
		foreach (KeyValuePair<string, MapLayerBase> val in blockPatchMapGens)
		{
			IntDataMap2D map = IntDataMap2D.CreateEmpty();
			map.Size = noiseSize + 1;
			map.BottomRightPadding = 1;
			map.Data = val.Value.GenLayer(regionX * noiseSize, regionZ * noiseSize, noiseSize + 1, noiseSize + 1);
			mapRegion.BlockPatchMaps[val.Key] = map;
		}
	}

	private void OnWorldGenBlockAccessor(IChunkProviderThread chunkProvider)
	{
		treeSupplier = new WgenTreeSupplier(sapi);
		blockAccessor = chunkProvider.GetBlockAccessor(updateHeightmap: true);
	}

	private void initWorldGenForSuperflat()
	{
		treeSupplier.LoadTrees();
	}

	public void initWorldGen()
	{
		regionSize = sapi.WorldManager.RegionSize;
		noiseSizeDensityMap = regionSize / TerraGenConfig.blockPatchesMapScale;
		LoadGlobalConfig(sapi);
		rnd = new LCGRandom(sapi.WorldManager.Seed - 87698);
		treeSupplier.LoadTrees();
		worldheight = sapi.WorldManager.MapSizeY;
		regionChunkSize = sapi.WorldManager.RegionSize / 32;
		RockBlockIdsByType = new Dictionary<string, int>();
		RockStrataConfig rockstrata = sapi.Assets.Get("worldgen/rockstrata.json").ToObject<RockStrataConfig>();
		for (int i = 0; i < rockstrata.Variants.Length; i++)
		{
			Block block = sapi.World.GetBlock(rockstrata.Variants[i].BlockCode);
			RockBlockIdsByType[block.LastCodePart()] = block.BlockId;
		}
		IAsset asset = sapi.Assets.Get("worldgen/blockpatches.json");
		bpc = asset.ToObject<BlockPatchConfig>();
		IOrderedEnumerable<KeyValuePair<AssetLocation, BlockPatch[]>> orderedEnumerable = from b in sapi.Assets.GetMany<BlockPatch[]>(sapi.World.Logger, "worldgen/blockpatches/")
			orderby b.Key.ToString()
			select b;
		List<BlockPatch> allPatches = new List<BlockPatch>();
		foreach (KeyValuePair<AssetLocation, BlockPatch[]> item in orderedEnumerable)
		{
			allPatches.AddRange(item.Value);
		}
		bpc.Patches = allPatches.ToArray();
		bpc.ResolveBlockIds(sapi, rockstrata, rnd);
		treeSupplier.treeGenerators.forestFloorSystem.SetBlockPatches(bpc);
		ITreeAttribute worldConfig = sapi.WorldManager.SaveGame.WorldConfiguration;
		forestMod = worldConfig.GetString("globalForestation").ToFloat();
		blockPatchMapGens.Clear();
		BlockPatch[] patches = bpc.Patches;
		foreach (BlockPatch patch in patches)
		{
			if (patch.MapCode != null && !blockPatchMapGens.ContainsKey(patch.MapCode))
			{
				int hs = patch.MapCode.GetHashCode();
				int seed = sapi.World.Seed + 112897 + hs;
				blockPatchMapGens[patch.MapCode] = new MapLayerWobbled(seed, 2, 0.9f, TerraGenConfig.forestMapScale, 4000f, -2500);
			}
		}
	}

	private void OnChunkColumnGen(IChunkColumnGenerateRequest request)
	{
		IServerChunk[] chunks = request.Chunks;
		int chunkX = request.ChunkX;
		int chunkZ = request.ChunkZ;
		blockAccessor.BeginColumn();
		rnd.InitPositionSeed(chunkX, chunkZ);
		IMapChunk mapChunk = chunks[0].MapChunk;
		IntDataMap2D forestMap = mapChunk.MapRegion.ForestMap;
		IntDataMap2D shrubMap = mapChunk.MapRegion.ShrubMap;
		IntDataMap2D climateMap = mapChunk.MapRegion.ClimateMap;
		int rlX = chunkX % regionChunkSize;
		int rlZ = chunkZ % regionChunkSize;
		float facS = (float)shrubMap.InnerSize / (float)regionChunkSize;
		shrubUpLeft = shrubMap.GetUnpaddedInt((int)((float)rlX * facS), (int)((float)rlZ * facS));
		shrubUpRight = shrubMap.GetUnpaddedInt((int)((float)rlX * facS + facS), (int)((float)rlZ * facS));
		shrubBotLeft = shrubMap.GetUnpaddedInt((int)((float)rlX * facS), (int)((float)rlZ * facS + facS));
		shrubBotRight = shrubMap.GetUnpaddedInt((int)((float)rlX * facS + facS), (int)((float)rlZ * facS + facS));
		float facF = (float)forestMap.InnerSize / (float)regionChunkSize;
		forestUpLeft = forestMap.GetUnpaddedInt((int)((float)rlX * facF), (int)((float)rlZ * facF));
		forestUpRight = forestMap.GetUnpaddedInt((int)((float)rlX * facF + facF), (int)((float)rlZ * facF));
		forestBotLeft = forestMap.GetUnpaddedInt((int)((float)rlX * facF), (int)((float)rlZ * facF + facF));
		forestBotRight = forestMap.GetUnpaddedInt((int)((float)rlX * facF + facF), (int)((float)rlZ * facF + facF));
		float facC = (float)climateMap.InnerSize / (float)regionChunkSize;
		climateUpLeft = climateMap.GetUnpaddedInt((int)((float)rlX * facC), (int)((float)rlZ * facC));
		climateUpRight = climateMap.GetUnpaddedInt((int)((float)rlX * facC + facC), (int)((float)rlZ * facC));
		climateBotLeft = climateMap.GetUnpaddedInt((int)((float)rlX * facC), (int)((float)rlZ * facC + facC));
		climateBotRight = climateMap.GetUnpaddedInt((int)((float)rlX * facC + facC), (int)((float)rlZ * facC + facC));
		heightmap = chunks[0].MapChunk.RainHeightMap;
		preventBroadly = false;
		IBlockPatchModifier[] array = patchMod;
		foreach (IBlockPatchModifier provider in array)
		{
			preventBroadly |= provider.PreventPlacementBroadlyAt(chunkX, chunkZ);
		}
		if (TerraGenConfig.GenerateVegetation)
		{
			genPatches(chunkX, chunkZ, postPass: false);
			genShrubs(chunkX, chunkZ);
			genTrees(chunkX, chunkZ);
			genPatches(chunkX, chunkZ, postPass: true);
		}
	}

	private void genPatches(int chunkX, int chunkZ, bool postPass)
	{
		int mapsizeY = blockAccessor.MapSizeY;
		LCGRandom patchIterRandom = new LCGRandom();
		LCGRandom blockPatchRandom = new LCGRandom();
		patchIterRandom.SetWorldSeed(sapi.WorldManager.Seed - 87698);
		patchIterRandom.InitPositionSeed(chunkX, chunkZ);
		BlockPatch[] patches = GetNonTreePatches(chunkX, chunkZ);
		IMapRegion mapregion = sapi?.WorldManager.GetMapRegion(chunkX * 32 / regionSize, chunkZ * 32 / regionSize);
		for (int i = 0; i < patches.Length; i++)
		{
			BlockPatch blockPatch = patches[i];
			if (blockPatch.PostPass != postPass)
			{
				continue;
			}
			patchIterRandom.SetWorldSeed(sapi.WorldManager.Seed - 87698 + i);
			patchIterRandom.InitPositionSeed(chunkX, chunkZ);
			float chance = blockPatch.Chance * bpc.ChanceMultiplier.nextFloat(1f, patchIterRandom);
			while (chance-- > patchIterRandom.NextFloat())
			{
				int dx = patchIterRandom.NextInt(32);
				int dz = patchIterRandom.NextInt(32);
				int x = dx + chunkX * 32;
				int z = dz + chunkZ * 32;
				int y = heightmap[dz * 32 + dx];
				if (y <= 0 || y >= worldheight - 15)
				{
					continue;
				}
				tmpPos.Set(x, y, z);
				Block liquidBlock = blockAccessor.GetBlock(tmpPos, 2);
				float forestRel = GameMath.BiLerp(forestUpLeft, forestUpRight, forestBotLeft, forestBotRight, (float)dx / 32f, (float)dz / 32f) / 255f;
				forestRel = GameMath.Clamp(forestRel + forestMod, 0f, 1f);
				float shrubRel = GameMath.BiLerp(shrubUpLeft, shrubUpRight, shrubBotLeft, shrubBotRight, (float)dx / 32f, (float)dz / 32f) / 255f;
				shrubRel = GameMath.Clamp(shrubRel + shrubMod, 0f, 1f);
				int climate = GameMath.BiLerpRgbColor((float)dx / 32f, (float)dz / 32f, climateUpLeft, climateUpRight, climateBotLeft, climateBotRight);
				if (!bpc.IsPatchSuitableAt(blockPatch, liquidBlock, mapsizeY, climate, y, forestRel, shrubRel) || (!preventBroadly && !canGenerateAt(tmpPos.X, tmpPos.Z, blockPatch.CategoryHashCode)) || (blockPatch.MapCode != null && patchIterRandom.NextInt(255) > GetPatchDensity(blockPatch.MapCode, x, z, mapregion)))
				{
					continue;
				}
				int firstBlockId = 0;
				bool found = true;
				if (blockPatch.BlocksByRockType != null)
				{
					found = false;
					for (int dy = 1; dy < 5 && y - dy > 0; dy++)
					{
						string lastCodePart = blockAccessor.GetBlockBelow(tmpPos, dy, 1).LastCodePart();
						if (RockBlockIdsByType.TryGetValue(lastCodePart, out firstBlockId))
						{
							found = true;
							break;
						}
					}
				}
				if (found)
				{
					blockPatchRandom.SetWorldSeed(sapi.WorldManager.Seed - 87698 + i);
					blockPatchRandom.InitPositionSeed(x, z);
					blockPatch.Generate(blockAccessor, patchIterRandom, x, y, z, firstBlockId, (!preventBroadly) ? null : ((ActionBoolReturn<int, int>)((int tx, int tz) => canGenerateAt(tx, tz, ModStdWorldGen.PatchesHashCode))));
				}
			}
		}
	}

	private bool canGenerateAt(int x, int z, int patchesHashCode)
	{
		IBlockPatchModifier[] array = patchMod;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].PreventPlacementAt(x, z, patchesHashCode))
			{
				return false;
			}
		}
		return true;
	}

	private BlockPatch[] GetNonTreePatches(int chunkX, int chunkZ)
	{
		BlockPatch[] bpcPatchesNonTree = null;
		EnumHandling handling = EnumHandling.PassThrough;
		IBlockPatchModifier[] array = patchMod;
		foreach (IBlockPatchModifier obj in array)
		{
			EnumHandling herehandling = EnumHandling.PassThrough;
			BlockPatchConfig pbpc = obj.GetPatchProviderAt(chunkX, chunkZ, ref herehandling);
			if (herehandling == EnumHandling.PassThrough)
			{
				continue;
			}
			handling = herehandling;
			if (pbpc != null)
			{
				bpcPatchesNonTree = ((bpcPatchesNonTree == null) ? pbpc.PatchesNonTree : bpcPatchesNonTree.Append(bpc.PatchesNonTree));
				if (herehandling == EnumHandling.PreventSubsequent)
				{
					break;
				}
			}
		}
		if (handling == EnumHandling.PassThrough)
		{
			bpcPatchesNonTree = bpc.PatchesNonTree;
		}
		return bpcPatchesNonTree;
	}

	private void genShrubs(int chunkX, int chunkZ)
	{
		rnd.InitPositionSeed(chunkX, chunkZ);
		int triesShrubs = (int)treeSupplier.treeGenProps.shrubsPerChunk.nextFloat(1f, rnd);
		LCGRandom shrubTryRandom = new LCGRandom();
		while (triesShrubs > 0)
		{
			shrubTryRandom.SetWorldSeed(sapi.World.Seed - 87698 + triesShrubs);
			shrubTryRandom.InitPositionSeed(chunkX, chunkZ);
			triesShrubs--;
			int dx = shrubTryRandom.NextInt(32);
			int dz = shrubTryRandom.NextInt(32);
			int x = dx + chunkX * 32;
			int z = dz + chunkZ * 32;
			int y = heightmap[dz * 32 + dx];
			if (y <= 0 || y >= worldheight - 15)
			{
				continue;
			}
			tmpPos.Set(x, y, z);
			if (blockAccessor.GetBlock(tmpPos).Fertility == 0)
			{
				continue;
			}
			int climate = GameMath.BiLerpRgbColor((float)dx / 32f, (float)dz / 32f, climateUpLeft, climateUpRight, climateBotLeft, climateBotRight);
			float shrubChance = GameMath.BiLerp(shrubUpLeft, shrubUpRight, shrubBotLeft, shrubBotRight, (float)dx / 32f, (float)dz / 32f);
			shrubChance = GameMath.Clamp(shrubChance + 255f * forestMod, 0f, 255f);
			if (shrubTryRandom.NextFloat() > shrubChance / 255f * (shrubChance / 255f))
			{
				continue;
			}
			TreeGenInstance treegenParams = treeSupplier.GetRandomShrubGenForClimate(shrubTryRandom, climate, (int)shrubChance, y);
			if (treegenParams != null && (!preventBroadly || canGenerateAt(tmpPos.X, tmpPos.Z, ModStdWorldGen.ShrubsHashCode)))
			{
				if (blockAccessor.GetBlock(tmpPos).Replaceable >= 6000)
				{
					tmpPos.Y--;
				}
				treegenParams.skipForestFloor = true;
				treegenParams.GrowTree(blockAccessor, tmpPos, shrubTryRandom);
			}
		}
	}

	private void genTrees(int chunkX, int chunkZ)
	{
		rnd.InitPositionSeed(chunkX, chunkZ);
		int climate = GameMath.BiLerpRgbColor(0.5f, 0.5f, climateUpLeft, climateUpRight, climateBotLeft, climateBotRight);
		float wetrel = (float)Climate.GetRainFall((climate >> 8) & 0xFF, heightmap[528]) / 255f;
		float temprel = (float)((climate >> 16) & 0xFF) / 255f;
		float dryrel = 1f - wetrel;
		float drypenalty = 1f - GameMath.Clamp(2f * (dryrel - 0.5f + 1.5f * Math.Max(temprel - 0.6f, 0f)), 0f, 0.8f);
		float wetboost = 1f + 3f * Math.Max(0f, wetrel - 0.75f);
		int triesTrees = (int)(treeSupplier.treeGenProps.treesPerChunk.nextFloat(1f, rnd) * drypenalty * wetboost);
		int treesGenerated = 0;
		EnumHemisphere hemisphere = sapi.World.Calendar.GetHemisphere(new BlockPos(chunkX * 32 + 16, 0, chunkZ * 32 + 16));
		LCGRandom treeTryRandom = new LCGRandom();
		while (triesTrees > 0)
		{
			treeTryRandom.SetWorldSeed(sapi.World.Seed - 87698 + triesTrees);
			treeTryRandom.InitPositionSeed(chunkX, chunkZ);
			triesTrees--;
			int dx = treeTryRandom.NextInt(32);
			int dz = treeTryRandom.NextInt(32);
			int x = dx + chunkX * 32;
			int z = dz + chunkZ * 32;
			int y = heightmap[dz * 32 + dx];
			if (y <= 0 || y >= worldheight - 15)
			{
				continue;
			}
			bool underwater = false;
			tmpPos.Set(x, y, z);
			if (blockAccessor.GetBlock(tmpPos, 2).IsLiquid())
			{
				underwater = true;
				tmpPos.Y--;
				if (blockAccessor.GetBlock(tmpPos, 2).IsLiquid())
				{
					tmpPos.Y--;
				}
			}
			if (blockAccessor.GetBlock(tmpPos).Fertility == 0)
			{
				continue;
			}
			float treeDensity = GameMath.BiLerp(forestUpLeft, forestUpRight, forestBotLeft, forestBotRight, (float)dx / 32f, (float)dz / 32f);
			climate = GameMath.BiLerpRgbColor((float)dx / 32f, (float)dz / 32f, climateUpLeft, climateUpRight, climateBotLeft, climateBotRight);
			treeDensity = GameMath.Clamp(treeDensity + forestMod * 255f, 0f, 255f);
			float treeDensityNormalized = treeDensity / 255f;
			if (treeTryRandom.NextFloat() > Math.Max(0.0025f, treeDensityNormalized * treeDensityNormalized) || forestMod <= -1f)
			{
				continue;
			}
			TreeGenInstance treegenParams = treeSupplier.GetRandomTreeGenForClimate(treeTryRandom, climate, (int)treeDensity, y, underwater);
			if (treegenParams != null && (!preventBroadly || canGenerateAt(tmpPos.X, tmpPos.Z, ModStdWorldGen.TreesHashCode)))
			{
				if (blockAccessor.GetBlock(tmpPos).Replaceable >= 6000)
				{
					tmpPos.Y--;
				}
				treegenParams.skipForestFloor = false;
				treegenParams.hemisphere = hemisphere;
				treegenParams.treesInChunkGenerated = treesGenerated;
				treegenParams.GrowTree(blockAccessor, tmpPos, treeTryRandom);
				treesGenerated++;
			}
		}
	}

	public int GetPatchDensity(string code, int posX, int posZ, IMapRegion mapregion)
	{
		if (mapregion == null)
		{
			return 0;
		}
		int lx = posX % regionSize;
		int lz = posZ % regionSize;
		mapregion.BlockPatchMaps.TryGetValue(code, out var map);
		if (map != null)
		{
			float posXInRegionOre = GameMath.Clamp((float)lx / (float)regionSize * (float)noiseSizeDensityMap, 0f, noiseSizeDensityMap - 1);
			float posZInRegionOre = GameMath.Clamp((float)lz / (float)regionSize * (float)noiseSizeDensityMap, 0f, noiseSizeDensityMap - 1);
			return map.GetUnpaddedColorLerped(posXInRegionOre, posZInRegionOre);
		}
		return 0;
	}
}
