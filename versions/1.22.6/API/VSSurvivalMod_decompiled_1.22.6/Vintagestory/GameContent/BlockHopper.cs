using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class BlockHopper : Block, IBlockItemFlow
{
	public bool HasItemFlowConnectorAt(BlockFacing facing)
	{
		return facing == BlockFacing.DOWN;
	}

	public override void OnEntityCollide(IWorldAccessor world, Entity entity, BlockPos pos, BlockFacing facing, Vec3d collideSpeed, bool isImpact)
	{
		base.OnEntityCollide(world, entity, pos, facing, collideSpeed, isImpact);
		if (facing != BlockFacing.UP)
		{
			return;
		}
		EntityItem inWorldItem = entity as EntityItem;
		if (inWorldItem == null || world.Side != EnumAppSide.Server || world.Rand.NextDouble() < 0.9)
		{
			return;
		}
		BlockPos tmpPos = pos.Copy();
		api.Event.EnqueueMainThreadTask(delegate
		{
			BlockEntity blockEntity = world.BlockAccessor.GetBlockEntity(tmpPos);
			if (inWorldItem.Alive && blockEntity is BlockEntityItemFlow blockEntityItemFlow)
			{
				WeightedSlot bestSuitedSlot = blockEntityItemFlow.inventory.GetBestSuitedSlot(inWorldItem.Slot);
				if (bestSuitedSlot.slot != null)
				{
					inWorldItem.Slot.TryPutInto(api.World, bestSuitedSlot.slot);
					if (inWorldItem.Slot.StackSize <= 0)
					{
						inWorldItem.Itemstack = null;
						inWorldItem.Alive = false;
					}
				}
			}
		}, "hopperitempickup");
	}
}
