using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using HarmonyLib;
using Vintagestory.GameContent;

namespace CombatOverhaul.Integration.Transpilers
{
	// Token: 0x0200005B RID: 91
	[NullableContext(1)]
	[Nullable(0)]
	[HarmonyPatch]
	[HarmonyPatchCategory("combatoverhaul")]
	internal static class EidolonSlam_KnockbackMultiplierPatch
	{
		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060004BE RID: 1214 RVA: 0x0001C376 File Offset: 0x0001A576
		// (set) Token: 0x060004BF RID: 1215 RVA: 0x0001C37D File Offset: 0x0001A57D
		public static float KnockbackMultiplier { get; set; } = 1f;

		// Token: 0x060004C0 RID: 1216 RVA: 0x0001C385 File Offset: 0x0001A585
		private static MethodBase TargetMethod()
		{
			return AccessTools.Method(typeof(AiTaskEidolonSlam).GetNestedType("<>c__DisplayClass20_0", 32), "<ContinueExecute>b__0", null, null);
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x0001C3A9 File Offset: 0x0001A5A9
		private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			EidolonSlam_KnockbackMultiplierPatch.<Transpiler>d__5 <Transpiler>d__ = new EidolonSlam_KnockbackMultiplierPatch.<Transpiler>d__5(-2);
			<Transpiler>d__.<>3__instructions = instructions;
			return <Transpiler>d__;
		}
	}
}
