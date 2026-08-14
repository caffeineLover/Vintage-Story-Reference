using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace VintageStoryModManager.Models
{
	// Token: 0x02000099 RID: 153
	[NullableContext(1)]
	[Nullable(0)]
	public class AuthorsResponse
	{
		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000D48 RID: 3400 RVA: 0x00050BA3 File Offset: 0x0004EDA3
		// (set) Token: 0x06000D49 RID: 3401 RVA: 0x00050BAB File Offset: 0x0004EDAB
		[JsonPropertyName("authors")]
		public List<ModAuthor> Authors { get; set; } = new List<ModAuthor>();
	}
}
