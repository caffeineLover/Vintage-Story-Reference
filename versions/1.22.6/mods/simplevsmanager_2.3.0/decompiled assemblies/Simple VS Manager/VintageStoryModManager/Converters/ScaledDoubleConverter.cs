using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace VintageStoryModManager.Converters
{
	// Token: 0x020000DE RID: 222
	[NullableContext(1)]
	[Nullable(0)]
	public class ScaledDoubleConverter : IValueConverter
	{
		// Token: 0x06001082 RID: 4226 RVA: 0x00057B00 File Offset: 0x00055D00
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is double)
			{
				double num = (double)value;
				return ScaledDoubleConverter.ResolveDouble(parameter, culture) * num;
			}
			return Binding.DoNothing;
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x00057B33 File Offset: 0x00055D33
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06001084 RID: 4228 RVA: 0x00057B3C File Offset: 0x00055D3C
		private static double ResolveDouble(object parameter, CultureInfo culture)
		{
			if (parameter is double)
			{
				return (double)parameter;
			}
			string text = parameter as string;
			double result;
			if (text != null && double.TryParse(text, 167, culture, ref result))
			{
				return result;
			}
			return 0.0;
		}
	}
}
