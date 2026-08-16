using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x02000106 RID: 262
	[NullableContext(2)]
	public interface IWeaponDamageSource
	{
		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06000D20 RID: 3360
		// (set) Token: 0x06000D21 RID: 3361
		ItemStack Weapon { get; set; }
	}
}
