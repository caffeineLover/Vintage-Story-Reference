using System;
using Concentus.Common;

namespace Concentus.Silk
{
	// Token: 0x02000035 RID: 53
	internal static class ResidualEnergy
	{
		// Token: 0x06000111 RID: 273 RVA: 0x00010B24 File Offset: 0x0000ED24
		internal static void silk_residual_energy(int[] nrgs, int[] nrgsQ, short[] x, short[][] a_Q12, int[] gains, int subfr_length, int nb_subfr, int LPC_order)
		{
			int num = 0;
			int num2 = LPC_order + subfr_length;
			short[] array = new short[2 * num2];
			for (int i = 0; i < nb_subfr >> 1; i++)
			{
				Filters.silk_LPC_analysis_filter(array, 0, x, num, a_Q12[i], 0, 2 * num2, LPC_order);
				int num3 = LPC_order;
				for (int j = 0; j < 2; j++)
				{
					int num4;
					int num5;
					SumSqrShift.silk_sum_sqr_shift(out num4, out num5, array, num3, subfr_length);
					nrgs[i * 2 + j] = num4;
					nrgsQ[i * 2 + j] = 0 - num5;
					num3 += num2;
				}
				num += 2 * num2;
			}
			for (int i = 0; i < nb_subfr; i++)
			{
				int num6 = Inlines.silk_CLZ32(nrgs[i]) - 1;
				int num7 = Inlines.silk_CLZ32(gains[i]) - 1;
				int num8 = Inlines.silk_LSHIFT32(gains[i], num7);
				num8 = Inlines.silk_SMMUL(num8, num8);
				nrgs[i] = Inlines.silk_SMMUL(num8, Inlines.silk_LSHIFT32(nrgs[i], num6));
				nrgsQ[i] += num6 + 2 * num7 - 32 - 32;
			}
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00010C24 File Offset: 0x0000EE24
		internal static int silk_residual_energy16_covar(short[] c, int c_ptr, int[] wXX, int wXX_ptr, int[] wXx, int wxx, int D, int cQ)
		{
			int[] array = new int[D];
			int num = 16 - cQ;
			int num2 = num;
			int num3 = 0;
			for (int i = c_ptr; i < c_ptr + D; i++)
			{
				num3 = Inlines.silk_max_32(num3, Inlines.silk_abs((int)c[i]));
			}
			num2 = Inlines.silk_min_int(num2, Inlines.silk_CLZ32(num3) - 17);
			int a = Inlines.silk_max_32(wXX[wXX_ptr], wXX[wXX_ptr + D * D - 1]);
			num2 = Inlines.silk_min_int(num2, Inlines.silk_CLZ32(Inlines.silk_MUL(D, Inlines.silk_RSHIFT(Inlines.silk_SMULWB(a, num3), 4))) - 5);
			num2 = Inlines.silk_max_int(num2, 0);
			for (int i = 0; i < D; i++)
			{
				array[i] = Inlines.silk_LSHIFT((int)c[c_ptr + i], num2);
			}
			num -= num2;
			int num4 = 0;
			for (int i = 0; i < D; i++)
			{
				num4 = Inlines.silk_SMLAWB(num4, wXx[i], array[i]);
			}
			int num5 = Inlines.silk_RSHIFT(wxx, 1 + num) - num4;
			int num6 = 0;
			for (int i = 0; i < D; i++)
			{
				num4 = 0;
				int num7 = wXX_ptr + i * D;
				for (int j = i + 1; j < D; j++)
				{
					num4 = Inlines.silk_SMLAWB(num4, wXX[num7 + j], array[j]);
				}
				num4 = Inlines.silk_SMLAWB(num4, Inlines.silk_RSHIFT(wXX[num7 + i], 1), array[i]);
				num6 = Inlines.silk_SMLAWB(num6, num4, array[i]);
			}
			num5 = Inlines.silk_ADD_LSHIFT32(num5, num6, num);
			if (num5 < 1)
			{
				num5 = 1;
			}
			else if (num5 > Inlines.silk_RSHIFT(2147483647, num + 2))
			{
				num5 = 1073741823;
			}
			else
			{
				num5 = Inlines.silk_LSHIFT(num5, num + 1);
			}
			return num5;
		}
	}
}
