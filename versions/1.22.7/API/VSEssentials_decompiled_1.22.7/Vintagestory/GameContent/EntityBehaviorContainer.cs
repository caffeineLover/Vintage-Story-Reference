using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

[DocumentAsJson]
public abstract class EntityBehaviorContainer : EntityBehavior
{
	protected ICoreAPI Api;

	private InWorldContainer container;

	public bool hideClothing;

	private bool eventRegistered;

	[DocumentAsJson("Optional", "False", false)]
	private bool dropContentsOnDeath;

	public abstract InventoryBase Inventory { get; }

	public abstract string InventoryClassName { get; }

	protected EntityBehaviorContainer(Entity entity)
		: base(entity)
	{
		container = new InWorldContainer(() => Inventory, InventoryClassName);
	}

	public override bool TryEarlyLoadCollectibleMappings(IWorldAccessor worldForCollectibleResolve, Dictionary<int, AssetLocation> oldBlockIdMapping, Dictionary<int, AssetLocation> oldItemIdMapping, bool resolveImports, EntityProperties entityProperties, JsonObject behaviorConfig)
	{
		OnLoadCollectibleMappings(worldForCollectibleResolve, oldBlockIdMapping, oldItemIdMapping, resolveImports);
		return true;
	}

	public override bool ShouldEarlyLoadCollectibleMappings()
	{
		return true;
	}

	public override void Initialize(EntityProperties properties, JsonObject attributes)
	{
		Api = entity.World.Api;
		container.Init(Api, () => entity.Pos.AsBlockPos, delegate
		{
			entity.WatchedAttributes.MarkPathDirty(InventoryClassName);
		});
		if (Api.Side == EnumAppSide.Client)
		{
			entity.WatchedAttributes.RegisterModifiedListener(InventoryClassName, inventoryModified);
		}
		dropContentsOnDeath = attributes?.IsTrue("dropContentsOnDeath") ?? false;
	}

	private void inventoryModified()
	{
		loadInv();
		entity.MarkShapeModified();
	}

	public override void OnGameTick(float deltaTime)
	{
		if (!eventRegistered && Inventory != null)
		{
			eventRegistered = true;
			Inventory.SlotModified += Inventory_SlotModified;
		}
	}

	public override void OnEntityDespawn(EntityDespawnData despawn)
	{
		if (Inventory != null)
		{
			Inventory.SlotModified -= Inventory_SlotModified;
		}
	}

	protected void Inventory_SlotModifiedBackpack(int slotid)
	{
		if (entity is EntityPlayer player && player.Player.InventoryManager.GetOwnInventory("backpack")[slotid] is ItemSlotBackpack)
		{
			entity.MarkShapeModified();
		}
	}

	protected virtual void Inventory_SlotModified(int slotid)
	{
		entity.MarkShapeModified();
	}

	public override void OnTesselation(ref Shape entityShape, string shapePathForLogging, ref bool shapeIsCloned, ref string[] willDeleteElements)
	{
		try
		{
			addGearToShape(ref entityShape, shapePathForLogging, ref shapeIsCloned, ref willDeleteElements);
		}
		catch (Exception innerException)
		{
			throw new Exception(string.Concat("Error tesselating entity ", entity.Code, " at ", entity.Pos.AsBlockPos?.ToString()), innerException);
		}
		base.OnTesselation(ref entityShape, shapePathForLogging, ref shapeIsCloned, ref willDeleteElements);
		if (Inventory != null)
		{
			ItemSlot brightestSlot = Inventory.MaxBy((ItemSlot slot) => (!slot.Empty) ? slot.Itemstack.Collectible.LightHsv[2] : 0);
			if (!brightestSlot.Empty)
			{
				entity.LightHsv = brightestSlot.Itemstack.Collectible.GetLightHsv(entity.World.BlockAccessor, null, brightestSlot.Itemstack);
			}
			else
			{
				entity.LightHsv = null;
			}
		}
	}

	protected virtual bool ShouldAddGearToShape(ItemSlot slot)
	{
		return true;
	}

