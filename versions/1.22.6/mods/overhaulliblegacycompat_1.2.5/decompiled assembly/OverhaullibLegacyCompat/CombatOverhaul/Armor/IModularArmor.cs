using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.DamageSystems;
using Vintagestory.API.Common;

namespace CombatOverhaul.Armor
{
	// Token: 0x0200012B RID: 299
	[NullableContext(1)]
	public interface IModularArmor
	{
		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06000E88 RID: 3720
		ArmorType ArmorType { get; }

		// Token: 0x06000E89 RID: 3721
		DamageResistData GetResists(ItemSlot slot, ArmorType type);
	}
}
