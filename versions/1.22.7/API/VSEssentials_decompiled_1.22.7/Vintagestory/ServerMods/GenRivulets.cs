using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace Vintagestory.ServerMods;

public class GenRivulets : ModStdWorldGen
{
	private ICoreServerAPI api;

	private LCGRandom rnd;

	private IWorldGenBlockAccessor blockAccessor;

	private int regionsize;

	private int chunkMapSizeY;

	private BlockPos chunkBase = new BlockPos(0);

	private BlockPos chunkend = new BlockPos(0);

	private List<Cuboidi> structuresIntersectingChunk = new List<Cuboidi>();

	public override bool ShouldLoad(EnumAppSide side)
	{
		return side == EnumAppSide.Server;
	}

	public override double ExecuteOrder()
	{
		return 0.9;
	}

	public override void StartServerSide(ICoreServerAPI api)
	{
		this.api = api;
		if (TerraGenConfig.DoDecorationPass)
		{
			api.Event.InitWorldGenerator(initWorldGen, "standard");
			api.Event.ChunkColumnGeneration(OnChunkColumnGen, EnumWorldGenPass.Vegetation, "standard");
			api.Event.GetWorldgenBlockAccessor(OnWorldGenBlockAccessor);
		}
	}

	private void OnWorldGenBlockAccessor(IChunkProviderThread chunkProvider)
	{
		blockAccessor = chunkProvider.GetBlockAccessor(updateHeightmap: true);
		regionsize = blockAccessor.RegionSize;
	}

	private void initWorldGen()
	{
		LoadGlobalConfig(api);
		rnd = new LCGRandom(api.WorldManager.Seed);
		chunkMapSizeY = api.WorldManager.MapSizeY / 32;
	}

	private void OnChunkColumnGen(IChunkColumnGenerateRequest request)
	{
		IServerChunk[] chunks = request.Chunks;
		int chunkX = request.ChunkX;
		int chunkZ = request.ChunkZ;
		blockAccessor.BeginColumn();
		IntDataMap2D climateMap = chunks[0].MapChunk.MapRegion.ClimateMap;
		int regionChunkSize = api.WorldManager.RegionSize / 32;
		float fac = (float)climateMap.InnerSize / (float)regionChunkSize;
		int rlX = chunkX % regionChunkSize;
		int rlZ = chunkZ % regionChunkSize;
		int climateUpLeft = climateMap.GetUnpaddedInt((int)((float)rlX * fac), (int)((float)rlZ * fac));
		int climateUpRight = climateMap.GetUnpaddedInt((int)((float)rlX * fac + fac), (int)((float)rlZ * fac));
		int climateBotLeft = climateMap.GetUnpaddedInt((int)((float)rlX * fac), (int)((float)rlZ * fac + fac));
		int climateBotRight = climateMap.GetUnpaddedInt((int)((float)rlX * fac + fac), (int)((float)rlZ * fac + fac));
		int num = GameMath.BiLerpRgbColor(0.5f, 0.5f, climateUpLeft, climateUpRight, climateBotLeft, climateBotRight);
		structuresIntersectingChunk.Clear();
		api.World.BlockAccessor.WalkStructures(chunkBase.Set(chunkX * 32, 0, chunkZ * 32), chunkend.Set(chunkX * 32 + 32, chunkMapSizeY * 32, chunkZ * 32 + 32), delegate(GeneratedStructure struc)
		{
			if (struc.SuppressRivulets)
			{
				structuresIntersectingChunk.Add(struc.Location.Clone().GrowBy(1, 1, 1));
			}
		});
		int rain = (num >> 8) & 0xFF;
		int humidity = num & 0xFF;
		int temp = (num >> 16) & 0xFF;
		int geoActivity = getGeologicActivity(chunkX * 32 + 16, chunkZ * 32 + 16);
		float geoActivityYThreshold = (float)getGeologicActivity(chunkX * 32 + 16, chunkZ * 32 + 16) / 2f * (float)api.World.BlockAccessor.MapSizeY / 256f;
		int quantityWaterRivulets = 2 * ((int)((float)(160 * (rain + humidity)) / 255f) * (api.WorldManager.MapSizeY / 32) - Math.Max(0, 100 - temp));
		int quantityWaterRivuletMountainSide = GameMath.RoundRandom(rnd, (float)quantityWaterRivulets / 400f);
		int quantityLavaRivers = (int)((float)(500 * geoActivity) / 255f * (float)(api.WorldManager.MapSizeY / 32));
		float chance = gcfg.waterRapidsChance * 2f;
		float scaledAdjustedTemperatureFloat = Climate.GetScaledAdjustedTemperatureFloat(temp, 0);
		rnd.InitPositionSeed(chunkX, chunkZ);
		if (scaledAdjustedTemperatureFloat >= -15f)
		{
			while (quantityWaterRivulets-- > 0)
			{
				tryGenRivulet(chunks, chunkX, chunkZ, geoActivityYThreshold, lava: false);
			}
			while (quantityWaterRivuletMountainSide-- > 0)
			{
				int blockid = ((rnd.NextDouble() < (double)chance) ? gcfg.rivuletRapidWaterBlockId : gcfg.rivuletWaterBlockId);
				tryGenMountainSideRivers(chunks, chunkX, chunkZ, blockid);
			}
		}
		while (quantityLavaRivers-- > 0)
		{
			tryGenRivulet(chunks, chunkX, chunkZ, geoActivityYThreshold + 10f, lava: true);
		}
	}

