using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;

namespace Vintagestory.Common;

public class InventoryNetworkUtil : IInventoryNetworkUtil
{
	protected InventoryBase inv;

	private bool pauseInvUpdates;

	private Queue<Packet_InventoryUpdate> pkts = new Queue<Packet_InventoryUpdate>();

	public ICoreAPI Api { get; set; }

	public bool PauseInventoryUpdates
	{
		get
		{
			return pauseInvUpdates;
		}
		set
		{
			bool num = !value && pauseInvUpdates;
			pauseInvUpdates = value;
			if (num)
			{
				while (pkts.Count > 0)
				{
					Packet_InventoryUpdate pkt = pkts.Dequeue();
					UpdateFromPacket(Api.World, pkt);
				}
			}
		}
	}

	public InventoryNetworkUtil(InventoryBase inv, ICoreAPI api)
	{
		this.inv = inv;
		Api = api;
	}

	public virtual void HandleClientPacket(IPlayer byPlayer, int packetId, byte[] data)
	{
		Packet_Client packet = new Packet_Client();
		Packet_ClientSerializer.DeserializeBuffer(data, data.Length, packet);
		HandleClientPacket(byPlayer, packetId, packet);
	}

	public virtual void HandleClientPacket(IPlayer byPlayer, int packetId, Packet_Client packet)
	{
		IWorldPlayerData plrData = byPlayer.WorldData;
		switch (packetId)
		{
		case 7:
			handleActivateInventorySlotPacket(byPlayer, packet, plrData);
			break;
		case 8:
			handleMoveItemStackPacket(byPlayer, packet);
			break;
		case 9:
			handleFlipItemstacksPacket(byPlayer, packet);
			break;
		}
	}

	private void handleFlipItemstacksPacket(IPlayer byPlayer, Packet_Client packet)
	{
		Packet_FlipItemstacks p = packet.Flipitemstacks;
		string[] invIds = new string[2] { p.SourceInventoryId, p.TargetInventoryId };
		int[] slotIds = new int[2] { p.SourceSlot, p.TargetSlot };
		long[] lastChanged = new long[2] { p.SourceLastChanged, p.TargetLastChanged };
		if (!SendDirtyInventoryContents(byPlayer, invIds[0], lastChanged[0]) && !SendDirtyInventoryContents(byPlayer, invIds[1], lastChanged[1]))
		{
			InventoryBase sourceInv = (InventoryBase)byPlayer.InventoryManager.GetInventory(invIds[0]);
			InventoryBase targetInv = (InventoryBase)byPlayer.InventoryManager.GetInventory(invIds[1]);
			if (sourceInv is ITabbedInventory)
			{
				((ITabbedInventory)sourceInv).SetTab(packet.Flipitemstacks.SourceTabIndex);
			}
			if (targetInv is ITabbedInventory)
			{
				((ITabbedInventory)targetInv).SetTab(packet.Flipitemstacks.TargetTabIndex);
			}
			if (inv.TryFlipItemStack(byPlayer, invIds, slotIds, lastChanged))
			{
				NotifyPlayersItemstackMoved(byPlayer, invIds, slotIds);
			}
			else
			{
				RevertPlayerItemstackMove(byPlayer, invIds, slotIds);
			}
		}
	}

