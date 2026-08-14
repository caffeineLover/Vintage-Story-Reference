using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;

namespace Vintagestory.GameContent;

public class BlockEntityContainedBagWorkspace
{
	private GuiDialogBlockEntityInventory? invDialog;

	private BlockEntityContainer be;

	private BagInventory bagInv;

	private InventoryGeneric wrapperInv;

	private string dialogTitleLangCode = string.Empty;

	private SoundAttributes? OpenSound;

	private SoundAttributes? CloseSound;

	private int slotId = -1;

	public BlockEntityContainedBagWorkspace(BlockEntityContainer be, int bagSlot)
	{
		this.be = be;
		slotId = bagSlot;
		bagInv = new BagInventory(be.Api, new ItemSlot[1] { be.Inventory[bagSlot] });
		wrapperInv = new InventoryGeneric(be.Api);
	}

	public bool TryLoadBagInv(ItemSlot bagSlot, CollectibleBehaviorGroundStoredHeldBag? bagBh)
	{
		if (bagBh == null || bagSlot.Itemstack?.Collectible.GetCollectibleInterface<IHeldBag>() == null)
		{
			return false;
		}
		dialogTitleLangCode = bagBh.DialogTitleLangCode;
		OpenSound = bagBh.OpenSound;
		CloseSound = bagBh.CloseSound;
		bagInv.ReloadBagInventory(wrapperInv, new ItemSlot[1] { bagSlot });
		wrapperInv.Init(bagInv.Count, "blockcontainedbaginv", "blockcontainedbag-slot" + slotId + "-" + be.Pos, onNewBagSlot);
		if (be.Api.World.Side == EnumAppSide.Server)
		{
			wrapperInv.SlotModified += BagInv_SlotModified;
		}
		return true;
	}

	private ItemSlot onNewBagSlot(int slotId, InventoryGeneric self)
	{
		return bagInv[slotId];
	}

	private void BagInv_SlotModified(int slotid)
	{
		ItemSlot slot = wrapperInv[slotid];
		bagInv.SaveSlotIntoBag((ItemSlotBagContent)slot);
	}

	protected void OnBagInvDialogClosed(IPlayer player)
	{
		GuiDialogBlockEntityInventory inv = invDialog;
		invDialog = null;
		if (inv != null && inv.IsOpened())
		{
			inv?.TryClose();
		}
		inv?.Dispose();
	}

	public bool OpenHeldBag(IPlayer byPlayer)
	{
		if (be.Api.World is IServerWorldAccessor)
		{
			byte[] data = BlockEntityContainerOpen.ToBytes("BlockEntityInventory", dialogTitleLangCode, 4, wrapperInv);
			((ICoreServerAPI)be.Api).Network.SendBlockEntityPacket((IServerPlayer)byPlayer, be.Pos, 5000 + (slotId << 16), data);
		}
		return true;
	}

	public void OnReceivedClientPacket(IPlayer player, int packetid, byte[] data)
	{
		CollectibleObject obj = be.Inventory[slotId].Itemstack?.Collectible;
		if (obj == null || obj.GetBehavior<CollectibleBehaviorGroundStoredHeldBag>() == null || obj.GetCollectibleInterface<IHeldBag>() == null)
		{
			return;
		}
		if (packetid < 1000)
		{
			if (!new BlockEntity.CachedAccessPerms(be.Api.World, be.Pos, player).IsInteractingPlayerAllowedTo(EnumBlockAccessFlags.Use, validatePickRange: true, "ground stored held bag"))
			{
				wrapperInv.InvNetworkUtil.SendInventoryRollback((IServerPlayer)player, packetid, data);
				return;
			}
			wrapperInv.InvNetworkUtil.HandleClientPacket(player, packetid, data);
			be.Api.World.BlockAccessor.GetChunkAtBlockPos(be.Pos).MarkModified();
			return;
		}
		if (packetid == 1001)
		{
			player.InventoryManager?.CloseInventory(wrapperInv);
		}
		if (packetid == 1000 && new BlockEntity.CachedAccessPerms(be.Api.World, be.Pos, player).IsInteractingPlayerAllowedTo(EnumBlockAccessFlags.Use, validatePickRange: true, "ground stored held bag"))
		{
			player.InventoryManager?.OpenInventory(wrapperInv);
		}
	}

	public void OnReceivedServerPacket(int packetid, byte[] data)
	{
		CollectibleObject obj = be.Inventory[slotId].Itemstack?.Collectible;
		if (obj == null || obj.GetBehavior<CollectibleBehaviorGroundStoredHeldBag>() == null || obj.GetCollectibleInterface<IHeldBag>() == null)
		{
			return;
		}
		IClientWorldAccessor clientWorld = (IClientWorldAccessor)be.Api.World;
		if (packetid == 5000)
		{
			if (invDialog != null)
			{
				GuiDialogBlockEntityInventory? guiDialogBlockEntityInventory = invDialog;
				if (guiDialogBlockEntityInventory != null && guiDialogBlockEntityInventory.IsOpened())
				{
					invDialog.TryClose();
				}
				invDialog?.Dispose();
				invDialog = null;
				return;
			}
			BlockEntityContainerOpen blockContainer = BlockEntityContainerOpen.FromBytes(data);
			wrapperInv.FromTreeAttributes(blockContainer.Tree);
			wrapperInv.ResolveBlocksOrItems();
			invDialog = new GuiDialogBlockEntityInventory(Lang.Get(blockContainer.DialogTitle), wrapperInv, be.Pos, slotId << 16, blockContainer.Columns, be.Api as ICoreClientAPI)
			{
				OpenSound = (OpenSound ?? new SoundAttributes()),
				CloseSound = (CloseSound ?? new SoundAttributes())
			};
			if (invDialog.TryOpen())
			{
				clientWorld.Player.InventoryManager.OpenInventory(wrapperInv);
				(be.Api as ICoreClientAPI)?.Network?.SendBlockEntityPacket(be.Pos, 1000 + invDialog.packetIdOffset);
			}
			invDialog.OnClosed += delegate
			{
				OnBagInvDialogClosed(clientWorld.Player);
			};
		}
		if (packetid == 1001)
		{
			clientWorld.Player.InventoryManager.CloseInventoryAndSync(wrapperInv);
			GuiDialogBlockEntityInventory? guiDialogBlockEntityInventory2 = invDialog;
			if (guiDialogBlockEntityInventory2 != null && guiDialogBlockEntityInventory2.IsOpened())
			{
				invDialog?.TryClose();
			}
			invDialog?.Dispose();
			invDialog = null;
		}
	}

	public virtual void Dispose()
	{
		GuiDialogBlockEntityInventory? guiDialogBlockEntityInventory = invDialog;
		if (guiDialogBlockEntityInventory != null && guiDialogBlockEntityInventory.IsOpened())
		{
			invDialog?.TryClose();
		}
		invDialog?.Dispose();
		dialogTitleLangCode = string.Empty;
		OpenSound = null;
		CloseSound = null;
		if (be.Api.Side == EnumAppSide.Server)
		{
			wrapperInv?.openedByPlayerGUIds?.Clear();
		}
	}
}
