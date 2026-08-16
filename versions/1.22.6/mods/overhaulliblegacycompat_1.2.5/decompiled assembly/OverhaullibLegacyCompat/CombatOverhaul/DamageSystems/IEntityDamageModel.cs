using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x02000116 RID: 278
	[NullableContext(2)]
	public interface IEntityDamageModel
	{
		// Token: 0x14000019 RID: 25
		// (add) Token: 0x06000D9F RID: 3487
		// (remove) Token: 0x06000DA0 RID: 3488
		event OnEntityReceiveDamageDelegate OnReceiveDamage;
	}
}
