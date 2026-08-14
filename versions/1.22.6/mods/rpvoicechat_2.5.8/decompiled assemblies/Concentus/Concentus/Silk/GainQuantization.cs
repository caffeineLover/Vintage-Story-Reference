using System;
using Concentus.Common;
using Concentus.Common.CPlusPlus;

namespace Concentus.Silk
{
	// Token: 0x02000026 RID: 38
	internal static class GainQuantization
	{
		// Token: 0x060000D4 RID: 212 RVA: 0x0000B080 File Offset: 0x00009280
		internal static void silk_gains_quant(sbyte[] ind, int[] gain_Q16, BoxedValueSbyte prev_ind, int conditional, int nb_subfr)
		{
			for (int i = 0; i < nb_subfr; i++)
			{
				ind[i] = (sbyte)Inlines.silk_SMULWB(GainQuantization.SCALE_Q16, Inlines.silk_lin2log(gain_Q16[i]) - GainQuantization.OFFSET);
				if (ind[i] < prev_ind.Val)
				{
					int num = i;
					ind[num] += 1;
				}
				ind[i] = (sbyte)Inlines.silk_LIMIT_int((int)ind[i], 0, 63);
				if (i == 0 && conditional == 0)
				{
					ind[i] = (sbyte)Inlines.silk_LIMIT_int((int)ind[i], (int)(prev_ind.Val + -4), 63);
					prev_ind.Val = ind[i];
				}
				else
				{
					ind[i] -= prev_ind.Val;
					int num2 = (int)(8 + prev_ind.Val);
					if ((int)ind[i] > num2)
					{
						ind[i] = (sbyte)(num2 + Inlines.silk_RSHIFT((int)ind[i] - num2 + 1, 1));
					}
					ind[i] = (sbyte)Inlines.silk_LIMIT_int((int)ind[i], -4, 36);
					if ((int)ind[i] > num2)
					{
						prev_ind.Val += (sbyte)(Inlines.silk_LSHIFT((int)ind[i], 1) - num2);
					}
					else
					{
						prev_ind.Val += ind[i];
					}
					int num3 = i;
					ind[num3] -= -4;
				}
				gain_Q16[i] = Inlines.silk_log2lin(Inlines.silk_min_32(Inlines.silk_SMULWB(GainQuantization.INV_SCALE_Q16, (int)prev_ind.Val) + GainQuantization.OFFSET, 3967));
			}
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x0000B1B8 File Offset: 0x000093B8
		internal static void silk_gains_dequant(int[] gain_Q16, sbyte[] ind, BoxedValueSbyte prev_ind, int conditional, int nb_subfr)
		{
			for (int i = 0; i < nb_subfr; i++)
			{
				if (i == 0 && conditional == 0)
				{
					prev_ind.Val = (sbyte)Inlines.silk_max_int((int)ind[i], (int)(prev_ind.Val - 16));
				}
				else
				{
					int num = (int)(ind[i] + -4);
					int num2 = (int)(8 + prev_ind.Val);
					if (num > num2)
					{
						prev_ind.Val += (sbyte)(Inlines.silk_LSHIFT(num, 1) - num2);
					}
					else
					{
						prev_ind.Val += (sbyte)num;
					}
				}
				prev_ind.Val = (sbyte)Inlines.silk_LIMIT_int((int)prev_ind.Val, 0, 63);
				gain_Q16[i] = Inlines.silk_log2lin(Inlines.silk_min_32(Inlines.silk_SMULWB(GainQuantization.INV_SCALE_Q16, (int)prev_ind.Val) + GainQuantization.OFFSET, 3967));
			}
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x0000B274 File Offset: 0x00009474
		internal static int silk_gains_ID(sbyte[] ind, int nb_subfr)
		{
			int num = 0;
			for (int i = 0; i < nb_subfr; i++)
			{
				num = Inlines.silk_ADD_LSHIFT32((int)ind[i], num, 8);
			}
			return num;
		}

		// Token: 0x04000036 RID: 54
		private static readonly int OFFSET = 2090;

		// Token: 0x04000037 RID: 55
		private static readonly int SCALE_Q16 = 2251;

		// Token: 0x04000038 RID: 56
		private static readonly int INV_SCALE_Q16 = 1907825;
	}
}
