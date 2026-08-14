using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace Vintagestory.GameContent;

public interface IHeldItemOnMouseWheel
{
	void OnMouseWheel(EntityPlayer byPlayer, ItemSlot inSlot, MouseWheelEventArgs args);
}