	private void handleMoveItemStackPacket(IPlayer byPlayer, Packet_Client packet)
	{
		string[] invIds = new string[2]
		{
			packet.MoveItemstack.SourceInventoryId,
			packet.MoveItemstack.TargetInventoryId
		};
		int[] slotIds = new int[2]
		{
			packet.MoveItemstack.SourceSlot,
			packet.MoveItemstack.TargetSlot
		};
		if (SendDirtyInventoryContents(byPlayer, invIds[0], packet.MoveItemstack.SourceLastChanged) || SendDirtyInventoryContents(byPlayer, invIds[1], packet.MoveItemstack.TargetLastChanged))
		{
			InventoryBase targetInv = (InventoryBase)byPlayer.InventoryManager.GetInventory(invIds[1]);
			Api.World.Logger.Audit("Revert itemstack move command by {0} to move {1}x{4} from {2} to {3}", byPlayer.PlayerName, packet.MoveItemstack.Quantity, invIds[0], invIds[1], targetInv[slotIds[1]].GetStackName());
			return;
		}
		if (inv is ITabbedInventory)
		{
			((ITabbedInventory)inv).SetTab(packet.MoveItemstack.TabIndex);
		}
		ItemStackMoveOperation op = new ItemStackMoveOperation(Api.World, (EnumMouseButton)packet.MoveItemstack.MouseButton, (EnumModifierKey)packet.MoveItemstack.Modifiers, (EnumMergePriority)packet.MoveItemstack.Priority, packet.MoveItemstack.Quantity);
		op.ActingPlayer = byPlayer;
		ItemSlot[] slots = inv.GetSlotsIfExists(byPlayer, invIds, slotIds);
		if (inv is InventoryBasePlayer && slots[1] is ItemSlotGround && !slots[0].Empty)
		{
			EnumHandling handling = EnumHandling.PassThrough;
			slots[0].Itemstack.Collectible.OnHeldDropped(Api.World, byPlayer, slots[0], packet.MoveItemstack.Quantity, ref handling);
			if (handling != EnumHandling.PassThrough)
			{
				return;
			}
		}
		AssetLocation collectibleCode = slots[0]?.Itemstack?.Collectible.Code;
		if (inv.TryMoveItemStack(byPlayer, invIds, slotIds, ref op))
		{
			Api.World.Logger.Audit("{0} moved {1}x{4} from {2} to {3}", byPlayer.PlayerName, packet.MoveItemstack.Quantity, invIds[0], invIds[1], collectibleCode);
		}
		else
		{
			SendInventoryContents(byPlayer, invIds[0]);
			SendInventoryContents(byPlayer, invIds[1]);
		}
	}

	private void handleActivateInventorySlotPacket(IPlayer byPlayer, Packet_Client packet, IWorldPlayerData plrData)
	{
		Packet_ActivateInventorySlot p = packet.ActivateInventorySlot;
		EnumMouseButton button = (EnumMouseButton)p.MouseButton;
		long lastChanged = p.TargetLastChanged;
		if (inv.lastChangedSinceServerStart < lastChanged)
		{
			SendInventoryContents(byPlayer, inv.InventoryID);
			return;
		}
		int targetSlotId = p.TargetSlot;
		IInventory targetInv = inv;
		if (inv is ITabbedInventory)
		{
			((ITabbedInventory)inv).SetTab(packet.ActivateInventorySlot.TabIndex);
		}
		ItemSlot targetSlot = targetInv[targetSlotId];
		if (targetSlot == null)
		{
			Api.World.Logger.Warning("{0} left-clicked slot {1} in {2}, but target slot did not exist!", byPlayer?.PlayerName, targetSlotId, targetInv.InventoryID);
			SendInventoryContents(byPlayer, inv.InventoryID);
			return;
		}
		string sourceInvId = "mouse-" + plrData.PlayerUID;
		ItemSlot sourceSlot = byPlayer.InventoryManager.GetInventory(sourceInvId)[0];
		if (sourceSlot == null)
		{
			Api.World.Logger.Warning("{0} left-clicked slot {1}, but source slot did not exist!", byPlayer?.PlayerName, sourceInvId);
			SendInventoryContents(byPlayer, inv.InventoryID);
			return;
		}
		ItemStackMoveOperation op = new ItemStackMoveOperation(Api.World, button, (EnumModifierKey)p.Modifiers, (EnumMergePriority)p.Priority);
		op.WheelDir = p.Dir;
		op.ActingPlayer = byPlayer;
		if (button == EnumMouseButton.Wheel)
		{
			op.RequestedQuantity = 1;
		}
		string mouseSlotContents = (sourceSlot.Empty ? "empty" : $"{sourceSlot.StackSize}x{sourceSlot.GetStackName()}");
		string targetSlotContents = (targetSlot.Empty ? "empty" : $"{targetSlot.StackSize}x{targetSlot.GetStackName()}");
		targetInv.ActivateSlot(targetSlotId, sourceSlot, ref op);
		string mouseSlotContentsAfter = (sourceSlot.Empty ? "empty" : $"{sourceSlot.StackSize}x{sourceSlot.GetStackName()}");
		if (mouseSlotContents != mouseSlotContentsAfter)
		{
			string targetSlotContentsAfter = (targetSlot.Empty ? "empty" : $"{targetSlot.StackSize}x{targetSlot.GetStackName()}");
			Api.World.Logger.Audit("{0} left clicked slot {1} in {2}. Before: (mouse: {3}, inv: {4}), after: (mouse: {5}, inv: {6})", op.ActingPlayer?.PlayerName, targetSlotId, targetInv.InventoryID, mouseSlotContents, targetSlotContents, mouseSlotContentsAfter, targetSlotContentsAfter);
		}
	}

