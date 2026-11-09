using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using CombatOverhaul.Armor;
using CombatOverhaul.Utils;
using CombatOverhaul.Vanity;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;
using Vintagestory.Client.NoObf;

namespace CombatOverhaul.Integration
{
	// Token: 0x0200003C RID: 60
	[NullableContext(1)]
	[Nullable(0)]
	public static class CharacterTabPatch
	{
		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060003DD RID: 989 RVA: 0x000120F6 File Offset: 0x000102F6
		// (set) Token: 0x060003DE RID: 990 RVA: 0x000120FD File Offset: 0x000102FD
		public static CharacterTabPatch.CharacterSlotsStatus SlotsStatus { get; set; } = new CharacterTabPatch.CharacterSlotsStatus();

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060003DF RID: 991 RVA: 0x00012105 File Offset: 0x00010305
		[Nullable(2)]
		public static ICoreClientAPI Api
		{
			[NullableContext(2)]
			get
			{
				return GuiDialogPatches.Api;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060003E0 RID: 992 RVA: 0x0001210C File Offset: 0x0001030C
		// (set) Token: 0x060003E1 RID: 993 RVA: 0x00012113 File Offset: 0x00010313
		public static bool VanityTabEnabled { get; set; } = false;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x060003E2 RID: 994 RVA: 0x0001211C File Offset: 0x0001031C
		// (remove) Token: 0x060003E3 RID: 995 RVA: 0x00012150 File Offset: 0x00010350
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public static event Action OnSaveAndApply;

		// Token: 0x060003E4 RID: 996 RVA: 0x00012184 File Offset: 0x00010384
		public static bool GuiDialogCharacter_ComposeCharacterTab(GuiDialogCharacter __instance, GuiComposer compo)
		{
			ICoreClientAPI api = CharacterTabPatch.Api;
			if (api != null && !api.Gui.Icons.CustomIcons.ContainsKey("armorhead"))
			{
				CharacterTabPatch.RegisterArmorIcons(CharacterTabPatch.Api);
			}
			CharacterTabPatch.CharacterTabType currentTabType = CharacterTabPatch._currentTabType;
			if (currentTabType != CharacterTabPatch.CharacterTabType.Default)
			{
				if (currentTabType == CharacterTabPatch.CharacterTabType.Vanity)
				{
					CharacterTabPatch.ComposeVanityTab(__instance, compo);
				}
			}
			else
			{
				CharacterTabPatch.ComposeCharacterTab(__instance, compo);
			}
			return false;
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x000121E8 File Offset: 0x000103E8
		private static void ComposeCharacterTab(GuiDialogCharacter dialog, GuiComposer composer)
		{
			double pad = GuiElementItemSlotGridBase.unscaledSlotPadding;
			ElementBounds vanityButtonBounds = ElementBounds.Fixed(260.0, -16.0, 120.0, 24.0);
			ElementBounds outerInsetBounds = ElementBounds.Fixed(0.0, 22.0, 414.0, 356.0);
			ElementBounds leftSlotBounds = ElementStdBounds.SlotGrid(0, 0.0, 20.0 + pad, 1, 6).FixedGrow(0.0, pad);
			ElementBounds leftArmorSlotBoundsHead = ElementStdBounds.SlotGrid(0, 0.0, 23.0, 1, 1).FixedGrow(0.0, pad);
			ElementBounds leftArmorSlotBoundsBody = ElementStdBounds.SlotGrid(0, 0.0, 74.0, 1, 1).FixedGrow(0.0, pad);
			ElementBounds leftArmorSlotBoundsLegs = ElementStdBounds.SlotGrid(0, 0.0, 125.0, 1, 1).FixedGrow(0.0, pad);
			ElementBounds leftMiscSlotBounds = ElementStdBounds.SlotGrid(0, 0.0, 20.0 + pad + 153.0, 1, 1).FixedGrow(0.0, pad);
			ElementBounds leftMiscSlotBounds2 = ElementStdBounds.SlotGrid(0, 0.0, 20.0 + pad + 204.0, 1, 1).FixedGrow(0.0, pad);
			ElementBounds leftMiscSlotBounds3 = ElementStdBounds.SlotGrid(0, 0.0, 20.0 + pad + 255.0, 1, 1).FixedGrow(0.0, pad);
			leftSlotBounds.FixedRightOf(leftArmorSlotBoundsLegs, 2.0);
			ElementBounds insetSlotBounds = ElementBounds.Fixed(0.0, 22.0 + pad, 190.0, leftSlotBounds.fixedHeight - 2.0 * pad - 4.0);
			FieldInfo guiDialogCharacter_insetSlotBounds = CharacterTabPatch.GuiDialogCharacter_insetSlotBounds;
			if (guiDialogCharacter_insetSlotBounds != null)
			{
				guiDialogCharacter_insetSlotBounds.SetValue(dialog, insetSlotBounds);
			}
			insetSlotBounds.FixedRightOf(leftSlotBounds, 10.0);
			ElementBounds rightSlotBounds = ElementStdBounds.SlotGrid(0, 0.0, 20.0 + pad, 1, 6).FixedGrow(0.0, pad).FixedRightOf(insetSlotBounds, 10.0);
			ElementBounds rightGearSlotBounds = ElementStdBounds.SlotGrid(0, 0.0, 20.0 + pad, 1, 1).FixedGrow(0.0, pad).FixedRightOf(rightSlotBounds, 0.0);
			ElementBounds additionalSlots1Bounds = ElementStdBounds.SlotGrid(0, 2.0, 20.0 + pad + 306.0, 1, 1).FixedGrow(0.0, pad);
			ElementBounds additionalSlots2Bounds = ElementStdBounds.SlotGrid(0, 2.0, 20.0 + pad + 306.0, 1, 1).FixedGrow(0.0, pad).FixedRightOf(additionalSlots1Bounds, 161.0);
			leftSlotBounds.fixedHeight -= 6.0;
			rightSlotBounds.fixedHeight -= 6.0;
			rightGearSlotBounds.fixedHeight -= 6.0;
			CairoFont hoverTextFont = CairoFont.WhiteSmallText();
			CairoFont vanityButtonFont = CairoFont.WhiteSmallText().WithOrientation(2);
			FieldInfo guiDialogCharacter_characterInv = CharacterTabPatch.GuiDialogCharacter_characterInv;
			IInventory characterInv = (IInventory)((guiDialogCharacter_characterInv != null) ? guiDialogCharacter_characterInv.GetValue(dialog) : null);
			Action<object> SendInvPacket = delegate(object parameter)
			{
				MethodInfo guiDialogCharacter_SendInvPacket = CharacterTabPatch.GuiDialogCharacter_SendInvPacket;
				if (guiDialogCharacter_SendInvPacket == null)
				{
					return;
				}
				guiDialogCharacter_SendInvPacket.Invoke(dialog, new object[]
				{
					parameter
				});
			};
			GuiComposerHelpers.AddButton(GuiComposerHelpers.AddAutoSizeHoverText(GuiComposerHelpers.AddItemSlotGrid(GuiComposerHelpers.AddAutoSizeHoverText(GuiComposerHelpers.AddItemSlotGrid(GuiComposerHelpers.AddAutoSizeHoverText(GuiComposerHelpers.AddItemSlotGrid(GuiComposerHelpers.AddAutoSizeHoverText(GuiComposerHelpers.AddItemSlotGrid(GuiComposerHelpers.AddAutoSizeHoverText(GuiComposerHelpers.AddItemSlotGrid(GuiComposerHelpers.AddAutoSizeHoverText(GuiComposerHelpers.AddItemSlotGrid(GuiComposerHelpers.AddAutoSizeHoverText(GuiComposerHelpers.AddItemSlotGrid(GuiComposerHelpers.AddAutoSizeHoverText(GuiComposerHelpers.AddItemSlotGrid(GuiComposerHelpers.AddAutoSizeHoverText(GuiComposerHelpers.AddItemSlotGrid(GuiComposerHelpers.AddAutoSizeHoverText(GuiComposerHelpers.AddItemSlotGrid(GuiComposerHelpers.AddAutoSizeHoverText(GuiComposerHelpers.AddItemSlotGrid(GuiComposerHelpers.AddAutoSizeHoverText(GuiComposerHelpers.AddItemSlotGrid(GuiComposerHelpers.AddAutoSizeHoverText(GuiComposerHelpers.AddItemSlotGrid(GuiComposerHelpers.AddAutoSizeHoverText(GuiComposerHelpers.AddItemSlotGrid(GuiComposerHelpers.AddItemSlotGrid(GuiElementInsetHelper.AddInset(GuiComposerHelpers.AddItemSlotGrid(GuiComposerHelpers.AddItemSlotGrid(GuiComposerHelpers.AddItemSlotGrid(GuiComposerHelpers.AddItemSlotGrid(GuiComposerHelpers.AddItemSlotGrid(GuiComposerHelpers.AddItemSlotGrid(GuiComposerHelpers.AddItemSlotGrid(GuiElementInsetHelper.AddInset(composer, outerInsetBounds, 0, 0.85f).AddIf(!ArmorInventory._disableVanillaArmorSlots), characterInv, SendInvPacket, 1, new int[]
			{
				12
			}, leftArmorSlotBoundsHead, "armorSlotsHead"), characterInv, SendInvPacket, 1, new int[]
			{
				13
			}, leftArmorSlotBoundsBody, "armorSlotsBody"), characterInv, SendInvPacket, 1, new int[]
			{
				14
			}, leftArmorSlotBoundsLegs, "armorSlotsLegs").EndIf().AddIf(CharacterTabPatch.SlotsStatus.Misc), characterInv, SendInvPacket, 1, new int[]
			{
				ArmorInventory._gearSlotsLastIndex - 11
			}, leftMiscSlotBounds, "miscSlot1"), characterInv, SendInvPacket, 1, new int[]
			{
				ArmorInventory._gearSlotsLastIndex - 10
			}, leftMiscSlotBounds2, "miscSlot2"), characterInv, SendInvPacket, 1, new int[]
			{
				ArmorInventory._gearSlotsLastIndex - 9
			}, leftMiscSlotBounds3, "miscSlot3").EndIf(), characterInv, SendInvPacket, 1, new int[]
			{
				0,
				1,
				2,
				11,
				3,
				4
			}, leftSlotBounds, "leftSlots"), insetSlotBounds, 2, 0.85f), characterInv, SendInvPacket, 1, new int[]
			{
				6,
				7,
				8,
				10,
				5,
				9
			}, rightSlotBounds, "rightSlots").AddIf(CharacterTabPatch.SlotsStatus.Headgear), characterInv, SendInvPacket, 1, new int[]
			{
				ArmorInventory._armorSlotsLastIndex
			}, rightGearSlotBounds, "gearSlots1"), Lang.Get("combatoverhaul:slot-headgear", Array.Empty<object>()), hoverTextFont, 200, rightGearSlotBounds, null).EndIf().AddIf(CharacterTabPatch.SlotsStatus.FrontGear), characterInv, SendInvPacket, 1, new int[]
			{
				ArmorInventory._armorSlotsLastIndex + 1
			}, rightGearSlotBounds.BelowCopy(0.0, pad, 0.0, 0.0), "gearSlots2"), Lang.Get("combatoverhaul:slot-frontgear", Array.Empty<object>()), hoverTextFont, 200, rightGearSlotBounds.BelowCopy(0.0, pad, 0.0, 0.0), null).EndIf().AddIf(CharacterTabPatch.SlotsStatus.BackGear), characterInv, SendInvPacket, 1, new int[]
			{
				ArmorInventory._armorSlotsLastIndex + 2
			}, rightGearSlotBounds.BelowCopy(0.0, pad, 0.0, 0.0).BelowCopy(0.0, pad, 0.0, 0.0), "gearSlots3"), Lang.Get("combatoverhaul:slot-backgear", Array.Empty<object>()), hoverTextFont, 200, rightGearSlotBounds.BelowCopy(0.0, pad, 0.0, 0.0).BelowCopy(0.0, pad, 0.0, 0.0), null).EndIf().AddIf(CharacterTabPatch.SlotsStatus.RightShoulderGear), characterInv, SendInvPacket, 1, new int[]
			{
				ArmorInventory._armorSlotsLastIndex + 3
			}, rightGearSlotBounds.BelowCopy(0.0, pad, 0.0, 0.0).BelowCopy(0.0, pad, 0.0, 0.0).BelowCopy(0.0, pad, 0.0, 0.0), "gearSlots4"), Lang.Get("combatoverhaul:slot-rightshouldergear", Array.Empty<object>()), hoverTextFont, 200, rightGearSlotBounds.BelowCopy(0.0, pad, 0.0, 0.0).BelowCopy(0.0, pad, 0.0, 0.0).BelowCopy(0.0, pad, 0.0, 0.0), null).EndIf().AddIf(CharacterTabPatch.SlotsStatus.LeftShoulderGear), characterInv, SendInvPacket, 1, new int[]
			{
				ArmorInventory._armorSlotsLastIndex + 4
			}, rightGearSlotBounds.BelowCopy(0.0, pad, 0.0, 0.0).BelowCopy(0.0, pad, 0.0, 0.0).BelowCopy(0.0, pad, 0.0, 0.0).BelowCopy(0.0, pad, 0.0, 0.0), "gearSlots5"), Lang.Get("combatoverhaul:slot-leftshouldergear", Array.Empty<object>()), hoverTextFont, 200, rightGearSlotBounds.BelowCopy(0.0, pad, 0.0, 0.0).BelowCopy(0.0, pad, 0.0, 0.0).BelowCopy(0.0, pad, 0.0, 0.0).BelowCopy(0.0, pad, 0.0, 0.0), null).EndIf().AddIf(CharacterTabPatch.SlotsStatus.WaistHear), characterInv, SendInvPacket, 1, new int[]
			{
				ArmorInventory._armorSlotsLastIndex + 5
			}, rightGearSlotBounds.BelowCopy(0.0, pad, 0.0, 0.0).BelowCopy(0.0, pad, 0.0, 0.0).BelowCopy(0.0, pad, 0.0, 0.0).BelowCopy(0.0, pad, 0.0, 0.0).BelowCopy(0.0, pad, 0.0, 0.0), "gearSlots6"), Lang.Get("combatoverhaul:slot-waistgear", Array.Empty<object>()), hoverTextFont, 200, rightGearSlotBounds.BelowCopy(0.0, pad, 0.0, 0.0).BelowCopy(0.0, pad, 0.0, 0.0).BelowCopy(0.0, pad, 0.0, 0.0).BelowCopy(0.0, pad, 0.0, 0.0).BelowCopy(0.0, pad, 0.0, 0.0), null).EndIf().AddIf(CharacterTabPatch.SlotsStatus.Belt), characterInv, SendInvPacket, 9, new int[]
			{
				ArmorInventory._armorSlotsLastIndex + 9
			}, additionalSlots1Bounds, "additionalSlots10"), Lang.Get("combatoverhaul:slot-addBeltLeft", Array.Empty<object>()), hoverTextFont, 200, additionalSlots1Bounds, null), characterInv, SendInvPacket, 9, new int[]
			{
				ArmorInventory._armorSlotsLastIndex + 10
			}, additionalSlots1Bounds.RightCopy(0.0, 0.0, 0.0, 0.0), "additionalSlots11"), Lang.Get("combatoverhaul:slot-addBeltRight", Array.Empty<object>()), hoverTextFont, 200, additionalSlots1Bounds.RightCopy(0.0, 0.0, 0.0, 0.0), null), characterInv, SendInvPacket, 9, new int[]
			{
				ArmorInventory._armorSlotsLastIndex + 11
			}, additionalSlots1Bounds.RightCopy(0.0, 0.0, 0.0, 0.0).RightCopy(0.0, 0.0, 0.0, 0.0), "additionalSlots12"), Lang.Get("combatoverhaul:slot-addBeltBack", Array.Empty<object>()), hoverTextFont, 200, additionalSlots1Bounds.RightCopy(0.0, 0.0, 0.0, 0.0).RightCopy(0.0, 0.0, 0.0, 0.0), null), characterInv, SendInvPacket, 9, new int[]
			{
				ArmorInventory._armorSlotsLastIndex + 12
			}, additionalSlots1Bounds.RightCopy(0.0, 0.0, 0.0, 0.0).RightCopy(0.0, 0.0, 0.0, 0.0).RightCopy(0.0, 0.0, 0.0, 0.0), "additionalSlots13"), Lang.Get("combatoverhaul:slot-addBeltFront", Array.Empty<object>()), hoverTextFont, 200, additionalSlots1Bounds.RightCopy(0.0, 0.0, 0.0, 0.0).RightCopy(0.0, 0.0, 0.0, 0.0).RightCopy(0.0, 0.0, 0.0, 0.0), null).EndIf().AddIf(CharacterTabPatch.SlotsStatus.Backpack), characterInv, SendInvPacket, 9, new int[]
			{
				ArmorInventory._armorSlotsLastIndex + 13
			}, additionalSlots2Bounds, "additionalSlots20"), Lang.Get("combatoverhaul:slot-addBackpack1", Array.Empty<object>()), hoverTextFont, 200, additionalSlots2Bounds, null), characterInv, SendInvPacket, 9, new int[]
			{
				ArmorInventory._armorSlotsLastIndex + 14
			}, additionalSlots2Bounds.RightCopy(0.0, 0.0, 0.0, 0.0), "additionalSlots21"), Lang.Get("combatoverhaul:slot-addBackpack2", Array.Empty<object>()), hoverTextFont, 200, additionalSlots2Bounds.RightCopy(0.0, 0.0, 0.0, 0.0), null), characterInv, SendInvPacket, 9, new int[]
			{
				ArmorInventory._armorSlotsLastIndex + 15
			}, additionalSlots2Bounds.RightCopy(0.0, 0.0, 0.0, 0.0).RightCopy(0.0, 0.0, 0.0, 0.0), "additionalSlots22"), Lang.Get("combatoverhaul:slot-addBackpack3", Array.Empty<object>()), hoverTextFont, 200, additionalSlots2Bounds.RightCopy(0.0, 0.0, 0.0, 0.0).RightCopy(0.0, 0.0, 0.0, 0.0), null), characterInv, SendInvPacket, 9, new int[]
			{
				ArmorInventory._armorSlotsLastIndex + 16
			}, additionalSlots2Bounds.RightCopy(0.0, 0.0, 0.0, 0.0).RightCopy(0.0, 0.0, 0.0, 0.0).RightCopy(0.0, 0.0, 0.0, 0.0), "additionalSlots23"), Lang.Get("combatoverhaul:slot-addBackpack4", Array.Empty<object>()), hoverTextFont, 200, additionalSlots2Bounds.RightCopy(0.0, 0.0, 0.0, 0.0).RightCopy(0.0, 0.0, 0.0, 0.0).RightCopy(0.0, 0.0, 0.0, 0.0), null).EndIf().AddIf(CharacterTabPatch.VanityTabEnabled), "to vanity tab", () => CharacterTabPatch.SwitchTab(dialog, composer), vanityButtonBounds, vanityButtonFont, 3, "vanityButton").EndIf();
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x000132FC File Offset: 0x000114FC
		private static void ComposeVanityTab(GuiDialogCharacter dialog, GuiComposer composer)
		{
			if (CharacterTabPatch.Api == null)
			{
				return;
			}
			FieldInfo guiDialogCharacter_characterInv = CharacterTabPatch.GuiDialogCharacter_characterInv;
			ArmorInventory characterInventory = (ArmorInventory)((guiDialogCharacter_characterInv != null) ? guiDialogCharacter_characterInv.GetValue(dialog) : null);
			VanityInventory vanityInventory = GeneralUtils.GetVanityInventory(CharacterTabPatch.Api.World.Player);
			if (vanityInventory == null || characterInventory == null)
			{
				return;
			}
			vanityInventory.RecolorSlots();
			List<ItemSlot> list = Enumerable.ToList<ItemSlot>(vanityInventory);
			EnumerableExtensions.Foreach<IClickableSlot>(Enumerable.OfType<IClickableSlot>(list), delegate(IClickableSlot slot)
			{
				IClickableSlot.SlotClickedDelegate onSlotClicked;
				if ((onSlotClicked = CharacterTabPatch.<>O.<0>__OnSlotClicked) == null)
				{
					onSlotClicked = (CharacterTabPatch.<>O.<0>__OnSlotClicked = new IClickableSlot.SlotClickedDelegate(CharacterTabPatch.OnSlotClicked));
				}
				slot.OnSlotClicked = onSlotClicked;
			});
			int[] slotIndexes = Enumerable.ToArray<int>(Enumerable.Where<int>(Enumerable.Where<int>(Enumerable.Select<ItemSlot, int>(list, (ItemSlot slot) => vanityInventory.GetSlotId(slot)), (int id) => !ArmorInventory._disableVanillaArmorSlots || !ArmorInventory.IsVanillaArmorSlot(id)), (int id) => ArmorInventory._disableVanillaArmorSlots || !ArmorInventory.IsModdedArmorSlot(id)));
			int skillItemsHeight = slotIndexes.Length * 44 / 4 + 176 - 44 + 3;
			double pad = GuiElementItemSlotGridBase.unscaledSlotPadding;
			ElementBounds vanityButtonBounds = ElementBounds.Fixed(260.0, -16.0, 120.0, 24.0);
			ElementBounds outerInsetBounds = ElementBounds.Fixed(0.0, 22.0, 416.0, 361.0);
			ElementBounds insetSlotBounds = ElementBounds.Fixed(2.0, 2.0, 174.0, outerInsetBounds.fixedHeight - 4.0 - 44.0).WithParent(outerInsetBounds);
			ElementBounds saveButtonBounds = ElementBounds.Fixed(4.0, 28.0, 172.0, 36.0).FixedUnder(insetSlotBounds, 0.0);
			FieldInfo guiDialogCharacter_insetSlotBounds = CharacterTabPatch.GuiDialogCharacter_insetSlotBounds;
			if (guiDialogCharacter_insetSlotBounds != null)
			{
				guiDialogCharacter_insetSlotBounds.SetValue(dialog, insetSlotBounds);
			}
			ElementBounds rightInsetBounds = ElementBounds.Fixed(0.0, 2.0, 212.0, outerInsetBounds.fixedHeight - 2.0).WithParent(outerInsetBounds).RightOf(insetSlotBounds, 4.0);
			ElementBounds childBounds = ElementBounds.Fixed(0.0, 2.0, 218.0, (double)skillItemsHeight);
			ElementBounds clipBounds = rightInsetBounds.FlatCopy().FixedRightOf(insetSlotBounds, 4.0).FixedGrow(6.0, 11.0).WithFixedHeight(outerInsetBounds.fixedHeight - 2.0);
			ElementBounds slotBounds = ElementStdBounds.SlotGrid(0, 4.0, 0.0, 4, vanityInventory.Count / 4 + 1).FixedGrow(0.0, pad);
			ElementBounds scrollbarBounds = rightInsetBounds.FlatCopy().FixedRightOf(rightInsetBounds, 178.0).WithFixedWidth(20.0);
			scrollbarBounds.absInnerHeight = 300.0;
			CairoFont.WhiteSmallText();
			CairoFont vanityButtonFont = CairoFont.WhiteSmallText().WithOrientation(2);
			GuiElementInsetHelper.AddInset(composer, outerInsetBounds, 0, 0.85f);
			GuiElementInsetHelper.AddInset(composer, insetSlotBounds, 1, 0.85f);
			GuiElementInsetHelper.AddInset(composer, rightInsetBounds, 1, 0.85f);
			composer.BeginChildElements(rightInsetBounds);
			GuiElementClipHelpler.BeginClip(composer, clipBounds);
			composer.BeginChildElements(childBounds);
			GuiComposer composer2 = composer;
			IInventory vanityInventory2 = vanityInventory;
			Action<object> action;
			if ((action = CharacterTabPatch.<>O.<1>__SendInventoryPacket) == null)
			{
				action = (CharacterTabPatch.<>O.<1>__SendInventoryPacket = new Action<object>(CharacterTabPatch.SendInventoryPacket));
			}
			GuiComposerHelpers.AddItemSlotGrid(composer2, vanityInventory2, action, 4, slotIndexes, slotBounds, "vanitySlots");
			composer.EndChildElements();
			GuiElementClipHelpler.EndClip(composer);
			GuiComposerHelpers.AddVerticalScrollbar(composer, delegate(float value)
			{
				CharacterTabPatch.OnNewScrollbarValue(dialog, composer, childBounds, value);
			}, scrollbarBounds, "scrollbar");
			composer.EndChildElements();
			GuiComposerHelpers.AddButton(composer, "to clothes tab", () => CharacterTabPatch.SwitchTab(dialog, composer), vanityButtonBounds, vanityButtonFont, 3, "vanityButton");
			GuiComposer composer3 = composer;
			string text = "Hide backpack";
			CairoFont cairoFont = vanityButtonFont;
			Action<bool> action2;
			if ((action2 = CharacterTabPatch.<>O.<2>__SetHideBackpack) == null)
			{
				action2 = (CharacterTabPatch.<>O.<2>__SetHideBackpack = new Action<bool>(CharacterTabPatch.SetHideBackpack));
			}
			GuiComposerHelpers.AddToggleButton(composer3, text, cairoFont, action2, saveButtonBounds, "hideBackpackButton");
			int scrollbarPosition = CharacterTabPatch._vanityScrollBarPosition;
			GuiComposerHelpers.GetScrollbar(composer, "scrollbar").SetHeights((float)outerInsetBounds.fixedHeight - 2f, (float)skillItemsHeight);
			GuiComposerHelpers.GetScrollbar(composer, "scrollbar").SetScrollbarPosition(scrollbarPosition);
			CharacterTabPatch.SetChildBoundsOffset(childBounds, (float)scrollbarPosition);
			GuiComposerHelpers.GetToggleButton(composer, "hideBackpackButton").SetValue(CharacterTabPatch.IsBackpackHidden());
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x000137F8 File Offset: 0x000119F8
		private static bool IsBackpackHidden()
		{
			ICoreClientAPI api = CharacterTabPatch.Api;
			bool? flag;
			if (api == null)
			{
				flag = default(bool?);
			}
			else
			{
				IClientWorldAccessor world = api.World;
				if (world == null)
				{
					flag = default(bool?);
				}
				else
				{
					IClientPlayer player = world.Player;
					if (player == null)
					{
						flag = default(bool?);
					}
					else
					{
						EntityPlayer entity = player.Entity;
						if (entity == null)
						{
							flag = default(bool?);
						}
						else
						{
							SyncedTreeAttribute watchedAttributes = entity.WatchedAttributes;
							flag = ((watchedAttributes != null) ? new bool?(watchedAttributes.GetBool("combatoverhaul:hide-backpack", false)) : default(bool?));
						}
					}
				}
			}
			bool? flag2 = flag;
			return flag2.GetValueOrDefault();
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x00013881 File Offset: 0x00011A81
		private static void SetHideBackpack(bool hide)
		{
			ICoreClientAPI api = CharacterTabPatch.Api;
			if (api == null)
			{
				return;
			}
			IModLoader modLoader = api.ModLoader;
			if (modLoader == null)
			{
				return;
			}
			CombatOverhaulSystem modSystem = modLoader.GetModSystem<CombatOverhaulSystem>(true);
			if (modSystem == null)
			{
				return;
			}
			VanitySystemClient clientVanitySystem = modSystem.ClientVanitySystem;
			if (clientVanitySystem == null)
			{
				return;
			}
			clientVanitySystem.HideBackpack(hide);
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x000138B2 File Offset: 0x00011AB2
		private static void SendInventoryPacket(object packet)
		{
			ICoreClientAPI api = CharacterTabPatch.Api;
			if (api == null)
			{
				return;
			}
			api.Network.SendPacketClient(packet);
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x000138C9 File Offset: 0x00011AC9
		private static bool OnSlotClicked(ItemSlot slot, ItemSlot sourceSlot, ref ItemStackMoveOperation operation)
		{
			return false;
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x000138CC File Offset: 0x00011ACC
		private static void RegisterArmorIcons(ICoreClientAPI api)
		{
			api.Gui.Icons.CustomIcons["armorhead"] = CharacterTabPatch.Api.Gui.Icons.SvgIconSource(new AssetLocation("textures/icons/character/armor-helmet.svg"));
			api.Gui.Icons.CustomIcons["armorbody"] = CharacterTabPatch.Api.Gui.Icons.SvgIconSource(new AssetLocation("textures/icons/character/armor-body.svg"));
			api.Gui.Icons.CustomIcons["armorlegs"] = CharacterTabPatch.Api.Gui.Icons.SvgIconSource(new AssetLocation("textures/icons/character/armor-legs.svg"));
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x00013981 File Offset: 0x00011B81
		private static bool SwitchTab(GuiDialogCharacter dialog, GuiComposer composer)
		{
			CharacterTabPatch.SwitchCurrentTabType();
			MethodInfo guiDialogCharacter_ComposeGuis = CharacterTabPatch.GuiDialogCharacter_ComposeGuis;
			if (guiDialogCharacter_ComposeGuis != null)
			{
				guiDialogCharacter_ComposeGuis.Invoke(dialog, Array.Empty<object>());
			}
			return true;
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x000139A0 File Offset: 0x00011BA0
		private static bool SaveAndApply(GuiDialogCharacter dialog, GuiComposer composer)
		{
			Action onSaveAndApply = CharacterTabPatch.OnSaveAndApply;
			if (onSaveAndApply != null)
			{
				onSaveAndApply.Invoke();
			}
			ICoreClientAPI api = CharacterTabPatch.Api;
			if (api != null)
			{
				IClientWorldAccessor world = api.World;
				if (world != null)
				{
					IClientPlayer player = world.Player;
					if (player != null)
					{
						EntityPlayer entity = player.Entity;
						if (entity != null)
						{
							entity.MarkShapeModified();
						}
					}
				}
			}
			return true;
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x000139EF File Offset: 0x00011BEF
		private static void OnNewScrollbarValue(GuiDialogCharacter dialog, GuiComposer composer, ElementBounds childBounds, float value)
		{
			if (childBounds != null)
			{
				CharacterTabPatch.SetChildBoundsOffset(childBounds, value);
			}
			CharacterTabPatch._vanityScrollBarPosition = (int)(GuiComposerHelpers.GetScrollbar(composer, "scrollbar").CurrentYPosition / GuiComposerHelpers.GetScrollbar(composer, "scrollbar").ScrollConversionFactor);
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x00013A22 File Offset: 0x00011C22
		private static void SetChildBoundsOffset(ElementBounds childBounds, float value)
		{
			childBounds.fixedY = (double)(2f - value);
			childBounds.fixedY = childBounds.fixedY - childBounds.fixedY % 51.0 + 2.0;
			childBounds.CalcWorldBounds();
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x00013A5F File Offset: 0x00011C5F
		private static void SwitchCurrentTabType()
		{
			CharacterTabPatch._currentTabType = (CharacterTabPatch._currentTabType + 1) % (CharacterTabPatch.CharacterTabType)Enum.GetValues<CharacterTabPatch.CharacterTabType>().Length;
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x00013A75 File Offset: 0x00011C75
		private static int GetSlotIndex(ItemSlot slot)
		{
			InventoryBase inventory = slot.Inventory;
			if (inventory == null)
			{
				return 0;
			}
			return inventory.GetSlotId(slot);
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x00013A8C File Offset: 0x00011C8C
		private static bool IsSlotItemHidden(ItemSlot slot)
		{
			int index = CharacterTabPatch.GetSlotIndex(slot);
			return CharacterTabPatch.HiddenSlots.Contains(index);
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x00013AAC File Offset: 0x00011CAC
		private static bool IsSlotEnabled(ItemSlot slot)
		{
			int index = CharacterTabPatch.GetSlotIndex(slot);
			return !CharacterTabPatch.SlotsThatCantBeHidden.Contains(index);
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x00013AD0 File Offset: 0x00011CD0
		private static string GetSlotColor(ItemSlot slot)
		{
			bool active = !CharacterTabPatch.IsSlotItemHidden(slot);
			if (!CharacterTabPatch.IsSlotEnabled(slot))
			{
				return "#333333FF";
			}
			if (!active)
			{
				return "#FF5555FF";
			}
			return "#FFFFFFFF";
		}

		// Token: 0x04000161 RID: 353
		public static readonly HashSet<int> SlotsThatCantBeHidden = new HashSet<int>();

		// Token: 0x04000162 RID: 354
		public static readonly HashSet<int> HiddenSlots = new HashSet<int>();

		// Token: 0x04000164 RID: 356
		[Nullable(2)]
		private static FieldInfo GuiDialogCharacter_insetSlotBounds = typeof(GuiDialogCharacter).GetField("insetSlotBounds", 36);

		// Token: 0x04000165 RID: 357
		[Nullable(2)]
		private static FieldInfo GuiDialogCharacter_characterInv = typeof(GuiDialogCharacter).GetField("characterInv", 36);

		// Token: 0x04000166 RID: 358
		[Nullable(2)]
		private static MethodInfo GuiDialogCharacter_SendInvPacket = typeof(GuiDialogCharacter).GetMethod("SendInvPacket", 36);

		// Token: 0x04000167 RID: 359
		[Nullable(2)]
		private static MethodInfo GuiDialogCharacter_ComposeGuis = typeof(GuiDialogCharacter).GetMethod("ComposeGuis", 36);

		// Token: 0x04000168 RID: 360
		private static CharacterTabPatch.CharacterTabType _currentTabType = CharacterTabPatch.CharacterTabType.Default;

		// Token: 0x04000169 RID: 361
		private static int _vanityScrollBarPosition = 0;

		// Token: 0x020001DA RID: 474
		[NullableContext(0)]
		public class CharacterSlotsStatus
		{
			// Token: 0x17000528 RID: 1320
			// (get) Token: 0x0600154F RID: 5455 RVA: 0x0006896C File Offset: 0x00066B6C
			// (set) Token: 0x06001550 RID: 5456 RVA: 0x00068974 File Offset: 0x00066B74
			public bool Misc { get; set; }

			// Token: 0x17000529 RID: 1321
			// (get) Token: 0x06001551 RID: 5457 RVA: 0x0006897D File Offset: 0x00066B7D
			// (set) Token: 0x06001552 RID: 5458 RVA: 0x00068985 File Offset: 0x00066B85
			public bool Belt { get; set; }

			// Token: 0x1700052A RID: 1322
			// (get) Token: 0x06001553 RID: 5459 RVA: 0x0006898E File Offset: 0x00066B8E
			// (set) Token: 0x06001554 RID: 5460 RVA: 0x00068996 File Offset: 0x00066B96
			public bool Backpack { get; set; }

			// Token: 0x1700052B RID: 1323
			// (get) Token: 0x06001555 RID: 5461 RVA: 0x0006899F File Offset: 0x00066B9F
			// (set) Token: 0x06001556 RID: 5462 RVA: 0x000689A7 File Offset: 0x00066BA7
			public bool Headgear { get; set; }

			// Token: 0x1700052C RID: 1324
			// (get) Token: 0x06001557 RID: 5463 RVA: 0x000689B0 File Offset: 0x00066BB0
			// (set) Token: 0x06001558 RID: 5464 RVA: 0x000689B8 File Offset: 0x00066BB8
			public bool FrontGear { get; set; }

			// Token: 0x1700052D RID: 1325
			// (get) Token: 0x06001559 RID: 5465 RVA: 0x000689C1 File Offset: 0x00066BC1
			// (set) Token: 0x0600155A RID: 5466 RVA: 0x000689C9 File Offset: 0x00066BC9
			public bool BackGear { get; set; }

			// Token: 0x1700052E RID: 1326
			// (get) Token: 0x0600155B RID: 5467 RVA: 0x000689D2 File Offset: 0x00066BD2
			// (set) Token: 0x0600155C RID: 5468 RVA: 0x000689DA File Offset: 0x00066BDA
			public bool RightShoulderGear { get; set; }

			// Token: 0x1700052F RID: 1327
			// (get) Token: 0x0600155D RID: 5469 RVA: 0x000689E3 File Offset: 0x00066BE3
			// (set) Token: 0x0600155E RID: 5470 RVA: 0x000689EB File Offset: 0x00066BEB
			public bool LeftShoulderGear { get; set; }

			// Token: 0x17000530 RID: 1328
			// (get) Token: 0x0600155F RID: 5471 RVA: 0x000689F4 File Offset: 0x00066BF4
			// (set) Token: 0x06001560 RID: 5472 RVA: 0x000689FC File Offset: 0x00066BFC
			public bool WaistHear { get; set; }
		}

		// Token: 0x020001DB RID: 475
		[NullableContext(0)]
		private enum CharacterTabType
		{
			// Token: 0x04000AF9 RID: 2809
			Default,
			// Token: 0x04000AFA RID: 2810
			Vanity
		}

		// Token: 0x020001DC RID: 476
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000AFB RID: 2811
			[Nullable(0)]
			public static IClickableSlot.SlotClickedDelegate <0>__OnSlotClicked;

			// Token: 0x04000AFC RID: 2812
			[Nullable(0)]
			public static Action<object> <1>__SendInventoryPacket;

			// Token: 0x04000AFD RID: 2813
			[Nullable(0)]
			public static Action<bool> <2>__SetHideBackpack;
		}
	}
}
