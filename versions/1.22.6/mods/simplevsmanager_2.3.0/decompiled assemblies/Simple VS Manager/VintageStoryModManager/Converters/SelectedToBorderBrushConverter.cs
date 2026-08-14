using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Media;

namespace VintageStoryModManager.Converters
{
	// Token: 0x020000CF RID: 207
	public class SelectedToBorderBrushConverter : IValueConverter
	{
		// Token: 0x06001052 RID: 4178 RVA: 0x00057024 File Offset: 0x00055224
		[NullableContext(1)]
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag;
			bool flag2;
			if (value is bool)
			{
				flag = (bool)value;
				flag2 = true;
			}
			else
			{
				flag2 = false;
			}
			if (!flag2 || !flag)
			{
				return new SolidColorBrush(Color.FromArgb(13, 161, 161, 170));
			}
			return new SolidColorBrush(Color.FromRgb(59, 130, 246));
		}

		// Token: 0x06001053 RID: 4179 RVA: 0x0005707A File Offset: 0x0005527A
		[NullableContext(1)]
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
