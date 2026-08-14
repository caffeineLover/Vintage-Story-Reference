using System;
using System.Collections.Generic;
using Cairo;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace Vintagestory.API.Client;

public class MealstackTextComponent : ItemstackComponentBase
{
	public bool ShowTooltip = true;

	private DummySlot dummySlot;

	protected Action<CookingRecipe>? onMealClicked;

	protected float secondsVisible = 1f;

	protected int curItemIndex;

	public Vec3f renderOffset = new Vec3f();

	public float renderSize = 0.58f;

	private double unscaledSize;

	private CookingRecipe recipe;

	private ItemStack? ingredient;

	private int slots;

	private bool isPie;

	public bool ShowStackSize { get; set; }

	public bool Background { get; set; }

	public bool RandomBowlBlock { get; set; } = true;

	public MealstackTextComponent(ICoreClientAPI capi, ItemStack mealBlock, CookingRecipe recipe, double unscaledSize, EnumFloat floatType, Action<CookingRecipe>? onMealClicked = null, int slots = 4, bool isPie = false, ItemStack? ingredient = null)
		: base(capi)
	{
		dummySlot = new DummySlot(mealBlock);
		if (dummySlot.Itemstack?.Collectible is IBlockMealContainer meal)
		{
			if (isPie)
			{
				dummySlot.Itemstack.Attributes.SetString("topCrustType", BlockPie.TopCrustTypes[capi.World.Rand.Next(BlockPie.TopCrustTypes.Length)].Code);
			}
			Dictionary<CookingRecipeIngredient, HashSet<ItemStack>> cachedValidStacks = ObjectCacheUtil.TryGet<Dictionary<CookingRecipeIngredient, HashSet<ItemStack>>>(capi, "valstacksbying-" + recipe.Code);
			meal.SetContents(recipe.Code, dummySlot.Itemstack, isPie ? BlockPie.GenerateRandomPie(capi, ref cachedValidStacks, recipe, ingredient) : recipe.GenerateRandomMeal(capi, ref cachedValidStacks, ObjectCacheUtil.TryGet<ItemStack[]>(capi, "handbookallstacks"), slots, ingredient));
		}
		this.ingredient = ingredient;
		this.slots = slots;
		this.isPie = isPie;
		this.recipe = recipe;
		this.unscaledSize = unscaledSize;
		Float = floatType;
		BoundsPerLine = new LineRectangled[1]
		{
			new LineRectangled(0.0, 0.0, GuiElement.scaled(unscaledSize), GuiElement.scaled(unscaledSize))
		};
		this.onMealClicked = onMealClicked;
	}

	public override EnumCalcBoundsResult CalcBounds(TextFlowPath[] flowPath, double currentLineHeight, double offsetX, double lineY, out double nextOffsetX)
	{
		TextFlowPath curfp = GetCurrentFlowPathSection(flowPath, lineY);
		offsetX += GuiElement.scaled(PaddingLeft);
		bool requireLinebreak = offsetX + BoundsPerLine[0].Width > curfp.X2;
		BoundsPerLine[0].X = (requireLinebreak ? 0.0 : offsetX);
		BoundsPerLine[0].Y = lineY + (requireLinebreak ? currentLineHeight : 0.0);
		BoundsPerLine[0].Width = GuiElement.scaled(unscaledSize) + GuiElement.scaled(PaddingRight);
		nextOffsetX = (requireLinebreak ? 0.0 : offsetX) + BoundsPerLine[0].Width;
		if (!requireLinebreak)
		{
			return EnumCalcBoundsResult.Continue;
		}
		return EnumCalcBoundsResult.Nextline;
	}

	public override void ComposeElements(Context ctx, ImageSurface surface)
	{
		if (Background)
		{
			ctx.SetSourceRGBA(1.0, 1.0, 1.0, 0.2);
			ctx.Rectangle(BoundsPerLine[0].X, BoundsPerLine[0].Y, BoundsPerLine[0].Width, BoundsPerLine[0].Height);
			ctx.Fill();
		}
	}

	public override void RenderInteractiveElements(float deltaTime, double renderX, double renderY, double renderZ)
	{
		int relx = (int)((double)api.Input.MouseX - renderX + (double)renderOffset.X);
		int rely = (int)((double)api.Input.MouseY - renderY + (double)renderOffset.Y);
		LineRectangled bounds = BoundsPerLine[0];
		bool mouseover = bounds.PointInside(relx, rely);
		if (!(dummySlot.Itemstack?.Collectible is IBlockMealContainer mealBlock))
		{
			return;
		}
		if (!mouseover && (secondsVisible -= deltaTime) <= 0f)
		{
			secondsVisible = 1f;
			if (RandomBowlBlock)
			{
				if (isPie)
				{
					dummySlot.Itemstack?.Attributes.SetString("topCrustType", BlockPie.TopCrustTypes[capi.World.Rand.Next(BlockPie.TopCrustTypes.Length)].Code);
				}
				else
				{
					dummySlot.Itemstack = new ItemStack(BlockMeal.RandomMealBowl(capi));
				}
			}
			Dictionary<CookingRecipeIngredient, HashSet<ItemStack>> cachedValidStacks = ObjectCacheUtil.TryGet<Dictionary<CookingRecipeIngredient, HashSet<ItemStack>>>(capi, "valstacksbying-" + recipe.Code);
			mealBlock.SetContents(recipe.Code, dummySlot.Itemstack, isPie ? BlockPie.GenerateRandomPie(capi, ref cachedValidStacks, recipe, ingredient) : recipe.GenerateRandomMeal(capi, ref cachedValidStacks, ObjectCacheUtil.TryGet<ItemStack[]>(capi, "handbookallstacks"), slots, ingredient));
		}
		ElementBounds scibounds = ElementBounds.FixedSize((int)(bounds.Width / (double)RuntimeEnv.GUIScale), (int)(bounds.Height / (double)RuntimeEnv.GUIScale));
		scibounds.ParentBounds = capi.Gui.WindowBounds;
		scibounds.CalcWorldBounds();
		scibounds.absFixedX = renderX + bounds.X + (double)renderOffset.X;
		scibounds.absFixedY = renderY + bounds.Y + (double)renderOffset.Y;
		scibounds.absInnerWidth *= renderSize / 0.58f;
		scibounds.absInnerHeight *= renderSize / 0.58f;
		api.Render.PushScissor(scibounds, stacking: true);
		api.Render.RenderItemstackToGui(dummySlot, renderX + bounds.X + bounds.Width * 0.5 + (double)renderOffset.X + offX, renderY + bounds.Y + bounds.Height * 0.5 + (double)renderOffset.Y + offY, 100f + renderOffset.Z, (float)GuiElement.scaled(unscaledSize) * renderSize, -1, shading: true, rotate: false, ShowStackSize);
		api.Render.PopScissor();
		if (mouseover && ShowTooltip)
		{
			RenderItemstackTooltip(dummySlot, renderX + (double)relx, renderY + (double)rely, deltaTime);
		}
	}

	public override void OnMouseDown(MouseEvent args)
	{
		LineRectangled[] boundsPerLine = BoundsPerLine;
		for (int i = 0; i < boundsPerLine.Length; i++)
		{
			if (boundsPerLine[i].PointInside(args.X, args.Y))
			{
				onMealClicked?.Invoke(recipe);
			}
		}
	}
}
