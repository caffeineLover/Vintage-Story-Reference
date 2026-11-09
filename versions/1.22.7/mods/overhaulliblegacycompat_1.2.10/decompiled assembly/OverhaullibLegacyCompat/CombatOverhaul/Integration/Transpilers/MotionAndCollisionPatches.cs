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
		// Token: 0x020001FF RID: 511
		[HarmonyPatch(typeof(EntityBehaviorPassivePhysics), "MotionAndCollision")]
		[HarmonyPatchCategory("combatoverhaul")]
		public class EntityBehaviorPassivePhysicsMotionAndCollisionPatch
		{
			// Token: 0x060015E7 RID: 5607 RVA: 0x0006A4DC File Offset: 0x000686DC
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
