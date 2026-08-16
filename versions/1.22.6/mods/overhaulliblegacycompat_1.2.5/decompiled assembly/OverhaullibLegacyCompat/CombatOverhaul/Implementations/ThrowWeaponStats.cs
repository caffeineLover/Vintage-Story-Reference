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
		// (get) Token: 0x06000705 RID: 1797 RVA: 0x0002A9ED File Offset: 0x00028BED
		// (set) Token: 0x06000706 RID: 1798 RVA: 0x0002A9F5 File Offset: 0x00028BF5
		public string AimAnimation { get; set; } = "";

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000707 RID: 1799 RVA: 0x0002A9FE File Offset: 0x00028BFE
		// (set) Token: 0x06000708 RID: 1800 RVA: 0x0002AA06 File Offset: 0x00028C06
		public string ThrowAnimation { get; set; } = "";

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000709 RID: 1801 RVA: 0x0002AA0F File Offset: 0x00028C0F
		// (set) Token: 0x0600070A RID: 1802 RVA: 0x0002AA17 File Offset: 0x00028C17
		public string TpAimAnimation { get; set; } = "";

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x0600070B RID: 1803 RVA: 0x0002AA20 File Offset: 0x00028C20
		// (set) Token: 0x0600070C RID: 1804 RVA: 0x0002AA28 File Offset: 0x00028C28
		public string TpThrowAnimation { get; set; } = "";

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x0600070D RID: 1805 RVA: 0x0002AA31 File Offset: 0x00028C31
		// (set) Token: 0x0600070E RID: 1806 RVA: 0x0002AA39 File Offset: 0x00028C39
		public AimingStatsJson Aiming { get; set; } = new AimingStatsJson();

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x0600070F RID: 1807 RVA: 0x0002AA42 File Offset: 0x00028C42
		// (set) Token: 0x06000710 RID: 1808 RVA: 0x0002AA4A File Offset: 0x00028C4A
		public int DamageTier { get; set; }

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000711 RID: 1809 RVA: 0x0002AA53 File Offset: 0x00028C53
		// (set) Token: 0x06000712 RID: 1810 RVA: 0x0002AA5B File Offset: 0x00028C5B
		public float Knockback { get; set; }

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000713 RID: 1811 RVA: 0x0002AA64 File Offset: 0x00028C64
		// (set) Token: 0x06000714 RID: 1812 RVA: 0x0002AA6C File Offset: 0x00028C6C
		public int DurabilityDamage { get; set; } = 1;

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000715 RID: 1813 RVA: 0x0002AA75 File Offset: 0x00028C75
		// (set) Token: 0x06000716 RID: 1814 RVA: 0x0002AA7D File Offset: 0x00028C7D
		public float Velocity { get; set; } = 1f;

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000717 RID: 1815 RVA: 0x0002AA86 File Offset: 0x00028C86
		// (set) Token: 0x06000718 RID: 1816 RVA: 0x0002AA8E File Offset: 0x00028C8E
		public float Zeroing { get; set; } = 1.5f;
	}
}
