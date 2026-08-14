using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;

namespace VintageStoryModManager.Converters
{
	// Token: 0x020000D8 RID: 216
	public class InstallButtonVisibilityConverter : IMultiValueConverter
	{
		// Token: 0x0600106F RID: 4207 RVA: 0x00057674 File Offset: 0x00055874
		[NullableContext(1)]
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values.Length < 2 || values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue)
			{
				return 2;
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
			return (flag3 || flag6) ? 0 : 2;
		}

		// Token: 0x06001070 RID: 4208 RVA: 0x000576E2 File Offset: 0x000558E2
		[NullableContext(1)]
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
