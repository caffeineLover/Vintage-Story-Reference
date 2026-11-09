using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.GameContent.Inventory
{
	// Token: 0x020000CE RID: 206
	[NullableContext(1)]
	[Nullable(0)]
	public class InventoryPrinter : InventoryGeneric
	{
		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000637 RID: 1591 RVA: 0x00021ADF File Offset: 0x0001FCDF
		public SlotPrinterPaper PaperSlot
		{
			get
			{
				return (SlotPrinterPaper)this.slots[0];
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000638 RID: 1592 RVA: 0x00021AF0 File Offset: 0x0001FCF0
		public SlotPrinterTelegram[] TelegramSlots
		{
			get
			{
				return new SlotPrinterTelegram[]
				{
					(SlotPrinterTelegram)this.slots[1],
					(SlotPrinterTelegram)this.slots[2],
					(SlotPrinterTelegram)this.slots[3],
					(SlotPrinterTelegram)this.slots[4],
					(SlotPrinterTelegram)this.slots[5],
					(SlotPrinterTelegram)this.slots[6],
					(SlotPrinterTelegram)this.slots[7],
					(SlotPrinterTelegram)this.slots[8],
					(SlotPrinterTelegram)this.slots[9]
				};
			}
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x00021B95 File Offset: 0x0001FD95
		public InventoryPrinter()
		{
			int num = 10;
			string text = "printer-temp";
			string text2 = "temp";
			ICoreAPI coreAPI = null;
			NewSlotDelegate newSlotDelegate;
			if ((newSlotDelegate = InventoryPrinter.<>O.<0>__OnNewSlot) == null)
			{
				newSlotDelegate = (InventoryPrinter.<>O.<0>__OnNewSlot = new NewSlotDelegate(InventoryPrinter.OnNewSlot));
			}
			base..ctor(num, text, text2, coreAPI, newSlotDelegate);
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x00021BC8 File Offset: 0x0001FDC8
		public InventoryPrinter(ICoreAPI api, [Nullable(2)] BlockPos pos = null)
		{
			int num = 10;
			string text = "printer-" + (((pos != null) ? pos.ToString() : null) ?? "fake");
			string text2 = ((pos != null) ? pos.ToString() : null) ?? "-fake";
			NewSlotDelegate newSlotDelegate;
			if ((newSlotDelegate = InventoryPrinter.<>O.<0>__OnNewSlot) == null)
			{
				newSlotDelegate = (InventoryPrinter.<>O.<0>__OnNewSlot = new NewSlotDelegate(InventoryPrinter.OnNewSlot));
			}
			base..ctor(num, text, text2, api, newSlotDelegate);
			this.Pos = pos;
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x00021C34 File Offset: 0x0001FE34
		private static ItemSlot OnNewSlot(int slotId, InventoryGeneric self)
		{
			if (slotId >= 1)
			{
				if (slotId <= 9)
				{
					return new SlotPrinterTelegram(self);
				}
			}
			else if (slotId == 0)
			{
				return new SlotPrinterPaper(self);
			}
			throw new ArgumentOutOfRangeException("slotId", "InventoryPrinter should have exactly 10 slots");
		}

		// Token: 0x0200016C RID: 364
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x040005BF RID: 1471
			[Nullable(0)]
			public static NewSlotDelegate <0>__OnNewSlot;
		}
	}
}
