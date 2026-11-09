using System;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x02000033 RID: 51
	internal static class LayoutUtils
	{
		// Token: 0x060001D7 RID: 471 RVA: 0x00006D40 File Offset: 0x00004F40
		public static double MeasureAndGetDesiredWidthFor(UIElement element, Size availableSize)
		{
			double result = 0.0;
			if (element != null)
			{
				element.Measure(availableSize);
				result = element.DesiredSize.Width;
			}
			return result;
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00006D71 File Offset: 0x00004F71
		public static double GetActualWidthFor(FrameworkElement element)
		{
			if (element == null)
			{
				return 0.0;
			}
			return element.ActualWidth;
		}
	}
}
