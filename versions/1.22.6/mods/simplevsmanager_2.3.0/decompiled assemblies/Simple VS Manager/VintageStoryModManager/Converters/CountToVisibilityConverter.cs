using System;
using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace VintageStoryModManager.Converters
{
	// Token: 0x020000D1 RID: 209
	public class CountToVisibilityConverter : IValueConverter
	{
		// Token: 0x06001058 RID: 4184 RVA: 0x000570E4 File Offset: 0x000552E4
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
			int num2;
			if (value is int)
			{
				int num = (int)value;
				num2 = num;
			}
			else
			{
				ICollection collection = value as ICollection;
				if (collection == null)
				{
					num2 = 0;
				}
				else
				{
					num2 = collection.Count;
				}
			}
			bool flag2 = num2 > 0;
			if (flag)
			{
				flag2 = !flag2;
			}
			return flag2 ? 0 : 2;
		}

		// Token: 0x06001059 RID: 4185 RVA: 0x0005716C File Offset: 0x0005536C
		[NullableContext(1)]
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
