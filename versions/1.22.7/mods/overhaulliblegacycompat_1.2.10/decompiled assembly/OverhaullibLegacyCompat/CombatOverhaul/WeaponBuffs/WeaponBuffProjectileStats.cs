using System;
using CombatOverhaul.RangedSystems;

namespace CombatOverhaul.WeaponBuffs
{
	// Token: 0x020000AB RID: 171
	public sealed class WeaponBuffProjectileStats
	{
		// Token: 0x17000211 RID: 529
		// (get) Token: 0x060008FA RID: 2298 RVA: 0x00030F13 File Offset: 0x0002F113
		// (set) Token: 0x060008FB RID: 2299 RVA: 0x00030F1B File Offset: 0x0002F11B
		public float DamageMultiplier { get; set; }

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x060008FC RID: 2300 RVA: 0x00030F24 File Offset: 0x0002F124
		// (set) Token: 0x060008FD RID: 2301 RVA: 0x00030F2C File Offset: 0x0002F12C
		public int DamageTierBonus { get; set; }

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x060008FE RID: 2302 RVA: 0x00030F35 File Offset: 0x0002F135
		// (set) Token: 0x060008FF RID: 2303 RVA: 0x00030F3D File Offset: 0x0002F13D
		public float KnockbackMultiplier { get; set; }

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000900 RID: 2304 RVA: 0x00030F46 File Offset: 0x0002F146
		// (set) Token: 0x06000901 RID: 2305 RVA: 0x00030F4E File Offset: 0x0002F14E
		public float DropChanceMultiplier { get; set; }

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000902 RID: 2306 RVA: 0x00030F57 File Offset: 0x0002F157
		// (set) Token: 0x06000903 RID: 2307 RVA: 0x00030F5F File Offset: 0x0002F15F
		public float PenetrationBonus { get; set; }

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000904 RID: 2308 RVA: 0x00030F68 File Offset: 0x0002F168
		// (set) Token: 0x06000905 RID: 2309 RVA: 0x00030F70 File Offset: 0x0002F170
		public int AdditionalDurabilityCost { get; set; }

		// Token: 0x06000906 RID: 2310 RVA: 0x00030F79 File Offset: 0x0002F179
		public ItemStackProjectileStats ToReadonly()
		{
			return new ItemStackProjectileStats(this.DamageMultiplier, this.DamageTierBonus, this.KnockbackMultiplier, this.DropChanceMultiplier, this.PenetrationBonus, this.AdditionalDurabilityCost);
		}
	}
}
