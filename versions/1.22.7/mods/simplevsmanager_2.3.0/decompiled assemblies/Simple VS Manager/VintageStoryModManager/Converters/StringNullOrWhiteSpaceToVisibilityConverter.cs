using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace VintageStoryModManager.Converters
{
	// Token: 0x020000DD RID: 221
	public class StringNullOrWhiteSpaceToVisibilityConverter : IValueConverter
	{
		// Token: 0x0600107F RID: 4223 RVA: 0x00057A88 File Offset: 0x00055C88
		[NullableContext(1)]
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string text = value as string;
			bool flag = text == null || string.IsNullOrWhiteSpace(text);
			bool flag2;
			if (parameter == null)
			{
				flag2 = false;
			}
			else
			{
				string text2 = parameter.ToString();
				flag2 = ((text2 != null) ? new bool?(text2.Equals("invert", 5)) : default(bool?)).GetValueOrDefault();
			}
			return (flag2 ? (!flag) : flag) ? 0 : 2;
		}

		// Token: 0x06001080 RID: 4224 RVA: 0x00057AF0 File Offset: 0x00055CF0
		[NullableContext(1)]
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
