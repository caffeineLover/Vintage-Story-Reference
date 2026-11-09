using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ModernWpf.Controls
{
	// Token: 0x0200026B RID: 619
	public class ScrollViewerEx : ScrollViewer
	{
		// Token: 0x060011AB RID: 4523 RVA: 0x00033B35 File Offset: 0x00031D35
		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);
			if (base.Style == null && base.ReadLocalValue(FrameworkElement.StyleProperty) == DependencyProperty.UnsetValue)
			{
				base.SetResourceReference(FrameworkElement.StyleProperty, typeof(ScrollViewer));
			}
		}

		// Token: 0x060011AC RID: 4524 RVA: 0x00033B6D File Offset: 0x00031D6D
		protected override void OnMouseWheel(MouseWheelEventArgs e)
		{
			if (e.Handled)
			{
				return;
			}
			if (base.ScrollableHeight > 0.0)
			{
				base.OnMouseWheel(e);
			}
		}
	}
}
