using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000043 RID: 67
	[NullableContext(1)]
	[Nullable(0)]
	public class YamlFormatter
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x00006941 File Offset: 0x00004B41
		public static YamlFormatter Default { get; } = new YamlFormatter();

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060001EA RID: 490 RVA: 0x00006948 File Offset: 0x00004B48
		// (set) Token: 0x060001EB RID: 491 RVA: 0x00006950 File Offset: 0x00004B50
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

		// Token: 0x060001EC RID: 492 RVA: 0x00006959 File Offset: 0x00004B59
		public string FormatNumber(object number)
		{
			return Convert.ToString(number, this.NumberFormat);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00006967 File Offset: 0x00004B67
		public string FormatNumber(double number)
		{
			return number.ToString("G", this.NumberFormat);
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000697B File Offset: 0x00004B7B
		public string FormatNumber(float number)
		{
			return number.ToString("G", this.NumberFormat);
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000698F File Offset: 0x00004B8F
		public string FormatBoolean(object boolean)
		{
			if (!boolean.Equals(true))
			{
				return "false";
			}
			return "true";
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x000069AC File Offset: 0x00004BAC
		public string FormatDateTime(object dateTime)
		{
			return ((DateTime)dateTime).ToString("o", CultureInfo.InvariantCulture);
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x000069D4 File Offset: 0x00004BD4
		public string FormatTimeSpan(object timeSpan)
		{
			return ((TimeSpan)timeSpan).ToString();
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x000069F5 File Offset: 0x00004BF5
		// (set) Token: 0x060001F3 RID: 499 RVA: 0x000069FD File Offset: 0x00004BFD
		public virtual Func<object, ITypeInspector, INamingConvention, string> FormatEnum { get; set; } = delegate(object value, ITypeInspector typeInspector, INamingConvention enumNamingConvention)
		{
			string value2 = string.Empty;
			if (value == null)
			{
				value2 = string.Empty;
			}
			else
			{
				value2 = typeInspector.GetEnumValue(value);
			}
			return enumNamingConvention.Apply(value2);
		};

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x00006A06 File Offset: 0x00004C06
		// (set) Token: 0x060001F5 RID: 501 RVA: 0x00006A0E File Offset: 0x00004C0E
		public virtual Func<object, bool> PotentiallyQuoteEnums { get; set; } = (object _) => true;
	}
}
