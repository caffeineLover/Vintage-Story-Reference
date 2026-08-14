using System;
using System.Collections.Generic;

namespace UglyToad.PdfPig.Images.Png
{
	// Token: 0x020000F1 RID: 241
	internal static class Crc32
	{
		// Token: 0x06000486 RID: 1158 RVA: 0x0001B5EC File Offset: 0x000197EC
		static Crc32()
		{
			for (uint num = 0U; num < 256U; num += 1U)
			{
				uint num2 = num;
				for (int i = 0; i < 8; i++)
				{
					if ((num2 & 1U) != 0U)
					{
						num2 = (num2 >> 1 ^ 3988292384U);
					}
					else
					{
						num2 >>= 1;
					}
				}
				Crc32.Lookup[(int)num] = num2;
			}
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x0001B644 File Offset: 0x00019844
		public static uint Calculate(byte[] data)
		{
			uint num = uint.MaxValue;
			for (int i = 0; i < data.Length; i++)
			{
				uint num2 = (num ^ (uint)data[i]) & 255U;
				num = (num >> 8 ^ Crc32.Lookup[(int)num2]);
			}
			return num ^ uint.MaxValue;
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x0001B67C File Offset: 0x0001987C
		public static uint Calculate(List<byte> data)
		{
			uint num = uint.MaxValue;
			for (int i = 0; i < data.Count; i++)
			{
				uint num2 = (num ^ (uint)data[i]) & 255U;
				num = (num >> 8 ^ Crc32.Lookup[(int)num2]);
			}
			return num ^ uint.MaxValue;
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x0001B6BC File Offset: 0x000198BC
		public static uint Calculate(byte[] data, byte[] data2)
		{
			uint num = uint.MaxValue;
			for (int i = 0; i < data.Length; i++)
			{
				uint num2 = (num ^ (uint)data[i]) & 255U;
				num = (num >> 8 ^ Crc32.Lookup[(int)num2]);
			}
			for (int j = 0; j < data2.Length; j++)
			{
				uint num3 = (num ^ (uint)data2[j]) & 255U;
				num = (num >> 8 ^ Crc32.Lookup[(int)num3]);
			}
			return num ^ uint.MaxValue;
		}

		// Token: 0x04000269 RID: 617
		private const uint Polynomial = 3988292384U;

		// Token: 0x0400026A RID: 618
		private static readonly uint[] Lookup = new uint[256];
	}
}
