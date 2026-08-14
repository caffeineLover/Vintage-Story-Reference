using System;
using System.Collections.Generic;
using System.Linq;
using Cairo;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.Client.NoObf;
using Vintagestory.Common;

namespace Vintagestory.Client;

public class GuiScreenWorldCustomize : GuiScreen
{
	private ElementBounds listBounds;

	private ElementBounds clippingBounds;

	private Dictionary<string, List<GuiElement>> elementsByCategory = new Dictionary<string, List<GuiElement>>();

	private List<GuiElement> allInputElements = new List<GuiElement>();

	private GuiTab[] tabs;

	private List<string> categories = new List<string>();

	private List<WorldConfigurationAttribute> sortedAttributes = new List<WorldConfigurationAttribute>();

	private Action<bool> didApply;

	private GuiElementContainer container;

	public WorldConfig wcu;

	private List<PlaystyleListEntry> cells;

	public GuiScreenWorldCustomize(Action<bool> didApply, ScreenManager screenManager, GuiScreen parentScreen, WorldConfig wcu, List<PlaystyleListEntry> playstyles)
		: base(screenManager, parentScreen)
	{
		this.wcu = wcu;
		if (playstyles == null)
		{
			loadPlaystyleCells();
		}
		List<ModContainer> list = ScreenManager.verifiedMods.ToList();
		list.Sort(delegate(ModContainer x, ModContainer y)
		{
			if (x?.Info?.CoreMod == true && y?.Info?.CoreMod == true)
			{
				return 0;
			}
			if (x?.Info?.CoreMod == true)
			{
				return -1;
			}
			return (y?.Info?.CoreMod == true) ? 1 : (x?.Info?.Name.CompareTo(y?.Info?.Name)).GetValueOrDefault();
		});
		foreach (ModContainer mod in list)
		{
			ModWorldConfiguration config = mod.WorldConfig;
			if (config == null)
			{
				continue;
			}
			WorldConfigurationAttribute[] worldConfigAttributes = config.WorldConfigAttributes;
			foreach (WorldConfigurationAttribute attribute in worldConfigAttributes)
			{
				if (attribute.OnCustomizeScreen && sortedAttributes.Find((WorldConfigurationAttribute sAttr) => sAttr.Code == attribute.Code) == null)
				{
					attribute.ModInfo = mod.Info;
					int index = sortedAttributes.FindLastIndex((WorldConfigurationAttribute nAttribute) => nAttribute.Category == attribute.Category);
					if (sortedAttributes.Count == 0 || index == -1 || index + 1 >= sortedAttributes.Count)
					{
						sortedAttributes.Add(attribute);
					}
					else
					{
						sortedAttributes.Insert(index + 1, attribute);
					}
				}
			}
		}
		this.didApply = didApply;
		ShowMainMenu = true;
		ClientSettings.Inst.AddWatcher<float>("guiScale", OnGuiScaleChange);
	}

	public override void OnScreenLoaded()
	{
		base.OnScreenLoaded();
		InitGui();
	}

	public override void OnWindowResized(int w, int h)
	{
		invalidate();
	}

	private void OnGuiScaleChange(float s)
	{
		invalidate();
	}

	public override void Dispose()
	{
		ClientSettings.Inst.RemoveWatcher<float>("guiScale", OnGuiScaleChange);
	}

	private void invalidate()
	{
		if (base.IsOpened)
		{
			InitGui();
		}
		else
		{
			ScreenManager.GuiComposers.Dispose("mainmenu-singleplayercustomize");
		}
	}

