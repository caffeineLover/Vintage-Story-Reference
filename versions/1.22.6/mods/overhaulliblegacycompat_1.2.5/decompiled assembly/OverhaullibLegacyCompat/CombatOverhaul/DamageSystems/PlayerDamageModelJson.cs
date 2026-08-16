using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x0200011E RID: 286
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PlayerDamageModelJson
	{
		// Token: 0x17000394 RID: 916
		// (get) Token: 0x06000E01 RID: 3585 RVA: 0x000445F6 File Offset: 0x000427F6
		// (set) Token: 0x06000E02 RID: 3586 RVA: 0x000445FE File Offset: 0x000427FE
		public DamageZoneStatsJson[] Zones { get; set; } = Array.Empty<DamageZoneStatsJson>();
	}
}
