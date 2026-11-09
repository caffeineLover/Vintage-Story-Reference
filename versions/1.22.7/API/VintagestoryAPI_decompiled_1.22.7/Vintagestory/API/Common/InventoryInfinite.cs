using System;
using System.Collections.Generic;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.API.Common;

/// <summary>
/// A inventory meant for block entities, with no upper limit in size (int.MaxValue, to be precise)
/// </summary>
public class InventoryInfinite : InventoryBase
{
	public Vintagestory.API.Datastructures.OrderedDictionary<string, ItemSlot> SlotsByslotId = new Vintagestory.API.Datastructures.OrderedDictionary<string, ItemSlot>();

	private NewInfSlotDelegate onNewSlot;

	public GetSuitabilityDelegate OnGetSuitability;

	public GetAutoPushIntoSlotDelegate OnGetAutoPushIntoSlot;

	public GetAutoPullFromSlotDelegate OnGetAutoPullFromSlot;

	public ItemSlot this[string slotId]
	{
		get
		{
			SlotsByslotId.TryGetValue(slotId, out var slot);
			slot?.Itemstack?.ResolveBlockOrItem(Api.World);
			return slot;
		}
		set
		{
			SlotsByslotId[slotId] = value;
		}
	}

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
	/// Amount of defined slots
	/// </summary>
	public override int Count => SlotsByslotId.Count;

	public override ItemSlot this[int slotId]
	{
		get
		{
			return SlotsByslotId.GetValueAtIndex(slotId);
		}
		set
		{
			throw new InvalidOperationException();
		}
	}

	/// <summary>
	/// Creates an inventory with no upper limit in size (int.MaxValue, to be precise)
	/// </summary>
	public InventoryInfinite(NewInfSlotDelegate onNewSlot = null)
		: base("-", null)
	{
		this.onNewSlot = onNewSlot;
		OnGetSuitability = (ItemSlot s, ItemSlot t, bool isMerge) => (!isMerge) ? (baseWeight + 1f) : (baseWeight + 3f);
	}

	public override IEnumerator<ItemSlot> GetEnumerator()
	{
		return SlotsByslotId.Values.GetEnumerator();
	}

	public override void DidModifyItemSlot(ItemSlot slot, ItemStack extractedStack = null)
	{
	}

	public override float GetSuitability(ItemSlot sourceSlot, ItemSlot targetSlot, bool isMerge)
	{
		return OnGetSuitability(sourceSlot, targetSlot, isMerge);
	}

	/// <summary>
	/// Loads the slot contents from given treeAttribute
	/// </summary>
	/// <param name="treeAttribute"></param>
	public override void FromTreeAttributes(ITreeAttribute tree)
	{
		SlotsByslotId.Clear();
		if (tree == null)
		{
			return;
		}
		foreach (KeyValuePair<string, IAttribute> val in tree)
		{
			string slotId = val.Key;
			ItemSlot slot = NewSlot(slotId);
			slot.Itemstack = (val.Value as ItemstackAttribute)?.value;
			SlotsByslotId[slotId] = slot;
		}
	}

	/// <summary>
	/// Stores the slot contents to invtree
	/// </summary>
	/// <param name="invtree"></param>
	public override void ToTreeAttributes(ITreeAttribute invtree)
	{
		foreach (KeyValuePair<string, ItemSlot> val in SlotsByslotId)
		{
			invtree[val.Key ?? ""] = new ItemstackAttribute(val.Value.Itemstack);
		}
	}

	public void Allocate(string slotId)
	{
		if (!SlotsByslotId.ContainsKey(slotId))
		{
			SlotsByslotId[slotId] = onNewSlot(slotId, this);
		}
	}

	/// <summary>
	/// Called when initializing the inventory or when loading the contents
	/// </summary>
	/// <param name="slotId"></param>
	/// <returns></returns>
	protected ItemSlot NewSlot(string slotId)
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
