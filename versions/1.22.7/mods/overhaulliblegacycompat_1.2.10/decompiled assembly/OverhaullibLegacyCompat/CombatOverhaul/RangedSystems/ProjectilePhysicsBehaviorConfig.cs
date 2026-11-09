using System;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000BF RID: 191
	public class ProjectilePhysicsBehaviorConfig
	{
		// Token: 0x1700023F RID: 575
		// (get) Token: 0x060009F2 RID: 2546 RVA: 0x00035975 File Offset: 0x00033B75
		// (set) Token: 0x060009F3 RID: 2547 RVA: 0x0003597D File Offset: 0x00033B7D
		public double ColliderRadius { get; set; }

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x060009F4 RID: 2548 RVA: 0x00035986 File Offset: 0x00033B86
		// (set) Token: 0x060009F5 RID: 2549 RVA: 0x0003598E File Offset: 0x00033B8E
		public bool CanRicochet { get; set; } = true;

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x060009F6 RID: 2550 RVA: 0x00035997 File Offset: 0x00033B97
		// (set) Token: 0x060009F7 RID: 2551 RVA: 0x0003599F File Offset: 0x00033B9F
		public float MinSpeedToRicochet { get; set; } = 0.5f;

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x060009F8 RID: 2552 RVA: 0x000359A8 File Offset: 0x00033BA8
		// (set) Token: 0x060009F9 RID: 2553 RVA: 0x000359B0 File Offset: 0x00033BB0
		public float RicochetSpeedFactor { get; set; } = 0.5f;

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x060009FA RID: 2554 RVA: 0x000359B9 File Offset: 0x00033BB9
		// (set) Token: 0x060009FB RID: 2555 RVA: 0x000359C1 File Offset: 0x00033BC1
		public float RicochetNormalSpeedFactor { get; set; } = 0.5f;

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x060009FC RID: 2556 RVA: 0x000359CA File Offset: 0x00033BCA
		// (set) Token: 0x060009FD RID: 2557 RVA: 0x000359D2 File Offset: 0x00033BD2
		public float MaxRicochetAngleDeg { get; set; } = 5f;
	}
}
