using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace Vintagestory.GameContent;

public class BlockFarmland : Block
{
	public override void OnBlockPlaced(IWorldAccessor world, BlockPos blockPos, ItemStack byItemStack = null)
	{
		base.OnBlockPlaced(world, blockPos, byItemStack);
		if (byItemStack != null)
		{
			(world.BlockAccessor.GetBlockEntity(blockPos) as BlockEntityFarmland)?.OnCreatedFromSoil(byItemStack.Block);
		}
	}

	public override bool CanAttachBlockAt(IBlockAccessor world, Block block, BlockPos pos, BlockFacing blockFace, Cuboidi attachmentArea = null)
	{
		if ((block is BlockCrop || block is BlockDeadCrop) && blockFace == BlockFacing.UP)
		{
			return true;
		}
		if (blockFace.IsHorizontal)
		{
			return false;
		}
		return base.CanAttachBlockAt(world, block, pos, blockFace, attachmentArea);
	}

	public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
	{
		if (world.BlockAccessor.GetBlockEntity(blockSel.Position) is BlockEntityFarmland befarmland && befarmland.OnBlockInteract(byPlayer))
		{
			return true;
		}
		return base.OnBlockInteractStart(world, byPlayer, blockSel);
	}

	public override string GetPlacedBlockName(IWorldAccessor world, BlockPos pos)
	{
		if (world.BlockAccessor.GetBlockEntity(pos) is BlockEntityFarmland befarmland)
		{
			return new ItemStack(api.World.GetBlock(CodeWithVariant("state", befarmland.IsVisiblyMoist ? "moist" : "dry"))).GetName();
		}
		return base.GetPlacedBlockName(world, pos);
	}

	public override int GetRetention(BlockPos pos, BlockFacing facing, EnumRetentionType type)
	{
		if (facing != BlockFacing.UP)
		{
			return 3;
		}
		return 0;
	}

	public override bool SideIsSolid(BlockPos pos, int faceIndex)
	{
		return faceIndex == 5;
	}

	public override void OnBeingLookedAt(IPlayer byPlayer, BlockSelection blockSel, bool firstTick)
	{
		if (firstTick && api is ICoreServerAPI sapi)
		{
			(sapi.World.BlockAccessor.GetBlockEntity(blockSel.Position) as BlockEntityFarmland)?.SendFullUpdateToClient(sapi, (IServerPlayer)byPlayer);
		}
	}
}
