using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.WeaponBuffs;
using Vintagestory.API.Common;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000C2 RID: 194
	public readonly struct ItemStackProjectileStats
	{
		// Token: 0x06000A24 RID: 2596 RVA: 0x0003622B File Offset: 0x0003442B
		public ItemStackProjectileStats()
		{
			this.DamageMultiplier = 1f;
			this.DamageTierBonus = 0;
			this.KnockbackMultiplier = 1f;
			this.DropChanceMultiplier = 1f;
			this.PenetrationBonus = 0f;
			this.AdditionalDurabilityCost = 0;
		}

		// Token: 0x06000A25 RID: 2597 RVA: 0x00036267 File Offset: 0x00034467
		public ItemStackProjectileStats(float damageMultiplier, int damageTierBonus, float knockbackMultiplier, float dropChanceMultiplier, float penetrationBonus, int additionalDurabilityCost)
		{
			this.DamageMultiplier = damageMultiplier;
			this.DamageTierBonus = damageTierBonus;
			this.KnockbackMultiplier = knockbackMultiplier;
			this.DropChanceMultiplier = dropChanceMultiplier;
			this.PenetrationBonus = penetrationBonus;
			this.AdditionalDurabilityCost = additionalDurabilityCost;
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x00036298 File Offset: 0x00034498
		[NullableContext(1)]
		public static ItemStackProjectileStats FromItemStack(ItemStack stack)
		{
			float damageMultiplier = stack.Attributes.GetFloat("damageMultiplier", 1f);
			int damageTierBonus = stack.Attributes.GetInt("damageTierBonus", 0);
			float knockbackMultiplier = stack.Attributes.GetFloat("knockbackMultiplier", 1f);
			float dropChanceMultiplier = stack.Attributes.GetFloat("dropChanceMultiplier", 1f);
			float penetrationBonus = stack.Attributes.GetFloat("penetrationBonus", 0f);
			int additionalDurabilityCost = stack.Attributes.GetInt("additionalDurabilityCost", 0);
			return WeaponBuffSystem.ComposeProjectileStackStats(stack, new WeaponBuffProjectileStats
			{
				DamageMultiplier = damageMultiplier,
				DamageTierBonus = damageTierBonus,
				KnockbackMultiplier = knockbackMultiplier,
				DropChanceMultiplier = dropChanceMultiplier,
				PenetrationBonus = penetrationBonus,
				AdditionalDurabilityCost = additionalDurabilityCost
			});
		}

		// Token: 0x040004B2 RID: 1202
		public readonly float DamageMultiplier;

		// Token: 0x040004B3 RID: 1203
		public readonly int DamageTierBonus;

		// Token: 0x040004B4 RID: 1204
		public readonly float KnockbackMultiplier;

		// Token: 0x040004B5 RID: 1205
		public readonly float DropChanceMultiplier;

		// Token: 0x040004B6 RID: 1206
		public readonly float PenetrationBonus;

		// Token: 0x040004B7 RID: 1207
		public readonly int AdditionalDurabilityCost;
	}
}
