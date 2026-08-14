using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class BlockTileConnector : Block
{
	public override ItemStack OnPickBlock(IWorldAccessor world, BlockPos pos)
	{
		ItemStack stack = base.OnPickBlock(world, pos);
		BETileConnector be = world.BlockAccessor.GetBlockEntity<BETileConnector>(pos);
		if (be != null)
		{
			stack.Attributes["constraints"] = new StringAttribute(be.Target);
			stack.Attributes["direction"] = new IntAttribute(be.Direction.Index);
		}
		return stack;
	}

	public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
	{
		if (CanPlaceBlock(world, byPlayer, blockSel, ref failureCode))
		{
			world.BlockAccessor.SetBlock(BlockId, blockSel.Position, itemstack);
			GetBlockEntity<BETileConnector>(blockSel.Position).Direction = blockSel.Face;
			return true;
		}
		return false;
	}

	public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
	{
		GetBlockEntity<BETileConnector>(blockSel.Position)?.OnInteract(byPlayer);
		return true;
	}
}
