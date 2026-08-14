using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.Common.Collectible.Block;

namespace Vintagestory.GameContent;

public class BlockEntityGroundStorage : BlockEntityDisplay, IBlockEntityContainer, IRotatable, IHeatSource, ITemperatureSensitive, IExternalTickable
{
	private static SimpleParticleProperties smokeParticles;

	public object inventoryLock = new object();

	protected InventoryGeneric inventory;

	public bool forceStorageProps;

	protected EnumGroundStorageLayout? overrideLayout;

	protected Cuboidf[] colBoxes;

	protected Cuboidf[] selBoxes;

	private ItemSlot isUsingSlot;

	public bool clientsideFirstPlacement;

	private GroundStorageRenderer renderer;

	public bool UseRenderer;

	public bool NeedsRetesselation;

	public MultiTextureMeshRef[] MeshRefs = new MultiTextureMeshRef[4];

	public ModelTransform[] ModelTransformsRenderer = new ModelTransform[4];

	private bool burning;

	private double burnStartTotalHours;

	private ILoadedSound ambientSound;

	private long listenerId;

	private float burnHoursPerItem;

	private BlockFacing[] facings = (BlockFacing[])BlockFacing.ALLFACES.Clone();

	private byte[] lastLightHsv;

	public GroundStorageProperties StorageProps { get; protected set; }

	public int TransferQuantity => StorageProps?.TransferQuantity ?? 1;

	public int BulkTransferQuantity => StorageProps?.Layout switch
	{
		EnumGroundStorageLayout.Stacking => StorageProps.BulkTransferQuantity, 
		EnumGroundStorageLayout.Messy12 => Math.Min(StorageProps.BulkTransferQuantity, 12), 
		_ => 1, 
	};

	protected virtual int invSlotCount => 4;

	private Dictionary<string, MultiTextureMeshRef> UploadedMeshCache => ObjectCacheUtil.GetOrCreate(Api, "groundStorageUMC", () => new Dictionary<string, MultiTextureMeshRef>());

	public virtual bool CanIgnite
	{
		get
		{
			if (burnHoursPerItem > 0f)
			{
				ItemStack? itemstack = inventory[0].Itemstack;
				if (itemstack == null)
				{
					return false;
				}
				return itemstack.Collectible.GetCombustibleProperties(Api.World, inventory[0].Itemstack, null)?.BurnTemperature > 200;
			}
			return false;
		}
	}

	public int ItemModelCount
	{
		get
		{
			if (StorageProps != null)
			{
				return (int)Math.Ceiling((float)inventory[0].StackSize / (float)StorageProps.ItemsPerModel);
			}
			return 1;
		}
	}

	[Obsolete("Use ItemModelCount * CuboidsPerModel instead, for correct rounding")]
	public int Layers
	{
		get
		{
			if (inventory[0].StackSize != 1 && StorageProps != null)
			{
				return (int)((float)inventory[0].StackSize * StorageProps.ModelItemsToStackSizeRatio);
			}
			return 1;
		}
	}

	public bool IsBurning => burning;

	public bool IsHot => burning;

	public bool IsExternallyTicked { get; set; }

	public override int DisplayedItems
	{
		get
		{
			if (StorageProps == null)
			{
				return 0;
			}
			return StorageProps.Layout switch
			{
				EnumGroundStorageLayout.SingleCenter => 1, 
				EnumGroundStorageLayout.Halves => 2, 
				EnumGroundStorageLayout.WallHalves => 2, 
				EnumGroundStorageLayout.Quadrants => 4, 
				EnumGroundStorageLayout.Messy12 => 1, 
				EnumGroundStorageLayout.Stacking => 1, 
				_ => 0, 
			};
		}
	}

	public int TotalStackSize
	{
		get
		{
			int sum = 0;
			foreach (ItemSlot slot in inventory)
			{
				sum += slot.StackSize;
			}
			return sum;
		}
	}

	public int Capacity
	{
		get
		{
			if (StorageProps == null)
			{
				return 1;
			}
			return StorageProps.Layout switch
			{
				EnumGroundStorageLayout.SingleCenter => 1, 
				EnumGroundStorageLayout.Halves => 2, 
				EnumGroundStorageLayout.WallHalves => 2, 
				EnumGroundStorageLayout.Quadrants => 4, 
				EnumGroundStorageLayout.Messy12 => 12, 
				EnumGroundStorageLayout.Stacking => StorageProps.StackingCapacity, 
				_ => 1, 
			};
		}
	}

	public override InventoryBase Inventory => inventory;

	public override string InventoryClassName => "groundstorage";

	public override string AttributeTransformCode => "groundStorageTransform";

	public float MeshAngle { get; set; }

	public BlockFacing AttachFace { get; set; }

	public override TextureAtlasPosition this[string textureCode]
	{
		get
		{
			GroundStorageProperties storageProps = StorageProps;
			if (storageProps != null && storageProps.Layout == EnumGroundStorageLayout.Stacking && StorageProps.StackingTextures != null && StorageProps.StackingTextures.TryGetValue(textureCode, out var texturePath))
			{
				return getOrCreateTexPos(texturePath);
			}
			return base[textureCode];
		}
	}

