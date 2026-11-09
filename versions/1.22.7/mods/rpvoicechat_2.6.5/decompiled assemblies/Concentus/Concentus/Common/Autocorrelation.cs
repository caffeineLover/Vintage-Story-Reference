using System;
using Concentus.Celt;

namespace Concentus.Common
{
	// Token: 0x02000078 RID: 120
	internal static class Autocorrelation
	{
		// Token: 0x0600031A RID: 794 RVA: 0x00023950 File Offset: 0x00021B50
		internal static void silk_autocorr(int[] results, out int scale, short[] inputData, int inputDataSize, int correlationCount)
		{
			int num = Inlines.silk_min_int(inputDataSize, correlationCount);
			scale = Autocorrelation._celt_autocorr(inputData, results, num - 1, inputDataSize);
		}

		// Token: 0x0600031B RID: 795 RVA: 0x00023974 File Offset: 0x00021B74
		internal static int _celt_autocorr(short[] x, int[] ac, int lag, int n)
		{
			int num = n - lag;
			short[] array = new short[n];
			short[] array2 = x;
			int num2 = 1 + (n << 7);
			if ((n & 1) != 0)
			{
				num2 += Inlines.SHR32(Inlines.MULT16_16(array2[0], array2[0]), 9);
			}
			for (int i = n & 1; i < n; i += 2)
			{
				num2 += Inlines.SHR32(Inlines.MULT16_16(array2[i], array2[i]), 9);
				num2 += Inlines.SHR32(Inlines.MULT16_16(array2[i + 1], array2[i + 1]), 9);
			}
			int num3 = Inlines.celt_ilog2(num2) - 30 + 10;
			num3 /= 2;
			if (num3 > 0)
			{
				for (int i = 0; i < n; i++)
				{
					array[i] = (short)Inlines.PSHR32((int)array2[i], num3);
				}
				array2 = array;
			}
			else
			{
				num3 = 0;
			}
			CeltPitchXCorr.pitch_xcorr(array2, 0, array2, 0, ac, num, lag + 1);
			for (int j = 0; j <= lag; j++)
			{
				int i = j + num;
				int num4 = 0;
				while (i < n)
				{
					num4 = Inlines.MAC16_16(num4, array2[i], array2[i - j]);
					i++;
				}
				ac[j] += num4;
			}
			num3 = 2 * num3;
			if (num3 <= 0)
			{
				ac[0] += Inlines.SHL32(1, -num3);
			}
			if (ac[0] < 268435456)
			{
				int num5 = 29 - Inlines.EC_ILOG((uint)ac[0]);
				for (int i = 0; i <= lag; i++)
				{
					ac[i] = Inlines.SHL32(ac[i], num5);
				}
				num3 -= num5;
			}
			else if (ac[0] >= 536870912)
			{
				int num6 = 1;
				if (ac[0] >= 1073741824)
				{
					num6++;
				}
				for (int i = 0; i <= lag; i++)
				{
					ac[i] = Inlines.SHR32(ac[i], num6);
				}
				num3 += num6;
			}
			return num3;
		}

