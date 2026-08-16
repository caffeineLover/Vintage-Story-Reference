using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x02000114 RID: 276
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class EntityDamageModelJson
	{
		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06000D81 RID: 3457 RVA: 0x00041E09 File Offset: 0x00040009
		// (set) Token: 0x06000D82 RID: 3458 RVA: 0x00041E11 File Offset: 0x00040011
		public float TorsoDamageMultiplier { get; set; } = 1f;

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06000D83 RID: 3459 RVA: 0x00041E1A File Offset: 0x0004001A
		// (set) Token: 0x06000D84 RID: 3460 RVA: 0x00041E22 File Offset: 0x00040022
		public float LimbsDamageMultiplier { get; set; } = 0.5f;

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06000D85 RID: 3461 RVA: 0x00041E2B File Offset: 0x0004002B
		// (set) Token: 0x06000D86 RID: 3462 RVA: 0x00041E33 File Offset: 0x00040033
		public float HeadDamageMultiplier { get; set; } = 1.5f;

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06000D87 RID: 3463 RVA: 0x00041E3C File Offset: 0x0004003C
		// (set) Token: 0x06000D88 RID: 3464 RVA: 0x00041E44 File Offset: 0x00040044
		public float CriticalDamageMultiplier { get; set; } = 2f;

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06000D89 RID: 3465 RVA: 0x00041E4D File Offset: 0x0004004D
		// (set) Token: 0x06000D8A RID: 3466 RVA: 0x00041E55 File Offset: 0x00040055
		public float ResistantDamageMultiplier { get; set; }

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06000D8B RID: 3467 RVA: 0x00041E5E File Offset: 0x0004005E
		// (set) Token: 0x06000D8C RID: 3468 RVA: 0x00041E66 File Offset: 0x00040066
		public Dictionary<string, float> DefaultResists { get; set; } = new Dictionary<string, float>();

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06000D8D RID: 3469 RVA: 0x00041E6F File Offset: 0x0004006F
		// (set) Token: 0x06000D8E RID: 3470 RVA: 0x00041E77 File Offset: 0x00040077
		public Dictionary<string, Dictionary<string, float>> ResistsForColliders { get; set; } = new Dictionary<string, Dictionary<string, float>>();

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06000D8F RID: 3471 RVA: 0x00041E80 File Offset: 0x00040080
		// (set) Token: 0x06000D90 RID: 3472 RVA: 0x00041E88 File Offset: 0x00040088
		public Dictionary<string, SoundEffectData> HitSounds { get; set; } = new Dictionary<string, SoundEffectData>();

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06000D91 RID: 3473 RVA: 0x00041E91 File Offset: 0x00040091
		// (set) Token: 0x06000D92 RID: 3474 RVA: 0x00041E99 File Offset: 0x00040099
		public Dictionary<string, string> HitParticles { get; set; } = new Dictionary<string, string>();

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06000D93 RID: 3475 RVA: 0x00041EA2 File Offset: 0x000400A2
		// (set) Token: 0x06000D94 RID: 3476 RVA: 0x00041EAA File Offset: 0x000400AA
		public bool ScaleParticlesCountWithDamage { get; set; } = true;
	}
}
