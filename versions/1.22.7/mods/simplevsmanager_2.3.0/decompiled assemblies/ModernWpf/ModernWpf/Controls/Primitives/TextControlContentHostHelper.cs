using System;
using System.Windows;
using System.Windows.Controls;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x020002A9 RID: 681
	public static class TextControlContentHostHelper
	{
		// Token: 0x0600146A RID: 5226 RVA: 0x0003CA4C File Offset: 0x0003AC4C
		public static Thickness GetContentPresenterMargin(ScrollViewer contentHost)
		{
			return (Thickness)contentHost.GetValue(TextControlContentHostHelper.ContentPresenterMarginProperty);
		}

		// Token: 0x0600146B RID: 5227 RVA: 0x0003CA5E File Offset: 0x0003AC5E
		public static void SetContentPresenterMargin(ScrollViewer contentHost, Thickness value)
		{
			contentHost.SetValue(TextControlContentHostHelper.ContentPresenterMarginProperty, value);
		}

		// Token: 0x040006D9 RID: 1753
		public static readonly DependencyProperty ContentPresenterMarginProperty = DependencyProperty.RegisterAttached("ContentPresenterMargin", typeof(Thickness), typeof(TextControlContentHostHelper));
	}
}
