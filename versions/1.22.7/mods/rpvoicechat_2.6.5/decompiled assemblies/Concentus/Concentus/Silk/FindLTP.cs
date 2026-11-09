using System;
using Concentus.Common;
using Concentus.Common.CPlusPlus;

namespace Concentus.Silk
{
	// Token: 0x02000023 RID: 35
	internal static class FindLTP
	{
		// Token: 0x060000D0 RID: 208 RVA: 0x0000A4E4 File Offset: 0x000086E4
		internal static void silk_find_LTP(short[] b_Q14, int[] WLTP, BoxedValueInt LTPredCodGain_Q7, short[] r_lpc, int[] lag, int[] Wght_Q15, int subfr_length, int nb_subfr, int mem_offset, int[] corr_rshifts)
		{
			int[] array = new int[5];
			int[] array2 = new int[5];
			int[] array3 = new int[4];
			int[] array4 = new int[4];
			int[] array5 = new int[4];
			int[] array6 = new int[5];
			int[] array7 = new int[4];
			int num = 0;
			int num2 = 0;
			int num3 = mem_offset;
			int num7;
			int num9;
			for (int i = 0; i < nb_subfr; i++)
			{
				int x_ptr = num3 - (lag[i] + 2);
				int num4;
				SumSqrShift.silk_sum_sqr_shift(out array7[i], out num4, r_lpc, num3, subfr_length);
				int num5 = Inlines.silk_CLZ32(array7[i]);
				if (num5 < 2)
				{
					array7[i] = Inlines.silk_RSHIFT_ROUND(array7[i], 2 - num5);
					num4 += 2 - num5;
				}
				corr_rshifts[i] = num4;
				BoxedValueInt boxedValueInt = new BoxedValueInt(corr_rshifts[i]);
				CorrelateMatrix.silk_corrMatrix(r_lpc, x_ptr, subfr_length, 5, 2, WLTP, num2, boxedValueInt);
				corr_rshifts[i] = boxedValueInt.Val;
				CorrelateMatrix.silk_corrVector(r_lpc, x_ptr, r_lpc, num3, subfr_length, 5, array6, corr_rshifts[i]);
				if (corr_rshifts[i] > num4)
				{
					array7[i] = Inlines.silk_RSHIFT(array7[i], corr_rshifts[i] - num4);
				}
				int num6 = 1;
				num6 = Inlines.silk_SMLAWB(num6, array7[i], 1092);
				num6 = Inlines.silk_SMLAWB(num6, Inlines.MatrixGet<int>(WLTP, num2, 0, 0, 5), 1092);
				num6 = Inlines.silk_SMLAWB(num6, Inlines.MatrixGet<int>(WLTP, num2, 4, 4, 5), 1092);
				RegularizeCorrelations.silk_regularize_correlations(WLTP, num2, array7, i, num6, 5);
				LinearAlgebra.silk_solve_LDL(WLTP, num2, 5, array6, array);
				FindLTP.silk_fit_LTP(array, b_Q14, num);
				array4[i] = ResidualEnergy.silk_residual_energy16_covar(b_Q14, num, WLTP, num2, array6, array7[i], 5, 14);
				num7 = Inlines.silk_min_int(corr_rshifts[i], 2);
				int num8 = Inlines.silk_LSHIFT_SAT32(Inlines.silk_SMULWB(array4[i], Wght_Q15[i]), 1 + num7) + Inlines.silk_RSHIFT(Inlines.silk_SMULWB(subfr_length, 655), corr_rshifts[i] - num7);
				num8 = Inlines.silk_max(num8, 1);
				num9 = Inlines.silk_DIV32(Inlines.silk_LSHIFT(Wght_Q15[i], 16), num8);
				num9 = Inlines.silk_RSHIFT(num9, 31 + corr_rshifts[i] - num7 - 26);
				int num10 = 0;
				for (int j = num2; j < num2 + 25; j++)
				{
					num10 = Inlines.silk_max(WLTP[j], num10);
				}
				int num11 = Inlines.silk_CLZ32(num10) - 1 - 3;
				if (8 + num11 < 31)
				{
					num9 = Inlines.silk_min_32(num9, Inlines.silk_LSHIFT(1, 8 + num11));
				}
				Inlines.silk_scale_vector32_Q26_lshift_18(WLTP, num2, num9, 25);
				array5[i] = Inlines.MatrixGet<int>(WLTP, num2, 2, 2, 5);
				num3 += subfr_length;
				num += 5;
				num2 += 25;
			}
			int num12 = 0;
			for (int i = 0; i < nb_subfr; i++)
			{
				num12 = Inlines.silk_max_int(corr_rshifts[i], num12);
			}
			if (LTPredCodGain_Q7 != null)
			{
				int num13 = 0;
				int num14 = 0;
				for (int i = 0; i < nb_subfr; i++)
				{
					num14 = Inlines.silk_ADD32(num14, Inlines.silk_RSHIFT(Inlines.silk_ADD32(Inlines.silk_SMULWB(array7[i], Wght_Q15[i]), 1), 1 + (num12 - corr_rshifts[i])));
					num13 = Inlines.silk_ADD32(num13, Inlines.silk_RSHIFT(Inlines.silk_ADD32(Inlines.silk_SMULWB(array4[i], Wght_Q15[i]), 1), 1 + (num12 - corr_rshifts[i])));
				}
				num13 = Inlines.silk_max(num13, 1);
				int inLin = Inlines.silk_DIV32_varQ(num14, num13, 16);
				LTPredCodGain_Q7.Val = Inlines.silk_SMULBB(3, Inlines.silk_lin2log(inLin) - 2048);
			}
			num = 0;
			for (int i = 0; i < nb_subfr; i++)
			{
				array3[i] = 0;
				for (int j = num; j < num + 5; j++)
				{
					array3[i] += (int)b_Q14[j];
				}
				num += 5;
			}
			int num15 = 0;
			int num16 = 0;
			for (int i = 0; i < nb_subfr; i++)
			{
				num15 = Inlines.silk_max_32(num15, Inlines.silk_abs(array3[i]));
				num16 = Inlines.silk_max_32(num16, 32 - Inlines.silk_CLZ32(array5[i]) + corr_rshifts[i] - num12);
			}
			num7 = num16 + 32 - Inlines.silk_CLZ32(num15) - 14;
			num7 -= 29 + num12;
			num7 = Inlines.silk_max_int(num7, 0);
			int num17 = num12 + num7;
			num9 = Inlines.silk_RSHIFT(262, num12 + num7) + 1;
			int num18 = 0;
			for (int i = 0; i < nb_subfr; i++)
			{
				num9 = Inlines.silk_ADD32(num9, Inlines.silk_RSHIFT(array5[i], num17 - corr_rshifts[i]));
				num18 = Inlines.silk_ADD32(num18, Inlines.silk_LSHIFT(Inlines.silk_SMULWW(Inlines.silk_RSHIFT(array5[i], num17 - corr_rshifts[i]), array3[i]), 2));
			}
			int a = Inlines.silk_DIV32_varQ(num18, num9, 12);
			num = 0;
			for (int i = 0; i < nb_subfr; i++)
			{
				if (2 - corr_rshifts[i] > 0)
				{
					num9 = Inlines.silk_RSHIFT(array5[i], 2 - corr_rshifts[i]);
				}
				else
				{
					num9 = Inlines.silk_LSHIFT_SAT32(array5[i], corr_rshifts[i] - 2);
				}
				int a2 = Inlines.silk_MUL(Inlines.silk_DIV32(6710887, Inlines.silk_RSHIFT(6710887, 10) + num9), Inlines.silk_LSHIFT_SAT32(Inlines.silk_SUB_SAT32(a, Inlines.silk_RSHIFT(array3[i], 2)), 4));
				num9 = 0;
				for (int j = 0; j < 5; j++)
				{
					array2[j] = (int)Inlines.silk_max_16(b_Q14[num + j], 1638);
					num9 += array2[j];
				}
				num9 = Inlines.silk_DIV32(a2, num9);
				for (int j = 0; j < 5; j++)
				{
					b_Q14[num + j] = (short)Inlines.silk_LIMIT_32((int)b_Q14[num + j] + Inlines.silk_SMULWB(Inlines.silk_LSHIFT_SAT32(num9, 4), array2[j]), -16000, 28000);
				}
				num += 5;
			}
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x0000AA20 File Offset: 0x00008C20
		internal unsafe static void silk_fit_LTP(int[] LTP_coefs_Q16, Span<short> LTP_coefs_Q14, int LTP_coefs_Q14_ptr)
		{
			for (int i = 0; i < 5; i++)
			{
				*LTP_coefs_Q14[LTP_coefs_Q14_ptr + i] = (short)Inlines.silk_SAT16(Inlines.silk_RSHIFT_ROUND(LTP_coefs_Q16[i], 2));
			}
		}

		// Token: 0x04000035 RID: 53
		private const int LTP_CORRS_HEAD_ROOM = 2;
	}
}
