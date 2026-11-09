using System;
using System.Collections.Generic;

namespace UglyToad.PdfPig.Util
{
	// Token: 0x02000025 RID: 37
	public static class Adler32Checksum
	{
		// Token: 0x06000112 RID: 274 RVA: 0x0000A5BC File Offset: 0x000087BC
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

		// Token: 0x040000A7 RID: 167
		private const int AdlerModulus = 65521;
	}
}
