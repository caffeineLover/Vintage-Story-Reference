using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Colliders
{
	// Token: 0x02000168 RID: 360
	[NullableContext(1)]
	[Nullable(0)]
	public class CollidersConfig
	{
		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x060011E6 RID: 4582 RVA: 0x00055339 File Offset: 0x00053539
		// (set) Token: 0x060011E7 RID: 4583 RVA: 0x00055341 File Offset: 0x00053541
		public ColliderTypesJson Elements { get; set; } = new ColliderTypesJson();

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x060011E8 RID: 4584 RVA: 0x0005534A File Offset: 0x0005354A
		// (set) Token: 0x060011E9 RID: 4585 RVA: 0x00055352 File Offset: 0x00053552
		public float DefaultPenetrationResistance { get; set; } = 5f;

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x060011EA RID: 4586 RVA: 0x0005535B File Offset: 0x0005355B
		// (set) Token: 0x060011EB RID: 4587 RVA: 0x00055363 File Offset: 0x00053563
		public Dictionary<string, float> PenetrationResistances { get; set; } = new Dictionary<string, float>();

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x060011EC RID: 4588 RVA: 0x0005536C File Offset: 0x0005356C
		// (set) Token: 0x060011ED RID: 4589 RVA: 0x00055374 File Offset: 0x00053574
		public bool ResistantCollidersStopProjectiles { get; set; } = true;
	}
}
