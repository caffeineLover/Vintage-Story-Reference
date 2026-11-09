using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class BEBehaviorDisplay : BEBehaviorContainer, IInteractable, IRotatablePlaceable, ITexPositionSource
{
	protected MeshData blockMesh;

	protected ICoreClientAPI capi;

	protected ITexPositionSource texSource;

	protected CollectibleObject nowTesselatingObj;

	protected Shape nowTesselatingShape;

	protected Cuboidf[] selectionBoxes;

	protected Cuboidf[] collisionboxes;

	protected InventoryInfinite inventory;

	protected ItemStack displayStack;

	protected bool meshesGenerated;

	public Dictionary<string, float[]> TfMatrices = new Dictionary<string, float[]>();

	public Dictionary<string, float> customRotationDegBySlot;

	private Cuboidf[] allBoxes;

	protected override string InventoryClassName => "cabinet";

	public ItemStack DisplayStack => displayStack;

	public InventoryInfinite InventoryInv => inventory;

	protected Dictionary<string, MeshData> MeshCache => ObjectCacheUtil.GetOrCreate(Api, "meshesDisplay-" + ClassCode, () => new Dictionary<string, MeshData>());

	public float MeshAngleRad { get; set; }

	public virtual string ClassCode => InventoryClassName;

	public virtual string AttributeTransformCode => "onCabinetTransform";

	public override InventoryBase Inventory => inventory;

	public virtual TextureAtlasPosition this[string textureCode]
	{
		get
		{
			if (texSource != null)
			{
				return texSource[textureCode];
			}
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

	public Size2i AtlasSize => capi.BlockTextureAtlas.Size;

	public BEBehaviorDisplay(BlockEntity blockentity)
		: base(blockentity)
	{
		BlockBehaviorDisplay bhd = base.Block.GetBehavior<BlockBehaviorDisplay>();
		inventory = new InventoryInfinite(delegate(string slotid, InventoryBase inv)
		{
			PlacementSurface placementSurface = bhd.PlacementSurfaces[slotid.Split('-')[0].ToInt()];
			return new ItemSlotDisplay(inv, placementSurface.DisplayCategory);
		});
	}

	public override void Initialize(ICoreAPI api, JsonObject properties)
	{
		base.Initialize(api, properties);
		capi = api as ICoreClientAPI;
		Inventory.OnAcquireTransitionSpeed += Inv_OnAcquireTransitionSpeed;
		capi?.Event.RegisterEventBusListener(OnSetTransform, 0.5, "onsettransform");
	}

	public override void OnBlockPlaced(ItemStack byItemStack = null)
	{
		displayStack = byItemStack?.Clone();
		ItemStack itemStack = displayStack;
		if (itemStack != null)
		{
			itemStack.StackSize = 1;
		}
	}

	protected void OnSetTransform(string eventName, ref EnumHandling handling, IAttribute data)
	{
		string target = (data as TreeAttribute).GetString("target");
		if ((target != AttributeTransformCode && target != "onshelfTransform") || base.Pos.DistanceTo(capi.World.Player.Entity.Pos.XYZ) > 20f)
		{
			return;
		}
		CollectibleObject collObj = capi.World.Player.InventoryManager.ActiveHotbarSlot.Itemstack.Collectible;
		if (collObj?.Attributes?["displayable"]["shelf"].AsObject<DisplayableAttributes>() != null)
		{
			if (collObj.Attributes == null)
			{
				collObj.Attributes = new JsonObject(new JObject());
			}
			collObj.Attributes.Token["displayable"]["shelf"]["transform"] = JToken.FromObject(ModelTransform.CreateFromTreeAttribute(data as TreeAttribute));
		}
		MeshCache.Clear();
		MarkMeshesDirty(Api.World);
		Api.World.BlockAccessor.MarkBlockDirty(base.Pos);
	}

	protected float Inv_OnAcquireTransitionSpeed(EnumTransitionType transType, ItemStack stack, float baseMul)
	{
		if ((transType == EnumTransitionType.Dry || transType == EnumTransitionType.Melt) ? true : false)
		{
			Room room = container.Room;
			return ((room != null && room.ExitCount == 0) ? 2f : 0.5f) * 4f;
		}
		if (Api == null)
		{
			return 0f;
		}
		if (transType != EnumTransitionType.Ripen)
		{
			return 1f;
		}
		return GameMath.Clamp((1f - container.GetPerishRate() - 0.5f) * 3f, 0f, 1f);
	}

	public bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling)
	{
		if (blockSel.SelectionBoxId == null)
		{
			return false;
		}
		if (byPlayer.InventoryManager.ActiveHotbarSlot.Empty)
		{
			return TryTake(byPlayer, blockSel, ref handling);
		}
		string selectedSlotId = getSelectedNonEmptySlotId(blockSel);
		if (selectedSlotId != null)
		{
			ItemStack? itemstack = byPlayer.InventoryManager.ActiveHotbarSlot.Itemstack;
			if (itemstack != null && itemstack.Collectible.GetTool(byPlayer.InventoryManager.ActiveHotbarSlot) == EnumTool.Wrench)
			{
				return TryRotate(selectedSlotId, byPlayer, blockSel, ref handling);
			}
		}
		return TryPut(byPlayer, blockSel, ref handling);
	}

	protected bool TryTake(IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling)
	{
		string selectedSlotId = getSelectedNonEmptySlotId(blockSel);
		if (selectedSlotId == null)
		{
			return false;
		}
		ItemSlot slot = inventory[selectedSlotId];
		if (slot != null && !slot.Empty)
		{
			handling = EnumHandling.PreventDefault;
			ItemStack stack = slot.TakeOut(1);
			if (byPlayer.InventoryManager.TryGiveItemstack(stack))
			{
				SoundAttributes? obj = stack?.Block?.Sounds?.Place;
				Api.World.PlaySoundAt(obj ?? GlobalConstants.DefaultBuildSound, byPlayer.Entity, byPlayer);
			}
			if (stack != null && stack.StackSize > 0)
			{
				Api.World.SpawnItemEntity(stack, base.Pos);
			}
			Api.World.Logger.Audit("{0} Took 1x{1} from Shelf at {2}.", byPlayer.PlayerName, stack?.Collectible.Code, base.Pos);
			(Api as ICoreClientAPI)?.World.Player.TriggerFpAnimation(EnumHandInteract.HeldItemInteract);
			MarkMeshesDirty(Api.World);
			Blockentity.MarkDirty(redrawOnClient: true);
			SlotLocation slotLoc = BlockBehaviorDisplay.decodeSlotid(selectedSlotId);
			SlotLocation aboveSlotLoc = slotLoc.UpCopy();
			while (true)
			{
				ItemSlot itemSlot = inventory[aboveSlotLoc.EncodedLocation];
				if (itemSlot == null || itemSlot.Empty)
				{
					break;
				}
				inventory[slotLoc.EncodedLocation].Itemstack = inventory[aboveSlotLoc.EncodedLocation].Itemstack;
				inventory[aboveSlotLoc.EncodedLocation].Itemstack = null;
				slotLoc.Y += 1f;
				aboveSlotLoc.Y += 1f;
			}
			return true;
		}
		return false;
	}

	protected bool TryPut(IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling)
	{
		BlockBehaviorDisplay bhd = base.Block.GetBehavior<BlockBehaviorDisplay>();
		ItemSlot heldSlot = byPlayer.InventoryManager.ActiveHotbarSlot;
		string targetSlotId = blockSel.SelectionBoxId;
		SlotLocation slotLoc = BlockBehaviorDisplay.decodeSlotid(targetSlotId);
		if (slotLoc == null)
		{
			return false;
		}
		PlacementSurface psurface = bhd.PlacementSurfaces[slotLoc.PlacementSurfaceIndex];
		string displayType = psurface.DisplayCategory ?? "shelf";
		_ = heldSlot.Itemstack.Collectible.Attributes;
		DisplayableAttributes heldDAttr = BlockBehaviorDisplay.GetDisplayableAttributes(heldSlot, displayType);
		handling = EnumHandling.PreventSubsequent;
		if (heldDAttr == null)
		{
			return false;
		}
		Size3i availSize = psurface.Size;
		Size3f requiredSize = heldDAttr.Size;
		if ((float)availSize.Width < requiredSize.Width || (float)availSize.Height < requiredSize.Height || (float)availSize.Length < requiredSize.Length)
		{
			(Api as ICoreClientAPI)?.TriggerIngameError(this, "toolarge", Lang.Get("shelfhelp-toolarge-error"));
			return true;
		}
		Vec3f offset = BlockBehaviorDisplay.getOffsetFromPreviewCuboid(requiredSize, MeshAngleRad);
		if (slotLoc.X + offset.X < 0f || slotLoc.Z + offset.Z < 0f || slotLoc.X + offset.X > (float)availSize.Width - requiredSize.Width || slotLoc.Z + offset.Z > (float)availSize.Length - requiredSize.Length)
		{
			(Api as ICoreClientAPI)?.TriggerIngameError(this, "outofbounds", Lang.Get("shelfhelp-outofbounds-error"));
			return true;
		}
		string selectedSlotId = getCollidingSlotId(blockSel, new Cuboidf(requiredSize));
		if (selectedSlotId != null)
		{
			if (heldDAttr.Behavior == EnumDisplayableBehavior.Pileable && heldSlot.Itemstack.Equals(Api.World, inventory[selectedSlotId].Itemstack, GlobalConstants.IgnoredStackAttributes))
			{
				return TryPile(byPlayer, blockSel, selectedSlotId, heldDAttr);
			}
			if (heldDAttr.Behavior == EnumDisplayableBehavior.Stacking)
			{
				return TryStack(byPlayer, blockSel, selectedSlotId, heldDAttr);
			}
			(Api as ICoreClientAPI)?.TriggerIngameError(this, "shelffull", Lang.Get("shelfhelp-shelffull-error"));
			return true;
		}
		return placeItem(byPlayer, heldSlot, targetSlotId);
	}

	private bool TryPile(IPlayer byPlayer, BlockSelection blockSel, string collidingSlotId, DisplayableAttributes heldDAttr)
	{
		ItemSlot heldSlot = byPlayer.InventoryManager.ActiveHotbarSlot;
		if (heldSlot.Itemstack.Equals(Api.World, inventory[collidingSlotId].Itemstack, GlobalConstants.IgnoredStackAttributes))
		{
			return false;
		}
		if (heldDAttr.PileableSelectiveElements.Length < inventory[collidingSlotId].StackSize)
		{
			return false;
		}
		inventory[collidingSlotId].Itemstack.StackSize++;
		heldSlot.TakeOut(1);
		MarkMeshesDirty(Api.World);
		Blockentity.MarkDirty(redrawOnClient: true);
		(Api as ICoreClientAPI)?.World.Player.TriggerFpAnimation(EnumHandInteract.HeldItemInteract);
		Api.World.PlaySoundAt(inventory[collidingSlotId].Itemstack?.Block?.Sounds?.Place ?? GlobalConstants.DefaultBuildSound, byPlayer.Entity, byPlayer);
		Api.World.Logger.Audit("{0} Put 1x{1} into Shelf at {2}, slot {3}.", byPlayer.PlayerName, inventory[collidingSlotId].Itemstack?.Collectible.Code, base.Pos, collidingSlotId);
		return true;
	}

	private bool TryStack(IPlayer byPlayer, BlockSelection blockSel, string collidingSlotId, DisplayableAttributes heldDAttr)
	{
		SlotLocation slotLoc = BlockBehaviorDisplay.decodeSlotid(collidingSlotId);
		PlacementSurface psurface = base.Block.GetBehavior<BlockBehaviorDisplay>().PlacementSurfaces[slotLoc.PlacementSurfaceIndex];
		if (heldDAttr.Category != BlockBehaviorDisplay.GetDisplayableAttributes(inventory[slotLoc.EncodedLocation], psurface.DisplayCategory).Category)
		{
			return false;
		}
		SlotLocation topSlotLoc = slotLoc;
		while (true)
		{
			ItemSlot itemSlot = inventory[topSlotLoc.EncodedLocation];
			if (itemSlot == null || itemSlot.Empty)
			{
				break;
			}
			topSlotLoc = topSlotLoc.UpCopy();
		}
		float num = topSlotLoc.Y * heldDAttr.Size.Height;
		float availHeight = psurface.Size.Height;
		if (num >= availHeight)
		{
			return false;
		}
		placeItem(byPlayer, byPlayer.InventoryManager.ActiveHotbarSlot, topSlotLoc.EncodedLocation);
		return true;
	}

	private bool placeItem(IPlayer byPlayer, ItemSlot heldSlot, string targetSlotId)
	{
		inventory.Allocate(targetSlotId);
		ItemSlot targetSlot = inventory[targetSlotId];
		int num = heldSlot.TryPutInto(Api.World, targetSlot);
		MarkMeshesDirty(Api.World);
		Blockentity.MarkDirty(redrawOnClient: true);
		(Api as ICoreClientAPI)?.World.Player.TriggerFpAnimation(EnumHandInteract.HeldItemInteract);
		if (num > 0)
		{
			Api.World.PlaySoundAt(targetSlot.Itemstack?.Block?.Sounds?.Place ?? GlobalConstants.DefaultBuildSound, byPlayer.Entity, byPlayer);
			Api.World.Logger.Audit("{0} Put 1x{1} into Shelf at {2}, slot {3}.", byPlayer.PlayerName, targetSlot.Itemstack?.Collectible.Code, base.Pos, targetSlotId);
			return true;
		}
		return false;
	}

	private bool TryRotate(string slotid, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling)
	{
		handling = EnumHandling.PreventDefault;
		bool ctrl = byPlayer.WorldData.EntityControls.CtrlKey;
		if (customRotationDegBySlot == null)
		{
			customRotationDegBySlot = new Dictionary<string, float>();
		}
		if (!customRotationDegBySlot.ContainsKey(slotid))
		{
			customRotationDegBySlot[slotid] = 0f;
		}
		else
		{
			customRotationDegBySlot[slotid] += (ctrl ? (-22.5f) : 22.5f);
		}
		Blockentity.MarkDirty(redrawOnClient: true);
		MarkMeshesDirty(Api.World);
		return true;
	}

	protected List<KeyValuePair<Cuboidf, string>> getContentCuboidsToSlotMapping(int placeSurfaceIndex)
	{
		List<KeyValuePair<Cuboidf, string>> mapping = new List<KeyValuePair<Cuboidf, string>>();
		foreach (var (slotid, slot) in inventory.SlotsByslotId)
		{
			if (!slot.Empty)
			{
				Size3f size = BlockBehaviorDisplay.getItemSize(slot, getDisplayType(slotid));
				SlotLocation slotLoc = BlockBehaviorDisplay.decodeSlotid(slotid);
				if (slotLoc.PlacementSurfaceIndex == placeSurfaceIndex)
				{
					Cuboidf box = new Cuboidf(slotLoc.X - size.Width / 2f, slotLoc.Y, slotLoc.Z - size.Length / 2f, slotLoc.X + size.Width / 2f, slotLoc.Y + 1f, slotLoc.Z + size.Length / 2f);
					mapping.Add(new KeyValuePair<Cuboidf, string>(box, slotid));
				}
			}
		}
		return mapping;
	}

	public string getSelectedNonEmptySlotId(BlockSelection blockSel)
	{
		SlotLocation slotLoc = BlockBehaviorDisplay.decodeSlotid(blockSel.SelectionBoxId);
		if (slotLoc == null)
		{
			return null;
		}
		List<KeyValuePair<Cuboidf, string>> contentCuboidsToSlotMapping = getContentCuboidsToSlotMapping(slotLoc.PlacementSurfaceIndex);
		string selectedSlotId = null;
		foreach (var (cuboidf2, slotid) in contentCuboidsToSlotMapping)
		{
			if (cuboidf2.Contains(slotLoc.X, slotLoc.Y + 0.1f, slotLoc.Z))
			{
				selectedSlotId = slotid;
				break;
			}
		}
		return selectedSlotId;
	}

	public string getCollidingSlotId(BlockSelection blockSel, Cuboidf placeBox)
	{
		SlotLocation slotLoc = BlockBehaviorDisplay.decodeSlotid(blockSel.SelectionBoxId);
		List<KeyValuePair<Cuboidf, string>> contentCuboidsToSlotMapping = getContentCuboidsToSlotMapping(slotLoc.PlacementSurfaceIndex);
		string selectedSlotId = null;
		foreach (var (cuboidf2, slotid) in contentCuboidsToSlotMapping)
		{
			if (cuboidf2.Intersects(placeBox, slotLoc.X, slotLoc.Y, slotLoc.Z))
			{
				selectedSlotId = slotid;
				break;
			}
		}
		return selectedSlotId;
	}

	public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldAccessForResolve)
	{
		base.FromTreeAttributes(tree, worldAccessForResolve);
		MeshAngleRad = tree.GetFloat("meshAngleRad");
		if (tree.HasAttribute("rotation"))
		{
			customRotationDegBySlot = new Dictionary<string, float>();
			foreach (KeyValuePair<string, IAttribute> val in tree.GetTreeAttribute("rotation"))
			{
				customRotationDegBySlot[val.Key] = (val.Value as FloatAttribute).value;
			}
		}
		displayStack = tree.GetItemstack("displayStack");
		displayStack?.ResolveBlockOrItem(worldAccessForResolve);
		MarkMeshesDirty(worldAccessForResolve);
	}

	public override void ToTreeAttributes(ITreeAttribute tree)
	{
		base.ToTreeAttributes(tree);
		container.ToTreeAttributes(tree);
		tree.SetFloat("meshAngleRad", MeshAngleRad);
		tree.SetItemstack("displayStack", displayStack);
		if (customRotationDegBySlot == null)
		{
			return;
		}
		TreeAttribute rotTree = new TreeAttribute();
		foreach (KeyValuePair<string, float> val in customRotationDegBySlot)
		{
			rotTree[val.Key] = new FloatAttribute(val.Value);
		}
		tree["rotation"] = rotTree;
	}

	public override void OnLoadCollectibleMappings(IWorldAccessor world, Dictionary<int, AssetLocation> oldBlockIdMapping, Dictionary<int, AssetLocation> oldItemIdMapping, int schematicSeed, bool resolveImports)
	{
		base.OnLoadCollectibleMappings(world, oldBlockIdMapping, oldItemIdMapping, schematicSeed, resolveImports);
		ItemStack itemStack = displayStack;
		if (itemStack != null && !itemStack.FixMapping(oldBlockIdMapping, oldItemIdMapping, world))
		{
			displayStack = null;
		}
		else
		{
			displayStack?.Collectible.OnLoadCollectibleMappings(world, new DummySlot(displayStack), oldBlockIdMapping, oldItemIdMapping, resolveImports);
		}
	}

	public override void OnStoreCollectibleMappings(Dictionary<int, AssetLocation> blockIdMapping, Dictionary<int, AssetLocation> itemIdMapping)
	{
		base.OnStoreCollectibleMappings(blockIdMapping, itemIdMapping);
		displayStack?.Collectible.OnStoreCollectibleMappings(Api.World, new DummySlot(displayStack), blockIdMapping, itemIdMapping);
	}

	protected void generateMeshes()
	{
		if (capi == null)
		{
			return;
		}
		BlockBehaviorDisplay bh = base.Block.GetBehavior<BlockBehaviorDisplay>();
		Shape shape = capi.TesselatorManager.GetCachedShape(base.Block.Shape.Base);
		PlacementSurface[] psurfaces = base.Block.GetBehavior<BlockBehaviorDisplay>().PlacementSurfaces;
		CollectibleBehaviorTypedTexture titem = displayStack?.Collectible.GetCollectibleBehavior<CollectibleBehaviorTypedTexture>(withInheritance: true);
		if (titem != null)
		{
			texSource = new TypedTextureSource(capi, capi.BlockTextureAtlas, displayStack.Collectible.Code.Domain, titem.TextureMapping, titem.GetMaterials(displayStack));
		}
		else
		{
			texSource = capi.Tesselator.GetTextureSource(base.Block);
		}
		capi.Tesselator.TesselateShape(new TesselationMetaData
		{
			TexSource = texSource,
			IgnoreElements = bh.PlacementSurfaces.Select((PlacementSurface dp) => dp.ElementName).ToArray().Append(base.Block.Shape.IgnoreElements)
		}, shape, out blockMesh);
		texSource = null;
		foreach (KeyValuePair<string, ItemSlot> item in inventory.SlotsByslotId)
		{
			item.Deconstruct(out var key, out var value);
			string slotid = key;
			ItemSlot slot = value;
			SlotLocation slotLoc = BlockBehaviorDisplay.decodeSlotid(slotid);
			ItemStack stack = slot.Itemstack;
			if (stack != null && !(stack.Collectible?.Code == null))
			{
				ItemSlotDisplay slotd = slot as ItemSlotDisplay;
				getOrCreateMesh(slotd);
				DisplayableAttributes attr = BlockBehaviorDisplay.GetDisplayableAttributes(slot, getDisplayType(slotid));
				if (attr == null)
				{
					return;
				}
				Vec3f surfaceVoxelOffset = psurfaces[slotLoc.PlacementSurfaceIndex].VoxelPosition;
				SlotLocation slotVoxelOffset = slotLoc;
				Vec3f voxelOffsetToSlot = BlockBehaviorDisplay.getOffsetFromPreviewCuboid(attr.Size, MeshAngleRad);
				float stackingOffsetY = slotLoc.Y * attr.Size.Height;
				float rndRot = ((attr.RandYRotAngle > 0) ? (GameMath.MurmurHash3Mod(base.Pos.X, base.Pos.Y + slotid.GetHashCode(), base.Pos.Z, attr.RandYRotAngle + 1) - attr.RandYRotAngle / 2) : 0);
				float rndscaleY = GameMath.MurmurHash3Mod(base.Pos.X, base.Pos.Y, base.Pos.Z + slotid.GetHashCode(), 101) - 50;
				if (customRotationDegBySlot != null && customRotationDegBySlot.TryGetValue(slotid, out var rot))
				{
					rndRot = rot;
				}
				TfMatrices[slotid] = new Matrixf().Translate(0.5f, 0f, 0.5f).RotateYDeg(base.Block.Shape.rotateY).RotateY(MeshAngleRad)
					.Translate((surfaceVoxelOffset.X + slotVoxelOffset.X + voxelOffsetToSlot.X) / 16f, (surfaceVoxelOffset.Y + stackingOffsetY) / 16f, (surfaceVoxelOffset.Z + slotVoxelOffset.Z + voxelOffsetToSlot.Z) / 16f)
					.Translate(-0.5f + attr.Size.Width / 2f / 16f, 0f, -0.5f + attr.Size.Length / 2f / 16f)
					.RotateY((float)Math.PI / 180f * rndRot)
					.Scale(1f, 1f + rndscaleY / 1500f, 1f)
					.Translate(-0.5f, 0f, -0.5f)
					.Values;
			}
		}
		meshesGenerated = true;
	}

	public Vec3f placementDirection()
	{
		return new Vec3f();
	}

	protected virtual MeshData getOrCreateMesh(ItemSlotDisplay slot)
	{
		MeshData mesh = getCachedMesh(slot);
		if (mesh != null)
		{
			return mesh;
		}
		CollectibleObject collObj = slot.Itemstack.Collectible;
		DisplayableAttributes attr = BlockBehaviorDisplay.GetDisplayableAttributes(slot, slot.DisplayCategory);
		if (attr == null)
		{
			return getDefaultMesh(slot);
		}
		CompositeShape customShape = attr.Shape;
		if (customShape != null)
		{
			if (attr.Behavior == EnumDisplayableBehavior.Pileable)
			{
				string[] eles = new string[slot.StackSize];
				for (int i = 0; i < slot.StackSize; i++)
				{
					eles[i] = attr.PileableSelectiveElements[i];
				}
				customShape.SelectiveElements = eles;
			}
			string customkey = string.Concat("displayedShape-", collObj.Code, "-", customShape.ToString());
			mesh = ObjectCacheUtil.GetOrCreate(capi, customkey, () => capi.TesselatorManager.CreateMesh("displayed item shape", customShape, delegate(Shape shape, string name)
			{
				Dictionary<string, AssetLocation> dictionary = new Dictionary<string, AssetLocation>(shape.Textures);
				IDictionary<string, CompositeTexture> dictionary2;
				if (slot.Itemstack.Class != EnumItemClass.Item)
				{
					dictionary2 = slot.Itemstack.Block.Textures;
				}
				else
				{
					IDictionary<string, CompositeTexture> textures = slot.Itemstack.Item.Textures;
					dictionary2 = textures;
				}
				foreach (KeyValuePair<string, CompositeTexture> current in dictionary2)
				{
					dictionary[current.Key] = current.Value.Base;
				}
				return new ContainedTextureSource(capi, capi.BlockTextureAtlas, dictionary, $"For displayed item {collObj.Code}");
			}));
		}
		else
		{
			mesh = (collObj?.GetCollectibleInterface<IContainedMeshSource>())?.GenMesh(slot, capi.BlockTextureAtlas, base.Pos);
		}
		if (mesh == null)
		{
			mesh = getDefaultMesh(slot);
		}
		applyDefaultTranforms(slot, mesh);
		MeshCache[getMeshCacheKey(slot)] = mesh;
		return mesh;
	}

	protected void applyDefaultTranforms(ItemSlotDisplay slot, MeshData mesh)
	{
		ItemStack stack = slot.Itemstack;
		DisplayableAttributes dispAttr = BlockBehaviorDisplay.GetDisplayableAttributes(slot, slot.DisplayCategory);
		if (dispAttr.Transform != null)
		{
			dispAttr.Transform.EnsureDefaultValues();
			mesh.ModelTransform(dispAttr.Transform);
		}
		else if (stack.Class == EnumItemClass.Item && (stack.Item.Shape == null || stack.Item.Shape.VoxelizeTexture))
		{
			mesh.Rotate((float)Math.PI / 2f, 0f, 0f);
			mesh.Scale(0.33f, 0.33f, 0.33f);
			mesh.Translate(0f, -15f / 32f, 0f);
		}
	}

	protected MeshData getDefaultMesh(ItemSlotDisplay slot)
	{
		ICoreClientAPI capi = Api as ICoreClientAPI;
		ItemStack stack = slot.Itemstack;
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

	protected MeshData getCachedMesh(ItemSlot slot)
	{
		string key = getMeshCacheKey(slot);
		MeshCache.TryGetValue(key, out var meshdata);
		return meshdata;
	}

	protected virtual string getMeshCacheKey(ItemSlot slot)
	{
		return (slot.Itemstack.Collectible?.GetCollectibleInterface<IContainedMeshSource>())?.GetMeshCacheKey(slot) ?? slot.Itemstack.Collectible.Code.ToString();
	}

	protected virtual void MarkMeshesDirty(IWorldAccessor worldForResolving)
	{
		if (worldForResolving.Side == EnumAppSide.Client && Api != null)
		{
			meshesGenerated = false;
			Api.World.BlockAccessor.MarkBlockDirty(base.Pos);
		}
		regenSelectionBoxes();
	}

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

	public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tessThreadTesselator)
	{
		if (!meshesGenerated)
		{
			generateMeshes();
		}
		inventory.ResolveBlocksOrItems();
		foreach (var (slotid, slot) in inventory.SlotsByslotId)
		{
			if (!slot.Empty && !(slot.Itemstack.Collectible?.Code == null))
			{
				mesher.AddMeshData(getCachedMesh(slot), TfMatrices[slotid]);
			}
		}
		float[] tfMatrix = new Matrixf().Translate(0.5f, 0f, 0.5f).RotateY(MeshAngleRad).Translate(-0.5f, 0f, -0.5f)
			.Values;
		mesher.AddMeshData(blockMesh, tfMatrix);
		return true;
	}

	public Cuboidf[] GetCollisionBoxes()
	{
		if (collisionboxes == null)
		{
			Cuboidf[] array = rotatedCopy(base.Block.CollisionBoxes);
			collisionboxes = array;
		}
		return collisionboxes;
	}

	public Cuboidf[] GetSelectionBoxes()
	{
		if (selectionBoxes == null)
		{
			regenSelectionBoxes();
		}
		if (capi != null && base.Block.GetBehavior<BlockBehaviorDisplay>().placingItemsPreview(this))
		{
			return allBoxes;
		}
		return selectionBoxes;
	}

	private void regenSelectionBoxes()
	{
		List<Cuboidf> cuboids = new List<Cuboidf>();
		cuboids.AddRange(rotatedCopy(base.Block.SelectionBoxes));
		CuboidfWithId[] array = base.Block.GetBehavior<BlockBehaviorDisplay>().SelectionBoxes;
		foreach (CuboidfWithId selbox in array)
		{
			CuboidfWithId rotBox = rotatedCopy(selbox);
			rotBox.Y1 -= 0.0625f;
			rotBox.Y2 = rotBox.Y1 + 0.0625f;
			cuboids.Add(rotBox);
		}
		selectionBoxes = cuboids.ToArray();
		PlacementSurface[] psurfaces = base.Block.GetBehavior<BlockBehaviorDisplay>().PlacementSurfaces;
		foreach (var (slotid, slot) in inventory.SlotsByslotId)
		{
			if (!slot.Empty)
			{
				DisplayableAttributes dattr = BlockBehaviorDisplay.GetDisplayableAttributes(slot, getDisplayType(slotid));
				if (dattr != null)
				{
					float w = dattr.Size.Width / 16f;
					float h = dattr.Size.Height / 16f;
					float l = dattr.Size.Length / 16f;
					SlotLocation slotLoc = BlockBehaviorDisplay.decodeSlotid(slotid);
					Vec3f surfaceVoxelOffset = psurfaces[slotLoc.PlacementSurfaceIndex].VoxelPosition;
					SlotLocation slotVoxelOffset = slotLoc;
					Vec3f voxelOffsetToSlot = BlockBehaviorDisplay.getOffsetFromPreviewCuboid(dattr.Size, MeshAngleRad);
					float stackingOffsetY = slotLoc.Y * dattr.Size.Height;
					Cuboidf cub = new CuboidfWithId(0f, 0f, 0f, w, h, l)
					{
						Id = "p-" + slotid
					}.Translate((surfaceVoxelOffset.X + slotVoxelOffset.X + voxelOffsetToSlot.X) / 16f, (surfaceVoxelOffset.Y + stackingOffsetY) / 16f, (surfaceVoxelOffset.Z + slotVoxelOffset.Z + voxelOffsetToSlot.Z) / 16f);
					cuboids.Add(rotatedCopy(cub));
				}
			}
		}
		allBoxes = cuboids.ToArray();
	}

	private string getDisplayType(string slotid)
	{
		SlotLocation slotLoc = BlockBehaviorDisplay.decodeSlotid(slotid);
		return base.Block.GetBehavior<BlockBehaviorDisplay>().PlacementSurfaces[slotLoc.PlacementSurfaceIndex].DisplayCategory ?? "shelf";
	}

	protected CuboidfWithId[] rotatedCopy(Cuboidf[] cuboids)
	{
		CuboidfWithId[] rotated = new CuboidfWithId[cuboids.Length];
		for (int i = 0; i < rotated.Length; i++)
		{
			rotated[i] = rotatedCopy(cuboids[i]);
		}
		return rotated;
	}

	public CuboidfWithId rotatedCopy(Cuboidf cuboid)
	{
		Cuboidf rotCub = cuboid.RotatedCopyRad(0f, MeshAngleRad, 0f, new Vec3d(0.5, 0.0, 0.5));
		CuboidfWithId cuboidfWithId = new CuboidfWithId();
		cuboidfWithId.Set(rotCub);
		cuboidfWithId.Id = (cuboid as CuboidfWithId)?.Id;
		return cuboidfWithId;
	}

	public override void OnBlockBroken(IPlayer byPlayer = null)
	{
		inventory.DropAll(base.Pos.ToVec3d().Add(0.5, 0.5, 0.5));
	}

	public bool DoPartialSelection()
	{
		return true;
	}

	public int GetParticleColorTextureSubId()
	{
		CollectibleBehaviorTypedTexture titem = displayStack?.Collectible.GetCollectibleBehavior<CollectibleBehaviorTypedTexture>(withInheritance: true);
		if (titem != null)
		{
			AssetLocation loc = TypedTextureSource.getMappedTexture("wood-top", displayStack.Collectible.Code.Domain, titem.GetMaterials(displayStack), titem.TextureMapping);
			capi.BlockTextureAtlas.GetOrInsertTexture(loc, out var subid, out var _);
			return subid;
		}
		return base.Block.TextureSubIdForBlockColor;
	}
}
