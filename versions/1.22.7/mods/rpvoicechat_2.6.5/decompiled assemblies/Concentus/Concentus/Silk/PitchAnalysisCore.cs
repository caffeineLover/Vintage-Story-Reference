using System;
using Concentus.Celt;
using Concentus.Common;
using Concentus.Common.CPlusPlus;

namespace Concentus.Silk
{
	// Token: 0x0200002F RID: 47
	internal static class PitchAnalysisCore
	{
		// Token: 0x060000F9 RID: 249 RVA: 0x0000DCE0 File Offset: 0x0000BEE0
		internal static int silk_pitch_analysis_core(short[] frame, int[] pitch_out, BoxedValueShort lagIndex, BoxedValueSbyte contourIndex, BoxedValueInt LTPCorr_Q15, int prevLag, int search_thres1_Q16, int search_thres2_Q13, int Fs_kHz, int complexity, int nb_subfr)
		{
			int[] array = new int[6];
			int[] array2 = new int[24];
			int[] array3 = new int[11];
			int num = (20 + nb_subfr * 5) * Fs_kHz;
			int num2 = (20 + nb_subfr * 5) * 4;
			int num3 = (20 + nb_subfr * 5) * 8;
			int num4 = 5 * Fs_kHz;
			int num5 = 2 * Fs_kHz;
			int num6 = 18 * Fs_kHz - 1;
			short[] array4 = new short[num3];
			if (Fs_kHz == 16)
			{
				Arrays.MemSetInt(array, 0, 2);
				Resampler.silk_resampler_down2(array, array4, frame, num);
			}
			else if (Fs_kHz == 12)
			{
				Arrays.MemSetInt(array, 0, 6);
				Resampler.silk_resampler_down2_3(array, array4, frame, num);
			}
			else
			{
				Arrays.MemCopy(frame, 0, array4, 0, num3);
			}
			Arrays.MemSetInt(array, 0, 2);
			short[] array5 = new short[num2];
			Resampler.silk_resampler_down2(array, array5, array4, num3);
			for (int i = num2 - 1; i > 0; i--)
			{
				array5[i] = Inlines.silk_ADD_SAT16(array5[i], array5[i - 1]);
			}
			int num7;
			int num8;
			SumSqrShift.silk_sum_sqr_shift(out num7, out num8, array5, num2);
			if (num8 > 0)
			{
				num8 = Inlines.silk_RSHIFT(num8, 1);
				for (int i = 0; i < num2; i++)
				{
					array5[i] = Inlines.silk_RSHIFT16(array5[i], num8);
				}
			}
			short[] array6 = new short[nb_subfr * 132];
			int[] array7 = new int[65];
			Arrays.MemSetShort(array6, 0, (nb_subfr >> 1) * 65);
			short[] array8 = array5;
			int num9 = Inlines.silk_LSHIFT(20, 2);
			for (int j = 0; j < nb_subfr >> 1; j++)
			{
				short[] array9 = array8;
				int num10 = num9 - 8;
				CeltPitchXCorr.pitch_xcorr(array8, num9, array8, num9 - 72, array7, 40, 65);
				int num11 = array7[64];
				int num12 = Inlines.silk_inner_prod_self(array8, num9, 40);
				num12 = Inlines.silk_ADD32(num12, Inlines.silk_inner_prod_self(array9, num10, 40));
				num12 = Inlines.silk_ADD32(num12, Inlines.silk_SMULBB(40, 4000));
				Inlines.MatrixSet<short>(array6, j, 0, 65, (short)Inlines.silk_DIV32_varQ(num11, num12, 14));
				for (int k = 9; k <= 72; k++)
				{
					num10--;
					num11 = array7[72 - k];
					num12 = Inlines.silk_ADD32(num12, Inlines.silk_SMULBB((int)array9[num10], (int)array9[num10]) - Inlines.silk_SMULBB((int)array9[num10 + 40], (int)array9[num10 + 40]));
					Inlines.MatrixSet<short>(array6, j, k - 8, 65, (short)Inlines.silk_DIV32_varQ(num11, num12, 14));
				}
				num9 += 40;
			}
			if (nb_subfr == 4)
			{
				for (int i = 72; i >= 8; i--)
				{
					int num13 = (int)(Inlines.MatrixGet<short>(array6, 0, i - 8, 65) + Inlines.MatrixGet<short>(array6, 1, i - 8, 65));
					num13 = Inlines.silk_SMLAWB(num13, num13, Inlines.silk_LSHIFT(-i, 4));
					array6[i - 8] = (short)num13;
				}
			}
			else
			{
				for (int i = 72; i >= 8; i--)
				{
					int num13 = Inlines.silk_LSHIFT((int)array6[i - 8], 1);
					num13 = Inlines.silk_SMLAWB(num13, num13, Inlines.silk_LSHIFT(-i, 4));
					array6[i - 8] = (short)num13;
				}
			}
			int num14 = Inlines.silk_ADD_LSHIFT32(4, complexity, 1);
			Sort.silk_insertion_sort_decreasing_int16(array6, array2, 65, num14);
			int num15 = (int)array6[0];
			if (num15 < 3277)
			{
				Arrays.MemSetInt(pitch_out, 0, nb_subfr);
				LTPCorr_Q15.Val = 0;
				lagIndex.Val = 0;
				contourIndex.Val = 0;
				return 1;
			}
			int num16 = Inlines.silk_SMULWB(search_thres1_Q16, num15);
			for (int i = 0; i < num14; i++)
			{
				if ((int)array6[i] <= num16)
				{
					num14 = i;
					break;
				}
				array2[i] = Inlines.silk_LSHIFT(array2[i] + 8, 1);
			}
			short[] array10 = new short[134];
			for (int i = 13; i < 147; i++)
			{
				array10[i - 13] = 0;
			}
			for (int i = 0; i < num14; i++)
			{
				array10[array2[i] - 13] = 1;
			}
			for (int i = 146; i >= 16; i--)
			{
				short[] array11 = array10;
				int num17 = i - 13;
				array11[num17] += array10[i - 1 - 13] + array10[i - 2 - 13];
			}
			num14 = 0;
			for (int i = 16; i < 144; i++)
			{
				if (array10[i + 1 - 13] > 0)
				{
					array2[num14] = i;
					num14++;
				}
			}
			for (int i = 146; i >= 16; i--)
			{
				short[] array12 = array10;
				int num18 = i - 13;
				array12[num18] += array10[i - 1 - 13] + array10[i - 2 - 13] + array10[i - 3 - 13];
			}
			int num19 = 0;
			for (int i = 16; i < 147; i++)
			{
				if (array10[i - 13] > 0)
				{
					array10[num19] = (short)(i - 2);
					num19++;
				}
			}
			SumSqrShift.silk_sum_sqr_shift(out num7, out num8, array4, num3);
			if (num8 > 0)
			{
				num8 = Inlines.silk_RSHIFT(num8, 1);
				for (int i = 0; i < num3; i++)
				{
					array4[i] = Inlines.silk_RSHIFT16(array4[i], num8);
				}
			}
			Arrays.MemSetShort(array6, 0, nb_subfr * 132);
			array8 = array4;
			num9 = 160;
			for (int j = 0; j < nb_subfr; j++)
			{
				int num20 = Inlines.silk_ADD32(Inlines.silk_inner_prod(array8, num9, array8, num9, 40), 1);
				for (int l = 0; l < num19; l++)
				{
					int k = (int)array10[l];
					short[] array9 = array8;
					int num10 = num9 - k;
					int num11 = Inlines.silk_inner_prod(array8, num9, array9, num10, 40);
					if (num11 > 0)
					{
						int b = Inlines.silk_inner_prod_self(array9, num10, 40);
						Inlines.MatrixSet<short>(array6, j, k - 14, 132, (short)Inlines.silk_DIV32_varQ(num11, Inlines.silk_ADD32(num20, b), 14));
					}
					else
					{
						Inlines.MatrixSet<short>(array6, j, k - 14, 132, 0);
					}
				}
				num9 += 40;
			}
			int num21 = int.MinValue;
			int num22 = int.MinValue;
			int num23 = 0;
			int num24 = -1;
			int num25;
			if (prevLag > 0)
			{
				if (Fs_kHz == 12)
				{
					prevLag = Inlines.silk_DIV32_16(Inlines.silk_LSHIFT(prevLag, 1), 3);
				}
				else if (Fs_kHz == 16)
				{
					prevLag = Inlines.silk_RSHIFT(prevLag, 1);
				}
				num25 = Inlines.silk_lin2log(prevLag);
			}
			else
			{
				num25 = 0;
			}
			sbyte[][] array13;
			int num26;
			if (nb_subfr == 4)
			{
				array13 = Tables.silk_CB_lags_stage2;
				if (Fs_kHz == 8 && complexity > 0)
				{
					num26 = 11;
				}
				else
				{
					num26 = 3;
				}
			}
			else
			{
				array13 = Tables.silk_CB_lags_stage2_10_ms;
				num26 = 3;
			}
			for (int j = 0; j < num14; j++)
			{
				int k = array2[j];
				for (int l = 0; l < num26; l++)
				{
					array3[l] = 0;
					for (int i = 0; i < nb_subfr; i++)
					{
						int num27 = k + (int)array13[i][l];
						array3[l] += (int)Inlines.MatrixGet<short>(array6, i, num27 - 14, 132);
					}
				}
				int num28 = int.MinValue;
				int num29 = 0;
				for (int i = 0; i < num26; i++)
				{
					if (array3[i] > num28)
					{
						num28 = array3[i];
						num29 = i;
					}
				}
				int num30 = Inlines.silk_lin2log(k);
				int num31 = num28 - Inlines.silk_RSHIFT(Inlines.silk_SMULBB(nb_subfr * 1638, num30), 7);
				if (prevLag > 0)
				{
					int num32 = num30 - num25;
					num32 = Inlines.silk_RSHIFT(Inlines.silk_SMULBB(num32, num32), 7);
					int num33 = Inlines.silk_RSHIFT(Inlines.silk_SMULBB(nb_subfr * 1638, LTPCorr_Q15.Val), 15);
					num33 = Inlines.silk_DIV32(Inlines.silk_MUL(num33, num32), num32 + 64);
					num31 -= num33;
				}
				if (num31 > num22 && num28 > Inlines.silk_SMULBB(nb_subfr, search_thres2_Q13) && Tables.silk_CB_lags_stage2[0][num29] <= 16)
				{
					num22 = num31;
					num21 = num28;
					num24 = k;
					num23 = num29;
				}
			}
			if (num24 == -1)
			{
				Arrays.MemSetInt(pitch_out, 0, nb_subfr);
				LTPCorr_Q15.Val = 0;
				lagIndex.Val = 0;
				contourIndex.Val = 0;
				return 1;
			}
			LTPCorr_Q15.Val = Inlines.silk_LSHIFT(Inlines.silk_DIV32_16(num21, nb_subfr), 2);
			if (Fs_kHz > 8)
			{
				SumSqrShift.silk_sum_sqr_shift(out num7, out num8, frame, num);
				short[] array15;
				if (num8 > 0)
				{
					short[] array14 = new short[num];
					num8 = Inlines.silk_RSHIFT(num8, 1);
					for (int i = 0; i < num; i++)
					{
						array14[i] = Inlines.silk_RSHIFT16(frame[i], num8);
					}
					array15 = array14;
				}
				else
				{
					array15 = frame;
				}
				int num34 = num23;
				if (Fs_kHz == 12)
				{
					num24 = Inlines.silk_RSHIFT(Inlines.silk_SMULBB(num24, 3), 1);
				}
				else if (Fs_kHz == 16)
				{
					num24 = Inlines.silk_LSHIFT(num24, 1);
				}
				else
				{
					num24 = Inlines.silk_SMULBB(num24, 3);
				}
				num24 = Inlines.silk_LIMIT_int(num24, num5, num6);
				int num35 = Inlines.silk_max_int(num24 - 2, num5);
				int num36 = Inlines.silk_min_int(num24 + 2, num6);
				int num37 = num24;
				num23 = 0;
				num21 = int.MinValue;
				for (int j = 0; j < nb_subfr; j++)
				{
					pitch_out[j] = num24 + (int)(2 * Tables.silk_CB_lags_stage2[j][num34]);
				}
				if (nb_subfr == 4)
				{
					num26 = (int)Tables.silk_nb_cbk_searchs_stage3[complexity];
					array13 = Tables.silk_CB_lags_stage3;
				}
				else
				{
					num26 = 12;
					array13 = Tables.silk_CB_lags_stage3_10_ms;
				}
				PitchAnalysisCore.silk_pe_stage3_vals[] array16 = new PitchAnalysisCore.silk_pe_stage3_vals[nb_subfr * num26];
				PitchAnalysisCore.silk_pe_stage3_vals[] array17 = new PitchAnalysisCore.silk_pe_stage3_vals[nb_subfr * num26];
				for (int m = 0; m < nb_subfr * num26; m++)
				{
					array16[m] = new PitchAnalysisCore.silk_pe_stage3_vals();
					array17[m] = new PitchAnalysisCore.silk_pe_stage3_vals();
				}
				PitchAnalysisCore.silk_P_Ana_calc_corr_st3(array17, array15, num35, num4, nb_subfr, complexity);
				PitchAnalysisCore.silk_P_Ana_calc_energy_st3(array16, array15, num35, num4, nb_subfr, complexity);
				int num38 = 0;
				int a = Inlines.silk_DIV32_16(1638, num24);
				array8 = array15;
				num9 = 20 * Fs_kHz;
				int num20 = Inlines.silk_ADD32(Inlines.silk_inner_prod_self(array8, num9, nb_subfr * num4), 1);
				for (int k = num35; k <= num36; k++)
				{
					for (int l = 0; l < num26; l++)
					{
						int num11 = 0;
						num7 = num20;
						for (int j = 0; j < nb_subfr; j++)
						{
							num11 = Inlines.silk_ADD32(num11, Inlines.MatrixGet<PitchAnalysisCore.silk_pe_stage3_vals>(array17, j, l, num26).Values[num38]);
							num7 = Inlines.silk_ADD32(num7, Inlines.MatrixGet<PitchAnalysisCore.silk_pe_stage3_vals>(array16, j, l, num26).Values[num38]);
						}
						int num28;
						if (num11 > 0)
						{
							num28 = Inlines.silk_DIV32_varQ(num11, num7, 14);
							int b2 = 32767 - Inlines.silk_MUL(a, l);
							num28 = Inlines.silk_SMULWB(num28, b2);
						}
						else
						{
							num28 = 0;
						}
						if (num28 > num21 && k + (int)Tables.silk_CB_lags_stage3[0][l] <= num6)
						{
							num21 = num28;
							num37 = k;
							num23 = l;
						}
					}
					num38++;
				}
				for (int j = 0; j < nb_subfr; j++)
				{
					pitch_out[j] = num37 + (int)array13[j][num23];
					pitch_out[j] = Inlines.silk_LIMIT(pitch_out[j], num5, 18 * Fs_kHz);
				}
				lagIndex.Val = (short)(num37 - num5);
				contourIndex.Val = (sbyte)num23;
			}
			else
			{
				for (int j = 0; j < nb_subfr; j++)
				{
					pitch_out[j] = num24 + (int)array13[j][num23];
					pitch_out[j] = Inlines.silk_LIMIT(pitch_out[j], 16, 144);
				}
				lagIndex.Val = (short)(num24 - 16);
				contourIndex.Val = (sbyte)num23;
			}
			return 0;
		}

