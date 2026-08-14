using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Client;

namespace Vintagestory.API.Common;

/// <summary>
/// The contents of one or more bags
/// </summary>
public class BagInventory : IReadOnlyCollection<ItemSlot>, IEnumerable<ItemSlot>, IEnumerable
{
	protected ICoreAPI Api;

	protected List<ItemSlot> bagContents = new List<ItemSlot>();

	private ItemSlot[] bagSlots;

	public int Count => bagContents.Count;

	public ItemSlot[] BagSlots
	{
		get
		{
			return bagSlots;
		}
		set
		{
			bagSlots = value;
		}
	}

	public ItemSlot this[int slotId]
	{
		get
		{
			return bagContents[slotId];
		}
		set
		{
			bagContents[slotId] = value;
		}
	}

	public BagInventory(ICoreAPI api, ItemSlot[] bagSlots)
	{
		BagSlots = bagSlots;
		Api = api;
	}

	public void SaveSlotIntoBag(ItemSlotBagContent slot)
	{
		ItemStack backpackStack = BagSlots[slot.BagIndex].Itemstack;
		backpackStack?.Collectible.GetCollectibleInterface<IHeldBag>().Store(backpackStack, slot);
	}

	public void SaveSlotsIntoBags()
	{
		if (BagSlots == null)
		{
			return;
		}
		foreach (ItemSlot slot in bagContents)
		{
			SaveSlotIntoBag((ItemSlotBagContent)slot);
		}
	}

	public void ReloadBagInventory(InventoryBase parentinv, ItemSlot[] bagSlots)
	{
		BagSlots = bagSlots;
		if (BagSlots == null || BagSlots.Length == 0)
		{
			bagContents.Clear();
			return;
		}
		bagContents.Clear();
		for (int bagIndex = 0; bagIndex < BagSlots.Length; bagIndex++)
		{
			ItemStack bagstack = BagSlots[bagIndex].Itemstack;
			if (bagstack != null && bagstack.ItemAttributes != null)
			{
				bagstack.ResolveBlockOrItem(Api.World);
				IHeldBag bag = bagstack.Collectible.GetCollectibleInterface<IHeldBag>();
				if (bag != null)
				{
					List<ItemSlotBagContent> slots = bag.GetOrCreateSlots(bagstack, parentinv, bagIndex, Api.World);
					bagContents.AddRange(slots);
				}
			}
		}
		if (!(Api is ICoreClientAPI capi))
		{
			return;
		}
		ItemSlotBagContent currentHoveredSlot = capi.World.Player?.InventoryManager.CurrentHoveredSlot as ItemSlotBagContent;
		if (currentHoveredSlot?.Inventory == parentinv)
		{
			ItemSlot hslot = bagContents.FirstOrDefault((ItemSlot slot) => (slot as ItemSlotBagContent).SlotIndex == currentHoveredSlot.SlotIndex && (slot as ItemSlotBagContent).BagIndex == currentHoveredSlot.BagIndex);
			if (hslot != null)
			{
				capi.World.Player.InventoryManager.CurrentHoveredSlot = hslot;
			}
		}
	}

	/// <summary>
	/// Gets the enumerator for the inventory.
	/// </summary>
	public IEnumerator<ItemSlot> GetEnumerator()
	{
		for (int i = 0; i < Count; i++)
		{
			yield return this[i];
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
