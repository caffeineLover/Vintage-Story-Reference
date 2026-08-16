using System;
using System.Runtime.CompilerServices;
using System.Text;
using HarmonyLib;
using Vintagestory.API.Common;

namespace CombatOverhaul.Integration
{
	// Token: 0x02000050 RID: 80
	[HarmonyPatch(typeof(CollectibleObject), "GetHeldItemInfo")]
	internal static class ArmorQuenchCollectibleTooltipPatch
	{
		// Token: 0x060004A3 RID: 1187 RVA: 0x0001B279 File Offset: 0x00019479
		[HarmonyPrepare]
		private static bool Prepare()
		{
			return QuenchablePatchGate.Prepare();
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x0001B280 File Offset: 0x00019480
		[NullableContext(1)]
		private static void Postfix(ItemSlot inSlot, StringBuilder dsc)
		{
			WeaponQuenchTooltipPatch.NormalizeTooltip(inSlot, dsc);
			ArmorQuenchTooltipPatch.AppendTooltip(inSlot, dsc);
		}
	}
}
