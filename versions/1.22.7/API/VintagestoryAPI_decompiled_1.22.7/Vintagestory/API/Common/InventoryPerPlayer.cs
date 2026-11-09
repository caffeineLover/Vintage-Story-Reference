using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Server;

namespace Vintagestory.API.Common;

public class InventoryPerPlayer : InventoryGeneric
{
	public Dictionary<string, int[]> PlayerQuantities;

	public int[] Quantities;

	public override bool PutLocked { get; set; } = true;

	public InventoryPerPlayer(int quantitySlots, string? invId, ICoreAPI? api, NewSlotDelegate? onNewSlot = null)
		: base(quantitySlots, invId, api, onNewSlot)
	{
		PlayerQuantities = new Dictionary<string, int[]>();
		Quantities = new int[quantitySlots];
	}

	public bool CanTake(ItemSlot fromSlot, ItemStackMoveOperation op)
	{
		return GetPlayerRemaining(op.ActingPlayer.PlayerUID, GetSlotId(fromSlot)) > 0;
	}

	public void AddPlayerUsage(string playerUid, int slotId, int value)
	{
		if (PlayerQuantities.TryGetValue(playerUid, out int[] quantities))
		{
			quantities[slotId] += value;
		}
	}

	public int GetPlayerRemaining(string playerUid, int slotId)
	{
		if (PlayerQuantities.TryGetValue(playerUid, out int[] quantities))
		{
			return Math.Max(0, Quantities[slotId] - quantities[slotId]);
		}
		quantities = new int[Quantities.Length];
		PlayerQuantities.Add(playerUid, quantities);
		return Math.Max(0, Quantities[slotId] - quantities[slotId]);
	}

	public override void MarkSlotDirty(int slotId)
	{
	}

	/// <summary>
	/// allow the server to resend changed PlayerQuantities
	/// </summary>
	public void MarkDirty()
	{
		if (Api is ICoreServerAPI sapi)
		{
			sapi.World.BlockAccessor.GetBlockEntity(Pos).MarkDirty();
		}
	}

	protected override ItemSlot NewSlot(int slotId)
	{
		return new ItemSlotPerPlayer(this, slotId);
	}

	public void OnPlacementBySchematic()
	{
		for (int i = 0; i < slots.Length; i++)
		{
			Quantities[i] = this[i].StackSize;
		}
	}

	public override void FromTreeAttributes(ITreeAttribute treeAttribute)
	{
		base.FromTreeAttributes(treeAttribute);
		if (treeAttribute["PlayerQuantities"] is TreeAttribute tree)
		{
			if (tree.Count > 0)
			{
				foreach (var (playerUid, attribute2) in tree)
				{
					PlayerQuantities[playerUid] = (attribute2 as IntArrayAttribute)?.value ?? new int[Count];
				}
			}
			else
			{
				PlayerQuantities.Clear();
			}
		}
		Quantities = (treeAttribute["Quantities"] as IntArrayAttribute)?.value ?? new int[Count];
		if (!(Api is ICoreClientAPI capi) || !PlayerQuantities.ContainsKey(capi.World.Player.PlayerUID))
		{
			return;
		}
		for (int i = 0; i < Quantities.Length; i++)
		{
			ItemStack itemStack = this[i].Itemstack;
			if (itemStack == null)
			{
				continue;
			}
			int remaining = GetPlayerRemaining(capi.World.Player.PlayerUID, i);
			if (remaining == 0)
			{
				if (capi.World.Player.WorldData.CurrentGameMode == EnumGameMode.Survival)
				{
					this[i].Itemstack = null;
				}
			}
			else
			{
				itemStack.StackSize = remaining;
			}
		}
	}

	public override void ToTreeAttributes(ITreeAttribute invtree)
	{
		TreeAttribute tree = new TreeAttribute();
		foreach (KeyValuePair<string, int[]> playerQuantity in PlayerQuantities)
		{
			playerQuantity.Deconstruct(out var key, out var value);
			string playerUid = key;
			int[] quantities = value;
			tree[playerUid] = new IntArrayAttribute(quantities);
		}
		invtree["PlayerQuantities"] = tree;
		invtree["Quantities"] = new IntArrayAttribute(Quantities);
		base.ToTreeAttributes(invtree);
	}
}
