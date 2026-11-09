using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000141 RID: 321
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class WearableStatsJson
	{
		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06000F87 RID: 3975 RVA: 0x0004C309 File Offset: 0x0004A509
		// (set) Token: 0x06000F88 RID: 3976 RVA: 0x0004C311 File Offset: 0x0004A511
		public Dictionary<string, float> PlayerStats { get; set; } = new Dictionary<string, float>();
	}
}
