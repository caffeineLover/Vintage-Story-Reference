using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000EF RID: 239
	[NullableContext(1)]
	[Nullable(0)]
	public class YamlFormatter
	{
		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000554 RID: 1364 RVA: 0x00013B21 File Offset: 0x00011D21
		public static YamlFormatter Default { get; } = new YamlFormatter();

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000555 RID: 1365 RVA: 0x00013B28 File Offset: 0x00011D28
		// (set) Token: 0x06000556 RID: 1366 RVA: 0x00013B30 File Offset: 0x00011D30
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

		// Token: 0x06000557 RID: 1367 RVA: 0x00013B39 File Offset: 0x00011D39
		public string FormatNumber(object number)
		{
			return Convert.ToString(number, this.NumberFormat);
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x00013B47 File Offset: 0x00011D47
		public string FormatNumber(double number)
		{
			return number.ToString("G", this.NumberFormat);
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x00013B5B File Offset: 0x00011D5B
		public string FormatNumber(float number)
		{
			return number.ToString("G", this.NumberFormat);
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x00013B6F File Offset: 0x00011D6F
		public string FormatBoolean(object boolean)
		{
			if (!boolean.Equals(true))
			{
				return "false";
			}
			return "true";
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x00013B8C File Offset: 0x00011D8C
		public string FormatDateTime(object dateTime)
		{
			return ((DateTime)dateTime).ToString("o", CultureInfo.InvariantCulture);
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x00013BB4 File Offset: 0x00011DB4
		public string FormatTimeSpan(object timeSpan)
		{
			return ((TimeSpan)timeSpan).ToString();
		}
	}
}
