using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;

namespace Vintagestory.API.Common;

/// <summary>
/// Creates a new base recipe type. Almost all recipe types extend from this.
/// </summary>
/// <typeparam name="TRecipe">The resulting recipe type.</typeparam>
[DocumentAsJson]
public abstract class RecipeBase : IRecipeBase, IByteSerializable, ICloneable, IConcreteCloneable<RecipeBase>
{
	public static readonly CollectiblePreSearchResultsCache CollectiblePreSearchResultsCache = new CollectiblePreSearchResultsCache();

	/// <summary>
	/// If set to false, the recipe will never be loaded.
	/// If loaded, you can use this field to disable recipes during runtime.
	/// </summary>
	[DocumentAsJson("Optional", "True", false)]
	public bool Enabled { get; set; } = true;

	/// <summary>
	/// Adds a name to this. Used for logging, and determining helve hammer workability for smithing recipes. Recipes for repairing objects must contain 'repair' in the name.
	/// </summary>
	[DocumentAsJson("Optional", "Asset Path", false)]
	public AssetLocation? Name { get; set; }

	/// <summary>
	/// Optional attribute data that you can attach any data to. Useful for code mods, but also required when using liquid ingredients.<br />
	/// See dough.json grid recipe file for example.
	/// </summary>
	[JsonConverter(typeof(JsonAttributesConverter))]
	[DocumentAsJson("Optional", "None", false)]
	public JsonObject? Attributes { get; set; }

	/// <summary>
	/// If set, only players with given trait can use this  See config/traits.json for a list of traits.
	/// </summary>
	[DocumentAsJson("Optional", "None", false)]
	public string? RequiresTrait { get; set; }

	/// <summary>
	/// If true, the output item will have its durability averaged over the input items
	/// </summary>
	[DocumentAsJson("Optional", "True", false)]
	public bool AverageDurability { get; set; } = true;

	/// <summary>
	/// If set, it will copy over the itemstack attributes from given ingredient code
	/// </summary>
	[DocumentAsJson("Optional", "None", false)]
	public string? CopyAttributesFrom { get; set; }

	/// <summary>
	/// Attributes from these ingredients will be merged into output itemstack
	/// </summary>
	[DocumentAsJson("Optional", "None", false)]
	public string[] MergeAttributesFrom { get; set; } = Array.Empty<string>();

	/// <summary>
	/// If '{code}' is used in ingredient code, allowed variants for this code should be specified in this map
	/// </summary>
	[DocumentAsJson("Optional", "None", false)]
	public Dictionary<string, string[]> AllowedVariants { get; set; } = new Dictionary<string, string[]>();

	/// <summary>
	/// If '{code}' is used in ingredient code, skip variants for this code should be specified in this map
	/// </summary>
	[DocumentAsJson("Optional", "None", false)]
	public Dictionary<string, string[]> SkipVariants { get; set; } = new Dictionary<string, string[]>();

	/// <summary>
	/// Used by the handbook. If false, will not appear in the "Created by" section
	/// </summary>
	[DocumentAsJson("Optional", "True", false)]
	public bool ShowInCreatedBy { get; set; } = true;

	/// <summary>
	/// The ID of the  Automatically generated when the recipe is loaded.
	/// </summary>
	public int RecipeId { get; set; }

	public abstract IEnumerable<IRecipeIngredient> RecipeIngredients { get; }

	public abstract IRecipeOutput RecipeOutput { get; }

	public abstract bool Resolve(IWorldAccessor world, string sourceForErrorLogging);

	public abstract RecipeBase Clone();

	object ICloneable.Clone()
	{
		return Clone();
	}

	public virtual void ToBytes(BinaryWriter writer)
	{
		writer.Write(Name?.ToShortString() ?? "");
		writer.Write(ShowInCreatedBy);
		writer.Write(AverageDurability);
		writer.Write(Attributes == null);
		if (Attributes?.Token != null)
		{
			writer.Write(Attributes.Token.ToString());
		}
		writer.Write(RequiresTrait != null);
		if (RequiresTrait != null)
		{
			writer.Write(RequiresTrait);
		}
		writer.Write(CopyAttributesFrom != null);
		if (CopyAttributesFrom != null)
		{
			writer.Write(CopyAttributesFrom);
		}
		writer.Write(AllowedVariants.Count);
		string key;
		string[] value;
		foreach (KeyValuePair<string, string[]> allowedVariant in AllowedVariants)
		{
			allowedVariant.Deconstruct(out key, out value);
			string key2 = key;
			string[] value2 = value;
			writer.Write(key2);
			writer.Write(value2.Length);
			value = value2;
			foreach (string variant in value)
			{
				writer.Write(variant);
			}
		}
		writer.Write(SkipVariants.Count);
		foreach (KeyValuePair<string, string[]> skipVariant in SkipVariants)
		{
			skipVariant.Deconstruct(out key, out value);
			string key3 = key;
			string[] value3 = value;
			writer.Write(key3);
			writer.Write(value3.Length);
			value = value3;
			foreach (string variant2 in value)
			{
				writer.Write(variant2);
			}
		}
		writer.Write(MergeAttributesFrom.Length);
		value = MergeAttributesFrom;
		foreach (string code in value)
		{
			writer.Write(code);
		}
	}

