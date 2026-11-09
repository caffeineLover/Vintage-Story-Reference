using System;
using System.Runtime.InteropServices;

namespace Xabe.FFmpeg
{
	// Token: 0x02000020 RID: 32
	internal class OperatingSystemArchitectureProvider : IOperatingSystemArchitectureProvider
	{
		// Token: 0x060000F0 RID: 240 RVA: 0x0000558C File Offset: 0x0000378C
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
