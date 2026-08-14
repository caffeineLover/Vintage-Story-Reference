using System;
using System.Security.Cryptography;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.ServerMods.NoObf;

namespace Vintagestory.ServerMods;

public abstract class ModStdWorldGen : ModSystem
{
	public GlobalConfig gcfg;

	protected const int chunksize = 32;

	public static int StructuresHashCode;

	public static int PatchesHashCode;

	public static int CavesHashCode;

	public static int TreesHashCode;

	public static int ShrubsHashCode;

	public static int StalagHashCode;

	public static int HotSpringsHashCode;

	public static int RivuletsHashCode;

	public static int PondsHashCode;

	public static int CreaturesHashCode;

	public static int[] AllHashCodes;

	private ModSystemWorldGenStructures provider;

	static ModStdWorldGen()
	{
		StructuresHashCode = BitConverter.ToInt32(SHA256.HashData("structures"u8.ToArray()));
		PatchesHashCode = BitConverter.ToInt32(SHA256.HashData("patches"u8.ToArray()));
		CavesHashCode = BitConverter.ToInt32(SHA256.HashData("caves"u8.ToArray()));
		TreesHashCode = BitConverter.ToInt32(SHA256.HashData("trees"u8.ToArray()));
		ShrubsHashCode = BitConverter.ToInt32(SHA256.HashData("shrubs"u8.ToArray()));
		HotSpringsHashCode = BitConverter.ToInt32(SHA256.HashData("hotsprings"u8.ToArray()));
		RivuletsHashCode = BitConverter.ToInt32(SHA256.HashData("rivulets"u8.ToArray()));
		StalagHashCode = BitConverter.ToInt32(SHA256.HashData("stalag"u8.ToArray()));
		PondsHashCode = BitConverter.ToInt32(SHA256.HashData("pond"u8.ToArray()));
		CreaturesHashCode = BitConverter.ToInt32(SHA256.HashData("creatures"u8.ToArray()));
		AllHashCodes = new int[10] { StructuresHashCode, PatchesHashCode, CavesHashCode, TreesHashCode, ShrubsHashCode, HotSpringsHashCode, RivuletsHashCode, StalagHashCode, PondsHashCode, CreaturesHashCode };
	}

	public override bool ShouldLoad(EnumAppSide side)
	{
		return side == EnumAppSide.Server;
	}

	public override void StartServerSide(ICoreServerAPI api)
	{
		LoadGlobalConfig(api);
	}

	public void LoadGlobalConfig(ICoreServerAPI api)
	{
		gcfg = GlobalConfig.GetInstance(api);
		provider = api.ModLoader.GetModSystem<ModSystemWorldGenStructures>();
	}

	public IStructureLocation GetIntersectingStructure(int x, int z, int category)
	{
		return provider?.GetIntersectingStructure(x, z, category);
	}

	public IStructureLocation GetIntersectingStructure(int x, int z)
	{
		return GetIntersectingStructure(x, z, -1);
	}

	public static void UpdateHeightmap(IChunkColumnGenerateRequest request, IWorldGenBlockAccessor worldGenBlockAccessor)
	{
		int updatedPositionsT = 0;
		int updatedPositionsR = 0;
		ushort[] rainHeightMap = request.Chunks[0].MapChunk.RainHeightMap;
		ushort[] terrainHeightMap = request.Chunks[0].MapChunk.WorldGenTerrainHeightMap;
		for (int i = 0; i < rainHeightMap.Length; i++)
		{
			rainHeightMap[i] = 0;
			terrainHeightMap[i] = 0;
		}
		int mapSizeY = worldGenBlockAccessor.MapSizeY;
		int mapSize2D = 1024;
		for (int x = 0; x < 32; x++)
		{
			for (int z = 0; z < 32; z++)
			{
				int mapIndex = z * 32 + x;
				bool rainSet = false;
				bool heightSet = false;
				for (int posY = mapSizeY - 1; posY >= 0; posY--)
				{
					int num = posY % 32;
					IServerChunk chunk = request.Chunks[posY / 32];
					int chunkIndex = (num * 32 + z) * 32 + x;
					int blockId = chunk.Data[chunkIndex];
					if (blockId != 0)
					{
						Block block = worldGenBlockAccessor.GetBlock(blockId);
						bool newRainPermeable = block.RainPermeable;
						bool num2 = block.SideSolid[BlockFacing.UP.Index];
						if (!newRainPermeable && !rainSet)
						{
							rainSet = true;
							rainHeightMap[mapIndex] = (ushort)posY;
							updatedPositionsR++;
						}
						if (num2 && !heightSet)
						{
							heightSet = true;
							terrainHeightMap[mapIndex] = (ushort)posY;
							updatedPositionsT++;
						}
						if (updatedPositionsR >= mapSize2D && updatedPositionsT >= mapSize2D)
						{
							return;
						}
					}
				}
			}
		}
	}

