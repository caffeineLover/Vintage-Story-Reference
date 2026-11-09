using System;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Xabe.FFmpeg.Downloader
{
	// Token: 0x02000005 RID: 5
	internal class FullFFmpegDownloader : FFmpegDownloaderBase
	{
		// Token: 0x0600000E RID: 14 RVA: 0x00002413 File Offset: 0x00000613
		internal FullFFmpegDownloader()
		{
		}

		// Token: 0x0600000F RID: 15 RVA: 0x0000241B File Offset: 0x0000061B
		internal FullFFmpegDownloader(IOperatingSystemProvider operatingSystemProvider) : base(operatingSystemProvider)
		{
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002424 File Offset: 0x00000624
		private string GenerateLink()
		{
			switch (this._operatingSystemProvider.GetOperatingSystem())
			{
			case OperatingSystem.Windows64:
				return "https://xabe.net/ffmpeg/versions/ffmpeg-latest-win64-static.zip";
			case OperatingSystem.Windows32:
				return "https://xabe.net/ffmpeg/versions/ffmpeg-latest-win32-static.zip";
			case OperatingSystem.Osx64:
				return "https://xabe.net/ffmpeg/versions/ffmpeg-latest-macos64-static.zip";
			default:
				throw new NotSupportedException(string.Format("The automated download of the full FFmpeg package is not supported for the current Operation System: {0}.", this._operatingSystemProvider.GetOperatingSystem()));
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002484 File Offset: 0x00000684
		public override Task GetLatestVersion(string path, IProgress<ProgressInfo> progress = null, int retries = 6)
		{
			FullFFmpegDownloader.<GetLatestVersion>d__3 <GetLatestVersion>d__;
			<GetLatestVersion>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<GetLatestVersion>d__.<>4__this = this;
			<GetLatestVersion>d__.path = path;
			<GetLatestVersion>d__.progress = progress;
			<GetLatestVersion>d__.retries = retries;
			<GetLatestVersion>d__.<>1__state = -1;
			<GetLatestVersion>d__.<>t__builder.Start<FullFFmpegDownloader.<GetLatestVersion>d__3>(ref <GetLatestVersion>d__);
			return <GetLatestVersion>d__.<>t__builder.Task;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000024E0 File Offset: 0x000006E0
		protected override void Extract(string ffMpegZipPath, string destinationDir)
		{
			base.Extract(ffMpegZipPath, destinationDir, (ZipArchiveEntry item) => item.FullName.Contains("bin"), (ZipArchiveEntry item) => item.Name);
		}
	}
}
