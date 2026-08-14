using System;
using Concentus.Common;

namespace Concentus.Silk
{
	// Token: 0x0200002B RID: 43
	internal static class LTPAnalysisFilter
	{
		// Token: 0x060000E4 RID: 228 RVA: 0x0000B960 File Offset: 0x00009B60
		internal static void silk_LTP_analysis_filter(short[] LTP_res, short[] x, int x_ptr, short[] LTPCoef_Q14, int[] pitchL, int[] invGains_Q16, int subfr_length, int nb_subfr, int pre_length)
		{
			short[] array = new short[5];
			int num = x_ptr;
			int num2 = 0;
			for (int i = 0; i < nb_subfr; i++)
			{
				int num3 = num - pitchL[i];
				array[0] = LTPCoef_Q14[i * 5];
				array[1] = LTPCoef_Q14[i * 5 + 1];
				array[2] = LTPCoef_Q14[i * 5 + 2];
				array[3] = LTPCoef_Q14[i * 5 + 3];
				array[4] = LTPCoef_Q14[i * 5 + 4];
				for (int j = 0; j < subfr_length + pre_length; j++)
				{
					int num4 = num2 + j;
					LTP_res[num4] = x[num + j];
					int num5 = Inlines.silk_SMULBB((int)x[num3 + 2], (int)array[0]);
					num5 = Inlines.silk_SMLABB_ovflw(num5, (int)x[num3 + 1], (int)array[1]);
					num5 = Inlines.silk_SMLABB_ovflw(num5, (int)x[num3], (int)array[2]);
					num5 = Inlines.silk_SMLABB_ovflw(num5, (int)x[num3 - 1], (int)array[3]);
					num5 = Inlines.silk_SMLABB_ovflw(num5, (int)x[num3 - 2], (int)array[4]);
					num5 = Inlines.silk_RSHIFT_ROUND(num5, 14);
					LTP_res[num4] = (short)Inlines.silk_SAT16((int)x[num + j] - num5);
					LTP_res[num4] = (short)Inlines.silk_SMULWB(invGains_Q16[i], (int)LTP_res[num4]);
					num3++;
				}
				num2 += subfr_length + pre_length;
				num += subfr_length;
			}
		}
	}
}
