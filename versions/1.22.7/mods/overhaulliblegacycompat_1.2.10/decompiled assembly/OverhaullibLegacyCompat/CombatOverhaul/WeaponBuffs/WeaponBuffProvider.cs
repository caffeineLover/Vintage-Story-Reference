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
		// Token: 0x06000912 RID: 2322 RVA: 0x00030FAC File Offset: 0x0002F1AC
		public virtual bool Handles(WeaponBuffQueryContext context, WeaponBuffInstance buff)
		{
			return true;
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x00030FAF File Offset: 0x0002F1AF
		public virtual void ModifyMeleeStats(WeaponBuffQueryContext context, WeaponBuffInstance buff, WeaponBuffMeleeStats stats)
		{
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x00030FB1 File Offset: 0x0002F1B1
		public virtual void ModifyRangedStats(WeaponBuffQueryContext context, WeaponBuffInstance buff, WeaponBuffRangedStats stats)
		{
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x00030FB3 File Offset: 0x0002F1B3
		public virtual void ModifyProjectileStackStats(WeaponBuffQueryContext context, WeaponBuffInstance buff, WeaponBuffProjectileStats stats)
		{
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x00030FB5 File Offset: 0x0002F1B5
		public virtual void ModifyProjectileStats(WeaponBuffQueryContext context, WeaponBuffInstance buff, ProjectileStats stats)
		{
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x00030FB7 File Offset: 0x0002F1B7
		public virtual void ModifyProjectileSpawn(WeaponBuffProjectileSpawnContext context, WeaponBuffInstance buff)
		{
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x00030FB9 File Offset: 0x0002F1B9
		public virtual void ModifyMeleeDamage(WeaponBuffDamageContext context, WeaponBuffInstance buff)
		{
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x00030FBB File Offset: 0x0002F1BB
		public virtual void ModifyRangedDamage(WeaponBuffDamageContext context, WeaponBuffInstance buff)
		{
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x00030FBD File Offset: 0x0002F1BD
		public virtual void AppendTooltip(WeaponBuffTooltipContext context, WeaponBuffInstance buff)
		{
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x00030FBF File Offset: 0x0002F1BF
		public virtual void OnConsumed(ItemStack stack, WeaponBuffInstance buff, WeaponBuffConsumptionTrigger trigger)
		{
		}
	}
}
