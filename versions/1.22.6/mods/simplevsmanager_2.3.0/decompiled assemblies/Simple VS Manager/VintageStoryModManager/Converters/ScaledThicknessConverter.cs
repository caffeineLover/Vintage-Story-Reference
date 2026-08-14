using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;

namespace VintageStoryModManager.Converters
{
	// Token: 0x020000DF RID: 223
	[NullableContext(1)]
	[Nullable(0)]
	public class ScaledThicknessConverter : IValueConverter
	{
		// Token: 0x06001086 RID: 4230 RVA: 0x00057B88 File Offset: 0x00055D88
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is double)
			{
				double num = (double)value;
				Thickness thickness = ScaledThicknessConverter.ResolveThickness(parameter, culture);
				return new Thickness(thickness.Left * num, thickness.Top * num, thickness.Right * num, thickness.Bottom * num);
			}
			return Binding.DoNothing;
		}

		// Token: 0x06001087 RID: 4231 RVA: 0x00057BE3 File Offset: 0x00055DE3
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06001088 RID: 4232 RVA: 0x00057BEC File Offset: 0x00055DEC
		private static Thickness ResolveThickness(object parameter, CultureInfo culture)
		{
			if (parameter is Thickness)
			{
				return (Thickness)parameter;
			}
			string text = parameter as string;
			if (text != null)
			{
				object obj = new ThicknessConverter().ConvertFromString(null, culture, text);
				if (obj is Thickness)
				{
					return (Thickness)obj;
				}
			}
			return default(Thickness);
		}
	}
}
