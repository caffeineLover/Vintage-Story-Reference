using System;
using System.ComponentModel;
using System.Globalization;

namespace ModernWpf.Controls
{
	// Token: 0x02000272 RID: 626
	public class IconElementConverter : TypeConverter
	{
		// Token: 0x060011F8 RID: 4600 RVA: 0x000348FF File Offset: 0x00032AFF
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string);
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x00034911 File Offset: 0x00032B11
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return false;
		}

		// Token: 0x060011FA RID: 4602 RVA: 0x00034914 File Offset: 0x00032B14
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string text = value as string;
			Symbol symbol;
			if (text != null && Enum.TryParse<Symbol>(text, true, ref symbol))
			{
				return new SymbolIcon(symbol);
			}
			throw base.GetConvertFromException(value);
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x00034944 File Offset: 0x00032B44
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			throw base.GetConvertToException(value, destinationType);
		}
	}
}
