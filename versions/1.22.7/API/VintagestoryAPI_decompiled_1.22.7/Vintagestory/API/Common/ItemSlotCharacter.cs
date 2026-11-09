using System;

namespace Vintagestory.API.Common;

public class ItemSlotCharacter : ItemSlot
{
	public EnumCharacterDressType Type;

	public override EnumItemStorageFlags StorageType => EnumItemStorageFlags.Outfit;

	public ItemSlotCharacter(EnumCharacterDressType type, InventoryBase inventory)
		: base(inventory)
	{
		Type = type;
	}

	public override bool CanTakeFrom(ItemSlot sourceSlot, EnumMergePriority priority = EnumMergePriority.AutoMerge)
	{
		if (!CheckDressType(sourceSlot.Itemstack, Type))
		{
			return false;
		}
		return base.CanTakeFrom(sourceSlot, priority);
	}

	public override bool CanHold(ItemSlot itemstackFromSourceSlot)
	{
		if (!CheckDressType(itemstackFromSourceSlot.Itemstack, Type))
		{
			return false;
		}
		return base.CanHold(itemstackFromSourceSlot);
	}

	protected virtual bool CheckDressType(IItemStack itemstack, EnumCharacterDressType dressType)
	{
		return IsDressType(itemstack, dressType);
	}

	/// <summary>
	/// Checks to see what dress type the given item is.
	/// </summary>
	/// <param name="itemstack"></param>
	/// <param name="dressType"></param>
	/// <returns></returns>
	public static bool IsDressType(IItemStack itemstack, EnumCharacterDressType dressType)
	{
		if (itemstack == null || itemstack.Collectible.Attributes == null)
		{
			return false;
		}
		IWearableStatsSupplier wearableStats = itemstack.Collectible.GetCollectibleInterface<IWearableStatsSupplier>();
		if (wearableStats != null)
		{
			return dressType == wearableStats.GetDressType(new DummySlot((ItemStack)itemstack));
		}
		string stackDressType = itemstack.Collectible.Attributes["clothescategory"].AsString() ?? itemstack.Collectible.Attributes["attachableToEntity"]["categoryCode"].AsString();
		if (stackDressType != null)
		{
			return dressType.ToString().Equals(stackDressType, StringComparison.InvariantCultureIgnoreCase);
		}
		return false;
	}
}
