using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class BEBehaviorMannequin : BEBehaviorRotatablePlaceable, IInteractable, ITexPositionSource, IRotatablePlaceable
{
	protected static AssetLocation ttas = new AssetLocation("block/transparent");

	protected InWorldContainer container;

	protected string InventoryClassName = "mannequin";

	protected ICoreClientAPI? capi;

	protected MeshData? mesh;

	protected IDictionary<string, CompositeTexture>? collectedTextures;

	protected Vec3f? shapeOffset;

	protected string[] allowedDresstypes;

	protected int[] placementOrder;

	protected int placementIndex;

	private CompositeShape? dressableEntityShape;

	public InventoryBase Inventory { get; set; }

	public Size2i AtlasSize => capi.BlockTextureAtlas.Size;

	public TextureAtlasPosition this[string textureCode]
	{
		get
		{
			if (textureCode == "seraph")
			{
				capi.BlockTextureAtlas.GetOrInsertTexture(ttas, out var _, out var texpos);
				return texpos;
			}
			return capi.BlockTextureAtlas.Positions[collectedTextures[textureCode].Baked.TextureSubId];
		}
	}

	public BEBehaviorMannequin(BlockEntity blockentity)
		: base(blockentity)
	{
		allowedDresstypes = base.Block.Attributes?["allowedDresstypes"].AsArray<string>();
		if (allowedDresstypes == null)
		{
			Api.Logger.Warning(string.Concat("Mannequin block ", base.Block.Code, " has no allowed dress types."));
			allowedDresstypes = Array.Empty<string>();
		}
		string[] array = allowedDresstypes;
		int qslots = ((array != null) ? array.Length : 0);
		placementOrder = new int[qslots].Fill(-1);
		placementIndex = 0;
		Inventory = new InventoryGeneric(qslots, InventoryClassName + "-0", null, (int id, InventoryGeneric inv) => new ItemSlotWearable(inv, new string[1] { allowedDresstypes[id] }));
		container = new InWorldContainer(() => Inventory, "inventory");
	}

	public override void Initialize(ICoreAPI api, JsonObject properties)
	{
		base.Initialize(api, properties);
		capi = api as ICoreClientAPI;
		dressableEntityShape = properties["dressableEntityShape"].AsObject<CompositeShape>();
		shapeOffset = properties["shapeOffset"]?.AsObject<Vec3f>();
		string inventoryClassName = InventoryClassName + "-" + base.Pos;
		Inventory.LateInitialize(inventoryClassName, api);
		Inventory.Pos = base.Pos;
		container.Init(Api, () => base.Pos, delegate
		{
			Blockentity.MarkDirty(redrawOnClient: true);
		});
		regenMesh();
	}

	protected virtual void regenMesh()
	{
		if (dressableEntityShape == null || capi == null)
		{
			return;
		}
		AssetLocation loc = dressableEntityShape.Base.CopyWithPathPrefixAndAppendixOnce("shapes/", ".json");
		IAsset asset = capi.Assets.TryGet(loc);
		if (asset == null)
		{
			capi.World.Logger.Warning(string.Concat(new string[5]
			{
				"Mannequin block ",
				base.Block.Code,
				" dressable shape ",
				loc,
				" not found."
			}));
			return;
		}
		Shape shape = asset.ToObject<Shape>();
		string[] willDeleEles = new string[0];
		addGearToShape(shape, dressableEntityShape.Base, ref willDeleEles);
		capi.Tesselator.TesselateShape(new TesselationMetaData
		{
			TexSource = this,
			IgnoreElements = willDeleEles
		}, shape, out mesh);
		for (int i = 0; i < mesh.RenderPassCount; i++)
		{
			mesh.RenderPassesAndExtraBits[i] = 2;
		}
		if (shapeOffset != null)
		{
			mesh.Translate(shapeOffset);
		}
	}

	protected virtual Shape addGearToShape(Shape entityShape, string shapePathForLogging, ref string[] willDeleteElements)
	{
		IInventory inv = Inventory;
		if (inv == null || inv.Empty)
		{
			return entityShape;
		}
		collectedTextures = new Dictionary<string, CompositeTexture>();
		foreach (ItemSlot gearslot in inv)
		{
			if (!gearslot.Empty)
			{
				IAttachableToEntity iatta = IAttachableToEntity.FromCollectible(gearslot.Itemstack.Collectible);
				if (iatta == null || !iatta.IsAttachable(null, gearslot.Itemstack))
				{
					return entityShape;
				}
				string slotCode = "default";
				EntityBehaviorContainer.addGearToShape(capi, null, capi.BlockTextureAtlas, entityShape, gearslot.Itemstack, iatta, slotCode, shapePathForLogging, ref willDeleteElements, collectedTextures);
			}
		}
		return entityShape;
	}

	public bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling)
	{
		handling = EnumHandling.PreventDefault;
		if (byPlayer.Entity.Controls.ShiftKey)
		{
			int i = 0;
			bool stackMoved = false;
			while (i < placementOrder.Length)
			{
				int index = placementOrder[GameMath.Mod(placementIndex - i - 1, placementOrder.Length)];
				i++;
				if (index == -1)
				{
					break;
				}
				ItemSlot slot = Inventory[index];
				if (!slot.Empty)
				{
					if (!byPlayer.InventoryManager.TryGiveItemstack(slot.Itemstack))
					{
						Api.World.SpawnItemEntity(slot.Itemstack, base.Pos);
					}
					slot.Itemstack = null;
					stackMoved = true;
					break;
				}
			}
			if (!stackMoved)
			{
				ItemSlot slot2 = Inventory.FirstNonEmptySlot;
				if (slot2 != null)
				{
					if (!byPlayer.InventoryManager.TryGiveItemstack(slot2.Itemstack))
					{
						Api.World.SpawnItemEntity(slot2.Itemstack, base.Pos);
					}
					slot2.Itemstack = null;
				}
			}
		}
		else
		{
			ItemSlot hslot = byPlayer.InventoryManager.ActiveHotbarSlot;
			if (!hslot.Empty)
			{
				string cat = hslot.Itemstack.ItemAttributes?["clothescategory"].AsString();
				IWearableStatsSupplier wearableStats = hslot.Itemstack.Collectible.GetCollectibleInterface<IWearableStatsSupplier>();
				if (wearableStats != null)
				{
					cat = wearableStats.GetDressType(hslot).ToString().ToLowerInvariant();
				}
				if (!allowedDresstypes.Contains<string>(cat))
				{
					return false;
				}
				int i2 = 0;
				foreach (ItemSlot slot3 in Inventory)
				{
					if (slot3.Empty && hslot.TryPutInto(Api.World, slot3) > 0)
					{
						placementOrder[placementIndex] = i2;
						placementIndex = (placementIndex + 1) % placementOrder.Length;
						break;
					}
					i2++;
				}
			}
		}
		regenMesh();
		Blockentity.MarkDirty(redrawOnClient: true);
		return true;
	}

	public override void OnBlockBroken(IPlayer? byPlayer = null)
	{
		base.OnBlockBroken(byPlayer);
		Inventory.DropAll(base.Pos.ToVec3d().AddCopy(0.5, 0.5, 0.5));
	}

	public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldAccessForResolve)
	{
		base.FromTreeAttributes(tree, worldAccessForResolve);
		container.FromTreeAttributes(tree, worldAccessForResolve);
		placementIndex = tree.GetInt("placementIndex");
		placementOrder = (tree["placementOrder"] as IntArrayAttribute)?.value ?? new int[Inventory.Count].Fill(-1);
	}

	public override void ToTreeAttributes(ITreeAttribute tree)
	{
		base.ToTreeAttributes(tree);
		container.ToTreeAttributes(tree);
		tree.SetInt("placementIndex", placementIndex);
		tree["placementOrder"] = new IntArrayAttribute(placementOrder);
	}

	public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tessThreadTesselator)
	{
		float[] tfMatrix = new Matrixf().Translate(0.5f, 0f, 0.5f).RotateY(base.MeshAngleRad + (base.Block.GetBehavior<BlockBehaviorRotateablePlaceable>()?.OffsetRad ?? 0f)).Translate(-0.5f, 0f, -0.5f)
			.Values;
		if (!Inventory.Empty)
		{
			mesher.AddMeshData(mesh, tfMatrix);
		}
		mesher.AddMeshData(capi.TesselatorManager.GetDefaultBlockMesh(base.Block), tfMatrix);
		return true;
	}
}
