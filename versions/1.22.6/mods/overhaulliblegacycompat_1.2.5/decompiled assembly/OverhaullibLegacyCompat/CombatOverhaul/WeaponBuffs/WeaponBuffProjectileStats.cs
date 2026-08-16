using System;
using CombatOverhaul.RangedSystems;

namespace CombatOverhaul.WeaponBuffs
{
	// Token: 0x020000AB RID: 171
	public sealed class WeaponBuffProjectileStats
	{
		// Token: 0x17000211 RID: 529
		// (get) Token: 0x060008FA RID: 2298 RVA: 0x00030EDB File Offset: 0x0002F0DB
		// (set) Token: 0x060008FB RID: 2299 RVA: 0x00030EE3 File Offset: 0x0002F0E3
		public float DamageMultiplier { get; set; }

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x060008FC RID: 2300 RVA: 0x00030EEC File Offset: 0x0002F0EC
		// (set) Token: 0x060008FD RID: 2301 RVA: 0x00030EF4 File Offset: 0x0002F0F4
		public int DamageTierBonus { get; set; }

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x060008FE RID: 2302 RVA: 0x00030EFD File Offset: 0x0002F0FD
		// (set) Token: 0x060008FF RID: 2303 RVA: 0x00030F05 File Offset: 0x0002F105
		public float KnockbackMultiplier { get; set; }

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000900 RID: 2304 RVA: 0x00030F0E File Offset: 0x0002F10E
		// (set) Token: 0x06000901 RID: 2305 RVA: 0x00030F16 File Offset: 0x0002F116
		public float DropChanceMultiplier { get; set; }

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000902 RID: 2306 RVA: 0x00030F1F File Offset: 0x0002F11F
		// (set) Token: 0x06000903 RID: 2307 RVA: 0x00030F27 File Offset: 0x0002F127
		public float PenetrationBonus { get; set; }

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000904 RID: 2308 RVA: 0x00030F30 File Offset: 0x0002F130
		// (set) Token: 0x06000905 RID: 2309 RVA: 0x00030F38 File Offset: 0x0002F138
		public int AdditionalDurabilityCost { get; set; }

		// Token: 0x06000906 RID: 2310 RVA: 0x00030F41 File Offset: 0x0002F141
		public ItemStackProjectileStats ToReadonly()
		{
			return new ItemStackProjectileStats(this.DamageMultiplier, this.DamageTierBonus, this.KnockbackMultiplier, this.DropChanceMultiplier, this.PenetrationBonus, this.AdditionalDurabilityCost);
		}
	}
}
