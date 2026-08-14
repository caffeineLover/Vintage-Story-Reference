using System;
using System.Windows;
using System.Windows.Media;

namespace ModernWpf
{
	// Token: 0x02000221 RID: 545
	internal static class ThemeResourceHelper
	{
		// Token: 0x06000FCC RID: 4044 RVA: 0x0002D1D4 File Offset: 0x0002B3D4
		internal static object GetColorKey(SolidColorBrush element)
		{
			return element.GetValue(ThemeResourceHelper.ColorKeyProperty);
		}

		// Token: 0x06000FCD RID: 4045 RVA: 0x0002D1E1 File Offset: 0x0002B3E1
		internal static void SetColorKey(SolidColorBrush element, object value)
		{
			element.SetValue(ThemeResourceHelper.ColorKeyProperty, value);
		}

		// Token: 0x04000411 RID: 1041
		private static readonly DependencyProperty ColorKeyProperty = DependencyProperty.RegisterAttached("ColorKey", typeof(object), typeof(ThemeResourceHelper));
	}
}
