using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class BlockSnow : Block
{
	private Cuboidf[] fullBox = new Cuboidf[1]
	{
		new Cuboidf(0f, 0f, 0f, 1f, 1f, 1f)
	};

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		snowLevel = 8f;
	}

	public override Cuboidf[] GetCollisionBoxes(IBlockAccessor world, BlockPos pos)
	{
		if (world.GetBlockAbove(pos) is BlockLayered)
		{
			return fullBox;
		}
		return base.GetCollisionBoxes(world, pos);
	}

	public override bool ShouldMergeFace(int facingIndex, Block neighbourIce, int intraChunkIndex3d)
	{
		return true;
	}

	public override Block GetSnowCoveredVariant(BlockPos pos, float snowLevel)
	{
		if (snowLevel <= 0f)
		{
			return api.World.Blocks[0];
		}
		if ((int)snowLevel >= 8)
		{
			return this;
		}
		return api.World.GetBlock("snowlayer-" + (int)snowLevel);
	}
}
