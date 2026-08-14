using System.Collections.Generic;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public abstract class BlockEntityOpenableContainer : BlockEntityContainer
{
	protected GuiDialogBlockEntity invDialog;

	public SoundAttributes OpenSound = new SoundAttributes(AssetLocation.Create("sounds/block/chestopen"), withRandomPitch: true);

	public SoundAttributes CloseSound = new SoundAttributes(AssetLocation.Create("sounds/block/chestclose"), withRandomPitch: true);

	public HashSet<long> LidOpenEntityId;

	public abstract bool OnPlayerRightClick(IPlayer byPlayer, BlockSelection blockSel);

	public override void Initialize(ICoreAPI api)
	{
		base.Initialize(api);
		LidOpenEntityId = new HashSet<long>();
		Inventory.LateInitialize(InventoryClassName + "-" + Pos, api);
		Inventory.ResolveBlocksOrItems();
		Inventory.OnInventoryOpened += OnInventoryOpened;
		Inventory.OnInventoryClosed += OnInventoryClosed;
		OpenSound = base.Block.Attributes?["openSound"].AsObject<SoundAttributes?>(null, base.Block.Code.Domain, randomPitch: true) ?? OpenSound;
		CloseSound = base.Block.Attributes?["closeSound"].AsObject<SoundAttributes?>(null, base.Block.Code.Domain, randomPitch: true) ?? CloseSound;
	}

	protected void OnInventoryOpened(IPlayer player)
	{
		LidOpenEntityId.Add(player.Entity.EntityId);
	}

	protected void OnInventoryClosed(IPlayer player)
	{
		LidOpenEntityId.Remove(player.Entity.EntityId);
	}

	protected void toggleInventoryDialogClient(IPlayer byPlayer, CreateDialogDelegate onCreateDialog)
	{
		if (invDialog == null)
		{
			ICoreClientAPI capi = Api as ICoreClientAPI;
			invDialog = onCreateDialog();
			invDialog.OnClosed += delegate
			{
				invDialog = null;
				capi.Network.SendBlockEntityPacket(Pos, 1001);
			};
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
		if (packetid == 1001)
		{
			player.InventoryManager?.CloseInventory(Inventory);
			data = SerializerUtil.Serialize(new OpenContainerLidPacket(player.Entity.EntityId, opened: false));
			((ICoreServerAPI)Api).Network.BroadcastBlockEntityPacket(Pos, 5001, data, (IServerPlayer)player);
		}
		if (packetid < 1000)
		{
			if (!new CachedAccessPerms(Api.World, Pos, player).IsInteractingPlayerAllowedTo(EnumBlockAccessFlags.Use, validatePickRange: true, "openable container"))
			{
				Inventory.InvNetworkUtil.SendInventoryRollback((IServerPlayer)player, packetid, data);
				return;
			}
			Inventory.InvNetworkUtil.HandleClientPacket(player, packetid, data);
			Api.World.BlockAccessor.GetChunkAtBlockPos(Pos).MarkModified();
		}
		else if (packetid == 1000 && new CachedAccessPerms(Api.World, Pos, player).IsInteractingPlayerAllowedTo(EnumBlockAccessFlags.Use, validatePickRange: true, "openable container"))
		{
			player.InventoryManager?.OpenInventory(Inventory);
			data = SerializerUtil.Serialize(new OpenContainerLidPacket(player.Entity.EntityId, opened: true));
			((ICoreServerAPI)Api).Network.BroadcastBlockEntityPacket(Pos, 5001, data, (IServerPlayer)player);
		}
	}

	public override void OnReceivedServerPacket(int packetid, byte[] data)
	{
		IClientWorldAccessor clientWorld = (IClientWorldAccessor)Api.World;
		if (packetid == 5000)
		{
			if (invDialog != null)
			{
				GuiDialogBlockEntity guiDialogBlockEntity = invDialog;
				if (guiDialogBlockEntity != null && guiDialogBlockEntity.IsOpened())
				{
					invDialog.TryClose();
				}
				invDialog?.Dispose();
				invDialog = null;
				return;
			}
			BlockEntityContainerOpen blockContainer = BlockEntityContainerOpen.FromBytes(data);
			Inventory.FromTreeAttributes(blockContainer.Tree);
			Inventory.ResolveBlocksOrItems();
			invDialog = new GuiDialogBlockEntityInventory(blockContainer.DialogTitle, Inventory, Pos, blockContainer.Columns, Api as ICoreClientAPI);
			Block block = Api.World.BlockAccessor.GetBlock(Pos);
			invDialog.OpenSound = block.Attributes?["openSound"]?.AsObject<SoundAttributes?>(null, base.Block.Code.Domain, randomPitch: true) ?? OpenSound;
			invDialog.CloseSound = block.Attributes?["closeSound"]?.AsObject<SoundAttributes?>(null, base.Block.Code.Domain, randomPitch: true) ?? CloseSound;
			invDialog.TryOpen();
		}
		if (packetid == 5001)
		{
			OpenContainerLidPacket containerPacket = SerializerUtil.Deserialize<OpenContainerLidPacket>(data);
			if (this is BlockEntityGenericTypedContainer genericContainer)
			{
				if (containerPacket.Opened)
				{
					LidOpenEntityId.Add(containerPacket.EntityId);
					genericContainer.OpenLid();
				}
				else
				{
					LidOpenEntityId.Remove(containerPacket.EntityId);
					if (LidOpenEntityId.Count == 0)
					{
						genericContainer.CloseLid();
					}
				}
			}
		}
		if (packetid == 1001)
		{
			clientWorld.Player.InventoryManager.CloseInventory(Inventory);
			GuiDialogBlockEntity guiDialogBlockEntity2 = invDialog;
			if (guiDialogBlockEntity2 != null && guiDialogBlockEntity2.IsOpened())
			{
				invDialog?.TryClose();
			}
			invDialog?.Dispose();
			invDialog = null;
		}
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
		GuiDialogBlockEntity guiDialogBlockEntity = invDialog;
		if (guiDialogBlockEntity != null && guiDialogBlockEntity.IsOpened())
		{
			invDialog?.TryClose();
		}
		invDialog?.Dispose();
		if (Api is ICoreServerAPI)
		{
			Inventory.openedByPlayerGUIds?.Clear();
		}
	}

	public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
	{
		base.GetBlockInfo(forPlayer, dsc);
	}

	public override void DropContents(Vec3d atPos)
	{
		Inventory.DropAll(atPos);
	}
}
