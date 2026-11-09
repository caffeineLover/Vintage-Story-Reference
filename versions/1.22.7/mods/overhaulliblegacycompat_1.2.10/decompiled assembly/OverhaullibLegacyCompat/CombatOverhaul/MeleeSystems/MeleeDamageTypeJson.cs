using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.DamageSystems;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x020000FA RID: 250
	[NullableContext(1)]
	[Nullable(0)]
	public class MeleeDamageTypeJson
	{
		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06000CCE RID: 3278 RVA: 0x0003F2D7 File Offset: 0x0003D4D7
		// (set) Token: 0x06000CCF RID: 3279 RVA: 0x0003F2DF File Offset: 0x0003D4DF
		public DamageDataJson Damage { get; set; } = new DamageDataJson();

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06000CD0 RID: 3280 RVA: 0x0003F2E8 File Offset: 0x0003D4E8
		// (set) Token: 0x06000CD1 RID: 3281 RVA: 0x0003F2F0 File Offset: 0x0003D4F0
		public float Knockback { get; set; }

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06000CD2 RID: 3282 RVA: 0x0003F2F9 File Offset: 0x0003D4F9
		// (set) Token: 0x06000CD3 RID: 3283 RVA: 0x0003F301 File Offset: 0x0003D501
		public int DurabilityDamage { get; set; } = 1;

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06000CD4 RID: 3284 RVA: 0x0003F30A File Offset: 0x0003D50A
		// (set) Token: 0x06000CD5 RID: 3285 RVA: 0x0003F312 File Offset: 0x0003D512
		public float[] Collider { get; set; } = new float[6];

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06000CD6 RID: 3286 RVA: 0x0003F31B File Offset: 0x0003D51B
		// (set) Token: 0x06000CD7 RID: 3287 RVA: 0x0003F323 File Offset: 0x0003D523
		public float Radius { get; set; } = 0.1f;

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06000CD8 RID: 3288 RVA: 0x0003F32C File Offset: 0x0003D52C
		// (set) Token: 0x06000CD9 RID: 3289 RVA: 0x0003F334 File Offset: 0x0003D534
		public int StaggerTimeMs { get; set; }

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06000CDA RID: 3290 RVA: 0x0003F33D File Offset: 0x0003D53D
		// (set) Token: 0x06000CDB RID: 3291 RVA: 0x0003F345 File Offset: 0x0003D545
		public int StaggerTier { get; set; } = 1;

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06000CDC RID: 3292 RVA: 0x0003F34E File Offset: 0x0003D54E
		// (set) Token: 0x06000CDD RID: 3293 RVA: 0x0003F356 File Offset: 0x0003D556
		public int PushTier { get; set; }

		// Token: 0x06000CDE RID: 3294 RVA: 0x0003F35F File Offset: 0x0003D55F
		public MeleeDamageType ToDamageType()
		{
			return new MeleeDamageType(this);
		}
	}
}
