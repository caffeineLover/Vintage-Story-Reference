using System;

namespace Xabe.FFmpeg.Exceptions
{
	// Token: 0x02000045 RID: 69
	public class FFmpegNoSuitableOutputFormatFoundException : ConversionException
	{
		// Token: 0x06000216 RID: 534 RVA: 0x00007204 File Offset: 0x00005404
		internal FFmpegNoSuitableOutputFormatFoundException(string errorMessage, string inputParameters) : base(errorMessage, inputParameters)
		{
		}
	}
}
