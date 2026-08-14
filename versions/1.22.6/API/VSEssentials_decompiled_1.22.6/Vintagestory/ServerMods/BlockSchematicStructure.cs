using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.ServerMods.NoObf;

namespace Vintagestory.ServerMods;

public class BlockSchematicStructure : BlockSchematic
{
	public Dictionary<int, AssetLocation> BlockCodesTmpForRemap = new Dictionary<int, AssetLocation>();

	public AssetLocation FromFile;

	public Block[,,] blocksByPos;

	public Dictionary<int, Block> FluidBlocksByPos;

	public BlockLayerConfig blockLayerConfig;

	private int mapheight;

	private PlaceBlockDelegate handler;

	internal GenBlockLayers genBlockLayers;

	public int MaxYDiff = 3;

	public int MaxBelowSealevel = 20;

	public int? StoryLocationMaxAmount;

	public int OffsetY { get; set; } = -1;

	public static bool SatisfiesMinSpawnDistance(int minSpawnDistance, BlockPos pos, BlockPos spawnPos)
	{
		if (minSpawnDistance <= 0)
		{
			return true;
		}
		return spawnPos.HorDistanceSqTo(pos.X, pos.Z) > (float)(minSpawnDistance * minSpawnDistance);
	}

	public override void Init(IBlockAccessor blockAccessor)
	{
		base.Init(blockAccessor);
		mapheight = blockAccessor.MapSizeY;
		blocksByPos = new Block[SizeX + 1, SizeY + 1, SizeZ + 1];
		FluidBlocksByPos = new Dictionary<int, Block>();
		for (int i = 0; i < Indices.Count; i++)
		{
			uint index = Indices[i];
			int storedBlockid = BlockIds[i];
			int dx = (int)(index & 0x3FF);
			int dy = (int)((index >> 20) & 0x3FF);
			int dz = (int)((index >> 10) & 0x3FF);
			Block block = blockAccessor.GetBlock(BlockCodes[storedBlockid]);
			if (block != null)
			{
				if (block.ForFluidsLayer)
				{
					FluidBlocksByPos.Add((int)index, block);
				}
				else
				{
					blocksByPos[dx, dy, dz] = block;
				}
			}
		}
		handler = null;
		switch (ReplaceMode)
		{
		case EnumReplaceMode.ReplaceAll:
			handler = PlaceReplaceAll;
			break;
		case EnumReplaceMode.Replaceable:
			handler = PlaceReplaceable;
			break;
		case EnumReplaceMode.ReplaceAllNoAir:
			handler = PlaceReplaceAllNoAir;
			break;
		case EnumReplaceMode.ReplaceOnlyAir:
			handler = PlaceReplaceOnlyAir;
			break;
		}
	}

