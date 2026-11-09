using System;
using System.Runtime.CompilerServices;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace CombatOverhaul.Integration
{
	// Token: 0x0200003B RID: 59
	[HarmonyPatch(typeof(BlockEntityGrindingWheel), "canBuff")]
	internal static class GrindingWheelCanBuffPatch
	{
		// Token: 0x060003DC RID: 988 RVA: 0x000120E7 File Offset: 0x000102E7
		[NullableContext(1)]
		private static void Postfix(ItemSlot slot, ref bool __result)
		{
			if (__result)
			{
				return;
			}
			__result = GrindingWheelCompat.TryEnableGrindingWheelBuff(slot);
		}
	}
}
