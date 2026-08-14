using System;
using System.Collections.Generic;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.API.Common;

/// <summary>
/// A general purpose inventory
/// </summary>
public class InventoryGeneric : InventoryBase
{
	protected ItemSlot[] slots;

	private NewSlotDelegate onNewSlot;

	public GetSuitabilityDelegate OnGetSuitability;

	public GetAutoPushIntoSlotDelegate OnGetAutoPushIntoSlot;

	public GetAutoPullFromSlotDelegate OnGetAutoPullFromSlot;

	public Dictionary<EnumTransitionType, float> TransitionableSpeedMulByType { get; set; }

	public Dictionary<EnumFoodCategory, float> PerishableFactorByFoodCategory { get; set; }

	public float BaseWeight
	{
		get
		{
			return baseWeight;
		}
		set
		{
			baseWeight = value;
		}
	}

	/// <summary>
	/// Amount of available slots
	/// </summary>
	public override int Count => slots.Length;

	/// <summary>
	/// Get slot for given slot index
	/// </summary>
	/// <param name="slotId"></param>
	/// <returns></returns>
	public override ItemSlot this[int slotId]
	{
		get
		{
			if (slotId < 0 || slotId >= Count)
			{
				throw new ArgumentOutOfRangeException("slotId", "Index was " + ((slotId < 0) ? "negative: " : "too large: ") + slotId + "/" + Count + " for " + className + " at " + Pos);
			}
			return slots[slotId];
		}
		set
		{
			if (slotId < 0 || slotId >= Count)
			{
				throw new ArgumentOutOfRangeException("slotId", "Index was " + ((slotId < 0) ? "negative: " : "too large: ") + slotId + "/" + Count + " for " + className + " at " + Pos);
			}
			slots[slotId] = value ?? throw new ArgumentNullException("value");
		}
	}

	/// <summary>
	/// Creates an empty (invalid) inventory. Must call init() to propery init the inventory
	/// </summary>
	public InventoryGeneric(ICoreAPI api)
		: base("-", api)
	{
	}

	public void Init(int quantitySlots, string className, string instanceId, NewSlotDelegate onNewSlot = null)
	{
		instanceID = instanceId;
		base.className = className;
		OnGetSuitability = (ItemSlot s, ItemSlot t, bool isMerge) => (!isMerge) ? (baseWeight + 1f) : (baseWeight + 3f);
		this.onNewSlot = onNewSlot;
		slots = GenEmptySlots(quantitySlots);
	}

	/// <summary>
	/// Create a new general purpose inventory
	/// </summary>
	/// <param name="quantitySlots"></param>
	/// <param name="className"></param>
	/// <param name="instanceId"></param>
	/// <param name="api"></param>
	/// <param name="onNewSlot"></param>
	public InventoryGeneric(int quantitySlots, string className, string instanceId, ICoreAPI api, NewSlotDelegate onNewSlot = null)
		: base(className, instanceId, api)
	{
		Init(quantitySlots, className, instanceId, onNewSlot);
	}

	/// <summary>
	/// Create a new general purpose inventory
	/// </summary>
	/// <param name="quantitySlots"></param>
	/// <param name="invId"></param>
	/// <param name="api"></param>
	/// <param name="onNewSlot"></param>
	public InventoryGeneric(int quantitySlots, string invId, ICoreAPI api, NewSlotDelegate onNewSlot = null)
		: base(invId, api)
	{
		OnGetSuitability = (ItemSlot s, ItemSlot t, bool isMerge) => (!isMerge) ? (baseWeight + 1f) : (baseWeight + 3f);
		this.onNewSlot = onNewSlot;
		slots = GenEmptySlots(quantitySlots);
	}

	public override float GetSuitability(ItemSlot sourceSlot, ItemSlot targetSlot, bool isMerge)
	{
		return OnGetSuitability(sourceSlot, targetSlot, isMerge);
	}

	/// <summary>
	/// Loads the slot contents from given treeAttribute
	/// </summary>
	/// <param name="treeAttribute"></param>
	public override void FromTreeAttributes(ITreeAttribute treeAttribute)
	{
		int num = slots.Length;
		slots = SlotsFromTreeAttributes(treeAttribute, slots);
		int add = num - slots.Length;
		AddSlots(add);
	}

	public void AddSlots(int amount)
	{
		while (amount-- > 0)
		{
			slots = slots.Append(NewSlot(slots.Length));
		}
	}

	/// <summary>
	/// Stores the slot contents to invtree
	/// </summary>
	/// <param name="invtree"></param>
	public override void ToTreeAttributes(ITreeAttribute invtree)
	{
		SlotsToTreeAttributes(slots, invtree);
	}

	/// <summary>
	/// Called when initializing the inventory or when loading the contents
	/// </summary>
	/// <param name="slotId"></param>
	/// <returns></returns>
	protected override ItemSlot NewSlot(int slotId)
	{
		if (onNewSlot != null)
		{
			return onNewSlot(slotId, this);
		}
		return new ItemSlotSurvival(this);
	}

	public override float GetTransitionSpeedMul(EnumTransitionType transType, ItemStack stack)
	{
		float mul = GetDefaultTransitionSpeedMul(transType);
		FoodNutritionProperties nutritionProps = stack.Collectible.GetNutritionProperties(Api.World, stack, null);
		if (transType == EnumTransitionType.Perish && PerishableFactorByFoodCategory != null && nutritionProps != null)
		{
			if (!PerishableFactorByFoodCategory.TryGetValue(nutritionProps.FoodCategory, out var rateMul))
			{
				rateMul = 1f;
			}
			mul *= rateMul;
		}
		if (TransitionableSpeedMulByType != null)
		{
			if (!TransitionableSpeedMulByType.TryGetValue(transType, out var rateMul2))
			{
				rateMul2 = 1f;
			}
			mul *= rateMul2;
		}
		return InvokeTransitionSpeedDelegates(transType, stack, mul);
	}

	public override ItemSlot GetAutoPullFromSlot(BlockFacing atBlockFace)
	{
		if (OnGetAutoPullFromSlot != null)
		{
			return OnGetAutoPullFromSlot(atBlockFace);
		}
		return base.GetAutoPullFromSlot(atBlockFace);
	}

	public override ItemSlot GetAutoPushIntoSlot(BlockFacing atBlockFace, ItemSlot fromSlot)
	{
		if (OnGetAutoPushIntoSlot != null)
		{
			return OnGetAutoPushIntoSlot(atBlockFace, fromSlot);
		}
		return base.GetAutoPushIntoSlot(atBlockFace, fromSlot);
	}
}
