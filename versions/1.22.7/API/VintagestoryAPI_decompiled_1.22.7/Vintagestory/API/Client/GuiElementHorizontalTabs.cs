using System;
using Cairo;
using Vintagestory.API.MathTools;

namespace Vintagestory.API.Client;

public class GuiElementHorizontalTabs : GuiElementTextBase
{
	private Action<int> handler;

	public GuiTab[] tabs;

	private LoadedTexture baseTexture;

	private LoadedTexture[] hoverTextures;

	private LoadedTexture[] notifyTextures;

	private LoadedTexture[] arrowTextures;

	private int[] tabWidths;

	private double[] tabOffsets;

	private CairoFont selectedFont;

	private double totalWidth;

	private double currentScrollOffset;

	public int activeElement;

	public double unscaledTabSpacing = 5.0;

	public double unscaledTabPadding = 4.0;

	public bool AlarmTabs;

	private float fontHeight;

	private CairoFont notifyFont;

	private double maxScrollOffset => Math.Max(0.0, totalWidth - Bounds.InnerWidth + GuiElement.scaled(unscaledTabSpacing));

	private bool displayLeftArrow => currentScrollOffset > GuiElement.scaled(unscaledTabSpacing);

	private bool displayRightArrow => currentScrollOffset < maxScrollOffset - GuiElement.scaled(unscaledTabSpacing);

	public bool[] TabHasAlarm { get; set; }

	public override bool Focusable => enabled;

	/// <summary>
	/// Creates a collection of horizontal tabs.
	/// </summary>
	/// <param name="capi">The client API</param>
	/// <param name="tabs">A collection of GUI tabs.</param>
	/// <param name="font">The font for the name of each tab.</param>
	/// <param name="selectedFont"></param>
	/// <param name="bounds">The bounds of each tab.</param>
	/// <param name="onTabClicked">The event fired whenever the tab is clicked.</param>
	public GuiElementHorizontalTabs(ICoreClientAPI capi, GuiTab[] tabs, CairoFont font, CairoFont selectedFont, ElementBounds bounds, Action<int> onTabClicked)
		: base(capi, "", font, bounds)
	{
		this.selectedFont = selectedFont;
		this.tabs = tabs;
		TabHasAlarm = new bool[tabs.Length];
		handler = onTabClicked;
		hoverTextures = new LoadedTexture[tabs.Length];
		for (int i = 0; i < tabs.Length; i++)
		{
			hoverTextures[i] = new LoadedTexture(capi);
		}
		arrowTextures = new LoadedTexture[2];
		arrowTextures[0] = new LoadedTexture(capi);
		arrowTextures[1] = new LoadedTexture(capi);
		tabWidths = new int[tabs.Length];
		tabOffsets = new double[tabs.Length];
		baseTexture = new LoadedTexture(capi);
	}

	[Obsolete("Use TabHasAlarm[] property instead. Used by the chat window to mark a tab/chat as unread")]
	public void SetAlarmTab(int tabIndex)
	{
	}

	public void WithAlarmTabs(CairoFont notifyFont)
	{
		this.notifyFont = notifyFont;
		notifyTextures = new LoadedTexture[tabs.Length];
		for (int i = 0; i < tabs.Length; i++)
		{
			notifyTextures[i] = new LoadedTexture(api);
		}
		AlarmTabs = true;
		ComposeOverlays(isNotifyTabs: true);
	}

