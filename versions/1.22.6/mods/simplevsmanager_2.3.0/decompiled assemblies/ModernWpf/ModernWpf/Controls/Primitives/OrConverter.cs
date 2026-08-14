using System;
using System.Globalization;
using System.Windows.Data;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x02000296 RID: 662
	public class OrConverter : IMultiValueConverter
	{
		// Token: 0x0600139D RID: 5021 RVA: 0x00039C80 File Offset: 0x00037E80
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			foreach (object obj in values)
			{
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
				if (flag2 && flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600139E RID: 5022 RVA: 0x00039CC3 File Offset: 0x00037EC3
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
