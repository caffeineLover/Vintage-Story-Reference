using System;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class BlockEntityHenBox : BlockEntityDisplay, IAnimalNest, IPointOfInterest
{
	protected InventoryGeneric inventory;

	public string inventoryClassName = "nestbox";

	public Entity occupier;

	protected double timeToIncubate;

	protected double occupiedTimeLast;

	protected bool IsOccupiedClientside;

	public override InventoryBase Inventory => inventory;

	public override string InventoryClassName => inventoryClassName;

	public Vec3d Position => Pos.ToVec3d().Add(0.5, 0.5, 0.5);

	public string Type => "nest";

	protected int Capacity => base.Block.Attributes?["quantitySlots"]?.AsInt(1) ?? 1;

	public virtual float DistanceWeighting => 2 / (CountEggs() + 2);

	public BlockEntityHenBox()
	{
		container = new ConstantPerishRateContainer(() => Inventory, "inventory");
	}

	public virtual bool IsSuitableFor(Entity entity, string[] nestTypes)
	{
		string nestType = (base.Block as BlockHenbox)?.NestType;
		if (nestType != null)
		{
			if (nestTypes == null)
			{
				return false;
			}
			return nestTypes.Contains(nestType);
		}
		return false;
	}

	public bool Occupied(Entity entity)
	{
		if (occupier != null)
		{
			return occupier != entity;
		}
		return false;
	}

	public virtual void SetOccupier(Entity entity)
	{
		if (occupier != entity)
		{
			occupier = entity;
			MarkDirty();
		}
	}

	public virtual bool TryAddEgg(ItemStack egg)
	{
		for (int i = 0; i < inventory.Count; i++)
		{
			if (inventory[i].Empty)
			{
				inventory[i].Itemstack = egg;
				inventory.DidModifyItemSlot(inventory[i]);
				double? incubationDays = (egg.Attributes["chick"] as TreeAttribute)?.GetDouble("incubationDays");
				if (incubationDays.HasValue)
				{
					timeToIncubate = Math.Max(timeToIncubate, incubationDays.Value);
				}
				occupiedTimeLast = Api.World.Calendar.TotalDays;
				MarkDirty();
				return true;
			}
		}
		return false;
	}

	public int CountEggs()
	{
		int count = 0;
		for (int i = 0; i < inventory.Count; i++)
		{
			if (!inventory[i].Empty)
			{
				count++;
			}
		}
		return count;
	}

	protected virtual void On1500msTick(float dt)
	{
		if (timeToIncubate == 0.0)
		{
			return;
		}
		double newTime = Api.World.Calendar.TotalDays;
		if (occupier != null && occupier.Alive && newTime > occupiedTimeLast)
		{
			timeToIncubate -= newTime - occupiedTimeLast;
			MarkDirty();
		}
		occupiedTimeLast = newTime;
		if (!(timeToIncubate <= 0.0))
		{
			return;
		}
		timeToIncubate = 0.0;
		Random rand = Api.World.Rand;
		for (int i = 0; i < inventory.Count; i++)
		{
			TreeAttribute chickData = (TreeAttribute)(inventory[i].Itemstack?.Attributes["chick"]);
			if (chickData == null)
			{
				continue;
			}
			string chickCode = chickData.GetString("code");
			if (chickCode == null || chickCode == "")
			{
				continue;
			}
			EntityProperties childType = Api.World.GetEntityType(chickCode);
			if (childType == null)
			{
				continue;
			}
			Entity childEntity = Api.World.ClassRegistry.CreateEntity(childType);
			if (childEntity != null)
			{
				childEntity.Pos.SetFrom(new EntityPos(Position.X + (rand.NextDouble() - 0.5) / 5.0, Position.Y, Position.Z + (rand.NextDouble() - 0.5) / 5.0, (float)rand.NextDouble() * ((float)Math.PI * 2f)));
				childEntity.Pos.Motion.X += (rand.NextDouble() - 0.5) / 200.0;
				childEntity.Pos.Motion.Z += (rand.NextDouble() - 0.5) / 200.0;
				childEntity.Attributes.SetString("origin", "reproduction");
				childEntity.WatchedAttributes.SetInt("generation", chickData.GetInt("generation"));
				if (childEntity is EntityAgent eagent)
				{
					eagent.HerdId = chickData.GetLong("herdID", 0L);
				}
				Api.World.SpawnEntity(childEntity);
				inventory[i].Itemstack = null;
				inventory.DidModifyItemSlot(inventory[i]);
			}
		}
	}

	public override void Initialize(ICoreAPI api)
	{
		inventoryClassName = base.Block.Attributes?["inventoryClassName"]?.AsString() ?? inventoryClassName;
		int capacity = Capacity;
		if (inventory == null)
		{
			CreateInventory(capacity, api);
		}
		else if (capacity != inventory.Count)
		{
			api.Logger.Warning(string.Concat(new string[7]
			{
				"Nest ",
				base.Block.Code,
				" loaded with ",
				inventory.Count.ToString(),
				" capacity when it should be ",
				capacity.ToString(),
				"."
			}));
			InventoryGeneric oldInv = inventory;
			CreateInventory(capacity, api);
			int from = 0;
			for (int to = 0; to < capacity; to++)
			{
				for (; from < oldInv.Count && oldInv[from].Empty; from++)
				{
				}
				if (from < oldInv.Count)
				{
					inventory[to].Itemstack = oldInv[from].Itemstack;
					inventory.DidModifyItemSlot(inventory[to]);
					from++;
				}
			}
		}
		base.Initialize(api);
		if (api.Side != EnumAppSide.Server)
		{
			return;
		}
		int eggsWithBlock = -1;
		if (base.Block.Code.Path.EndsWith("empty"))
		{
			eggsWithBlock = 0;
		}
		else if (base.Block.Code.Path.EndsWith("1egg"))
		{
			eggsWithBlock = 1;
		}
		else if (base.Block.Code.Path.EndsWith("eggs"))
		{
			int eggs = base.Block.LastCodePart()[0];
			eggsWithBlock = ((eggs <= 57 && eggs >= 48) ? (eggs - 48) : 0);
		}
		if (eggsWithBlock >= 0)
		{
			Block emptyNest = api.World.GetBlock(new AssetLocation(base.Block.FirstCodePart()));
			api.World.BlockAccessor.ExchangeBlock(emptyNest.Id, Pos);
			MarkDirty();
		}
		for (int i = 0; i < eggsWithBlock; i++)
		{
			ItemSlot itemSlot = inventory[i];
			if (itemSlot.Itemstack == null)
			{
				ItemStack itemStack = (itemSlot.Itemstack = new ItemStack(api.World.GetItem("egg-chicken-raw")));
			}
			inventory.DidModifyItemSlot(inventory[i]);
		}
		IsOccupiedClientside = false;
		api.ModLoader.GetModSystem<POIRegistry>().AddPOI(this);
		RegisterGameTickListener(On1500msTick, 1500);
	}

	protected void CreateInventory(int capacity, ICoreAPI api)
	{
		inventory = new InventoryGeneric(capacity, InventoryClassName, Pos?.ToString(), api);
		inventory.Pos = Pos;
		inventory.SlotModified += OnSlotModified;
	}

	protected virtual void OnSlotModified(int slot)
	{
		MarkDirty();
	}

	public override void OnBlockRemoved()
	{
		base.OnBlockRemoved();
		if (Api.Side == EnumAppSide.Server)
		{
			Api.ModLoader.GetModSystem<POIRegistry>().RemovePOI(this);
		}
	}

	public override void OnBlockUnloaded()
	{
		base.OnBlockUnloaded();
		ICoreAPI api = Api;
		if (api != null && api.Side == EnumAppSide.Server)
		{
			Api.ModLoader.GetModSystem<POIRegistry>().RemovePOI(this);
		}
	}

	public override void ToTreeAttributes(ITreeAttribute tree)
	{
		base.ToTreeAttributes(tree);
		tree.SetDouble("inc", timeToIncubate);
		tree.SetDouble("occ", occupiedTimeLast);
		tree.SetBool("isOccupied", occupier != null && occupier.Alive);
	}

	public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
	{
		TreeAttribute invTree = (TreeAttribute)tree["inventory"];
		if (inventory == null)
		{
			int capacity = invTree?.GetInt("qslots") ?? Capacity;
			CreateInventory(capacity, worldForResolving.Api);
		}
		base.FromTreeAttributes(tree, worldForResolving);
		timeToIncubate = tree.GetDouble("inc");
		occupiedTimeLast = tree.GetDouble("occ");
		for (int i = 0; i < 10; i++)
		{
			string chickCode = tree.GetString("chick" + i);
			if (chickCode != null)
			{
				int generation = tree.GetInt("gen" + i);
				inventory[i].Itemstack = new ItemStack(worldForResolving.GetItem("egg-chicken-raw"));
				TreeAttribute chickTree = new TreeAttribute();
				chickTree.SetString("code", chickCode);
				chickTree.SetInt("generation", generation);
				inventory[i].Itemstack.Attributes["chick"] = chickTree;
				inventory.DidModifyItemSlot(inventory[i]);
			}
		}
		IsOccupiedClientside = tree.GetBool("isOccupied");
		RedrawAfterReceivingTreeAttributes(worldForResolving);
	}

	public virtual bool CanPlayerPlaceItem(ItemStack itemstack)
	{
		return false;
	}

	public bool OnInteract(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
	{
		if (!world.Claims.TryAccess(byPlayer, blockSel.Position, EnumBlockAccessFlags.Use))
		{
			return false;
		}
		ItemSlot slot = byPlayer.InventoryManager.ActiveHotbarSlot;
		if (CanPlayerPlaceItem(slot.Itemstack))
		{
			for (int i = 0; i < inventory.Count; i++)
			{
				if (inventory[i].Empty)
				{
					SoundAttributes? sound = slot.Itemstack?.Block?.Sounds?.Place;
					AssetLocation itemPlaced = slot.Itemstack?.Collectible?.Code;
					ItemStackMoveOperation op = new ItemStackMoveOperation(Api.World, EnumMouseButton.Left, (EnumModifierKey)0, EnumMergePriority.AutoMerge, 1);
					if (slot.TryPutInto(inventory[i], ref op) > 0)
					{
						Api.Logger.Audit(string.Concat(new string[7]
						{
							byPlayer.PlayerName,
							" put 1x",
							itemPlaced,
							" into ",
							base.Block.Code,
							" at ",
							Pos?.ToString()
						}));
						Api.World.PlaySoundAt(sound ?? GlobalConstants.DefaultBuildSound, byPlayer.Entity, byPlayer);
						return true;
					}
				}
			}
			return false;
		}
		bool anyEggs = false;
		for (int j = 0; j < inventory.Count; j++)
		{
			if (inventory[j].Empty)
			{
				continue;
			}
			string audit = inventory[j].Itemstack.Collectible?.Code;
			int quantity = inventory[j].Itemstack.StackSize;
			bool gave = byPlayer.InventoryManager.TryGiveItemstack(inventory[j].Itemstack);
			int taken = quantity - (inventory[j].Itemstack?.StackSize ?? 0);
			if (gave)
			{
				if (inventory[j].Itemstack != null && quantity == inventory[j].Itemstack.StackSize)
				{
					inventory[j].TakeOutWhole();
					taken = quantity;
				}
				anyEggs = true;
				world.Api.Logger.Audit(string.Concat(new string[9]
				{
					byPlayer.PlayerName,
					" took ",
					taken.ToString(),
					"x ",
					audit,
					" from ",
					base.Block.Code,
					" at ",
					Pos?.ToString()
				}));
				inventory.DidModifyItemSlot(inventory[j]);
			}
			if (inventory[j].Itemstack != null && inventory[j].Itemstack.StackSize == 0)
			{
				if (!gave)
				{
					world.Api.Logger.Audit(string.Concat(new string[9]
					{
						byPlayer.PlayerName,
						" voided ",
						taken.ToString(),
						"x ",
						audit,
						" from ",
						base.Block.Code,
						" at ",
						Pos?.ToString()
					}));
				}
				inventory[j].Itemstack = null;
				inventory.DidModifyItemSlot(inventory[j]);
			}
		}
		if (anyEggs)
		{
			world.PlaySoundAt(new AssetLocation("sounds/player/collect"), blockSel.Position.X, blockSel.Position.Y, blockSel.Position.Z, byPlayer);
		}
		return anyEggs;
	}

	public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
	{
		if (!(base.Block is BlockHenbox))
		{
			dsc.AppendLine("error-blockentity-wrongblockclass");
		}
		int eggCount = 0;
		int fertileCount = 0;
		for (int i = 0; i < inventory.Count; i++)
		{
			if (!inventory[i].Empty)
			{
				eggCount++;
				if (((TreeAttribute)inventory[i].Itemstack.Attributes["chick"])?.GetString("code") != null)
				{
					fertileCount++;
				}
			}
		}
		if (fertileCount > 0)
		{
			if (fertileCount > 1)
			{
				dsc.AppendLine(Lang.Get("{0} fertile eggs", fertileCount));
			}
			else
			{
				dsc.AppendLine(Lang.Get("1 fertile egg"));
			}
			if (timeToIncubate >= 1.5)
			{
				dsc.AppendLine(Lang.Get("Incubation time remaining: {0:0} days", timeToIncubate));
			}
			else if (timeToIncubate >= 0.75)
			{
				dsc.AppendLine(Lang.Get("Incubation time remaining: 1 day"));
			}
			else if (timeToIncubate > 0.0)
			{
				dsc.AppendLine(Lang.Get("Incubation time remaining: {0:0} hours", timeToIncubate * 24.0));
			}
			if (!IsOccupiedClientside && eggCount >= inventory.Count)
			{
				dsc.AppendLine(Lang.Get("A broody hen is needed!"));
			}
		}
		else if (eggCount > 0)
		{
			dsc.AppendLine(Lang.Get("No eggs are fertilized"));
		}
	}

	protected override float[][] genTransformationMatrices()
	{
		ModelTransform[] transforms = base.Block.Attributes?["displayTransforms"]?.AsArray<ModelTransform>();
		if (transforms == null)
		{
			capi.Logger.Warning(string.Concat("No display transforms found for ", base.Block.Code, ", placed items may be invisible or in the wrong location."));
			transforms = new ModelTransform[DisplayedItems];
			for (int i = 0; i < transforms.Length; i++)
			{
				transforms[i] = new ModelTransform();
			}
		}
		if (transforms.Length != DisplayedItems)
		{
			capi.Logger.Warning(string.Concat(new string[6]
			{
				"Display transforms for ",
				base.Block.Code,
				" block entity do not match number of displayed items, later placed items may be invisible or in the wrong location. Items: ",
				DisplayedItems.ToString(),
				", transforms: ",
				transforms.Length.ToString()
			}));
		}
		float[][] tfMatrices = new float[transforms.Length][];
		for (int j = 0; j < transforms.Length; j++)
		{
			FastVec3f off = transforms[j].Translation;
			FastVec3f rot = transforms[j].Rotation;
			tfMatrices[j] = new Matrixf().Translate(off.X, off.Y, off.Z).Translate(0.5f, 0f, 0.5f).RotateX(rot.X * ((float)Math.PI / 180f))
				.RotateY(rot.Y * ((float)Math.PI / 180f))
				.RotateZ(rot.Z * ((float)Math.PI / 180f))
				.Translate(-0.5f, 0f, -0.5f)
				.Values;
		}
		return tfMatrices;
	}
}
