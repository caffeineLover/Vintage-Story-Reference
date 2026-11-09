using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ModernWpf.Controls.Primitives;

namespace ModernWpf.Controls
{
	// Token: 0x02000280 RID: 640
	[StyleTypedProperty(Property = "Style", StyleTargetType = typeof(TitleBarControl))]
	[StyleTypedProperty(Property = "ButtonStyle", StyleTargetType = typeof(TitleBarButton))]
	[StyleTypedProperty(Property = "BackButtonStyle", StyleTargetType = typeof(TitleBarButton))]
	public static class TitleBar
	{
		// Token: 0x170003AE RID: 942
		// (get) Token: 0x0600129A RID: 4762 RVA: 0x00036ACF File Offset: 0x00034CCF
		public static ComponentResourceKey HeightKey { get; } = new ComponentResourceKey(typeof(TitleBar), "HeightKey");

		// Token: 0x0600129B RID: 4763 RVA: 0x00036AD6 File Offset: 0x00034CD6
		public static Brush GetBackground(Window window)
		{
			return (Brush)window.GetValue(TitleBar.BackgroundProperty);
		}

		// Token: 0x0600129C RID: 4764 RVA: 0x00036AE8 File Offset: 0x00034CE8
		public static void SetBackground(Window window, Brush value)
		{
			window.SetValue(TitleBar.BackgroundProperty, value);
		}

		// Token: 0x0600129D RID: 4765 RVA: 0x00036AF6 File Offset: 0x00034CF6
		public static Brush GetForeground(Window window)
		{
			return (Brush)window.GetValue(TitleBar.ForegroundProperty);
		}

		// Token: 0x0600129E RID: 4766 RVA: 0x00036B08 File Offset: 0x00034D08
		public static void SetForeground(Window window, Brush value)
		{
			window.SetValue(TitleBar.ForegroundProperty, value);
		}

		// Token: 0x0600129F RID: 4767 RVA: 0x00036B16 File Offset: 0x00034D16
		public static Brush GetInactiveBackground(Window window)
		{
			return (Brush)window.GetValue(TitleBar.InactiveBackgroundProperty);
		}

		// Token: 0x060012A0 RID: 4768 RVA: 0x00036B28 File Offset: 0x00034D28
		public static void SetInactiveBackground(Window window, Brush value)
		{
			window.SetValue(TitleBar.InactiveBackgroundProperty, value);
		}

		// Token: 0x060012A1 RID: 4769 RVA: 0x00036B36 File Offset: 0x00034D36
		public static Brush GetInactiveForeground(Window window)
		{
			return (Brush)window.GetValue(TitleBar.InactiveForegroundProperty);
		}

		// Token: 0x060012A2 RID: 4770 RVA: 0x00036B48 File Offset: 0x00034D48
		public static void SetInactiveForeground(Window window, Brush value)
		{
			window.SetValue(TitleBar.InactiveForegroundProperty, value);
		}

		// Token: 0x060012A3 RID: 4771 RVA: 0x00036B56 File Offset: 0x00034D56
		public static Style GetStyle(Window window)
		{
			return (Style)window.GetValue(TitleBar.StyleProperty);
		}

		// Token: 0x060012A4 RID: 4772 RVA: 0x00036B68 File Offset: 0x00034D68
		public static void SetStyle(Window window, Style value)
		{
			window.SetValue(TitleBar.StyleProperty, value);
		}

		// Token: 0x060012A5 RID: 4773 RVA: 0x00036B76 File Offset: 0x00034D76
		public static Style GetButtonStyle(Window window)
		{
			return (Style)window.GetValue(TitleBar.ButtonStyleProperty);
		}

		// Token: 0x060012A6 RID: 4774 RVA: 0x00036B88 File Offset: 0x00034D88
		public static void SetButtonStyle(Window window, Style value)
		{
			window.SetValue(TitleBar.ButtonStyleProperty, value);
		}

		// Token: 0x060012A7 RID: 4775 RVA: 0x00036B96 File Offset: 0x00034D96
		public static bool GetIsIconVisible(Window window)
		{
			return (bool)window.GetValue(TitleBar.IsIconVisibleProperty);
		}

		// Token: 0x060012A8 RID: 4776 RVA: 0x00036BA8 File Offset: 0x00034DA8
		public static void SetIsIconVisible(Window window, bool value)
		{
			window.SetValue(TitleBar.IsIconVisibleProperty, value);
		}

		// Token: 0x060012A9 RID: 4777 RVA: 0x00036BBB File Offset: 0x00034DBB
		public static bool GetIsBackButtonVisible(Window window)
		{
			return (bool)window.GetValue(TitleBar.IsBackButtonVisibleProperty);
		}

		// Token: 0x060012AA RID: 4778 RVA: 0x00036BCD File Offset: 0x00034DCD
		public static void SetIsBackButtonVisible(Window window, bool value)
		{
			window.SetValue(TitleBar.IsBackButtonVisibleProperty, value);
		}

		// Token: 0x060012AB RID: 4779 RVA: 0x00036BE0 File Offset: 0x00034DE0
		public static bool GetIsBackEnabled(Window window)
		{
			return (bool)window.GetValue(TitleBar.IsBackEnabledProperty);
		}