	public int PlaceRespectingBlockLayers(IBlockAccessor blockAccessor, IWorldAccessor worldForCollectibleResolve, BlockPos startPos, int climateUpLeft, int climateUpRight, int climateBotLeft, int climateBotRight, Dictionary<int, Dictionary<int, int>> replaceBlocks, int[] replaceWithBlockLayersBlockids, bool replaceMetaBlocks = true, bool replaceBlockEntities = false, bool suppressSoilIfAirBelow = false, bool displaceWater = false)
	{
		Unpack(worldForCollectibleResolve.Api);
		if (genBlockLayers == null)
		{
			genBlockLayers = worldForCollectibleResolve.Api.ModLoader.GetModSystem<GenBlockLayers>();
		}
		BlockPos curPos = new BlockPos(startPos.dimension);
		BlockPos localCurrentPos = new BlockPos(startPos.dimension);
		BlockPos tmpPos = new BlockPos(startPos.dimension);
		int placed = 0;
		int chunkBaseX = startPos.X / 32 * 32;
		int chunkBaseZ = startPos.Z / 32 * 32;
		curPos.Set(SizeX / 2 + startPos.X, startPos.Y, SizeZ / 2 + startPos.Z);
		IMapChunk mapchunk = blockAccessor.GetMapChunkAtBlockPos(curPos);
		int centerrockblockid = mapchunk.TopRockIdMap[curPos.Z % 32 * 32 + curPos.X % 32];
		IWorldAccessor worldAccessor;
		if (!(blockAccessor is IWorldGenBlockAccessor wgba))
		{
			worldAccessor = worldForCollectibleResolve;
		}
		else
		{
			IWorldAccessor worldgenWorldAccessor = wgba.WorldgenWorldAccessor;
			worldAccessor = worldgenWorldAccessor;
		}
		IWorldAccessor worldgenWorldAccessor2 = worldAccessor;
		resolveReplaceRemapsForBlockEntities(blockAccessor, worldForCollectibleResolve, replaceBlocks, centerrockblockid);
		Dictionary<BlockPos, Block> layerBlockForBlockEntities = new Dictionary<BlockPos, Block>();
		for (int x = 0; x < SizeX; x++)
		{
			for (int z = 0; z < SizeZ; z++)
			{
				curPos.Set(x + startPos.X, startPos.Y, z + startPos.Z);
				if (!blockAccessor.IsValidPos(curPos))
				{
					continue;
				}
				mapchunk = blockAccessor.GetMapChunkAtBlockPos(curPos);
				int rockblockid = mapchunk.TopRockIdMap[curPos.Z % 32 * 32 + curPos.X % 32];
				int depth = mapchunk.WorldGenTerrainHeightMap[curPos.Z % 32 * 32 + curPos.X % 32] - (SizeY + startPos.Y);
				int maxY = -1;
				int underWaterDepth = -1;
				Block aboveLiqBlock = blockAccessor.GetBlockAbove(curPos, SizeY, 2);
				if (aboveLiqBlock != null && aboveLiqBlock.IsLiquid())
				{
					underWaterDepth++;
				}
				bool highestBlockinCol = true;
				for (int y = SizeY - 1; y >= 0; y--)
				{
					depth++;
					curPos.Set(x + startPos.X, y + startPos.Y, z + startPos.Z);
					if (!blockAccessor.IsValidPos(curPos))
					{
						continue;
					}
					localCurrentPos.Set(x, y, z);
					Block block = blocksByPos[x, y, z];
					FluidBlocksByPos.TryGetValue(localCurrentPos.ToSchematicIndex(), out var fluidBlock);
					if (block == null)
					{
						block = fluidBlock;
					}
					aboveLiqBlock = blockAccessor.GetBlock(curPos, 2);
					if (aboveLiqBlock != null && aboveLiqBlock.IsLiquid())
					{
						underWaterDepth++;
					}
					if (block == null || (replaceMetaBlocks && (block.Id == BlockSchematic.UndergroundBlockId || block.Id == BlockSchematic.AbovegroundBlockId)))
					{
						continue;
					}
					if (block.Replaceable < 1000 && (depth >= 0 || underWaterDepth >= 0) && (replaceWithBlockLayersBlockids.Contains(block.BlockId) || block.CustomBlockLayerHandler))
					{
						if (suppressSoilIfAirBelow && (y == 0 || blocksByPos[x, y - 1, z] == null) && blockAccessor.GetBlockBelow(curPos, 1, 1).Replaceable > 3000)
						{
							for (int yy = y + 1; yy < SizeY; yy++)
							{
								Block placedBlock = blocksByPos[x, yy, z];
								if (placedBlock == null || !replaceWithBlockLayersBlockids.Contains(placedBlock.BlockId))
								{
									break;
								}
								blockAccessor.SetBlock(0, tmpPos.Set(curPos.X, startPos.Y + yy, curPos.Z), 1);
							}
							continue;
						}
						if (depth == 0 && replaceWithBlockLayersBlockids.Length > 1)
						{
							Block aboveBlock = blockAccessor.GetBlockAbove(curPos, 1, 1);
							if (aboveBlock.SideSolid[BlockFacing.DOWN.Index] && aboveBlock.BlockMaterial != EnumBlockMaterial.Wood && aboveBlock.BlockMaterial != EnumBlockMaterial.Snow && aboveBlock.BlockMaterial != EnumBlockMaterial.Ice)
							{
								depth++;
							}
						}
						int climate = GameMath.BiLerpRgbColor(GameMath.Clamp((float)(curPos.X - chunkBaseX) / 32f, 0f, 1f), GameMath.Clamp((float)(curPos.Z - chunkBaseZ) / 32f, 0f, 1f), climateUpLeft, climateUpRight, climateBotLeft, climateBotRight);
						Block layerBlock = GetBlockLayerBlock((climate >> 8) & 0xFF, (climate >> 16) & 0xFF, curPos.Y - 1, rockblockid, depth, block, worldForCollectibleResolve.Blocks, curPos, underWaterDepth);
						if (block.CustomBlockLayerHandler && layerBlock != block)
						{
							if (depth >= 0 || underWaterDepth < 0)
							{
								layerBlockForBlockEntities[curPos.Copy()] = layerBlock;
							}
						}
						else
						{
							block = layerBlock;
						}
					}
					if (replaceBlocks != null && replaceBlocks.TryGetValue(block.Id, out var replaceByBlock) && replaceByBlock.TryGetValue(centerrockblockid, out var newBlockId))
					{
						block = blockAccessor.GetBlock(newBlockId);
					}
					if (block.ForFluidsLayer)
					{
						blockAccessor.SetBlock(0, curPos, 1);
					}
					int p = handler(blockAccessor, curPos, block, replaceMeta: true);
					if (fluidBlock != null && !block.Equals(fluidBlock))
					{
						handler(blockAccessor, curPos, fluidBlock, replaceMeta: true);
					}
					if (p > 0)
					{
						if (displaceWater && !block.ForFluidsLayer)
						{
							blockAccessor.SetBlock(0, curPos, 2);
						}
						else if (block.Id != 0 && !block.SideSolid.All)
						{
							aboveLiqBlock = blockAccessor.GetBlockAbove(curPos, 1, 2);
							if (aboveLiqBlock.Id != 0)
							{
								blockAccessor.SetBlock(aboveLiqBlock.BlockId, curPos, 2);
							}
						}
						if (highestBlockinCol)
						{
							Block aboveBlock2 = blockAccessor.GetBlockAbove(curPos, 1, 1);
							if (aboveBlock2.Id > 0)
							{
								aboveBlock2.OnNeighbourBlockChange(worldgenWorldAccessor2, curPos.UpCopy(), curPos);
							}
							highestBlockinCol = false;
						}
						placed += p;
						if (!block.RainPermeable)
						{
							if (IsFillerOrPath(block))
							{
								int lx = curPos.X % 32;
								int lz = curPos.Z % 32;
								if (mapchunk.RainHeightMap[lz * 32 + lx] == curPos.Y)
								{
									mapchunk.RainHeightMap[lz * 32 + lx]--;
								}
							}
							else
							{
								maxY = Math.Max(curPos.Y, maxY);
							}
						}
					}
					if (block.GetLightHsv(blockAccessor, curPos)[2] > 0 && blockAccessor is IWorldGenBlockAccessor)
					{
						Block oldBlock = blockAccessor.GetBlock(curPos);
						((IWorldGenBlockAccessor)blockAccessor).ScheduleBlockLightUpdate(curPos, oldBlock.BlockId, block.BlockId);
					}
				}
				if (maxY >= 0)
				{
					int lx2 = curPos.X % 32;
					int lz2 = curPos.Z % 32;
					int y2 = mapchunk.RainHeightMap[lz2 * 32 + lx2];
					mapchunk.RainHeightMap[lz2 * 32 + lx2] = (ushort)Math.Max(y2, maxY);
				}
			}
		}
		PlaceDecors(blockAccessor, startPos);
		PlaceEntitiesAndBlockEntities(blockAccessor, worldForCollectibleResolve, startPos, BlockCodesTmpForRemap, ItemCodes, replaceBlockEntities, replaceBlocks, centerrockblockid, layerBlockForBlockEntities, replaceMetaBlocks);
		return placed;
	}

