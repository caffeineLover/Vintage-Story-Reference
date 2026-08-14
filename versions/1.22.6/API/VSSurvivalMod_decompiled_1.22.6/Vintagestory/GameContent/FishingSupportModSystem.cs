using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace Vintagestory.GameContent;

public class FishingSupportModSystem : ModSystem
{
	private ICoreClientAPI capi;

	private ICoreServerAPI sapi;

	public NormalizedSimplexNoise NoiseGen;

	public override bool ShouldLoad(EnumAppSide forSide)
	{
		return true;
	}

	public override void StartClientSide(ICoreClientAPI api)
	{
		api.Event.BeforeActiveSlotChanged += Event_BeforeActiveSlotChanged;
		capi = api;
	}

	private EnumHandling Event_BeforeActiveSlotChanged(ActiveSlotChangeEventArgs args)
	{
		return beforeActiveSlotChanged(capi.World.Player, args);
	}

	public override void StartServerSide(ICoreServerAPI api)
	{
		sapi = api;
		api.Event.BeforeActiveSlotChanged += beforeActiveSlotChanged;
		api.Event.PlayerDisconnect += Event_PlayerDisconnect;
		api.Event.ServerRunPhase(EnumServerRunPhase.Shutdown, onShutdown);
		NoiseGen = NormalizedSimplexNoise.FromDefaultOctaves(2, 0.10000000149011612, 0.9, sapi.World.Seed);
	}

	private void onShutdown()
	{
		IPlayer[] allOnlinePlayers = sapi.World.AllOnlinePlayers;
		foreach (IPlayer plr in allOnlinePlayers)
		{
			ItemSlot slot = plr.InventoryManager.ActiveHotbarSlot;
			(slot?.Itemstack?.Collectible as ItemFishingPole)?.StopFishing(slot.Itemstack, plr.Entity);
		}
	}

	private void Event_PlayerDisconnect(IServerPlayer plr)
	{
		ItemSlot slot = plr.InventoryManager.ActiveHotbarSlot;
		(slot?.Itemstack?.Collectible as ItemFishingPole)?.StopFishing(slot.Itemstack, plr.Entity);
	}

	private EnumHandling beforeActiveSlotChanged(IPlayer plr, ActiveSlotChangeEventArgs args)
	{
		ItemSlot slot = plr.InventoryManager.ActiveHotbarSlot;
		if (slot?.Itemstack?.Collectible is ItemFishingPole && slot.Itemstack.Attributes.GetLong("fishingEntityId", 0L) != 0L)
		{
			return EnumHandling.PreventSubsequent;
		}
		return EnumHandling.PassThrough;
	}
}
