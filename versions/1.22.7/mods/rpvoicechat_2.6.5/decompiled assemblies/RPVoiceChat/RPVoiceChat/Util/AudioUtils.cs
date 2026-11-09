using System;
using System.Runtime.CompilerServices;
using OpenTK.Audio.OpenAL;

namespace RPVoiceChat.Util
{
	// Token: 0x02000024 RID: 36
	public static class AudioUtils
	{
		// Token: 0x060000E4 RID: 228 RVA: 0x00007698 File Offset: 0x00005898
		public static int ChannelsPerFormat(ALFormat format)
		{
			int result;
			if (format != 4353)
			{
				if (format != 4355)
				{
					if (format != 4613)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
						defaultInterpolatedStringHandler..ctor(36, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Format ");
						defaultInterpolatedStringHandler.AppendFormatted<ALFormat>(format);
						defaultInterpolatedStringHandler.AppendLiteral(" is not supported for capture");
						throw new NotSupportedException(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					result = 4;
				}
				else
				{
					result = 2;
				}
			}
			else
			{
				result = 1;
			}
			return result;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00007704 File Offset: 0x00005904
		public static byte[] ShortsToBytes(short[] audio, int offset, int length)
		{
			byte[] byteBuffer = new byte[length * 2];
			int bytesToCopy = (length - offset) * 2;
			Buffer.BlockCopy(audio, offset, byteBuffer, offset * 2, bytesToCopy);
			return byteBuffer;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00007730 File Offset: 0x00005930
		public static short[] BytesToShorts(byte[] pcmBytes)
		{
			if (pcmBytes == null || pcmBytes.Length < 2)
			{
				return Array.Empty<short>();
			}
			int sampleCount = pcmBytes.Length / 2;
			short[] samples = new short[sampleCount];
			Buffer.BlockCopy(pcmBytes, 0, samples, 0, sampleCount * 2);
			return samples;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00007766 File Offset: 0x00005966
		public static float DBsToFactor(float dB)
		{
			return (float)Math.Pow(10.0, (double)(dB / 20f));
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x0000777F File Offset: 0x0000597F
		public static float FactorToDBs(float factor)
		{
			return (float)Math.Log10((double)factor) * 20f;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00007790 File Offset: 0x00005990
		public static void FadeEdges(byte[] data, int maxFadeDuration)
		{
			if (data == null || data.Length == 0)
			{
				return;
			}
			int sampleCount = data.Length / 2;
			int safeFadeDuration = Math.Min(maxFadeDuration, sampleCount / 2);
			if (safeFadeDuration <= 0)
			{
				return;
			}
			AudioUtils.FadeEdge(data, safeFadeDuration, false);
			AudioUtils.FadeEdge(data, safeFadeDuration, true);
		}

		// Token: 0x060000EA RID: 234 RVA: 0x000077CC File Offset: 0x000059CC
		public static void FadeEdge(byte[] data, int maxFadeDuration, bool isRightEdge)
		{
			if (data == null || data.Length < 2)
			{
				return;
			}
			int sampleCount = data.Length / 2;
			if (maxFadeDuration > sampleCount)
			{
				maxFadeDuration = sampleCount;
			}
			int startIndex = 0;
			int endIndex = maxFadeDuration;
			AudioUtils.TransformDelegate transform = (double e) => e;
			if (isRightEdge)
			{
				startIndex = Math.Max(0, sampleCount - maxFadeDuration);
				endIndex = sampleCount - 1;
				transform = ((double e) => 1.0 - e);
			}
			if (startIndex < 0 || startIndex >= sampleCount || endIndex >= sampleCount || startIndex >= endIndex)
			{
				Logger client = Logger.client;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(48, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid fade indices: start=");
				defaultInterpolatedStringHandler.AppendFormatted<int>(startIndex);
				defaultInterpolatedStringHandler.AppendLiteral(", end=");
				defaultInterpolatedStringHandler.AppendFormatted<int>(endIndex);
				defaultInterpolatedStringHandler.AppendLiteral(", sampleCount=");
				defaultInterpolatedStringHandler.AppendFormatted<int>(sampleCount);
				client.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			int zeroCrossingIndex = -1;
			double? lastPcm = default(double?);
			int i = startIndex;
			while (i < endIndex)
			{
				int byteIndex = i * 2;
				if (byteIndex + 2 <= data.Length)
				{
					double pcm = (double)BitConverter.ToInt16(data, byteIndex);
					if (lastPcm == null)
					{
						goto IL_15B;
					}
					double? num = pcm * lastPcm;
					double num2 = 0.0;
					if (num.GetValueOrDefault() > num2 & num != null)
					{
						goto IL_15B;
					}
					zeroCrossingIndex = i;
					if (!isRightEdge)
					{
						break;
					}
					lastPcm = new double?(pcm);
					IL_176:
					i++;
					continue;
					IL_15B:
					lastPcm = new double?(pcm);
					goto IL_176;
				}
				break;
			}
			if (zeroCrossingIndex == -1)
			{
				AudioUtils.Fade(data, startIndex, endIndex, transform);
				return;
			}
			int silenceFrom = 0;
			int silenceTo = zeroCrossingIndex * 2;
			if (isRightEdge)
			{
				silenceFrom = zeroCrossingIndex * 2;
				silenceTo = sampleCount * 2;
			}
			if (silenceFrom < 0 || silenceTo > data.Length || silenceFrom >= silenceTo)
			{
				return;
			}
			int silenceBytes = silenceTo - silenceFrom;
			Buffer.BlockCopy(new byte[silenceBytes], 0, data, silenceFrom, silenceBytes);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x000079B0 File Offset: 0x00005BB0
		public static void Fade(byte[] data, int startIndex, int endIndex, AudioUtils.TransformDelegate transform)
		{
			int fadeDuration = endIndex - startIndex;
			short[] pcmBuffer = new short[fadeDuration + 1];
			for (int i = startIndex; i <= endIndex; i++)
			{
				double pcm = (double)BitConverter.ToInt16(data, i * 2);
				int pcmIndex = i - startIndex;
				double fadeMultiplier = (double)pcmIndex / (double)fadeDuration;
				pcm *= transform(fadeMultiplier);
				pcmBuffer[pcmIndex] = (short)pcm;
			}
			Buffer.BlockCopy(pcmBuffer, 0, data, startIndex * 2, pcmBuffer.Length * 2);
		}

		// Token: 0x02000118 RID: 280
		// (Invoke) Token: 0x06000A1B RID: 2587
		public delegate double TransformDelegate(double progress);
	}
}