	public virtual void FromBytes(BinaryReader reader, IWorldAccessor resolver)
	{
		Name = new AssetLocation(reader.ReadString());
		ShowInCreatedBy = reader.ReadBoolean();
		AverageDurability = reader.ReadBoolean();
		if (!reader.ReadBoolean())
		{
			string json = reader.ReadString();
			Attributes = new JsonObject(JToken.Parse(json));
		}
		if (reader.ReadBoolean())
		{
			RequiresTrait = reader.ReadString().DeDuplicate();
		}
		if (reader.ReadBoolean())
		{
			CopyAttributesFrom = reader.ReadString().DeDuplicate();
		}
		int allowedVariantsCount = reader.ReadInt32();
		AllowedVariants = new Dictionary<string, string[]>();
		for (int i = 0; i < allowedVariantsCount; i++)
		{
			string key = reader.ReadString().DeDuplicate();
			int len = reader.ReadInt32();
			string[] variants = new string[len];
			for (int j = 0; j < len; j++)
			{
				variants[j] = reader.ReadString().DeDuplicate();
			}
			AllowedVariants[key] = variants;
		}
		int skipVariantsCount = reader.ReadInt32();
		SkipVariants = new Dictionary<string, string[]>();
		for (int k = 0; k < skipVariantsCount; k++)
		{
			string key2 = reader.ReadString().DeDuplicate();
			int len2 = reader.ReadInt32();
			string[] variants2 = new string[len2];
			for (int l = 0; l < len2; l++)
			{
				variants2[l] = reader.ReadString().DeDuplicate();
			}
			SkipVariants[key2] = variants2;
		}
		int mergedAttributesCount = reader.ReadInt32();
		MergeAttributesFrom = new string[mergedAttributesCount];
		for (int m = 0; m < mergedAttributesCount; m++)
		{
			MergeAttributesFrom[m] = reader.ReadString().DeDuplicate();
		}
	}

	public virtual void OnParsed(IWorldAccessor world)
	{
	}

	public virtual IEnumerable<IRecipeBase> GenerateRecipesForAllIngredientCombinations(IWorldAccessor world)
	{
		Dictionary<string, HashSet<string>> nameToCodeMapping = GetNameToCodeMapping(world);
		if (nameToCodeMapping.Count <= 0)
		{
			return new _003C_003Ez__ReadOnlySingleElementList<IRecipeBase>(this);
		}
		List<RecipeBase> subRecipes = new List<RecipeBase>();
		int variantsCombinations = 1;
		string key;
		HashSet<string> value;
		foreach (KeyValuePair<string, HashSet<string>> item in nameToCodeMapping)
		{
			item.Deconstruct(out key, out value);
			HashSet<string> mapping = value;
			variantsCombinations *= mapping.Count;
		}
		bool first = true;
		int variantCodeIndexDivider = 1;
		foreach (KeyValuePair<string, HashSet<string>> item2 in nameToCodeMapping)
		{
			item2.Deconstruct(out key, out value);
			string variantCode = key;
			HashSet<string> variantsSet = value;
			if (variantsSet.Count == 0)
			{
				continue;
			}
			string[] variants = variantsSet.ToArray();
			for (int i = 0; i < variantsCombinations; i++)
			{
				string currentVariant = variants[i / variantCodeIndexDivider % variants.Length];
				RecipeBase currentRecipe;
				if (first)
				{
					currentRecipe = Clone();
					subRecipes.Add(currentRecipe);
				}
				else
				{
					currentRecipe = subRecipes[i];
				}
				currentRecipe.FillPlaceHolder(variantCode, currentVariant);
			}
			variantCodeIndexDivider *= variants.Length;
			first = false;
		}
		if (RecipeIngredients.Any((IRecipeIngredient ingredient) => ingredient.MatchingType == EnumRecipeMatchType.TagsOnly))
		{
			return subRecipes.SelectMany((RecipeBase recipe) => GenerateRecipesForTagOnlyIngredients(world, recipe));
		}
		return subRecipes;
	}

	protected virtual void FillPlaceHolder(string variantCode, string currentVariant)
	{
		foreach (IRecipeIngredient ingredient in RecipeIngredients)
		{
			FillIngredientPlaceHolders(ingredient, variantCode, currentVariant);
			ingredient.SkipVariants = null;
			ingredient.AllowedVariants = null;
		}
		RecipeOutput.FillPlaceHolder(variantCode, currentVariant);
	}

