using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Armor
{
	// Token: 0x0200014D RID: 333
	[NullableContext(1)]
	public interface IHasSlotBackpackCategory
	{
		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06001078 RID: 4216
		string BackpackCategoryCode { get; }

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06001079 RID: 4217
		float OrderPriority { get; }
	}
}
