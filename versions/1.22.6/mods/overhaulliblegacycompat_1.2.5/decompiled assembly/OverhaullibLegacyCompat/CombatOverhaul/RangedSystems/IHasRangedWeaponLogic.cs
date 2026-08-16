using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000D2 RID: 210
	[NullableContext(2)]
	public interface IHasRangedWeaponLogic
	{
		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000AF1 RID: 2801
		IServerRangedWeaponLogic ServerWeaponLogic { get; }
	}
}