	public virtual void GenerateOutputStack(ItemSlot[] inputSlots, ItemSlot outputSlot)
	{
		if (RecipeOutput.ResolvedItemStack == null)
		{
			throw new InvalidOperationException($"Missing or errored output result for recipe '{Name}'");
		}
		outputSlot.Itemstack = RecipeOutput.ResolvedItemStack.Clone();
		ItemStack outputStack = outputSlot.Itemstack;
		if (CopyAttributesFrom != null)
		{
			ItemStack inputStack = GetInputStackForIngredientId(CopyAttributesFrom, inputSlots);
			if (inputStack != null)
			{
				ITreeAttribute attr = inputStack.Attributes.Clone();
				attr.MergeTree(outputStack.Attributes);
				outputStack.Attributes = attr;
			}
		}
		if (MergeAttributesFrom.Length != 0)
		{
			string[] mergeAttributesFrom = MergeAttributesFrom;
			foreach (string ingredientId in mergeAttributesFrom)
			{
				ItemStack inputStack2 = GetInputStackForIngredientId(ingredientId, inputSlots);
				if (inputStack2 != null)
				{
					ITreeAttribute attr2 = inputStack2.Attributes.Clone();
					attr2.MergeTree(outputStack.Attributes);
					outputStack.Attributes = attr2;
				}
			}
		}
		outputSlot.Itemstack.Collectible.OnCreatedByCrafting(inputSlots, outputSlot, this);
	}

	public virtual TGridCellElement? GetElementInGrid<TGridCellElement>(int row, int column, TGridCellElement[] cells, int gridWidth)
	{
		int gridHeight = cells.Length / gridWidth;
		if (row < 0 || column < 0 || row >= gridHeight || column >= gridWidth)
		{
			return default(TGridCellElement);
		}
		return cells[row * gridWidth + column];
	}

	public virtual int GetGridIndex<TGridCellElement>(int row, int col, TGridCellElement[] cells, int gridWidth)
	{
		int gridHeight = cells.Length / gridWidth;
		if (row < 0 || col < 0 || row >= gridHeight || col >= gridWidth)
		{
			return -1;
		}
		return row * gridWidth + col;
	}

	public static string ReplaceVariantsToRegex(string value, out List<string> variants, Dictionary<string, string>? allowedVariants = null)
	{
		variants = new List<string>();
		string result = value;
		while (result.Contains('{') && result.Contains('}'))
		{
			int start = result.IndexOf('{');
			int end = result.IndexOf('}');
			string variant = result.Substring(start + 1, end - start - 1);
			result = ((allowedVariants == null || !allowedVariants.TryGetValue(variant, out string variantValue)) ? result.Replace("{" + variant + "}", "@") : result.Replace("{" + variant + "}", variantValue));
			variants.Add(variant);
		}
		result = WildCardToRegex(result);
		result = result.Replace("@", "([\\w-]+)");
		return "^" + result + "$";
	}

	public static string WildCardToRegex(string value)
	{
		return Regex.Escape(value).Replace("\\?", ".").Replace("\\*", ".*");
	}

	protected virtual void CloneTo(object recipe)
	{
		RecipeBase obj = (recipe as RecipeBase) ?? throw new ArgumentException("CloneTo should take object of same class or it subclass");
		obj.Enabled = Enabled;
		obj.Name = Name;
		obj.Attributes = Attributes?.Clone();
		obj.RequiresTrait = RequiresTrait;
		obj.AverageDurability = AverageDurability;
		obj.CopyAttributesFrom = CopyAttributesFrom;
		obj.MergeAttributesFrom = MergeAttributesFrom;
		obj.AllowedVariants = AllowedVariants;
		obj.SkipVariants = SkipVariants;
		obj.ShowInCreatedBy = ShowInCreatedBy;
	}

	public virtual IRecipeIngredient? GetIngredientById(string id)
	{
		return RecipeIngredients.FirstOrDefault((IRecipeIngredient ingredient) => ingredient.Id == id);
	}

	protected virtual Dictionary<string, HashSet<string>> GetNameToCodeMapping(IWorldAccessor world)
	{
		Dictionary<string, HashSet<string>> mappings = new Dictionary<string, HashSet<string>>();
		foreach (IRecipeIngredient ingredient in RecipeIngredients)
		{
			ingredient.MatchingType = IRecipeIngredient.GetMatchType(ingredient.Code?.ToString(), ingredient.Name != null);
			switch (ingredient.MatchingType)
			{
			case EnumRecipeMatchType.NamedWildcard:
				GetNameToCodeMappingForBasicWildcard(world, ingredient, mappings);
				break;
			case EnumRecipeMatchType.AdvancedWildcard:
				GetNameToCodeMappingForAdvancedWildcard(world, ingredient, mappings);
				break;
			}
		}
		return mappings;
	}

