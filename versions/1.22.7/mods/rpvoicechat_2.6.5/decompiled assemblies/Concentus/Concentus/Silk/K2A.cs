using System;
using Concentus.Common;

namespace Concentus.Silk
{
	// Token: 0x02000028 RID: 40
	internal static class K2A
	{
		// Token: 0x060000D9 RID: 217 RVA: 0x0000B3AC File Offset: 0x000095AC
		internal static void silk_k2a(int[] A_Q24, short[] rc_Q15, int order)
		{
			int[] array = new int[16];
			for (int i = 0; i < order; i++)
			{
				for (int j = 0; j < i; j++)
				{
					array[j] = A_Q24[j];
				}
				for (int j = 0; j < i; j++)
				{
					A_Q24[j] = Inlines.silk_SMLAWB(A_Q24[j], Inlines.silk_LSHIFT(array[i - j - 1], 1), (int)rc_Q15[i]);
				}
				A_Q24[i] = 0 - Inlines.silk_LSHIFT((int)rc_Q15[i], 9);
			}
		}

		// Token: 0x060000DA RID: 218 RVA: 0x0000B418 File Offset: 0x00009618
		internal static void silk_k2a_Q16(int[] A_Q24, int[] rc_Q16, int order)
		{
			int[] array = new int[16];
			for (int i = 0; i < order; i++)
			{
				for (int j = 0; j < i; j++)
				{
					array[j] = A_Q24[j];
				}
				for (int j = 0; j < i; j++)
				{
					A_Q24[j] = Inlines.silk_SMLAWW(A_Q24[j], array[i - j - 1], rc_Q16[i]);
				}
				A_Q24[i] = 0 - Inlines.silk_LSHIFT(rc_Q16[i], 8);
			}
		}
	}
}
