using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x0200010D RID: 269
	[NullableContext(2)]
	[Nullable(0)]
	public class TypedDamageSource : DamageSource, ITypedDamage, IWeaponDamageSource
	{
		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06000D43 RID: 3395 RVA: 0x00040C26 File Offset: 0x0003EE26
		// (set) Token: 0x06000D44 RID: 3396 RVA: 0x00040C2E File Offset: 0x0003EE2E
		public DamageData DamageTypeData { get; set; }

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06000D45 RID: 3397 RVA: 0x00040C37 File Offset: 0x0003EE37
		// (set) Token: 0x06000D46 RID: 3398 RVA: 0x00040C3F File Offset: 0x0003EE3F
		public ItemStack Weapon { get; set; }
	}
}
