using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Armor;
using Vintagestory.API.Common;

namespace CombatOverhaul.Vanity
{
	// Token: 0x020000B2 RID: 178
	public class ClothesVanitySlot : ClothesSlot, ISlotContentCanHide
	{
		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000965 RID: 2405 RVA: 0x00032D58 File Offset: 0x00030F58
		// (set) Token: 0x06000966 RID: 2406 RVA: 0x00032D60 File Offset: 0x00030F60
		public bool Hide { get; set; }

		// Token: 0x06000967 RID: 2407 RVA: 0x00032D69 File Offset: 0x00030F69
		[NullableContext(1)]
		public ClothesVanitySlot(EnumCharacterDressType type, InventoryBase inventory) : base(type, inventory)
		{
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x00032D74 File Offset: 0x00030F74
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

		// Token: 0x06000969 RID: 2409 RVA: 0x00032E38 File Offset: 0x00031038
		[NullableContext(2)]
		public override ItemStack TakeOut(int quantity)
		{
			return null;
		}
	}
}