	private void InitGui()
	{
		cells = loadPlaystyleCells();
		tabs = loadTabs();
		double windowWidth = (float)ScreenManager.GamePlatform.WindowSize.Width / RuntimeEnv.GUIScale;
		double windowHeight = (float)ScreenManager.GamePlatform.WindowSize.Height / RuntimeEnv.GUIScale;
		double insetWidth = Math.Max(400.0, windowWidth * 0.5);
		double insetHeight = Math.Max(300.0, windowHeight - 175.0);
		double elementWidth = insetWidth - 20.0;
		ElementBounds buttonBounds = ElementBounds.FixedSize(60.0, 25.0).WithFixedPadding(10.0, 0.0);
		ElementBounds titleBounds = ElementBounds.Fixed(EnumDialogArea.LeftTop, 0.0, 0.0, 690.0, 35.0);
		ElementBounds leftColumn = ElementBounds.Fixed(0.0, 0.0, (int)GameMath.Clamp(elementWidth / 2.0, 300.0, elementWidth), 30.0);
		ElementBounds rightColumn = ElementBounds.Fixed(-52.0, 0.0, (int)GameMath.Clamp(elementWidth / 2.0, 125.0, elementWidth - 300.0), 30.0).WithAlignment(EnumDialogArea.RightFixed);
		string[] pvalues = cells.Select((PlaystyleListEntry c) => c.PlayStyle.Code).ToArray();
		string[] pnames = cells.Select((PlaystyleListEntry c) => c.Title).ToArray();
		int selectedIndex = cells.FindIndex((PlaystyleListEntry c) => c.PlayStyle.Code == wcu.CurrentPlayStyle.Code);
		ElementBounds tabBounds = ElementBounds.Fixed(0.0, 185.0, insetWidth, 20.0);
		ElementBounds insetBounds;
		ElementComposer = dialogBase("mainmenu-singleplayercustomize").AddStaticText(Lang.Get("singleplayer-customize"), CairoFont.WhiteSmallishText(), titleBounds).AddStaticText(Lang.Get("singleplayer-playstyle"), CairoFont.WhiteSmallishText(), leftColumn = leftColumn.BelowCopy(0.0, 12.0), "playstyleText").AddDropDown(pvalues, pnames, selectedIndex, onPlayStyleChanged, rightColumn = rightColumn.BelowCopy(-31.0, 12.0).WithFixedWidth(rightColumn.fixedWidth - 30.0), "playstyleDropDown")
			.AddIconButton("paste", OnPasteWorldConfig, rightColumn.CopyOffsetedSibling(32.0).WithFixedSize(30.0, 31.0))
			.AddHoverText(Lang.Get("playstyle-pastefromclipboard"), CairoFont.WhiteDetailText(), 200, rightColumn.CopyOffsetedSibling(32.0).WithFixedSize(30.0, 31.0).WithFixedPadding(5.0))
			.AddStaticText(Lang.Get("singleplayer-worldheight"), CairoFont.WhiteSmallishText(), leftColumn = leftColumn.BelowCopy(0.0, 12.0), "worldHeightText")
			.AddSlider(onNewWorldHeightValue, rightColumn = rightColumn.BelowCopy(31.0, 15.0).WithFixedSize(rightColumn.fixedWidth + 30.0, 20.0), "worldHeight")
			.AddStaticText(Lang.Get("singleplayer-seed"), CairoFont.WhiteSmallishText(), leftColumn = leftColumn.BelowCopy(0.0, 11.0), "worldseedText")
			.AddTextInput(rightColumn = rightColumn.BelowCopy(0.0, 18.0).WithFixedHeight(30.0), null, null, "worldseed")
			.AddIf(!wcu.IsNewWorld)
			.AddRichtext("<font opacity=\"0.6\">" + Lang.Get("singleplayer-disabledcustomizations") + "</font>", CairoFont.WhiteDetailText(), leftColumn = leftColumn.BelowCopy(0.0, 8.0).WithFixedWidth(600.0))
			.Execute(delegate
			{
				rightColumn = rightColumn.BelowCopy(0.0, 18.0);
			})
			.EndIf()
			.AddHorizontalTabs(tabs, tabBounds, onTabClicked, CairoFont.WhiteSmallText().WithWeight(FontWeight.Bold), CairoFont.WhiteSmallText().WithWeight(FontWeight.Bold).WithColor(GuiStyle.ActiveButtonTextColor), "tabs")
			.AddInset(insetBounds = leftColumn.BelowCopy(0.0, 12.0).WithFixedSize(insetWidth, insetHeight - leftColumn.fixedY - leftColumn.fixedHeight))
			.BeginClip(clippingBounds = insetBounds.ForkContainingChild(3.0, 3.0, 3.0, 3.0));
		if (!wcu.IsNewWorld)
		{
			ElementComposer.GetStaticText("playstyleText").Font.Color[3] = 0.5;
			ElementComposer.GetStaticText("worldHeightText").Font.Color[3] = 0.5;
			ElementComposer.GetStaticText("worldseedText").Font.Color[3] = 0.5;
			ElementComposer.GetDropDown("playstyleDropDown").Enabled = false;
			ElementComposer.GetSlider("worldHeight").Enabled = false;
			ElementComposer.GetTextInput("worldseed").Enabled = false;
		}
		container = new GuiElementContainer(ElementComposer.Api, listBounds = clippingBounds.ForkContainingChild(0.0, 0.0, 0.0, -3.0).WithFixedPadding(10.0));
		container.Tabbable = true;
		int i = 0;
		double size = 26.0;
		ElementBounds leftColumni = ElementBounds.Fixed(0.0, 2.0, (int)GameMath.Clamp(elementWidth / 2.0, 300.0, elementWidth), size);
		ElementBounds rightColumni = ElementBounds.Fixed(-20.0, 0.0, (int)GameMath.Clamp(elementWidth / 2.0, 125.0, elementWidth - 300.0), size).WithAlignment(EnumDialogArea.RightFixed);
		leftColumni = leftColumni.FlatCopy();
		rightColumni = rightColumni.FlatCopy();
		elementsByCategory.Clear();
		allInputElements.Clear();
		Dictionary<string, List<GuiElement>> hoverElementsByCat = new Dictionary<string, List<GuiElement>>();
		foreach (WorldConfigurationAttribute attribute in sortedAttributes)
		{
			if (!attribute.OnCustomizeScreen)
			{
				continue;
			}
			if (!elementsByCategory.TryGetValue(attribute.Category, out var elements))
			{
				elements = (elementsByCategory[attribute.Category] = new List<GuiElement>());
				leftColumni = ElementBounds.Fixed(0.0, 2.0, (int)GameMath.Clamp(elementWidth / 2.0, 300.0, elementWidth), size);
				rightColumni = ElementBounds.Fixed(-20.0, 0.0, (int)GameMath.Clamp(elementWidth / 2.0, 125.0, elementWidth - 300.0), size).WithAlignment(EnumDialogArea.RightFixed);
				leftColumni = leftColumni.FlatCopy();
				rightColumni = rightColumni.FlatCopy();
			}
			if (!hoverElementsByCat.TryGetValue(attribute.Category, out var hoverElements))
			{
				hoverElements = (hoverElementsByCat[attribute.Category] = new List<GuiElement>());
			}
			bool enabled = wcu.IsNewWorld || !attribute.OnlyDuringWorldCreate;
			WorldConfigurationValue value = wcu[attribute.Code];
			object defaultValue = attribute.TypedDefault;
			GuiElementControl elem = null;
			switch (attribute.DataType)
			{
			case EnumDataType.Bool:
			{
				bool on = (bool)defaultValue;
				if (value != null)
				{
					on = (bool)value.Value;
				}
				GuiElementSwitch guiElementSwitch = new GuiElementSwitch(ScreenManager.api, null, rightColumni.FlatCopy().WithFixedAlignmentOffset(0.0 - rightColumni.fixedWidth + size, 0.0), size);
				guiElementSwitch.SetValue(on);
				elem = guiElementSwitch;
				break;
			}
			case EnumDataType.DoubleInput:
			{
				double val6 = (double)defaultValue;
				if (value != null)
				{
					val6 = (double)value.Value;
				}
				GuiElementNumberInput guiElementNumberInput2 = new GuiElementNumberInput(ScreenManager.api, rightColumni?.FlatCopy(), null, CairoFont.WhiteSmallText());
				guiElementNumberInput2.SetValue(val6);
				elem = guiElementNumberInput2;
				break;
			}
			case EnumDataType.DoubleRange:
			{
				double val7 = (double)defaultValue;
				if (value != null)
				{
					val7 = (double)value.Value;
				}
				GuiElementSlider sliderElem2 = new GuiElementSlider(ScreenManager.api, null, rightColumni.FlatCopy());
				sliderElem2.TooltipExceedClipBounds = true;
				sliderElem2.ShowTextWhenResting = true;
				if (attribute.SkipValues != null)
				{
					string[] skipValues = attribute.SkipValues;
					for (int num = 0; num < skipValues.Length; num++)
					{
						string[] split2 = skipValues[num].Split("...", 2, StringSplitOptions.RemoveEmptyEntries);
						if (split2.Length == 1)
						{
							sliderElem2.AddSkipValue((int)((decimal)split2[0].ToDouble() * attribute.Multiplier));
							continue;
						}
						int l2 = (int)((decimal)split2[0].ToDouble() * attribute.Multiplier);
						int max2 = (int)((decimal)split2[1].ToDouble() * attribute.Multiplier);
						for (int intStep2 = (int)((decimal)attribute.Step * attribute.Multiplier); l2 <= max2; l2 += intStep2)
						{
							sliderElem2.AddSkipValue(l2);
						}
					}
				}
				string unitCode = "worldattribute-" + attribute.Code + "-unit";
				int alarm2 = (int)((decimal)Math.Clamp(attribute.Alarm, attribute.Min, attribute.Max) * attribute.Multiplier);
				if (attribute.Values != null && attribute.Names != null)
				{
					string[] values4 = attribute.Values;
					string[] names4 = new string[attribute.Names.Length];
					for (int k4 = 0; k4 < values4.Length; k4++)
					{
						string langkey4 = "worldconfig-" + attribute.Code + "-" + attribute.Names[k4];
						if (ClientSettings.DeveloperMode && !Lang.HasTranslation(langkey4))
						{
							ScreenManager.api.Logger.Debug("\"{0}\": \"{1}\",", langkey4, attribute.Names[k4]);
						}
						names4[k4] = Lang.Get(langkey4);
					}
					sliderElem2.OnSliderTooltip = (int num2) => values4.Contains<string>(((decimal)num2 / attribute.Multiplier).ToString()) ? names4[values4.IndexOf<string>(((decimal)num2 / attribute.Multiplier).ToString())] : (Lang.GetWithFallback(unitCode, "{0}", (decimal)num2 / attribute.Multiplier / attribute.DisplayUnit) + ((num2 > alarm2) ? Lang.GetWithFallback("worldattribute-" + attribute.Code + "-warning", "") : ""));
					sliderElem2.OnSliderRestingText = (int num2) => values4.Contains<string>(((decimal)num2 / attribute.Multiplier).ToString()) ? names4[values4.IndexOf<string>(((decimal)num2 / attribute.Multiplier).ToString())] : Lang.GetWithFallback(unitCode, "{0}", (decimal)num2 / attribute.Multiplier / attribute.DisplayUnit);
				}
				else
				{
					sliderElem2.OnSliderTooltip = (int num2) => Lang.GetWithFallback(unitCode, "{0}", (decimal)num2 / attribute.Multiplier / attribute.DisplayUnit) + ((num2 > alarm2) ? Lang.GetWithFallback("worldattribute-" + attribute.Code + "-warning", "") : "");
					sliderElem2.OnSliderRestingText = (int num2) => Lang.GetWithFallback(unitCode, "{0}", (decimal)num2 / attribute.Multiplier / attribute.DisplayUnit);
				}
				sliderElem2.SetValues((int)((decimal)val7 * attribute.Multiplier), (int)((decimal)attribute.Min * attribute.Multiplier), (int)((decimal)attribute.Max * attribute.Multiplier), (int)((decimal)attribute.Step * attribute.Multiplier));
				elem = sliderElem2;
				break;
			}
			case EnumDataType.IntInput:
			{
				double val4 = (int)defaultValue;
				if (value != null)
				{
					val4 = (int)value.Value;
				}
				GuiElementNumberInput guiElementNumberInput = new GuiElementNumberInput(ScreenManager.api, rightColumni?.FlatCopy(), null, CairoFont.WhiteSmallText());
				guiElementNumberInput.IntMode = true;
				guiElementNumberInput.SetValue(val4);
				elem = guiElementNumberInput;
				break;
			}
			case EnumDataType.IntRange:
			{
				int val2 = (int)defaultValue;
				if (value != null)
				{
					val2 = (int)value.Value;
				}
				GuiElementSlider sliderElem = new GuiElementSlider(ScreenManager.api, null, rightColumni.FlatCopy());
				sliderElem.TooltipExceedClipBounds = true;
				sliderElem.ShowTextWhenResting = true;
				if (attribute.SkipValues != null)
				{
					string[] skipValues = attribute.SkipValues;
					for (int num = 0; num < skipValues.Length; num++)
					{
						string[] split = skipValues[num].Split("...", 2, StringSplitOptions.RemoveEmptyEntries);
						if (split.Length == 1)
						{
							sliderElem.AddSkipValue(split[0].ToInt());
							continue;
						}
						int max = split[1].ToInt();
						int intStep = (int)attribute.Step;
						for (int l = split[0].ToInt(); l <= max; l += intStep)
						{
							sliderElem.AddSkipValue(l);
						}
					}
				}
				string unit = "worldattribute-" + attribute.Code + "-unit";
				int alarm = (int)Math.Clamp(attribute.Alarm, attribute.Min, attribute.Max);
				if (attribute.Values != null && attribute.Names != null)
				{
					string[] values2 = attribute.Values;
					string[] names2 = new string[attribute.Names.Length];
					for (int k2 = 0; k2 < values2.Length; k2++)
					{
						string langkey2 = "worldconfig-" + attribute.Code + "-" + attribute.Names[k2];
						if (ClientSettings.DeveloperMode && !Lang.HasTranslation(langkey2))
						{
							ScreenManager.api.Logger.Debug("\"{0}\": \"{1}\",", langkey2, attribute.Names[k2]);
						}
						names2[k2] = Lang.Get(langkey2);
					}
					sliderElem.OnSliderTooltip = (int num2) => values2.Contains<string>(num2.ToString()) ? names2[values2.IndexOf<string>(num2.ToString())] : (Lang.Get(unit, (decimal)num2 / attribute.DisplayUnit) + ((num2 > alarm) ? Lang.GetWithFallback("worldattribute-" + attribute.Code + "-warning", "") : ""));
					sliderElem.OnSliderRestingText = (int num2) => values2.Contains<string>(num2.ToString()) ? names2[values2.IndexOf<string>(num2.ToString())] : Lang.Get(unit, (decimal)num2 / attribute.DisplayUnit);
				}
				else
				{
					sliderElem.OnSliderTooltip = (int num2) => Lang.GetWithFallback(unit, "{0}", (decimal)num2 / attribute.DisplayUnit) + ((num2 > alarm) ? Lang.GetWithFallback("worldattribute-" + attribute.Code + "-warning", "") : "");
					sliderElem.OnSliderRestingText = (int num2) => Lang.GetWithFallback(unit, "{0}", (decimal)num2 / attribute.DisplayUnit);
				}
				sliderElem.SetValues(val2, (int)attribute.Min, (int)attribute.Max, (int)attribute.Step);
				elem = sliderElem;
				break;
			}
			case EnumDataType.DropDown:
			{
				string val3 = (string)defaultValue;
				if (value != null)
				{
					val3 = (string)value.Value;
				}
				int selindex = attribute.Values.IndexOf(val3);
				string[] values3 = attribute.Values;
				string[] names3 = new string[attribute.Names.Length];
				for (int k3 = 0; k3 < values3.Length; k3++)
				{
					string langkey3 = "worldconfig-" + attribute.Code + "-" + attribute.Names[k3];
					if (ClientSettings.DeveloperMode && !Lang.HasTranslation(langkey3))
					{
						ScreenManager.api.Logger.Debug("\"{0}\": \"{1}\",", langkey3, attribute.Names[k3]);
					}
					names3[k3] = Lang.Get(langkey3);
				}
				if (selindex < 0)
				{
					values3 = values3.Append(val3);
					names3 = names3.Append(val3);
					selindex = names3.Length - 1;
				}
				elem = new GuiElementDropDown(ScreenManager.api, values3, names3, selindex, null, rightColumni.FlatCopy(), CairoFont.WhiteSmallText(), multiSelect: false);
				break;
			}
			case EnumDataType.String:
			{
				string val5 = (string)defaultValue;
				if (value != null)
				{
					val5 = (string)value.Value;
				}
				GuiElementTextInput guiElementTextInput = new GuiElementTextInput(ScreenManager.api, rightColumni.FlatCopy(), null, CairoFont.WhiteSmallText());
				guiElementTextInput.SetValue(val5);
				elem = guiElementTextInput;
				break;
			}
			case EnumDataType.StringRange:
			{
				string[] values = attribute.Values;
				string[] names = new string[attribute.Names.Length];
				int val = values.IndexOf((string)defaultValue);
				if (value != null)
				{
					val = values.IndexOf((string)value.Value);
				}
				for (int k = 0; k < values.Length; k++)
				{
					string langkey = "worldconfig-" + attribute.Code + "-" + attribute.Names[k];
					if (ClientSettings.DeveloperMode && !Lang.HasTranslation(langkey))
					{
						ScreenManager.api.Logger.Debug("\"{0}\": \"{1}\",", langkey, attribute.Names[k]);
					}
					names[k] = Lang.Get(langkey);
				}
				GuiElementSlider guiElementSlider = new GuiElementSlider(ScreenManager.api, null, rightColumni.FlatCopy());
				guiElementSlider.TooltipExceedClipBounds = true;
				guiElementSlider.ShowTextWhenResting = true;
				guiElementSlider.OnSliderTooltip = (int num2) => names[num2];
				guiElementSlider.SetValues(val, 0, values.Length - 1, 1);
				elem = guiElementSlider;
				break;
			}
			}
			elem.Enabled = enabled;
			elements.Add(elem);
			allInputElements.Add(elem);
			CairoFont font = (attribute.ModInfo.CoreMod ? CairoFont.WhiteSmallText() : CairoFont.WhiteSmallText().WithColor(GuiStyle.WarningTextColor));
			if (!enabled)
			{
				font.Color[3] = 0.5;
			}
			elements.Add(new GuiElementStaticText(ScreenManager.api, Lang.Get("worldattribute-" + attribute.Code), EnumTextOrientation.Left, leftColumni, font));
			string tooltip = Lang.GetIfExists("worldattribute-" + attribute.Code + "-desc");
			if (tooltip != null)
			{
				ElementBounds hbounds = leftColumni.FlatCopy();
				hbounds.fixedWidth -= 50.0;
				if (!attribute.ModInfo.CoreMod)
				{
					tooltip = tooltip + "\n\n<font color=\"#F2C983\">" + Lang.Get("createworld-worldattribute-notcoremodhover", attribute.ModInfo.Name) + "</font>";
				}
				GuiElementHoverText hoverelem = new GuiElementHoverText(ScreenManager.api, tooltip, CairoFont.WhiteSmallText(), 320, hbounds);
				hoverElements.Add(hoverelem);
			}
			leftColumni = leftColumni.BelowCopy(0.0, 9.9);
			rightColumni = rightColumni.BelowCopy(0.0, 10.0);
			i++;
		}
		foreach (KeyValuePair<string, List<GuiElement>> val8 in elementsByCategory)
		{
			if (hoverElementsByCat.TryGetValue(val8.Key, out var hoverEles))
			{
				foreach (GuiElement hoverEle in hoverEles)
				{
					val8.Value.Add(hoverEle);
				}
			}
			val8.Value.Add(new GuiElementStaticText(ScreenManager.api, " ", EnumTextOrientation.Left, leftColumni = leftColumni.BelowCopy(), CairoFont.WhiteDetailText()));
		}
		updateWorldHeightSlider();
		ElementComposer.AddInteractiveElement(container, "configlist").EndClip().AddVerticalScrollbar(OnNewScrollbarvalue, ElementStdBounds.VerticalScrollbar(insetBounds), "scrollbar")
			.AddButton(Lang.Get("general-back"), OnBack, buttonBounds.FlatCopy().FixedUnder(insetBounds, 10.0))
			.AddButton(Lang.Get("general-apply"), OnApply, buttonBounds.FlatCopy().FixedUnder(insetBounds, 10.0).WithFixedWidth(200.0)
				.WithAlignment(EnumDialogArea.RightFixed)
				.WithFixedAlignmentOffset(-13.0, 0.0))
			.EndChildElements()
			.Compose();
		ElementComposer.GetScrollbar("scrollbar").SetHeights((float)clippingBounds.fixedHeight, (float)listBounds.fixedHeight);
		selectTab(0);
		ElementComposer.GetTextInput("worldseed").SetValue(wcu.Seed);
		setConfigSliderAlarmValues();
	}