	protected Shape addGearToShape(ref Shape entityShape, string shapePathForLogging, ref bool shapeIsCloned, ref string[] willDeleteElements)
	{
		IInventory inv = Inventory;
		if (inv == null || (!(entity is EntityPlayer) && inv.Empty))
		{
			return entityShape;
		}
		foreach (ItemSlot slot in inv)
		{
			if (!slot.Empty && !hideClothing && ShouldAddGearToShape(slot))
			{
				entityShape = addGearToShape(entityShape, slot, "default", shapePathForLogging, ref shapeIsCloned, ref willDeleteElements);
			}
		}
		if (shapeIsCloned && Api is ICoreClientAPI capi)
		{
			EntityProperties etype = Api.World.GetEntityType(entity.Code);
			if (etype != null)
			{
				foreach (KeyValuePair<string, CompositeTexture> val in etype.Client.Textures)
				{
					CompositeTexture cmpt = val.Value;
					cmpt.Bake(Api.Assets);
					capi.EntityTextureAtlas.GetOrInsertTexture(cmpt.Baked.TextureFilenames[0], out var textureSubid, out var _);
					cmpt.Baked.TextureSubId = textureSubid;
					entity.Properties.Client.Textures[val.Key] = val.Value;
				}
			}
		}
		return entityShape;
	}

	protected virtual Shape addGearToShape(Shape entityShape, ItemSlot gearslot, string slotCode, string shapePathForLogging, ref bool shapeIsCloned, ref string[] willDeleteElements, Dictionary<string, StepParentElementTo> overrideStepParent = null)
	{
		if (gearslot.Empty || entityShape == null)
		{
			return entityShape;
		}
		IAttachableToEntity iatta = IAttachableToEntity.FromCollectible(gearslot.Itemstack.Collectible);
		if (iatta == null || !iatta.IsAttachable(entity, gearslot.Itemstack))
		{
			return entityShape;
		}
		if (!shapeIsCloned)
		{
			entityShape = entityShape.Clone();
			shapeIsCloned = true;
		}
		return addGearToShape(entityShape, gearslot.Itemstack, iatta, slotCode, shapePathForLogging, ref willDeleteElements, overrideStepParent);
	}

	public virtual Shape addGearToShape(Shape entityShape, ItemStack stack, IAttachableToEntity iatta, string slotCode, string shapePathForLogging, ref string[] willDeleteElements, Dictionary<string, StepParentElementTo> overrideStepParent = null)
	{
		IDictionary<string, CompositeTexture> textures = entity?.Properties.Client.Textures;
		return addGearToShape(Api, entity, (Api as ICoreClientAPI)?.EntityTextureAtlas, entityShape, stack, iatta, slotCode, shapePathForLogging, ref willDeleteElements, textures, overrideStepParent);
	}

