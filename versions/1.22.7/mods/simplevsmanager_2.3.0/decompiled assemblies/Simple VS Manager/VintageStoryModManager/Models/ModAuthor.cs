using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace VintageStoryModManager.Models
{
	// Token: 0x020000A4 RID: 164
	[NullableContext(1)]
	[Nullable(0)]
	public class ModAuthor
	{
		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000DED RID: 3565 RVA: 0x00051814 File Offset: 0x0004FA14
		// (set) Token: 0x06000DEE RID: 3566 RVA: 0x0005181C File Offset: 0x0004FA1C
		[JsonPropertyName("userid")]
		[JsonConverter(typeof(FlexibleStringConverter))]
		public string UserId { get; set; } = string.Empty;

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000DEF RID: 3567 RVA: 0x00051825 File Offset: 0x0004FA25
		// (set) Token: 0x06000DF0 RID: 3568 RVA: 0x0005182D File Offset: 0x0004FA2D
		[JsonPropertyName("name")]
		public string Name { get; set; } = string.Empty;

		// Token: 0x06000DF1 RID: 3569 RVA: 0x00051836 File Offset: 0x0004FA36
		public override string ToString()
		{
			return this.Name;
		}
	}
}
