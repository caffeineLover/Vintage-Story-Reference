using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.RangedSystems;
using Vintagestory.API.Common;

namespace CombatOverhaul.WeaponBuffs
{
	// Token: 0x020000AC RID: 172
	[NullableContext(1)]
	public interface IWeaponBuffProvider
	{
		// Token: 0x06000908 RID: 2312
		bool Handles(WeaponBuffQueryContext context, WeaponBuffInstance buff);

		// Token: 0x06000909 RID: 2313
		void ModifyMeleeStats(WeaponBuffQueryContext context, WeaponBuffInstance buff, WeaponBuffMeleeStats stats);

		// Token: 0x0600090A RID: 2314
		void ModifyRangedStats(WeaponBuffQueryContext context, WeaponBuffInstance buff, WeaponBuffRangedStats stats);

		// Token: 0x0600090B RID: 2315
		void ModifyProjectileStackStats(WeaponBuffQueryContext context, WeaponBuffInstance buff, WeaponBuffProjectileStats stats);

		// Token: 0x0600090C RID: 2316
		void ModifyProjectileStats(WeaponBuffQueryContext context, WeaponBuffInstance buff, ProjectileStats stats);

		// Token: 0x0600090D RID: 2317
		void ModifyProjectileSpawn(WeaponBuffProjectileSpawnContext context, WeaponBuffInstance buff);

		// Token: 0x0600090E RID: 2318
		void ModifyMeleeDamage(WeaponBuffDamageContext context, WeaponBuffInstance buff);

		// Token: 0x0600090F RID: 2319
		void ModifyRangedDamage(WeaponBuffDamageContext context, WeaponBuffInstance buff);

		// Token: 0x06000910 RID: 2320
		void AppendTooltip(WeaponBuffTooltipContext context, WeaponBuffInstance buff);

		// Token: 0x06000911 RID: 2321
		void OnConsumed(ItemStack stack, WeaponBuffInstance buff, WeaponBuffConsumptionTrigger trigger);
	}
}
