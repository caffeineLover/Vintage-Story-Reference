using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace Vintagestory.GameContent;

public class ModSystemStopRaiseShieldAnim : ModSystem
{
	private ICoreClientAPI? capi;

	public override bool ShouldLoad(EnumAppSide forSide)
	{
		return forSide == EnumAppSide.Client;
	}

	public override void StartClientSide(ICoreClientAPI? api)
	{
		capi = api;
		if (capi != null)
		{
			capi.Event.AfterActiveSlotChanged += delegate
			{
				maybeStopRaiseShield();
			};
		}
	}

	public override void AssetsFinalize(ICoreAPI api)
	{
		if (capi != null)
		{
			capi.World.Player.InventoryManager.GetHotbarInventory().SlotModified += delegate
			{
				maybeStopRaiseShield();
			};
		}
	}

	private void maybeStopRaiseShield()
	{
		if (capi?.World?.Player != null)
		{
			EntityPlayer entityPlayer = capi.World.Player.Entity;
			if (!(entityPlayer.RightHandItemSlot.Itemstack?.Item is ItemShield) && entityPlayer.AnimManager.IsAnimationActive("raiseshield-right"))
			{
				entityPlayer.AnimManager.StopAnimation("raiseshield-right");
			}
		}
	}
}
