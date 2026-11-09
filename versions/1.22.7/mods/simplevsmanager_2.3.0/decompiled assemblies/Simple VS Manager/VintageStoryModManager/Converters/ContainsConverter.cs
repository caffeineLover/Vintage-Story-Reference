using System;
using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;

namespace VintageStoryModManager.Converters
{
	// Token: 0x020000D2 RID: 210
	public class ContainsConverter : IMultiValueConverter
	{
		// Token: 0x0600105B RID: 4187 RVA: 0x0005717C File Offset: 0x0005537C
		[NullableContext(1)]
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values.Length < 2 || values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue)
			{
				return false;
			}
			object obj = values[0];
			object obj2 = values[1];
			if (obj2 == null)
			{
				return false;
			}
			IList list = obj2 as IList;
			if (list != null)
			{
				return list.Contains(obj);
			}
			IEnumerable enumerable = obj2 as IEnumerable;
			if (enumerable != null)
			{
				using (IEnumerator enumerator = enumerable.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (object.Equals(enumerator.Current, obj))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600105C RID: 4188 RVA: 0x0005723C File Offset: 0x0005543C
		[NullableContext(1)]
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
