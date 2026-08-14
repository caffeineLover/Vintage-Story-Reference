using System;
using System.Collections.Generic;

namespace UglyToad.PdfPig.Images.Png
{
	// Token: 0x020000ED RID: 237
	internal static class Adler32Checksum
	{
		// Token: 0x0600047D RID: 1149 RVA: 0x0001B454 File Offset: 0x00019654
		public static int Calculate(IEnumerable<byte> data)
		{
			int num = 1;
			int num2 = 0;
			foreach (byte b in data)
			{
				num = (num + (int)b) % 65521;
				num2 = (num + num2) % 65521;
			}
			return num2 * 65536 + num;
		}

		// Token: 0x0400025E RID: 606
		private const int AdlerModulus = 65521;
	}
}
