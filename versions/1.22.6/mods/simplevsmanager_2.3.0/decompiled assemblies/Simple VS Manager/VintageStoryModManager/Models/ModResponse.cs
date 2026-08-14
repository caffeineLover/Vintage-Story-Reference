using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace VintageStoryModManager.Models
{
	// Token: 0x02000098 RID: 152
	[NullableContext(2)]
	[Nullable(0)]
	public class ModResponse
	{
		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06000D43 RID: 3395 RVA: 0x00050B79 File Offset: 0x0004ED79
		// (set) Token: 0x06000D44 RID: 3396 RVA: 0x00050B81 File Offset: 0x0004ED81
		[JsonPropertyName("statuscode")]
		[JsonConverter(typeof(FlexibleIntConverter))]
		public int StatusCode { get; set; }

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06000D45 RID: 3397 RVA: 0x00050B8A File Offset: 0x0004ED8A
		// (set) Token: 0x06000D46 RID: 3398 RVA: 0x00050B92 File Offset: 0x0004ED92
		[JsonPropertyName("mod")]
		public DownloadableMod Mod { get; set; }
	}
}
