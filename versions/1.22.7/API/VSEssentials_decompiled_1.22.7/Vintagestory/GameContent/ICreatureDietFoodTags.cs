using Vintagestory.API.Common;

namespace Vintagestory.GameContent;

public interface ICreatureDietFoodTags
{
	string[] GetFoodTags(ItemStack itemstack);
}