	private void resolveReplaceRemapsForBlockEntities(IBlockAccessor blockAccessor, IWorldAccessor worldForCollectibleResolve, Dictionary<int, Dictionary<int, int>> replaceBlocks, int centerrockblockid)
	{
		if (replaceBlocks == null)
		{
			BlockCodesTmpForRemap = BlockCodes;
			return;
		}
		foreach (KeyValuePair<int, AssetLocation> val in BlockCodes)
		{
			Block origBlock = worldForCollectibleResolve.GetBlock(val.Value);
			if (origBlock != null)
			{
				BlockCodesTmpForRemap[val.Key] = val.Value;
				if (replaceBlocks.TryGetValue(origBlock.Id, out var replaceByBlock) && replaceByBlock.TryGetValue(centerrockblockid, out var newBlockId))
				{
					BlockCodesTmpForRemap[val.Key] = blockAccessor.GetBlock(newBlockId).Code;
				}
			}
		}
	}

	public virtual int PlaceReplacingBlocks(IBlockAccessor blockAccessor, IWorldAccessor worldForCollectibleResolve, BlockPos startPos, EnumReplaceMode mode, Dictionary<int, Dictionary<int, int>> replaceBlocks, int? rockBlockId, bool replaceMetaBlocks = true)
	{
		Unpack(worldForCollectibleResolve.Api);
		BlockPos curPos = new BlockPos(startPos.dimension);
		int placed = 0;
		curPos.Set(SizeX / 2 + startPos.X, startPos.Y, SizeZ / 2 + startPos.Z);
		IMapChunk mapchunk = blockAccessor.GetMapChunkAtBlockPos(curPos);
		int centerrockblockid = rockBlockId ?? mapchunk.TopRockIdMap[curPos.Z % 32 * 32 + curPos.X % 32];
		resolveReplaceRemapsForBlockEntities(blockAccessor, worldForCollectibleResolve, replaceBlocks, centerrockblockid);
		PlaceBlockDelegate handler = null;
		switch (ReplaceMode)
		{
		case EnumReplaceMode.ReplaceAll:
			handler = PlaceReplaceAll;
			break;
		case EnumReplaceMode.Replaceable:
			handler = PlaceReplaceable;
			break;
		case EnumReplaceMode.ReplaceAllNoAir:
			handler = PlaceReplaceAllNoAir;
			break;
		case EnumReplaceMode.ReplaceOnlyAir:
			handler = PlaceReplaceOnlyAir;
			break;
		}
		for (int i = 0; i < Indices.Count; i++)
		{
			uint index = Indices[i];
			int storedBlockid = BlockIds[i];
			int dx = (int)(index & 0x3FF);
			int dy = (int)((index >> 20) & 0x3FF);
			int dz = (int)((index >> 10) & 0x3FF);
			AssetLocation blockCode = BlockCodes[storedBlockid];
			Block newBlock = blockAccessor.GetBlock(blockCode);
			if (newBlock == null || (replaceMetaBlocks && (newBlock.Id == BlockSchematic.UndergroundBlockId || newBlock.Id == BlockSchematic.AbovegroundBlockId)))
			{
				continue;
			}
			curPos.Set(dx + startPos.X, dy + startPos.Y, dz + startPos.Z);
			if (blockAccessor.IsValidPos(curPos))
			{
				if (replaceBlocks.TryGetValue(newBlock.Id, out var replaceByBlock) && replaceByBlock.TryGetValue(centerrockblockid, out var newBlockId))
				{
					newBlock = blockAccessor.GetBlock(newBlockId);
				}
				if (newBlock.ForFluidsLayer && index != Indices[i - 1])
				{
					blockAccessor.SetBlock(0, curPos, 1);
				}
				placed += handler(blockAccessor, curPos, newBlock, replaceMeta: true);
				if (newBlock.LightHsv[2] > 0 && blockAccessor is IWorldGenBlockAccessor)
				{
					Block oldBlock = blockAccessor.GetBlock(curPos);
					((IWorldGenBlockAccessor)blockAccessor).ScheduleBlockLightUpdate(curPos, oldBlock.BlockId, newBlock.BlockId);
				}
			}
		}
		if (!(blockAccessor is IBlockAccessorRevertable))
		{
			PlaceDecors(blockAccessor, startPos);
			PlaceEntitiesAndBlockEntities(blockAccessor, worldForCollectibleResolve, startPos, BlockCodesTmpForRemap, ItemCodes, replaceBlockEntities: false, null, centerrockblockid, null, GlobalConfig.ReplaceMetaBlocks);
		}
		return placed;
	}

