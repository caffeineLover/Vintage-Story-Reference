using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000159 RID: 345
	[NullableContext(1)]
	public interface IHasSlotBackpackCategory
	{
		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x060010EC RID: 4332
		string BackpackCategoryCode { get; }

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x060010ED RID: 4333
		float OrderPriority { get; }
	}
}
