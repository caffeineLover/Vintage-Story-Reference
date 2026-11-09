using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x02000114 RID: 276
	[NullableContext(1)]
	[Nullable(0)]
	public class ProjectileDamageDataJson
	{
		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06000D8B RID: 3467 RVA: 0x00041B9A File Offset: 0x0003FD9A
		// (set) Token: 0x06000D8C RID: 3468 RVA: 0x00041BA2 File Offset: 0x0003FDA2
		public string DamageType { get; set; } = "PiercingAttack";

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06000D8D RID: 3469 RVA: 0x00041BAB File Offset: 0x0003FDAB
		// (set) Token: 0x06000D8E RID: 3470 RVA: 0x00041BB3 File Offset: 0x0003FDB3
		public float Damage { get; set; }
	}
}
