using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.Armor
{
	// Token: 0x0200012F RID: 303
	[NullableContext(1)]
	[Nullable(0)]
	public class ArmorStandArmorSlot : ItemSlot
	{
		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x06000EBC RID: 3772 RVA: 0x00047FAA File Offset: 0x000461AA
		public ArmorType ArmorType { get; }

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06000EBD RID: 3773 RVA: 0x00047FB2 File Offset: 0x000461B2
		public ArmorType StoredArmoredType
		{
			get
			{
				return this.GetStoredArmorType();
			}
		}

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06000EBE RID: 3774 RVA: 0x00047FBA File Offset: 0x000461BA
		public override int MaxSlotStackSize
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06000EBF RID: 3775 RVA: 0x00047FBD File Offset: 0x000461BD
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

		// Token: 0x06000EC0 RID: 3776 RVA: 0x00047FE4 File Offset: 0x000461E4
		public override bool CanHold(ItemSlot sourceSlot)
		{
			IArmor armor;
			return !this.DrawUnavailable && base.CanHold(sourceSlot) && sourceSlot.Itemstack != null && ArmorStandArmorSlot.TryGetArmor(sourceSlot.Itemstack, out armor) && armor != null && this._inventory.CanHoldArmorPiece(armor, sourceSlot.Itemstack, this) && armor.ArmorType.Intersect(this.ArmorType);
		}

		// Token: 0x06000EC1 RID: 3777 RVA: 0x0004804C File Offset: 0x0004624C
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

		// Token: 0x06000EC2 RID: 3778 RVA: 0x0004808C File Offset: 0x0004628C
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

		// Token: 0x04000722 RID: 1826
		private readonly ArmorStandArmorInventory _inventory;
	}
}
