using System;
using CombatOverhaul.Implementations;

namespace CombatOverhaul.WeaponBuffs
{
	// Token: 0x020000A9 RID: 169
	public sealed class WeaponBuffMeleeStats
	{
		// Token: 0x170001FF RID: 511
		// (get) Token: 0x060008D2 RID: 2258 RVA: 0x00030D4C File Offset: 0x0002EF4C
		// (set) Token: 0x060008D3 RID: 2259 RVA: 0x00030D54 File Offset: 0x0002EF54
		public float DamageMultiplier { get; set; }

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x060008D4 RID: 2260 RVA: 0x00030D5D File Offset: 0x0002EF5D
		// (set) Token: 0x060008D5 RID: 2261 RVA: 0x00030D65 File Offset: 0x0002EF65
		public float DamageBonus { get; set; }

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x060008D6 RID: 2262 RVA: 0x00030D6E File Offset: 0x0002EF6E
		// (set) Token: 0x060008D7 RID: 2263 RVA: 0x00030D76 File Offset: 0x0002EF76
		public int DamageTierBonus { get; set; }

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x060008D8 RID: 2264 RVA: 0x00030D7F File Offset: 0x0002EF7F
		// (set) Token: 0x060008D9 RID: 2265 RVA: 0x00030D87 File Offset: 0x0002EF87
		public float AttackSpeed { get; set; }

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x060008DA RID: 2266 RVA: 0x00030D90 File Offset: 0x0002EF90
		// (set) Token: 0x060008DB RID: 2267 RVA: 0x00030D98 File Offset: 0x0002EF98
		public int BlockTierBonus { get; set; }

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x060008DC RID: 2268 RVA: 0x00030DA1 File Offset: 0x0002EFA1
		// (set) Token: 0x060008DD RID: 2269 RVA: 0x00030DA9 File Offset: 0x0002EFA9
		public int ParryTierBonus { get; set; }

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x060008DE RID: 2270 RVA: 0x00030DB2 File Offset: 0x0002EFB2
		// (set) Token: 0x060008DF RID: 2271 RVA: 0x00030DBA File Offset: 0x0002EFBA
		public float ThrownDamageMultiplier { get; set; }

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x060008E0 RID: 2272 RVA: 0x00030DC3 File Offset: 0x0002EFC3
		// (set) Token: 0x060008E1 RID: 2273 RVA: 0x00030DCB File Offset: 0x0002EFCB
		public int ThrownDamageTierBonus { get; set; }

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x060008E2 RID: 2274 RVA: 0x00030DD4 File Offset: 0x0002EFD4
		// (set) Token: 0x060008E3 RID: 2275 RVA: 0x00030DDC File Offset: 0x0002EFDC
		public float ThrownAimingDifficulty { get; set; }

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x060008E4 RID: 2276 RVA: 0x00030DE5 File Offset: 0x0002EFE5
		// (set) Token: 0x060008E5 RID: 2277 RVA: 0x00030DED File Offset: 0x0002EFED
		public float ThrownProjectileSpeedMultiplier { get; set; }

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x060008E6 RID: 2278 RVA: 0x00030DF6 File Offset: 0x0002EFF6
		// (set) Token: 0x060008E7 RID: 2279 RVA: 0x00030DFE File Offset: 0x0002EFFE
		public float KnockbackMultiplier { get; set; }

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x060008E8 RID: 2280 RVA: 0x00030E07 File Offset: 0x0002F007
		// (set) Token: 0x060008E9 RID: 2281 RVA: 0x00030E0F File Offset: 0x0002F00F
		public int ArmorPiercingBonus { get; set; }

		// Token: 0x060008EA RID: 2282 RVA: 0x00030E18 File Offset: 0x0002F018
		public ItemStackMeleeWeaponStats ToReadonly()
		{
			return new ItemStackMeleeWeaponStats(this.DamageMultiplier, this.DamageBonus, this.DamageTierBonus, this.AttackSpeed, this.BlockTierBonus, this.ParryTierBonus, this.ThrownDamageMultiplier, this.ThrownDamageTierBonus, this.ThrownAimingDifficulty, this.ThrownProjectileSpeedMultiplier, this.KnockbackMultiplier, this.ArmorPiercingBonus);
		}
	}
}
