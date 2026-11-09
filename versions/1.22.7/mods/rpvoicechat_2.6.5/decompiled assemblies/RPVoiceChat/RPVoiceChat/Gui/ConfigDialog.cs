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
	// Token: 0x02000096 RID: 150
	public abstract class ConfigDialog : GuiDialog
	{
		// Token: 0x060003F5 RID: 1013 RVA: 0x00015782 File Offset: 0x00013982
		public ConfigDialog(ICoreClientAPI capi) : base(capi)
		{
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x000157AC File Offset: 0x000139AC
		protected void RegisterTab(ConfigDialog.ConfigTab tab)
		{
			this.ConfigTabs.Add(tab);
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x000157BA File Offset: 0x000139BA
		protected void RegisterOption(ConfigDialog.ConfigOption option)
		{
			this.ConfigOptions.Add(option);
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x000157C8 File Offset: 0x000139C8
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

		// Token: 0x060003F9 RID: 1017 RVA: 0x00015C88 File Offset: 0x00013E88
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

		// Token: 0x060003FA RID: 1018 RVA: 0x00015CA9 File Offset: 0x00013EA9
		public override bool TryClose()
		{
			ModConfig.SaveClient(this.capi);
			return base.TryClose();
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x00015CBC File Offset: 0x00013EBC
		protected void DisableHoverText()
		{
			foreach (GuiElementHoverText guiElementHoverText in this.hoverTextElements)
			{
				guiElementHoverText.SetAutoDisplay(false);
			}
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x00015D10 File Offset: 0x00013F10
		protected void EnableHoverText()
		{
			foreach (GuiElementHoverText guiElementHoverText in this.hoverTextElements)
			{
				guiElementHoverText.SetAutoDisplay(true);
			}
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x00015D64 File Offset: 0x00013F64
		private void OnTitleBarCloseClicked()
		{
			this.TryClose();
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x00015D6D File Offset: 0x00013F6D
		private void OnTabClicked(int dataIntOrIndex, GuiTab _)
		{
			ModConfig.ClientConfig.ActiveConfigTab = dataIntOrIndex;
			this.SetupDialog();
			this.RefreshValues();
		}

		// Token: 0x060003FF RID: 1023
		protected abstract void RefreshValues();

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000400 RID: 1024 RVA: 0x00015D86 File Offset: 0x00013F86
		public override string ToggleKeyCombinationCode
		{
			get
			{
				return null;
			}
		}

		// Token: 0x04000221 RID: 545
		protected const string i18nPrefix = "Gui.ModMenu";

		// Token: 0x04000222 RID: 546
		private const string composerName = "RPVC_ModMenu";

		// Token: 0x04000223 RID: 547
		private const int tabsHeight = 300;

		// Token: 0x04000224 RID: 548
		private const int tabsTopPadding = 5;

		// Token: 0x04000225 RID: 549
		private const int textLeftPadding = 20;

		// Token: 0x04000226 RID: 550
		private const int settingsLeftPadding = 40;

		// Token: 0x04000227 RID: 551
		private const int settingDeltaY = 10;

		// Token: 0x04000228 RID: 552
		private const int settingHeight = 20;

		// Token: 0x04000229 RID: 553
		private const int switchSize = 20;

		// Token: 0x0400022A RID: 554
		private const double sliderWidth = 200.0;

		// Token: 0x0400022B RID: 555
		private const int tooltipWidth = 260;

		// Token: 0x0400022C RID: 556
		private const int _tabTextPadding = 4;

		// Token: 0x0400022D RID: 557
		private bool isSetup;

		// Token: 0x0400022E RID: 558
		private List<ConfigDialog.ConfigOption> ConfigOptions = new List<ConfigDialog.ConfigOption>();

		// Token: 0x0400022F RID: 559
		private List<ConfigDialog.ConfigTab> ConfigTabs = new List<ConfigDialog.ConfigTab>();

		// Token: 0x04000230 RID: 560
		private List<GuiElementHoverText> hoverTextElements = new List<GuiElementHoverText>();

		// Token: 0x02000156 RID: 342
		protected class ConfigTab : GuiTab
		{
			// Token: 0x170001BD RID: 445
			// (get) Token: 0x06000B14 RID: 2836 RVA: 0x0003A254 File Offset: 0x00038454
			public double TextWidth
			{
				get
				{
					return this._TextWidth();
				}
			}

			// Token: 0x06000B15 RID: 2837 RVA: 0x0003A25C File Offset: 0x0003845C
			public ConfigTab(string i18nTabKey)
			{
				this.Name = UIUtils.I18n("Gui.ModMenu.Tab." + i18nTabKey);
			}

			// Token: 0x06000B16 RID: 2838 RVA: 0x0003A290 File Offset: 0x00038490
			private double _TextWidth()
			{
				if (this.Name == null || this.Name == "")
				{
					return 0.0;
				}
				return this.Font.GetTextExtents(this.Name).Width + 8.0;
			}

			// Token: 0x0400057B RID: 1403
			public CairoFont Font = CairoFont.WhiteDetailText().WithFontSize(17f);
		}

		// Token: 0x02000157 RID: 343
		protected class ConfigOption
		{
			// Token: 0x170001BE RID: 446
			// (get) Token: 0x06000B17 RID: 2839 RVA: 0x0003A2E4 File Offset: 0x000384E4
			// (set) Token: 0x06000B18 RID: 2840 RVA: 0x0003A2EC File Offset: 0x000384EC
			public string[] DropdownValues { get; internal set; }

			// Token: 0x170001BF RID: 447
			// (get) Token: 0x06000B19 RID: 2841 RVA: 0x0003A2F5 File Offset: 0x000384F5
			// (set) Token: 0x06000B1A RID: 2842 RVA: 0x0003A2FD File Offset: 0x000384FD
			public string[] DropdownNames { get; internal set; }

			// Token: 0x170001C0 RID: 448
			// (get) Token: 0x06000B1B RID: 2843 RVA: 0x0003A306 File Offset: 0x00038506
			// (set) Token: 0x06000B1C RID: 2844 RVA: 0x0003A30E File Offset: 0x0003850E
			public SelectionChangedDelegate DropdownSelect { get; internal set; }

			// Token: 0x170001C1 RID: 449
			// (get) Token: 0x06000B1D RID: 2845 RVA: 0x0003A317 File Offset: 0x00038517
			public string Text
			{
				get
				{
					return UIUtils.I18n("Gui.ModMenu." + this.Key + ".Label");
				}
			}

			// Token: 0x170001C2 RID: 450
			// (get) Token: 0x06000B1E RID: 2846 RVA: 0x0003A333 File Offset: 0x00038533
			public string TooltipText
			{
				get
				{
					return UIUtils.I18n("Gui.ModMenu." + this.Key + ".Tooltip");
				}
			}

			// Token: 0x170001C3 RID: 451
			// (get) Token: 0x06000B1F RID: 2847 RVA: 0x0003A34F File Offset: 0x0003854F
			public string ButtonText
			{
				get
				{
					return UIUtils.I18n("Gui.ModMenu." + this.Key + ".ButtonText");
				}
			}

			// Token: 0x170001C4 RID: 452
			// (get) Token: 0x06000B20 RID: 2848 RVA: 0x0003A36B File Offset: 0x0003856B
			public double TextWidth
			{
				get
				{
					return this._TextWidth();
				}
			}

			// Token: 0x06000B21 RID: 2849 RVA: 0x0003A374 File Offset: 0x00038574
			private double _TextWidth()
			{
				if (!this.Label || this.Text == null || this.Text == "")
				{
					return 0.0;
				}
				return this.Font.GetTextExtents(this.Text).Width + 2.0;
			}

			// Token: 0x0400057C RID: 1404
			public bool Enabled = true;

			// Token: 0x0400057D RID: 1405
			public string Key;

			// Token: 0x0400057E RID: 1406
			public ConfigDialog.ElementType Type;

			// Token: 0x0400057F RID: 1407
			public bool Label;

			// Token: 0x04000580 RID: 1408
			public bool Tooltip;

			// Token: 0x04000581 RID: 1409
			public GuiTab Tab;

			// Token: 0x04000582 RID: 1410
			public IExtendedGuiElement CustomElement;

			// Token: 0x04000583 RID: 1411
			public Action<bool> ToggleAction;

			// Token: 0x04000584 RID: 1412
			public ActionConsumable<int> SlideAction;

			// Token: 0x04000585 RID: 1413
			public SliderTooltipDelegate SlideTooltip;

			// Token: 0x04000589 RID: 1417
			public ActionConsumable ButtonAction;

			// Token: 0x0400058A RID: 1418
			public CairoFont Font = CairoFont.WhiteSmallText();
		}

		// Token: 0x02000158 RID: 344
		protected enum ElementType
		{
			// Token: 0x0400058C RID: 1420
			Slider,
			// Token: 0x0400058D RID: 1421
			Switch,
			// Token: 0x0400058E RID: 1422
			Dropdown,
			// Token: 0x0400058F RID: 1423
			Button,
			// Token: 0x04000590 RID: 1424
			Custom
		}
	}
}
