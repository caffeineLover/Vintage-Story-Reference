using System;

namespace Xabe.FFmpeg.Exceptions
{
	// Token: 0x02000048 RID: 72
	public class InvalidBitstreamFilterException : ConversionException
	{
		// Token: 0x06000219 RID: 537 RVA: 0x00007221 File Offset: 0x00005421
		internal InvalidBitstreamFilterException(string errorMessage, string inputParameters) : base(errorMessage, inputParameters)
		{
		}
	}
}
