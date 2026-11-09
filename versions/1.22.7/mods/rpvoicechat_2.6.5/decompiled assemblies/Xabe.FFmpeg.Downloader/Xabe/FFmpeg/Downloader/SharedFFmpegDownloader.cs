using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Xabe.FFmpeg.Downloader
{
	// Token: 0x0200001C RID: 28
	internal class SharedFFmpegDownloader : FFmpegDownloaderBase
	{
		// Token: 0x06000057 RID: 87 RVA: 0x00002B86 File Offset: 0x00000D86
		internal SharedFFmpegDownloader()
		{
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002B8E File Offset: 0x00000D8E
		internal SharedFFmpegDownloader(IOperatingSystemProvider operatingSystemProvider) : base(operatingSystemProvider)
		{
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002B98 File Offset: 0x00000D98
		private string GenerateLink()
		{
			switch (this._operatingSystemProvider.GetOperatingSystem())
			{
			case OperatingSystem.Windows64:
				return "https://xabe.net/ffmpeg/versions/ffmpeg-latest-win64-shared.zip";
			case OperatingSystem.Windows32:
				return "https://xabe.net/ffmpeg/versions/ffmpeg-latest-win32-shared.zip";
			case OperatingSystem.Osx64:
				return "https://xabe.net/ffmpeg/versions/ffmpeg-latest-macos64-shared.zip";
			default:
				throw new NotSupportedException(string.Format("The automated download of the full Shared FFmpeg package is not supported for the current Operation System: {0}.", this._operatingSystemProvider.GetOperatingSystem()));
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002BF8 File Offset: 0x00000DF8
		public override Task GetLatestVersion(string path, IProgress<ProgressInfo> progress = null, int retries = 6)
		{
			SharedFFmpegDownloader.<GetLatestVersion>d__3 <GetLatestVersion>d__;
			<GetLatestVersion>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<GetLatestVersion>d__.<>4__this = this;
			<GetLatestVersion>d__.path = path;
			<GetLatestVersion>d__.progress = progress;
			<GetLatestVersion>d__.retries = retries;
			<GetLatestVersion>d__.<>1__state = -1;
			<GetLatestVersion>d__.<>t__builder.Start<SharedFFmpegDownloader.<GetLatestVersion>d__3>(ref <GetLatestVersion>d__);
			return <GetLatestVersion>d__.<>t__builder.Task;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002C54 File Offset: 0x00000E54
		protected override void Extract(string ffMpegZipPath, string destinationDir)
		{
			using (ZipArchive zipArchive = ZipFile.OpenRead(ffMpegZipPath))
			{
				if (!Directory.Exists(destinationDir))
				{
					Directory.CreateDirectory(destinationDir);
				}
				foreach (ZipArchiveEntry zipArchiveEntry in Enumerable.Where<ZipArchiveEntry>(zipArchive.Entries, (ZipArchiveEntry item) => item.FullName.Contains("bin")))
				{
					string text = Path.Combine(destinationDir, zipArchiveEntry.Name);
					if (zipArchiveEntry.Name == string.Empty)
					{
						Directory.CreateDirectory(text);
					}
					else
					{
						ZipFileExtensions.ExtractToFile(zipArchiveEntry, text, true);
					}
				}
			}
			File.Delete(ffMpegZipPath);
		}
	}
}
