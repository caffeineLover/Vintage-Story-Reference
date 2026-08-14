using System;
using RPVoiceChat.GameContent.BlockEntity;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

// Token: 0x0200000F RID: 15
public class TelephoneBlock : WireNodeBlock
{
	// Token: 0x06000056 RID: 86 RVA: 0x00003BC7 File Offset: 0x00001DC7
	public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer)
	{
		return new WorldInteraction[]
		{
			new WorldInteraction
			{
				ActionLangCode = "rpvoicechat:Telephone.Interaction.Use",
				MouseButton = 2
			}
		};
	}

	// Token: 0x06000057 RID: 87 RVA: 0x00003BEC File Offset: 0x00001DEC
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
			text = ((itemstack != null) ? itemstack.Collectible.Code.ToShortString() : null);
		}
		if (text == "rpvoicechat:telegraphwire")
		{
			return false;
		}
		BlockEntityTelephone blockEntityTelephone = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BlockEntityTelephone;
		if (blockEntityTelephone != null)
		{
			blockEntityTelephone.OnInteract(byPlayer);
		}
		return true;
	}
}
