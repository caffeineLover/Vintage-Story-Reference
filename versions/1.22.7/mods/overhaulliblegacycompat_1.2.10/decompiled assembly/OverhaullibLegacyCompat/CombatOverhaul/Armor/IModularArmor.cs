using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.DamageSystems;
using Vintagestory.API.Common;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000137 RID: 311
	[NullableContext(1)]
	public interface IModularArmor
	{
		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06000EFC RID: 3836
		ArmorType ArmorType { get; }

		// Token: 0x06000EFD RID: 3837
		DamageResistData GetResists(ItemSlot slot, ArmorType type);
	}
}
