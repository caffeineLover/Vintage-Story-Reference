using System;
using System.Text.Json.Serialization;

namespace Xabe.FFmpeg.Downloader
{
	// Token: 0x02000016 RID: 22
	internal class FFbinariesVersionInfo
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000047 RID: 71 RVA: 0x0000286C File Offset: 0x00000A6C
		// (set) Token: 0x06000048 RID: 72 RVA: 0x00002874 File Offset: 0x00000A74
		public string Version { get; set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000049 RID: 73 RVA: 0x0000287D File Offset: 0x00000A7D
		// (set) Token: 0x0600004A RID: 74 RVA: 0x00002885 File Offset: 0x00000A85
		[JsonPropertyName("bin")]
		public Bin BinariesUrl { get; set; }
	}
}
