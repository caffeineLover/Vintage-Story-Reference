using Vintagestory.API.Common;

namespace Vintagestory.GameContent;

public class ItemPocketSun : Item
{
	public override float GetTemperature(IWorldAccessor world, ItemStack itemstack)
	{
		return 6000f;
	}

	public override float GetTemperature(IWorldAccessor world, ItemStack itemstack, double didReceiveHeat)
	{
		return 6000f;
	}
}
