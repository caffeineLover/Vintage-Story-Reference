using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using HarmonyLib;
using Vintagestory.API.Common;

namespace CombatOverhaul.Integration.Transpilers
{
	// Token: 0x0200005D RID: 93
	internal static class ElementPosePatches
	{
		// Token: 0x020001FD RID: 509
		[HarmonyPatch(typeof(Animation), "GenerateFrame")]
		[HarmonyPatchCategory("combatoverhaul")]
		public static class Animation_GenerateFrame_Patch
		{
			// Token: 0x060015E5 RID: 5605 RVA: 0x0006A234 File Offset: 0x00068434
			[NullableContext(1)]
			private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
			{
				ConstructorInfo elementPoseCtor = AccessTools.Constructor(typeof(ElementPose), null, false);
				ConstructorInfo extendedPoseCtor = AccessTools.Constructor(typeof(ExtendedElementPose), null, false);
				MethodInfo setElementName = AccessTools.Method(typeof(ExtendedElementPose), "ResolveElementName", null, null);
				List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
				for (int i = 0; i < codes.Count; i++)
				{
					CodeInstruction instr = codes[i];
					if (instr.opcode == OpCodes.Newobj && instr.operand == elementPoseCtor)
					{
						instr.operand = extendedPoseCtor;
						if (i + 1 < codes.Count && codes[i + 1].opcode == OpCodes.Stloc_3)
						{
							List<CodeInstruction> list = new List<CodeInstruction>();
							list.Add(new CodeInstruction(OpCodes.Ldloc_3, null));
							list.Add(new CodeInstruction(OpCodes.Ldloc_2, null));
							list.Add(new CodeInstruction(OpCodes.Callvirt, setElementName));
							List<CodeInstruction> injected = list;
							codes.InsertRange(i + 2, injected);
							i += injected.Count;
						}
					}
				}
				return codes;
			}
		}

		// Token: 0x020001FE RID: 510
		[HarmonyPatch(typeof(ClientAnimator), "LoadPosesAndAttachmentPoints")]
		[HarmonyPatchCategory("combatoverhaul")]
		public static class ClientAnimator_LoadPosesAndAttachmentPoints_Patch
		{
			// Token: 0x060015E6 RID: 5606 RVA: 0x0006A350 File Offset: 0x00068550
			[NullableContext(1)]
			private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
			{
				List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
				ConstructorInfo elementPoseCtor = AccessTools.Constructor(typeof(ElementPose), null, false);
				ConstructorInfo extendedPoseCtor = AccessTools.Constructor(typeof(ExtendedElementPose), null, false);
				MethodInfo setElementName = AccessTools.Method(typeof(ExtendedElementPose), "ResolveElementName", null, null);
				for (int i = 0; i < codes.Count; i++)
				{
					CodeInstruction instr = codes[i];
					if (instr.opcode == OpCodes.Newobj && instr.operand == elementPoseCtor)
					{
						instr.operand = extendedPoseCtor;
						int insertIndex = -1;
						for (int j = i + 1; j < codes.Count; j++)
						{
							CodeInstruction c = codes[j];
							if (c.opcode == OpCodes.Callvirt)
							{
								MethodInfo mi = c.operand as MethodInfo;
								if (mi != null && mi.Name == "Add" && mi.DeclaringType.IsGenericType && mi.DeclaringType.GetGenericTypeDefinition() == typeof(List))
								{
									insertIndex = j + 1;
									break;
								}
							}
						}
						if (insertIndex != -1)
						{
							List<CodeInstruction> list = new List<CodeInstruction>();
							list.Add(new CodeInstruction(OpCodes.Ldloc_0, null));
							list.Add(new CodeInstruction(OpCodes.Ldloc_2, null));
							list.Add(new CodeInstruction(OpCodes.Callvirt, setElementName));
							List<CodeInstruction> injected = list;
							codes.InsertRange(insertIndex, injected);
							i = insertIndex + injected.Count - 1;
						}
					}
				}
				return codes;
			}
		}
	}
}
