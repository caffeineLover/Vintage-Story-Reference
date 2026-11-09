using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x0200029A RID: 666
	public class PlacementRectangleConverter : IMultiValueConverter
	{
		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x060013DA RID: 5082 RVA: 0x0003A7CB File Offset: 0x000389CB
		// (set) Token: 0x060013DB RID: 5083 RVA: 0x0003A7D3 File Offset: 0x000389D3
		public Thickness Margin { get; set; }

		// Token: 0x060013DC RID: 5084 RVA: 0x0003A7DC File Offset: 0x000389DC
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values.Length == 2)
			{
				object obj = values[0];
				if (obj is double)
				{
					double num = (double)obj;
					obj = values[1];
					if (obj is double)
					{
						double num2 = (double)obj;
						Thickness margin = this.Margin;
						Point point;
						point..ctor(margin.Left, margin.Top);
						Point point2;
						point2..ctor(num - margin.Right, num2 - margin.Bottom);
						return new Rect(point, point2);
					}
				}
			}
			return Rect.Empty;
		}

		// Token: 0x060013DD RID: 5085 RVA: 0x0003A863 File Offset: 0x00038A63
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
