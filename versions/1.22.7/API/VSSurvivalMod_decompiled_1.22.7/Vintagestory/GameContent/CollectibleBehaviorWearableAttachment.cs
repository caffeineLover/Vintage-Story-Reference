using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class CollectibleBehaviorWearableAttachment : CollectibleBehavior, IContainedMeshSource, ITexPositionSource
{
	private ICoreAPI api;

	private bool attachableToEntity;

	private ITextureAtlasAPI curAtlas;

	private Shape nowTesselatingShape;

	public virtual Size2i AtlasSize => curAtlas.Size;

	public virtual TextureAtlasPosition this[string textureCode]
	{
		get
		{
			AssetLocation texturePath = null;
			if ((collObj as Item).Textures.TryGetValue(textureCode, out var tex))
			{
				texturePath = tex.Baked.BakedName;
			}
			if (texturePath == null && (collObj as Item).Textures.TryGetValue("all", out tex))
			{
				texturePath = tex.Baked.BakedName;
			}
			if (texturePath == null)
			{
				nowTesselatingShape?.Textures.TryGetValue(textureCode, out texturePath);
			}
			if (texturePath == null)
			{
				texturePath = new AssetLocation(textureCode);
			}
			return getOrCreateTexPos(texturePath);
		}
	}

	public CollectibleBehaviorWearableAttachment(CollectibleObject collObj)
		: base(collObj)
	{
	}

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		this.api = api;
		attachableToEntity = IAttachableToEntity.FromCollectible(collObj) != null;
	}

	public override void OnUnloaded(ICoreAPI api)
	{
		Dictionary<string, MultiTextureMeshRef> meshRefs = ObjectCacheUtil.TryGet<Dictionary<string, MultiTextureMeshRef>>(api, "wearableAttachmentMeshRefs");
		if (meshRefs == null || meshRefs.Count <= 0)
		{
			return;
		}
		foreach (MultiTextureMeshRef value in meshRefs.Values)
		{
			value?.Dispose();
		}
		ObjectCacheUtil.Delete(api, "wearableAttachmentMeshRefs");
	}

	protected virtual TextureAtlasPosition getOrCreateTexPos(AssetLocation texturePath)
	{
		ICoreClientAPI capi = api as ICoreClientAPI;
		curAtlas.GetOrInsertTexture(texturePath, out var _, out var texpos, delegate
		{
			IAsset asset = capi.Assets.TryGet(texturePath.Clone().WithPathPrefixOnce("textures/").WithPathAppendixOnce(".png"));
			if (asset != null)
			{
				return asset.ToBitmap(capi);
			}
			capi.World.Logger.Warning("Item {0} defined texture {1}, not no such texture found.", collObj.Code, texturePath);
			return (IBitmap)null;
		}, 0.1f);
		return texpos;
	}

	public virtual MeshData GenMesh(ItemSlot slot, ITextureAtlasAPI targetAtlas, BlockPos forBlockPos)
	{
		MeshData mesh = null;
		string obj = (slot as ItemSlotDisplay)?.DisplayCategory ?? "default";
		ItemStack itemstack = slot.Itemstack;
		if (obj == "shelf" && IsFoldable(slot.Itemstack))
		{
			mesh = genFoldedMesh(itemstack, targetAtlas, forBlockPos);
		}
		if (mesh == null)
		{
			ICoreClientAPI capi = api as ICoreClientAPI;
			if (targetAtlas == capi.ItemTextureAtlas)
			{
				ITexPositionSource texSource = capi.Tesselator.GetTextureSource(itemstack.Item);
				return genFullBodyMesh(capi, itemstack, texSource);
			}
			curAtlas = targetAtlas;
			mesh = genFullBodyMesh(api as ICoreClientAPI, itemstack, this);
		}
		mesh.RenderPassesAndExtraBits.Fill((short)1);
		return mesh;
	}

	public virtual bool IsFoldable(ItemStack itemstack)
	{
		return itemstack.ItemAttributes.IsTrue("isFoldable");
	}

	private MeshData genFoldedMesh(ItemStack itemstack, ITextureAtlasAPI targetAtlas, BlockPos forBlockPos)
	{
		DisplayableAttributes displayableProps = itemstack.Collectible.GetCollectibleInterface<IDisplayableProps>()?.GetDisplayableProps(new DummySlot(itemstack), "shelf");
		if (displayableProps == null)
		{
			displayableProps = itemstack.ItemAttributes["displayable"]["shelf"].AsObject<DisplayableAttributes>();
		}
		if (displayableProps == null)
		{
			return null;
		}
		CompositeShape cshape = displayableProps.Shape;
		ICoreClientAPI obj = api as ICoreClientAPI;
		AssetLocation shapePath = new AssetLocation(string.Concat("shapes/", cshape.Base, ".json"));
		Shape shape = Shape.TryGet(obj, shapePath);
		obj.Tesselator.TesselateShape(itemstack.Collectible, shape, out var meshdata);
		return meshdata;
	}

	public virtual string GetMeshCacheKey(ItemSlot slot)
	{
		string slotType = (slot as ItemSlotDisplay)?.DisplayCategory ?? "default";
		return GetMeshCacheKey(slot.Itemstack, slotType);
	}

	public virtual string GetMeshCacheKey(ItemStack stack, string slotType)
	{
		return slotType + "-wearableAttachmentModelRef-" + stack.Collectible.Code.ToString();
	}

	public override void OnBeforeRender(ICoreClientAPI capi, ItemStack itemstack, EnumItemRenderTarget target, ref ItemRenderInfo renderinfo)
	{
		if (attachableToEntity)
		{
			Dictionary<string, MultiTextureMeshRef> meshrefs = ObjectCacheUtil.GetOrCreate(capi, "wearableAttachmentMeshRefs", () => new Dictionary<string, MultiTextureMeshRef>());
			string key = GetMeshCacheKey(itemstack, "default");
			if (!meshrefs.TryGetValue(key, out renderinfo.ModelRef))
			{
				ITexPositionSource texSource = capi.Tesselator.GetTextureSource(itemstack.Item);
				MeshData mesh = genFullBodyMesh(capi, itemstack, texSource);
				ItemRenderInfo obj = renderinfo;
				MultiTextureMeshRef modelRef = (meshrefs[key] = ((mesh == null) ? renderinfo.ModelRef : capi.Render.UploadMultiTextureMesh(mesh)));
				obj.ModelRef = modelRef;
			}
			if (collObj.Attributes["visibleDamageEffect"].AsBool())
			{
				renderinfo.DamageEffect = Math.Max(0f, 1f - (float)collObj.GetRemainingDurability(itemstack) / (float)collObj.GetMaxDurability(itemstack) * 1.1f);
			}
		}
	}

	protected virtual MeshData genFullBodyMesh(ICoreClientAPI capi, ItemStack itemstack, ITexPositionSource texSource)
	{
		JsonObject attrObj = itemstack.Collectible.Attributes;
		EntityProperties? entityType = capi.World.GetEntityType(new AssetLocation(attrObj?["wearerEntityCode"].ToString() ?? "player"));
		Shape entityShape = entityType.Client.LoadedShape;
		AssetLocation shapePathForLogging = entityType.Client.Shape.Base;
		Shape newShape = (attachableToEntity ? new Shape
		{
			Elements = entityShape.CloneElements(),
			Animations = entityShape.CloneAnimations(),
			AnimationsByCrc32 = entityShape.AnimationsByCrc32,
			JointsById = entityShape.JointsById,
			TextureWidth = entityShape.TextureWidth,
			TextureHeight = entityShape.TextureHeight,
			Textures = null
		} : entityShape);
		MeshData meshdata;
		if (attrObj["wearableInvShape"].Exists)
		{
			AssetLocation shapePath = new AssetLocation("shapes/" + attrObj["wearableInvShape"]?.ToString() + ".json");
			Shape shape = Shape.TryGet(capi, shapePath);
			capi.Tesselator.TesselateShape(itemstack.Collectible, shape, out meshdata);
		}
		else
		{
			CompositeShape compArmorShape = (attrObj["attachShape"].Exists ? attrObj["attachShape"].AsObject<CompositeShape>(null, itemstack.Collectible.Code.Domain) : ((itemstack.Class == EnumItemClass.Item) ? itemstack.Item.Shape : itemstack.Block.Shape));
			if (compArmorShape == null)
			{
				capi.World.Logger.Warning("Wearable shape {0} {1} does not define a shape through either the shape property or the attachShape Attribute. Item will be invisible.", itemstack.Class, itemstack.Collectible.Code);
				return null;
			}
			AssetLocation shapePath2 = compArmorShape.Base.CopyWithPathPrefixAndAppendixOnce("shapes/", ".json");
			Shape armorShape = Shape.TryGet(capi, shapePath2);
			if (armorShape == null)
			{
				capi.World.Logger.Warning("Wearable shape {0} defined in {1} {2} not found or errored, was supposed to be at {3}. Item will be invisible.", compArmorShape.Base, itemstack.Class, itemstack.Collectible.Code, shapePath2);
				return null;
			}
			newShape.StepParentShape(armorShape, shapePath2.ToShortString(), shapePathForLogging.ToShortString(), capi.Logger, delegate
			{
			});
			if (compArmorShape.Overlays != null)
			{
				CompositeShape[] overlays = compArmorShape.Overlays;
				foreach (CompositeShape overlay in overlays)
				{
					Shape oshape = Shape.TryGet(capi, overlay.Base.CopyWithPathPrefixAndAppendixOnce("shapes/", ".json"));
					if (oshape == null)
					{
						capi.World.Logger.Warning("Wearable shape {0} overlay {4} defined in {1} {2} not found or errored, was supposed to be at {3}. Item will be invisible.", compArmorShape.Base, itemstack.Class, itemstack.Collectible.Code, shapePath2, overlay.Base);
					}
					else
					{
						newShape.StepParentShape(oshape, overlay.Base.ToShortString(), shapePathForLogging.ToShortString(), capi.Logger, delegate
						{
						});
					}
				}
			}
			nowTesselatingShape = newShape;
			capi.Tesselator.TesselateShapeWithJointIds("entity", newShape, out meshdata, texSource, new Vec3f());
			nowTesselatingShape = null;
		}
		return meshdata;
	}
}