	public override void ComposeTextElements(Context ctxStatic, ImageSurface surfaceStatic)
	{
		ImageSurface surface = new ImageSurface(Format.Argb32, (int)Bounds.InnerWidth + 1, (int)Bounds.InnerHeight + 1);
		Context ctx = new Context(surface);
		Font.SetupContext(ctx);
		fontHeight = (float)Font.GetFontExtents().Height;
		double radius = GuiElement.scaled(1.0);
		double spacing = GuiElement.scaled(unscaledTabSpacing);
		double padding = GuiElement.scaled(unscaledTabPadding);
		totalWidth = 0.0;
		for (int i = 0; i < tabs.Length; i++)
		{
			tabWidths[i] = (int)(ctx.TextExtents(tabs[i].Name).Width + 2.0 * padding + 1.0);
			totalWidth += spacing + (double)tabWidths[i];
		}
		ctx.Dispose();
		surface.Dispose();
		surface = new ImageSurface(Format.Argb32, (int)totalWidth + 1, (int)Bounds.InnerHeight + 1);
		ctx = new Context(surface);
		double xpos = spacing;
		Font.Color[3] = 0.5;
		for (int j = 0; j < tabs.Length; j++)
		{
			ctx.NewPath();
			ctx.MoveTo(xpos, Bounds.InnerHeight);
			ctx.LineTo(xpos, radius);
			ctx.Arc(xpos + radius, radius, radius, 3.1415927410125732, 4.71238899230957);
			ctx.Arc(xpos + (double)tabWidths[j] - radius, radius, radius, -1.5707963705062866, 0.0);
			ctx.LineTo(xpos + (double)tabWidths[j], Bounds.InnerHeight);
			ctx.ClosePath();
			double[] color = GuiStyle.DialogDefaultBgColor;
			ctx.SetSourceRGBA(color[0], color[1], color[2], color[3] * 0.75);
			ctx.FillPreserve();
			ShadePath(ctx);
			if (AlarmTabs)
			{
				notifyFont.SetupContext(ctx);
			}
			else
			{
				Font.SetupContext(ctx);
			}
			DrawTextLineAt(ctx, tabs[j].Name, xpos + padding, ((float)surface.Height - fontHeight) / 2f);
			xpos += (double)tabWidths[j] + spacing;
			tabOffsets[j] = xpos;
		}
		Font.Color[3] = 1.0;
		ComposeOverlays();
		ComposeArrows();
		generateTexture(surface, ref baseTexture);
		ctx.Dispose();
		surface.Dispose();
	}

	private void ComposeOverlays(bool isNotifyTabs = false)
	{
		double radius = GuiElement.scaled(1.0);
		GuiElement.scaled(unscaledTabSpacing);
		double padding = GuiElement.scaled(unscaledTabPadding);
		for (int i = 0; i < tabs.Length; i++)
		{
			ImageSurface surface = new ImageSurface(Format.Argb32, tabWidths[i], (int)Bounds.InnerHeight + 1);
			Context ctx = genContext(surface);
			double degrees = Math.PI / 180.0;
			ctx.SetSourceRGBA(1.0, 1.0, 1.0, 0.0);
			ctx.Paint();
			ctx.NewPath();
			ctx.MoveTo(0.0, Bounds.InnerHeight + 1.0);
			ctx.LineTo(0.0, radius);
			ctx.Arc(radius, radius, radius, 180.0 * degrees, 270.0 * degrees);
			ctx.Arc((double)tabWidths[i] - radius, radius, radius, -90.0 * degrees, 0.0 * degrees);
			ctx.LineTo(tabWidths[i], surface.Height);
			ctx.ClosePath();
			double[] color = GuiStyle.DialogDefaultBgColor;
			ctx.SetSourceRGBA(color[0], color[1], color[2], color[3] * 0.75);
			ctx.FillPreserve();
			ctx.SetSourceRGBA(color[0] * 1.6, color[1] * 1.6, color[2] * 1.6, 1.0);
			ctx.LineWidth = 3.5;
			ctx.StrokePreserve();
			surface.BlurPartial(5.2, 10);
			ctx.SetSourceRGBA(color[0], color[1], color[2], color[3] * 0.75);
			ctx.LineWidth = 1.0;
			ctx.StrokePreserve();
			ctx.NewPath();
			ctx.MoveTo(0.0, Bounds.InnerHeight);
			ctx.LineTo(0.0, radius);
			ctx.Arc(radius, radius, radius, 180.0 * degrees, 270.0 * degrees);
			ctx.Arc((double)tabWidths[i] - radius, radius, radius, -90.0 * degrees, 0.0 * degrees);
			ctx.LineTo(tabWidths[i], Bounds.InnerHeight);
			ShadePath(ctx);
			if (isNotifyTabs)
			{
				notifyFont.SetupContext(ctx);
			}
			else
			{
				selectedFont.SetupContext(ctx);
			}
			ctx.Operator = Operator.Clear;
			ctx.Rectangle(0.0, surface.Height - 1, surface.Width, 1.0);
			ctx.Fill();
			ctx.Operator = Operator.Over;
			DrawTextLineAt(ctx, tabs[i].Name, padding, ((float)surface.Height - fontHeight) / 2f);
			if (isNotifyTabs)
			{
				generateTexture(surface, ref notifyTextures[i]);
			}
			else
			{
				generateTexture(surface, ref hoverTextures[i]);
			}
			ctx.Dispose();
			surface.Dispose();
		}
	}

