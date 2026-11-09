using System;
using Concentus.Common;

namespace Concentus.Celt
{
	// Token: 0x02000086 RID: 134
	internal static class CeltLPC
	{
		// Token: 0x060004C4 RID: 1220 RVA: 0x0002D544 File Offset: 0x0002B744
		internal unsafe static void celt_lpc(int[] _lpc, int[] ac, int p)
		{
			int num = ac[0];
			Span<int> span = new Span<int>(stackalloc byte[(UIntPtr)96], 24);
			if (ac[0] != 0)
			{
				for (int i = 0; i < p; i++)
				{
					int num2 = 0;
					for (int j = 0; j < i; j++)
					{
						num2 += Inlines.MULT32_32_Q31(*span[j], ac[i - j]);
					}
					num2 += Inlines.SHR32(ac[i + 1], 3);
					int num3 = 0 - Inlines.frac_div32(Inlines.SHL32(num2, 3), num);
					*span[i] = Inlines.SHR32(num3, 3);
					for (int j = 0; j < i + 1 >> 1; j++)
					{
						int num4 = *span[j];
						int num5 = *span[i - 1 - j];
						*span[j] = num4 + Inlines.MULT32_32_Q31(num3, num5);
						*span[i - 1 - j] = num5 + Inlines.MULT32_32_Q31(num3, num4);
					}
					num -= Inlines.MULT32_32_Q31(Inlines.MULT32_32_Q31(num3, num3), num);
					if (num < Inlines.SHR32(ac[0], 10))
					{
						break;
					}
				}
			}
			for (int i = 0; i < p; i++)
			{
				_lpc[i] = Inlines.ROUND16(*span[i], 16);
			}
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x0002D668 File Offset: 0x0002B868
		internal unsafe static void celt_iir(Span<int> _x, int[] den, Span<int> _y, int N, int ord, Span<int> mem)
		{
			int[] array = new int[ord];
			int[] array2 = new int[N + ord];
			int i;
			for (i = 0; i < ord; i++)
			{
				array[i] = den[ord - i - 1];
			}
			for (i = 0; i < ord; i++)
			{
				array2[i] = 0 - *mem[ord - i - 1];
			}
			while (i < N + ord)
			{
				array2[i] = 0;
				i++;
			}
			for (i = 0; i < N - 3; i += 4)
			{
				int num = *_x[i];
				int num2 = *_x[i + 1];
				int num3 = *_x[i + 2];
				int num4 = *_x[i + 3];
				Kernels.xcorr_kernel(array, 0, array2, i, ref num, ref num2, ref num3, ref num4, ord);
				array2[i + ord] = 0 - Inlines.ROUND16(num, 12);
				*_y[i] = num;
				num2 = Inlines.MAC16_16(num2, array2[i + ord], den[0]);
				array2[i + ord + 1] = 0 - Inlines.ROUND16(num2, 12);
				*_y[i + 1] = num2;
				num3 = Inlines.MAC16_16(num3, array2[i + ord + 1], den[0]);
				num3 = Inlines.MAC16_16(num3, array2[i + ord], den[1]);
				array2[i + ord + 2] = 0 - Inlines.ROUND16(num3, 12);
				*_y[i + 2] = num3;
				num4 = Inlines.MAC16_16(num4, array2[i + ord + 2], den[0]);
				num4 = Inlines.MAC16_16(num4, array2[i + ord + 1], den[1]);
				num4 = Inlines.MAC16_16(num4, array2[i + ord], den[2]);
				array2[i + ord + 3] = 0 - Inlines.ROUND16(num4, 12);
				*_y[i + 3] = num4;
			}
			while (i < N)
			{
				int num5 = *_x[i];
				for (int j = 0; j < ord; j++)
				{
					num5 -= Inlines.MULT16_16(array[j], array2[i + j]);
				}
				array2[i + ord] = Inlines.ROUND16(num5, 12);
				*_y[i] = num5;
				i++;
			}
			for (i = 0; i < ord; i++)
			{
				*mem[i] = *_y[N - i - 1];
			}
		}

		// Token: 0x0400044F RID: 1103
		private const int LPC_ORDER = 24;
	}
}
