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
		// (get) Token: 0x06000752 RID: 1874 RVA: 0x0002C13E File Offset: 0x0002A33E
		// (set) Token: 0x06000753 RID: 1875 RVA: 0x0002C146 File Offset: 0x0002A346
		public string LoadAnimation { get; set; } = "";

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000754 RID: 1876 RVA: 0x0002C14F File Offset: 0x0002A34F
		// (set) Token: 0x06000755 RID: 1877 RVA: 0x0002C157 File Offset: 0x0002A357
		public string WindUpAnimation { get; set; } = "";

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000756 RID: 1878 RVA: 0x0002C160 File Offset: 0x0002A360
		// (set) Token: 0x06000757 RID: 1879 RVA: 0x0002C168 File Offset: 0x0002A368
		public string WindUpAfterLoadAnimation { get; set; } = "";

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000758 RID: 1880 RVA: 0x0002C171 File Offset: 0x0002A371
		// (set) Token: 0x06000759 RID: 1881 RVA: 0x0002C179 File Offset: 0x0002A379
		public string SwingAnimation { get; set; } = "";

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x0600075A RID: 1882 RVA: 0x0002C182 File Offset: 0x0002A382
		// (set) Token: 0x0600075B RID: 1883 RVA: 0x0002C18A File Offset: 0x0002A38A
		public string ReleaseAnimation { get; set; } = "";

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x0600075C RID: 1884 RVA: 0x0002C193 File Offset: 0x0002A393
		// (set) Token: 0x0600075D RID: 1885 RVA: 0x0002C19B File Offset: 0x0002A39B
		public AimingStatsJson Aiming { get; set; } = new AimingStatsJson();

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x0600075E RID: 1886 RVA: 0x0002C1A4 File Offset: 0x0002A3A4
		// (set) Token: 0x0600075F RID: 1887 RVA: 0x0002C1AC File Offset: 0x0002A3AC
		public float BulletDamageMultiplier { get; set; } = 1f;

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000760 RID: 1888 RVA: 0x0002C1B5 File Offset: 0x0002A3B5
		// (set) Token: 0x06000761 RID: 1889 RVA: 0x0002C1BD File Offset: 0x0002A3BD
		public int BulletDamageTier { get; set; } = 1;

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000762 RID: 1890 RVA: 0x0002C1C6 File Offset: 0x0002A3C6
		// (set) Token: 0x06000763 RID: 1891 RVA: 0x0002C1CE File Offset: 0x0002A3CE
		public float BulletVelocity { get; set; } = 1f;

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000764 RID: 1892 RVA: 0x0002C1D7 File Offset: 0x0002A3D7
		// (set) Token: 0x06000765 RID: 1893 RVA: 0x0002C1DF File Offset: 0x0002A3DF
		public string BulletWildcard { get; set; } = "*slingbullet-*";

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000766 RID: 1894 RVA: 0x0002C1E8 File Offset: 0x0002A3E8
		// (set) Token: 0x06000767 RID: 1895 RVA: 0x0002C1F0 File Offset: 0x0002A3F0
		public float Zeroing { get; set; } = 1.5f;

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000768 RID: 1896 RVA: 0x0002C1F9 File Offset: 0x0002A3F9
		// (set) Token: 0x06000769 RID: 1897 RVA: 0x0002C201 File Offset: 0x0002A401
		public float[] DispersionMOA { get; set; } = new float[2];

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x0600076A RID: 1898 RVA: 0x0002C20A File Offset: 0x0002A40A
		// (set) Token: 0x0600076B RID: 1899 RVA: 0x0002C212 File Offset: 0x0002A412
		public float ScreenShakeStrength { get; set; } = 0.2f;

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x0600076C RID: 1900 RVA: 0x0002C21B File Offset: 0x0002A41B
		// (set) Token: 0x0600076D RID: 1901 RVA: 0x0002C223 File Offset: 0x0002A423
		public bool TwoHanded { get; set; }

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x0600076E RID: 1902 RVA: 0x0002C22C File Offset: 0x0002A42C
		// (set) Token: 0x0600076F RID: 1903 RVA: 0x0002C234 File Offset: 0x0002A434
		public float MaxSwingSpeed { get; set; } = 1f;

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000770 RID: 1904 RVA: 0x0002C23D File Offset: 0x0002A43D
		// (set) Token: 0x06000771 RID: 1905 RVA: 0x0002C245 File Offset: 0x0002A445
		public float MinSwingSpeed { get; set; } = 0.5f;

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000772 RID: 1906 RVA: 0x0002C24E File Offset: 0x0002A44E
		// (set) Token: 0x06000773 RID: 1907 RVA: 0x0002C256 File Offset: 0x0002A456
		public float SwingSpeedPerSwing { get; set; } = 0.11f;

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000774 RID: 1908 RVA: 0x0002C25F File Offset: 0x0002A45F
		// (set) Token: 0x06000775 RID: 1909 RVA: 0x0002C267 File Offset: 0x0002A467
		public float SwingAnimationSpeed { get; set; } = 1f;
	}
}
