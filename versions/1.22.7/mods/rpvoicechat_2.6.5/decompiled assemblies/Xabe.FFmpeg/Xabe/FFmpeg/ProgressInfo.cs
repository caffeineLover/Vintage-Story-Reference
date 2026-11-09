using System;

namespace Xabe.FFmpeg
{
	// Token: 0x0200002A RID: 42
	public class ProgressInfo
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000126 RID: 294 RVA: 0x00005B75 File Offset: 0x00003D75
		// (set) Token: 0x06000127 RID: 295 RVA: 0x00005B7D File Offset: 0x00003D7D
		public long DownloadedBytes { get; set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000128 RID: 296 RVA: 0x00005B86 File Offset: 0x00003D86
		// (set) Token: 0x06000129 RID: 297 RVA: 0x00005B8E File Offset: 0x00003D8E
		public long TotalBytes { get; set; }

		// Token: 0x0600012A RID: 298 RVA: 0x00005B97 File Offset: 0x00003D97
		public ProgressInfo()
		{
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00005B9F File Offset: 0x00003D9F
		public ProgressInfo(long downloadedBytes, long totalBytes)
		{
			this.DownloadedBytes = downloadedBytes;
			this.TotalBytes = totalBytes;
		}
	}
}
