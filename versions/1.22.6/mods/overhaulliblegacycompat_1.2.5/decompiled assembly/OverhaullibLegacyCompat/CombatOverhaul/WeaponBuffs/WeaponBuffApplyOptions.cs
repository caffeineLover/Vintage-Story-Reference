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
		// (get) Token: 0x06000891 RID: 2193 RVA: 0x000309D3 File Offset: 0x0002EBD3
		// (set) Token: 0x06000892 RID: 2194 RVA: 0x000309DB File Offset: 0x0002EBDB
		public string InstanceId { get; set; } = "";

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000893 RID: 2195 RVA: 0x000309E4 File Offset: 0x0002EBE4
		// (set) Token: 0x06000894 RID: 2196 RVA: 0x000309EC File Offset: 0x0002EBEC
		public bool ReplaceExisting { get; set; } = true;

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000895 RID: 2197 RVA: 0x000309F5 File Offset: 0x0002EBF5
		// (set) Token: 0x06000896 RID: 2198 RVA: 0x000309FD File Offset: 0x0002EBFD
		public bool MarkDirty { get; set; } = true;

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000897 RID: 2199 RVA: 0x00030A06 File Offset: 0x0002EC06
		// (set) Token: 0x06000898 RID: 2200 RVA: 0x00030A0E File Offset: 0x0002EC0E
		public double? DurationHoursOverride { get; set; }

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000899 RID: 2201 RVA: 0x00030A17 File Offset: 0x0002EC17
		// (set) Token: 0x0600089A RID: 2202 RVA: 0x00030A1F File Offset: 0x0002EC1F
		public int? UsesOverride { get; set; }
	}
}
