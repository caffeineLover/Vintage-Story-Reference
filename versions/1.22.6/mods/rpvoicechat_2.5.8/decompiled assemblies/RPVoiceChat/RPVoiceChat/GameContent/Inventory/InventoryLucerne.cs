using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.GameContent.Inventory
{
	// Token: 0x020000A1 RID: 161
	[NullableContext(1)]
	[Nullable(0)]
	public class InventoryLucerne : InventoryGeneric
	{
		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060004F4 RID: 1268 RVA: 0x0001ABAB File Offset: 0x00018DAB
		public SlotRestrictedItemPath FatSlot
		{
			get
			{
				return (SlotRestrictedItemPath)this.slots[0];
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060004F5 RID: 1269 RVA: 0x0001ABBA File Offset: 0x00018DBA
		public SlotRestrictedItemPath FirewoodSlot
		{
			get
			{
				return (SlotRestrictedItemPath)this.slots[1];
			}
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0001ABC9 File Offset: 0x00018DC9
		public InventoryLucerne()
		{
			int num = 2;
			string text = "lucerne-temp";
			string text2 = "temp";
			ICoreAPI coreAPI = null;
			NewSlotDelegate newSlotDelegate;
			if ((newSlotDelegate = InventoryLucerne.<>O.<0>__OnNewSlot) == null)
			{
				newSlotDelegate = (InventoryLucerne.<>O.<0>__OnNewSlot = new NewSlotDelegate(InventoryLucerne.OnNewSlot));
			}
			base..ctor(num, text, text2, coreAPI, newSlotDelegate);
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0001ABF8 File Offset: 0x00018DF8
		public InventoryLucerne(ICoreAPI api, [Nullable(2)] BlockPos pos = null)
		{
			int num = 2;
			string text = "lucerne-" + (((pos != null) ? pos.ToString() : null) ?? "fake");
			string text2 = ((pos != null) ? pos.ToString() : null) ?? "-fake";
			NewSlotDelegate newSlotDelegate;
			if ((newSlotDelegate = InventoryLucerne.<>O.<0>__OnNewSlot) == null)
			{
				newSlotDelegate = (InventoryLucerne.<>O.<0>__OnNewSlot = new NewSlotDelegate(InventoryLucerne.OnNewSlot));
			}
			base..ctor(num, text, text2, api, newSlotDelegate);
			this.Pos = pos;
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x0001AC64 File Offset: 0x00018E64
		private static ItemSlot OnNewSlot(int slotId, InventoryGeneric self)
		{
			SlotRestrictedItemPath result;
			if (slotId != 0)
			{
				if (slotId != 1)
				{
					throw new ArgumentOutOfRangeException("slotId", "InventoryLucerne has 2 slots");
				}
				result = new SlotRestrictedItemPath(self, "firewood", 32);
			}
			else
			{
				result = new SlotRestrictedItemPath(self, "fat", 64);
			}
			return result;
		}

		// Token: 0x02000119 RID: 281
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x0400045E RID: 1118
			[Nullable(0)]
			public static NewSlotDelegate <0>__OnNewSlot;
		}
	}
}
