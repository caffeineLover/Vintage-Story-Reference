using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.RangedSystems.Aiming;

namespace CombatOverhaul.Implementations
{
	// Token: 0x02000074 RID: 116
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BowStats : WeaponStats
	{
		// Token: 0x170000FF RID: 255
		// (get) Token: 0x0600055E RID: 1374 RVA: 0x0001F881 File Offset: 0x0001DA81
		// (set) Token: 0x0600055F RID: 1375 RVA: 0x0001F889 File Offset: 0x0001DA89
		public string LoadAnimation { get; set; } = "";

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000560 RID: 1376 RVA: 0x0001F892 File Offset: 0x0001DA92
		// (set) Token: 0x06000561 RID: 1377 RVA: 0x0001F89A File Offset: 0x0001DA9A
		public string DrawAnimation { get; set; } = "";

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000562 RID: 1378 RVA: 0x0001F8A3 File Offset: 0x0001DAA3
		// (set) Token: 0x06000563 RID: 1379 RVA: 0x0001F8AB File Offset: 0x0001DAAB
		public string DrawAfterLoadAnimation { get; set; } = "";

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000564 RID: 1380 RVA: 0x0001F8B4 File Offset: 0x0001DAB4
		// (set) Token: 0x06000565 RID: 1381 RVA: 0x0001F8BC File Offset: 0x0001DABC
		public string ReleaseAnimation { get; set; } = "";

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000566 RID: 1382 RVA: 0x0001F8C5 File Offset: 0x0001DAC5
		// (set) Token: 0x06000567 RID: 1383 RVA: 0x0001F8CD File Offset: 0x0001DACD
		public string TpAimAnimation { get; set; } = "";

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000568 RID: 1384 RVA: 0x0001F8D6 File Offset: 0x0001DAD6
		// (set) Token: 0x06000569 RID: 1385 RVA: 0x0001F8DE File Offset: 0x0001DADE
		public AimingStatsJson Aiming { get; set; } = new AimingStatsJson();

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x0600056A RID: 1386 RVA: 0x0001F8E7 File Offset: 0x0001DAE7
		// (set) Token: 0x0600056B RID: 1387 RVA: 0x0001F8EF File Offset: 0x0001DAEF
		public float ArrowDamageMultiplier { get; set; } = 1f;

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x0600056C RID: 1388 RVA: 0x0001F8F8 File Offset: 0x0001DAF8
		// (set) Token: 0x0600056D RID: 1389 RVA: 0x0001F900 File Offset: 0x0001DB00
		public int ArrowDamageTier { get; set; } = 1;

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x0600056E RID: 1390 RVA: 0x0001F909 File Offset: 0x0001DB09
		// (set) Token: 0x0600056F RID: 1391 RVA: 0x0001F911 File Offset: 0x0001DB11
		public float ArrowVelocity { get; set; } = 1f;

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000570 RID: 1392 RVA: 0x0001F91A File Offset: 0x0001DB1A
		// (set) Token: 0x06000571 RID: 1393 RVA: 0x0001F922 File Offset: 0x0001DB22
		public string ArrowWildcard { get; set; } = "*arrow-*";

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000572 RID: 1394 RVA: 0x0001F92B File Offset: 0x0001DB2B
		// (set) Token: 0x06000573 RID: 1395 RVA: 0x0001F933 File Offset: 0x0001DB33
		public float Zeroing { get; set; } = 1.5f;

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000574 RID: 1396 RVA: 0x0001F93C File Offset: 0x0001DB3C
		// (set) Token: 0x06000575 RID: 1397 RVA: 0x0001F944 File Offset: 0x0001DB44
		public float[] DispersionMOA { get; set; } = new float[2];

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000576 RID: 1398 RVA: 0x0001F94D File Offset: 0x0001DB4D
		// (set) Token: 0x06000577 RID: 1399 RVA: 0x0001F955 File Offset: 0x0001DB55
		public float ScreenShakeStrength { get; set; } = 0.2f;

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000578 RID: 1400 RVA: 0x0001F95E File Offset: 0x0001DB5E
		// (set) Token: 0x06000579 RID: 1401 RVA: 0x0001F966 File Offset: 0x0001DB66
		public bool TwoHanded { get; set; } = true;

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600057A RID: 1402 RVA: 0x0001F96F File Offset: 0x0001DB6F
		// (set) Token: 0x0600057B RID: 1403 RVA: 0x0001F977 File Offset: 0x0001DB77
		public float ReloadAnimationSpeed { get; set; } = 1f;
	}
}
