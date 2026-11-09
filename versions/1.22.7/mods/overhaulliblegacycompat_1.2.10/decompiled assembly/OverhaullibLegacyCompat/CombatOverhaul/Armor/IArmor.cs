using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.DamageSystems;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000136 RID: 310
	[NullableContext(1)]
	public interface IArmor
	{
		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06000EFA RID: 3834
		ArmorType ArmorType { get; }

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06000EFB RID: 3835
		DamageResistData Resists { get; }
	}
}