	protected virtual bool SendDirtyInventoryContents(IPlayer owningPlayer, string inventoryId, long lastChangedClient)
	{
		InventoryBase targetInv = (InventoryBase)owningPlayer.InventoryManager.GetInventory(inventoryId);
		if (targetInv == null)
		{
			return false;
		}
		if (targetInv.lastChangedSinceServerStart > lastChangedClient)
		{
			SendInventoryContents(owningPlayer, inventoryId);
			return true;
		}
		return false;
	}

	protected virtual void RevertPlayerItemstackMove(IPlayer owningPlayer, string[] invIds, int[] slotIds)
	{
		ItemSlot[] slots = inv.GetSlotsIfExists(owningPlayer, invIds, slotIds);
		if (slots[0] != null && slots[1] != null)
		{
			Packet_Server serverPacket = getDoubleUpdatePacket(owningPlayer, invIds, slotIds);
			((ICoreServerAPI)Api).Network.SendArbitraryPacket(serverPacket, (IServerPlayer)owningPlayer);
		}
	}

	protected virtual void SendInventoryContents(IPlayer owningPlayer, string inventoryId)
	{
		InventoryBase targetInv = (InventoryBase)owningPlayer.InventoryManager.GetInventory(inventoryId);
		if (targetInv != null)
		{
			Packet_InventoryContents packet = (targetInv.InvNetworkUtil as InventoryNetworkUtil).ToPacket(owningPlayer);
			Packet_Server serverPacket = new Packet_Server
			{
				Id = 30,
				InventoryContents = packet
			};
			((ICoreServerAPI)Api).Network.SendArbitraryPacket(serverPacket, (IServerPlayer)owningPlayer);
		}
	}

	public virtual void SendInventoryRollback(IServerPlayer byPlayer, int packetId, ReadOnlySpan<byte> packetData)
	{
		Packet_Client packet = new Packet_Client();
		byte[] pooledMemory = ArrayPool<byte>.Shared.Rent(packetData.Length);
		try
		{
			packetData.CopyTo(pooledMemory);
			Packet_ClientSerializer.DeserializeBuffer(pooledMemory, packetData.Length, packet);
		}
		finally
		{
			ArrayPool<byte>.Shared.Return(pooledMemory);
		}
		switch (packetId)
		{
		case 7:
			SendInventoryContents(byPlayer, inv.InventoryID);
			break;
		case 8:
			SendInventoryContents(byPlayer, packet.MoveItemstack.SourceInventoryId);
			SendInventoryContents(byPlayer, packet.MoveItemstack.TargetInventoryId);
			break;
		case 9:
		{
			Packet_FlipItemstacks p = packet.Flipitemstacks;
			string[] invIds = new string[2] { p.SourceInventoryId, p.TargetInventoryId };
			int[] slotIds = new int[2] { p.SourceSlot, p.TargetSlot };
			RevertPlayerItemstackMove(byPlayer, invIds, slotIds);
			break;
		}
		}
	}

	protected virtual void NotifyPlayersItemstackMoved(IPlayer player, string[] invIds, int[] slotIds)
	{
		Packet_Server serverPacket = getDoubleUpdatePacket(player, invIds, slotIds);
		((ICoreServerAPI)Api).Network.BroadcastArbitraryPacket(serverPacket, (IServerPlayer)player);
	}

