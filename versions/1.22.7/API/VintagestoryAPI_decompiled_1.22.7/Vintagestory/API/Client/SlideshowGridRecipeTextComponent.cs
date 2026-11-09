using System;
using System.Collections.Generic;
using System.Linq;
using Cairo;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Util;

namespace Vintagestory.API.Client;

/// <summary>
/// Draws multiple ItemStack
/// </summary>
public sealed class SlideshowGridRecipeTextComponent : ItemstackComponentBase
{
	public GridRecipeAndUnnamedIngredients[] GridRecipesAndUnnamedIngredients;

	public GridRecipeAndUnnamedIngredients? CurrentVisibleRecipe;

	private readonly Action<ItemStack?>? onStackClicked;

	private readonly ItemSlot dummySlot = new DummySlot();

	private readonly double size;

	private readonly Dictionary<string, ItemStack[]> resolvedIngredientsCache = new Dictionary<string, ItemStack[]>();

	private readonly Dictionary<int, LoadedTexture> extraTexts = new Dictionary<int, LoadedTexture>();

	private readonly ItemSlot currentVisibleOutputSlot = new DummySlot();

	private readonly ItemSlot[] currentVisibleInputs = new ItemSlot[9]
	{
		new DummySlot(),
		new DummySlot(),
		new DummySlot(),
		new DummySlot(),
		new DummySlot(),
		new DummySlot(),
		new DummySlot(),
		new DummySlot(),
		new DummySlot()
	};

	private LoadedTexture? hoverTexture;

	private int secondCounter;

	private float secondsVisible = 1f;

	private int currentItemIndex;

	private static readonly int[][,] variantDisplaySequence = new int[30][,];

	/// <summary>
	/// Flips through given array of grid recipes every second
	/// </summary>
	/// <param name="api"></param>
	/// <param name="gridRecipes"></param>
	/// <param name="size"></param>
	/// <param name="floatType"></param>
	/// <param name="onStackClicked"></param>
	/// <param name="allStacks">If set, will resolve wildcards based on this list, otherwise will search all available blocks/items</param>
	public SlideshowGridRecipeTextComponent(ICoreClientAPI api, GridRecipe[] gridRecipes, double size, EnumFloat floatType, Action<ItemStack>? onStackClicked = null, ItemStack[]? allStacks = null)
		: base(api)
	{
		SlideshowGridRecipeTextComponent slideshowGridRecipeTextComponent = this;
		size = GuiElement.scaled(size);
		double innerMargin = GuiElement.scaled(3.0);
		this.onStackClicked = onStackClicked;
		Float = floatType;
		BoundsPerLine = new LineRectangled[1]
		{
			new LineRectangled(0.0, 0.0, 2.0 * (size + innerMargin) + size, 2.0 * (size + innerMargin) + size)
		};
		this.size = size;
		Random fixedRand = new Random(123);
		for (int i = 0; i < 30; i++)
		{
			int[,] sq = (variantDisplaySequence[i] = new int[3, 3]);
			for (int x = 0; x < 3; x++)
			{
				for (int y = 0; y < 3; y++)
				{
					sq[x, y] = api.World.Rand.Next(99999);
				}
			}
		}
		List<GridRecipeAndUnnamedIngredients> resolvedGridRecipes = new List<GridRecipeAndUnnamedIngredients>();
		ResolveRecipes(resolvedGridRecipes, gridRecipes, allStacks, fixedRand);
		resolvedIngredientsCache.Clear();
		GridRecipesAndUnnamedIngredients = resolvedGridRecipes.ToArray();
		GridRecipesAndUnnamedIngredients.Shuffle(fixedRand);
		bool extraLine = false;
		for (int j = 0; j < GridRecipesAndUnnamedIngredients.Length; j++)
		{
			string trait = GridRecipesAndUnnamedIngredients[j]?.Recipe?.RequiresTrait;
			if (trait != null)
			{
				extraTexts[j] = api.Gui.TextTexture.GenTextTexture(Lang.Get("gridrecipe-requirestrait", Lang.Get("traitname-" + trait)), CairoFont.WhiteDetailText());
				if (!extraLine)
				{
					BoundsPerLine[0].Height += GuiElement.scaled(20.0);
				}
				extraLine = true;
			}
		}
		if (GridRecipesAndUnnamedIngredients.Length == 0)
		{
			throw new ArgumentException("Could not resolve any of the supplied grid recipes?");
		}
		GenerateHover();
		stackInfo.onRenderStack = delegate
		{
			GridRecipeAndUnnamedIngredients gridRecipeAndUnnamedIngredients = slideshowGridRecipeTextComponent.GridRecipesAndUnnamedIngredients[slideshowGridRecipeTextComponent.currentItemIndex];
			double num = (int)GuiElement.scaled(30.0 + GuiElementItemstackInfo.ItemStackSize / 2.0);
			ItemSlot curSlot = slideshowGridRecipeTextComponent.stackInfo.curSlot;
			if (curSlot?.Itemstack != null)
			{
				int stackSize = curSlot.StackSize;
				curSlot.Itemstack.StackSize = 1;
				curSlot.Itemstack.Collectible.OnHandbookRecipeRender(api, gridRecipeAndUnnamedIngredients.Recipe, curSlot, (double)(int)slideshowGridRecipeTextComponent.stackInfo.Bounds.renderX + num, (double)(int)slideshowGridRecipeTextComponent.stackInfo.Bounds.renderY + num + (double)(int)GuiElement.scaled(GuiElementItemstackInfo.MarginTop), 1000.0 + GuiElement.scaled(GuiElementPassiveItemSlot.unscaledItemSize) * 2.0, (float)GuiElement.scaled(GuiElementItemstackInfo.ItemStackSize) * 1f / 0.58f);
				curSlot.Itemstack.StackSize = stackSize;
			}
		};
	}

