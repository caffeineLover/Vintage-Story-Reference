using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.Common;

public class InventoryPlayerHotbar : InventoryBasePlayer
{
	protected List<string> mainHandStatMod = new List<string>();

	protected List<string> offHandStatMod = new List<string>();

	protected ItemSlot[] slots;

	protected const float offHandSlotSuitability = 0.4f;

	protected const int skillSlotIndex = 10;

	protected const int offHandSlotIndex = 11;

	protected const float specialSlotsSuitabilityBonus = 0.5f;

	public virtual float OffHandHungerPenalty { get; set; } = 0.2f;

	protected virtual int slotsNumber => 12;

	public override int Count => slots.Length;

	public override ItemSlot? this[int slotId]
	{
		get
		{
			if (slotId < 0 || slotId >= Count)
			{
				return null;
			}
			return slots[slotId];
		}
		set
		{
			if (slotId < 0 || slotId >= Count)
			{
				throw new ArgumentOutOfRangeException("slotId", $"Trying to access hotbar slot by index, but index '{slotId}' is outside of range [0, {Count})");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value", $"Trying to set hotbar slot with index '{slotId}' to null");
			}
			slots[slotId] = value;
		}
	}

	public InventoryPlayerHotbar(string className, string playerUID, ICoreAPI api)
		: base(className, playerUID, api)
	{
		slots = GenEmptySlots(slotsNumber);
		baseWeight = 1.1f;
		InvNetworkUtil = new PlayerInventoryNetworkUtil(this, api);
	}

	public InventoryPlayerHotbar(string inventoryId, ICoreAPI api)
		: base(inventoryId, api)
	{
		slots = GenEmptySlots(slotsNumber);
		baseWeight = 1.1f;
		InvNetworkUtil = new PlayerInventoryNetworkUtil(this, api);
	}

	public override void OnItemSlotModified(ItemSlot slot)
	{
		base.OnItemSlotModified(slot);
		updateSlotStatMods(slot);
	}

	public virtual void updateSlotStatMods(ItemSlot slot)
	{
		if (slot is ItemSlotOffhand)
		{
			updateSlotStatMods(offHandStatMod, slot, "offhanditem");
		}
		else if (slot == base.Player.InventoryManager.ActiveHotbarSlot)
		{
			DropSlotIfHot(slot, base.Player);
			updateSlotStatMods(mainHandStatMod, slot, "mainhanditem");
		}
	}

	public virtual void updateSlotStatMods(List<string> list, ItemSlot slot, string handcategory)
	{
		IPlayer player = Api.World.PlayerByUid(playerUID);
		if (Api.Side == EnumAppSide.Client)
		{
			player.InventoryManager.BroadcastHotbarSlot();
		}
		if (player.Entity?.Stats == null)
		{
			return;
		}
		foreach (string stat in list)
		{
			player.Entity.Stats.Remove(stat, handcategory);
		}
		list.Clear();
		if (slot.Empty)
		{
			return;
		}
		JsonObject itemAttributes = slot.Itemstack.ItemAttributes;
		if (itemAttributes == null || !itemAttributes["statModifier"].Exists)
		{
			if (handcategory == "offhanditem")
			{
				player.Entity.Stats.Set("hungerrate", "offhanditem", OffHandHungerPenalty, persistent: true);
				list.Add("hungerrate");
			}
			return;
		}
		JsonObject statmods = slot.Itemstack.ItemAttributes?["statModifier"];
		if (statmods == null)
		{
			return;
		}
		foreach (JsonObject item in statmods)
		{
			string stat2 = item.AsString();
			if (stat2 != null)
			{
				player.Entity.Stats.Set(stat2, handcategory, statmods[stat2].AsFloat(), persistent: true);
				list.Add(stat2);
			}
		}
	}

	public override void FromTreeAttributes(ITreeAttribute tree)
	{
		slots = SlotsFromTreeAttributes(tree);
		UpdateSlotsToCurrentGameVersion();
	}

	public override float GetSuitability(ItemSlot sourceSlot, ItemSlot targetSlot, bool isMerge)
	{
		if (!isMerge && targetSlot == slots[11])
		{
			return 0.4f;
		}
		float specialSlotsSuitabilityOffset = ((sourceSlot is ItemSlotGround || sourceSlot is DummySlot) ? 0.5f : 0f);
		return base.GetSuitability(sourceSlot, targetSlot, isMerge) + specialSlotsSuitabilityOffset;
	}

	public override void ToTreeAttributes(ITreeAttribute tree)
	{
		SlotsToTreeAttributes(slots, tree);
		ResolveBlocksOrItems();
	}

	public override void DropAll(Vec3d pos, int maxStackSize = 0)
	{
		slots[10].Itemstack = null;
		base.DropAll(pos, maxStackSize);
	}

	protected override ItemSlot NewSlot(int slotId)
	{
		return slotId switch
		{
			10 => new ItemSlotSkill(this), 
			11 => new ItemSlotOffhand(this), 
			_ => new ItemSlotSurvival(this)
			{
				BackgroundIcon = (1 + slotId).ToString()
			}, 
		};
	}

	protected virtual void UpdateSlotsToCurrentGameVersion()
	{
		if (slots.Length <= 10)
		{
			slots = slots.Append(new ItemSlotSkill(this));
			slots = slots.Append(new ItemSlotOffhand(this));
		}
		else if (slots.Length <= 11)
		{
			slots = slots.Append(new ItemSlotOffhand(this));
			if (slots.Length == 12)
			{
				slots[11].Itemstack = slots[10].Itemstack;
				slots[10].Itemstack = null;
			}
		}
	}
}
