using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x020002A4 RID: 676
	public class SharedSizeGroupConverter : IValueConverter
	{
		// Token: 0x06001440 RID: 5184 RVA: 0x0003C245 File Offset: 0x0003A445
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((Visibility)value == 2)
			{
				return null;
			}
			return (string)parameter;
		}

		// Token: 0x06001441 RID: 5185 RVA: 0x0003C258 File Offset: 0x0003A458
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
