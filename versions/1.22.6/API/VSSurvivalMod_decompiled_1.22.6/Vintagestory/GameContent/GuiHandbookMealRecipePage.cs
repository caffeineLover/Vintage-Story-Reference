using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class GuiHandbookMealRecipePage : GuiHandbookPage
{
	public CookingRecipe Recipe;

	public string pageCode;

	public string Title;

	private readonly string titleCached;

	private readonly string textCached;

	protected float secondsVisible;

	protected const int TinyPadding = 2;

	protected const int TinyIndent = 2;

	protected const int MarginBottom = 3;

	protected const int SmallPadding = 7;

	protected const int MediumPadding = 14;

	public LoadedTexture? Texture;

	public InventoryBase unspoilableInventory;

	public DummySlot dummySlot;

	private HandbookMealNutritionFacts? cachedNutritionFacts;

	private Dictionary<string, List<ItemStack>> cachedIngredientStacks;

	private bool isPie;

	private int slots;

	private ElementBounds? scissorBounds;

	public override string PageCode => pageCode;

	public override float SearchWeightOffset => 1f;

	public override string CategoryCode => "stack";

	public override bool IsDuplicate => false;

	public GuiHandbookMealRecipePage(ICoreClientAPI capi, CookingRecipe recipe, int slots = 4, bool isPie = false)
	{
		Recipe = recipe;
		pageCode = "handbook-mealrecipe-" + recipe.Code + (isPie ? "-pie" : "");
		this.isPie = isPie;
		this.slots = slots;
		unspoilableInventory = new CreativeInventoryTab(1, "not-used", null);
		if (isPie)
		{
			dummySlot = new DummySlot(new ItemStack(capi.World.BlockAccessor.GetBlock("pie-perfect")), unspoilableInventory);
			dummySlot.Itemstack.Attributes.SetInt("pieSize", 4);
			dummySlot.Itemstack.Attributes.SetString("topCrustType", BlockPie.TopCrustTypes[capi.World.Rand.Next(BlockPie.TopCrustTypes.Length)].Code);
			dummySlot.Itemstack.Attributes.SetInt("bakeLevel", 2);
		}
		else
		{
			dummySlot = new DummySlot(new ItemStack(BlockMeal.RandomMealBowl(capi)), unspoilableInventory);
		}
		Title = Lang.Get(isPie ? ("pie-" + recipe.Code + "-perfect") : ("mealrecipe-name-" + recipe.Code));
		titleCached = (Title + " " + Lang.Get("handbook-mealrecipe-" + (isPie ? "pie" : "meal") + "searchkeywords")).ToSearchFriendly();
		string directionsText = Lang.GetMatchingIfExists("handbook-mealrecipe-directionstext-" + Recipe.Code);
		if (isPie)
		{
			if (directionsText == null)
			{
				directionsText = Lang.GetMatchingIfExists("handbook-mealrecipe-directionstext-" + Recipe.Code + "-pie");
			}
			if (directionsText == null)
			{
				directionsText = Lang.GetMatchingIfExists("handbook-mealrecipe-directionstext-pie");
			}
		}
		if (directionsText == null)
		{
			directionsText = Lang.Get("handbook-mealrecipe-directionstext");
		}
		string notesText = Lang.GetMatchingIfExists("handbook-mealrecipe-notestext-" + Recipe.Code);
		if (isPie)
		{
			if (notesText == null)
			{
				notesText = Lang.GetMatchingIfExists("handbook-mealrecipe-notestext-" + Recipe.Code + "-pie");
			}
			if (notesText == null)
			{
				notesText = Lang.GetMatchingIfExists("handbook-mealrecipe-notestext-pie");
			}
		}
		textCached = (directionsText + "\n" + ((notesText == null) ? "" : notesText)).ToSearchFriendly();
		cachedIngredientStacks = new Dictionary<string, List<ItemStack>>();
	}

	[MemberNotNull(new string[] { "Texture", "scissorBounds" })]
	public void Recompose(ICoreClientAPI capi)
	{
		Texture?.Dispose();
		Texture = new TextTextureUtil(capi).GenTextTexture(Title, CairoFont.WhiteSmallText());
		scissorBounds = ElementBounds.FixedSize(50.0, 50.0);
		scissorBounds.ParentBounds = capi.Gui.WindowBounds;
	}

	public override void RenderListEntryTo(ICoreClientAPI capi, float dt, double x, double y, double cellWidth, double cellHeight)
	{
		float size = (float)GuiElement.scaled(25.0);
		float pad = (float)GuiElement.scaled(10.0);
		IBlockMealContainer mealBlock = dummySlot.Itemstack?.Collectible as IBlockMealContainer;
		if ((secondsVisible -= dt) <= 0f)
		{
			secondsVisible = 1f;
			if (isPie)
			{
				dummySlot.Itemstack?.Attributes.SetString("topCrustType", BlockPie.TopCrustTypes[capi.World.Rand.Next(BlockPie.TopCrustTypes.Length)].Code);
			}
			else
			{
				dummySlot.Itemstack = new ItemStack(BlockMeal.RandomMealBowl(capi));
			}
			Dictionary<CookingRecipeIngredient, HashSet<ItemStack>> cachedValidStacks = ObjectCacheUtil.TryGet<Dictionary<CookingRecipeIngredient, HashSet<ItemStack>>>(capi, "valstacksbying-" + Recipe.Code);
			mealBlock?.SetContents(Recipe.Code, dummySlot.Itemstack, isPie ? BlockPie.GenerateRandomPie(capi, ref cachedValidStacks, Recipe) : Recipe.GenerateRandomMeal(capi, ref cachedValidStacks, ObjectCacheUtil.TryGet<ItemStack[]>(capi, "handbookallstacks"), slots));
		}
		if (Texture == null || scissorBounds == null)
		{
			Recompose(capi);
		}
		scissorBounds.fixedX = ((double)pad + x - (double)(size / 2f)) / (double)RuntimeEnv.GUIScale;
		scissorBounds.fixedY = (y - (double)(size / 2f)) / (double)RuntimeEnv.GUIScale;
		scissorBounds.CalcWorldBounds();
		if (!(scissorBounds.InnerWidth <= 0.0) && !(scissorBounds.InnerHeight <= 0.0))
		{
			capi.Render.PushScissor(scissorBounds, stacking: true);
			capi.Render.RenderItemstackToGui(dummySlot, x + (double)pad + (double)(size / 2f), y + (double)(size / 2f), 100.0, size, -1, shading: true, rotate: false, showStackSize: false);
			capi.Render.PopScissor();
			capi.Render.Render2DTexturePremultipliedAlpha(Texture.TextureId, x + (double)size + GuiElement.scaled(25.0), y + (double)(size / 4f) - GuiElement.scaled(3.0), Texture.Width, Texture.Height);
		}
	}

	public override void Dispose()
	{
		Texture?.Dispose();
		Texture = null;
	}

	public override void ComposePage(GuiComposer detailViewGui, ElementBounds textBounds, ItemStack[] allstacks, ActionConsumable<string> openDetailPageFor)
	{
		RichTextComponentBase[] cmps = GetPageText(detailViewGui.Api, allstacks, openDetailPageFor);
		detailViewGui.AddRichtext(cmps, textBounds, "richtext");
	}

	protected virtual RichTextComponentBase[] GetPageText(ICoreClientAPI capi, ItemStack[] allStacks, ActionConsumable<string> openDetailPageFor)
	{
		List<RichTextComponentBase> components = new List<RichTextComponentBase>();
		addGeneralInfo(capi, allStacks, components);
		addIngredientLists(capi, allStacks, components, openDetailPageFor);
		addCookingDirections(capi, components);
		addRecipeNotes(capi, components);
		return components.ToArray();
	}

	protected void addGeneralInfo(ICoreClientAPI capi, ItemStack[] allStacks, List<RichTextComponentBase> components)
	{
		ItemStack mealBlock = dummySlot.Itemstack.Clone();
		components.Add(new MealstackTextComponent(capi, mealBlock, Recipe, 100.0, EnumFloat.Left, null, slots, isPie)
		{
			PaddingRight = GuiElement.scaled(10.0),
			offX = -8.0
		});
		components.AddRange(VtmlUtil.Richtextify(capi, Title + "\n", CairoFont.WhiteSmallishText()));
		if (capi.Settings.Bool["extendedDebugInfo"])
		{
			CairoFont font = CairoFont.WhiteDetailText();
			font.Color[3] = 0.5;
			components.AddRange(VtmlUtil.Richtextify(capi, "Page code:" + pageCode + "\n", font));
		}
		HandbookMealNutritionFacts nutritionFacts = getNutritionFacts(capi, allStacks, slots);
		float minSat = (float)Math.Round(nutritionFacts.MinSatiety);
		float maxSat = (float)Math.Round(nutritionFacts.MaxSatiety);
		float minHP = nutritionFacts.MinHealth;
		float maxHP = nutritionFacts.MaxHealth;
		string satCategories = "";
		if (nutritionFacts.Categories.Count > 0)
		{
			satCategories = Lang.Get("handbook-mealrecipe-nutritionfacts-satietycategories", string.Join(", ", nutritionFacts.Categories.Select((EnumFoodCategory category) => Lang.Get("foodcategory-" + category.ToString().ToLowerInvariant()))));
		}
		bool num = minSat != 0f || maxSat != 0f;
		bool isHP = minHP > 0f || maxHP > 0f;
		if (num || isHP)
		{
			components.AddRange(VtmlUtil.Richtextify(capi, Lang.Get("handbook-mealrecipe-nutritionfacts") + "\n", CairoFont.WhiteSmallText()));
		}
		if (num)
		{
			components.AddRange(VtmlUtil.Richtextify(capi, Lang.Get("handbook-mealrecipe-nutritionfactsline-satiety", minSat, maxSat, satCategories) + "\n", CairoFont.WhiteDetailText()));
		}
		if (isHP)
		{
			components.AddRange(VtmlUtil.Richtextify(capi, Lang.Get("handbook-mealrecipe-nutritionfactsline-health", ((minHP >= 0f) ? "+" : "") + minHP, ((maxHP >= 0f) ? "+" : "") + maxHP) + "\n", CairoFont.WhiteDetailText()));
		}
	}

	private void addIngredientLists(ICoreClientAPI capi, ItemStack[] allStacks, List<RichTextComponentBase> components, ActionConsumable<string> openDetailPageFor)
	{
		List<CookingRecipeIngredient> ingredients = (isPie ? Recipe.Ingredients.ToList() : CombinedIngredientsList());
		bool haveText = components.Count > 0;
		CollectibleBehaviorHandbookTextAndExtraInfo.AddHeading(components, capi, "handbook-mealrecipe-requiredingredients", ref haveText);
		foreach (CookingRecipeIngredient ingredient in ingredients.Where((CookingRecipeIngredient cookingRecipeIngredient) => cookingRecipeIngredient.MinQuantity > 0))
		{
			List<ItemStack> ingredientStacks = getIngredientStacks(capi, ingredient, allStacks);
			if (ingredientStacks.Count > 0)
			{
				addIngredientHeading(components, capi, ingredient, "handbook-mealrecipe" + (isPie ? "-pie" : "-") + "ingredients", ingredientStacks, openDetailPageFor);
			}
		}
		IEnumerable<CookingRecipeIngredient> enumerable = ingredients.Where((CookingRecipeIngredient cookingRecipeIngredient) => cookingRecipeIngredient.MinQuantity == 0);
		if (enumerable.Count() > 0)
		{
			haveText = components.Count > 0;
			CollectibleBehaviorHandbookTextAndExtraInfo.AddHeading(components, capi, "handbook-mealrecipe-optionalingredients", ref haveText);
		}
		foreach (CookingRecipeIngredient ingredient2 in enumerable)
		{
			List<ItemStack> ingredientStacks2 = getIngredientStacks(capi, ingredient2, allStacks);
			if (ingredientStacks2.Count > 0)
			{
				addIngredientHeading(components, capi, ingredient2, "handbook-mealrecipe" + (isPie ? "-pie" : "-") + "ingredients", ingredientStacks2, openDetailPageFor);
			}
		}
	}

	private void addIngredientHeading(List<RichTextComponentBase> components, ICoreClientAPI capi, CookingRecipeIngredient ingredient, string ingredientHeader, List<ItemStack> ingredientStacks, ActionConsumable<string> openDetailPageFor)
	{
		ClearFloatTextComponent verticalSpace = new ClearFloatTextComponent(capi, 3f);
		int minQuantity = ingredient.MinQuantity;
		int maxQuantity = ingredient.MaxQuantity;
		if (maxQuantity == minQuantity)
		{
			minQuantity = 0;
		}
		ingredientHeader = Lang.Get(ingredientHeader, minQuantity, maxQuantity, Lang.Get("handbook-mealingredient-" + ingredient.TypeName));
		components.Add(verticalSpace);
		CollectibleBehaviorHandbookTextAndExtraInfo.AddSubHeading(components, capi, null, ingredientHeader, null);
		int firstPadding = 2;
		while (ingredientStacks.Count > 0)
		{
			ItemStack istack = ingredientStacks[0];
			ingredientStacks.RemoveAt(0);
			if (istack != null)
			{
				SlideshowItemstackTextComponent comp = new SlideshowItemstackTextComponent(capi, istack, ingredientStacks, 30.0, EnumFloat.Inline, "mealRecipeGroupBy", delegate(ItemStack cs)
				{
					string t = cs.Collectible.GetCollectibleInterface<IHandBookPageCodeProvider>()?.HandbookPageCodeForStack(capi.World, cs) ?? GuiHandbookItemStackPage.PageCodeForStack(cs);
					openDetailPageFor(t);
				});
				comp.PaddingLeft = firstPadding;
				comp.ShowStackSize = true;
				firstPadding = 0;
				components.Add(comp);
			}
		}
		components.Add(new RichTextComponent(capi, "\n", CairoFont.WhiteSmallText()));
	}

	protected void addCookingDirections(ICoreClientAPI capi, List<RichTextComponentBase> components)
	{
		string directionsText = Lang.GetMatchingIfExists("handbook-mealrecipe-directionstext-" + Recipe.Code);
		if (isPie)
		{
			if (directionsText == null)
			{
				directionsText = Lang.GetMatchingIfExists("handbook-mealrecipe-directionstext-" + Recipe.Code + "-pie");
			}
			if (directionsText == null)
			{
				directionsText = Lang.GetMatchingIfExists("handbook-mealrecipe-directionstext-pie");
			}
		}
		if (directionsText == null)
		{
			directionsText = Lang.Get("handbook-mealrecipe-directionstext");
		}
		components.Add(new ClearFloatTextComponent(capi, 14f));
		components.AddRange(VtmlUtil.Richtextify(capi, Lang.Get("handbook-mealrecipe" + (isPie ? "-pie" : "-") + "directions", directionsText) + "\n", CairoFont.WhiteSmallText()));
	}

	protected void addRecipeNotes(ICoreClientAPI capi, List<RichTextComponentBase> components)
	{
		string notesText = Lang.GetMatchingIfExists("handbook-mealrecipe-notestext-" + Recipe.Code);
		if (isPie)
		{
			if (notesText == null)
			{
				notesText = Lang.GetMatchingIfExists("handbook-mealrecipe-notestext-" + Recipe.Code + "-pie");
			}
			if (notesText == null)
			{
				notesText = Lang.GetMatchingIfExists("handbook-mealrecipe-notestext-pie");
			}
		}
		if (notesText != null)
		{
			components.Add(new ClearFloatTextComponent(capi, 14f));
			components.AddRange(VtmlUtil.Richtextify(capi, Lang.Get("handbook-mealrecipe-notes", notesText), CairoFont.WhiteSmallText()));
		}
	}

	private HandbookMealNutritionFacts getNutritionFacts(ICoreClientAPI capi, ItemStack[] allStacks, int slots = 4)
	{
		if (cachedNutritionFacts == null)
		{
			float highestSat;
			float lowestHP;
			float highestHP;
			float lowestSat = (highestSat = (lowestHP = (highestHP = 0f)));
			List<IngredientMinMax> ingredientMinMax = new List<IngredientMinMax>();
			HashSet<EnumFoodCategory> categories = new HashSet<EnumFoodCategory>();
			List<CookingRecipeIngredient> ingredientsToSkip = new List<CookingRecipeIngredient>();
			foreach (CookingRecipeIngredient rIngredient in isPie ? Recipe.Ingredients.ToList() : CombinedIngredientsList())
			{
				CookingRecipeIngredient ingredient = rIngredient.Clone();
				List<ItemStack> ingredientStacks = getIngredientStacks(capi, ingredient, allStacks);
				float ingredientMinHP;
				float ingredientMinSat = (ingredientMinHP = float.MaxValue);
				float ingredientMaxHP;
				float ingredientMaxSat = (ingredientMaxHP = float.MinValue);
				bool skip = true;
				while (ingredientStacks.Count > 0)
				{
					ItemStack istack = ingredientStacks[0];
					ingredientStacks.RemoveAt(0);
					if (istack == null)
					{
						continue;
					}
					CookingRecipeStack? obj = Recipe.GetIngrendientFor(istack, ingredientsToSkip.ToArray())?.GetMatchingStack(istack);
					int statMultiplier = obj?.StackSize ?? 1;
					istack = obj?.CookedStack?.ResolvedItemstack?.Clone() ?? istack;
					istack.StackSize = 1;
					WaterTightContainableProps props = BlockLiquidContainerBase.GetContainableProps(istack);
					if (props != null)
					{
						istack.StackSize = (int)(props.ItemsPerLitre * ingredient.PortionSizeLitres);
					}
					FoodNutritionProperties stackProps = BlockMeal.GetIngredientStackNutritionProperties(capi.World, istack, capi.World.Player.Entity);
					if (stackProps != null)
					{
						ingredientMinSat = GameMath.Min(ingredientMinSat, stackProps.Satiety * (float)statMultiplier);
						ingredientMaxSat = GameMath.Max(ingredientMaxSat, stackProps.Satiety * (float)statMultiplier);
						ingredientMinHP = GameMath.Min(ingredientMinHP, stackProps.Health * (float)statMultiplier);
						ingredientMaxHP = GameMath.Max(ingredientMaxHP, stackProps.Health * (float)statMultiplier);
					}
					skip = skip && stackProps == null;
					EnumFoodCategory? enumFoodCategory = stackProps?.FoodCategory;
					if (enumFoodCategory.HasValue)
					{
						EnumFoodCategory valueOrDefault = enumFoodCategory.GetValueOrDefault();
						if (valueOrDefault != EnumFoodCategory.NoNutrition && valueOrDefault != EnumFoodCategory.Unknown)
						{
							categories.Add(stackProps.FoodCategory);
						}
					}
				}
				slots -= ingredient.MinQuantity;
				if (!skip)
				{
					int minQuantity = ingredient.MinQuantity;
					int possibleSlots = ingredient.MaxQuantity - minQuantity;
					while (minQuantity > 0)
					{
						lowestSat += ingredientMinSat;
						highestSat += ingredientMaxSat;
						lowestHP += ingredientMinHP;
						highestHP += ingredientMaxHP;
						minQuantity--;
					}
					if (possibleSlots > 0)
					{
						ingredientMinMax.Add(new IngredientMinMax
						{
							Code = ingredient.Code,
							ExtraSlots = possibleSlots,
							MinSat = ingredientMinSat,
							MaxSat = ingredientMaxSat,
							MinHP = ingredientMinHP,
							MaxHP = ingredientMaxHP
						});
					}
					ingredientsToSkip.Add(rIngredient);
				}
			}
			if (ingredientMinMax.Count > 0 && slots > 0)
			{
				List<IngredientMinMax> satMin = (from ingredientMinMax2 in ingredientMinMax
					orderby ingredientMinMax2.MinSat
					select ingredientMinMax2.Clone()).ToList();
				List<IngredientMinMax> satMax = (from ingredientMinMax2 in ingredientMinMax
					orderby ingredientMinMax2.MaxSat
					select ingredientMinMax2.Clone()).ToList();
				List<IngredientMinMax> hpMin = (from ingredientMinMax2 in ingredientMinMax
					orderby ingredientMinMax2.MinHP
					select ingredientMinMax2.Clone()).ToList();
				List<IngredientMinMax> hpMax = (from ingredientMinMax2 in ingredientMinMax
					orderby ingredientMinMax2.MaxHP
					select ingredientMinMax2.Clone()).ToList();
				while (slots > 0)
				{
					float slotMinHP;
					float slotMaxSat;
					float slotMaxHP;
					float slotMinSat = (slotMinHP = (slotMaxSat = (slotMaxHP = 0f)));
					if (satMin.First().MinSat < 0f)
					{
						IngredientMinMax ingredient2 = satMin.First();
						slotMinSat = ingredient2.MinSat;
						ingredient2.ExtraSlots--;
						if (ingredient2.ExtraSlots == 0)
						{
							satMin.Remove(ingredient2);
						}
					}
					if (satMax.Last().MaxSat > 0f)
					{
						IngredientMinMax ingredient3 = satMax.Last();
						slotMaxSat = ingredient3.MaxSat;
						ingredient3.ExtraSlots--;
						if (ingredient3.ExtraSlots == 0)
						{
							satMax.Remove(ingredient3);
						}
					}
					if (hpMin.First().MinHP < 0f)
					{
						IngredientMinMax ingredient4 = hpMin.First();
						slotMinHP = ingredient4.MinHP;
						ingredient4.ExtraSlots--;
						if (ingredient4.ExtraSlots == 0)
						{
							hpMin.Remove(ingredient4);
						}
					}
					if (hpMax.Last().MaxHP > 0f)
					{
						IngredientMinMax ingredient5 = hpMax.Last();
						slotMaxHP = ingredient5.MaxHP;
						ingredient5.ExtraSlots--;
						if (ingredient5.ExtraSlots == 0)
						{
							hpMax.Remove(ingredient5);
						}
					}
					if (slotMinSat < 0f)
					{
						lowestSat += slotMinSat;
					}
					if (slotMaxSat > 0f)
					{
						highestSat += slotMaxSat;
					}
					if (slotMinHP < 0f)
					{
						lowestHP += slotMinHP;
					}
					if (slotMaxHP > 0f)
					{
						highestHP += slotMaxHP;
					}
					slots--;
				}
			}
			cachedNutritionFacts = new HandbookMealNutritionFacts
			{
				Categories = categories,
				MinSatiety = lowestSat,
				MaxSatiety = highestSat,
				MinHealth = lowestHP,
				MaxHealth = highestHP
			};
		}
		return cachedNutritionFacts;
	}

	private List<ItemStack> getIngredientStacks(ICoreClientAPI capi, CookingRecipeIngredient ingredient, ItemStack[] allStacks)
	{
		if (!cachedIngredientStacks.TryGetValue(ingredient.Code, out List<ItemStack> cachedStacks))
		{
			HashSet<ItemStack> ingredientStacks = new HashSet<ItemStack>();
			foreach (ItemStack aStack in allStacks)
			{
				CookingRecipeStack jstack = ingredient.GetMatchingStack(aStack);
				if (jstack != null)
				{
					ItemStack istack = aStack.Clone();
					istack.StackSize = jstack.StackSize;
					WaterTightContainableProps props = BlockLiquidContainerBase.GetContainableProps(istack);
					if (props != null)
					{
						istack.StackSize = (int)(props.ItemsPerLitre * ingredient.PortionSizeLitres) * jstack.StackSize;
					}
					ingredientStacks.Add(istack);
				}
			}
			cachedIngredientStacks.Add(ingredient.Code, ingredientStacks.ToList());
			return ingredientStacks.Select((ItemStack stack) => stack.Clone()).ToList();
		}
		return cachedStacks.Select((ItemStack stack) => stack.Clone()).ToList();
	}

	protected List<CookingRecipeIngredient> CombinedIngredientsList()
	{
		List<CookingRecipeIngredient> combinedIngredients = new List<CookingRecipeIngredient>();
		CookingRecipeIngredient[] ingredients = Recipe.Ingredients;
		foreach (CookingRecipeIngredient ingredient in ingredients)
		{
			if (combinedIngredients.Count > 0)
			{
				List<AssetLocation> valCodes = new List<AssetLocation>();
				CookingRecipeStack[] validStacks = ingredient.ValidStacks;
				foreach (CookingRecipeStack vstack in validStacks)
				{
					valCodes.Add(vstack.Code);
				}
				bool add = true;
				foreach (CookingRecipeIngredient comIngred in combinedIngredients)
				{
					List<AssetLocation> combinedCodes = new List<AssetLocation>();
					validStacks = comIngred.ValidStacks;
					foreach (CookingRecipeStack vstack2 in validStacks)
					{
						combinedCodes.Add(vstack2.Code);
					}
					valCodes.Sort();
					combinedCodes.Sort();
					bool matches = true;
					if (valCodes.Count != combinedCodes.Count)
					{
						matches = false;
					}
					for (int k = 0; k < combinedCodes.Count; k++)
					{
						if (k < valCodes.Count && valCodes[k] != combinedCodes[k])
						{
							matches = false;
						}
					}
					if (matches)
					{
						comIngred.MinQuantity += ingredient.MinQuantity;
						comIngred.MaxQuantity += ingredient.MaxQuantity;
						add = false;
					}
				}
				if (add)
				{
					combinedIngredients.Add(ingredient.Clone());
				}
			}
			else
			{
				combinedIngredients.Add(ingredient.Clone());
			}
		}
		return combinedIngredients;
	}

	public override PageText GetPageText()
	{
		return new PageText
		{
			Title = titleCached,
			Text = textCached
		};
	}
}
