using System;
using System.IO;
using System.Runtime.CompilerServices;
using NVorbis;
using OpenTK.Audio.OpenAL;

namespace RPVoiceChat.Audio
{
	// Token: 0x020000CE RID: 206
	public static class PcmUtils
	{
		// Token: 0x06000790 RID: 1936 RVA: 0x0002A300 File Offset: 0x00028500
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

		// Token: 0x06000791 RID: 1937 RVA: 0x0002A3BC File Offset: 0x000285BC
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

		// Token: 0x06000792 RID: 1938 RVA: 0x0002A478 File Offset: 0x00028678
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

		// Token: 0x06000793 RID: 1939 RVA: 0x0002A534 File Offset: 0x00028734
		private static float SoftClip(float x)
		{
			float norm = x / 32768f;
			if (Math.Abs(norm) < 0.95f)
			{
				return x;
			}
			return (float)Math.Sin((double)norm * 3.141592653589793 / 2.0) * 32768f;
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x0002A57C File Offset: 0x0002877C
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
