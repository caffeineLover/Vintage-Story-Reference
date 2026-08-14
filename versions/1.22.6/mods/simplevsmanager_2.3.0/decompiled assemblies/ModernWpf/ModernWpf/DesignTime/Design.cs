using System;
using System.ComponentModel;
using System.Windows;

namespace ModernWpf.DesignTime
{
	// Token: 0x02000260 RID: 608
	public static class Design
	{
		// Token: 0x06001158 RID: 4440 RVA: 0x00033172 File Offset: 0x00031372
		public static ElementTheme GetRequestedTheme(FrameworkElement element)
		{
			return (ElementTheme)element.GetValue(Design.RequestedThemeProperty);
		}

		// Token: 0x06001159 RID: 4441 RVA: 0x00033184 File Offset: 0x00031384
		public static void SetRequestedTheme(FrameworkElement element, ElementTheme value)
		{
			element.SetValue(Design.RequestedThemeProperty, value);
		}

		// Token: 0x0600115A RID: 4442 RVA: 0x00033197 File Offset: 0x00031397
		private static void OnRequestedThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (DesignerProperties.GetIsInDesignMode(d))
			{
				ThemeManager.SetRequestedTheme((FrameworkElement)d, (ElementTheme)e.NewValue);
			}
		}

		// Token: 0x040004C5 RID: 1221
		public static readonly DependencyProperty RequestedThemeProperty = DependencyProperty.RegisterAttached("RequestedTheme", typeof(ElementTheme), typeof(Design), new PropertyMetadata(ElementTheme.Default, new PropertyChangedCallback(Design.OnRequestedThemeChanged)));
	}
}
