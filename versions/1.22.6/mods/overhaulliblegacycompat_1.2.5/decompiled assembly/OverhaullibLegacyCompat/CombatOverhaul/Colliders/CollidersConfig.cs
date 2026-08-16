using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Colliders
{
	// Token: 0x0200015C RID: 348
	[NullableContext(1)]
	[Nullable(0)]
	public class CollidersConfig
	{
		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06001172 RID: 4466 RVA: 0x00053A31 File Offset: 0x00051C31
		// (set) Token: 0x06001173 RID: 4467 RVA: 0x00053A39 File Offset: 0x00051C39
		public ColliderTypesJson Elements { get; set; } = new ColliderTypesJson();

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x06001174 RID: 4468 RVA: 0x00053A42 File Offset: 0x00051C42
		// (set) Token: 0x06001175 RID: 4469 RVA: 0x00053A4A File Offset: 0x00051C4A
		public float DefaultPenetrationResistance { get; set; } = 5f;

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x06001176 RID: 4470 RVA: 0x00053A53 File Offset: 0x00051C53
		// (set) Token: 0x06001177 RID: 4471 RVA: 0x00053A5B File Offset: 0x00051C5B
		public Dictionary<string, float> PenetrationResistances { get; set; } = new Dictionary<string, float>();

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x06001178 RID: 4472 RVA: 0x00053A64 File Offset: 0x00051C64
		// (set) Token: 0x06001179 RID: 4473 RVA: 0x00053A6C File Offset: 0x00051C6C
		public bool ResistantCollidersStopProjectiles { get; set; } = true;
	}
}
