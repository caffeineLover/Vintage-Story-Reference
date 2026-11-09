using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Models
{
	// Token: 0x020000AB RID: 171
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class ModlistGroupDefinition
	{
		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06000EA0 RID: 3744 RVA: 0x000520B1 File Offset: 0x000502B1
		// (set) Token: 0x06000EA1 RID: 3745 RVA: 0x000520B9 File Offset: 0x000502B9
		public int SchemaVersion { get; set; } = 1;

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06000EA2 RID: 3746 RVA: 0x000520C2 File Offset: 0x000502C2
		// (set) Token: 0x06000EA3 RID: 3747 RVA: 0x000520CA File Offset: 0x000502CA
		public string Name { get; set; }

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06000EA4 RID: 3748 RVA: 0x000520D3 File Offset: 0x000502D3
		// (set) Token: 0x06000EA5 RID: 3749 RVA: 0x000520DB File Offset: 0x000502DB
		public string Description { get; set; }

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06000EA6 RID: 3750 RVA: 0x000520E4 File Offset: 0x000502E4
		// (set) Token: 0x06000EA7 RID: 3751 RVA: 0x000520EC File Offset: 0x000502EC
		public string Version { get; set; }

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06000EA8 RID: 3752 RVA: 0x000520F5 File Offset: 0x000502F5
		// (set) Token: 0x06000EA9 RID: 3753 RVA: 0x000520FD File Offset: 0x000502FD
		public string GameVersion { get; set; }

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06000EAA RID: 3754 RVA: 0x00052106 File Offset: 0x00050306
		// (set) Token: 0x06000EAB RID: 3755 RVA: 0x0005210E File Offset: 0x0005030E
		[Nullable(1)]
		public List<string> Members { [NullableContext(1)] get; [NullableContext(1)] set; } = new List<string>();

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06000EAC RID: 3756 RVA: 0x00052117 File Offset: 0x00050317
		// (set) Token: 0x06000EAD RID: 3757 RVA: 0x0005211F File Offset: 0x0005031F
		[Nullable(1)]
		public Dictionary<string, string> ConflictResolutions { [NullableContext(1)] get; [NullableContext(1)] set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
	}
}
