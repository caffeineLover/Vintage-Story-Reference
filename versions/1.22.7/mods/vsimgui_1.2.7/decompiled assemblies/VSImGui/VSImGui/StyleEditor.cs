using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using ImGuiNET;
using Newtonsoft.Json;
using VSImGui.API;

namespace VSImGui
{
	// Token: 0x0200000B RID: 11
	[NullableContext(1)]
	[Nullable(0)]
	public class StyleEditor
	{
		// Token: 0x06000150 RID: 336 RVA: 0x0000870C File Offset: 0x0000690C
		public StyleEditor(Style style)
		{
			this.mStyle = style;
			this.mBackup = new Style(style);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00008774 File Offset: 0x00006974
		public bool Draw()
		{
			StyleEditor.sIdCounter = 0U;
			bool opened = true;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(21, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Style editor##vsimgui");
			defaultInterpolatedStringHandler.AppendFormatted<Guid>(this.mId);
			if (!ImGui.Begin(MemoryExtensions.AsSpan(defaultInterpolatedStringHandler.ToStringAndClear()), ref opened, 1024))
			{
				return false;
			}
			if (ImGui.BeginMenuBar())
			{
				ImGui.BeginDisabled();
				if (ImGui.MenuItem(MemoryExtensions.AsSpan("Save##StyleEditor.vsimgui")))
				{
					this.Save();
				}
				if (ImGui.MenuItem(MemoryExtensions.AsSpan("Load##StyleEditor.vsimgui")))
				{
					this.Load();
				}
				ImGui.EndDisabled();
				if (ImGui.MenuItem(MemoryExtensions.AsSpan("Restore##StyleEditor.vsimgui")))
				{
					this.mStyle.SetFrom(this.mBackup);
				}
				this.Code(ImGui.MenuItem(MemoryExtensions.AsSpan("Code##StyleEditor.vsimgui")));
				ImGui.EndMenuBar();
			}
			if (ImGui.BeginTabBar(MemoryExtensions.AsSpan("##StyleEditor.vsimgui.tabs"), 0))
			{
				if (ImGui.BeginTabItem(MemoryExtensions.AsSpan("Font & texture")))
				{
					StyleEditor.FontAndTextureEditor(this.mStyle);
					ImGui.EndTabItem();
				}
				if (ImGui.BeginTabItem(MemoryExtensions.AsSpan("Colors")))
				{
					StyleEditor.ColorsEditor(this.mStyle, ref this.mColorsFilter);
					ImGui.EndTabItem();
				}
				if (ImGui.BeginTabItem(MemoryExtensions.AsSpan("Layout")))
				{
					StyleEditor.LayoutSettingsEditor(this.mStyle, ref this.mLayoutFilter);
					ImGui.EndTabItem();
				}
				WindowStyle windowStyle = this.mStyle as WindowStyle;
				if (windowStyle != null && ImGui.BeginTabItem(MemoryExtensions.AsSpan("Window")))
				{
					windowStyle.Flags.DrawEditor("Window flags");
					ImGui.EndTabItem();
				}
				ImGui.EndTabBar();
			}
			ImGui.End();
			return opened;
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00008907 File Offset: 0x00006B07
		public void Save()
		{
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00008909 File Offset: 0x00006B09
		public void Load()
		{
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0000890C File Offset: 0x00006B0C
		public void Code(bool open)
		{
			if (open)
			{
				ImGui.OpenPopup(MemoryExtensions.AsSpan("Output"));
				this.mSerialized = JsonConvert.SerializeObject(this.mStyle, 1);
				this.mCsharpCode = this.mStyle.ToCode();
				this.mOutputOpened = true;
			}
			if (this.mOutputOpened && ImGui.BeginPopupModal(MemoryExtensions.AsSpan("Output"), ref this.mOutputOpened))
			{
				ImGui.RadioButton(MemoryExtensions.AsSpan("JSON##codeoutputvsimgui"), ref this.mOutputOption, 0);
				ImGui.SameLine();
				ImGui.RadioButton(MemoryExtensions.AsSpan("C# ##codeoutputvsimgui"), ref this.mOutputOption, 1);
				ImGui.BeginChild(MemoryExtensions.AsSpan("##codeoutputvsimguijson"));
				int num = this.mOutputOption;
				if (num != 0)
				{
					if (num == 1)
					{
						ImGui.Separator();
						this.CsharpCode();
					}
				}
				else
				{
					this.JsonCode();
				}
				ImGui.End();
				ImGui.EndPopup();
			}
		}

		// Token: 0x06000155 RID: 341 RVA: 0x000089E8 File Offset: 0x00006BE8
		private void CsharpCode()
		{
			Vector2 size = ImGui.GetWindowSize();
			size.X -= 10f;
			size.Y -= 94f;
			if (ImGui.Button(MemoryExtensions.AsSpan("Copy##resultjsonoutputvsimgui")) || this.CopyCombination())
			{
				ImGui.SetClipboardText(MemoryExtensions.AsSpan(this.mCsharpCode));
			}
			ImGui.InputTextMultiline(MemoryExtensions.AsSpan("##C#resultjsonoutputvsimgui"), ref this.mCsharpCode, (uint)(this.mCsharpCode.Length * 2), size, 16384);
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00008A70 File Offset: 0x00006C70
		private void JsonCode()
		{
			Vector2 size = ImGui.GetWindowSize();
			size.X -= 10f;
			size.Y -= 124f;
			if (ImGui.Button(MemoryExtensions.AsSpan("Apply##jsonoutputvsimgui")))
			{
				this.Deserialize(this.mSerialized);
			}
			ImGui.SameLine();
			if (ImGui.Button(MemoryExtensions.AsSpan("Copy##resultjsonoutputvsimgui")) || this.CopyCombination())
			{
				ImGui.SetClipboardText(MemoryExtensions.AsSpan(this.mSerialized));
			}
			ImGui.SameLine();
			if (ImGui.Button(MemoryExtensions.AsSpan("Paste##resultjsonoutputvsimgui")) || this.PasteCombination())
			{
				this.mSerialized = ImGui.GetClipboardText();
			}
			if (this.mException)
			{
				this.ShowException(ref size);
			}
			ImGui.InputTextMultiline(MemoryExtensions.AsSpan("##JSONresultjsonoutputvsimgui"), ref this.mSerialized, (uint)(this.mSerialized.Length * 2), size);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00008B4C File Offset: 0x00006D4C
		private unsafe bool CopyCombination()
		{
			ImGuiIOPtr io = ImGui.GetIO();
			return *io.KeyCtrl && *io.KeysDown[548];
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00008B80 File Offset: 0x00006D80
		private unsafe bool PasteCombination()
		{
			ImGuiIOPtr io = ImGui.GetIO();
			return *io.KeyCtrl && *io.KeysDown[567];
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00008BB4 File Offset: 0x00006DB4
		private unsafe bool SelectAllCombination()
		{
			ImGuiIOPtr io = ImGui.GetIO();
			return *io.KeyCtrl && *io.KeysDown[546];
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00008BE8 File Offset: 0x00006DE8
		private void Deserialize(string json)
		{
			try
			{
				this.mException = false;
				WindowStyle windowStyle = this.mStyle as WindowStyle;
				if (windowStyle != null)
				{
					WindowStyle style = JsonConvert.DeserializeObject<WindowStyle>(json);
					style.OnDeserilization(windowStyle.Api);
					this.mStyle.SetFrom(style);
				}
				else
				{
					Style style2 = JsonConvert.DeserializeObject<Style>(json);
					this.mStyle.SetFrom(style2);
				}
			}
			catch (Exception exception)
			{
				this.mException = true;
				this.mMessage = "Error: " + exception.Message + "\n\nStack trace: " + exception.StackTrace;
			}
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00008C7C File Offset: 0x00006E7C
		private void ShowException(ref Vector2 size)
		{
			Vector2 errorSize = ImGui.GetWindowSize();
			errorSize.X -= 10f;
			errorSize.Y = 100f;
			size.Y -= 104f;
			ImGui.BeginChild(MemoryExtensions.AsSpan("Apply##Exceptionjsonoutputvsimgui"), errorSize, true);
			ImGui.PushTextWrapPos(ImGui.GetCursorPosX() + errorSize.X);
			ImGui.Text(MemoryExtensions.AsSpan(this.mMessage));
			ImGui.PopTextWrapPos();
			ImGui.EndChild();
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00008CF8 File Offset: 0x00006EF8
		private static void FontAndTextureEditor(Style style)
		{
			string[] fonts = Enumerable.ToArray<string>(Enumerable.Prepend<string>(Enumerable.ToArray<string>(FontManager.Fonts), ""));
			string[] fontsNames = Enumerable.ToArray<string>(Enumerable.Prepend<string>(Enumerable.ToArray<string>(Enumerable.Select<string, string>(FontManager.Fonts, (string font) => Path.GetFileNameWithoutExtension(font))), ""));
			int[] sizes = FontManager.Sizes.ToArray();
			string[] sizesNames = Enumerable.ToArray<string>(Enumerable.Select<int, string>(FontManager.Sizes, delegate(int size)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(2, 1);
				defaultInterpolatedStringHandler.AppendFormatted<int>(size);
				defaultInterpolatedStringHandler.AppendLiteral("px");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}));
			int currentFontIndex = 0;
			while (currentFontIndex < fonts.Length && !(fontsNames[currentFontIndex] == style.FontName))
			{
				currentFontIndex++;
			}
			if (currentFontIndex >= fonts.Length)
			{
				currentFontIndex = 0;
			}
			int currentSizeIndex = 0;
			while (currentSizeIndex < sizes.Length && sizes[currentSizeIndex] != style.FontSize)
			{
				currentSizeIndex++;
			}
			if (currentSizeIndex >= sizes.Length)
			{
				currentSizeIndex = 0;
			}
			ImGui.Combo(MemoryExtensions.AsSpan("Font"), ref currentFontIndex, fontsNames, fonts.Length);
			ImGui.Combo(MemoryExtensions.AsSpan("Size"), ref currentSizeIndex, sizesNames, sizes.Length);
			style.Font = new ValueTuple<string, int>(fontsNames[currentFontIndex], sizes[currentSizeIndex]);
			WindowStyle windowStyle = style as WindowStyle;
			if (windowStyle != null)
			{
				if (!windowStyle.BackgroundTextureApplied && ImGui.Button(MemoryExtensions.AsSpan("Reenable texture")))
				{
					windowStyle.BackgroundTextureApplied = true;
				}
				else if (windowStyle.BackgroundTextureApplied && ImGui.Button(MemoryExtensions.AsSpan("Disable texture")))
				{
					windowStyle.BackgroundTextureApplied = false;
				}
				string texture = (string)windowStyle.BackgroundTexture.Clone();
				if (ImGui.InputText(MemoryExtensions.AsSpan("Texture path with domain"), ref texture, 1000U, 32))
				{
					windowStyle.BackgroundTexture = texture;
				}
				Vector2 size2 = windowStyle.BackgroundTextureSize;
				ImGui.InputFloat2(MemoryExtensions.AsSpan("Texture size"), ref size2);
				windowStyle.BackgroundTextureSize = size2;
			}
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00008EE4 File Offset: 0x000070E4
		private static void ColorsEditor(Style style, ref string filter)
		{
			ImGui.InputTextWithHint(MemoryExtensions.AsSpan("Filter colors"), MemoryExtensions.AsSpan("filter, supports wildcards"), ref filter, 100U);
			string filter2 = StyleEditor.WildCardToRegular(filter);
			ImGui.BeginChild(MemoryExtensions.AsSpan("Filtered colors"), default(Vector2), true);
			StyleEditor.BackgroundColorsEditor(filter2, style);
			StyleEditor.TextEditor(filter2, style);
			StyleEditor.BordersEditor(filter2, style);
			StyleEditor.ScrollEditor(filter2, style);
			StyleEditor.ButtonsEditor(filter2, style);
			StyleEditor.HeadersEditor(filter2, style);
			StyleEditor.SeparatorEditor(filter2, style);
			StyleEditor.ResizeEditor(filter2, style);
			StyleEditor.TabsEditor(filter2, style);
			StyleEditor.PlotsEditor(filter2, style);
			StyleEditor.OtherColorsEditor(filter2, style);
			ImGui.EndChild();
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00008F80 File Offset: 0x00007180
		private static void LayoutSettingsEditor(Style style, ref string filter)
		{
			ImGui.InputTextWithHint(MemoryExtensions.AsSpan("Filter settings"), MemoryExtensions.AsSpan("filter, supports wildcards"), ref filter, 100U);
			string filter2 = StyleEditor.WildCardToRegular(filter);
			ImGui.BeginChild(MemoryExtensions.AsSpan("Filtered settings"), default(Vector2), true);
			StyleEditor.SizesEditor(filter2, style);
			StyleEditor.SpacingEditor(filter2, style);
			StyleEditor.BordersLayoutEditor(filter2, style);
			StyleEditor.PaddingsEditor(filter2, style);
			StyleEditor.RoundingEditor(filter2, style);
			StyleEditor.AlignEditor(filter2, style);
			StyleEditor.HoverEditor(filter2, style);
			StyleEditor.RenderingEditor(filter2, style);
			StyleEditor.OtherEditor(filter2, style);
			ImGui.EndChild();
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00009010 File Offset: 0x00007210
		private static void PaddingsEditor(string filter, Style style)
		{
			if (!StyleEditor.Match(filter, new string[]
			{
				"Window",
				"Frame",
				"Cell",
				"TouchExtra",
				"SeparatorText",
				"DisplayWindow",
				"DisplaySafeArea"
			}))
			{
				return;
			}
			ImGui.SeparatorText(MemoryExtensions.AsSpan("Paddings"));
			style.PaddingWindow = StyleEditor.Pair("PaddingWindow", "Window", filter, style.PaddingWindow);
			style.PaddingFrame = StyleEditor.Pair("PaddingFrame", "Frame", filter, style.PaddingFrame);
			style.PaddingCell = StyleEditor.Pair("PaddingCell", "Cell", filter, style.PaddingCell);
			style.PaddingTouchExtra = StyleEditor.Pair("PaddingTouchExtra", "TouchExtra", filter, style.PaddingTouchExtra);
			style.PaddingSeparatorText = StyleEditor.Pair("PaddingSeparatorText", "SeparatorText", filter, style.PaddingSeparatorText);
			style.PaddingDisplayWindow = StyleEditor.Pair("PaddingDisplayWindow", "DisplayWindow", filter, style.PaddingDisplayWindow);
			style.PaddingDisplaySafeArea = StyleEditor.Pair("PaddingDisplaySafeArea", "DisplaySafeArea", filter, style.PaddingDisplaySafeArea);
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00009138 File Offset: 0x00007338
		private static void SpacingEditor(string filter, Style style)
		{
			if (!StyleEditor.Match(filter, new string[]
			{
				"Item",
				"ItemInner",
				"Indent",
				"ColumnsMin"
			}))
			{
				return;
			}
			ImGui.SeparatorText(MemoryExtensions.AsSpan("Spacing"));
			style.SpacingItem = StyleEditor.Pair("SpacingItem", "Item", filter, style.SpacingItem);
			style.SpacingItemInner = StyleEditor.Pair("SpacingItemInner", "ItemInner", filter, style.SpacingItemInner);
			style.SpacingIndent = StyleEditor.Float("SpacingIndent", "Indent", filter, style.SpacingIndent);
			style.SpacingColumnsMin = StyleEditor.Float("SpacingColumnsMin", "ColumnsMin", filter, style.SpacingColumnsMin);
		}

		// Token: 0x06000161 RID: 353 RVA: 0x000091F4 File Offset: 0x000073F4
		private static void BordersLayoutEditor(string filter, Style style)
		{
			if (!StyleEditor.Match(filter, new string[]
			{
				"Window",
				"Child",
				"Popup",
				"Frame",
				"Tab",
				"SeparatorText"
			}))
			{
				return;
			}
			ImGui.SeparatorText(MemoryExtensions.AsSpan("Borders"));
			style.BorderWindow = StyleEditor.Float("BorderWindow", "Window", filter, style.BorderWindow);
			style.BorderChild = StyleEditor.Float("BorderChild", "Child", filter, style.BorderChild);
			style.BorderPopup = StyleEditor.Float("BorderPopup", "Popup", filter, style.BorderPopup);
			style.BorderFrame = StyleEditor.Float("BorderFrame", "Frame", filter, style.BorderFrame);
			style.BorderTab = StyleEditor.Float("BorderTab", "Tab", filter, style.BorderTab);
			style.BorderSeparatorText = StyleEditor.Float("BorderSeparatorText", "SeparatorText", filter, style.BorderSeparatorText);
		}

		// Token: 0x06000162 RID: 354 RVA: 0x000092F8 File Offset: 0x000074F8
		private static void RoundingEditor(string filter, Style style)
		{
			if (!StyleEditor.Match(filter, new string[]
			{
				"Window",
				"Child",
				"Popup",
				"Frame",
				"Scrollbar",
				"Grab",
				"Tab"
			}))
			{
				return;
			}
			ImGui.SeparatorText(MemoryExtensions.AsSpan("Rounding"));
			style.RoundingWindow = StyleEditor.Float("RoundingWindow", "Window", filter, style.RoundingWindow);
			style.RoundingChild = StyleEditor.Float("RoundingChild", "Child", filter, style.RoundingChild);
			style.RoundingPopup = StyleEditor.Float("RoundingPopup", "Popup", filter, style.RoundingPopup);
			style.RoundingFrame = StyleEditor.Float("RoundingFrame", "Frame", filter, style.RoundingFrame);
			style.RoundingScrollbar = StyleEditor.Float("RoundingScrollbar", "Scrollbar", filter, style.RoundingScrollbar);
			style.RoundingGrab = StyleEditor.Float("RoundingGrab", "Grab", filter, style.RoundingGrab);
			style.RoundingTab = StyleEditor.Float("RoundingTab", "Tab", filter, style.RoundingTab);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00009420 File Offset: 0x00007620
		private static void SizesEditor(string filter, Style style)
		{
			if (!StyleEditor.Match(filter, new string[]
			{
				"WindowMin",
				"Scrollbar",
				"GrabMin"
			}))
			{
				return;
			}
			ImGui.SeparatorText(MemoryExtensions.AsSpan("Sizes"));
			style.SizeWindowMin = StyleEditor.Pair("SizeWindowMin", "WindowMin", filter, style.SizeWindowMin);
			style.SizeScrollbar = StyleEditor.Float("SizeScrollbar", "Scrollbar", filter, style.SizeScrollbar);
			style.SizeGrabMin = StyleEditor.Float("SizeGrabMin", "GrabMin", filter, style.SizeGrabMin);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x000094B8 File Offset: 0x000076B8
		private static void AlignEditor(string filter, Style style)
		{
			if (!StyleEditor.Match(filter, new string[]
			{
				"WindowTitle",
				"ButtonText",
				"SelectableText",
				"SeparatorText"
			}))
			{
				return;
			}
			ImGui.SeparatorText(MemoryExtensions.AsSpan("Align"));
			style.AlignWindowTitle = StyleEditor.Pair("AlignWindowTitle", "WindowTitle", filter, style.AlignWindowTitle);
			style.AlignButtonText = StyleEditor.Pair("AlignButtonText", "ButtonText", filter, style.AlignButtonText);
			style.AlignSelectableText = StyleEditor.Pair("AlignSelectableText", "SelectableText", filter, style.AlignSelectableText);
			style.AlignSeparatorText = StyleEditor.Pair("AlignSeparatorText", "SeparatorText", filter, style.AlignSeparatorText);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00009574 File Offset: 0x00007774
		private static void RenderingEditor(string filter, Style style)
		{
			if (!StyleEditor.Match(filter, new string[]
			{
				"Alpha",
				"DisabledAlpha",
				"AntiAliasedLines",
				"AntiAliasedLinesUseTex",
				"AntiAliasedFill",
				"CurveTessellationTol",
				"CircleTessellationMaxError"
			}))
			{
				return;
			}
			ImGui.SeparatorText(MemoryExtensions.AsSpan("Rendering"));
			style.Alpha = StyleEditor.Float("Alpha", "Alpha", filter, style.Alpha);
			style.DisabledAlpha = StyleEditor.Float("DisabledAlpha", "DisabledAlpha", filter, style.DisabledAlpha);
			style.AntiAliasedLines = StyleEditor.Boolean("AntiAliasedLines", "AntiAliasedLines", filter, style.AntiAliasedLines);
			style.AntiAliasedLinesUseTex = StyleEditor.Boolean("AntiAliasedLinesUseTex", "AntiAliasedLinesUseTex", filter, style.AntiAliasedLinesUseTex);
			style.AntiAliasedFill = StyleEditor.Boolean("AntiAliasedFill", "AntiAliasedFill", filter, style.AntiAliasedFill);
			style.CurveTessellationTol = StyleEditor.Float("CurveTessellationTol", "CurveTessellationTol", filter, style.CurveTessellationTol);
			style.CircleTessellationMaxError = StyleEditor.Float("CircleTessellationMaxError", "CircleTessellationMaxError", filter, style.CircleTessellationMaxError);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0000969C File Offset: 0x0000789C
		private static void HoverEditor(string filter, Style style)
		{
			if (!StyleEditor.Match(filter, new string[]
			{
				"StationaryDelay",
				"DelayShort",
				"DelayNormal",
				"FlagsForTooltipMouse",
				"FlagsForTooltipNav"
			}))
			{
				return;
			}
			ImGui.SeparatorText(MemoryExtensions.AsSpan("Hover"));
			style.HoverStationaryDelay = StyleEditor.Float("HoverStationaryDelay", "StationaryDelay", filter, style.HoverStationaryDelay);
			style.HoverDelayShort = StyleEditor.Float("HoverDelayShort", "DelayShort", filter, style.HoverDelayShort);
			style.HoverDelayNormal = StyleEditor.Float("HoverDelayNormal", "DelayNormal", filter, style.HoverDelayNormal);
			style.HoverFlagsForTooltipMouse = StyleEditor.HoveredFlag("HoverFlagsForTooltipMouse", "FlagsForTooltipMouse", filter, style.HoverFlagsForTooltipMouse);
			style.HoverFlagsForTooltipNav = StyleEditor.HoveredFlag("HoverFlagsForTooltipNav", "FlagsForTooltipNav", filter, style.HoverFlagsForTooltipNav);
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0000977C File Offset: 0x0000797C
		private static void OtherEditor(string filter, Style style)
		{
			if (!StyleEditor.Match(filter, new string[]
			{
				"MenuButtonPosition",
				"ButtonPosition",
				"SliderDeadzone",
				"MinWidthForCloseButton",
				"CursorScale"
			}))
			{
				return;
			}
			ImGui.SeparatorText(MemoryExtensions.AsSpan("Other"));
			style.WindowMenuButtonPosition = StyleEditor.Direction("WindowMenuButtonPosition", "WindowMenuButtonPosition", filter, style.WindowMenuButtonPosition);
			style.ColorButtonPosition = StyleEditor.Direction("ColorButtonPosition", "ColorButtonPosition", filter, style.ColorButtonPosition);
			style.LogSliderDeadzone = StyleEditor.Float("LogSliderDeadzone", "LogSliderDeadzone", filter, style.LogSliderDeadzone);
			style.TabMinWidthForCloseButton = StyleEditor.Float("TabMinWidthForCloseButton", "TabMinWidthForCloseButton", filter, style.TabMinWidthForCloseButton);
			style.MouseCursorScale = StyleEditor.Float("MouseCursorScale", "MouseCursorScale", filter, style.MouseCursorScale);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0000985C File Offset: 0x00007A5C
		private static void BackgroundColorsEditor(string filter, Style style)
		{
			if (!StyleEditor.Match(filter, new string[]
			{
				"BackgroundWindow",
				"BackgroundChild",
				"BackgroundPopup",
				"BackgroundFrame",
				"BackgroundFrameHovered",
				"BackgroundFrameActive",
				"BackgroundTitle",
				"BackgroundTitleActive",
				"BackgroundTitleCollapsed",
				"BackgroundMenuBar",
				"BackgroundScrollbar",
				"BackgroundDockingEmpty",
				"BackgroundTableHeader",
				"BackgroundTableRow",
				"BackgroundTableRowAlt",
				"BackgroundTextSelected",
				"BackgroundNavWindowingDim",
				"BackgroundModalWindowDim"
			}))
			{
				return;
			}
			ImGui.SeparatorText(MemoryExtensions.AsSpan("Backgrounds"));
			style.ColorBackgroundWindow = StyleEditor.Color("BackgroundWindow", "Window", filter, style.ColorBackgroundWindow);
			style.ColorBackgroundChild = StyleEditor.Color("BackgroundChild", "Child", filter, style.ColorBackgroundChild);
			style.ColorBackgroundPopup = StyleEditor.Color("BackgroundPopup", "Popup", filter, style.ColorBackgroundPopup);
			style.ColorBackgroundFrame = StyleEditor.Color("BackgroundFrame", "Frame", filter, style.ColorBackgroundFrame);
			style.ColorBackgroundFrameHovered = StyleEditor.Color("BackgroundFrameHovered", "FrameHovered", filter, style.ColorBackgroundFrameHovered);
			style.ColorBackgroundFrameActive = StyleEditor.Color("BackgroundFrameActive", "FrameActive", filter, style.ColorBackgroundFrameActive);
			style.ColorBackgroundTitle = StyleEditor.Color("BackgroundTitle", "Title", filter, style.ColorBackgroundTitle);
			style.ColorBackgroundTitleActive = StyleEditor.Color("BackgroundTitleActive", "TitleActive", filter, style.ColorBackgroundTitleActive);
			style.ColorBackgroundTitleCollapsed = StyleEditor.Color("BackgroundTitleCollapsed", "TitleCollapsed", filter, style.ColorBackgroundTitleCollapsed);
			style.ColorBackgroundMenuBar = StyleEditor.Color("BackgroundMenuBar", "MenuBar", filter, style.ColorBackgroundMenuBar);
			style.ColorBackgroundScrollbar = StyleEditor.Color("BackgroundScrollbar", "Scrollbar", filter, style.ColorBackgroundScrollbar);
			style.ColorBackgroundDockingEmpty = StyleEditor.Color("BackgroundDockingEmpty", "DockingEmpty", filter, style.ColorBackgroundDockingEmpty);
			style.ColorBackgroundTableHeader = StyleEditor.Color("BackgroundTableHeader", "TableHeader", filter, style.ColorBackgroundTableHeader);
			style.ColorBackgroundTableRow = StyleEditor.Color("BackgroundTableRow", "TableRow", filter, style.ColorBackgroundTableRow);
			style.ColorBackgroundTableRowAlt = StyleEditor.Color("BackgroundTableRowAlt", "TableRowAlt", filter, style.ColorBackgroundTableRowAlt);
			style.ColorBackgroundTextSelected = StyleEditor.Color("BackgroundTextSelected", "TextSelected", filter, style.ColorBackgroundTextSelected);
			style.ColorBackgroundNavWindowingDim = StyleEditor.Color("BackgroundNavWindowingDim", "NavWindowingDim", filter, style.ColorBackgroundNavWindowingDim);
			style.ColorBackgroundModalWindowDim = StyleEditor.Color("BackgroundModalWindowDim", "ModalWindowDim", filter, style.ColorBackgroundModalWindowDim);
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00009B1C File Offset: 0x00007D1C
		private static void TextEditor(string filter, Style style)
		{
			if (!StyleEditor.Match(filter, new string[]
			{
				"Text",
				"TextDisabled"
			}))
			{
				return;
			}
			ImGui.SeparatorText(MemoryExtensions.AsSpan("Text"));
			style.ColorText = StyleEditor.Color("Text", "Text", filter, style.ColorText);
			style.ColorTextDisabled = StyleEditor.Color("TextDisabled", "TextDisabled", filter, style.ColorTextDisabled);
			style.ColorBackgroundTextSelected = StyleEditor.Color("BackgroundTextSelected", "TextSelected", filter, style.ColorBackgroundTextSelected);
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00009BAC File Offset: 0x00007DAC
		private static void BordersEditor(string filter, Style style)
		{
			if (!StyleEditor.Match(filter, new string[]
			{
				"Border",
				"BorderShadow",
				"TableBorderStrong",
				"TableBorderLight"
			}))
			{
				return;
			}
			ImGui.SeparatorText(MemoryExtensions.AsSpan("Borders"));
			style.ColorBorder = StyleEditor.Color("Border", "Border", filter, style.ColorBorder);
			style.ColorBorderShadow = StyleEditor.Color("BorderShadow", "BorderShadow", filter, style.ColorBorderShadow);
			style.ColorTableBorderStrong = StyleEditor.Color("TableBorderStrong", "TableBorderStrong", filter, style.ColorTableBorderStrong);
			style.ColorTableBorderLight = StyleEditor.Color("TableBorderLight", "TableBorderLight", filter, style.ColorTableBorderLight);
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00009C68 File Offset: 0x00007E68
		private static void ScrollEditor(string filter, Style style)
		{
			if (!StyleEditor.Match(filter, new string[]
			{
				"ScrollbarGrab",
				"ScrollbarGrabHovered",
				"ScrollbarGrabActive",
				"SliderGrab",
				"SliderGrabActive"
			}))
			{
				return;
			}
			ImGui.SeparatorText(MemoryExtensions.AsSpan("Scrollbars & sliders"));
			style.ColorBackgroundScrollbar = StyleEditor.Color("BackgroundScrollbar", "ScrollbarBackground", filter, style.ColorBackgroundScrollbar);
			style.ColorScrollbarGrab = StyleEditor.Color("ScrollbarGrab", "ScrollbarGrab", filter, style.ColorScrollbarGrab);
			style.ColorScrollbarGrabHovered = StyleEditor.Color("ScrollbarGrabHovered", "ScrollbarGrabHovered", filter, style.ColorScrollbarGrabHovered);
			style.ColorScrollbarGrabActive = StyleEditor.Color("ScrollbarGrabActive", "ScrollbarGrabActive", filter, style.ColorScrollbarGrabActive);
			style.ColorSliderGrab = StyleEditor.Color("SliderGrab", "SliderGrab", filter, style.ColorSliderGrab);
			style.ColorSliderGrabActive = StyleEditor.Color("SliderGrabActive", "SliderGrabActive", filter, style.ColorSliderGrabActive);
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00009D64 File Offset: 0x00007F64
		private static void ButtonsEditor(string filter, Style style)
		{
			if (!StyleEditor.Match(filter, new string[]
			{
				"Button",
				"ButtonHovered",
				"ButtonActive"
			}))
			{
				return;
			}
			ImGui.SeparatorText(MemoryExtensions.AsSpan("Buttons"));
			style.ColorButton = StyleEditor.Color("Button", "Button", filter, style.ColorButton);
			style.ColorButtonHovered = StyleEditor.Color("ButtonHovered", "ButtonHovered", filter, style.ColorButtonHovered);
			style.ColorButtonActive = StyleEditor.Color("ButtonActive", "ButtonActive", filter, style.ColorButtonActive);
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00009DFC File Offset: 0x00007FFC
		private static void HeadersEditor(string filter, Style style)
		{
			if (!StyleEditor.Match(filter, new string[]
			{
				"Header",
				"HeaderHovered",
				"HeaderActive"
			}))
			{
				return;
			}
			ImGui.SeparatorText(MemoryExtensions.AsSpan("Headers"));
			style.ColorHeader = StyleEditor.Color("Header", "Header", filter, style.ColorHeader);
			style.ColorHeaderHovered = StyleEditor.Color("HeaderHovered", "HeaderHovered", filter, style.ColorHeaderHovered);
			style.ColorHeaderActive = StyleEditor.Color("HeaderActive", "HeaderActive", filter, style.ColorHeaderActive);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00009E94 File Offset: 0x00008094
		private static void SeparatorEditor(string filter, Style style)
		{
			if (!StyleEditor.Match(filter, new string[]
			{
				"Separator",
				"SeparatorHovered",
				"SeparatorActive"
			}))
			{
				return;
			}
			ImGui.SeparatorText(MemoryExtensions.AsSpan("Separators"));
			style.ColorSeparator = StyleEditor.Color("Separator", "Separator", filter, style.ColorSeparator);
			style.ColorSeparatorHovered = StyleEditor.Color("SeparatorHovered", "SeparatorHovered", filter, style.ColorSeparatorHovered);
			style.ColorSeparatorActive = StyleEditor.Color("SeparatorActive", "SeparatorActive", filter, style.ColorSeparatorActive);
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00009F2C File Offset: 0x0000812C
		private static void ResizeEditor(string filter, Style style)
		{
			if (!StyleEditor.Match(filter, new string[]
			{
				"ResizeGrip",
				"ResizeGripHovered",
				"ResizeGripActive"
			}))
			{
				return;
			}
			ImGui.SeparatorText(MemoryExtensions.AsSpan("Resize"));
			style.ColorResizeGrip = StyleEditor.Color("ResizeGrip", "ResizeGrip", filter, style.ColorResizeGrip);
			style.ColorResizeGripHovered = StyleEditor.Color("ResizeGripHovered", "ResizeGripHovered", filter, style.ColorResizeGripHovered);
			style.ColorResizeGripActive = StyleEditor.Color("ResizeGripActive", "ResizeGripActive", filter, style.ColorResizeGripActive);
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00009FC4 File Offset: 0x000081C4
		private static void TabsEditor(string filter, Style style)
		{
			if (!StyleEditor.Match(filter, new string[]
			{
				"Tab",
				"TabHovered",
				"TabActive",
				"TabUnfocused",
				"TabUnfocusedActive"
			}))
			{
				return;
			}
			ImGui.SeparatorText(MemoryExtensions.AsSpan("Tabs"));
			style.ColorTab = StyleEditor.Color("Tab", "Tab", filter, style.ColorTab);
			style.ColorTabHovered = StyleEditor.Color("TabHovered", "TabHovered", filter, style.ColorTabHovered);
			style.ColorTabActive = StyleEditor.Color("TabActive", "TabActive", filter, style.ColorTabActive);
			style.ColorTabUnfocused = StyleEditor.Color("TabUnfocused", "TabUnfocused", filter, style.ColorTabUnfocused);
			style.ColorTabUnfocusedActive = StyleEditor.Color("TabUnfocusedActive", "TabUnfocusedActive", filter, style.ColorTabUnfocusedActive);
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000A0A4 File Offset: 0x000082A4
		private static void PlotsEditor(string filter, Style style)
		{
			if (!StyleEditor.Match(filter, new string[]
			{
				"PlotLines",
				"PlotLinesHovered",
				"PlotHistogram",
				"PlotHistogramHovered"
			}))
			{
				return;
			}
			ImGui.SeparatorText(MemoryExtensions.AsSpan("Plots"));
			style.ColorPlotLines = StyleEditor.Color("PlotLines", "PlotLines", filter, style.ColorPlotLines);
			style.ColorPlotLinesHovered = StyleEditor.Color("PlotLinesHovered", "PlotLinesHovered", filter, style.ColorPlotLinesHovered);
			style.ColorPlotHistogram = StyleEditor.Color("PlotHistogram", "PlotHistogram", filter, style.ColorPlotHistogram);
			style.ColorPlotHistogramHovered = StyleEditor.Color("PlotHistogramHovered", "PlotHistogramHovered", filter, style.ColorPlotHistogramHovered);
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0000A160 File Offset: 0x00008360
		private static void OtherColorsEditor(string filter, Style style)
		{
			if (!StyleEditor.Match(filter, new string[]
			{
				"CheckMark",
				"DockingPreview",
				"DragDropTarget",
				"NavHighlight",
				"NavWindowingHighlight"
			}))
			{
				return;
			}
			ImGui.SeparatorText(MemoryExtensions.AsSpan("Other"));
			style.ColorCheckMark = StyleEditor.Color("CheckMark", "CheckMark", filter, style.ColorCheckMark);
			style.ColorDockingPreview = StyleEditor.Color("DockingPreview", "DockingPreview", filter, style.ColorDockingPreview);
			style.ColorDragDropTarget = StyleEditor.Color("DragDropTarget", "DragDropTarget", filter, style.ColorDragDropTarget);
			style.ColorNavHighlight = StyleEditor.Color("NavHighlight", "NavHighlight", filter, style.ColorNavHighlight);
			style.ColorNavWindowingHighlight = StyleEditor.Color("NavWindowingHighlight", "NavWindowingHighlight", filter, style.ColorNavWindowingHighlight);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000A240 File Offset: 0x00008440
		private static string Title(string title)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(2, 2);
			defaultInterpolatedStringHandler.AppendFormatted(title);
			defaultInterpolatedStringHandler.AppendLiteral("##");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(StyleEditor.sIdCounter++);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000A285 File Offset: 0x00008485
		public static string WildCardToRegular(string value)
		{
			return "^.*" + Regex.Escape(value).Replace("\\?", ".").Replace("\\*", ".*") + ".*$";
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0000A2BA File Offset: 0x000084BA
		public static bool Match(string filter, string value)
		{
			return Regex.IsMatch(value, filter, 1);
		}

		// Token: 0x06000176 RID: 374 RVA: 0x0000A2C4 File Offset: 0x000084C4
		public static bool Match(string filter, params string[] values)
		{
			foreach (string value in values)
			{
				if (StyleEditor.Match(filter, value))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000177 RID: 375 RVA: 0x0000A2F4 File Offset: 0x000084F4
		private static Value4 Color(string id, string name, string filter, Value4 value)
		{
			if (filter == "" || StyleEditor.Match(filter, id))
			{
				return Editors.Color(StyleEditor.Title(name), value, StyleEditor.Hints.ContainsKey(id) ? StyleEditor.Hints[id] : null);
			}
			return value;
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0000A348 File Offset: 0x00008548
		private static Value2 Pair(string id, string name, string filter, Value2 value)
		{
			if (filter == "" || StyleEditor.Match(filter, id))
			{
				return Editors.Pair(StyleEditor.Title(name), value, StyleEditor.Hints.ContainsKey(id) ? StyleEditor.Hints[id] : null);
			}
			return value;
		}

		// Token: 0x06000179 RID: 377 RVA: 0x0000A394 File Offset: 0x00008594
		private static float Float(string id, string name, string filter, float value)
		{
			if (filter == "" || StyleEditor.Match(filter, id))
			{
				return Editors.Float(StyleEditor.Title(name), value, StyleEditor.Hints.ContainsKey(id) ? StyleEditor.Hints[id] : null);
			}
			return value;
		}

		// Token: 0x0600017A RID: 378 RVA: 0x0000A3E0 File Offset: 0x000085E0
		private static int Integer(string id, string name, string filter, int value)
		{
			if (filter == "" || StyleEditor.Match(filter, id))
			{
				return Editors.Integer(StyleEditor.Title(name), value, StyleEditor.Hints.ContainsKey(id) ? StyleEditor.Hints[id] : null);
			}
			return value;
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0000A42C File Offset: 0x0000862C
		private static bool Boolean(string id, string name, string filter, bool value)
		{
			if (filter == "" || StyleEditor.Match(filter, id))
			{
				return Editors.Boolean(StyleEditor.Title(name), value, StyleEditor.Hints.ContainsKey(id) ? StyleEditor.Hints[id] : null);
			}
			return value;
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0000A478 File Offset: 0x00008678
		private static ImGuiDir Direction(string id, string name, string filter, ImGuiDir value)
		{
			if (filter == "" || StyleEditor.Match(filter, id))
			{
				return Editors.Direction(StyleEditor.Title(name), value, StyleEditor.Hints.ContainsKey(id) ? StyleEditor.Hints[id] : null);
			}
			return value;
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0000A4C4 File Offset: 0x000086C4
		private static ImGuiHoveredFlags HoveredFlag(string id, string name, string filter, ImGuiHoveredFlags value)
		{
			if (filter == "" || StyleEditor.Match(filter, id))
			{
				return Editors.HoveredFlag(StyleEditor.Title(name), value);
			}
			return value;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x0000A4EA File Offset: 0x000086EA
		// Note: this type is marked as 'beforefieldinit'.
		static StyleEditor()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("BackgroundWindow", "TEST");
			StyleEditor.Hints = dictionary;
		}

		// Token: 0x04000022 RID: 34
		private Style mStyle;

		// Token: 0x04000023 RID: 35
		private Style mBackup;

		// Token: 0x04000024 RID: 36
		private string mColorsFilter = "";

		// Token: 0x04000025 RID: 37
		private string mLayoutFilter = "";

		// Token: 0x04000026 RID: 38
		private Guid mId = Guid.NewGuid();

		// Token: 0x04000027 RID: 39
		private static uint sIdCounter = 0U;

		// Token: 0x04000028 RID: 40
		private string mSerialized = "";

		// Token: 0x04000029 RID: 41
		private string mCsharpCode = "";

		// Token: 0x0400002A RID: 42
		private bool mOutputOpened;

		// Token: 0x0400002B RID: 43
		private bool mException;

		// Token: 0x0400002C RID: 44
		private string mMessage = "";

		// Token: 0x0400002D RID: 45
		private int mOutputOption;

		// Token: 0x0400002E RID: 46
		public static readonly Dictionary<string, string> Hints;
	}
}
