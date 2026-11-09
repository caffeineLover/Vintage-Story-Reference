using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Inputs
{
	// Token: 0x02000066 RID: 102
	[NullableContext(2)]
	public interface IHasWeaponLogic
	{
		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000504 RID: 1284
		IClientWeaponLogic ClientLogic { get; }
	}
}
