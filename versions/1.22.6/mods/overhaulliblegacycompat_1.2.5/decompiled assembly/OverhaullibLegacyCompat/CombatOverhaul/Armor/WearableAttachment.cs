using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000133 RID: 307
	[NullableContext(1)]
	[Nullable(0)]
	public class WearableAttachment : Item, IContainedMeshSource, IAttachableToEntity, ITexPositionSource
	{
		// Token: 0x06000EE3 RID: 3811 RVA: 0x000498CF File Offset: 0x00047ACF
		public override void OnLoaded(ICoreAPI api)
		{
			this.attachableToEntity = (IAttachableToEntity.FromCollectible(this) != null);
			base.OnLoaded(api);
		}

		// Token: 0x06000EE4 RID: 3812 RVA: 0x000498E8 File Offset: 0x00047AE8
		public override void OnUnloaded(ICoreAPI api)
		{
			Dictionary<string, MultiTextureMeshRef> meshRefs = ObjectCacheUtil.TryGet<Dictionary<string, MultiTextureMeshRef>>(api, "wearableAttachmentMeshRefs");
			if (meshRefs != null && meshRefs.Count > 0)
			{
				foreach (KeyValuePair<string, MultiTextureMeshRef> keyValuePair in meshRefs)
				{
					string text;
					MultiTextureMeshRef multiTextureMeshRef;
					keyValuePair.Deconstruct(ref text, ref multiTextureMeshRef);
					multiTextureMeshRef.Dispose();
				}
				ObjectCacheUtil.Delete(api, "wearableAttachmentMeshRefs");
			}
			base.OnUnloaded(api);
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06000EE5 RID: 3813 RVA: 0x0004996C File Offset: 0x00047B6C
		public Size2i AtlasSize
		{
			get
			{
				return this.curAtlas.Size;
			}
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06000EE6 RID: 3814 RVA: 0x00049979 File Offset: 0x00047B79
		// (set) Token: 0x06000EE7 RID: 3815 RVA: 0x00049981 File Offset: 0x00047B81
		public int RequiresBehindSlots { get; set; }

		// Token: 0x170003BD RID: 957
		public virtual TextureAtlasPosition this[string textureCode]
		{
			get
			{
				AssetLocation texturePath = null;
				CompositeTexture tex;
				if (this.Textures.TryGetValue(textureCode, ref tex))
				{
					texturePath = tex.Baked.BakedName;
				}
				if (texturePath == null && this.Textures.TryGetValue("all", ref tex))
				{
					texturePath = tex.Baked.BakedName;
				}
				if (texturePath == null)
				{
					Shape shape = this.nowTesselatingShape;
					if (shape != null)
					{
						shape.Textures.TryGetValue(textureCode, ref texturePath);
					}
				}
				if (texturePath == null)
				{
					texturePath = new AssetLocation(textureCode);
				}
				return this.getOrCreateTexPos(texturePath);
			}
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x00049A1C File Offset: 0x00047C1C
		protected TextureAtlasPosition getOrCreateTexPos(AssetLocation texturePath)
		{
			ICoreClientAPI capi = this.api as ICoreClientAPI;
			int num;
			TextureAtlasPosition texpos;
			this.curAtlas.GetOrInsertTexture(texturePath, ref num, ref texpos, delegate()
			{
				IAsset texAsset = capi.Assets.TryGet(texturePath.Clone().WithPathPrefixOnce("textures/").WithPathAppendixOnce(".png"), true);
				if (texAsset != null)
				{
					return texAsset.ToBitmap(capi);
				}
				capi.World.Logger.Warning("Item {0} defined texture {1}, not no such texture found.", new object[]
				{
					this.Code,
					texturePath
				});
				return null;
			}, 0.1f);
			return texpos;
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x00049A78 File Offset: 0x00047C78
		public MeshData GenMesh(ItemStack itemstack, ITextureAtlasAPI targetAtlas, BlockPos forBlockPos = null)
		{
			ICoreClientAPI capi = this.api as ICoreClientAPI;
			if (targetAtlas == capi.ItemTextureAtlas)
			{
				ITexPositionSource texSource = capi.Tesselator.GetTextureSource(itemstack.Item, false);
				return this.genMesh(capi, itemstack, texSource);
			}
			this.curAtlas = targetAtlas;
			MeshData meshData = this.genMesh(this.api as ICoreClientAPI, itemstack, this);
			ArrayExtensions.Fill<short>(meshData.RenderPassesAndExtraBits, 1);
			return meshData;
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x00049ADE File Offset: 0x00047CDE
		MeshData IContainedMeshSource.GenMesh(ItemSlot inSlot, ITextureAtlasAPI targetAtlas, BlockPos atBlockPos)
		{
			return this.GenMesh(inSlot.Itemstack, targetAtlas, atBlockPos);
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x00049AEE File Offset: 0x00047CEE
		public virtual string GetMeshCacheKey(ItemStack itemstack)
		{
			return "wearableAttachmentModelRef-" + itemstack.Collectible.Code.ToString();
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x00049B0A File Offset: 0x00047D0A
		string IContainedMeshSource.GetMeshCacheKey(ItemSlot inSlot)
		{
			return this.GetMeshCacheKey(inSlot.Itemstack);
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x00049B18 File Offset: 0x00047D18
		public override void OnBeforeRender(ICoreClientAPI capi, ItemStack itemstack, EnumItemRenderTarget target, ref ItemRenderInfo renderinfo)
		{
			if (!this.attachableToEntity)
			{
				return;
			}
			Dictionary<string, MultiTextureMeshRef> meshrefs = ObjectCacheUtil.GetOrCreate<Dictionary<string, MultiTextureMeshRef>>(capi, "wearableAttachmentMeshRefs", () => new Dictionary<string, MultiTextureMeshRef>());
			string key = this.GetMeshCacheKey(itemstack);
			if (!meshrefs.TryGetValue(key, ref renderinfo.ModelRef))
			{
				ITexPositionSource texSource = capi.Tesselator.GetTextureSource(itemstack.Item, false);
				MeshData mesh = this.genMesh(capi, itemstack, texSource);
				renderinfo.ModelRef = (meshrefs[key] = ((mesh == null) ? renderinfo.ModelRef : capi.Render.UploadMultiTextureMesh(mesh)));
			}
			if (this.Attributes["visibleDamageEffect"].AsBool(false))
			{
				renderinfo.DamageEffect = Math.Max(0f, 1f - (float)this.GetRemainingDurability(itemstack) / (float)this.GetMaxDurability(itemstack) * 1.1f);
			}
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x00049C04 File Offset: 0x00047E04
		protected MeshData genMesh(ICoreClientAPI capi, ItemStack itemstack, ITexPositionSource texSource)
		{
			JsonObject attrObj = itemstack.Collectible.Attributes;
			EntityProperties entityType = capi.World.GetEntityType(new AssetLocation(((attrObj != null) ? attrObj["wearerEntityCode"].ToString() : null) ?? "player"));
			Shape entityShape = entityType.Client.LoadedShape;
			AssetLocation shapePathForLogging = entityType.Client.Shape.Base;
			Shape newShape;
			if (!this.attachableToEntity)
			{
				newShape = entityShape;
			}
			else
			{
				newShape = new Shape
				{
					Elements = entityShape.CloneElements(),
					Animations = entityShape.CloneAnimations(),
					AnimationsByCrc32 = entityShape.AnimationsByCrc32,
					JointsById = entityShape.JointsById,
					TextureWidth = entityShape.TextureWidth,
					TextureHeight = entityShape.TextureHeight,
					Textures = null
				};
			}
			MeshData meshdata;
			if (attrObj["wearableInvShape"].Exists)
			{
				string text = "shapes/";
				JsonObject jsonObject = attrObj["wearableInvShape"];
				AssetLocation shapePath = new AssetLocation(text + ((jsonObject != null) ? jsonObject.ToString() : null) + ".json");
				Shape shape = Shape.TryGet(capi, shapePath);
				capi.Tesselator.TesselateShape(itemstack.Collectible, shape, ref meshdata, null, default(int?), null);
			}
			else
			{
				CompositeShape compArmorShape = (!attrObj["attachShape"].Exists) ? ((itemstack.Class == 1) ? itemstack.Item.Shape : itemstack.Block.Shape) : attrObj["attachShape"].AsObject<CompositeShape>(null, itemstack.Collectible.Code.Domain);
				if (compArmorShape == null)
				{
					capi.World.Logger.Warning("Wearable shape {0} {1} does not define a shape through either the shape property or the attachShape Attribute. Item will be invisible.", new object[]
					{
						itemstack.Class,
						itemstack.Collectible.Code
					});
					return null;
				}
				AssetLocation shapePath2 = compArmorShape.Base.CopyWithPath("shapes/" + compArmorShape.Base.Path + ".json");
				Shape armorShape = Shape.TryGet(capi, shapePath2);
				if (armorShape == null)
				{
					capi.World.Logger.Warning("Wearable shape {0} defined in {1} {2} not found or errored, was supposed to be at {3}. Item will be invisible.", new object[]
					{
						compArmorShape.Base,
						itemstack.Class,
						itemstack.Collectible.Code,
						shapePath2
					});
					return null;
				}
				newShape.StepParentShape(armorShape, shapePath2.ToShortString(), shapePathForLogging.ToShortString(), capi.Logger, delegate(string key, AssetLocation code)
				{
				});
				if (compArmorShape.Overlays != null)
				{
					foreach (CompositeShape overlay in compArmorShape.Overlays)
					{
						Shape oshape = Shape.TryGet(capi, overlay.Base.CopyWithPath("shapes/" + overlay.Base.Path + ".json"));
						if (oshape == null)
						{
							capi.World.Logger.Warning("Wearable shape {0} overlay {4} defined in {1} {2} not found or errored, was supposed to be at {3}. Item will be invisible.", new object[]
							{
								compArmorShape.Base,
								itemstack.Class,
								itemstack.Collectible.Code,
								shapePath2,
								overlay.Base
							});
						}
						else
						{
							newShape.StepParentShape(oshape, overlay.Base.ToShortString(), shapePathForLogging.ToShortString(), capi.Logger, delegate(string key, AssetLocation Code)
							{
							});
						}
					}
				}
				this.nowTesselatingShape = newShape;
				capi.Tesselator.TesselateShapeWithJointIds("entity", newShape, ref meshdata, texSource, new Vec3f(), default(int?), null);
				this.nowTesselatingShape = null;
			}
			return meshdata;
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x00049FB5 File Offset: 0x000481B5
		public bool IsAttachable(Entity toEntity, ItemStack itemStack)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000EF1 RID: 3825 RVA: 0x00049FBC File Offset: 0x000481BC
		public void CollectTextures(ItemStack stack, Shape shape, string texturePrefixCode, Dictionary<string, CompositeTexture> intoDict)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000EF2 RID: 3826 RVA: 0x00049FC3 File Offset: 0x000481C3
		public string GetCategoryCode(ItemStack stack)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000EF3 RID: 3827 RVA: 0x00049FCA File Offset: 0x000481CA
		public CompositeShape GetAttachedShape(ItemStack stack, string slotCode)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000EF4 RID: 3828 RVA: 0x00049FD1 File Offset: 0x000481D1
		public string[] GetDisableElements(ItemStack stack)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000EF5 RID: 3829 RVA: 0x00049FD8 File Offset: 0x000481D8
		public string[] GetKeepElements(ItemStack stack)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000EF6 RID: 3830 RVA: 0x00049FDF File Offset: 0x000481DF
		public string GetTexturePrefixCode(ItemStack stack)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000EF7 RID: 3831 RVA: 0x00049FE8 File Offset: 0x000481E8
		protected static IAttachableToEntity FromAttributes(CollectibleObject cobj)
		{
			JsonObject attributes2 = cobj.Attributes;
			AttributeAttachableToEntity attributeAttachableToEntity = (attributes2 != null) ? attributes2["attachableToEntity"].AsObject<AttributeAttachableToEntity>(null, cobj.Code.Domain) : null;
			if (attributeAttachableToEntity == null)
			{
				JsonObject attributes = cobj.Attributes;
				if (attributes != null && attributes["wearableAttachment"].Exists)
				{
					return new AttributeAttachableToEntity
					{
						CategoryCode = cobj.Attributes["clothescategory"].AsString(null),
						KeepElements = cobj.Attributes["keepElements"].AsStringArray(null, "game"),
						DisableElements = cobj.Attributes["disableElements"].AsStringArray(null, "game")
					};
				}
			}
			return attributeAttachableToEntity;
		}

		// Token: 0x0400072A RID: 1834
		private bool attachableToEntity;

		// Token: 0x0400072B RID: 1835
		private ITextureAtlasAPI curAtlas;

		// Token: 0x0400072C RID: 1836
		private Shape nowTesselatingShape;
	}
}
