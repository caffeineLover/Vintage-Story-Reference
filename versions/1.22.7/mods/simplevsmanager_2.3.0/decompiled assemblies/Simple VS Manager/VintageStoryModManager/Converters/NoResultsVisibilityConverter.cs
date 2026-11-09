using System;
using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;

namespace VintageStoryModManager.Converters
{
	// Token: 0x020000D6 RID: 214
	public class NoResultsVisibilityConverter : IMultiValueConverter
	{
		// Token: 0x06001069 RID: 4201 RVA: 0x000574DC File Offset: 0x000556DC
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
			int num2;
			if (obj is int)
			{
				int num = (int)obj;
				num2 = num;
			}
			else
			{
				ICollection collection = obj as ICollection;
				if (collection == null)
				{
					num2 = 0;
				}
				else
				{
					num2 = collection.Count;
				}
			}
			int num3 = num2;
			return (!flag3 && num3 == 0) ? 0 : 2;
		}

		// Token: 0x0600106A RID: 4202 RVA: 0x0005756A File Offset: 0x0005576A
		[NullableContext(1)]
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
