using System;
using System.Collections.Generic;

namespace Vintagestory.API.Common;

public interface IRecipeBase : IByteSerializable, ICloneable
{
	int RecipeId { get; set; }

	AssetLocation? Name { get; set; }

	bool Enabled { get; set; }

	bool AverageDurability { get; set; }

	string? RequiresTrait { get; set; }

	bool ShowInCreatedBy { get; set; }

	IEnumerable<IRecipeIngredient> RecipeIngredients { get; }

	IRecipeOutput RecipeOutput { get; }

	bool Resolve(IWorldAccessor world, string sourceForErrorLogging);

	void OnParsed(IWorldAccessor world);

	IEnumerable<IRecipeBase> GenerateRecipesForAllIngredientCombinations(IWorldAccessor world);

	IRecipeBase CloneAsInterface()
	{
		return (IRecipeBase)Clone();
	}
}
