using System;
using System.Text.Json.Serialization;

namespace Xabe.FFmpeg.Downloader
{
	// Token: 0x0200000A RID: 10
	internal class BaseBinaries
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000019 RID: 25 RVA: 0x000026EB File Offset: 0x000008EB
		// (set) Token: 0x0600001A RID: 26 RVA: 0x000026F3 File Offset: 0x000008F3
		[JsonPropertyName("ffmpeg")]
		public string Ffmpeg { get; set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600001B RID: 27 RVA: 0x000026FC File Offset: 0x000008FC
		// (set) Token: 0x0600001C RID: 28 RVA: 0x00002704 File Offset: 0x00000904
		[JsonPropertyName("ffplay")]
		public string Ffplay { get; set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600001D RID: 29 RVA: 0x0000270D File Offset: 0x0000090D
		// (set) Token: 0x0600001E RID: 30 RVA: 0x00002715 File Offset: 0x00000915
		[JsonPropertyName("ffprobe")]
		public string Ffprobe { get; set; }
	}
}
