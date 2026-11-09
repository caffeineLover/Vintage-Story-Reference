using System;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class BlockSnowLayer : BlockLayered
{
	private int height;

	private bool canMelt;

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		height = Variant["height"].ToInt();
		snowLevel = height;
		notSnowCovered = api.World.GetBlock(0);
		snowCovered1 = api.World.GetBlock(CodeWithVariant("height", "1"));
		snowCovered2 = api.World.GetBlock(CodeWithVariant("height", "2"));
		snowCovered3 = api.World.GetBlock(CodeWithVariant("height", "3"));
		canMelt = api.World.Config.GetBool("snowAccum", defaultValue: true);
	}

	public override bool ShouldReceiveServerGameTicks(IWorldAccessor world, BlockPos pos, Random offThreadRandom, out object extra)
	{
		extra = null;
		if (!canMelt)
		{
			return false;
		}
		ClimateCondition conds = world.BlockAccessor.GetClimateAt(pos);
		if (conds != null)
		{
			return offThreadRandom.NextDouble() < (double)GameMath.Clamp((conds.Temperature - 0.5f) / (15f - 10f * conds.Rainfall), 0f, 1f);
		}
		return false;
	}

	public override void OnServerGameTick(IWorldAccessor world, BlockPos pos, object extra = null)
	{
		if (height == 1)
		{
			world.BlockAccessor.SetBlock(0, pos);
			return;
		}
		Block block = world.GetBlock(CodeWithVariant("height", (height - 1).ToString() ?? ""));
		world.BlockAccessor.SetBlock(block.Id, pos);
	}

	public override Block GetSnowCoveredVariant(BlockPos pos, float snowLevel)
	{
		if ((int)snowLevel <= 0)
		{
			return api.World.Blocks[0];
		}
		if ((int)snowLevel < 8)
		{
			return api.World.GetBlock(CodeWithVariant("height", ((int)snowLevel).ToString() ?? ""));
		}
		if ((int)snowLevel >= 8)
		{
			return api.World.GetBlock("snowblock");
		}
		return null;
	}

	public override bool CanAcceptFallOnto(IWorldAccessor world, BlockPos pos, Block fallingBlock, TreeAttribute blockEntityAttributes)
	{
		if (fallingBlock is BlockSnowLayer || fallingBlock is BlockSnow)
		{
			if (world.BlockAccessor.GetMostSolidBlock(pos) is BlockSnowLayer ourBlock)
			{
				return ourBlock.height < 8;
			}
			return false;
		}
		return false;
	}

	public override bool OnFallOnto(IWorldAccessor world, BlockPos pos, Block block, TreeAttribute blockEntityAttributes)
	{
		Block targetPosBlock = world.BlockAccessor.GetMostSolidBlock(pos);
		Block fallingBlock = block;
		if (fallingBlock != null && targetPosBlock != null && targetPosBlock.GetSnowLevel(pos) < 8f)
		{
			FastVec3i originalPos = new FastVec3i(pos);
			while (fallingBlock is BlockSnowLayer && targetPosBlock.GetSnowLevel(pos) < 8f)
			{
				Block nextnBlock = targetPosBlock.GetSnowCoveredVariant(pos, targetPosBlock.GetSnowLevel(pos) + 1f);
				if (nextnBlock == null)
				{
					break;
				}
				targetPosBlock = nextnBlock;
				fallingBlock = fallingBlock.GetSnowCoveredVariant(pos, fallingBlock.GetSnowLevel(pos) - 1f);
			}
			int downId = 0;
			while (downId == 0)
			{
				downId = world.BlockAccessor.GetMostSolidBlock(pos.Down()).BlockId;
				if (downId != 0)
				{
					pos.Up();
				}
			}
			world.BlockAccessor.SetBlock(targetPosBlock.BlockId, pos);
			if (fallingBlock != null)
			{
				BlockPos upos = pos.UpCopy();
				Block aboveBlock = world.BlockAccessor.GetMostSolidBlock(upos);
				if (aboveBlock.BlockId == 0)
				{
					world.BlockAccessor.SetBlock(fallingBlock.BlockId, upos);
				}
				else if (!originalPos.Equals(pos))
				{
					aboveBlock.OnFallOnto(world, pos, fallingBlock, blockEntityAttributes);
				}
			}
			world.BlockAccessor.TriggerNeighbourBlockUpdate(pos);
			return true;
		}
		return base.OnFallOnto(world, pos, block, blockEntityAttributes);
	}
}
