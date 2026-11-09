using System;
using System.Runtime.CompilerServices;
using System.Text;
using CombatOverhaul.Utils;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.GameContent;

namespace CombatOverhaul.Integration
{
	// Token: 0x0200004F RID: 79
	[NullableContext(1)]
	[Nullable(0)]
	[HarmonyPatch(typeof(CollectibleBehaviorQuenchable), "GetHeldItemInfo")]
	internal static class ArmorQuenchTooltipPatch
	{
		// Token: 0x0600049F RID: 1183 RVA: 0x0001B1C3 File Offset: 0x000193C3
		[HarmonyPrepare]
		private static bool Prepare()
		{
			return QuenchablePatchGate.Prepare();
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x0001B1CA File Offset: 0x000193CA
		private static void Postfix(ItemSlot inSlot, StringBuilder dsc)
		{
			ArmorQuenchTooltipPatch.AppendTooltip(inSlot, dsc);
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x0001B1D4 File Offset: 0x000193D4
		internal static void AppendTooltip(ItemSlot inSlot, StringBuilder dsc)
		{
			if (inSlot.Empty || !QuenchableStateUtil.IsArmorOrArmorComponent(inSlot.Itemstack) || !QuenchableStateUtil.IsFerrous(inSlot.Itemstack))
			{
				return;
			}
			if (QuenchableStateUtil.GetArmorQuenchState(inSlot.Itemstack) > 0)
			{
				ArmorQuenchTooltipPatch.AppendLineOnce(dsc, Lang.Get("combatoverhaul:quenchable-armor-durability-gain", Array.Empty<object>()));
			}
			if (QuenchableStateUtil.HasDirectArmorQuench(inSlot.Itemstack))
			{
				ArmorQuenchTooltipPatch.AppendLineOnce(dsc, Lang.Get("combatoverhaul:quenchable-armor-flat-reduction-gain", Array.Empty<object>()));
			}
			float penaltyMultiplier = QuenchableStatUtil.GetArmorPenaltyMultiplier(inSlot.Itemstack);
			float penaltyReduction = (1f - penaltyMultiplier) * 100f;
			if (penaltyReduction > 0f)
			{
				ArmorQuenchTooltipPatch.AppendLineOnce(dsc, Lang.Get("combatoverhaul:quenchable-armor-penalty-reduction", new object[]
				{
					MathF.Round(penaltyReduction)
				}));
			}
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x0001B28F File Offset: 0x0001948F
		private static void AppendLineOnce(StringBuilder dsc, string line)
		{
			if (string.IsNullOrWhiteSpace(line) || dsc.ToString().Contains(line, 4))
			{
				return;
			}
			dsc.AppendLine(line);
		}
	}
}
