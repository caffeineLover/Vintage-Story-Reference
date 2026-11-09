using System;
using System.Runtime.InteropServices;

namespace Xabe.FFmpeg
{
	// Token: 0x02000022 RID: 34
	internal class OperatingSystemProvider : IOperatingSystemProvider
	{
		// Token: 0x060000F2 RID: 242 RVA: 0x000055CC File Offset: 0x000037CC
		public OperatingSystem GetOperatingSystem()
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				return OperatingSystem.Windows;
			}
			if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			{
				return OperatingSystem.Osx;
			}
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				return OperatingSystem.Linux;
			}
			throw new InvalidOperationException("Missing system type and architecture.");
		}
	}
}
