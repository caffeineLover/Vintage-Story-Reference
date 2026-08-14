using System;
using UglyToad.PdfPig.Content;

namespace UglyToad.PdfPig.Util
{
	// Token: 0x02000037 RID: 55
	public static class WhitespaceSizeStatistics
	{
		// Token: 0x0600015D RID: 349 RVA: 0x0000DF67 File Offset: 0x0000C167
		public static double GetExpectedWhitespaceSize(Letter letter)
		{
			return letter.PointSize * 0.27;
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0000DF79 File Offset: 0x0000C179
		public static bool IsProbablyWhitespace(double gap, Letter letter)
		{
			return gap > WhitespaceSizeStatistics.GetExpectedWhitespaceSize(letter) - letter.PointSize * 0.05;
		}
	}
}
