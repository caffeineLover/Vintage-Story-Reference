using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Vintagestory.API;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

[DocumentAsJson]
public class CookingRecipe : IByteSerializable
{
	[DocumentAsJson("Required", "", false)]
	public string? Code;

	[DocumentAsJson("Required", "", false)]
	public CookingRecipeIngredient[]? Ingredients;

	[DocumentAsJson("Optional", "True", false)]
	public bool Enabled = true;

	[DocumentAsJson("Required", "", false)]
	public CompositeShape? Shape;

	[DocumentAsJson("Required", "", false)]
	public TransitionableProperties? PerishableProps;

	[DocumentAsJson("Optional", "None", false)]
	public JsonItemStack? CooksInto;

	[DocumentAsJson("Optional", "False", false)]
	public bool IsFood;

	public static Dictionary<string, ICookingRecipeNamingHelper> NamingRegistry = new Dictionary<string, ICookingRecipeNamingHelper>();

	public bool Matches(ItemStack?[] inputStacks)
	{
		int useless = 0;
		return Matches(inputStacks, ref useless);
	}

	public int GetQuantityServings(ItemStack[] stacks)
	{
		int quantity = 0;
		Matches(stacks, ref quantity);
		return quantity;
	}

	public string GetOutputName(IWorldAccessor worldForResolve, ItemStack[] inputStacks)
	{
		if (inputStacks.Any((ItemStack stack) => stack?.Collectible.Code.Path == "rot"))
		{
			return Lang.Get("Rotten Food");
		}
		if (NamingRegistry.TryGetValue(Code, out ICookingRecipeNamingHelper namer))
		{
			return namer.GetNameForIngredients(worldForResolve, Code, inputStacks);
		}
		return new VanillaCookingRecipeNames().GetNameForIngredients(worldForResolve, Code, inputStacks);
	}

	public bool Matches(ItemStack?[] inputStacks, ref int quantityServings)
	{
		if (Ingredients == null)
		{
			return false;
		}
		List<ItemStack> inputStacksList = new List<ItemStack>(inputStacks);
		List<CookingRecipeIngredient> ingredientList = Ingredients.ToList();
		int totalOutputQuantity = 99999;
		int[] curQuantities = new int[ingredientList.Count];
		for (int i = 0; i < curQuantities.Length; i++)
		{
			curQuantities[i] = 0;
		}
		while (inputStacksList.Count > 0)
		{
			ItemStack inputStack = inputStacksList[0];
			inputStacksList.RemoveAt(0);
			if (inputStack == null)
			{
				continue;
			}
			bool found = false;
			for (int j = 0; j < ingredientList.Count; j++)
			{
				CookingRecipeIngredient ingred = ingredientList[j];
				CookingRecipeStack jstack = ingred.GetMatchingStack(inputStack);
				if (jstack != null && curQuantities[j] < ingred.MaxQuantity)
				{
					int stackPortion = inputStack.StackSize / jstack.StackSize;
					WaterTightContainableProps props = BlockLiquidContainerBase.GetContainableProps(inputStack);
					if (props != null)
					{
						stackPortion = (int)((float)(inputStack.StackSize / jstack.StackSize) / props.ItemsPerLitre / ingred.PortionSizeLitres);
					}
					totalOutputQuantity = Math.Min(totalOutputQuantity, stackPortion);
					curQuantities[j]++;
					found = true;
					break;
				}
			}
			if (!found)
			{
				return false;
			}
		}
		for (int k = 0; k < ingredientList.Count; k++)
		{
			if (curQuantities[k] < ingredientList[k].MinQuantity)
			{
				return false;
			}
		}
		quantityServings = totalOutputQuantity;
		foreach (ItemStack stack in inputStacks)
		{
			if (stack == null)
			{
				continue;
			}
			int jStackSize = GetIngrendientFor(stack)?.GetMatchingStack(stack)?.StackSize ?? 1;
			WaterTightContainableProps props2 = BlockLiquidContainerBase.GetContainableProps(stack);
			if (props2 != null)
			{
				if (stack.StackSize / jStackSize != (int)((float)quantityServings * props2.ItemsPerLitre * (GetIngrendientFor(stack)?.PortionSizeLitres ?? 100f)))
				{
					quantityServings = -1;
				}
			}
			else if (stack.StackSize / jStackSize != quantityServings)
			{
				quantityServings = -1;
			}
			if (quantityServings == -1)
			{
				return false;
			}
		}
		return true;
	}

