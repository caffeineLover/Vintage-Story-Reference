using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x02000290 RID: 656
	public static class HyperlinkHelper
	{
		// Token: 0x06001368 RID: 4968 RVA: 0x00039180 File Offset: 0x00037380
		public static bool GetIsPressEnabled(Hyperlink hyperlink)
		{
			return (bool)hyperlink.GetValue(HyperlinkHelper.IsPressEnabledProperty);
		}

		// Token: 0x06001369 RID: 4969 RVA: 0x00039192 File Offset: 0x00037392
		public static void SetIsPressEnabled(Hyperlink hyperlink, bool value)
		{
			hyperlink.SetValue(HyperlinkHelper.IsPressEnabledProperty, value);
		}

		// Token: 0x0600136A RID: 4970 RVA: 0x000391A8 File Offset: 0x000373A8
		private static void OnIsPressEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Hyperlink hyperlink = (Hyperlink)d;
			if ((bool)e.NewValue)
			{
				hyperlink.AddHandler(ContentElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(HyperlinkHelper.OnMouseLeftButtonDown), true);
				hyperlink.AddHandler(ContentElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(HyperlinkHelper.OnMouseLeftButtonUp), true);
				return;
			}
			hyperlink.RemoveHandler(ContentElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(HyperlinkHelper.OnMouseLeftButtonDown));
			hyperlink.RemoveHandler(ContentElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(HyperlinkHelper.OnMouseLeftButtonUp));
		}

		// Token: 0x0600136B RID: 4971 RVA: 0x0003922C File Offset: 0x0003742C
		private static void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			Hyperlink hyperlink = (Hyperlink)sender;
			if (hyperlink.IsMouseCaptured && e.ButtonState == 1)
			{
				hyperlink.SetValue(HyperlinkHelper.IsPressedPropertyKey, true);
			}
		}

		// Token: 0x0600136C RID: 4972 RVA: 0x00039264 File Offset: 0x00037464
		private static void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			Hyperlink hyperlink = (Hyperlink)sender;
			if (HyperlinkHelper.GetIsPressed(hyperlink))
			{
				hyperlink.SetValue(HyperlinkHelper.IsPressedPropertyKey, false);
			}
		}

		// Token: 0x0600136D RID: 4973 RVA: 0x00039291 File Offset: 0x00037491
		public static bool GetIsPressed(Hyperlink hyperlink)
		{
			return (bool)hyperlink.GetValue(HyperlinkHelper.IsPressedProperty);
		}

		// Token: 0x04000680 RID: 1664
		private static readonly DependencyProperty IsPressEnabledProperty = DependencyProperty.RegisterAttached("IsPressEnabled", typeof(bool), typeof(HyperlinkHelper), new PropertyMetadata(false, new PropertyChangedCallback(HyperlinkHelper.OnIsPressEnabledChanged)));

		// Token: 0x04000681 RID: 1665
		private static readonly DependencyPropertyKey IsPressedPropertyKey = DependencyProperty.RegisterAttachedReadOnly("IsPressed", typeof(bool), typeof(HyperlinkHelper), new PropertyMetadata(false));

		// Token: 0x04000682 RID: 1666
		public static readonly DependencyProperty IsPressedProperty = HyperlinkHelper.IsPressedPropertyKey.DependencyProperty;
	}
}
