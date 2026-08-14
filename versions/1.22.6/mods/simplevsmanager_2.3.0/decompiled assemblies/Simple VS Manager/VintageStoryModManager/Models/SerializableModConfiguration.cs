using System;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Models
{
	// Token: 0x020000C3 RID: 195
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class SerializableModConfiguration
	{
		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x06000FF4 RID: 4084 RVA: 0x00054C6C File Offset: 0x00052E6C
		// (set) Token: 0x06000FF5 RID: 4085 RVA: 0x00054C74 File Offset: 0x00052E74
		public string ModId { get; set; }

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06000FF6 RID: 4086 RVA: 0x00054C7D File Offset: 0x00052E7D
		// (set) Token: 0x06000FF7 RID: 4087 RVA: 0x00054C85 File Offset: 0x00052E85
		public string FileName { get; set; }

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06000FF8 RID: 4088 RVA: 0x00054C8E File Offset: 0x00052E8E
		// (set) Token: 0x06000FF9 RID: 4089 RVA: 0x00054C96 File Offset: 0x00052E96
		public string RelativePath { get; set; }

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06000FFA RID: 4090 RVA: 0x00054C9F File Offset: 0x00052E9F
		// (set) Token: 0x06000FFB RID: 4091 RVA: 0x00054CA7 File Offset: 0x00052EA7
		public string Content { get; set; }
	}
}
