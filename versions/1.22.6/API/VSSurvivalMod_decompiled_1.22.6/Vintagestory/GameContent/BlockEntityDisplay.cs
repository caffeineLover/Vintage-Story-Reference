using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public abstract class BlockEntityDisplay : BlockEntityContainer, ITexPositionSource
{
	protected CollectibleObject nowTesselatingObj;

	protected Shape nowTesselatingShape;

	protected ICoreClientAPI capi;

	protected float[][] tfMatrices;

	public virtual string ClassCode => InventoryClassName;

	public virtual int DisplayedItems => Inventory.Count;

	public Size2i AtlasSize => capi.BlockTextureAtlas.Size;

	public virtual string AttributeTransformCode => "onDisplayTransform";

	public virtual TextureAtlasPosition this[string textureCode]
	{
		get
		{
			IDictionary<string, CompositeTexture> dictionary;
			if (!(nowTesselatingObj is Item item))
			{
				dictionary = (nowTesselatingObj as Block).Textures;
			}
			else
			{
				IDictionary<string, CompositeTexture> textures = item.Textures;
				dictionary = textures;
			}
			IDictionary<string, CompositeTexture> textures2 = dictionary;
			AssetLocation texturePath = null;
			if (textures2.TryGetValue(textureCode, out var tex))
			{
				texturePath = tex.Baked.BakedName;
			}
			if (texturePath == null && textures2.TryGetValue("all", out tex))
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

	protected virtual string slotType => "default";

	protected Dictionary<string, MeshData> MeshCache => ObjectCacheUtil.GetOrCreate(Api, "meshesDisplay-" + ClassCode, () => new Dictionary<string, MeshData>());

	protected TextureAtlasPosition getOrCreateTexPos(AssetLocation texturePath)
	{
		TextureAtlasPosition texpos = capi.BlockTextureAtlas[texturePath];
		if (texpos == null && !capi.BlockTextureAtlas.GetOrInsertTexture(texturePath, out var _, out texpos))
		{
			capi.World.Logger.Warning(string.Concat("For render in block ", base.Block.Code, ", item {0} defined texture {1}, no such texture found."), nowTesselatingObj.Code, texturePath);
			return capi.BlockTextureAtlas.UnknownTexturePosition;
		}
		return texpos;
	}

	public override void Initialize(ICoreAPI api)
	{
		base.Initialize(api);
		capi = api as ICoreClientAPI;
		if (capi != null)
		{
			MarkMeshesDirty();
			api.Event.RegisterEventBusListener(OnEventBusEvent);
		}
	}

	private void OnEventBusEvent(string eventname, ref EnumHandling handling, IAttribute data)
	{
		if ((eventname != "genjsontransform" && eventname != "oncloseedittransforms" && eventname != "onapplytransforms") || capi == null || Inventory.Empty)
		{
			return;
		}
		EntityPos PlayerPos = capi.World.Player.Entity.Pos;
		if (Pos.DistanceTo(PlayerPos.X, PlayerPos.Y, PlayerPos.Z) > 20f)
		{
			return;
		}
		int DisplayedItems = this.DisplayedItems;
		for (int i = 0; i < DisplayedItems; i++)
		{
			if (Inventory[i].Itemstack != null)
			{
				string key = getMeshCacheKey(Inventory[i]);
				MeshCache.Remove(key);
			}
		}
		MarkMeshesDirty();
		Api.World.BlockAccessor.MarkBlockDirty(Pos);
	}

	protected virtual void RedrawAfterReceivingTreeAttributes(IWorldAccessor worldForResolving)
	{
		if (worldForResolving.Side == EnumAppSide.Client && Api != null)
		{
			MarkMeshesDirty();
			Api.World.BlockAccessor.MarkBlockDirty(Pos);
		}
	}

	public virtual void MarkMeshesDirty()
	{
		tfMatrices = null;
	}

	public virtual void updateMeshes()
	{
		if (Api == null || Api.Side == EnumAppSide.Server)
		{
			return;
		}
		int DisplayedItems = this.DisplayedItems;
		if (DisplayedItems != 0)
		{
			for (int i = 0; i < DisplayedItems; i++)
			{
				updateMesh(i);
			}
			tfMatrices = genTransformationMatrices();
		}
	}

	protected virtual void updateMesh(int index)
	{
		if (Api != null && Api.Side != EnumAppSide.Server)
		{
			ItemStack stack = Inventory[index].Itemstack;
			if (stack != null && !(stack.Collectible?.Code == null))
			{
				getOrCreateMesh(Inventory[index], index);
			}
		}
	}

	protected virtual string getMeshCacheKey(ItemSlot slot)
	{
		IContainedMeshSource meshSource = slot.Itemstack.Collectible?.GetCollectibleInterface<IContainedMeshSource>();
		if (meshSource != null)
		{
			return meshSource.GetMeshCacheKey(slot);
		}
		return slot.Itemstack.Collectible.Code.ToString();
	}

	protected MeshData getMesh(ItemSlot slot)
	{
		string key = getMeshCacheKey(slot);
		MeshCache.TryGetValue(key, out var meshdata);
		return meshdata;
	}

	protected virtual MeshData getOrCreateMesh(ItemSlot slot, int index)
	{
		MeshData mesh = getMesh(slot);
		if (mesh != null)
		{
			return mesh;
		}
		ItemStack stack = slot.Itemstack;
		CompositeShape customShape = stack.Collectible.Attributes?["displayedShape"].AsObject<CompositeShape>(null, stack.Collectible.Code.Domain);
		if (customShape != null)
		{
			string customkey = "displayedShape-" + customShape.ToString();
			mesh = ObjectCacheUtil.GetOrCreate(capi, customkey, () => capi.TesselatorManager.CreateMesh("displayed item shape", customShape, (Shape shape, string name) => new ContainedTextureSource(capi, capi.BlockTextureAtlas, shape.Textures, $"For displayed item {stack.Collectible.Code}")));
		}
		else
		{
			IContainedMeshSource meshSource = stack.Collectible?.GetCollectibleInterface<IContainedMeshSource>();
			if (meshSource != null)
			{
				mesh = meshSource.GenMesh(slot, capi.BlockTextureAtlas, Pos);
			}
		}
		if (mesh == null)
		{
			mesh = getDefaultMesh(stack);
		}
		applyDefaultTranforms(stack, mesh);
		string key = getMeshCacheKey(slot);
		MeshCache[key] = mesh;
		return mesh;
	}

	protected void applyDefaultTranforms(ItemStack stack, MeshData mesh)
	{
		ModelTransform transform = stack.Collectible.Attributes?[AttributeTransformCode].AsObject<ModelTransform>();
		if (AttributeTransformCode == "onshelfTransform")
		{
			transform = stack.Collectible.GetCollectibleInterface<IShelvable>()?.GetOnShelfTransform(stack) ?? transform;
			if (transform == null)
			{
				transform = stack.Collectible.Attributes?["onDisplayTransform"].AsObject<ModelTransform>();
			}
		}
		if (transform != null)
		{
			transform.EnsureDefaultValues();
			mesh.ModelTransform(transform);
		}
		if (stack.Class == EnumItemClass.Item && (stack.Item.Shape == null || stack.Item.Shape.VoxelizeTexture))
		{
			mesh.Rotate((float)Math.PI / 2f, 0f, 0f);
			mesh.Scale(0.33f, 0.33f, 0.33f);
			mesh.Translate(0f, -15f / 32f, 0f);
		}
	}

	protected MeshData getDefaultMesh(ItemStack stack)
	{
		ICoreClientAPI capi = Api as ICoreClientAPI;
		if (stack.Class == EnumItemClass.Block)
		{
			return capi.TesselatorManager.GetDefaultBlockMesh(stack.Block).Clone();
		}
		nowTesselatingObj = stack.Collectible;
		nowTesselatingShape = null;
		if (stack.Item.Shape?.Base != null)
		{
			nowTesselatingShape = capi.TesselatorManager.GetCachedShape(stack.Item.Shape.Base);
		}
		capi.Tesselator.TesselateItem(stack.Item, out var mesh, this);
		mesh.RenderPassesAndExtraBits.Fill((short)2);
		return mesh;
	}

	protected abstract float[][] genTransformationMatrices();

	public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tessThreadTesselator)
	{
		if (tfMatrices == null)
		{
			updateMeshes();
		}
		for (int index = 0; index < DisplayedItems; index++)
		{
			ItemSlot slot = Inventory[index];
			if (!slot.Empty && tfMatrices != null && !(slot.Itemstack.Collectible?.Code == null))
			{
				mesher.AddMeshData(getMesh(slot), tfMatrices[index]);
			}
		}
		return base.OnTesselation(mesher, tessThreadTesselator);
	}
}
