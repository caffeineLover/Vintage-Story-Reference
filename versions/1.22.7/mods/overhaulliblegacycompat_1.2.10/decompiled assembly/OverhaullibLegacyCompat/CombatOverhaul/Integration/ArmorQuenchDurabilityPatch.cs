using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using HarmonyLib;
using Vintagestory.API.Common;

namespace CombatOverhaul.Integration
{
	// Token: 0x0200004E RID: 78
	[HarmonyPatch(typeof(CollectibleObject), "GetMaxDurability")]
	internal static class ArmorQuenchDurabilityPatch
	{
		// Token: 0x0600049D RID: 1181 RVA: 0x0001B18B File Offset: 0x0001938B
		[HarmonyPrepare]
		private static bool Prepare()
		{
			return QuenchablePatchGate.Prepare();
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x0001B192 File Offset: 0x00019392
		[NullableContext(1)]
		private static void Postfix(ItemStack itemstack, ref int __result)
		{
			if (__result <= 0 || !QuenchableStateUtil.IsArmorOrArmorComponent(itemstack))
			{
				return;
			}
			if (QuenchableStateUtil.GetArmorQuenchState(itemstack) <= 0)
			{
				return;
			}
			__result = Math.Max(1, (int)MathF.Round((float)__result * 1.1f));
		}
	}
}
