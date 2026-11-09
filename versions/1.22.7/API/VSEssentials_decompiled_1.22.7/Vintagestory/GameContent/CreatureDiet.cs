using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Vintagestory.API.Common;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class CreatureDiet
{
	public EnumFoodCategory[] FoodCategories;

	public string[] FoodTags;

	public WeightedFoodTag[] WeightedFoodTags;

	public string[] SkipFoodTags;

	[OnDeserialized]
	private void OnDeserialized(StreamingContext ctx)
	{
		if (FoodTags != null)
		{
			List<WeightedFoodTag> wfoodTags = new List<WeightedFoodTag>(WeightedFoodTags ?? Array.Empty<WeightedFoodTag>());
			string[] foodTags = FoodTags;
			foreach (string tag in foodTags)
			{
				wfoodTags.Add(new WeightedFoodTag
				{
					Code = tag,
					Weight = 1f
				});
			}
			WeightedFoodTags = wfoodTags.ToArray();
		}
	}

	public bool Matches(EnumFoodCategory foodSourceCategory, string[] foodSourceTags, float foodTagMinWeight = 0f)
	{
		if (SkipFoodTags != null && foodSourceTags != null)
		{
			for (int i = 0; i < foodSourceTags.Length; i++)
			{
				if (SkipFoodTags.Contains(foodSourceTags[i]))
				{
					return false;
				}
			}
		}
		if (FoodCategories != null && FoodCategories.Contains(foodSourceCategory))
		{
			return true;
		}
		if (WeightedFoodTags != null && foodSourceTags != null)
		{
			for (int j = 0; j < foodSourceTags.Length; j++)
			{
				for (int k = 0; k < WeightedFoodTags.Length; k++)
				{
					WeightedFoodTag wFoodTag = WeightedFoodTags[k];
					if (wFoodTag.Weight >= foodTagMinWeight && wFoodTag.Code == foodSourceTags[j])
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public bool Matches(ItemStack itemstack, bool checkCategory = true, float foodTagMinWeight = 0f)
	{
		CollectibleObject cobj = itemstack.Collectible;
		EnumFoodCategory foodCat = ((!checkCategory) ? EnumFoodCategory.NoNutrition : (cobj.GetNutritionProperties(null, itemstack, null)?.FoodCategory ?? EnumFoodCategory.NoNutrition));
		string[] foodTags = itemstack.Collectible.GetCollectibleInterface<ICreatureDietFoodTags>()?.GetFoodTags(itemstack);
		if (foodTags == null)
		{
			foodTags = cobj?.Attributes?["foodTags"].AsArray<string>();
		}
		return Matches(foodCat, foodTags, foodTagMinWeight);
	}
}