	protected virtual ItemStack? GetInputStackForIngredientId(string id, ItemSlot[] inputSlots)
	{
		IRecipeIngredient ingredient = GetIngredientById(id);
		if (ingredient == null)
		{
			return null;
		}
		foreach (ItemSlot slot in inputSlots)
		{
			if (!slot.Empty)
			{
				ItemStack inputStack = slot.Itemstack;
				if (inputStack != null && ingredient.SatisfiesAsIngredient(inputStack) && inputStack.Collectible.MatchesForCrafting(inputStack, this, ingredient))
				{
					return inputStack;
				}
			}
		}
		return null;
	}

	protected virtual void GetNameToCodeMappingForBasicWildcard(IWorldAccessor world, IRecipeIngredient ingredient, Dictionary<string, HashSet<string>> mappings)
	{
		List<CollectibleObject> collectibles = CollectiblePreSearchResultsCache.GetOrCreate(world, ingredient.Code, ingredient.Type);
		if (collectibles.Count == 0)
		{
			world.Logger.Warning("Crafting recipe {0} {1} does not resolve to any collectible. Grid recipe {2} will never match.", ingredient.Type, ingredient.Code, Name);
		}
		HashSet<string> codes = new HashSet<string>();
		int wildcardStartLen = ingredient.Code.Path.IndexOf('*');
		int wildcardEndLen = ingredient.Code.Path.Length - wildcardStartLen - 1;
		bool hasTagsFilter = !ingredient.Tags.IsEmpty;
		foreach (CollectibleObject collectible in collectibles)
		{
			AssetLocation code = collectible.Code;
			if (!(code == null) && (ingredient.SkipVariants == null || !WildcardUtil.MatchesVariants(ingredient.Code, code, ingredient.SkipVariants)) && WildcardUtil.Match(ingredient.Code, code, ingredient.AllowedVariants) && (!hasTagsFilter || ingredient.Tags.Matches(in collectible.Tags)))
			{
				string path = code.Path;
				int num = wildcardStartLen;
				path = path.Substring(num, path.Length - num);
				num = wildcardEndLen;
				string codePart = path.Substring(0, path.Length - num).DeDuplicate();
				codes.Add(codePart);
			}
		}
		if (ingredient.Name == null)
		{
			string path = (ingredient.Name = ingredient.Code);
		}
		mappings[ingredient.Name] = codes;
	}

	protected virtual void GetNameToCodeMappingForAdvancedWildcard(IWorldAccessor world, IRecipeIngredient ingredient, Dictionary<string, HashSet<string>> mappings)
	{
		IEnumerable<CollectibleObject> collectibles = CollectiblePreSearchResultsCache.GetOrCreate(world, ingredient.Code, ingredient.Type);
		List<Dictionary<string, string>> allowedVariantsCombinations = GenerateAllowedVariantsCombinations(AllowedVariants);
		if (allowedVariantsCombinations.Count > 0)
		{
			foreach (Dictionary<string, string> combination in allowedVariantsCombinations)
			{
				Dictionary<string, HashSet<string>> variantValues = GetVariantValuesFromAdvancedWildcard(ingredient, collectibles, combination);
				AddVariantValuesToMapping(mappings, variantValues);
			}
			return;
		}
		Dictionary<string, HashSet<string>> variantValues2 = GetVariantValuesFromAdvancedWildcard(ingredient, collectibles, new Dictionary<string, string>());
		AddVariantValuesToMapping(mappings, variantValues2);
	}

	protected virtual Dictionary<string, HashSet<string>> GetVariantValuesFromAdvancedWildcard(IRecipeIngredient ingredient, IEnumerable<CollectibleObject> collectibles, Dictionary<string, string> allowedVariantsCombination)
	{
		Dictionary<string, HashSet<string>> variantValues = new Dictionary<string, HashSet<string>>();
		List<string> variants;
		Regex regex = new Regex(ReplaceVariantsToRegex(ingredient.Code.Path, out variants, allowedVariantsCombination));
		bool hasTagsFilter = !ingredient.Tags.IsEmpty;
		foreach (CollectibleObject collectible in collectibles.Where((CollectibleObject collectibleObject) => WildcardUtil.Match(ingredient.Code.Domain, collectibleObject.Code.Domain)))
		{
			Match match = regex.Match(collectible.Code.Path);
			if (match.Success && (!hasTagsFilter || ingredient.Tags.Matches(in collectible.Tags)))
			{
				AddVariantCombination(variantValues, allowedVariantsCombination);
				CollectVariantValues(match, variants, variantValues);
			}
		}
		return variantValues;
	}

