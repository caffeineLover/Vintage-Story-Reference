using System;
using Concentus.Common;

namespace Concentus.Silk
{
	// Token: 0x02000038 RID: 56
	internal static class Sigmoid
	{
		// Token: 0x0600011B RID: 283 RVA: 0x00011450 File Offset: 0x0000F650
		internal static int silk_sigm_Q15(int in_Q5)
		{
			if (in_Q5 < 0)
			{
				in_Q5 = -in_Q5;
				if (in_Q5 >= 192)
				{
					return 0;
				}
				int num = Inlines.silk_RSHIFT(in_Q5, 5);
				return Sigmoid.sigm_LUT_neg_Q15[num] - Inlines.silk_SMULBB(Sigmoid.sigm_LUT_slope_Q10[num], in_Q5 & 31);
			}
			else
			{
				if (in_Q5 >= 192)
				{
					return 32767;
				}
				int num = Inlines.silk_RSHIFT(in_Q5, 5);
				return Sigmoid.sigm_LUT_pos_Q15[num] + Inlines.silk_SMULBB(Sigmoid.sigm_LUT_slope_Q10[num], in_Q5 & 31);
			}
		}

		// Token: 0x04000058 RID: 88
		private static readonly int[] sigm_LUT_slope_Q10 = new int[]
		{
			237,
			153,
			73,
			30,
			12,
			7
		};

		// Token: 0x04000059 RID: 89
		private static readonly int[] sigm_LUT_pos_Q15 = new int[]
		{
			16384,
			23955,
			28861,
			31213,
			32178,
			32548
		};

		// Token: 0x0400005A RID: 90
		private static readonly int[] sigm_LUT_neg_Q15 = new int[]
		{
			16384,
			8812,
			3906,
			1554,
			589,
			219
		};
	}
}
