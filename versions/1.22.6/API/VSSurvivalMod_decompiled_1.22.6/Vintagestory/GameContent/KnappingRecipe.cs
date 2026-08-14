using System;
using Vintagestory.API;
using Vintagestory.API.Common;

namespace Vintagestory.GameContent;

[DocumentAsJson]
public class KnappingRecipe : LayeredVoxelRecipe, IConcreteCloneable<KnappingRecipe>, ICloneable
{
	public override int QuantityLayers => 1;

	public override string RecipeCategoryCode => "knapping";

	public override KnappingRecipe Clone()
	{
		KnappingRecipe recipe = new KnappingRecipe();
		CloneTo(recipe);
		return recipe;
	}
}