	protected virtual void AddVariantCombination(Dictionary<string, HashSet<string>> variantValues, Dictionary<string, string> combination)
	{
		foreach (var (variant, variantValue) in combination)
		{
			if (!variantValues.ContainsKey(variant))
			{
				variantValues[variant] = new HashSet<string>();
			}
			variantValues[variant].Add(variantValue);
		}
	}

	protected virtual List<Dictionary<string, string>> GenerateAllowedVariantsCombinations(Dictionary<string, string[]> allowedVariants)
	{
		if (allowedVariants.Count == 0)
		{
			return new List<Dictionary<string, string>>();
		}
		string[] variants = allowedVariants.Keys.ToArray();
		List<Dictionary<string, string>> combinations = new List<Dictionary<string, string>>();
		GenerateAllowedVariantsCombinationsRecursive(0, variants, Array.Empty<(string, string)>(), allowedVariants, combinations);
		return combinations;
	}

	protected virtual void GenerateAllowedVariantsCombinationsRecursive(int index, string[] variants, IEnumerable<(string variant, string value)> combination, Dictionary<string, string[]> allowedVariants, List<Dictionary<string, string>> combinations)
	{
		if (index >= variants.Length)
		{
			combinations.Add(combination.ToDictionary<(string, string), string, string>(((string variant, string value) entry) => entry.variant, ((string variant, string value) entry) => entry.value));
			return;
		}
		string currentVariant = variants[index];
		string[] array = allowedVariants[currentVariant];
		foreach (string variantValue in array)
		{
			IEnumerable<(string, string)> newCombination = combination.Append((currentVariant, variantValue));
			GenerateAllowedVariantsCombinationsRecursive(index + 1, variants, newCombination, allowedVariants, combinations);
		}
	}

	protected virtual void CollectVariantValues(Match match, List<string> variants, Dictionary<string, HashSet<string>> variantCodes)
	{
		for (int variantIndex = 0; variantIndex < variants.Count && variantIndex + 1 < match.Groups.Count; variantIndex++)
		{
			string variant = match.Groups[variantIndex + 1].Value;
			string variantCode = variants[variantIndex];
			if ((!AllowedVariants.ContainsKey(variantCode) || AllowedVariants[variantCode].Contains<string>(variant)) && (!SkipVariants.ContainsKey(variantCode) || !SkipVariants[variantCode].Contains<string>(variant)))
			{
				if (!variantCodes.ContainsKey(variantCode))
				{
					variantCodes[variantCode] = new HashSet<string>();
				}
				variantCodes[variantCode].Add(variant);
			}
		}
	}

	protected virtual void AddVariantValuesToMapping(Dictionary<string, HashSet<string>> mappings, Dictionary<string, HashSet<string>> variantValues)
	{
		foreach (var (variantCode, variantsList) in variantValues)
		{
			if (mappings.TryGetValue(variantCode, out HashSet<string> mapping))
			{
				foreach (string variant in variantsList)
				{
					mapping.Add(variant);
				}
			}
			else
			{
				mappings[variantCode] = variantsList;
			}
		}
	}

	protected virtual void FillIngredientPlaceHolders(IRecipeIngredient ingredient, string variantCode, string variantValue)
	{
		switch (ingredient.MatchingType)
		{
		case EnumRecipeMatchType.NamedWildcard:
			if (ingredient.Name == variantCode)
			{
				ingredient.FillPlaceHolder(variantCode, variantValue);
				ingredient.Code.Path = ingredient.Code.Path.Replace("*", variantValue);
				ingredient.MatchingType = IRecipeIngredient.GetMatchType(ingredient.Code.ToString());
			}
			break;
		case EnumRecipeMatchType.AdvancedWildcard:
			ingredient.FillPlaceHolder(variantCode, variantValue);
			ingredient.MatchingType = IRecipeIngredient.GetMatchType(ingredient.Code.ToString());
			break;
		}
		if (ingredient.ReturnedStack?.Code != null)
		{
			ingredient.ReturnedStack.Code.Path = ingredient.ReturnedStack.Code.Path.Replace("{" + variantCode + "}", variantValue);
		}
	}

	protected virtual bool MatchesAtPosition(ItemSlot[] inputSlots, IRecipeIngredient?[] ingredients)
	{
		int width = inputSlots.Length;
		return MatchesAtPosition(0, 0, inputSlots, width, width, ingredients);
	}

	protected virtual bool MatchesAtPosition(int colStart, int rowStart, ItemSlot[] inputSlots, int gridWidth, int recipeWidth, IRecipeIngredient?[] ingredients)
	{
		int gridHeight = inputSlots.Length / gridWidth;
		for (int col = 0; col < gridWidth; col++)
		{
			for (int row = 0; row < gridHeight; row++)
			{
				ItemStack inputStack = GetElementInGrid(row, col, inputSlots, gridWidth)?.Itemstack;
				IRecipeIngredient ingredient = GetElementInGrid(row - rowStart, col - colStart, ingredients, recipeWidth);
				if (!MatchStackToIngredient(inputStack, ingredient))
				{
					return false;
				}
			}
		}
		return true;
	}

