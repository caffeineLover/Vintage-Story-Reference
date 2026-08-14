using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class VanillaCookingRecipeNames : ICookingRecipeNamingHelper
{
	protected enum EnumIngredientNameType
	{
		None,
		InsturmentalCase,
		Topping
	}

	public string GetNameForIngredients(IWorldAccessor worldForResolve, string recipeCode, ItemStack[] stacks)
	{
		Vintagestory.API.Datastructures.OrderedDictionary<ItemStack, int> quantitiesByStack = new Vintagestory.API.Datastructures.OrderedDictionary<ItemStack, int>();
		quantitiesByStack = mergeStacks(worldForResolve, stacks);
		CookingRecipe recipe = worldForResolve.Api.GetCookingRecipe(recipeCode);
		if (recipeCode == null || recipe == null || quantitiesByStack.Count == 0)
		{
			return Lang.Get("unknown");
		}
		return GetNameForMergedIngredients(worldForResolve, recipe, quantitiesByStack);
	}

	protected virtual string GetNameForMergedIngredients(IWorldAccessor worldForResolve, CookingRecipe recipe, Vintagestory.API.Datastructures.OrderedDictionary<ItemStack, int> quantitiesByStack)
	{
		string recipeCode = recipe.Code;
		switch (recipeCode)
		{
		case "soup":
		{
			List<string> BoiledIngredientNames2 = new List<string>();
			List<string> StewedIngredientNames2 = new List<string>();
			CookingRecipeIngredient ingred4 = null;
			ItemStack stockStack = null;
			ItemStack creamStack = null;
			ItemStack mainStack = null;
			string itemName4 = string.Empty;
			int max4 = 0;
			foreach (KeyValuePair<ItemStack, int> val3 in quantitiesByStack)
			{
				if (val3.Key.Collectible.Code.Path.Contains("waterportion"))
				{
					continue;
				}
				ItemStack stack5 = val3.Key;
				ingred4 = recipe.GetIngrendientFor(stack5);
				if (ingred4?.Code == "cream")
				{
					creamStack = stack5;
					continue;
				}
				if (ingred4?.Code == "stock")
				{
					stockStack = stack5;
					continue;
				}
				if (max4 < val3.Value)
				{
					max4 = val3.Value;
					stack5 = mainStack;
					mainStack = val3.Key;
				}
				if (stack5 == null)
				{
					continue;
				}
				itemName4 = ingredientName(stack5, EnumIngredientNameType.InsturmentalCase);
				if (getFoodCat(worldForResolve, stack5, ingred4) == EnumFoodCategory.Vegetable || stack5.Collectible.FirstCodePart().Contains("egg"))
				{
					if (!BoiledIngredientNames2.Contains(itemName4))
					{
						BoiledIngredientNames2.Add(itemName4);
					}
				}
				else if (!StewedIngredientNames2.Contains(itemName4))
				{
					StewedIngredientNames2.Add(itemName4);
				}
			}
			List<string> MainIngredientNames4 = new List<string>();
			string MainIngredientFormat3 = "{0}";
			if (creamStack != null)
			{
				if (stockStack != null)
				{
					itemName4 = getMainIngredientName(stockStack, "soup");
				}
				else if (mainStack != null)
				{
					itemName4 = getMainIngredientName(mainStack, "soup");
				}
				MainIngredientNames4.Add(itemName4);
				MainIngredientNames4.Add(getMainIngredientName(creamStack, "soup", secondary: true));
				MainIngredientFormat3 = "meal-soup-in-cream-format";
			}
			else if (stockStack != null)
			{
				if (mainStack != null)
				{
					itemName4 = getMainIngredientName(mainStack, "soup");
				}
				MainIngredientNames4.Add(itemName4);
				MainIngredientNames4.Add(getMainIngredientName(stockStack, "soup", secondary: true));
				MainIngredientFormat3 = "meal-soup-in-stock-format";
			}
			else if (mainStack != null)
			{
				MainIngredientNames4.Add(getMainIngredientName(mainStack, "soup"));
			}
			string ExtraIngredientsFormat3 = "meal-adds-soup-boiled";
			if (StewedIngredientNames2.Count > 0)
			{
				ExtraIngredientsFormat3 = ((BoiledIngredientNames2.Count <= 0) ? "meal-adds-soup-stewed" : "meal-adds-soup-boiled-and-stewed");
			}
			return Lang.Get(getMaxMealFormat("meal", "soup", max4), getMainIngredientsString(MainIngredientNames4, MainIngredientFormat3), getMealAddsString(ExtraIngredientsFormat3, BoiledIngredientNames2, StewedIngredientNames2)).Trim().UcFirst();
		}
		case "porridge":
		{
			string MealFormat3 = "meal";
			List<string> MainIngredientNames5 = new List<string>();
			List<string> MashedIngredientNames = new List<string>();
			List<string> FreshIngredientNames2 = new List<string>();
			string ToppingName3 = string.Empty;
			string itemName5 = string.Empty;
			int typesOfGrain = quantitiesByStack.Where<KeyValuePair<ItemStack, int>>((KeyValuePair<ItemStack, int> keyValuePair) => recipe.GetIngrendientFor(keyValuePair.Key)?.Code == "grain-base").Count();
			int max5 = 0;
			foreach (KeyValuePair<ItemStack, int> val4 in quantitiesByStack)
			{
				CookingRecipeIngredient ingred5 = recipe.GetIngrendientFor(val4.Key);
				if (ingred5?.Code == "topping")
				{
					ToppingName3 = ingredientName(val4.Key, EnumIngredientNameType.Topping);
					continue;
				}
				if (ingred5?.Code == "grain-base")
				{
					if (typesOfGrain < 3)
					{
						if (MainIngredientNames5.Count < 2)
						{
							itemName5 = getMainIngredientName(val4.Key, recipeCode, MainIngredientNames5.Count > 0);
							if (!MainIngredientNames5.Contains(itemName5))
							{
								MainIngredientNames5.Add(itemName5);
							}
						}
					}
					else
					{
						itemName5 = ingredientName(val4.Key);
						if (!MainIngredientNames5.Contains(itemName5))
						{
							MainIngredientNames5.Add(itemName5);
						}
					}
					max5 += val4.Value;
					continue;
				}
				itemName5 = ingredientName(val4.Key, EnumIngredientNameType.InsturmentalCase);
				if (getFoodCat(worldForResolve, val4.Key, ingred5) == EnumFoodCategory.Vegetable)
				{
					if (!MashedIngredientNames.Contains(itemName5))
					{
						MashedIngredientNames.Add(itemName5);
					}
				}
				else if (!FreshIngredientNames2.Contains(itemName5))
				{
					FreshIngredientNames2.Add(itemName5);
				}
			}
			string ExtraIngredientsFormat4 = "meal-adds-porridge-mashed";
			if (FreshIngredientNames2.Count > 0)
			{
				ExtraIngredientsFormat4 = ((MashedIngredientNames.Count <= 0) ? "meal-adds-porridge-fresh" : "meal-adds-porridge-mashed-and-fresh");
			}
			string MainIngredientFormat4 = "{0}";
			if (MainIngredientNames5.Count == 2)
			{
				MainIngredientFormat4 = "multi-main-ingredients-format";
			}
			MealFormat3 = getMaxMealFormat(MealFormat3, recipeCode, max5);
			MealFormat3 = Lang.Get(MealFormat3, getMainIngredientsString(MainIngredientNames5, MainIngredientFormat4), getMealAddsString(ExtraIngredientsFormat4, MashedIngredientNames, FreshIngredientNames2));
			if (ToppingName3 != string.Empty)
			{
				MealFormat3 = Lang.Get("meal-topping-ingredient-format", ToppingName3, MealFormat3);
			}
			return MealFormat3.Trim().UcFirst();
		}
		case "meatystew":
		case "vegetablestew":
		{
			ItemStack[] requiredStacks2 = new ItemStack[quantitiesByStack.Count];
			int vegetableCount = 0;
			int proteinCount = 0;
			CookingRecipeIngredient[] ingredients = recipe.Ingredients;
			foreach (CookingRecipeIngredient ingred2 in ingredients)
			{
				if (!ingred2.Code.Contains("base"))
				{
					continue;
				}
				for (int l = 0; l < quantitiesByStack.Count; l++)
				{
					ItemStack stack3 = quantitiesByStack.GetKeyAtIndex(l);
					if (ingred2.Matches(stack3) && !requiredStacks2.Contains(stack3))
					{
						requiredStacks2[l] = stack3;
						if (getFoodCat(worldForResolve, stack3, ingred2) == EnumFoodCategory.Vegetable)
						{
							vegetableCount++;
						}
						if (getFoodCat(worldForResolve, stack3, ingred2) == EnumFoodCategory.Protein)
						{
							proteinCount++;
						}
					}
				}
			}
			List<string> MainIngredientNames3 = new List<string>();
			List<string> BoiledIngredientNames = new List<string>();
			List<string> StewedIngredientNames = new List<string>();
			string ToppingName2 = string.Empty;
			string itemName3 = string.Empty;
			EnumFoodCategory primaryCategory = EnumFoodCategory.Protein;
			int max3 = 0;
			if (vegetableCount > proteinCount)
			{
				primaryCategory = EnumFoodCategory.Vegetable;
			}
			for (int m = 0; m < quantitiesByStack.Count; m++)
			{
				ItemStack stack4 = quantitiesByStack.GetKeyAtIndex(m);
				int quantity2 = quantitiesByStack.GetValueAtIndex(m);
				CookingRecipeIngredient ingred3 = recipe.GetIngrendientFor(stack4);
				if (ingred3?.Code == "topping")
				{
					ToppingName2 = ingredientName(stack4, EnumIngredientNameType.Topping);
					continue;
				}
				EnumFoodCategory cat = getFoodCat(worldForResolve, requiredStacks2[m], ingred3);
				bool flag = (uint)(cat - 1) <= 1u;
				if ((flag && quantitiesByStack.Count <= 2) || cat == primaryCategory)
				{
					max3 += quantity2;
					if (MainIngredientNames3.Count < 2)
					{
						itemName3 = getMainIngredientName(stack4, "stew", MainIngredientNames3.Count > 0);
						if (!MainIngredientNames3.Contains(itemName3))
						{
							MainIngredientNames3.Add(itemName3);
						}
						continue;
					}
				}
				itemName3 = ingredientName(stack4, EnumIngredientNameType.InsturmentalCase);
				if (getFoodCat(worldForResolve, stack4, ingred3) == EnumFoodCategory.Vegetable || stack4.Collectible.FirstCodePart().Contains("egg"))
				{
					if (!BoiledIngredientNames.Contains(itemName3))
					{
						BoiledIngredientNames.Add(itemName3);
					}
				}
				else if (!StewedIngredientNames.Contains(itemName3))
				{
					StewedIngredientNames.Add(itemName3);
				}
			}
			string ExtraIngredientsFormat2 = "meal-adds-stew-boiled";
			if (StewedIngredientNames.Count > 0)
			{
				ExtraIngredientsFormat2 = ((BoiledIngredientNames.Count <= 0) ? "meal-adds-stew-stewed" : "meal-adds-stew-boiled-and-stewed");
			}
			string MainIngredientFormat2 = "{0}";
			if (MainIngredientNames3.Count == 2)
			{
				MainIngredientFormat2 = "multi-main-ingredients-format";
			}
			string MealFormat2 = getMaxMealFormat("meal", "stew", max3);
			MealFormat2 = Lang.Get(MealFormat2, getMainIngredientsString(MainIngredientNames3, MainIngredientFormat2), getMealAddsString(ExtraIngredientsFormat2, BoiledIngredientNames, StewedIngredientNames));
			if (ToppingName2 != string.Empty)
			{
				MealFormat2 = Lang.Get("meal-topping-ingredient-format", ToppingName2, MealFormat2);
			}
			return MealFormat2.Trim().UcFirst();
		}
		case "scrambledeggs":
		{
			List<string> MainIngredientNames2 = new List<string>();
			List<string> FreshIngredientNames = new List<string>();
			List<string> MeltedIngredientNames = new List<string>();
			string itemName2 = string.Empty;
			int max2 = 0;
			foreach (KeyValuePair<ItemStack, int> val2 in quantitiesByStack)
			{
				if (recipe.GetIngrendientFor(val2.Key)?.Code == "egg-base")
				{
					itemName2 = getMainIngredientName(val2.Key, recipeCode);
					if (!MainIngredientNames2.Contains(itemName2))
					{
						MainIngredientNames2.Add(itemName2);
					}
					max2 += val2.Value;
					continue;
				}
				itemName2 = ingredientName(val2.Key, EnumIngredientNameType.InsturmentalCase);
				if (val2.Key.Collectible.FirstCodePart() == "cheese")
				{
					if (!MeltedIngredientNames.Contains(itemName2))
					{
						MeltedIngredientNames.Add(itemName2);
					}
				}
				else if (!FreshIngredientNames.Contains(itemName2))
				{
					FreshIngredientNames.Add(itemName2);
				}
			}
			string ExtraIngredientsFormat = "meal-adds-scrambledeggs-fresh";
			if (MeltedIngredientNames.Count > 0)
			{
				ExtraIngredientsFormat = ((FreshIngredientNames.Count <= 0) ? "meal-adds-scrambledeggs-melted" : "meal-adds-scrambledeggs-melted-and-fresh");
			}
			return Lang.Get(getMaxMealFormat("meal", recipeCode, max2), getMainIngredientsString(MainIngredientNames2, "{0}"), getMealAddsString(ExtraIngredientsFormat, MeltedIngredientNames, FreshIngredientNames)).Trim().UcFirst();
		}
		case "jam":
		{
			ItemStack[] fruits = new ItemStack[2];
			int i2 = 0;
			foreach (KeyValuePair<ItemStack, int> val in quantitiesByStack)
			{
				if (recipe.GetIngrendientFor(val.Key)?.Code != "sweetener")
				{
					fruits[i2++] = val.Key;
					if (i2 == 2)
					{
						break;
					}
				}
			}
			if (fruits[0] != null)
			{
				string jamName = fruits[0].Collectible.LastCodePart() + ((fruits[1] != null) ? ("-" + fruits[1].Collectible.LastCodePart() + "-") : "-") + "jam";
				if (Lang.HasTranslation(jamName))
				{
					return Lang.Get(jamName);
				}
				return Lang.Get((fruits[1] != null) ? "mealname-mixedjam" : "mealname-singlejam", getInJamName(fruits[0]), getInJamName(fruits[1]));
			}
			return Lang.Get("unknown");
		}
		default:
		{
			if (Lang.HasTranslation("meal-" + recipeCode))
			{
				return Lang.Get("meal-" + recipeCode);
			}
			ItemStack[] requiredStacks = new ItemStack[quantitiesByStack.Count];
			int requiredCount = 0;
			bool hasBase = false;
			CookingRecipeIngredient[] ingredients = recipe.Ingredients;
			foreach (CookingRecipeIngredient ingred in ingredients)
			{
				bool num = ingred.Code.Contains("base");
				if (num && !hasBase)
				{
					hasBase = true;
					requiredStacks = new ItemStack[quantitiesByStack.Count];
					requiredCount = 0;
				}
				if (!(num && hasBase) && (ingred.MinQuantity <= 0 || hasBase))
				{
					continue;
				}
				for (int j = 0; j < quantitiesByStack.Count; j++)
				{
					ItemStack stack = quantitiesByStack.GetKeyAtIndex(j);
					if (ingred.Matches(quantitiesByStack.GetKeyAtIndex(j)) && !requiredStacks.Contains(stack))
					{
						requiredStacks[j] = stack;
						requiredCount++;
					}
				}
			}
			List<string> MainIngredientNames = new List<string>();
			List<string> ExtraIngredientNames = new List<string>();
			string ToppingName = string.Empty;
			string itemName = string.Empty;
			int max = 0;
			for (int k = 0; k < quantitiesByStack.Count; k++)
			{
				ItemStack stack2 = quantitiesByStack.GetKeyAtIndex(k);
				int quantity = quantitiesByStack.GetValueAtIndex(k);
				if (recipe.GetIngrendientFor(stack2)?.Code == "topping")
				{
					ToppingName = ingredientName(stack2, EnumIngredientNameType.Topping);
				}
				else if (requiredStacks[k] != null)
				{
					if (requiredCount < 3)
					{
						if (MainIngredientNames.Count < 2)
						{
							itemName = getMainIngredientName(stack2, recipeCode, MainIngredientNames.Count > 0);
							if (!MainIngredientNames.Contains(itemName))
							{
								MainIngredientNames.Add(itemName);
							}
						}
					}
					else
					{
						itemName = ingredientName(stack2);
						if (!MainIngredientNames.Contains(itemName))
						{
							MainIngredientNames.Add(itemName);
						}
					}
					max += quantity;
				}
				else
				{
					itemName = ingredientName(stack2, EnumIngredientNameType.InsturmentalCase);
					if (!ExtraIngredientNames.Contains(itemName))
					{
						ExtraIngredientNames.Add(itemName);
					}
				}
			}
			string MainIngredientFormat = "{0}";
			if (MainIngredientNames.Count == 2)
			{
				MainIngredientFormat = "multi-main-ingredients-format";
			}
			string MealFormat = getMaxMealFormat("meal", recipeCode, max);
			MealFormat = Lang.Get(MealFormat, getMainIngredientsString(MainIngredientNames, MainIngredientFormat), getMealAddsString("meal-adds-generic", ExtraIngredientNames));
			if (ToppingName != string.Empty)
			{
				MealFormat = Lang.Get("meal-topping-ingredient-format", ToppingName, MealFormat);
			}
			return MealFormat.Trim().UcFirst();
		}
		}
	}

	protected string getMaxMealFormat(string format, string recipeCode, int max)
	{
		format = max switch
		{
			3 => format + "-hearty-" + recipeCode, 
			4 => format + "-hefty-" + recipeCode, 
			_ => format + "-normal-" + recipeCode, 
		};
		return format;
	}

	protected EnumFoodCategory getFoodCat(IWorldAccessor worldForResolve, ItemStack stack, CookingRecipeIngredient? ingred)
	{
		ItemStack cookedStack = ingred?.GetMatchingStack(stack)?.CookedStack?.ResolvedItemstack;
		return (BlockMeal.GetIngredientStackNutritionProperties(worldForResolve, cookedStack, null) ?? BlockMeal.GetIngredientStackNutritionProperties(worldForResolve, stack, null))?.FoodCategory ?? EnumFoodCategory.Unknown;
	}

	protected string ingredientName(ItemStack stack, EnumIngredientNameType NameType = EnumIngredientNameType.None)
	{
		string code = stack.Collectible.Code?.Domain + ":recipeingredient-" + stack.Class.ToString().ToLowerInvariant() + "-" + stack.Collectible.Code?.Path;
		if (NameType == EnumIngredientNameType.InsturmentalCase)
		{
			code += "-insturmentalcase";
		}
		if (NameType == EnumIngredientNameType.Topping)
		{
			code += "-topping";
		}
		if (Lang.HasTranslation(code))
		{
			return Lang.GetMatching(code);
		}
		code = stack.Collectible.Code?.Domain + ":recipeingredient-" + stack.Class.ToString().ToLowerInvariant() + "-" + stack.Collectible.FirstCodePart();
		if (NameType == EnumIngredientNameType.InsturmentalCase)
		{
			code += "-insturmentalcase";
		}
		if (NameType == EnumIngredientNameType.Topping)
		{
			code += "-topping";
		}
		return Lang.GetMatching(code);
	}

	protected string getMainIngredientName(ItemStack itemstack, string code, bool secondary = false)
	{
		string t = (secondary ? "secondary" : "primary");
		string langcode = $"meal-ingredient-{code}-{t}-{itemstack.Collectible.Code.Path}";
		if (Lang.HasTranslation(langcode))
		{
			return Lang.GetMatching(langcode);
		}
		langcode = $"meal-ingredient-{code}-{t}-{itemstack.Collectible.FirstCodePart()}";
		return Lang.GetMatching(langcode);
	}

	protected string getInJamName(ItemStack fruit)
	{
		if (fruit == null)
		{
			return "";
		}
		string fruitInJam = (fruit.Collectible.Code.Domain + ":" + fruit.Collectible.LastCodePart() + "-in-jam-name").Replace("game:", "");
		if (!Lang.HasTranslation(fruitInJam))
		{
			return fruit.GetName();
		}
		return Lang.Get(fruitInJam);
	}

	protected string getMainIngredientsString(List<string> ingredients, string format, bool list = true)
	{
		if (ingredients.Count == 0)
		{
			return "";
		}
		if (ingredients.Count < 3 || !list)
		{
			object[] args = ingredients.ToArray();
			return Lang.Get(format, args);
		}
		return getMealAddsString(format, ingredients);
	}

	protected string getMealAddsString(string code, List<string> ingredients1, List<string>? ingredients2 = null)
	{
		if (ingredients1.Count == 0)
		{
			if (ingredients2 == null || ingredients2.Count == 0)
			{
				return "";
			}
			ingredients1 = ingredients2.ToList();
			ingredients2 = null;
		}
		object[] array = new object[2];
		string key = $"meal-ingredientlist-{ingredients1?.Count ?? 0}";
		object[] args = ingredients1?.ToArray() ?? new string[1] { "" };
		array[0] = Lang.Get(key, args);
		string key2 = $"meal-ingredientlist-{ingredients2?.Count ?? 0}";
		args = ingredients2?.ToArray() ?? new string[1] { "" };
		array[1] = Lang.Get(key2, args);
		return Lang.Get(code, array);
	}

	protected static Vintagestory.API.Datastructures.OrderedDictionary<ItemStack, int> mergeStacks(IWorldAccessor worldForResolve, ItemStack[] stacks)
	{
		Vintagestory.API.Datastructures.OrderedDictionary<ItemStack, int> dict = new Vintagestory.API.Datastructures.OrderedDictionary<ItemStack, int>();
		List<ItemStack> stackslist = stacks.ToList();
		while (stackslist.Count > 0)
		{
			ItemStack stack = stackslist[0];
			stackslist.RemoveAt(0);
			if (stack == null)
			{
				continue;
			}
			int cnt = 1;
			while (true)
			{
				ItemStack fstack = stackslist.FirstOrDefault((ItemStack ostack) => ostack?.Equals(worldForResolve, stack, GlobalConstants.IgnoredStackAttributes) ?? false);
				if (fstack == null)
				{
					break;
				}
				stackslist.Remove(fstack);
				cnt++;
			}
			dict[stack] = cnt;
		}
		return dict;
	}
}
