using System;
using Concentus.Common;

namespace Concentus.Silk
{
	// Token: 0x02000012 RID: 18
	internal static class ApplySineWindow
	{
		// Token: 0x060000A6 RID: 166 RVA: 0x00005344 File Offset: 0x00003544
		internal static void silk_apply_sine_window(short[] px_win, int px_win_ptr, short[] px, int px_ptr, int win_type, int length)
		{
			int i = (length >> 2) - 4;
			int num = (int)ApplySineWindow.freq_table_Q16[i];
			int num2 = Inlines.silk_SMULWB(num, -num);
			int num3;
			int num4;
			if (win_type == 1)
			{
				num3 = 0;
				num4 = num + Inlines.silk_RSHIFT(length, 3);
			}
			else
			{
				num3 = 65536;
				num4 = 65536 + Inlines.silk_RSHIFT(num2, 1) + Inlines.silk_RSHIFT(length, 4);
			}
			for (i = 0; i < length; i += 4)
			{
				int num5 = px_win_ptr + i;
				int num6 = px_ptr + i;
				px_win[num5] = (short)Inlines.silk_SMULWB(Inlines.silk_RSHIFT(num3 + num4, 1), (int)px[num6]);
				px_win[num5 + 1] = (short)Inlines.silk_SMULWB(num4, (int)px[num6 + 1]);
				num3 = Inlines.silk_SMULWB(num4, num2) + Inlines.silk_LSHIFT(num4, 1) - num3 + 1;
				num3 = Inlines.silk_min(num3, 65536);
				px_win[num5 + 2] = (short)Inlines.silk_SMULWB(Inlines.silk_RSHIFT(num3 + num4, 1), (int)px[num6 + 2]);
				px_win[num5 + 3] = (short)Inlines.silk_SMULWB(num3, (int)px[num6 + 3]);
				num4 = Inlines.silk_SMULWB(num3, num2) + Inlines.silk_LSHIFT(num3, 1) - num4;
				num4 = Inlines.silk_min(num4, 65536);
			}
		}

		// Token: 0x0400002B RID: 43
		private static readonly short[] freq_table_Q16 = new short[]
		{
			12111,
			9804,
			8235,
			7100,
			6239,
			5565,
			5022,
			4575,
			4202,
			3885,
			3612,
			3375,
			3167,
			2984,
			2820,
			2674,
			2542,
			2422,
			2313,
			2214,
			2123,
			2038,
			1961,
			1889,
			1822,
			1760,
			1702
		};
	}
}
