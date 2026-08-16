using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.DamageSystems;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x020000EF RID: 239
	[NullableContext(1)]
	[Nullable(0)]
	public class MeleeDamageTypeJson
	{
		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06000C72 RID: 3186 RVA: 0x0003E2E7 File Offset: 0x0003C4E7
		// (set) Token: 0x06000C73 RID: 3187 RVA: 0x0003E2EF File Offset: 0x0003C4EF
		public DamageDataJson Damage { get; set; } = new DamageDataJson();

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000C74 RID: 3188 RVA: 0x0003E2F8 File Offset: 0x0003C4F8
		// (set) Token: 0x06000C75 RID: 3189 RVA: 0x0003E300 File Offset: 0x0003C500
		public float Knockback { get; set; }

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06000C76 RID: 3190 RVA: 0x0003E309 File Offset: 0x0003C509
		// (set) Token: 0x06000C77 RID: 3191 RVA: 0x0003E311 File Offset: 0x0003C511
		public int DurabilityDamage { get; set; } = 1;

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06000C78 RID: 3192 RVA: 0x0003E31A File Offset: 0x0003C51A
		// (set) Token: 0x06000C79 RID: 3193 RVA: 0x0003E322 File Offset: 0x0003C522
		public float[] Collider { get; set; } = new float[6];

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06000C7A RID: 3194 RVA: 0x0003E32B File Offset: 0x0003C52B
		// (set) Token: 0x06000C7B RID: 3195 RVA: 0x0003E333 File Offset: 0x0003C533
		public float Radius { get; set; } = 0.1f;

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06000C7C RID: 3196 RVA: 0x0003E33C File Offset: 0x0003C53C
		// (set) Token: 0x06000C7D RID: 3197 RVA: 0x0003E344 File Offset: 0x0003C544
		public int StaggerTimeMs { get; set; }

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000C7E RID: 3198 RVA: 0x0003E34D File Offset: 0x0003C54D
		// (set) Token: 0x06000C7F RID: 3199 RVA: 0x0003E355 File Offset: 0x0003C555
		public int StaggerTier { get; set; } = 1;

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000C80 RID: 3200 RVA: 0x0003E35E File Offset: 0x0003C55E
		// (set) Token: 0x06000C81 RID: 3201 RVA: 0x0003E366 File Offset: 0x0003C566
		public int PushTier { get; set; }

		// Token: 0x06000C82 RID: 3202 RVA: 0x0003E36F File Offset: 0x0003C56F
		public MeleeDamageType ToDamageType()
		{
			return new MeleeDamageType(this);
		}
	}
}
