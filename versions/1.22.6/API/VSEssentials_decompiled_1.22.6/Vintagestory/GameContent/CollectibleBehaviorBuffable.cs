using System.Collections.Generic;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;

namespace Vintagestory.GameContent;

public class CollectibleBehaviorBuffable : CollectibleBehavior
{
	public CollectibleBehaviorBuffable(CollectibleObject collObj)
		: base(collObj)
	{
	}

	public override void OnCreatedByCrafting(ItemSlot[] allInputslots, ItemSlot outputSlot, IRecipeBase byRecipe, ref EnumHandling bhHandling)
	{
		JsonObject attr = (byRecipe.RecipeOutput as CraftingRecipeIngredient)?.RecipeAttributes;
		if (attr == null || !attr["applyquenchablebuffs"].AsBool())
		{
			return;
		}
		foreach (ItemSlot slot in allInputslots)
		{
			if (!slot.Empty)
			{
				_ = slot.Itemstack;
				_ = slot.Itemstack.Collectible;
				applyQuenchableBuffs(outputSlot.Itemstack, slot.Itemstack);
			}
		}
	}

	public void applyQuenchableBuffs(ItemStack stack, ItemStack takeBuffsFromStack)
	{
		if (takeBuffsFromStack.Attributes.HasAttribute("powervalue") || takeBuffsFromStack.Attributes.HasAttribute("durationbonus"))
		{
			float powervalue = takeBuffsFromStack.Attributes.GetFloat("powervalue");
			float durationbonus = takeBuffsFromStack.Attributes.GetFloat("durationbonus");
			if (powervalue > 0f)
			{
				AddBuff(stack, new AppliedCollectibleBuff
				{
					Code = "hardened",
					Multiplier = 1f + powervalue,
					StatCode = "attackpower"
				}, EnumBuffAddType.AddOnDuplicate);
				AddBuff(stack, new AppliedCollectibleBuff
				{
					Code = "hardened",
					Multiplier = 1f + powervalue,
					StatCode = "miningspeed"
				}, EnumBuffAddType.AddOnDuplicate);
			}
			if (durationbonus > 0f)
			{
				AddBuff(stack, new AppliedCollectibleBuff
				{
					Code = "hardened",
					Multiplier = 1f + durationbonus,
					StatCode = "maxdurability"
				}, EnumBuffAddType.AddOnDuplicate);
			}
		}
	}

	public List<AppliedCollectibleBuff> GetItemBuffs(ItemStack stack)
	{
		List<AppliedCollectibleBuff> appliedbuffs = new List<AppliedCollectibleBuff>();
		ITreeAttribute buffs = stack.Attributes.GetTreeAttribute("buffs");
		if (buffs == null)
		{
			return appliedbuffs;
		}
		foreach (KeyValuePair<string, IAttribute> item in buffs)
		{
			if (item.Value is TreeAttribute bufftree)
			{
				appliedbuffs.Add(AppliedCollectibleBuff.FromAttribute(bufftree));
			}
		}
		return appliedbuffs;
	}

	public void StoreItemBuffs(ItemStack stack, List<AppliedCollectibleBuff> buffs)
	{
		TreeAttribute buffstree = new TreeAttribute();
		for (int i = 0; i < buffs.Count; i++)
		{
			buffstree[i.ToString()] = buffs[i].ToAttribute();
		}
		stack.Attributes["buffs"] = buffstree;
	}

	public void AddBuff(ItemStack stack, AppliedCollectibleBuff buff, EnumBuffAddType mode = EnumBuffAddType.AddStat)
	{
		ITreeAttribute buffstree = stack.Attributes.GetTreeAttribute("buffs");
		if (buffstree == null)
		{
			buffstree = new TreeAttribute();
			stack.Attributes["buffs"] = buffstree;
		}
		int index = 0;
		while (buffstree.HasAttribute(index.ToString()))
		{
			if (mode == EnumBuffAddType.AddOnDuplicate || mode == EnumBuffAddType.ReplaceOnDuplicate)
			{
				TreeAttribute bufftree = buffstree[index.ToString()] as TreeAttribute;
				if (bufftree?.GetString("code") == buff.Code && bufftree?.GetString("statcode") == buff.StatCode)
				{
					if (mode == EnumBuffAddType.ReplaceOnDuplicate)
					{
						bufftree.SetInt("durability", buff.RemainingDurability);
						bufftree.SetFloat("multiplier", buff.Multiplier);
						bufftree.SetFloat("flatchange", buff.FlatChange);
					}
					else
					{
						bufftree.SetInt("durability", bufftree.GetInt("durability") + buff.RemainingDurability);
						bufftree.SetFloat("multiplier", bufftree.GetFloat("multiplier") + (buff.Multiplier - 1f));
						bufftree.SetFloat("flatchange", bufftree.GetFloat("flatchange") + buff.FlatChange);
					}
					return;
				}
			}
			index++;
		}
		buffstree[index.ToString()] = buff.ToAttribute();
	}