	private GuiTab[] loadTabs()
	{
		List<GuiTab> tabs = new List<GuiTab>();
		categories.Clear();
		int i = 0;
		foreach (WorldConfigurationAttribute attribute in sortedAttributes)
		{
			if (attribute.OnCustomizeScreen && !categories.Contains(attribute.Category))
			{
				categories.Add(attribute.Category);
				tabs.Add(new GuiTab
				{
					Name = Lang.Get("worldconfig-category-" + attribute.Category),
					DataInt = i++
				});
			}
		}
		return tabs.ToArray();
	}

	private void onTabClicked(int dataint)
	{
		selectTab(dataint);
	}

	private void selectTab(int tabIndex)
	{
		string cat = categories[tabIndex];
		container.Clear();
		foreach (GuiElement ele in elementsByCategory[cat])
		{
			container.Add(ele);
		}
		ElementComposer.ReCompose();
		listBounds.CalcWorldBounds();
		clippingBounds.CalcWorldBounds();
		GuiElementScrollbar scrollbar = ElementComposer.GetScrollbar("scrollbar");
		scrollbar.SetHeights((float)clippingBounds.fixedHeight, (float)listBounds.fixedHeight);
		scrollbar.CurrentYPosition = 0f;
		scrollbar.TriggerChanged();
		updateWorldHeightSlider();
	}

