using System;
using System.Collections.Generic;
using Cairo;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;

namespace Vintagestory.API.Client;

public class GuiElementListMenu : GuiElementTextBase
{
	/// <summary>
	/// Max height of the expanded list
	/// </summary>
	public int MaxHeight = 350;

	protected double expandedBoxWidth;

	protected double expandedBoxHeight;

	protected double unscaledLineHeight = 30.0;

	private GuiElementSwitch[] switches = Array.Empty<GuiElementSwitch>();

	protected SelectionChangedDelegate onSelectionChanged;

	protected LoadedTexture hoverTexture;

	protected LoadedTexture dropDownTexture;

	protected LoadedTexture scrollbarTexture;

	protected bool expanded;

	protected bool multiSelect;

	protected double scrollOffY;

	protected GuiElementCompactScrollbar scrollbar;

	protected GuiElementRichtext[] richtTextElem;

	protected ElementBounds visibleBounds;

	public string[] Values { get; set; }

	public string[] Names { get; set; }

	/// <summary>
	/// The (first) currently selected element
	/// </summary>
	public int SelectedIndex
	{
		get
		{
			if (SelectedIndices != null && SelectedIndices.Length != 0)
			{
				return SelectedIndices[0];
			}
			return 0;
		}
		set
		{
			if (value < 0)
			{
				SelectedIndices = Array.Empty<int>();
				return;
			}
			if (SelectedIndices != null && SelectedIndices.Length != 0)
			{
				SelectedIndices[0] = value;
				return;
			}
			SelectedIndices = new int[1] { value };
		}
	}

	/// <summary>
	/// The element the user currently has the mouse over
	/// </summary>
	public int HoveredIndex { get; set; }

	/// <summary>
	/// On multi select mode, the list of all selected elements
	/// </summary>
	public int[] SelectedIndices { get; set; }

	/// <summary>
	/// Is the current menu opened?
	/// </summary>
	public bool IsOpened => expanded;

	public override double DrawOrder => 0.5;

	public override bool Focusable => enabled;

	/// <summary>
	/// Creates a new GUI Element List Menu
	/// </summary>
	/// <param name="capi">The Client API.</param>
	/// <param name="values">The values of the list.</param>
	/// <param name="names">The names for each of the values.</param>
	/// <param name="selectedIndex">The default selected index.</param>
	/// <param name="onSelectionChanged">The event fired when the selection is changed.</param>
	/// <param name="bounds">The bounds of the GUI element.</param>
	/// <param name="font"></param>
	/// <param name="multiSelect"></param>
	public GuiElementListMenu(ICoreClientAPI capi, string[] values, string[] names, int selectedIndex, SelectionChangedDelegate onSelectionChanged, ElementBounds bounds, CairoFont font, bool multiSelect)
		: base(capi, "", font, bounds)
	{
		if (values.Length != names.Length)
		{
			throw new ArgumentException("Values and Names arrays must be of the same length!");
		}
		hoverTexture = new LoadedTexture(capi);
		dropDownTexture = new LoadedTexture(capi);
		scrollbarTexture = new LoadedTexture(capi);
		Values = values;
		Names = names;
		SelectedIndex = selectedIndex;
		this.multiSelect = multiSelect;
		this.onSelectionChanged = onSelectionChanged;
		HoveredIndex = selectedIndex;
		ElementBounds scrollbarBounds = ElementBounds.Fixed(0.0, 0.0, 0.0, 0.0).WithEmptyParent();
		scrollbar = new GuiElementCompactScrollbar(api, OnNewScrollbarValue, scrollbarBounds);
		scrollbar.zOffset = 300f;
		richtTextElem = new GuiElementRichtext[values.Length];
		for (int i = 0; i < values.Length; i++)
		{
			ElementBounds textBounds = ElementBounds.Fixed(0.0, 0.0, 700.0, 100.0).WithEmptyParent();
			richtTextElem[i] = new GuiElementRichtext(capi, Array.Empty<RichTextComponentBase>(), textBounds);
		}
	}

	private void OnNewScrollbarValue(float offY)
	{
		scrollOffY = (int)((double)offY / (30.0 * Scale) * 30.0 * Scale);
	}

	public override void ComposeElements(Context ctx, ImageSurface surface)
	{
		ComposeDynamicElements();
	}

