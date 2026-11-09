using System;

namespace Xabe.FFmpeg.Exceptions
{
	// Token: 0x02000042 RID: 66
	public class ConversionException : Exception
	{
		// Token: 0x0600020E RID: 526 RVA: 0x00006F11 File Offset: 0x00005111
		public ConversionException(string message, Exception innerException, string inputParameters) : base(message, innerException)
		{
			this.InputParameters = inputParameters;
		}

		// Token: 0x0600020F RID: 527 RVA: 0x00006F22 File Offset: 0x00005122
		internal ConversionException(string errorMessage, string inputParameters) : base(errorMessage)
		{
			this.InputParameters = inputParameters;
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000210 RID: 528 RVA: 0x00006F32 File Offset: 0x00005132
		public string InputParameters { get; }
	}
}