	internal Block GetBlockLayerBlock(int unscaledRain, int unscaledTemp, int posY, int rockBlockId, int forDepth, Block defaultBlock, IList<Block> blocks, BlockPos pos, int underWaterDepth)
	{
		if (blockLayerConfig == null)
		{
			return defaultBlock;
		}
		posY -= forDepth;
		float distx = (float)genBlockLayers.distort2dx.Noise(pos.X, pos.Z);
		float temperature = Climate.GetScaledAdjustedTemperatureFloat(unscaledTemp, posY - TerraGenConfig.seaLevel + (int)(distx / 5f));
		float rainRel = (float)Climate.GetRainFall(unscaledRain, posY) / 255f;
		float heightRel = ((float)posY - (float)TerraGenConfig.seaLevel) / ((float)mapheight - (float)TerraGenConfig.seaLevel);
		float fertilityRel = (float)Climate.GetFertilityFromUnscaledTemp((int)(rainRel * 255f), unscaledTemp, heightRel) / 255f;
		double posRand = (double)GameMath.MurmurHash3(pos.X, 1, pos.Z) / 2147483647.0;
		posRand = (posRand + 1.0) * (double)blockLayerConfig.blockLayerTransitionSize;
		for (int j = 0; j < blockLayerConfig.Blocklayers.Length; j++)
		{
			if (underWaterDepth < 0)
			{
				BlockLayer bl = blockLayerConfig.Blocklayers[j];
				float yDist = bl.CalcYDistance(posY, mapheight);
				if (!((double)(bl.CalcTrfDistance(temperature, rainRel, fertilityRel) + yDist) > posRand))
				{
					int blockId = bl.GetBlockId(posRand, temperature, rainRel, fertilityRel, rockBlockId, pos, mapheight);
					if (blockId != 0 && forDepth-- <= 0)
					{
						return blocks[blockId];
					}
				}
			}
			else if (j < blockLayerConfig.LakeBedLayer.BlockCodeByMin.Length)
			{
				LakeBedBlockCodeByMin lbbc = blockLayerConfig.LakeBedLayer.BlockCodeByMin[j];
				if (lbbc.Suitable(temperature, rainRel, (float)posY / (float)mapheight, (float)posRand))
				{
					return blocks[lbbc.GetBlockForMotherRock(rockBlockId)];
				}
			}
		}
		return defaultBlock;
	}