	public ItemStack? GenerateCurrentVisibleOutputStack()
	{
		CurrentVisibleRecipe?.Recipe?.GenerateOutputStack(currentVisibleInputs, currentVisibleOutputSlot);
		return currentVisibleOutputSlot.Itemstack;
	}

	public override EnumCalcBoundsResult CalcBounds(TextFlowPath[] flowPath, double currentLineHeight, double offsetX, double lineY, out double nextOffsetX)
	{
		TextFlowPath currentFlowPath = GetCurrentFlowPathSection(flowPath, lineY);
		offsetX += GuiElement.scaled(PaddingLeft);
		bool requireLineBreak = offsetX + BoundsPerLine[0].Width > currentFlowPath.X2;
		BoundsPerLine[0].X = (requireLineBreak ? 0.0 : offsetX);
		BoundsPerLine[0].Y = lineY + (requireLineBreak ? (currentLineHeight + GuiElement.scaled(UnscaledMarginTop)) : 0.0);
		BoundsPerLine[0].Width = 3.0 * (size + 3.0) + GuiElement.scaled(PaddingRight);
		nextOffsetX = (requireLineBreak ? 0.0 : offsetX) + BoundsPerLine[0].Width;
		if (!requireLineBreak)
		{
			return EnumCalcBoundsResult.Continue;
		}
		return EnumCalcBoundsResult.Nextline;
	}

	public override void ComposeElements(Context ctx, ImageSurface surface)
	{
		ctx.SetSourceRGBA(1.0, 1.0, 1.0, 0.2);
		for (int x = 0; x < 3; x++)
		{
			for (int y = 0; y < 3; y++)
			{
				ctx.Rectangle(BoundsPerLine[0].X + (double)x * (size + GuiElement.scaled(3.0)), BoundsPerLine[0].Y + (double)y * (size + GuiElement.scaled(3.0)), size, size);
				ctx.Fill();
			}
		}
	}

