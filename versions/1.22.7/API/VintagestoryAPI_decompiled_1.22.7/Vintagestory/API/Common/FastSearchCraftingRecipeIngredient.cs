using System;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;

namespace Vintagestory.API.Common;

/// <summary>
/// A simplified version of a CraftingRecipeIngredient, for faster searching and ingredient matching; note this object stores the actual ResolvedItemStack object (if any - there may be none for a Tags based ingredient)
/// </summary>
public class FastSearchCraftingRecipeIngredient : IRecipeIngredientBase, IEquatable<FastSearchCraftingRecipeIngredient>
{
	public EnumItemClass Type { get; set; }

	public AssetLocation? Code { get; set; }

	public ComplexTagCondition<TagSet> Tags { get; set; }

	public string[]? AllowedVariants { get; set; }

	public string[]? SkipVariants { get; set; }

	public EnumRecipeMatchType MatchingType { get; set; }

	public ItemStack? ResolvedItemStack { get; set; }

	public FastSearchCraftingRecipeIngredient(CraftingRecipeIngredient parent)
	{
		Type = parent.Type;
		Code = parent.Code;
		Tags = parent.Tags;
		AllowedVariants = parent.AllowedVariants;
		SkipVariants = parent.SkipVariants;
	}

	public override int GetHashCode()
	{
		if (!(Code == null))
		{
			return Code.Path.GetHashCode();
		}
		return 0;
	}

	public override bool Equals(object? obj)
	{
		if (obj is FastSearchCraftingRecipeIngredient other)
		{
			return Equals(other);
		}
		return false;
	}

	public virtual bool Equals(FastSearchCraftingRecipeIngredient? other)
	{
		if (other == null)
		{
			return false;
		}
		if (!Tags.Equals(other.Tags))
		{
			return false;
		}
		if (Code == null)
		{
			if (other.Code != null)
			{
				return false;
			}
		}
		else if (!Code.Equals(other.Code))
		{
			return false;
		}
		if (!Type.Equals(other.Type))
		{
			return false;
		}
		if (AllowedVariants == null)
		{
			if (other.AllowedVariants != null)
			{
				return false;
			}
		}
		else if (!AllowedVariants.DeepEquals(other.AllowedVariants))
		{
			return false;
		}
		if (SkipVariants == null)
		{
			if (other.SkipVariants != null)
			{
				return false;
			}
		}
		else if (!SkipVariants.DeepEquals(other.SkipVariants))
		{
			return false;
		}
		return true;
	}

	public bool SatisfiesAsIngredient(ItemStack inputStack, bool unused = false)
	{
		if (inputStack == null || inputStack.Collectible == null)
		{
			return false;
		}
		if (MatchingType != EnumRecipeMatchType.Exact)
		{
			if (Type != inputStack.Class)
			{
				return false;
			}
			if (Code != null && !WildcardUtil.Match(Code, inputStack.Collectible.Code, AllowedVariants))
			{
				return false;
			}
			if (SkipVariants != null && WildcardUtil.Match(Code, inputStack.Collectible.Code, SkipVariants))
			{
				return false;
			}
			if (!CraftingRecipeIngredient.CheckTags(Tags, inputStack, inputStack.Collectible))
			{
				return false;
			}
		}
		else
		{
			ItemStack resolvedItemStack = ResolvedItemStack;
			if (resolvedItemStack == null)
			{
				return false;
			}
			if (!resolvedItemStack.Satisfies(inputStack))
			{
				return false;
			}
		}
		return true;
	}
}
