using System;
using System.Windows;
using System.Windows.Controls;
using ModernWpf.Controls.Primitives;

namespace ModernWpf.Controls
{
	// Token: 0x02000044 RID: 68
	public static class FlyoutService
	{
		// Token: 0x06000293 RID: 659 RVA: 0x00008EF9 File Offset: 0x000070F9
		public static FlyoutBase GetFlyout(Button button)
		{
			return (FlyoutBase)button.GetValue(FlyoutService.FlyoutProperty);
		}

		// Token: 0x06000294 RID: 660 RVA: 0x00008F0B File Offset: 0x0000710B
		public static void SetFlyout(Button button, FlyoutBase value)
		{
			button.SetValue(FlyoutService.FlyoutProperty, value);
		}

		// Token: 0x06000295 RID: 661 RVA: 0x00008F1C File Offset: 0x0000711C
		private static void OnFlyoutChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Button button = (Button)d;
			if (e.OldValue is FlyoutBase)
			{
				button.Click -= new RoutedEventHandler(FlyoutService.OnButtonClick);
			}
			if (e.NewValue is FlyoutBase)
			{
				button.Click += new RoutedEventHandler(FlyoutService.OnButtonClick);
			}
		}

		// Token: 0x06000296 RID: 662 RVA: 0x00008F70 File Offset: 0x00007170
		private static void OnButtonClick(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;
			FlyoutBase flyout = FlyoutService.GetFlyout(button);
			if (flyout != null)
			{
				flyout.ShowAt(button);
			}
		}

		// Token: 0x04000100 RID: 256
		public static readonly DependencyProperty FlyoutProperty = DependencyProperty.RegisterAttached("Flyout", typeof(FlyoutBase), typeof(FlyoutService), new PropertyMetadata(new PropertyChangedCallback(FlyoutService.OnFlyoutChanged)));
	}
}
