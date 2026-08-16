using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using HarmonyLib;
using Vintagestory.API.Common;

namespace CombatOverhaul.Integration.Transpilers
{
	// Token: 0x0200005E RID: 94
	internal static class MotionAndCollisionPatches
	{
		// Token: 0x020001F1 RID: 497
		[HarmonyPatch(typeof(EntityBehaviorPassivePhysics), "MotionAndCollision")]
		[HarmonyPatchCategory("combatoverhaul")]
		public class EntityBehaviorPassivePhysicsMotionAndCollisionPatch
		{
			// Token: 0x0600156F RID: 5487 RVA: 0x00068A34 File Offset: 0x00066C34
			[NullableContext(1)]
			[HarmonyTranspiler]
			public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
			{
				List<CodeInstruction> code = Enumerable.ToList<CodeInstruction>(instructions);
				for (int i = 0; i < code.Count; i++)
				{
					if (code[i].opcode == OpCodes.Ldc_R8 && (double)code[i].operand == -0.014999999664723873)
					{
						code[i].operand = 0.0;
						return code;
					}
				}
				return code;
			}
		}
	}
}
