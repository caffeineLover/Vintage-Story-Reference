using System;
using Concentus.Celt;
using Concentus.Celt.Structs;
using Concentus.Common;
using Concentus.Common.CPlusPlus;
using Concentus.Structs;

namespace Concentus
{
	// Token: 0x02000008 RID: 8
	internal static class Analysis
	{
		// Token: 0x0600007F RID: 127 RVA: 0x000021C8 File Offset: 0x000003C8
		internal static float fast_atan2f(float y, float x)
		{
			if (Inlines.ABS16(x) + Inlines.ABS16(y) < 1E-09f)
			{
				x *= 1E+12f;
				y *= 1E+12f;
			}
			float num = x * x;
			float num2 = y * y;
			if (num < num2)
			{
				float num3 = (num2 + 0.678484f * num) * (num2 + 0.08595542f * num);
				if (num3 != 0f)
				{
					return -x * y * (num2 + 0.43157974f * num) / num3 + ((y < 0f) ? -1.5707964f : 1.5707964f);
				}
				if (y >= 0f)
				{
					return 1.5707964f;
				}
				return -1.5707964f;
			}
			else
			{
				float num4 = (num + 0.678484f * num2) * (num + 0.08595542f * num2);
				if (num4 != 0f)
				{
					return x * y * (num + 0.43157974f * num2) / num4 + ((y < 0f) ? -1.5707964f : 1.5707964f) - ((x * y < 0f) ? -1.5707964f : 1.5707964f);
				}
				return ((y < 0f) ? -1.5707964f : 1.5707964f) - ((x * y < 0f) ? -1.5707964f : 1.5707964f);
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000022E0 File Offset: 0x000004E0
		internal static void tonality_analysis_init(TonalityAnalysisState tonal)
		{
			tonal.Reset();
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000022E8 File Offset: 0x000004E8
		internal static void tonality_get_info(TonalityAnalysisState tonal, AnalysisInfo info_out, int len)
		{
			int num = tonal.read_pos;
			int num2 = tonal.write_pos - tonal.read_pos;
			if (num2 < 0)
			{
				num2 += 200;
			}
			if (len > 480 && num != tonal.write_pos)
			{
				num++;
				if (num == 200)
				{
					num = 0;
				}
			}
			if (num == tonal.write_pos)
			{
				num--;
			}
			if (num < 0)
			{
				num = 199;
			}
			info_out.Assign(tonal.info[num]);
			tonal.read_subframe += len / 120;
			while (tonal.read_subframe >= 4)
			{
				tonal.read_subframe -= 4;
				tonal.read_pos++;
			}
			if (tonal.read_pos >= 200)
			{
				tonal.read_pos -= 200;
			}
			num2 = Inlines.IMAX(num2 - 10, 0);
			float num3 = 0f;
			int i;
			for (i = 0; i < 200 - num2; i++)
			{
				num3 += tonal.pmusic[i];
			}
			while (i < 200)
			{
				num3 += tonal.pspeech[i];
				i++;
			}
			num3 = num3 * tonal.music_confidence + (1f - num3) * tonal.speech_confidence;
			info_out.music_prob = num3;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00002418 File Offset: 0x00000618
		internal static void tonality_analysis<T>(TonalityAnalysisState tonal, CeltMode celt_mode, ReadOnlySpan<T> x, int len, int offset, int c1, int c2, int C, int lsb_depth, Downmix.downmix_func<T> downmix)
		{
			int num = 480;
			int num2 = 240;
			float[] angle = tonal.angle;
			float[] d_angle = tonal.d_angle;
			float[] d2_angle = tonal.d2_angle;
			float[] array = new float[18];
			float[] array2 = new float[18];
			float[] array3 = new float[8];
			float[] array4 = new float[25];
			float num3 = 97.40909f;
			float num4 = 0f;
			float[] array5 = new float[2];
			tonal.last_transition++;
			float num5 = 1f / (float)Inlines.IMIN(20, 1 + tonal.count);
			float num6 = 1f / (float)Inlines.IMIN(50, 1 + tonal.count);
			float num7 = 1f / (float)Inlines.IMIN(1000, 1 + tonal.count);
			if (tonal.count < 4)
			{
				tonal.music_prob = 0.5f;
			}
			FFTState st = celt_mode.mdct.kfft[0];
			if (tonal.count == 0)
			{
				tonal.mem_fill = 240;
			}
			downmix.Invoke(x, tonal.inmem, tonal.mem_fill, Inlines.IMIN(len, 720 - tonal.mem_fill), offset, c1, c2, C);
			if (tonal.mem_fill + len < 720)
			{
				tonal.mem_fill += len;
				return;
			}
			AnalysisInfo[] info = tonal.info;
			int num8 = tonal.write_pos;
			tonal.write_pos = num8 + 1;
			AnalysisInfo analysisInfo = info[num8];
			if (tonal.write_pos >= 200)
			{
				tonal.write_pos -= 200;
			}
			int[] array6 = new int[960];
			int[] array7 = new int[960];
			float[] array8 = new float[240];
			float[] array9 = new float[240];
			for (int i = 0; i < num2; i++)
			{
				float num9 = Tables.analysis_window[i];
				array6[2 * i] = (int)(num9 * (float)tonal.inmem[i]);
				array6[2 * i + 1] = (int)(num9 * (float)tonal.inmem[num2 + i]);
				array6[2 * (num - i - 1)] = (int)(num9 * (float)tonal.inmem[num - i - 1]);
				array6[2 * (num - i - 1) + 1] = (int)(num9 * (float)tonal.inmem[num + num2 - i - 1]);
			}
			Arrays.MemMoveInt(tonal.inmem, 480, 0, 240);
			int num10 = len - (720 - tonal.mem_fill);
			downmix.Invoke(x, tonal.inmem, 240, num10, offset + 720 - tonal.mem_fill, c1, c2, C);
			tonal.mem_fill = 240 + num10;
			KissFFT.opus_fft(st, array6, array7);
			for (int i = 1; i < num2; i++)
			{
				float x2 = (float)array7[2 * i] + (float)array7[2 * (num - i)];
				float y = (float)array7[2 * i + 1] - (float)array7[2 * (num - i) + 1];
				float x3 = (float)array7[2 * i + 1] + (float)array7[2 * (num - i) + 1];
				float y2 = (float)array7[2 * (num - i)] - (float)array7[2 * i];
				float num11 = 0.15915494f * Analysis.fast_atan2f(y, x2);
				float num12 = num11 - angle[i];
				float num13 = num12 - d_angle[i];
				float num14 = 0.15915494f * Analysis.fast_atan2f(y2, x3);
				float num15 = num14 - num11;
				float num16 = num15 - num12;
				float num17 = num13 - (float)Math.Floor((double)(0.5f + num13));
				array9[i] = Inlines.ABS16(num17);
				num17 *= num17;
				num17 *= num17;
				float num18 = num16 - (float)Math.Floor((double)(0.5f + num16));
				array9[i] += Inlines.ABS16(num18);
				num18 *= num18;
				num18 *= num18;
				float num19 = 0.25f * (d2_angle[i] + 2f * num17 + num18);
				array8[i] = 1f / (1f + 640f * num3 * num19) - 0.015f;
				angle[i] = num14;
				d_angle[i] = num15;
				d2_angle[i] = num18;
			}
			float num20 = 0f;
			float num21 = 0f;
			analysisInfo.activity = 0f;
			float num22 = 0f;
			float num23 = 0f;
			if (tonal.count == 0)
			{
				for (int j = 0; j < 18; j++)
				{
					tonal.lowE[j] = 1E+10f;
					tonal.highE[j] = -1E+10f;
				}
			}
			float num24 = 0f;
			float num25 = 0f;
			for (int j = 0; j < 18; j++)
			{
				float num26 = 0f;
				float num27 = 0f;
				float num28 = 0f;
				for (int i = Tables.tbands[j]; i < Tables.tbands[j + 1]; i++)
				{
					float num29 = (float)array7[2 * i] * (float)array7[2 * i] + (float)array7[2 * (num - i)] * (float)array7[2 * (num - i)] + (float)array7[2 * i + 1] * (float)array7[2 * i + 1] + (float)array7[2 * (num - i) + 1] * (float)array7[2 * (num - i) + 1];
					num29 *= 5.55E-17f;
					num26 += num29;
					num27 += num29 * array8[i];
					num28 += num29 * 2f * (0.5f - array9[i]);
				}
				tonal.E[tonal.E_count][j] = num26;
				num22 += num28 / (1E-15f + num26);
				num25 += (float)Math.Sqrt((double)(num26 + 1E-10f));
				array2[j] = (float)Math.Log((double)(num26 + 1E-10f));
				tonal.lowE[j] = Inlines.MIN32(array2[j], tonal.lowE[j] + 0.01f);
				tonal.highE[j] = Inlines.MAX32(array2[j], tonal.highE[j] - 0.1f);
				if (tonal.highE[j] < tonal.lowE[j] + 1f)
				{
					tonal.highE[j] += 0.5f;
					tonal.lowE[j] -= 0.5f;
				}
				num24 += (array2[j] - tonal.lowE[j]) / (1E-15f + tonal.highE[j] - tonal.lowE[j]);
				float num31;
				float num30 = num31 = 0f;
				for (int i = 0; i < 8; i++)
				{
					num31 += (float)Math.Sqrt((double)tonal.E[i][j]);
					num30 += tonal.E[i][j];
				}
				float num32 = Inlines.MIN16(0.99f, num31 / (float)Math.Sqrt(1E-15 + (double)(8f * num30)));
				num32 *= num32;
				num32 *= num32;
				num23 += num32;
				array[j] = Inlines.MAX16(num27 / (1E-15f + num26), num32 * tonal.prev_band_tonality[j]);
				num20 += array[j];
				if (j >= 9)
				{
					num20 -= array[j - 18 + 9];
				}
				num21 = Inlines.MAX16(num21, (1f + 0.03f * (float)(j - 18)) * num20);
				num4 += array[j] * (float)(j - 8);
				tonal.prev_band_tonality[j] = array[j];
			}
			float num33 = 0f;
			int bandwidth = 0;
			float num34 = 0f;
			float num35 = 0.00057f / (float)(1 << Inlines.IMAX(0, lsb_depth - 8));
			num35 *= 134217730f;
			num35 *= num35;
			for (int j = 0; j < 21; j++)
			{
				float num36 = 0f;
				int num37 = Tables.extra_bands[j];
				int num38 = Tables.extra_bands[j + 1];
				for (int i = num37; i < num38; i++)
				{
					float num39 = (float)array7[2 * i] * (float)array7[2 * i] + (float)array7[2 * (num - i)] * (float)array7[2 * (num - i)] + (float)array7[2 * i + 1] * (float)array7[2 * i + 1] + (float)array7[2 * (num - i) + 1] * (float)array7[2 * (num - i) + 1];
					num36 += num39;
				}
				num34 = Inlines.MAX32(num34, num36);
				tonal.meanE[j] = Inlines.MAX32((1f - num7) * tonal.meanE[j], num36);
				num36 = Inlines.MAX32(num36, tonal.meanE[j]);
				num33 = Inlines.MAX32(0.05f * num33, num36);
				if ((double)num36 > 0.1 * (double)num33 && num36 * 1E+09f > num34 && num36 > num35 * (float)(num38 - num37))
				{
					bandwidth = j;
				}
			}
			if (tonal.count <= 2)
			{
				bandwidth = 20;
			}
			num25 = 20f * (float)Math.Log10((double)num25);
			tonal.Etracker = Inlines.MAX32(tonal.Etracker - 0.03f, num25);
			tonal.lowECount *= 1f - num6;
			if (num25 < tonal.Etracker - 30f)
			{
				tonal.lowECount += num6;
			}
			for (int i = 0; i < 8; i++)
			{
				float num40 = 0f;
				for (int j = 0; j < 16; j++)
				{
					num40 += Tables.dct_table[i * 16 + j] * array2[j];
				}
				array3[i] = num40;
			}
			num23 /= 18f;
			num24 /= 18f;
			if (tonal.count < 10)
			{
				num24 = 0.5f;
			}
			num22 /= 18f;
			analysisInfo.activity = num22 + (1f - num22) * num24;
			num20 = num21 / 9f;
			num20 = Inlines.MAX16(num20, tonal.prev_tonality * 0.8f);
			tonal.prev_tonality = num20;
			num4 /= 64f;
			analysisInfo.tonality_slope = num4;
			tonal.E_count = (tonal.E_count + 1) % 8;
			tonal.count++;
			analysisInfo.tonality = num20;
			for (int i = 0; i < 4; i++)
			{
				array4[i] = -0.12299f * (array3[i] + tonal.mem[i + 24]) + 0.49195f * (tonal.mem[i] + tonal.mem[i + 16]) + 0.69693f * tonal.mem[i + 8] - 1.4349f * tonal.cmean[i];
			}
			for (int i = 0; i < 4; i++)
			{
				tonal.cmean[i] = (1f - num5) * tonal.cmean[i] + num5 * array3[i];
			}
			for (int i = 0; i < 4; i++)
			{
				array4[4 + i] = 0.63246f * (array3[i] - tonal.mem[i + 24]) + 0.31623f * (tonal.mem[i] - tonal.mem[i + 16]);
			}
			for (int i = 0; i < 3; i++)
			{
				array4[8 + i] = 0.53452f * (array3[i] + tonal.mem[i + 24]) - 0.26726f * (tonal.mem[i] + tonal.mem[i + 16]) - 0.53452f * tonal.mem[i + 8];
			}
			if (tonal.count > 5)
			{
				for (int i = 0; i < 9; i++)
				{
					tonal.std[i] = (1f - num5) * tonal.std[i] + num5 * array4[i] * array4[i];
				}
			}
			for (int i = 0; i < 8; i++)
			{
				tonal.mem[i + 24] = tonal.mem[i + 16];
				tonal.mem[i + 16] = tonal.mem[i + 8];
				tonal.mem[i + 8] = tonal.mem[i];
				tonal.mem[i] = array3[i];
			}
			for (int i = 0; i < 9; i++)
			{
				array4[11 + i] = (float)Math.Sqrt((double)tonal.std[i]);
			}
			array4[20] = analysisInfo.tonality;
			array4[21] = analysisInfo.activity;
			array4[22] = num23;
			array4[23] = analysisInfo.tonality_slope;
			array4[24] = tonal.lowECount;
			MultiLayerPerceptron.mlp_process(Tables.net, array4, array5);
			array5[0] = 0.5f * (array5[0] + 1f);
			array5[0] = 0.01f + 1.21f * array5[0] * array5[0] - 0.23f * (float)Math.Pow((double)array5[0], 10.0);
			array5[1] = 0.5f * array5[1] + 0.5f;
			array5[0] = array5[1] * array5[0] + (1f - array5[1]) * 0.5f;
			float num41 = 5E-05f * array5[1];
			float num42 = Inlines.MAX16(0.05f, Inlines.MIN16(0.95f, array5[0]));
			float num43 = Inlines.MAX16(0.05f, Inlines.MIN16(0.95f, tonal.music_prob));
			float num44 = 0.01f + 0.05f * Inlines.ABS16(num42 - num43) / (num42 * (1f - num43) + num43 * (1f - num42));
			float num45 = (1f - tonal.music_prob) * (1f - num41) + tonal.music_prob * num41;
			float num46 = tonal.music_prob * (1f - num41) + (1f - tonal.music_prob) * num41;
			num45 *= (float)Math.Pow((double)(1f - array5[0]), (double)num44);
			num46 *= (float)Math.Pow((double)array5[0], (double)num44);
			tonal.music_prob = num46 / (num45 + num46);
			analysisInfo.music_prob = tonal.music_prob;
			float num47 = 1E-20f;
			float num48 = (float)Math.Pow((double)(1f - array5[0]), (double)num44);
			float num49 = (float)Math.Pow((double)array5[0], (double)num44);
			if (tonal.count == 1)
			{
				tonal.pspeech[0] = 0.5f;
				tonal.pmusic[0] = 0.5f;
			}
			float num50 = tonal.pspeech[0] + tonal.pspeech[1];
			float num51 = tonal.pmusic[0] + tonal.pmusic[1];
			tonal.pspeech[0] = num50 * (1f - num41) * num48;
			tonal.pmusic[0] = num51 * (1f - num41) * num49;
			for (int i = 1; i < 199; i++)
			{
				tonal.pspeech[i] = tonal.pspeech[i + 1] * num48;
				tonal.pmusic[i] = tonal.pmusic[i + 1] * num49;
			}
			tonal.pspeech[199] = num51 * num41 * num48;
			tonal.pmusic[199] = num50 * num41 * num49;
			for (int i = 0; i < 200; i++)
			{
				num47 += tonal.pspeech[i] + tonal.pmusic[i];
			}
			num47 = 1f / num47;
			for (int i = 0; i < 200; i++)
			{
				tonal.pspeech[i] *= num47;
				tonal.pmusic[i] *= num47;
			}
			num47 = tonal.pmusic[0];
			for (int i = 1; i < 200; i++)
			{
				num47 += tonal.pspeech[i];
			}
			if ((double)array5[1] > 0.75)
			{
				if ((double)tonal.music_prob > 0.9)
				{
					float num52 = 1f;
					num8 = tonal.music_confidence_count + 1;
					tonal.music_confidence_count = num8;
					float num53 = num52 / (float)num8;
					tonal.music_confidence_count = Inlines.IMIN(tonal.music_confidence_count, 500);
					tonal.music_confidence += num53 * Inlines.MAX16(-0.2f, array5[0] - tonal.music_confidence);
				}
				if ((double)tonal.music_prob < 0.1)
				{
					float num54 = 1f;
					num8 = tonal.speech_confidence_count + 1;
					tonal.speech_confidence_count = num8;
					float num55 = num54 / (float)num8;
					tonal.speech_confidence_count = Inlines.IMIN(tonal.speech_confidence_count, 500);
					tonal.speech_confidence += num55 * Inlines.MIN16(0.2f, array5[0] - tonal.speech_confidence);
				}
			}
			else
			{
				if (tonal.music_confidence_count == 0)
				{
					tonal.music_confidence = 0.9f;
				}
				if (tonal.speech_confidence_count == 0)
				{
					tonal.speech_confidence = 0.1f;
				}
			}
			if (tonal.last_music != ((tonal.music_prob > 0.5f) ? 1 : 0))
			{
				tonal.last_transition = 0;
			}
			tonal.last_music = ((tonal.music_prob > 0.5f) ? 1 : 0);
			analysisInfo.bandwidth = bandwidth;
			analysisInfo.noisiness = num22;
			analysisInfo.valid = 1;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x0000344C File Offset: 0x0000164C
		internal static void run_analysis<T>(TonalityAnalysisState analysis, CeltMode celt_mode, ReadOnlySpan<T> analysis_pcm, int analysis_frame_size, int frame_size, int c1, int c2, int C, int Fs, int lsb_depth, Downmix.downmix_func<T> downmix, AnalysisInfo analysis_info)
		{
			if (!analysis_pcm.IsEmpty)
			{
				analysis_frame_size = Inlines.IMIN(195 * Fs / 100, analysis_frame_size);
				int num = analysis_frame_size - analysis.analysis_offset;
				int num2 = analysis.analysis_offset;
				do
				{
					Analysis.tonality_analysis<T>(analysis, celt_mode, analysis_pcm, Inlines.IMIN(480, num), num2, c1, c2, C, lsb_depth, downmix);
					num2 += 480;
					num -= 480;
				}
				while (num > 0);
				analysis.analysis_offset = analysis_frame_size;
				analysis.analysis_offset -= frame_size;
			}
			analysis_info.valid = 0;
			Analysis.tonality_get_info(analysis, analysis_info, frame_size);
		}

		// Token: 0x04000005 RID: 5
		private const double M_PI = 3.141592653;

		// Token: 0x04000006 RID: 6
		private const float cA = 0.43157974f;

		// Token: 0x04000007 RID: 7
		private const float cB = 0.678484f;

		// Token: 0x04000008 RID: 8
		private const float cC = 0.08595542f;

		// Token: 0x04000009 RID: 9
		private const float cE = 1.5707964f;

		// Token: 0x0400000A RID: 10
		private const int NB_TONAL_SKIP_BANDS = 9;
	}
}
