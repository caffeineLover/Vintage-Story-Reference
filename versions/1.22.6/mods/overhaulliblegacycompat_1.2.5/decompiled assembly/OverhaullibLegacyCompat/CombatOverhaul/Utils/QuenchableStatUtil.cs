using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.DamageSystems;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace CombatOverhaul.Utils
{
	// Token: 0x0200002C RID: 44
	[NullableContext(2)]
	[Nullable(0)]
	public static class QuenchableStatUtil
	{
		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060002FF RID: 767 RVA: 0x0000DA3C File Offset: 0x0000BC3C
		// (set) Token: 0x06000300 RID: 768 RVA: 0x0000DA43 File Offset: 0x0000BC43
		public static float WeaponDamageMultiplier { get; set; } = 1f;

		// Token: 0x06000301 RID: 769 RVA: 0x0000DA4C File Offset: 0x0000BC4C
		public static float GetAttackPowerMultiplier(ItemStack stack)
		{
			if (((stack != null) ? stack.Collectible : null) == null || QuenchableStateUtil.GetKind(stack) == "armor")
			{
				return 1f;
			}
			if (QuenchableStatUtil.GetWeaponQuenchIteration(stack) > 0)
			{
				return QuenchableStatUtil.GetVanillaStateAttackPowerMultiplier(stack);
			}
			float buffMultiplier = QuenchableStatUtil.GetBuffMultiplier(stack, "attackpower");
			float powerValueMultiplier = 1f + Math.Max(0f, stack.Attributes.GetFloat("powervalue", 0f));
			float vanillaStateMultiplier = QuenchableStatUtil.GetVanillaStateAttackPowerMultiplier(stack);
			return Math.Max(Math.Max(buffMultiplier, powerValueMultiplier), vanillaStateMultiplier);
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000DAD4 File Offset: 0x0000BCD4
		[NullableContext(1)]
		public static DamageResistData ApplyArmorProtection([Nullable(2)] ItemStack stack, DamageResistData resists)
		{
			QuenchableStateUtil.SanitizeArmorQuenchBuffs(stack);
			float flatReductionBonus = QuenchableStatUtil.GetArmorFlatReductionBonus(stack);
			if (flatReductionBonus <= 0f)
			{
				return resists;
			}
			return new DamageResistData(Enumerable.ToDictionary<KeyValuePair<EnumDamageType, float>, EnumDamageType, float>(resists.Resists, (KeyValuePair<EnumDamageType, float> entry) => entry.Key, (KeyValuePair<EnumDamageType, float> entry) => entry.Value), Enumerable.ToDictionary<KeyValuePair<EnumDamageType, float>, EnumDamageType, float>(resists.FlatDamageReduction, (KeyValuePair<EnumDamageType, float> entry) => entry.Key, (KeyValuePair<EnumDamageType, float> entry) => entry.Value + flatReductionBonus), resists.DirectionsCoverage);
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000DB92 File Offset: 0x0000BD92
		private static int GetArmorProtectionLevelBonus(ItemStack stack)
		{
			if (((stack != null) ? stack.Collectible : null) == null || !QuenchableStateUtil.IsFerrous(stack))
			{
				return 0;
			}
			return QuenchableStateUtil.GetArmorQuenchState(stack);
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000DBB4 File Offset: 0x0000BDB4
		private static float GetArmorFlatReductionBonus(ItemStack stack)
		{
			if (((stack != null) ? stack.Collectible : null) == null || !QuenchableStateUtil.IsFerrous(stack))
			{
				return 0f;
			}
			if (QuenchableStateUtil.HasDirectArmorQuench(stack))
			{
				return 0.2f;
			}
			if (QuenchableStateUtil.GetArmorQuenchMode(stack) == "direct" && QuenchableStateUtil.GetArmorQuenchState(stack) > 0)
			{
				return 0.2f;
			}
			return 0f;
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0000DC14 File Offset: 0x0000BE14
		public static float GetArmorPenaltyMultiplier(ItemStack stack)
		{
			QuenchableStateUtil.SanitizeArmorQuenchBuffs(stack);
			if (((stack != null) ? stack.Collectible : null) == null || !QuenchableStateUtil.IsFerrous(stack))
			{
				return 1f;
			}
			string mode = QuenchableStateUtil.GetArmorQuenchMode(stack);
			bool isClayPath = mode == "clay";
			if (!isClayPath && string.IsNullOrEmpty(mode) && stack.Attributes.GetInt("quenchIteration", 0) > 0 && !QuenchableStateUtil.HasDirectArmorQuench(stack))
			{
				isClayPath = true;
			}
			if (!isClayPath)
			{
				return 1f;
			}
			int quenchIteration = stack.Attributes.GetInt("quenchIteration", 0);
			if (quenchIteration <= 0)
			{
				return 1f;
			}
			float effectiveQuench = (float)quenchIteration * QuenchableStatUtil.GetTemperPowerFactor(stack);
			return Math.Max(1f - effectiveQuench * 0.1f, 0.5f);
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000DCC8 File Offset: 0x0000BEC8
		public static float GetTemperPowerFactor(ItemStack stack)
		{
			if (((stack != null) ? stack.Attributes : null) == null)
			{
				return 1f;
			}
			int temperIteration = Math.Max(0, stack.Attributes.GetInt("temperIteration", 0));
			if (temperIteration <= 0)
			{
				return 1f;
			}
			return MathF.Pow(0.92f, (float)temperIteration);
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000DD18 File Offset: 0x0000BF18
		[NullableContext(1)]
		private static float GetBuffMultiplier(ItemStack stack, string statCode)
		{
			ITreeAttribute buffs = stack.Attributes.GetTreeAttribute("buffs");
			if (buffs == null)
			{
				return 1f;
			}
			float multiplier = 1f;
			IAttribute[] values = buffs.Values;
			for (int i = 0; i < values.Length; i++)
			{
				ITreeAttribute buff = values[i] as ITreeAttribute;
				if (buff != null && string.Equals(buff.GetString("statcode", ""), statCode, 5))
				{
					multiplier *= buff.GetFloat("multiplier", 1f);
				}
			}
			return multiplier;
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000DD98 File Offset: 0x0000BF98
		[NullableContext(1)]
		private static float GetVanillaStateAttackPowerMultiplier(ItemStack stack)
		{
			int quenchIteration = QuenchableStatUtil.GetWeaponQuenchIteration(stack);
			if (quenchIteration <= 0)
			{
				return 1f;
			}
			int temperIteration = Math.Max(0, stack.Attributes.GetInt("temperIteration", 0));
			float effectivePower = QuenchableStatUtil.GetWeaponQuenchDamageBonus(quenchIteration, temperIteration);
			return 1f + Math.Max(0f, effectivePower);
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000DDE8 File Offset: 0x0000BFE8
		public static int GetWeaponQuenchIteration(ItemStack stack)
		{
			if (((stack != null) ? stack.Attributes : null) == null)
			{
				return 0;
			}
			int quenchIteration = Math.Max(0, stack.Attributes.GetInt("quenchIteration", 0));
			if (quenchIteration <= 0)
			{
				return QuenchableStatUtil.InferLegacyWeaponQuenchIteration(stack);
			}
			return quenchIteration;
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000DE2C File Offset: 0x0000C02C
		[NullableContext(1)]
		private static int InferLegacyWeaponQuenchIteration(ItemStack stack)
		{
			float storedPower = Math.Max(0f, stack.Attributes.GetFloat("powervalue", 0f));
			if (storedPower <= 0f)
			{
				return 0;
			}
			int temperIteration = Math.Max(0, stack.Attributes.GetInt("temperIteration", 0));
			float oldTemperFactor = MathF.Pow(0.92f, (float)temperIteration);
			if (oldTemperFactor <= 0f)
			{
				return 0;
			}
			return Math.Clamp((int)MathF.Round(storedPower / (0.1f * oldTemperFactor)), 0, 100);
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000DEAC File Offset: 0x0000C0AC
		public static float GetWeaponQuenchDamageBonus(int quenchIteration, int temperIteration)
		{
			int quenches = Math.Max(0, quenchIteration);
			if (quenches <= 0)
			{
				return 0f;
			}
			float power = 0f;
			for (int i = 0; i < quenches; i++)
			{
				power += 0.1f / (1f + (float)i * 0.2f);
			}
			int tempers = Math.Clamp(temperIteration, 0, quenches);
			for (int j = 0; j < tempers; j++)
			{
				float temperWeight = 1f / (1f + (float)j * 0.05f);
				float temperFactor = 1f + -0.07999998f * temperWeight;
				power *= temperFactor;
			}
			return power * Math.Max(0f, QuenchableStatUtil.WeaponDamageMultiplier);
		}

		// Token: 0x04000122 RID: 290
		private const bool UseLegacyArmorProtectionLevelBonus = false;

		// Token: 0x04000123 RID: 291
		private const float ArmorFlatReductionPerQuench = 0.2f;

		// Token: 0x04000124 RID: 292
		public const float TemperShatterChanceMultiplier = 0.8f;

		// Token: 0x04000125 RID: 293
		public const float TemperPowerMultiplier = 0.92f;

		// Token: 0x04000126 RID: 294
		private const float VanillaWeaponPowerPerFirstQuench = 0.1f;

		// Token: 0x04000127 RID: 295
		private const float VanillaWeaponPowerDiminishingFactor = 0.2f;

		// Token: 0x04000128 RID: 296
		private const float VanillaTemperDiminishingFactor = 0.05f;
	}
}
