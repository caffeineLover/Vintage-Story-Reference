using System;

namespace RPVoiceChat.Audio.Effects
{
	// Token: 0x020000D0 RID: 208
	public interface IDenoiser : IDisposable
	{
		// Token: 0x06000799 RID: 1945
		void Denoise(ref short[] pcms);

		// Token: 0x0600079A RID: 1946
		void SetBackgroundNoiseThreshold(float value);

		// Token: 0x0600079B RID: 1947
		void SetVoiceDenoisingStrength(float value);

		// Token: 0x0600079C RID: 1948
		bool SupportsFormat(int frequency, int channels, int bits);
	}
}
