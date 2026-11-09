using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace VintageStoryModManager.Models
{
	// Token: 0x020000A3 RID: 163
	[NullableContext(1)]
	[Nullable(0)]
	public class GameVersion
	{
		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000DE5 RID: 3557 RVA: 0x000517BB File Offset: 0x0004F9BB
		// (set) Token: 0x06000DE6 RID: 3558 RVA: 0x000517C3 File Offset: 0x0004F9C3
		[JsonPropertyName("tagid")]
		public long TagId { get; set; }

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000DE7 RID: 3559 RVA: 0x000517CC File Offset: 0x0004F9CC
		// (set) Token: 0x06000DE8 RID: 3560 RVA: 0x000517D4 File Offset: 0x0004F9D4
		[JsonPropertyName("name")]
		public string Name { get; set; } = string.Empty;

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000DE9 RID: 3561 RVA: 0x000517DD File Offset: 0x0004F9DD
		// (set) Token: 0x06000DEA RID: 3562 RVA: 0x000517E5 File Offset: 0x0004F9E5
		[JsonPropertyName("color")]
		public string Color { get; set; } = string.Empty;

		// Token: 0x06000DEB RID: 3563 RVA: 0x000517EE File Offset: 0x0004F9EE
		public override string ToString()
		{
			return this.Name;
		}
	}
}
