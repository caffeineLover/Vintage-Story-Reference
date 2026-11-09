using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace CombatOverhaul.Utils
{
	// Token: 0x02000027 RID: 39
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class GenericDisplayProto : BlockEntityContainer, ITexPositionSource
	{
		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060002AC RID: 684 RVA: 0x0000B75D File Offset: 0x0000995D
		public virtual string ClassCode
		{
			get
			{
				return this.InventoryClassName;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060002AD RID: 685 RVA: 0x0000B765 File Offset: 0x00009965
		public virtual int DisplayedItems
		{
			get
			{
				return this.Inventory.Count;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060002AE RID: 686 RVA: 0x0000B772 File Offset: 0x00009972
		public Size2i AtlasSize
		{
			get
			{
				return this.ClientApi.BlockTextureAtlas.Size;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060002AF RID: 687 RVA: 0x0000B784 File Offset: 0x00009984
		public virtual string AttributeTransformCode
		{
			get
			{
				return "onDisplayTransform";
			}
		}

		// Token: 0x170000AE RID: 174
		public virtual TextureAtlasPosition this[string textureCode]
		{
			get
			{
				Item item = this.NowTessellatingObj as Item;
				IDictionary<string, CompositeTexture> dictionary;
				if (item == null)
				{
					dictionary = (this.NowTessellatingObj as Block).Textures;
				}
				else
				{
					IDictionary<string, CompositeTexture> textures2 = item.Textures;
					dictionary = textures2;
				}
				IDictionary<string, CompositeTexture> textures = dictionary;
				AssetLocation texturePath = null;
				CompositeTexture tex;
				if (textures.TryGetValue(textureCode, ref tex))
				{
					texturePath = tex.Baked.BakedName;
				}
				if (texturePath == null && textures.TryGetValue("all", ref tex))
				{
					texturePath = tex.Baked.BakedName;
				}
				if (texturePath == null)
				{
					Shape nowTessellatingShape = this.NowTessellatingShape;
					if (nowTessellatingShape != null)
					{
						nowTessellatingShape.Textures.TryGetValue(textureCode, ref texturePath);
					}
				}
				if (texturePath == null)
				{
					texturePath = new AssetLocation(textureCode);
				}
				return this.getOrCreateTexPos(texturePath);
			}
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000B83C File Offset: 0x00009A3C
		public override void Initialize(ICoreAPI api)
		{
			base.Initialize(api);
			this.ClientApi = (api as ICoreClientAPI);
			if (this.ClientApi != null)
			{
				this.updateMeshes(false);
				api.Event.RegisterEventBusListener(new EventBusListenerDelegate(this.OnEventBusEvent), 0.5, null);
			}
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000B88C File Offset: 0x00009A8C
		public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tessThreadTesselator)
		{
			for (int index = 0; index < this.DisplayedItems; index++)
			{
				ItemSlot slot = this.Inventory[index];
				if (!slot.Empty && this.TfMatrices != null)
				{
					mesher.AddMeshData(this.getMesh(slot.Itemstack), this.TfMatrices[index], 1);
				}
			}
			return base.OnTesselation(mesher, tessThreadTesselator);
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000B8EC File Offset: 0x00009AEC
		public virtual void updateMeshes(bool forceUpdate = false)
		{
			if (this.Api == null || this.Api.Side == 1)
			{
				return;
			}
			if (this.DisplayedItems == 0)
			{
				return;
			}
			for (int i = 0; i < this.DisplayedItems; i++)
			{
				this.updateMesh(i, forceUpdate);
			}
			this.TfMatrices = this.genTransformationMatrices();
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000B940 File Offset: 0x00009B40
		public virtual void RegenerateMeshes()
		{
			for (int i = 0; i < this.DisplayedItems; i++)
			{
				if (!this.Inventory[i].Empty)
				{
					string key = this.getMeshCacheKey(this.Inventory[i].Itemstack);
					this.MeshCache.Remove(key);
				}
			}
			this.updateMeshes(false);
			this.MarkDirty(true, null);
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000B9A8 File Offset: 0x00009BA8
		protected virtual void updateMesh(int index, bool forceUpdate = false)
		{
			if (this.Api == null || this.Api.Side == 1)
			{
				return;
			}
			if (this.Inventory[index].Empty)
			{
				return;
			}
			try
			{
				this.getOrCreateMesh(this.Inventory[index].Itemstack, index, forceUpdate);
			}
			catch (Exception exception)
			{
				ICoreAPI api = this.Api;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(53, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Error while refreshing displayed item mesh in slot ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(index);
				defaultInterpolatedStringHandler.AppendLiteral(":\n");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
				LoggerUtil.Warn(api, this, defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000BA58 File Offset: 0x00009C58
		protected void OnEventBusEvent(string eventname, ref EnumHandling handling, IAttribute data)
		{
			if (eventname != "genjsontransform" && eventname != "oncloseedittransforms" && eventname != "onapplytransforms")
			{
				return;
			}
			if (this.Inventory.Empty)
			{
				return;
			}
			for (int i = 0; i < this.DisplayedItems; i++)
			{
				if (!this.Inventory[i].Empty)
				{
					string key = this.getMeshCacheKey(this.Inventory[i].Itemstack);
					this.MeshCache.Remove(key);
				}
			}
			this.updateMeshes(false);
			this.MarkDirty(true, null);
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000BAF3 File Offset: 0x00009CF3
		protected virtual void RedrawAfterReceivingTreeAttributes(IWorldAccessor worldForResolving)
		{
			if (worldForResolving.Side == 2 && this.Api != null)
			{
				this.updateMeshes(false);
				base.MarkDirty(true, null);
			}
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000BB18 File Offset: 0x00009D18
		protected TextureAtlasPosition getOrCreateTexPos(AssetLocation texturePath)
		{
			TextureAtlasPosition texpos = this.ClientApi.BlockTextureAtlas[texturePath];
			int num;
			if (texpos == null && !this.ClientApi.BlockTextureAtlas.GetOrInsertTexture(texturePath, ref num, ref texpos, null, 0f))
			{
				this.ClientApi.World.Logger.Warning("For render in block " + base.Block.Code + ", item {0} defined texture {1}, no such texture found.", new object[]
				{
					this.NowTessellatingObj.Code,
					texturePath
				});
				return this.ClientApi.BlockTextureAtlas.UnknownTexturePosition;
			}
			return texpos;
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000BBB8 File Offset: 0x00009DB8
		protected virtual string getMeshCacheKey(ItemStack stack)
		{
			IContainedMeshSource meshSource = stack.Collectible as IContainedMeshSource;
			if (meshSource != null)
			{
				return meshSource.GetMeshCacheKey(new DummySlot(stack));
			}
			ITreeAttribute attributes = stack.Attributes;
			int renderVariant = (attributes != null) ? attributes.GetInt("renderVariant", 0) : 0;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<AssetLocation>(stack.Collectible.Code);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			defaultInterpolatedStringHandler.AppendFormatted<int>(renderVariant);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060002BA RID: 698 RVA: 0x0000BC30 File Offset: 0x00009E30
		protected Dictionary<string, MeshData> MeshCache
		{
			get
			{
				return ObjectCacheUtil.GetOrCreate<Dictionary<string, MeshData>>(this.Api, "meshesDisplay-" + this.ClassCode, () => new Dictionary<string, MeshData>());
			}
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000BC6C File Offset: 0x00009E6C
		protected MeshData getMesh(ItemStack stack)
		{
			string key = this.getMeshCacheKey(stack);
			MeshData meshdata;
			this.MeshCache.TryGetValue(key, ref meshdata);
			return meshdata;
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000BC94 File Offset: 0x00009E94
		protected virtual MeshData getOrCreateMesh(ItemStack stack, int index, bool forceUpdate = false)
		{
			MeshData mesh = this.getMesh(stack);
			if (mesh != null && !forceUpdate)
			{
				return mesh;
			}
			IContainedMeshSource meshSource = stack.Collectible as IContainedMeshSource;
			if (meshSource != null)
			{
				mesh = meshSource.GenMesh(new DummySlot(stack), this.ClientApi.BlockTextureAtlas, this.Pos);
			}
			if (mesh == null)
			{
				ICoreClientAPI capi = this.Api as ICoreClientAPI;
				if (stack.Class == null)
				{
					mesh = capi.TesselatorManager.GetDefaultBlockMesh(stack.Block).Clone();
				}
				else
				{
					int renderVariant = stack.Attributes.GetInt("renderVariant", 0);
					this.NowTessellatingObj = stack.Collectible;
					this.NowTessellatingShape = null;
					AssetLocation shapeLocation = null;
					CompositeShape compositeShape = null;
					CompositeShape shape = stack.Item.Shape;
					if (((shape != null) ? shape.Base : null) != null && renderVariant < 2)
					{
						this.NowTessellatingShape = capi.TesselatorManager.GetCachedShape(stack.Item.Shape.Base);
						shapeLocation = stack.Item.Shape.Base;
						compositeShape = stack.Item.Shape;
					}
					else if (renderVariant >= 2)
					{
						renderVariant -= 2;
						if (stack.Item.Shape.BakedAlternates.Length > renderVariant)
						{
							this.NowTessellatingShape = capi.TesselatorManager.GetCachedShape(stack.Item.Shape.Alternates[renderVariant].Base);
							shapeLocation = stack.Item.Shape.Alternates[renderVariant].Base;
							compositeShape = stack.Item.Shape.Alternates[renderVariant];
						}
					}
					capi.Tesselator.TesselateShape("", shapeLocation, compositeShape, ref mesh, this, 0, 0, 0, default(int?), null);
					ArrayExtensions.Fill<short>(mesh.RenderPassesAndExtraBits, 2);
				}
			}
			JsonObject attributes = stack.Collectible.Attributes;
			if (attributes != null && attributes[this.AttributeTransformCode].Exists)
			{
				if (this.EditedTransforms.ContainsKey(stack.Collectible.Id))
				{
					ModelTransform transform = this.EditedTransforms[stack.Collectible.Id];
					transform.EnsureDefaultValues();
					mesh.ModelTransform(transform);
				}
				else
				{
					JsonObject attributes2 = stack.Collectible.Attributes;
					ModelTransform transform2 = (attributes2 != null) ? attributes2[this.AttributeTransformCode].AsObject<ModelTransform>(null) : null;
					transform2.EnsureDefaultValues();
					mesh.ModelTransform(transform2);
				}
			}
			else if (this.AttributeTransformCode == "onshelfTransform")
			{
				JsonObject attributes3 = stack.Collectible.Attributes;
				if (attributes3 != null && attributes3["onDisplayTransform"].Exists)
				{
					JsonObject attributes4 = stack.Collectible.Attributes;
					ModelTransform transform3 = (attributes4 != null) ? attributes4["onDisplayTransform"].AsObject<ModelTransform>(null) : null;
					transform3.EnsureDefaultValues();
					mesh.ModelTransform(transform3);
				}
			}
			if (stack.Class == 1 && (stack.Item.Shape == null || stack.Item.Shape.VoxelizeTexture))
			{
				mesh.Rotate(new Vec3f(0.5f, 0.5f, 0.5f), 1.5707964f, 0f, 0f);
				mesh.Scale(new Vec3f(0.5f, 0.5f, 0.5f), 0.33f, 0.33f, 0.33f);
				mesh.Translate(0f, -0.46875f, 0f);
			}
			string key = this.getMeshCacheKey(stack);
			this.MeshCache[key] = mesh;
			return mesh;
		}

		// Token: 0x060002BD RID: 701
		protected abstract float[][] genTransformationMatrices();

		// Token: 0x04000108 RID: 264
		public readonly Dictionary<int, ModelTransform> EditedTransforms = new Dictionary<int, ModelTransform>();

		// Token: 0x04000109 RID: 265
		[Nullable(2)]
		protected CollectibleObject NowTessellatingObj;

		// Token: 0x0400010A RID: 266
		[Nullable(2)]
		protected Shape NowTessellatingShape;

		// Token: 0x0400010B RID: 267
		[Nullable(2)]
		protected ICoreClientAPI ClientApi;

		// Token: 0x0400010C RID: 268
		[Nullable(new byte[]
		{
			2,
			1
		})]
		protected float[][] TfMatrices;
	}
}
