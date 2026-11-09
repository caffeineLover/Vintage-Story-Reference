using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.WeaponBuffs;
using Vintagestory.API.Common;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000CD RID: 205
	public readonly struct ItemStackProjectileStats
	{
		// Token: 0x06000A80 RID: 2688 RVA: 0x0003721B File Offset: 0x0003541B
		public ItemStackProjectileStats()
		{
			this.DamageMultiplier = 1f;
			this.DamageTierBonus = 0;
			this.KnockbackMultiplier = 1f;
			this.DropChanceMultiplier = 1f;
			this.PenetrationBonus = 0f;
			this.AdditionalDurabilityCost = 0;
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x00037257 File Offset: 0x00035457
		public ItemStackProjectileStats(float damageMultiplier, int damageTierBonus, float knockbackMultiplier, float dropChanceMultiplier, float penetrationBonus, int additionalDurabilityCost)
		{
			this.DamageMultiplier = damageMultiplier;
			this.DamageTierBonus = damageTierBonus;
			this.KnockbackMultiplier = knockbackMultiplier;
			this.DropChanceMultiplier = dropChanceMultiplier;
			this.PenetrationBonus = penetrationBonus;
			this.AdditionalDurabilityCost = additionalDurabilityCost;
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x00037288 File Offset: 0x00035488
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

		// Token: 0x040004D4 RID: 1236
		public readonly float DamageMultiplier;

		// Token: 0x040004D5 RID: 1237
		public readonly int DamageTierBonus;

		// Token: 0x040004D6 RID: 1238
		public readonly float KnockbackMultiplier;

		// Token: 0x040004D7 RID: 1239
		public readonly float DropChanceMultiplier;

		// Token: 0x040004D8 RID: 1240
		public readonly float PenetrationBonus;

		// Token: 0x040004D9 RID: 1241
		public readonly int AdditionalDurabilityCost;
	}
}
