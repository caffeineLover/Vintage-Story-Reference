using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x0200011C RID: 284
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DamageZoneStatsJson
	{
		// Token: 0x17000385 RID: 901
		// (get) Token: 0x06000DC1 RID: 3521 RVA: 0x00042724 File Offset: 0x00040924
		// (set) Token: 0x06000DC2 RID: 3522 RVA: 0x0004272C File Offset: 0x0004092C
		public string Zone { get; set; } = "None";

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06000DC3 RID: 3523 RVA: 0x00042735 File Offset: 0x00040935
		// (set) Token: 0x06000DC4 RID: 3524 RVA: 0x0004273D File Offset: 0x0004093D
		public float Coverage { get; set; }

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06000DC5 RID: 3525 RVA: 0x00042746 File Offset: 0x00040946
		// (set) Token: 0x06000DC6 RID: 3526 RVA: 0x0004274E File Offset: 0x0004094E
		public float Top { get; set; }

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06000DC7 RID: 3527 RVA: 0x00042757 File Offset: 0x00040957
		// (set) Token: 0x06000DC8 RID: 3528 RVA: 0x0004275F File Offset: 0x0004095F
		public float Bottom { get; set; }

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x06000DC9 RID: 3529 RVA: 0x00042768 File Offset: 0x00040968
		// (set) Token: 0x06000DCA RID: 3530 RVA: 0x00042770 File Offset: 0x00040970
		public float Left { get; set; }

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06000DCB RID: 3531 RVA: 0x00042779 File Offset: 0x00040979
		// (set) Token: 0x06000DCC RID: 3532 RVA: 0x00042781 File Offset: 0x00040981
		public float Right { get; set; }

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06000DCD RID: 3533 RVA: 0x0004278A File Offset: 0x0004098A
		// (set) Token: 0x06000DCE RID: 3534 RVA: 0x00042792 File Offset: 0x00040992
		public float DamageMultiplier { get; set; } = 1f;

		// Token: 0x06000DCF RID: 3535 RVA: 0x0004279B File Offset: 0x0004099B
		public DamageZoneStats ToStats()
		{
			return new DamageZoneStats(Enum.Parse<PlayerBodyPart>(this.Zone), this.Coverage, DirectionConstrain.FromDegrees(this.Top, this.Bottom, this.Right, this.Left), this.DamageMultiplier);
		}
	}
}
