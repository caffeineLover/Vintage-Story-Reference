using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace Vintagestory.GameContent;

public class BlockEntityBarrel : BlockEntityLiquidContainer, ICoolingMedium
{
	private GuiDialogBarrel invDialog;

	private MeshData currentMesh;

	private BlockBarrel ownBlock;

	public bool Sealed;

	public double SealedSinceTotalHours;

	public BarrelRecipe CurrentRecipe;

	public int CurrentOutSize;

	protected static SoundAttributes barrelOpen = new SoundAttributes(AssetLocation.Create("sounds/block/barrelopen"), withRandomPitch: true);

	protected static SoundAttributes barrelClose = new SoundAttributes(AssetLocation.Create("sounds/block/barrelclose"), withRandomPitch: true);

	private bool ignoreChange;

	public int CapacityLitres { get; set; } = 50;

	public override string InventoryClassName => "barrel";

	[Obsolete("Use player aware 'GetCanSeal' instead")]
	public bool CanSeal
	{
		get
		{
			FindMatchingRecipe();
			if (CurrentRecipe != null && CurrentRecipe.SealHours > 0.0)
			{
				return true;
			}
			return false;
		}
	}

	public bool GetCanSeal(IPlayer byPlayer)
	{
		FindMatchingRecipe(byPlayer);
		if (CurrentRecipe != null && CurrentRecipe.SealHours > 0.0)
		{
			return true;
		}
		return false;
	}

	public BlockEntityBarrel()
	{
		inventory = new InventoryGeneric(2, null, null, (int id, InventoryGeneric self) => (id == 0) ? ((ItemSlot)new ItemSlotBarrelInput(self)) : ((ItemSlot)new ItemSlotLiquidOnly(self, 50f)));
		inventory.BaseWeight = 1f;
		inventory.OnGetSuitability = GetSuitability;
		inventory.SlotModified += Inventory_SlotModified;
		inventory.OnAcquireTransitionSpeed += Inventory_OnAcquireTransitionSpeed1;
	}

	protected float Inventory_OnAcquireTransitionSpeed1(EnumTransitionType transType, ItemStack stack, float mul)
	{
		int num;
		if (Sealed)
		{
			BarrelRecipe currentRecipe = CurrentRecipe;
			if (currentRecipe != null && currentRecipe.SealHours > 0.0)
			{
				num = 0;
				goto IL_002b;
			}
		}
		num = 1;
		goto IL_002b;
		IL_002b:
		return num;
	}

	protected float GetSuitability(ItemSlot sourceSlot, ItemSlot targetSlot, bool isMerge)
	{
		if (targetSlot == inventory[1] && inventory[0].StackSize > 0)
		{
			ItemStack currentStack = inventory[0].Itemstack;
			ItemStack testStack = sourceSlot.Itemstack;
			if (currentStack.Collectible.Equals(currentStack, testStack, GlobalConstants.IgnoredStackAttributes))
			{
				return -1f;
			}
		}
		return (isMerge ? (inventory.BaseWeight + 3f) : (inventory.BaseWeight + 1f)) + (float)((sourceSlot.Inventory is InventoryBasePlayer) ? 1 : 0);
	}

	protected override ItemSlot GetAutoPushIntoSlot(BlockFacing atBlockFace, ItemSlot fromSlot)
	{
		if (atBlockFace == BlockFacing.UP)
		{
			return inventory[0];
		}
		return null;
	}

	public override void Initialize(ICoreAPI api)
	{
		base.Initialize(api);
		ownBlock = base.Block as BlockBarrel;
		JsonObject capacityAttribute = ownBlock?.Attributes?["capacityLitres"];
		if (capacityAttribute != null && capacityAttribute.Exists)
		{
			CapacityLitres = capacityAttribute.AsInt(50);
			(inventory[1] as ItemSlotLiquidOnly).CapacityLitres = CapacityLitres;
		}
		if (api.Side == EnumAppSide.Server)
		{
			RegisterGameTickListener(OnEvery3Second, 3000);
		}
		FindMatchingRecipe();
	}

	protected void Inventory_SlotModified(int slotId)
	{
		if (!ignoreChange && (slotId == 0 || slotId == 1))
		{
			invDialog?.UpdateContents();
			ICoreAPI api = Api;
			if (api != null && api.Side == EnumAppSide.Client)
			{
				currentMesh = null;
			}
			MarkDirty(redrawOnClient: true);
			FindMatchingRecipe();
		}
	}