	public static Packet_Server getDoubleUpdatePacket(IPlayer player, string[] invIds, int[] slotIds)
	{
		IInventory inventory = player.InventoryManager.GetInventory(invIds[0]);
		IInventory inv2 = player.InventoryManager.GetInventory(invIds[1]);
		ItemStack itemstack1 = inventory[slotIds[0]].Itemstack;
		ItemStack itemstack2 = inv2[slotIds[1]].Itemstack;
		Packet_InventoryDoubleUpdate packet = new Packet_InventoryDoubleUpdate
		{
			ClientId = player.ClientId,
			InventoryId1 = invIds[0],
			InventoryId2 = invIds[1],
			SlotId1 = slotIds[0],
			SlotId2 = slotIds[1],
			ItemStack1 = ((itemstack1 != null) ? StackConverter.ToPacket(itemstack1) : null),
			ItemStack2 = ((itemstack2 != null) ? StackConverter.ToPacket(itemstack2) : null)
		};
		return new Packet_Server
		{
			Id = 32,
			InventoryDoubleUpdate = packet
		};
	}

	internal virtual Packet_ItemStack[] CreatePacketItemStacks()
	{
		Packet_ItemStack[] itemstacks = new Packet_ItemStack[inv.CountForNetworkPacket];
		for (int i = 0; i < inv.CountForNetworkPacket; i++)
		{
			IItemStack stack = inv[i].Itemstack;
			if (stack != null)
			{
				MemoryStream ms = new MemoryStream();
				BinaryWriter writer = new BinaryWriter(ms);
				stack.Attributes.ToBytes(writer);
				itemstacks[i] = new Packet_ItemStack
				{
					ItemClass = (int)stack.Class,
					ItemId = stack.Id,
					StackSize = stack.StackSize,
					Attributes = ms.ToArray()
				};
			}
			else
			{
				itemstacks[i] = new Packet_ItemStack
				{
					ItemClass = -1,
					ItemId = 0,
					StackSize = 0
				};
			}
		}
		return itemstacks;
	}

	public virtual Packet_InventoryContents ToPacket(IPlayer player)
	{
		Packet_InventoryContents packet_InventoryContents = new Packet_InventoryContents();
		packet_InventoryContents.ClientId = player.ClientId;
		packet_InventoryContents.InventoryId = inv.InventoryID;
		packet_InventoryContents.InventoryClass = inv.ClassName;
		Packet_ItemStack[] Itemstacks = CreatePacketItemStacks();
		packet_InventoryContents.SetItemstacks(Itemstacks, Itemstacks.Length, Itemstacks.Length);
		return packet_InventoryContents;
	}

	public virtual Packet_Server getSlotUpdatePacket(IPlayer player, int slotId)
	{
		ItemSlot slot = inv[slotId];
		if (slot == null)
		{
			return null;
		}
		ItemStack itemstack = slot.Itemstack;
		Packet_ItemStack pstack = null;
		if (itemstack != null)
		{
			pstack = StackConverter.ToPacket(itemstack);
		}
		Packet_InventoryUpdate packet = new Packet_InventoryUpdate
		{
			ClientId = player.ClientId,
			InventoryId = inv.InventoryID,
			ItemStack = pstack,
			SlotId = slotId
		};
		return new Packet_Server
		{
			Id = 31,
			InventoryUpdate = packet
		};
	}

	public virtual object DidOpen(IPlayer player)
	{
		if (inv.Api.Side != EnumAppSide.Client)
		{
			return null;
		}
		Packet_InvOpenClose packet = new Packet_InvOpenClose
		{
			InventoryId = inv.InventoryID,
			Opened = 1
		};
		return new Packet_Client
		{
			Id = 30,
			InvOpenedClosed = packet
		};
	}

	public virtual object DidClose(IPlayer player)
	{
		if (inv.Api.Side != EnumAppSide.Client)
		{
			return null;
		}
		Packet_InvOpenClose packet = new Packet_InvOpenClose
		{
			InventoryId = inv.InventoryID,
			Opened = 0
		};
		return new Packet_Client
		{
			Id = 30,
			InvOpenedClosed = packet
		};
	}

	public virtual void UpdateFromPacket(IWorldAccessor resolver, Packet_InventoryContents packet)
	{
		for (int i = 0; i < packet.ItemstacksCount; i++)
		{
			ItemSlot slot = inv[i];
			if (UpdateSlotStack(slot, ItemStackFromPacket(resolver, packet.Itemstacks[i])))
			{
				inv.DidModifyItemSlot(slot);
			}
		}
	}