	public override void RenderInteractiveElements(float deltaTime, double renderX, double renderY, double renderZ)
	{
		LineRectangled bounds = BoundsPerLine[0];
		int relx = (int)((double)api.Input.MouseX - renderX);
		int rely = (int)((double)api.Input.MouseY - renderY);
		if (!bounds.PointInside(relx, rely) && (secondsVisible -= deltaTime) <= 0f)
		{
			secondsVisible = 1f;
			currentItemIndex = (currentItemIndex + 1) % GridRecipesAndUnnamedIngredients.Length;
			secondCounter++;
		}
		GridRecipeAndUnnamedIngredients recipeunin = (CurrentVisibleRecipe = GridRecipesAndUnnamedIngredients[currentItemIndex]);
		if (extraTexts.TryGetValue(currentItemIndex, out LoadedTexture extraTextTexture))
		{
			capi.Render.Render2DTexturePremultipliedAlpha(extraTextTexture.TextureId, (float)(renderX + bounds.X), (float)(renderY + bounds.Y + 3.0 * (size + 3.0)), extraTextTexture.Width, extraTextTexture.Height);
		}
		double rx = 0.0;
		double ry = 0.0;
		int mx = api.Input.MouseX;
		int my = api.Input.MouseY;
		for (int x = 0; x < 3; x++)
		{
			for (int y = 0; y < 3; y++)
			{
				int index = recipeunin.Recipe.GetGridIndex(y, x, recipeunin.Recipe.ResolvedIngredients, recipeunin.Recipe.Width);
				rx = renderX + bounds.X + (double)x * (size + GuiElement.scaled(3.0));
				ry = renderY + bounds.Y + (double)y * (size + GuiElement.scaled(3.0));
				float scale = RuntimeEnv.GUIScale;
				ElementBounds scissorBounds = ElementBounds.Fixed(rx / (double)scale, ry / (double)scale, size / (double)scale, size / (double)scale).WithEmptyParent();
				scissorBounds.CalcWorldBounds();
				CraftingRecipeIngredient ingred = recipeunin.Recipe.GetElementInGrid(y, x, recipeunin.Recipe.ResolvedIngredients, recipeunin.Recipe.Width);
				if (ingred == null)
				{
					currentVisibleInputs[x + y * 3].Itemstack = null;
					continue;
				}
				api.Render.PushScissor(scissorBounds, stacking: true);
				Dictionary<int, ItemStack[]>? unnamedIngredients = recipeunin.UnnamedIngredients;
				if (unnamedIngredients != null && unnamedIngredients.TryGetValue(index, out ItemStack[] unnamedWildcardStacklist) && unnamedWildcardStacklist.Length != 0)
				{
					dummySlot.Itemstack = unnamedWildcardStacklist[variantDisplaySequence[secondCounter % 30][x, y] % unnamedWildcardStacklist.Length];
					dummySlot.Itemstack.StackSize = ingred.Quantity;
					currentVisibleInputs[x + y * 3].Itemstack = dummySlot.Itemstack;
				}
				else
				{
					dummySlot.Itemstack = ingred.ResolvedItemStack?.Clone();
					currentVisibleInputs[x + y * 3].Itemstack = dummySlot.Itemstack;
				}
				dummySlot.BackgroundIcon = index.ToString() ?? "";
				dummySlot.Itemstack?.Collectible.OnHandbookRecipeRender(capi, recipeunin.Recipe, dummySlot, rx + size * 0.5, ry + size * 0.5, 100.0, size);
				api.Render.PopScissor();
				double dx = (double)mx - rx + 1.0;
				double dy = (double)my - ry + 2.0;
				if (dx >= 0.0 && dx < size && dy >= 0.0 && dy < size)
				{
					RenderItemstackTooltip(dummySlot, rx + dx, ry + dy, deltaTime);
				}
				dummySlot.BackgroundIcon = null;
			}
		}
	}

	public override void OnMouseDown(MouseEvent args)
	{
		if (BoundsPerLine == null)
		{
			return;
		}
		GridRecipeAndUnnamedIngredients recipeAndIngredients = GridRecipesAndUnnamedIngredients[currentItemIndex];
		GridRecipe recipe = recipeAndIngredients.Recipe;
		LineRectangled[] boundsPerLine = BoundsPerLine;
		foreach (LineRectangled val in boundsPerLine)
		{
			if (val.PointInside(args.X, args.Y))
			{
				int x = (int)(((double)args.X - val.X) / (size + 3.0));
				int y = (int)(((double)args.Y - val.Y) / (size + 3.0));
				CraftingRecipeIngredient ingredient = recipe?.GetElementInGrid(y, x, recipe.ResolvedIngredients ?? Array.Empty<CraftingRecipeIngredient>(), recipe.Width);
				if (recipe == null || ingredient == null)
				{
					break;
				}
				int index = recipe.GetGridIndex(y, x, recipe.ResolvedIngredients ?? Array.Empty<CraftingRecipeIngredient>(), recipe.Width);
				Dictionary<int, ItemStack[]>? unnamedIngredients = recipeAndIngredients.UnnamedIngredients;
				if (unnamedIngredients != null && unnamedIngredients.TryGetValue(index, out ItemStack[] unnamedWildcardStacklist))
				{
					onStackClicked?.Invoke(unnamedWildcardStacklist[variantDisplaySequence[secondCounter % 30][x, y] % unnamedWildcardStacklist.Length]);
				}
				else
				{
					onStackClicked?.Invoke(ingredient.ResolvedItemStack);
				}
			}
		}
	}

	public override void Dispose()
	{
		base.Dispose();
		foreach (KeyValuePair<int, LoadedTexture> extraText in extraTexts)
		{
			extraText.Value.Dispose();
		}
		hoverTexture?.Dispose();
	}

	private void GenerateHover()
	{
		ImageSurface surface = new ImageSurface(Format.Argb32, 1, 1);
		Context context = new Context(surface);
		context.SetSourceRGBA(1.0, 1.0, 1.0, 0.6);
		context.Paint();
		hoverTexture = new LoadedTexture(capi);
		api.Gui.LoadOrUpdateCairoTexture(surface, linearMag: false, ref hoverTexture);
		context.Dispose();
		surface.Dispose();
	}

