using System;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;

namespace Xabe.FFmpeg.Downloader
{
	// Token: 0x02000017 RID: 23
	internal class OfficialFFmpegDownloader : FFmpegDownloaderBase
	{
		// Token: 0x0600004C RID: 76 RVA: 0x00002896 File Offset: 0x00000A96
		internal OfficialFFmpegDownloader()
		{
			this._linkProvider = new LinkProvider(this._operatingSystemProvider);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x000028D3 File Offset: 0x00000AD3
		internal OfficialFFmpegDownloader(IOperatingSystemProvider operatingSystemProvider) : base(operatingSystemProvider)
		{
			this._linkProvider = new LinkProvider(operatingSystemProvider);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x0000290C File Offset: 0x00000B0C
		public override Task GetLatestVersion(string path, IProgress<ProgressInfo> progress = null, int retries = 6)
		{
			OfficialFFmpegDownloader.<GetLatestVersion>d__4 <GetLatestVersion>d__;
			<GetLatestVersion>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<GetLatestVersion>d__.<>4__this = this;
			<GetLatestVersion>d__.path = path;
			<GetLatestVersion>d__.progress = progress;
			<GetLatestVersion>d__.retries = retries;
			<GetLatestVersion>d__.<>1__state = -1;
			<GetLatestVersion>d__.<>t__builder.Start<OfficialFFmpegDownloader.<GetLatestVersion>d__4>(ref <GetLatestVersion>d__);
			return <GetLatestVersion>d__.<>t__builder.Task;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002968 File Offset: 0x00000B68
		internal FFbinariesVersionInfo GetLatestVersionInfo()
		{
			FFbinariesVersionInfo result;
			using (WebClient webClient = new WebClient())
			{
				result = JsonSerializer.Deserialize<FFbinariesVersionInfo>(webClient.DownloadString("https://ffbinaries.com/api/v1/version/latest"), this._defaultSerializerOptions);
			}
			return result;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000029B0 File Offset: 0x00000BB0
		internal Task DownloadLatestVersion(FFbinariesVersionInfo latestFFmpegBinaries, string path, IProgress<ProgressInfo> progress = null, int retries = 6)
		{
			OfficialFFmpegDownloader.<DownloadLatestVersion>d__6 <DownloadLatestVersion>d__;
			<DownloadLatestVersion>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<DownloadLatestVersion>d__.<>4__this = this;
			<DownloadLatestVersion>d__.latestFFmpegBinaries = latestFFmpegBinaries;
			<DownloadLatestVersion>d__.path = path;
			<DownloadLatestVersion>d__.progress = progress;
			<DownloadLatestVersion>d__.retries = retries;
			<DownloadLatestVersion>d__.<>1__state = -1;
			<DownloadLatestVersion>d__.<>t__builder.Start<OfficialFFmpegDownloader.<DownloadLatestVersion>d__6>(ref <DownloadLatestVersion>d__);
			return <DownloadLatestVersion>d__.<>t__builder.Task;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002A14 File Offset: 0x00000C14
		private bool CheckIfUpdateAvailable(string latestVersion, string path)
		{
			string text = Path.Combine(path ?? ".", "version.json");
			if (!File.Exists(text))
			{
				return true;
			}
			FFbinariesVersionInfo ffbinariesVersionInfo = JsonSerializer.Deserialize<FFbinariesVersionInfo>(File.ReadAllText(text), this._defaultSerializerOptions);
			return ffbinariesVersionInfo != null && new Version(latestVersion) > new Version(ffbinariesVersionInfo.Version);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002A70 File Offset: 0x00000C70
		internal void SaveVersion(FFbinariesVersionInfo latestVersion, string path)
		{
			string text = Path.Combine(path ?? ".", "version.json");
			DownloadedVersion downloadedVersion = new DownloadedVersion
			{
				Version = latestVersion.Version
			};
			File.WriteAllText(text, JsonSerializer.Serialize<DownloadedVersion>(downloadedVersion, this._defaultSerializerOptions));
		}

		// Token: 0x04000020 RID: 32
		private readonly JsonSerializerOptions _defaultSerializerOptions = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
			NumberHandling = 1,
			WriteIndented = true
		};

		// Token: 0x04000021 RID: 33
		private readonly LinkProvider _linkProvider;
	}
}