	public CookingRecipeIngredient? GetIngrendientFor(ItemStack? stack, params CookingRecipeIngredient[] ingredsToskip)
	{
		if (stack == null)
		{
			return null;
		}
		for (int i = 0; i < Ingredients.Length; i++)
		{
			if (Ingredients[i].Matches(stack) && !ingredsToskip.Contains<CookingRecipeIngredient>(Ingredients[i]))
			{
				return Ingredients[i];
			}
		}
		return null;
	}

	public void Resolve(IServerWorldAccessor world, string sourceForErrorLogging)
	{
		if (Ingredients != null)
		{
			for (int i = 0; i < Ingredients.Length; i++)
			{
				Ingredients[i].Resolve(world, sourceForErrorLogging);
			}
			CooksInto?.Resolve(world, sourceForErrorLogging);
		}
	}

	public ItemStack?[] GenerateRandomMeal(ICoreAPI api, ref Dictionary<CookingRecipeIngredient, HashSet<ItemStack?>>? cachedValidStacksByIngredient, ItemStack[] allstacks, int slots = 4, ItemStack? ingredientStack = null)
	{
		if (Ingredients == null)
		{
			return new ItemStack[slots];
		}
		Dictionary<CookingRecipeIngredient, HashSet<ItemStack>> validStacksByIngredient = cachedValidStacksByIngredient;
		if (validStacksByIngredient == null)
		{
			validStacksByIngredient = new Dictionary<CookingRecipeIngredient, HashSet<ItemStack>>();
			CookingRecipeIngredient[] ingredients = Ingredients;
			foreach (CookingRecipeIngredient ingredient in ingredients)
			{
				HashSet<ItemStack> ingredientStacks = new HashSet<ItemStack>();
				ingredient.Resolve(api.World, "handbook meal recipes");
				foreach (ItemStack astack in allstacks)
				{
					CookingRecipeStack vstack = ingredient.GetMatchingStack(astack);
					if (vstack != null)
					{
						ItemStack stack = astack.Clone();
						stack.StackSize = vstack.StackSize;
						WaterTightContainableProps props = BlockLiquidContainerBase.GetContainableProps(stack);
						if (props != null)
						{
							stack.StackSize *= (int)(props.ItemsPerLitre * ingredient.PortionSizeLitres);
						}
						ingredientStacks.Add(stack);
					}
				}
				if (ingredient.MinQuantity <= 0)
				{
					ingredientStacks.Add(null);
				}
				validStacksByIngredient.Add(ingredient.Clone(), ingredientStacks);
			}
			cachedValidStacksByIngredient = validStacksByIngredient;
		}
		if (validStacksByIngredient == null)
		{
			return new ItemStack[slots];
		}
		List<ItemStack> randomMeal = new List<ItemStack>();
		while (!Matches(randomMeal.ToArray()))
		{
			Dictionary<CookingRecipeIngredient, List<ItemStack>> valIngStacks = new Dictionary<CookingRecipeIngredient, List<ItemStack>>();
			foreach (KeyValuePair<CookingRecipeIngredient, HashSet<ItemStack>> entry in validStacksByIngredient)
			{
				valIngStacks.Add(entry.Key.Clone(), entry.Value.ToList());
			}
			valIngStacks = valIngStacks.OrderBy((KeyValuePair<CookingRecipeIngredient, List<ItemStack>> x) => api.World.Rand.Next()).ToDictionary((KeyValuePair<CookingRecipeIngredient, List<ItemStack>> item) => item.Key, (KeyValuePair<CookingRecipeIngredient, List<ItemStack>> item) => item.Value);
			CookingRecipeIngredient requestedIngredient = null;
			if (ingredientStack != null)
			{
				List<CookingRecipeIngredient> validIngredients = Ingredients.Where((CookingRecipeIngredient cookingRecipeIngredient) => cookingRecipeIngredient.Matches(ingredientStack)).ToList();
				requestedIngredient = validIngredients[api.World.Rand.Next(validIngredients.Count)].Clone();
			}
			randomMeal = new List<ItemStack>();
			foreach (KeyValuePair<CookingRecipeIngredient, List<ItemStack>> entry2 in valIngStacks.Where((KeyValuePair<CookingRecipeIngredient, List<ItemStack>> keyValuePair) => keyValuePair.Key.MinQuantity > 0))
			{
				CookingRecipeIngredient ingredient2 = entry2.Key;
				List<ItemStack> validStacks = entry2.Value;
				if (ingredient2.Code == requestedIngredient?.Code)
				{
					ItemStack stack2 = validStacks.First((ItemStack itemStack) => itemStack?.Collectible.Code == ingredientStack?.Collectible.Code);
					if (stack2 != null)
					{
						randomMeal.Add(stack2.Clone());
						ingredient2.MinQuantity--;
						ingredient2.MaxQuantity--;
					}
					requestedIngredient = null;
				}
				while (ingredient2.MinQuantity > 0)
				{
					randomMeal.Add(validStacks[api.World.Rand.Next(validStacks.Count)]?.Clone());
					ingredient2.MinQuantity--;
					ingredient2.MaxQuantity--;
				}
				if (ingredient2.MaxQuantity > 0)
				{
					validStacks.Add(null);
				}
				else
				{
					valIngStacks.Remove(ingredient2);
				}
			}
			int tries = slots - randomMeal.Count;
			int requestedTry = 0;
			if (requestedIngredient != null)
			{
				requestedTry = api.World.Rand.Next(tries) + 1;
			}
			while (tries > 0)
			{
				if (api.World.Rand.NextDouble() > 0.25 || tries == requestedTry)
				{
					valIngStacks = valIngStacks.OrderBy((KeyValuePair<CookingRecipeIngredient, List<ItemStack>> x) => api.World.Rand.Next()).ToDictionary((KeyValuePair<CookingRecipeIngredient, List<ItemStack>> item) => item.Key, (KeyValuePair<CookingRecipeIngredient, List<ItemStack>> item) => item.Value);
					foreach (KeyValuePair<CookingRecipeIngredient, List<ItemStack>> entry3 in valIngStacks)
					{
						CookingRecipeIngredient ingredient3 = entry3.Key;
						List<ItemStack> validStacks2 = entry3.Value;
						if (tries == requestedTry)
						{
							if (requestedIngredient != null && ingredient3.Code != requestedIngredient?.Code)
							{
								continue;
							}
							if (ingredient3.Code == requestedIngredient?.Code)
							{
								ItemStack stack3 = validStacks2.First((ItemStack itemStack) => itemStack?.Collectible.Code == ingredientStack?.Collectible.Code);
								if (stack3 != null)
								{
									randomMeal.Add(stack3.Clone());
									ingredient3.MaxQuantity--;
									requestedIngredient = null;
									break;
								}
							}
						}
						if (ingredient3.MaxQuantity > 0 && api.World.Rand.NextDouble() < 0.5)
						{
							ItemStack stack4 = validStacks2[api.World.Rand.Next(validStacks2.Count)];
							if (stack4 != null)
							{
								randomMeal.Add(stack4.Clone());
								ingredient3.MaxQuantity--;
								break;
							}
						}
					}
				}
				tries--;
			}
		}
		randomMeal.Shuffle(api.World.Rand);
		while (randomMeal.Count < slots)
		{
			randomMeal.Add(null);
		}
		return randomMeal.ToArray();
	}

