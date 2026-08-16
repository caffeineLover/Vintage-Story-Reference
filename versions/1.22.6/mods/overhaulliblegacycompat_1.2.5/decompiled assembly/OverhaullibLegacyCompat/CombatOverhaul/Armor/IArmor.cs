using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.DamageSystems;

namespace CombatOverhaul.Armor
{
	// Token: 0x0200012A RID: 298
	[NullableContext(1)]
	public interface IArmor
	{
		// Token: 0x170003AA RID: 938
		// (get) Token: 0x06000E86 RID: 3718
		ArmorType ArmorType { get; }

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x06000E87 RID: 3719
		DamageResistData Resists { get; }
	}
}
