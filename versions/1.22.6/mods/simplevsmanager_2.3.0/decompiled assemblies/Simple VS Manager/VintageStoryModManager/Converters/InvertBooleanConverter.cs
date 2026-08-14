using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace VintageStoryModManager.Converters
{
	// Token: 0x020000CD RID: 205
	public class InvertBooleanConverter : IValueConverter
	{
		// Token: 0x0600104C RID: 4172 RVA: 0x00056F5C File Offset: 0x0005515C
		[NullableContext(1)]
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool)
			{
				bool flag = (bool)value;
				return !flag;
			}
			return false;
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x00056F88 File Offset: 0x00055188
		[NullableContext(1)]
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool)
			{
				bool flag = (bool)value;
				return !flag;
			}
			return false;
		}
	}
}
