using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000135 RID: 309
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class WearableStatsJson
	{
		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06000F13 RID: 3859 RVA: 0x0004AA09 File Offset: 0x00048C09
		// (set) Token: 0x06000F14 RID: 3860 RVA: 0x0004AA11 File Offset: 0x00048C11
		public Dictionary<string, float> PlayerStats { get; set; } = new Dictionary<string, float>();
	}
}