		// Token: 0x060000FA RID: 250 RVA: 0x0000E798 File Offset: 0x0000C998
		private static void silk_P_Ana_calc_corr_st3(PitchAnalysisCore.silk_pe_stage3_vals[] cross_corr_st3, short[] frame, int start_lag, int sf_length, int nb_subfr, int complexity)
		{
			sbyte[][] array;
			sbyte[][] array2;
			int num;
			if (nb_subfr == 4)
			{
				array = Tables.silk_Lag_range_stage3[complexity];
				array2 = Tables.silk_CB_lags_stage3;
				num = (int)Tables.silk_nb_cbk_searchs_stage3[complexity];
			}
			else
			{
				array = Tables.silk_Lag_range_stage3_10_ms;
				array2 = Tables.silk_CB_lags_stage3_10_ms;
				num = 12;
			}
			int[] array3 = new int[22];
			int[] array4 = new int[22];
			int num2 = Inlines.silk_LSHIFT(sf_length, 2);
			for (int i = 0; i < nb_subfr; i++)
			{
				int num3 = 0;
				int num4 = (int)array[i][0];
				int num5 = (int)array[i][1];
				CeltPitchXCorr.pitch_xcorr(frame, num2, frame, num2 - start_lag - num5, array4, sf_length, num5 - num4 + 1);
				for (int j = num4; j <= num5; j++)
				{
					array3[num3] = array4[num5 - j];
					num3++;
				}
				int num6 = (int)array[i][0];
				for (int k = 0; k < num; k++)
				{
					int num7 = (int)array2[i][k] - num6;
					for (int j = 0; j < 5; j++)
					{
						Inlines.MatrixGet<PitchAnalysisCore.silk_pe_stage3_vals>(cross_corr_st3, i, k, num).Values[j] = array3[num7 + j];
					}
				}
				num2 += sf_length;
			}
		}

