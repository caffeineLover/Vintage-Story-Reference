using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x02000111 RID: 273
	[NullableContext(2)]
	public interface IWeaponDamageSource
	{
		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06000D7C RID: 3452
		// (set) Token: 0x06000D7D RID: 3453
		ItemStack Weapon { get; set; }
	}
}
