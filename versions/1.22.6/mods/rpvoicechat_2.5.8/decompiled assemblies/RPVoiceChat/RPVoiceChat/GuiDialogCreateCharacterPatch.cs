using System;
using System.Reflection;
using HarmonyLib;
using Vintagestory.GameContent;

namespace RPVoiceChat
{
	// Token: 0x02000016 RID: 22
	internal static class GuiDialogCreateCharacterPatch
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000074 RID: 116 RVA: 0x000042FC File Offset: 0x000024FC
		// (remove) Token: 0x06000075 RID: 117 RVA: 0x00004330 File Offset: 0x00002530
		public static event Action OnCharacterSelection;

		// Token: 0x06000076 RID: 118 RVA: 0x00004364 File Offset: 0x00002564
		public static void Patch(Harmony harmony)
		{
			MethodInfo OriginalMethod = AccessTools.Method(typeof(GuiDialogCreateCharacter), "OnGuiClosed", null, null);
			MethodInfo PostfixMethod = AccessTools.Method(typeof(GuiDialogCreateCharacterPatch), "OnGuiClosed", null, null);
			harmony.Patch(OriginalMethod, null, new HarmonyMethod(PostfixMethod), null, null);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x000043B0 File Offset: 0x000025B0
		public static void OnGuiClosed()
		{
			Action onCharacterSelection = GuiDialogCreateCharacterPatch.OnCharacterSelection;
			if (onCharacterSelection == null)
			{
				return;
			}
			onCharacterSelection.Invoke();
		}
	}
}
