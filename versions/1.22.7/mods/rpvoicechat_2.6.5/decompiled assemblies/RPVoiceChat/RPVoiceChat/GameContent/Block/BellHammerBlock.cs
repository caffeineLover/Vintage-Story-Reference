using System;
using RPVoiceChat.GameContent.BlockEntity;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent.Mechanics;

namespace RPVoiceChat.GameContent.Block
{
	// Token: 0x020000D2 RID: 210
	public class BellHammerBlock : Block, IMechanicalPowerBlock
	{
		// Token: 0x06000646 RID: 1606 RVA: 0x0002205C File Offset: 0x0002025C
		public override bool CanPlaceBlock(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref string failureCode)
		{
			if (!base.CanPlaceBlock(world, byPlayer, blockSel, ref failureCode))
			{
				return false;
			}
			if (blockSel.Face != BlockFacing.UP && blockSel.Face != BlockFacing.DOWN)
			{
				failureCode = RPVoiceChatMod.modID + ":BellHammer.FloorOrCeilingOnly";
				return false;
			}
			BlockPos placePos = (world.BlockAccessor.GetBlock(blockSel.Position).Replaceable >= 6000) ? blockSel.Position : blockSel.Position.AddCopy(blockSel.Face);
			foreach (BlockFacing face in BellHammerBlock.HorizontalFaces)
			{
				if (BellHammerBlock.IsBellAt(world, placePos, face))
				{
					return true;
				}
			}
			failureCode = RPVoiceChatMod.modID + ":BellHammer.NeedBell";
			return false;
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x00022114 File Offset: 0x00020314
		public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
		{
			return this.CanPlaceBlock(world, byPlayer, blockSel, ref failureCode) && this.DoPlaceBlock(world, byPlayer, blockSel, itemstack);
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x00022134 File Offset: 0x00020334
		public override bool DoPlaceBlock(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ItemStack byItemStack)
		{
			Block blockAtSel = world.BlockAccessor.GetBlock(blockSel.Position);
			BlockPos placePos = (blockAtSel.Replaceable >= 6000) ? blockSel.Position : blockSel.Position.AddCopy(blockSel.Face);
			string vVariant = ((blockAtSel.Replaceable >= 6000 && blockSel.Face == BlockFacing.DOWN) || (blockAtSel.Replaceable < 6000 && blockSel.Face == BlockFacing.UP)) ? "down" : "up";
			string sideVariant = BlockFacing.HorizontalFromAngle(byPlayer.Entity.Pos.Yaw).Code;
			foreach (BlockFacing face in BellHammerBlock.HorizontalFaces)
			{
				if (BellHammerBlock.IsBellAt(world, placePos, face))
				{
					sideVariant = face.Code;
					break;
				}
			}
			string path = this.Code.Path;
			int last = path.LastIndexOf('-');
			int prev = (last > 0) ? path.LastIndexOf('-', last - 1) : -1;
			string basePath = (prev > 0) ? path.Substring(0, prev) : ((last > 0) ? path.Substring(0, last) : "bellhammer-aged");
			AssetLocation codeWithVariants = this.Code.WithPath(string.Concat(new string[]
			{
				basePath,
				"-",
				vVariant,
				"-",
				sideVariant
			}));
			Block blockToPlace = world.GetBlock(codeWithVariants) ?? world.GetBlock(this.Code);
			world.BlockAccessor.SetBlock(blockToPlace.BlockId, placePos);
			return true;
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x000222D0 File Offset: 0x000204D0
		private static bool IsBellAt(IWorldAccessor world, BlockPos fromPos, BlockFacing face)
		{
			IBlockAccessor ba = world.BlockAccessor;
			return BellHammerBlock.IsBellBlock(ba.GetBlock(fromPos.AddCopy(face))) || BellHammerBlock.IsChurchBellBlock(ba.GetBlock(fromPos.AddCopy(face).Add(face, 1)));
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x00022318 File Offset: 0x00020518
		private static bool IsBellBlock(Block block)
		{
			if (block == null)
			{
				return false;
			}
			AssetLocation code = block.Code;
			string path = ((code != null) ? code.Path : null) ?? "";
			return path.StartsWith("carillonbell") || path.StartsWith("churchbell");
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x00022360 File Offset: 0x00020560
		private static bool IsChurchBellBlock(Block block)
		{
			if (block == null)
			{
				return false;
			}
			AssetLocation code = block.Code;
			return (((code != null) ? code.Path : null) ?? "").StartsWith("churchbell");
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x0002238C File Offset: 0x0002058C
		public override void OnNeighbourBlockChange(IWorldAccessor world, BlockPos pos, BlockPos neibpos)
		{
			base.OnNeighbourBlockChange(world, pos, neibpos);
			if (world.Side == 1)
			{
				BlockEntityBellHammer blockEntityBellHammer = world.BlockAccessor.GetBlockEntity(pos) as BlockEntityBellHammer;
				if (blockEntityBellHammer == null)
				{
					return;
				}
				blockEntityBellHammer.TryDiscoverNetwork();
			}
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x000223BC File Offset: 0x000205BC
		public override ItemStack OnPickBlock(IWorldAccessor world, BlockPos pos)
		{
			Block blockAt = world.BlockAccessor.GetBlock(pos);
			if (blockAt != null && blockAt.Variant != null)
			{
				return new ItemStack(blockAt, 1);
			}
			return new ItemStack(world.GetBlock(base.CodeWithVariant("side", "north")) ?? world.GetBlock(this.Code), 1);
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x00022418 File Offset: 0x00020618
		public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
		{
			BlockEntityBellHammer be = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BlockEntityBellHammer;
			if (be != null)
			{
				return be.OnPlayerRightClick(byPlayer, blockSel);
			}
			return base.OnBlockInteractStart(world, byPlayer, blockSel);
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x00022454 File Offset: 0x00020654
		public bool HasMechPowerConnectorAt(IWorldAccessor world, BlockPos pos, BlockFacing face, BlockMPBase forBlock)
		{
			Block blockAtPos = world.BlockAccessor.GetBlock(pos);
			string sideStr;
			if (((blockAtPos != null) ? blockAtPos.Variant : null) == null || !blockAtPos.Variant.TryGetValue("side", ref sideStr))
			{
				return false;
			}
			BlockFacing frontFace = BlockFacing.FromCode(sideStr);
			return frontFace != null && face == frontFace.Opposite;
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x000224A7 File Offset: 0x000206A7
		public void DidConnectAt(IWorldAccessor world, BlockPos pos, BlockFacing face)
		{
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x000224A9 File Offset: 0x000206A9
		public MechanicalNetwork GetNetwork(IWorldAccessor world, BlockPos pos)
		{
			BlockEntity blockEntity = world.BlockAccessor.GetBlockEntity(pos);
			if (blockEntity == null)
			{
				return null;
			}
			BEBehaviorMPBase behavior = blockEntity.GetBehavior<BEBehaviorMPBase>();
			if (behavior == null)
			{
				return null;
			}
			return behavior.Network;
		}

		// Token: 0x04000320 RID: 800
		private static readonly BlockFacing[] HorizontalFaces = BlockFacing.HORIZONTALS;
	}
}
