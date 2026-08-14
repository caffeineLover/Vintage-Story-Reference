using System;
using Concentus.Celt;
using Concentus.Common;
using Concentus.Common.CPlusPlus;
using Concentus.Silk.Structs;

namespace Concentus.Silk
{
	// Token: 0x02000021 RID: 33
	internal static class Filters
	{
		// Token: 0x060000C3 RID: 195 RVA: 0x00009720 File Offset: 0x00007920
		internal static void silk_warped_LPC_analysis_filter(int[] state, int[] res_Q2, short[] coef_Q13, int coef_Q13_ptr, short[] input, int input_ptr, short lambda_Q16, int length, int order)
		{
			for (int i = 0; i < length; i++)
			{
				int num = Inlines.silk_SMLAWB(state[0], state[1], (int)lambda_Q16);
				state[0] = Inlines.silk_LSHIFT((int)input[input_ptr + i], 14);
				int num2 = Inlines.silk_SMLAWB(state[1], state[2] - num, (int)lambda_Q16);
				state[1] = num;
				int num3 = Inlines.silk_RSHIFT(order, 1);
				num3 = Inlines.silk_SMLAWB(num3, num, (int)coef_Q13[coef_Q13_ptr]);
				for (int j = 2; j < order; j += 2)
				{
					num = Inlines.silk_SMLAWB(state[j], state[j + 1] - num2, (int)lambda_Q16);
					state[j] = num2;
					num3 = Inlines.silk_SMLAWB(num3, num2, (int)coef_Q13[coef_Q13_ptr + j - 1]);
					num2 = Inlines.silk_SMLAWB(state[j + 1], state[j + 2] - num, (int)lambda_Q16);
					state[j + 1] = num;
					num3 = Inlines.silk_SMLAWB(num3, num, (int)coef_Q13[coef_Q13_ptr + j]);
				}
				state[order] = num2;
				num3 = Inlines.silk_SMLAWB(num3, num2, (int)coef_Q13[coef_Q13_ptr + order - 1]);
				res_Q2[i] = Inlines.silk_LSHIFT((int)input[input_ptr + i], 2) - Inlines.silk_RSHIFT_ROUND(num3, 9);
			}
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00009818 File Offset: 0x00007A18
		internal unsafe static void silk_prefilter(SilkChannelEncoder psEnc, SilkEncoderControl psEncCtrl, int[] xw_Q3, short[] x, int x_ptr)
		{
			SilkPrefilterState sPrefilt = psEnc.sPrefilt;
			Span<short> span = new Span<short>(stackalloc byte[(UIntPtr)4], 2);
			int num = x_ptr;
			int num2 = 0;
			int lag = sPrefilt.lagPrev;
			int[] array = new int[psEnc.subfr_length];
			int[] array2 = new int[psEnc.subfr_length];
			for (int i = 0; i < psEnc.nb_subfr; i++)
			{
				if (psEnc.indices.signalType == 2)
				{
					lag = psEncCtrl.pitchL[i];
				}
				int num3 = Inlines.silk_SMULWB(psEncCtrl.HarmShapeGain_Q14[i], 16384 - psEncCtrl.HarmBoost_Q14[i]);
				int num4 = Inlines.silk_RSHIFT(num3, 2);
				num4 |= Inlines.silk_LSHIFT(Inlines.silk_RSHIFT(num3, 1), 16);
				int tilt_Q = psEncCtrl.Tilt_Q14[i];
				int lf_shp_Q = psEncCtrl.LF_shp_Q14[i];
				int coef_Q13_ptr = i * 16;
				Filters.silk_warped_LPC_analysis_filter(sPrefilt.sAR_shp, array2, psEncCtrl.AR1_Q13, coef_Q13_ptr, x, num, (short)psEnc.warping_Q16, psEnc.subfr_length, psEnc.shapingLPCOrder);
				*span[0] = (short)Inlines.silk_RSHIFT_ROUND(psEncCtrl.GainsPre_Q14[i], 4);
				int num5 = Inlines.silk_SMLABB(3355443, psEncCtrl.HarmBoost_Q14[i], num3);
				num5 = Inlines.silk_SMLABB(num5, psEncCtrl.coding_quality_Q14, 410);
				num5 = Inlines.silk_SMULWB(num5, -psEncCtrl.GainsPre_Q14[i]);
				num5 = Inlines.silk_RSHIFT_ROUND(num5, 14);
				*span[1] = (short)Inlines.silk_SAT16(num5);
				array[0] = Inlines.silk_MLA(Inlines.silk_MUL(array2[0], (int)(*span[0])), sPrefilt.sHarmHP_Q2, (int)(*span[1]));
				for (int j = 1; j < psEnc.subfr_length; j++)
				{
					array[j] = Inlines.silk_MLA(Inlines.silk_MUL(array2[j], (int)(*span[0])), array2[j - 1], (int)(*span[1]));
				}
				sPrefilt.sHarmHP_Q2 = array2[psEnc.subfr_length - 1];
				Filters.silk_prefilt(sPrefilt, array, xw_Q3, num2, num4, tilt_Q, lf_shp_Q, lag, psEnc.subfr_length);
				num += psEnc.subfr_length;
				num2 += psEnc.subfr_length;
			}
			sPrefilt.lagPrev = psEncCtrl.pitchL[psEnc.nb_subfr - 1];
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00009A38 File Offset: 0x00007C38
		private static void silk_prefilt(SilkPrefilterState P, int[] st_res_Q12, int[] xw_Q3, int xw_Q3_ptr, int HarmShapeFIRPacked_Q12, int Tilt_Q14, int LF_shp_Q14, int lag, int length)
		{
			short[] sLTP_shp = P.sLTP_shp;
			int num = P.sLTP_shp_buf_idx;
			int num2 = P.sLF_AR_shp_Q12;
			int num3 = P.sLF_MA_shp_Q12;
			for (int i = 0; i < length; i++)
			{
				int num5;
				if (lag > 0)
				{
					int num4 = lag + num;
					num5 = Inlines.silk_SMULBB((int)sLTP_shp[num4 - 1 - 1 & 511], HarmShapeFIRPacked_Q12);
					num5 = Inlines.silk_SMLABT(num5, (int)sLTP_shp[num4 - 1 & 511], HarmShapeFIRPacked_Q12);
					num5 = Inlines.silk_SMLABB(num5, (int)sLTP_shp[num4 - 1 + 1 & 511], HarmShapeFIRPacked_Q12);
				}
				else
				{
					num5 = 0;
				}
				int a = Inlines.silk_SMULWB(num2, Tilt_Q14);
				int a2 = Inlines.silk_SMLAWB(Inlines.silk_SMULWT(num2, LF_shp_Q14), num3, LF_shp_Q14);
				num2 = Inlines.silk_SUB32(st_res_Q12[i], Inlines.silk_LSHIFT(a, 2));
				num3 = Inlines.silk_SUB32(num2, Inlines.silk_LSHIFT(a2, 2));
				num = (num - 1 & 511);
				sLTP_shp[num] = (short)Inlines.silk_SAT16(Inlines.silk_RSHIFT_ROUND(num3, 12));
				xw_Q3[xw_Q3_ptr + i] = Inlines.silk_RSHIFT_ROUND(Inlines.silk_SUB32(num3, num5), 9);
			}
			P.sLF_AR_shp_Q12 = num2;
			P.sLF_MA_shp_Q12 = num3;
			P.sLTP_shp_buf_idx = num;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00009B54 File Offset: 0x00007D54
		internal unsafe static void silk_biquad_alt(ReadOnlySpan<short> input, int input_ptr, int[] B_Q28, int[] A_Q28, int[] S, Span<short> output, int output_ptr, int len, int stride)
		{
			int b = -A_Q28[0] & 16383;
			int c = Inlines.silk_RSHIFT(-A_Q28[0], 14);
			int b2 = -A_Q28[1] & 16383;
			int c2 = Inlines.silk_RSHIFT(-A_Q28[1], 14);
			for (int i = 0; i < len; i++)
			{
				int c3 = (int)(*input[input_ptr + i * stride]);
				int num = Inlines.silk_LSHIFT(Inlines.silk_SMLAWB(S[0], B_Q28[0], c3), 2);
				S[0] = S[1] + Inlines.silk_RSHIFT_ROUND(Inlines.silk_SMULWB(num, b), 14);
				S[0] = Inlines.silk_SMLAWB(S[0], num, c);
				S[0] = Inlines.silk_SMLAWB(S[0], B_Q28[1], c3);
				S[1] = Inlines.silk_RSHIFT_ROUND(Inlines.silk_SMULWB(num, b2), 14);
				S[1] = Inlines.silk_SMLAWB(S[1], num, c2);
				S[1] = Inlines.silk_SMLAWB(S[1], B_Q28[2], c3);
				*output[output_ptr + i * stride] = (short)Inlines.silk_SAT16(Inlines.silk_RSHIFT(num + 16384 - 1, 14));
			}
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00009C60 File Offset: 0x00007E60
		internal unsafe static void silk_biquad_alt(ReadOnlySpan<short> input, int input_ptr, int[] B_Q28, int[] A_Q28, Span<int> S, int S_ptr, Span<short> output, int output_ptr, int len, int stride)
		{
			int b = -A_Q28[0] & 16383;
			int c = Inlines.silk_RSHIFT(-A_Q28[0], 14);
			int b2 = -A_Q28[1] & 16383;
			int c2 = Inlines.silk_RSHIFT(-A_Q28[1], 14);
			for (int i = 0; i < len; i++)
			{
				int num = S_ptr + 1;
				int c3 = (int)(*input[input_ptr + i * stride]);
				int num2 = Inlines.silk_LSHIFT(Inlines.silk_SMLAWB(*S[S_ptr], B_Q28[0], c3), 2);
				*S[S_ptr] = *S[num] + Inlines.silk_RSHIFT_ROUND(Inlines.silk_SMULWB(num2, b), 14);
				*S[S_ptr] = Inlines.silk_SMLAWB(*S[S_ptr], num2, c);
				*S[S_ptr] = Inlines.silk_SMLAWB(*S[S_ptr], B_Q28[1], c3);
				*S[num] = Inlines.silk_RSHIFT_ROUND(Inlines.silk_SMULWB(num2, b2), 14);
				*S[num] = Inlines.silk_SMLAWB(*S[num], num2, c2);
				*S[num] = Inlines.silk_SMLAWB(*S[num], B_Q28[2], c3);
				*output[output_ptr + i * stride] = (short)Inlines.silk_SAT16(Inlines.silk_RSHIFT(num2 + 16384 - 1, 14));
			}
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00009DB8 File Offset: 0x00007FB8
		internal unsafe static void silk_ana_filt_bank_1(Span<short> input, int input_ptr, int[] S, short[] outL, Span<short> outH, int outH_ptr, int N)
		{
			int num = Inlines.silk_RSHIFT(N, 1);
			for (int i = 0; i < num; i++)
			{
				int a = Inlines.silk_LSHIFT((int)(*input[input_ptr + 2 * i]), 10);
				int num2 = Inlines.silk_SUB32(a, S[0]);
				int b = Inlines.silk_SMLAWB(num2, num2, (int)Filters.A_fb1_21);
				int b2 = Inlines.silk_ADD32(S[0], b);
				S[0] = Inlines.silk_ADD32(a, b);
				a = Inlines.silk_LSHIFT((int)(*input[input_ptr + 2 * i + 1]), 10);
				b = Inlines.silk_SMULWB(Inlines.silk_SUB32(a, S[1]), (int)Filters.A_fb1_20);
				int a2 = Inlines.silk_ADD32(S[1], b);
				S[1] = Inlines.silk_ADD32(a, b);
				outL[i] = (short)Inlines.silk_SAT16(Inlines.silk_RSHIFT_ROUND(Inlines.silk_ADD32(a2, b2), 11));
				*outH[outH_ptr + i] = (short)Inlines.silk_SAT16(Inlines.silk_RSHIFT_ROUND(Inlines.silk_SUB32(a2, b2), 11));
			}
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00009E9C File Offset: 0x0000809C
		internal static void silk_bwexpander_32(int[] ar, int d, int chirp_Q16)
		{
			int b = chirp_Q16 - 65536;
			for (int i = 0; i < d - 1; i++)
			{
				ar[i] = Inlines.silk_SMULWW(chirp_Q16, ar[i]);
				chirp_Q16 += Inlines.silk_RSHIFT_ROUND(Inlines.silk_MUL(chirp_Q16, b), 16);
			}
			ar[d - 1] = Inlines.silk_SMULWW(chirp_Q16, ar[d - 1]);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00009EF0 File Offset: 0x000080F0
		internal static void silk_LP_interpolate_filter_taps(int[] B_Q28, int[] A_Q28, int ind, int fac_Q16)
		{
			if (ind >= 4)
			{
				Arrays.MemCopy(Tables.silk_Transition_LP_B_Q28[4], 0, B_Q28, 0, 3);
				Arrays.MemCopy(Tables.silk_Transition_LP_A_Q28[4], 0, A_Q28, 0, 2);
				return;
			}
			if (fac_Q16 <= 0)
			{
				Arrays.MemCopy(Tables.silk_Transition_LP_B_Q28[ind], 0, B_Q28, 0, 3);
				Arrays.MemCopy(Tables.silk_Transition_LP_A_Q28[ind], 0, A_Q28, 0, 2);
				return;
			}
			if (fac_Q16 < 32768)
			{
				for (int i = 0; i < 3; i++)
				{
					B_Q28[i] = Inlines.silk_SMLAWB(Tables.silk_Transition_LP_B_Q28[ind][i], Tables.silk_Transition_LP_B_Q28[ind + 1][i] - Tables.silk_Transition_LP_B_Q28[ind][i], fac_Q16);
				}
				for (int j = 0; j < 2; j++)
				{
					A_Q28[j] = Inlines.silk_SMLAWB(Tables.silk_Transition_LP_A_Q28[ind][j], Tables.silk_Transition_LP_A_Q28[ind + 1][j] - Tables.silk_Transition_LP_A_Q28[ind][j], fac_Q16);
				}
				return;
			}
			for (int i = 0; i < 3; i++)
			{
				B_Q28[i] = Inlines.silk_SMLAWB(Tables.silk_Transition_LP_B_Q28[ind + 1][i], Tables.silk_Transition_LP_B_Q28[ind + 1][i] - Tables.silk_Transition_LP_B_Q28[ind][i], fac_Q16 - 65536);
			}
			for (int j = 0; j < 2; j++)
			{
				A_Q28[j] = Inlines.silk_SMLAWB(Tables.silk_Transition_LP_A_Q28[ind + 1][j], Tables.silk_Transition_LP_A_Q28[ind + 1][j] - Tables.silk_Transition_LP_A_Q28[ind][j], fac_Q16 - 65536);
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x0000A034 File Offset: 0x00008234
		internal unsafe static void silk_LPC_analysis_filter(Span<short> output, int output_ptr, Span<short> input, int input_ptr, Span<short> B, int B_ptr, int len, int d)
		{
			Span<short> mem = new Span<short>(stackalloc byte[(UIntPtr)32], 16);
			Span<short> num = new Span<short>(stackalloc byte[(UIntPtr)32], 16);
			for (int i = 0; i < d; i++)
			{
				*num[i] = 0 - *B[B_ptr + i];
			}
			for (int i = 0; i < d; i++)
			{
				*mem[i] = *input[input_ptr + d - i - 1];
			}
			Kernels.celt_fir(input.Slice(input_ptr + d), num, output.Slice(output_ptr + d), len - d, d, mem);
			for (int i = output_ptr; i < output_ptr + d; i++)
			{
				*output[i] = 0;
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x0000A0E4 File Offset: 0x000082E4
		internal static int LPC_inverse_pred_gain_QA(int[][] A_QA, int order)
		{
			int[] array = A_QA[order & 1];
			int a = 1073741824;
			int num;
			int num2;
			for (int i = order - 1; i > 0; i--)
			{
				if (array[i] > Filters.A_LIMIT || array[i] < -Filters.A_LIMIT)
				{
					return 0;
				}
				num = 0 - Inlines.silk_LSHIFT(array[i], 7);
				num2 = 1073741824 - Inlines.silk_SMMUL(num, num);
				int num3 = 32 - Inlines.silk_CLZ32(Inlines.silk_abs(num2));
				int b = Inlines.silk_INVERSE32_varQ(num2, num3 + 30);
				a = Inlines.silk_LSHIFT(Inlines.silk_SMMUL(a, num2), 2);
				int[] array2 = array;
				array = A_QA[i & 1];
				for (int j = 0; j < i; j++)
				{
					int a2 = array2[j] - Inlines.MUL32_FRAC_Q(array2[i - j - 1], num, 31);
					array[j] = Inlines.MUL32_FRAC_Q(a2, b, num3);
				}
			}
			if (array[0] > Filters.A_LIMIT || array[0] < -Filters.A_LIMIT)
			{
				return 0;
			}
			num = 0 - Inlines.silk_LSHIFT(array[0], 7);
			num2 = 1073741824 - Inlines.silk_SMMUL(num, num);
			return Inlines.silk_LSHIFT(Inlines.silk_SMMUL(a, num2), 2);
		}

		// Token: 0x060000CD RID: 205 RVA: 0x0000A1FC File Offset: 0x000083FC
		internal static int silk_LPC_inverse_pred_gain(short[] A_Q12, int order)
		{
			int[][] array = new int[][]
			{
				new int[order],
				new int[order]
			};
			int num = 0;
			int[] array2 = array[order & 1];
			for (int i = 0; i < order; i++)
			{
				num += (int)A_Q12[i];
				array2[i] = Inlines.silk_LSHIFT32((int)A_Q12[i], 12);
			}
			if (num >= 4096)
			{
				return 0;
			}
			return Filters.LPC_inverse_pred_gain_QA(array, order);
		}

		// Token: 0x04000031 RID: 49
		private static readonly short A_fb1_20 = 10788;

		// Token: 0x04000032 RID: 50
		private static readonly short A_fb1_21 = -24290;

		// Token: 0x04000033 RID: 51
		private const int QA = 24;

		// Token: 0x04000034 RID: 52
		private static readonly int A_LIMIT = 16773022;
	}
}
