using System;
using Concentus.Common;
using Concentus.Common.CPlusPlus;
using Concentus.Silk.Structs;

namespace Concentus.Silk
{
	// Token: 0x02000022 RID: 34
	internal static class FindLPC
	{
		// Token: 0x060000CF RID: 207 RVA: 0x0000A27C File Offset: 0x0000847C
		internal static void silk_find_LPC(SilkChannelEncoder psEncC, short[] NLSF_Q15, short[] x, int minInvGain_Q30)
		{
			int[] a_Q = new int[16];
			BoxedValueInt boxedValueInt = new BoxedValueInt(0);
			BoxedValueInt boxedValueInt2 = new BoxedValueInt(0);
			int[] a_Q2 = new int[16];
			short[] array = new short[16];
			short[] array2 = new short[16];
			int num = psEncC.subfr_length + psEncC.predictLPCOrder;
			psEncC.indices.NLSFInterpCoef_Q2 = 4;
			BurgModified.silk_burg_modified(boxedValueInt, boxedValueInt2, a_Q, x, 0, minInvGain_Q30, num, psEncC.nb_subfr, psEncC.predictLPCOrder);
			int num2 = boxedValueInt.Val;
			int num3 = boxedValueInt2.Val;
			if (psEncC.useInterpolatedNLSFs != 0 && psEncC.first_frame_after_reset == 0 && psEncC.nb_subfr == 4)
			{
				BurgModified.silk_burg_modified(boxedValueInt, boxedValueInt2, a_Q2, x, 2 * num, minInvGain_Q30, num, 2, psEncC.predictLPCOrder);
				int val = boxedValueInt.Val;
				int val2 = boxedValueInt2.Val;
				int num4 = val2 - num3;
				if (num4 >= 0)
				{
					if (num4 < 32)
					{
						num2 -= Inlines.silk_RSHIFT(val, num4);
					}
				}
				else
				{
					num2 = Inlines.silk_RSHIFT(num2, -num4) - val;
					num3 = val2;
				}
				NLSF.silk_A2NLSF(NLSF_Q15, a_Q2, psEncC.predictLPCOrder);
				short[] array3 = new short[2 * num];
				for (int i = 3; i >= 0; i--)
				{
					Inlines.silk_interpolate(array2, psEncC.prev_NLSFq_Q15, NLSF_Q15, i, psEncC.predictLPCOrder);
					NLSF.silk_NLSF2A(array, array2, psEncC.predictLPCOrder);
					Filters.silk_LPC_analysis_filter(array3, 0, x, 0, array, 0, 2 * num, psEncC.predictLPCOrder);
					int a;
					int num5;
					SumSqrShift.silk_sum_sqr_shift(out a, out num5, array3, psEncC.predictLPCOrder, num - psEncC.predictLPCOrder);
					int num6;
					int num7;
					SumSqrShift.silk_sum_sqr_shift(out num6, out num7, array3, psEncC.predictLPCOrder + num, num - psEncC.predictLPCOrder);
					num4 = num5 - num7;
					int num8;
					if (num4 >= 0)
					{
						num6 = Inlines.silk_RSHIFT(num6, num4);
						num8 = -num5;
					}
					else
					{
						a = Inlines.silk_RSHIFT(a, -num4);
						num8 = -num7;
					}
					int num9 = Inlines.silk_ADD32(a, num6);
					num4 = num8 - num3;
					int num10;
					if (num4 >= 0)
					{
						if (Inlines.silk_RSHIFT(num9, num4) < num2)
						{
							num10 = 1;
						}
						else
						{
							num10 = 0;
						}
					}
					else if (-num4 < 32)
					{
						if (num9 < Inlines.silk_RSHIFT(num2, -num4))
						{
							num10 = 1;
						}
						else
						{
							num10 = 0;
						}
					}
					else
					{
						num10 = 0;
					}
					if (num10 == 1)
					{
						num2 = num9;
						num3 = num8;
						psEncC.indices.NLSFInterpCoef_Q2 = (sbyte)i;
					}
				}
			}
			if (psEncC.indices.NLSFInterpCoef_Q2 == 4)
			{
				NLSF.silk_A2NLSF(NLSF_Q15, a_Q, psEncC.predictLPCOrder);
			}
		}
	}
}