		// Token: 0x060000FB RID: 251 RVA: 0x0000E89C File Offset: 0x0000CA9C
		private static void silk_P_Ana_calc_energy_st3(PitchAnalysisCore.silk_pe_stage3_vals[] energies_st3, short[] frame, int start_lag, int sf_length, int nb_subfr, int complexity)
		{
			sbyte[][] array;
			sbyte[][] array2;
			int num;
			if (nb_subfr == 4)
			{
				array = Tables.silk_Lag_range_stage3[complexity];
				array2 = Tables.silk_CB_lags_stage3;
				num = (int)Tables.silk_nb_cbk_searchs_stage3[complexity];
			}
			else
			{
				array = Tables.silk_Lag_range_stage3_10_ms;
				array2 = Tables.silk_CB_lags_stage3_10_ms;
				num = 12;
			}
			int[] array3 = new int[22];
			int num2 = Inlines.silk_LSHIFT(sf_length, 2);
			for (int i = 0; i < nb_subfr; i++)
			{
				int num3 = 0;
				int num4 = num2 - (start_lag + (int)array[i][0]);
				int num5 = Inlines.silk_inner_prod_self(frame, num4, sf_length);
				array3[num3] = num5;
				num3++;
				int num6 = (int)(array[i][1] - array[i][0] + 1);
				for (int j = 1; j < num6; j++)
				{
					num5 -= Inlines.silk_SMULBB((int)frame[num4 + sf_length - j], (int)frame[num4 + sf_length - j]);
					num5 = Inlines.silk_ADD_SAT32(num5, Inlines.silk_SMULBB((int)frame[num4 - j], (int)frame[num4 - j]));
					array3[num3] = num5;
					num3++;
				}
				int num7 = (int)array[i][0];
				for (int j = 0; j < num; j++)
				{
					int num8 = (int)array2[i][j] - num7;
					for (int k = 0; k < 5; k++)
					{
						Inlines.MatrixGet<PitchAnalysisCore.silk_pe_stage3_vals>(energies_st3, i, j, num).Values[k] = array3[num8 + k];
					}
				}
				num2 += sf_length;
			}
		}