	private void ComposeArrows()
	{
		ImageSurface surface = new ImageSurface(Format.Argb32, (int)((Bounds.InnerHeight - 2.0) / 2.0), (int)Bounds.InnerHeight - 2);
		Context ctx = genContext(surface);
		ctx.SetSourceRGBA(ColorUtil.Hex2Doubles("#a88b6c", 1.0));
		GuiElement.RoundRectangle(ctx, 0.0, 0.0, surface.Width, surface.Height, 1.0);
		ctx.Fill();
		EmbossRoundRectangleElement(ctx, 0.0, 0.0, surface.Width, surface.Height, inverse: false, 2, 1);
		ctx.NewPath();
		ctx.LineTo(GuiElement.scaled(1.0) * Scale + 1.0, (double)surface.Height - GuiElement.scaled(9.5) * Scale);
		ctx.LineTo(((double)surface.Width - GuiElement.scaled(2.0) - 1.0) * Scale, (double)surface.Height - GuiElement.scaled(14.25) * Scale);
		ctx.LineTo(((double)surface.Width - GuiElement.scaled(2.0) - 1.0) * Scale, (double)surface.Height - GuiElement.scaled(4.75) * Scale);
		ctx.ClosePath();
		ctx.SetSourceRGBA(1.0, 1.0, 1.0, 1.0);
		ctx.Fill();
		generateTexture(surface, ref arrowTextures[0]);
		surface.Dispose();
		ctx.Dispose();
		surface = new ImageSurface(Format.Argb32, (int)((Bounds.InnerHeight - 2.0) / 2.0), (int)Bounds.InnerHeight - 2);
		ctx = genContext(surface);
		ctx.SetSourceRGBA(ColorUtil.Hex2Doubles("#a88b6c", 1.0));
		GuiElement.RoundRectangle(ctx, 0.0, 0.0, surface.Width, surface.Height, 1.0);
		ctx.Fill();
		EmbossRoundRectangleElement(ctx, 0.0, 0.0, surface.Width, surface.Height, inverse: false, 2, 1);
		ctx.NewPath();
		ctx.LineTo(((double)surface.Width - GuiElement.scaled(2.0) - 1.0) * Scale, (double)surface.Height - GuiElement.scaled(9.5) * Scale);
		ctx.LineTo(GuiElement.scaled(1.0) * Scale + 1.0, (double)surface.Height - GuiElement.scaled(14.25) * Scale);
		ctx.LineTo(GuiElement.scaled(1.0) * Scale + 1.0, (double)surface.Height - GuiElement.scaled(4.75) * Scale);
		ctx.ClosePath();
		ctx.SetSourceRGBA(1.0, 1.0, 1.0, 1.0);
		ctx.Fill();
		generateTexture(surface, ref arrowTextures[1]);
		ctx.Dispose();
		surface.Dispose();
	}

	public override void RenderInteractiveElements(float deltaTime)
	{
		api.Render.PushScissor(Bounds, stacking: true);
		api.Render.Render2DTexture(baseTexture.TextureId, (int)(Bounds.renderX - currentScrollOffset), (int)Bounds.renderY, (int)totalWidth + 1, (int)Bounds.InnerHeight + 1);
		api.Render.PopScissor();
		double spacing = GuiElement.scaled(unscaledTabSpacing);
		int mouseRelX = api.Input.MouseX - (int)Bounds.absX;
		int mouseRelY = api.Input.MouseY - (int)Bounds.absY;
		double xpos = spacing;
		for (int i = 0; i < tabs.Length; i++)
		{
			if ((i == activeElement || ((double)mouseRelX > xpos - currentScrollOffset && (double)mouseRelX < xpos + (double)tabWidths[i] - currentScrollOffset && mouseRelY > 0 && (double)mouseRelY < Bounds.InnerHeight && mouseRelX > 0 && (double)mouseRelX < Bounds.InnerWidth)) && (i == activeElement || ((!displayLeftArrow || mouseRelX <= 0 || mouseRelX >= arrowTextures[0].Width) && (!displayRightArrow || !((double)mouseRelX > Bounds.InnerWidth - (double)arrowTextures[1].Width) || !((double)mouseRelX < Bounds.InnerWidth)))))
			{
				api.Render.PushScissor(Bounds, stacking: true);
				api.Render.Render2DTexturePremultipliedAlpha(hoverTextures[i].TextureId, (int)((double)(int)(Bounds.renderX - currentScrollOffset) + xpos), (int)Bounds.renderY, tabWidths[i], (int)Bounds.InnerHeight + 1);
				api.Render.PopScissor();
			}
			if (TabHasAlarm[i])
			{
				api.Render.PushScissor(Bounds, stacking: true);
				api.Render.Render2DTexturePremultipliedAlpha(notifyTextures[i].TextureId, (int)((double)(int)(Bounds.renderX - currentScrollOffset) + xpos), (int)Bounds.renderY, tabWidths[i], (int)Bounds.InnerHeight + 1);
				api.Render.PopScissor();
			}
			xpos += (double)tabWidths[i] + spacing;
		}
		if (displayLeftArrow)
		{
			api.Render.Render2DTexturePremultipliedAlpha(arrowTextures[0].TextureId, (int)Bounds.renderX, (int)Bounds.renderY + 1, (int)((Bounds.InnerHeight - 2.0) / 2.0), (int)Bounds.InnerHeight - 2);
		}
		if (displayRightArrow)
		{
			api.Render.Render2DTexturePremultipliedAlpha(arrowTextures[1].TextureId, (double)(int)Bounds.renderX + Bounds.InnerWidth - (double)arrowTextures[1].Width, (int)Bounds.renderY + 1, (int)((Bounds.InnerHeight - 2.0) / 2.0), (int)Bounds.InnerHeight - 2);
		}
	}

