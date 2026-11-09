using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class ItemStackRandomizer : Item, IResolvableCollectible
{
	private RandomStack[] Stacks;

	private Random rand;

	public override void OnLoaded(ICoreAPI api)
	{
		rand = new Random();
		Stacks = Attributes["stacks"].AsObject<RandomStack[]>();
		float totalchance = 0f;
		for (int i = 0; i < Stacks.Length; i++)
		{
			totalchance += Stacks[i].Chance;
			Stacks[i].Resolve(api.World);
		}
		float scale = 1f / totalchance;
		for (int j = 0; j < Stacks.Length; j++)
		{
			Stacks[j].Chance *= scale;
		}
		base.OnLoaded(api);
	}

	public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handling)
	{
		IPlayer byPlayer = null;
		if (byEntity is EntityPlayer)
		{
			byPlayer = byEntity.World.PlayerByUid(((EntityPlayer)byEntity).PlayerUID);
		}
		if (byPlayer != null)
		{
			TreeAttribute tree = new TreeAttribute();
			tree.SetFloat("totalChance", slot.Itemstack.Attributes.GetFloat("totalChance", 1f));
			tree.SetString("inventoryId", slot.Inventory.InventoryID);
			tree.SetInt("slotId", slot.Inventory.GetSlotId(slot));
			api.Event.PushEvent("OpenStackRandomizerDialog", tree);
		}
	}

	public void Resolve(ItemSlot intoslot, IWorldAccessor worldForResolve, bool resolveImports = true)
	{
		if (!resolveImports)
		{
			return;
		}
		double diceRoll = rand.NextDouble();
		if ((double)intoslot.Itemstack.Attributes.GetFloat("totalChance", 1f) < rand.NextDouble())
		{
			intoslot.Itemstack = null;
			return;
		}
		intoslot.Itemstack = null;
		if (Stacks == null)
		{
			worldForResolve.Logger.Warning("ItemStackRandomizer 'Stacks' was null! Won't resolve into something.");
			return;
		}
		Stacks.Shuffle(rand);
		for (int i = 0; i < Stacks.Length; i++)
		{
			if ((double)Stacks[i].Chance > diceRoll)
			{
				if (Stacks[i].ResolvedStack != null)
				{
					intoslot.Itemstack = Stacks[i].ResolvedStack.Clone();
					intoslot.Itemstack.StackSize = (int)Stacks[i].Quantity.nextFloat(1f, rand);
					break;
				}
			}
			else
			{
				diceRoll -= (double)Stacks[i].Chance;
			}
		}
	}

	public BlockDropItemStack[] GetDropsForHandbook(ItemStack handbookStack, IPlayer forPlayer)
	{
		List<BlockDropItemStack> resolvedDrops = new List<BlockDropItemStack>();
		RandomStack[] stacks = Stacks;
		foreach (RandomStack randomStack in stacks)
		{
			resolvedDrops.Add(new BlockDropItemStack(randomStack.ResolvedStack.Clone()));
		}
		return resolvedDrops.ToArray();
	}

	public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
	{
		base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
		float total = inSlot.Itemstack.Attributes.GetFloat("totalChance", 1f);
		dsc.Append("<font size=\"12\">");
		dsc.AppendLine(Lang.Get("With a {0}% chance, will generate one of the following:", (total * 100f).ToString("0.#")));
		IEnumerable<RandomStack> sortedStacks = (from randomStack in Stacks
			where randomStack.ResolvedStack != null
			orderby randomStack.Chance
			select randomStack).Reverse();
		int i = 0;
		foreach (RandomStack stack in sortedStacks)
		{
			if (stack.Quantity.var == 0f)
			{
				dsc.AppendLine(Lang.Get("stackrandomizer-description-chance-and-quantity-and-name", (stack.Chance * 100f).ToString("0.#"), stack.Quantity.avg, stack.ResolvedStack.GetName()));
			}
			else
			{
				dsc.AppendLine(Lang.Get("stackrandomizer-description-chance-and-minquantity-to-maxquantity-and-name", (stack.Chance * 100f).ToString("0.#"), stack.Quantity.avg - stack.Quantity.var, stack.Quantity.avg + stack.Quantity.var, stack.ResolvedStack.GetName()));
			}
			if (i++ > 50)
			{
				dsc.AppendLine(Lang.Get("stackrandomizer-description-itemamount-more-items-and-check-itemtype", sortedStacks.ToList().Count - i));
				break;
			}
		}
		dsc.Append("</font>");
	}
}
