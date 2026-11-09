using System;
using System.Collections.Generic;
using OpenTK.Audio.OpenAL;

namespace RPVoiceChat.Audio
{
	// Token: 0x020000FF RID: 255
	public interface IAudioCapture : IDisposable
	{
		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000913 RID: 2323
		int AvailableSamples { get; }

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000914 RID: 2324
		string CurrentDevice { get; }

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000915 RID: 2325 RVA: 0x00032BD0 File Offset: 0x00030DD0
		string DefaultDevice { get; }

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000916 RID: 2326
		ALFormat SampleFormat { get; }

		// Token: 0x06000917 RID: 2327
		void Start();

		// Token: 0x06000918 RID: 2328
		void Stop();

		// Token: 0x06000919 RID: 2329
		void ReadSamples(byte[] buffer, int count);

		// Token: 0x0600091A RID: 2330
		public abstract static List<string> GetAvailableDevices();
	}
}