	public virtual void UpdateFromPacket(IWorldAccessor resolver, Packet_InventoryUpdate packet)
	{
		if (PauseInventoryUpdates)
		{
			pkts.Enqueue(packet);
			return;
		}
		if (packet.SlotId >= inv.Count)
		{
			throw new Exception("Client received server InventoryUpdate for " + inv.InventoryID + ", slot " + packet.SlotId + " but max is " + (inv.Count - 1) + ". For " + inv.ClassName + " at " + inv.Pos);
		}
		ItemSlot slot = inv[packet.SlotId];
		if (slot != null)
		{
			UpdateSlotStack(slot, ItemStackFromPacket(resolver, packet.ItemStack));
			inv.DidModifyItemSlot(slot);
		}
	}

	public virtual void UpdateFromPacket(IWorldAccessor resolver, Packet_InventoryDoubleUpdate packet)
	{
		if (packet.InventoryId1 == inv.InventoryID)
		{
			ItemSlot slot = inv[packet.SlotId1];
			UpdateSlotStack(slot, ItemStackFromPacket(resolver, packet.ItemStack1));
			inv.DidModifyItemSlot(slot);
		}
		if (packet.InventoryId2 == inv.InventoryID)
		{
			ItemSlot slot2 = inv[packet.SlotId2];
			UpdateSlotStack(slot2, ItemStackFromPacket(resolver, packet.ItemStack2));
			inv.DidModifyItemSlot(slot2);
		}
	}

	protected ItemStack ItemStackFromPacket(IWorldAccessor resolver, Packet_ItemStack pItemStack)
	{
		if (pItemStack == null || ((pItemStack.ItemClass == -1) | (pItemStack.ItemId == 0)))
		{
			return null;
		}
		return StackConverter.FromPacket(pItemStack, resolver);
	}

	private bool UpdateSlotStack(ItemSlot slot, ItemStack newStack)
	{
		if (slot.Itemstack != null && newStack != null && slot.Itemstack.Collectible == newStack.Collectible)
		{
			newStack.TempAttributes = slot.Itemstack?.TempAttributes;
		}
		bool result = newStack == null != (slot.Itemstack == null) || (newStack != null && !newStack.Equals(Api.World, slot.Itemstack, GlobalConstants.IgnoredStackAttributes));
		slot.Itemstack = newStack;
		return result;
	}

	public object GetActivateSlotPacket(int slotId, ItemStackMoveOperation op)
	{
		Packet_ActivateInventorySlot activateSlotPacket = new Packet_ActivateInventorySlot
		{
			MouseButton = (int)op.MouseButton,
			TargetInventoryId = inv.InventoryID,
			TargetSlot = slotId,
			TargetLastChanged = inv.lastChangedSinceServerStart,
			Modifiers = (int)op.Modifiers,
			Priority = (int)op.CurrentPriority,
			Dir = op.WheelDir
		};
		if (inv is ITabbedInventory)
		{
			activateSlotPacket.TabIndex = ((ITabbedInventory)inv).CurrentTab.Index;
		}
		return new Packet_Client
		{
			Id = 7,
			ActivateInventorySlot = activateSlotPacket
		};
	}

	public object GetFlipSlotsPacket(IInventory sourceInv, int sourceSlotId, int targetSlotId)
	{
		Packet_Client p = new Packet_Client
		{
			Id = 9,
			Flipitemstacks = new Packet_FlipItemstacks
			{
				SourceInventoryId = sourceInv.InventoryID,
				SourceLastChanged = ((InventoryBase)sourceInv).lastChangedSinceServerStart,
				SourceSlot = sourceSlotId,
				TargetInventoryId = inv.InventoryID,
				TargetLastChanged = inv.lastChangedSinceServerStart,
				TargetSlot = targetSlotId
			}
		};
		if (sourceInv is ITabbedInventory)
		{
			p.Flipitemstacks.SourceTabIndex = (sourceInv as ITabbedInventory).CurrentTab.Index;
		}
		if (sourceInv is CreativeInventoryTab)
		{
			p.Flipitemstacks.SourceTabIndex = (sourceInv as CreativeInventoryTab).TabIndex;
		}
		if (inv is ITabbedInventory)
		{
			p.Flipitemstacks.TargetTabIndex = (inv as ITabbedInventory).CurrentTab.Index;
		}
		if (inv is CreativeInventoryTab)
		{
			p.Flipitemstacks.TargetTabIndex = (inv as CreativeInventoryTab).TabIndex;
		}
		return p;
	}
}
