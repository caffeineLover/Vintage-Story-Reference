using System;
using Concentus.Common;

namespace Concentus.Silk
{
	// Token: 0x02000014 RID: 20
	internal static class BWExpander
	{
		// Token: 0x060000A9 RID: 169 RVA: 0x00005C6C File Offset: 0x00003E6C
		internal static void silk_bwexpander_32(int[] ar, int d, int chirp_Q16)
		{
			int b = chirp_Q16 - 65536;
			for (int i = 0; i < d - 1; i++)
			{
				ar[i] = Inlines.silk_SMULWW(chirp_Q16, ar[i]);
				chirp_Q16 += Inlines.silk_RSHIFT_ROUND(Inlines.silk_MUL(chirp_Q16, b), 16);
			}
			ar[d - 1] = Inlines.silk_SMULWW(chirp_Q16, ar[d - 1]);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00005CC0 File Offset: 0x00003EC0
		internal static void silk_bwexpander(short[] ar, int d, int chirp_Q16)
		{
			int b = chirp_Q16 - 65536;
			for (int i = 0; i < d - 1; i++)
			{
				ar[i] = (short)Inlines.silk_RSHIFT_ROUND(Inlines.silk_MUL(chirp_Q16, (int)ar[i]), 16);
				chirp_Q16 += Inlines.silk_RSHIFT_ROUND(Inlines.silk_MUL(chirp_Q16, b), 16);
			}
			ar[d - 1] = (short)Inlines.silk_RSHIFT_ROUND(Inlines.silk_MUL(chirp_Q16, (int)ar[d - 1]), 16);
		}
	}
}