	private void setConfigSliderAlarmValues()
	{
		int i = 0;
		foreach (WorldConfigurationAttribute attribute in sortedAttributes)
		{
			if (!attribute.OnCustomizeScreen)
			{
				continue;
			}
			if (!wcu.IsNewWorld && attribute.OnlyDuringWorldCreate)
			{
				i++;
				continue;
			}
			_ = wcu[attribute.Code];
			_ = attribute.TypedDefault;
			GuiElement elem = allInputElements[i];
			switch (attribute.DataType)
			{
			case EnumDataType.DoubleRange:
				(elem as GuiElementSlider).SetAlarmValue((int)((decimal)Math.Clamp(attribute.Alarm, attribute.Min, attribute.Max) * attribute.Multiplier));
				break;
			case EnumDataType.IntRange:
				(elem as GuiElementSlider).SetAlarmValue((int)Math.Clamp(attribute.Alarm, attribute.Min, attribute.Max));
				break;
			}
			i++;
		}
	}

	private void setFieldValues()
	{
		int i = 0;
		foreach (WorldConfigurationAttribute attribute in sortedAttributes)
		{
			if (!attribute.OnCustomizeScreen)
			{
				continue;
			}
			if (!wcu.IsNewWorld && attribute.OnlyDuringWorldCreate)
			{
				i++;
				continue;
			}
			WorldConfigurationValue value = wcu[attribute.Code];
			object defaultValue = attribute.TypedDefault;
			GuiElement elem = allInputElements[i];
			switch (attribute.DataType)
			{
			case EnumDataType.Bool:
			{
				GuiElementSwitch obj4 = elem as GuiElementSwitch;
				bool on = (bool)defaultValue;
				if (value != null)
				{
					on = (bool)value.Value;
				}
				obj4.SetValue(on);
				break;
			}
			case EnumDataType.DoubleInput:
			{
				GuiElementNumberInput obj6 = elem as GuiElementNumberInput;
				double val7 = (double)defaultValue;
				if (value != null)
				{
					val7 = (double)value.Value;
				}
				obj6.SetValue(val7);
				break;
			}
			case EnumDataType.DoubleRange:
			{
				GuiElementSlider obj5 = elem as GuiElementSlider;
				double val5 = (double)defaultValue;
				if (value != null)
				{
					val5 = (double)value.Value;
				}
				obj5.SetValues((int)((decimal)val5 * attribute.Multiplier), (int)((decimal)attribute.Min * attribute.Multiplier), (int)((decimal)attribute.Max * attribute.Multiplier), (int)((decimal)attribute.Step * attribute.Multiplier));
				break;
			}
			case EnumDataType.IntInput:
			{
				GuiElementNumberInput obj3 = elem as GuiElementNumberInput;
				int val3 = (int)defaultValue;
				if (value != null)
				{
					val3 = (int)value.Value;
				}
				obj3.SetValue(val3);
				break;
			}
			case EnumDataType.IntRange:
			{
				GuiElementSlider obj2 = elem as GuiElementSlider;
				int val2 = (int)defaultValue;
				if (value != null)
				{
					val2 = (int)value.Value;
				}
				obj2.SetValues(val2, (int)attribute.Min, (int)attribute.Max, (int)attribute.Step);
				break;
			}
			case EnumDataType.DropDown:
			{
				string val6 = (string)defaultValue;
				if (value != null)
				{
					val6 = (string)value.Value;
				}
				int selindex = attribute.Values.IndexOf(val6);
				(elem as GuiElementDropDown).SetSelectedIndex(selindex);
				break;
			}
			case EnumDataType.String:
			{
				string val4 = (string)defaultValue;
				if (value != null)
				{
					val4 = (string)value.Value;
				}
				(elem as GuiElementTextInput).SetValue(val4);
				break;
			}
			case EnumDataType.StringRange:
			{
				GuiElementSlider obj = elem as GuiElementSlider;
				int val = attribute.Values.IndexOf((string)defaultValue);
				if (value != null)
				{
					val = attribute.Values.IndexOf((string)value.Value);
				}
				obj.SetValues(val, 0, attribute.Values.Length - 1, 1);
				break;
			}
			}
			i++;
		}
	}

