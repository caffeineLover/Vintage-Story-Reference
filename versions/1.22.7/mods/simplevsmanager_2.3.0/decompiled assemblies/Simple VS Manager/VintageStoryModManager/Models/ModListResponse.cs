using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace VintageStoryModManager.Models
{
	// Token: 0x02000097 RID: 151
	[NullableContext(1)]
	[Nullable(0)]
	public class ModListResponse
	{
		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06000D40 RID: 3392 RVA: 0x00050B55 File Offset: 0x0004ED55
		// (set) Token: 0x06000D41 RID: 3393 RVA: 0x00050B5D File Offset: 0x0004ED5D
		[JsonPropertyName("mods")]
		public List<DownloadableModOnList> Mods { get; set; } = new List<DownloadableModOnList>();
	}
}