	public void GenerateGrass(ICoreServerAPI api, IChunkColumnGenerateRequest request, LCGRandom grassRand, GenBlockLayers genBlockLayers)
	{
		IServerChunk[] chunks = request.Chunks;
		int chunkX = request.ChunkX;
		int chunkZ = request.ChunkZ;
		grassRand.InitPositionSeed(chunkX, chunkZ);
		IntDataMap2D forestMap = chunks[0].MapChunk.MapRegion.ForestMap;
		IntDataMap2D climateMap = chunks[0].MapChunk.MapRegion.ClimateMap;
		ushort[] heightMap = chunks[0].MapChunk.RainHeightMap;
		int regionChunkSize = api.WorldManager.RegionSize / 32;
		int rdx = chunkX % regionChunkSize;
		int rdz = chunkZ % regionChunkSize;
		float climateStep = (float)climateMap.InnerSize / (float)regionChunkSize;
		float forestStep = (float)forestMap.InnerSize / (float)regionChunkSize;
		int forestUpLeft = forestMap.GetUnpaddedInt((int)((float)rdx * forestStep), (int)((float)rdz * forestStep));
		int forestUpRight = forestMap.GetUnpaddedInt((int)((float)rdx * forestStep + forestStep), (int)((float)rdz * forestStep));
		int forestBotLeft = forestMap.GetUnpaddedInt((int)((float)rdx * forestStep), (int)((float)rdz * forestStep + forestStep));
		int forestBotRight = forestMap.GetUnpaddedInt((int)((float)rdx * forestStep + forestStep), (int)((float)rdz * forestStep + forestStep));
		BlockPos herePos = new BlockPos(0);
		int mapheight = api.WorldManager.MapSizeY;
		for (int x = 0; x < 32; x++)
		{
			for (int z = 0; z < 32; z++)
			{
				herePos.Set(chunkX * 32 + x, 1, chunkZ * 32 + z);
				double distx;
				double distz;
				int rnd = genBlockLayers.RandomlyAdjustPosition(herePos, out distx, out distz);
				int posY = heightMap[z * 32 + x];
				if (posY < mapheight)
				{
					int unpaddedColorLerped = climateMap.GetUnpaddedColorLerped((float)rdx * climateStep + climateStep * ((float)x + (float)distx) / 32f, (float)rdz * climateStep + climateStep * ((float)z + (float)distz) / 32f);
					int unscaledTemp = (unpaddedColorLerped >> 16) & 0xFF;
					float temp = Climate.GetScaledAdjustedTemperatureFloat(unscaledTemp, posY - TerraGenConfig.seaLevel + rnd);
					float tempRel = (float)Climate.GetAdjustedTemperature(unscaledTemp, posY - TerraGenConfig.seaLevel + rnd) / 255f;
					float rainRel = (float)Climate.GetRainFall((unpaddedColorLerped >> 8) & 0xFF, posY + rnd) / 255f;
					float forestRel = GameMath.BiLerp(forestUpLeft, forestUpRight, forestBotLeft, forestBotRight, (float)x / 32f, (float)z / 32f) / 255f;
					ushort num = chunks[0].MapChunk.WorldGenTerrainHeightMap[z * 32 + x];
					int chunkY = num / 32;
					int lY = num % 32;
					int index3d = (32 * lY + z) * 32 + x;
					int rockblockID = chunks[chunkY].Data.GetBlockIdUnsafe(index3d);
					if (api.World.Blocks[rockblockID].BlockMaterial == EnumBlockMaterial.Soil)
					{
						genBlockLayers.PlaceTallGrass(x, posY, z, chunks, rainRel, tempRel, temp, forestRel, 0);
					}
				}
			}
		}
	}
}
