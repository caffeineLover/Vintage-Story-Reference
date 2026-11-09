using System;
using System.Runtime.InteropServices;

namespace Xabe.FFmpeg.Downloader
{
	// Token: 0x0200001B RID: 27
	internal class OperatingSystemProvider : IOperatingSystemProvider
	{
		// Token: 0x06000055 RID: 85 RVA: 0x00002AF4 File Offset: 0x00000CF4
		public OperatingSystem GetOperatingSystem()
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				if (RuntimeInformation.OSArchitecture == null)
				{
					return OperatingSystem.Windows32;
				}
				if (RuntimeInformation.OSArchitecture == 1)
				{
					return OperatingSystem.Windows64;
				}
				if (RuntimeInformation.OSArchitecture == 2)
				{
					return OperatingSystem.Windows32;
				}
				if (RuntimeInformation.OSArchitecture == 3)
				{
					return OperatingSystem.Windows64;
				}
			}
			else
			{
				if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
				{
					return OperatingSystem.Osx64;
				}
				if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
				{
					switch (RuntimeInformation.OSArchitecture)
					{
					case 0:
						return OperatingSystem.Linux32;
					case 1:
						return OperatingSystem.Linux64;
					case 2:
						return OperatingSystem.LinuxArmhf;
					case 3:
						return OperatingSystem.LinuxArm64;
					}
				}
			}
			throw new InvalidOperationException("Missing system type and architecture.");
		}
	}
}
