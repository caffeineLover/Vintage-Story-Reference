using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RPVoiceChat.Gui;
using RPVoiceChat.Util;
using Vintagestory.API.Client;

namespace RPVoiceChat
{
	// Token: 0x02000017 RID: 23
	internal static class GuiElementSliderPatch
	{
		// Token: 0x06000079 RID: 121 RVA: 0x000044E8 File Offset: 0x000026E8
		public static void Patch(Harmony harmony)
		{
			MethodInfo OriginalMethod = AccessTools.Method(typeof(GuiElementSlider), "RenderInteractiveElements", null, null);
			MethodInfo TranspilerMethod = AccessTools.Method(typeof(GuiElementSliderPatch), "RenderInteractiveElements_Transpiler", null, null);
			harmony.Patch(OriginalMethod, null, null, new HarmonyMethod(TranspilerMethod), null);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00004534 File Offset: 0x00002734
		public static IEnumerable<CodeInstruction> RenderInteractiveElements_Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			int firstPatchIndex = -1;
			int secondPatchIndex = -1;
			Label firstPatchLabel = default(Label);
			Label secondPatchLabel = default(Label);
			List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
			for (int i = 0; i < codes.Count; i++)
			{
				CodeInstruction codeInstr = codes[i];
				if (!(codeInstr.opcode != GuiElementSliderPatch.anchor.opcode) && codeInstr.operand == GuiElementSliderPatch.anchor.operand)
				{
					for (int j = i + 1; j < codes.Count; j++)
					{
						codeInstr = codes[j];
						if (firstPatchLabel == default(Label))
						{
							if (codeInstr.opcode == GuiElementSliderPatch.patchOneIndexPointer.opcode)
							{
								firstPatchLabel = (Label)codeInstr.operand;
							}
						}
						else
						{
							if (codeInstr.labels.Contains(firstPatchLabel))
							{
								firstPatchIndex = j;
								codeInstr.labels.Remove(firstPatchLabel);
							}
							if (secondPatchLabel == default(Label))
							{
								if (codeInstr.opcode == GuiElementSliderPatch.patchTwoIndexPointer.opcode)
								{
									secondPatchLabel = (Label)codeInstr.operand;
								}
							}
							else if (codeInstr.labels.Contains(secondPatchLabel))
							{
								secondPatchIndex = j;
								codeInstr.labels.Remove(secondPatchLabel);
							}
						}
					}
					break;
				}
			}
			if (firstPatchIndex == -1 || secondPatchIndex == -1)
			{
				Logger.client.Error("Couldn't find transpiler anchor for GuiElementSlider patch");
				return instructions;
			}
			GuiElementSliderPatch.secondPatch[0].labels.Add(secondPatchLabel);
			GuiElementSliderPatch.firstPatch[0].labels.Add(firstPatchLabel);
			codes.InsertRange(secondPatchIndex, GuiElementSliderPatch.secondPatch);
			codes.InsertRange(firstPatchIndex, GuiElementSliderPatch.firstPatch);
			Logger.client.Notification("GuiElementSlider was successfully patched");
			return Enumerable.AsEnumerable<CodeInstruction>(codes);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x0000470B File Offset: 0x0000290B
		private static void DisableScissors(ICoreClientAPI api, GuiElementSlider slider)
		{
			if (api.Render.ScissorStack.Count > 0 && slider is NamedSlider)
			{
				api.Render.GlScissorFlag(false);
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x0000473A File Offset: 0x0000293A
		private static void EnableScissors(ICoreClientAPI api, GuiElementSlider slider)
		{
			if (api.Render.ScissorStack.Count > 0 && slider is NamedSlider)
			{
				api.Render.GlScissorFlag(true);
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x0000476C File Offset: 0x0000296C
		// Note: this type is marked as 'beforefieldinit'.
		static GuiElementSliderPatch()
		{
			List<CodeInstruction> list = new List<CodeInstruction>();
			list.Add(new CodeInstruction(OpCodes.Ldarg_0, null));
			list.Add(new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(GuiElement), "api")));
			list.Add(new CodeInstruction(OpCodes.Ldarg_0, null));
			list.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GuiElementSliderPatch), "DisableScissors", null, null)));
			GuiElementSliderPatch.firstPatch = list;
			List<CodeInstruction> list2 = new List<CodeInstruction>();
			list2.Add(new CodeInstruction(OpCodes.Ldarg_0, null));
			list2.Add(new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(GuiElement), "api")));
			list2.Add(new CodeInstruction(OpCodes.Ldarg_0, null));
			list2.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GuiElementSliderPatch), "EnableScissors", null, null)));
			GuiElementSliderPatch.secondPatch = list2;
		}

		// Token: 0x04000025 RID: 37
		private static readonly CodeInstruction anchor = new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(GuiElementSlider), "mouseDownOnSlider"));

		// Token: 0x04000026 RID: 38
		private static readonly CodeInstruction patchOneIndexPointer = new CodeInstruction(OpCodes.Brtrue_S, null);

		// Token: 0x04000027 RID: 39
		private static readonly CodeInstruction patchTwoIndexPointer = new CodeInstruction(OpCodes.Brfalse, null);

		// Token: 0x04000028 RID: 40
		private static List<CodeInstruction> firstPatch;

		// Token: 0x04000029 RID: 41
		private static List<CodeInstruction> secondPatch;
	}
}
