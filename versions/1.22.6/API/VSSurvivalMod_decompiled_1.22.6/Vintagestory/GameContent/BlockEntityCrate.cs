using System;
using System.Collections.Generic;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class BlockEntityCrate : BlockEntityContainer, IRotatable
{
	public static SoundAttributes ChalkDraw = new SoundAttributes(AssetLocation.Create("sounds/player/chalkdraw"), withRandomPitch: true)
	{
		Range = 8f
	};

	private InventoryGeneric inventory;

	private BlockCrate ownBlock;

	public string type = "wood-aged";

	public string label;

	public string preferredLidState = "closed";

	public int quantitySlots = 16;

	public bool retrieveOnly;

	private float rotAngleY;

	private MeshData ownMesh;

	private MeshData labelMesh;

	private Cuboidf selBoxCrate;

	private Cuboidf selBoxLabel;

	private int labelColor;

	private ItemStack labelStack;

	private ModSystemLabelMeshCache labelCacheSys;

	private bool requested;

	private static Vec3f origin = new Vec3f(0.5f, 0f, 0.5f);

	private float rndScale = 1f;

	public bool Labelled
	{
		get
		{
			if (label != null)
			{
				return label != "";
			}
			return false;
		}
	}

	public virtual float MeshAngle
	{
		get
		{
			return rotAngleY;
		}
		set
		{
			rotAngleY = value;
		}
	}

	public override InventoryBase Inventory => inventory;

	public override string InventoryClassName => "crate";

	public LabelProps LabelProps
	{
		get
		{
			if (label == null)
			{
				return null;
			}
			ownBlock.Props.Labels.TryGetValue(label, out var prop);
			return prop;
		}
	}

	public string LidState
	{
		get
		{
			if (preferredLidState == "closed")
			{
				return preferredLidState;
			}
			if (inventory.Empty)
			{
				return preferredLidState;
			}
			ItemStack stack = inventory.FirstNonEmptySlot.Itemstack;
			if (stack?.Collectible == null || (stack.ItemAttributes != null && stack.ItemAttributes["inContainerTexture"].Exists))
			{
				return preferredLidState;
			}
			JsonObject itemAttributes = stack.ItemAttributes;
			bool? displayInsideCrate = ((itemAttributes == null || !itemAttributes["displayInsideCrate"].Exists) ? ((bool?)null) : stack.ItemAttributes?["displayInsideCrate"].AsBool(defaultValue: true));
			if ((stack.Block == null || stack.Block.DrawType != EnumDrawType.Cube || displayInsideCrate == false) && displayInsideCrate != true)
			{
				return "closed";
			}
			return preferredLidState;
		}
	}

	public override void Initialize(ICoreAPI api)
	{
		ownBlock = base.Block as BlockCrate;
		bool isNewlyplaced = inventory == null;
		if (isNewlyplaced)
		{
			InitInventory(base.Block, api);
		}
		base.Initialize(api);
		if (api.Side == EnumAppSide.Client)
		{
			rndScale = 1f + (float)(GameMath.MurmurHash3Mod(Pos.X, Pos.Y, Pos.Z, 100) - 50) / 1000f;
			if (!isNewlyplaced)
			{
				labelCacheSys = api.ModLoader.GetModSystem<ModSystemLabelMeshCache>();
				ownMesh = null;
				PreLoadContentsTexture(api as ICoreClientAPI);
			}
		}
	}

	protected virtual void PreLoadContentsTexture(ICoreClientAPI capi)
	{
		ItemStack contentStack = inventory.FirstNonEmptySlot?.Itemstack;
		if (contentStack != null && LidState != "closed")
		{
			_ = BlockBarrel.getContentTexture(capi, contentStack, out var _)?["null"];
		}
	}

	public override void OnBlockPlaced(ItemStack byItemStack = null)
	{
		if (byItemStack?.Attributes != null)
		{
			string nowType = byItemStack.Attributes.GetString("type", ownBlock.Props.DefaultType);
			string nowLabel = byItemStack.Attributes.GetString("label");
			string nowLidState = byItemStack.Attributes.GetString("lidState", "closed");
			if (nowType != type || nowLabel != label || nowLidState != preferredLidState)
			{
				label = nowLabel;
				type = nowType;
				preferredLidState = nowLidState;
				InitInventory(base.Block, Api);
				Inventory.LateInitialize(InventoryClassName + "-" + Pos, Api);
				Inventory.ResolveBlocksOrItems();
				container.LateInit();
				MarkDirty();
			}
		}
		base.OnBlockPlaced((ItemStack)null);
	}

	public bool OnBlockInteractStart(IPlayer byPlayer, BlockSelection blockSel)
	{
		bool put = byPlayer.Entity.Controls.ShiftKey;
		bool take = !put;
		bool bulk = byPlayer.Entity.Controls.CtrlKey;
		ItemSlot hotbarslot = byPlayer.InventoryManager.ActiveHotbarSlot;
		if (hotbarslot == null)
		{
			throw new Exception("Interact called when byPlayer has null ActiveHotbarSlot");
		}
		if (bulk && Labelled)
		{
			ItemStack? itemstack = hotbarslot.Itemstack;
			if (itemstack != null && itemstack.ItemAttributes?["pigment"]?["color"].Exists == true && blockSel.SelectionBoxIndex == 1)
			{
				if (inventory.Empty && labelStack != null)
				{
					FreeAtlasSpace();
					labelStack = null;
					labelMesh = null;
					MarkDirty(redrawOnClient: true);
					return true;
				}
				if (!Inventory.Empty)
				{
					JsonObject jsonObject = hotbarslot.Itemstack.ItemAttributes["pigment"]["color"];
					int r = jsonObject["red"].AsInt();
					int g = jsonObject["green"].AsInt();
					int b = jsonObject["blue"].AsInt();
					int newLabelColor = ColorUtil.ToRgba(255, (int)GameMath.Clamp((float)r * 1.2f, 0f, 255f), (int)GameMath.Clamp((float)g * 1.2f, 0f, 255f), (int)GameMath.Clamp((float)b * 1.2f, 0f, 255f));
					if (labelStack == null || labelColor != newLabelColor)
					{
						FreeAtlasSpace();
						labelColor = newLabelColor;
						labelStack = inventory.FirstNonEmptySlot.Itemstack.Clone();
						labelStack.Attributes.RemoveAttribute("temperature");
						labelStack.Attributes.RemoveAttribute("transitionstate");
						labelMesh = null;
						byPlayer.Entity.World.PlaySoundAt(ChalkDraw, (double)blockSel.Position.X + blockSel.HitPosition.X, (double)blockSel.Position.InternalY + blockSel.HitPosition.Y, (double)blockSel.Position.Z + blockSel.HitPosition.Z, blockSel.Position.dimension, byPlayer);
						MarkDirty(redrawOnClient: true);
						return true;
					}
				}
				else if (take)
				{
					(Api as ICoreClientAPI)?.TriggerIngameError(this, "empty", Lang.Get("Can't draw item symbol on an empty crate. Put something inside the crate first"));
				}
			}
		}
		if (take)
		{
			int i;
			for (i = 0; i < inventory.Count && inventory[i].Empty; i++)
			{
			}
			if (i >= inventory.Count)
			{
				return true;
			}
			ItemSlot ownSlot = inventory[i];
			int requestedQuantity = ((!bulk) ? 1 : ownSlot.Itemstack.Collectible.MaxStackSize);
			for (; i < inventory.Count; i++)
			{
				if (ownSlot.StackSize >= requestedQuantity)
				{
					break;
				}
				inventory[i].TryPutInto(Api.World, ownSlot, requestedQuantity - ownSlot.StackSize);
			}
			ItemStack stack = ownSlot.TakeOut(requestedQuantity);
			int originalQuantity = stack.StackSize;
			bool num = byPlayer.InventoryManager.TryGiveItemstack(stack, slotNotifyEffect: true);
			int taken = originalQuantity - stack.StackSize;
			if (num)
			{
				if (taken == 0)
				{
					taken = originalQuantity;
				}
				if (originalQuantity > taken)
				{
					new DummySlot(stack).TryPutInto(Api.World, ownSlot, originalQuantity - taken);
				}
				didMoveItems(stack, byPlayer);
			}
			else
			{
				new DummySlot(stack).TryPutInto(Api.World, ownSlot, originalQuantity - taken);
			}
			if (taken == 0)
			{
				(Api as ICoreClientAPI)?.TriggerIngameError(this, "invfull", Lang.Get("item-take-error-invfull"));
			}
			else
			{
				Api.Logger.Audit(string.Concat("{0} Took {1}x{2} from ", base.Block?.Code, " at {3}."), byPlayer.PlayerName, taken, stack?.Collectible.Code, Pos);
				ownSlot.MarkDirty();
				MarkDirty();
			}
			return true;
		}
		if (put && !hotbarslot.Empty)
		{
			ItemSlot ownSlot2 = inventory.FirstNonEmptySlot;
			int quantity = ((!bulk) ? 1 : hotbarslot.StackSize);
			if (ownSlot2 == null)
			{
				if (!hotbarslot.Itemstack.Equals(Api.World, labelStack, GlobalConstants.IgnoredStackAttributes))
				{
					FreeAtlasSpace();
					labelStack = null;
					labelMesh = null;
				}
				if (hotbarslot.TryPutInto(Api.World, inventory[0], quantity) > 0)
				{
					didMoveItems(inventory[0].Itemstack, byPlayer);
					Api.World.Logger.Audit("{0} Put {1}x{2} into Crate at {3}.", byPlayer.PlayerName, quantity, inventory[0].Itemstack?.Collectible.Code, Pos);
				}
			}
			else if (hotbarslot.Itemstack.Equals(Api.World, ownSlot2.Itemstack, GlobalConstants.IgnoredStackAttributes))
			{
				List<ItemSlot> skipSlots = new List<ItemSlot>();
				while (hotbarslot.StackSize > 0 && skipSlots.Count < inventory.Count)
				{
					WeightedSlot wslot = inventory.GetBestSuitedSlot(hotbarslot, null, skipSlots);
					if (wslot.slot == null)
					{
						break;
					}
					if (hotbarslot.TryPutInto(Api.World, wslot.slot, quantity) > 0)
					{
						didMoveItems(wslot.slot.Itemstack, byPlayer);
						Api.World.Logger.Audit("{0} Put {1}x{2} into Crate at {3}.", byPlayer.PlayerName, quantity, wslot.slot.Itemstack?.Collectible.Code, Pos);
						if (!bulk)
						{
							break;
						}
					}
					skipSlots.Add(wslot.slot);
				}
			}
			hotbarslot.MarkDirty();
			MarkDirty();
		}
		return true;
	}

	protected void didMoveItems(ItemStack stack, IPlayer byPlayer)
	{
		if (Api.Side == EnumAppSide.Client)
		{
			ownMesh = null;
			PreLoadContentsTexture(Api as ICoreClientAPI);
		}
		(Api as ICoreClientAPI)?.World.Player.TriggerFpAnimation(EnumHandInteract.HeldItemInteract);
		SoundAttributes? obj = stack?.Block?.Sounds?.Place;
		Api.World.PlaySoundAt(obj ?? GlobalConstants.DefaultBuildSound, byPlayer.Entity, byPlayer);
	}

	protected virtual void InitInventory(Block block, ICoreAPI api)
	{
		if (block?.Attributes != null)
		{
			JsonObject props = block.Attributes["properties"][type];
			if (!props.Exists)
			{
				props = block.Attributes["properties"]["*"];
			}
			quantitySlots = props["quantitySlots"].AsInt(quantitySlots);
			retrieveOnly = props["retrieveOnly"].AsBool();
		}
		inventory = new InventoryGeneric(quantitySlots, null, null);
		inventory.BaseWeight = 1f;
		inventory.OnGetSuitability = (ItemSlot sourceSlot, ItemSlot targetSlot, bool isMerge) => (isMerge ? (inventory.BaseWeight + 3f) : (inventory.BaseWeight + 1f)) + (float)((sourceSlot.Inventory is InventoryBasePlayer) ? 1 : 0);
		inventory.OnGetAutoPullFromSlot = GetAutoPullFromSlot;
		inventory.OnGetAutoPushIntoSlot = GetAutoPushIntoSlot;
		if (block?.Attributes != null)
		{
			if (block.Attributes["spoilSpeedMulByFoodCat"][type].Exists)
			{
				inventory.PerishableFactorByFoodCategory = block.Attributes["spoilSpeedMulByFoodCat"][type].AsObject<Dictionary<EnumFoodCategory, float>>();
			}
			if (block.Attributes["transitionSpeedMul"][type].Exists)
			{
				inventory.TransitionableSpeedMulByType = block.Attributes["transitionSpeedMul"][type].AsObject<Dictionary<EnumTransitionType, float>>();
			}
		}
		inventory.PutLocked = retrieveOnly;
		inventory.OnInventoryClosed += OnInvClosed;
		inventory.OnInventoryOpened += OnInvOpened;
		if (api.Side == EnumAppSide.Server)
		{
			inventory.SlotModified += Inventory_SlotModified;
		}
		container.Reset();
	}

	private void Inventory_SlotModified(int obj)
	{
		MarkDirty();
	}

	public Cuboidf[] GetSelectionBoxes()
	{
		if (selBoxCrate == null)
		{
			selBoxCrate = base.Block.SelectionBoxes[0].RotatedCopy(0f, (int)Math.Round(rotAngleY * (180f / (float)Math.PI) / 90f) * 90, 0f, new Vec3d(0.5, 0.0, 0.5));
			selBoxLabel = base.Block.SelectionBoxes[1].RotatedCopy(0f, rotAngleY * (180f / (float)Math.PI), 0f, new Vec3d(0.5, 0.0, 0.5));
		}
		if (Api.Side == EnumAppSide.Client)
		{
			ItemSlot hotbarslot = ((ICoreClientAPI)Api).World.Player.InventoryManager.ActiveHotbarSlot;
			if (Labelled)
			{
				ItemStack? itemstack = hotbarslot.Itemstack;
				if (itemstack != null && itemstack.ItemAttributes?["pigment"]?["color"].Exists == true)
				{
					return new Cuboidf[2] { selBoxCrate, selBoxLabel };
				}
			}
		}
		return new Cuboidf[1] { selBoxCrate };
	}

	public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
	{
		BlockCrate block = worldForResolving.GetBlock(new AssetLocation(tree.GetString("blockCode"))) as BlockCrate;
		type = tree.GetString("type", block?.Props.DefaultType);
		label = tree.GetString("label");
		MeshAngle = tree.GetFloat("meshAngle", MeshAngle);
		labelColor = tree.GetInt("labelColor");
		labelStack = tree.GetItemstack("labelStack");
		preferredLidState = tree.GetString("lidState");
		if (labelStack != null && !labelStack.ResolveBlockOrItem(worldForResolving))
		{
			labelStack = null;
		}
		if (inventory == null)
		{
			if (tree.HasAttribute("blockCode"))
			{
				InitInventory(block, worldForResolving.Api);
			}
			else
			{
				InitInventory(null, worldForResolving.Api);
			}
		}
		if (Api != null && Api.Side == EnumAppSide.Client)
		{
			ownMesh = null;
			PreLoadContentsTexture(Api as ICoreClientAPI);
			MarkDirty(redrawOnClient: true);
		}
		base.FromTreeAttributes(tree, worldForResolving);
	}

	public override void ToTreeAttributes(ITreeAttribute tree)
	{
		base.ToTreeAttributes(tree);
		if (base.Block != null)
		{
			tree.SetString("forBlockCode", base.Block.Code.ToShortString());
		}
		if (type == null)
		{
			type = ownBlock.Props.DefaultType;
		}
		tree.SetString("label", label);
		tree.SetString("type", type);
		tree.SetFloat("meshAngle", MeshAngle);
		tree.SetInt("labelColor", labelColor);
		tree.SetString("lidState", preferredLidState);
		tree.SetItemstack("labelStack", labelStack);
	}

	public override void OnLoadCollectibleMappings(IWorldAccessor worldForResolve, Dictionary<int, AssetLocation> oldBlockIdMapping, Dictionary<int, AssetLocation> oldItemIdMapping, int schematicSeed, bool resolveImports)
	{
		base.OnLoadCollectibleMappings(worldForResolve, oldBlockIdMapping, oldItemIdMapping, schematicSeed, resolveImports);
		if (labelStack != null && !labelStack.FixMapping(oldBlockIdMapping, oldItemIdMapping, worldForResolve))
		{
			labelStack = null;
		}
	}

	public override void OnStoreCollectibleMappings(Dictionary<int, AssetLocation> blockIdMapping, Dictionary<int, AssetLocation> itemIdMapping)
	{
		base.OnStoreCollectibleMappings(blockIdMapping, itemIdMapping);
		labelStack?.Collectible.OnStoreCollectibleMappings(Api.World, new DummySlot(labelStack), blockIdMapping, itemIdMapping);
	}

	private ItemSlot GetAutoPullFromSlot(BlockFacing atBlockFace)
	{
		if (atBlockFace == BlockFacing.DOWN)
		{
			return inventory.FirstNonEmptySlot;
		}
		return null;
	}

	private ItemSlot GetAutoPushIntoSlot(BlockFacing atBlockFace, ItemSlot fromSlot)
	{
		ItemSlot slotNonEmpty = inventory.FirstNonEmptySlot;
		if (slotNonEmpty == null)
		{
			return inventory[0];
		}
		if (slotNonEmpty.Itemstack.Equals(Api.World, fromSlot.Itemstack, GlobalConstants.IgnoredStackAttributes))
		{
			foreach (ItemSlot slot in inventory)
			{
				if (slot.Itemstack == null || slot.StackSize < slot.Itemstack.Collectible.MaxStackSize)
				{
					return slot;
				}
			}
			return null;
		}
		return null;
	}

	protected virtual void OnInvOpened(IPlayer player)
	{
		inventory.PutLocked = retrieveOnly && player.WorldData.CurrentGameMode != EnumGameMode.Creative;
	}

	protected virtual void OnInvClosed(IPlayer player)
	{
		inventory.PutLocked = retrieveOnly;
	}

	private void loadOrCreateMesh()
	{
		if (base.Block == null)
		{
			Block block = (base.Block = Api.World.BlockAccessor.GetBlock(Pos) as BlockCrate);
		}
		if (!(base.Block is BlockCrate block3))
		{
			return;
		}
		string cacheKey = "crateMeshes" + block3.FirstCodePart();
		Dictionary<string, MeshData> meshes = ObjectCacheUtil.GetOrCreate(Api, cacheKey, () => new Dictionary<string, MeshData>());
		CompositeShape cshape = ownBlock.Props[type].Shape;
		if (!(cshape?.Base == null))
		{
			ItemStack firstStack = inventory.FirstNonEmptySlot?.Itemstack;
			string meshKey = type + block3.Subtype + "-" + label + "-" + LidState + "-" + ((LidState == "closed") ? null : (firstStack?.StackSize + "-" + firstStack?.GetHashCode()));
			if (!meshes.TryGetValue(meshKey, out var mesh))
			{
				mesh = (meshes[meshKey] = block3.GenMesh(Api as ICoreClientAPI, firstStack, type, label, LidState, cshape, new Vec3f(cshape.rotateX, cshape.rotateY, cshape.rotateZ)));
			}
			ownMesh = mesh.Clone().Rotate(origin, 0f, MeshAngle, 0f).Scale(origin, rndScale, rndScale, rndScale);
		}
	}

	private void genLabelMesh()
	{
		if (LabelProps?.EditableShape == null || labelStack == null || requested)
		{
			return;
		}
		if (labelCacheSys == null)
		{
			labelCacheSys = Api.ModLoader.GetModSystem<ModSystemLabelMeshCache>();
		}
		requested = true;
		labelCacheSys.RequestLabelTexture(labelColor, Pos, labelStack, delegate(int texSubId)
		{
			GenLabelMeshWithItemStack(texSubId);
			((ICoreClientAPI)Api).Event.EnqueueMainThreadTask(delegate
			{
				MarkDirty(redrawOnClient: true);
			}, "markcratedirty");
			requested = false;
		});
	}

	private void GenLabelMeshWithItemStack(int textureSubId)
	{
		ICoreClientAPI capi = (ICoreClientAPI)Api;
		TextureAtlasPosition texPos = capi.BlockTextureAtlas.Positions[textureSubId];
		labelMesh = ownBlock.GenLabelMesh(capi, label, texPos, editableVariant: true);
		labelMesh.Rotate(origin, 0f, rotAngleY + (float)Math.PI, 0f).Scale(origin, rndScale, rndScale, rndScale);
	}

	public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tesselator)
	{
		if (base.OnTesselation(mesher, tesselator))
		{
			return true;
		}
		if (ownMesh == null)
		{
			loadOrCreateMesh();
			if (ownMesh == null)
			{
				return true;
			}
		}
		if (labelMesh == null)
		{
			genLabelMesh();
		}
		mesher.AddMeshData(ownMesh);
		mesher.AddMeshData(labelMesh);
		return true;
	}

	public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
	{
		int stacksize = 0;
		foreach (ItemSlot slot in inventory)
		{
			stacksize += slot.StackSize;
		}
		if (stacksize > 0)
		{
			dsc.AppendLine(Lang.Get("Contents: {0}x{1}", stacksize, inventory.FirstNonEmptySlot.GetStackName()));
		}
		else
		{
			dsc.AppendLine(Lang.Get("Empty"));
		}
		base.GetBlockInfo(forPlayer, dsc);
	}

	public override void OnBlockUnloaded()
	{
		FreeAtlasSpace();
		base.OnBlockUnloaded();
	}

	public override void OnBlockRemoved()
	{
		FreeAtlasSpace();
		base.OnBlockRemoved();
	}

	private void FreeAtlasSpace()
	{
		if (labelStack != null)
		{
			labelCacheSys?.FreeLabelTexture(labelStack, labelColor, Pos);
		}
	}

	public void OnTransformed(IWorldAccessor worldAccessor, ITreeAttribute tree, int degreeRotation, Dictionary<int, AssetLocation> oldBlockIdMapping, Dictionary<int, AssetLocation> oldItemIdMapping, EnumAxis? flipAxis)
	{
		ownMesh = null;
		MeshAngle = tree.GetFloat("meshAngle");
		MeshAngle -= (float)degreeRotation * ((float)Math.PI / 180f);
		tree.SetFloat("meshAngle", MeshAngle);
	}
}
