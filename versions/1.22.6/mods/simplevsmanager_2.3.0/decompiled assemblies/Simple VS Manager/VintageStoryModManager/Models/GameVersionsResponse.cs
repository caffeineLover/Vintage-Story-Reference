using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace VintageStoryModManager.Models
{
	// Token: 0x0200009A RID: 154
	[NullableContext(1)]
	[Nullable(0)]
	public class GameVersionsResponse
	{
		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06000D4B RID: 3403 RVA: 0x00050BC7 File Offset: 0x0004EDC7
		// (set) Token: 0x06000D4C RID: 3404 RVA: 0x00050BCF File Offset: 0x0004EDCF
		[JsonPropertyName("gameversions")]
		public List<GameVersion> GameVersions { get; set; } = new List<GameVersion>();
	}
}
