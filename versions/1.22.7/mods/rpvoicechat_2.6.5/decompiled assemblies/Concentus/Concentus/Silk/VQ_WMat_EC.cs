using System;
using Concentus.Common;
using Concentus.Common.CPlusPlus;

namespace Concentus.Silk
{
	// Token: 0x02000040 RID: 64
	internal static class VQ_WMat_EC
	{
		// Token: 0x06000131 RID: 305 RVA: 0x00013F84 File Offset: 0x00012184
		internal static void silk_VQ_WMat_EC(BoxedValueSbyte ind, BoxedValueInt rate_dist_Q14, BoxedValueInt gain_Q7, short[] in_Q14, int in_Q14_ptr, int[] W_Q18, int W_Q18_ptr, sbyte[][] cb_Q7, byte[] cb_gain_Q7, byte[] cl_Q5, int mu_Q9, int max_gain_Q7, int L)
		{
			int num = 0;
			short[] array = new short[5];
			rate_dist_Q14.Val = int.MaxValue;
			for (int i = 0; i < L; i++)
			{
				sbyte[] array2 = cb_Q7[num++];
				int num2 = (int)cb_gain_Q7[i];
				array[0] = (short)((int)in_Q14[in_Q14_ptr] - Inlines.silk_LSHIFT((int)array2[0], 7));
				array[1] = (short)((int)in_Q14[in_Q14_ptr + 1] - Inlines.silk_LSHIFT((int)array2[1], 7));
				array[2] = (short)((int)in_Q14[in_Q14_ptr + 2] - Inlines.silk_LSHIFT((int)array2[2], 7));
				array[3] = (short)((int)in_Q14[in_Q14_ptr + 3] - Inlines.silk_LSHIFT((int)array2[3], 7));
				array[4] = (short)((int)in_Q14[in_Q14_ptr + 4] - Inlines.silk_LSHIFT((int)array2[4], 7));
				int num3 = Inlines.silk_SMULBB(mu_Q9, (int)cl_Q5[i]);
				num3 = Inlines.silk_ADD_LSHIFT32(num3, Inlines.silk_max(Inlines.silk_SUB32(num2, max_gain_Q7), 0), 10);
				int num4 = Inlines.silk_SMULWB(W_Q18[W_Q18_ptr + 1], (int)array[1]);
				num4 = Inlines.silk_SMLAWB(num4, W_Q18[W_Q18_ptr + 2], (int)array[2]);
				num4 = Inlines.silk_SMLAWB(num4, W_Q18[W_Q18_ptr + 3], (int)array[3]);
				num4 = Inlines.silk_SMLAWB(num4, W_Q18[W_Q18_ptr + 4], (int)array[4]);
				num4 = Inlines.silk_LSHIFT(num4, 1);
				num4 = Inlines.silk_SMLAWB(num4, W_Q18[W_Q18_ptr], (int)array[0]);
				num3 = Inlines.silk_SMLAWB(num3, num4, (int)array[0]);
				num4 = Inlines.silk_SMULWB(W_Q18[W_Q18_ptr + 7], (int)array[2]);
				num4 = Inlines.silk_SMLAWB(num4, W_Q18[W_Q18_ptr + 8], (int)array[3]);
				num4 = Inlines.silk_SMLAWB(num4, W_Q18[W_Q18_ptr + 9], (int)array[4]);
				num4 = Inlines.silk_LSHIFT(num4, 1);
				num4 = Inlines.silk_SMLAWB(num4, W_Q18[W_Q18_ptr + 6], (int)array[1]);
				num3 = Inlines.silk_SMLAWB(num3, num4, (int)array[1]);
				num4 = Inlines.silk_SMULWB(W_Q18[W_Q18_ptr + 13], (int)array[3]);
				num4 = Inlines.silk_SMLAWB(num4, W_Q18[W_Q18_ptr + 14], (int)array[4]);
				num4 = Inlines.silk_LSHIFT(num4, 1);
				num4 = Inlines.silk_SMLAWB(num4, W_Q18[W_Q18_ptr + 12], (int)array[2]);
				num3 = Inlines.silk_SMLAWB(num3, num4, (int)array[2]);
				num4 = Inlines.silk_SMULWB(W_Q18[W_Q18_ptr + 19], (int)array[4]);
				num4 = Inlines.silk_LSHIFT(num4, 1);
				num4 = Inlines.silk_SMLAWB(num4, W_Q18[W_Q18_ptr + 18], (int)array[3]);
				num3 = Inlines.silk_SMLAWB(num3, num4, (int)array[3]);
				num4 = Inlines.silk_SMULWB(W_Q18[W_Q18_ptr + 24], (int)array[4]);
				num3 = Inlines.silk_SMLAWB(num3, num4, (int)array[4]);
				if (num3 < rate_dist_Q14.Val)
				{
					rate_dist_Q14.Val = num3;
					ind.Val = (sbyte)i;
					gain_Q7.Val = num2;
				}
			}
		}
	}
}