	public static Shape addGearToShape(ICoreAPI api, Entity optionalTargetEntity, ITextureAtlasAPI targetAtlas, Shape entityShape, ItemStack stack, IAttachableToEntity iatta, string slotCode, string shapePathForLogging, ref string[] willDeleteElements, IDictionary<string, CompositeTexture> collectedTextures, Dictionary<string, StepParentElementTo> overrideStepParent = null)
	{
		if (stack == null || iatta == null)
		{
			return entityShape;
		}
		float damageEffect = 0f;
		JsonObject itemAttributes = stack.ItemAttributes;
		if (itemAttributes != null && itemAttributes["visibleDamageEffect"].AsBool())
		{
			damageEffect = Math.Max(0f, 1f - (float)stack.Collectible.GetRemainingDurability(stack) / (float)stack.Collectible.GetMaxDurability(stack) * 1.1f);
		}
		string[] disableElements = iatta.GetDisableElements(stack);
		if (disableElements != null)
		{
			willDeleteElements = willDeleteElements.Append(disableElements);
			entityShape.RemoveElements(disableElements);
		}
		string[] keepEles = iatta.GetKeepElements(stack);
		if (keepEles != null && willDeleteElements != null)
		{
			string[] array = keepEles;
			foreach (string val in array)
			{
				willDeleteElements = willDeleteElements.Remove(val);
			}
		}
		string texturePrefixCode = iatta.GetTexturePrefixCode(stack);
		if (optionalTargetEntity != null && optionalTargetEntity.Properties.Attributes?["useSlotPrefix"].AsBool() == true)
		{
			texturePrefixCode = ((texturePrefixCode != null) ? (texturePrefixCode + "-" + slotCode) : slotCode);
		}
		Shape gearShape = null;
		AssetLocation shapePath = null;
		CompositeShape compGearShape = null;
		if (stack.Collectible is IWearableShapeSupplier iwss)
		{
			gearShape = iwss.GetShape(stack, optionalTargetEntity, texturePrefixCode);
		}
		if (gearShape == null)
		{
			compGearShape = iatta.GetAttachedShape(stack, slotCode);
			if (compGearShape == null)
			{
				return entityShape;
			}
			shapePath = compGearShape.Base.CopyWithPathPrefixAndAppendixOnce("shapes/", ".json");
			gearShape = Shape.TryGet(api, shapePath);
			if (gearShape == null)
			{
				api.World.Logger.Warning("Entity attachable shape {0} defined in {1} {2} not found or errored, was supposed to be at {3}. Shape will be invisible.", compGearShape.Base, stack.Class, stack.Collectible.Code, shapePath);
				return entityShape;
			}
			gearShape.SubclassForStepParenting(texturePrefixCode, damageEffect);
			gearShape.ResolveReferences(api.World.Logger, shapePath);
		}
		ICoreClientAPI capi = api as ICoreClientAPI;
		Dictionary<string, CompositeTexture> intoDict = null;
		if (capi != null)
		{
			intoDict = new Dictionary<string, CompositeTexture>();
			iatta.CollectTextures(stack, gearShape, texturePrefixCode, intoDict);
		}
		applyStepParentOverrides(overrideStepParent, gearShape);
		entityShape.StepParentShape(gearShape, (compGearShape?.Base.ToString() ?? "Custom texture from ItemWearableShapeSupplier") + $" defined in {stack.Class} {stack.Collectible.Code}", shapePathForLogging, api.World.Logger, delegate(string texcode, AssetLocation tloc)
		{
			addTexture(capi, texcode, tloc, collectedTextures, texturePrefixCode, targetAtlas);
		});
		if (compGearShape?.Overlays != null)
		{
			CompositeShape[] overlays = compGearShape.Overlays;
			foreach (CompositeShape overlay in overlays)
			{
				Shape oshape = Shape.TryGet(api, overlay.Base.CopyWithPathPrefixAndAppendixOnce("shapes/", ".json"));
				if (oshape == null)
				{
					api.World.Logger.Warning("Entity attachable shape {0} overlay {4} defined in {1} {2} not found or errored, was supposed to be at {3}. Shape will be invisible.", compGearShape.Base, stack.Class, stack.Collectible.Code, shapePath, overlay.Base);
					continue;
				}
				oshape.SubclassForStepParenting(texturePrefixCode, damageEffect);
				if (capi != null)
				{
					iatta.CollectTextures(stack, oshape, texturePrefixCode, intoDict);
				}
				applyStepParentOverrides(overrideStepParent, oshape);
				entityShape.StepParentShape(oshape, overlay.Base.ToShortString(), shapePathForLogging, api.Logger, delegate(string texcode, AssetLocation tloc)
				{
					addTexture(capi, texcode, tloc, collectedTextures, texturePrefixCode, targetAtlas);
				});
			}
		}
		if (capi != null)
		{
			foreach (KeyValuePair<string, CompositeTexture> val2 in intoDict)
			{
				CompositeTexture compositeTexture = (collectedTextures[val2.Key] = val2.Value.Clone());
				CompositeTexture cmpt = compositeTexture;
				targetAtlas.GetOrInsertTexture(cmpt, out var textureSubid, out var _);
				cmpt.Baked.TextureSubId = textureSubid;
			}
		}
		return entityShape;
	}

