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
		// (get) Token: 0x0600055E RID: 1374 RVA: 0x0001F8B9 File Offset: 0x0001DAB9
		// (set) Token: 0x0600055F RID: 1375 RVA: 0x0001F8C1 File Offset: 0x0001DAC1
		public string LoadAnimation { get; set; } = "";

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000560 RID: 1376 RVA: 0x0001F8CA File Offset: 0x0001DACA
		// (set) Token: 0x06000561 RID: 1377 RVA: 0x0001F8D2 File Offset: 0x0001DAD2
		public string DrawAnimation { get; set; } = "";

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000562 RID: 1378 RVA: 0x0001F8DB File Offset: 0x0001DADB
		// (set) Token: 0x06000563 RID: 1379 RVA: 0x0001F8E3 File Offset: 0x0001DAE3
		public string DrawAfterLoadAnimation { get; set; } = "";

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000564 RID: 1380 RVA: 0x0001F8EC File Offset: 0x0001DAEC
		// (set) Token: 0x06000565 RID: 1381 RVA: 0x0001F8F4 File Offset: 0x0001DAF4
		public string ReleaseAnimation { get; set; } = "";

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000566 RID: 1382 RVA: 0x0001F8FD File Offset: 0x0001DAFD
		// (set) Token: 0x06000567 RID: 1383 RVA: 0x0001F905 File Offset: 0x0001DB05
		public string TpAimAnimation { get; set; } = "";

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000568 RID: 1384 RVA: 0x0001F90E File Offset: 0x0001DB0E
		// (set) Token: 0x06000569 RID: 1385 RVA: 0x0001F916 File Offset: 0x0001DB16
		public AimingStatsJson Aiming { get; set; } = new AimingStatsJson();

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x0600056A RID: 1386 RVA: 0x0001F91F File Offset: 0x0001DB1F
		// (set) Token: 0x0600056B RID: 1387 RVA: 0x0001F927 File Offset: 0x0001DB27
		public float ArrowDamageMultiplier { get; set; } = 1f;

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x0600056C RID: 1388 RVA: 0x0001F930 File Offset: 0x0001DB30
		// (set) Token: 0x0600056D RID: 1389 RVA: 0x0001F938 File Offset: 0x0001DB38
		public int ArrowDamageTier { get; set; } = 1;

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x0600056E RID: 1390 RVA: 0x0001F941 File Offset: 0x0001DB41
		// (set) Token: 0x0600056F RID: 1391 RVA: 0x0001F949 File Offset: 0x0001DB49
		public float ArrowVelocity { get; set; } = 1f;

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000570 RID: 1392 RVA: 0x0001F952 File Offset: 0x0001DB52
		// (set) Token: 0x06000571 RID: 1393 RVA: 0x0001F95A File Offset: 0x0001DB5A
		public string ArrowWildcard { get; set; } = "*arrow-*";

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000572 RID: 1394 RVA: 0x0001F963 File Offset: 0x0001DB63
		// (set) Token: 0x06000573 RID: 1395 RVA: 0x0001F96B File Offset: 0x0001DB6B
		public float Zeroing { get; set; } = 1.5f;

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000574 RID: 1396 RVA: 0x0001F974 File Offset: 0x0001DB74
		// (set) Token: 0x06000575 RID: 1397 RVA: 0x0001F97C File Offset: 0x0001DB7C
		public float[] DispersionMOA { get; set; } = new float[2];

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000576 RID: 1398 RVA: 0x0001F985 File Offset: 0x0001DB85
		// (set) Token: 0x06000577 RID: 1399 RVA: 0x0001F98D File Offset: 0x0001DB8D
		public float ScreenShakeStrength { get; set; } = 0.2f;

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000578 RID: 1400 RVA: 0x0001F996 File Offset: 0x0001DB96
		// (set) Token: 0x06000579 RID: 1401 RVA: 0x0001F99E File Offset: 0x0001DB9E
		public bool TwoHanded { get; set; } = true;

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600057A RID: 1402 RVA: 0x0001F9A7 File Offset: 0x0001DBA7
		// (set) Token: 0x0600057B RID: 1403 RVA: 0x0001F9AF File Offset: 0x0001DBAF
		public float ReloadAnimationSpeed { get; set; } = 1f;
	}
}
