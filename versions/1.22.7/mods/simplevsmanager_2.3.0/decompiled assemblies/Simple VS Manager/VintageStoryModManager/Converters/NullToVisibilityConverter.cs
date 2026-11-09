using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace VintageStoryModManager.Converters
{
	// Token: 0x020000CE RID: 206
	public class NullToVisibilityConverter : IValueConverter
	{
		// Token: 0x0600104F RID: 4175 RVA: 0x00056FBC File Offset: 0x000551BC
		[NullableContext(1)]
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag;
			if (parameter == null)
			{
				flag = false;
			}
			else
			{
				string text = parameter.ToString();
				flag = ((text != null) ? new bool?(text.Equals("invert", 5)) : default(bool?)).GetValueOrDefault();
			}
			bool flag2 = value != null;
			if (flag)
			{
				flag2 = !flag2;
			}
			return flag2 ? 0 : 2;
		}

		// Token: 0x06001050 RID: 4176 RVA: 0x00057014 File Offset: 0x00055214
		[NullableContext(1)]
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
