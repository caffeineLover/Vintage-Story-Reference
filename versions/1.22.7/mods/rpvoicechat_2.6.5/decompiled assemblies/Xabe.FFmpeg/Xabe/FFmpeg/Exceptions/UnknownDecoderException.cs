using System;

namespace Xabe.FFmpeg.Exceptions
{
	// Token: 0x0200004A RID: 74
	public class UnknownDecoderException : ConversionException
	{
		// Token: 0x0600021B RID: 539 RVA: 0x00007234 File Offset: 0x00005434
		internal UnknownDecoderException(string errorMessage, string inputParameters) : base(errorMessage, inputParameters)
		{
		}
	}
}
