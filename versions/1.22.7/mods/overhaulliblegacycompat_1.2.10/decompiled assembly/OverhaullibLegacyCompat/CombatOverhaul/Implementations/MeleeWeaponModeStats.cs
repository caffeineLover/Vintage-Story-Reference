using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Implementations
{
	// Token: 0x02000086 RID: 134
	[NullableContext(1)]
	[Nullable(0)]
	public class MeleeWeaponModeStats : MeleeWeaponStats
	{
		// Token: 0x1700016A RID: 362
		// (get) Token: 0x0600072F RID: 1839 RVA: 0x0002AC20 File Offset: 0x00028E20
		// (set) Token: 0x06000730 RID: 1840 RVA: 0x0002AC28 File Offset: 0x00028E28
		public string Icon { get; set; } = "";

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000731 RID: 1841 RVA: 0x0002AC31 File Offset: 0x00028E31
		// (set) Token: 0x06000732 RID: 1842 RVA: 0x0002AC39 File Offset: 0x00028E39
		public string Name { get; set; } = "";
	}
}
