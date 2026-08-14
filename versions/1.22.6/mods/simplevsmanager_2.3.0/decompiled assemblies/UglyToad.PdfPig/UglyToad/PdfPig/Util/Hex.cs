using System;
using System.Text;

namespace UglyToad.PdfPig.Util
{
	// Token: 0x0200002F RID: 47
	internal static class Hex
	{
		// Token: 0x06000133 RID: 307 RVA: 0x0000BE8C File Offset: 0x0000A08C
		public static string GetString(byte[] bytes)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			StringBuilder stringBuilder = new StringBuilder(bytes.Length * 2);
			foreach (byte b in bytes)
			{
				stringBuilder.Append(Hex.HexChars[Hex.GetHighNibble(b)]).Append(Hex.HexChars[Hex.GetLowNibble(b)]);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000134 RID: 308 RVA: 0x0000BEF0 File Offset: 0x0000A0F0
		private static int GetHighNibble(byte b)
		{
			return (b & 240) >> 4;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x0000BEFB File Offset: 0x0000A0FB
		private static int GetLowNibble(byte b)
		{
			return (int)(b & 15);
		}

		// Token: 0x040000AA RID: 170
		private static readonly char[] HexChars = new char[]
		{
			'0',
			'1',
			'2',
			'3',
			'4',
			'5',
			'6',
			'7',
			'8',
			'9',
			'A',
			'B',
			'C',
			'D',
			'E',
			'F'
		};
	}
}
