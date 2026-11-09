using Newtonsoft.Json.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Server;

namespace Vintagestory.GameContent;

public class ModSystemSubTongsDurability : ModSystem
{
	private ICoreServerAPI sapi;

	private ICoreClientAPI capi;

	public override bool ShouldLoad(EnumAppSide forSide)
	{
		return true;
	}

	public override void StartServerSide(ICoreServerAPI api)
	{
		sapi = api;
		api.Event.RegisterEventBusListener(onCollected, 0.5, "onitemcollected");
	}

	public override void StartClientSide(ICoreClientAPI api)
	{
		capi = api;
		api.Event.RegisterEventBusListener(OnEventBusEvent);
	}

	private void OnEventBusEvent(string eventName, ref EnumHandling handling, IAttribute data)
	{
		TreeAttribute tree = data as TreeAttribute;
		if ((eventName == "onsettransform" || eventName == "ongettransform") && tree.GetString("target") == "onTongTransform")
		{
			string transformCode = (capi.World.Player.Entity?.LeftHandItemSlot?.Itemstack)?.ItemAttributes["transformCode"].AsString();
			CollectibleObject collobj = capi.World.Player.InventoryManager.ActiveHotbarSlot.Itemstack.Collectible;
			if (!collobj.Attributes[transformCode].Exists)
			{
				transformCode = "onTongTransform";
			}
			if (eventName == "ongettransform")
			{
				ModelTransform defaultmt = EntityPlayerShapeRenderer.DefaultTongTransform;
				(collobj.Attributes?[transformCode].AsObject(defaultmt) ?? defaultmt).ToTreeAttribute(tree);
				tree.SetBool("preventDefault", value: true);
			}
			else
			{
				collobj.Attributes.Token[transformCode] = JToken.FromObject(ModelTransform.CreateFromTreeAttribute(tree));
				tree.SetBool("preventDefault", value: true);
			}
		}
	}

	private void onCollected(string eventName, ref EnumHandling handling, IAttribute data)
	{
		TreeAttribute tree = data as TreeAttribute;
		OnItemPickedUp(sapi.World.GetEntityById(tree.GetLong("byentityid", 0L)), tree.GetItemstack("itemstack"));
	}

	public void OnItemPickedUp(Entity byEntity, ItemStack stack = null)
	{
		if (byEntity is EntityPlayer { Player: IServerPlayer srvplr } eplr)
		{
			ItemSlot hotbarslot = srvplr.InventoryManager.ActiveHotbarSlot;
			ItemSlot offhandslot = srvplr.InventoryManager.OffhandHotbarSlot;
			if (srvplr.InventoryManager.OffhandTool == EnumTool.Tongs && (stack == null || stack.Equals(sapi.World, hotbarslot.Itemstack, GlobalConstants.IgnoredStackAttributes)) && hotbarslot.Itemstack.Collectible.GetTemperature(sapi.World, hotbarslot.Itemstack) > (float)GlobalConstants.TooHotToTouchTemperature)
			{
				offhandslot.Itemstack.Collectible.DamageItem(sapi.World, eplr, offhandslot);
			}
		}
	}
}
