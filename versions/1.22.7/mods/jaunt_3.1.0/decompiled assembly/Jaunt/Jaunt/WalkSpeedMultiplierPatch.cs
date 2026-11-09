using System;
using HarmonyLib;
using Jaunt.Behaviors;
using Vintagestory.API.Common;

namespace Jaunt
{
	// Token: 0x02000002 RID: 2
	[HarmonyPatch(typeof(EntityAgent))]
	[HarmonyPatch("GetWalkSpeedMultiplier")]
	public static class WalkSpeedMultiplierPatch
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static void Postfix(EntityAgent __instance, ref double __result)
		{
			EntityBehaviorJauntWalkspeed ebw = __instance.GetBehavior<EntityBehaviorJauntWalkspeed>();
			if (ebw == null)
			{
				return;
			}
			__result *= (double)ebw.ExtraSpeedModifier;
		}
	}
}
