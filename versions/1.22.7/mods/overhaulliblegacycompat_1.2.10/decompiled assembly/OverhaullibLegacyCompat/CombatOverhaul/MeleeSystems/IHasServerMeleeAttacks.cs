using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x02000103 RID: 259
	[NullableContext(1)]
	public interface IHasServerMeleeAttacks
	{
		// Token: 0x06000CFC RID: 3324
		IEnumerable<ServerMeleeAttackStats> GetServerMeleeAttacks(EntityPlayer attacker, Entity target, ItemSlot weaponSlot);
	}
}
