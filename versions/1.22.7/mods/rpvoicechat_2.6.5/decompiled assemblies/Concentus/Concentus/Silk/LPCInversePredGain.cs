using System;
using Concentus.Common;
using Concentus.Common.CPlusPlus;

namespace Concentus.Silk
{
	// Token: 0x0200002A RID: 42
	internal static class LPCInversePredGain
	{
		// Token: 0x060000E0 RID: 224 RVA: 0x0000B7B0 File Offset: 0x000099B0
		internal static int LPC_inverse_pred_gain_QA(int[][] A_QA, int order)
		{
			int[] array = A_QA[order & 1];
			int a = 1073741824;
			int num;
			int num2;
			for (int i = order - 1; i > 0; i--)
			{
				if (array[i] > LPCInversePredGain.A_LIMIT || array[i] < -LPCInversePredGain.A_LIMIT)
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
			if (array[0] > LPCInversePredGain.A_LIMIT || array[0] < -LPCInversePredGain.A_LIMIT)
			{
				return 0;
			}
			num = 0 - Inlines.silk_LSHIFT(array[0], 7);
			num2 = 1073741824 - Inlines.silk_SMMUL(num, num);
			return Inlines.silk_LSHIFT(Inlines.silk_SMMUL(a, num2), 2);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x0000B8C8 File Offset: 0x00009AC8
		internal static int silk_LPC_inverse_pred_gain(short[] A_Q12, int order)
		{
			int[][] array = Arrays.InitTwoDimensionalArray<int>(2, 16);
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
			return LPCInversePredGain.LPC_inverse_pred_gain_QA(array, order);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x0000B918 File Offset: 0x00009B18
		internal static int silk_LPC_inverse_pred_gain_Q24(int[] A_Q24, int order)
		{
			int[][] array = Arrays.InitTwoDimensionalArray<int>(2, 16);
			int[] array2 = array[order & 1];
			for (int i = 0; i < order; i++)
			{
				array2[i] = Inlines.silk_RSHIFT32(A_Q24[i], 0);
			}
			return LPCInversePredGain.LPC_inverse_pred_gain_QA(array, order);
		}

		// Token: 0x04000039 RID: 57
		private const float RC_THRESHOLD = 0.9999f;

		// Token: 0x0400003A RID: 58
		private const int QA = 24;

		// Token: 0x0400003B RID: 59
		private static readonly int A_LIMIT = 16773022;
	}
}