		// Token: 0x0600031C RID: 796 RVA: 0x00023B1C File Offset: 0x00021D1C
		internal static int _celt_autocorr(int[] x, int[] ac, int[] window, int overlap, int lag, int n)
		{
			int num = n - lag;
			int[] array = new int[n];
			int[] array2;
			if (overlap == 0)
			{
				array2 = x;
			}
			else
			{
				for (int i = 0; i < n; i++)
				{
					array[i] = x[i];
				}
				for (int i = 0; i < overlap; i++)
				{
					array[i] = Inlines.MULT16_16_Q15(x[i], window[i]);
					array[n - i - 1] = Inlines.MULT16_16_Q15(x[n - i - 1], window[i]);
				}
				array2 = array;
			}
			int num2 = 1 + (n << 7);
			if ((n & 1) != 0)
			{
				num2 += Inlines.SHR32(Inlines.MULT16_16(array2[0], array2[0]), 9);
			}
			for (int i = n & 1; i < n; i += 2)
			{
				num2 += Inlines.SHR32(Inlines.MULT16_16(array2[i], array2[i]), 9);
				num2 += Inlines.SHR32(Inlines.MULT16_16(array2[i + 1], array2[i + 1]), 9);
			}
			int num3 = Inlines.celt_ilog2(num2) - 30 + 10;
			num3 /= 2;
			if (num3 > 0)
			{
				for (int i = 0; i < n; i++)
				{
					array[i] = Inlines.PSHR32(array2[i], num3);
				}
				array2 = array;
			}
			else
			{
				num3 = 0;
			}
			CeltPitchXCorr.pitch_xcorr(array2, 0, array2, 0, ac, num, lag + 1);
			for (int j = 0; j <= lag; j++)
			{
				int i = j + num;
				int num4 = 0;
				while (i < n)
				{
					num4 = Inlines.MAC16_16(num4, array2[i], array2[i - j]);
					i++;
				}
				ac[j] += num4;
			}
			num3 = 2 * num3;
			if (num3 <= 0)
			{
				ac[0] += Inlines.SHL32(1, -num3);
			}
			if (ac[0] < 268435456)
			{
				int num5 = 29 - Inlines.EC_ILOG((uint)ac[0]);
				for (int i = 0; i <= lag; i++)
				{
					ac[i] = Inlines.SHL32(ac[i], num5);
				}
				num3 -= num5;
			}
			else if (ac[0] >= 536870912)
			{
				int num6 = 1;
				if (ac[0] >= 1073741824)
				{
					num6++;
				}
				for (int i = 0; i <= lag; i++)
				{
					ac[i] = Inlines.SHR32(ac[i], num6);
				}
				num3 += num6;
			}
			return num3;
		}

		// Token: 0x0600031D RID: 797 RVA: 0x00023D28 File Offset: 0x00021F28
		internal unsafe static void silk_warped_autocorrelation(int[] corr, out int scale, short[] input, int warping_Q16, int length, int order)
		{
			Span<int> span = new int[order + 1];
			Span<long> span2 = new long[order + 1];
			for (int i = 0; i < length; i++)
			{
				int num = Inlines.silk_LSHIFT32((int)input[i], 14);
				for (int j = 0; j < order; j += 2)
				{
					int num2 = Inlines.silk_SMLAWB(*span[j], *span[j + 1] - num, warping_Q16);
					*span[j] = num;
					*span2[j] += Inlines.silk_RSHIFT64(Inlines.silk_SMULL(num, *span[0]), 18);
					num = Inlines.silk_SMLAWB(*span[j + 1], *span[j + 2] - num2, warping_Q16);
					*span[j + 1] = num2;
					*span2[j + 1] += Inlines.silk_RSHIFT64(Inlines.silk_SMULL(num2, *span[0]), 18);
				}
				*span[order] = num;
				*span2[order] += Inlines.silk_RSHIFT64(Inlines.silk_SMULL(num, *span[0]), 18);
			}
			int num3 = Inlines.silk_CLZ64(*span2[0]) - 35;
			num3 = Inlines.silk_LIMIT(num3, -22, 20);
			scale = -(10 + num3);
			if (num3 >= 0)
			{
				for (int j = 0; j < order + 1; j++)
				{
					corr[j] = (int)Inlines.silk_LSHIFT64(*span2[j], num3);
				}
				return;
			}
			for (int j = 0; j < order + 1; j++)
			{
				corr[j] = (int)Inlines.silk_RSHIFT64(*span2[j], -num3);
			}
		}

		// Token: 0x040003F6 RID: 1014
		private const int QC = 10;

		// Token: 0x040003F7 RID: 1015
		private const int QS = 14;
	}
}
