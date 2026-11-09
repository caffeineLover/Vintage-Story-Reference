using System;

namespace UglyToad.PdfPig.Encryption
{
	// Token: 0x020001D5 RID: 469
	internal static class RC4
	{
		// Token: 0x06000B1E RID: 2846 RVA: 0x00038DA0 File Offset: 0x00036FA0
		public static byte[] Encrypt(byte[] key, byte[] data)
		{
			byte[] array = new byte[256];
			for (int i = 0; i < 256; i++)
			{
				array[i] = (byte)i;
			}
			int num = 0;
			for (int j = 0; j < 256; j++)
			{
				num = (num + (int)array[j] + (int)key[j % key.Length]) % 256;
				byte b = array[j];
				array[j] = array[num];
				array[num] = b;
			}
			byte[] array2 = new byte[data.Length];
			num = 0;
			int num2 = 0;
			for (int k = 0; k < data.Length; k++)
			{
				num2 = (num2 + 1) % 256;
				num = (num + (int)array[num2]) % 256;
				byte b2 = array[num2];
				array[num2] = array[num];
				array[num] = b2;
				byte b3 = array[(int)(array[num2] + array[num]) % 256];
				array2[k] = (data[k] ^ b3);
			}
			return array2;
		}
	}
}
