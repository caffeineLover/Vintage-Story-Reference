using System;
using System.Runtime.CompilerServices;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace ConfigLib.Patches
{
	// Token: 0x02000050 RID: 80
	internal static class PauseMenuPatch
	{
		// Token: 0x0600025C RID: 604 RVA: 0x0000D520 File Offset: 0x0000B720
		public static void Patch()
		{
			new Harmony("configlib").Patch(typeof(GuiComposerHelpers).GetMethod("AddButton", AccessTools.all, new Type[]
			{
				typeof(GuiComposer),
				typeof(string),
				typeof(ActionConsumable),
				typeof(ElementBounds),
				typeof(EnumButtonStyle),
				typeof(string)
			}), new HarmonyMethod(AccessTools.Method(typeof(PauseMenuPatch), "AddButton", null, null)), null, null, null);
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000D5C8 File Offset: 0x0000B7C8
		public static void Unpatch()
		{
			new Harmony("configlib").Unpatch(typeof(GuiComposerHelpers).GetMethod("AddButton", AccessTools.all, new Type[]
			{
				typeof(GuiComposer),
				typeof(string),
				typeof(ActionConsumable),
				typeof(ElementBounds),
				typeof(EnumButtonStyle),
				typeof(string)
			}), 1, "*");
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000D658 File Offset: 0x0000B858
		[NullableContext(1)]
		private static bool AddButton(ref GuiComposer __result, GuiComposer composer, string text, ActionConsumable onClick, ElementBounds bounds)
		{
			if (text != Lang.Get("game:mainmenu-settings", Array.Empty<object>()) || bounds.fixedWidth < 200.0)
			{
				return true;
			}
			ElementBounds left = new ElementBounds
			{
				Alignment = 4,
				BothSizing = 0,
				fixedY = bounds.fixedY,
				fixedPaddingX = 2.0,
				fixedPaddingY = 2.0
			};
			ElementBounds right = new ElementBounds
			{
				Alignment = 12,
				BothSizing = 0,
				fixedY = bounds.fixedY,
				fixedPaddingX = 2.0,
				fixedPaddingY = 2.0
			};
			GuiComposer guiComposer = GuiComposerHelpers.AddButton(composer, text, onClick, left.WithFixedWidth(144.0), 2, null);
			string text2 = "Mods settings";
			ActionConsumable actionConsumable;
			if ((actionConsumable = PauseMenuPatch.<>O.<0>__ShowConfigWindowStatic) == null)
			{
				actionConsumable = (PauseMenuPatch.<>O.<0>__ShowConfigWindowStatic = new ActionConsumable(GuiManager.ShowConfigWindowStatic));
			}
			__result = GuiComposerHelpers.AddButton(guiComposer, text2, actionConsumable, right.WithFixedWidth(183.0), 2, null);
			return false;
		}

		// Token: 0x02000051 RID: 81
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x0400011C RID: 284
			public static ActionConsumable <0>__ShowConfigWindowStatic;
		}
	}
}
