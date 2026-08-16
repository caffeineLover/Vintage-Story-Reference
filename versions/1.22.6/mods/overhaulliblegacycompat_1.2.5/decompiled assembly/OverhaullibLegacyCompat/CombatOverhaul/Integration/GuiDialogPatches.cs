using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using CombatOverhaul.Armor;
using CombatOverhaul.DamageSystems;
using CombatOverhaul.Utils;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.Client.NoObf;
using Vintagestory.Common;
using Vintagestory.GameContent;

namespace CombatOverhaul.Integration
{
	// Token: 0x0200003E RID: 62
	[NullableContext(1)]
	[Nullable(0)]
	internal static class GuiDialogPatches
	{
		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060003F6 RID: 1014 RVA: 0x00013B75 File Offset: 0x00011D75
		// (set) Token: 0x060003F7 RID: 1015 RVA: 0x00013B7C File Offset: 0x00011D7C
		[Nullable(2)]
		public static GuiDialogInventory GuiDialogInventoryInstance { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x060003F8 RID: 1016 RVA: 0x00013B84 File Offset: 0x00011D84
		public static void Patch(string harmonyId, ICoreClientAPI api)
		{
			GuiDialogPatches.Api = api;
			Harmony harmony = new Harmony(harmonyId);
			harmony.Patch(typeof(GuiDialogCharacter).GetMethod("ComposeCharacterTab", AccessTools.all), new HarmonyMethod(AccessTools.Method(typeof(CharacterTabPatch), "GuiDialogCharacter_ComposeCharacterTab", null, null)), null, null, null);
			harmony.Patch(typeof(GuiDialogCharacter).GetMethod("OnRenderGUI", AccessTools.all), null, new HarmonyMethod(AccessTools.Method(typeof(GuiDialogPatches), "OnRenderGUI", null, null)), null, null);
			harmony.Patch(typeof(GuiDialogInventory).GetMethod("ComposeSurvivalInvDialog", AccessTools.all), new HarmonyMethod(AccessTools.Method(typeof(GuiDialogPatches), "GuiDialogInventory_ComposeSurvivalInvDialog", null, null)), null, null, null);
			harmony.Patch(typeof(GuiDialogInventory).GetMethod("OnNewScrollbarvalue", AccessTools.all), new HarmonyMethod(AccessTools.Method(typeof(GuiDialogPatches), "GuiDialogInventory_OnNewScrollbarvalue_Patch", null, null)), null, null, null);
			harmony.Patch(typeof(CharacterSystem).GetMethod("StartClientSide", AccessTools.all), null, new HarmonyMethod(AccessTools.Method(typeof(GuiDialogPatches), "CharacterSystem_StartClientSide", null, null)), null, null);
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x00013CD4 File Offset: 0x00011ED4
		public static void Unpatch(string harmonyId)
		{
			Harmony harmony = new Harmony(harmonyId);
			harmony.Unpatch(typeof(GuiDialogCharacter).GetMethod("ComposeCharacterTab", AccessTools.all), 1, harmonyId);
			harmony.Unpatch(typeof(GuiDialogCharacter).GetMethod("OnRenderGUI", AccessTools.all), 2, harmonyId);
			harmony.Unpatch(typeof(GuiDialogInventory).GetMethod("ComposeSurvivalInvDialog", AccessTools.all), 1, harmonyId);
			harmony.Unpatch(typeof(GuiDialogInventory).GetMethod("OnNewScrollbarvalue", AccessTools.all), 1, harmonyId);
			harmony.Unpatch(typeof(CharacterSystem).GetMethod("StartClientSide", AccessTools.all), 2, harmonyId);
			GuiDialogPatches.Api = null;
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x00013D94 File Offset: 0x00011F94
		public static void RecalculateArmorStatsForGui()
		{
			try
			{
				Action recalcualteArmorStats = GuiDialogPatches._recalcualteArmorStats;
				if (recalcualteArmorStats != null)
				{
					recalcualteArmorStats.Invoke();
				}
			}
			catch (Exception exception)
			{
				ICoreAPI api = GuiDialogPatches.Api;
				Type typeFromHandle = typeof(GuiDialogPatches);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(47, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Error while recalculating armor stats for GUI:\n");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
				LoggerUtil.Warn(api, typeFromHandle, defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060003FB RID: 1019 RVA: 0x00013E00 File Offset: 0x00012000
		// (set) Token: 0x060003FC RID: 1020 RVA: 0x00013E07 File Offset: 0x00012007
		[Nullable(2)]
		public static ICoreClientAPI Api { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x060003FD RID: 1021 RVA: 0x00013E10 File Offset: 0x00012010
		private static bool GuiDialogInventory_ComposeSurvivalInvDialog(GuiDialogInventory __instance)
		{
			GuiDialogPatches.GuiDialogInventoryInstance = __instance;
			if (GuiDialogPatches.Api == null)
			{
				return true;
			}
			IInventory backPackInv = GuiDialogPatches.ResolveInventoryField(__instance, GuiDialogPatches.GuiDialogInventory_backPackInv, new string[]
			{
				"backPackInv",
				"backpackInv",
				"BackPackInv",
				"BackpackInv"
			});
			IInventory craftingInv = GuiDialogPatches.ResolveInventoryField(__instance, GuiDialogPatches.GuiDialogInventory_craftingInv, new string[]
			{
				"craftingInv",
				"CraftingInv"
			});
			if (backPackInv == null || craftingInv == null)
			{
				try
				{
					List<string> candidates = new List<string>();
					foreach (FieldInfo field in __instance.GetType().GetFields(52))
					{
						if (typeof(IInventory).IsAssignableFrom(field.FieldType))
						{
							object value = field.GetValue(__instance);
							List<string> list = candidates;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 3);
							defaultInterpolatedStringHandler.AppendFormatted(field.Name);
							defaultInterpolatedStringHandler.AppendLiteral(":");
							defaultInterpolatedStringHandler.AppendFormatted(field.FieldType.Name);
							defaultInterpolatedStringHandler.AppendLiteral("=");
							defaultInterpolatedStringHandler.AppendFormatted((value == null) ? "null" : "set");
							list.Add(defaultInterpolatedStringHandler.ToStringAndClear());
						}
					}
					ILogger logger = GuiDialogPatches.Api.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(88, 3);
					defaultInterpolatedStringHandler2.AppendLiteral("[OverhaullibLegacyCompat] Inventory fields unresolved. backpack=");
					defaultInterpolatedStringHandler2.AppendFormatted((backPackInv == null) ? "null" : "set");
					defaultInterpolatedStringHandler2.AppendLiteral(" crafting=");
					defaultInterpolatedStringHandler2.AppendFormatted((craftingInv == null) ? "null" : "set");
					defaultInterpolatedStringHandler2.AppendLiteral(" candidates=[");
					defaultInterpolatedStringHandler2.AppendFormatted(string.Join(", ", candidates));
					defaultInterpolatedStringHandler2.AppendLiteral("]");
					logger.Warning(defaultInterpolatedStringHandler2.ToStringAndClear());
				}
				catch (Exception ex)
				{
					ILogger logger2 = GuiDialogPatches.Api.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3;
					defaultInterpolatedStringHandler3..ctor(62, 1);
					defaultInterpolatedStringHandler3.AppendLiteral("[OverhaullibLegacyCompat] Inventory field diagnostics failed: ");
					defaultInterpolatedStringHandler3.AppendFormatted<Exception>(ex);
					logger2.Warning(defaultInterpolatedStringHandler3.ToStringAndClear());
				}
				return true;
			}
			double elemToDlgPad = GuiStyle.ElementToDialogPadding;
			double pad = GuiElementItemSlotGridBase.unscaledSlotPadding;
			int rows2 = (int)Math.Ceiling((double)((float)backPackInv.Count / 6f));
			FieldInfo guiDialogInventory_prevRows = GuiDialogPatches.GuiDialogInventory_prevRows;
			if (guiDialogInventory_prevRows != null)
			{
				guiDialogInventory_prevRows.SetValue(__instance, rows2);
			}
			ElementBounds slotGridBounds = ElementStdBounds.SlotGrid(0, pad, pad, 6, 7).FixedGrow(2.0 * pad, 2.0 * pad);
			ElementBounds fullGridBounds = ElementStdBounds.SlotGrid(0, 0.0, 0.0, 6, rows2);
			ElementBounds insetBounds = slotGridBounds.ForkBoundingParent(3.0, 3.0, 3.0, 3.0);
			ElementBounds clippingBounds = slotGridBounds.CopyOffsetedSibling(0.0, 0.0, 0.0, 0.0);
			clippingBounds.fixedHeight -= 3.0;
			ElementBounds gridBounds = ElementStdBounds.SlotGrid(0, 0.0, 0.0, 3, 3).FixedRightOf(insetBounds, 45.0);
			gridBounds.fixedY += 50.0;
			ElementBounds outputBounds = ElementStdBounds.SlotGrid(0, 0.0, 0.0, 1, 1).FixedRightOf(insetBounds, 45.0).FixedUnder(gridBounds, 20.0);
			outputBounds.fixedX += pad + GuiElementPassiveItemSlot.unscaledSlotSize;
			ElementBounds dialogBounds = insetBounds.ForkBoundingParent(elemToDlgPad, elemToDlgPad + 30.0, elemToDlgPad + gridBounds.fixedWidth + 20.0, elemToDlgPad);
			if (GuiDialogPatches.Api.Settings.Bool["immersiveMouseMode"])
			{
				dialogBounds.WithAlignment(10).WithFixedAlignmentOffset(-12.0, 0.0);
			}
			else
			{
				dialogBounds.WithAlignment(6).WithFixedAlignmentOffset(20.0, 0.0);
			}
			ElementBounds scrollBarBounds = ElementStdBounds.VerticalScrollbar(insetBounds).WithParent(dialogBounds);
			scrollBarBounds.fixedOffsetX -= 2.0;
			scrollBarBounds.fixedWidth = 15.0;
			GuiComposer survivalInvDialog = GuiDialogPatches.Api.Gui.CreateCompo("inventory-backpack", dialogBounds);
			GuiComposerHelpers.AddShadedDialogBG(survivalInvDialog, ElementBounds.Fill, true, 5.0, 0.75f);
			GuiComposerHelpers.AddDialogTitleBar(survivalInvDialog, Lang.Get("Inventory and Crafting", Array.Empty<object>()), delegate()
			{
				GuiDialogPatches.CloseIconPressed(__instance);
			}, null, null, null);
			GuiComposerHelpers.AddVerticalScrollbar(survivalInvDialog, delegate(float data)
			{
				GuiDialogPatches.OnNewScrollbarvalue(__instance, data);
			}, scrollBarBounds, "scrollbar");
			GuiElementInsetHelper.AddInset(survivalInvDialog, insetBounds, 3, 0.85f);
			GuiElementClipHelpler.BeginClip(survivalInvDialog, clippingBounds);
			GuiDialogPatches.ComposeBackpackSlots(survivalInvDialog, __instance, backPackInv, fullGridBounds);
			GuiElementClipHelpler.EndClip(survivalInvDialog);
			GuiComposerHelpers.AddItemSlotGrid(survivalInvDialog, craftingInv, delegate(object data)
			{
				GuiDialogPatches.SendInvPacket(__instance, data);
			}, 3, new int[]
			{
				0,
				1,
				2,
				3,
				4,
				5,
				6,
				7,
				8
			}, gridBounds, "craftinggrid");
			GuiComposerHelpers.AddItemSlotGrid(survivalInvDialog, craftingInv, delegate(object data)
			{
				GuiDialogPatches.SendInvPacket(__instance, data);
			}, 1, new int[]
			{
				9
			}, outputBounds, "outputslot");
			try
			{
				survivalInvDialog.Compose(true);
			}
			catch (Exception ex2)
			{
				ICoreAPI api = GuiDialogPatches.Api;
				Type typeFromHandle = typeof(GuiDialogPatches);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4;
				defaultInterpolatedStringHandler4..ctor(49, 1);
				defaultInterpolatedStringHandler4.AppendLiteral("Error while composing survival inventory dialog:\n");
				defaultInterpolatedStringHandler4.AppendFormatted<Exception>(ex2);
				LoggerUtil.Warn(api, typeFromHandle, defaultInterpolatedStringHandler4.ToStringAndClear());
				return true;
			}
			GuiComposerHelpers.GetScrollbar(survivalInvDialog, "scrollbar").SetHeights((float)slotGridBounds.fixedHeight, (float)Enumerable.Sum(Enumerable.Select<int, double>(GuiDialogPatches._rows, (int rows) => ((double)rows + 0.4) * (GuiElementPassiveItemSlot.unscaledSlotSize + GuiElementItemSlotGridBase.unscaledSlotPadding))) - 12f);
			FieldInfo guiDialogInventory_survivalInvDialog = GuiDialogPatches.GuiDialogInventory_survivalInvDialog;
			if (guiDialogInventory_survivalInvDialog != null)
			{
				guiDialogInventory_survivalInvDialog.SetValue(__instance, survivalInvDialog);
			}
			return false;
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x00014470 File Offset: 0x00012670
		private static bool GuiDialogInventory_OnNewScrollbarvalue_Patch(GuiDialogInventory __instance, float value)
		{
			if (!__instance.IsOpened())
			{
				return true;
			}
			GuiComposer sid = (GuiComposer)GuiDialogPatches.GuiDialogInventory_survivalInvDialog.GetValue(__instance);
			if (GuiDialogPatches.Api.World.Player.WorldData.CurrentGameMode == 2)
			{
				return true;
			}
			if (GuiDialogPatches.Api.World.Player.WorldData.CurrentGameMode == 1 && sid != null)
			{
				ElementBounds bounds = GuiComposerHelpers.GetSlotGridExcl(sid, "slotgrid").Bounds;
				bounds.fixedY = 10.0 - GuiElementItemSlotGridBase.unscaledSlotPadding - (double)value;
				bounds.CalcWorldBounds();
				int index = 0;
				for (;;)
				{
					try
					{
						GuiComposer guiComposer = sid;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 1);
						defaultInterpolatedStringHandler.AppendLiteral("slotgrid-");
						defaultInterpolatedStringHandler.AppendFormatted<int>(index);
						if (guiComposer.GetElement(defaultInterpolatedStringHandler.ToStringAndClear()) != null)
						{
							GuiComposer guiComposer2 = sid;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(9, 1);
							defaultInterpolatedStringHandler2.AppendLiteral("slotgrid-");
							defaultInterpolatedStringHandler2.AppendFormatted<int>(index);
							ElementBounds bounds3 = GuiComposerHelpers.GetSlotGridExcl(guiComposer2, defaultInterpolatedStringHandler2.ToStringAndClear()).Bounds;
							GuiComposer guiComposer3 = sid;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(9, 1);
							defaultInterpolatedStringHandler3.AppendLiteral("category-");
							defaultInterpolatedStringHandler3.AppendFormatted<int>(index);
							ElementBounds bounds2 = GuiComposerHelpers.GetRichtext(guiComposer3, defaultInterpolatedStringHandler3.ToStringAndClear()).Bounds;
							bounds2.fixedY = bounds.fixedY + (GuiElementPassiveItemSlot.unscaledSlotSize + GuiElementItemSlotGridBase.unscaledSlotPadding) * (double)GuiDialogPatches._rows[index];
							bounds3.fixedY = bounds.fixedY + (GuiElementPassiveItemSlot.unscaledSlotSize + GuiElementItemSlotGridBase.unscaledSlotPadding) * ((double)GuiDialogPatches._rows[index] + 0.4);
							bounds3.CalcWorldBounds();
							bounds2.CalcWorldBounds();
							bounds = bounds3;
							index++;
							continue;
						}
					}
					catch (Exception exception)
					{
						GuiDialogPatches.LogInventoryLayoutError(exception);
					}
					break;
				}
			}
			return false;
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x00014624 File Offset: 0x00012824
		private static void CharacterSystem_StartClientSide(CharacterSystem __instance, ICoreClientAPI api)
		{
			if (!api.ModLoader.IsModEnabled("combatoverhaul") && !api.ModLoader.IsModEnabled("combatoverhaulfork"))
			{
				return;
			}
			FieldInfo characterSystem_charDlg = GuiDialogPatches.CharacterSystem_charDlg;
			GuiDialogCharacterBase dialog = (GuiDialogCharacterBase)((characterSystem_charDlg != null) ? characterSystem_charDlg.GetValue(__instance) : null);
			if (dialog == null)
			{
				return;
			}
			dialog.Tabs.Add(new GuiTab
			{
				Name = Lang.Get("charactertab-armor", Array.Empty<object>()),
				DataInt = dialog.Tabs.Count
			});
			List<Action<GuiComposer>> renderTabHandlers = dialog.RenderTabHandlers;
			Action<GuiComposer> action;
			if ((action = GuiDialogPatches.<>O.<0>__composeArmorTab) == null)
			{
				action = (GuiDialogPatches.<>O.<0>__composeArmorTab = new Action<GuiComposer>(GuiDialogPatches.composeArmorTab));
			}
			renderTabHandlers.Add(action);
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x000146D0 File Offset: 0x000128D0
		private static void composeArmorTab(GuiComposer compo)
		{
			if (!ArmorInventory._disableVanillaArmorSlots)
			{
				return;
			}
			double gap = GuiElement.scaled(GuiElementItemSlotGridBase.unscaledSlotPadding);
			double textGap = gap;
			GuiElement.scaled(9.0);
			double textWidth = 60.0;
			ArmorInventory inv = GuiDialogPatches.Api.World.Player.InventoryManager.GetOwnInventory("character") as ArmorInventory;
			if (inv == null)
			{
				return;
			}
			CairoFont textFont = CairoFont.WhiteSmallText();
			CairoFont textFontMiddle = CairoFont.WhiteSmallText();
			CairoFont.ButtonText();
			textFont.Orientation = 1;
			textFontMiddle.Orientation = 2;
			int height = 276;
			ElementBounds topInsetBounds = ElementBounds.Fixed(0.0, 22.0, 414.0, 80.0);
			ElementBounds tierTextBounds = ElementBounds.Fixed(0.0, 22.0, 80.0, 40.0);
			ElementBounds tierSliderBounds = ElementBounds.Fixed(0.0, 22.0, 130.0, 40.0).RightOf(tierTextBounds, 4.0);
			ElementBounds button1Bounds = ElementBounds.Fixed(0.0, 22.0, 194.0, 36.0).FixedRightOf(tierSliderBounds, 84.0).WithFixedOffset(0.0, 2.0);
			ElementBounds button1TextBounds = button1Bounds.FlatCopy();
			ElementBounds leftLegendTextBounds = ElementBounds.Fixed(0.0, 0.0, 80.0, 40.0).FixedUnder(button1Bounds, 4.0);
			ElementBounds middleLegendTextBounds = ElementBounds.Fixed(0.0, 0.0, 120.0, 40.0).FixedUnder(button1Bounds, 4.0).RightOf(leftLegendTextBounds, 4.0);
			ElementBounds rightLegendTextBounds = ElementBounds.Fixed(0.0, 0.0, 190.0, 40.0).FixedUnder(button1Bounds, 4.0).RightOf(middleLegendTextBounds, 12.0);
			ElementBounds outerInsetBounds = ElementBounds.Fixed(0.0, 0.0, 414.0, (double)height).FixedUnder(topInsetBounds, 8.0);
			ElementBounds baseBounds = ElementBounds.Fixed(0.0, -52.0, 388.0, (double)height).FixedUnder(topInsetBounds, 8.0);
			ElementBounds childBounds = ElementBounds.Fixed(0.0, 2.0, 388.0, (double)(height + 140));
			ElementBounds textBounds = ElementStdBounds.Slot(0.0, 0.0).WithFixedWidth(textWidth).WithFixedAlignmentOffset(0.0, 12.0);
			ElementBounds slot0Bounds = ElementStdBounds.Slot(textBounds.RightCopy(8.0, 0.0, 0.0, 0.0).fixedX, textBounds.RightCopy(0.0, 0.0, 0.0, 0.0).fixedY);
			ElementBounds slot1Bounds = ElementStdBounds.Slot(slot0Bounds.RightCopy(gap, 0.0, 0.0, 0.0).fixedX, slot0Bounds.RightCopy(0.0, 0.0, 0.0, 0.0).fixedY);
			ElementBounds slot2Bounds = ElementStdBounds.Slot(slot1Bounds.RightCopy(gap, 0.0, 0.0, 0.0).fixedX, slot1Bounds.RightCopy(0.0, 0.0, 0.0, 0.0).fixedY);
			ElementBounds statsText1Bounds = ElementStdBounds.Slot(slot2Bounds.RightCopy(gap, 0.0, 0.0, 0.0).fixedX, slot2Bounds.RightCopy(0.0, 0.0, 0.0, 0.0).fixedY).WithFixedWidth(56.0).WithFixedAlignmentOffset(0.0, 12.0);
			ElementBounds statsText2Bounds = statsText1Bounds.RightCopy(0.0, 0.0, 0.0, 0.0);
			ElementBounds statsText3Bounds = statsText2Bounds.RightCopy(0.0, 0.0, 0.0, 0.0);
			ElementBounds scrollbarBounds = baseBounds.CopyOffsetedSibling(baseBounds.fixedWidth + 7.0, -6.0, 0.0, 0.0).WithFixedWidth(20.0);
			ElementBounds clipBounds = baseBounds.FlatCopy().FixedGrow(6.0, 11.0).WithFixedOffset(0.0, -6.0).WithFixedHeight((double)height);
			GuiElementInsetHelper.AddInset(compo, topInsetBounds, 2, 0.85f);
			GuiElementDynamicTextHelper.AddDynamicText(compo, "", textFont, tierTextBounds.WithFixedOffset(0.0, 10.0), "textTier");
			GuiComposerHelpers.AddHoverText(compo, Lang.Get("combatoverhaul:armor-inventory-dialog-attack-tier-hint", Array.Empty<object>()), CairoFont.WhiteSmallText(), 400, tierTextBounds.FlatCopy().WithFixedOffset(0.0, -10.0), null);
			GuiComposerHelpers.AddSlider(compo, delegate(int value)
			{
				GuiDialogPatches._currentAttackTier = value;
				GuiDialogPatches.SetStatsValues(compo, GuiDialogPatches.Api.World.Player, value);
				return true;
			}, tierSliderBounds, "tierSlider");
			GuiComposerHelpers.AddButton(compo, "", () => GuiDialogPatches.ToggleBodyPartsMultipliers(compo), button1Bounds, textFont, 3, "button1");
			GuiElementDynamicTextHelper.AddDynamicText(compo, GuiDialogPatches.GetCalcTypeText(), textFontMiddle, button1TextBounds.WithFixedOffset(0.0, 8.0), "textButton");
			GuiComposerHelpers.AddHoverText(compo, GuiDialogPatches.GetCalcTypeHoverText(), CairoFont.WhiteSmallText(), 600, button1TextBounds.FlatCopy().WithFixedOffset(0.0, -10.0), null);
			GuiElementDynamicTextHelper.AddDynamicText(compo, "", textFontMiddle, leftLegendTextBounds.WithFixedOffset(0.0, 10.0), "textLeftLegend");
			GuiComposerHelpers.AddHoverText(compo, Lang.Get("combatoverhaul:armor-inventory-dialog-attack-legend-left-hint", Array.Empty<object>()), CairoFont.WhiteSmallText(), 400, leftLegendTextBounds.FlatCopy().WithFixedOffset(0.0, -10.0), null);
			GuiElementDynamicTextHelper.AddDynamicText(compo, "", textFontMiddle, middleLegendTextBounds.WithFixedOffset(0.0, 10.0), "textMiddleLegend");
			GuiComposerHelpers.AddHoverText(compo, Lang.Get("combatoverhaul:armor-inventory-dialog-attack-legend-middle-hint", Array.Empty<object>()), CairoFont.WhiteSmallText(), 400, middleLegendTextBounds.FlatCopy().RightOf(leftLegendTextBounds, 12.0).WithFixedOffset(0.0, -10.0), null);
			GuiElementDynamicTextHelper.AddDynamicText(compo, "", textFontMiddle, rightLegendTextBounds.WithFixedOffset(0.0, 10.0), "textRightLegend");
			GuiComposerHelpers.AddHoverText(compo, Lang.Get("combatoverhaul:armor-inventory-dialog-attack-legend-right-hint", Array.Empty<object>()), CairoFont.WhiteSmallText(), 500, rightLegendTextBounds.FlatCopy().RightOf(middleLegendTextBounds, 12.0).WithFixedOffset(0.0, -10.0), null);
			GuiElementInsetHelper.AddInset(compo, outerInsetBounds, 2, 0.85f);
			compo.BeginChildElements(baseBounds);
			GuiElementClipHelpler.BeginClip(compo, clipBounds);
			compo.BeginChildElements(childBounds);
			GuiElementDynamicTextHelper.AddDynamicText(compo, "", textFont, textBounds, "textHead");
			GuiElementDynamicTextHelper.AddDynamicText(compo, "", textFont, GuiDialogPatches.BelowCopySet(ref textBounds, 0.0, textGap, 0.0, 0.0), "textFace");
			GuiElementDynamicTextHelper.AddDynamicText(compo, "", textFont, GuiDialogPatches.BelowCopySet(ref textBounds, 0.0, textGap, 0.0, 0.0), "textNeck");
			GuiElementDynamicTextHelper.AddDynamicText(compo, "", textFont, GuiDialogPatches.BelowCopySet(ref textBounds, 0.0, textGap, 0.0, 0.0), "textTorso");
			GuiElementDynamicTextHelper.AddDynamicText(compo, "", textFont, GuiDialogPatches.BelowCopySet(ref textBounds, 0.0, textGap, 0.0, 0.0), "textArms");
			GuiElementDynamicTextHelper.AddDynamicText(compo, "", textFont, GuiDialogPatches.BelowCopySet(ref textBounds, 0.0, textGap, 0.0, 0.0), "textHands");
			GuiElementDynamicTextHelper.AddDynamicText(compo, "", textFont, GuiDialogPatches.BelowCopySet(ref textBounds, 0.0, textGap, 0.0, 0.0), "textLegs");
			GuiElementDynamicTextHelper.AddDynamicText(compo, "", textFont, GuiDialogPatches.BelowCopySet(ref textBounds, 0.0, textGap, 0.0, 0.0), "textFeet");
			GuiDialogPatches.AddSlotHere(compo, inv, ArmorLayers.Outer, DamageZone.Head, ref slot0Bounds, gap);
			GuiDialogPatches.AddSlot(compo, inv, ArmorLayers.Outer, DamageZone.Face, ref slot0Bounds, gap);
			GuiDialogPatches.AddSlot(compo, inv, ArmorLayers.Outer, DamageZone.Neck, ref slot0Bounds, gap);
			GuiDialogPatches.AddSlot(compo, inv, ArmorLayers.Outer, DamageZone.Torso, ref slot0Bounds, gap);
			GuiDialogPatches.AddSlot(compo, inv, ArmorLayers.Outer, DamageZone.Arms, ref slot0Bounds, gap);
			GuiDialogPatches.AddSlot(compo, inv, ArmorLayers.Outer, DamageZone.Hands, ref slot0Bounds, gap);
			GuiDialogPatches.AddSlot(compo, inv, ArmorLayers.Outer, DamageZone.Legs, ref slot0Bounds, gap);
			GuiDialogPatches.AddSlot(compo, inv, ArmorLayers.Outer, DamageZone.Feet, ref slot0Bounds, gap);
			GuiDialogPatches.AddSlotHere(compo, inv, ArmorLayers.Middle, DamageZone.Head, ref slot1Bounds, gap);
			GuiDialogPatches.AddSlot(compo, inv, ArmorLayers.Middle, DamageZone.Face, ref slot1Bounds, gap);
			GuiDialogPatches.AddSlot(compo, inv, ArmorLayers.Middle, DamageZone.Neck, ref slot1Bounds, gap);
			GuiDialogPatches.AddSlot(compo, inv, ArmorLayers.Middle, DamageZone.Torso, ref slot1Bounds, gap);
			GuiDialogPatches.AddSlot(compo, inv, ArmorLayers.Middle, DamageZone.Arms, ref slot1Bounds, gap);
			GuiDialogPatches.AddSlot(compo, inv, ArmorLayers.Middle, DamageZone.Hands, ref slot1Bounds, gap);
			GuiDialogPatches.AddSlot(compo, inv, ArmorLayers.Middle, DamageZone.Legs, ref slot1Bounds, gap);
			GuiDialogPatches.AddSlot(compo, inv, ArmorLayers.Middle, DamageZone.Feet, ref slot1Bounds, gap);
			GuiDialogPatches.AddSlotHere(compo, inv, ArmorLayers.Skin, DamageZone.Head, ref slot2Bounds, gap);
			GuiDialogPatches.AddSlot(compo, inv, ArmorLayers.Skin, DamageZone.Face, ref slot2Bounds, gap);
			GuiDialogPatches.AddSlot(compo, inv, ArmorLayers.Skin, DamageZone.Neck, ref slot2Bounds, gap);
			GuiDialogPatches.AddSlot(compo, inv, ArmorLayers.Skin, DamageZone.Torso, ref slot2Bounds, gap);
			GuiDialogPatches.AddSlot(compo, inv, ArmorLayers.Skin, DamageZone.Arms, ref slot2Bounds, gap);
			GuiDialogPatches.AddSlot(compo, inv, ArmorLayers.Skin, DamageZone.Hands, ref slot2Bounds, gap);
			GuiDialogPatches.AddSlot(compo, inv, ArmorLayers.Skin, DamageZone.Legs, ref slot2Bounds, gap);
			GuiDialogPatches.AddSlot(compo, inv, ArmorLayers.Skin, DamageZone.Feet, ref slot2Bounds, gap);
			GuiElementDynamicTextHelper.AddDynamicText(compo, "", textFontMiddle, statsText1Bounds, "textHeadStats1");
			GuiElementDynamicTextHelper.AddDynamicText(compo, "", textFontMiddle, GuiDialogPatches.BelowCopySet(ref statsText1Bounds, 0.0, textGap, 0.0, 0.0), "textFaceStats1");
			GuiElementDynamicTextHelper.AddDynamicText(compo, "", textFontMiddle, GuiDialogPatches.BelowCopySet(ref statsText1Bounds, 0.0, textGap, 0.0, 0.0), "textNeckStats1");
			GuiElementDynamicTextHelper.AddDynamicText(compo, "", textFontMiddle, GuiDialogPatches.BelowCopySet(ref statsText1Bounds, 0.0, textGap, 0.0, 0.0), "textTorsoStats1");
			GuiElementDynamicTextHelper.AddDynamicText(compo, "", textFontMiddle, GuiDialogPatches.BelowCopySet(ref statsText1Bounds, 0.0, textGap, 0.0, 0.0), "textArmsStats1");
			GuiElementDynamicTextHelper.AddDynamicText(compo, "", textFontMiddle, GuiDialogPatches.BelowCopySet(ref statsText1Bounds, 0.0, textGap, 0.0, 0.0), "textHandsStats1");
			GuiElementDynamicTextHelper.AddDynamicText(compo, "", textFontMiddle, GuiDialogPatches.BelowCopySet(ref statsText1Bounds, 0.0, textGap, 0.0, 0.0), "textLegsStats1");
			GuiElementDynamicTextHelper.AddDynamicText(compo, "", textFontMiddle, GuiDialogPatches.BelowCopySet(ref statsText1Bounds, 0.0, textGap, 0.0, 0.0), "textFeetStats1");
			GuiElementDynamicTextHelper.AddDynamicText(compo, "", textFontMiddle, statsText2Bounds, "textHeadStats2");
			GuiElementDynamicTextHelper.AddDynamicText(compo, "", textFontMiddle, GuiDialogPatches.BelowCopySet(ref statsText2Bounds, 0.0, textGap, 0.0, 0.0), "textFaceStats2");
			GuiElementDynamicTextHelper.AddDynamicText(compo, "", textFontMiddle, GuiDialogPatches.BelowCopySet(ref statsText2Bounds, 0.0, textGap, 0.0, 0.0), "textNeckStats2");
			GuiElementDynamicTextHelper.AddDynamicText(compo, "", textFontMiddle, GuiDialogPatches.BelowCopySet(ref statsText2Bounds, 0.0, textGap, 0.0, 0.0), "textTorsoStats2");
			GuiElementDynamicTextHelper.AddDynamicText(compo, "", textFontMiddle, GuiDialogPatches.BelowCopySet(ref statsText2Bounds, 0.0, textGap, 0.0, 0.0), "textArmsStats2");
			GuiElementDynamicTextHelper.AddDynamicText(compo, "", textFontMiddle, GuiDialogPatches.BelowCopySet(ref statsText2Bounds, 0.0, textGap, 0.0, 0.0), "textHandsStats2");
			GuiElementDynamicTextHelper.AddDynamicText(compo, "", textFontMiddle, GuiDialogPatches.BelowCopySet(ref statsText2Bounds, 0.0, textGap, 0.0, 0.0), "textLegsStats2");
			GuiElementDynamicTextHelper.AddDynamicText(compo, "", textFontMiddle, GuiDialogPatches.BelowCopySet(ref statsText2Bounds, 0.0, textGap, 0.0, 0.0), "textFeetStats2");
			GuiElementDynamicTextHelper.AddDynamicText(compo, "", textFontMiddle, statsText3Bounds, "textHeadStats3");
			GuiElementDynamicTextHelper.AddDynamicText(compo, "", textFontMiddle, GuiDialogPatches.BelowCopySet(ref statsText3Bounds, 0.0, textGap, 0.0, 0.0), "textFaceStats3");
			GuiElementDynamicTextHelper.AddDynamicText(compo, "", textFontMiddle, GuiDialogPatches.BelowCopySet(ref statsText3Bounds, 0.0, textGap, 0.0, 0.0), "textNeckStats3");
			GuiElementDynamicTextHelper.AddDynamicText(compo, "", textFontMiddle, GuiDialogPatches.BelowCopySet(ref statsText3Bounds, 0.0, textGap, 0.0, 0.0), "textTorsoStats3");
			GuiElementDynamicTextHelper.AddDynamicText(compo, "", textFontMiddle, GuiDialogPatches.BelowCopySet(ref statsText3Bounds, 0.0, textGap, 0.0, 0.0), "textArmsStats3");
			GuiElementDynamicTextHelper.AddDynamicText(compo, "", textFontMiddle, GuiDialogPatches.BelowCopySet(ref statsText3Bounds, 0.0, textGap, 0.0, 0.0), "textHandsStats3");
			GuiElementDynamicTextHelper.AddDynamicText(compo, "", textFontMiddle, GuiDialogPatches.BelowCopySet(ref statsText3Bounds, 0.0, textGap, 0.0, 0.0), "textLegsStats3");
			GuiElementDynamicTextHelper.AddDynamicText(compo, "", textFontMiddle, GuiDialogPatches.BelowCopySet(ref statsText3Bounds, 0.0, textGap, 0.0, 0.0), "textFeetStats3");
			compo.EndChildElements();
			GuiElementClipHelpler.EndClip(compo);
			GuiComposer compo2 = compo;
			Action<float> action;
			if ((action = GuiDialogPatches.<>O.<1>__OnNewScrollbarValue) == null)
			{
				action = (GuiDialogPatches.<>O.<1>__OnNewScrollbarValue = new Action<float>(GuiDialogPatches.OnNewScrollbarValue));
			}
			GuiComposerHelpers.AddVerticalScrollbar(compo2, action, scrollbarBounds, "scrollbar");
			compo.EndChildElements();
			GuiComposerHelpers.GetSlider(compo, "tierSlider").SetValues(GuiDialogPatches._currentAttackTier, 0, 12, 1, "");
			GuiElementDynamicText dynamicText = GuiElementDynamicTextHelper.GetDynamicText(compo, "textTier");
			if (dynamicText != null)
			{
				dynamicText.SetNewText(Lang.Get("combatoverhaul:armor-inventory-dialog-attack-tier", Array.Empty<object>()), false, false, false);
			}
			GuiElementDynamicText dynamicText2 = GuiElementDynamicTextHelper.GetDynamicText(compo, "textLeftLegend");
			if (dynamicText2 != null)
			{
				dynamicText2.SetNewText(Lang.Get("combatoverhaul:armor-inventory-dialog-attack-legend-left", Array.Empty<object>()), false, false, false);
			}
			GuiElementDynamicText dynamicText3 = GuiElementDynamicTextHelper.GetDynamicText(compo, "textMiddleLegend");
			if (dynamicText3 != null)
			{
				dynamicText3.SetNewText(Lang.Get("combatoverhaul:armor-inventory-dialog-attack-legend-middle", Array.Empty<object>()), false, false, false);
			}
			GuiElementDynamicText dynamicText4 = GuiElementDynamicTextHelper.GetDynamicText(compo, "textRightLegend");
			if (dynamicText4 != null)
			{
				dynamicText4.SetNewText(Lang.Get("combatoverhaul:armor-inventory-dialog-attack-legend-right", Array.Empty<object>()), false, false, false);
			}
			GuiElementDynamicText dynamicText5 = GuiElementDynamicTextHelper.GetDynamicText(compo, "textHead");
			if (dynamicText5 != null)
			{
				dynamicText5.SetNewText(Lang.Get("combatoverhaul:armor-inventory-dialog-head", Array.Empty<object>()), false, false, false);
			}
			GuiElementDynamicText dynamicText6 = GuiElementDynamicTextHelper.GetDynamicText(compo, "textFace");
			if (dynamicText6 != null)
			{
				dynamicText6.SetNewText(Lang.Get("combatoverhaul:armor-inventory-dialog-face", Array.Empty<object>()), false, false, false);
			}
			GuiElementDynamicText dynamicText7 = GuiElementDynamicTextHelper.GetDynamicText(compo, "textNeck");
			if (dynamicText7 != null)
			{
				dynamicText7.SetNewText(Lang.Get("combatoverhaul:armor-inventory-dialog-neck", Array.Empty<object>()), false, false, false);
			}
			GuiElementDynamicText dynamicText8 = GuiElementDynamicTextHelper.GetDynamicText(compo, "textTorso");
			if (dynamicText8 != null)
			{
				dynamicText8.SetNewText(Lang.Get("combatoverhaul:armor-inventory-dialog-torso", Array.Empty<object>()), false, false, false);
			}
			GuiElementDynamicText dynamicText9 = GuiElementDynamicTextHelper.GetDynamicText(compo, "textArms");
			if (dynamicText9 != null)
			{
				dynamicText9.SetNewText(Lang.Get("combatoverhaul:armor-inventory-dialog-arms", Array.Empty<object>()), false, false, false);
			}
			GuiElementDynamicText dynamicText10 = GuiElementDynamicTextHelper.GetDynamicText(compo, "textHands");
			if (dynamicText10 != null)
			{
				dynamicText10.SetNewText(Lang.Get("combatoverhaul:armor-inventory-dialog-hands", Array.Empty<object>()), false, false, false);
			}
			GuiElementDynamicText dynamicText11 = GuiElementDynamicTextHelper.GetDynamicText(compo, "textLegs");
			if (dynamicText11 != null)
			{
				dynamicText11.SetNewText(Lang.Get("combatoverhaul:armor-inventory-dialog-legs", Array.Empty<object>()), false, false, false);
			}
			GuiElementDynamicText dynamicText12 = GuiElementDynamicTextHelper.GetDynamicText(compo, "textFeet");
			if (dynamicText12 != null)
			{
				dynamicText12.SetNewText(Lang.Get("combatoverhaul:armor-inventory-dialog-feet", Array.Empty<object>()), false, false, false);
			}
			GuiElementDynamicText dynamicText13 = GuiElementDynamicTextHelper.GetDynamicText(compo, "textHeadStats1");
			if (dynamicText13 != null)
			{
				dynamicText13.SetNewText("0%", false, false, false);
			}
			GuiElementDynamicText dynamicText14 = GuiElementDynamicTextHelper.GetDynamicText(compo, "textFaceStats1");
			if (dynamicText14 != null)
			{
				dynamicText14.SetNewText("0%", false, false, false);
			}
			GuiElementDynamicText dynamicText15 = GuiElementDynamicTextHelper.GetDynamicText(compo, "textNeckStats1");
			if (dynamicText15 != null)
			{
				dynamicText15.SetNewText("0%", false, false, false);
			}
			GuiElementDynamicText dynamicText16 = GuiElementDynamicTextHelper.GetDynamicText(compo, "textTorsoStats1");
			if (dynamicText16 != null)
			{
				dynamicText16.SetNewText("0%", false, false, false);
			}
			GuiElementDynamicText dynamicText17 = GuiElementDynamicTextHelper.GetDynamicText(compo, "textArmsStats1");
			if (dynamicText17 != null)
			{
				dynamicText17.SetNewText("0%", false, false, false);
			}
			GuiElementDynamicText dynamicText18 = GuiElementDynamicTextHelper.GetDynamicText(compo, "textHandsStats1");
			if (dynamicText18 != null)
			{
				dynamicText18.SetNewText("0%", false, false, false);
			}
			GuiElementDynamicText dynamicText19 = GuiElementDynamicTextHelper.GetDynamicText(compo, "textLegsStats1");
			if (dynamicText19 != null)
			{
				dynamicText19.SetNewText("0%", false, false, false);
			}
			GuiElementDynamicText dynamicText20 = GuiElementDynamicTextHelper.GetDynamicText(compo, "textFeetStats1");
			if (dynamicText20 != null)
			{
				dynamicText20.SetNewText("0%", false, false, false);
			}
			GuiElementDynamicText dynamicText21 = GuiElementDynamicTextHelper.GetDynamicText(compo, "textHeadStats2");
			if (dynamicText21 != null)
			{
				dynamicText21.SetNewText("0%", false, false, false);
			}
			GuiElementDynamicText dynamicText22 = GuiElementDynamicTextHelper.GetDynamicText(compo, "textFaceStats2");
			if (dynamicText22 != null)
			{
				dynamicText22.SetNewText("0%", false, false, false);
			}
			GuiElementDynamicText dynamicText23 = GuiElementDynamicTextHelper.GetDynamicText(compo, "textNeckStats2");
			if (dynamicText23 != null)
			{
				dynamicText23.SetNewText("0%", false, false, false);
			}
			GuiElementDynamicText dynamicText24 = GuiElementDynamicTextHelper.GetDynamicText(compo, "textTorsoStats2");
			if (dynamicText24 != null)
			{
				dynamicText24.SetNewText("0%", false, false, false);
			}
			GuiElementDynamicText dynamicText25 = GuiElementDynamicTextHelper.GetDynamicText(compo, "textArmsStats2");
			if (dynamicText25 != null)
			{
				dynamicText25.SetNewText("0%", false, false, false);
			}
			GuiElementDynamicText dynamicText26 = GuiElementDynamicTextHelper.GetDynamicText(compo, "textHandsStats2");
			if (dynamicText26 != null)
			{
				dynamicText26.SetNewText("0%", false, false, false);
			}
			GuiElementDynamicText dynamicText27 = GuiElementDynamicTextHelper.GetDynamicText(compo, "textLegsStats2");
			if (dynamicText27 != null)
			{
				dynamicText27.SetNewText("0%", false, false, false);
			}
			GuiElementDynamicText dynamicText28 = GuiElementDynamicTextHelper.GetDynamicText(compo, "textFeetStats2");
			if (dynamicText28 != null)
			{
				dynamicText28.SetNewText("0%", false, false, false);
			}
			GuiElementDynamicText dynamicText29 = GuiElementDynamicTextHelper.GetDynamicText(compo, "textHeadStats3");
			if (dynamicText29 != null)
			{
				dynamicText29.SetNewText("0%", false, false, false);
			}
			GuiElementDynamicText dynamicText30 = GuiElementDynamicTextHelper.GetDynamicText(compo, "textFaceStats3");
			if (dynamicText30 != null)
			{
				dynamicText30.SetNewText("0%", false, false, false);
			}
			GuiElementDynamicText dynamicText31 = GuiElementDynamicTextHelper.GetDynamicText(compo, "textNeckStats3");
			if (dynamicText31 != null)
			{
				dynamicText31.SetNewText("0%", false, false, false);
			}
			GuiElementDynamicText dynamicText32 = GuiElementDynamicTextHelper.GetDynamicText(compo, "textTorsoStats3");
			if (dynamicText32 != null)
			{
				dynamicText32.SetNewText("0%", false, false, false);
			}
			GuiElementDynamicText dynamicText33 = GuiElementDynamicTextHelper.GetDynamicText(compo, "textArmsStats3");
			if (dynamicText33 != null)
			{
				dynamicText33.SetNewText("0%", false, false, false);
			}
			GuiElementDynamicText dynamicText34 = GuiElementDynamicTextHelper.GetDynamicText(compo, "textHandsStats3");
			if (dynamicText34 != null)
			{
				dynamicText34.SetNewText("0%", false, false, false);
			}
			GuiElementDynamicText dynamicText35 = GuiElementDynamicTextHelper.GetDynamicText(compo, "textLegsStats3");
			if (dynamicText35 != null)
			{
				dynamicText35.SetNewText("0%", false, false, false);
			}
			GuiElementDynamicText dynamicText36 = GuiElementDynamicTextHelper.GetDynamicText(compo, "textFeetStats3");
			if (dynamicText36 != null)
			{
				dynamicText36.SetNewText("0%", false, false, false);
			}
			GuiComposerHelpers.GetScrollbar(compo, "scrollbar").SetHeights((float)height, (float)(height + 150));
			GuiComposerHelpers.GetScrollbar(compo, "scrollbar").SetScrollbarPosition(0);
			GuiDialogPatches.SetStatsValues(compo, GuiDialogPatches.Api.World.Player, GuiDialogPatches._currentAttackTier);
			GuiDialogPatches._recalcualteArmorStats = delegate()
			{
				GuiDialogPatches.SetStatsValues(compo, GuiDialogPatches.Api.World.Player, GuiDialogPatches._currentAttackTier);
			};
			GuiDialogPatches._childBounds = childBounds;
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x00015E5C File Offset: 0x0001405C
		private static void AddSlotHere(GuiComposer compo, ArmorInventory inv, ArmorLayers layers, DamageZone zone, ref ElementBounds bounds, double gap)
		{
			int slotIndex = ArmorInventory.IndexFromArmorType(layers, zone);
			if (!inv.IsSlotAvailable(slotIndex))
			{
				bool flag = !inv[slotIndex].Empty;
			}
			Action<object> action;
			if ((action = GuiDialogPatches.<>O.<2>__SendInvPacket) == null)
			{
				action = (GuiDialogPatches.<>O.<2>__SendInvPacket = new Action<object>(GuiDialogPatches.SendInvPacket));
			}
			GuiComposerHelpers.AddItemSlotGrid(compo, inv, action, 1, new int[]
			{
				slotIndex
			}, bounds, null);
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x00015EC0 File Offset: 0x000140C0
		private static void AddSlot(GuiComposer compo, ArmorInventory inv, ArmorLayers layers, DamageZone zone, ref ElementBounds bounds, double gap)
		{
			int slotIndex = ArmorInventory.IndexFromArmorType(layers, zone);
			if (!inv.IsSlotAvailable(slotIndex))
			{
				bool flag = !inv[slotIndex].Empty;
			}
			Action<object> action;
			if ((action = GuiDialogPatches.<>O.<2>__SendInvPacket) == null)
			{
				action = (GuiDialogPatches.<>O.<2>__SendInvPacket = new Action<object>(GuiDialogPatches.SendInvPacket));
			}
			GuiComposerHelpers.AddItemSlotGrid(compo, inv, action, 1, new int[]
			{
				slotIndex
			}, GuiDialogPatches.BelowCopySet(ref bounds, 0.0, gap, 0.0, 0.0), null);
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x00015F44 File Offset: 0x00014144
		private static void SendInvPacket(object packet)
		{
			GuiDialogPatches.Api.Network.SendPacketClient(packet);
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x00015F58 File Offset: 0x00014158
		private static ElementBounds BelowCopySet(ref ElementBounds bounds, double fixedDeltaX = 0.0, double fixedDeltaY = 0.0, double fixedDeltaWidth = 0.0, double fixedDeltaHeight = 0.0)
		{
			ElementBounds result;
			bounds = (result = bounds.BelowCopy(fixedDeltaX, fixedDeltaY, fixedDeltaWidth, fixedDeltaHeight));
			return result;
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x00015F76 File Offset: 0x00014176
		private static void OnNewScrollbarValue(float value)
		{
			if (GuiDialogPatches._childBounds != null)
			{
				GuiDialogPatches._childBounds.fixedY = (double)(2f - value);
				GuiDialogPatches._childBounds.CalcWorldBounds();
			}
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x00015F9C File Offset: 0x0001419C
		private static void OnRenderGUI()
		{
			ICoreClientAPI api = GuiDialogPatches.Api;
			int? num;
			if (api == null)
			{
				num = default(int?);
			}
			else
			{
				ItemSlot mouseItemSlot = api.World.Player.InventoryManager.MouseItemSlot;
				if (mouseItemSlot == null)
				{
					num = default(int?);
				}
				else
				{
					ItemStack itemstack = mouseItemSlot.Itemstack;
					if (itemstack == null)
					{
						num = default(int?);
					}
					else
					{
						Item item = itemstack.Item;
						num = ((item != null) ? new int?(item.ItemId) : default(int?));
					}
				}
			}
			int? num2 = num;
			int currentItem = num2.GetValueOrDefault();
			if (GuiDialogPatches._anySlotsHighlighted && currentItem == 0 && GuiDialogPatches._lastItemId == 0)
			{
				ICoreClientAPI api2 = GuiDialogPatches.Api;
				InventoryCharacter inventory2 = GeneralUtils.GetCharacterInventory((api2 != null) ? api2.World.Player : null);
				if (inventory2 == null)
				{
					return;
				}
				foreach (ItemSlot slot in inventory2)
				{
					ClothesSlot clothesSlot = slot as ClothesSlot;
					if (clothesSlot != null)
					{
						clothesSlot.HexBackgroundColor = clothesSlot.PreviousColor;
					}
					else
					{
						slot.HexBackgroundColor = null;
					}
				}
				GuiDialogPatches._anySlotsHighlighted = false;
				return;
			}
			else
			{
				if (currentItem == GuiDialogPatches._lastItemId)
				{
					return;
				}
				GuiDialogPatches._lastItemId = currentItem;
				ICoreClientAPI api3 = GuiDialogPatches.Api;
				InventoryCharacter inventory3 = GeneralUtils.GetCharacterInventory((api3 != null) ? api3.World.Player : null);
				if (inventory3 == null)
				{
					return;
				}
				ItemSlot mouseSlot = GuiDialogPatches.Api.World.Player.InventoryManager.MouseItemSlot;
				GuiDialogPatches._anySlotsHighlighted = false;
				foreach (ItemSlot slot2 in inventory3)
				{
					if (slot2.CanHold(mouseSlot))
					{
						ClothesSlot clothesSlot2 = slot2 as ClothesSlot;
						if (clothesSlot2 != null && clothesSlot2.HexBackgroundColor != "#5fbed4")
						{
							clothesSlot2.PreviousColor = clothesSlot2.HexBackgroundColor;
						}
						slot2.HexBackgroundColor = "#5fbed4";
						GuiDialogPatches._anySlotsHighlighted = true;
					}
					else if (slot2.HexBackgroundColor == "#5fbed4")
					{
						ClothesSlot clothesSlot3 = slot2 as ClothesSlot;
						if (clothesSlot3 != null)
						{
							clothesSlot3.HexBackgroundColor = clothesSlot3.PreviousColor;
						}
						else
						{
							slot2.HexBackgroundColor = null;
						}
					}
				}
				return;
			}
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x000161CC File Offset: 0x000143CC
		private unsafe static void ComposeBackpackSlots(GuiComposer composer, GuiDialogInventory __instance, [Nullable(2)] IInventory backPackInv, ElementBounds fullGridBounds)
		{
			if (backPackInv == null)
			{
				return;
			}
			GuiDialogPatches._rows.Clear();
			List<int> generalSlots = new List<int>();
			List<ValueTuple<float, string, List<int>>> specialSlots = new List<ValueTuple<float, string, List<int>>>();
			for (int slotIndex = 4; slotIndex < backPackInv.Count; slotIndex++)
			{
				ItemSlot slot = backPackInv[slotIndex];
				if (slot != null)
				{
					string categoryCode;
					float orderPriority;
					if (GuiDialogPatches.TryGetBackpackCategory(slot, out categoryCode, out orderPriority) && categoryCode != "")
					{
						bool categoryExists = false;
						foreach (ValueTuple<float, string, List<int>> valueTuple in specialSlots)
						{
							string category = valueTuple.Item2;
							List<int> indexes = valueTuple.Item3;
							if (category == categoryCode)
							{
								indexes.Add(slotIndex);
								categoryExists = true;
								break;
							}
						}
						if (!categoryExists)
						{
							List<ValueTuple<float, string, List<int>>> list = specialSlots;
							float num = orderPriority;
							string text = categoryCode;
							int num2 = 1;
							List<int> list2 = new List<int>(num2);
							CollectionsMarshal.SetCount<int>(list2, num2);
							*CollectionsMarshal.AsSpan<int>(list2)[0] = slotIndex;
							list.Add(new ValueTuple<float, string, List<int>>(num, text, list2));
						}
					}
					else
					{
						generalSlots.Add(slotIndex);
					}
				}
			}
			specialSlots.Sort(([TupleElementNames(new string[]
			{
				"order",
				"category",
				"indexes"
			})] [Nullable(new byte[]
			{
				0,
				1,
				1
			})] ValueTuple<float, string, List<int>> a, [TupleElementNames(new string[]
			{
				"order",
				"category",
				"indexes"
			})] [Nullable(new byte[]
			{
				0,
				1,
				1
			})] ValueTuple<float, string, List<int>> b) => Math.Sign(b.Item1 - a.Item1));
			GuiComposerHelpers.AddItemSlotGridExcl(composer, backPackInv, delegate(object data)
			{
				GuiDialogPatches.SendInvPacket(__instance, data);
			}, 6, GuiDialogPatches.GetInvertedIndexes(generalSlots, backPackInv.Count), fullGridBounds, "slotgrid");
			GuiDialogPatches._rows.Add((int)Math.Ceiling((double)((float)generalSlots.Count / 6f)));
			ElementBounds specialGridBounds = fullGridBounds.FlatCopy();
			Action<object> <>9__2;
			for (int categoryIndex = 0; categoryIndex < specialSlots.Count; categoryIndex++)
			{
				GuiDialogPatches._rows.Add((int)Math.Ceiling((double)((float)specialSlots[categoryIndex].Item3.Count / 6f)));
				double Y = specialGridBounds.fixedY + (GuiElementPassiveItemSlot.unscaledSlotSize + GuiElementItemSlotGridBase.unscaledSlotPadding) * (double)GuiDialogPatches._rows[categoryIndex];
				specialGridBounds = fullGridBounds.FlatCopy();
				specialGridBounds.fixedY = Y;
				string text2 = Lang.Get("slotcategory-" + specialSlots[categoryIndex].Item2, Array.Empty<object>());
				CairoFont cairoFont = CairoFont.WhiteSmallText().WithFontSize(14f);
				ElementBounds elementBounds = specialGridBounds;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(9, 1);
				defaultInterpolatedStringHandler.AppendLiteral("category-");
				defaultInterpolatedStringHandler.AppendFormatted<int>(categoryIndex);
				GuiComposerHelpers.AddRichtext(composer, text2, cairoFont, elementBounds, defaultInterpolatedStringHandler.ToStringAndClear());
				specialGridBounds = fullGridBounds.FlatCopy();
				specialGridBounds.fixedY = Y + (GuiElementPassiveItemSlot.unscaledSlotSize + GuiElementItemSlotGridBase.unscaledSlotPadding) * 0.4;
				Action<object> action;
				if ((action = <>9__2) == null)
				{
					action = (<>9__2 = delegate(object data)
					{
						GuiDialogPatches.SendInvPacket(__instance, data);
					});
				}
				int num3 = 6;
				int[] invertedIndexes = GuiDialogPatches.GetInvertedIndexes(specialSlots[categoryIndex].Item3, backPackInv.Count);
				ElementBounds elementBounds2 = specialGridBounds;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
				defaultInterpolatedStringHandler2..ctor(9, 1);
				defaultInterpolatedStringHandler2.AppendLiteral("slotgrid-");
				defaultInterpolatedStringHandler2.AppendFormatted<int>(categoryIndex);
				GuiComposerHelpers.AddItemSlotGridExcl(composer, backPackInv, action, num3, invertedIndexes, elementBounds2, defaultInterpolatedStringHandler2.ToStringAndClear());
			}
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x000164C8 File Offset: 0x000146C8
		private static bool TryGetBackpackCategory(ItemSlot slot, out string categoryCode, out float orderPriority)
		{
			categoryCode = "";
			orderPriority = 1f;
			IHasSlotBackpackCategory typed = slot as IHasSlotBackpackCategory;
			if (typed != null)
			{
				categoryCode = (typed.BackpackCategoryCode ?? "");
				orderPriority = typed.OrderPriority;
				return categoryCode != "";
			}
			object anyMemberValue = GuiDialogPatches.GetAnyMemberValue(slot, "BackpackCategoryCode");
			object directOrderValue = GuiDialogPatches.GetAnyMemberValue(slot, "OrderPriority");
			string directCategory = GuiDialogPatches.AsNonEmptyString(anyMemberValue);
			if (directCategory != null)
			{
				categoryCode = directCategory;
				orderPriority = GuiDialogPatches.ParseOrderPriority(directOrderValue, 1f);
				return true;
			}
			object config = GuiDialogPatches.GetAnyMemberValue(slot, "Config");
			if (config == null)
			{
				return GuiDialogPatches.TryInferBackpackCategory(slot, out categoryCode, out orderPriority);
			}
			object value = GuiDialogPatches.GetAnyMemberValue(config, "BackpackCategoryCode") ?? GuiDialogPatches.GetAnyMemberValue(config, "SlotBackpackCategory");
			object orderValue = GuiDialogPatches.GetAnyMemberValue(config, "OrderPriority") ?? GuiDialogPatches.GetAnyMemberValue(config, "CategoryOrderPriority");
			string category = GuiDialogPatches.AsNonEmptyString(value);
			if (category != null)
			{
				categoryCode = category;
				orderPriority = GuiDialogPatches.ParseOrderPriority(orderValue, 1f);
				return true;
			}
			return GuiDialogPatches.TryInferBackpackCategory(slot, out categoryCode, out orderPriority);
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x000165BC File Offset: 0x000147BC
		[return: Nullable(2)]
		private static IInventory ResolveInventoryField(object instance, [Nullable(2)] FieldInfo primaryField, params string[] fallbackFieldNames)
		{
			if (primaryField != null)
			{
				IInventory invFromPrimary = primaryField.GetValue(instance) as IInventory;
				if (invFromPrimary != null)
				{
					return invFromPrimary;
				}
			}
			Type type = instance.GetType();
			foreach (string fieldName in fallbackFieldNames)
			{
				FieldInfo fallback = type.GetField(fieldName, 52);
				if (!(fallback == null))
				{
					IInventory inv = fallback.GetValue(instance) as IInventory;
					if (inv != null)
					{
						return inv;
					}
				}
			}
			foreach (FieldInfo field in type.GetFields(52))
			{
				if (typeof(IInventory).IsAssignableFrom(field.FieldType))
				{
					IInventory inv2 = field.GetValue(instance) as IInventory;
					if (inv2 != null)
					{
						return inv2;
					}
				}
			}
			return null;
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x0001667C File Offset: 0x0001487C
		private static bool TryInferBackpackCategory(ItemSlot slot, out string categoryCode, out float orderPriority)
		{
			categoryCode = "";
			orderPriority = 1f;
			if (slot.GetType().Name.ToLowerInvariant().Contains("takeout"))
			{
				categoryCode = "takeout";
				orderPriority = 0.1f;
				return true;
			}
			object anyMemberValue = GuiDialogPatches.GetAnyMemberValue(slot, "BackgroundIcon");
			string iconCode = (((anyMemberValue != null) ? anyMemberValue.ToString() : null) ?? "").ToLowerInvariant();
			if (iconCode == "")
			{
				return false;
			}
			if (iconCode.Contains("arrow") || iconCode.Contains("bolt") || iconCode.Contains("bullet") || iconCode.Contains("sling"))
			{
				categoryCode = "ammunition";
				orderPriority = 0.8f;
				return true;
			}
			if (iconCode.Contains("sword") || iconCode.Contains("bow") || iconCode.Contains("pistol") || iconCode.Contains("weapon"))
			{
				categoryCode = "weapons";
				orderPriority = 0.9f;
				return true;
			}
			if (iconCode.Contains("spanningtool") || iconCode.Contains("tool") || iconCode.Contains("knife"))
			{
				categoryCode = "tools";
				orderPriority = 0.7f;
				return true;
			}
			if (iconCode.Contains("gear") || iconCode.Contains("bag") || iconCode.Contains("flute"))
			{
				categoryCode = "miscellaneous";
				orderPriority = 0.6f;
				return true;
			}
			return false;
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x000167F0 File Offset: 0x000149F0
		[return: Nullable(2)]
		private static object GetAnyMemberValue(object instance, string memberName)
		{
			Type type = instance.GetType();
			PropertyInfo property = type.GetProperty(memberName, 116);
			if (property != null)
			{
				try
				{
					return property.GetValue(instance);
				}
				catch (Exception exception)
				{
					GuiDialogPatches.LogMemberAccessError(type, memberName, exception);
				}
			}
			FieldInfo field = type.GetField(memberName, 116);
			if (field != null)
			{
				try
				{
					return field.GetValue(instance);
				}
				catch (Exception exception2)
				{
					GuiDialogPatches.LogMemberAccessError(type, memberName, exception2);
				}
			}
			foreach (PropertyInfo candidate in type.GetProperties(116))
			{
				if (string.Equals(candidate.Name, memberName, 5))
				{
					try
					{
						return candidate.GetValue(instance);
					}
					catch (Exception exception3)
					{
						GuiDialogPatches.LogMemberAccessError(type, memberName, exception3);
					}
				}
			}
			foreach (FieldInfo candidate2 in type.GetFields(116))
			{
				if (string.Equals(candidate2.Name, memberName, 5) || string.Equals(candidate2.Name, "<" + memberName + ">k__BackingField", 5) || candidate2.Name.IndexOf(memberName, 5) >= 0)
				{
					try
					{
						return candidate2.GetValue(instance);
					}
					catch (Exception exception4)
					{
						GuiDialogPatches.LogMemberAccessError(type, memberName, exception4);
					}
				}
			}
			return null;
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x00016964 File Offset: 0x00014B64
		private static void LogInventoryLayoutError(Exception exception)
		{
			if (GuiDialogPatches._reportedInventoryLayoutError)
			{
				return;
			}
			GuiDialogPatches._reportedInventoryLayoutError = true;
			ICoreAPI api = GuiDialogPatches.Api;
			Type typeFromHandle = typeof(GuiDialogPatches);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(51, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Error while repositioning survival inventory rows:\n");
			defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
			LoggerUtil.Warn(api, typeFromHandle, defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x000169B8 File Offset: 0x00014BB8
		private static void LogMemberAccessError(Type type, string memberName, Exception exception)
		{
			if (GuiDialogPatches._reportedMemberAccessError)
			{
				return;
			}
			GuiDialogPatches._reportedMemberAccessError = true;
			ICoreAPI api = GuiDialogPatches.Api;
			Type typeFromHandle = typeof(GuiDialogPatches);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(39, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Error while reading member '");
			defaultInterpolatedStringHandler.AppendFormatted(memberName);
			defaultInterpolatedStringHandler.AppendLiteral("' from '");
			defaultInterpolatedStringHandler.AppendFormatted(type.FullName);
			defaultInterpolatedStringHandler.AppendLiteral("':\n");
			defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
			LoggerUtil.Warn(api, typeFromHandle, defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x00016A3C File Offset: 0x00014C3C
		[NullableContext(2)]
		private static string AsNonEmptyString(object value)
		{
			if (value == null)
			{
				return null;
			}
			string text = value.ToString() ?? "";
			if (!(text != ""))
			{
				return null;
			}
			return text;
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x00016A70 File Offset: 0x00014C70
		[NullableContext(2)]
		private static float ParseOrderPriority(object value, float fallback)
		{
			if (value is float)
			{
				return (float)value;
			}
			if (value is double)
			{
				double d = (double)value;
				return (float)d;
			}
			if (value is int)
			{
				int i = (int)value;
				return (float)i;
			}
			string text = ((value != null) ? value.ToString() : null) ?? "";
			float parsed;
			if (float.TryParse(text, 167, CultureInfo.InvariantCulture, ref parsed))
			{
				return parsed;
			}
			if (float.TryParse(text, ref parsed))
			{
				return parsed;
			}
			return fallback;
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x00016AF0 File Offset: 0x00014CF0
		private static int[] GetInvertedIndexes(List<int> indexes, int total)
		{
			List<int> result = new List<int>();
			for (int index = 0; index < total; index++)
			{
				if (!indexes.Contains(index))
				{
					result.Add(index);
				}
			}
			return result.ToArray();
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x00016B28 File Offset: 0x00014D28
		private static string GetCalcTypeText()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(40, 1);
			defaultInterpolatedStringHandler.AppendLiteral("combatoverhaul:armor-inventory-calctype-");
			defaultInterpolatedStringHandler.AppendFormatted<DamageReceivedCalculationType>(GuiDialogPatches._calculationType);
			return Lang.Get(defaultInterpolatedStringHandler.ToStringAndClear(), Array.Empty<object>());
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x00016B68 File Offset: 0x00014D68
		private static string GetCalcTypeHoverText()
		{
			StringBuilder builder = new StringBuilder();
			foreach (DamageReceivedCalculationType value2 in Enumerable.Where<DamageReceivedCalculationType>(Enum.GetValues<DamageReceivedCalculationType>(), (DamageReceivedCalculationType value) => value != DamageReceivedCalculationType.TypesNumber))
			{
				StringBuilder stringBuilder = builder;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(45, 1);
				defaultInterpolatedStringHandler.AppendLiteral("combatoverhaul:armor-inventory-calctype-hint-");
				defaultInterpolatedStringHandler.AppendFormatted<DamageReceivedCalculationType>(value2);
				stringBuilder.AppendLine(Lang.Get(defaultInterpolatedStringHandler.ToStringAndClear(), Array.Empty<object>()));
				builder.AppendLine();
			}
			return builder.ToString();
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x00016C1C File Offset: 0x00014E1C
		private static void SetStatsValues(GuiComposer composer, IPlayer player, int damageTier)
		{
			bool invert = GuiDialogPatches._calculationType == DamageReceivedCalculationType.OnlyArmor;
			foreach (KeyValuePair<DamageZone, string> keyValuePair in GuiDialogPatches._zonesStatsTextIds)
			{
				DamageZone damageZone;
				string text;
				keyValuePair.Deconstruct(ref damageZone, ref text);
				DamageZone zone = damageZone;
				string textId = text;
				float piercing = PlayerDamageModelBehavior.GetDamageReductionFactor(player, zone, 4, damageTier, GuiDialogPatches._calculationType);
				float blunt = PlayerDamageModelBehavior.GetDamageReductionFactor(player, zone, 2, damageTier, GuiDialogPatches._calculationType);
				float slash = PlayerDamageModelBehavior.GetDamageReductionFactor(player, zone, 3, damageTier, GuiDialogPatches._calculationType);
				if (invert)
				{
					piercing = 1f - piercing;
					blunt = 1f - blunt;
					slash = 1f - slash;
				}
				switch (GuiDialogPatches._calculationType)
				{
				case DamageReceivedCalculationType.OnlyArmor:
				case DamageReceivedCalculationType.WithBodyParts:
				{
					GuiElementDynamicText dynamicText = GuiElementDynamicTextHelper.GetDynamicText(composer, textId + "1");
					if (dynamicText != null)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
						defaultInterpolatedStringHandler.AppendFormatted<float>(piercing, "P0");
						dynamicText.SetNewText(defaultInterpolatedStringHandler.ToStringAndClear(), false, false, false);
					}
					GuiElementDynamicText dynamicText2 = GuiElementDynamicTextHelper.GetDynamicText(composer, textId + "2");
					if (dynamicText2 != null)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(0, 1);
						defaultInterpolatedStringHandler2.AppendFormatted<float>(slash, "P0");
						dynamicText2.SetNewText(defaultInterpolatedStringHandler2.ToStringAndClear(), false, false, false);
					}
					GuiElementDynamicText dynamicText3 = GuiElementDynamicTextHelper.GetDynamicText(composer, textId + "3");
					if (dynamicText3 != null)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(0, 1);
						defaultInterpolatedStringHandler3.AppendFormatted<float>(blunt, "P0");
						dynamicText3.SetNewText(defaultInterpolatedStringHandler3.ToStringAndClear(), false, false, false);
					}
					break;
				}
				case DamageReceivedCalculationType.HitChance:
				{
					GuiElementDynamicText dynamicText4 = GuiElementDynamicTextHelper.GetDynamicText(composer, textId + "1");
					if (dynamicText4 != null)
					{
						dynamicText4.SetNewText("", false, false, false);
					}
					GuiElementDynamicText dynamicText5 = GuiElementDynamicTextHelper.GetDynamicText(composer, textId + "2");
					if (dynamicText5 != null)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(0, 1);
						defaultInterpolatedStringHandler4.AppendFormatted<float>(slash, "P1");
						dynamicText5.SetNewText(defaultInterpolatedStringHandler4.ToStringAndClear(), false, false, false);
					}
					GuiElementDynamicText dynamicText6 = GuiElementDynamicTextHelper.GetDynamicText(composer, textId + "3");
					if (dynamicText6 != null)
					{
						dynamicText6.SetNewText("", false, false, false);
					}
					break;
				}
				case DamageReceivedCalculationType.Average:
				{
					GuiElementDynamicText dynamicText7 = GuiElementDynamicTextHelper.GetDynamicText(composer, textId + "1");
					if (dynamicText7 != null)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler5 = new DefaultInterpolatedStringHandler(0, 1);
						defaultInterpolatedStringHandler5.AppendFormatted<float>(piercing, "P1");
						dynamicText7.SetNewText(defaultInterpolatedStringHandler5.ToStringAndClear(), false, false, false);
					}
					GuiElementDynamicText dynamicText8 = GuiElementDynamicTextHelper.GetDynamicText(composer, textId + "2");
					if (dynamicText8 != null)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler6 = new DefaultInterpolatedStringHandler(0, 1);
						defaultInterpolatedStringHandler6.AppendFormatted<float>(slash, "P1");
						dynamicText8.SetNewText(defaultInterpolatedStringHandler6.ToStringAndClear(), false, false, false);
					}
					GuiElementDynamicText dynamicText9 = GuiElementDynamicTextHelper.GetDynamicText(composer, textId + "3");
					if (dynamicText9 != null)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler7 = new DefaultInterpolatedStringHandler(0, 1);
						defaultInterpolatedStringHandler7.AppendFormatted<float>(blunt, "P1");
						dynamicText9.SetNewText(defaultInterpolatedStringHandler7.ToStringAndClear(), false, false, false);
					}
					break;
				}
				}
			}
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x00016F0C File Offset: 0x0001510C
		private static bool ToggleBodyPartsMultipliers(GuiComposer composer)
		{
			GuiDialogPatches._calculationType = (GuiDialogPatches._calculationType + 1) % DamageReceivedCalculationType.TypesNumber;
			GuiElementDynamicText dynamicText = GuiElementDynamicTextHelper.GetDynamicText(composer, "textButton");
			if (dynamicText != null)
			{
				dynamicText.SetNewText(GuiDialogPatches.GetCalcTypeText(), false, false, false);
			}
			GuiDialogPatches.SetStatsValues(composer, GuiDialogPatches.Api.World.Player, GuiDialogPatches._currentAttackTier);
			return false;
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x00016F60 File Offset: 0x00015160
		private static void CloseIconPressed(GuiDialogInventory __instance)
		{
			MethodInfo guiDialogInventory_CloseIconPressed = GuiDialogPatches.GuiDialogInventory_CloseIconPressed;
			if (guiDialogInventory_CloseIconPressed == null)
			{
				return;
			}
			guiDialogInventory_CloseIconPressed.Invoke(__instance, Array.Empty<object>());
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x00016F78 File Offset: 0x00015178
		private static void OnNewScrollbarvalue(GuiDialogInventory __instance, float data)
		{
			MethodInfo guiDialogInventory_OnNewScrollbarvalue = GuiDialogPatches.GuiDialogInventory_OnNewScrollbarvalue;
			if (guiDialogInventory_OnNewScrollbarvalue == null)
			{
				return;
			}
			guiDialogInventory_OnNewScrollbarvalue.Invoke(__instance, new object[]
			{
				data
			});
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x00016F9A File Offset: 0x0001519A
		private static void SendInvPacket(GuiDialogInventory __instance, object data)
		{
			MethodInfo guiDialogInventory_SendInvPacket = GuiDialogPatches.GuiDialogInventory_SendInvPacket;
			if (guiDialogInventory_SendInvPacket == null)
			{
				return;
			}
			guiDialogInventory_SendInvPacket.Invoke(__instance, new object[]
			{
				data
			});
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x00016FB8 File Offset: 0x000151B8
		// Note: this type is marked as 'beforefieldinit'.
		static GuiDialogPatches()
		{
			Dictionary<DamageZone, string> dictionary = new Dictionary<DamageZone, string>();
			dictionary.Add(DamageZone.Head, "textHeadStats");
			dictionary.Add(DamageZone.Face, "textFaceStats");
			dictionary.Add(DamageZone.Neck, "textNeckStats");
			dictionary.Add(DamageZone.Torso, "textTorsoStats");
			dictionary.Add(DamageZone.Arms, "textArmsStats");
			dictionary.Add(DamageZone.Hands, "textHandsStats");
			dictionary.Add(DamageZone.Legs, "textLegsStats");
			dictionary.Add(DamageZone.Feet, "textFeetStats");
			GuiDialogPatches._zonesStatsTextIds = dictionary;
			GuiDialogPatches._calculationType = DamageReceivedCalculationType.OnlyArmor;
			GuiDialogPatches.GuiDialogInventory_survivalInvDialog = typeof(GuiDialogInventory).GetField("survivalInvDialog", 36);
			GuiDialogPatches.GuiDialogInventory_prevRows = typeof(GuiDialogInventory).GetField("prevRows", 36);
			GuiDialogPatches.GuiDialogInventory_backPackInv = typeof(GuiDialogInventory).GetField("backPackInv", 36);
			GuiDialogPatches.GuiDialogInventory_craftingInv = typeof(GuiDialogInventory).GetField("craftingInv", 36);
			GuiDialogPatches.GuiDialogInventory_CloseIconPressed = typeof(GuiDialogInventory).GetMethod("CloseIconPressed", 36);
			GuiDialogPatches.GuiDialogInventory_SendInvPacket = typeof(GuiDialogInventory).GetMethod("SendInvPacket", 36);
			GuiDialogPatches.GuiDialogInventory_OnNewScrollbarvalue = typeof(GuiDialogInventory).GetMethod("OnNewScrollbarvalue", 36);
			GuiDialogPatches.CharacterSystem_charDlg = typeof(CharacterSystem).GetField("charDlg", 36);
		}

		// Token: 0x04000172 RID: 370
		private static int _lastItemId = 0;

		// Token: 0x04000173 RID: 371
		private static bool _anySlotsHighlighted = false;

		// Token: 0x04000174 RID: 372
		private static List<int> _rows = new List<int>();

		// Token: 0x04000175 RID: 373
		private static int _totalScrollableRows = 0;

		// Token: 0x04000176 RID: 374
		private static bool _reportedInventoryLayoutError;

		// Token: 0x04000177 RID: 375
		private static bool _reportedMemberAccessError;

		// Token: 0x04000178 RID: 376
		[Nullable(2)]
		private static ElementBounds _childBounds;

		// Token: 0x04000179 RID: 377
		private static int _currentAttackTier = 1;

		// Token: 0x0400017A RID: 378
		private static readonly Dictionary<DamageZone, string> _zonesStatsTextIds;

		// Token: 0x0400017B RID: 379
		[Nullable(2)]
		private static Action _recalcualteArmorStats;

		// Token: 0x0400017C RID: 380
		private static DamageReceivedCalculationType _calculationType;

		// Token: 0x0400017D RID: 381
		[Nullable(2)]
		private static FieldInfo GuiDialogInventory_survivalInvDialog;

		// Token: 0x0400017E RID: 382
		[Nullable(2)]
		private static FieldInfo GuiDialogInventory_prevRows;

		// Token: 0x0400017F RID: 383
		[Nullable(2)]
		private static FieldInfo GuiDialogInventory_backPackInv;

		// Token: 0x04000180 RID: 384
		[Nullable(2)]
		private static FieldInfo GuiDialogInventory_craftingInv;

		// Token: 0x04000181 RID: 385
		[Nullable(2)]
		private static MethodInfo GuiDialogInventory_CloseIconPressed;

		// Token: 0x04000182 RID: 386
		[Nullable(2)]
		private static MethodInfo GuiDialogInventory_SendInvPacket;

		// Token: 0x04000183 RID: 387
		[Nullable(2)]
		private static MethodInfo GuiDialogInventory_OnNewScrollbarvalue;

		// Token: 0x04000184 RID: 388
		[Nullable(2)]
		private static FieldInfo CharacterSystem_charDlg;

		// Token: 0x020001D2 RID: 466
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000AE0 RID: 2784
			[Nullable(0)]
			public static Action<GuiComposer> <0>__composeArmorTab;

			// Token: 0x04000AE1 RID: 2785
			[Nullable(0)]
			public static Action<float> <1>__OnNewScrollbarValue;

			// Token: 0x04000AE2 RID: 2786
			[Nullable(0)]
			public static Action<object> <2>__SendInvPacket;
		}
	}
}