		// Token: 0x04000043 RID: 67
		private const int SCRATCH_SIZE = 22;

		// Token: 0x04000044 RID: 68
		private const int SF_LENGTH_4KHZ = 20;

		// Token: 0x04000045 RID: 69
		private const int SF_LENGTH_8KHZ = 40;

		// Token: 0x04000046 RID: 70
		private const int MIN_LAG_4KHZ = 8;

		// Token: 0x04000047 RID: 71
		private const int MIN_LAG_8KHZ = 16;

		// Token: 0x04000048 RID: 72
		private const int MAX_LAG_4KHZ = 72;

		// Token: 0x04000049 RID: 73
		private const int MAX_LAG_8KHZ = 143;

		// Token: 0x0400004A RID: 74
		private const int CSTRIDE_4KHZ = 65;

		// Token: 0x0400004B RID: 75
		private const int CSTRIDE_8KHZ = 132;

		// Token: 0x0400004C RID: 76
		private const int D_COMP_MIN = 13;

		// Token: 0x0400004D RID: 77
		private const int D_COMP_MAX = 147;

		// Token: 0x0400004E RID: 78
		private const int D_COMP_STRIDE = 134;

		// Token: 0x0200009C RID: 156
		private class silk_pe_stage3_vals
		{
			// Token: 0x040005D9 RID: 1497
			internal readonly int[] Values = new int[5];
		}
	}
}
