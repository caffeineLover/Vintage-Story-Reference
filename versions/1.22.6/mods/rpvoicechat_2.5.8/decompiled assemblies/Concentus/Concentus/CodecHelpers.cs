using System;
using Concentus.Common;
using Concentus.Common.CPlusPlus;
using Concentus.Enums;
using Concentus.Silk;
using Concentus.Structs;

namespace Concentus
{
	// Token: 0x02000009 RID: 9
	internal static class CodecHelpers
	{
		// Token: 0x06000084 RID: 132 RVA: 0x000034E0 File Offset: 0x000016E0
		internal static byte gen_toc(OpusMode mode, int framerate, OpusBandwidth bandwidth, int channels)
		{
			int num = 0;
			while (framerate < 400)
			{
				framerate <<= 1;
				num++;
			}
			byte b;
			if (mode == OpusMode.MODE_SILK_ONLY)
			{
				b = (byte)(bandwidth - OpusBandwidth.OPUS_BANDWIDTH_NARROWBAND << 5);
				b |= (byte)(num - 2 << 3);
			}
			else if (mode == OpusMode.MODE_CELT_ONLY)
			{
				int num2 = bandwidth - OpusBandwidth.OPUS_BANDWIDTH_MEDIUMBAND;
				if (num2 < 0)
				{
					num2 = 0;
				}
				b = 128;
				b |= (byte)(num2 << 5);
				b |= (byte)(num << 3);
			}
			else
			{
				b = 96;
				b |= (byte)(bandwidth - OpusBandwidth.OPUS_BANDWIDTH_SUPERWIDEBAND << 4);
				b |= (byte)(num - 2 << 3);
			}
			return b | (((channels == 2) << 2) ? 1 : 0);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00003578 File Offset: 0x00001778
		internal static void hp_cutoff(ReadOnlySpan<short> input, int input_ptr, int cutoff_Hz, Span<short> output, int output_ptr, int[] hp_mem, int len, int channels, int Fs)
		{
			int[] array = new int[3];
			int[] array2 = new int[2];
			int num = Inlines.silk_DIV32_16(Inlines.silk_SMULBB(2471, cutoff_Hz), Fs / 1000);
			int num2 = 268435456 - Inlines.silk_MUL(471, num);
			array[0] = num2;
			array[1] = Inlines.silk_LSHIFT(-num2, 1);
			array[2] = num2;
			int num3 = Inlines.silk_RSHIFT(num2, 6);
			array2[0] = Inlines.silk_SMULWW(num3, Inlines.silk_SMULWW(num, num) - 8388608);
			array2[1] = Inlines.silk_SMULWW(num3, num3);
			Filters.silk_biquad_alt(input, input_ptr, array, array2, hp_mem, 0, output, output_ptr, len, channels);
			if (channels == 2)
			{
				Filters.silk_biquad_alt(input, input_ptr + 1, array, array2, hp_mem, 2, output, output_ptr + 1, len, channels);
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00003638 File Offset: 0x00001838
		internal unsafe static void dc_reject(ReadOnlySpan<short> input, int input_ptr, int cutoff_Hz, Span<short> output, int output_ptr, int[] hp_mem, int len, int channels, int Fs)
		{
			int shift = Inlines.celt_ilog2(Fs / (cutoff_Hz * 3));
			for (int i = 0; i < channels; i++)
			{
				for (int j = 0; j < len; j++)
				{
					int num = Inlines.SHL32(Inlines.EXTEND32(*input[channels * j + i + input_ptr]), 15);
					int num2 = num - hp_mem[2 * i];
					hp_mem[2 * i] = hp_mem[2 * i] + Inlines.PSHR32(num - hp_mem[2 * i], shift);
					int a = num2 - hp_mem[2 * i + 1];
					hp_mem[2 * i + 1] = hp_mem[2 * i + 1] + Inlines.PSHR32(num2 - hp_mem[2 * i + 1], shift);
					*output[channels * j + i + output_ptr] = Inlines.EXTRACT16(Inlines.SATURATE(Inlines.PSHR32(a, 15), 32767));
				}
			}
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003714 File Offset: 0x00001914
		internal static void stereo_fade(short[] pcm_buf, int g1, int g2, int overlap48, int frame_size, int channels, int[] window, int Fs)
		{
			int num = 48000 / Fs;
			int num2 = overlap48 / num;
			g1 = 32767 - g1;
			g2 = 32767 - g2;
			int i;
			for (i = 0; i < num2; i++)
			{
				int num3 = Inlines.MULT16_16_Q15(window[i * num], window[i * num]);
				int a = Inlines.SHR32(Inlines.MAC16_16(Inlines.MULT16_16(num3, g2), 32767 - num3, g1), 15);
				int num4 = (int)Inlines.EXTRACT16(Inlines.HALF32((int)(pcm_buf[i * channels] - pcm_buf[i * channels + 1])));
				num4 = Inlines.MULT16_16_Q15(a, num4);
				pcm_buf[i * channels] = (short)((int)pcm_buf[i * channels] - num4);
				pcm_buf[i * channels + 1] = (short)((int)pcm_buf[i * channels + 1] + num4);
			}
			while (i < frame_size)
			{
				int num5 = (int)Inlines.EXTRACT16(Inlines.HALF32((int)(pcm_buf[i * channels] - pcm_buf[i * channels + 1])));
				num5 = Inlines.MULT16_16_Q15(g2, num5);
				pcm_buf[i * channels] = (short)((int)pcm_buf[i * channels] - num5);
				pcm_buf[i * channels + 1] = (short)((int)pcm_buf[i * channels + 1] + num5);
				i++;
			}
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003814 File Offset: 0x00001A14
		internal static void gain_fade(short[] buffer, int buf_ptr, int g1, int g2, int overlap48, int frame_size, int channels, int[] window, int Fs)
		{
			int num = 48000 / Fs;
			int num2 = overlap48 / num;
			if (channels == 1)
			{
				for (int i = 0; i < num2; i++)
				{
					int num3 = Inlines.MULT16_16_Q15(window[i * num], window[i * num]);
					int a = Inlines.SHR32(Inlines.MAC16_16(Inlines.MULT16_16(num3, g2), 32767 - num3, g1), 15);
					buffer[buf_ptr + i] = (short)Inlines.MULT16_16_Q15(a, (int)buffer[buf_ptr + i]);
				}
			}
			else
			{
				for (int i = 0; i < num2; i++)
				{
					int num4 = Inlines.MULT16_16_Q15(window[i * num], window[i * num]);
					int a2 = Inlines.SHR32(Inlines.MAC16_16(Inlines.MULT16_16(num4, g2), 32767 - num4, g1), 15);
					buffer[buf_ptr + i * 2] = (short)Inlines.MULT16_16_Q15(a2, (int)buffer[buf_ptr + i * 2]);
					buffer[buf_ptr + i * 2 + 1] = (short)Inlines.MULT16_16_Q15(a2, (int)buffer[buf_ptr + i * 2 + 1]);
				}
			}
			int num5 = 0;
			do
			{
				for (int i = num2; i < frame_size; i++)
				{
					buffer[buf_ptr + i * channels + num5] = (short)Inlines.MULT16_16_Q15(g2, (int)buffer[buf_ptr + i * channels + num5]);
				}
			}
			while (++num5 < channels);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003928 File Offset: 0x00001B28
		internal unsafe static float transient_boost(Span<float> E, int E_ptr, float[] E_1, int LM, int maxM)
		{
			float num = 0f;
			float num2 = 0f;
			int num3 = Inlines.IMIN(maxM, (1 << LM) + 1);
			for (int i = E_ptr; i < num3 + E_ptr; i++)
			{
				num += *E[i];
				num2 += E_1[i];
			}
			float num4 = num * num2 / (float)(num3 * num3);
			return Inlines.MIN16(1f, (float)Math.Sqrt((double)Inlines.MAX16(0f, 0.05f * (num4 - 2f))));
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000039A4 File Offset: 0x00001BA4
		internal static int transient_viterbi(float[] E, float[] E_1, int N, int frame_cost, int rate)
		{
			float[][] array = Arrays.InitTwoDimensionalArray<float>(24, 16);
			int[][] array2 = Arrays.InitTwoDimensionalArray<int>(24, 16);
			float num;
			if (rate < 80)
			{
				num = 0f;
			}
			else if (rate > 160)
			{
				num = 1f;
			}
			else
			{
				num = ((float)rate - 80f) / 80f;
			}
			for (int i = 0; i < 16; i++)
			{
				array2[0][i] = -1;
				array[0][i] = 1E+10f;
			}
			for (int i = 0; i < 4; i++)
			{
				array[0][1 << i] = (float)(frame_cost + rate * (1 << i)) * (1f + num * CodecHelpers.transient_boost(E, 0, E_1, i, N + 1));
				array2[0][1 << i] = i;
			}
			for (int i = 1; i < N; i++)
			{
				for (int j = 2; j < 16; j++)
				{
					array[i][j] = array[i - 1][j - 1];
					array2[i][j] = j - 1;
				}
				for (int j = 0; j < 4; j++)
				{
					array2[i][1 << j] = 1;
					float num2 = array[i - 1][1];
					for (int k = 1; k < 4; k++)
					{
						float num3 = array[i - 1][(1 << k + 1) - 1];
						if (num3 < num2)
						{
							array2[i][1 << j] = (1 << k + 1) - 1;
							num2 = num3;
						}
					}
					float num4 = (float)(frame_cost + rate * (1 << j)) * (1f + num * CodecHelpers.transient_boost(E, i, E_1, j, N - i + 1));
					array[i][1 << j] = num2;
					if (N - i < 1 << j)
					{
						array[i][1 << j] += num4 * (float)(N - i) / (float)(1 << j);
					}
					else
					{
						array[i][1 << j] += num4;
					}
				}
			}
			int num5 = 1;
			float num6 = array[N - 1][1];
			for (int i = 2; i < 16; i++)
			{
				if (array[N - 1][i] < num6)
				{
					num6 = array[N - 1][i];
					num5 = i;
				}
			}
			for (int i = N - 1; i >= 0; i--)
			{
				num5 = array2[i][num5];
			}
			return num5;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00003BCC File Offset: 0x00001DCC
		internal static int optimize_framesize<T>(ReadOnlySpan<T> x, int len, int C, int Fs, int bitrate, int tonality, float[] mem, int buffering, Downmix.downmix_func<T> downmix)
		{
			float[] array = new float[28];
			float[] array2 = new float[27];
			int num = Fs / 400;
			int[] array3 = new int[num];
			array[0] = mem[0];
			array2[0] = 1f / (1f + mem[0]);
			int num2;
			int num3;
			if (buffering != 0)
			{
				num2 = 2 * num - buffering;
				len -= num2;
				array[1] = mem[1];
				array2[1] = 1f / (1f + mem[1]);
				array[2] = mem[2];
				array2[2] = 1f / (1f + mem[2]);
				num3 = 3;
			}
			else
			{
				num3 = 1;
				num2 = 0;
			}
			int num4 = Inlines.IMIN(len / num, 24);
			int num5 = 0;
			int i;
			for (i = 0; i < num4; i++)
			{
				float num6 = 1f;
				downmix.Invoke(x, array3, 0, num, i * num + num2, 0, -2, C);
				if (i == 0)
				{
					num5 = array3[0];
				}
				for (int j = 0; j < num; j++)
				{
					int num7 = array3[j];
					num6 += (float)(num7 - num5) * (float)(num7 - num5);
					num5 = num7;
				}
				array[i + num3] = num6;
				array2[i + num3] = 1f / num6;
			}
			array[i + num3] = array[i + num3 - 1];
			if (buffering != 0)
			{
				num4 = Inlines.IMIN(24, num4 + 2);
			}
			int num8 = CodecHelpers.transient_viterbi(array, array2, num4, (int)((1f + 0.5f * (float)tonality) * (float)(60 * C + 40)), bitrate / 400);
			mem[0] = array[1 << num8];
			if (buffering != 0)
			{
				mem[1] = array[(1 << num8) + 1];
				mem[2] = array[(1 << num8) + 2];
			}
			return num8;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00003D70 File Offset: 0x00001F70
		internal static int frame_size_select(int frame_size, OpusFramesize variable_duration, int Fs)
		{
			if (frame_size < Fs / 400)
			{
				return -1;
			}
			int num;
			if (variable_duration == OpusFramesize.OPUS_FRAMESIZE_ARG)
			{
				num = frame_size;
			}
			else if (variable_duration == OpusFramesize.OPUS_FRAMESIZE_VARIABLE)
			{
				num = Fs / 50;
			}
			else
			{
				if (variable_duration < OpusFramesize.OPUS_FRAMESIZE_2_5_MS || variable_duration > OpusFramesize.OPUS_FRAMESIZE_60_MS)
				{
					return -1;
				}
				num = Inlines.IMIN(3 * Fs / 50, Fs / 400 << variable_duration - OpusFramesize.OPUS_FRAMESIZE_2_5_MS);
			}
			if (num > frame_size)
			{
				return -1;
			}
			if (400 * num != Fs && 200 * num != Fs && 100 * num != Fs && 50 * num != Fs && 25 * num != Fs && 50 * num != 3 * Fs)
			{
				return -1;
			}
			return num;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00003E14 File Offset: 0x00002014
		internal static int compute_frame_size<T>(ReadOnlySpan<T> analysis_pcm, int frame_size, OpusFramesize variable_duration, int C, int Fs, int bitrate_bps, int delay_compensation, Downmix.downmix_func<T> downmix, float[] subframe_mem, bool analysis_enabled)
		{
			if (analysis_enabled && variable_duration == OpusFramesize.OPUS_FRAMESIZE_VARIABLE && frame_size >= Fs / 200)
			{
				int num = CodecHelpers.optimize_framesize<T>(analysis_pcm, frame_size, C, Fs, bitrate_bps, 0, subframe_mem, delay_compensation, downmix);
				while (Fs / 400 << num > frame_size)
				{
					num--;
				}
				frame_size = Fs / 400 << num;
			}
			else
			{
				frame_size = CodecHelpers.frame_size_select(frame_size, variable_duration, Fs);
			}
			if (frame_size < 0)
			{
				return -1;
			}
			return frame_size;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003E88 File Offset: 0x00002088
		internal unsafe static int compute_stereo_width(ReadOnlySpan<short> pcm, int pcm_ptr, int frame_size, int Fs, StereoWidthState mem)
		{
			int num = Fs / frame_size;
			int a = 32767 - 819175 / Inlines.IMAX(50, num);
			int num4;
			int num3;
			int num2 = num3 = (num4 = 0);
			for (int i = 0; i < frame_size - 3; i += 4)
			{
				int num5 = pcm_ptr + 2 * i;
				int num6 = (int)(*pcm[num5]);
				int num7 = (int)(*pcm[num5 + 1]);
				int num8 = Inlines.SHR32(Inlines.MULT16_16(num6, num6), 2);
				int num9 = Inlines.SHR32(Inlines.MULT16_16(num6, num7), 2);
				int num10 = Inlines.SHR32(Inlines.MULT16_16(num7, num7), 2);
				num6 = (int)(*pcm[num5 + 2]);
				num7 = (int)(*pcm[num5 + 3]);
				num8 += Inlines.SHR32(Inlines.MULT16_16(num6, num6), 2);
				num9 += Inlines.SHR32(Inlines.MULT16_16(num6, num7), 2);
				num10 += Inlines.SHR32(Inlines.MULT16_16(num7, num7), 2);
				num6 = (int)(*pcm[num5 + 4]);
				num7 = (int)(*pcm[num5 + 5]);
				num8 += Inlines.SHR32(Inlines.MULT16_16(num6, num6), 2);
				num9 += Inlines.SHR32(Inlines.MULT16_16(num6, num7), 2);
				num10 += Inlines.SHR32(Inlines.MULT16_16(num7, num7), 2);
				num6 = (int)(*pcm[num5 + 6]);
				num7 = (int)(*pcm[num5 + 7]);
				num8 += Inlines.SHR32(Inlines.MULT16_16(num6, num6), 2);
				num9 += Inlines.SHR32(Inlines.MULT16_16(num6, num7), 2);
				num10 += Inlines.SHR32(Inlines.MULT16_16(num7, num7), 2);
				num3 += Inlines.SHR32(num8, 10);
				num2 += Inlines.SHR32(num9, 10);
				num4 += Inlines.SHR32(num10, 10);
			}
			mem.XX += Inlines.MULT16_32_Q15(a, num3 - mem.XX);
			mem.XY += Inlines.MULT16_32_Q15(a, num2 - mem.XY);
			mem.YY += Inlines.MULT16_32_Q15(a, num4 - mem.YY);
			mem.XX = Inlines.MAX32(0, mem.XX);
			mem.XY = Inlines.MAX32(0, mem.XY);
			mem.YY = Inlines.MAX32(0, mem.YY);
			if (Inlines.MAX32(mem.XX, mem.YY) > 210)
			{
				int num11 = Inlines.celt_sqrt(mem.XX);
				int num12 = Inlines.celt_sqrt(mem.YY);
				int num13 = Inlines.celt_sqrt(num11);
				int num14 = Inlines.celt_sqrt(num12);
				mem.XY = Inlines.MIN32(mem.XY, num11 * num12);
				int num15 = Inlines.SHR32(Inlines.frac_div32(mem.XY, 1 + Inlines.MULT16_16(num11, num12)), 16);
				int b = 32767 * Inlines.ABS16(num13 - num14) / (1 + num13 + num14);
				int num16 = Inlines.MULT16_16_Q15(Inlines.celt_sqrt(1073741824 - Inlines.MULT16_16(num15, num15)), b);
				mem.smoothed_width += (num16 - mem.smoothed_width) / num;
				mem.max_follower = Inlines.MAX16(mem.max_follower - 655 / num, mem.smoothed_width);
			}
			return (int)Inlines.EXTRACT16(Inlines.MIN32(32767, 20 * mem.max_follower));
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00004210 File Offset: 0x00002410
		internal unsafe static void smooth_fade(Span<short> in1, int in1_ptr, Span<short> in2, int in2_ptr, Span<short> output, int output_ptr, int overlap, int channels, int[] window, int Fs)
		{
			int num = 48000 / Fs;
			for (int i = 0; i < channels; i++)
			{
				for (int j = 0; j < overlap; j++)
				{
					int num2 = Inlines.MULT16_16_Q15(window[j * num], window[j * num]);
					*output[output_ptr + j * channels + i] = (short)Inlines.SHR32(Inlines.MAC16_16(Inlines.MULT16_16(num2, (int)(*in2[in2_ptr + j * channels + i])), 32767 - num2, (int)(*in1[in1_ptr + j * channels + i])), 15);
				}
			}
		}

		// Token: 0x06000090 RID: 144 RVA: 0x000042A0 File Offset: 0x000024A0
		internal static string opus_strerror(int error)
		{
			string[] array = new string[]
			{
				"success",
				"invalid argument",
				"buffer too small",
				"internal error",
				"corrupted stream",
				"request not implemented",
				"invalid state",
				"memory allocation failed"
			};
			if (error > 0 || error < -7)
			{
				return "unknown error";
			}
			return array[-error];
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00004307 File Offset: 0x00002507
		internal static string GetVersionString()
		{
			return "Concentus 2.1.2";
		}

		// Token: 0x0400000B RID: 11
		private const int MAX_DYNAMIC_FRAMESIZE = 24;
	}
}