	protected virtual bool MatchStackToIngredient(ItemStack? inputStack, IRecipeIngredient? ingredient)
	{
		if (inputStack != null && ingredient != null)
		{
			if (!ingredient.SatisfiesAsIngredient(inputStack))
			{
				return false;
			}
			if (!inputStack.Collectible.MatchesForCrafting(inputStack, this, ingredient))
			{
				return false;
			}
		}
		else if (inputStack != null || ingredient != null)
		{
			return false;
		}
		return true;
	}

	protected virtual bool ConsumeInputShapeLess(IPlayer byPlayer, ItemSlot[] inputSlots, IRecipeIngredient?[] ingredients)
	{
		List<IRecipeIngredient> exactMatchIngredients = new List<IRecipeIngredient>();
		List<IRecipeIngredient> wildcardAndToolIngredients = new List<IRecipeIngredient>();
		SeparateAndMergeIngredients(ingredients, exactMatchIngredients, wildcardAndToolIngredients);
		foreach (var (inputStack, inputSlot) in inputSlots.Select((ItemSlot slot) => (Itemstack: slot.Itemstack, slot: slot)))
		{
			if (inputStack != null)
			{
				ConsumeExactMatchIngredient(inputStack, inputSlot, inputSlots, exactMatchIngredients, byPlayer);
				ConsumeWildcardMatchIngredient(inputStack, inputSlot, inputSlots, wildcardAndToolIngredients, byPlayer);
			}
		}
		return exactMatchIngredients.Count == 0;
	}

	protected virtual bool ConsumeInputAt(IPlayer byPlayer, ItemSlot[] inputSlots, IRecipeIngredient?[] ingredients)
	{
		int width = inputSlots.Length;
		return ConsumeInputAt(byPlayer, inputSlots, width, width, 0, 0, ingredients);
	}

	protected virtual bool ConsumeInputAt(IPlayer byPlayer, ItemSlot[] inputSlots, int gridWidth, int recipeWidth, int colStart, int rowStart, IRecipeIngredient?[] ingredients)
	{
		int gridHeight = inputSlots.Length / gridWidth;
		for (int col = 0; col < gridWidth; col++)
		{
			for (int row = 0; row < gridHeight; row++)
			{
				ItemSlot slot = GetElementInGrid(row, col, inputSlots, gridWidth);
				IRecipeIngredient ingredient = GetElementInGrid(row - rowStart, col - colStart, ingredients, recipeWidth);
				if (ingredient != null && (ingredient.ResolvedItemStack != null || ingredient.MatchingType != EnumRecipeMatchType.Exact))
				{
					if (slot?.Itemstack == null)
					{
						return false;
					}
					int quantity = ((ingredient.MatchingType != EnumRecipeMatchType.Exact) ? ingredient.Quantity : (ingredient.ResolvedItemStack?.StackSize ?? 0));
					slot.Itemstack.Collectible.OnConsumedByCrafting(inputSlots, slot, this, ingredient, byPlayer, quantity);
				}
			}
		}
		return true;
	}

	protected virtual bool MatchesShapeLess(ItemSlot[] suppliedSlots, IWorldAccessor world, IRecipeIngredient?[] ingredients)
	{
		List<ItemStack> suppliedStacks = new List<ItemStack>();
		MergeStacks(suppliedSlots, suppliedStacks);
		if (!MatchWildcardIngredients(suppliedStacks, ingredients))
		{
			return false;
		}
		List<(ItemStack, IRecipeIngredient)> ingredientStacks = new List<(ItemStack, IRecipeIngredient)>();
		MergeIngredientStacks(suppliedSlots, ingredientStacks, ingredients, world);
		if (ingredientStacks.Count != suppliedStacks.Count)
		{
			return false;
		}
		return MatchIngredientStacks(ingredientStacks, suppliedStacks);
	}

	protected virtual void MergeStacks(ItemSlot[] slots, List<ItemStack> stacks)
	{
		foreach (ItemStack suppliedStack in slots.Select((ItemSlot slot) => slot.Itemstack).OfType<ItemStack>())
		{
			ItemStack similarStack = stacks.Find((ItemStack stack) => stack.Satisfies(suppliedStack));
			if (similarStack != null)
			{
				similarStack.StackSize += suppliedStack.StackSize;
			}
			else
			{
				stacks.Add(suppliedStack.Clone());
			}
		}
	}

