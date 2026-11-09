using System;
using System.IO;
using System.Runtime.CompilerServices;
using NVorbis;
using OpenTK.Audio.OpenAL;

namespace RPVoiceChat.Audio
{
	// Token: 0x02000107 RID: 263
	public static class PcmUtils
	{
		// Token: 0x060009BD RID: 2493 RVA: 0x00035F90 File Offset: 0x00034190
		public static void ApplyGainWithSoftClipping(ref byte[] data, ALFormat format, float gain)
		{
			if (gain <= 1f)
			{
				return;
			}
			if (data == null || data.Length < 2)
			{
				return;
			}
			if (format != 4353)
			{
				if (format != 4355)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(20, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported format: ");
					defaultInterpolatedStringHandler.AppendFormatted<ALFormat>(format);
					throw new NotSupportedException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			for (int i = 0; i < data.Length; i += 2)
			{
				short result = (short)Math.Clamp(PcmUtils.SoftClip((float)((short)((int)data[i] | (int)data[i + 1] << 8)) * gain), -32768f, 32767f);
				data[i] = (byte)(result & 255);
				data[i + 1] = (byte)(result >> 8 & 255);
			}
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x0003604C File Offset: 0x0003424C
		public static void ApplyCompressor(ref byte[] data, ALFormat format, float threshold = 0.6f, float ratio = 4f)
		{
			if (data == null || data.Length < 2)
			{
				return;
			}
			if (format != 4353)
			{
				if (format != 4355)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(20, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported format: ");
					defaultInterpolatedStringHandler.AppendFormatted<ALFormat>(format);
					throw new NotSupportedException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			for (int i = 0; i < data.Length; i += 2)
			{
				short result = (short)Math.Clamp(PcmUtils.CompressSample((float)((short)((int)data[i] | (int)data[i + 1] << 8)) / 32768f, threshold, ratio) * 32768f, -32768f, 32767f);
				data[i] = (byte)(result & 255);
				data[i + 1] = (byte)(result >> 8 & 255);
			}
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x00036108 File Offset: 0x00034308
		public static byte[] LoadOggToPCM(Stream stream, out int sampleRate, out int channels)
		{
			byte[] result;
			using (VorbisReader vorbis = new VorbisReader(stream, false))
			{
				sampleRate = vorbis.SampleRate;
				channels = vorbis.Channels;
				float[] buffer = new float[vorbis.TotalSamples * (long)channels];
				int samplesRead = vorbis.ReadSamples(buffer, 0, buffer.Length);
				byte[] pcmData = new byte[samplesRead * 2];
				for (int i = 0; i < samplesRead; i++)
				{
					short sample = (short)Math.Clamp(buffer[i] * 32767f, -32768f, 32767f);
					pcmData[i * 2] = (byte)(sample & 255);
					pcmData[i * 2 + 1] = (byte)(sample >> 8 & 255);
				}
				result = pcmData;
			}
			return result;
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x000361C4 File Offset: 0x000343C4
		private static float SoftClip(float x)
		{
			float norm = x / 32768f;
			if (Math.Abs(norm) < 0.95f)
			{
				return x;
			}
			return (float)Math.Sin((double)norm * 3.141592653589793 / 2.0) * 32768f;
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x0003620C File Offset: 0x0003440C
		private static float CompressSample(float sample, float threshold, float ratio)
		{
			float abs = Math.Abs(sample);
			if (abs < threshold)
			{
				return sample;
			}
			float excess = abs - threshold;
			float compressed = threshold + excess / ratio;
			return (float)Math.Sign(sample) * compressed;
		}
	}
}
