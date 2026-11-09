using System;
using System.Runtime.CompilerServices;

namespace Firearms
{
	// Token: 0x0200002B RID: 43
	[NullableContext(1)]
	[Nullable(0)]
	public class RevolverLoadStageStats
	{
		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060001D6 RID: 470 RVA: 0x0000A04C File Offset: 0x0000824C
		// (set) Token: 0x060001D7 RID: 471 RVA: 0x0000A054 File Offset: 0x00008254
		public string LoadedAnimation { get; set; } = "";

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060001D8 RID: 472 RVA: 0x0000A05D File Offset: 0x0000825D
		// (set) Token: 0x060001D9 RID: 473 RVA: 0x0000A065 File Offset: 0x00008265
		public string UnloadedAnimation { get; set; } = "";

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060001DA RID: 474 RVA: 0x0000A06E File Offset: 0x0000826E
		// (set) Token: 0x060001DB RID: 475 RVA: 0x0000A076 File Offset: 0x00008276
		public string LoadingAnimation { get; set; } = "";

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001DC RID: 476 RVA: 0x0000A07F File Offset: 0x0000827F
		// (set) Token: 0x060001DD RID: 477 RVA: 0x0000A087 File Offset: 0x00008287
		public string CockingAnimation { get; set; } = "";

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001DE RID: 478 RVA: 0x0000A090 File Offset: 0x00008290
		// (set) Token: 0x060001DF RID: 479 RVA: 0x0000A098 File Offset: 0x00008298
		public string CockedAnimation { get; set; } = "";

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x0000A0A1 File Offset: 0x000082A1
		// (set) Token: 0x060001E1 RID: 481 RVA: 0x0000A0A9 File Offset: 0x000082A9
		public string ShootAnimation { get; set; } = "";

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001E2 RID: 482 RVA: 0x0000A0B2 File Offset: 0x000082B2
		// (set) Token: 0x060001E3 RID: 483 RVA: 0x0000A0BA File Offset: 0x000082BA
		public string FiredAnimation { get; set; } = "";

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x0000A0C3 File Offset: 0x000082C3
		// (set) Token: 0x060001E5 RID: 485 RVA: 0x0000A0CB File Offset: 0x000082CB
		public string AimAnimation { get; set; } = "";

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x0000A0D4 File Offset: 0x000082D4
		// (set) Token: 0x060001E7 RID: 487 RVA: 0x0000A0DC File Offset: 0x000082DC
		public string StartLoadingAnimation { get; set; } = "";

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x0000A0E5 File Offset: 0x000082E5
		// (set) Token: 0x060001E9 RID: 489 RVA: 0x0000A0ED File Offset: 0x000082ED
		public string ContinueLoadingAnimation { get; set; } = "";

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060001EA RID: 490 RVA: 0x0000A0F6 File Offset: 0x000082F6
		// (set) Token: 0x060001EB RID: 491 RVA: 0x0000A0FE File Offset: 0x000082FE
		public string FinishLoadingAnimation { get; set; } = "";

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060001EC RID: 492 RVA: 0x0000A107 File Offset: 0x00008307
		// (set) Token: 0x060001ED RID: 493 RVA: 0x0000A10F File Offset: 0x0000830F
		public string CancelLoadingAnimation { get; set; } = "";

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060001EE RID: 494 RVA: 0x0000A118 File Offset: 0x00008318
		// (set) Token: 0x060001EF RID: 495 RVA: 0x0000A120 File Offset: 0x00008320
		public string CylinderPositionAnimation { get; set; } = "";

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x0000A129 File Offset: 0x00008329
		// (set) Token: 0x060001F1 RID: 497 RVA: 0x0000A131 File Offset: 0x00008331
		public int LoadToStage { get; set; }

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x0000A13A File Offset: 0x0000833A
		// (set) Token: 0x060001F3 RID: 499 RVA: 0x0000A142 File Offset: 0x00008342
		public int UnloadToStage { get; set; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x0000A14B File Offset: 0x0000834B
		// (set) Token: 0x060001F5 RID: 501 RVA: 0x0000A153 File Offset: 0x00008353
		public bool CanFire { get; set; } = true;

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x0000A15C File Offset: 0x0000835C
		// (set) Token: 0x060001F7 RID: 503 RVA: 0x0000A164 File Offset: 0x00008364
		public bool CanLoad { get; set; } = true;

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x0000A16D File Offset: 0x0000836D
		// (set) Token: 0x060001F9 RID: 505 RVA: 0x0000A175 File Offset: 0x00008375
		public int WaddingUsed { get; set; } = 1;

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060001FA RID: 506 RVA: 0x0000A17E File Offset: 0x0000837E
		// (set) Token: 0x060001FB RID: 507 RVA: 0x0000A186 File Offset: 0x00008386
		public int PowderConsumption { get; set; } = 1;

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060001FC RID: 508 RVA: 0x0000A18F File Offset: 0x0000838F
		// (set) Token: 0x060001FD RID: 509 RVA: 0x0000A197 File Offset: 0x00008397
		public int BulletsFired { get; set; } = 1;

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060001FE RID: 510 RVA: 0x0000A1A0 File Offset: 0x000083A0
		// (set) Token: 0x060001FF RID: 511 RVA: 0x0000A1A8 File Offset: 0x000083A8
		public int BulletLoaded { get; set; } = 1;

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000200 RID: 512 RVA: 0x0000A1B1 File Offset: 0x000083B1
		// (set) Token: 0x06000201 RID: 513 RVA: 0x0000A1B9 File Offset: 0x000083B9
		public float LoadSpeedPenalty { get; set; } = -2f;
	}
}
