using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000124 RID: 292
	[NullableContext(1)]
	[Nullable(0)]
	public class ArmorStandArmorInventory : InventoryBase
	{
		// Token: 0x06000E4F RID: 3663 RVA: 0x000467FB File Offset: 0x000449FB
		public ArmorStandArmorInventory(string className, string id, ICoreAPI api) : base(className, id, api)
		{
			this.slots = base.GenEmptySlots(ArmorInventory._totalSlotsNumber + 6);
			this.baseWeight = 2.5f;
		}

		// Token: 0x06000E50 RID: 3664 RVA: 0x00046824 File Offset: 0x00044A24
		public ArmorStandArmorInventory(string inventoryId, ICoreAPI api) : base(inventoryId, api)
		{
			this.slots = base.GenEmptySlots(ArmorInventory._totalSlotsNumber + 6);
			this.baseWeight = 2.5f;
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06000E51 RID: 3665 RVA: 0x0004684C File Offset: 0x00044A4C
		public override int Count
		{
			get
			{
				return this.slots.Length;
			}
		}

		// Token: 0x170003A4 RID: 932
		public override ItemSlot this[int slotId]
		{
			get
			{
				return this.slots[slotId];
			}
			set
			{
				this.slots[slotId] = value;
			}
		}

		// Token: 0x06000E54 RID: 3668 RVA: 0x0004686C File Offset: 0x00044A6C
		public override void FromTreeAttributes(ITreeAttribute tree)
		{
			List<ItemSlot> modifiedSlots = new List<ItemSlot>();
			ItemSlot[] loadedSlots = this.SlotsFromTreeAttributes(tree, base.GenEmptySlots(ArmorInventory._totalSlotsNumber + 6), modifiedSlots);
			if (loadedSlots.Length == ArmorInventory._totalSlotsNumber + 6)
			{
				this.slots = loadedSlots;
				return;
			}
			this.slots = base.GenEmptySlots(ArmorInventory._totalSlotsNumber + 6);
			int copyCount = Math.Min(loadedSlots.Length, this.slots.Length);
			for (int i = 0; i < copyCount; i++)
			{
				this.slots[i].Itemstack = loadedSlots[i].Itemstack;
			}
		}

		// Token: 0x06000E55 RID: 3669 RVA: 0x000468EE File Offset: 0x00044AEE
		public override void ToTreeAttributes(ITreeAttribute tree)
		{
			base.SlotsToTreeAttributes(this.slots, tree);
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x000468FD File Offset: 0x00044AFD
		public bool IsSlotAvailable(ArmorType armorType)
		{
			return this.CanHoldArmorPiece(armorType);
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x00046906 File Offset: 0x00044B06
		public bool CanHoldArmorPiece(ArmorType armorType)
		{
			return this.CanHoldArmorPiece(armorType, null);
		}

		// Token: 0x06000E58 RID: 3672 RVA: 0x00046910 File Offset: 0x00044B10
		[NullableContext(2)]
		public bool CanHoldArmorPiece(ArmorType armorType, ItemSlot targetSlot)
		{
			return this.CanHoldArmorPiece(armorType, targetSlot, null);
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x0004691C File Offset: 0x00044B1C
		[NullableContext(2)]
		public bool CanHoldArmorPiece(ArmorType armorType, ItemSlot targetSlot, ItemSlot ignoredSourceSlot)
		{
			foreach (ItemSlot slot in this.slots)
			{
				if (slot != targetSlot && slot != ignoredSourceSlot && !slot.Empty)
				{
					ItemStack storedStack = slot.Itemstack;
					IArmor storedArmor;
					if (((storedStack != null) ? storedStack.Collectible : null) != null && ArmorStandArmorSlot.TryGetArmor(storedStack, out storedArmor) && storedArmor != null && storedArmor.ArmorType.Intersect(armorType))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x0004698B File Offset: 0x00044B8B
		public bool CanHoldArmorPiece(IArmor armor)
		{
			return this.CanHoldArmorPiece(armor.ArmorType);
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x00046999 File Offset: 0x00044B99
		public bool CanHoldArmorPiece(IArmor armor, ItemStack sourceStack, ItemSlot targetSlot)
		{
			return this.CanAcceptArmorStack(sourceStack, targetSlot);
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x000469A3 File Offset: 0x00044BA3
		public bool CanAcceptArmorStack(ItemStack sourceStack, [Nullable(2)] ItemSlot targetSlot = null)
		{
			return this.CanAcceptArmorStack(sourceStack, targetSlot, null);
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x000469B0 File Offset: 0x00044BB0
		[NullableContext(2)]
		public bool CanAcceptArmorStack([Nullable(1)] ItemStack sourceStack, ItemSlot targetSlot, ItemSlot ignoredSourceSlot)
		{
			IArmor sourceArmor;
			return sourceStack.Collectible != null && ArmorStandArmorSlot.TryGetArmor(sourceStack, out sourceArmor) && sourceArmor != null && !this.ContainsSameArmorPiece(sourceStack, targetSlot, ignoredSourceSlot) && this.CanHoldArmorPiece(sourceArmor.ArmorType, targetSlot, ignoredSourceSlot);
		}

		// Token: 0x06000E5E RID: 3678 RVA: 0x000469EF File Offset: 0x00044BEF
		public bool ContainsSameArmorPiece(ItemStack sourceStack, [Nullable(2)] ItemSlot targetSlot = null)
		{
			return this.ContainsSameArmorPiece(sourceStack, targetSlot, null);
		}

		// Token: 0x06000E5F RID: 3679 RVA: 0x000469FC File Offset: 0x00044BFC
		[NullableContext(2)]
		public bool ContainsSameArmorPiece([Nullable(1)] ItemStack sourceStack, ItemSlot targetSlot, ItemSlot ignoredSourceSlot)
		{
			CollectibleObject collectible = sourceStack.Collectible;
			AssetLocation sourceCode = (collectible != null) ? collectible.Code : null;
			if (sourceCode == null)
			{
				return false;
			}
			foreach (ItemSlot slot in this.slots)
			{
				if (slot != targetSlot && slot != ignoredSourceSlot && !slot.Empty)
				{
					ItemStack itemstack = slot.Itemstack;
					bool flag;
					if (itemstack == null)
					{
						flag = false;
					}
					else
					{
						CollectibleObject collectible2 = itemstack.Collectible;
						bool? flag2;
						if (collectible2 == null)
						{
							flag2 = default(bool?);
						}
						else
						{
							AssetLocation code = collectible2.Code;
							flag2 = ((code != null) ? new bool?(code.Equals(sourceCode)) : default(bool?));
						}
						bool? flag3 = flag2;
						flag = flag3.GetValueOrDefault();
					}
					if (flag)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000E60 RID: 3680 RVA: 0x00046AA0 File Offset: 0x00044CA0
		protected override ItemSlot NewSlot(int slotId)
		{
			if (slotId == ArmorInventory._totalSlotsNumber || slotId == ArmorInventory._totalSlotsNumber + 1)
			{
				return new ItemSlotSurvival(this);
			}
			if (slotId > ArmorInventory._totalSlotsNumber + 1)
			{
				return new ItemSlotBackpack(this);
			}
			return new ArmorStandArmorSlot(this, ArmorInventory.ArmorTypeFromIndex(slotId));
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x00046AD8 File Offset: 0x00044CD8
		public override void DiscardAll()
		{
			base.DiscardAll();
			for (int i = 0; i < this.Count; i++)
			{
				this.DidModifyItemSlot(this[i], null);
			}
		}

		// Token: 0x06000E62 RID: 3682 RVA: 0x00046B0A File Offset: 0x00044D0A
		public override void OnOwningEntityDeath(Vec3d pos)
		{
		}

		// Token: 0x040006FD RID: 1789
		private ItemSlot[] slots;
	}
}
