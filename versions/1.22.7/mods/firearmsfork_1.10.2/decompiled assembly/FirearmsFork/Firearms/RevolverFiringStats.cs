using System;
using System.Runtime.CompilerServices;

namespace Firearms
{
	// Token: 0x0200002C RID: 44
	[NullableContext(1)]
	[Nullable(0)]
	public class RevolverFiringStats
	{
		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000203 RID: 515 RVA: 0x0000A29B File Offset: 0x0000849B
		// (set) Token: 0x06000204 RID: 516 RVA: 0x0000A2A3 File Offset: 0x000084A3
		public float[] DispersionMOA { get; set; } = new float[2];

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000205 RID: 517 RVA: 0x0000A2AC File Offset: 0x000084AC
		// (set) Token: 0x06000206 RID: 518 RVA: 0x0000A2B4 File Offset: 0x000084B4
		public float BulletDamageMultiplier { get; set; } = 1f;

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000207 RID: 519 RVA: 0x0000A2BD File Offset: 0x000084BD
		// (set) Token: 0x06000208 RID: 520 RVA: 0x0000A2C5 File Offset: 0x000084C5
		public int BulletDamageTier { get; set; } = 1;

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000209 RID: 521 RVA: 0x0000A2CE File Offset: 0x000084CE
		// (set) Token: 0x0600020A RID: 522 RVA: 0x0000A2D6 File Offset: 0x000084D6
		public float BulletVelocity { get; set; } = 1f;

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600020B RID: 523 RVA: 0x0000A2DF File Offset: 0x000084DF
		// (set) Token: 0x0600020C RID: 524 RVA: 0x0000A2E7 File Offset: 0x000084E7
		public float Zeroing { get; set; }
	}
}
