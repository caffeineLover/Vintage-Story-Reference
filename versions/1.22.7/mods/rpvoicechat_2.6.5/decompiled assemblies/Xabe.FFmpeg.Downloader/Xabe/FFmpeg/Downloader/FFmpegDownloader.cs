using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Xabe.FFmpeg.Downloader
{
	// Token: 0x02000003 RID: 3
	public abstract class FFmpegDownloader
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static Task GetLatestVersion(FFmpegVersion version, IProgress<ProgressInfo> progress = null)
		{
			FFmpegDownloader.<GetLatestVersion>d__0 <GetLatestVersion>d__;
			<GetLatestVersion>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<GetLatestVersion>d__.version = version;
			<GetLatestVersion>d__.progress = progress;
			<GetLatestVersion>d__.<>1__state = -1;
			<GetLatestVersion>d__.<>t__builder.Start<FFmpegDownloader.<GetLatestVersion>d__0>(ref <GetLatestVersion>d__);
			return <GetLatestVersion>d__.<>t__builder.Task;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000209C File Offset: 0x0000029C
		public static Task GetLatestVersion(FFmpegVersion version, string path, IProgress<ProgressInfo> progress = null)
		{
			FFmpegDownloader.<GetLatestVersion>d__1 <GetLatestVersion>d__;
			<GetLatestVersion>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<GetLatestVersion>d__.version = version;
			<GetLatestVersion>d__.path = path;
			<GetLatestVersion>d__.progress = progress;
			<GetLatestVersion>d__.<>1__state = -1;
			<GetLatestVersion>d__.<>t__builder.Start<FFmpegDownloader.<GetLatestVersion>d__1>(ref <GetLatestVersion>d__);
			return <GetLatestVersion>d__.<>t__builder.Task;
		}
	}
}
