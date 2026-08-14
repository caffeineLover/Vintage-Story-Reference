using System;
using System.Runtime.CompilerServices;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace ConfigLib.Patches
{
	// Token: 0x0200004E RID: 78
	internal static class PauseMenuPatch
	{
		// Token: 0x06000237 RID: 567 RVA: 0x0000D204 File Offset: 0x0000B404
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

		// Token: 0x06000238 RID: 568 RVA: 0x0000D2AC File Offset: 0x0000B4AC
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

		// Token: 0x06000239 RID: 569 RVA: 0x0000D33C File Offset: 0x0000B53C
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

		// Token: 0x0200004F RID: 79
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000119 RID: 281
			public static ActionConsumable <0>__ShowConfigWindowStatic;
		}
	}
}