	public static void applyStepParentOverrides(Dictionary<string, StepParentElementTo> overrideStepParent, Shape gearShape)
	{
		if (overrideStepParent == null)
		{
			return;
		}
		overrideStepParent.TryGetValue("", out var noparentoverride);
		ShapeElement[] elements = gearShape.Elements;
		foreach (ShapeElement ele in elements)
		{
			StepParentElementTo parentovr;
			if (ele.StepParentName == null || ele.StepParentName.Length == 0)
			{
				ele.StepParentName = noparentoverride.ElementName;
			}
			else if (overrideStepParent.TryGetValue(ele.StepParentName, out parentovr))
			{
				ele.StepParentName = parentovr.ElementName;
			}
		}
	}

	public static void addTexture(ICoreClientAPI capi, string texcode, AssetLocation tloc, IDictionary<string, CompositeTexture> textures, string texturePrefixCode, ITextureAtlasAPI targetAtlas)
	{
		if (capi != null)
		{
			CompositeTexture cmpt = new CompositeTexture(tloc);
			if (textures != null)
			{
				textures[texturePrefixCode + texcode] = cmpt;
			}
			cmpt.Bake(capi.Assets);
			targetAtlas.GetOrInsertTexture(cmpt.Baked.TextureFilenames[0], out var textureSubid, out var _);
			cmpt.Baked.TextureSubId = textureSubid;
		}
	}

	public override void OnLoadCollectibleMappings(IWorldAccessor worldForNewMappings, Dictionary<int, AssetLocation> oldBlockIdMapping, Dictionary<int, AssetLocation> oldItemIdMapping, bool resolveImports)
	{
		container.OnLoadCollectibleMappings(worldForNewMappings, oldBlockIdMapping, oldItemIdMapping, 0, resolveImports);
	}

	public override void OnStoreCollectibleMappings(Dictionary<int, AssetLocation> blockIdMapping, Dictionary<int, AssetLocation> itemIdMapping)
	{
		container.OnStoreCollectibleMappings(blockIdMapping, itemIdMapping);
	}

	public override void FromBytes(bool isSync)
	{
		loadInv();
	}

	protected virtual void loadInv()
	{
		if (Inventory != null)
		{
			container.FromTreeAttributes(entity.WatchedAttributes, entity.World);
			entity.MarkShapeModified();
		}
	}

	public override void ToBytes(bool forClient)
	{
		storeInv();
	}

	public virtual void storeInv()
	{
		container.ToTreeAttributes(entity.WatchedAttributes);
		entity.WatchedAttributes.MarkPathDirty(InventoryClassName);
		entity.World.BlockAccessor.GetChunkAtBlockPos(entity.Pos.AsBlockPos)?.MarkModified();
	}

	public override bool TryGiveItemStack(ItemStack itemstack, ref EnumHandling handling)
	{
		ItemSlot dummySlot = new DummySlot(null);
		dummySlot.Itemstack = itemstack.Clone();
		ItemStackMoveOperation op = new ItemStackMoveOperation(entity.World, EnumMouseButton.Left, (EnumModifierKey)0, EnumMergePriority.AutoMerge, itemstack.StackSize);
		if (Inventory != null)
		{
			WeightedSlot wslot = Inventory.GetBestSuitedSlot(dummySlot, null, new List<ItemSlot>());
			if (wslot.weight > 0f)
			{
				dummySlot.TryPutInto(wslot.slot, ref op);
				itemstack.StackSize -= op.MovedQuantity;
				entity.WatchedAttributes.MarkAllDirty();
				return op.MovedQuantity > 0;
			}
		}
		if ((entity as EntityAgent)?.LeftHandItemSlot?.Inventory != null)
		{
			WeightedSlot wslot2 = (entity as EntityAgent)?.LeftHandItemSlot.Inventory.GetBestSuitedSlot(dummySlot, null, new List<ItemSlot>());
			if (wslot2.weight > 0f)
			{
				dummySlot.TryPutInto(wslot2.slot, ref op);
				itemstack.StackSize -= op.MovedQuantity;
				entity.WatchedAttributes.MarkAllDirty();
				return op.MovedQuantity > 0;
			}
		}
		return false;
	}

	public override void OnEntityDeath(DamageSource damageSourceForDeath)
	{
		base.OnEntityDeath(damageSourceForDeath);
		if (dropContentsOnDeath)
		{
			Inventory.DropAll(entity.Pos.XYZ);
		}
	}
}
