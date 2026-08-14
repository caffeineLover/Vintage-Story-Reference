using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class CollectibleBehaviorTypedDisplayableProps : CollectibleBehavior, IDisplayableProps
{
	public Dictionary<string, Dictionary<string, DisplayableAttributes>>? PropsByCode;

	public CollectibleBehaviorTypedDisplayableProps(CollectibleObject collObj)
		: base(collObj)
	{
	}

	public override void Initialize(JsonObject properties)
	{
		base.Initialize(properties);
		PropsByCode = properties["displayable"].AsObject<Dictionary<string, Dictionary<string, DisplayableAttributes>>>();
	}

	public virtual DisplayableAttributes? GetDisplayableProps(ItemSlot inSlot, string displayType)
	{
		if (PropsByCode != null)
		{
			string type = inSlot.Itemstack?.Attributes.GetString("type", "");
			if (type != null)
			{
				return PropsByCode.FirstOrDefault<KeyValuePair<string, Dictionary<string, DisplayableAttributes>>>((KeyValuePair<string, Dictionary<string, DisplayableAttributes>> code) => WildcardUtil.Match(code.Key, type)).Value?.TryGetValue(displayType);
			}
		}
		return null;
	}
}
