using System;

namespace RPVoiceChat.Audio.Effects
{
	// Token: 0x0200010B RID: 267
	public interface IDenoiser : IDisposable
	{
		// Token: 0x060009E6 RID: 2534
		void Denoise(ref short[] pcms);

		// Token: 0x060009E7 RID: 2535
		void SetBackgroundNoiseThreshold(float value);

		// Token: 0x060009E8 RID: 2536
		void SetVoiceDenoisingStrength(float value);

		// Token: 0x060009E9 RID: 2537
		bool SupportsFormat(int frequency, int channels, int bits);
	}
}
