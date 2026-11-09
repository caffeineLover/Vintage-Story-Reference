using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000131 RID: 305
	[NullableContext(1)]
	public interface IAffectsPlayerStats
	{
		// Token: 0x06000ED7 RID: 3799
		Dictionary<string, float> PlayerStats(ItemSlot slot, EntityPlayer player);

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06000ED8 RID: 3800
		// (set) Token: 0x06000ED9 RID: 3801
		bool StatsChanged { get; set; }
	}
}
