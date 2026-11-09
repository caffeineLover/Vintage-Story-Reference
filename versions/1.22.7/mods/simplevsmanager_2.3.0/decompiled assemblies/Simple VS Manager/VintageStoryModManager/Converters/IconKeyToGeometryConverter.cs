using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace VintageStoryModManager.Converters
{
	// Token: 0x020000D4 RID: 212
	public class IconKeyToGeometryConverter : IValueConverter
	{
		// Token: 0x06001061 RID: 4193 RVA: 0x000572A8 File Offset: 0x000554A8
		[NullableContext(1)]
		[return: Nullable(2)]
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string text = value as string;
			if (text != null && !string.IsNullOrEmpty(text))
			{
				Geometry geometry = Application.Current.TryFindResource(text) as Geometry;
				if (geometry != null)
				{
					return geometry;
				}
			}
			return null;
		}

		// Token: 0x06001062 RID: 4194 RVA: 0x000572DE File Offset: 0x000554DE
		[NullableContext(1)]
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
