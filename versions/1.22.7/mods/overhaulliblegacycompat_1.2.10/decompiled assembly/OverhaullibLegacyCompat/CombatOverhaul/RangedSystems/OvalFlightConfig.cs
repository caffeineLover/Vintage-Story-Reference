using System;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000C5 RID: 197
	public sealed class OvalFlightConfig
	{
		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000A34 RID: 2612 RVA: 0x00036699 File Offset: 0x00034899
		// (set) Token: 0x06000A35 RID: 2613 RVA: 0x000366A1 File Offset: 0x000348A1
		public float MaximumDistance { get; set; } = 8f;

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000A36 RID: 2614 RVA: 0x000366AA File Offset: 0x000348AA
		// (set) Token: 0x06000A37 RID: 2615 RVA: 0x000366B2 File Offset: 0x000348B2
		public float LateralRadius { get; set; } = 5f;

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000A38 RID: 2616 RVA: 0x000366BB File Offset: 0x000348BB
		// (set) Token: 0x06000A39 RID: 2617 RVA: 0x000366C3 File Offset: 0x000348C3
		public float FlightDurationSeconds { get; set; } = 1.2f;

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000A3A RID: 2618 RVA: 0x000366CC File Offset: 0x000348CC
		// (set) Token: 0x06000A3B RID: 2619 RVA: 0x000366D4 File Offset: 0x000348D4
		public int Direction { get; set; } = 1;

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000A3C RID: 2620 RVA: 0x000366DD File Offset: 0x000348DD
		// (set) Token: 0x06000A3D RID: 2621 RVA: 0x000366E5 File Offset: 0x000348E5
		public float CatchAfterProgress { get; set; } = 0.5f;

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000A3E RID: 2622 RVA: 0x000366EE File Offset: 0x000348EE
		// (set) Token: 0x06000A3F RID: 2623 RVA: 0x000366F6 File Offset: 0x000348F6
		public float CatchCollisionPadding { get; set; } = 0.1f;
	}
}
