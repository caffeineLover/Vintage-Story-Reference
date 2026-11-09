using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using CombatOverhaul.WeaponBuffs;
using Vintagestory.API.Common;

namespace CombatOverhaul.Implementations
{
	// Token: 0x02000088 RID: 136
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct ItemStackMeleeWeaponStats
	{
		// Token: 0x06000737 RID: 1847 RVA: 0x0002AC84 File Offset: 0x00028E84
		public ItemStackMeleeWeaponStats(float damageMultiplier, float damageBonus, int damageTierBonus, float attackSpeed, int blockTierBonus, int parryTierBonus, float thrownDamageMultiplier, int thrownDamageTierBonus, float thrownAimingDifficulty, float thrownProjectileSpeedMultiplier, float knockbackMultiplier, int armorPiercingBonus)
		{
			this.DamageMultiplier = damageMultiplier;
			this.DamageBonus = damageBonus;
			this.DamageTierBonus = damageTierBonus;
			this.AttackSpeed = attackSpeed;
			this.BlockTierBonus = blockTierBonus;
			this.ParryTierBonus = parryTierBonus;
			this.ThrownDamageMultiplier = thrownDamageMultiplier;
			this.ThrownDamageTierBonus = thrownDamageTierBonus;
			this.ThrownAimingDifficulty = thrownAimingDifficulty;
			this.ThrownProjectileSpeedMultiplier = thrownProjectileSpeedMultiplier;
			this.KnockbackMultiplier = knockbackMultiplier;
			this.ArmorPiercingBonus = armorPiercingBonus;
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x0002ACF0 File Offset: 0x00028EF0
		public ItemStackMeleeWeaponStats()
		{
			this.ArmorPiercingBonus = 0;
			this.DamageMultiplier = 1f;
			this.DamageBonus = 0f;
			this.DamageTierBonus = 0;
			this.AttackSpeed = 1f;
			this.BlockTierBonus = 0;
			this.ParryTierBonus = 0;
			this.ThrownDamageMultiplier = 1f;
			this.ThrownDamageTierBonus = 0;
			this.ThrownAimingDifficulty = 1f;
			this.ThrownProjectileSpeedMultiplier = 1f;
			this.KnockbackMultiplier = 1f;
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x0002AD70 File Offset: 0x00028F70
		public static ItemStackMeleeWeaponStats FromItemStack(ItemStack stack)
		{
			SplitMaterialWeaponUtil.Normalize(stack);
			float damageMultiplier = stack.Attributes.GetFloat("damageMultiplier", 1f) * QuenchableStatUtil.GetAttackPowerMultiplier(stack);
			float damageBonus = stack.Attributes.GetFloat("damageBonus", 0f);
			int damageTierBonus = stack.Attributes.GetInt("damageTierBonus", 0);
			float attackSpeed = stack.Attributes.GetFloat("attackSpeed", 1f);
			int blockTierBonus = stack.Attributes.GetInt("blockTierBonus", 0);
			int parryTierBonus = stack.Attributes.GetInt("parryTierBonus", 0);
			float thrownDamageMultiplier = stack.Attributes.GetFloat("thrownDamageMultiplier", 1f) * QuenchableStatUtil.GetAttackPowerMultiplier(stack);
			int thrownDamageTierBonus = stack.Attributes.GetInt("thrownDamageTierBonus", 0);
			float thrownAimingDifficulty = stack.Attributes.GetFloat("thrownAimingDifficulty", 1f);
			float thrownProjectileSpeedMultiplier = stack.Attributes.GetFloat("thrownProjectileSpeedMultiplier", 1f);
			float knockbackMultiplier = stack.Attributes.GetFloat("knockbackMultiplier", 1f);
			int armorPiercingBonus = stack.Attributes.GetInt("armorPiercingBonus", 0);
			return WeaponBuffSystem.ComposeMeleeStats(stack, new WeaponBuffMeleeStats
			{
				DamageMultiplier = damageMultiplier,
				DamageBonus = damageBonus,
				DamageTierBonus = damageTierBonus,
				AttackSpeed = attackSpeed,
				BlockTierBonus = blockTierBonus,
				ParryTierBonus = parryTierBonus,
				ThrownDamageMultiplier = thrownDamageMultiplier,
				ThrownDamageTierBonus = thrownDamageTierBonus,
				ThrownAimingDifficulty = thrownAimingDifficulty,
				ThrownProjectileSpeedMultiplier = thrownProjectileSpeedMultiplier,
				KnockbackMultiplier = knockbackMultiplier,
				ArmorPiercingBonus = armorPiercingBonus
			});
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x0002AEF5 File Offset: 0x000290F5
		public static float GetAttackSpeed(ItemStack stack)
		{
			return ItemStackMeleeWeaponStats.FromItemStack(stack).AttackSpeed;
		}

		// Token: 0x04000329 RID: 809
		public readonly float DamageMultiplier;

		// Token: 0x0400032A RID: 810
		public readonly float DamageBonus;

		// Token: 0x0400032B RID: 811
		public readonly int DamageTierBonus;

		// Token: 0x0400032C RID: 812
		public readonly float AttackSpeed;

		// Token: 0x0400032D RID: 813
		public readonly int BlockTierBonus;

		// Token: 0x0400032E RID: 814
		public readonly int ParryTierBonus;

		// Token: 0x0400032F RID: 815
		public readonly float ThrownDamageMultiplier;

		// Token: 0x04000330 RID: 816
		public readonly int ThrownDamageTierBonus;

		// Token: 0x04000331 RID: 817
		public readonly float ThrownAimingDifficulty;

		// Token: 0x04000332 RID: 818
		public readonly float ThrownProjectileSpeedMultiplier;

		// Token: 0x04000333 RID: 819
		public readonly float KnockbackMultiplier;

		// Token: 0x04000334 RID: 820
		public readonly int ArmorPiercingBonus;
	}
}
