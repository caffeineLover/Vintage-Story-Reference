using System;

namespace Vintagestory.API.Common;

/// <summary>
/// Allows you to make collectibles edible, and adds data about their nutrition.
/// </summary>
/// <example>
/// <code language="json">
///             "nutritionPropsByType": {
///             	"*-flyagaric-*": {
///             		"satiety": 80,
///             		"health": -6.5,
///             		"foodcategory": "Vegetable"
///             	},
///             	"*-earthball-*": {
///             		"satiety": 80,
///             		"health": -8,
///             		"foodcategory": "Vegetable"
///             	},
/// ...
/// </code>
/// </example>
[DocumentAsJson]
public class FoodNutritionProperties
{
	/// <summary>
	/// The category of the food.
	/// </summary>
	[DocumentAsJson("Recommended", "Fruit", false)]
	public EnumFoodCategory FoodCategory;

	/// <summary>
	/// The saturation restored by the food.
	/// </summary>
	[DocumentAsJson("Recommended", "0", false)]
	public float Satiety;

	/// <summary>
	/// The delay before that extra saturation starts to go away.
	/// </summary>
	[DocumentAsJson("Optional", "10", false)]
	public float SaturationLossDelay = 10f;

	/// <summary>
	/// The health restored by the food. Usually actually used to hurt the player with negative values.
	/// </summary>
	[DocumentAsJson("Optional", "0", false)]
	public float Health;

	/// <summary>
	/// When an instance of this collectible is eaten, what item stack should be returned to the player?
	/// Possible example: Eating a 'meat on a stick' item would return a single stick.
	/// (Note: Bowl meals/liquids are specially designed to do this through their attributes and class.)
	/// </summary>
	[DocumentAsJson("Optional", "None", false)]
	public JsonItemStack EatenStack;

	/// <summary>
	/// Obsolete - Please use <see cref="F:Vintagestory.API.Common.FoodNutritionProperties.Satiety" /> instead.
	/// </summary>
	[DocumentAsJson("Obsolete", "", false)]
	[Obsolete("Use Satiety instead.")]
	public float Saturation
	{
		get
		{
			return Satiety;
		}
		set
		{
			Satiety = value;
		}
	}

	/// <summary>
	/// How much eating this will affect the player's intoxication. (0..1)
	/// </summary>
	[DocumentAsJson("Optional", "0", false)]
	public float Intoxication { get; set; }

	/// <summary>
	/// How much eating this will affect the player's psychedelic perception. (0..1)
	/// </summary>
	[DocumentAsJson("Optional", "0", false)]
	public float Psychedelic { get; set; }

	/// <summary>
	/// Duplicates the nutrition properties, which includes cloning the stack that was eaten.
	/// </summary>
	/// <returns></returns>
	public FoodNutritionProperties Clone()
	{
		return new FoodNutritionProperties
		{
			FoodCategory = FoodCategory,
			Satiety = Satiety,
			Health = Health,
			Intoxication = Intoxication,
			Psychedelic = Psychedelic,
			EatenStack = EatenStack?.Clone()
		};
	}
}
