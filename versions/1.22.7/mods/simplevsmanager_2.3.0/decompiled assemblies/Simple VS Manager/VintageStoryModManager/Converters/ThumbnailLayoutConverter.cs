using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;

namespace VintageStoryModManager.Converters
{
	// Token: 0x020000E0 RID: 224
	[NullableContext(1)]
	[Nullable(0)]
	public class ThumbnailLayoutConverter : IMultiValueConverter
	{
		// Token: 0x0600108A RID: 4234 RVA: 0x00057C48 File Offset: 0x00055E48
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			string text = parameter as string;
			if (text == null)
			{
				return DependencyProperty.UnsetValue;
			}
			double aspectRatio = ThumbnailLayoutConverter.GetAspectRatio(values);
			if (double.IsNaN(aspectRatio))
			{
				return Binding.DoNothing;
			}
			ThumbnailLayoutConverter.ThumbnailLayout thumbnailLayout = ThumbnailLayoutConverter.Classify(aspectRatio);
			object result;
			if (!(text == "RowSpan"))
			{
				if (!(text == "Stretch"))
				{
					if (!(text == "VerticalAlignment"))
					{
						result = DependencyProperty.UnsetValue;
					}
					else
					{
						result = ((thumbnailLayout == ThumbnailLayoutConverter.ThumbnailLayout.Wide) ? 1 : 0);
					}
				}
				else
				{
					result = ((thumbnailLayout == ThumbnailLayoutConverter.ThumbnailLayout.Square) ? 3 : 2);
				}
			}
			else
			{
				result = ((thumbnailLayout == ThumbnailLayoutConverter.ThumbnailLayout.Square) ? 2 : 1);
			}
			return result;
		}

		// Token: 0x0600108B RID: 4235 RVA: 0x00057CDF File Offset: 0x00055EDF
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600108C RID: 4236 RVA: 0x00057CE8 File Offset: 0x00055EE8
		private static double GetAspectRatio(IReadOnlyList<object> values)
		{
			if (values.Count < 2)
			{
				return double.NaN;
			}
			double @double = ThumbnailLayoutConverter.GetDouble(values[0]);
			double double2 = ThumbnailLayoutConverter.GetDouble(values[1]);
			if (@double <= 0.0 || double2 <= 0.0)
			{
				return double.NaN;
			}
			return @double / double2;
		}

		// Token: 0x0600108D RID: 4237 RVA: 0x00057D48 File Offset: 0x00055F48
		private static double GetDouble(object value)
		{
			double result;
			if (value is double)
			{
				double num = (double)value;
				result = num;
			}
			else if (value is float)
			{
				float num2 = (float)value;
				result = (double)num2;
			}
			else if (value is int)
			{
				int num3 = (int)value;
				result = (double)num3;
			}
			else if (value is long)
			{
				long num4 = (long)value;
				result = (double)num4;
			}
			else
			{
				result = double.NaN;
			}
			return result;
		}

		// Token: 0x0600108E RID: 4238 RVA: 0x00057DBD File Offset: 0x00055FBD
		private static ThumbnailLayoutConverter.ThumbnailLayout Classify(double aspectRatio)
		{
			if (double.IsNaN(aspectRatio))
			{
				return ThumbnailLayoutConverter.ThumbnailLayout.Standard;
			}
			if (aspectRatio <= 1.1)
			{
				return ThumbnailLayoutConverter.ThumbnailLayout.Square;
			}
			if (aspectRatio > 1.7)
			{
				return ThumbnailLayoutConverter.ThumbnailLayout.Wide;
			}
			return ThumbnailLayoutConverter.ThumbnailLayout.Standard;
		}

		// Token: 0x02000320 RID: 800
		[NullableContext(0)]
		private enum ThumbnailLayout
		{
			// Token: 0x040013E4 RID: 5092
			Square,
			// Token: 0x040013E5 RID: 5093
			Standard,
			// Token: 0x040013E6 RID: 5094
			Wide
		}
	}
}
