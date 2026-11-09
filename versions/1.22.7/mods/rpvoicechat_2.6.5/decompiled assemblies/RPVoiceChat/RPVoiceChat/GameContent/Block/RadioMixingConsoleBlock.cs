using System;
using RPVoiceChat.GameContent.BlockEntity;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace RPVoiceChat.GameContent.Block
{
	// Token: 0x020000D9 RID: 217
	public class RadioMixingConsoleBlock : WireNodeBlock
	{
		// Token: 0x06000677 RID: 1655 RVA: 0x00022F53 File Offset: 0x00021153
		public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer)
		{
			return new WorldInteraction[]
			{
				new WorldInteraction
				{
					ActionLangCode = "rpvoicechat:Radio.MixingConsole.Interaction.Use",
					MouseButton = 2
				}
			};
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x00022F78 File Offset: 0x00021178
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
			BlockEntityRadioMixingConsole blockEntityRadioMixingConsole = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BlockEntityRadioMixingConsole;
			if (blockEntityRadioMixingConsole != null)
			{
				blockEntityRadioMixingConsole.OnInteract(byPlayer);
			}
			return true;
		}
	}
}