	static BlockEntityGroundStorage()
	{
		smokeParticles = new SimpleParticleProperties(1f, 1f, ColorUtil.ToRgba(150, 40, 40, 40), new Vec3d(), new Vec3d(1.0, 0.0, 1.0), new Vec3f(-1f / 32f, 0.1f, -1f / 32f), new Vec3f(1f / 32f, 0.1f, 1f / 32f), 2f, -1f / 160f, 0.2f, 1f, EnumParticleModel.Quad);
		smokeParticles.SizeEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEAR, -0.25f);
		smokeParticles.SelfPropelled = true;
		smokeParticles.AddPos.Set(1.0, 0.0, 1.0);
	}

	public bool CanAttachBlockAt(BlockFacing blockFace, Cuboidi attachmentArea)
	{
		if (StorageProps == null)
		{
			return false;
		}
		if (blockFace == BlockFacing.UP && StorageProps.Layout == EnumGroundStorageLayout.Stacking && inventory[0].StackSize == Capacity)
		{
			return StorageProps.UpSolid;
		}
		return false;
	}

	public BlockEntityGroundStorage()
	{
		inventory = new InventoryGeneric(invSlotCount, null, null, (int slotId, InventoryGeneric inv) => new ItemSlot(inv));
		foreach (ItemSlot item in inventory)
		{
			item.StorageType |= EnumItemStorageFlags.Backpack;
		}
		inventory.OnGetAutoPushIntoSlot = GetAutoPushIntoSlot;
		inventory.OnGetAutoPullFromSlot = GetAutoPullFromSlot;
		colBoxes = new Cuboidf[1]
		{
			new Cuboidf(0f, 0f, 0f, 1f, 0.25f, 1f)
		};
		selBoxes = new Cuboidf[1]
		{
			new Cuboidf(0f, 0f, 0f, 1f, 0.25f, 1f)
		};
	}

	private ItemSlot GetAutoPullFromSlot(BlockFacing atBlockFace)
	{
		return null;
	}

	private ItemSlot GetAutoPushIntoSlot(BlockFacing atBlockFace, ItemSlot fromSlot)
	{
		return null;
	}

	public void ForceStorageProps(GroundStorageProperties storageProps)
	{
		StorageProps = storageProps;
		forceStorageProps = true;
	}

	public override void Initialize(ICoreAPI api)
	{
		capi = api as ICoreClientAPI;
		base.Initialize(api);
		BEBehaviorBurning bh = GetBehavior<BEBehaviorBurning>();
		if (bh != null)
		{
			bh.FirePos = Pos.Copy();
			bh.FuelPos = Pos.Copy();
			bh.OnFireDeath = delegate
			{
				Extinguish();
			};
		}
		UpdateIgnitable();
		DetermineStorageProperties(null);
		if (capi != null)
		{
			float temp = 0f;
			if (!Inventory.Empty)
			{
				int index = 0;
				foreach (ItemSlot item in Inventory)
				{
					ItemStack stack = item.Itemstack;
					if (stack != null)
					{
						temp = Math.Max(temp, stack.Collectible.GetTemperature(capi.World, stack));
						if (stack.Class == EnumItemClass.Block && stack.Block is IBlockMealContainer be && !(be is BlockCrock))
						{
							GetOrCreateMealMesh(be, stack, index);
						}
					}
					index++;
				}
				if (temp >= 450f)
				{
					renderer = new GroundStorageRenderer(capi, this);
				}
			}
			MarkMeshesDirty();
		}
		UpdateBurningState();
	}

	public override void CheckInventoryClearedMidTick()
	{
		if (Inventory.Empty)
		{
			Api.World.BlockAccessor.SetBlock(0, Pos);
		}
	}

	public void CoolNow(float amountRel, OnStackToCool onStackToCoolCallback)
	{
		if (Inventory.Empty)
		{
			return;
		}
		for (int index = 0; index < Inventory.Count; index++)
		{
			ItemSlot slot = Inventory[index];
			ItemStack itemStack = slot.Itemstack;
			if (itemStack?.Collectible == null)
			{
				continue;
			}
			if (itemStack.Collectible.HasBehavior<CollectibleBehaviorQuenchable>())
			{
				onStackToCoolCallback(slot, Pos.ToVec3d(), GlobalConstants.CollectibleDefaultTemperature, sizzle: true);
				continue;
			}
			float temperature = itemStack.Collectible.GetTemperature(Api.World, itemStack);
			float breakChance = Math.Max(0f, amountRel - 0.6f) * Math.Max(temperature - 250f, 0f) / 5000f;
			if ((itemStack.Collectible.Code.Path.Contains("burn") || itemStack.Collectible.Code.Path.Contains("fired")) && Api.World.Rand.NextDouble() < (double)breakChance)
			{
				Api.World.PlaySoundAt(new AssetLocation("sounds/block/ceramicbreak"), Pos, -0.4);
				base.Block.SpawnBlockBrokenParticles(Pos);
				base.Block.SpawnBlockBrokenParticles(Pos);
				int size = itemStack.StackSize;
				slot.Itemstack = GetShatteredStack(itemStack);
				StorageProps = null;
				DetermineStorageProperties(slot);
				forceStorageProps = true;
				slot.Itemstack.StackSize = size;
				slot.MarkDirty();
			}
			else
			{
				if (temperature > 120f)
				{
					Api.World.PlaySoundAt(new AssetLocation("sounds/effect/extinguish"), Pos, -0.5, null, randomizePitch: false, 16f);
				}
				itemStack.Collectible.SetTemperature(Api.World, itemStack, Math.Max(20f, temperature - amountRel * 20f), delayCooldown: false);
			}
		}
		MarkDirty(redrawOnClient: true);
		CheckInventoryClearedMidTick();
	}

	private void UpdateIgnitable()
	{
		CollectibleBehaviorGroundStorable cbhgs = Inventory[0].Itemstack?.Collectible?.GetBehavior<CollectibleBehaviorGroundStorable>();
		if (cbhgs != null)
		{
			burnHoursPerItem = JsonObject.FromJson(cbhgs.propertiesAtString)["burnHoursPerItem"].AsFloat(burnHoursPerItem);
		}
	}

	protected ItemStack GetShatteredStack(ItemStack contents)
	{
		JsonItemStack shatteredStack = contents.Collectible.Attributes?["shatteredStack"].AsObject<JsonItemStack>();
		if (shatteredStack != null)
		{
			shatteredStack.Resolve(Api.World, "shatteredStack for" + contents.Collectible.Code);
			if (shatteredStack.ResolvedItemstack != null)
			{
				return shatteredStack.ResolvedItemstack;
			}
		}
		shatteredStack = base.Block.Attributes?["shatteredStack"].AsObject<JsonItemStack>();
		if (shatteredStack != null)
		{
			shatteredStack.Resolve(Api.World, "shatteredStack for" + contents.Collectible.Code);
			if (shatteredStack.ResolvedItemstack != null)
			{
				return shatteredStack.ResolvedItemstack;
			}
		}
		return null;
	}

	public Cuboidf[] GetSelectionBoxes()
	{
		return selBoxes;
	}

	public Cuboidf[] GetCollisionBoxes()
	{
		return colBoxes;
	}

	public virtual bool OnPlayerInteractStart(IPlayer player, BlockSelection bs)
	{
		isUsingSlot = null;
		ItemSlot ourSlot = GetSlotAt(bs);
		if (ourSlot != null && !ourSlot.Empty)
		{
			IContainedInteractable collectibleInterface = ourSlot.Itemstack.Collectible.GetCollectibleInterface<IContainedInteractable>();
			if (collectibleInterface != null && collectibleInterface.OnContainedInteractStart(this, ourSlot, player, bs))
			{
				BlockGroundStorage.IsUsingContainedBlock = true;
				isUsingSlot = ourSlot;
				return true;
			}
		}
		ItemSlot hotbarSlot = player.InventoryManager.ActiveHotbarSlot;
		if (!hotbarSlot.Empty && !hotbarSlot.Itemstack.Collectible.HasBehavior<CollectibleBehaviorGroundStorable>())
		{
			return false;
		}
		if (!BlockBehaviorReinforcable.AllowRightClickPickup(Api.World, Pos, player))
		{
			return false;
		}
		DetermineStorageProperties(hotbarSlot);
		bool ok = false;
		if (StorageProps != null)
		{
			if (!hotbarSlot.Empty && StorageProps.CtrlKey && !player.Entity.Controls.CtrlKey)
			{
				return false;
			}
			Vec3f hitPos = rotatedOffset(bs.HitPosition.ToVec3f(), 0f - MeshAngle);
			if (StorageProps.Layout == EnumGroundStorageLayout.Quadrants && inventory.Empty)
			{
				double num = Math.Abs((double)hitPos.X - 0.5);
				double dz = Math.Abs((double)hitPos.Z - 0.5);
				if (num < 0.125 && dz < 0.125)
				{
					overrideLayout = EnumGroundStorageLayout.SingleCenter;
					DetermineStorageProperties(hotbarSlot);
				}
			}
			switch (StorageProps.Layout)
			{
			case EnumGroundStorageLayout.SingleCenter:
				if (StorageProps.RandomizeCenterRotation)
				{
					double randomX = Api.World.Rand.NextDouble() * 6.28 - 3.14;
					double randomZ = Api.World.Rand.NextDouble() * 6.28 - 3.14;
					MeshAngle = (float)Math.Atan2(randomX, randomZ);
				}
				ok = putOrGetItemSingle(inventory[0], player, bs);
				break;
			case EnumGroundStorageLayout.Halves:
			case EnumGroundStorageLayout.WallHalves:
				ok = ((!((double)hitPos.X < 0.5)) ? putOrGetItemSingle(inventory[1], player, bs) : putOrGetItemSingle(inventory[0], player, bs));
				break;
			case EnumGroundStorageLayout.Quadrants:
			{
				int pos = (((double)hitPos.X > 0.5) ? 2 : 0) + (((double)hitPos.Z > 0.5) ? 1 : 0);
				ok = putOrGetItemSingle(inventory[pos], player, bs);
				break;
			}
			case EnumGroundStorageLayout.Stacking:
			case EnumGroundStorageLayout.Messy12:
				ok = putOrGetItemStacking(player, bs);
				break;
			}
		}
		UpdateIgnitable();
		renderer?.UpdateTemps();
		if (ok)
		{
			MarkDirty();
		}
		if (inventory.Empty && !clientsideFirstPlacement)
		{
			Api.World.BlockAccessor.SetBlock(0, Pos);
			Api.World.BlockAccessor.TriggerNeighbourBlockUpdate(Pos);
			if (lastLightHsv != null && lastLightHsv[2] > 0)
			{
				Api.World.BlockAccessor.RemoveBlockLight((byte[])lastLightHsv.Clone(), Pos);
			}
		}
		else
		{
			byte[] lshv = GetLightHsv();
			if (lastLightHsv != null && lastLightHsv[2] > 0 && (lshv == null || lshv[2] == 0))
			{
				Api.World.BlockAccessor.RemoveBlockLight((byte[])lastLightHsv.Clone(), Pos);
			}
		}
		return ok;
	}

	public bool OnPlayerInteractStep(float secondsUsed, IPlayer byPlayer, BlockSelection blockSel)
	{
		IContainedInteractable collIci = isUsingSlot?.Itemstack?.Collectible.GetCollectibleInterface<IContainedInteractable>();
		if (collIci != null)
		{
			return collIci.OnContainedInteractStep(secondsUsed, this, isUsingSlot, byPlayer, blockSel);
		}
		isUsingSlot = null;
		return false;
	}

	public void OnPlayerInteractStop(float secondsUsed, IPlayer byPlayer, BlockSelection blockSel)
	{
		(isUsingSlot?.Itemstack?.Collectible.GetCollectibleInterface<IContainedInteractable>())?.OnContainedInteractStop(secondsUsed, this, isUsingSlot, byPlayer, blockSel);
		isUsingSlot = null;
	}

	public bool OnPlayerInteractCancel(float secondsUsed, IPlayer byPlayer, BlockSelection blockSel, EnumItemUseCancelReason cancelReason)
	{
		(isUsingSlot?.Itemstack?.Collectible.GetCollectibleInterface<IContainedInteractable>())?.OnContainedInteractCancel(secondsUsed, this, isUsingSlot, byPlayer, blockSel, cancelReason);
		isUsingSlot = null;
		return true;
	}

	public ItemSlot GetSlotAt(BlockSelection bs)
	{
		if (StorageProps == null)
		{
			return null;
		}
		Vec3f hitPos = rotatedOffset(bs.HitPosition.ToVec3f(), 0f - MeshAngle);
		switch (StorageProps.Layout)
		{
		case EnumGroundStorageLayout.Halves:
		case EnumGroundStorageLayout.WallHalves:
			if ((double)hitPos.X < 0.5)
			{
				return inventory[0];
			}
			return inventory[1];
		case EnumGroundStorageLayout.Quadrants:
		{
			int pos = (((double)hitPos.X > 0.5) ? 2 : 0) + (((double)hitPos.Z > 0.5) ? 1 : 0);
			return inventory[pos];
		}
		case EnumGroundStorageLayout.SingleCenter:
		case EnumGroundStorageLayout.Stacking:
		case EnumGroundStorageLayout.Messy12:
			return inventory[0];
		default:
			return null;
		}
	}

	public bool OnTryCreateKiln()
	{
		ItemStack stack = inventory.FirstNonEmptySlot.Itemstack;
		if (stack == null || StorageProps == null)
		{
			return false;
		}
		if (stack.StackSize > StorageProps.MaxFireable)
		{
			capi?.TriggerIngameError(this, "overfull", Lang.Get("Can only fire up to {0} at once.", StorageProps.MaxFireable));
			return false;
		}
		CombustibleProperties combustibleProps = stack.Collectible.GetCombustibleProperties(Api.World, stack, null);
		if (combustibleProps == null || combustibleProps.SmeltingType != EnumSmeltType.Fire)
		{
			capi?.TriggerIngameError(this, "notfireable", Lang.Get("This is not a fireable block or item", StorageProps.MaxFireable));
			return false;
		}
		return true;
	}

	public virtual void DetermineStorageProperties(ItemSlot sourceSlot)
	{
		ItemStack sourceStack = inventory.FirstNonEmptySlot?.Itemstack ?? sourceSlot?.Itemstack;
		GroundStorageProperties StorageProps = this.StorageProps;
		if (!forceStorageProps && StorageProps == null)
		{
			if (sourceStack == null)
			{
				return;
			}
			GroundStorageProperties groundStorageProperties = (this.StorageProps = sourceStack.Collectible?.GetBehavior<CollectibleBehaviorGroundStorable>()?.StorageProps);
			StorageProps = groundStorageProperties;
		}
		if (StorageProps != null)
		{
			regenCollisionSelectionBox();
			UpdateLegacyStorageLayouts();
			if (overrideLayout.HasValue)
			{
				this.StorageProps = StorageProps.Clone();
				this.StorageProps.Layout = overrideLayout.Value;
			}
		}
	}

	private void regenCollisionSelectionBox()
	{
		ItemStack sourceStack = inventory.FirstNonEmptySlot?.Itemstack;
		Cuboidf selBox;
		Cuboidf colBox = ((StorageProps.CollisionBox != null) ? (selBox = StorageProps.CollisionBox.Clone()) : ((sourceStack?.Block == null) ? (selBox = null) : (selBox = sourceStack.Block.CollisionBoxes[0].Clone())));
		if (StorageProps.SelectionBox != null)
		{
			selBox = StorageProps.SelectionBox.Clone();
		}
		if (StorageProps.CbScaleYByLayer != 0f)
		{
			colBox = colBox.Clone();
			colBox.Y2 *= (int)Math.Ceiling(StorageProps.CbScaleYByLayer * (float)inventory[0].StackSize) * 8 / 8;
			selBox = colBox;
		}
		if (colBox != null)
		{
			colBoxes[0] = colBox;
		}
		if (selBox != null)
		{
			selBoxes[0] = selBox;
		}
	}

	protected virtual void UpdateLegacyStorageLayouts()
	{
		if (StorageProps == null)
		{
			return;
		}
		EnumGroundStorageLayout layout = StorageProps.Layout;
		bool flag = ((layout == EnumGroundStorageLayout.WallHalves || layout == EnumGroundStorageLayout.Stacking) ? true : false);
		bool flag2 = flag;
		if (!flag2)
		{
			bool flag3;
			switch (overrideLayout)
			{
			case EnumGroundStorageLayout.WallHalves:
			case EnumGroundStorageLayout.Stacking:
				flag3 = true;
				break;
			default:
				flag3 = false;
				break;
			}
			flag2 = flag3;
		}
		if (flag2)
		{
			overrideLayout = null;
		}
		EnumGroundStorageLayout currentLayout = overrideLayout ?? StorageProps.Layout;
		int totalSlots = UsableSlots(currentLayout);
		if (totalSlots <= 0 || totalSlots >= 4)
		{
			return;
		}
		ItemSlot[] fullSlots = inventory.Where((ItemSlot slot) => !slot.Empty).ToArray();
		if (fullSlots.Length == 0 || (fullSlots.Length == 1 && totalSlots == 1 && !inventory[0].Empty) || (fullSlots.Length == 2 && totalSlots == 2 && !inventory[0].Empty && !inventory[1].Empty))
		{
			return;
		}
		if (fullSlots.Length == 1)
		{
			if (totalSlots == 2 && (!inventory[0].Empty || !inventory[1].Empty))
			{
				return;
			}
			inventory[0].TryFlipWith(fullSlots[0]);
			if (!inventory[0].Empty)
			{
				return;
			}
		}
		if (currentLayout == EnumGroundStorageLayout.Stacking && inventory.All((ItemSlot slot) => slot.Empty || slot.Itemstack.Equals(Api.World, fullSlots[0].Itemstack, GlobalConstants.IgnoredStackAttributes)))
		{
			for (int i = 0; i < fullSlots.Length; i++)
			{
				fullSlots[i].TryPutInto(Api.World, inventory[0]);
			}
			fullSlots = inventory.Where((ItemSlot slot) => !slot.Empty).ToArray();
			if (fullSlots.Length == 1)
			{
				return;
			}
		}
		if (totalSlots == 2 && fullSlots.Length == 2)
		{
			fullSlots[0].TryPutInto(Api.World, inventory[0]);
			fullSlots[1].TryPutInto(Api.World, inventory[1]);
			if (!inventory[0].Empty && !inventory[1].Empty)
			{
				return;
			}
		}
		if (fullSlots.Length > 2)
		{
			EnumGroundStorageLayout? enumGroundStorageLayout = overrideLayout;
			if (enumGroundStorageLayout.HasValue && enumGroundStorageLayout == EnumGroundStorageLayout.Halves)
			{
				overrideLayout = null;
			}
		}
		if (StorageProps.Layout != EnumGroundStorageLayout.Quadrants)
		{
			layout = overrideLayout.GetValueOrDefault();
			if (!overrideLayout.HasValue)
			{
				layout = EnumGroundStorageLayout.Quadrants;
				overrideLayout = layout;
			}
		}
	}

	public int UsableSlots(EnumGroundStorageLayout layout)
	{
		return layout switch
		{
			EnumGroundStorageLayout.SingleCenter => 1, 
			EnumGroundStorageLayout.Halves => 2, 
			EnumGroundStorageLayout.WallHalves => 2, 
			EnumGroundStorageLayout.Quadrants => 4, 
			EnumGroundStorageLayout.Messy12 => 1, 
			EnumGroundStorageLayout.Stacking => 1, 
			_ => 0, 
		};
	}

	protected bool putOrGetItemStacking(IPlayer byPlayer, BlockSelection bs)
	{
		if (Api.Side == EnumAppSide.Client)
		{
			(byPlayer as IClientPlayer)?.TriggerFpAnimation(EnumHandInteract.HeldItemInteract);
			return true;
		}
		ItemSlot hotbarSlot = byPlayer.InventoryManager.ActiveHotbarSlot;
		bool sneaking = byPlayer.Entity.Controls.ShiftKey;
		bool equalStack = inventory[0].Empty || !sneaking || (hotbarSlot.Itemstack != null && hotbarSlot.Itemstack.Equals(Api.World, inventory[0].Itemstack, GlobalConstants.IgnoredStackAttributes));
		BlockPos abovePos = Pos.UpCopy();
		BlockEntityGroundStorage beg = base.Block.GetBlockEntity<BlockEntityGroundStorage>(abovePos);
		if (TotalStackSize >= Capacity && ((beg != null && equalStack && beg.StorageProps.Layout == EnumGroundStorageLayout.Stacking) || (hotbarSlot.Empty && beg != null && beg.inventory[0].Itemstack?.Equals(Api.World, inventory[0].Itemstack, GlobalConstants.IgnoredStackAttributes) == true)))
		{
			return beg.OnPlayerInteractStart(byPlayer, bs);
		}
		if (sneaking && hotbarSlot.Empty)
		{
			return false;
		}
		if (StorageProps == null)
		{
			return false;
		}
		if (sneaking && TotalStackSize >= Capacity)
		{
			Block pileblock = Api.World.BlockAccessor.GetBlock(Pos);
			Block aboveblock = Api.World.BlockAccessor.GetBlock(abovePos);
			if (abovePos.Y >= Api.World.BlockAccessor.MapSizeY)
			{
				return false;
			}
			if (aboveblock.IsReplacableBy(pileblock))
			{
				if (!equalStack && bs.Face != BlockFacing.UP)
				{
					return false;
				}
				int stackHeight = 1;
				if (StorageProps.MaxStackingHeight > 0)
				{
					BlockPos tempPos = Pos.Copy();
					while (true)
					{
						BlockEntityGroundStorage blockEntity = base.Block.GetBlockEntity<BlockEntityGroundStorage>(tempPos.Down());
						if (blockEntity == null || blockEntity.inventory[0].Itemstack?.Equals(Api.World, inventory[0].Itemstack, GlobalConstants.IgnoredStackAttributes) != true)
						{
							break;
						}
						stackHeight++;
					}
				}
				if (StorageProps.MaxStackingHeight < 0 || stackHeight < StorageProps.MaxStackingHeight || !equalStack)
				{
					BlockGroundStorage obj = pileblock as BlockGroundStorage;
					BlockSelection bsc = bs.Clone();
					bsc.Position = Pos;
					bsc.Face = BlockFacing.UP;
					return obj.CreateStorage(Api.World, bsc, byPlayer);
				}
			}
			return false;
		}
		if (sneaking && !equalStack)
		{
			return false;
		}
		lock (inventoryLock)
		{
			if (sneaking)
			{
				return TryPutItem(byPlayer);
			}
			return TryTakeItem(byPlayer);
		}
	}

	public virtual bool TryPutItem(IPlayer player)
	{
		if (TotalStackSize >= Capacity)
		{
			return false;
		}
		ItemSlot hotbarSlot = player.InventoryManager.ActiveHotbarSlot;
		if (hotbarSlot.Itemstack == null)
		{
			return false;
		}
		ItemSlot invSlot = inventory[0];
		if (invSlot.Empty)
		{
			bool putBulk = player.Entity.Controls.CtrlKey;
			if (hotbarSlot.TryPutInto(Api.World, invSlot, putBulk ? BulkTransferQuantity : TransferQuantity) > 0)
			{
				Api.World.PlaySoundAt(StorageProps.PlaceRemoveSound.WithPathPrefixOnce("sounds/"), (double)Pos.X + 0.5, Pos.InternalY, (double)Pos.Z + 0.5, null, 0.88f + (float)Api.World.Rand.NextDouble() * 0.24f, 16f);
				LightUpdate(invSlot.Itemstack);
			}
			Api.World.Logger.Audit("{0} Put {1}x{2} into new Ground storage at {3}.", player.PlayerName, TransferQuantity, invSlot.Itemstack.Collectible.Code, Pos);
			Api.World.BlockAccessor.TriggerNeighbourBlockUpdate(Pos);
			regenCollisionSelectionBox();
			return true;
		}
		if (invSlot.Itemstack.Equals(Api.World, hotbarSlot.Itemstack, GlobalConstants.IgnoredStackAttributes))
		{
			bool putBulk2 = player.Entity.Controls.CtrlKey;
			int q = GameMath.Min(hotbarSlot.StackSize, putBulk2 ? BulkTransferQuantity : TransferQuantity, Capacity - TotalStackSize);
			int oldSize = invSlot.Itemstack.StackSize;
			invSlot.Itemstack.StackSize += q;
			if (oldSize + q > 0)
			{
				float tempPile = invSlot.Itemstack.Collectible.GetTemperature(Api.World, invSlot.Itemstack);
				float tempAdded = hotbarSlot.Itemstack.Collectible.GetTemperature(Api.World, hotbarSlot.Itemstack);
				invSlot.Itemstack.Collectible.SetTemperature(Api.World, invSlot.Itemstack, (tempPile * (float)oldSize + tempAdded * (float)q) / (float)(oldSize + q), delayCooldown: false);
			}
			if (player.WorldData.CurrentGameMode != EnumGameMode.Creative)
			{
				hotbarSlot.TakeOut(q);
				hotbarSlot.OnItemSlotModified(null);
			}
			LightUpdate(invSlot.Itemstack);
			Api.World.PlaySoundAt(StorageProps.PlaceRemoveSound.WithPathPrefixOnce("sounds/"), (double)Pos.X + 0.5, Pos.InternalY, (double)Pos.Z + 0.5, null, 0.88f + (float)Api.World.Rand.NextDouble() * 0.24f, 16f);
			Api.World.Logger.Audit("{0} Put {1}x{2} into Ground storage at {3}.", player.PlayerName, q, invSlot.Itemstack.Collectible.Code, Pos);
			Api.World.BlockAccessor.TriggerNeighbourBlockUpdate(Pos);
			regenCollisionSelectionBox();
			MarkDirty();
			Cuboidf[] collBoxes = Api.World.BlockAccessor.GetBlock(Pos).GetCollisionBoxes(Api.World.BlockAccessor, Pos);
			if (collBoxes != null && collBoxes.Length != 0 && CollisionTester.AabbIntersect(collBoxes[0], Pos.X, Pos.Y, Pos.Z, player.Entity.SelectionBox, player.Entity.Pos.XYZ))
			{
				player.Entity.Pos.Y += (double)collBoxes[0].Y2 - (player.Entity.Pos.Y - (double)(int)player.Entity.Pos.Y);
			}
			return true;
		}
		return false;
	}

	public void LightUpdate(ItemStack itemstack)
	{
		lastLightHsv = itemstack.Collectible.GetLightHsv(Api.World.BlockAccessor, null, itemstack);
		if (lastLightHsv != null && lastLightHsv[2] > 0)
		{
			Api.World.BlockAccessor.ExchangeBlock(base.Block.Id, Pos);
		}
	}

	public bool TryTakeItem(IPlayer player)
	{
		int q = GameMath.Min(player.Entity.Controls.CtrlKey ? BulkTransferQuantity : TransferQuantity, TotalStackSize);
		ItemStack stack = null;
		if (inventory[0]?.Itemstack != null)
		{
			stack = inventory[0].TakeOut(q);
			player.InventoryManager.TryGiveItemstack(stack);
			if (stack.StackSize > 0)
			{
				Api.World.SpawnItemEntity(stack, Pos);
			}
			Api.World.Logger.Audit("{0} Took {1}x{2} from Ground storage at {3}.", player.PlayerName, q, stack.Collectible.Code, Pos);
			LightUpdate(stack);
		}
		if (TotalStackSize == 0)
		{
			Api.World.BlockAccessor.SetBlock(0, Pos);
			if (stack != null)
			{
				Api.World.BlockAccessor.RemoveBlockLight(stack.Collectible.GetLightHsv(Api.World.BlockAccessor, null, stack), Pos);
			}
		}
		else
		{
			Api.World.BlockAccessor.TriggerNeighbourBlockUpdate(Pos);
		}
		Api.World.PlaySoundAt(StorageProps.PlaceRemoveSound, (double)Pos.X + 0.5, Pos.InternalY, (double)Pos.Z + 0.5, null, 0.88f + (float)Api.World.Rand.NextDouble() * 0.24f, 16f);
		MarkDirty();
		regenCollisionSelectionBox();
		(player as IClientPlayer)?.TriggerFpAnimation(EnumHandInteract.HeldItemInteract);
		return true;
	}

	public bool putOrGetItemSingle(ItemSlot ourSlot, IPlayer player, BlockSelection bs)
	{
		ItemSlot hotbarSlot = player.InventoryManager.ActiveHotbarSlot;
		if (!hotbarSlot.Empty && !inventory.Empty)
		{
			EnumGroundStorageLayout? hotbarlayout = hotbarSlot.Itemstack.Collectible.GetBehavior<CollectibleBehaviorGroundStorable>()?.StorageProps.Layout;
			bool layoutEqual = StorageProps.Layout == hotbarlayout;
			if (StorageProps.Layout == EnumGroundStorageLayout.Quadrants && hotbarlayout == EnumGroundStorageLayout.Messy12)
			{
				layoutEqual = true;
				overrideLayout = EnumGroundStorageLayout.Quadrants;
			}
			if (!layoutEqual)
			{
				return false;
			}
		}
		lock (inventoryLock)
		{
			if (ourSlot.Empty)
			{
				if (hotbarSlot.Empty || !player.Entity.Controls.ShiftKey)
				{
					return false;
				}
				if (player.WorldData.CurrentGameMode == EnumGameMode.Creative)
				{
					ItemStack stack = hotbarSlot.Itemstack.Clone();
					stack.StackSize = 1;
					if (new DummySlot(stack).TryPutInto(Api.World, ourSlot, TransferQuantity) > 0)
					{
						Api.World.PlaySoundAt(StorageProps.PlaceRemoveSound, (double)Pos.X + 0.5, Pos.InternalY, (double)Pos.Z + 0.5, player, 0.88f + (float)Api.World.Rand.NextDouble() * 0.24f, 16f);
						Api.World.Logger.Audit("{0} Put 1x{1} into Ground storage at {2}.", player.PlayerName, ourSlot.Itemstack.Collectible.Code, Pos);
						LightUpdate(stack);
					}
				}
				else if (hotbarSlot.TryPutInto(Api.World, ourSlot, TransferQuantity) > 0)
				{
					Api.World.PlaySoundAt(StorageProps.PlaceRemoveSound, (double)Pos.X + 0.5, Pos.InternalY, (double)Pos.Z + 0.5, player, 0.88f + (float)Api.World.Rand.NextDouble() * 0.24f, 16f);
					Api.World.Logger.Audit("{0} Put 1x{1} into Ground storage at {2}.", player.PlayerName, ourSlot.Itemstack.Collectible.Code, Pos);
					LightUpdate(ourSlot.Itemstack);
				}
			}
			else
			{
				if (!player.InventoryManager.TryGiveItemstack(ourSlot.Itemstack, slotNotifyEffect: true))
				{
					Api.World.SpawnItemEntity(ourSlot.Itemstack, Pos);
				}
				LightUpdate(ourSlot.Itemstack);
				Api.World.PlaySoundAt(StorageProps.PlaceRemoveSound, (double)Pos.X + 0.5, Pos.InternalY, (double)Pos.Z + 0.5, player, 0.88f + (float)Api.World.Rand.NextDouble() * 0.24f, 16f);
				Api.World.Logger.Audit("{0} Took 1x{1} from Ground storage at {2}.", player.PlayerName, ourSlot.Itemstack?.Collectible.Code, Pos);
				ourSlot.Itemstack = null;
				ourSlot.MarkDirty();
			}
		}
		return true;
	}

	public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
	{
		base.FromTreeAttributes(tree, worldForResolving);
		clientsideFirstPlacement = false;
		forceStorageProps = tree.GetBool("forceStorageProps");
		if (forceStorageProps)
		{
			StorageProps = JsonUtil.FromString<GroundStorageProperties>(tree.GetString("storageProps"));
		}
		overrideLayout = null;
		if (tree.HasAttribute("overrideLayout"))
		{
			overrideLayout = (EnumGroundStorageLayout)tree.GetInt("overrideLayout");
		}
		if (Api != null)
		{
			DetermineStorageProperties(null);
		}
		MeshAngle = tree.GetFloat("meshAngle");
		AttachFace = BlockFacing.ALLFACES[tree.GetInt("attachFace")];
		bool wasBurning = burning;
		burning = tree.GetBool("burning");
		burnStartTotalHours = tree.GetDouble("lastTickTotalHours");
		IsExternallyTicked = tree.GetBool("isExternallyTicked");
		ICoreAPI api = Api;
		if (api != null && api.Side == EnumAppSide.Client && !wasBurning && burning)
		{
			UpdateBurningState();
		}
		if (!burning)
		{
			UnregisterTickListener();
			ambientSound?.Stop();
		}
		RedrawAfterReceivingTreeAttributes(worldForResolving);
	}

	public override void ToTreeAttributes(ITreeAttribute tree)
	{
		base.ToTreeAttributes(tree);
		tree.SetBool("forceStorageProps", forceStorageProps);
		if (forceStorageProps && StorageProps != null)
		{
			tree.SetString("storageProps", JsonUtil.ToString(StorageProps));
		}
		if (overrideLayout.HasValue)
		{
			tree.SetInt("overrideLayout", (int)overrideLayout.Value);
		}
		tree.SetBool("burning", burning);
		tree.SetDouble("lastTickTotalHours", burnStartTotalHours);
		tree.SetFloat("meshAngle", MeshAngle);
		tree.SetInt("attachFace", AttachFace?.Index ?? 0);
		tree.SetBool("isExternallyTicked", IsExternallyTicked);
	}

	public override void OnBlockBroken(IPlayer byPlayer = null)
	{
		Api.World.BlockAccessor.RemoveBlockLight(GetLightHsv(), Pos);
	}

	public virtual string GetBlockName()
	{
		if (StorageProps == null || inventory.Empty)
		{
			return Lang.Get("Empty pile");
		}
		string[] contentSummary = getContentSummary();
		if (contentSummary.Length == 1)
		{
			ItemSlot firstSlot = inventory.FirstNonEmptySlot;
			ItemStack stack = firstSlot.Itemstack;
			int sumQ = inventory.Sum((ItemSlot s) => s.StackSize);
			string name = firstSlot.Itemstack.Collectible.GetCollectibleInterface<IContainedCustomName>()?.GetContainedName(firstSlot, sumQ);
			if (name != null)
			{
				return name;
			}
			if (sumQ == 1)
			{
				return stack.GetName();
			}
			return contentSummary[0];
		}
		return Lang.Get("Ground Storage");
	}

	public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
	{
		if (inventory.Empty)
		{
			return;
		}
		string[] contentSummary = getContentSummary();
		ItemStack stack = inventory.FirstNonEmptySlot.Itemstack;
		if (contentSummary.Length == 1 && stack.Collectible.GetCollectibleInterface<IContainedCustomName>() == null && stack.Class == EnumItemClass.Block && ((Block)stack.Collectible).EntityClass == null)
		{
			string detailedInfo = stack.Block.GetPlacedBlockInfo(Api.World, Pos, forPlayer);
			if (detailedInfo != null && detailedInfo.Length > 0)
			{
				dsc.Append(detailedInfo);
			}
		}
		else
		{
			string[] array = contentSummary;
			foreach (string line in array)
			{
				dsc.AppendLine(line);
			}
		}
		if (inventory.Empty)
		{
			return;
		}
		foreach (ItemSlot slot in inventory)
		{
			float temperature = slot.Itemstack?.Collectible.GetTemperature(Api.World, slot.Itemstack) ?? 0f;
			if (temperature > 20f)
			{
				float f = slot.Itemstack?.Attributes.GetFloat("hoursHeatReceived") ?? 0f;
				dsc.AppendLine(Lang.Get("temperature-precise", temperature));
				if (f > 0f)
				{
					dsc.AppendLine(Lang.Get("Fired for {0:0.##} hours", f));
				}
			}
		}
	}

	public virtual string[] getContentSummary()
	{
		Vintagestory.API.Datastructures.OrderedDictionary<string, int> dict = new Vintagestory.API.Datastructures.OrderedDictionary<string, int>();
		foreach (ItemSlot slot in inventory)
		{
			if (!slot.Empty)
			{
				string stackName = slot.Itemstack.GetName();
				stackName = slot.Itemstack.Collectible.GetCollectibleInterface<IContainedCustomName>()?.GetContainedInfo(slot) ?? stackName;
				if (!dict.TryGetValue(stackName, out var cnt))
				{
					cnt = 0;
				}
				dict[stackName] = cnt + slot.StackSize;
			}
		}
		return dict.Select((KeyValuePair<string, int> elem) => Lang.Get("{0}x {1}", elem.Value, elem.Key)).ToArray();
	}

	public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tesselator)
	{
		float temp = 0f;
		if (!Inventory.Empty)
		{
			foreach (ItemSlot slot in Inventory)
			{
				temp = Math.Max(temp, slot.Itemstack?.Collectible.GetTemperature(capi.World, slot.Itemstack) ?? 0f);
			}
		}
		UseRenderer = temp >= 500f;
		if (renderer == null && temp >= 450f)
		{
			capi.Event.EnqueueMainThreadTask(delegate
			{
				if (renderer == null)
				{
					renderer = new GroundStorageRenderer(capi, this);
				}
			}, "groundStorageRendererE");
		}
		if (UseRenderer)
		{
			updateMeshes();
			return true;
		}
		if (renderer != null && temp < 450f)
		{
			capi.Event.EnqueueMainThreadTask(delegate
			{
				if (renderer != null)
				{
					renderer.Dispose();
					renderer = null;
				}
			}, "groundStorageRendererD");
		}
		NeedsRetesselation = false;
		lock (inventoryLock)
		{
			return base.OnTesselation(mesher, tesselator);
		}
	}

	protected Vec3f rotatedOffset(Vec3f offset, float radY)
	{
		Matrixf matrixf = new Matrixf();
		matrixf.Translate(0.5f, 0.5f, 0.5f).RotateY(radY).Translate(-0.5f, -0.5f, -0.5f);
		return matrixf.TransformVector(new Vec4f(offset.X, offset.Y, offset.Z, 1f)).XYZ;
	}

	protected override float[][] genTransformationMatrices()
	{
		float[][] tfMatrices = new float[DisplayedItems][];
		Vec3f[] offs = new Vec3f[DisplayedItems];
		lock (inventoryLock)
		{
			GetLayoutOffset(offs);
		}
		for (int i = 0; i < tfMatrices.Length; i++)
		{
			Vec3f off = offs[i];
			off = new Matrixf().RotateY(MeshAngle).TransformVector(off.ToVec4f(0f)).XYZ;
			tfMatrices[i] = new Matrixf().Translate(off.X + 0.5f, off.Y, off.Z + 0.5f).RotateY(MeshAngle).Translate(-0.5f, 0f, -0.5f)
				.Values;
		}
		return tfMatrices;
	}

	public void GetLayoutOffset(Vec3f[] offs)
	{
		if (StorageProps != null)
		{
			switch (StorageProps.Layout)
			{
			case EnumGroundStorageLayout.SingleCenter:
			case EnumGroundStorageLayout.Stacking:
			case EnumGroundStorageLayout.Messy12:
				offs[0] = new Vec3f();
				break;
			case EnumGroundStorageLayout.Halves:
			case EnumGroundStorageLayout.WallHalves:
				offs[0] = new Vec3f(-0.25f, 0f, 0f);
				offs[1] = new Vec3f(0.25f, 0f, 0f);
				break;
			case EnumGroundStorageLayout.Quadrants:
				offs[0] = new Vec3f(-0.25f, 0f, -0.25f);
				offs[1] = new Vec3f(-0.25f, 0f, 0.25f);
				offs[2] = new Vec3f(0.25f, 0f, -0.25f);
				offs[3] = new Vec3f(0.25f, 0f, 0.25f);
				break;
			}
		}
	}

	protected override string getMeshCacheKey(ItemSlot slot)
	{
		GroundStorageProperties storageProps = StorageProps;
		return ((storageProps != null && storageProps.Layout == EnumGroundStorageLayout.Messy12) ? "messy12-" : "") + ItemModelCount + "x" + base.getMeshCacheKey(slot);
	}

	protected override MeshData getOrCreateMesh(ItemSlot slot, int index)
	{
		ItemStack stack = slot.Itemstack;
		if (stack.Class == EnumItemClass.Block)
		{
			if (stack.Block is IBlockMealContainer be && !(be is BlockCrock))
			{
				GetOrCreateMealMesh(be, stack, index);
			}
			else
			{
				MeshRefs[index] = capi.TesselatorManager.GetDefaultBlockMeshRef(stack.Block);
			}
		}
		else if (stack.Class == EnumItemClass.Item && StorageProps != null && StorageProps.Layout != EnumGroundStorageLayout.Stacking)
		{
			MeshRefs[index] = capi.TesselatorManager.GetDefaultItemMeshRef(stack.Item);
		}
		GroundStorageProperties storageProps = StorageProps;
		if (storageProps != null && storageProps.Layout == EnumGroundStorageLayout.Messy12)
		{
			string key = getMeshCacheKey(slot);
			MeshData mesh = getMesh(slot);
			if (mesh != null)
			{
				return mesh;
			}
			MeshData meshDataOne = getDefaultMesh(stack);
			applyDefaultTranforms(stack, meshDataOne);
			MeshData meshData12 = meshDataOne.Clone().Clear();
			BlockGroundStorage bgs = base.Block as BlockGroundStorage;
			for (int i = 0; i < stack.StackSize; i++)
			{
				ModelTransform tf = bgs.Messy12Transforms[Math.Min(11, i)];
				meshDataOne.Rotate(0f, (float)Math.PI * 2f * tf.Rotation.Y, 0f);
				meshDataOne.Scale(tf.Origin.ToVec3f(), tf.ScaleXYZ.X, tf.ScaleXYZ.Y, tf.ScaleXYZ.Z);
				meshData12.AddMeshData(meshDataOne, tf.Translation.X, tf.Translation.Y, tf.Translation.Z);
			}
			base.MeshCache[key] = meshData12;
			return meshData12;
		}
		GroundStorageProperties storageProps2 = StorageProps;
		if (storageProps2 != null && storageProps2.Layout == EnumGroundStorageLayout.Stacking)
		{
			string key2 = getMeshCacheKey(slot);
			MeshData mesh2 = getMesh(slot);
			if (mesh2 != null && UploadedMeshCache.TryGetValue(key2, out MeshRefs[index]))
			{
				return mesh2;
			}
			if (mesh2 == null)
			{
				AssetLocation loc = StorageProps.StackingModel.Clone().WithPathPrefixOnce("shapes/").WithPathAppendixOnce(".json");
				nowTesselatingShape = Shape.TryGet(capi, loc);
				nowTesselatingObj = stack.Collectible;
				if (nowTesselatingShape == null)
				{
					capi.Logger.Error(string.Concat("Stacking model shape for collectible ", stack.Collectible.Code, " not found. Block will be invisible!"));
					return null;
				}
				capi.Tesselator.TesselateShape("storagePile", nowTesselatingShape, out mesh2, this, null, 0, 0, 0, StorageProps.CuboidsPerModel * ItemModelCount);
			}
			base.MeshCache[key2] = mesh2;
			if (UploadedMeshCache.TryGetValue(key2, out var mr))
			{
				mr.Dispose();
			}
			if (Environment.CurrentManagedThreadId != RuntimeEnv.MainThreadId)
			{
				Api.Event.EnqueueMainThreadTask(delegate
				{
					MultiTextureMeshRef multiTextureMeshRef = capi.Render.UploadMultiTextureMesh(mesh2);
					UploadedMeshCache[key2] = multiTextureMeshRef;
					MeshRefs[index] = multiTextureMeshRef;
					Api.World.BlockAccessor.MarkBlockDirty(Pos);
				}, "groundstoragemeshupload");
				return null;
			}
			MultiTextureMeshRef newMeshRef = capi.Render.UploadMultiTextureMesh(mesh2);
			UploadedMeshCache[key2] = newMeshRef;
			MeshRefs[index] = newMeshRef;
			return mesh2;
		}
		MeshData orCreateMesh = base.getOrCreateMesh(slot, index);
		JsonObject attributes = stack.Collectible.Attributes;
		if (attributes != null && attributes[AttributeTransformCode].Exists)
		{
			ModelTransform transform = stack.Collectible.Attributes?[AttributeTransformCode].AsObject<ModelTransform>();
			ModelTransformsRenderer[index] = transform;
			return orCreateMesh;
		}
		ModelTransformsRenderer[index] = null;
		return orCreateMesh;
	}

	public virtual void GetOrCreateMealMesh(IBlockMealContainer be, ItemStack stack, int index)
	{
		MealMeshCache mealMeshCache = capi.ModLoader.GetModSystem<MealMeshCache>();
		Vec3f vec = ((be is BlockCookedContainer bc) ? new Vec3f(0f, bc.yoff / 16f, 0f) : null);
		MeshRefs[index] = mealMeshCache.GetOrCreateMealInContainerMeshRef(be, stack, vec);
	}

	public void TryIgnite()
	{
		if (!burning && CanIgnite)
		{
			burning = true;
			burnStartTotalHours = Api.World.Calendar.TotalHours;
			MarkDirty();
			UpdateBurningState();
		}
	}

	public void Extinguish()
	{
		if (burning)
		{
			burning = false;
			UnregisterTickListener();
			MarkDirty(redrawOnClient: true);
			Api.World.PlaySoundAt(new AssetLocation("sounds/effect/extinguish"), Pos, 0.0, null, randomizePitch: false, 16f);
		}
	}

	public float GetHoursLeft(double currentTotalHours)
	{
		double totalHoursPassed = currentTotalHours - burnStartTotalHours;
		double burnHourTimeLeft = (float)inventory[0].StackSize * burnHoursPerItem;
		return (float)Math.Max(0.0, Math.Min(totalHoursPassed, burnHourTimeLeft));
	}

	private void UpdateBurningState()
	{
		if (!burning)
		{
			return;
		}
		if (Api.World.Side == EnumAppSide.Client)
		{
			if (ambientSound == null || !ambientSound.IsPlaying)
			{
				ambientSound = ((IClientWorldAccessor)Api.World).LoadSound(new SoundParams
				{
					Location = new AssetLocation("sounds/held/torch-idle.ogg"),
					ShouldLoop = true,
					Position = Pos.ToVec3f().Add(0.5f, 0.25f, 0.5f),
					DisposeOnFinish = false,
					Volume = 1f
				});
				if (ambientSound != null)
				{
					ambientSound.PlaybackPosition = ambientSound.SoundLengthSeconds * (float)Api.World.Rand.NextDouble();
					ambientSound.Start();
				}
			}
			listenerId = RegisterGameTickListener(OnBurningTickClient, 100);
		}
		else if (!IsExternallyTicked)
		{
			RegisterServerTickListener();
		}
	}

	public void UnregisterTickListener()
	{
		if (listenerId != 0L)
		{
			UnregisterGameTickListener(listenerId);
			listenerId = 0L;
		}
	}

	public void RegisterServerTickListener()
	{
		if (listenerId == 0L && IsBurning)
		{
			listenerId = RegisterGameTickListener(OnBurningTickServer, 10000);
		}
	}

	public void OnExternalTick(float dt)
	{
		OnBurningTickServer(dt);
	}

	private void OnBurningTickClient(float dt)
	{
		if (!burning || !(Api.World.Rand.NextDouble() < 0.93) || StorageProps == null)
		{
			return;
		}
		int yOffset = ItemModelCount * StorageProps.ItemsPerModel / StorageProps.StackingCapacity;
		Vec3d pos = Pos.ToVec3d().Add(0.0, yOffset, 0.0);
		Random rnd = Api.World.Rand;
		for (int i = 0; i < Entity.FireParticleProps.Length; i++)
		{
			AdvancedParticleProperties particles = Entity.FireParticleProps[i];
			particles.Velocity[1].avg = (float)(rnd.NextDouble() - 0.5);
			particles.basePos.Set(pos.X + 0.5, pos.Y, pos.Z + 0.5);
			if (i == 0)
			{
				particles.Quantity.avg = 1f;
			}
			if (i == 1)
			{
				particles.Quantity.avg = 2f;
				particles.PosOffset[0].var = 0.39f;
				particles.PosOffset[1].var = 0.39f;
				particles.PosOffset[2].var = 0.39f;
			}
			Api.World.SpawnParticles(particles);
		}
	}

	private void OnBurningTickServer(float dt)
	{
		facings.Shuffle(Api.World.Rand);
		BEBehaviorBurning bh = GetBehavior<BEBehaviorBurning>();
		BlockFacing[] array = facings;
		foreach (BlockFacing val in array)
		{
			BlockPos blockPos = Pos.AddCopy(val);
			BlockEntity blockEntity = Api.World.BlockAccessor.GetBlockEntity(blockPos);
			if (blockEntity is BlockEntityCoalPile becp)
			{
				becp.TryIgnite();
				if (Api.World.Rand.NextDouble() < 0.75)
				{
					break;
				}
			}
			else if (blockEntity is BlockEntityGroundStorage besg)
			{
				besg.TryIgnite();
				if (Api.World.Rand.NextDouble() < 0.75)
				{
					break;
				}
			}
			else if (Api.World.Config.GetBool("allowFireSpread") && 0.5 > Api.World.Rand.NextDouble() && bh.TrySpreadTo(blockPos) && Api.World.Rand.NextDouble() < 0.75)
			{
				break;
			}
		}
		bool changed = false;
		while (Api.World.Calendar.TotalHours - burnStartTotalHours > (double)burnHoursPerItem)
		{
			burnStartTotalHours += burnHoursPerItem;
			inventory[0].TakeOut(1);
			if (inventory[0].Empty)
			{
				Api.World.BlockAccessor.SetBlock(0, Pos);
				break;
			}
			changed = true;
		}
		if (changed)
		{
			MarkDirty(redrawOnClient: true);
		}
	}

	public byte[] GetLightHsv()
	{
		byte[] lighthsv = null;
		foreach (ItemSlot slot in inventory)
		{
			if (!slot.Empty)
			{
				byte[] stacklighthsv = slot.Itemstack?.Collectible?.GetLightHsv(Api.World.BlockAccessor, null, slot.Itemstack);
				lighthsv = ((lighthsv != null) ? ColorUtil.MergeLightHSV(lighthsv, stacklighthsv) : ((byte[])stacklighthsv?.Clone()));
			}
		}
		return lighthsv ?? new byte[3];
	}

	public void OnTransformed(IWorldAccessor worldAccessor, ITreeAttribute tree, int degreeRotation, Dictionary<int, AssetLocation> oldBlockIdMapping, Dictionary<int, AssetLocation> oldItemIdMapping, EnumAxis? flipAxis)
	{
		MeshAngle = tree.GetFloat("meshAngle");
		MeshAngle -= (float)degreeRotation * ((float)Math.PI / 180f);
		tree.SetFloat("meshAngle", MeshAngle);
		AttachFace = BlockFacing.ALLFACES[tree.GetInt("attachFace")];
		AttachFace = AttachFace.FaceWhenRotatedBy(0f, (float)(-degreeRotation) * ((float)Math.PI / 180f), 0f);
		tree.SetInt("attachFace", AttachFace?.Index ?? 0);
	}

	public override void OnBlockUnloaded()
	{
		base.OnBlockUnloaded();
		Dispose();
	}

	public override void OnBlockRemoved()
	{
		base.OnBlockRemoved();
		Dispose();
	}

	protected override void Dispose()
	{
		base.Dispose();
		renderer?.Dispose();
		ambientSound?.Stop();
	}

	public static void OnGameDisposed(ICoreClientAPI capi)
	{
		if (capi == null)
		{
			return;
		}
		Dictionary<string, MultiTextureMeshRef> UploadedMeshCache = ObjectCacheUtil.TryGet<Dictionary<string, MultiTextureMeshRef>>(capi, "groundStorageUMC");
		if (UploadedMeshCache == null)
		{
			return;
		}
		foreach (MultiTextureMeshRef value in UploadedMeshCache.Values)
		{
			value?.Dispose();
		}
	}

	public virtual float GetHeatStrength(IWorldAccessor world, BlockPos heatSourcePos, BlockPos heatReceiverPos)
	{
		return IsBurning ? 10 : 0;
	}
}
