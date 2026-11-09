using System;

namespace ModernWpf.Controls
{
	// Token: 0x02000072 RID: 114
	internal class DefaultNumberBoxNumberFormatter : INumberBoxNumberFormatter
	{
		// Token: 0x0600059A RID: 1434 RVA: 0x0001452A File Offset: 0x0001272A
		public string FormatDouble(double value)
		{
			return value.ToString();
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x00014534 File Offset: 0x00012734
		public double? ParseDouble(string text)
		{
			double num;
			if (double.TryParse(text, ref num))
			{
				return new double?(num);
			}
			return default(double?);
		}
	}
}
