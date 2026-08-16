using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x020000F8 RID: 248
	[NullableContext(1)]
	public interface IHasServerMeleeAttacks
	{
		// Token: 0x06000CA0 RID: 3232
		IEnumerable<ServerMeleeAttackStats> GetServerMeleeAttacks(EntityPlayer attacker, Entity target, ItemSlot weaponSlot);
	}
}
