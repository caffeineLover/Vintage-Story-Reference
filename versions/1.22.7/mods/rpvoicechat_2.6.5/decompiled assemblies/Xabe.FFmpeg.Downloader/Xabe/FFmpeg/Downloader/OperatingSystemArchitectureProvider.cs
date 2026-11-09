using System;
using System.Runtime.InteropServices;

namespace Xabe.FFmpeg.Downloader
{
	// Token: 0x02000019 RID: 25
	internal class OperatingSystemArchitectureProvider : IOperatingSystemArchitectureProvider
	{
		// Token: 0x06000053 RID: 83 RVA: 0x00002AB4 File Offset: 0x00000CB4
		public OperatingSystemArchitecture GetArchitecture()
		{
			switch (RuntimeInformation.ProcessArchitecture)
			{
			case 0:
				return OperatingSystemArchitecture.X86;
			case 1:
				return OperatingSystemArchitecture.X64;
			case 2:
				return OperatingSystemArchitecture.Arm;
			case 3:
				return OperatingSystemArchitecture.Arm64;
			default:
				throw new NotImplementedException();
			}
		}
	}
}
