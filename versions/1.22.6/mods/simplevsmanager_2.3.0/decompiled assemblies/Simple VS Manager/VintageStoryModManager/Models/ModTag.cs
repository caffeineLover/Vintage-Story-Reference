using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace VintageStoryModManager.Models
{
	// Token: 0x020000BA RID: 186
	[NullableContext(1)]
	[Nullable(0)]
	public class ModTag
	{
		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06000FAB RID: 4011 RVA: 0x0005469C File Offset: 0x0005289C
		// (set) Token: 0x06000FAC RID: 4012 RVA: 0x000546A4 File Offset: 0x000528A4
		[JsonPropertyName("tagid")]
		[JsonConverter(typeof(FlexibleIntConverter))]
		public int TagId { get; set; }

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06000FAD RID: 4013 RVA: 0x000546AD File Offset: 0x000528AD
		// (set) Token: 0x06000FAE RID: 4014 RVA: 0x000546B5 File Offset: 0x000528B5
		[JsonPropertyName("name")]
		public string Name { get; set; } = string.Empty;

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x06000FAF RID: 4015 RVA: 0x000546BE File Offset: 0x000528BE
		// (set) Token: 0x06000FB0 RID: 4016 RVA: 0x000546C6 File Offset: 0x000528C6
		[JsonPropertyName("color")]
		public string Color { get; set; } = string.Empty;

		// Token: 0x06000FB1 RID: 4017 RVA: 0x000546CF File Offset: 0x000528CF
		public override string ToString()
		{
			return this.Name;
		}
	}
}
