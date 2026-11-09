using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000DD RID: 221
	[NullableContext(2)]
	public interface IHasRangedWeaponLogic
	{
		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000B4D RID: 2893
		IServerRangedWeaponLogic ServerWeaponLogic { get; }
	}
}