	protected void FindMatchingRecipe()
	{
		FindMatchingRecipe(null);
	}

	protected void FindMatchingRecipe(IPlayer byPlayer)
	{
		ItemSlot[] inputSlots = new ItemSlot[2]
		{
			inventory[0],
			inventory[1]
		};
		CurrentRecipe = null;
		foreach (BarrelRecipe recipe in Api.GetBarrelRecipes())
		{
			if (!((byPlayer == null) ? recipe.Matches(inputSlots, out var outsize) : recipe.Matches(byPlayer, inputSlots, out outsize)))
			{
				continue;
			}
			ignoreChange = true;
			if (recipe.SealHours > 0.0)
			{
				CurrentRecipe = recipe;
				CurrentOutSize = outsize;
			}
			else
			{
				ICoreAPI api = Api;
				if (api != null && api.Side == EnumAppSide.Server)
				{
					recipe.TryCraftNow(Api, 0.0, inputSlots);
					MarkDirty(redrawOnClient: true);
					Api.World.BlockAccessor.MarkBlockEntityDirty(Pos);
				}
			}
			invDialog?.UpdateContents();
			ICoreAPI api2 = Api;
			if (api2 != null && api2.Side == EnumAppSide.Client)
			{
				currentMesh = null;
				MarkDirty(redrawOnClient: true);
			}
			ignoreChange = false;
			break;
		}
	}

	protected void OnEvery3Second(float dt)
	{
		if (!inventory[0].Empty && CurrentRecipe == null)
		{
			FindMatchingRecipe();
		}
		if (CurrentRecipe != null)
		{
			if (Sealed && CurrentRecipe.TryCraftNow(Api, Api.World.Calendar.TotalHours - SealedSinceTotalHours, new ItemSlot[2]
			{
				inventory[0],
				inventory[1]
			}))
			{
				MarkDirty(redrawOnClient: true);
				Api.World.BlockAccessor.MarkBlockEntityDirty(Pos);
				Sealed = false;
			}
		}
		else if (Sealed)
		{
			Sealed = false;
			MarkDirty(redrawOnClient: true);
		}
	}

	public override void OnBlockPlaced(ItemStack byItemStack = null)
	{
		base.OnBlockPlaced(byItemStack);
		ItemSlot inputSlot = Inventory[0];
		ItemSlot liquidSlot = Inventory[1];
		if (!inputSlot.Empty && liquidSlot.Empty && BlockLiquidContainerBase.GetContainableProps(inputSlot.Itemstack) != null)
		{
			Inventory.TryFlipItems(1, inputSlot);
		}
	}

	public override void OnBlockBroken(IPlayer byPlayer = null)
	{
		if (!Sealed)
		{
			base.OnBlockBroken(byPlayer);
		}
		invDialog?.TryClose();
		invDialog = null;
	}

	public void SealBarrel()
	{
		if (!Sealed)
		{
			Sealed = true;
			SealedSinceTotalHours = Api.World.Calendar.TotalHours;
			MarkDirty(redrawOnClient: true);
		}
	}

	public void OnPlayerRightClick(IPlayer byPlayer)
	{
		if (!Sealed)
		{
			FindMatchingRecipe(byPlayer);
			if (Api.Side == EnumAppSide.Client)
			{
				toggleInventoryDialogClient(byPlayer);
			}
		}
	}

	protected void toggleInventoryDialogClient(IPlayer byPlayer)
	{
		if (invDialog == null)
		{
			ICoreClientAPI capi = Api as ICoreClientAPI;
			invDialog = new GuiDialogBarrel(Lang.Get("Barrel"), Inventory, Pos, Api as ICoreClientAPI);
			invDialog.OnClosed += delegate
			{
				invDialog = null;
				capi.Network.SendBlockEntityPacket(Pos, 1001);
				capi.Network.SendPacketClient(Inventory.Close(byPlayer));
			};
			invDialog.OpenSound = base.Block.Attributes?["openSound"]?.AsObject<SoundAttributes?>(null, base.Block.Code.Domain, randomPitch: true) ?? barrelOpen;
			invDialog.CloseSound = base.Block.Attributes?["closeSound"]?.AsObject<SoundAttributes?>(null, base.Block.Code.Domain, randomPitch: true) ?? barrelClose;
			invDialog.TryOpen();
			capi.Network.SendPacketClient(Inventory.Open(byPlayer));
			capi.Network.SendBlockEntityPacket(Pos, 1000);
		}
		else
		{
			invDialog.TryClose();
		}
	}

