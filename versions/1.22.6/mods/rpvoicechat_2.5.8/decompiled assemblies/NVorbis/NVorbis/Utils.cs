using System;

namespace NVorbis
{
	// Token: 0x02000016 RID: 22
	internal static class Utils
	{
		// Token: 0x060000CD RID: 205 RVA: 0x00005D28 File Offset: 0x00003F28
		internal static int ilog(int x)
		{
			int num = 0;
			while (x > 0)
			{
				num++;
				x >>= 1;
			}
			return num;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00005D47 File Offset: 0x00003F47
		internal static uint BitReverse(uint n)
		{
			return Utils.BitReverse(n, 32);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00005D54 File Offset: 0x00003F54
		internal static uint BitReverse(uint n, int bits)
		{
			n = ((n & 2863311530U) >> 1 | (n & 1431655765U) << 1);
			n = ((n & 3435973836U) >> 2 | (n & 858993459U) << 2);
			n = ((n & 4042322160U) >> 4 | (n & 252645135U) << 4);
			n = ((n & 4278255360U) >> 8 | (n & 16711935U) << 8);
			return (n >> 16 | n << 16) >> 32 - bits;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00005DC6 File Offset: 0x00003FC6
		internal static float ClipValue(float value, ref bool clipped)
		{
			if (value > 0.99999994f)
			{
				clipped = true;
				return 0.99999994f;
			}
			if (value < -0.99999994f)
			{
				clipped = true;
				return -0.99999994f;
			}
			return value;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00005DEC File Offset: 0x00003FEC
		internal static float ConvertFromVorbisFloat32(uint bits)
		{
			int num = (int)bits >> 31;
			double num2 = (double)(((bits & 2145386496U) >> 21) - 788U);
			return (float)(((ulong)(bits & 2097151U) ^ (ulong)((long)num)) + (ulong)((long)(num & 1))) * (float)Math.Pow(2.0, num2);
		}
	}
}
