using System;
using CombatOverhaul.Implementations;

namespace CombatOverhaul.WeaponBuffs
{
	// Token: 0x020000AA RID: 170
	public sealed class WeaponBuffRangedStats
	{
		// Token: 0x1700020B RID: 523
		// (get) Token: 0x060008EC RID: 2284 RVA: 0x00030E42 File Offset: 0x0002F042
		// (set) Token: 0x060008ED RID: 2285 RVA: 0x00030E4A File Offset: 0x0002F04A
		public float ReloadSpeed { get; set; }

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x060008EE RID: 2286 RVA: 0x00030E53 File Offset: 0x0002F053
		// (set) Token: 0x060008EF RID: 2287 RVA: 0x00030E5B File Offset: 0x0002F05B
		public float DamageMultiplier { get; set; }

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x060008F0 RID: 2288 RVA: 0x00030E64 File Offset: 0x0002F064
		// (set) Token: 0x060008F1 RID: 2289 RVA: 0x00030E6C File Offset: 0x0002F06C
		public int DamageTierBonus { get; set; }

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x060008F2 RID: 2290 RVA: 0x00030E75 File Offset: 0x0002F075
		// (set) Token: 0x060008F3 RID: 2291 RVA: 0x00030E7D File Offset: 0x0002F07D
		public float ProjectileSpeed { get; set; }

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x060008F4 RID: 2292 RVA: 0x00030E86 File Offset: 0x0002F086
		// (set) Token: 0x060008F5 RID: 2293 RVA: 0x00030E8E File Offset: 0x0002F08E
		public float DispersionMultiplier { get; set; }

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x060008F6 RID: 2294 RVA: 0x00030E97 File Offset: 0x0002F097
		// (set) Token: 0x060008F7 RID: 2295 RVA: 0x00030E9F File Offset: 0x0002F09F
		public float AimingDifficulty { get; set; }

		// Token: 0x060008F8 RID: 2296 RVA: 0x00030EA8 File Offset: 0x0002F0A8
		public ItemStackRangedStats ToReadonly()
		{
			return new ItemStackRangedStats(this.ReloadSpeed, this.DamageMultiplier, this.DamageTierBonus, this.ProjectileSpeed, this.DispersionMultiplier, this.AimingDifficulty);
		}
	}
}
