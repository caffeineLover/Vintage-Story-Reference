using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.WeaponBuffs
{
	// Token: 0x020000A3 RID: 163
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class WeaponBuffApplyOptions
	{
		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000891 RID: 2193 RVA: 0x00030A0B File Offset: 0x0002EC0B
		// (set) Token: 0x06000892 RID: 2194 RVA: 0x00030A13 File Offset: 0x0002EC13
		public string InstanceId { get; set; } = "";

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000893 RID: 2195 RVA: 0x00030A1C File Offset: 0x0002EC1C
		// (set) Token: 0x06000894 RID: 2196 RVA: 0x00030A24 File Offset: 0x0002EC24
		public bool ReplaceExisting { get; set; } = true;

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000895 RID: 2197 RVA: 0x00030A2D File Offset: 0x0002EC2D
		// (set) Token: 0x06000896 RID: 2198 RVA: 0x00030A35 File Offset: 0x0002EC35
		public bool MarkDirty { get; set; } = true;

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000897 RID: 2199 RVA: 0x00030A3E File Offset: 0x0002EC3E
		// (set) Token: 0x06000898 RID: 2200 RVA: 0x00030A46 File Offset: 0x0002EC46
		public double? DurationHoursOverride { get; set; }

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000899 RID: 2201 RVA: 0x00030A4F File Offset: 0x0002EC4F
		// (set) Token: 0x0600089A RID: 2202 RVA: 0x00030A57 File Offset: 0x0002EC57
		public int? UsesOverride { get; set; }
	}
}
