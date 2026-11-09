using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Xabe.FFmpeg.Downloader
{
	// Token: 0x02000004 RID: 4
	internal abstract class FFmpegDownloaderBase : IFFmpegDownloader
	{
		// Token: 0x06000004 RID: 4 RVA: 0x000020F7 File Offset: 0x000002F7
		protected FFmpegDownloaderBase(IOperatingSystemProvider operatingSystemProvider)
		{
			this._operatingSystemProvider = operatingSystemProvider;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000212E File Offset: 0x0000032E
		protected FFmpegDownloaderBase(IOperatingSystemArchitectureProvider operatingSystemArchitectureProvider)
		{
			this._operatingSystemArchitectureProvider = operatingSystemArchitectureProvider;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002168 File Offset: 0x00000368
		protected FFmpegDownloaderBase()
		{
			this._operatingSystemProvider = new OperatingSystemProvider();
			this._operatingSystemArchitectureProvider = new OperatingSystemArchitectureProvider();
		}

		// Token: 0x06000007 RID: 7
		public abstract Task GetLatestVersion(string path, IProgress<ProgressInfo> progress = null, int retries = 6);

		// Token: 0x06000008 RID: 8 RVA: 0x000021BC File Offset: 0x000003BC
		protected bool CheckIfFilesExist(string path)
		{
			if (this._operatingSystemProvider != null)
			{
				return !File.Exists(this.ComputeFileDestinationPath("ffmpeg", this._operatingSystemProvider.GetOperatingSystem(), path)) || !File.Exists(this.ComputeFileDestinationPath("ffprobe", this._operatingSystemProvider.GetOperatingSystem(), path));
			}
			return this._operatingSystemArchitectureProvider != null && (!File.Exists(this.ComputeFileDestinationPath("ffmpeg", path)) || !File.Exists(this.ComputeFileDestinationPath("ffprobe", path)));
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002244 File Offset: 0x00000444
		internal string ComputeFileDestinationPath(string filename, OperatingSystem os, string destinationPath)
		{
			string text = Path.Combine(destinationPath ?? ".", filename);
			if (os == OperatingSystem.Windows32 || os == OperatingSystem.Windows64)
			{
				text += ".exe";
			}
			return text;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002276 File Offset: 0x00000476
		internal string ComputeFileDestinationPath(string filename, string destinationPath)
		{
			return Path.Combine(destinationPath ?? ".", filename);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002288 File Offset: 0x00000488
		protected virtual void Extract(string ffMpegZipPath, string destinationDir)
		{
			this.Extract(ffMpegZipPath, destinationDir, (ZipArchiveEntry _) => true, (ZipArchiveEntry zipEntry) => zipEntry.FullName);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000022DC File Offset: 0x000004DC
		internal void Extract(string ffMpegZipPath, string destinationDir, Func<ZipArchiveEntry, bool> filter, Func<ZipArchiveEntry, string> getName)
		{
			destinationDir = Path.GetFullPath(destinationDir);
			using (ZipArchive zipArchive = ZipFile.OpenRead(ffMpegZipPath))
			{
				if (!Directory.Exists(destinationDir))
				{
					Directory.CreateDirectory(destinationDir);
				}
				foreach (ZipArchiveEntry zipArchiveEntry in Enumerable.Where<ZipArchiveEntry>(zipArchive.Entries, filter))
				{
					string fullPath = Path.GetFullPath(Path.Combine(destinationDir, getName.Invoke(zipArchiveEntry)));
					if (fullPath.StartsWith(destinationDir, 4))
					{
						if (string.IsNullOrEmpty(zipArchiveEntry.Name))
						{
							Directory.CreateDirectory(fullPath);
						}
						else
						{
							string directoryName = Path.GetDirectoryName(fullPath);
							if (!Directory.Exists(directoryName))
							{
								Directory.CreateDirectory(directoryName);
							}
							ZipFileExtensions.ExtractToFile(zipArchiveEntry, fullPath, true);
						}
					}
				}
			}
			File.Delete(ffMpegZipPath);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000023B8 File Offset: 0x000005B8
		protected Task<string> DownloadFile(string url, IProgress<ProgressInfo> progress, int retries)
		{
			FFmpegDownloaderBase.<DownloadFile>d__15 <DownloadFile>d__;
			<DownloadFile>d__.<>t__builder = AsyncTaskMethodBuilder<string>.Create();
			<DownloadFile>d__.<>4__this = this;
			<DownloadFile>d__.url = url;
			<DownloadFile>d__.progress = progress;
			<DownloadFile>d__.retries = retries;
			<DownloadFile>d__.<>1__state = -1;
			<DownloadFile>d__.<>t__builder.Start<FFmpegDownloaderBase.<DownloadFile>d__15>(ref <DownloadFile>d__);
			return <DownloadFile>d__.<>t__builder.Task;
		}

		// Token: 0x04000006 RID: 6
		public const int DEFAULT_MAX_RETRIES = 6;

		// Token: 0x04000007 RID: 7
		private readonly TimeSpan _initialDelay = TimeSpan.FromSeconds(1.0);

		// Token: 0x04000008 RID: 8
		private readonly TimeSpan _maxDelay = TimeSpan.FromMinutes(2.0);

		// Token: 0x04000009 RID: 9
		private const double DELAY_MULTIPLIER = 2.0;

		// Token: 0x0400000A RID: 10
		protected IOperatingSystemProvider _operatingSystemProvider;

		// Token: 0x0400000B RID: 11
		protected IOperatingSystemArchitectureProvider _operatingSystemArchitectureProvider;
	}
}
