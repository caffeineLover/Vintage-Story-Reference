using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Armor;
using Vintagestory.API.Common;

namespace CombatOverhaul.Vanity
{
	// Token: 0x020000B3 RID: 179
	public class GearVanitySlot : GearSlot, ISlotContentCanHide
	{
		// Token: 0x1700021C RID: 540
		// (get) Token: 0x0600096A RID: 2410 RVA: 0x00032E3B File Offset: 0x0003103B
		// (set) Token: 0x0600096B RID: 2411 RVA: 0x00032E43 File Offset: 0x00031043
		public bool Hide { get; set; }

		// Token: 0x0600096C RID: 2412 RVA: 0x00032E4C File Offset: 0x0003104C
		[NullableContext(1)]
		public GearVanitySlot(string slotType, InventoryBase inventory) : base(slotType, inventory)
		{
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x00032E58 File Offset: 0x00031058
		[NullableContext(1)]
		public override void ActivateSlot(ItemSlot sourceSlot, ref ItemStackMoveOperation op)
		{
			if (base.Inventory.Api.Side == 2)
			{
				return;
			}
			if (sourceSlot.Itemstack != null)
			{
				if (!this.CanHold(sourceSlot))
				{
					return;
				}
				base.Itemstack = sourceSlot.Itemstack.Clone();
				this.Hide = false;
				this.HexBackgroundColor = null;
				IClickableSlot.SlotClickedDelegate onSlotClicked = base.OnSlotClicked;
				if (onSlotClicked != null)
				{
					onSlotClicked(this, sourceSlot, ref op);
				}
				this.MarkDirty();
				return;
			}
			else
			{
				if (base.Itemstack == null)
				{
					this.Hide = !this.Hide;
					this.HexBackgroundColor = (this.Hide ? "#776666" : null);
					this.MarkDirty();
					return;
				}
				base.Itemstack = null;
				IClickableSlot.SlotClickedDelegate onSlotClicked2 = base.OnSlotClicked;
				if (onSlotClicked2 != null)
				{
					onSlotClicked2(this, sourceSlot, ref op);
				}
				this.MarkDirty();
				return;
			}
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x00032F1C File Offset: 0x0003111C
		[NullableContext(2)]
		public override ItemStack TakeOut(int quantity)
		{
			return null;
		}
	}
}
