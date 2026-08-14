using System;
using System.Windows;
using System.Windows.Controls;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x02000282 RID: 642
	public static class BorderHelper
	{
		// Token: 0x060012C6 RID: 4806 RVA: 0x000370B3 File Offset: 0x000352B3
		public static UIElement GetChild(Border border)
		{
			return (UIElement)border.GetValue(BorderHelper.ChildProperty);
		}

		// Token: 0x060012C7 RID: 4807 RVA: 0x000370C5 File Offset: 0x000352C5
		public static void SetChild(Border border, UIElement value)
		{
			border.SetValue(BorderHelper.ChildProperty, value);
		}

		// Token: 0x060012C8 RID: 4808 RVA: 0x000370D3 File Offset: 0x000352D3
		private static void OnChildChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Border)d).Child = (UIElement)e.NewValue;
		}

		// Token: 0x04000624 RID: 1572
		public static readonly DependencyProperty ChildProperty = DependencyProperty.RegisterAttached("Child", typeof(UIElement), typeof(BorderHelper), new PropertyMetadata(null, new PropertyChangedCallback(BorderHelper.OnChildChanged)));
	}
}
