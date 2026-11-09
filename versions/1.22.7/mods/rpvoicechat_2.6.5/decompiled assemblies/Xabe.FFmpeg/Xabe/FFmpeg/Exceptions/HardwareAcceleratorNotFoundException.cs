using System;

namespace Xabe.FFmpeg.Exceptions
{
	// Token: 0x02000047 RID: 71
	public class HardwareAcceleratorNotFoundException : ConversionException
	{
		// Token: 0x06000218 RID: 536 RVA: 0x00007217 File Offset: 0x00005417
		internal HardwareAcceleratorNotFoundException(string errorMessage, string inputParameters) : base(errorMessage, inputParameters)
		{
		}
	}
}
