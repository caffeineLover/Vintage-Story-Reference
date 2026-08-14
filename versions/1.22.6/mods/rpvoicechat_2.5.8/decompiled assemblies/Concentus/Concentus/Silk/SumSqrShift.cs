using System;
using Concentus.Common;

namespace Concentus.Silk
{
	// Token: 0x0200003C RID: 60
	internal static class SumSqrShift
	{
		// Token: 0x06000129 RID: 297 RVA: 0x00012470 File Offset: 0x00010670
		internal unsafe static void silk_sum_sqr_shift(out int energy, out int shift, Span<short> x, int x_ptr, int len)
		{
			int num = 0;
			int num2 = 0;
			len--;
			for (int i = 0; i < len; i += 2)
			{
				num = Inlines.silk_SMLABB_ovflw(num, (int)(*x[x_ptr + i]), (int)(*x[x_ptr + i]));
				num = Inlines.silk_SMLABB_ovflw(num, (int)(*x[x_ptr + i + 1]), (int)(*x[x_ptr + i + 1]));
				if (num < 0)
				{
					num = (int)Inlines.silk_RSHIFT_uint((uint)num, 2);
					num2 = 2;
					i += 2;
					IL_C5:
					while (i < len)
					{
						int num3 = Inlines.silk_SMULBB((int)(*x[x_ptr + i]), (int)(*x[x_ptr + i]));
						num3 = Inlines.silk_SMLABB_ovflw(num3, (int)(*x[x_ptr + i + 1]), (int)(*x[x_ptr + i + 1]));
						num = (int)Inlines.silk_ADD_RSHIFT_uint((uint)num, (uint)num3, num2);
						if (num < 0)
						{
							num = (int)Inlines.silk_RSHIFT_uint((uint)num, 2);
							num2 += 2;
						}
						i += 2;
					}
					if (i == len)
					{
						int num3 = Inlines.silk_SMULBB((int)(*x[x_ptr + i]), (int)(*x[x_ptr + i]));
						num = (int)Inlines.silk_ADD_RSHIFT_uint((uint)num, (uint)num3, num2);
					}
					if (((long)num & (long)((ulong)-1073741824)) != 0L)
					{
						num = (int)Inlines.silk_RSHIFT_uint((uint)num, 2);
						num2 += 2;
					}
					shift = num2;
					energy = num;
					return;
				}
			}
			goto IL_C5;
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00012590 File Offset: 0x00010790
		internal static void silk_sum_sqr_shift(out int energy, out int shift, short[] x, int len)
		{
			int num = 0;
			int num2 = 0;
			len--;
			for (int i = 0; i < len; i += 2)
			{
				num = Inlines.silk_SMLABB_ovflw(num, (int)x[i], (int)x[i]);
				num = Inlines.silk_SMLABB_ovflw(num, (int)x[i + 1], (int)x[i + 1]);
				if (num < 0)
				{
					num = (int)Inlines.silk_RSHIFT_uint((uint)num, 2);
					num2 = 2;
					i += 2;
					IL_83:
					while (i < len)
					{
						int num3 = Inlines.silk_SMULBB((int)x[i], (int)x[i]);
						num3 = Inlines.silk_SMLABB_ovflw(num3, (int)x[i + 1], (int)x[i + 1]);
						num = (int)Inlines.silk_ADD_RSHIFT_uint((uint)num, (uint)num3, num2);
						if (num < 0)
						{
							num = (int)Inlines.silk_RSHIFT_uint((uint)num, 2);
							num2 += 2;
						}
						i += 2;
					}
					if (i == len)
					{
						int num3 = Inlines.silk_SMULBB((int)x[i], (int)x[i]);
						num = (int)Inlines.silk_ADD_RSHIFT_uint((uint)num, (uint)num3, num2);
					}
					if (((long)num & (long)((ulong)-1073741824)) != 0L)
					{
						num = (int)Inlines.silk_RSHIFT_uint((uint)num, 2);
						num2 += 2;
					}
					shift = num2;
					energy = num;
					return;
				}
			}
			goto IL_83;
		}
	}
}
