using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x02000129 RID: 297
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PlayerDamageModelJson
	{
		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06000E67 RID: 3687 RVA: 0x0004587E File Offset: 0x00043A7E
		// (set) Token: 0x06000E68 RID: 3688 RVA: 0x00045886 File Offset: 0x00043A86
		public DamageZoneStatsJson[] Zones { get; set; } = Array.Empty<DamageZoneStatsJson>();
	}
}
