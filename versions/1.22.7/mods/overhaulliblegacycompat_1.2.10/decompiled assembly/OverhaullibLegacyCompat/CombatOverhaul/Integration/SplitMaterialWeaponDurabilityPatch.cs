using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using HarmonyLib;
using Vintagestory.API.Common;

namespace CombatOverhaul.Integration
{
	// Token: 0x02000057 RID: 87
	[HarmonyPatch(typeof(CollectibleObject), "GetMaxDurability")]
	internal static class SplitMaterialWeaponDurabilityPatch
	{
		// Token: 0x060004B4 RID: 1204 RVA: 0x0001BE74 File Offset: 0x0001A074
		[NullableContext(1)]
		private static void Postfix(ItemStack itemstack, ref int __result)
		{
			int maxDurability;
			if (SplitMaterialWeaponUtil.TryGetBodyMaxDurability(itemstack, __result, out maxDurability))
			{
				__result = maxDurability;
			}
		}
	}
}
