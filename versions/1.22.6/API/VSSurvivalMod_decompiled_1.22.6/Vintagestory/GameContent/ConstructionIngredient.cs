using Vintagestory.API.Common;

namespace Vintagestory.GameContent;

public class ConstructionIngredient : CraftingRecipeIngredient
{
	public string StoreWildCard;

	public override ConstructionIngredient Clone()
	{
		ConstructionIngredient result = new ConstructionIngredient();
		CloneTo(result);
		return result;
	}

	protected override void CloneTo(object cloneTo)
	{
		base.CloneTo(cloneTo);
		if (cloneTo is ConstructionIngredient ingredient)
		{
			ingredient.StoreWildCard = StoreWildCard;
		}
	}
}
