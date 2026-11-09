using System;
using RPVoiceChat.GameContent.BlockEntity;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

// Token: 0x0200000E RID: 14
public class TelegraphBlock : WireNodeBlock
{
	// Token: 0x06000054 RID: 84 RVA: 0x00003C54 File Offset: 0x00001E54
	public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer)
	{
		return new WorldInteraction[]
		{
			new WorldInteraction
			{
				ActionLangCode = "rpvoicechat:Telegraph.Interaction.SendReceive",
				MouseButton = 2
			}
		};
	}

	// Token: 0x06000055 RID: 85 RVA: 0x00003C78 File Offset: 0x00001E78
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
		BlockEntityTelegraph blockEntityTelegraph = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BlockEntityTelegraph;
		if (blockEntityTelegraph != null)
		{
			blockEntityTelegraph.OnInteract();
		}
		return true;
	}
}
