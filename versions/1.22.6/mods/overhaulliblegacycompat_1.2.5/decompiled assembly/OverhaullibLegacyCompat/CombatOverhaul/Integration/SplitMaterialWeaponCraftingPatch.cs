using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using HarmonyLib;
using Vintagestory.API.Common;

namespace CombatOverhaul.Integration
{
	// Token: 0x02000056 RID: 86
	[HarmonyPatch(typeof(CollectibleObject), "OnCreatedByCrafting")]
	internal static class SplitMaterialWeaponCraftingPatch
	{
		// Token: 0x060004B3 RID: 1203 RVA: 0x0001BE08 File Offset: 0x0001A008
		[NullableContext(1)]
		private static void Postfix(ItemSlot outputSlot)
		{
			if (outputSlot.Empty || outputSlot.Itemstack == null)
			{
				return;
			}
			bool flag = SplitMaterialWeaponUtil.Normalize(outputSlot.Itemstack);
			SplitMaterialWeaponUtil.NormalizeDurabilityIfNeeded(outputSlot.Itemstack);
			if (flag)
			{
				outputSlot.MarkDirty();
			}
		}
	}
}
