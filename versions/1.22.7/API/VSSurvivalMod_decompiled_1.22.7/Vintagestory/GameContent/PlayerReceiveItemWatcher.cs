using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;

namespace Vintagestory.GameContent;

public class PlayerReceiveItemWatcher : PlayerMilestoneWatcherGeneric
{
	public ItemStackMatcherDelegate StackMatcher;

	public string MatchEventName;

	public override void OnItemStackReceived(ItemStack stack, string eventName)
	{
		if (eventName == MatchEventName && !MilestoneReached() && StackMatcher(stack))
		{
			QuantityAchieved += stack.StackSize;
			Dirty = true;
		}
	}

	public override void DoCheckPlayerInventory(IPlayerInventoryManager inventoryManager)
	{
		if (!(MatchEventName == "onitemcollected"))
		{
			return;
		}
		IEnumerable<ItemStack> stacks = (from val in inventoryManager.Inventories.Where(delegate(KeyValuePair<string, IInventory> val)
			{
				string className = val.Value.ClassName;
				return !(className == "creative") && !(className == "ground");
			})
			select val.Value).SelectMany((IInventory inv) => from slot in inv
			where slot?.Itemstack != null && StackMatcher(slot.Itemstack)
			select slot.Itemstack);
		if (stacks.Count() > 0)
		{
			QuantityAchieved = stacks.Sum((ItemStack stack) => stack.StackSize);
			Dirty = true;
		}
	}
}