	private void onPlayStyleChanged(string code, bool selected)
	{
		wcu.WorldConfigsCustom.Clear();
		wcu.selectPlayStyle(code);
		updateWorldHeightSlider();
		setFieldValues();
	}

	private void updateWorldHeightSlider()
	{
		if (wcu.CurrentPlayStyle.Code != "creativebuilding")
		{
			ElementComposer.GetSlider("worldHeight").SetValues(wcu.MapsizeY, 128, 512, 64, " blocks");
			ElementComposer.GetSlider("worldHeight").SetAlarmValue(384);
			ElementComposer.GetSlider("worldHeight").OnSliderTooltip = (int value) => Lang.Get("createworld-worldheight", value) + ((value > 384) ? ("\n" + Lang.Get("createworld-worldheight-warning")) : "");
		}
		else
		{
			ElementComposer.GetSlider("worldHeight").SetValues(wcu.MapsizeY, 128, 2048, 64, " blocks");
			ElementComposer.GetSlider("worldHeight").SetAlarmValue(1024);
			ElementComposer.GetSlider("worldHeight").OnSliderTooltip = (int value) => Lang.Get("createworld-worldheight", value) + ((value > 1024) ? ("\n" + Lang.Get("createworld-worldheight-warning")) : "");
		}
	}

