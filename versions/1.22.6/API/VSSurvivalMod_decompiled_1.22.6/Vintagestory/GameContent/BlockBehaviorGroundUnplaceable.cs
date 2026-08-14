using Vintagestory.API;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

[DocumentAsJson]
public class BlockBehaviorGroundUnplaceable : BlockBehavior
{
	public BlockBehaviorGroundUnplaceable(Block block)
		: base(block)
	{
	}

	public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref EnumHandling handling, ref string failureCode)
	{
		if (blockSel.Face == BlockFacing.UP)
		{
			handling = EnumHandling.PreventSubsequent;
			failureCode = "__ignore__";
			return false;
		}
		return base.TryPlaceBlock(world, byPlayer, itemstack, blockSel, ref handling, ref failureCode);
	}

	public override bool CanPlaceBlock(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling, ref string failureCode)
	{
		if (blockSel.Face == BlockFacing.UP)
		{
			handling = EnumHandling.PreventSubsequent;
			failureCode = "__ignore__";
			return false;
		}
		return true;
	}
}