	private static string GenerateCacheKey(IRecipeIngredient ingredient)
	{
		if (!ingredient.Tags.IsEmpty)
		{
			return $"{ingredient.Code}:{ingredient.Tags.GetHashCode()}";
		}
		return $"{ingredient.Code}:-";
	}

	private ItemStack[] MatchIngredients(IWorldAccessor world, IRecipeIngredient ingredient, ItemStack[]? allStacks = null)
	{
		string cacheKey = GenerateCacheKey(ingredient);
		if (resolvedIngredientsCache.TryGetValue(cacheKey, out ItemStack[] result))
		{
			return result;
		}
		ItemStack[] matches = ((allStacks == null) ? (from collectible in world.Collectibles
			select new ItemStack(collectible, ingredient.Quantity) into stack
			where ingredient.SatisfiesAsIngredient(stack, checkStackSize: false)
			select stack).ToArray() : allStacks.Where((ItemStack stack) => ingredient.SatisfiesAsIngredient(stack, checkStackSize: false)).ToArray());
		if (matches.Length != 0)
		{
			resolvedIngredientsCache[cacheKey] = matches;
		}
		return matches;
	}

	private void ResolveRecipes(List<GridRecipeAndUnnamedIngredients> resolvedGridRecipes, GridRecipe[] gridRecipes, ItemStack[]? allStacks, Random fixedRand)
	{
		Queue<GridRecipe> halfResolvedRecipes = new Queue<GridRecipe>(gridRecipes);
		bool allResolved = false;
		while (!allResolved)
		{
			allResolved = true;
			int recipesToResolveCount = halfResolvedRecipes.Count;
			while (recipesToResolveCount-- > 0)
			{
				GridRecipe toTestRecipe = halfResolvedRecipes.Dequeue();
				Dictionary<int, ItemStack[]> unnamedIngredients = null;
				if (ResolveIngredients(ref unnamedIngredients, halfResolvedRecipes, toTestRecipe, ref allResolved, allStacks, fixedRand))
				{
					resolvedGridRecipes.Add(new GridRecipeAndUnnamedIngredients
					{
						Recipe = toTestRecipe,
						UnnamedIngredients = unnamedIngredients
					});
				}
			}
		}
	}

	private bool ResolveIngredients(ref Dictionary<int, ItemStack[]>? unnamedIngredients, Queue<GridRecipe> halfResolvedRecipes, GridRecipe toTestRecipe, ref bool allResolved, ItemStack[]? allStacks, Random fixedRand)
	{
		bool thisResolved = true;
		if (toTestRecipe.ResolvedIngredients == null)
		{
			return false;
		}
		for (int j = 0; j < toTestRecipe.ResolvedIngredients.Length; j++)
		{
			CraftingRecipeIngredient ingredient = toTestRecipe.ResolvedIngredients[j];
			if (ingredient == null || ingredient.MatchingType == EnumRecipeMatchType.Exact)
			{
				continue;
			}
			allResolved = false;
			thisResolved = false;
			ItemStack[] stacks = MatchIngredients(api.World, ingredient, allStacks);
			if (stacks.Length == 0)
			{
				resolvedIngredientsCache.Remove(ingredient.Code);
				stacks = MatchIngredients(api.World, ingredient);
				if (stacks.Length == 0)
				{
					throw new ArgumentException(string.Concat(new string[5]
					{
						"Attempted to resolve the recipe ingredient wildcard ",
						ingredient.Type.ToString(),
						" ",
						ingredient.Code,
						" but there are no such items/blocks!"
					}));
				}
			}
			if (ingredient.Name == null)
			{
				if (unnamedIngredients == null)
				{
					unnamedIngredients = new Dictionary<int, ItemStack[]>();
				}
				unnamedIngredients[j] = ((ItemStack[])stacks.Clone()).Shuffle(fixedRand);
				thisResolved = true;
				continue;
			}
			for (int k = 0; k < stacks.Length; k++)
			{
				GridRecipe cloned = toTestRecipe.Clone();
				if (cloned.ResolvedIngredients == null)
				{
					continue;
				}
				for (int m = 0; m < cloned.ResolvedIngredients.Length; m++)
				{
					CraftingRecipeIngredient clonedIngredient = cloned.ResolvedIngredients[m];
					if (clonedIngredient != null)
					{
						AssetLocation? code = clonedIngredient.Code;
						if ((object)code != null && code.Equals(ingredient.Code))
						{
							clonedIngredient.Code = stacks[k].Collectible.Code;
							clonedIngredient.MatchingType = EnumRecipeMatchType.Exact;
							clonedIngredient.ResolvedItemStack = stacks[k];
						}
					}
				}
				halfResolvedRecipes.Enqueue(cloned);
			}
		}
		return thisResolved;
	}
}
