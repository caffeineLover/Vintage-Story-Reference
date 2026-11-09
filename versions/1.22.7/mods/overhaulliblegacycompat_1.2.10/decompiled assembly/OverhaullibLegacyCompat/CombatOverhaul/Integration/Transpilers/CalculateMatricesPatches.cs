using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using HarmonyLib;
using Vintagestory.API.Common;

namespace CombatOverhaul.Integration.Transpilers
{
	// Token: 0x02000059 RID: 89
	internal static class CalculateMatricesPatches
	{
		// Token: 0x020001F9 RID: 505
		[HarmonyPatch(typeof(ClientAnimator), "calculateMatrices", new Type[]
		{
			typeof(int),
			typeof(float),
			typeof(List<ElementPose>),
			typeof(ShapeElementWeights[][]),
			typeof(float[]),
			typeof(List<ElementPose>[]),
			typeof(List<ElementPose>[]),
			typeof(int)
		})]
		[HarmonyPatchCategory("combatoverhaul")]
		public class ClientAnimatorCalculateMatricesPatch
		{
			// Token: 0x060015D6 RID: 5590 RVA: 0x00069EE8 File Offset: 0x000680E8
			[NullableContext(1)]
			[HarmonyTranspiler]
			public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
			{
				List<CodeInstruction> code = Enumerable.ToList<CodeInstruction>(instructions);
				MethodInfo onFrameInvokeMethod = AccessTools.Method(typeof(AnimationPatches), "OnFrameInvoke", null, null);
				MethodInfo getLocalTransformMatrixMethod = AccessTools.Method(typeof(ShapeElement), "GetLocalTransformMatrix", null, null);
				for (int i = 0; i < code.Count; i++)
				{
					if (CodeInstructionExtensions.Calls(code[i], getLocalTransformMatrixMethod))
					{
						code.Insert(i, new CodeInstruction(OpCodes.Ldarg_0, null));
						code.Insert(i + 1, new CodeInstruction(OpCodes.Ldloc, 4));
						code.Insert(i + 2, new CodeInstruction(OpCodes.Call, onFrameInvokeMethod));
						break;
					}
				}
				return code;
			}
		}
	}
}