	public override void OnReceivedClientPacket(IPlayer player, int packetid, byte[] data)
	{
		base.OnReceivedClientPacket(player, packetid, data);
		if (packetid < 1000)
		{
			if (!new CachedAccessPerms(Api.World, Pos, player).IsInteractingPlayerAllowedTo(EnumBlockAccessFlags.Use, validatePickRange: true, "barrel"))
			{
				Inventory.InvNetworkUtil.SendInventoryRollback((IServerPlayer)player, packetid, data);
				return;
			}
			Inventory.InvNetworkUtil.HandleClientPacket(player, packetid, data);
			Api.World.BlockAccessor.GetChunkAtBlockPos(Pos).MarkModified();
			return;
		}
		if (packetid == 1001)
		{
			player.InventoryManager?.CloseInventory(Inventory);
		}
		if (packetid == 1000)
		{
			if (!new CachedAccessPerms(Api.World, Pos, player).IsInteractingPlayerAllowedTo(EnumBlockAccessFlags.Use, validatePickRange: true, "barrel"))
			{
				return;
			}
			player.InventoryManager?.OpenInventory(Inventory);
		}
		if (packetid == 1337)
		{
			if (!new CachedAccessPerms(Api.World, Pos, player).IsInteractingPlayerAllowedTo(EnumBlockAccessFlags.Use, validatePickRange: true, "barrel"))
			{
				((ICoreServerAPI)Api).Network.SendBlockEntityPacket((IServerPlayer)player, Pos, 1338);
			}
			else
			{
				SealBarrel();
			}
		}
	}

	public override void OnReceivedServerPacket(int packetid, byte[] data)
	{
		base.OnReceivedServerPacket(packetid, data);
		switch (packetid)
		{
		case 1001:
			(Api.World as IClientWorldAccessor).Player.InventoryManager.CloseInventory(Inventory);
			invDialog?.TryClose();
			invDialog?.Dispose();
			invDialog = null;
			break;
		case 1338:
			Sealed = false;
			SealedSinceTotalHours = 0.0;
			currentMesh = null;
			MarkDirty(redrawOnClient: true);
			break;
		}
	}

	public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
	{
		base.FromTreeAttributes(tree, worldForResolving);
		Sealed = tree.GetBool("sealed");
		ICoreAPI api = Api;
		if (api != null && api.Side == EnumAppSide.Client)
		{
			currentMesh = null;
			MarkDirty(redrawOnClient: true);
			invDialog?.UpdateContents();
		}
		SealedSinceTotalHours = tree.GetDouble("sealedSinceTotalHours");
		if (Api != null)
		{
			FindMatchingRecipe();
		}
	}

	public override void ToTreeAttributes(ITreeAttribute tree)
	{
		base.ToTreeAttributes(tree);
		tree.SetBool("sealed", Sealed);
		tree.SetDouble("sealedSinceTotalHours", SealedSinceTotalHours);
	}

	public MeshData GenMesh()
	{
		if (ownBlock == null)
		{
			return null;
		}
		MeshData mesh = ownBlock.GenMesh(inventory[0].Itemstack, inventory[1].Itemstack, Sealed, Pos);
		if (mesh.CustomInts != null)
		{
			int[] CustomInts = mesh.CustomInts.Values;
			int count = mesh.CustomInts.Count;
			for (int i = 0; i < CustomInts.Length && i < count; i++)
			{
				CustomInts[i] |= 805306368;
			}
		}
		return mesh;
	}

	public override void OnBlockUnloaded()
	{
		base.OnBlockUnloaded();
		invDialog?.Dispose();
	}

	public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tesselator)
	{
		if (currentMesh == null)
		{
			currentMesh = GenMesh();
		}
		mesher.AddMeshData(currentMesh);
		return true;
	}

	public void CoolNow(ItemSlot slot, Vec3d pos, float dt, bool playSizzle = true)
	{
		ItemSlot liquidSlot = Inventory[1];
		if (!liquidSlot.Empty)
		{
			liquidSlot.Itemstack.Collectible.GetCollectibleInterface<ICoolingMedium>()?.CoolNow(slot, pos, dt, playSizzle);
		}
	}

	public bool CanCool(ItemSlot slot, Vec3d pos)
	{
		ItemSlot liquidSlot = Inventory[1];
		if (liquidSlot.Empty)
		{
			return false;
		}
		return liquidSlot.Itemstack.Collectible.GetCollectibleInterface<ICoolingMedium>()?.CanCool(slot, pos) ?? false;
	}
}
