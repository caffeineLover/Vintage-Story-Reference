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
	// Token: 0x02000004 RID: 4
	public class GuiDialogOreCalculator : GuiDialog
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000009 RID: 9 RVA: 0x00002227 File Offset: 0x00000427
		public override string ToggleKeyCombinationCode
		{
			get
			{
				return "critcherscalculator_open";
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002230 File Offset: 0x00000430
		public GuiDialogOreCalculator(ICoreClientAPI capi) : base(capi)
		{
			try
			{
				this.InitializeAlloyData();
				this.InitializeAlloyState();
				bool flag = this.alloys.Count > 0;
				if (flag)
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

		// Token: 0x0600000B RID: 11 RVA: 0x000022F0 File Offset: 0x000004F0
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

		// Token: 0x0600000C RID: 12 RVA: 0x0000277C File Offset: 0x0000097C
		private string AlloyName(string code)
		{
			return Lang.Get("critcherscalculator:alloy-" + code.ToLowerInvariant(), Array.Empty<object>());
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002798 File Offset: 0x00000998
		private string MetalName(string code)
		{
			return Lang.Get("game:material-" + code, Array.Empty<object>());
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000027B0 File Offset: 0x000009B0
		private string GetAlloyIngotTexture(string alloyCode)
		{
			if (!true)
			{
			}
			uint num = <PrivateImplementationDetails>.ComputeStringHash(alloyCode);
			string result;
			if (num <= 1692222443U)
			{
				if (num <= 480537986U)
				{
					if (num != 424762942U)
					{
						if (num == 480537986U)
						{
							if (alloyCode == "leadSolder")
							{
								result = "icons/Ingot-leadsolder";
								goto IL_17B;
							}
						}
					}
					else if (alloyCode == "cupronickel")
					{
						result = "icons/Ingot-cupronickel";
						goto IL_17B;
					}
				}
				else if (num != 1587576770U)
				{
					if (num == 1692222443U)
					{
						if (alloyCode == "bismuthBronze")
						{
							result = "icons/Ingot-bismuthbronze";
							goto IL_17B;
						}
					}
				}
				else if (alloyCode == "molybdochalkos")
				{
					result = "icons/Ingot-molybdochalkos";
					goto IL_17B;
				}
			}
			else if (num <= 2652484074U)
			{
				if (num != 2600538436U)
				{
					if (num == 2652484074U)
					{
						if (alloyCode == "brass")
						{
							result = "icons/Ingot-brass";
							goto IL_17B;
						}
					}
				}
				else if (alloyCode == "blackBronze")
				{
					result = "icons/Ingot-blackbronze";
					goto IL_17B;
				}
			}
			else if (num != 2964420429U)
			{
				if (num != 3159024584U)
				{
					if (num == 4156416592U)
					{
						if (alloyCode == "electrum")
						{
							result = "icons/Ingot-electrum";
							goto IL_17B;
						}
					}
				}
				else if (alloyCode == "tinBronze")
				{
					result = "icons/Ingot-tinbronze";
					goto IL_17B;
				}
			}
			else if (alloyCode == "silverSolder")
			{
				result = "icons/Ingot-silversolder";
				goto IL_17B;
			}
			result = null;
			IL_17B:
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002944 File Offset: 0x00000B44
		private void InitializeAlloyState()
		{
			foreach (KeyValuePair<string, AlloyData> alloy in this.alloys)
			{
				string key = alloy.Key;
				AlloyData value = alloy.Value;
				this.alloyIngots[key] = 1;
				foreach (KeyValuePair<string, double[]> item in value.Recipe)
				{
					string key2 = item.Key;
					double[] value2 = item.Value;
					int num = (int)(value2[0] * 100.0);
					int num2 = (int)(value2[1] * 100.0);
					int value3 = (num + num2) / 2;
					string key3 = key + "_" + key2;
					this.alloyMetalPercentages[key3] = value3;
				}
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002A58 File Offset: 0x00000C58
		private void SetupDialog()
		{
			bool flag = string.IsNullOrEmpty(this.currentAlloyCode) || !this.alloys.ContainsKey(this.currentAlloyCode);
			if (flag)
			{
				this.capi.Logger.Error("Critcher's Calculator: Invalid current alloy code");
			}
			else
			{
				double num = 950.0;
				double num2 = 600.0;
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
					bool selected = code == this.currentAlloyCode;
					ElementBounds btnBounds = ElementBounds.Fixed(num3, num8, num5, num6);
					EnumButtonStyle btnStyle = selected ? 1 : 2;
					GuiComposerHelpers.AddButton(composer, name, () => this.OnAlloySelected(code), btnBounds, btnStyle, "btn_" + code);
					num8 += num6 + num7;
				}
				double num9 = num3 + num5 + 20.0;
				double num10 = num - num9 - 20.0;
				double num11 = num4;
				AlloyData alloyData = this.alloys[this.currentAlloyCode];
				string alloyIngotTexture = this.GetAlloyIngotTexture(this.currentAlloyCode);
				bool flag2 = !string.IsNullOrEmpty(alloyIngotTexture);
				if (flag2)
				{
					AssetLocation ingotLoc = new AssetLocation("critcherscalculator", alloyIngotTexture + ".png");
					AssetLocation texturePath = ingotLoc.Clone().WithPathPrefixOnce("textures/");
					bool flag3 = this.capi.Assets.TryGet(texturePath, true) != null;
					if (flag3)
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
				double num13 = num12 + 50.0;
				ElementBounds pctHeaderBounds = ElementBounds.Fixed(num9, num13 - 20.0, num10, 20.0);
				GuiComposerHelpers.AddStaticText(composer, Lang.Get("critcherscalculator:metal-percentages", Array.Empty<object>()), CairoFont.WhiteSmallText(), pctHeaderBounds, null);
				double num14 = num13;
				double num15 = 45.0;
				foreach (KeyValuePair<string, double[]> item in alloyData.Recipe)
				{
					string key = item.Key;
					double[] value = item.Value;
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
					int value2 = (int)(value[0] * 100.0);
					int value3 = (int)(value[1] * 100.0);
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
				ElementBounds insetOuter = ElementBounds.Fixed(num9, num16, num10, 70.0);
				GuiElementInsetHelper.AddInset(composer, insetOuter, 2, 0.85f);
				ElementBounds resultHeaderBounds = ElementBounds.Fixed(num9 + 10.0, num16 + 10.0, 200.0, 20.0);
				GuiComposerHelpers.AddStaticText(composer, Lang.Get("critcherscalculator:required-nuggets", Array.Empty<object>()), CairoFont.WhiteSmallText(), resultHeaderBounds, null);
				ElementBounds resultBounds = ElementBounds.Fixed(num9 + 10.0, num16 + 35.0, num10 - 20.0, 30.0);
				GuiComposerHelpers.AddRichtext(composer, "", CairoFont.WhiteSmallText(), resultBounds, "resultText");
				double slotRowY = num16 + 90.0;
				ElementBounds slotsHeaderBounds = ElementBounds.Fixed(num9, slotRowY, 200.0, 20.0);
				GuiComposerHelpers.AddStaticText(composer, Lang.Get("critcherscalculator:crucible-slots-header", Array.Empty<object>()), CairoFont.WhiteSmallText(), slotsHeaderBounds, null);
				double slotBoxSize = 70.0;
				double slotGap = 10.0;
				double slotsTopY = slotRowY + 25.0;
				for (int i = 0; i < 4; i++)
				{
					double slotX = num9 + (double)i * (slotBoxSize + slotGap);
					ElementBounds slotInset = ElementBounds.Fixed(slotX, slotsTopY, slotBoxSize, slotBoxSize);
					GuiElementInsetHelper.AddInset(composer, slotInset, 2, 0.85f);
					ElementBounds slotMetalBounds = ElementBounds.Fixed(slotX, slotsTopY + 10.0, slotBoxSize, 20.0);
					GuiElementDynamicTextHelper.AddDynamicText(composer, "", CairoFont.WhiteSmallText().WithOrientation(2), slotMetalBounds, "slot_metal_" + i.ToString());
					ElementBounds slotCountBounds = ElementBounds.Fixed(slotX, slotsTopY + 38.0, slotBoxSize, 20.0);
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
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000032DC File Offset: 0x000014DC
		private void InitializeValues()
		{
			bool flag = base.SingleComposer == null || string.IsNullOrEmpty(this.currentAlloyCode) || !this.alloys.ContainsKey(this.currentAlloyCode);
			if (!flag)
			{
				AlloyData alloyData = this.alloys[this.currentAlloyCode];
				int num = (!this.alloyIngots.ContainsKey(this.currentAlloyCode)) ? 1 : this.alloyIngots[this.currentAlloyCode];
				GuiElementNumberInput numberInput = GuiComposerHelpers.GetNumberInput(base.SingleComposer, "ingotInput");
				bool flag2 = numberInput != null;
				if (flag2)
				{
					numberInput.SetValue(num.ToString(), true);
				}
				foreach (KeyValuePair<string, double[]> item in alloyData.Recipe)
				{
					string key = item.Key;
					string key2 = this.currentAlloyCode + "_" + key;
					double[] value = item.Value;
					int num2 = (int)(value[0] * 100.0);
					int num3 = (int)(value[1] * 100.0);
					bool flag3 = !this.alloyMetalPercentages.ContainsKey(key2);
					if (flag3)
					{
						int value2 = (num2 + num3) / 2;
						this.alloyMetalPercentages[key2] = value2;
					}
					int val = this.alloyMetalPercentages[key2];
					val = Math.Max(num2, Math.Min(num3, val));
					this.alloyMetalPercentages[key2] = val;
					GuiElementSlider slider = GuiComposerHelpers.GetSlider(base.SingleComposer, "slider_" + key);
					bool flag4 = slider != null;
					if (flag4)
					{
						slider.SetValues(val, num2, num3, 1, "%");
					}
				}
				this.UpdatePercentageLabels();
				this.CalculateResults();
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000034D0 File Offset: 0x000016D0
		private bool OnAlloySelected(string code)
		{
			bool flag = code != this.currentAlloyCode && this.alloys.ContainsKey(code);
			if (flag)
			{
				this.currentAlloyCode = code;
				this.SetupDialog();
			}
			return true;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00003514 File Offset: 0x00001714
		private void OnIngotChanged(string value)
		{
			int result;
			bool flag = base.SingleComposer == null || string.IsNullOrEmpty(this.currentAlloyCode) || !this.alloys.ContainsKey(this.currentAlloyCode) || string.IsNullOrEmpty(value) || !int.TryParse(value, ref result);
			if (!flag)
			{
				bool flag2 = result < 1;
				if (flag2)
				{
					result = 1;
				}
				this.alloyIngots[this.currentAlloyCode] = result;
				this.CalculateResults();
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00003590 File Offset: 0x00001790
		private void OnSliderChanged(string metal, int value)
		{
			try
			{
				bool flag = base.SingleComposer != null && !string.IsNullOrEmpty(this.currentAlloyCode) && this.alloys.ContainsKey(this.currentAlloyCode);
				if (flag)
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

		// Token: 0x06000015 RID: 21 RVA: 0x00003644 File Offset: 0x00001844
		private void NormalizeAlloyPercentages(string changedMetal)
		{
			bool flag = base.SingleComposer == null || string.IsNullOrEmpty(this.currentAlloyCode) || !this.alloys.ContainsKey(this.currentAlloyCode);
			if (!flag)
			{
				AlloyData alloyData = this.alloys[this.currentAlloyCode];
				List<string> list = Enumerable.ToList<string>(alloyData.Recipe.Keys);
				bool flag2 = list.Count <= 1;
				if (!flag2)
				{
					int num = 0;
					foreach (string item in list)
					{
						bool flag3 = item != changedMetal;
						if (flag3)
						{
							string key = this.currentAlloyCode + "_" + item;
							bool flag4 = this.alloyMetalPercentages.ContainsKey(key);
							if (flag4)
							{
								num += this.alloyMetalPercentages[key];
							}
						}
					}
					string key2 = this.currentAlloyCode + "_" + changedMetal;
					bool flag5 = !this.alloyMetalPercentages.ContainsKey(key2);
					if (!flag5)
					{
						int num2 = this.alloyMetalPercentages[key2];
						int num3 = 100 - num2;
						bool flag6 = num <= 0 || num3 < 0;
						if (!flag6)
						{
							foreach (string item2 in list)
							{
								bool flag7 = item2 == changedMetal;
								if (!flag7)
								{
									string key3 = this.currentAlloyCode + "_" + item2;
									bool flag8 = this.alloyMetalPercentages.ContainsKey(key3);
									if (flag8)
									{
										int val = (int)Math.Round((double)this.alloyMetalPercentages[key3] / (double)num * (double)num3);
										int num4 = (int)(alloyData.Recipe[item2][0] * 100.0);
										int num5 = (int)(alloyData.Recipe[item2][1] * 100.0);
										val = Math.Max(num4, Math.Min(num5, val));
										this.alloyMetalPercentages[key3] = val;
										GuiElementSlider slider = GuiComposerHelpers.GetSlider(base.SingleComposer, "slider_" + item2);
										bool flag9 = slider != null;
										if (flag9)
										{
											slider.SetValues(val, num4, num5, 1, "%");
										}
										this.UpdateSinglePercentageLabel(item2);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00003900 File Offset: 0x00001B00
		private void UpdateSinglePercentageLabel(string metal)
		{
			bool flag = base.SingleComposer == null || string.IsNullOrEmpty(this.currentAlloyCode) || !this.alloys.ContainsKey(this.currentAlloyCode);
			if (!flag)
			{
				AlloyData alloyData = this.alloys[this.currentAlloyCode];
				bool flag2 = alloyData.Recipe.ContainsKey(metal);
				if (flag2)
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
					bool flag3 = staticText != null;
					if (flag3)
					{
						staticText.SetValue(value4);
					}
				}
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00003A54 File Offset: 0x00001C54
		private void UpdatePercentageLabels()
		{
			bool flag = base.SingleComposer == null || string.IsNullOrEmpty(this.currentAlloyCode) || !this.alloys.ContainsKey(this.currentAlloyCode);
			if (!flag)
			{
				foreach (KeyValuePair<string, double[]> item in this.alloys[this.currentAlloyCode].Recipe)
				{
					string key = item.Key;
					string key2 = this.currentAlloyCode + "_" + key;
					int value = 50;
					GuiElementSlider slider = GuiComposerHelpers.GetSlider(base.SingleComposer, "slider_" + key);
					bool flag2 = slider != null;
					if (flag2)
					{
						value = slider.GetValue();
						this.alloyMetalPercentages[key2] = value;
					}
					else
					{
						bool flag3 = this.alloyMetalPercentages.ContainsKey(key2);
						if (flag3)
						{
							value = this.alloyMetalPercentages[key2];
						}
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
					bool flag4 = staticText != null;
					if (flag4)
					{
						staticText.SetValue(value4);
					}
				}
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00003C34 File Offset: 0x00001E34
		private void CalculateResults()
		{
			bool flag = base.SingleComposer == null;
			if (flag)
			{
				this.capi.Logger.Warning("Critcher's Calculator: CalculateResults called but SingleComposer is null!");
			}
			else
			{
				bool flag2 = string.IsNullOrEmpty(this.currentAlloyCode) || !this.alloys.ContainsKey(this.currentAlloyCode);
				if (flag2)
				{
					this.capi.Logger.Warning("Critcher's Calculator: Invalid currentAlloyCode: {0}", new object[]
					{
						this.currentAlloyCode
					});
				}
				else
				{
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
						bool flag3 = this.alloyMetalPercentages.ContainsKey(key2);
						int num7;
						if (flag3)
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
					bool flag4 = num4 != num3;
					if (flag4)
					{
						int num9 = num3 - num4;
						List<KeyValuePair<string, int>> list = Enumerable.ToList<KeyValuePair<string, int>>(Enumerable.OrderByDescending<KeyValuePair<string, int>, int>(dictionary, (KeyValuePair<string, int> x) => x.Value));
						int num10 = 0;
						while (num9 != 0 && num10 < 100)
						{
							num10++;
							foreach (KeyValuePair<string, int> item2 in list)
							{
								bool flag5 = num9 == 0;
								if (flag5)
								{
									break;
								}
								string key3 = item2.Key;
								bool flag6 = num9 > 0;
								if (flag6)
								{
									Dictionary<string, int> dictionary2 = dictionary;
									string text2 = key3;
									int num11 = dictionary2[text2];
									dictionary2[text2] = num11 + 1;
									num9--;
								}
								else
								{
									bool flag7 = num9 < 0 && dictionary[key3] > 1;
									if (flag7)
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
					}
					int totalUnitsAfter = num * 100;
					List<KeyValuePair<string, int>> orderedMetals = Enumerable.ToList<KeyValuePair<string, int>>(Enumerable.OrderByDescending<KeyValuePair<string, int>, int>(dictionary, (KeyValuePair<string, int> x) => x.Value));
					foreach (KeyValuePair<string, double[]> item3 in alloyData.Recipe)
					{
						string metal = item3.Key;
						bool flag8 = !dictionary.ContainsKey(metal);
						if (!flag8)
						{
							double minPct = item3.Value[0];
							double maxPct = item3.Value[1];
							int actualUnits = dictionary[metal] * 5;
							double actualPct = (double)actualUnits / (double)totalUnitsAfter;
							bool flag9 = actualPct < minPct;
							if (flag9)
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
										bool flag10 = kv.Key == metal;
										if (!flag10)
										{
											double otherMax = alloyData.Recipe[kv.Key][1];
											int otherUnits = (dictionary[kv.Key] - 1) * 5;
											bool flag11 = dictionary[kv.Key] > 1 && (double)otherUnits / (double)totalUnitsAfter <= otherMax;
											if (flag11)
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
						bool flag12 = dictionary.ContainsKey(key4);
						if (flag12)
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
					bool flag13 = string.IsNullOrEmpty(text);
					if (flag13)
					{
						text = Lang.Get("critcherscalculator:no-data", Array.Empty<object>());
					}
					GuiElementRichtext richtext = GuiComposerHelpers.GetRichtext(base.SingleComposer, "resultText");
					bool flag14 = richtext != null;
					if (flag14)
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
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00004328 File Offset: 0x00002528
		private static List<GuiDialogOreCalculator.SlotFill> PackNuggetsIntoSlots(Dictionary<string, int> nuggetsPerMetal)
		{
			List<GuiDialogOreCalculator.SlotFill> slots = new List<GuiDialogOreCalculator.SlotFill>();
			foreach (KeyValuePair<string, int> kv in nuggetsPerMetal)
			{
				int remaining = kv.Value;
				bool flag = remaining <= 0;
				if (!flag)
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

		// Token: 0x0600001A RID: 26 RVA: 0x000043D0 File Offset: 0x000025D0
		private static int SlotsRequired(Dictionary<string, int> nuggetsPerMetal)
		{
			int slots = 0;
			foreach (KeyValuePair<string, int> kv in nuggetsPerMetal)
			{
				bool flag = kv.Value <= 0;
				if (!flag)
				{
					slots += (kv.Value + 128 - 1) / 128;
				}
			}
			return slots;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00004450 File Offset: 0x00002650
		private Dictionary<string, int> ComputeNuggetsForIngotCount(int ingotCount)
		{
			Dictionary<string, int> result = new Dictionary<string, int>();
			bool flag = string.IsNullOrEmpty(this.currentAlloyCode) || !this.alloys.ContainsKey(this.currentAlloyCode) || ingotCount < 1;
			Dictionary<string, int> result2;
			if (flag)
			{
				result2 = result;
			}
			else
			{
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
				bool flag2 = sum != totalNuggetsTarget;
				if (flag2)
				{
					int diff = totalNuggetsTarget - sum;
					List<KeyValuePair<string, int>> ordered = Enumerable.ToList<KeyValuePair<string, int>>(Enumerable.OrderByDescending<KeyValuePair<string, int>, int>(result, (KeyValuePair<string, int> x) => x.Value));
					int safety = 0;
					while (diff != 0 && safety < 100)
					{
						safety++;
						foreach (KeyValuePair<string, int> kv in ordered)
						{
							bool flag3 = diff == 0;
							if (flag3)
							{
								break;
							}
							bool flag4 = diff > 0;
							if (flag4)
							{
								Dictionary<string, int> dictionary = result;
								string text = kv.Key;
								int num = dictionary[text];
								dictionary[text] = num + 1;
								diff--;
							}
							else
							{
								bool flag5 = diff < 0 && result[kv.Key] > 1;
								if (flag5)
								{
									Dictionary<string, int> dictionary2 = result;
									string text = kv.Key;
									int num = dictionary2[text];
									dictionary2[text] = num - 1;
									diff++;
								}
							}
						}
					}
					AlloyData ad = alloyData;
					List<KeyValuePair<string, int>> orderedSafe = ordered;
					foreach (KeyValuePair<string, double[]> kv2 in ad.Recipe)
					{
						string metal2 = kv2.Key;
						bool flag6 = !result.ContainsKey(metal2);
						if (!flag6)
						{
							double minPct2 = kv2.Value[0];
							double maxPct2 = kv2.Value[1];
							int actualUnits = result[metal2] * 5;
							double actualPct = (double)actualUnits / (double)totalUnits;
							bool flag7 = actualPct < minPct2;
							if (flag7)
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
										bool flag8 = other.Key == metal2;
										if (!flag8)
										{
											double otherMax = ad.Recipe[other.Key][1];
											int otherUnits = (result[other.Key] - 1) * 5;
											bool flag9 = result[other.Key] > 1 && (double)otherUnits / (double)totalUnits <= otherMax;
											if (flag9)
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
				result2 = result;
			}
			return result2;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000048FC File Offset: 0x00002AFC
		private int ComputeMaxCraftableIngots(int upperBound)
		{
			for (int candidate = upperBound; candidate >= 1; candidate--)
			{
				Dictionary<string, int> nuggets = this.ComputeNuggetsForIngotCount(candidate);
				bool flag = GuiDialogOreCalculator.SlotsRequired(nuggets) <= 4;
				if (flag)
				{
					return candidate;
				}
			}
			return 0;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00004944 File Offset: 0x00002B44
		private void UpdateCrucibleSlots(Dictionary<string, int> nuggetsPerMetal)
		{
			bool flag = base.SingleComposer == null;
			if (!flag)
			{
				int slotsNeeded = GuiDialogOreCalculator.SlotsRequired(nuggetsPerMetal);
				bool impossible = slotsNeeded > 4;
				List<GuiDialogOreCalculator.SlotFill> slots = impossible ? new List<GuiDialogOreCalculator.SlotFill>() : GuiDialogOreCalculator.PackNuggetsIntoSlots(nuggetsPerMetal);
				for (int i = 0; i < 4; i++)
				{
					GuiElementDynamicText metalLabel = GuiElementDynamicTextHelper.GetDynamicText(base.SingleComposer, "slot_metal_" + i.ToString());
					GuiElementDynamicText countLabel = GuiElementDynamicTextHelper.GetDynamicText(base.SingleComposer, "slot_count_" + i.ToString());
					bool flag2 = i < slots.Count;
					if (flag2)
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
				bool flag3 = warning == null;
				if (!flag3)
				{
					bool flag4 = impossible;
					if (flag4)
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
					}
					else
					{
						warning.SetNewText("", CairoFont.WhiteSmallText(), null);
					}
				}
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00004B29 File Offset: 0x00002D29
		private void OnTitleBarClose()
		{
			this.TryClose();
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00004B34 File Offset: 0x00002D34
		private bool OnCloseClick()
		{
			this.TryClose();
			return true;
		}

		// Token: 0x04000004 RID: 4
		private const int UNITS_PER_INGOT = 100;

		// Token: 0x04000005 RID: 5
		private const int UNITS_PER_CHUNK = 5;

		// Token: 0x04000006 RID: 6
		private const int NUGGETS_PER_SLOT = 128;

		// Token: 0x04000007 RID: 7
		private const int CRUCIBLE_SLOTS = 4;

		// Token: 0x04000008 RID: 8
		private Dictionary<string, AlloyData> alloys;

		// Token: 0x04000009 RID: 9
		private Dictionary<string, int> alloyIngots = new Dictionary<string, int>();

		// Token: 0x0400000A RID: 10
		private Dictionary<string, int> alloyMetalPercentages = new Dictionary<string, int>();

		// Token: 0x0400000B RID: 11
		private string currentAlloyCode;

		// Token: 0x02000006 RID: 6
		private readonly struct SlotFill : IEquatable<GuiDialogOreCalculator.SlotFill>
		{
			// Token: 0x06000021 RID: 33 RVA: 0x00004B8A File Offset: 0x00002D8A
			public SlotFill(string MetalCode, int NuggetCount)
			{
				this.MetalCode = MetalCode;
				this.NuggetCount = NuggetCount;
			}

			// Token: 0x17000003 RID: 3
			// (get) Token: 0x06000022 RID: 34 RVA: 0x00004B9A File Offset: 0x00002D9A
			// (set) Token: 0x06000023 RID: 35 RVA: 0x00004BA2 File Offset: 0x00002DA2
			public string MetalCode { get; set; }

			// Token: 0x17000004 RID: 4
			// (get) Token: 0x06000024 RID: 36 RVA: 0x00004BAB File Offset: 0x00002DAB
			// (set) Token: 0x06000025 RID: 37 RVA: 0x00004BB3 File Offset: 0x00002DB3
			public int NuggetCount { get; set; }

			// Token: 0x06000026 RID: 38 RVA: 0x00004BBC File Offset: 0x00002DBC
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

			// Token: 0x06000027 RID: 39 RVA: 0x00004C08 File Offset: 0x00002E08
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				builder.Append("MetalCode = ");
				builder.Append(this.MetalCode);
				builder.Append(", NuggetCount = ");
				builder.Append(this.NuggetCount.ToString());
				return true;
			}

			// Token: 0x06000028 RID: 40 RVA: 0x00004C56 File Offset: 0x00002E56
			[CompilerGenerated]
			public static bool operator !=(GuiDialogOreCalculator.SlotFill left, GuiDialogOreCalculator.SlotFill right)
			{
				return !(left == right);
			}

			// Token: 0x06000029 RID: 41 RVA: 0x00004C62 File Offset: 0x00002E62
			[CompilerGenerated]
			public static bool operator ==(GuiDialogOreCalculator.SlotFill left, GuiDialogOreCalculator.SlotFill right)
			{
				return left.Equals(right);
			}

			// Token: 0x0600002A RID: 42 RVA: 0x00004C6C File Offset: 0x00002E6C
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return EqualityComparer<string>.Default.GetHashCode(this.<MetalCode>k__BackingField) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.<NuggetCount>k__BackingField);
			}

			// Token: 0x0600002B RID: 43 RVA: 0x00004C95 File Offset: 0x00002E95
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return obj is GuiDialogOreCalculator.SlotFill && this.Equals((GuiDialogOreCalculator.SlotFill)obj);
			}

			// Token: 0x0600002C RID: 44 RVA: 0x00004CAE File Offset: 0x00002EAE
			[CompilerGenerated]
			public bool Equals(GuiDialogOreCalculator.SlotFill other)
			{
				return EqualityComparer<string>.Default.Equals(this.<MetalCode>k__BackingField, other.<MetalCode>k__BackingField) && EqualityComparer<int>.Default.Equals(this.<NuggetCount>k__BackingField, other.<NuggetCount>k__BackingField);
			}

			// Token: 0x0600002D RID: 45 RVA: 0x00004CE1 File Offset: 0x00002EE1
			[CompilerGenerated]
			public void Deconstruct(out string MetalCode, out int NuggetCount)
			{
				MetalCode = this.MetalCode;
				NuggetCount = this.NuggetCount;
			}
		}
	}
}
