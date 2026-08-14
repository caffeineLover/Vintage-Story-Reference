using Vintagestory.API.Common;

namespace Vintagestory.GameContent;

public interface ILookAwarePlacement
{
	Block GetLookAwareBlockVariant(IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel);
}
