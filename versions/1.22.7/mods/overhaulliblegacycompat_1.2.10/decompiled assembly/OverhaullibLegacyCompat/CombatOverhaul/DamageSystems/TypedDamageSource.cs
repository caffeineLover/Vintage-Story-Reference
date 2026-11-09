using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x02000118 RID: 280
	[NullableContext(2)]
	[Nullable(0)]
	public class TypedDamageSource : DamageSource, ITypedDamage, IWeaponDamageSource
	{
		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06000D9F RID: 3487 RVA: 0x00041C4A File Offset: 0x0003FE4A
		// (set) Token: 0x06000DA0 RID: 3488 RVA: 0x00041C52 File Offset: 0x0003FE52
		public DamageData DamageTypeData { get; set; }

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06000DA1 RID: 3489 RVA: 0x00041C5B File Offset: 0x0003FE5B
		// (set) Token: 0x06000DA2 RID: 3490 RVA: 0x00041C63 File Offset: 0x0003FE63
		public ItemStack Weapon { get; set; }
	}
}
