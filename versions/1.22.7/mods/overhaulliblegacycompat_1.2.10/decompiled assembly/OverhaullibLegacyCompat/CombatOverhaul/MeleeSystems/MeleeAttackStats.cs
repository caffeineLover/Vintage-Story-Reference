using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x020000F6 RID: 246
	[NullableContext(1)]
	[Nullable(0)]
	public class MeleeAttackStats
	{
		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000C66 RID: 3174 RVA: 0x0003E5C8 File Offset: 0x0003C7C8
		// (set) Token: 0x06000C67 RID: 3175 RVA: 0x0003E5D0 File Offset: 0x0003C7D0
		public bool StopOnTerrainHit { get; set; }

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000C68 RID: 3176 RVA: 0x0003E5D9 File Offset: 0x0003C7D9
		// (set) Token: 0x06000C69 RID: 3177 RVA: 0x0003E5E1 File Offset: 0x0003C7E1
		public bool StopOnEntityHit { get; set; }

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000C6A RID: 3178 RVA: 0x0003E5EA File Offset: 0x0003C7EA
		// (set) Token: 0x06000C6B RID: 3179 RVA: 0x0003E5F2 File Offset: 0x0003C7F2
		public bool CollideWithTerrain { get; set; } = true;

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000C6C RID: 3180 RVA: 0x0003E5FB File Offset: 0x0003C7FB
		// (set) Token: 0x06000C6D RID: 3181 RVA: 0x0003E603 File Offset: 0x0003C803
		public bool HitOnlyOneEntity { get; set; }

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000C6E RID: 3182 RVA: 0x0003E60C File Offset: 0x0003C80C
		// (set) Token: 0x06000C6F RID: 3183 RVA: 0x0003E614 File Offset: 0x0003C814
		public float MaxReach { get; set; } = 6f;

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000C70 RID: 3184 RVA: 0x0003E61D File Offset: 0x0003C81D
		// (set) Token: 0x06000C71 RID: 3185 RVA: 0x0003E625 File Offset: 0x0003C825
		[Nullable(2)]
		public ImpaleAttackStats Impale { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000C72 RID: 3186 RVA: 0x0003E62E File Offset: 0x0003C82E
		// (set) Token: 0x06000C73 RID: 3187 RVA: 0x0003E636 File Offset: 0x0003C836
		public MeleeDamageTypeJson[] DamageTypes { get; set; } = Array.Empty<MeleeDamageTypeJson>();
	}
}
