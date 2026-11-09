using System;
using System.IO;
using System.Security.Cryptography;

namespace UglyToad.PdfPig.Encryption
{
	// Token: 0x020001CC RID: 460
	internal static class AesEncryptionHelper
	{
		// Token: 0x06000AE7 RID: 2791 RVA: 0x00037205 File Offset: 0x00035405
		public static byte[] Encrypt256()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x0003720C File Offset: 0x0003540C
		public static byte[] Decrypt(byte[] data, byte[] finalKey)
		{
			if (data.Length == 0)
			{
				return data;
			}
			byte[] array = new byte[16];
			Array.Copy(data, array, array.Length);
			byte[] result;
			using (Aes aes = Aes.Create())
			{
				aes.Key = finalKey;
				aes.IV = array;
				byte[] array2 = new byte[256];
				using (ICryptoTransform cryptoTransform = aes.CreateDecryptor(aes.Key, aes.IV))
				{
					using (MemoryStream memoryStream = new MemoryStream(data))
					{
						using (MemoryStream memoryStream2 = new MemoryStream())
						{
							memoryStream.Seek((long)array.Length, 0);
							using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, 0))
							{
								int num;
								do
								{
									num = cryptoStream.Read(array2, 0, array2.Length);
									if (num > 0)
									{
										memoryStream2.Write(array2, 0, num);
									}
								}
								while (num > 0);
								result = memoryStream2.ToArray();
							}
						}
					}
				}
			}
			return result;
		}
	}
}
