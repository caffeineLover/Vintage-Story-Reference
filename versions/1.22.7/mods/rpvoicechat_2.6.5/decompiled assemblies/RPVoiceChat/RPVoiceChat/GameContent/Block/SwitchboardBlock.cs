using System;
using RPVoiceChat.GameContent.BlockEntity;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent.Mechanics;

namespace RPVoiceChat.GameContent.Block
{
	// Token: 0x020000DE RID: 222
	public class SwitchboardBlock : WireNodeBlock, IMechanicalPowerBlock
	{
		// Token: 0x06000686 RID: 1670 RVA: 0x0002323C File Offset: 0x0002143C
		public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
		{
			ItemSlot activeHotbarSlot = byPlayer.InventoryManager.ActiveHotbarSlot;
			string text;
			if (activeHotbarSlot == null)
			{
				text = null;
			}
			else
			{
				ItemStack itemstack = activeHotbarSlot.Itemstack;
				if (itemstack == null)
				{
					text = null;
				}
				else
				{
					CollectibleObject collectible = itemstack.Collectible;
					text = ((collectible != null) ? collectible.Code.ToShortString() : null);
				}
			}
			if (text == "rpvoicechat:telegraphwire")
			{
				return false;
			}
			BlockEntitySwitchboard blockEntitySwitchboard = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BlockEntitySwitchboard;
			if (blockEntitySwitchboard != null)
			{
				blockEntitySwitchboard.OnInteract();
			}
			return true;
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x000232B0 File Offset: 0x000214B0
		public bool HasMechPowerConnectorAt(IWorldAccessor world, BlockPos pos, BlockFacing face, BlockMPBase forBlock)
		{
			Block blockAtPos = world.BlockAccessor.GetBlock(pos);
			string sideStr;
			if (((blockAtPos != null) ? blockAtPos.Variant : null) == null || !blockAtPos.Variant.TryGetValue("side", ref sideStr))
			{
				return false;
			}
			BlockFacing frontFace = BlockFacing.FromCode(sideStr);
			return frontFace != null && face == frontFace;
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x000232FE File Offset: 0x000214FE
		public void DidConnectAt(IWorldAccessor world, BlockPos pos, BlockFacing face)
		{
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x00023300 File Offset: 0x00021500
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

		// Token: 0x0600068A RID: 1674 RVA: 0x00023324 File Offset: 0x00021524
		public override void OnNeighbourBlockChange(IWorldAccessor world, BlockPos pos, BlockPos neibpos)
		{
			base.OnNeighbourBlockChange(world, pos, neibpos);
			if (world.Side == 1)
			{
				BlockEntitySwitchboard blockEntitySwitchboard = world.BlockAccessor.GetBlockEntity(pos) as BlockEntitySwitchboard;
				if (blockEntitySwitchboard == null)
				{
					return;
				}
				blockEntitySwitchboard.TryDiscoverNetwork();
			}
		}
	}
}
