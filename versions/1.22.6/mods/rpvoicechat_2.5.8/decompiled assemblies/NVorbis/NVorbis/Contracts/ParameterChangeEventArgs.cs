using System;

namespace NVorbis.Contracts
{
	// Token: 0x02000034 RID: 52
	public class ParameterChangeEventArgs : EventArgs
	{
		// Token: 0x06000228 RID: 552 RVA: 0x0000868D File Offset: 0x0000688D
		public ParameterChangeEventArgs(int channels, int sampleRate)
		{
			this.Channels = channels;
			this.SampleRate = sampleRate;
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000229 RID: 553 RVA: 0x000086A3 File Offset: 0x000068A3
		public int Channels { get; }

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600022A RID: 554 RVA: 0x000086AB File Offset: 0x000068AB
		public int SampleRate { get; }
	}
}
