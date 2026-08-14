using System;
using System.Runtime.CompilerServices;
using OpenTK.Audio.OpenAL;

namespace RPVoiceChat.Util
{
	// Token: 0x02000024 RID: 36
	public static class AudioUtils
	{
		// Token: 0x060000DC RID: 220 RVA: 0x00006E90 File Offset: 0x00005090
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

		// Token: 0x060000DD RID: 221 RVA: 0x00006EFC File Offset: 0x000050FC
		public static byte[] ShortsToBytes(short[] audio, int offset, int length)
		{
			byte[] byteBuffer = new byte[length * 2];
			int bytesToCopy = (length - offset) * 2;
			Buffer.BlockCopy(audio, offset, byteBuffer, offset * 2, bytesToCopy);
			return byteBuffer;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00006F25 File Offset: 0x00005125
		public static float DBsToFactor(float dB)
		{
			return (float)Math.Pow(10.0, (double)(dB / 20f));
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00006F3E File Offset: 0x0000513E
		public static float FactorToDBs(float factor)
		{
			return (float)Math.Log10((double)factor) * 20f;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00006F50 File Offset: 0x00005150
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

		// Token: 0x060000E1 RID: 225 RVA: 0x00006F8C File Offset: 0x0000518C
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

		// Token: 0x060000E2 RID: 226 RVA: 0x00007170 File Offset: 0x00005370
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

		// Token: 0x020000DD RID: 221
		// (Invoke) Token: 0x060007CE RID: 1998
		public delegate double TransformDelegate(double progress);
	}
}