	public override void OnKeyDown(ICoreClientAPI api, KeyEvent args)
	{
		if (base.HasFocus)
		{
			if (args.KeyCode == 48)
			{
				args.Handled = true;
				SetValue((activeElement + 1) % tabs.Length);
			}
			if (args.KeyCode == 47)
			{
				SetValue(GameMath.Mod(activeElement - 1, tabs.Length));
				args.Handled = true;
			}
		}
	}

	public override void OnMouseWheel(ICoreClientAPI api, MouseWheelEventArgs args)
	{
		if (enabled && Bounds.PointInside(api.Input.MouseX, api.Input.MouseY))
		{
			args.SetHandled();
			double dir = (double)args.deltaPrecise * GuiElement.scaled(10.0);
			if ((!(currentScrollOffset <= 0.0) || !(dir < 0.0)) && (!(currentScrollOffset >= maxScrollOffset) || !(dir > 0.0)))
			{
				currentScrollOffset = Math.Clamp(currentScrollOffset + dir, 0.0, maxScrollOffset);
			}
		}
	}

	public override void OnMouseDownOnElement(ICoreClientAPI api, MouseEvent args)
	{
		base.OnMouseDownOnElement(api, args);
		double spacing = GuiElement.scaled(unscaledTabSpacing);
		double xpos = spacing;
		int mouseRelX = api.Input.MouseX - (int)Bounds.absX;
		int mouseRelY = api.Input.MouseY - (int)Bounds.absY;
		if (displayLeftArrow && mouseRelX > 0 && mouseRelX < arrowTextures[0].Width)
		{
			currentScrollOffset -= GuiElement.scaled(5.0);
			return;
		}
		if (displayRightArrow && (double)mouseRelX > Bounds.InnerWidth - (double)arrowTextures[1].Width && (double)mouseRelX < Bounds.InnerWidth)
		{
			currentScrollOffset += GuiElement.scaled(5.0);
			return;
		}
		for (int i = 0; i < tabs.Length; i++)
		{
			if ((double)mouseRelX > xpos - currentScrollOffset && (double)mouseRelX < xpos + (double)tabWidths[i] - currentScrollOffset && mouseRelY > 0 && (double)mouseRelY < Bounds.InnerHeight)
			{
				SetValue(i);
				break;
			}
			xpos += (double)tabWidths[i] + spacing;
		}
	}

	/// <summary>
	/// Sets the current tab to the given index.
	/// </summary>
	/// <param name="selectedIndex">The current index of the tab.</param>
	/// <param name="callhandler"></param>
	public void SetValue(int selectedIndex, bool callhandler = true)
	{
		if (callhandler)
		{
			handler(tabs[selectedIndex].DataInt);
			api.Gui.PlaySound("menubutton_wood");
		}
		activeElement = selectedIndex;
		double newOffset = tabOffsets[activeElement] - Bounds.InnerWidth + (double)arrowTextures[1].Width;
		if (currentScrollOffset < newOffset)
		{
			currentScrollOffset = Math.Clamp(newOffset, 0.0, maxScrollOffset);
		}
		newOffset = tabOffsets[activeElement] - (double)tabWidths[activeElement] - GuiElement.scaled(unscaledTabSpacing) - (double)arrowTextures[0].Width;
		if (currentScrollOffset > newOffset)
		{
			currentScrollOffset = Math.Clamp(newOffset, 0.0, maxScrollOffset);
		}
	}

	public override void Dispose()
	{
		base.Dispose();
		baseTexture?.Dispose();
		LoadedTexture[] array = arrowTextures;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Dispose();
		}
		for (int j = 0; j < hoverTextures.Length; j++)
		{
			hoverTextures[j].Dispose();
			if (notifyTextures != null)
			{
				notifyTextures[j].Dispose();
			}
		}
	}
}
