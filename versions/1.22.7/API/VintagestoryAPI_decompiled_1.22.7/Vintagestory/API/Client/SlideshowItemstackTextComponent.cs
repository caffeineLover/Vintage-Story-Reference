using System;
using System.Collections.Generic;
using Cairo;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.API.Client;

/// <summary>
/// Draws multiple itemstacks
/// </summary>
public class SlideshowItemstackTextComponent : ItemstackComponentBase
{
	public bool ShowTooltip = true;

	public ItemStack[] Itemstacks;

	protected ItemSlot slot;

	protected Action<ItemStack> onStackClicked;

	protected float secondsVisible = 1f;

	protected int curItemIndex;

	public string ExtraTooltipText;

	public Vec3f renderOffset = new Vec3f();

	public float renderSize = 0.58f;

	private double unscaledSize;

	public StackDisplayDelegate overrideCurrentItemStack;

	public bool ShowStackSize { get; set; }

	public bool Background { get; set; }

	/// <summary>
	/// Flips through given array of item stacks every second
	/// </summary>
	/// <param name="capi"></param>
	/// <param name="itemstacks"></param>
	/// <param name="unscaledSize"></param>
	/// <param name="floatType"></param>
	/// <param name="onStackClicked"></param>
	public SlideshowItemstackTextComponent(ICoreClientAPI capi, ItemStack[] itemstacks, double unscaledSize, EnumFloat floatType, Action<ItemStack> onStackClicked = null)
		: base(capi)
	{
		initSlot();
		this.unscaledSize = unscaledSize;
		Itemstacks = itemstacks;
		Float = floatType;
		BoundsPerLine = new LineRectangled[1]
		{
			new LineRectangled(0.0, 0.0, GuiElement.scaled(unscaledSize), GuiElement.scaled(unscaledSize))
		};
		this.onStackClicked = onStackClicked;
	}

	public SlideshowItemstackTextComponent(ICoreClientAPI capi, ItemStack itemstackgroup, List<ItemStack> allstacks, double unscaleSize, EnumFloat floatType, Action<ItemStack> onStackClicked = null)
		: this(capi, itemstackgroup, allstacks, unscaleSize, floatType, "groupBy", onStackClicked)
	{
	}

	/// <summary>
	/// Looks at the collectibles handbook groupBy attribute and makes a list of itemstacks from that
	/// </summary>
	/// <param name="capi"></param>
	/// <param name="itemstackgroup"></param>
	/// <param name="allstacks"></param>
	/// <param name="unscaleSize"></param>
	/// <param name="floatType"></param>
	/// <param name="groupByCode"></param>
	/// <param name="onStackClicked"></param>
	public SlideshowItemstackTextComponent(ICoreClientAPI capi, ItemStack itemstackgroup, List<ItemStack> allstacks, double unscaleSize, EnumFloat floatType, string groupByCode, Action<ItemStack> onStackClicked = null)
		: base(capi)
	{
		initSlot();
		this.onStackClicked = onStackClicked;
		unscaledSize = unscaleSize;
		string[] groups = itemstackgroup.Collectible.Attributes?["handbook"]?[groupByCode ?? "groupBy"]?.AsArray<string>();
		List<ItemStack> nowGroupedStacks = new List<ItemStack>();
		List<ItemStack> stacks = new List<ItemStack>();
		nowGroupedStacks.Add(itemstackgroup);
		stacks.Add(itemstackgroup);
		if (allstacks != null)
		{
			if (groups != null)
			{
				AssetLocation[] groupWildCards = new AssetLocation[groups.Length];
				for (int i = 0; i < groups.Length; i++)
				{
					string groupStr = itemstackgroup.Collectible.GetCollectibleInterface<IHandbookGrouping>()?.GetWildcardForHandbookGrouping(groups[i], itemstackgroup) ?? groups[i];
					if (!groupStr.Contains(":"))
					{
						groupWildCards[i] = new AssetLocation(itemstackgroup.Collectible.Code.Domain, groupStr);
					}
					else
					{
						groupWildCards[i] = new AssetLocation(groupStr);
					}
				}
				foreach (ItemStack val in allstacks)
				{
					JsonObject attributes = val.Collectible.Attributes;
					if (attributes != null && attributes["handbook"]?["isDuplicate"].AsBool() == true)
					{
						nowGroupedStacks.Add(val);
						continue;
					}
					AssetLocation code = val.Collectible.GetCollectibleInterface<IHandbookGrouping>()?.GetCodeForHandbookGrouping(val) ?? val.Collectible.Code;
					for (int j = 0; j < groupWildCards.Length; j++)
					{
						if (code != null && WildcardUtil.Match(groupWildCards[j], code))
						{
							stacks.Add(val);
							nowGroupedStacks.Add(val);
							break;
						}
					}
				}
			}
			foreach (ItemStack val2 in nowGroupedStacks)
			{
				allstacks.Remove(val2);
			}
		}
		Itemstacks = stacks.ToArray();
		Float = floatType;
		BoundsPerLine = new LineRectangled[1]
		{
			new LineRectangled(0.0, 0.0, GuiElement.scaled(unscaleSize), GuiElement.scaled(unscaleSize))
		};
	}

