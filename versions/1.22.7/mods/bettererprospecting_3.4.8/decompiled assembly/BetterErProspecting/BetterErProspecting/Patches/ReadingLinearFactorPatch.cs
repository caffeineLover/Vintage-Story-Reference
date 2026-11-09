using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using BetterErProspecting.Config;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace BetterErProspecting.Patches
{
	// Token: 0x02000009 RID: 9
	[HarmonyPatch(typeof(PropickReading), "ToHumanReadable")]
	[HarmonyPatchCategory("Always")]
	public class ReadingLinearFactorPatch
	{
		// Token: 0x0600003F RID: 63 RVA: 0x00003A84 File Offset: 0x00001C84
		[NullableContext(1)]
		private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			CodeMatcher matcher = new CodeMatcher(instructions, null);
			matcher.Start().MatchStartForward(new CodeMatch[]
			{
				new CodeMatch(new OpCode?(OpCodes.Ldc_R8), 7.5, null),
				new CodeMatch(new OpCode?(OpCodes.Mul), null, null)
			});
			if (matcher.IsInvalid)
			{
				ILogger logger = BetterErProspect.Logger;
				if (logger != null)
				{
					logger.Error("[PropickReadingPatch] Failed to find the 7.5 multiplication pattern");
				}
				return instructions;
			}
			matcher.SetOperandAndAdvance(Constants.LinearFactorValue);
			return matcher.InstructionEnumeration();
		}
	}
}
