using System;
using RPVoiceChat.GameContent.BlockEntity;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.GameContent.Block
{
	// Token: 0x020000D6 RID: 214
	public class RadioAntennaPartBlock : Block
	{
		// Token: 0x06000660 RID: 1632 RVA: 0x000229D0 File Offset: 0x00020BD0
		public override bool CanPlaceBlock(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref string failureCode)
		{
			BlockPos placePos = RadioAntennaPartBlock.ResolvePlacePos(world, blockSel);
			BlockSelection placeSel = blockSel.Clone();
			placeSel.Position = placePos;
			placeSel.DidOffset = false;
			return base.CanPlaceBlock(world, byPlayer, placeSel, ref failureCode) && RadioAntennaPartBlock.CanPlaceOnSupport(world, placePos.DownCopy(1), ref failureCode);
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x00022A18 File Offset: 0x00020C18
		public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
		{
			if (!this.CanPlaceBlock(world, byPlayer, blockSel, ref failureCode))
			{
				return false;
			}
			BlockPos placePos = RadioAntennaPartBlock.ResolvePlacePos(world, blockSel);
			BlockSelection placeSel = blockSel.Clone();
			placeSel.Position = placePos;
			placeSel.DidOffset = false;
			bool flag = this.DoPlaceBlock(world, byPlayer, placeSel, itemstack);
			if (flag)
			{
				BlockEntity blockEntity = world.BlockAccessor.GetBlockEntity(placePos.DownCopy(1));
				if (blockEntity == null)
				{
					return flag;
				}
				blockEntity.MarkDirty(true, null);
			}
			return flag;
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x00022A7F File Offset: 0x00020C7F
		public override bool CanAttachBlockAt(IBlockAccessor blockAccessor, Block block, BlockPos pos, BlockFacing blockFace, Cuboidi attachmentArea = null)
		{
			return (blockFace == BlockFacing.UP && RadioAntennaPartBlock.IsAntennaPartBlock(block)) || base.CanAttachBlockAt(blockAccessor, block, pos, blockFace, attachmentArea);
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x00022AA1 File Offset: 0x00020CA1
		public override void OnNeighbourBlockChange(IWorldAccessor world, BlockPos pos, BlockPos neibpos)
		{
			base.OnNeighbourBlockChange(world, pos, neibpos);
			if (neibpos.Y == pos.Y + 1)
			{
				BlockEntity blockEntity = world.BlockAccessor.GetBlockEntity(pos);
				if (blockEntity == null)
				{
					return;
				}
				blockEntity.MarkDirty(true, null);
			}
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x00022AD4 File Offset: 0x00020CD4
		private static bool IsAntennaPartBlock(Block block)
		{
			return block is RadioAntennaPartBlock || ((block != null) ? block.Class : null) == "radioantenna_partblock" || ((block != null) ? block.Class : null) == "radioantennapartblock";
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x00022B10 File Offset: 0x00020D10
		private static BlockPos ResolvePlacePos(IWorldAccessor world, BlockSelection blockSel)
		{
			BlockPos clicked = blockSel.DidOffset ? blockSel.Position.AddCopy(blockSel.Face.Opposite) : blockSel.Position;
			if (RadioAntennaPartBlock.IsAntennaSupport(world, clicked))
			{
				return RadioAntennaPartBlock.FindStackTip(world, clicked).UpCopy(1);
			}
			BlockPos below = blockSel.Position.DownCopy(1);
			if (world.BlockAccessor.GetBlock(blockSel.Position).Replaceable >= 6000 && RadioAntennaPartBlock.IsAntennaSupport(world, below))
			{
				return RadioAntennaPartBlock.FindStackTip(world, below).UpCopy(1);
			}
			if (world.BlockAccessor.GetBlock(blockSel.Position).Replaceable < 6000)
			{
				return blockSel.Position.AddCopy(blockSel.Face);
			}
			return blockSel.Position.Copy();
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x00022BD8 File Offset: 0x00020DD8
		private static BlockPos FindStackTip(IWorldAccessor world, BlockPos supportOrPart)
		{
			BlockPos tip = supportOrPart.Copy();
			while (RadioAntennaPartBlock.IsAntennaPart(world, tip.UpCopy(1)))
			{
				tip = tip.UpCopy(1);
			}
			return tip;
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x00022C06 File Offset: 0x00020E06
		private static bool IsAntennaSupport(IWorldAccessor world, BlockPos pos)
		{
			return RadioAntennaPartBlock.IsAntennaPart(world, pos) || RadioAntennaPartBlock.IsRadioEmitter(world, pos);
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x00022C1C File Offset: 0x00020E1C
		private static bool IsAntennaPart(IWorldAccessor world, BlockPos pos)
		{
			if (world.BlockAccessor.GetBlockEntity(pos) is BlockEntityRadioAntennaPart)
			{
				return true;
			}
			Block block = world.BlockAccessor.GetBlock(pos);
			return block is RadioAntennaPartBlock || ((block != null) ? block.Class : null) == "radioantenna_partblock" || ((block != null) ? block.Class : null) == "radioantennapartblock";
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x00022C83 File Offset: 0x00020E83
		private static bool IsRadioEmitter(IWorldAccessor world, BlockPos pos)
		{
			if (world.BlockAccessor.GetBlockEntity(pos) is BlockEntityRadioEmitter)
			{
				return true;
			}
			Block block = world.BlockAccessor.GetBlock(pos);
			return ((block != null) ? block.Class : null) == "radioemitterblock";
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x00022CBC File Offset: 0x00020EBC
		private static bool CanPlaceOnSupport(IWorldAccessor world, BlockPos supportPos, ref string failureCode)
		{
			if (RadioAntennaPartBlock.IsAntennaSupport(world, supportPos))
			{
				return true;
			}
			if (world.BlockAccessor.GetBlockEntity(supportPos) is BlockEntityRadioSupervisionConsole)
			{
				failureCode = RPVoiceChatMod.modID + ":Radio.AntennaPart.PlaceFailure.WrongBlock";
				return false;
			}
			failureCode = RPVoiceChatMod.modID + ":Radio.AntennaPart.PlaceFailure.NeedSupport";
			return false;
		}
	}
}
