using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class ItemPressedMash : Item
{
	public override string GetHeldItemName(ItemStack itemStack)
	{
		string ap = (((float)Math.Round(itemStack.Attributes.GetDecimal("juiceableLitresLeft"), 2) > 0f) ? "wet" : "dry");
		string type = ItemClass.Name();
		return Lang.GetMatching(Code?.Domain + ":" + type + "-" + Code?.Path + "-" + ap);
	}

	public override List<ItemStack> GetHandBookStacks(ICoreClientAPI capi)
	{
		List<ItemStack> stacks = base.GetHandBookStacks(capi);
		if (stacks != null)
		{
			foreach (ItemStack stack in stacks)
			{
				JuiceableProperties? obj = stack?.ItemAttributes?["juiceableProperties"]?.AsObject<JuiceableProperties>(null, stack.Collectible.Code.Domain);
				obj?.LiquidStack?.Resolve(api.World, "juiceable properties liquidstack");
				obj?.PressedStack?.Resolve(api.World, "juiceable properties pressedstack");
				obj?.ReturnStack?.Resolve(api.World, "juiceable properties returnstack");
				if (obj?.ReturnStack?.ResolvedItemstack != null)
				{
					stack.Attributes.SetDouble("juiceableLitresLeft", 1.0);
				}
			}
		}
		return stacks;
	}

	public override ItemStack OnTransitionNow(ItemSlot slot, TransitionableProperties props)
	{
		float pressedDryRatio = slot.Itemstack.ItemAttributes["juiceableProperties"]["pressedDryRatio"].AsFloat(1f);
		double juiceableLitresTotal = slot.Itemstack.Attributes.GetDouble("juiceableLitresLeft") + slot.Itemstack.Attributes.GetDouble("juiceableLitresTransfered");
		TransitionableProperties nProps = props.Clone();
		if (juiceableLitresTotal > 0.0)
		{
			nProps.TransitionRatio = props.TransitionRatio * (float)(int)((float)GameMath.RoundRandom(api.World.Rand, (float)juiceableLitresTotal) * pressedDryRatio);
		}
		return base.OnTransitionNow(slot, nProps);
	}
}
