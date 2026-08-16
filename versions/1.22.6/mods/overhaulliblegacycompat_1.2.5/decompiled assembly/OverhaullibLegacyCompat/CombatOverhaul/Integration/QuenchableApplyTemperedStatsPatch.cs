using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace CombatOverhaul.Integration
{
	// Token: 0x0200004B RID: 75
	[NullableContext(1)]
	[Nullable(0)]
	[HarmonyPatch(typeof(CollectibleBehaviorQuenchable), "applyTemperedStats")]
	internal static class QuenchableApplyTemperedStatsPatch
	{
		// Token: 0x06000491 RID: 1169 RVA: 0x0001A803 File Offset: 0x00018A03
		[HarmonyPrepare]
		private static bool Prepare()
		{
			return QuenchablePatchGate.Prepare();
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x0001A80C File Offset: 0x00018A0C
		private static bool Prefix(CollectibleBehaviorQuenchable __instance, ItemStack itemstack, object[] __args)
		{
			if (!QuenchableStateUtil.HasAnyQuenchState(itemstack))
			{
				return false;
			}
			if (!QuenchableStateUtil.IsArmorOrArmorComponent(itemstack) || !QuenchableStateUtil.IsFerrous(itemstack))
			{
				return true;
			}
			int clayQuenchIteration = Math.Max(0, itemstack.Attributes.GetInt("quenchIteration", 0));
			int temperIteration = Math.Max(0, itemstack.Attributes.GetInt("temperIteration", 0));
			if (clayQuenchIteration <= 0 || temperIteration >= clayQuenchIteration)
			{
				return false;
			}
			itemstack.Attributes.SetInt("temperIteration", temperIteration + 1);
			IWorldAccessor world = QuenchableApplyTemperedStatsPatch.TryGetWorld(__args);
			if (world != null)
			{
				QuenchableApplyQuenchedStatsPatch.SetArmorShatterChance(__instance, world, itemstack);
			}
			return false;
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x0001A895 File Offset: 0x00018A95
		private static void Postfix(ItemStack itemstack)
		{
			QuenchableStateUtil.NormalizeGenericBuffs(itemstack);
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x0001A8A0 File Offset: 0x00018AA0
		[NullableContext(2)]
		private static IWorldAccessor TryGetWorld([Nullable(new byte[]
		{
			2,
			1
		})] object[] args)
		{
			if (args == null)
			{
				return null;
			}
			for (int i = 0; i < args.Length; i++)
			{
				IWorldAccessor world = args[i] as IWorldAccessor;
				if (world != null)
				{
					return world;
				}
			}
			return null;
		}
	}
}
