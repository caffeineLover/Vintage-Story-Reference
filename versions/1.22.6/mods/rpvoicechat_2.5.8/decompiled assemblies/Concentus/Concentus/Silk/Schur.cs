using System;
using Concentus.Common;
using Concentus.Common.CPlusPlus;

namespace Concentus.Silk
{
	// Token: 0x02000036 RID: 54
	internal static class Schur
	{
		// Token: 0x06000113 RID: 275 RVA: 0x00010DAC File Offset: 0x0000EFAC
		internal static int silk_schur(short[] rc_Q15, int[] c, int order)
		{
			int[][] array = Arrays.InitTwoDimensionalArray<int>(17, 2);
			int num = Inlines.silk_CLZ32(c[0]);
			if (num < 2)
			{
				for (int i = 0; i < order + 1; i++)
				{
					array[i][0] = (array[i][1] = Inlines.silk_RSHIFT(c[i], 1));
				}
			}
			else if (num > 2)
			{
				num -= 2;
				for (int i = 0; i < order + 1; i++)
				{
					array[i][0] = (array[i][1] = Inlines.silk_LSHIFT(c[i], num));
				}
			}
			else
			{
				for (int i = 0; i < order + 1; i++)
				{
					array[i][0] = (array[i][1] = c[i]);
				}
			}
			for (int i = 0; i < order; i++)
			{
				if (Inlines.silk_abs_int32(array[i + 1][0]) >= array[0][1])
				{
					if (array[i + 1][0] > 0)
					{
						rc_Q15[i] = -32440;
					}
					else
					{
						rc_Q15[i] = 32440;
					}
					i++;
					IL_163:
					while (i < order)
					{
						rc_Q15[i] = 0;
						i++;
					}
					return Inlines.silk_max_32(1, array[0][1]);
				}
				int num2 = 0 - Inlines.silk_DIV32_16(array[i + 1][0], Inlines.silk_max_32(Inlines.silk_RSHIFT(array[0][1], 15), 1));
				num2 = Inlines.silk_SAT16(num2);
				rc_Q15[i] = (short)num2;
				for (int j = 0; j < order - i; j++)
				{
					int num3 = array[j + i + 1][0];
					int num4 = array[j][1];
					array[j + i + 1][0] = Inlines.silk_SMLAWB(num3, Inlines.silk_LSHIFT(num4, 1), num2);
					array[j][1] = Inlines.silk_SMLAWB(num4, Inlines.silk_LSHIFT(num3, 1), num2);
				}
			}
			goto IL_163;
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00010F2C File Offset: 0x0000F12C
		internal static int silk_schur64(int[] rc_Q16, int[] c, int order)
		{
			int[][] array = Arrays.InitTwoDimensionalArray<int>(17, 2);
			if (c[0] <= 0)
			{
				Arrays.MemSetInt(rc_Q16, 0, order);
				return 0;
			}
			for (int i = 0; i < order + 1; i++)
			{
				array[i][0] = (array[i][1] = c[i]);
			}
			for (int i = 0; i < order; i++)
			{
				if (Inlines.silk_abs_int32(array[i + 1][0]) >= array[0][1])
				{
					if (array[i + 1][0] > 0)
					{
						rc_Q16[i] = -64881;
					}
					else
					{
						rc_Q16[i] = 64881;
					}
					i++;
					IL_FE:
					while (i < order)
					{
						rc_Q16[i] = 0;
						i++;
					}
					return Inlines.silk_max_32(1, array[0][1]);
				}
				int num = Inlines.silk_DIV32_varQ(-array[i + 1][0], array[0][1], 31);
				rc_Q16[i] = Inlines.silk_RSHIFT_ROUND(num, 15);
				for (int j = 0; j < order - i; j++)
				{
					int num2 = array[j + i + 1][0];
					int num3 = array[j][1];
					array[j + i + 1][0] = num2 + Inlines.silk_SMMUL(Inlines.silk_LSHIFT(num3, 1), num);
					array[j][1] = num3 + Inlines.silk_SMMUL(Inlines.silk_LSHIFT(num2, 1), num);
				}
			}
			goto IL_FE;
		}
	}
}
