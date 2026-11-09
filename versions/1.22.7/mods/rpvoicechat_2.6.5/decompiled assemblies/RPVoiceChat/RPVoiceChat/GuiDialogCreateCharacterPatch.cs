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
		// (add) Token: 0x06000075 RID: 117 RVA: 0x00004420 File Offset: 0x00002620
		// (remove) Token: 0x06000076 RID: 118 RVA: 0x00004454 File Offset: 0x00002654
		public static event Action OnCharacterSelection;

		// Token: 0x06000077 RID: 119 RVA: 0x00004488 File Offset: 0x00002688
		public static void Patch(Harmony harmony)
		{
			MethodInfo OriginalMethod = AccessTools.Method(typeof(GuiDialogCreateCharacter), "OnGuiClosed", null, null);
			MethodInfo PostfixMethod = AccessTools.Method(typeof(GuiDialogCreateCharacterPatch), "OnGuiClosed", null, null);
			harmony.Patch(OriginalMethod, null, new HarmonyMethod(PostfixMethod), null, null);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x000044D4 File Offset: 0x000026D4
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
