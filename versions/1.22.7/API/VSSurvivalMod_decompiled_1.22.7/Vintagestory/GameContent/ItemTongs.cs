using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class ItemTongs : Item, IHeldHandAnimOverrider
{
	public override void OnBeforeRender(ICoreClientAPI capi, ItemStack itemstack, EnumItemRenderTarget target, ref ItemRenderInfo renderinfo)
	{
		if (target == EnumItemRenderTarget.HandTpOff && Attributes["shapeByOpening"].Exists)
		{
			ItemSlot hslot = capi.World.Player.InventoryManager.ActiveHotbarSlot;
			string opening = (hslot.Empty ? null : hslot.Itemstack.Collectible.Attributes?["tongOpening"].AsString());
			if (!string.IsNullOrEmpty(opening))
			{
				Dictionary<string, MultiTextureMeshRef> meshrefs = ObjectCacheUtil.GetOrCreate(capi, string.Concat(Code, "tongMeshrefsByOpening"), () => new Dictionary<string, MultiTextureMeshRef>());
				if (meshrefs.TryGetValue(opening, out var mtmeshref))
				{
					renderinfo.ModelRef = mtmeshref;
				}
				else
				{
					CompositeShape cshape = Attributes["shapeByOpening"][opening]?.AsObject<CompositeShape>();
					if (cshape != null)
					{
						cshape.Base.WithPathPrefixOnce("shapes/").WithPathAppendixOnce(".json");
						Shape shape = capi.Assets.TryGet(cshape.Base).ToObject<Shape>();
						capi.Tesselator.TesselateShape(this, shape, out var meshdata);
						ItemRenderInfo obj = renderinfo;
						MultiTextureMeshRef modelRef = (meshrefs[opening] = capi.Render.UploadMultiTextureMesh(meshdata));
						obj.ModelRef = modelRef;
					}
				}
			}
		}
		base.OnBeforeRender(capi, itemstack, target, ref renderinfo);
	}

	public override void OnUnloaded(ICoreAPI api)
	{
		Dictionary<string, MultiTextureMeshRef> dict = ObjectCacheUtil.TryGet<Dictionary<string, MultiTextureMeshRef>>(api, string.Concat(Code, "tongMeshrefsByOpening"));
		if (dict != null)
		{
			foreach (KeyValuePair<string, MultiTextureMeshRef> item in dict)
			{
				item.Value.Dispose();
			}
		}
		base.OnUnloaded(api);
	}

	public bool AllowHeldIdleHandAnim(Entity forEntity, ItemSlot slot, EnumHand hand)
	{
		string tpIdleAnim;
		if (!(slot.BackgroundIcon != "left_hand"))
		{
			return !isHoldingHotItem(forEntity, out tpIdleAnim);
		}
		return true;
	}

	public override string GetHeldTpIdleAnimation(ItemSlot activeHotbarSlot, Entity forEntity, EnumHand hand)
	{
		if (isHoldingHotItem(forEntity, out var tpIdleAnim))
		{
			return tpIdleAnim;
		}
		return base.GetHeldTpIdleAnimation(activeHotbarSlot, forEntity, hand);
	}

	private static bool isHoldingHotItem(Entity forEntity, out string tpIdleAnim)
	{
		tpIdleAnim = "holdbothhands-tongs1";
		if (forEntity is EntityPlayer eplr && !eplr.RightHandItemSlot.Empty)
		{
			ItemStack stack = eplr.RightHandItemSlot.Itemstack;
			if (stack.Collectible.GetTemperature(forEntity.World, stack) > (float)GlobalConstants.TooHotToTouchTemperature)
			{
				string opening = stack.Collectible.Attributes?["tongOpening"]?.AsString("1") ?? "1";
				tpIdleAnim = "holdbothhands-tongs" + opening;
				return true;
			}
		}
		return false;
	}
}
