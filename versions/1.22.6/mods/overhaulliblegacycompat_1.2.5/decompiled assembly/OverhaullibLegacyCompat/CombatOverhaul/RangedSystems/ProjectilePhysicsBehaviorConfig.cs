using System;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000BF RID: 191
	public class ProjectilePhysicsBehaviorConfig
	{
		// Token: 0x1700023D RID: 573
		// (get) Token: 0x060009EE RID: 2542 RVA: 0x000357D9 File Offset: 0x000339D9
		// (set) Token: 0x060009EF RID: 2543 RVA: 0x000357E1 File Offset: 0x000339E1
		public double ColliderRadius { get; set; }

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x060009F0 RID: 2544 RVA: 0x000357EA File Offset: 0x000339EA
		// (set) Token: 0x060009F1 RID: 2545 RVA: 0x000357F2 File Offset: 0x000339F2
		public bool CanRicochet { get; set; } = true;

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x060009F2 RID: 2546 RVA: 0x000357FB File Offset: 0x000339FB
		// (set) Token: 0x060009F3 RID: 2547 RVA: 0x00035803 File Offset: 0x00033A03
		public float MinSpeedToRicochet { get; set; } = 0.5f;

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x060009F4 RID: 2548 RVA: 0x0003580C File Offset: 0x00033A0C
		// (set) Token: 0x060009F5 RID: 2549 RVA: 0x00035814 File Offset: 0x00033A14
		public float RicochetSpeedFactor { get; set; } = 0.5f;

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x060009F6 RID: 2550 RVA: 0x0003581D File Offset: 0x00033A1D
		// (set) Token: 0x060009F7 RID: 2551 RVA: 0x00035825 File Offset: 0x00033A25
		public float RicochetNormalSpeedFactor { get; set; } = 0.5f;

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x060009F8 RID: 2552 RVA: 0x0003582E File Offset: 0x00033A2E
		// (set) Token: 0x060009F9 RID: 2553 RVA: 0x00035836 File Offset: 0x00033A36
		public float MaxRicochetAngleDeg { get; set; } = 5f;
	}
}