	public override BlockSchematic ClonePacked()
	{
		return new BlockSchematicStructure
		{
			SizeX = SizeX,
			SizeY = SizeY,
			SizeZ = SizeZ,
			OffsetY = OffsetY,
			MaxYDiff = MaxYDiff,
			MaxBelowSealevel = MaxBelowSealevel,
			GameVersion = GameVersion,
			FromFile = FromFile,
			BlockCodes = new Dictionary<int, AssetLocation>(BlockCodes),
			ItemCodes = new Dictionary<int, AssetLocation>(ItemCodes),
			Indices = new List<uint>(Indices),
			BlockIds = new List<int>(BlockIds),
			BlockEntities = new Dictionary<uint, string>(BlockEntities),
			Entities = new List<string>(Entities),
			DecorIndices = new List<uint>(DecorIndices),
			DecorIds = new List<long>(DecorIds),
			ReplaceMode = ReplaceMode,
			EntranceRotation = EntranceRotation,
			OriginalPos = OriginalPos
		};
	}

	public void Unpack(ICoreAPI api)
	{
		if (blocksByPos == null)
		{
			Init(api.World.BlockAccessor);
			LoadMetaInformationAndValidate(api.World.BlockAccessor, api.World, FromFile);
		}
	}

	public void Unpack(ICoreAPI api, int orientation)
	{
		if (orientation > 0 && blocksByPos == null)
		{
			TransformWhilePacked(api.World, EnumOrigin.BottomCenter, orientation * 90);
		}
		Unpack(api);
	}
}
