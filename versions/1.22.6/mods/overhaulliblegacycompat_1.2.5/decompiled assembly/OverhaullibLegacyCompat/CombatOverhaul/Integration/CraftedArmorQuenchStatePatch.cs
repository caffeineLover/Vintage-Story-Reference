using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace CombatOverhaul.Integration
{
	// Token: 0x0200004D RID: 77
	[NullableContext(1)]
	[Nullable(0)]
	[HarmonyPatch(typeof(CollectibleObject), "OnCreatedByCrafting")]
	internal static class CraftedArmorQuenchStatePatch
	{
		// Token: 0x06000497 RID: 1175 RVA: 0x0001AA8F File Offset: 0x00018C8F
		[HarmonyPrepare]
		private static bool Prepare()
		{
			return QuenchablePatchGate.Prepare();
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x0001AA98 File Offset: 0x00018C98
		private static void Postfix(ItemSlot[] allInputSlots, ItemSlot outputSlot)
		{
			if (outputSlot.Empty || outputSlot.Itemstack == null)
			{
				return;
			}
			bool outputWasClayCovered = outputSlot.Itemstack.Attributes.GetBool("clayCovered", false);
			int? inheritedState = default(int?);
			bool? inheritedDirect = default(bool?);
			int? inheritedClayQuenchIteration = default(int?);
			int? inheritedClayTemperIteration = default(int?);
			foreach (ItemSlot slot in allInputSlots)
			{
				if (!slot.Empty && QuenchableStateUtil.IsArmorOrArmorComponent(slot.Itemstack) && QuenchableStateUtil.IsFerrous(slot.Itemstack))
				{
					int state = QuenchableStateUtil.GetArmorQuenchState(slot.Itemstack);
					int num = inheritedState.GetValueOrDefault();
					if (inheritedState == null)
					{
						num = state;
						inheritedState = new int?(num);
					}
					if (inheritedState.Value != state)
					{
						return;
					}
					bool direct = QuenchableStateUtil.HasDirectArmorQuench(slot.Itemstack);
					int clayQuenchIteration = (QuenchableStateUtil.GetArmorQuenchMode(slot.Itemstack) == "clay") ? slot.Itemstack.Attributes.GetInt("quenchIteration", 0) : 0;
					int clayTemperIteration = (QuenchableStateUtil.GetArmorQuenchMode(slot.Itemstack) == "clay") ? slot.Itemstack.Attributes.GetInt("temperIteration", 0) : 0;
					bool flag = inheritedDirect.GetValueOrDefault();
					if (inheritedDirect == null)
					{
						flag = direct;
						inheritedDirect = new bool?(flag);
					}
					num = inheritedClayQuenchIteration.GetValueOrDefault();
					if (inheritedClayQuenchIteration == null)
					{
						num = clayQuenchIteration;
						inheritedClayQuenchIteration = new int?(num);
					}
					num = inheritedClayTemperIteration.GetValueOrDefault();
					if (inheritedClayTemperIteration == null)
					{
						num = clayTemperIteration;
						inheritedClayTemperIteration = new int?(num);
					}
					if (inheritedDirect.Value != direct || inheritedClayQuenchIteration.Value != clayQuenchIteration || inheritedClayTemperIteration.Value != clayTemperIteration)
					{
						return;
					}
				}
			}
			if (inheritedState != null && QuenchableStateUtil.IsArmorOrArmorComponent(outputSlot.Itemstack) && (inheritedState.Value > 0 || inheritedDirect.GetValueOrDefault() || inheritedClayQuenchIteration.GetValueOrDefault() > 0))
			{
				QuenchableStateUtil.ApplyInheritedArmorQuenchState(outputSlot.Itemstack, inheritedDirect.GetValueOrDefault(), inheritedClayQuenchIteration.GetValueOrDefault(), inheritedClayTemperIteration.GetValueOrDefault());
			}
			if (outputWasClayCovered)
			{
				outputSlot.Itemstack.Attributes.SetBool("clayCovered", true);
			}
			CraftedArmorQuenchStatePatch.NormalizeCraftedWeaponQuenchState(allInputSlots, outputSlot.Itemstack);
			QuenchableStateUtil.NormalizeGenericBuffs(outputSlot.Itemstack);
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x0001ACF4 File Offset: 0x00018EF4
		private static void NormalizeCraftedWeaponQuenchState(ItemSlot[] allInputSlots, ItemStack output)
		{
			if (output.Collectible == null || QuenchableStateUtil.IsArmorOrArmorComponent(output) || !QuenchableStateUtil.IsFerrous(output) || QuenchableStateUtil.GetKind(output) != "weapon")
			{
				return;
			}
			int bestQuenchIteration = 0;
			int bestTemperIteration = 0;
			float bestPowerValue = 0f;
			float bestDurationBonus = 0f;
			foreach (ItemSlot input in allInputSlots)
			{
				if (!input.Empty)
				{
					ItemStack itemstack = input.Itemstack;
					if (((itemstack != null) ? itemstack.Collectible : null) != null && QuenchableStateUtil.IsFerrous(input.Itemstack) && !(QuenchableStateUtil.GetKind(input.Itemstack) != "weapon"))
					{
						int quenchIteration = Math.Max(0, input.Itemstack.Attributes.GetInt("quenchIteration", 0));
						int temperIteration = Math.Clamp(input.Itemstack.Attributes.GetInt("temperIteration", 0), 0, quenchIteration);
						float powerValue = Math.Max(0f, input.Itemstack.Attributes.GetFloat("powervalue", 0f));
						float durationBonus = Math.Max(0f, input.Itemstack.Attributes.GetFloat("durationbonus", 0f));
						bestDurationBonus = Math.Max(bestDurationBonus, durationBonus);
						if (quenchIteration > bestQuenchIteration || (quenchIteration == bestQuenchIteration && temperIteration > bestTemperIteration) || (quenchIteration == bestQuenchIteration && temperIteration == bestTemperIteration && powerValue > bestPowerValue))
						{
							bestQuenchIteration = quenchIteration;
							bestTemperIteration = temperIteration;
							bestPowerValue = powerValue;
						}
					}
				}
			}
			if (bestQuenchIteration <= 0 && bestPowerValue <= 0f && bestDurationBonus <= 0f)
			{
				return;
			}
			int finalQuenchIteration = bestQuenchIteration;
			int finalTemperIteration = Math.Clamp(bestTemperIteration, 0, bestQuenchIteration);
			float normalizedPowerValue = (finalQuenchIteration > 0) ? QuenchableStatUtil.GetWeaponQuenchDamageBonus(finalQuenchIteration, finalTemperIteration) : bestPowerValue;
			output.Attributes.SetInt("quenchIteration", finalQuenchIteration);
			output.Attributes.SetInt("temperIteration", finalTemperIteration);
			output.Attributes.SetFloat("powervalue", Math.Max(0f, normalizedPowerValue));
			if (bestDurationBonus > 0f)
			{
				output.Attributes.SetFloat("durationbonus", bestDurationBonus);
			}
			else
			{
				output.Attributes.RemoveAttribute("durationbonus");
			}
			CraftedArmorQuenchStatePatch.NormalizeVisibleWeaponQuenchBuffs(output, normalizedPowerValue, bestDurationBonus);
			CraftedArmorQuenchStatePatch.NormalizeCraftedWeaponDurability(output);
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x0001AF1C File Offset: 0x0001911C
		internal static bool NormalizeWeaponQuenchDamageBuff(ItemStack output)
		{
			if (((output != null) ? output.Collectible : null) == null || QuenchableStateUtil.GetKind(output) != "weapon")
			{
				return false;
			}
			int storedQuenchIteration = Math.Max(0, output.Attributes.GetInt("quenchIteration", 0));
			int quenchIteration = QuenchableStatUtil.GetWeaponQuenchIteration(output);
			if (quenchIteration <= 0)
			{
				return false;
			}
			int temperIteration = Math.Clamp(output.Attributes.GetInt("temperIteration", 0), 0, quenchIteration);
			float powerValue = QuenchableStatUtil.GetWeaponQuenchDamageBonus(quenchIteration, temperIteration);
			float durationBonus = Math.Max(0f, output.Attributes.GetFloat("durationbonus", 0f));
			bool result = storedQuenchIteration != quenchIteration || Math.Abs(output.Attributes.GetFloat("powervalue", 0f) - powerValue) > 0.0001f || output.Attributes.GetInt("temperIteration", 0) != temperIteration;
			output.Attributes.SetInt("quenchIteration", quenchIteration);
			output.Attributes.SetInt("temperIteration", temperIteration);
			output.Attributes.SetFloat("powervalue", Math.Max(0f, powerValue));
			CraftedArmorQuenchStatePatch.NormalizeVisibleWeaponQuenchBuffs(output, powerValue, durationBonus);
			return result;
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x0001B03C File Offset: 0x0001923C
		private static void NormalizeVisibleWeaponQuenchBuffs(ItemStack output, float powerValue, float durationBonus)
		{
			QuenchableStateUtil.RemoveBuffs(output, new string[]
			{
				"attackpower",
				"miningspeed",
				"maxdurability"
			});
			CollectibleObject collectible = output.Collectible;
			CollectibleBehaviorBuffable behavior = (collectible != null) ? collectible.GetBehavior<CollectibleBehaviorBuffable>() : null;
			if (behavior == null)
			{
				return;
			}
			if (powerValue > 0f)
			{
				behavior.AddBuff(output, new AppliedCollectibleBuff
				{
					Code = "hardened",
					Multiplier = 1f + powerValue,
					StatCode = "attackpower"
				}, 1);
			}
			if (durationBonus > 0f)
			{
				behavior.AddBuff(output, new AppliedCollectibleBuff
				{
					Code = "hardened",
					Multiplier = 1f + durationBonus,
					StatCode = "maxdurability"
				}, 1);
			}
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x0001B0F4 File Offset: 0x000192F4
		private static void NormalizeCraftedWeaponDurability(ItemStack output)
		{
			if (output.Collectible == null || !output.Attributes.HasAttribute("durability"))
			{
				return;
			}
			int maxDurability = output.Collectible.GetMaxDurability(output);
			if (maxDurability <= 0)
			{
				return;
			}
			int currentDurability = output.Collectible.GetRemainingDurability(output);
			if (currentDurability <= 0 || currentDurability > maxDurability)
			{
				output.Attributes.RemoveAttribute("durability");
			}
		}
	}
}
