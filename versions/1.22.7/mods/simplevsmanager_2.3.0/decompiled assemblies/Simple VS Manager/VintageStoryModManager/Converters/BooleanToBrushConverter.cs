using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Media;

namespace VintageStoryModManager.Converters
{
	// Token: 0x020000D3 RID: 211
	public class BooleanToBrushConverter : IValueConverter
	{
		// Token: 0x0600105E RID: 4190 RVA: 0x0005724C File Offset: 0x0005544C
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
				return new SolidColorBrush(Color.FromRgb(9, 9, 11));
			}
			return new SolidColorBrush(Color.FromRgb(251, 191, 36));
		}

		// Token: 0x0600105F RID: 4191 RVA: 0x00057297 File Offset: 0x00055497
		[NullableContext(1)]
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
