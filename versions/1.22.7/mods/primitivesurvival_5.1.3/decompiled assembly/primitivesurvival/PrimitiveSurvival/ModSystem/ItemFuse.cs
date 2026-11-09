using System;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000046 RID: 70
	public class ItemFuse : Item
	{
		// Token: 0x060002BB RID: 699 RVA: 0x0001FCF9 File Offset: 0x0001DEF9
		public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handling)
		{
			handling = 3;
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0001FD00 File Offset: 0x0001DF00
		public override void OnHeldInteractStop(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
		{
			if (blockSel == null || byEntity == null)
			{
				return;
			}
			IWorldAccessor world = byEntity.World;
			if (world == null)
			{
				return;
			}
			Block block = world.BlockAccessor.GetBlock(blockSel.Position, 0);
			string face = blockSel.Face.ToString();
			Block blockNew = world.GetBlock(new AssetLocation("primitivesurvival:bfuse-blackmatch-empty"));
			if (blockNew == null)
			{
				return;
			}
			if (face == "down")
			{
				BlockSelection blockSelBelow = blockSel.Clone();
				blockSelBelow.Position.Y--;
				Block blockAbove = world.BlockAccessor.GetBlock(blockSelBelow.Position, 0);
				if (blockAbove.BlockId == 0 || blockAbove.Code.Path.Contains("tallgrass-") || block.Code.Path.Contains("tallgrass-"))
				{
					IBlockAccessor blockAccessor = world.BlockAccessor;
					if (block.Code.Path.Contains("tallgrass-"))
					{
						blockAccessor.SetBlock(blockNew.BlockId, blockSel.Position);
						blockAccessor.TriggerNeighbourBlockUpdate(blockSel.Position);
					}
					else
					{
						blockAccessor.SetBlock(blockNew.BlockId, blockSelBelow.Position);
						blockAccessor.TriggerNeighbourBlockUpdate(blockSelBelow.Position);
					}
					slot.TakeOut(1);
					slot.MarkDirty();
					return;
				}
			}
			else if (face == "up")
			{
				if (block.FirstCodePart(0) == "bfuse")
				{
					return;
				}
				BlockSelection blockSelAbove = blockSel.Clone();
				blockSelAbove.Position.Y++;
				Block blockAbove2 = world.BlockAccessor.GetBlock(blockSelAbove.Position, 0);
				if (blockAbove2.BlockId == 0 || blockAbove2.Code.Path.Contains("tallgrass-") || block.Code.Path.Contains("tallgrass-"))
				{
					IBlockAccessor blockAccessor2 = world.BlockAccessor;
					BlockPos posFinal = blockSelAbove.Position;
					BlockSelection neib = blockSelAbove.Clone();
					if (block.Code.Path.Contains("tallgrass-"))
					{
						posFinal = blockSel.Position;
						neib = blockSel.Clone();
					}
					blockAccessor2.SetBlock(blockNew.BlockId, posFinal);
					blockAccessor2.TriggerNeighbourBlockUpdate(posFinal);
					neib.Position.Z++;
					world.BlockAccessor.GetBlock(neib.Position, 0);
					blockAccessor2.TriggerNeighbourBlockUpdate(neib.Position);
					slot.TakeOut(1);
					slot.MarkDirty();
					return;
				}
			}
			else if (block.Code.Path.Contains("tallgrass-"))
			{
				IBlockAccessor blockAccessor3 = world.BlockAccessor;
				blockAccessor3.SetBlock(blockNew.BlockId, blockSel.Position);
				blockAccessor3.TriggerNeighbourBlockUpdate(blockSel.Position);
				BlockSelection neib2 = blockSel.Clone();
				neib2.Position.Z++;
				world.BlockAccessor.GetBlock(neib2.Position, 0);
				blockAccessor3.TriggerNeighbourBlockUpdate(neib2.Position);
				slot.TakeOut(1);
				slot.MarkDirty();
			}
		}
	}
}
