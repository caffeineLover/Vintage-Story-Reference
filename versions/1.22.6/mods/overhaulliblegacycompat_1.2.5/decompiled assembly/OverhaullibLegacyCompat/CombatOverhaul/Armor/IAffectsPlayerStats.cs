using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000125 RID: 293
	[NullableContext(1)]
	public interface IAffectsPlayerStats
	{
		// Token: 0x06000E63 RID: 3683
		Dictionary<string, float> PlayerStats(ItemSlot slot, EntityPlayer player);

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06000E64 RID: 3684
		// (set) Token: 0x06000E65 RID: 3685
		bool StatsChanged { get; set; }
	}
}
