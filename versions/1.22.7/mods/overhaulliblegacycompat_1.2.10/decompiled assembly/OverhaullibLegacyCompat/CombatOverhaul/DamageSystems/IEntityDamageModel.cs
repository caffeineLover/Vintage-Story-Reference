using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x02000121 RID: 289
	[NullableContext(2)]
	public interface IEntityDamageModel
	{
		// Token: 0x14000019 RID: 25
		// (add) Token: 0x06000DFB RID: 3579
		// (remove) Token: 0x06000DFC RID: 3580
		event OnEntityReceiveDamageDelegate OnReceiveDamage;
	}
}
