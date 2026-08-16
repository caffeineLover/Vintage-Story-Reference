using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Armor;
using Vintagestory.API.Common;

namespace CombatOverhaul.Vanity
{
	// Token: 0x020000B4 RID: 180
	public class ArmorVanitySlot : ArmorSlot, ISlotContentCanHide
	{
		// Token: 0x1700021D RID: 541
		// (get) Token: 0x0600096F RID: 2415 RVA: 0x00032F1F File Offset: 0x0003111F
		// (set) Token: 0x06000970 RID: 2416 RVA: 0x00032F27 File Offset: 0x00031127
		public bool Hide { get; set; }

		// Token: 0x06000971 RID: 2417 RVA: 0x00032F30 File Offset: 0x00031130
		[NullableContext(1)]
		public ArmorVanitySlot(InventoryBase inventory, ArmorType armorType) : base(inventory, armorType)
		{
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x00032F3C File Offset: 0x0003113C
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

		// Token: 0x06000973 RID: 2419 RVA: 0x00033000 File Offset: 0x00031200
		[NullableContext(2)]
		public override ItemStack TakeOut(int quantity)
		{
			return null;
		}
	}
}
