using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;

namespace VintageStoryModManager.Converters
{
	// Token: 0x020000DA RID: 218
	public class InstallButtonShouldShowConverter : IMultiValueConverter
	{
		// Token: 0x06001075 RID: 4213 RVA: 0x000577E8 File Offset: 0x000559E8
		[NullableContext(1)]
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values.Length < 2 || values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue)
			{
				return false;
			}
			object obj = values[0];
			bool flag;
			bool flag2;
			if (obj is bool)
			{
				flag = (bool)obj;
				flag2 = true;
			}
			else
			{
				flag2 = false;
			}
			bool flag3 = flag2 && flag;
			obj = values[1];
			bool flag4;
			bool flag5;
			if (obj is bool)
			{
				flag4 = (bool)obj;
				flag5 = true;
			}
			else
			{
				flag5 = false;
			}
			bool flag6 = flag5 && flag4;
			return flag3 || flag6;
		}

		// Token: 0x06001076 RID: 4214 RVA: 0x00057850 File Offset: 0x00055A50
		[NullableContext(1)]
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
