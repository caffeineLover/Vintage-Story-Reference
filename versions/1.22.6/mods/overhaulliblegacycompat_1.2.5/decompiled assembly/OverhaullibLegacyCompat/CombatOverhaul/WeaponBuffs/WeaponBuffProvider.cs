using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.RangedSystems;
using Vintagestory.API.Common;

namespace CombatOverhaul.WeaponBuffs
{
	// Token: 0x020000AD RID: 173
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class WeaponBuffProvider : IWeaponBuffProvider
	{
		// Token: 0x06000912 RID: 2322 RVA: 0x00030F74 File Offset: 0x0002F174
		public virtual bool Handles(WeaponBuffQueryContext context, WeaponBuffInstance buff)
		{
			return true;
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x00030F77 File Offset: 0x0002F177
		public virtual void ModifyMeleeStats(WeaponBuffQueryContext context, WeaponBuffInstance buff, WeaponBuffMeleeStats stats)
		{
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x00030F79 File Offset: 0x0002F179
		public virtual void ModifyRangedStats(WeaponBuffQueryContext context, WeaponBuffInstance buff, WeaponBuffRangedStats stats)
		{
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x00030F7B File Offset: 0x0002F17B
		public virtual void ModifyProjectileStackStats(WeaponBuffQueryContext context, WeaponBuffInstance buff, WeaponBuffProjectileStats stats)
		{
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x00030F7D File Offset: 0x0002F17D
		public virtual void ModifyProjectileStats(WeaponBuffQueryContext context, WeaponBuffInstance buff, ProjectileStats stats)
		{
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x00030F7F File Offset: 0x0002F17F
		public virtual void ModifyProjectileSpawn(WeaponBuffProjectileSpawnContext context, WeaponBuffInstance buff)
		{
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x00030F81 File Offset: 0x0002F181
		public virtual void ModifyMeleeDamage(WeaponBuffDamageContext context, WeaponBuffInstance buff)
		{
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x00030F83 File Offset: 0x0002F183
		public virtual void ModifyRangedDamage(WeaponBuffDamageContext context, WeaponBuffInstance buff)
		{
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x00030F85 File Offset: 0x0002F185
		public virtual void AppendTooltip(WeaponBuffTooltipContext context, WeaponBuffInstance buff)
		{
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x00030F87 File Offset: 0x0002F187
		public virtual void OnConsumed(ItemStack stack, WeaponBuffInstance buff, WeaponBuffConsumptionTrigger trigger)
		{
		}
	}
}
