using System;
using Vintagestory.API.Common;

namespace RPVoiceChat.GameContent.Inventory
{
	// Token: 0x020000A5 RID: 165
	public class SlotRestrictedItemPath : ItemSlot
	{
		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000505 RID: 1285 RVA: 0x0001B1A5 File Offset: 0x000193A5
		public string AllowedPath { get; }

		// Token: 0x06000506 RID: 1286 RVA: 0x0001B1AD File Offset: 0x000193AD
		public SlotRestrictedItemPath(InventoryBase inventory, string allowedPath, int maxSlotStackSize = 64) : base(inventory)
		{
			this.AllowedPath = (allowedPath ?? "");
			this.MaxSlotStackSize = maxSlotStackSize;
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x0001B1D0 File Offset: 0x000193D0
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
