using System;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000C3 RID: 195
	public sealed class CurvedFlightConfig
	{
		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000A1B RID: 2587 RVA: 0x000363B6 File Offset: 0x000345B6
		// (set) Token: 0x06000A1C RID: 2588 RVA: 0x000363BE File Offset: 0x000345BE
		public float HorizontalTurnRate { get; set; }

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000A1D RID: 2589 RVA: 0x000363C7 File Offset: 0x000345C7
		// (set) Token: 0x06000A1E RID: 2590 RVA: 0x000363CF File Offset: 0x000345CF
		public int Direction { get; set; } = 1;

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000A1F RID: 2591 RVA: 0x000363D8 File Offset: 0x000345D8
		// (set) Token: 0x06000A20 RID: 2592 RVA: 0x000363E0 File Offset: 0x000345E0
		public float StartAfterSeconds { get; set; }

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000A21 RID: 2593 RVA: 0x000363E9 File Offset: 0x000345E9
		// (set) Token: 0x06000A22 RID: 2594 RVA: 0x000363F1 File Offset: 0x000345F1
		public float ReturnAfterSeconds { get; set; } = -1f;

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000A23 RID: 2595 RVA: 0x000363FA File Offset: 0x000345FA
		// (set) Token: 0x06000A24 RID: 2596 RVA: 0x00036402 File Offset: 0x00034602
		public float ReturnAcceleration { get; set; } = 12f;

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000A25 RID: 2597 RVA: 0x0003640B File Offset: 0x0003460B
		// (set) Token: 0x06000A26 RID: 2598 RVA: 0x00036413 File Offset: 0x00034613
		public float MaximumReturnSpeed { get; set; }

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000A27 RID: 2599 RVA: 0x0003641C File Offset: 0x0003461C
		// (set) Token: 0x06000A28 RID: 2600 RVA: 0x00036424 File Offset: 0x00034624
		public float CatchRadius { get; set; } = 1.2f;

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000A29 RID: 2601 RVA: 0x0003642D File Offset: 0x0003462D
		// (set) Token: 0x06000A2A RID: 2602 RVA: 0x00036435 File Offset: 0x00034635
		public float ReturnTargetHeight { get; set; } = 1.2f;
	}
}
