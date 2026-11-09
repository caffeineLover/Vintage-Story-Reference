using System;
using Vintagestory.API;
using Vintagestory.API.Common;

namespace Vintagestory.GameContent;

[DocumentAsJson]
public class ClayFormingRecipe : LayeredVoxelRecipe, IConcreteCloneable<ClayFormingRecipe>, ICloneable
{
	public override int QuantityLayers => 16;

	public override string RecipeCategoryCode => "clay forming";

	public override ClayFormingRecipe Clone()
	{
		ClayFormingRecipe recipe = new ClayFormingRecipe();
		CloneTo(recipe);
		return recipe;
	}
}
