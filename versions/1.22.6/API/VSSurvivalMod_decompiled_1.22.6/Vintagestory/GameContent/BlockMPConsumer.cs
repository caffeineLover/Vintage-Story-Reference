using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent.Mechanics;

namespace Vintagestory.GameContent;

public class BlockMPConsumer : BlockMPBase
{
	public BlockFacing Facing;

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		Facing = BlockFacing.FromCode(Variant["side"]);
	}

	public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
	{
		bool num = base.TryPlaceBlock(world, byPlayer, itemstack, blockSel, ref failureCode);
		if (num)
		{
			tryConnect(face: BlockFacing.FromCode(api.World.BlockAccessor.GetBlock(blockSel.Position).Variant["side"]), world: world, byPlayer: byPlayer, pos: blockSel.Position);
		}
		return num;
	}

	public override bool HasMechPowerConnectorAt(IWorldAccessor world, BlockPos pos, BlockFacing face, BlockMPBase forblock)
	{
		return face == Facing;
	}

	public override void DidConnectAt(IWorldAccessor world, BlockPos pos, BlockFacing face)
	{
	}
}