	private void initSlot()
	{
		dummyInv = new DummyInventory(capi);
		dummyInv.OnAcquireTransitionSpeed += (EnumTransitionType transType, ItemStack stack, float mul) => 0f;
		slot = new DummySlot(null, dummyInv);
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

	protected override string OnRequireInfoText(ItemSlot slot)
	{
		return base.OnRequireInfoText(slot) + ExtraTooltipText;
	}

	public override void RenderInteractiveElements(float deltaTime, double renderX, double renderY, double renderZ)
	{
		int relx = (int)((double)api.Input.MouseX - renderX + (double)renderOffset.X);
		int rely = (int)((double)api.Input.MouseY - renderY + (double)renderOffset.Y);
		LineRectangled bounds = BoundsPerLine[0];
		bool num = bounds.PointInside(relx, rely);
		ItemStack itemstack = Itemstacks[curItemIndex];
		if (!num && (secondsVisible -= deltaTime) <= 0f)
		{
			secondsVisible = 1f;
			curItemIndex = (curItemIndex + 1) % Itemstacks.Length;
		}
		if (overrideCurrentItemStack != null)
		{
			itemstack = overrideCurrentItemStack();
		}
		slot.Itemstack = itemstack;
		ElementBounds scibounds = ElementBounds.FixedSize((int)(bounds.Width / (double)RuntimeEnv.GUIScale), (int)(bounds.Height / (double)RuntimeEnv.GUIScale));
		scibounds.ParentBounds = capi.Gui.WindowBounds;
		scibounds.CalcWorldBounds();
		scibounds.absFixedX = renderX + bounds.X + (double)renderOffset.X;
		scibounds.absFixedY = renderY + bounds.Y + (double)renderOffset.Y;
		scibounds.absInnerWidth *= renderSize / 0.58f;
		scibounds.absInnerHeight *= renderSize / 0.58f;
		api.Render.PushScissor(scibounds, stacking: true);
		api.Render.RenderItemstackToGui(slot, renderX + bounds.X + bounds.Width * 0.5 + (double)renderOffset.X + offX, renderY + bounds.Y + bounds.Height * 0.5 + (double)renderOffset.Y + offY, 100f + renderOffset.Z, (float)bounds.Width * renderSize, -1, shading: true, rotate: false, ShowStackSize);
		api.Render.PopScissor();
		if (num && ShowTooltip)
		{
			RenderItemstackTooltip(slot, renderX + (double)relx, renderY + (double)rely, deltaTime);
		}
	}

	public override void OnMouseDown(MouseEvent args)
	{
		if (slot.Itemstack == null)
		{
			return;
		}
		LineRectangled[] boundsPerLine = BoundsPerLine;
		for (int i = 0; i < boundsPerLine.Length; i++)
		{
			if (boundsPerLine[i].PointInside(args.X, args.Y))
			{
				onStackClicked?.Invoke(slot.Itemstack);
			}
		}
	}
}
