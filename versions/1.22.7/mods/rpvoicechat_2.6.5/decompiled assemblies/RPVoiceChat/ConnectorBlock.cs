using System;
using Vintagestory.API.Common;

// Token: 0x0200000B RID: 11
public class ConnectorBlock : WireNodeBlock
{
	// Token: 0x06000049 RID: 73 RVA: 0x00003AAC File Offset: 0x00001CAC
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
		return !(text == "rpvoicechat:telegraphwire") && base.OnBlockInteractStart(world, byPlayer, blockSel);
	}
}
