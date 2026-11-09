using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.Common;

namespace Vintagestory.Client.NoObf;

internal class ClientChunkData : ChunkData
{
	private int[][] light2;

	private FastRWLock light2Lock;

	private int blocksArrayCount;

	private Block blockAir;

	private System.Func<int, Block> GetBlockAsBlock;

	private ClientChunkData(ChunkDataPool chunkDataPool)
		: base(chunkDataPool)
	{
		GetBlockAsBlock = getBlockAir;
		light2Lock = new FastRWLock(chunkDataPool);
	}

	public new static ClientChunkData CreateNew(int chunksize, ChunkDataPool chunkDataPool)
	{
		return new ClientChunkData(chunkDataPool);
	}

	public void BuildFastBlockAccessArray(Block[] blocks)
	{
		int count;
		if (blocksLayer != null && (count = blocksLayer.paletteCount) > 0)
		{
			int[] bp = blocksLayer.palette;
			GetBlockAsBlock = blocksLayer.SelectDelegateBlockClient(getBlockAir);
			if (BlockChunkDataLayer.blocksByPaletteIndex == null || BlockChunkDataLayer.blocksByPaletteIndex.Length < count)
			{
				BlockChunkDataLayer.blocksByPaletteIndex = new Block[count];
			}
			for (int i = 0; i < count; i++)
			{
				BlockChunkDataLayer.blocksByPaletteIndex[i] = blocks[bp[i]];
			}
			blocksArrayCount = count;
		}
		else
		{
			GetBlockAsBlock = getBlockAir;
		}
		blockAir = blocks[0];
	}

	protected Block getBlockAir(int index3d)
	{
		return blockAir;
	}

	public int GetOne(out ushort lightOut, out int lightSatOut, out int fluidBlockId, int index3d)
	{
		light2Lock.AcquireReadLock();
		uint l = ((light2 != null) ? Light2(index3d) : Light(index3d));
		light2Lock.ReleaseReadLock();
		lightOut = (ushort)l;
		lightSatOut = (int)((l >> 16) & 7);
		fluidBlockId = GetFluid(index3d);
		return GetSolidBlock(index3d);
	}

	public void GetRange(Block[] currentChunkBlocksExt, Block[] currentChunkFluidsExt, int[] currentChunkRgbsExt, int extIndex3d, int index3d, int index3dEnd, Block[] blocksFast, ColorUtil.LightUtil lightConverter)
	{
		BlockChunkDataLayer bl = blocksLayer;
		if (bl == null)
		{
			blockAir = blocksFast[0];
			light2Lock.AcquireReadLock();
			try
			{
				do
				{
					uint l = ((light2 != null) ? Light2(index3d) : Light(index3d));
					currentChunkBlocksExt[++extIndex3d] = blockAir;
					currentChunkRgbsExt[extIndex3d] = lightConverter.ToRgba((ushort)l, (int)((l >> 16) & 7));
					int blockId = GetFluid(index3d);
					currentChunkFluidsExt[extIndex3d] = blocksFast[blockId];
				}
				while (++index3d < index3dEnd);
				return;
			}
			finally
			{
				light2Lock.ReleaseReadLock();
			}
		}
		bl.readWriteLock.AcquireReadLock();
		light2Lock.AcquireReadLock();
		try
		{
			do
			{
				uint l2 = ((light2 != null) ? Light2(index3d) : Light(index3d));
				int blockId2 = bl.GetUnsafe(index3d);
				currentChunkBlocksExt[++extIndex3d] = blocksFast[blockId2];
				currentChunkRgbsExt[extIndex3d] = lightConverter.ToRgba((ushort)l2, (int)((l2 >> 16) & 7));
				blockId2 = GetFluid(index3d);
				currentChunkFluidsExt[extIndex3d] = blocksFast[blockId2];
			}
			while (++index3d < index3dEnd);
		}
		finally
		{
			light2Lock.ReleaseReadLock();
			bl.readWriteLock.ReleaseReadLock();
		}
	}