	private void tryGenRivulet(IServerChunk[] chunks, int chunkX, int chunkZ, float geoActivityYThreshold, bool lava)
	{
		IMapChunk mapChunk = chunks[0].MapChunk;
		int surfaceY = (int)((float)TerraGenConfig.seaLevel * 1.1f);
		int aboveSurfaceHeight = api.WorldManager.MapSizeY - surfaceY;
		int dx = 1 + rnd.NextInt(30);
		int y = Math.Min(1 + rnd.NextInt(surfaceY) + rnd.NextInt(aboveSurfaceHeight) * rnd.NextInt(aboveSurfaceHeight), api.WorldManager.MapSizeY - 2);
		int dz = 1 + rnd.NextInt(30);
		ushort hereSurfaceY = mapChunk.WorldGenTerrainHeightMap[dz * 32 + dx];
		if ((y > hereSurfaceY && rnd.NextInt(2) == 0) || ((float)y < geoActivityYThreshold && !lava) || ((float)y > geoActivityYThreshold && lava))
		{
			return;
		}
		int quantitySolid = 0;
		int quantityAir = 0;
		for (int i = 0; i < 6; i++)
		{
			BlockFacing facing = BlockFacing.ALLFACES[i];
			int fx = dx + facing.Normali.X;
			int fy = y + facing.Normali.Y;
			int fz = dz + facing.Normali.Z;
			Block block = api.World.Blocks[chunks[fy / 32].Data.GetBlockIdUnsafe((32 * (fy % 32) + fz) * 32 + fx)];
			bool solid = block.BlockMaterial == EnumBlockMaterial.Stone;
			quantitySolid += (solid ? 1 : 0);
			quantityAir += ((block.BlockMaterial == EnumBlockMaterial.Air) ? 1 : 0);
			if (solid)
			{
				continue;
			}
			if (facing == BlockFacing.UP)
			{
				quantitySolid = 0;
			}
			else if (facing == BlockFacing.DOWN)
			{
				fy = y + 1;
				block = api.World.Blocks[chunks[fy / 32].Data.GetBlockIdUnsafe((32 * (fy % 32) + fz) * 32 + fx)];
				if (block.BlockMaterial != EnumBlockMaterial.Stone)
				{
					quantitySolid = 0;
				}
			}
		}
		if (quantitySolid != 5 || quantityAir != 1)
		{
			return;
		}
		BlockPos pos = new BlockPos(chunkX * 32 + dx, y, chunkZ * 32 + dz);
		for (int j = 0; j < structuresIntersectingChunk.Count; j++)
		{
			if (structuresIntersectingChunk[j].Contains(pos))
			{
				return;
			}
		}
		if (GetIntersectingStructure(pos.X, pos.Z, ModStdWorldGen.RivuletsHashCode) == null)
		{
			IServerChunk chunk = chunks[y / 32];
			int index = (32 * (y % 32) + dz) * 32 + dx;
			if (api.World.GetBlock(chunk.Data.GetBlockId(index, 1)).EntityClass != null)
			{
				chunk.RemoveBlockEntity(pos);
			}
			chunk.Data.SetBlockAir(index);
			chunk.Data.SetFluid(index, ((float)y < geoActivityYThreshold) ? gcfg.lavaBlockId : gcfg.waterBlockId);
			blockAccessor.ScheduleBlockUpdate(pos);
		}
	}