	public void RemoveBuff(ItemStack stack, int index)
	{
		stack.Attributes.GetTreeAttribute("buffs")?.RemoveAttribute(index.ToString());
	}

	public override void OnDamageItem(IWorldAccessor world, Entity byEntity, ItemSlot itemslot, ref int amount, ref EnumHandling bhHandling)
	{
		List<AppliedCollectibleBuff> buffs = GetItemBuffs(itemslot.Itemstack);
		bool modified = false;
		for (int i = 0; i < buffs.Count; i++)
		{
			AppliedCollectibleBuff buff = buffs[i];
			if (buff.RemainingDurability > 0)
			{
				buff.RemainingDurability--;
				modified = true;
				if (buff.RemainingDurability == 0)
				{
					buffs.RemoveAt(i);
					i--;
				}
			}
		}
		if (modified)
		{
			StoreItemBuffs(itemslot.Itemstack, buffs);
			itemslot.MarkDirty();
		}
	}

	public override int GetMaxDurability(ItemStack itemstack, int durability, ref EnumHandling bhHandling)
	{
		return (int)applyBuffs(itemstack, durability, "maxdurability", ref bhHandling);
	}

	public override int GetRemainingDurability(ItemStack itemstack, int durability, ref EnumHandling bhHandling)
	{
		return (int)applyBuffs(itemstack, durability, "durability", ref bhHandling);
	}

	public override float GetAttackPower(ItemStack itemstack, float attackPower, ref EnumHandling bhHandling)
	{
		return applyBuffs(itemstack, attackPower, "attackpower", ref bhHandling);
	}

	public override float GetDamageToEntity(float baseDamage, Entity entity, ItemStack itemStack, ref bool isCriticalHit, ref EnumHandling handling)
	{
		float critChance = applyBuffs(itemStack, 1f, "critchance", ref handling) - 1f;
		if (entity.World.Rand.NextDouble() < (double)critChance)
		{
			baseDamage *= 2f;
			isCriticalHit = true;
		}
		return baseDamage;
	}

	public override float GetMiningSpeedModifier(ItemStack itemstack, ref EnumHandling bhHandling)
	{
		return applyBuffs(itemstack, 1f, "miningspeed", ref bhHandling);
	}

	public override float GetMiningSpeed(ItemStack itemstack, BlockSelection blockSel, Block block, IPlayer forPlayer, ref EnumHandling bhHandling)
	{
		return GetMiningSpeedModifier(itemstack, ref bhHandling);
	}

	public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
	{
		List<AppliedCollectibleBuff> itemBuffs = GetItemBuffs(inSlot.Itemstack);
		if (itemBuffs.Count > 0)
		{
			dsc.AppendLine();
		}
		foreach (AppliedCollectibleBuff buff in itemBuffs)
		{
			bool positiveBuff = buff.Multiplier > 1f || buff.FlatChange > 0f;
			dsc.Append(string.Format("<font color=\"{0}\">", positiveBuff ? "#00bb00" : "#bb0000"));
			dsc.Append(buff.ToString());
			dsc.AppendLine("</font>");
		}
	}

	private float applyBuffs(ItemStack itemstack, float currentvalue, string statcode, ref EnumHandling bhHandling)
	{
		foreach (AppliedCollectibleBuff buff in GetItemBuffs(itemstack))
		{
			if (buff.StatCode == statcode)
			{
				currentvalue *= buff.Multiplier;
				currentvalue += buff.FlatChange;
				bhHandling = EnumHandling.Handled;
			}
		}
		return currentvalue;
	}

	public void ApplyBuffs(ItemStack stack, List<AppliedCollectibleBuff> buffs, EnumBuffAddType mode = EnumBuffAddType.AddStat)
	{
		foreach (AppliedCollectibleBuff buff in buffs)
		{
			AddBuff(stack, buff, mode);
		}
	}
}
