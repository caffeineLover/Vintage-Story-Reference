using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class CollectibleBehaviorCustomTongedShape : CollectibleBehavior
{
	private CompositeShape tongedShape;

	public CollectibleBehaviorCustomTongedShape(CollectibleObject collObj)
		: base(collObj)
	{
	}

	public override void Initialize(JsonObject properties)
	{
		tongedShape = properties["shape"].AsObject<CompositeShape>(null, collObj.Code.Domain);
		base.Initialize(properties);
	}

	public override void OnBeforeRender(ICoreClientAPI capi, ItemStack itemstack, EnumItemRenderTarget target, ref ItemRenderInfo renderinfo)
	{
		if (target == EnumItemRenderTarget.HandTpOff && tongedShape != null && itemstack.Collectible.GetTemperature(capi.World, itemstack) > (float)GlobalConstants.TooHotToTouchTemperature)
		{
			_ = capi.World.Player.InventoryManager.ActiveHotbarSlot;
			string key = collObj.Code.ToShortString();
			Dictionary<string, MultiTextureMeshRef> meshrefs = ObjectCacheUtil.GetOrCreate(capi, "tongedMeshrefsByOpening", () => new Dictionary<string, MultiTextureMeshRef>());
			if (meshrefs.TryGetValue(key, out var mtmeshref))
			{
				renderinfo.ModelRef = mtmeshref;
				return;
			}
			tongedShape.Base.WithPathPrefixOnce("shapes/").WithPathAppendixOnce(".json");
			Shape shape = capi.Assets.TryGet(tongedShape.Base).ToObject<Shape>();
			capi.Tesselator.TesselateShape(collObj, shape, out var meshdata);
			ItemRenderInfo obj = renderinfo;
			MultiTextureMeshRef modelRef = (meshrefs[key] = capi.Render.UploadMultiTextureMesh(meshdata));
			obj.ModelRef = modelRef;
		}
	}

	public override void OnUnloaded(ICoreAPI api)
	{
		Dictionary<string, MultiTextureMeshRef> dict = ObjectCacheUtil.TryGet<Dictionary<string, MultiTextureMeshRef>>(api, "tongedMeshrefsByOpening");
		if (dict != null)
		{
			foreach (KeyValuePair<string, MultiTextureMeshRef> item in dict)
			{
				item.Value.Dispose();
			}
		}
		base.OnUnloaded(api);
	}
}
