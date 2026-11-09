using System;
using Vintagestory.API.Common;

namespace RPVoiceChat.GameContent.Inventory
{
	// Token: 0x020000CF RID: 207
	public class SlotPrinterPaper : SlotRestrictedItemPath
	{
		// Token: 0x0600063C RID: 1596 RVA: 0x00021C72 File Offset: 0x0001FE72
		public SlotPrinterPaper(InventoryBase inventory) : base(inventory, "paperslip", 64)
		{
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x00021C82 File Offset: 0x0001FE82
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