	public void GetRange_Faster(Block[] currentChunkBlocksExt, Block[] currentChunkFluidsExt, int[] currentChunkRgbsExt, int extIndex3d, int index3d, int index3dEnd, Block[] blocksFast, ColorUtil.LightUtil lightConverter)
	{
		BlockChunkDataLayer bl = blocksLayer;
		if (bl == null)
		{
			light2Lock.AcquireReadLock();
			try
			{
				do
				{
					currentChunkBlocksExt[++extIndex3d] = blockAir;
					uint l = ((light2 != null) ? Light2(index3d) : Light(index3d));
					currentChunkRgbsExt[extIndex3d] = lightConverter.ToRgba((ushort)l, (int)((l >> 16) & 7));
					int fluidId = GetFluid(index3d);
					currentChunkFluidsExt[extIndex3d] = blocksFast[fluidId];
				}
				while (++index3d < index3dEnd);
				return;
			}
			finally
			{
				light2Lock.ReleaseReadLock();
			}
		}
		if (bl.paletteCount == blocksArrayCount)
		{
			bl.readWriteLock.AcquireReadLock();
			light2Lock.AcquireReadLock();
			try
			{
				do
				{
					currentChunkBlocksExt[++extIndex3d] = GetBlockAsBlock(index3d);
					uint l2 = ((light2 != null) ? Light2(index3d) : Light(index3d));
					currentChunkRgbsExt[extIndex3d] = lightConverter.ToRgba((ushort)l2, (int)((l2 >> 16) & 7));
					int fluidId2 = GetFluid(index3d);
					currentChunkFluidsExt[extIndex3d] = blocksFast[fluidId2];
				}
				while (++index3d < index3dEnd);
				return;
			}
			finally
			{
				light2Lock.ReleaseReadLock();
				bl.readWriteLock.ReleaseReadLock();
			}
		}
		GetRange(currentChunkBlocksExt, currentChunkFluidsExt, currentChunkRgbsExt, extIndex3d, index3d, index3dEnd, blocksFast, lightConverter);
	}

	internal override void EmptyAndReuseArrays(List<int[]> datas)
	{
		GetBlockAsBlock = getBlockAir;
		base.EmptyAndReuseArrays(datas);
		int[][] light2Copy = light2;
		if (light2Copy == null)
		{
			return;
		}
		light2Lock.AcquireWriteLock();
		light2 = null;
		for (int j = 0; j < light2Copy.Length; j++)
		{
			int[] lighting = light2Copy[j];
			if (lighting != null)
			{
				datas?.Add(lighting);
				light2Copy[j] = null;
			}
		}
		light2Lock.ReleaseWriteLock();
	}

	public override void SetSunlight_Buffered(int index3d, int sunLevel)
	{
		if (lightLayer == null)
		{
			lightLayer = new ChunkDataLayer(pool);
			lightLayer.Set(index3d, sunLevel);
			return;
		}
		if (light2 == null)
		{
			StartDoubleBuffering();
		}
		lightLayer.Set(index3d, (lightLayer.Get(index3d) & -32) | sunLevel);
	}

	public override void SetBlocklight_Buffered(int index3d, int lightLevel)
	{
		if (lightLayer == null)
		{
			lightLayer = new ChunkDataLayer(pool);
			lightLayer.Set(index3d, lightLevel);
			return;
		}
		if (light2 == null)
		{
			StartDoubleBuffering();
		}
		lightLayer.Set(index3d, (lightLayer.Get(index3d) & 0x1F) | lightLevel);
	}

	public uint Light2(int index3d)
	{
		int[] palette = lightLayer?.palette;
		if (index3d < 0 || palette == null)
		{
			return 0u;
		}
		int[][] light2Data = light2;
		int bitIndex = index3d % 32;
		index3d = index3d / 32 % 1024;
		int idx = 0;
		int bitValue = 1;
		for (int j = 0; j < light2Data.Length; j++)
		{
			idx += ((light2Data[j][index3d] >> bitIndex) & 1) * bitValue;
			bitValue *= 2;
		}
		return (uint)palette[idx];
	}

	private void StartDoubleBuffering()
	{
		light2 = lightLayer.CopyData();
	}

	public void FinishLightDoubleBuffering()
	{
		int[][] array = light2;
		if (array == null)
		{
			return;
		}
		light2Lock.AcquireWriteLock();
		light2 = null;
		for (int j = 0; j < array.Length; j++)
		{
			if (array[j] != null)
			{
				pool.Return(array[j]);
				array[j] = null;
			}
		}
		light2Lock.ReleaseWriteLock();
	}
}
