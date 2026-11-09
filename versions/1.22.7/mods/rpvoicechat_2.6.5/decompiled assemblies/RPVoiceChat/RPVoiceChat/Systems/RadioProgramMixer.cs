using System;

namespace RPVoiceChat.Systems
{
	// Token: 0x02000038 RID: 56
	public static class RadioProgramMixer
	{
		// Token: 0x0600015B RID: 347 RVA: 0x00009FD0 File Offset: 0x000081D0
		public unsafe static void MixFrames(ReadOnlySpan<short> music, ReadOnlySpan<short> mic, Span<short> output)
		{
			int count = Math.Min(Math.Min(music.Length, mic.Length), output.Length);
			float musicMultiplier = (RadioProgramMixer.ComputeRms(mic, count) >= 0.012) ? 0.1f : 1f;
			for (int i = 0; i < count; i++)
			{
				float mixed = (float)(*music[i]) * 0.7f * musicMultiplier + (float)(*mic[i]) * 1f;
				*output[i] = (short)Math.Clamp(mixed, -32768f, 32767f);
			}
		}

		// Token: 0x0600015C RID: 348 RVA: 0x0000A068 File Offset: 0x00008268
		public unsafe static double ComputeRms(ReadOnlySpan<short> samples, int count)
		{
			if (count <= 0)
			{
				return 0.0;
			}
			double sum = 0.0;
			for (int i = 0; i < count; i++)
			{
				double normalized = (double)(*samples[i]) / 32767.0;
				sum += normalized * normalized;
			}
			return Math.Sqrt(sum / (double)count);
		}

		// Token: 0x0400008B RID: 139
		public const float MusicGain = 0.7f;

		// Token: 0x0400008C RID: 140
		public const float MicGain = 1f;

		// Token: 0x0400008D RID: 141
		public const float DuckMusicGain = 0.1f;

		// Token: 0x0400008E RID: 142
		public const double MicActivityThreshold = 0.012;
	}
}
