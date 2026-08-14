using System;
using System.Windows;
using System.Windows.Input;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x0200029D RID: 669
	public static class PressHelper
	{
		// Token: 0x0600140E RID: 5134 RVA: 0x0003B90F File Offset: 0x00039B0F
		public static bool GetIsEnabled(UIElement element)
		{
			return (bool)element.GetValue(PressHelper.IsEnabledProperty);
		}

		// Token: 0x0600140F RID: 5135 RVA: 0x0003B921 File Offset: 0x00039B21
		public static void SetIsEnabled(UIElement element, bool value)
		{
			element.SetValue(PressHelper.IsEnabledProperty, value);
		}

		// Token: 0x06001410 RID: 5136 RVA: 0x0003B934 File Offset: 0x00039B34
		private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			UIElement uielement = (UIElement)d;
			if ((bool)e.NewValue)
			{
				uielement.MouseLeftButtonDown += new MouseButtonEventHandler(PressHelper.OnMouseLeftButtonDown);
				uielement.MouseLeftButtonUp += new MouseButtonEventHandler(PressHelper.OnMouseLeftButtonUp);
				uielement.MouseEnter += new MouseEventHandler(PressHelper.OnMouseEnter);
				uielement.MouseLeave += new MouseEventHandler(PressHelper.OnMouseLeave);
				PressHelper.UpdateIsPressed(uielement);
				return;
			}
			uielement.MouseLeftButtonDown -= new MouseButtonEventHandler(PressHelper.OnMouseLeftButtonDown);
			uielement.MouseLeftButtonUp -= new MouseButtonEventHandler(PressHelper.OnMouseLeftButtonUp);
			uielement.MouseEnter -= new MouseEventHandler(PressHelper.OnMouseEnter);
			uielement.MouseLeave -= new MouseEventHandler(PressHelper.OnMouseLeave);
			uielement.ClearValue(PressHelper.IsPressedPropertyKey);
		}

		// Token: 0x06001411 RID: 5137 RVA: 0x0003B9F8 File Offset: 0x00039BF8
		public static bool GetIsPressed(UIElement element)
		{
			return (bool)element.GetValue(PressHelper.IsPressedProperty);
		}

		// Token: 0x06001412 RID: 5138 RVA: 0x0003BA0A File Offset: 0x00039C0A
		private static void SetIsPressed(UIElement element, bool value)
		{
			element.SetValue(PressHelper.IsPressedPropertyKey, value);
		}

		// Token: 0x06001413 RID: 5139 RVA: 0x0003BA1D File Offset: 0x00039C1D
		private static void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			PressHelper.UpdateIsPressed((UIElement)sender);
		}

		// Token: 0x06001414 RID: 5140 RVA: 0x0003BA2A File Offset: 0x00039C2A
		private static void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			PressHelper.UpdateIsPressed((UIElement)sender);
		}

		// Token: 0x06001415 RID: 5141 RVA: 0x0003BA37 File Offset: 0x00039C37
		private static void OnMouseEnter(object sender, MouseEventArgs e)
		{
			PressHelper.UpdateIsPressed((UIElement)sender);
		}

		// Token: 0x06001416 RID: 5142 RVA: 0x0003BA44 File Offset: 0x00039C44
		private static void OnMouseLeave(object sender, MouseEventArgs e)
		{
			PressHelper.UpdateIsPressed((UIElement)sender);
		}

		// Token: 0x06001417 RID: 5143 RVA: 0x0003BA54 File Offset: 0x00039C54
		private static void UpdateIsPressed(UIElement element)
		{
			Rect rect;
			rect..ctor(default(Point), element.RenderSize);
			if (Mouse.LeftButton == 1 && element.IsMouseOver && rect.Contains(Mouse.GetPosition(element)))
			{
				PressHelper.SetIsPressed(element, true);
				return;
			}
			element.ClearValue(PressHelper.IsPressedPropertyKey);
		}

		// Token: 0x040006B6 RID: 1718
		public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(PressHelper), new PropertyMetadata(new PropertyChangedCallback(PressHelper.OnIsEnabledChanged)));

		// Token: 0x040006B7 RID: 1719
		private static readonly DependencyPropertyKey IsPressedPropertyKey = DependencyProperty.RegisterAttachedReadOnly("IsPressed", typeof(bool), typeof(PressHelper), null);

		// Token: 0x040006B8 RID: 1720
		public static readonly DependencyProperty IsPressedProperty = PressHelper.IsPressedPropertyKey.DependencyProperty;
	}
}