	private List<PlaystyleListEntry> loadPlaystyleCells()
	{
		cells = new List<PlaystyleListEntry>();
		wcu.LoadPlayStyles();
		foreach (PlayStyle ps in wcu.PlayStyles)
		{
			cells.Add(new PlaystyleListEntry
			{
				Title = Lang.Get("playstyle-" + ps.LangCode),
				PlayStyle = ps
			});
		}
		if (cells.Count == 0)
		{
			cells.Add(new PlaystyleListEntry
			{
				Title = Lang.Get("noplaystyles-title"),
				DetailText = Lang.Get("noplaystyles-desc"),
				PlayStyle = null,
				Enabled = false
			});
		}
		return cells;
	}

	private bool onNewWorldHeightValue(int value)
	{
		wcu.MapsizeY = value;
		return true;
	}

	public override void OnKeyDown(KeyEvent e)
	{
		base.OnKeyDown(e);
		if (e.CtrlPressed && e.KeyCode == 104)
		{
			OnPasteWorldConfig();
		}
	}

	private void OnPasteWorldConfig(bool ok = false)
	{
		try
		{
			string json = ScreenManager.Platform.XPlatInterface.GetClipboardText();
			if (json.StartsWith("{"))
			{
				wcu.FromJson(json);
				ScreenManager.GamePlatform.Logger.Notification("Pasted world config loaded!");
				updateWorldHeightSlider();
				setFieldValues();
			}
		}
		catch (Exception e)
		{
			ScreenManager.GamePlatform.Logger.Warning("Unable to load pasted world config:");
			ScreenManager.GamePlatform.Logger.Warning(e);
		}
	}

