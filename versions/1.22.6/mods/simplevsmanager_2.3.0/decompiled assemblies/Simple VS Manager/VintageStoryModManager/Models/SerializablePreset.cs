using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Models
{
	// Token: 0x020000C0 RID: 192
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class SerializablePreset
	{
		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06000FCF RID: 4047 RVA: 0x00054B33 File Offset: 0x00052D33
		// (set) Token: 0x06000FD0 RID: 4048 RVA: 0x00054B3B File Offset: 0x00052D3B
		public string Name { get; set; }

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06000FD1 RID: 4049 RVA: 0x00054B44 File Offset: 0x00052D44
		// (set) Token: 0x06000FD2 RID: 4050 RVA: 0x00054B4C File Offset: 0x00052D4C
		public string Description { get; set; }

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06000FD3 RID: 4051 RVA: 0x00054B55 File Offset: 0x00052D55
		// (set) Token: 0x06000FD4 RID: 4052 RVA: 0x00054B5D File Offset: 0x00052D5D
		public string Version { get; set; }

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06000FD5 RID: 4053 RVA: 0x00054B66 File Offset: 0x00052D66
		// (set) Token: 0x06000FD6 RID: 4054 RVA: 0x00054B6E File Offset: 0x00052D6E
		public string GameVersion { get; set; }

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06000FD7 RID: 4055 RVA: 0x00054B77 File Offset: 0x00052D77
		// (set) Token: 0x06000FD8 RID: 4056 RVA: 0x00054B7F File Offset: 0x00052D7F
		public string Uploader { get; set; }

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06000FD9 RID: 4057 RVA: 0x00054B88 File Offset: 0x00052D88
		// (set) Token: 0x06000FDA RID: 4058 RVA: 0x00054B90 File Offset: 0x00052D90
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public List<string> DisabledEntries { [return: Nullable(new byte[]
		{
			2,
			1
		})] get; [param: Nullable(new byte[]
		{
			2,
			1
		})] set; }

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06000FDB RID: 4059 RVA: 0x00054B99 File Offset: 0x00052D99
		// (set) Token: 0x06000FDC RID: 4060 RVA: 0x00054BA1 File Offset: 0x00052DA1
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public List<SerializablePresetModState> Mods { [return: Nullable(new byte[]
		{
			2,
			1
		})] get; [param: Nullable(new byte[]
		{
			2,
			1
		})] set; }

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06000FDD RID: 4061 RVA: 0x00054BAA File Offset: 0x00052DAA
		// (set) Token: 0x06000FDE RID: 4062 RVA: 0x00054BB2 File Offset: 0x00052DB2
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public List<SerializableModConfiguration> Configurations { [return: Nullable(new byte[]
		{
			2,
			1
		})] get; [param: Nullable(new byte[]
		{
			2,
			1
		})] set; }

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06000FDF RID: 4063 RVA: 0x00054BBB File Offset: 0x00052DBB
		// (set) Token: 0x06000FE0 RID: 4064 RVA: 0x00054BC3 File Offset: 0x00052DC3
		public bool? IncludeModStatus { get; set; }

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06000FE1 RID: 4065 RVA: 0x00054BCC File Offset: 0x00052DCC
		// (set) Token: 0x06000FE2 RID: 4066 RVA: 0x00054BD4 File Offset: 0x00052DD4
		public bool? IncludeModVersions { get; set; }

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06000FE3 RID: 4067 RVA: 0x00054BDD File Offset: 0x00052DDD
		// (set) Token: 0x06000FE4 RID: 4068 RVA: 0x00054BE5 File Offset: 0x00052DE5
		public bool? Exclusive { get; set; }
	}
}
