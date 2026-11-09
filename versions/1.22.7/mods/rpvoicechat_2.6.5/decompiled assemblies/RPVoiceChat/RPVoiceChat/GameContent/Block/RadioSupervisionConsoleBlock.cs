using System;
using RPVoiceChat.GameContent.BlockEntity;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace RPVoiceChat.GameContent.Block
{
	// Token: 0x020000DB RID: 219
	public class RadioSupervisionConsoleBlock : WireNodeBlock
	{
		// Token: 0x0600067D RID: 1661 RVA: 0x00023093 File Offset: 0x00021293
		public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer)
		{
			return new WorldInteraction[]
			{
				new WorldInteraction
				{
					ActionLangCode = "rpvoicechat:Radio.Console.Interaction.Use",
					MouseButton = 2
				}
			};
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x000230B8 File Offset: 0x000212B8
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
			BlockEntityRadioSupervisionConsole blockEntityRadioSupervisionConsole = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BlockEntityRadioSupervisionConsole;
			if (blockEntityRadioSupervisionConsole != null)
			{
				blockEntityRadioSupervisionConsole.OnInteract(byPlayer);
			}
			return true;
		}
	}
}