	private void tryGenMountainSideRivers(IServerChunk[] chunks, int chunkX, int chunkZ, int liquidBlockID)
	{
		IMapChunk mapChunk = chunks[0].MapChunk;
		float minY = (float)TerraGenConfig.seaLevel * 1.1f + 10f;
		int dx = 4 + rnd.NextInt(24);
		int dz = 4 + rnd.NextInt(24);
		int y = mapChunk.WorldGenTerrainHeightMap[dz * 32 + dx] - 2;
		if ((float)y < minY || (liquidBlockID == gcfg.rivuletWaterBlockId && y - TerraGenConfig.seaLevel > 20))
		{
			return;
		}
		Vec3i dir = null;
		int len = 0;
		for (int i = 0; i < BlockFacing.HORIZONTALS.Length; i++)
		{
			BlockFacing facing = BlockFacing.HORIZONTALS[i];
			int fx = dx + facing.Normali.X;
			int fy = y;
			int fz = dz + facing.Normali.Z;
			Block block = api.World.Blocks[chunks[fy / 32].Data.GetBlockIdUnsafe((32 * (fy % 32) + fz) * 32 + fx)];
			if (!block.SideSolid.All)
			{
				return;
			}
			if (dir != null)
			{
				continue;
			}
			for (len = 1; len <= 4; len++)
			{
				fx = dx + facing.Normali.X * len;
				fy = y;
				fz = dz + facing.Normali.Z * len;
				block = api.World.Blocks[chunks[fy / 32].Data.GetBlockIdUnsafe((32 * (fy % 32) + fz) * 32 + fx)];
				bool belowSolid = api.World.Blocks[chunks[(fy - 1) / 32].Data.GetBlockIdUnsafe((32 * ((fy - 1) % 32) + fz) * 32 + fx)].SideSolid.All;
				bool solid = block.SideSolid.All;
				if ((len < 2 && !solid) || !belowSolid)
				{
					break;
				}
				if (len >= 2 && block.BlockMaterial == EnumBlockMaterial.Air)
				{
					dir = facing.Normali;
					break;
				}
			}
		}
		if (dir == null)
		{
			return;
		}
		for (int k = 0; k <= len; k++)
		{
			BlockPos pos = new BlockPos(chunkX * 32 + dx + k * dir.X, y, chunkZ * 32 + dz + k * dir.Z);
			for (int j = 0; j < structuresIntersectingChunk.Count; j++)
			{
				if (structuresIntersectingChunk[j].Contains(pos))
				{
					return;
				}
			}
			if (GetIntersectingStructure(pos.X, pos.Z, ModStdWorldGen.RivuletsHashCode) != null)
			{
				return;
			}
		}
		for (int l = 0; l <= len; l++)
		{
			BlockPos pos2 = new BlockPos(chunkX * 32 + dx + l * dir.X, y, chunkZ * 32 + dz + l * dir.Z);
			blockAccessor.SetBlock(0, pos2);
			if (l == 0)
			{
				blockAccessor.SetBlock(liquidBlockID, pos2, 2);
				blockAccessor.ScheduleBlockUpdate(pos2);
			}
		}
	}

	private int getGeologicActivity(int posx, int posz)
	{
		IntDataMap2D climateMap = blockAccessor.GetMapRegion(posx / regionsize, posz / regionsize)?.ClimateMap;
		if (climateMap == null)
		{
			return 0;
		}
		int regionChunkSize = regionsize / 32;
		float fac = (float)climateMap.InnerSize / (float)regionChunkSize;
		int rlX = posx / 32 % regionChunkSize;
		int rlZ = posz / 32 % regionChunkSize;
		return climateMap.GetUnpaddedInt((int)((float)rlX * fac), (int)((float)rlZ * fac)) & 0xFF;
	}
}
