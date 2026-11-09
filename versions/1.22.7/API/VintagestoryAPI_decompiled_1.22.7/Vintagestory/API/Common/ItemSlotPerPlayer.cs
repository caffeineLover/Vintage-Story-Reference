using System;
using Vintagestory.API.Client;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace Vintagestory.API.Common;

public class ItemSlotPerPlayer : ItemSlot
{
	public int Slotid;

	public new InventoryPerPlayer Inventory => (InventoryPerPlayer)inventory;

	public override bool DrawUnavailable
	{
		get
		{
			IClientWorldAccessor world = ((ICoreClientAPI)Inventory.Api).World;
			if (world.Player.WorldData.CurrentGameMode == EnumGameMode.Survival)
			{
				return false;
			}
			return Inventory.GetPlayerRemaining(world.Player.PlayerUID, Slotid) <= 0;
		}
		set
		{
		}
	}

	public ItemSlotPerPlayer(InventoryBase inventory, int slotid)
		: base(inventory)
	{
		Slotid = slotid;
	}

	public override bool CanTakeFrom(ItemSlot sourceSlot, EnumMergePriority priority = EnumMergePriority.AutoMerge)
	{
		return false;
	}

	public override bool CanHold(ItemSlot sourceSlot)
	{
		return false;
	}

	public override bool CanTake()
	{
		return false;
	}

	public override ItemStack? TakeOutWhole()
	{
		return null;
	}

	public override ItemStack? TakeOut(int quantity)
	{
		return null;
	}

	public override int TryPutInto(IWorldAccessor world, ItemSlot sinkSlot, int quantity = 1)
	{
		return 0;
	}

	public override int TryPutInto(ItemSlot sinkSlot, ref ItemStackMoveOperation op)
	{
		if (!sinkSlot.CanTakeFrom(this) || base.Itemstack == null)
		{
			return 0;
		}
		InventoryBase inventoryBase = sinkSlot.Inventory;
		if (inventoryBase != null && !inventoryBase.CanContain(sinkSlot, this))
		{
			return 0;
		}
		int remaining = Inventory.GetPlayerRemaining(op.ActingPlayer.PlayerUID, Slotid);
		if (sinkSlot.Itemstack == null)
		{
			int q = GameMath.Min(sinkSlot.GetRemainingSlotSpace(itemstack), op.RequestedQuantity, remaining);
			if (q > 0)
			{
				sinkSlot.Itemstack = base.Itemstack.GetEmptyClone();
				sinkSlot.Itemstack.StackSize = q;
				op.MovedQuantity = (op.MovableQuantity = Math.Min(sinkSlot.StackSize, q));
				remaining -= op.MovedQuantity;
				Inventory.AddPlayerUsage(op.ActingPlayer.PlayerUID, Slotid, op.MovedQuantity);
				if (op.World is IClientWorldAccessor)
				{
					base.Itemstack.StackSize = remaining;
					if (remaining == 0)
					{
						base.Itemstack = null;
					}
				}
				sinkSlot.OnItemSlotModified(sinkSlot.Itemstack);
				OnItemSlotModified(sinkSlot.Itemstack);
				Inventory.MarkDirty();
			}
			return op.MovedQuantity;
		}
		ItemStackMergeOperation mergeop = (ItemStackMergeOperation)(op = op.ToMergeOperation(sinkSlot, this));
		int origRequestedQuantity = op.RequestedQuantity;
		op.RequestedQuantity = GameMath.Min(sinkSlot.GetRemainingSlotSpace(itemstack), op.RequestedQuantity, remaining);
		ItemStack ownStack = base.Itemstack.Clone();
		sinkSlot.Itemstack.Collectible.TryMergeStacks(mergeop);
		if (op.World is IServerWorldAccessor)
		{
			base.Itemstack = ownStack;
		}
		else if (remaining == 0)
		{
			base.Itemstack = null;
		}
		sinkSlot.OnItemSlotModified(sinkSlot.Itemstack);
		OnItemSlotModified(sinkSlot.Itemstack);
		op.RequestedQuantity = origRequestedQuantity;
		Inventory.AddPlayerUsage(op.ActingPlayer.PlayerUID, Slotid, mergeop.MovedQuantity);
		Inventory.MarkDirty();
		return mergeop.MovedQuantity;
	}

	public override void ActivateSlot(ItemSlot sourceSlot, ref ItemStackMoveOperation op)
	{
		if (!Empty || !sourceSlot.Empty)
		{
			switch (op.MouseButton)
			{
			case EnumMouseButton.Left:
				ActivateSlotLeftClick(sourceSlot, ref op);
				break;
			case EnumMouseButton.Right:
				ActivateSlotRightClick(sourceSlot, ref op);
				break;
			}
		}
	}

	protected override void ActivateSlotLeftClick(ItemSlot sinkSlot, ref ItemStackMoveOperation op)
	{
		if (!sinkSlot.Empty && op.ActingPlayer.WorldData.CurrentGameMode == EnumGameMode.Creative)
		{
			itemstack = sinkSlot.Itemstack.Clone();
			Inventory.Quantities[Slotid] = itemstack.StackSize;
			MarkDirty();
			Inventory.MarkDirty();
		}
		else if (sinkSlot.Empty && op.ActingPlayer.WorldData.CurrentGameMode == EnumGameMode.Creative && op.CtrlDown)
		{
			itemstack.StackSize = 0;
			itemstack = null;
			Inventory.Quantities[Slotid] = 0;
			MarkDirty();
			Inventory.MarkDirty();
		}
		else if (sinkSlot.Empty)
		{
			if (Inventory.CanTake(this, op))
			{
				op.RequestedQuantity = Inventory.GetPlayerRemaining(op.ActingPlayer.PlayerUID, Slotid);
				TryPutInto(sinkSlot, ref op);
			}
		}
		else if (sinkSlot.Itemstack.Equals(op.World, base.Itemstack, GlobalConstants.IgnoredStackAttributes) && Inventory.CanTake(this, op))
		{
			op.RequestedQuantity = 1;
			ItemStackMergeOperation mergeop = (ItemStackMergeOperation)(op = op.ToMergeOperation(sinkSlot, this));
			ItemStack ownStack = base.Itemstack?.Clone();
			sinkSlot.Itemstack.Collectible.TryMergeStacks(mergeop);
			if (op.World is IServerWorldAccessor)
			{
				base.Itemstack = ownStack;
			}
			Inventory.AddPlayerUsage(op.ActingPlayer.PlayerUID, Slotid, mergeop.MovedQuantity);
			Inventory.MarkDirty();
		}
	}

	protected override void ActivateSlotRightClick(ItemSlot sourceSlot, ref ItemStackMoveOperation op)
	{
		if (sourceSlot.Empty && Inventory.CanTake(this, op))
		{
			int remaining = Inventory.GetPlayerRemaining(op.ActingPlayer.PlayerUID, Slotid);
			op.RequestedQuantity = remaining / 2;
			TryPutInto(sourceSlot, ref op);
		}
	}

	public override bool TryFlipWith(ItemSlot itemSlot)
	{
		return false;
	}

	protected override void FlipWith(ItemSlot withslot)
	{
	}
}