	private bool OnApply()
	{
		wcu.Seed = ElementComposer.GetTextInput("worldseed").GetText();
		wcu.MapsizeY = ElementComposer.GetSlider("worldHeight").GetValue();
		int i = 0;
		wcu.WorldConfigsCustom.Clear();
		foreach (WorldConfigurationAttribute attribute in sortedAttributes)
		{
			if (attribute.OnCustomizeScreen)
			{
				GuiElement elem = allInputElements[i];
				WorldConfigurationValue value = new WorldConfigurationValue();
				value.Attribute = attribute;
				value.Code = attribute.Code;
				switch (attribute.DataType)
				{
				case EnumDataType.Bool:
				{
					GuiElementSwitch switchElem = elem as GuiElementSwitch;
					value.Value = switchElem.On;
					break;
				}
				case EnumDataType.DoubleInput:
				{
					GuiElementNumberInput numInput2 = elem as GuiElementNumberInput;
					value.Value = numInput2.GetText().ToDouble();
					break;
				}
				case EnumDataType.DoubleRange:
				{
					GuiElementSlider slider3 = elem as GuiElementSlider;
					value.Value = (double)((decimal)slider3.GetValue() / attribute.Multiplier);
					break;
				}
				case EnumDataType.IntInput:
				{
					GuiElementNumberInput numInput = elem as GuiElementNumberInput;
					value.Value = numInput.GetText().ToInt();
					break;
				}
				case EnumDataType.IntRange:
				{
					GuiElementSlider slider2 = elem as GuiElementSlider;
					value.Value = slider2.GetValue();
					break;
				}
				case EnumDataType.DropDown:
				{
					GuiElementDropDown dropDown = elem as GuiElementDropDown;
					value.Value = dropDown.SelectedValue;
					break;
				}
				case EnumDataType.String:
				{
					GuiElementTextInput textInput = elem as GuiElementTextInput;
					value.Value = textInput.GetText();
					break;
				}
				case EnumDataType.StringRange:
				{
					GuiElementSlider slider = elem as GuiElementSlider;
					value.Value = attribute.Values[slider.GetValue()];
					break;
				}
				}
				wcu.WorldConfigsCustom.Add(value.Code, value);
				i++;
			}
		}
		wcu.updateJWorldConfig();
		didApply(obj: true);
		return true;
	}

	private bool OnBack()
	{
		didApply(obj: false);
		return true;
	}

	private void OnNewScrollbarvalue(float value)
	{
		ElementBounds bounds = container.Bounds;
		bounds.fixedY = 0f - value;
		bounds.CalcWorldBounds();
		foreach (GuiElement element in container.Elements)
		{
			if (element is GuiElementDropDown gelemd && gelemd.listMenu.IsOpened)
			{
				gelemd.listMenu.Close();
			}
		}
	}
}
