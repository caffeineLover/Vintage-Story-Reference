using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Inputs
{
	// Token: 0x02000067 RID: 103
	[NullableContext(1)]
	public interface IHasMultipleWeaponLogicModes
	{
		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000505 RID: 1285
		IEnumerable<IClientWeaponLogic> ClientLogicModes { get; }
	}
}
