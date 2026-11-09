using Vintagestory.API.Datastructures;

namespace Vintagestory.API.Common;

/// <summary>
/// A minimal set of data for ItemStack matching; may include the ResolvedItemStack
/// </summary>
public interface IRecipeIngredientBase
{
	EnumItemClass Type { get; set; }

	AssetLocation? Code { get; set; }

	string[]? AllowedVariants { get; set; }

	string[]? SkipVariants { get; set; }

	ComplexTagCondition<TagSet> Tags { get; set; }

	EnumRecipeMatchType MatchingType { get; set; }

	ItemStack? ResolvedItemStack { get; set; }

	bool SatisfiesAsIngredient(ItemStack inputStack, bool checkStackSize = true);
}
