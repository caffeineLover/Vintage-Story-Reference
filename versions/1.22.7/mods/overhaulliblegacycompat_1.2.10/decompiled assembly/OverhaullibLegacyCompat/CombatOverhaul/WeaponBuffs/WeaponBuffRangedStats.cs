using System;
using CombatOverhaul.Implementations;

namespace CombatOverhaul.WeaponBuffs
{
	// Token: 0x020000AA RID: 170
	public sealed class WeaponBuffRangedStats
	{
		// Token: 0x1700020B RID: 523
		// (get) Token: 0x060008EC RID: 2284 RVA: 0x00030E7A File Offset: 0x0002F07A
		// (set) Token: 0x060008ED RID: 2285 RVA: 0x00030E82 File Offset: 0x0002F082
		public float ReloadSpeed { get; set; }

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x060008EE RID: 2286 RVA: 0x00030E8B File Offset: 0x0002F08B
		// (set) Token: 0x060008EF RID: 2287 RVA: 0x00030E93 File Offset: 0x0002F093
		public float DamageMultiplier { get; set; }

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x060008F0 RID: 2288 RVA: 0x00030E9C File Offset: 0x0002F09C
		// (set) Token: 0x060008F1 RID: 2289 RVA: 0x00030EA4 File Offset: 0x0002F0A4
		public int DamageTierBonus { get; set; }

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x060008F2 RID: 2290 RVA: 0x00030EAD File Offset: 0x0002F0AD
		// (set) Token: 0x060008F3 RID: 2291 RVA: 0x00030EB5 File Offset: 0x0002F0B5
		public float ProjectileSpeed { get; set; }

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x060008F4 RID: 2292 RVA: 0x00030EBE File Offset: 0x0002F0BE
		// (set) Token: 0x060008F5 RID: 2293 RVA: 0x00030EC6 File Offset: 0x0002F0C6
		public float DispersionMultiplier { get; set; }

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x060008F6 RID: 2294 RVA: 0x00030ECF File Offset: 0x0002F0CF
		// (set) Token: 0x060008F7 RID: 2295 RVA: 0x00030ED7 File Offset: 0x0002F0D7
		public float AimingDifficulty { get; set; }

		// Token: 0x060008F8 RID: 2296 RVA: 0x00030EE0 File Offset: 0x0002F0E0
		public ItemStackRangedStats ToReadonly()
		{
			return new ItemStackRangedStats(this.ReloadSpeed, this.DamageMultiplier, this.DamageTierBonus, this.ProjectileSpeed, this.DispersionMultiplier, this.AimingDifficulty);
		}
	}
}
