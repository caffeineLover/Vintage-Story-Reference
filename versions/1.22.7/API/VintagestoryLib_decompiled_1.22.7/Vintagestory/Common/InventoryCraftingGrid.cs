using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.Common;

public class InventoryCraftingGrid : InventoryBasePlayer
{
	private int GridSize = 3;

	private int GridSizeSq = 9;

	private ItemSlot[] slots;

	private ItemSlotCraftingOutput outputSlot;

	public GridRecipe MatchingRecipe;

	private bool isCrafting;

	public override int Count => GridSizeSq + 1;

	public override ItemSlot this[int slotId]
	{
		get
		{
			if (slotId < 0 || slotId >= Count)
			{
				return null;
			}
			if (slotId == GridSizeSq)
			{
				return outputSlot;
			}
			return slots[slotId];
		}
		set
		{
			if (slotId < 0 || slotId >= Count)
			{
				throw new ArgumentOutOfRangeException("slotId");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (slotId == GridSizeSq)
			{
				outputSlot = (ItemSlotCraftingOutput)value;
			}
			else
			{
				slots[slotId] = value;
			}
		}
	}

	public InventoryCraftingGrid(string inventoryID, ICoreAPI api)
		: base(inventoryID, api)
	{
		slots = GenEmptySlots(GridSizeSq);
		outputSlot = new ItemSlotCraftingOutput(this);
		InvNetworkUtil = new CraftingInventoryNetworkUtil(this, api);
	}

	public InventoryCraftingGrid(string className, string instanceID, ICoreAPI api)
		: base(className, instanceID, api)
	{
		slots = GenEmptySlots(GridSizeSq);
		outputSlot = new ItemSlotCraftingOutput(this);
		InvNetworkUtil = new CraftingInventoryNetworkUtil(this, api);
	}

	public override void LateInitialize(string inventoryID, ICoreAPI api)
	{
		base.LateInitialize(inventoryID, api);
		(InvNetworkUtil as CraftingInventoryNetworkUtil).Api = api;
	}

	internal void BeginCraft()
	{
		isCrafting = true;
	}

	internal void EndCraft()
	{
		isCrafting = false;
		FindMatchingRecipe();
	}

	public bool CanStillCraftCurrent(GridRecipe justCraftedRecipe)
	{
		if (MatchingRecipe != null && MatchingRecipe == justCraftedRecipe)
		{
			return MatchingRecipe.Matches(Api.World.PlayerByUid(playerUID), Api.World, slots, GridSize);
		}
		return false;
	}

	public override float GetTransitionSpeedMul(EnumTransitionType transType, ItemStack stack)
	{
		if (stack == outputSlot.Itemstack)
		{
			return 0f;
		}
		return base.GetTransitionSpeedMul(transType, stack);
	}

	public override object ActivateSlot(int slotId, ItemSlot sourceSlot, ref ItemStackMoveOperation op)
	{
		object packet;
		if (slotId == GridSizeSq)
		{
			BeginCraft();
			packet = base.ActivateSlot(slotId, sourceSlot, ref op);
			if (!outputSlot.Empty && op.ShiftDown)
			{
				if (Api.Side == EnumAppSide.Client)
				{
					outputSlot.Itemstack = null;
				}
				else
				{
					base.Player.InventoryManager.DropItem(outputSlot, fullStack: true);
				}
			}
			EndCraft();
		}
		else
		{
			packet = base.ActivateSlot(slotId, sourceSlot, ref op);
		}
		return packet;
	}

	public override void OnItemSlotModified(ItemSlot slot)
	{
		if (!isCrafting && !(slot is ItemSlotCraftingOutput))
		{
			DropSlotIfHot(slot, base.Player);
			FindMatchingRecipe();
		}
	}

	public override void DidModifyItemSlot(ItemSlot slot, ItemStack extractedStack = null)
	{
		base.DidModifyItemSlot(slot, extractedStack);
	}

	public override bool TryMoveItemStack(IPlayer player, string[] invIds, int[] slotIds, ref ItemStackMoveOperation op)
	{
		bool num = base.TryMoveItemStack(player, invIds, slotIds, ref op);
		if (num)
		{
			FindMatchingRecipe();
		}
		return num;
	}

	internal void FindMatchingRecipe()
	{
		MatchingRecipe = null;
		outputSlot.Itemstack = null;
		List<GridRecipe> recipes = new List<GridRecipe>();
		IPlayer player = Api.World.PlayerByUid(playerUID);
		System.Collections.Generic.OrderedDictionary<IRecipeIngredientBase, List<IRecipeBase>> fastSearchDict = Api.World.FastSearchRecipesByIngredient;
		ItemSlot[] array = slots;
		for (int i = 0; i < array.Length; i++)
		{
			ItemStack inputStack = array[i].Itemstack;
			if (inputStack == null || inputStack.StackSize == 0)
			{
				continue;
			}
			foreach (KeyValuePair<IRecipeIngredientBase, List<IRecipeBase>> entry in fastSearchDict)
			{
				if (!entry.Key.SatisfiesAsIngredient(inputStack, checkStackSize: false))
				{
					continue;
				}
				foreach (IRecipeBase item in entry.Value)
				{
					if (item is GridRecipe { Enabled: not false } gridRecipe)
					{
						recipes.AddIfNotPresent(gridRecipe);
					}
				}
			}
		}
		foreach (GridRecipe recipe in recipes)
		{
			if (!recipe.Shapeless && recipe.Matches(player, Api.World, slots, GridSize))
			{
				FoundMatch(recipe);
				return;
			}
		}
		foreach (GridRecipe recipe2 in recipes)
		{
			if (recipe2.Shapeless && recipe2.Matches(player, Api.World, slots, GridSize))
			{
				FoundMatch(recipe2);
				return;
			}
		}
		dirtySlots.Add(GridSizeSq);
	}

	private void FoundMatch(GridRecipe recipe)
	{
		MatchingRecipe = recipe;
		MatchingRecipe.GenerateOutputStack(slots, outputSlot);
		dirtySlots.Add(GridSizeSq);
	}

	internal GridRecipe ConsumeIngredients(ItemSlot outputSlot)
	{
		if (MatchingRecipe == null || outputSlot.Itemstack == null)
		{
			return null;
		}
		GridRecipe craftedRecipe = MatchingRecipe;
		if (!outputSlot.Itemstack.Collectible.ConsumeCraftingIngredients(slots, outputSlot, MatchingRecipe) && MatchingRecipe.ConsumeInput(Api.World.PlayerByUid(playerUID), slots, GridSize))
		{
			FindMatchingRecipe();
		}
		for (int i = 0; i < GridSizeSq + 1; i++)
		{
			dirtySlots.Add(i);
		}
		return craftedRecipe;
	}

	public override WeightedSlot GetBestSuitedSlot(ItemSlot sourceSlot, ItemStackMoveOperation op, List<ItemSlot> skipSlots = null)
	{
		return new WeightedSlot
		{
			slot = null,
			weight = 0f
		};
	}

	public override void FromTreeAttributes(ITreeAttribute tree)
	{
		ItemSlot[] attrSlots = SlotsFromTreeAttributes(tree);
		if (attrSlots?.Length == slots.Length)
		{
			slots = attrSlots;
		}
	}

	public override void ToTreeAttributes(ITreeAttribute tree)
	{
		SlotsToTreeAttributes(slots, tree);
		ResolveBlocksOrItems();
	}

	public override void OnOwningEntityDeath(Vec3d pos)
	{
		using IEnumerator<ItemSlot> enumerator = GetEnumerator();
		while (enumerator.MoveNext())
		{
			ItemSlot slot = enumerator.Current;
			if (!(slot is ItemSlotCraftingOutput) && !slot.Empty)
			{
				Api.World.SpawnItemEntity(slot.Itemstack, pos);
				slot.Itemstack = null;
				slot.MarkDirty();
			}
		}
	}
}
