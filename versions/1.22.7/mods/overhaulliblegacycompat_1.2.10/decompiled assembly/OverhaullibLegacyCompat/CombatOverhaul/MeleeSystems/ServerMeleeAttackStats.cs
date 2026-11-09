using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Implementations;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x02000102 RID: 258
	[NullableContext(2)]
	[Nullable(0)]
	public readonly struct ServerMeleeAttackStats
	{
		// Token: 0x06000CFB RID: 3323 RVA: 0x0003FD2F File Offset: 0x0003DF2F
		public ServerMeleeAttackStats(MeleeAttackStats attack, ItemStackMeleeWeaponStats stackStats)
		{
			this.Attack = attack;
			this.StackStats = stackStats;
		}

		// Token: 0x04000647 RID: 1607
		public readonly MeleeAttackStats Attack;

		// Token: 0x04000648 RID: 1608
		public readonly ItemStackMeleeWeaponStats StackStats;
	}
}
