using Vintagestory.API.Common;

namespace Vintagestory.GameContent;

public class ItemSlotAuction : ItemSlotSurvival
{
	public ItemSlotAuction(InventoryBase inventory)
		: base(inventory)
	{
		StorageType = EnumItemStorageFlags.General | EnumItemStorageFlags.Backpack | EnumItemStorageFlags.Metallurgy | EnumItemStorageFlags.Jewellery | EnumItemStorageFlags.Alchemy | EnumItemStorageFlags.Agriculture | EnumItemStorageFlags.Outfit;
	}

	public override bool CanHold(ItemSlot sourceSlot)
	{
		IHeldBag bag = sourceSlot.Itemstack?.Collectible.GetCollectibleInterface<IHeldBag>() ?? null;
		if (bag != null && !bag.IsEmpty(sourceSlot.Itemstack))
		{
			return false;
		}
		return base.CanHold(sourceSlot);
	}
}
