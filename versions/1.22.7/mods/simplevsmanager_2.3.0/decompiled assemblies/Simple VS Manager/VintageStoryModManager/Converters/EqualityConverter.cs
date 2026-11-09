using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace VintageStoryModManager.Converters
{
	// Token: 0x020000D0 RID: 208
	public class EqualityConverter : IValueConverter
	{
		// Token: 0x06001055 RID: 4181 RVA: 0x00057089 File Offset: 0x00055289
		[NullableContext(1)]
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((value != null) ? value.ToString() : null) == ((parameter != null) ? parameter.ToString() : null);
		}

		// Token: 0x06001056 RID: 4182 RVA: 0x000570B0 File Offset: 0x000552B0
		[NullableContext(1)]
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
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
			if (flag2 && flag)
			{
				return parameter;
			}
			return Binding.DoNothing;
		}
	}
}
