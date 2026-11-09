using System;
using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;

namespace VintageStoryModManager.Converters
{
	// Token: 0x020000D7 RID: 215
	public class FavoriteButtonVisibilityConverter : IMultiValueConverter
	{
		// Token: 0x0600106C RID: 4204 RVA: 0x0005757C File Offset: 0x0005577C
		[NullableContext(1)]
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values.Length < 3 || values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue || values[2] == DependencyProperty.UnsetValue)
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
			object obj2 = values[1];
			object obj3 = values[2];
			bool flag4 = false;
			IList list = obj3 as IList;
			if (list != null && obj2 != null)
			{
				flag4 = list.Contains(obj2);
			}
			else
			{
				IEnumerable enumerable = obj3 as IEnumerable;
				if (enumerable != null && obj2 != null)
				{
					using (IEnumerator enumerator = enumerable.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (object.Equals(enumerator.Current, obj2))
							{
								flag4 = true;
								break;
							}
						}
					}
				}
			}
			return (flag3 || flag4) ? 0 : 2;
		}

		// Token: 0x0600106D RID: 4205 RVA: 0x00057664 File Offset: 0x00055864
		[NullableContext(1)]
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
