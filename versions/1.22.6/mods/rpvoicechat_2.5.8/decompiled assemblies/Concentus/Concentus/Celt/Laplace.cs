using System;
using Concentus.Common;

namespace Concentus.Celt
{
	// Token: 0x0200008B RID: 139
	internal static class Laplace
	{
		// Token: 0x060004E0 RID: 1248 RVA: 0x0002F621 File Offset: 0x0002D821
		internal static uint ec_laplace_get_freq1(uint fs0, int decay)
		{
			return (uint)((ulong)(32736U - fs0) * (ulong)((long)(16384 - decay)) >> 15);
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x0002F638 File Offset: 0x0002D838
		internal static void ec_laplace_encode(EntropyCoder enc, Span<byte> encodedData, ref int value, uint fs, int decay)
		{
			int num = value;
			uint num2 = 0U;
			if (num != 0)
			{
				int num3 = (false - (num < 0)) ? 1 : 0;
				num = (num + num3 ^ num3);
				num2 = fs;
				fs = Laplace.ec_laplace_get_freq1(fs, decay);
				int num4 = 1;
				while (fs > 0U && num4 < num)
				{
					fs *= 2U;
					num2 += fs + 2U;
					fs = (uint)((ulong)fs * (ulong)((long)decay) >> 15);
					num4++;
				}
				if (fs == 0U)
				{
					int num5 = (int)(32768U - num2 + 1U - 1U);
					num5 = num5 - num3 >> 1;
					int num6 = Inlines.IMIN(num - num4, num5 - 1);
					num2 += (uint)(2 * num6 + 1 + num3);
					fs = Inlines.IMIN(1U, 32768U - num2);
					value = (num4 + num6 + num3 ^ num3);
				}
				else
				{
					fs += 1U;
					num2 += (uint)((ulong)fs & (ulong)((long)(~(long)num3)));
				}
			}
			enc.encode_bin(encodedData, num2, num2 + fs, 15U);
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x0002F6F8 File Offset: 0x0002D8F8
		internal static int ec_laplace_decode(EntropyCoder dec, ReadOnlySpan<byte> encodedData, uint fs, int decay)
		{
			int num = 0;
			uint num2 = dec.decode_bin(15U);
			uint num3 = 0U;
			if (num2 >= fs)
			{
				num++;
				num3 = fs;
				fs = Laplace.ec_laplace_get_freq1(fs, decay) + 1U;
				while (fs > 1U && num2 >= num3 + 2U * fs)
				{
					fs *= 2U;
					num3 += fs;
					fs = (uint)((ulong)(fs - 2U) * (ulong)((long)decay) >> 15);
					fs += 1U;
					num++;
				}
				if (fs <= 1U)
				{
					int num4 = (int)(num2 - num3) >> 1;
					num += num4;
					num3 += (uint)((long)(2 * num4));
				}
				if (num2 < num3 + fs)
				{
					num = -num;
				}
				else
				{
					num3 += fs;
				}
			}
			dec.dec_update(encodedData, num3, Inlines.IMIN(num3 + fs, 32768U), 32768U);
			return num;
		}

		// Token: 0x04000452 RID: 1106
		private const int LAPLACE_LOG_MINP = 0;

		// Token: 0x04000453 RID: 1107
		private const uint LAPLACE_MINP = 1U;

		// Token: 0x04000454 RID: 1108
		private const int LAPLACE_NMIN = 16;
	}
}