	protected virtual void MergeIngredientStacks(ItemSlot[] slots, List<(ItemStack stack, IRecipeIngredient ingredient)> stacks, IRecipeIngredient?[] ingredients, IWorldAccessor world)
	{
		foreach (IRecipeIngredient ingredient in ingredients)
		{
			if (ingredient == null || ingredient.MatchingType != EnumRecipeMatchType.Exact)
			{
				continue;
			}
			ItemStack ingredientStack = ingredient.ResolvedItemStack;
			if (ingredientStack == null)
			{
				continue;
			}
			ItemStack similarStack = null;
			foreach (var entry in stacks)
			{
				if (entry.stack.Equals(world, ingredientStack, GlobalConstants.IgnoredStackAttributes) && ingredient.RecipeAttributes == null)
				{
					(similarStack, _) = entry;
					break;
				}
			}
			if (similarStack != null)
			{
				similarStack.StackSize += ingredientStack.StackSize;
			}
			else
			{
				stacks.Add((ingredientStack.Clone(), ingredient));
			}
		}
	}

	protected virtual bool MatchWildcardIngredients(List<ItemStack> suppliedStacks, IRecipeIngredient?[] ingredients)
	{
		foreach (IRecipeIngredient ingredient in from recipeIngredient in ingredients.OfType<IRecipeIngredient>()
			where recipeIngredient.MatchingType != EnumRecipeMatchType.Exact
			select recipeIngredient)
		{
			bool found = false;
			int j = 0;
			while (!found && j < suppliedStacks.Count)
			{
				ItemStack inputStack = suppliedStacks[j];
				found = ingredient.Type == inputStack.Class && WildcardUtil.Match(ingredient.Code, inputStack.Collectible.Code, ingredient.AllowedVariants) && inputStack.StackSize >= ingredient.Quantity && ingredient.Tags.Matches(in inputStack.Collectible.Tags);
				found &= inputStack.Collectible.MatchesForCrafting(inputStack, this, ingredient);
				j++;
			}
			if (!found)
			{
				return false;
			}
			suppliedStacks.RemoveAt(j - 1);
		}
		return true;
	}

	protected virtual bool MatchIngredientStacks(List<(ItemStack stack, IRecipeIngredient ingredient)> ingredientStacks, List<ItemStack> suppliedStacks)
	{
		bool equals = true;
		foreach (var ingredientStack in ingredientStacks)
		{
			ItemStack stack = ingredientStack.stack;
			IRecipeIngredient ingredient = ingredientStack.ingredient;
			bool found = false;
			int j = 0;
			while (!found && j < suppliedStacks.Count)
			{
				found = stack.Satisfies(suppliedStacks[j]) && stack.StackSize <= suppliedStacks[j].StackSize && suppliedStacks[j].Collectible.MatchesForCrafting(suppliedStacks[j], this, ingredient);
				if (found)
				{
					suppliedStacks.RemoveAt(j);
				}
				j++;
			}
			equals = equals && found;
		}
		return equals;
	}

	protected virtual void SeparateAndMergeIngredients(IRecipeIngredient?[] ingredients, List<IRecipeIngredient> exactMatchIngredients, List<IRecipeIngredient> wildcardAndToolIngredients)
	{
		foreach (IRecipeIngredient ingredient in ingredients.OfType<IRecipeIngredient>())
		{
			if (ingredient.MatchingType != EnumRecipeMatchType.Exact || !ingredient.ConsumeProperties.Consume)
			{
				wildcardAndToolIngredients.Add((IRecipeIngredient)ingredient.Clone());
				continue;
			}
			ItemStack ingredientStack = ingredient.ResolvedItemStack;
			if (ingredientStack != null)
			{
				ItemStack similarIngredientStack = exactMatchIngredients.Select((IRecipeIngredient element) => element.ResolvedItemStack).OfType<ItemStack>().FirstOrDefault((ItemStack element) => element.Satisfies(ingredientStack));
				if (similarIngredientStack != null)
				{
					similarIngredientStack.StackSize += ingredientStack.StackSize;
				}
				else
				{
					exactMatchIngredients.Add((IRecipeIngredient)ingredient.Clone());
				}
			}
		}
	}

	protected virtual void ConsumeExactMatchIngredient(ItemStack inputStack, ItemSlot inputSlot, ItemSlot[] inputSlots, List<IRecipeIngredient> exactMatchIngredients, IPlayer byPlayer)
	{
		IRecipeIngredient ingredient = exactMatchIngredients.Find((IRecipeIngredient recipeIngredient) => recipeIngredient.ResolvedItemStack?.Satisfies(inputStack) ?? false);
		ItemStack ingredientStack = ingredient?.ResolvedItemStack;
		if (ingredientStack != null)
		{
			int stackSize = ingredientStack.StackSize;
			int quantity = Math.Min(stackSize, inputStack.StackSize);
			inputStack.Collectible.OnConsumedByCrafting(inputSlots, inputSlot, this, ingredient, byPlayer, quantity);
			if (stackSize - quantity <= 0)
			{
				exactMatchIngredients.Remove(ingredient);
			}
		}
	}

