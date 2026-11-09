using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Server;

namespace Vintagestory.GameContent;

public class ModSystemNPCHairStyling : ModSystem
{
	private GuiDialogHairStyling hairStylingDialog;

	private ICoreAPI Api;

	public override bool ShouldLoad(EnumAppSide forSide)
	{
		return true;
	}

	public override void Start(ICoreAPI api)
	{
		Api = api;
		api.Network.RegisterChannel("hairstyling").RegisterMessageType<PacketHairStyle>();
	}

	public override void StartServerSide(ICoreServerAPI api)
	{
		api.Network.GetChannel("hairstyling").SetMessageHandler<PacketHairStyle>(onPacketHairstyle);
	}

	private void onPacketHairstyle(IServerPlayer fromPlayer, PacketHairStyle packet)
	{
		if (!(Api.World.GetEntityById(packet.HairstylingNpcEntityId) is EntityTradingHumanoid hairstylingNpc) || !hairstylingNpc.interactingWithPlayer.Contains(fromPlayer.Entity))
		{
			return;
		}
		int costs = getCost(fromPlayer, packet.Hairstyle, GetPricesByCode(hairstylingNpc));
		if (InventoryTrader.GetPlayerAssets(fromPlayer.Entity) >= costs)
		{
			InventoryTrader.DeductFromEntity(Api, fromPlayer.Entity, costs);
			EntityBehaviorExtraSkinnable skinMod = fromPlayer.Entity.GetBehavior<EntityBehaviorExtraSkinnable>();
			foreach (KeyValuePair<string, string> val in packet.Hairstyle)
			{
				skinMod.selectSkinPart(val.Key, val.Value, retesselateShape: false, playVoice: false);
			}
			fromPlayer.Entity.WatchedAttributes.MarkPathDirty("skinConfig");
		}
		else
		{
			fromPlayer.SendIngameError("notenoughmoney", Lang.GetL(fromPlayer.LanguageCode, "Not enough money"));
		}
	}

	public int getCost(IServerPlayer player, Dictionary<string, string> hairstyle, Dictionary<string, int> costs)
	{
		int cost = 0;
		EntityBehaviorExtraSkinnable skinMod = player.Entity.GetBehavior<EntityBehaviorExtraSkinnable>();
		foreach (KeyValuePair<string, string> val in hairstyle)
		{
			AppliedSkinnablePartVariant appliedVar = skinMod.AppliedSkinParts.FirstOrDefault((AppliedSkinnablePartVariant sp) => sp.PartCode == val.Key);
			if (hairstyle[val.Key] != appliedVar.Code)
			{
				cost += costs[val.Key];
			}
		}
		return cost;
	}

	public void handleHairstyling(EntityTradingHumanoid hairstylingNpc, EntityAgent triggeringEntity, string[] hairStylingCategories)
	{
		EntityPlayer eplr = triggeringEntity as EntityPlayer;
		if (hairstylingNpc.Alive && triggeringEntity.Pos.SquareDistanceTo(hairstylingNpc.Pos) <= 7f && !hairstylingNpc.interactingWithPlayer.Contains(eplr))
		{
			hairstylingNpc.interactingWithPlayer.Add(triggeringEntity as EntityPlayer);
			ICoreAPI api = Api;
			ICoreClientAPI capi = api as ICoreClientAPI;
			if (capi != null)
			{
				hairStylingDialog = new GuiDialogHairStyling(capi, hairstylingNpc.EntityId, hairStylingCategories, GetPricesByCode(triggeringEntity));
				hairStylingDialog.charNaked = false;
				hairStylingDialog.TryOpen();
				hairStylingDialog.OnClosed += delegate
				{
					capi.World.Player.Entity.GetBehavior<EntityBehaviorPlayerInventory>().hideClothing = false;
					(capi.World.Player.Entity.Properties.Client.Renderer as EntityShapeRenderer).TesselateShape();
					hairstylingNpc.interactingWithPlayer.Remove(eplr);
					capi.Network.SendEntityPacket(hairstylingNpc.EntityId, 1212);
				};
			}
		}
		else
		{
			hairStylingDialog?.TryClose();
			hairstylingNpc.interactingWithPlayer.Remove(eplr);
			if (Api is ICoreServerAPI sapi)
			{
				sapi.Network.SendEntityPacket(eplr.Player as IServerPlayer, hairstylingNpc.EntityId, 1212);
			}
		}
	}

	public Dictionary<string, int> GetPricesByCode(Entity hairstylingNpc)
	{
		Dictionary<string, int> defaultValue = new Dictionary<string, int>
		{
			{ "hairbase", 2 },
			{ "hairextra", 2 },
			{ "mustache", 2 },
			{ "beard", 2 }
		};
		return hairstylingNpc?.Properties?.Attributes["hairstylingCosts"].AsObject(defaultValue) ?? defaultValue;
	}
}
