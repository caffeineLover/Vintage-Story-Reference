using Vintagestory.API;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent.Mechanics;

[DocumentAsJson]
public class BlockBehaviorCoverable : StrongBlockBehavior
{
	public BlockBehaviorCoverable(Block block)
		: base(block)
	{
	}

	public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling)
	{
		ItemSlot hslot = byPlayer.InventoryManager.ActiveHotbarSlot;
		if (byPlayer.Entity.Controls.CtrlKey)
		{
			ItemStack? itemstack = byPlayer.InventoryManager.OffhandHotbarSlot.Itemstack;
			if (itemstack != null && itemstack.Collectible.GetTool(byPlayer.InventoryManager.OffhandHotbarSlot) == EnumTool.Wrench)
			{
				if (BlockEntityBehaviorCoverable.SuitableMaterial(hslot))
				{
					block.GetBEBehavior<BlockEntityBehaviorCoverable>(blockSel.Position).TryAddMaterial(byPlayer, blockSel);
				}
				else if (hslot.Empty)
				{
					(world.Api as ICoreClientAPI)?.TriggerIngameError(this, "unsuitablematerial", Lang.Get("Put suitable block material in your hands"));
				}
				else
				{
					(world.Api as ICoreClientAPI)?.TriggerIngameError(this, "unsuitablematerial", Lang.Get("Unsuitable block material for axle coverage"));
				}
				handling = EnumHandling.PreventDefault;
				return true;
			}
		}
		return base.OnBlockInteractStart(world, byPlayer, blockSel, ref handling);
	}

	public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer, ref EnumHandling handling)
	{
		if (BlockBehaviorWrenchOrientable.wrenchItems == null)
		{
			BlockBehaviorWrenchOrientable.loadWrenchItems(world);
		}
		return new WorldInteraction[1]
		{
			new WorldInteraction
			{
				HotKeyCode = "ctrl",
				MouseButton = EnumMouseButton.Right,
				Itemstacks = BlockBehaviorWrenchOrientable.wrenchItems,
				GetMatchingStacks = (WorldInteraction wi, BlockSelection bs, EntitySelection es) => BlockBehaviorWrenchOrientable.wrenchItems,
				ActionLangCode = "Add block covering"
			}
		};
	}

	public override int GetPlacedBlockInteractionHelpCount(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer, ref EnumHandling handling)
	{
		return 1;
	}

	public override bool SideIsSolid(BlockPos pos, int faceIndex, ref EnumHandling handling)
	{
		BlockEntityBehaviorCoverable coverable = block.GetBEBehavior<BlockEntityBehaviorCoverable>(pos);
		if (coverable != null && coverable.WallStack?.Block != null)
		{
			handling = EnumHandling.PreventSubsequent;
			return true;
		}
		return base.SideIsSolid(pos, faceIndex, ref handling);
	}

	public override bool SideIsSolid(IBlockAccessor blockAccess, BlockPos pos, int faceIndex, ref EnumHandling handling)
	{
		BlockEntityBehaviorCoverable coverable = block.GetBEBehavior<BlockEntityBehaviorCoverable>(pos);
		if (coverable != null && coverable.WallStack?.Block != null)
		{
			handling = EnumHandling.PreventSubsequent;
			return true;
		}
		return base.SideIsSolid(blockAccess, pos, faceIndex, ref handling);
	}

	public override int GetRetention(BlockPos pos, BlockFacing facing, EnumRetentionType type, ref EnumHandling handling)
	{
		BlockEntityBehaviorCoverable bebehavior = block.GetBEBehavior<BlockEntityBehaviorCoverable>(pos);
		if (bebehavior?.WallStack?.Collectible is Block wallBlock)
		{
			if (type == EnumRetentionType.Sound)
			{
				return 10;
			}
			EnumBlockMaterial mat = wallBlock.GetBlockMaterial(bebehavior.Api.World.BlockAccessor, pos);
			handling = EnumHandling.PreventSubsequent;
			bool flag = (uint)(mat - 6) <= 1u;
			if (flag || mat == EnumBlockMaterial.Soil || mat == EnumBlockMaterial.Ceramic)
			{
				return -1;
			}
			return 1;
		}
		return base.GetRetention(pos, facing, type, ref handling);
	}

	public override Cuboidf[] GetCollisionBoxes(IBlockAccessor blockAccessor, BlockPos pos, ref EnumHandling handling)
	{
		if (block.GetBEBehavior<BlockEntityBehaviorCoverable>(pos)?.WallStack != null)
		{
			handling = EnumHandling.PreventSubsequent;
			return Block.DefaultCollisionSelectionBoxes;
		}
		return base.GetCollisionBoxes(blockAccessor, pos, ref handling);
	}

	public override Cuboidf[] GetSelectionBoxes(IBlockAccessor blockAccessor, BlockPos pos, ref EnumHandling handling)
	{
		if (block.GetBEBehavior<BlockEntityBehaviorCoverable>(pos)?.WallStack != null)
		{
			handling = EnumHandling.PreventSubsequent;
			return Block.DefaultCollisionSelectionBoxes;
		}
		return base.GetSelectionBoxes(blockAccessor, pos, ref handling);
	}

	public override bool CanAttachBlockAt(IBlockAccessor world, Block block, BlockPos pos, BlockFacing blockFace, ref EnumHandling handling, Cuboidi attachmentArea = null)
	{
		if (block.GetBEBehavior<BlockEntityBehaviorCoverable>(pos)?.WallStack != null)
		{
			handling = EnumHandling.PreventSubsequent;
			return true;
		}
		return base.CanAttachBlockAt(world, block, pos, blockFace, ref handling, attachmentArea);
	}

	public override float GetLiquidBarrierHeightOnSide(BlockFacing face, BlockPos pos, ref EnumHandling handling)
	{
		if (block.GetBEBehavior<BlockEntityBehaviorCoverable>(pos)?.WallStack != null)
		{
			handling = EnumHandling.PreventSubsequent;
			return 1f;
		}
		return base.GetLiquidBarrierHeightOnSide(face, pos, ref handling);
	}

	public override int GetLightAbsorption(IBlockAccessor blockAccessor, BlockPos pos, ref EnumHandling handling)
	{
		if (block.GetBEBehavior<BlockEntityBehaviorCoverable>(pos)?.WallStack?.Collectible is Block wallBlock)
		{
			handling = EnumHandling.PreventSubsequent;
			return wallBlock.GetLightAbsorption(blockAccessor, pos);
		}
		return base.GetLightAbsorption(blockAccessor, pos, ref handling);
	}

	public override int GetLightAbsorption(IWorldChunk chunk, BlockPos pos, ref EnumHandling handling)
	{
		if (block.GetBEBehavior<BlockEntityBehaviorCoverable>(pos)?.WallStack?.Collectible is Block wallBlock)
		{
			handling = EnumHandling.PreventSubsequent;
			return wallBlock.GetLightAbsorption(chunk, pos);
		}
		return base.GetLightAbsorption(chunk, pos, ref handling);
	}
}
