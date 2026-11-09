using System;
using Vintagestory.API.Common;

namespace RPVoiceChat.GameContent.Inventory
{
	// Token: 0x020000D1 RID: 209
	public class SlotRestrictedItemPath : ItemSlot
	{
		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000643 RID: 1603 RVA: 0x00021FD9 File Offset: 0x000201D9
		public string AllowedPath { get; }

		// Token: 0x06000644 RID: 1604 RVA: 0x00021FE1 File Offset: 0x000201E1
		public SlotRestrictedItemPath(InventoryBase inventory, string allowedPath, int maxSlotStackSize = 64) : base(inventory)
		{
			this.AllowedPath = (allowedPath ?? "");
			this.MaxSlotStackSize = maxSlotStackSize;
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x00022004 File Offset: 0x00020204
		public override bool CanHold(ItemSlot sourceSlot)
		{
			if (string.IsNullOrEmpty(this.AllowedPath) || ((sourceSlot != null) ? sourceSlot.Itemstack : null) == null)
			{
				return false;
			}
			CollectibleObject collectible = sourceSlot.Itemstack.Collectible;
			string text;
			if (collectible == null)
			{
				text = null;
			}
			else
			{
				AssetLocation code = collectible.Code;
				text = ((code != null) ? code.Path : null);
			}
			return text == this.AllowedPath;
		}
	}
}
