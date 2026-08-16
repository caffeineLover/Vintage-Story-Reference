using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x02000111 RID: 273
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DamageZoneStatsJson
	{
		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06000D65 RID: 3429 RVA: 0x00041700 File Offset: 0x0003F900
		// (set) Token: 0x06000D66 RID: 3430 RVA: 0x00041708 File Offset: 0x0003F908
		public string Zone { get; set; } = "None";

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06000D67 RID: 3431 RVA: 0x00041711 File Offset: 0x0003F911
		// (set) Token: 0x06000D68 RID: 3432 RVA: 0x00041719 File Offset: 0x0003F919
		public float Coverage { get; set; }

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06000D69 RID: 3433 RVA: 0x00041722 File Offset: 0x0003F922
		// (set) Token: 0x06000D6A RID: 3434 RVA: 0x0004172A File Offset: 0x0003F92A
		public float Top { get; set; }

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06000D6B RID: 3435 RVA: 0x00041733 File Offset: 0x0003F933
		// (set) Token: 0x06000D6C RID: 3436 RVA: 0x0004173B File Offset: 0x0003F93B
		public float Bottom { get; set; }

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06000D6D RID: 3437 RVA: 0x00041744 File Offset: 0x0003F944
		// (set) Token: 0x06000D6E RID: 3438 RVA: 0x0004174C File Offset: 0x0003F94C
		public float Left { get; set; }

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06000D6F RID: 3439 RVA: 0x00041755 File Offset: 0x0003F955
		// (set) Token: 0x06000D70 RID: 3440 RVA: 0x0004175D File Offset: 0x0003F95D
		public float Right { get; set; }

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06000D71 RID: 3441 RVA: 0x00041766 File Offset: 0x0003F966
		// (set) Token: 0x06000D72 RID: 3442 RVA: 0x0004176E File Offset: 0x0003F96E
		public float DamageMultiplier { get; set; } = 1f;

		// Token: 0x06000D73 RID: 3443 RVA: 0x00041777 File Offset: 0x0003F977
		public DamageZoneStats ToStats()
		{
			return new DamageZoneStats(Enum.Parse<PlayerBodyPart>(this.Zone), this.Coverage, DirectionConstrain.FromDegrees(this.Top, this.Bottom, this.Right, this.Left), this.DamageMultiplier);
		}
	}
}
