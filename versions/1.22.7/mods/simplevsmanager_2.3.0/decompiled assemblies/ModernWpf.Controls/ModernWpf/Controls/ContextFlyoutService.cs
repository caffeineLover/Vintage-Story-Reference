using System;
using System.Windows;
using System.Windows.Controls;
using ModernWpf.Controls.Primitives;

namespace ModernWpf.Controls
{
	// Token: 0x02000040 RID: 64
	public static class ContextFlyoutService
	{
		// Token: 0x06000272 RID: 626 RVA: 0x00008640 File Offset: 0x00006840
		public static FlyoutBase GetContextFlyout(FrameworkElement element)
		{
			return (FlyoutBase)element.GetValue(ContextFlyoutService.ContextFlyoutProperty);
		}

		// Token: 0x06000273 RID: 627 RVA: 0x00008652 File Offset: 0x00006852
		public static void SetContextFlyout(FrameworkElement element, FlyoutBase value)
		{
			element.SetValue(ContextFlyoutService.ContextFlyoutProperty, value);
		}

		// Token: 0x06000274 RID: 628 RVA: 0x00008660 File Offset: 0x00006860
		private static void OnContextFlyoutChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement frameworkElement = (FrameworkElement)d;
			bool flag = (FlyoutBase)e.OldValue != null;
			FlyoutBase flyoutBase = (FlyoutBase)e.NewValue;
			if (flag)
			{
				frameworkElement.ContextMenuOpening -= new ContextMenuEventHandler(ContextFlyoutService.OnContextMenuOpening);
			}
			if (flyoutBase != null)
			{
				frameworkElement.ContextMenuOpening += new ContextMenuEventHandler(ContextFlyoutService.OnContextMenuOpening);
			}
		}

		// Token: 0x06000275 RID: 629 RVA: 0x000086B8 File Offset: 0x000068B8
		private static void OnContextMenuOpening(object sender, ContextMenuEventArgs e)
		{
			FrameworkElement frameworkElement = (FrameworkElement)sender;
			FlyoutBase contextFlyout = ContextFlyoutService.GetContextFlyout(frameworkElement);
			if (contextFlyout != null)
			{
				e.Handled = true;
				contextFlyout.ShowAsContextFlyout(frameworkElement);
			}
		}

		// Token: 0x040000F1 RID: 241
		public static readonly DependencyProperty ContextFlyoutProperty = DependencyProperty.RegisterAttached("ContextFlyout", typeof(FlyoutBase), typeof(ContextFlyoutService), new PropertyMetadata(new PropertyChangedCallback(ContextFlyoutService.OnContextFlyoutChanged)));
	}
}
