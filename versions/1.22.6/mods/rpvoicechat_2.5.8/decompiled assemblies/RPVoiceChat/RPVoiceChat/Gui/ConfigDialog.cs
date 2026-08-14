using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RPVoiceChat.Config;
using RPVoiceChat.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace RPVoiceChat.Gui
{
	// Token: 0x02000073 RID: 115
	public abstract class ConfigDialog : GuiDialog
	{
		// Token: 0x06000330 RID: 816 RVA: 0x000114C9 File Offset: 0x0000F6C9
		public ConfigDialog(ICoreClientAPI capi) : base(capi)
		{
		}

		// Token: 0x06000331 RID: 817 RVA: 0x000114F3 File Offset: 0x0000F6F3
		protected void RegisterTab(ConfigDialog.ConfigTab tab)
		{
			this.ConfigTabs.Add(tab);
		}

		// Token: 0x06000332 RID: 818 RVA: 0x00011501 File Offset: 0x0000F701
		protected void RegisterOption(ConfigDialog.ConfigOption option)
		{
			this.ConfigOptions.Add(option);
		}

		// Token: 0x06000333 RID: 819 RVA: 0x00011510 File Offset: 0x0000F710
		protected void SetupDialog()
		{
			this.isSetup = true;
			this.hoverTextElements = new List<GuiElementHoverText>();
			ConfigDialog.ConfigTab activeTab = this.ConfigTabs[ModConfig.ClientConfig.ActiveConfigTab];
			List<ConfigDialog.ConfigOption> list = this.ConfigOptions.FindAll((ConfigDialog.ConfigOption e) => e.Tab == activeTab && e.Enabled);
			double maxTextWidth = Enumerable.Max<ConfigDialog.ConfigOption>(Enumerable.DefaultIfEmpty<ConfigDialog.ConfigOption>(list), delegate(ConfigDialog.ConfigOption e)
			{
				if (e == null)
				{
					return 0.0;
				}
				return e.TextWidth;
			});
			double maxTabWidth = Enumerable.Max<ConfigDialog.ConfigTab>(Enumerable.DefaultIfEmpty<ConfigDialog.ConfigTab>(this.ConfigTabs), delegate(ConfigDialog.ConfigTab e)
			{
				if (e == null)
				{
					return 0.0;
				}
				return e.TextWidth;
			});
			ElementBounds tabsBounds = ElementBounds.Fixed(0.0, GuiStyle.TitleBarHeight + 5.0, maxTabWidth, 300.0).WithAlignment(1);
			ElementBounds textBounds = ElementBounds.Fixed(tabsBounds.fixedWidth + 20.0, GuiStyle.TitleBarHeight, maxTextWidth, 20.0);
			ElementBounds settingBounds = ElementBounds.Fixed(textBounds.fixedWidth + textBounds.fixedX + 40.0, GuiStyle.TitleBarHeight, 0.0, 20.0);
			ElementBounds bgBounds = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding).WithSizing(2);
			GuiComposer guiComposer = GuiComposerHelpers.AddDialogTitleBar(GuiComposerHelpers.AddShadedDialogBG(this.capi.Gui.CreateCompo("RPVC_ModMenu", ElementStdBounds.AutosizedMainDialog), bgBounds, true, 5.0, 0.75f), UIUtils.I18n("Gui.ModMenu.TitleBar"), new Action(this.OnTitleBarCloseClicked), null, null, null).BeginChildElements(bgBounds);
			GuiTab[] array = this.ConfigTabs.ToArray();
			GuiComposer composer = GuiComposerHelpers.AddVerticalTabs(guiComposer, array, tabsBounds, new Action<int, GuiTab>(this.OnTabClicked), "configTabs");
			foreach (ConfigDialog.ConfigOption option in list)
			{
				ElementBounds wideSettingBounds = settingBounds.FlatCopy().WithFixedWidth(200.0);
				if (option.Label)
				{
					GuiComposerHelpers.AddStaticText(composer, option.Text, option.Font, textBounds, null);
				}
				if (option.Tooltip)
				{
					GuiComposerHelpers.AddHoverText(composer, option.TooltipText, option.Font, 260, textBounds, null);
					this.hoverTextElements.Add(composer.LastAddedElement as GuiElementHoverText);
				}
				switch (option.Type)
				{
				case ConfigDialog.ElementType.Slider:
				{
					GuiElementSlider slider = new GuiElementSlider(this.capi, option.SlideAction, wideSettingBounds);
					if (option.SlideTooltip != null)
					{
						slider.OnSliderTooltip = option.SlideTooltip;
					}
					composer.AddInteractiveElement(slider, option.Key);
					break;
				}
				case ConfigDialog.ElementType.Switch:
					GuiComposerHelpers.AddSwitch(composer, option.ToggleAction, settingBounds, option.Key, 20.0, 4.0);
					break;
				case ConfigDialog.ElementType.Dropdown:
					GuiComposerHelpers.AddDropDown(composer, option.DropdownValues, option.DropdownNames, 0, option.DropdownSelect, wideSettingBounds, option.Key);
					break;
				case ConfigDialog.ElementType.Button:
					GuiComposerHelpers.AddSmallButton(composer, option.ButtonText, option.ButtonAction, wideSettingBounds, 2, null);
					break;
				case ConfigDialog.ElementType.Custom:
				{
					IExtendedGuiElement element = option.CustomElement;
					ElementBounds bounds = option.Label ? settingBounds : textBounds;
					element.Init(option.Key, bounds, composer);
					composer.AddInteractiveElement((GuiElement)element, option.Key);
					element.OnAdd(composer);
					break;
				}
				default:
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unknown or missing element type: ");
					defaultInterpolatedStringHandler.AppendFormatted<ConfigDialog.ElementType>(option.Type);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				}
				textBounds = textBounds.BelowCopy(0.0, 10.0, 0.0, 0.0);
				settingBounds = settingBounds.BelowCopy(0.0, 10.0, 0.0, 0.0);
			}
			foreach (ConfigDialog.ConfigTab tab in this.ConfigTabs)
			{
				if (tab != activeTab)
				{
					tab.Active = false;
				}
			}
			base.SingleComposer = composer.EndChildElements().Compose(true);
		}

		// Token: 0x06000334 RID: 820 RVA: 0x000119D0 File Offset: 0x0000FBD0
		public override bool TryOpen()
		{
			if (!this.isSetup)
			{
				this.SetupDialog();
			}
			if (!base.TryOpen())
			{
				return false;
			}
			this.RefreshValues();
			return true;
		}

		// Token: 0x06000335 RID: 821 RVA: 0x000119F1 File Offset: 0x0000FBF1
		public override bool TryClose()
		{
			ModConfig.SaveClient(this.capi);
			return base.TryClose();
		}

		// Token: 0x06000336 RID: 822 RVA: 0x00011A04 File Offset: 0x0000FC04
		protected void DisableHoverText()
		{
			foreach (GuiElementHoverText guiElementHoverText in this.hoverTextElements)
			{
				guiElementHoverText.SetAutoDisplay(false);
			}
		}

		// Token: 0x06000337 RID: 823 RVA: 0x00011A58 File Offset: 0x0000FC58
		protected void EnableHoverText()
		{
			foreach (GuiElementHoverText guiElementHoverText in this.hoverTextElements)
			{
				guiElementHoverText.SetAutoDisplay(true);
			}
		}

		// Token: 0x06000338 RID: 824 RVA: 0x00011AAC File Offset: 0x0000FCAC
		private void OnTitleBarCloseClicked()
		{
			this.TryClose();
		}

		// Token: 0x06000339 RID: 825 RVA: 0x00011AB5 File Offset: 0x0000FCB5
		private void OnTabClicked(int dataIntOrIndex, GuiTab _)
		{
			ModConfig.ClientConfig.ActiveConfigTab = dataIntOrIndex;
			this.SetupDialog();
			this.RefreshValues();
		}

		// Token: 0x0600033A RID: 826
		protected abstract void RefreshValues();

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600033B RID: 827 RVA: 0x00011ACE File Offset: 0x0000FCCE
		public override string ToggleKeyCombinationCode
		{
			get
			{
				return null;
			}
		}

		// Token: 0x040001A7 RID: 423
		protected const string i18nPrefix = "Gui.ModMenu";

		// Token: 0x040001A8 RID: 424
		private const string composerName = "RPVC_ModMenu";

		// Token: 0x040001A9 RID: 425
		private const int tabsHeight = 300;

		// Token: 0x040001AA RID: 426
		private const int tabsTopPadding = 5;

		// Token: 0x040001AB RID: 427
		private const int textLeftPadding = 20;

		// Token: 0x040001AC RID: 428
		private const int settingsLeftPadding = 40;

		// Token: 0x040001AD RID: 429
		private const int settingDeltaY = 10;

		// Token: 0x040001AE RID: 430
		private const int settingHeight = 20;

		// Token: 0x040001AF RID: 431
		private const int switchSize = 20;

		// Token: 0x040001B0 RID: 432
		private const double sliderWidth = 200.0;

		// Token: 0x040001B1 RID: 433
		private const int tooltipWidth = 260;

		// Token: 0x040001B2 RID: 434
		private const int _tabTextPadding = 4;

		// Token: 0x040001B3 RID: 435
		private bool isSetup;

		// Token: 0x040001B4 RID: 436
		private List<ConfigDialog.ConfigOption> ConfigOptions = new List<ConfigDialog.ConfigOption>();

		// Token: 0x040001B5 RID: 437
		private List<ConfigDialog.ConfigTab> ConfigTabs = new List<ConfigDialog.ConfigTab>();

		// Token: 0x040001B6 RID: 438
		private List<GuiElementHoverText> hoverTextElements = new List<GuiElementHoverText>();

		// Token: 0x02000104 RID: 260
		protected class ConfigTab : GuiTab
		{
			// Token: 0x1700015A RID: 346
			// (get) Token: 0x06000855 RID: 2133 RVA: 0x0002C510 File Offset: 0x0002A710
			public double TextWidth
			{
				get
				{
					return this._TextWidth();
				}
			}

			// Token: 0x06000856 RID: 2134 RVA: 0x0002C518 File Offset: 0x0002A718
			public ConfigTab(string i18nTabKey)
			{
				this.Name = UIUtils.I18n("Gui.ModMenu.Tab." + i18nTabKey);
			}

			// Token: 0x06000857 RID: 2135 RVA: 0x0002C54C File Offset: 0x0002A74C
			private double _TextWidth()
			{
				if (this.Name == null || this.Name == "")
				{
					return 0.0;
				}
				return this.Font.GetTextExtents(this.Name).Width + 8.0;
			}

			// Token: 0x0400041B RID: 1051
			public CairoFont Font = CairoFont.WhiteDetailText().WithFontSize(17f);
		}

		// Token: 0x02000105 RID: 261
		protected class ConfigOption
		{
			// Token: 0x1700015B RID: 347
			// (get) Token: 0x06000858 RID: 2136 RVA: 0x0002C5A0 File Offset: 0x0002A7A0
			// (set) Token: 0x06000859 RID: 2137 RVA: 0x0002C5A8 File Offset: 0x0002A7A8
			public string[] DropdownValues { get; internal set; }

			// Token: 0x1700015C RID: 348
			// (get) Token: 0x0600085A RID: 2138 RVA: 0x0002C5B1 File Offset: 0x0002A7B1
			// (set) Token: 0x0600085B RID: 2139 RVA: 0x0002C5B9 File Offset: 0x0002A7B9
			public string[] DropdownNames { get; internal set; }

			// Token: 0x1700015D RID: 349
			// (get) Token: 0x0600085C RID: 2140 RVA: 0x0002C5C2 File Offset: 0x0002A7C2
			// (set) Token: 0x0600085D RID: 2141 RVA: 0x0002C5CA File Offset: 0x0002A7CA
			public SelectionChangedDelegate DropdownSelect { get; internal set; }

			// Token: 0x1700015E RID: 350
			// (get) Token: 0x0600085E RID: 2142 RVA: 0x0002C5D3 File Offset: 0x0002A7D3
			public string Text
			{
				get
				{
					return UIUtils.I18n("Gui.ModMenu." + this.Key + ".Label");
				}
			}

			// Token: 0x1700015F RID: 351
			// (get) Token: 0x0600085F RID: 2143 RVA: 0x0002C5EF File Offset: 0x0002A7EF
			public string TooltipText
			{
				get
				{
					return UIUtils.I18n("Gui.ModMenu." + this.Key + ".Tooltip");
				}
			}

			// Token: 0x17000160 RID: 352
			// (get) Token: 0x06000860 RID: 2144 RVA: 0x0002C60B File Offset: 0x0002A80B
			public string ButtonText
			{
				get
				{
					return UIUtils.I18n("Gui.ModMenu." + this.Key + ".ButtonText");
				}
			}

			// Token: 0x17000161 RID: 353
			// (get) Token: 0x06000861 RID: 2145 RVA: 0x0002C627 File Offset: 0x0002A827
			public double TextWidth
			{
				get
				{
					return this._TextWidth();
				}
			}

			// Token: 0x06000862 RID: 2146 RVA: 0x0002C630 File Offset: 0x0002A830
			private double _TextWidth()
			{
				if (!this.Label || this.Text == null || this.Text == "")
				{
					return 0.0;
				}
				return this.Font.GetTextExtents(this.Text).Width + 2.0;
			}

			// Token: 0x0400041C RID: 1052
			public bool Enabled = true;

			// Token: 0x0400041D RID: 1053
			public string Key;

			// Token: 0x0400041E RID: 1054
			public ConfigDialog.ElementType Type;

			// Token: 0x0400041F RID: 1055
			public bool Label;

			// Token: 0x04000420 RID: 1056
			public bool Tooltip;

			// Token: 0x04000421 RID: 1057
			public GuiTab Tab;

			// Token: 0x04000422 RID: 1058
			public IExtendedGuiElement CustomElement;

			// Token: 0x04000423 RID: 1059
			public Action<bool> ToggleAction;

			// Token: 0x04000424 RID: 1060
			public ActionConsumable<int> SlideAction;

			// Token: 0x04000425 RID: 1061
			public SliderTooltipDelegate SlideTooltip;

			// Token: 0x04000429 RID: 1065
			public ActionConsumable ButtonAction;

			// Token: 0x0400042A RID: 1066
			public CairoFont Font = CairoFont.WhiteSmallText();
		}

		// Token: 0x02000106 RID: 262
		protected enum ElementType
		{
			// Token: 0x0400042C RID: 1068
			Slider,
			// Token: 0x0400042D RID: 1069
			Switch,
			// Token: 0x0400042E RID: 1070
			Dropdown,
			// Token: 0x0400042F RID: 1071
			Button,
			// Token: 0x04000430 RID: 1072
			Custom
		}
	}
}
