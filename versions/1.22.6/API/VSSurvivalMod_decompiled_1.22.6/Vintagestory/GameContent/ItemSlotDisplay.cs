using Vintagestory.API.Common;

namespace Vintagestory.GameContent;

public class ItemSlotDisplay : ItemSlotSurvival
{
	public string DisplayCategory;

	public ItemSlotDisplay(InventoryBase inventory, string displayCategory)
		: base(inventory)
	{
		DisplayCategory = displayCategory;
	}
}
