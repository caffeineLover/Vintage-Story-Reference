using System;
using System.Threading.Tasks;

namespace Xabe.FFmpeg.Downloader
{
	// Token: 0x02000006 RID: 6
	public interface IFFmpegDownloader
	{
		// Token: 0x06000013 RID: 19
		Task GetLatestVersion(string path, IProgress<ProgressInfo> progress = null, int retries = 6);
	}
}