	public void ToBytes(BinaryWriter writer)
	{
		writer.Write(Code);
		writer.Write(Ingredients.Length);
		for (int i = 0; i < Ingredients.Length; i++)
		{
			Ingredients[i].ToBytes(writer);
		}
		writer.Write(Shape == null);
		if (Shape != null)
		{
			writer.Write(Shape.Base.ToString());
		}
		PerishableProps.ToBytes(writer);
		writer.Write(CooksInto != null);
		if (CooksInto != null)
		{
			CooksInto.ToBytes(writer);
		}
		writer.Write(IsFood);
	}

	public void FromBytes(BinaryReader reader, IWorldAccessor resolver)
	{
		Code = reader.ReadString();
		Ingredients = new CookingRecipeIngredient[reader.ReadInt32()];
		for (int i = 0; i < Ingredients.Length; i++)
		{
			Ingredients[i] = new CookingRecipeIngredient
			{
				Code = null,
				ValidStacks = null
			};
			Ingredients[i].FromBytes(reader, resolver.ClassRegistry);
			Ingredients[i].Resolve(resolver, "[FromBytes]");
		}
		if (!reader.ReadBoolean())
		{
			Shape = new CompositeShape
			{
				Base = new AssetLocation(reader.ReadString())
			};
		}
		PerishableProps = new TransitionableProperties();
		PerishableProps.FromBytes(reader, resolver.ClassRegistry);
		if (reader.ReadBoolean())
		{
			CooksInto = new JsonItemStack();
			CooksInto.FromBytes(reader, resolver.ClassRegistry);
			CooksInto.Resolve(resolver, "[FromBytes]");
		}
		IsFood = reader.ReadBoolean();
	}
}