		// Token: 0x060012AC RID: 4780 RVA: 0x00036BF2 File Offset: 0x00034DF2
		public static void SetIsBackEnabled(Window window, bool value)
		{
			window.SetValue(TitleBar.IsBackEnabledProperty, value);
		}

		// Token: 0x060012AD RID: 4781 RVA: 0x00036C05 File Offset: 0x00034E05
		public static ICommand GetBackButtonCommand(Window window)
		{
			return (ICommand)window.GetValue(TitleBar.BackButtonCommandProperty);
		}

		// Token: 0x060012AE RID: 4782 RVA: 0x00036C17 File Offset: 0x00034E17
		public static void SetBackButtonCommand(Window window, ICommand value)
		{
			window.SetValue(TitleBar.BackButtonCommandProperty, value);
		}

		// Token: 0x060012AF RID: 4783 RVA: 0x00036C25 File Offset: 0x00034E25
		public static object GetBackButtonCommandParameter(Window window)
		{
			return window.GetValue(TitleBar.BackButtonCommandParameterProperty);
		}

		// Token: 0x060012B0 RID: 4784 RVA: 0x00036C32 File Offset: 0x00034E32
		public static void SetBackButtonCommandParameter(Window window, object value)
		{
			window.SetValue(TitleBar.BackButtonCommandParameterProperty, value);
		}

		// Token: 0x060012B1 RID: 4785 RVA: 0x00036C40 File Offset: 0x00034E40
		public static IInputElement GetBackButtonCommandTarget(Window window)
		{
			return (IInputElement)window.GetValue(TitleBar.BackButtonCommandTargetProperty);
		}

