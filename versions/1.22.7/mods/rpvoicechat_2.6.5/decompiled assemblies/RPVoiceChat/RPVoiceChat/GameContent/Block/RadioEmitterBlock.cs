using System;
using RPVoiceChat.GameContent.BlockEntity;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent.Mechanics;

namespace RPVoiceChat.GameContent.Block
{
	// Token: 0x020000D7 RID: 215
	public class RadioEmitterBlock : WireNodeBlock, IMechanicalPowerBlock
	{
		// Token: 0x0600066C RID: 1644 RVA: 0x00022D14 File Offset: 0x00020F14
		public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer)
		{
			return new WorldInteraction[]
			{
				new WorldInteraction
				{
					ActionLangCode = "rpvoicechat:Radio.Emitter.Interaction.Use",
					MouseButton = 2
				}
			};
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x00022D38 File Offset: 0x00020F38
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
			BlockEntityRadioEmitter blockEntityRadioEmitter = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BlockEntityRadioEmitter;
			if (blockEntityRadioEmitter != null)
			{
				blockEntityRadioEmitter.OnInteract(byPlayer);
			}
			return true;
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x00022DAC File Offset: 0x00020FAC
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

		// Token: 0x0600066F RID: 1647 RVA: 0x00022DFA File Offset: 0x00020FFA
		public void DidConnectAt(IWorldAccessor world, BlockPos pos, BlockFacing face)
		{
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x00022DFC File Offset: 0x00020FFC
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

		// Token: 0x06000671 RID: 1649 RVA: 0x00022E20 File Offset: 0x00021020
		public override void OnNeighbourBlockChange(IWorldAccessor world, BlockPos pos, BlockPos neibpos)
		{
			base.OnNeighbourBlockChange(world, pos, neibpos);
			if (world.Side == 1)
			{
				BlockEntityRadioEmitter blockEntityRadioEmitter = world.BlockAccessor.GetBlockEntity(pos) as BlockEntityRadioEmitter;
				if (blockEntityRadioEmitter == null)
				{
					return;
				}
				blockEntityRadioEmitter.TryDiscoverNetwork();
			}
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x00022E50 File Offset: 0x00021050
		public override bool CanAttachBlockAt(IBlockAccessor blockAccessor, Block block, BlockPos pos, BlockFacing blockFace, Cuboidi attachmentArea = null)
		{
			return (blockFace == BlockFacing.UP && (block is RadioAntennaPartBlock || ((block != null) ? block.Class : null) == "radioantenna_partblock" || ((block != null) ? block.Class : null) == "radioantennapartblock")) || base.CanAttachBlockAt(blockAccessor, block, pos, blockFace, attachmentArea);
		}
	}
}
