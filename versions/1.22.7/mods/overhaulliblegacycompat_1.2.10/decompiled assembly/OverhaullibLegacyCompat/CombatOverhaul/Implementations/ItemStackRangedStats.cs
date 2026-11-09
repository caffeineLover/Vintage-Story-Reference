using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.WeaponBuffs;
using Vintagestory.API.Common;

namespace CombatOverhaul.Implementations
{
	// Token: 0x0200009B RID: 155
	public readonly struct ItemStackRangedStats
	{
		// Token: 0x06000851 RID: 2129 RVA: 0x0003050B File Offset: 0x0002E70B
		public ItemStackRangedStats(float reloadSpeed, float damageMultiplier, int damageTierBonus, float projectileSpeed, float dispersionMultiplier, float aimingDifficulty)
		{
			this.ReloadSpeed = reloadSpeed;
			this.DamageMultiplier = damageMultiplier;
			this.DamageTierBonus = damageTierBonus;
			this.ProjectileSpeed = projectileSpeed;
			this.DispersionMultiplier = dispersionMultiplier;
			this.AimingDifficulty = aimingDifficulty;
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x0003053A File Offset: 0x0002E73A
		public ItemStackRangedStats()
		{
			this.ReloadSpeed = 1f;
			this.DamageMultiplier = 1f;
			this.DamageTierBonus = 0;
			this.ProjectileSpeed = 1f;
			this.DispersionMultiplier = 1f;
			this.AimingDifficulty = 0f;
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x0003057C File Offset: 0x0002E77C
		[NullableContext(1)]
		public static ItemStackRangedStats FromItemStack(ItemStack stack)
		{
			float reloadSpeed = stack.Attributes.GetFloat("reloadSpeed", 1f);
			float damageMultiplier = stack.Attributes.GetFloat("damageMultiplier", 1f);
			int damageTierBonus = stack.Attributes.GetInt("damageTierBonus", 0);
			float projectileSpeed = stack.Attributes.GetFloat("projectileSpeed", 1f);
			float dispersionMultiplier = stack.Attributes.GetFloat("dispersionMultiplier", 1f);
			float aimingDifficulty = stack.Attributes.GetFloat("aimingDifficulty", 1f);
			return WeaponBuffSystem.ComposeRangedStats(stack, new WeaponBuffRangedStats
			{
				ReloadSpeed = reloadSpeed,
				DamageMultiplier = damageMultiplier,
				DamageTierBonus = damageTierBonus,
				ProjectileSpeed = projectileSpeed,
				DispersionMultiplier = dispersionMultiplier,
				AimingDifficulty = aimingDifficulty
			});
		}

		// Token: 0x040003C7 RID: 967
		public readonly float ReloadSpeed;

		// Token: 0x040003C8 RID: 968
		public readonly float DamageMultiplier;

		// Token: 0x040003C9 RID: 969
		public readonly int DamageTierBonus;

		// Token: 0x040003CA RID: 970
		public readonly float ProjectileSpeed;

		// Token: 0x040003CB RID: 971
		public readonly float DispersionMultiplier;

		// Token: 0x040003CC RID: 972
		public readonly float AimingDifficulty;
	}
}
