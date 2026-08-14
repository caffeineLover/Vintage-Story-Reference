using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.GameContent.Inventory
{
	// Token: 0x020000A2 RID: 162
	[NullableContext(1)]
	[Nullable(0)]
	public class InventoryPrinter : InventoryGeneric
	{
		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060004F9 RID: 1273 RVA: 0x0001ACAB File Offset: 0x00018EAB
		public SlotPrinterPaper PaperSlot
		{
			get
			{
				return (SlotPrinterPaper)this.slots[0];
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060004FA RID: 1274 RVA: 0x0001ACBC File Offset: 0x00018EBC
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

		// Token: 0x060004FB RID: 1275 RVA: 0x0001AD61 File Offset: 0x00018F61
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

		// Token: 0x060004FC RID: 1276 RVA: 0x0001AD94 File Offset: 0x00018F94
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

		// Token: 0x060004FD RID: 1277 RVA: 0x0001AE00 File Offset: 0x00019000
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

		// Token: 0x0200011A RID: 282
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x0400045F RID: 1119
			[Nullable(0)]
			public static NewSlotDelegate <0>__OnNewSlot;
		}
	}
}