		// Token: 0x060012B2 RID: 4786 RVA: 0x00036C52 File Offset: 0x00034E52
		public static void SetBackButtonCommandTarget(Window window, IInputElement value)
		{
			window.SetValue(TitleBar.BackButtonCommandTargetProperty, value);
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x00036C60 File Offset: 0x00034E60
		public static Style GetBackButtonStyle(Window window)
		{
			return (Style)window.GetValue(TitleBar.BackButtonStyleProperty);
		}

		// Token: 0x060012B4 RID: 4788 RVA: 0x00036C72 File Offset: 0x00034E72
		public static void SetBackButtonStyle(Window window, Style value)
		{
			window.SetValue(TitleBar.BackButtonStyleProperty, value);
		}

		// Token: 0x060012B5 RID: 4789 RVA: 0x00036C80 File Offset: 0x00034E80
		public static bool GetExtendViewIntoTitleBar(Window window)
		{
			return (bool)window.GetValue(TitleBar.ExtendViewIntoTitleBarProperty);
		}

		// Token: 0x060012B6 RID: 4790 RVA: 0x00036C92 File Offset: 0x00034E92
		public static void SetExtendViewIntoTitleBar(Window window, bool value)
		{
			window.SetValue(TitleBar.ExtendViewIntoTitleBarProperty, value);
		}

		// Token: 0x060012B7 RID: 4791 RVA: 0x00036CA5 File Offset: 0x00034EA5
		public static double GetSystemOverlayLeftInset(Window window)
		{
			return (double)window.GetValue(TitleBar.SystemOverlayLeftInsetProperty);
		}

		// Token: 0x060012B8 RID: 4792 RVA: 0x00036CB7 File Offset: 0x00034EB7
		internal static void SetSystemOverlayLeftInset(Window window, double value)
		{
			window.SetValue(TitleBar.SystemOverlayLeftInsetPropertyKey, value);
		}

		// Token: 0x060012B9 RID: 4793 RVA: 0x00036CCA File Offset: 0x00034ECA
		public static double GetSystemOverlayRightInset(Window window)
		{
			return (double)window.GetValue(TitleBar.SystemOverlayRightInsetProperty);
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x00036CDC File Offset: 0x00034EDC
		internal static void SetSystemOverlayRightInset(Window window, double value)
		{
			window.SetValue(TitleBar.SystemOverlayRightInsetPropertyKey, value);
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x00036CEF File Offset: 0x00034EEF
		public static double GetHeight(Window window)
		{
			return (double)window.GetValue(TitleBar.HeightProperty);
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x00036D01 File Offset: 0x00034F01
		internal static void SetHeight(Window window, double value)
		{
			window.SetValue(TitleBar.HeightPropertyKey, value);
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x00036D14 File Offset: 0x00034F14
		public static void AddBackRequestedHandler(Window window, EventHandler<BackRequestedEventArgs> handler)
		{
			window.AddHandler(TitleBar.BackRequestedEvent, handler);
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x00036D22 File Offset: 0x00034F22
		public static void RemoveBackRequestedHandler(Window window, EventHandler<BackRequestedEventArgs> handler)
		{
			window.RemoveHandler(TitleBar.BackRequestedEvent, handler);
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x00036D30 File Offset: 0x00034F30
		internal static void RaiseBackRequested(Window window)
		{
			window.RaiseEvent(new BackRequestedEventArgs(window));
		}

		// Token: 0x0400060A RID: 1546
		private const string StylePropertyName = "Style";

		// Token: 0x0400060B RID: 1547
		private const string ButtonStylePropertyName = "ButtonStyle";

		// Token: 0x0400060C RID: 1548
		private const string BackButtonStylePropertyName = "BackButtonStyle";

		// Token: 0x0400060E RID: 1550
		public static readonly DependencyProperty BackgroundProperty = DependencyProperty.RegisterAttached("Background", typeof(Brush), typeof(TitleBar));

		// Token: 0x0400060F RID: 1551
		public static readonly DependencyProperty ForegroundProperty = DependencyProperty.RegisterAttached("Foreground", typeof(Brush), typeof(TitleBar));

		// Token: 0x04000610 RID: 1552
		public static readonly DependencyProperty InactiveBackgroundProperty = DependencyProperty.RegisterAttached("InactiveBackground", typeof(Brush), typeof(TitleBar));

		// Token: 0x04000611 RID: 1553
		public static readonly DependencyProperty InactiveForegroundProperty = DependencyProperty.RegisterAttached("InactiveForeground", typeof(Brush), typeof(TitleBar));

		// Token: 0x04000612 RID: 1554
		public static readonly DependencyProperty StyleProperty = DependencyProperty.RegisterAttached("Style", typeof(Style), typeof(TitleBar));

		// Token: 0x04000613 RID: 1555
		public static readonly DependencyProperty ButtonStyleProperty = DependencyProperty.RegisterAttached("ButtonStyle", typeof(Style), typeof(TitleBar));

		// Token: 0x04000614 RID: 1556
		public static readonly DependencyProperty IsIconVisibleProperty = DependencyProperty.RegisterAttached("IsIconVisible", typeof(bool), typeof(TitleBar), new PropertyMetadata(false));

		// Token: 0x04000615 RID: 1557
		public static readonly DependencyProperty IsBackButtonVisibleProperty = DependencyProperty.RegisterAttached("IsBackButtonVisible", typeof(bool), typeof(TitleBar));

		// Token: 0x04000616 RID: 1558
		public static readonly DependencyProperty IsBackEnabledProperty = DependencyProperty.RegisterAttached("IsBackEnabled", typeof(bool), typeof(TitleBar), new PropertyMetadata(true));

		// Token: 0x04000617 RID: 1559
		public static readonly DependencyProperty BackButtonCommandProperty = DependencyProperty.RegisterAttached("BackButtonCommand", typeof(ICommand), typeof(TitleBar));

		// Token: 0x04000618 RID: 1560
		public static readonly DependencyProperty BackButtonCommandParameterProperty = DependencyProperty.RegisterAttached("BackButtonCommandParameter", typeof(object), typeof(TitleBar));

		// Token: 0x04000619 RID: 1561
		public static readonly DependencyProperty BackButtonCommandTargetProperty = DependencyProperty.RegisterAttached("BackButtonCommandTarget", typeof(IInputElement), typeof(TitleBar));

		// Token: 0x0400061A RID: 1562
		public static readonly DependencyProperty BackButtonStyleProperty = DependencyProperty.RegisterAttached("BackButtonStyle", typeof(Style), typeof(TitleBar));

		// Token: 0x0400061B RID: 1563
		public static readonly DependencyProperty ExtendViewIntoTitleBarProperty = DependencyProperty.RegisterAttached("ExtendViewIntoTitleBar", typeof(bool), typeof(TitleBar), new PropertyMetadata(false));

		// Token: 0x0400061C RID: 1564
		internal static readonly DependencyPropertyKey SystemOverlayLeftInsetPropertyKey = DependencyProperty.RegisterAttachedReadOnly("SystemOverlayLeftInset", typeof(double), typeof(TitleBar), new PropertyMetadata(0.0));

		// Token: 0x0400061D RID: 1565
		public static readonly DependencyProperty SystemOverlayLeftInsetProperty = TitleBar.SystemOverlayLeftInsetPropertyKey.DependencyProperty;

		// Token: 0x0400061E RID: 1566
		internal static readonly DependencyPropertyKey SystemOverlayRightInsetPropertyKey = DependencyProperty.RegisterAttachedReadOnly("SystemOverlayRightInset", typeof(double), typeof(TitleBar), new PropertyMetadata(0.0));

		// Token: 0x0400061F RID: 1567
		public static readonly DependencyProperty SystemOverlayRightInsetProperty = TitleBar.SystemOverlayRightInsetPropertyKey.DependencyProperty;

		// Token: 0x04000620 RID: 1568
		internal static readonly DependencyPropertyKey HeightPropertyKey = DependencyProperty.RegisterAttachedReadOnly("Height", typeof(double), typeof(TitleBar), new PropertyMetadata(32.0));

		// Token: 0x04000621 RID: 1569
		public static readonly DependencyProperty HeightProperty = TitleBar.HeightPropertyKey.DependencyProperty;

		// Token: 0x04000622 RID: 1570
		public static readonly RoutedEvent BackRequestedEvent = EventManager.RegisterRoutedEvent("BackRequested", 1, typeof(EventHandler<BackRequestedEventArgs>), typeof(TitleBar));
	}
}