	protected virtual void ConsumeWildcardMatchIngredient(ItemStack inputStack, ItemSlot inputSlot, ItemSlot[] inputSlots, List<IRecipeIngredient> wildcardAndToolIngredients, IPlayer byPlayer)
	{
		IRecipeIngredient ingredient = wildcardAndToolIngredients.Find((IRecipeIngredient recipeIngredient) => recipeIngredient.Type == inputStack.Class && WildcardUtil.Match(recipeIngredient.Code, inputStack.Collectible.Code, recipeIngredient.AllowedVariants) && recipeIngredient.Tags.Matches(inputStack.Collectible.GetTags(inputStack)));
		if (ingredient == null)
		{
			return;
		}
		int quantity = Math.Min(ingredient.Quantity, inputStack.StackSize);
		inputStack.Collectible.OnConsumedByCrafting(inputSlots, inputSlot, this, ingredient, byPlayer, quantity);
		if (!ingredient.ConsumeProperties.Consume)
		{
			wildcardAndToolIngredients.Remove(ingredient);
			return;
		}
		ingredient.Quantity -= quantity;
		if (ingredient.Quantity <= 0)
		{
			wildcardAndToolIngredients.Remove(ingredient);
		}
	}

	protected virtual IEnumerable<IRecipeBase> GenerateRecipesForTagOnlyIngredients(IWorldAccessor world, IRecipeBase recipe)
	{
		Dictionary<string, List<IRecipeIngredient>> concreteIngredientsMap = new Dictionary<string, List<IRecipeIngredient>>();
		foreach (IRecipeIngredient ingredient in recipe.RecipeIngredients.Where((IRecipeIngredient recipeIngredient) => recipeIngredient.MatchingType == EnumRecipeMatchType.TagsOnly))
		{
			List<IRecipeIngredient> concreteIngredients = FindConcreteIngredientsForTagOnlyIngredient(world, ingredient).ToList();
			concreteIngredientsMap.Add(ingredient.Id, concreteIngredients);
		}
		int totalCombinations = 1;
		foreach (var (id, validIngredients) in concreteIngredientsMap)
		{
			if (validIngredients.Count == 0)
			{
				world.Logger.VerboseDebug("Ingredient {0} on {1} has no valid concrete ingredients, cannot generate permutations.", id, recipe.Name);
				return new _003C_003Ez__ReadOnlySingleElementList<IRecipeBase>(recipe);
			}
			totalCombinations *= validIngredients.Count;
			if (totalCombinations > 10)
			{
				world.Logger.VerboseDebug("Generating permutations for {0} would result in {1} concrete generated recipes, which is above the current threshold of {2}. Will match this recipe during gameplay instead.", recipe.Name, totalCombinations, 10);
				return new _003C_003Ez__ReadOnlySingleElementList<IRecipeBase>(recipe);
			}
		}
		world.Logger.VerboseDebug("Generating permutations for  {0} with {1} virtual ingredients into {2} concrete recipes...", recipe.Name, concreteIngredientsMap.Count, totalCombinations);
		return GeneratePermutations(recipe, concreteIngredientsMap);
		static IEnumerable<IRecipeIngredient> FindConcreteIngredientsForTagOnlyIngredient(IWorldAccessor worldAccessor, IRecipeIngredient ingredientSpecification)
		{
			return (from collectible in worldAccessor.Items
				where collectible?.Code != null
				where ingredientSpecification.Tags.Matches(in collectible.Tags)
				select collectible).Select(delegate(Item collectible)
			{
				IRecipeIngredient obj = (IRecipeIngredient)ingredientSpecification.Clone();
				obj.Code = collectible.Code;
				obj.Type = collectible.ItemClass;
				return obj;
			});
		}
		static IEnumerable<IRecipeBase> GeneratePermutations(IRecipeBase recipeBase, Dictionary<string, List<IRecipeIngredient>> dictionary)
		{
			bool hadTagIngredient = false;
			foreach (IRecipeIngredient tagIngredient in recipeBase.RecipeIngredients.Where((IRecipeIngredient recipeIngredient) => recipeIngredient.MatchingType == EnumRecipeMatchType.TagsOnly))
			{
				hadTagIngredient = true;
				List<IRecipeIngredient> replacements = dictionary[tagIngredient.Id];
				foreach (IRecipeIngredient concreteIngredient in replacements)
				{
					tagIngredient.Code = concreteIngredient.Code;
					tagIngredient.Type = concreteIngredient.Type;
					tagIngredient.MatchingType = EnumRecipeMatchType.Exact;
					IRecipeBase partialRecipe = recipeBase.CloneAsInterface();
					foreach (IRecipeBase item in GeneratePermutations(partialRecipe, dictionary))
					{
						yield return item;
					}
				}
			}
			if (!hadTagIngredient)
			{
				yield return recipeBase;
			}
		}
	}
}
