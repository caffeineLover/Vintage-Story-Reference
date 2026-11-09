using System;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class BlockMeteorite : Block
{
	private BlockPos tmpPos = new BlockPos(0);

	public override bool TryPlaceBlockForWorldGen(IBlockAccessor blAcc, BlockPos pos, BlockFacing onBlockFace, IRandom worldgenRand, BlockPatchAttributes attributes = null)
	{
		int cnt = 2 + worldgenRand.NextInt(25);
		float depth = GameMath.Sqrt(GameMath.Sqrt(cnt));
		float craterRadius = GameMath.Sqrt(cnt) * 1.25f;
		if (pos.Y > 250 || !IsSolid(blAcc, pos.X, pos.Y - 3, pos.Z) || !IsSolid(blAcc, pos.X, pos.Y - (int)depth, pos.Z) || !IsSolid(blAcc, pos.X + (int)craterRadius, pos.Y - 1, pos.Z) || !IsSolid(blAcc, pos.X - (int)craterRadius, pos.Y - 1, pos.Z) || !IsSolid(blAcc, pos.X, pos.Y - 1, pos.Z - (int)craterRadius) || !IsSolid(blAcc, pos.X, pos.Y - 1, pos.Z + (int)craterRadius))
		{
			return false;
		}
		int y1 = blAcc.GetTerrainMapheightAt(tmpPos.Set(pos.X - 5, pos.Y, pos.Z));
		int y2 = blAcc.GetTerrainMapheightAt(tmpPos.Set(pos.X + 5, pos.Y, pos.Z));
		int y3 = blAcc.GetTerrainMapheightAt(tmpPos.Set(pos.X, pos.Y, pos.Z + 5));
		int y4 = blAcc.GetTerrainMapheightAt(tmpPos.Set(pos.X, pos.Y, pos.Z - 5));
		if (GameMath.Max(y1, y2, y3, y4) - GameMath.Min(y1, y2, y3, y4) > 4)
		{
			return false;
		}
		tmpPos = tmpPos.Set(pos.X, pos.Y - (int)depth - 2, pos.Z);
		while (cnt-- > 0)
		{
			tmpPos.X += ((worldgenRand.NextInt(3) == 0) ? (worldgenRand.NextInt(3) - 1) : 0);
			tmpPos.Y += ((worldgenRand.NextInt(8) == 0) ? (worldgenRand.NextInt(3) - 1) : 0);
			tmpPos.Z += ((worldgenRand.NextInt(3) == 0) ? (worldgenRand.NextInt(3) - 1) : 0);
			blAcc.SetBlock(BlockId, tmpPos);
		}
		int sueviteBlockId = api.World.GetBlock(new AssetLocation("rock-suevite")).BlockId;
		int fragmentBlockId = api.World.GetBlock(new AssetLocation("loosestones-meteorite-iron-free")).BlockId;
		int looseSueviteBlockId = api.World.GetBlock(new AssetLocation("loosestones-suevite-free")).BlockId;
		float impactRockRadius = craterRadius * 1.2f;
		int range = (int)Math.Ceiling(impactRockRadius);
		Vec2i vecTmp = new Vec2i();
		for (int dx = -range; dx <= range; dx++)
		{
			for (int dz = -range; dz <= range; dz++)
			{
				float distImpactRockEdge = (float)(dx * dx + dz * dz) / (impactRockRadius * impactRockRadius);
				if (distImpactRockEdge > 1f)
				{
					continue;
				}
				tmpPos.X = pos.X + dx;
				tmpPos.Z = pos.Z + dz;
				int surfaceY = blAcc.GetTerrainMapheightAt(tmpPos);
				tmpPos.Y = surfaceY - (int)depth;
				vecTmp.X = tmpPos.X / 32;
				vecTmp.Y = tmpPos.Z / 32;
				IMapChunk mapchunk = blAcc.GetMapChunk(vecTmp);
				float q = 3f * Math.Max(0f, 2f * (1f - distImpactRockEdge) - 0.2f);
				tmpPos.Y -= (int)q + 1;
				while (q > 0f && (!(q < 1f) || !(worldgenRand.NextDouble() > (double)q)))
				{
					Block block = blAcc.GetBlock(tmpPos);
					if (block != this && block.BlockMaterial == EnumBlockMaterial.Stone)
					{
						blAcc.SetBlock(sueviteBlockId, tmpPos);
					}
					q -= 1f;
					tmpPos.Y++;
				}
				float distToCraterEdge = (float)(dx * dx + dz * dz) / (craterRadius * craterRadius) + (float)worldgenRand.NextDouble() * 0.1f;
				if (distToCraterEdge > 1f)
				{
					continue;
				}
				q = depth * (1f - distToCraterEdge);
				tmpPos.Y = surfaceY;
				ItemStack surfaceBEStack;
				TreeAttribute surfaceBETree;
				Block surfaceblock = GetBlockAndBEdata(blAcc, tmpPos, out surfaceBEStack, out surfaceBETree);
				tmpPos.Y++;
				ItemStack aboveBEStack;
				TreeAttribute aboveBETree;
				Block abovesurfaceblock = GetBlockAndBEdata(blAcc, tmpPos, out aboveBEStack, out aboveBETree);
				tmpPos.Y++;
				ItemStack above2BEStack;
				TreeAttribute above2BETree;
				Block above2surfaceblock = GetBlockAndBEdata(blAcc, tmpPos, out above2BEStack, out above2BETree);
				for (int i = -2; i <= (int)q; i++)
				{
					tmpPos.Y = surfaceY - i;
					int id = ((i == (int)q) ? surfaceblock.BlockId : 0);
					if (!blAcc.GetBlock(tmpPos, 2).IsLiquid())
					{
						blAcc.SetBlock(id, tmpPos);
						if (id > 0)
						{
							MaybeSpawnBlockEntity(surfaceblock, blAcc, tmpPos, surfaceBEStack, surfaceBETree);
						}
					}
				}
				mapchunk.WorldGenTerrainHeightMap[tmpPos.Z % 32 * 32 + tmpPos.X % 32] -= (ushort)q;
				tmpPos.Y = blAcc.GetTerrainMapheightAt(tmpPos) + 1;
				if (abovesurfaceblock.BlockId > 0)
				{
					blAcc.SetBlock(abovesurfaceblock.BlockId, tmpPos);
					MaybeSpawnBlockEntity(abovesurfaceblock, blAcc, tmpPos, aboveBEStack, aboveBETree);
				}
				tmpPos.Y++;
				if (above2surfaceblock.BlockId > 0)
				{
					blAcc.SetBlock(above2surfaceblock.BlockId, tmpPos);
					MaybeSpawnBlockEntity(above2surfaceblock, blAcc, tmpPos, above2BEStack, above2BETree);
				}
			}
		}
		int quantityFragments = 0;
		if (worldgenRand.NextInt(10) == 0)
		{
			quantityFragments = worldgenRand.NextInt(10);
		}
		else if (worldgenRand.NextInt(5) == 0)
		{
			quantityFragments = worldgenRand.NextInt(5);
		}
		while (quantityFragments-- > 0)
		{
			tmpPos.Set(pos.X + (worldgenRand.NextInt(11) + worldgenRand.NextInt(11)) / 2 - 5, 0, pos.Z + (worldgenRand.NextInt(11) + worldgenRand.NextInt(11)) / 2 - 5);
			tmpPos.Y = blAcc.GetTerrainMapheightAt(tmpPos) + 1;
			if (blAcc.IsSideSolid(tmpPos.X, tmpPos.Y - 1, tmpPos.Z, BlockFacing.UP))
			{
				if (worldgenRand.NextDouble() < 0.3)
				{
					blAcc.SetBlock(fragmentBlockId, tmpPos);
				}
				else
				{
					blAcc.SetBlock(looseSueviteBlockId, tmpPos);
				}
			}
		}
		return true;
	}

	private Block GetBlockAndBEdata(IBlockAccessor blAcc, BlockPos pos, out ItemStack BEStack, out TreeAttribute BETree)
	{
		BEStack = null;
		BETree = null;
		Block block = blAcc.GetBlock(pos, 1);
		try
		{
			if (block.EntityClass != null)
			{
				BlockEntity be = blAcc.GetBlockEntity(pos);
				if (be != null)
				{
					BEStack = be.stackForWorldgen;
					if (BEStack == null)
					{
						BETree = new TreeAttribute();
						be.ToTreeAttributes(BETree);
					}
				}
				if (BEStack == null && BETree == null)
				{
					BEStack = block.OnPickBlock(api.World, pos);
				}
			}
		}
		catch
		{
			BEStack = null;
			BETree = null;
		}
		return block;
	}

	private void MaybeSpawnBlockEntity(Block block, IBlockAccessor blAcc, BlockPos pos, ItemStack BEStack, TreeAttribute BETree)
	{
		if (block.EntityClass == null)
		{
			return;
		}
		try
		{
			blAcc.SpawnBlockEntity(block.EntityClass, pos, BEStack);
			if (BETree != null)
			{
				BlockEntity blockEntity = blAcc.GetBlockEntity(pos);
				BETree.SetInt("posy", pos.Y);
				blockEntity?.FromTreeAttributes(BETree, api.World);
			}
		}
		catch (Exception e)
		{
			api.Logger.Error(e);
		}
	}

	private bool IsSolid(IBlockAccessor blAcc, int x, int y, int z)
	{
		return blAcc.IsSideSolid(x, y, z, BlockFacing.UP);
	}
}
