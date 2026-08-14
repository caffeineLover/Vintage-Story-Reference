using System;
using Concentus.Common;
using Concentus.Common.CPlusPlus;

namespace Concentus.Silk
{
	// Token: 0x02000017 RID: 23
	internal static class CorrelateMatrix
	{
		// Token: 0x060000B2 RID: 178 RVA: 0x00006320 File Offset: 0x00004520
		internal static void silk_corrVector(short[] x, int x_ptr, short[] t, int t_ptr, int L, int order, int[] Xt, int rshifts)
		{
			int num = x_ptr + order - 1;
			if (rshifts > 0)
			{
				for (int i = 0; i < order; i++)
				{
					int num2 = 0;
					for (int j = 0; j < L; j++)
					{
						num2 += Inlines.silk_RSHIFT32(Inlines.silk_SMULBB((int)x[num + j], (int)t[t_ptr + j]), rshifts);
					}
					Xt[i] = num2;
					num--;
				}
				return;
			}
			for (int i = 0; i < order; i++)
			{
				Xt[i] = Inlines.silk_inner_prod(x, num, t, t_ptr, L);
				num--;
			}
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x000063A0 File Offset: 0x000045A0
		internal static void silk_corrMatrix(short[] x, int x_ptr, int L, int order, int head_room, int[] XX, int XX_ptr, BoxedValueInt rshifts)
		{
			int num;
			int num2;
			SumSqrShift.silk_sum_sqr_shift(out num, out num2, x, x_ptr, L + order - 1);
			int num3 = Inlines.silk_max(head_room - Inlines.silk_CLZ32(num), 0);
			num = Inlines.silk_RSHIFT32(num, num3);
			num2 += num3;
			for (int i = x_ptr; i < x_ptr + order - 1; i++)
			{
				num -= Inlines.silk_RSHIFT32(Inlines.silk_SMULBB((int)x[i], (int)x[i]), num2);
			}
			if (num2 < rshifts.Val)
			{
				num = Inlines.silk_RSHIFT32(num, rshifts.Val - num2);
				num2 = rshifts.Val;
			}
			Inlines.MatrixSet<int>(XX, XX_ptr, 0, 0, order, num);
			int num4 = x_ptr + order - 1;
			for (int j = 1; j < order; j++)
			{
				num = Inlines.silk_SUB32(num, Inlines.silk_RSHIFT32(Inlines.silk_SMULBB((int)x[num4 + L - j], (int)x[num4 + L - j]), num2));
				num = Inlines.silk_ADD32(num, Inlines.silk_RSHIFT32(Inlines.silk_SMULBB((int)x[num4 - j], (int)x[num4 - j]), num2));
				Inlines.MatrixSet<int>(XX, XX_ptr, j, j, order, num);
			}
			int num5 = x_ptr + order - 2;
			if (num2 > 0)
			{
				for (int k = 1; k < order; k++)
				{
					num = 0;
					for (int i = 0; i < L; i++)
					{
						num += Inlines.silk_RSHIFT32(Inlines.silk_SMULBB((int)x[num4 + i], (int)x[num5 + i]), num2);
					}
					Inlines.MatrixSet<int>(XX, XX_ptr, k, 0, order, num);
					Inlines.MatrixSet<int>(XX, XX_ptr, 0, k, order, num);
					for (int j = 1; j < order - k; j++)
					{
						num = Inlines.silk_SUB32(num, Inlines.silk_RSHIFT32(Inlines.silk_SMULBB((int)x[num4 + L - j], (int)x[num5 + L - j]), num2));
						num = Inlines.silk_ADD32(num, Inlines.silk_RSHIFT32(Inlines.silk_SMULBB((int)x[num4 - j], (int)x[num5 - j]), num2));
						Inlines.MatrixSet<int>(XX, XX_ptr, k + j, j, order, num);
						Inlines.MatrixSet<int>(XX, XX_ptr, j, k + j, order, num);
					}
					num5--;
				}
			}
			else
			{
				for (int k = 1; k < order; k++)
				{
					num = Inlines.silk_inner_prod(x, num4, x, num5, L);
					Inlines.MatrixSet<int>(XX, XX_ptr, k, 0, order, num);
					Inlines.MatrixSet<int>(XX, XX_ptr, 0, k, order, num);
					for (int j = 1; j < order - k; j++)
					{
						num = Inlines.silk_SUB32(num, Inlines.silk_SMULBB((int)x[num4 + L - j], (int)x[num5 + L - j]));
						num = Inlines.silk_SMLABB(num, (int)x[num4 - j], (int)x[num5 - j]);
						Inlines.MatrixSet<int>(XX, XX_ptr, k + j, j, order, num);
						Inlines.MatrixSet<int>(XX, XX_ptr, j, k + j, order, num);
					}
					num5--;
				}
			}
			rshifts.Val = num2;
		}
	}
}
