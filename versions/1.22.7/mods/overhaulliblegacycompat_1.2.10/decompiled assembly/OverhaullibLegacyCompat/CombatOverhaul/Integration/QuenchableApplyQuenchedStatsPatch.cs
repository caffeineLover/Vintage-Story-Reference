using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace CombatOverhaul.Integration
{
	// Token: 0x0200004A RID: 74
	[NullableContext(1)]
	[Nullable(0)]
	[HarmonyPatch(typeof(CollectibleBehaviorQuenchable), "applyQuenchedStats")]
	internal static class QuenchableApplyQuenchedStatsPatch
	{
		// Token: 0x0600048D RID: 1165 RVA: 0x0001A766 File Offset: 0x00018966
		[HarmonyPrepare]
		private static bool Prepare()
		{
			return QuenchablePatchGate.Prepare();
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x0001A770 File Offset: 0x00018970
		private static bool Prefix(CollectibleBehaviorQuenchable __instance, IWorldAccessor world, ItemStack itemstack)
		{
			if (!QuenchableStateUtil.IsArmorOrArmorComponent(itemstack) || !QuenchableStateUtil.IsFerrous(itemstack))
			{
				return true;
			}
			if (itemstack.Attributes.GetBool("clayCovered", false))
			{
				QuenchableStateUtil.ApplyClayArmorQuench(itemstack);
				QuenchableApplyQuenchedStatsPatch.SetArmorShatterChance(__instance, world, itemstack);
				return false;
			}
			if (QuenchableStateUtil.HasDirectArmorQuench(itemstack))
			{
				return false;
			}
			QuenchableStateUtil.ApplyDirectArmorQuench(itemstack);
			QuenchableApplyQuenchedStatsPatch.SetArmorShatterChance(__instance, world, itemstack);
			return false;
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x0001A7CB File Offset: 0x000189CB
		private static void Postfix(ItemStack itemstack)
		{
			QuenchableStateUtil.NormalizeGenericBuffs(itemstack);
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x0001A7D4 File Offset: 0x000189D4
		internal static void SetArmorShatterChance(CollectibleBehaviorQuenchable behavior, IWorldAccessor world, ItemStack itemstack)
		{
			int quenchIteration = itemstack.Attributes.GetInt("quenchIteration", 0);
			int temperIteration = itemstack.Attributes.GetInt("temperIteration", 0);
			float temperedChance = (0.05f + (float)quenchIteration * behavior.BreakChancePerQuench) * MathF.Pow(0.8f, (float)Math.Max(0, temperIteration));
			behavior.SetShatterChance(world, itemstack, Math.Max(0f, temperedChance));
		}
	}
}
