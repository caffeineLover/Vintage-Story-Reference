using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace CritchersCalculator
{
	// Token: 0x02000006 RID: 6
	public class GuiDialogOreCalculator : GuiDialog
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000016 RID: 22 RVA: 0x00002921 File Offset: 0x00000B21
		public override string ToggleKeyCombinationCode
		{
			get
			{
				return "critcherscalculator_open";
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002928 File Offset: 0x00000B28
		public GuiDialogOreCalculator(ICoreClientAPI capi) : base(capi)
		{
			try
			{
				this.InitializeAlloyData();
				this.InitializeAlloyState();
				if (this.alloys.Count > 0)
				{
					this.currentAlloyCode = Enumerable.First<string>(this.alloys.Keys);
				}
				this.SetupDialog();
			}
			catch (Exception ex)
			{
				capi.Logger.Error("Critcher's Calculator: Error initializing calculator: {0}", new object[]
				{
					ex
				});
				capi.Logger.Error("Critcher's Calculator: Stack trace: {0}", new object[]
				{
					ex.StackTrace
				});
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000029E4 File Offset: 0x00000BE4
		private void InitializeAlloyData()
		{
			Dictionary<string, AlloyData> dictionary = new Dictionary<string, AlloyData>();
			Dictionary<string, AlloyData> dictionary2 = dictionary;
			string text = "tinBronze";
			AlloyData alloyData = new AlloyData();
			AlloyData alloyData2 = alloyData;
			Dictionary<string, double[]> dictionary3 = new Dictionary<string, double[]>();
			dictionary3["copper"] = new double[]
			{
				0.88,
				0.92
			};
			dictionary3["tin"] = new double[]
			{
				0.08,
				0.12
			};
			alloyData2.Recipe = dictionary3;
			dictionary2[text] = alloyData;
			Dictionary<string, AlloyData> dictionary4 = dictionary;
			string text2 = "bismuthBronze";
			alloyData = new AlloyData();
			AlloyData alloyData3 = alloyData;
			dictionary3 = new Dictionary<string, double[]>();
			dictionary3["copper"] = new double[]
			{
				0.5,
				0.7
			};
			dictionary3["zinc"] = new double[]
			{
				0.2,
				0.3
			};
			dictionary3["bismuth"] = new double[]
			{
				0.1,
				0.2
			};
			alloyData3.Recipe = dictionary3;
			dictionary4[text2] = alloyData;
			Dictionary<string, AlloyData> dictionary5 = dictionary;
			string text3 = "blackBronze";
			alloyData = new AlloyData();
			AlloyData alloyData4 = alloyData;
			dictionary3 = new Dictionary<string, double[]>();
			dictionary3["copper"] = new double[]
			{
				0.68,
				0.84
			};
			dictionary3["gold"] = new double[]
			{
				0.08,
				0.16
			};
			dictionary3["silver"] = new double[]
			{
				0.08,
				0.16
			};
			alloyData4.Recipe = dictionary3;
			dictionary5[text3] = alloyData;
			Dictionary<string, AlloyData> dictionary6 = dictionary;
			string text4 = "brass";
			alloyData = new AlloyData();
			AlloyData alloyData5 = alloyData;
			dictionary3 = new Dictionary<string, double[]>();
			dictionary3["copper"] = new double[]
			{
				0.6,
				0.7
			};
			dictionary3["zinc"] = new double[]
			{
				0.3,
				0.4
			};
			alloyData5.Recipe = dictionary3;
			dictionary6[text4] = alloyData;
			Dictionary<string, AlloyData> dictionary7 = dictionary;
			string text5 = "cupronickel";
			alloyData = new AlloyData();
			AlloyData alloyData6 = alloyData;
			dictionary3 = new Dictionary<string, double[]>();
			dictionary3["copper"] = new double[]
			{
				0.65,
				0.75
			};
			dictionary3["nickel"] = new double[]
			{
				0.25,
				0.35
			};
			alloyData6.Recipe = dictionary3;
			dictionary7[text5] = alloyData;
			Dictionary<string, AlloyData> dictionary8 = dictionary;
			string text6 = "electrum";
			alloyData = new AlloyData();
			AlloyData alloyData7 = alloyData;
			dictionary3 = new Dictionary<string, double[]>();
			dictionary3["gold"] = new double[]
			{
				0.4,
				0.6
			};
			dictionary3["silver"] = new double[]
			{
				0.4,
				0.6
			};
			alloyData7.Recipe = dictionary3;
			dictionary8[text6] = alloyData;
			Dictionary<string, AlloyData> dictionary9 = dictionary;
			string text7 = "molybdochalkos";
			alloyData = new AlloyData();
			AlloyData alloyData8 = alloyData;
			dictionary3 = new Dictionary<string, double[]>();
			dictionary3["lead"] = new double[]
			{
				0.88,
				0.92
			};
			dictionary3["copper"] = new double[]
			{
				0.08,
				0.12
			};
			alloyData8.Recipe = dictionary3;
			dictionary9[text7] = alloyData;
			Dictionary<string, AlloyData> dictionary10 = dictionary;
			string text8 = "leadSolder";
			alloyData = new AlloyData();
			AlloyData alloyData9 = alloyData;
			dictionary3 = new Dictionary<string, double[]>();
			dictionary3["lead"] = new double[]
			{
				0.45,
				0.55
			};
			dictionary3["tin"] = new double[]
			{
				0.45,
				0.55
			};
			alloyData9.Recipe = dictionary3;
			dictionary10[text8] = alloyData;
			Dictionary<string, AlloyData> dictionary11 = dictionary;
			string text9 = "silverSolder";
			alloyData = new AlloyData();
			AlloyData alloyData10 = alloyData;
			dictionary3 = new Dictionary<string, double[]>();
			dictionary3["silver"] = new double[]
			{
				0.4,
				0.5
			};
			dictionary3["tin"] = new double[]
			{
				0.5,
				0.6
			};
			alloyData10.Recipe = dictionary3;
			dictionary11[text9] = alloyData;
			this.alloys = dictionary;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002E49 File Offset: 0x00001049
		private string AlloyName(string code)
		{
			return Lang.Get("critcherscalculator:alloy-" + code.ToLowerInvariant(), Array.Empty<object>());
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002E65 File Offset: 0x00001065
		private string MetalName(string code)
		{
			return Lang.Get("game:material-" + code, Array.Empty<object>());
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002E7C File Offset: 0x0000107C
		private string GetAlloyIngotTexture(string alloyCode)
		{
			if (alloyCode != null)
			{
				switch (alloyCode.Length)
				{
				case 5:
					if (alloyCode == "brass")
					{
						return "icons/Ingot-brass";
					}
					break;
				case 8:
					if (alloyCode == "electrum")
					{
						return "icons/Ingot-electrum";
					}
					break;
				case 9:
					if (alloyCode == "tinBronze")
					{
						return "icons/Ingot-tinbronze";
					}
					break;
				case 10:
					if (alloyCode == "leadSolder")
					{
						return "icons/Ingot-leadsolder";
					}
					break;
				case 11:
				{
					char c = alloyCode.get_Chars(0);
					if (c != 'b')
					{
						if (c == 'c')
						{
							if (alloyCode == "cupronickel")
							{
								return "icons/Ingot-cupronickel";
							}
						}
					}
					else if (alloyCode == "blackBronze")
					{
						return "icons/Ingot-blackbronze";
					}
					break;
				}
				case 12:
					if (alloyCode == "silverSolder")
					{
						return "icons/Ingot-silversolder";
					}
					break;
				case 13:
					if (alloyCode == "bismuthBronze")
					{
						return "icons/Ingot-bismuthbronze";
					}
					break;
				case 14:
					if (alloyCode == "molybdochalkos")
					{
						return "icons/Ingot-molybdochalkos";
					}
					break;
				}
			}
			return null;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002FC8 File Offset: 0x000011C8
		private void InitializeAlloyState()
		{
			foreach (KeyValuePair<string, AlloyData> alloy in this.alloys)
			{
				string key = alloy.Key;
				AlloyData value4 = alloy.Value;
				this.alloyIngots[key] = 1;
				foreach (KeyValuePair<string, double[]> item in value4.Recipe)
				{
					string key2 = item.Key;
					double[] value5 = item.Value;
					int num = (int)(value5[0] * 100.0);
					int num2 = (int)(value5[1] * 100.0);
					int value3 = (num + num2) / 2;
					string key3 = key + "_" + key2;
					this.alloyMetalPercentages[key3] = value3;
				}
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000030C8 File Offset: 0x000012C8
		private void SetupDialog()
		{
			if (string.IsNullOrEmpty(this.currentAlloyCode) || !this.alloys.ContainsKey(this.currentAlloyCode))
			{
				this.capi.Logger.Error("Critcher's Calculator: Invalid current alloy code");
				return;
			}
			double num = 950.0;
			double num2 = 680.0;
			ElementBounds dialogBounds = ElementStdBounds.AutosizedMainDialog.WithAlignment(6).WithFixedSize(num, num2);
			GuiComposer composer = GuiComposerHelpers.AddDialogTitleBar(GuiComposerHelpers.AddDialogBG(this.capi.Gui.CreateCompo("orecalculator", dialogBounds), ElementBounds.Fill, true, 1f), Lang.Get("critcherscalculator:dialog-title", Array.Empty<object>()), new Action(this.OnTitleBarClose), null, null, null);
			double num3 = 10.0;
			double num4 = 50.0;
			double num5 = 200.0;
			double num6 = 35.0;
			double num7 = 5.0;
			double num8 = num4;
			foreach (KeyValuePair<string, AlloyData> alloy in this.alloys)
			{
				string code = alloy.Key;
				string name = this.AlloyName(code);
				bool flag = code == this.currentAlloyCode;
				ElementBounds btnBounds = ElementBounds.Fixed(num3, num8, num5, num6);
				EnumButtonStyle btnStyle = flag ? 1 : 2;
				GuiComposerHelpers.AddButton(composer, name, () => this.OnAlloySelected(code), btnBounds, btnStyle, "btn_" + code);
				num8 += num6 + num7;
			}
			double num9 = num3 + num5 + 20.0;
			double num10 = num - num9 - 20.0;
			double num11 = num4;
			AlloyData alloyData = this.alloys[this.currentAlloyCode];
			string alloyIngotTexture = this.GetAlloyIngotTexture(this.currentAlloyCode);
			if (!string.IsNullOrEmpty(alloyIngotTexture))
			{
				AssetLocation ingotLoc = new AssetLocation("critcherscalculator", alloyIngotTexture + ".png");
				AssetLocation texturePath = ingotLoc.Clone().WithPathPrefixOnce("textures/");
				if (this.capi.Assets.TryGet(texturePath, true) != null)
				{
					ElementBounds imgBounds = ElementBounds.Fixed(num9 + num10 - 110.0, num11, 100.0, 100.0);
					GuiElementHelpers.AddImage(composer, imgBounds, ingotLoc);
				}
			}
			ElementBounds titleBounds = ElementBounds.Fixed(num9, num11 + 5.0, num10 - 120.0, 30.0);
			GuiComposerHelpers.AddStaticText(composer, this.AlloyName(this.currentAlloyCode), CairoFont.WhiteMediumText(), titleBounds, null);
			double num12 = num11 + 110.0;
			ElementBounds ingotLabelBounds = ElementBounds.Fixed(num9, num12, 150.0, 25.0);
			GuiComposerHelpers.AddStaticText(composer, Lang.Get("critcherscalculator:target-ingots", Array.Empty<object>()), CairoFont.WhiteSmallText(), ingotLabelBounds, null);
			ElementBounds ingotInputBounds = ElementBounds.Fixed(num9 + 160.0, num12, 80.0, 30.0);
			GuiComposerHelpers.AddNumberInput(composer, ingotInputBounds, new Action<string>(this.OnIngotChanged), CairoFont.WhiteSmallText(), "ingotInput");
			ElementBounds fuelLabelBounds = ElementBounds.Fixed(num9 + 270.0, num12 + 5.0, 60.0, 25.0);
			GuiComposerHelpers.AddStaticText(composer, Lang.Get("critcherscalculator:fuel-label", Array.Empty<object>()), CairoFont.WhiteSmallText(), fuelLabelBounds, null);
			List<FuelType> validFuels = FuelData.GetValidFuels(this.currentAlloyCode);
			if (validFuels.Count > 0)
			{
				string[] fuelCodes = Enumerable.ToArray<string>(Enumerable.Select<FuelType, string>(validFuels, (FuelType f) => f.Code));
				string[] fuelNames = Enumerable.ToArray<string>(Enumerable.Select<FuelType, string>(validFuels, (FuelType f) => Lang.Get(f.LangKey, Array.Empty<object>())));
				int selectedIndex = Math.Max(0, Array.IndexOf<string>(fuelCodes, this.ResolveFuelCode(this.currentAlloyCode)));
				ElementBounds fuelDropBounds = ElementBounds.Fixed(num9 + 335.0, num12, 220.0, 30.0);
				GuiComposerHelpers.AddDropDown(composer, fuelCodes, fuelNames, selectedIndex, new SelectionChangedDelegate(this.OnFuelSelected), fuelDropBounds, "fuelDropdown");
			}
			double num13 = num12 + 50.0;
			ElementBounds pctHeaderBounds = ElementBounds.Fixed(num9, num13 - 20.0, num10, 20.0);
			GuiComposerHelpers.AddStaticText(composer, Lang.Get("critcherscalculator:metal-percentages", Array.Empty<object>()), CairoFont.WhiteSmallText(), pctHeaderBounds, null);
			double num14 = num13;
			double num15 = 45.0;
			foreach (KeyValuePair<string, double[]> item in alloyData.Recipe)
			{
				string key = item.Key;
				double[] value4 = item.Value;
				ElementBounds metalLabelBounds = ElementBounds.Fixed(num9, num14 + 5.0, 100.0, 25.0);
				string text = this.MetalName(key);
				GuiComposerHelpers.AddStaticText(composer, text, CairoFont.WhiteSmallText(), metalLabelBounds, null);
				string capturedMetal = key;
				ElementBounds sliderBounds = ElementBounds.Fixed(num9 + 110.0, num14 + 5.0, 250.0, 25.0);
				GuiComposerHelpers.AddSlider(composer, delegate(int v)
				{
					this.OnSliderChanged(capturedMetal, v);
					return true;
				}, sliderBounds, "slider_" + key);
				ElementBounds rangeBounds = ElementBounds.Fixed(num9 + 370.0, num14 + 5.0, 120.0, 25.0);
				int value2 = (int)(value4[0] * 100.0);
				int value3 = (int)(value4[1] * 100.0);
				GuiComposer guiComposer = composer;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 2);
				defaultInterpolatedStringHandler.AppendLiteral("(");
				defaultInterpolatedStringHandler.AppendFormatted<int>(value2);
				defaultInterpolatedStringHandler.AppendLiteral("-");
				defaultInterpolatedStringHandler.AppendFormatted<int>(value3);
				defaultInterpolatedStringHandler.AppendLiteral("%)");
				GuiComposerHelpers.AddStaticText(guiComposer, defaultInterpolatedStringHandler.ToStringAndClear(), CairoFont.WhiteSmallText(), rangeBounds, "pct_" + key);
				num14 += num15;
			}
			double num16 = num14 + 20.0;
			ElementBounds insetOuter = ElementBounds.Fixed(num9, num16, num10, 100.0);
			GuiElementInsetHelper.AddInset(composer, insetOuter, 2, 0.85f);
			ElementBounds resultHeaderBounds = ElementBounds.Fixed(num9 + 10.0, num16 + 8.0, 200.0, 20.0);
			GuiComposerHelpers.AddStaticText(composer, Lang.Get("critcherscalculator:required-nuggets", Array.Empty<object>()), CairoFont.WhiteSmallText(), resultHeaderBounds, null);
			ElementBounds resultBounds = ElementBounds.Fixed(num9 + 10.0, num16 + 28.0, num10 - 20.0, 26.0);
			GuiComposerHelpers.AddRichtext(composer, "", CairoFont.WhiteSmallText(), resultBounds, "resultText");
			ElementBounds fuelHeaderBounds = ElementBounds.Fixed(num9 + 10.0, num16 + 54.0, num10 - 20.0, 20.0);
			GuiComposerHelpers.AddStaticText(composer, Lang.Get("critcherscalculator:fuel-needed", new object[]
			{
				FuelData.GetMeltingPointC(this.currentAlloyCode)
			}), CairoFont.WhiteSmallText(), fuelHeaderBounds, "fuelHeader");
			ElementBounds fuelResultBounds = ElementBounds.Fixed(num9 + 10.0, num16 + 74.0, num10 - 20.0, 26.0);
			GuiComposerHelpers.AddRichtext(composer, "", CairoFont.WhiteSmallText(), fuelResultBounds, "fuelText");
			double slotRowY = num16 + 120.0;
			ElementBounds slotsHeaderBounds = ElementBounds.Fixed(num9, slotRowY, 200.0, 20.0);
			GuiComposerHelpers.AddStaticText(composer, Lang.Get("critcherscalculator:crucible-slots-header", Array.Empty<object>()), CairoFont.WhiteSmallText(), slotsHeaderBounds, null);
			double slotBoxSize = 70.0;
			double slotGap = 10.0;
			double slotsTopY = slotRowY + 25.0;
			for (int i = 0; i < 4; i++)
			{
				double num17 = num9 + (double)i * (slotBoxSize + slotGap);
				ElementBounds slotInset = ElementBounds.Fixed(num17, slotsTopY, slotBoxSize, slotBoxSize);
				GuiElementInsetHelper.AddInset(composer, slotInset, 2, 0.85f);
				ElementBounds slotMetalBounds = ElementBounds.Fixed(num17, slotsTopY + 10.0, slotBoxSize, 20.0);
				GuiElementDynamicTextHelper.AddDynamicText(composer, "", CairoFont.WhiteSmallText().WithOrientation(2), slotMetalBounds, "slot_metal_" + i.ToString());
				ElementBounds slotCountBounds = ElementBounds.Fixed(num17, slotsTopY + 38.0, slotBoxSize, 20.0);
				GuiElementDynamicTextHelper.AddDynamicText(composer, "", CairoFont.WhiteSmallText().WithOrientation(2), slotCountBounds, "slot_count_" + i.ToString());
			}
			double warningY = slotsTopY + slotBoxSize + 10.0;
			ElementBounds warningBounds = ElementBounds.Fixed(num9, warningY, num10, 30.0);
			GuiComposerHelpers.AddRichtext(composer, "", CairoFont.WhiteSmallText(), warningBounds, "crucibleWarning");
			ElementBounds closeBounds = ElementBounds.Fixed(num - 120.0, num2 - 40.0, 100.0, 30.0);
			GuiComposerHelpers.AddButton(composer, Lang.Get("critcherscalculator:close", Array.Empty<object>()), new ActionConsumable(this.OnCloseClick), closeBounds, 2, null);
			base.SingleComposer = composer.Compose(true);
			this.InitializeValues();
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00003AD4 File Offset: 0x00001CD4
		private void InitializeValues()
		{
			if (base.SingleComposer == null || string.IsNullOrEmpty(this.currentAlloyCode) || !this.alloys.ContainsKey(this.currentAlloyCode))
			{
				return;
			}
			AlloyData alloyData = this.alloys[this.currentAlloyCode];
			int num = (!this.alloyIngots.ContainsKey(this.currentAlloyCode)) ? 1 : this.alloyIngots[this.currentAlloyCode];
			GuiElementNumberInput numberInput = GuiComposerHelpers.GetNumberInput(base.SingleComposer, "ingotInput");
			if (numberInput != null)
			{
				numberInput.SetValue(num.ToString(), true);
			}
			foreach (KeyValuePair<string, double[]> item in alloyData.Recipe)
			{
				string key = item.Key;
				string key2 = this.currentAlloyCode + "_" + key;
				double[] value3 = item.Value;
				int num2 = (int)(value3[0] * 100.0);
				int num3 = (int)(value3[1] * 100.0);
				if (!this.alloyMetalPercentages.ContainsKey(key2))
				{
					int value2 = (num2 + num3) / 2;
					this.alloyMetalPercentages[key2] = value2;
				}
				int val = this.alloyMetalPercentages[key2];
				val = Math.Max(num2, Math.Min(num3, val));
				this.alloyMetalPercentages[key2] = val;
				GuiElementSlider slider = GuiComposerHelpers.GetSlider(base.SingleComposer, "slider_" + key);
				if (slider != null)
				{
					slider.SetValues(val, num2, num3, 1, "%");
				}
			}
			this.UpdatePercentageLabels();
			this.CalculateResults();
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00003C80 File Offset: 0x00001E80
		private bool OnAlloySelected(string code)
		{
			if (code != this.currentAlloyCode && this.alloys.ContainsKey(code))
			{
				this.currentAlloyCode = code;
				this.SetupDialog();
			}
			return true;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00003CAC File Offset: 0x00001EAC
		private string ResolveFuelCode(string alloyCode)
		{
			List<FuelType> validFuels = FuelData.GetValidFuels(alloyCode);
			if (validFuels.Count == 0)
			{
				return null;
			}
			for (int i = 0; i < validFuels.Count; i++)
			{
				if (validFuels[i].Code == this.preferredFuelCode)
				{
					return this.preferredFuelCode;
				}
			}
			return validFuels[0].Code;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00003D07 File Offset: 0x00001F07
		private void OnFuelSelected(string code, bool selected)
		{
			if (!string.IsNullOrEmpty(code))
			{
				this.preferredFuelCode = code;
				this.CalculateResults();
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00003D20 File Offset: 0x00001F20
		private void UpdateFuelText(int ingots)
		{
			GuiElementRichtext fuelRichtext = GuiComposerHelpers.GetRichtext(base.SingleComposer, "fuelText");
			if (fuelRichtext == null)
			{
				return;
			}
			string fuelCode = this.ResolveFuelCode(this.currentAlloyCode);
			int fuelCount = (fuelCode == null) ? -1 : FuelData.GetFuelCost(this.currentAlloyCode, ingots, fuelCode);
			string text;
			if (fuelCount < 0)
			{
				text = Lang.Get("critcherscalculator:no-data", Array.Empty<object>());
			}
			else
			{
				string fuelName = fuelCode;
				List<FuelType> validFuels = FuelData.GetValidFuels(this.currentAlloyCode);
				for (int i = 0; i < validFuels.Count; i++)
				{
					if (validFuels[i].Code == fuelCode)
					{
						fuelName = Lang.Get(validFuels[i].LangKey, Array.Empty<object>());
						break;
					}
				}
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(3, 2);
				defaultInterpolatedStringHandler.AppendFormatted<int>(fuelCount);
				defaultInterpolatedStringHandler.AppendLiteral(" x ");
				defaultInterpolatedStringHandler.AppendFormatted(fuelName);
				text = defaultInterpolatedStringHandler.ToStringAndClear();
				if (validFuels.Count == 1)
				{
					text = text + "   " + Lang.Get("critcherscalculator:fuel-only-option", Array.Empty<object>());
				}
			}
			try
			{
				fuelRichtext.SetNewText(text, CairoFont.WhiteSmallText(), null);
			}
			catch (Exception ex)
			{
				this.capi.Logger.Error("Critcher's Calculator: Error setting fuel text: {0}", new object[]
				{
					ex.Message
				});
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00003E70 File Offset: 0x00002070
		private void OnIngotChanged(string value)
		{
			int result;
			if (base.SingleComposer == null || string.IsNullOrEmpty(this.currentAlloyCode) || !this.alloys.ContainsKey(this.currentAlloyCode) || string.IsNullOrEmpty(value) || !int.TryParse(value, ref result))
			{
				return;
			}
			if (result < 1)
			{
				result = 1;
			}
			this.alloyIngots[this.currentAlloyCode] = result;
			this.CalculateResults();
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00003ED8 File Offset: 0x000020D8
		private void OnSliderChanged(string metal, int value)
		{
			try
			{
				if (base.SingleComposer != null && !string.IsNullOrEmpty(this.currentAlloyCode) && this.alloys.ContainsKey(this.currentAlloyCode))
				{
					string key = this.currentAlloyCode + "_" + metal;
					this.alloyMetalPercentages[key] = value;
					this.UpdateSinglePercentageLabel(metal);
					this.NormalizeAlloyPercentages(metal);
					this.CalculateResults();
				}
			}
			catch (Exception ex)
			{
				this.capi.Logger.Error("Critcher's Calculator: Error in OnSliderChanged: {0}", new object[]
				{
					ex.Message
				});
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00003F7C File Offset: 0x0000217C
		private void NormalizeAlloyPercentages(string changedMetal)
		{
			if (base.SingleComposer == null || string.IsNullOrEmpty(this.currentAlloyCode) || !this.alloys.ContainsKey(this.currentAlloyCode))
			{
				return;
			}
			AlloyData alloyData = this.alloys[this.currentAlloyCode];
			List<string> list = Enumerable.ToList<string>(alloyData.Recipe.Keys);
			if (list.Count <= 1)
			{
				return;
			}
			int num = 0;
			foreach (string item in list)
			{
				if (item != changedMetal)
				{
					string key = this.currentAlloyCode + "_" + item;
					if (this.alloyMetalPercentages.ContainsKey(key))
					{
						num += this.alloyMetalPercentages[key];
					}
				}
			}
			string key2 = this.currentAlloyCode + "_" + changedMetal;
			if (!this.alloyMetalPercentages.ContainsKey(key2))
			{
				return;
			}
			int num2 = this.alloyMetalPercentages[key2];
			int num3 = 100 - num2;
			if (num <= 0 || num3 < 0)
			{
				return;
			}
			foreach (string item2 in list)
			{
				if (!(item2 == changedMetal))
				{
					string key3 = this.currentAlloyCode + "_" + item2;
					if (this.alloyMetalPercentages.ContainsKey(key3))
					{
						int val = (int)Math.Round((double)this.alloyMetalPercentages[key3] / (double)num * (double)num3);
						int num4 = (int)(alloyData.Recipe[item2][0] * 100.0);
						int num5 = (int)(alloyData.Recipe[item2][1] * 100.0);
						val = Math.Max(num4, Math.Min(num5, val));
						this.alloyMetalPercentages[key3] = val;
						GuiElementSlider slider = GuiComposerHelpers.GetSlider(base.SingleComposer, "slider_" + item2);
						if (slider != null)
						{
							slider.SetValues(val, num4, num5, 1, "%");
						}
						this.UpdateSinglePercentageLabel(item2);
					}
				}
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000041BC File Offset: 0x000023BC
		private void UpdateSinglePercentageLabel(string metal)
		{
			if (base.SingleComposer == null || string.IsNullOrEmpty(this.currentAlloyCode) || !this.alloys.ContainsKey(this.currentAlloyCode))
			{
				return;
			}
			AlloyData alloyData = this.alloys[this.currentAlloyCode];
			if (alloyData.Recipe.ContainsKey(metal))
			{
				string key = this.currentAlloyCode + "_" + metal;
				int value = this.alloyMetalPercentages.ContainsKey(key) ? this.alloyMetalPercentages[key] : 50;
				double[] array = alloyData.Recipe[metal];
				int value2 = (int)(array[0] * 100.0);
				int value3 = (int)(array[1] * 100.0);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(6, 3);
				defaultInterpolatedStringHandler.AppendFormatted<int>(value);
				defaultInterpolatedStringHandler.AppendLiteral("% (");
				defaultInterpolatedStringHandler.AppendFormatted<int>(value2);
				defaultInterpolatedStringHandler.AppendLiteral("-");
				defaultInterpolatedStringHandler.AppendFormatted<int>(value3);
				defaultInterpolatedStringHandler.AppendLiteral("%)");
				string value4 = defaultInterpolatedStringHandler.ToStringAndClear();
				GuiElementStaticText staticText = GuiComposerHelpers.GetStaticText(base.SingleComposer, "pct_" + metal);
				if (staticText != null)
				{
					staticText.SetValue(value4);
				}
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000042E4 File Offset: 0x000024E4
		private void UpdatePercentageLabels()
		{
			if (base.SingleComposer == null || string.IsNullOrEmpty(this.currentAlloyCode) || !this.alloys.ContainsKey(this.currentAlloyCode))
			{
				return;
			}
			foreach (KeyValuePair<string, double[]> item in this.alloys[this.currentAlloyCode].Recipe)
			{
				string key = item.Key;
				string key2 = this.currentAlloyCode + "_" + key;
				int value = 50;
				GuiElementSlider slider = GuiComposerHelpers.GetSlider(base.SingleComposer, "slider_" + key);
				if (slider != null)
				{
					value = slider.GetValue();
					this.alloyMetalPercentages[key2] = value;
				}
				else if (this.alloyMetalPercentages.ContainsKey(key2))
				{
					value = this.alloyMetalPercentages[key2];
				}
				int value2 = (int)(item.Value[0] * 100.0);
				int value3 = (int)(item.Value[1] * 100.0);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 3);
				defaultInterpolatedStringHandler.AppendFormatted<int>(value);
				defaultInterpolatedStringHandler.AppendLiteral("% (");
				defaultInterpolatedStringHandler.AppendFormatted<int>(value2);
				defaultInterpolatedStringHandler.AppendLiteral("-");
				defaultInterpolatedStringHandler.AppendFormatted<int>(value3);
				defaultInterpolatedStringHandler.AppendLiteral("%)");
				string value4 = defaultInterpolatedStringHandler.ToStringAndClear();
				GuiElementStaticText staticText = GuiComposerHelpers.GetStaticText(base.SingleComposer, "pct_" + key);
				if (staticText != null)
				{
					staticText.SetValue(value4);
				}
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x0000448C File Offset: 0x0000268C
		private void CalculateResults()
		{
			if (base.SingleComposer == null)
			{
				this.capi.Logger.Warning("Critcher's Calculator: CalculateResults called but SingleComposer is null!");
				return;
			}
			if (string.IsNullOrEmpty(this.currentAlloyCode) || !this.alloys.ContainsKey(this.currentAlloyCode))
			{
				this.capi.Logger.Warning("Critcher's Calculator: Invalid currentAlloyCode: {0}", new object[]
				{
					this.currentAlloyCode
				});
				return;
			}
			AlloyData alloyData = this.alloys[this.currentAlloyCode];
			int num = (!this.alloyIngots.ContainsKey(this.currentAlloyCode)) ? 1 : this.alloyIngots[this.currentAlloyCode];
			int num2 = num * 100;
			int num3 = num * 20;
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			int num4 = 0;
			foreach (KeyValuePair<string, double[]> item in alloyData.Recipe)
			{
				string key = item.Key;
				string key2 = this.currentAlloyCode + "_" + key;
				int num5 = (int)(item.Value[0] * 100.0);
				int num6 = (int)(item.Value[1] * 100.0);
				int num7;
				if (this.alloyMetalPercentages.ContainsKey(key2))
				{
					num7 = this.alloyMetalPercentages[key2];
				}
				else
				{
					num7 = (num5 + num6) / 2;
					this.alloyMetalPercentages[key2] = num7;
				}
				num7 = Math.Max(num5, Math.Min(num6, num7));
				int num8 = dictionary[key] = (int)Math.Ceiling((double)((int)Math.Round((double)num7 / 100.0 * (double)num2)) / 5.0);
				num4 += num8;
			}
			if (num4 != num3)
			{
				int num9 = num3 - num4;
				List<KeyValuePair<string, int>> list = Enumerable.ToList<KeyValuePair<string, int>>(Enumerable.OrderByDescending<KeyValuePair<string, int>, int>(dictionary, (KeyValuePair<string, int> x) => x.Value));
				int num10 = 0;
				while (num9 != 0 && num10 < 100)
				{
					num10++;
					foreach (KeyValuePair<string, int> item2 in list)
					{
						if (num9 == 0)
						{
							break;
						}
						string key3 = item2.Key;
						if (num9 > 0)
						{
							Dictionary<string, int> dictionary2 = dictionary;
							string text2 = key3;
							int num11 = dictionary2[text2];
							dictionary2[text2] = num11 + 1;
							num9--;
						}
						else if (num9 < 0 && dictionary[key3] > 1)
						{
							Dictionary<string, int> dictionary3 = dictionary;
							string text2 = key3;
							int num11 = dictionary3[text2];
							dictionary3[text2] = num11 - 1;
							num9++;
						}
					}
				}
			}
			int totalUnitsAfter = num * 100;
			List<KeyValuePair<string, int>> orderedMetals = Enumerable.ToList<KeyValuePair<string, int>>(Enumerable.OrderByDescending<KeyValuePair<string, int>, int>(dictionary, (KeyValuePair<string, int> x) => x.Value));
			foreach (KeyValuePair<string, double[]> item3 in alloyData.Recipe)
			{
				string metal = item3.Key;
				if (dictionary.ContainsKey(metal))
				{
					double minPct = item3.Value[0];
					double num12 = item3.Value[1];
					int actualUnits = dictionary[metal] * 5;
					if ((double)actualUnits / (double)totalUnitsAfter < minPct)
					{
						int needed = (int)Math.Ceiling((minPct * (double)totalUnitsAfter - (double)actualUnits) / 5.0);
						for (int i = 0; i < needed; i++)
						{
							Dictionary<string, int> dictionary4 = dictionary;
							string text2 = metal;
							int num11 = dictionary4[text2];
							dictionary4[text2] = num11 + 1;
							foreach (KeyValuePair<string, int> kv in orderedMetals)
							{
								if (!(kv.Key == metal))
								{
									double otherMax = alloyData.Recipe[kv.Key][1];
									int otherUnits = (dictionary[kv.Key] - 1) * 5;
									if (dictionary[kv.Key] > 1 && (double)otherUnits / (double)totalUnitsAfter <= otherMax)
									{
										Dictionary<string, int> dictionary5 = dictionary;
										text2 = kv.Key;
										num11 = dictionary5[text2];
										dictionary5[text2] = num11 - 1;
										break;
									}
								}
							}
						}
					}
				}
			}
			string text = "";
			foreach (KeyValuePair<string, double[]> item4 in alloyData.Recipe)
			{
				string key4 = item4.Key;
				if (dictionary.ContainsKey(key4))
				{
					int value = dictionary[key4];
					string value2 = this.MetalName(key4);
					string text3 = text;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 2);
					defaultInterpolatedStringHandler.AppendFormatted(value2);
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(value);
					defaultInterpolatedStringHandler.AppendLiteral("   ");
					text = text3 + defaultInterpolatedStringHandler.ToStringAndClear();
				}
			}
			this.UpdateCrucibleSlots(dictionary);
			this.UpdateFuelText(num);
			if (string.IsNullOrEmpty(text))
			{
				text = Lang.Get("critcherscalculator:no-data", Array.Empty<object>());
			}
			GuiElementRichtext richtext = GuiComposerHelpers.GetRichtext(base.SingleComposer, "resultText");
			if (richtext != null)
			{
				try
				{
					richtext.SetNewText(text.Trim(), CairoFont.WhiteSmallText(), null);
					return;
				}
				catch (Exception ex)
				{
					this.capi.Logger.Error("Critcher's Calculator: Error setting result text: {0}", new object[]
					{
						ex.Message
					});
					return;
				}
			}
			this.capi.Logger.Error("Critcher's Calculator: resultText element not found!");
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00004AB8 File Offset: 0x00002CB8
		private static List<GuiDialogOreCalculator.SlotFill> PackNuggetsIntoSlots(Dictionary<string, int> nuggetsPerMetal)
		{
			List<GuiDialogOreCalculator.SlotFill> slots = new List<GuiDialogOreCalculator.SlotFill>();
			foreach (KeyValuePair<string, int> kv in nuggetsPerMetal)
			{
				int remaining = kv.Value;
				if (remaining > 0)
				{
					while (remaining > 0)
					{
						int take = Math.Min(remaining, 128);
						slots.Add(new GuiDialogOreCalculator.SlotFill(kv.Key, take));
						remaining -= take;
					}
				}
			}
			return slots;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00004B40 File Offset: 0x00002D40
		private static int SlotsRequired(Dictionary<string, int> nuggetsPerMetal)
		{
			int slots = 0;
			foreach (KeyValuePair<string, int> kv in nuggetsPerMetal)
			{
				if (kv.Value > 0)
				{
					slots += (kv.Value + 128 - 1) / 128;
				}
			}
			return slots;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00004BAC File Offset: 0x00002DAC
		private Dictionary<string, int> ComputeNuggetsForIngotCount(int ingotCount)
		{
			Dictionary<string, int> result = new Dictionary<string, int>();
			if (string.IsNullOrEmpty(this.currentAlloyCode) || !this.alloys.ContainsKey(this.currentAlloyCode) || ingotCount < 1)
			{
				return result;
			}
			AlloyData alloyData = this.alloys[this.currentAlloyCode];
			int totalUnits = ingotCount * 100;
			int totalNuggetsTarget = ingotCount * 20;
			int sum = 0;
			foreach (KeyValuePair<string, double[]> item in alloyData.Recipe)
			{
				string metal = item.Key;
				string pctKey = this.currentAlloyCode + "_" + metal;
				int minPct = (int)(item.Value[0] * 100.0);
				int maxPct = (int)(item.Value[1] * 100.0);
				int pct = this.alloyMetalPercentages.ContainsKey(pctKey) ? this.alloyMetalPercentages[pctKey] : ((minPct + maxPct) / 2);
				pct = Math.Max(minPct, Math.Min(maxPct, pct));
				int nuggets = (int)Math.Ceiling((double)((int)Math.Round((double)pct / 100.0 * (double)totalUnits)) / 5.0);
				result[metal] = nuggets;
				sum += nuggets;
			}
			if (sum != totalNuggetsTarget)
			{
				int diff = totalNuggetsTarget - sum;
				List<KeyValuePair<string, int>> ordered = Enumerable.ToList<KeyValuePair<string, int>>(Enumerable.OrderByDescending<KeyValuePair<string, int>, int>(result, (KeyValuePair<string, int> x) => x.Value));
				int safety = 0;
				while (diff != 0 && safety < 100)
				{
					safety++;
					foreach (KeyValuePair<string, int> kv in ordered)
					{
						if (diff == 0)
						{
							break;
						}
						if (diff > 0)
						{
							Dictionary<string, int> dictionary = result;
							string text = kv.Key;
							int num = dictionary[text];
							dictionary[text] = num + 1;
							diff--;
						}
						else if (diff < 0 && result[kv.Key] > 1)
						{
							Dictionary<string, int> dictionary2 = result;
							string text = kv.Key;
							int num = dictionary2[text];
							dictionary2[text] = num - 1;
							diff++;
						}
					}
				}
				AlloyData ad = alloyData;
				List<KeyValuePair<string, int>> orderedSafe = ordered;
				foreach (KeyValuePair<string, double[]> kv2 in ad.Recipe)
				{
					string metal2 = kv2.Key;
					if (result.ContainsKey(metal2))
					{
						double minPct2 = kv2.Value[0];
						double num2 = kv2.Value[1];
						int actualUnits = result[metal2] * 5;
						if ((double)actualUnits / (double)totalUnits < minPct2)
						{
							int needed = (int)Math.Ceiling((minPct2 * (double)totalUnits - (double)actualUnits) / 5.0);
							for (int i = 0; i < needed; i++)
							{
								Dictionary<string, int> dictionary3 = result;
								string text = metal2;
								int num = dictionary3[text];
								dictionary3[text] = num + 1;
								foreach (KeyValuePair<string, int> other in orderedSafe)
								{
									if (!(other.Key == metal2))
									{
										double otherMax = ad.Recipe[other.Key][1];
										int otherUnits = (result[other.Key] - 1) * 5;
										if (result[other.Key] > 1 && (double)otherUnits / (double)totalUnits <= otherMax)
										{
											Dictionary<string, int> dictionary4 = result;
											text = other.Key;
											num = dictionary4[text];
											dictionary4[text] = num - 1;
											break;
										}
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00004FC4 File Offset: 0x000031C4
		private int ComputeMaxCraftableIngots(int upperBound)
		{
			for (int candidate = upperBound; candidate >= 1; candidate--)
			{
				if (GuiDialogOreCalculator.SlotsRequired(this.ComputeNuggetsForIngotCount(candidate)) <= 4)
				{
					return candidate;
				}
			}
			return 0;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00004FF0 File Offset: 0x000031F0
		private void UpdateCrucibleSlots(Dictionary<string, int> nuggetsPerMetal)
		{
			if (base.SingleComposer == null)
			{
				return;
			}
			bool impossible = GuiDialogOreCalculator.SlotsRequired(nuggetsPerMetal) > 4;
			List<GuiDialogOreCalculator.SlotFill> slots = impossible ? new List<GuiDialogOreCalculator.SlotFill>() : GuiDialogOreCalculator.PackNuggetsIntoSlots(nuggetsPerMetal);
			for (int i = 0; i < 4; i++)
			{
				GuiElementDynamicText metalLabel = GuiElementDynamicTextHelper.GetDynamicText(base.SingleComposer, "slot_metal_" + i.ToString());
				GuiElementDynamicText countLabel = GuiElementDynamicTextHelper.GetDynamicText(base.SingleComposer, "slot_count_" + i.ToString());
				if (i < slots.Count)
				{
					GuiDialogOreCalculator.SlotFill fill = slots[i];
					if (metalLabel != null)
					{
						metalLabel.SetNewText(this.MetalName(fill.MetalCode), false, false, false);
					}
					if (countLabel != null)
					{
						countLabel.SetNewText(fill.NuggetCount.ToString(), false, false, false);
					}
				}
				else
				{
					if (metalLabel != null)
					{
						metalLabel.SetNewText(Lang.Get("critcherscalculator:slot-empty", Array.Empty<object>()), false, false, false);
					}
					if (countLabel != null)
					{
						countLabel.SetNewText("", false, false, false);
					}
				}
			}
			GuiElementRichtext warning = GuiComposerHelpers.GetRichtext(base.SingleComposer, "crucibleWarning");
			if (warning == null)
			{
				return;
			}
			if (impossible)
			{
				int requested = this.alloyIngots.ContainsKey(this.currentAlloyCode) ? this.alloyIngots[this.currentAlloyCode] : 1;
				int max = this.ComputeMaxCraftableIngots(requested);
				string msg = Lang.Get("critcherscalculator:crucible-impossible", new object[]
				{
					requested,
					this.AlloyName(this.currentAlloyCode),
					max
				});
				warning.SetNewText(msg, CairoFont.WhiteSmallText(), null);
				return;
			}
			warning.SetNewText("", CairoFont.WhiteSmallText(), null);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x0000518C File Offset: 0x0000338C
		private void OnTitleBarClose()
		{
			this.TryClose();
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00005195 File Offset: 0x00003395
		private bool OnCloseClick()
		{
			this.TryClose();
			return true;
		}

		// Token: 0x0400000D RID: 13
		private const int UNITS_PER_INGOT = 100;

		// Token: 0x0400000E RID: 14
		private const int UNITS_PER_CHUNK = 5;

		// Token: 0x0400000F RID: 15
		private const int NUGGETS_PER_SLOT = 128;

		// Token: 0x04000010 RID: 16
		private const int CRUCIBLE_SLOTS = 4;

		// Token: 0x04000011 RID: 17
		private Dictionary<string, AlloyData> alloys;

		// Token: 0x04000012 RID: 18
		private Dictionary<string, int> alloyIngots = new Dictionary<string, int>();

		// Token: 0x04000013 RID: 19
		private Dictionary<string, int> alloyMetalPercentages = new Dictionary<string, int>();

		// Token: 0x04000014 RID: 20
		private string currentAlloyCode;

		// Token: 0x04000015 RID: 21
		private string preferredFuelCode = "charcoal";

		// Token: 0x02000008 RID: 8
		private readonly struct SlotFill : IEquatable<GuiDialogOreCalculator.SlotFill>
		{
			// Token: 0x06000030 RID: 48 RVA: 0x0000519F File Offset: 0x0000339F
			public SlotFill(string MetalCode, int NuggetCount)
			{
				this.MetalCode = MetalCode;
				this.NuggetCount = NuggetCount;
			}

			// Token: 0x17000007 RID: 7
			// (get) Token: 0x06000031 RID: 49 RVA: 0x000051AF File Offset: 0x000033AF
			// (set) Token: 0x06000032 RID: 50 RVA: 0x000051B7 File Offset: 0x000033B7
			public string MetalCode { get; set; }

			// Token: 0x17000008 RID: 8
			// (get) Token: 0x06000033 RID: 51 RVA: 0x000051C0 File Offset: 0x000033C0
			// (set) Token: 0x06000034 RID: 52 RVA: 0x000051C8 File Offset: 0x000033C8
			public int NuggetCount { get; set; }

			// Token: 0x06000035 RID: 53 RVA: 0x000051D4 File Offset: 0x000033D4
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("SlotFill");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x06000036 RID: 54 RVA: 0x00005220 File Offset: 0x00003420
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				builder.Append("MetalCode = ");
				builder.Append(this.MetalCode);
				builder.Append(", NuggetCount = ");
				builder.Append(this.NuggetCount.ToString());
				return true;
			}

			// Token: 0x06000037 RID: 55 RVA: 0x0000526E File Offset: 0x0000346E
			[CompilerGenerated]
			public static bool operator !=(GuiDialogOreCalculator.SlotFill left, GuiDialogOreCalculator.SlotFill right)
			{
				return !(left == right);
			}

			// Token: 0x06000038 RID: 56 RVA: 0x0000527A File Offset: 0x0000347A
			[CompilerGenerated]
			public static bool operator ==(GuiDialogOreCalculator.SlotFill left, GuiDialogOreCalculator.SlotFill right)
			{
				return left.Equals(right);
			}

			// Token: 0x06000039 RID: 57 RVA: 0x00005284 File Offset: 0x00003484
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return EqualityComparer<string>.Default.GetHashCode(this.<MetalCode>k__BackingField) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.<NuggetCount>k__BackingField);
			}

			// Token: 0x0600003A RID: 58 RVA: 0x000052AD File Offset: 0x000034AD
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return obj is GuiDialogOreCalculator.SlotFill && this.Equals((GuiDialogOreCalculator.SlotFill)obj);
			}

			// Token: 0x0600003B RID: 59 RVA: 0x000052C5 File Offset: 0x000034C5
			[CompilerGenerated]
			public bool Equals(GuiDialogOreCalculator.SlotFill other)
			{
				return EqualityComparer<string>.Default.Equals(this.<MetalCode>k__BackingField, other.<MetalCode>k__BackingField) && EqualityComparer<int>.Default.Equals(this.<NuggetCount>k__BackingField, other.<NuggetCount>k__BackingField);
			}

			// Token: 0x0600003C RID: 60 RVA: 0x000052F7 File Offset: 0x000034F7
			[CompilerGenerated]
			public void Deconstruct(out string MetalCode, out int NuggetCount)
			{
				MetalCode = this.MetalCode;
				NuggetCount = this.NuggetCount;
			}
		}
	}
}
