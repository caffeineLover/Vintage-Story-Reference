using System;
using Vintagestory.API.Common;

namespace RPVoiceChat.GameContent.Inventory
{
	// Token: 0x020000A3 RID: 163
	public class SlotPrinterPaper : SlotRestrictedItemPath
	{
		// Token: 0x060004FE RID: 1278 RVA: 0x0001AE3E File Offset: 0x0001903E
		public SlotPrinterPaper(InventoryBase inventory) : base(inventory, "paperslip", 64)
		{
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x0001AE4E File Offset: 0x0001904E
		public bool TryConsumePaperSlip()
		{
			if (!this.Empty && base.Itemstack.StackSize > 0)
			{
				this.TakeOut(1);
				this.MarkDirty();
				return true;
			}
			return false;
		}
	}
}