	/// <summary>
	/// Composes the list of elements dynamically.
	/// </summary>
	public void ComposeDynamicElements()
	{
		Bounds.CalcWorldBounds();
		if (multiSelect)
		{
			if (switches != null)
			{
				GuiElementSwitch[] array = switches;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].Dispose();
				}
			}
			switches = new GuiElementSwitch[Names.Length];
		}
		GuiElementRichtext[] richTextElem = richtTextElem;
		for (int j = 0; j < richTextElem.Length; j++)
		{
			richTextElem[j].Dispose();
		}
		richTextElem = (richtTextElem = new GuiElementRichtext[Values.Length]);
		for (int k = 0; k < richTextElem.Length; k++)
		{
			ElementBounds textBounds = ElementBounds.Fixed(0.0, 0.0, 700.0, 100.0).WithEmptyParent();
			richTextElem[k] = new GuiElementRichtext(api, Array.Empty<RichTextComponentBase>(), textBounds);
		}
		double scaleMul = Scale * (double)RuntimeEnv.GUIScale;
		double lineHeight = unscaledLineHeight * scaleMul;
		expandedBoxWidth = Bounds.InnerWidth;
		expandedBoxHeight = (double)richTextElem.Length * lineHeight;
		double scrollbarWidth = 10.0;
		for (int l = 0; l < richTextElem.Length; l++)
		{
			GuiElementRichtext elem = richTextElem[l];
			elem.SetNewTextWithoutRecompose(Names[l], Font);
			elem.BeforeCalcBounds();
			expandedBoxWidth = Math.Max(expandedBoxWidth, elem.MaxLineWidth + 5.0 * scaleMul + GuiElement.scaled(scrollbarWidth + 5.0));
		}
		double maximumHeightAvailable = (double)api.Render.FrameHeight - Bounds.absY + Bounds.ParentBounds.absFixedY - Bounds.InnerHeight;
		ImageSurface surface = new ImageSurface(Format.Argb32, (int)expandedBoxWidth, (int)expandedBoxHeight);
		Context ctx = genContext(surface);
		visibleBounds = Bounds.FlatCopy();
		visibleBounds.fixedHeight = Math.Min(Math.Min(MaxHeight, expandedBoxHeight / (double)RuntimeEnv.GUIScale), maximumHeightAvailable / (double)RuntimeEnv.GUIScale);
		visibleBounds.fixedWidth = expandedBoxWidth / (double)RuntimeEnv.GUIScale;
		visibleBounds.fixedY += Bounds.InnerHeight / (double)RuntimeEnv.GUIScale;
		visibleBounds.CalcWorldBounds();
		Font.SetupContext(ctx);
		ctx.SetSourceRGBA(GuiStyle.DialogStrongBgColor);
		GuiElement.RoundRectangle(ctx, 0.0, 0.0, expandedBoxWidth, expandedBoxHeight, 1.0);
		ctx.FillPreserve();
		ctx.SetSourceRGBA(0.0, 0.0, 0.0, 0.5);
		ctx.LineWidth = 2.0;
		ctx.Stroke();
		double unscaledHeight = Font.GetFontExtents().Height / (double)RuntimeEnv.GUIScale;
		double unscaledOffY = (unscaledLineHeight - unscaledHeight) / 2.0;
		double unscaledOffx = (multiSelect ? (unscaledHeight + 10.0) : 0.0);
		double scaledHeight = unscaledHeight * scaleMul;
		ctx.SetSourceRGBA(GuiStyle.DialogDefaultTextColor);
		ElementBounds switchParentBounds = Bounds.FlatCopy();
		switchParentBounds.IsDrawingSurface = true;
		switchParentBounds.CalcWorldBounds();
		for (int m = 0; m < richTextElem.Length; m++)
		{
			int num = m;
			double y = ((double)(int)unscaledOffY + (double)m * unscaledLineHeight) * scaleMul;
			double x = unscaledOffx + 5.0 * scaleMul;
			double offy = (scaledHeight - Font.GetTextExtents(Names[m]).Height) / 2.0;
			if (multiSelect)
			{
				double pad = 2.0;
				ElementBounds switchBounds = new ElementBounds
				{
					ParentBounds = switchParentBounds,
					fixedX = 4.0 * Scale,
					fixedY = (y + offy) / (double)RuntimeEnv.GUIScale,
					fixedWidth = unscaledHeight * Scale,
					fixedHeight = unscaledHeight * Scale,
					fixedPaddingX = 0.0,
					fixedPaddingY = 0.0
				};
				switches[m] = new GuiElementSwitch(api, delegate(bool on)
				{
					toggled(on, num);
				}, switchBounds, switchBounds.fixedHeight, pad);
				switches[m].On = ((SelectedIndices == null) ? (SelectedIndex == m) : SelectedIndices.Contains(m));
				switches[m].ComposeElements(ctx, surface);
				ctx.SetSourceRGBA(GuiStyle.DialogDefaultTextColor);
			}
			GuiElementRichtext obj = richTextElem[m];
			obj.Bounds.fixedX = x;
			obj.Bounds.fixedY = (y + offy) / (double)RuntimeEnv.GUIScale;
			obj.BeforeCalcBounds();
			obj.Bounds.CalcWorldBounds();
			obj.ComposeFor(obj.Bounds, ctx, surface);
		}
		generateTexture(surface, ref dropDownTexture);
		ctx.Dispose();
		surface.Dispose();
		scrollbar.Bounds.WithFixedSize(scrollbarWidth, visibleBounds.fixedHeight - 3.0).WithFixedPosition(expandedBoxWidth / (double)RuntimeEnv.GUIScale - 10.0, 0.0).WithFixedPadding(0.0, 2.0);
		scrollbar.Bounds.WithEmptyParent();
		scrollbar.Bounds.CalcWorldBounds();
		scrollbar.CurrentYPosition = 0f;
		scrollbar.TriggerChanged();
		surface = new ImageSurface(Format.Argb32, (int)expandedBoxWidth, (int)scrollbar.Bounds.OuterHeight);
		ctx = genContext(surface);
		scrollbar.ComposeElements(ctx, surface);
		scrollbar.SetHeights((int)visibleBounds.InnerHeight, (int)expandedBoxHeight);
		generateTexture(surface, ref scrollbarTexture);
		ctx.Dispose();
		surface.Dispose();
		surface = new ImageSurface(Format.Argb32, (int)expandedBoxWidth, (int)(unscaledLineHeight * scaleMul));
		ctx = genContext(surface);
		double[] col = GuiStyle.DialogHighlightColor;
		col[3] = 0.5;
		ctx.SetSourceRGBA(col);
		GuiElement.RoundRectangle(ctx, 0.0, 0.0, expandedBoxWidth, unscaledLineHeight * scaleMul, 0.0);
		ctx.Fill();
		generateTexture(surface, ref hoverTexture);
		ctx.Dispose();
		surface.Dispose();
	}

	private void toggled(bool on, int num)
	{
		List<int> selected = new List<int>();
		for (int i = 0; i < switches.Length; i++)
		{
			if (switches[i].On)
			{
				selected.Add(i);
			}
		}
		SelectedIndices = selected.ToArray();
	}

	public override bool IsPositionInside(int posX, int posY)
	{
		if (!IsOpened)
		{
			return false;
		}
		if ((double)posX >= Bounds.absX && (double)posX <= Bounds.absX + expandedBoxWidth && (double)posY >= Bounds.absY)
		{
			return (double)posY <= Bounds.absY + expandedBoxHeight;
		}
		return false;
	}

	public override void RenderInteractiveElements(float deltaTime)
	{
		if (!expanded)
		{
			return;
		}
		double scaleMul = Scale * (double)RuntimeEnv.GUIScale;
		api.Render.PushScissor(visibleBounds);
		api.Render.Render2DTexture(dropDownTexture.TextureId, (int)Bounds.renderX, (int)Bounds.renderY + (int)Bounds.InnerHeight - (int)scrollOffY, (int)expandedBoxWidth, (int)expandedBoxHeight, 310f);
		if (multiSelect)
		{
			api.Render.GlPushMatrix();
			api.Render.GlTranslate(0.0, Bounds.InnerHeight - (double)(int)scrollOffY, 350.0);
			for (int i = 0; i < switches.Length; i++)
			{
				switches[i].RenderInteractiveElements(deltaTime);
			}
			api.Render.GlPopMatrix();
		}
		if (HoveredIndex >= 0)
		{
			api.Render.Render2DTexturePremultipliedAlpha(hoverTexture.TextureId, (int)Bounds.renderX + 1, (int)(Bounds.renderY + Bounds.InnerHeight + unscaledLineHeight * scaleMul * (double)HoveredIndex - (double)(int)scrollOffY + 1.0), (double)(int)expandedBoxWidth - GuiElement.scaled(10.0), (double)(int)unscaledLineHeight * scaleMul - 2.0, 311f);
		}
		api.Render.PopScissor();
		if (api.Render.ScissorStack.Count > 0)
		{
			api.Render.GlScissorFlag(enable: false);
		}
		api.Render.GlPushMatrix();
		api.Render.GlTranslate(0f, 0f, 200f);
		api.Render.Render2DTexturePremultipliedAlpha(scrollbarTexture.TextureId, (int)visibleBounds.renderX, (int)visibleBounds.renderY, scrollbarTexture.Width, scrollbarTexture.Height, 316f);
		scrollbar.Bounds.WithParent(Bounds);
		scrollbar.Bounds.absFixedY = Bounds.InnerHeight;
		scrollbar.RenderInteractiveElements(deltaTime);
		api.Render.GlPopMatrix();
		if (api.Render.ScissorStack.Count > 0)
		{
			api.Render.GlScissorFlag(enable: true);
		}
	}

	public override void OnKeyDown(ICoreClientAPI api, KeyEvent args)
	{
		if (!hasFocus)
		{
			return;
		}
		if ((args.KeyCode == 49 || args.KeyCode == 82) && expanded)
		{
			Close();
			SelectedIndex = HoveredIndex;
			onSelectionChanged?.Invoke(Values[SelectedIndex], selected: true);
			args.Handled = true;
		}
		else
		{
			if (args.KeyCode != 45 && args.KeyCode != 46 && args.KeyCode != 58 && args.KeyCode != 59)
			{
				return;
			}
			args.Handled = true;
			if (!expanded)
			{
				switch (args.KeyCode)
				{
				case 45:
					SelectedIndex = GameMath.Mod(SelectedIndex - 1, Values.Length);
					break;
				case 46:
					SelectedIndex = GameMath.Mod(SelectedIndex + 1, Values.Length);
					break;
				case 58:
					SelectedIndex = 0;
					break;
				case 59:
					SelectedIndex = Values.Length - 1;
					break;
				}
				onSelectionChanged?.Invoke(Values[SelectedIndex], selected: true);
				EnsureItemVisible(SelectedIndex);
			}
			else
			{
				switch (args.KeyCode)
				{
				case 45:
					HoveredIndex = GameMath.Mod(HoveredIndex - 1, Values.Length);
					break;
				case 46:
					HoveredIndex = GameMath.Mod(HoveredIndex + 1, Values.Length);
					break;
				case 58:
					HoveredIndex = 0;
					break;
				case 59:
					HoveredIndex = Values.Length - 1;
					break;
				}
				EnsureItemVisible(HoveredIndex);
			}
		}
	}

	public override void OnKeyPress(ICoreClientAPI api, KeyEvent args)
	{
		if (expanded)
		{
			for (int i = 0; i < Names.Length; i++)
			{
				int pos = (HoveredIndex + 1 + i) % Names.Length;
				if (Names[pos].ToLowerInvariant()[0] == (((char)args.KeyCode).ToString() ?? "").ToLowerInvariant()[0])
				{
					HoveredIndex = pos;
					EnsureItemVisible(pos);
					return;
				}
			}
		}
		base.OnKeyPress(api, args);
	}

	protected void EnsureItemVisible(int index)
	{
		double lineheight = unscaledLineHeight * Scale * (double)RuntimeEnv.GUIScale;
		double posY = (double)index * lineheight;
		double curIndex = scrollOffY / lineheight;
		scrollbar.EnsureVisible(0.0, posY + ((curIndex < (double)index) ? lineheight : 0.0));
	}

	public override void OnMouseMove(ICoreClientAPI api, MouseEvent args)
	{
		if (!expanded)
		{
			return;
		}
		int mouseX = api.Input.MouseX;
		int mouseY = api.Input.MouseY;
		double scaleMul = Scale * (double)RuntimeEnv.GUIScale;
		if (!((double)mouseX >= Bounds.renderX) || !((double)mouseX <= Bounds.renderX + expandedBoxWidth))
		{
			return;
		}
		if (scrollbar.mouseDownOnScrollbarHandle && (scrollbar.mouseDownOnScrollbarHandle || Bounds.renderX + expandedBoxWidth - (double)args.X < GuiElement.scaled(10.0)))
		{
			scrollbar.OnMouseMove(api, args);
			return;
		}
		int num = (int)(((double)mouseY - Bounds.renderY - Bounds.InnerHeight + scrollOffY) / (unscaledLineHeight * scaleMul));
		if (num >= 0 && num < Values.Length)
		{
			HoveredIndex = num;
			args.Handled = true;
		}
	}

	public override void OnMouseUp(ICoreClientAPI api, MouseEvent args)
	{
		base.OnMouseUp(api, args);
		if (expanded)
		{
			scrollbar.OnMouseUp(api, args);
		}
	}

	/// <summary>
	/// Opens the menu.
	/// </summary>
	public void Open()
	{
		ComposeDynamicElements();
		expanded = true;
	}

	internal void Close()
	{
		expanded = false;
	}

	public override void OnMouseDown(ICoreClientAPI api, MouseEvent args)
	{
		if (!expanded || !((double)args.X >= Bounds.renderX) || !((double)args.X <= Bounds.renderX + expandedBoxWidth))
		{
			return;
		}
		double scaleMul = Scale * (double)RuntimeEnv.GUIScale;
		if (Bounds.renderX + expandedBoxWidth - (double)args.X < GuiElement.scaled(10.0))
		{
			scrollbar.OnMouseDown(api, args);
			return;
		}
		double dy = (double)args.Y - Bounds.renderY - unscaledLineHeight * scaleMul;
		if (dy < 0.0 || dy > visibleBounds.OuterHeight)
		{
			Close();
			args.Handled = true;
			api.Gui.PlaySound("menubutton");
			return;
		}
		int selectedElement = (int)(((double)api.Input.MouseY - Bounds.renderY - Bounds.InnerHeight + scrollOffY) / (unscaledLineHeight * scaleMul));
		if (selectedElement >= 0 && selectedElement < Values.Length)
		{
			if (multiSelect)
			{
				switches[selectedElement].OnMouseDownOnElement(api, args);
				onSelectionChanged?.Invoke(Values[selectedElement], switches[selectedElement].On);
			}
			else
			{
				SelectedIndex = selectedElement;
				onSelectionChanged?.Invoke(Values[SelectedIndex], selected: true);
			}
			api.Gui.PlaySound("toggleswitch");
			if (!multiSelect)
			{
				Close();
			}
			args.Handled = true;
		}
	}

	public override void OnMouseWheel(ICoreClientAPI api, MouseWheelEventArgs args)
	{
		if (expanded && visibleBounds.PointInside(api.Input.MouseX, api.Input.MouseY))
		{
			scrollbar.OnMouseWheel(api, args);
		}
	}

	public override void OnFocusLost()
	{
		base.OnFocusLost();
		Close();
	}

	/// <summary>
	/// Sets the selected index.
	/// </summary>
	/// <param name="selectedIndex">The index to be set to.</param>
	public void SetSelectedIndex(int selectedIndex)
	{
		SelectedIndex = selectedIndex;
	}

	/// <summary>
	/// Sets the selected index to the given value.
	/// </summary>
	/// <param name="value">The value to be set to.</param>
	public void SetSelectedValue(params string[] value)
	{
		if (value == null)
		{
			SelectedIndices = Array.Empty<int>();
			return;
		}
		List<int> selectedIndices = new List<int>();
		for (int i = 0; i < Values.Length; i++)
		{
			if (multiSelect)
			{
				switches[i].On = false;
			}
			for (int j = 0; j < value.Length; j++)
			{
				if (Values[i] == value[j])
				{
					selectedIndices.Add(i);
					if (multiSelect)
					{
						switches[i].On = true;
					}
				}
			}
		}
		SelectedIndices = selectedIndices.ToArray();
	}

	/// <summary>
	/// Sets the list for the GUI Element list value.
	/// </summary>
	/// <param name="values">The values of the list.</param>
	/// <param name="names">The names of the values.</param>
	public void SetList(string[] values, string[] names)
	{
		Values = values;
		Names = names;
		ComposeDynamicElements();
	}

	public override void Dispose()
	{
		base.Dispose();
		hoverTexture.Dispose();
		dropDownTexture.Dispose();
		scrollbarTexture.Dispose();
		scrollbar?.Dispose();
	}
}
