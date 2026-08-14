using System;
using System.Collections.Generic;
using OpenTK.Audio.OpenAL;

namespace RPVoiceChat.Audio
{
	// Token: 0x020000C6 RID: 198
	public interface IAudioCapture : IDisposable
	{
		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060006F8 RID: 1784
		int AvailableSamples { get; }

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060006F9 RID: 1785
		string CurrentDevice { get; }

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060006FA RID: 1786 RVA: 0x000276A8 File Offset: 0x000258A8
		string DefaultDevice { get; }

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060006FB RID: 1787
		ALFormat SampleFormat { get; }

		// Token: 0x060006FC RID: 1788
		void Start();

		// Token: 0x060006FD RID: 1789
		void Stop();

		// Token: 0x060006FE RID: 1790
		void ReadSamples(byte[] buffer, int count);

		// Token: 0x060006FF RID: 1791
		public abstract static List<string> GetAvailableDevices();
	}
}
