using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.GameContent.Inventory
{
	// Token: 0x020000CD RID: 205
	[NullableContext(1)]
	[Nullable(0)]
	public class InventoryLucerne : InventoryGeneric
	{
		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000632 RID: 1586 RVA: 0x000219DF File Offset: 0x0001FBDF
		public SlotRestrictedItemPath FatSlot
		{
			get
			{
				return (SlotRestrictedItemPath)this.slots[0];
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000633 RID: 1587 RVA: 0x000219EE File Offset: 0x0001FBEE
		public SlotRestrictedItemPath FirewoodSlot
		{
			get
			{
				return (SlotRestrictedItemPath)this.slots[1];
			}
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x000219FD File Offset: 0x0001FBFD
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

		// Token: 0x06000635 RID: 1589 RVA: 0x00021A2C File Offset: 0x0001FC2C
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

		// Token: 0x06000636 RID: 1590 RVA: 0x00021A98 File Offset: 0x0001FC98
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

		// Token: 0x0200016B RID: 363
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x040005BE RID: 1470
			[Nullable(0)]
			public static NewSlotDelegate <0>__OnNewSlot;
		}
	}
}
