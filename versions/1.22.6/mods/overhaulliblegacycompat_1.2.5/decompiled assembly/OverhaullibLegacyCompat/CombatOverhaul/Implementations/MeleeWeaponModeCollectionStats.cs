using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Implementations
{
	// Token: 0x02000087 RID: 135
	[NullableContext(1)]
	[Nullable(0)]
	public class MeleeWeaponModeCollectionStats
	{
		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000734 RID: 1844 RVA: 0x0002AC28 File Offset: 0x00028E28
		// (set) Token: 0x06000735 RID: 1845 RVA: 0x0002AC30 File Offset: 0x00028E30
		public Dictionary<string, MeleeWeaponModeStats> Modes { get; set; } = new Dictionary<string, MeleeWeaponModeStats>();
	}
}
