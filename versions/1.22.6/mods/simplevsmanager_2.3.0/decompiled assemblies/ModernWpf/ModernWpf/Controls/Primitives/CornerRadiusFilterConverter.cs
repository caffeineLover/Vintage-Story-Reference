using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x02000287 RID: 647
	public class CornerRadiusFilterConverter : DependencyObject, IValueConverter
	{
		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x060012FB RID: 4859 RVA: 0x0003795D File Offset: 0x00035B5D
		// (set) Token: 0x060012FC RID: 4860 RVA: 0x00037965 File Offset: 0x00035B65
		public CornerRadiusFilterKind Filter { get; set; }

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x060012FD RID: 4861 RVA: 0x0003796E File Offset: 0x00035B6E
		// (set) Token: 0x060012FE RID: 4862 RVA: 0x00037976 File Offset: 0x00035B76
		public double Scale { get; set; } = 1.0;

		// Token: 0x060012FF RID: 4863 RVA: 0x00037980 File Offset: 0x00035B80
		public CornerRadius Convert(CornerRadius radius, CornerRadiusFilterKind filterKind)
		{
			CornerRadius result = radius;
			switch (filterKind)
			{
			case CornerRadiusFilterKind.Top:
				result.BottomLeft = 0.0;
				result.BottomRight = 0.0;
				break;
			case CornerRadiusFilterKind.Right:
				result.TopLeft = 0.0;
				result.BottomLeft = 0.0;
				break;
			case CornerRadiusFilterKind.Bottom:
				result.TopLeft = 0.0;
				result.TopRight = 0.0;
				break;
			case CornerRadiusFilterKind.Left:
				result.TopRight = 0.0;
				result.BottomRight = 0.0;
				break;
			}
			return result;
		}

		// Token: 0x06001300 RID: 4864 RVA: 0x00037A34 File Offset: 0x00035C34
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			CornerRadius radius = (CornerRadius)value;
			double scale = this.Scale;
			if (!double.IsNaN(scale))
			{
				radius.TopLeft *= scale;
				radius.TopRight *= scale;
				radius.BottomRight *= scale;
				radius.BottomLeft *= scale;
			}
			CornerRadiusFilterKind filter = this.Filter;
			if (filter == CornerRadiusFilterKind.TopLeftValue || filter == CornerRadiusFilterKind.BottomRightValue)
			{
				return this.GetDoubleValue(radius, filter);
			}
			return this.Convert(radius, filter);
		}

		// Token: 0x06001301 RID: 4865 RVA: 0x00037ABD File Offset: 0x00035CBD
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001302 RID: 4866 RVA: 0x00037AC4 File Offset: 0x00035CC4
		private double GetDoubleValue(CornerRadius radius, CornerRadiusFilterKind filterKind)
		{
			if (filterKind == CornerRadiusFilterKind.TopLeftValue)
			{
				return radius.TopLeft;
			}
			if (filterKind != CornerRadiusFilterKind.BottomRightValue)
			{
				return 0.0;
			}
			return radius.BottomRight;
		}
	}
}
