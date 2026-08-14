using System;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public interface ITemperatureSensitive
{
	bool IsHot { get; }

	void CoolNow(float amountRel, OnStackToCool onStackToCoolCallback);

	static void CoolStack(ICoreAPI api, ItemSlot slot, Vec3d pos, float amountRel, float targettemp, bool sizzle)
	{
		ItemStack stack = slot.Itemstack;
		if (stack != null)
		{
			if (stack.Collectible.HasBehavior<CollectibleBehaviorQuenchable>())
			{
				float dt = amountRel / 10f;
				CollectibleBehaviorQuenchable.CoolToTemperature(api.World, slot, pos, dt, GlobalConstants.CollectibleDefaultTemperature, playSizzle: false);
			}
			else
			{
				float temp = stack.Collectible.GetTemperature(api.World, stack);
				stack.Collectible.SetTemperature(api.World, stack, Math.Min(1100f, temp - amountRel * 20f), delayCooldown: false);
			}
		}
	}
}
