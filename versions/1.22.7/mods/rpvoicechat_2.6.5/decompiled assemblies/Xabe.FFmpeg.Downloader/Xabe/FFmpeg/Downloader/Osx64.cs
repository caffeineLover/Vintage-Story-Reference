using System;
using System.Text.Json.Serialization;

namespace Xabe.FFmpeg.Downloader
{
	// Token: 0x02000012 RID: 18
	internal class Osx64
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000027 RID: 39 RVA: 0x0000275E File Offset: 0x0000095E
		// (set) Token: 0x06000028 RID: 40 RVA: 0x00002766 File Offset: 0x00000966
		[JsonPropertyName("ffmpeg")]
		public string Ffmpeg { get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000029 RID: 41 RVA: 0x0000276F File Offset: 0x0000096F
		// (set) Token: 0x0600002A RID: 42 RVA: 0x00002777 File Offset: 0x00000977
		[JsonPropertyName("ffplay")]
		public string Ffplay { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00002780 File Offset: 0x00000980
		// (set) Token: 0x0600002C RID: 44 RVA: 0x00002788 File Offset: 0x00000988
		[JsonPropertyName("ffprobe")]
		public string Ffprobe { get; set; }
	}
}
