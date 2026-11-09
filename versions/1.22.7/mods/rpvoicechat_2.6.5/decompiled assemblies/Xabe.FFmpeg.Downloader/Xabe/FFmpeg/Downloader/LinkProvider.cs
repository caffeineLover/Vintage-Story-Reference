using System;

namespace Xabe.FFmpeg.Downloader
{
	// Token: 0x02000009 RID: 9
	internal class LinkProvider
	{
		// Token: 0x06000016 RID: 22 RVA: 0x00002533 File Offset: 0x00000733
		internal LinkProvider()
		{
			this._operatingSystemProvider = new OperatingSystemProvider();
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002546 File Offset: 0x00000746
		internal LinkProvider(IOperatingSystemProvider operatingSystemProvider)
		{
			this._operatingSystemProvider = operatingSystemProvider;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002558 File Offset: 0x00000758
		internal Links GetLinks(FFbinariesVersionInfo version)
		{
			Links links = new Links();
			switch (this._operatingSystemProvider.GetOperatingSystem())
			{
			case OperatingSystem.Windows64:
				links.FFmpegLink = version.BinariesUrl.Windows64.Ffmpeg;
				links.FFprobeLink = version.BinariesUrl.Windows64.Ffprobe;
				break;
			case OperatingSystem.Windows32:
				links.FFmpegLink = version.BinariesUrl.Windows32.Ffmpeg;
				links.FFprobeLink = version.BinariesUrl.Windows32.Ffprobe;
				break;
			case OperatingSystem.Osx64:
				links.FFmpegLink = version.BinariesUrl.Osx64.Ffmpeg;
				links.FFprobeLink = version.BinariesUrl.Osx64.Ffprobe;
				break;
			case OperatingSystem.Linux64:
				links.FFmpegLink = version.BinariesUrl.Linux64.Ffmpeg;
				links.FFprobeLink = version.BinariesUrl.Linux64.Ffprobe;
				break;
			case OperatingSystem.Linux32:
				links.FFmpegLink = version.BinariesUrl.Linux32.Ffmpeg;
				links.FFprobeLink = version.BinariesUrl.Linux32.Ffprobe;
				break;
			case OperatingSystem.LinuxArmhf:
				links.FFmpegLink = version.BinariesUrl.LinuxArmhf.Ffmpeg;
				links.FFprobeLink = version.BinariesUrl.LinuxArmhf.Ffprobe;
				break;
			case OperatingSystem.LinuxArm64:
				links.FFmpegLink = version.BinariesUrl.LinuxArm64.Ffmpeg;
				links.FFprobeLink = version.BinariesUrl.LinuxArm64.Ffprobe;
				break;
			}
			return links;
		}

		// Token: 0x0400000C RID: 12
		private readonly IOperatingSystemProvider _operatingSystemProvider;
	}
}
