using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Data;

namespace VintageStoryModManager.Converters
{
	// Token: 0x020000CC RID: 204
	public class CompactViewWidthConverter : IValueConverter
	{
		// Token: 0x06001049 RID: 4169 RVA: 0x00056EA8 File Offset: 0x000550A8
		[NullableContext(1)]
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
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
			bool flag3 = flag2 && flag;
			double num = flag3 ? 40.0 : 80.0;
			string text = parameter as string;
			if (text != null)
			{
				string[] array = text.Split(',', 0);
				double num2;
				double num3;
				if (array.Length == 2 && double.TryParse(array[0], ref num2) && double.TryParse(array[1], ref num3))
				{
					num = (flag3 ? num3 : num2);
				}
			}
			if (targetType == typeof(DataGridLength))
			{
				return new DataGridLength(num);
			}
			return num;
		}

		// Token: 0x0600104A RID: 4170 RVA: 0x00056F47 File Offset: 0x00055147
		[NullableContext(1)]
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException("CompactViewWidthConverter does not support ConvertBack.");
		}
	}
}
