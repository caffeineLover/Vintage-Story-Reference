using Vintagestory.API.Common;

namespace Vintagestory.GameContent;

public interface IDisplayableProps
{
	DisplayableAttributes? GetDisplayableProps(ItemSlot inSlot, string displayType);
}
