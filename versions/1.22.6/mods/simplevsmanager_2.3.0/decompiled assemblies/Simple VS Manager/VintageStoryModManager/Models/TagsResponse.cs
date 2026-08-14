using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace VintageStoryModManager.Models
{
	// Token: 0x0200009B RID: 155
	[NullableContext(1)]
	[Nullable(0)]
	public class TagsResponse
	{
		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06000D4E RID: 3406 RVA: 0x00050BEB File Offset: 0x0004EDEB
		// (set) Token: 0x06000D4F RID: 3407 RVA: 0x00050BF3 File Offset: 0x0004EDF3
		[JsonPropertyName("tags")]
		public List<ModTag> Tags { get; set; } = new List<ModTag>();
	}
}
