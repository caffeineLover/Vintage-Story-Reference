using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x02000291 RID: 657
	public class IsGridViewConverter : IValueConverter
	{
		// Token: 0x0600136F RID: 4975 RVA: 0x00039328 File Offset: 0x00037528
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value is GridView;
		}

		// Token: 0x06001370 RID: 4976 RVA: 0x00039338 File Offset: 0x00037538
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
