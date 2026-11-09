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
	// Token: 0x0200013F RID: 319
	[NullableContext(1)]
	[Nullable(0)]
	public class WearableAttachment : Item, IContainedMeshSource, IAttachableToEntity, ITexPositionSource
	{
		// Token: 0x06000F57 RID: 3927 RVA: 0x0004B1CF File Offset: 0x000493CF
		public override void OnLoaded(ICoreAPI api)
		{
			this.attachableToEntity = (IAttachableToEntity.FromCollectible(this) != null);
			base.OnLoaded(api);
		}

		// Token: 0x06000F58 RID: 3928 RVA: 0x0004B1E8 File Offset: 0x000493E8
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

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06000F59 RID: 3929 RVA: 0x0004B26C File Offset: 0x0004946C
		public Size2i AtlasSize
		{
			get
			{
				return this.curAtlas.Size;
			}
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x06000F5A RID: 3930 RVA: 0x0004B279 File Offset: 0x00049479
		// (set) Token: 0x06000F5B RID: 3931 RVA: 0x0004B281 File Offset: 0x00049481
		public int RequiresBehindSlots { get; set; }

		// Token: 0x170003DF RID: 991
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

		// Token: 0x06000F5D RID: 3933 RVA: 0x0004B31C File Offset: 0x0004951C
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

		// Token: 0x06000F5E RID: 3934 RVA: 0x0004B378 File Offset: 0x00049578
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

		// Token: 0x06000F5F RID: 3935 RVA: 0x0004B3DE File Offset: 0x000495DE
		MeshData IContainedMeshSource.GenMesh(ItemSlot inSlot, ITextureAtlasAPI targetAtlas, BlockPos atBlockPos)
		{
			return this.GenMesh(inSlot.Itemstack, targetAtlas, atBlockPos);
		}

		// Token: 0x06000F60 RID: 3936 RVA: 0x0004B3EE File Offset: 0x000495EE
		public virtual string GetMeshCacheKey(ItemStack itemstack)
		{
			return "wearableAttachmentModelRef-" + itemstack.Collectible.Code.ToString();
		}

		// Token: 0x06000F61 RID: 3937 RVA: 0x0004B40A File Offset: 0x0004960A
		string IContainedMeshSource.GetMeshCacheKey(ItemSlot inSlot)
		{
			return this.GetMeshCacheKey(inSlot.Itemstack);
		}

		// Token: 0x06000F62 RID: 3938 RVA: 0x0004B418 File Offset: 0x00049618
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

		// Token: 0x06000F63 RID: 3939 RVA: 0x0004B504 File Offset: 0x00049704
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

		// Token: 0x06000F64 RID: 3940 RVA: 0x0004B8B5 File Offset: 0x00049AB5
		public bool IsAttachable(Entity toEntity, ItemStack itemStack)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000F65 RID: 3941 RVA: 0x0004B8BC File Offset: 0x00049ABC
		public void CollectTextures(ItemStack stack, Shape shape, string texturePrefixCode, Dictionary<string, CompositeTexture> intoDict)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000F66 RID: 3942 RVA: 0x0004B8C3 File Offset: 0x00049AC3
		public string GetCategoryCode(ItemStack stack)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000F67 RID: 3943 RVA: 0x0004B8CA File Offset: 0x00049ACA
		public CompositeShape GetAttachedShape(ItemStack stack, string slotCode)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000F68 RID: 3944 RVA: 0x0004B8D1 File Offset: 0x00049AD1
		public string[] GetDisableElements(ItemStack stack)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000F69 RID: 3945 RVA: 0x0004B8D8 File Offset: 0x00049AD8
		public string[] GetKeepElements(ItemStack stack)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000F6A RID: 3946 RVA: 0x0004B8DF File Offset: 0x00049ADF
		public string GetTexturePrefixCode(ItemStack stack)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000F6B RID: 3947 RVA: 0x0004B8E8 File Offset: 0x00049AE8
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

		// Token: 0x04000750 RID: 1872
		private bool attachableToEntity;

		// Token: 0x04000751 RID: 1873
		private ITextureAtlasAPI curAtlas;

		// Token: 0x04000752 RID: 1874
		private Shape nowTesselatingShape;
	}
}
