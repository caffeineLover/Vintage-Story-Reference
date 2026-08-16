using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000123 RID: 291
	[NullableContext(1)]
	[Nullable(0)]
	public class ArmorStandArmorSlot : ItemSlot
	{
		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06000E48 RID: 3656 RVA: 0x000466AA File Offset: 0x000448AA
		public ArmorType ArmorType { get; }

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06000E49 RID: 3657 RVA: 0x000466B2 File Offset: 0x000448B2
		public ArmorType StoredArmoredType
		{
			get
			{
				return this.GetStoredArmorType();
			}
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06000E4A RID: 3658 RVA: 0x000466BA File Offset: 0x000448BA
		public override int MaxSlotStackSize
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06000E4B RID: 3659 RVA: 0x000466BD File Offset: 0x000448BD
		public ArmorStandArmorSlot(InventoryBase inventory, ArmorType armorType) : base(inventory)
		{
			this.ArmorType = armorType;
			ArmorStandArmorInventory armorStandArmorInventory = inventory as ArmorStandArmorInventory;
			if (armorStandArmorInventory == null)
			{
				throw new Exception();
			}
			this._inventory = armorStandArmorInventory;
		}

		// Token: 0x06000E4C RID: 3660 RVA: 0x000466E4 File Offset: 0x000448E4
		public override bool CanHold(ItemSlot sourceSlot)
		{
			IArmor armor;
			return !this.DrawUnavailable && base.CanHold(sourceSlot) && sourceSlot.Itemstack != null && ArmorStandArmorSlot.TryGetArmor(sourceSlot.Itemstack, out armor) && armor != null && this._inventory.CanHoldArmorPiece(armor, sourceSlot.Itemstack, this) && armor.ArmorType.Intersect(this.ArmorType);
		}

		// Token: 0x06000E4D RID: 3661 RVA: 0x0004674C File Offset: 0x0004494C
		private ArmorType GetStoredArmorType()
		{
			ItemStack itemstack = base.Itemstack;
			IArmor armor;
			if (((itemstack != null) ? itemstack.Collectible : null) != null && ArmorStandArmorSlot.TryGetArmor(base.Itemstack, out armor) && armor != null)
			{
				return armor.ArmorType;
			}
			return ArmorType.Empty;
		}

		// Token: 0x06000E4E RID: 3662 RVA: 0x0004678C File Offset: 0x0004498C
		public static bool TryGetArmor(ItemStack stack, [Nullable(2)] out IArmor armor)
		{
			if (stack.Collectible == null)
			{
				armor = null;
				return false;
			}
			IArmor armorItem = stack.Collectible as IArmor;
			if (armorItem != null)
			{
				armor = armorItem;
				return true;
			}
			IArmor armorBehavior = Enumerable.FirstOrDefault<CollectibleBehavior>(stack.Collectible.CollectibleBehaviors, (CollectibleBehavior x) => x is IArmor) as IArmor;
			if (armorBehavior == null)
			{
				armor = null;
				return false;
			}
			armor = armorBehavior;
			return true;
		}

		// Token: 0x040006FC RID: 1788
		private readonly ArmorStandArmorInventory _inventory;
	}
}
