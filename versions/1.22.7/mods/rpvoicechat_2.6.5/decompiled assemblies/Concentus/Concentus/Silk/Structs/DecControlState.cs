using System;

namespace Concentus.Silk.Structs
{
	// Token: 0x02000042 RID: 66
	internal class DecControlState
	{
		// Token: 0x06000134 RID: 308 RVA: 0x00014291 File Offset: 0x00012491
		internal void Reset()
		{
			this.nChannelsAPI = 0;
			this.nChannelsInternal = 0;
			this.API_sampleRate = 0;
			this.internalSampleRate = 0;
			this.payloadSize_ms = 0;
			this.prevPitchLag = 0;
		}

		// Token: 0x04000186 RID: 390
		internal int nChannelsAPI;

		// Token: 0x04000187 RID: 391
		internal int nChannelsInternal;

		// Token: 0x04000188 RID: 392
		internal int API_sampleRate;

		// Token: 0x04000189 RID: 393
		internal int internalSampleRate;

		// Token: 0x0400018A RID: 394
		internal int payloadSize_ms;

		// Token: 0x0400018B RID: 395
		internal int prevPitchLag;
	}
}
