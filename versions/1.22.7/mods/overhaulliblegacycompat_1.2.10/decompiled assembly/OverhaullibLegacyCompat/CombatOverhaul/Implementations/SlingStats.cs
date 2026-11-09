using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.RangedSystems.Aiming;

namespace CombatOverhaul.Implementations
{
	// Token: 0x0200008B RID: 139
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SlingStats : WeaponStats
	{
		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000752 RID: 1874 RVA: 0x0002C176 File Offset: 0x0002A376
		// (set) Token: 0x06000753 RID: 1875 RVA: 0x0002C17E File Offset: 0x0002A37E
		public string LoadAnimation { get; set; } = "";

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000754 RID: 1876 RVA: 0x0002C187 File Offset: 0x0002A387
		// (set) Token: 0x06000755 RID: 1877 RVA: 0x0002C18F File Offset: 0x0002A38F
		public string WindUpAnimation { get; set; } = "";

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000756 RID: 1878 RVA: 0x0002C198 File Offset: 0x0002A398
		// (set) Token: 0x06000757 RID: 1879 RVA: 0x0002C1A0 File Offset: 0x0002A3A0
		public string WindUpAfterLoadAnimation { get; set; } = "";

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000758 RID: 1880 RVA: 0x0002C1A9 File Offset: 0x0002A3A9
		// (set) Token: 0x06000759 RID: 1881 RVA: 0x0002C1B1 File Offset: 0x0002A3B1
		public string SwingAnimation { get; set; } = "";

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x0600075A RID: 1882 RVA: 0x0002C1BA File Offset: 0x0002A3BA
		// (set) Token: 0x0600075B RID: 1883 RVA: 0x0002C1C2 File Offset: 0x0002A3C2
		public string ReleaseAnimation { get; set; } = "";

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x0600075C RID: 1884 RVA: 0x0002C1CB File Offset: 0x0002A3CB
		// (set) Token: 0x0600075D RID: 1885 RVA: 0x0002C1D3 File Offset: 0x0002A3D3
		public AimingStatsJson Aiming { get; set; } = new AimingStatsJson();

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x0600075E RID: 1886 RVA: 0x0002C1DC File Offset: 0x0002A3DC
		// (set) Token: 0x0600075F RID: 1887 RVA: 0x0002C1E4 File Offset: 0x0002A3E4
		public float BulletDamageMultiplier { get; set; } = 1f;

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000760 RID: 1888 RVA: 0x0002C1ED File Offset: 0x0002A3ED
		// (set) Token: 0x06000761 RID: 1889 RVA: 0x0002C1F5 File Offset: 0x0002A3F5
		public int BulletDamageTier { get; set; } = 1;

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000762 RID: 1890 RVA: 0x0002C1FE File Offset: 0x0002A3FE
		// (set) Token: 0x06000763 RID: 1891 RVA: 0x0002C206 File Offset: 0x0002A406
		public float BulletVelocity { get; set; } = 1f;

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000764 RID: 1892 RVA: 0x0002C20F File Offset: 0x0002A40F
		// (set) Token: 0x06000765 RID: 1893 RVA: 0x0002C217 File Offset: 0x0002A417
		public string BulletWildcard { get; set; } = "*slingbullet-*";

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000766 RID: 1894 RVA: 0x0002C220 File Offset: 0x0002A420
		// (set) Token: 0x06000767 RID: 1895 RVA: 0x0002C228 File Offset: 0x0002A428
		public float Zeroing { get; set; } = 1.5f;

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000768 RID: 1896 RVA: 0x0002C231 File Offset: 0x0002A431
		// (set) Token: 0x06000769 RID: 1897 RVA: 0x0002C239 File Offset: 0x0002A439
		public float[] DispersionMOA { get; set; } = new float[2];

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x0600076A RID: 1898 RVA: 0x0002C242 File Offset: 0x0002A442
		// (set) Token: 0x0600076B RID: 1899 RVA: 0x0002C24A File Offset: 0x0002A44A
		public float ScreenShakeStrength { get; set; } = 0.2f;

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x0600076C RID: 1900 RVA: 0x0002C253 File Offset: 0x0002A453
		// (set) Token: 0x0600076D RID: 1901 RVA: 0x0002C25B File Offset: 0x0002A45B
		public bool TwoHanded { get; set; }

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x0600076E RID: 1902 RVA: 0x0002C264 File Offset: 0x0002A464
		// (set) Token: 0x0600076F RID: 1903 RVA: 0x0002C26C File Offset: 0x0002A46C
		public float MaxSwingSpeed { get; set; } = 1f;

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000770 RID: 1904 RVA: 0x0002C275 File Offset: 0x0002A475
		// (set) Token: 0x06000771 RID: 1905 RVA: 0x0002C27D File Offset: 0x0002A47D
		public float MinSwingSpeed { get; set; } = 0.5f;

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000772 RID: 1906 RVA: 0x0002C286 File Offset: 0x0002A486
		// (set) Token: 0x06000773 RID: 1907 RVA: 0x0002C28E File Offset: 0x0002A48E
		public float SwingSpeedPerSwing { get; set; } = 0.11f;

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000774 RID: 1908 RVA: 0x0002C297 File Offset: 0x0002A497
		// (set) Token: 0x06000775 RID: 1909 RVA: 0x0002C29F File Offset: 0x0002A49F
		public float SwingAnimationSpeed { get; set; } = 1f;
	}
}
