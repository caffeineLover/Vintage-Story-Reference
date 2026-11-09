using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.RangedSystems.Aiming;

namespace CombatOverhaul.Implementations
{
	// Token: 0x02000084 RID: 132
	[NullableContext(1)]
	[Nullable(0)]
	public class ThrowWeaponStats
	{
		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000705 RID: 1797 RVA: 0x0002AA25 File Offset: 0x00028C25
		// (set) Token: 0x06000706 RID: 1798 RVA: 0x0002AA2D File Offset: 0x00028C2D
		public string AimAnimation { get; set; } = "";

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000707 RID: 1799 RVA: 0x0002AA36 File Offset: 0x00028C36
		// (set) Token: 0x06000708 RID: 1800 RVA: 0x0002AA3E File Offset: 0x00028C3E
		public string ThrowAnimation { get; set; } = "";

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000709 RID: 1801 RVA: 0x0002AA47 File Offset: 0x00028C47
		// (set) Token: 0x0600070A RID: 1802 RVA: 0x0002AA4F File Offset: 0x00028C4F
		public string TpAimAnimation { get; set; } = "";

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x0600070B RID: 1803 RVA: 0x0002AA58 File Offset: 0x00028C58
		// (set) Token: 0x0600070C RID: 1804 RVA: 0x0002AA60 File Offset: 0x00028C60
		public string TpThrowAnimation { get; set; } = "";

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x0600070D RID: 1805 RVA: 0x0002AA69 File Offset: 0x00028C69
		// (set) Token: 0x0600070E RID: 1806 RVA: 0x0002AA71 File Offset: 0x00028C71
		public AimingStatsJson Aiming { get; set; } = new AimingStatsJson();

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x0600070F RID: 1807 RVA: 0x0002AA7A File Offset: 0x00028C7A
		// (set) Token: 0x06000710 RID: 1808 RVA: 0x0002AA82 File Offset: 0x00028C82
		public int DamageTier { get; set; }

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000711 RID: 1809 RVA: 0x0002AA8B File Offset: 0x00028C8B
		// (set) Token: 0x06000712 RID: 1810 RVA: 0x0002AA93 File Offset: 0x00028C93
		public float Knockback { get; set; }

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000713 RID: 1811 RVA: 0x0002AA9C File Offset: 0x00028C9C
		// (set) Token: 0x06000714 RID: 1812 RVA: 0x0002AAA4 File Offset: 0x00028CA4
		public int DurabilityDamage { get; set; } = 1;

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000715 RID: 1813 RVA: 0x0002AAAD File Offset: 0x00028CAD
		// (set) Token: 0x06000716 RID: 1814 RVA: 0x0002AAB5 File Offset: 0x00028CB5
		public float Velocity { get; set; } = 1f;

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000717 RID: 1815 RVA: 0x0002AABE File Offset: 0x00028CBE
		// (set) Token: 0x06000718 RID: 1816 RVA: 0x0002AAC6 File Offset: 0x00028CC6
		public float Zeroing { get; set; } = 1.5f;
	}
}
