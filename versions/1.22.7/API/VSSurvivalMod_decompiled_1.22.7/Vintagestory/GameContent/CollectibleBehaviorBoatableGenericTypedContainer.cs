using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace Vintagestory.GameContent;

public class CollectibleBehaviorBoatableGenericTypedContainer : CollectibleBehaviorHeldBag
{
	public CollectibleBehaviorBoatableGenericTypedContainer(CollectibleObject collObj)
		: base(collObj)
	{
	}

	public override int GetQuantitySlots(ItemStack bagstack)
	{
		ITreeAttribute attributes = bagstack.Attributes;
		if (attributes != null && attributes.HasAttribute("animalSerialized"))
		{
			return 0;
		}
		string type = bagstack.Attributes.GetString("type");
		if (type == null)
		{
			type = bagstack.Block.Attributes["defaultType"].AsString();
		}
		return (bagstack.ItemAttributes?["quantitySlots"]?[type]?.AsInt()).GetValueOrDefault();
	}
}
