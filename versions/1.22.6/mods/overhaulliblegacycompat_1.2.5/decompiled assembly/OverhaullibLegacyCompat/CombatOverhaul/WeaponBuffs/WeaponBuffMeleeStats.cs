using System;
using CombatOverhaul.Implementations;

namespace CombatOverhaul.WeaponBuffs
{
	// Token: 0x020000A9 RID: 169
	public sealed class WeaponBuffMeleeStats
	{
		// Token: 0x170001FF RID: 511
		// (get) Token: 0x060008D2 RID: 2258 RVA: 0x00030D14 File Offset: 0x0002EF14
		// (set) Token: 0x060008D3 RID: 2259 RVA: 0x00030D1C File Offset: 0x0002EF1C
		public float DamageMultiplier { get; set; }

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x060008D4 RID: 2260 RVA: 0x00030D25 File Offset: 0x0002EF25
		// (set) Token: 0x060008D5 RID: 2261 RVA: 0x00030D2D File Offset: 0x0002EF2D
		public float DamageBonus { get; set; }

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x060008D6 RID: 2262 RVA: 0x00030D36 File Offset: 0x0002EF36
		// (set) Token: 0x060008D7 RID: 2263 RVA: 0x00030D3E File Offset: 0x0002EF3E
		public int DamageTierBonus { get; set; }

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x060008D8 RID: 2264 RVA: 0x00030D47 File Offset: 0x0002EF47
		// (set) Token: 0x060008D9 RID: 2265 RVA: 0x00030D4F File Offset: 0x0002EF4F
		public float AttackSpeed { get; set; }

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x060008DA RID: 2266 RVA: 0x00030D58 File Offset: 0x0002EF58
		// (set) Token: 0x060008DB RID: 2267 RVA: 0x00030D60 File Offset: 0x0002EF60
		public int BlockTierBonus { get; set; }

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x060008DC RID: 2268 RVA: 0x00030D69 File Offset: 0x0002EF69
		// (set) Token: 0x060008DD RID: 2269 RVA: 0x00030D71 File Offset: 0x0002EF71
		public int ParryTierBonus { get; set; }

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x060008DE RID: 2270 RVA: 0x00030D7A File Offset: 0x0002EF7A
		// (set) Token: 0x060008DF RID: 2271 RVA: 0x00030D82 File Offset: 0x0002EF82
		public float ThrownDamageMultiplier { get; set; }

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x060008E0 RID: 2272 RVA: 0x00030D8B File Offset: 0x0002EF8B
		// (set) Token: 0x060008E1 RID: 2273 RVA: 0x00030D93 File Offset: 0x0002EF93
		public int ThrownDamageTierBonus { get; set; }

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x060008E2 RID: 2274 RVA: 0x00030D9C File Offset: 0x0002EF9C
		// (set) Token: 0x060008E3 RID: 2275 RVA: 0x00030DA4 File Offset: 0x0002EFA4
		public float ThrownAimingDifficulty { get; set; }

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x060008E4 RID: 2276 RVA: 0x00030DAD File Offset: 0x0002EFAD
		// (set) Token: 0x060008E5 RID: 2277 RVA: 0x00030DB5 File Offset: 0x0002EFB5
		public float ThrownProjectileSpeedMultiplier { get; set; }

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x060008E6 RID: 2278 RVA: 0x00030DBE File Offset: 0x0002EFBE
		// (set) Token: 0x060008E7 RID: 2279 RVA: 0x00030DC6 File Offset: 0x0002EFC6
		public float KnockbackMultiplier { get; set; }

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x060008E8 RID: 2280 RVA: 0x00030DCF File Offset: 0x0002EFCF
		// (set) Token: 0x060008E9 RID: 2281 RVA: 0x00030DD7 File Offset: 0x0002EFD7
		public int ArmorPiercingBonus { get; set; }

		// Token: 0x060008EA RID: 2282 RVA: 0x00030DE0 File Offset: 0x0002EFE0
		public ItemStackMeleeWeaponStats ToReadonly()
		{
			return new ItemStackMeleeWeaponStats(this.DamageMultiplier, this.DamageBonus, this.DamageTierBonus, this.AttackSpeed, this.BlockTierBonus, this.ParryTierBonus, this.ThrownDamageMultiplier, this.ThrownDamageTierBonus, this.ThrownAimingDifficulty, this.ThrownProjectileSpeedMultiplier, this.KnockbackMultiplier, this.ArmorPiercingBonus);
		}
	}
}
