using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Implementations;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x020000F7 RID: 247
	[NullableContext(2)]
	[Nullable(0)]
	public readonly struct ServerMeleeAttackStats
	{
		// Token: 0x06000C9F RID: 3231 RVA: 0x0003ED3F File Offset: 0x0003CF3F
		public ServerMeleeAttackStats(MeleeAttackStats attack, ItemStackMeleeWeaponStats stackStats)
		{
			this.Attack = attack;
			this.StackStats = stackStats;
		}

		// Token: 0x04000625 RID: 1573
		public readonly MeleeAttackStats Attack;

		// Token: 0x04000626 RID: 1574
		public readonly ItemStackMeleeWeaponStats StackStats;
	}
}
