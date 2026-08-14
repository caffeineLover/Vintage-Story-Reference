using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000F3 RID: 243
	[NullableContext(1)]
	[Nullable(0)]
	public class YamlFormatter
	{
		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000590 RID: 1424 RVA: 0x00014469 File Offset: 0x00012669
		public static YamlFormatter Default { get; } = new YamlFormatter();

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000591 RID: 1425 RVA: 0x00014470 File Offset: 0x00012670
		// (set) Token: 0x06000592 RID: 1426 RVA: 0x00014478 File Offset: 0x00012678
		public NumberFormatInfo NumberFormat { get; set; } = new NumberFormatInfo
		{
			CurrencyDecimalSeparator = ".",
			CurrencyGroupSeparator = "_",
			CurrencyGroupSizes = new int[]
			{
				3
			},
			CurrencySymbol = string.Empty,
			CurrencyDecimalDigits = 99,
			NumberDecimalSeparator = ".",
			NumberGroupSeparator = "_",
			NumberGroupSizes = new int[]
			{
				3
			},
			NumberDecimalDigits = 99,
			NaNSymbol = ".nan",
			PositiveInfinitySymbol = ".inf",
			NegativeInfinitySymbol = "-.inf"
		};

		// Token: 0x06000593 RID: 1427 RVA: 0x00014481 File Offset: 0x00012681
		public string FormatNumber(object number)
		{
			return Convert.ToString(number, this.NumberFormat);
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x0001448F File Offset: 0x0001268F
		public string FormatNumber(double number)
		{
			return number.ToString("G", this.NumberFormat);
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x000144A3 File Offset: 0x000126A3
		public string FormatNumber(float number)
		{
			return number.ToString("G", this.NumberFormat);
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x000144B7 File Offset: 0x000126B7
		public string FormatBoolean(object boolean)
		{
			if (!boolean.Equals(true))
			{
				return "false";
			}
			return "true";
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x000144D4 File Offset: 0x000126D4
		public string FormatDateTime(object dateTime)
		{
			return ((DateTime)dateTime).ToString("o", CultureInfo.InvariantCulture);
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x000144FC File Offset: 0x000126FC
		public string FormatTimeSpan(object timeSpan)
		{
			return ((TimeSpan)timeSpan).ToString();
		}
	}
}
