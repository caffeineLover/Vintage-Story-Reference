using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000130 RID: 304
	[NullableContext(1)]
	[Nullable(0)]
	public class ArmorStandArmorInventory : InventoryBase
	{
		// Token: 0x06000EC3 RID: 3779 RVA: 0x000480FB File Offset: 0x000462FB
		public ArmorStandArmorInventory(string className, string id, ICoreAPI api) : base(className, id, api)
		{
			this.slots = base.GenEmptySlots(ArmorInventory._totalSlotsNumber + 6);
			this.baseWeight = 2.5f;
		}

		// Token: 0x06000EC4 RID: 3780 RVA: 0x00048124 File Offset: 0x00046324
		public ArmorStandArmorInventory(string inventoryId, ICoreAPI api) : base(inventoryId, api)
		{
			this.slots = base.GenEmptySlots(ArmorInventory._totalSlotsNumber + 6);
			this.baseWeight = 2.5f;
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06000EC5 RID: 3781 RVA: 0x0004814C File Offset: 0x0004634C
		public override int Count
		{
			get
			{
				return this.slots.Length;
			}
		}

		// Token: 0x170003C6 RID: 966
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

		// Token: 0x06000EC8 RID: 3784 RVA: 0x0004816C File Offset: 0x0004636C
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

		// Token: 0x06000EC9 RID: 3785 RVA: 0x000481EE File Offset: 0x000463EE
		public override void ToTreeAttributes(ITreeAttribute tree)
		{
			base.SlotsToTreeAttributes(this.slots, tree);
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x000481FD File Offset: 0x000463FD
		public bool IsSlotAvailable(ArmorType armorType)
		{
			return this.CanHoldArmorPiece(armorType);
		}

		// Token: 0x06000ECB RID: 3787 RVA: 0x00048206 File Offset: 0x00046406
		public bool CanHoldArmorPiece(ArmorType armorType)
		{
			return this.CanHoldArmorPiece(armorType, null);
		}

		// Token: 0x06000ECC RID: 3788 RVA: 0x00048210 File Offset: 0x00046410
		[NullableContext(2)]
		public bool CanHoldArmorPiece(ArmorType armorType, ItemSlot targetSlot)
		{
			return this.CanHoldArmorPiece(armorType, targetSlot, null);
		}

		// Token: 0x06000ECD RID: 3789 RVA: 0x0004821C File Offset: 0x0004641C
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

		// Token: 0x06000ECE RID: 3790 RVA: 0x0004828B File Offset: 0x0004648B
		public bool CanHoldArmorPiece(IArmor armor)
		{
			return this.CanHoldArmorPiece(armor.ArmorType);
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x00048299 File Offset: 0x00046499
		public bool CanHoldArmorPiece(IArmor armor, ItemStack sourceStack, ItemSlot targetSlot)
		{
			return this.CanAcceptArmorStack(sourceStack, targetSlot);
		}

		// Token: 0x06000ED0 RID: 3792 RVA: 0x000482A3 File Offset: 0x000464A3
		public bool CanAcceptArmorStack(ItemStack sourceStack, [Nullable(2)] ItemSlot targetSlot = null)
		{
			return this.CanAcceptArmorStack(sourceStack, targetSlot, null);
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x000482B0 File Offset: 0x000464B0
		[NullableContext(2)]
		public bool CanAcceptArmorStack([Nullable(1)] ItemStack sourceStack, ItemSlot targetSlot, ItemSlot ignoredSourceSlot)
		{
			IArmor sourceArmor;
			return sourceStack.Collectible != null && ArmorStandArmorSlot.TryGetArmor(sourceStack, out sourceArmor) && sourceArmor != null && !this.ContainsSameArmorPiece(sourceStack, targetSlot, ignoredSourceSlot) && this.CanHoldArmorPiece(sourceArmor.ArmorType, targetSlot, ignoredSourceSlot);
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x000482EF File Offset: 0x000464EF
		public bool ContainsSameArmorPiece(ItemStack sourceStack, [Nullable(2)] ItemSlot targetSlot = null)
		{
			return this.ContainsSameArmorPiece(sourceStack, targetSlot, null);
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x000482FC File Offset: 0x000464FC
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

		// Token: 0x06000ED4 RID: 3796 RVA: 0x000483A0 File Offset: 0x000465A0
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

		// Token: 0x06000ED5 RID: 3797 RVA: 0x000483D8 File Offset: 0x000465D8
		public override void DiscardAll()
		{
			base.DiscardAll();
			for (int i = 0; i < this.Count; i++)
			{
				this.DidModifyItemSlot(this[i], null);
			}
		}

		// Token: 0x06000ED6 RID: 3798 RVA: 0x0004840A File Offset: 0x0004660A
		public override void OnOwningEntityDeath(Vec3d pos)
		{
		}

		// Token: 0x04000723 RID: 1827
		private ItemSlot[] slots;
	}
}
