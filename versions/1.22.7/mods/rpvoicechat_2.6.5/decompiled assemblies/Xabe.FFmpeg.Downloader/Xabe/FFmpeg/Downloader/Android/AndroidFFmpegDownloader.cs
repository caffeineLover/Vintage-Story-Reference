using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Xabe.FFmpeg.Downloader.Android
{
	// Token: 0x0200001D RID: 29
	internal class AndroidFFmpegDownloader : FFmpegDownloaderBase
	{
		// Token: 0x0600005C RID: 92 RVA: 0x00002D20 File Offset: 0x00000F20
		public AndroidFFmpegDownloader()
		{
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00002D28 File Offset: 0x00000F28
		public AndroidFFmpegDownloader(IOperatingSystemArchitectureProvider archProvider) : base(archProvider)
		{
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00002D31 File Offset: 0x00000F31
		private string GenerateLink(OperatingSystemArchitecture arch)
		{
			if (arch == OperatingSystemArchitecture.Arm64)
			{
				return "https://xabe.net/ffmpeg/versions/ffmpeg-android-arm64.zip";
			}
			if (arch == OperatingSystemArchitecture.Arm)
			{
				return "https://xabe.net/ffmpeg/versions/ffmpeg-android-arm.zip";
			}
			if (arch == OperatingSystemArchitecture.X86)
			{
				return "https://xabe.net/ffmpeg/versions/ffmpeg-android-x86.zip";
			}
			if (arch == OperatingSystemArchitecture.X64)
			{
				return "https://xabe.net/ffmpeg/versions/ffmpeg-android-x86_64.zip";
			}
			return string.Empty;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00002D60 File Offset: 0x00000F60
		public override Task GetLatestVersion(string path, IProgress<ProgressInfo> progress = null, int retries = 6)
		{
			AndroidFFmpegDownloader.<GetLatestVersion>d__3 <GetLatestVersion>d__;
			<GetLatestVersion>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<GetLatestVersion>d__.<>4__this = this;
			<GetLatestVersion>d__.path = path;
			<GetLatestVersion>d__.progress = progress;
			<GetLatestVersion>d__.retries = retries;
			<GetLatestVersion>d__.<>1__state = -1;
			<GetLatestVersion>d__.<>t__builder.Start<AndroidFFmpegDownloader.<GetLatestVersion>d__3>(ref <GetLatestVersion>d__);
			return <GetLatestVersion>d__.<>t__builder.Task;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00002DBC File Offset: 0x00000FBC
		protected Task GetLatestVersionForArchitecture(string path, OperatingSystemArchitecture arch, IProgress<ProgressInfo> progress = null, int retries = 6)
		{
			AndroidFFmpegDownloader.<GetLatestVersionForArchitecture>d__4 <GetLatestVersionForArchitecture>d__;
			<GetLatestVersionForArchitecture>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<GetLatestVersionForArchitecture>d__.<>4__this = this;
			<GetLatestVersionForArchitecture>d__.path = path;
			<GetLatestVersionForArchitecture>d__.arch = arch;
			<GetLatestVersionForArchitecture>d__.progress = progress;
			<GetLatestVersionForArchitecture>d__.retries = retries;
			<GetLatestVersionForArchitecture>d__.<>1__state = -1;
			<GetLatestVersionForArchitecture>d__.<>t__builder.Start<AndroidFFmpegDownloader.<GetLatestVersionForArchitecture>d__4>(ref <GetLatestVersionForArchitecture>d__);
			return <GetLatestVersionForArchitecture>d__.<>t__builder.Task;
		}
	}
}
