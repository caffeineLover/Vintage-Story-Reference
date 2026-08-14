using System;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Models
{
	// Token: 0x020000C1 RID: 193
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class SerializablePresetModState
	{
		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06000FE6 RID: 4070 RVA: 0x00054BF6 File Offset: 0x00052DF6
		// (set) Token: 0x06000FE7 RID: 4071 RVA: 0x00054BFE File Offset: 0x00052DFE
		public string ModId { get; set; }

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06000FE8 RID: 4072 RVA: 0x00054C07 File Offset: 0x00052E07
		// (set) Token: 0x06000FE9 RID: 4073 RVA: 0x00054C0F File Offset: 0x00052E0F
		public string Version { get; set; }

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06000FEA RID: 4074 RVA: 0x00054C18 File Offset: 0x00052E18
		// (set) Token: 0x06000FEB RID: 4075 RVA: 0x00054C20 File Offset: 0x00052E20
		public bool? IsActive { get; set; }

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x06000FEC RID: 4076 RVA: 0x00054C29 File Offset: 0x00052E29
		// (set) Token: 0x06000FED RID: 4077 RVA: 0x00054C31 File Offset: 0x00052E31
		public string ConfigurationFileName { get; set; }

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x06000FEE RID: 4078 RVA: 0x00054C3A File Offset: 0x00052E3A
		// (set) Token: 0x06000FEF RID: 4079 RVA: 0x00054C42 File Offset: 0x00052E42
		public string ConfigurationContent { get; set; }
	}
}
