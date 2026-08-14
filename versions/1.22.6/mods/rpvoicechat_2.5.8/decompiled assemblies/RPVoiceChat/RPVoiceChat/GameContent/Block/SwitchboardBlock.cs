using System;
using RPVoiceChat.GameContent.BlockEntity;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent.Mechanics;

namespace RPVoiceChat.GameContent.Block
{
	// Token: 0x020000AC RID: 172
	public class SwitchboardBlock : WireNodeBlock, IMechanicalPowerBlock
	{
		// Token: 0x06000528 RID: 1320 RVA: 0x0001BC10 File Offset: 0x00019E10
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

		// Token: 0x06000529 RID: 1321 RVA: 0x0001BC84 File Offset: 0x00019E84
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

		// Token: 0x0600052A RID: 1322 RVA: 0x0001BCD2 File Offset: 0x00019ED2
		public void DidConnectAt(IWorldAccessor world, BlockPos pos, BlockFacing face)
		{
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0001BCD4 File Offset: 0x00019ED4
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

		// Token: 0x0600052C RID: 1324 RVA: 0x0001BCF8 File Offset: 0x00019EF8
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
