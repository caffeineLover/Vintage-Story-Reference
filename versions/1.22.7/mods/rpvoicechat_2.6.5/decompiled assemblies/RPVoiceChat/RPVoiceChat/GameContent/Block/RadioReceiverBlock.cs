using System;
using RPVoiceChat.GameContent.BlockEntity;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace RPVoiceChat.GameContent.Block
{
	// Token: 0x020000DA RID: 218
	public class RadioReceiverBlock : WireNodeBlock
	{
		// Token: 0x0600067A RID: 1658 RVA: 0x00022FF3 File Offset: 0x000211F3
		public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer)
		{
			return new WorldInteraction[]
			{
				new WorldInteraction
				{
					ActionLangCode = "rpvoicechat:Radio.Receiver.Interaction.Use",
					MouseButton = 2
				}
			};
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x00023018 File Offset: 0x00021218
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
			BlockEntityRadioReceiver blockEntityRadioReceiver = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BlockEntityRadioReceiver;
			if (blockEntityRadioReceiver != null)
			{
				blockEntityRadioReceiver.OnInteract(byPlayer);
			}
			return true;
		}
	}
}
