using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using ModernWpf.Controls.Primitives;

namespace ModernWpf.Controls
{
	// Token: 0x02000043 RID: 67
	public class FlyoutPresenter : ContentControl
	{
		// Token: 0x0600028B RID: 651 RVA: 0x00008DAC File Offset: 0x00006FAC
		static FlyoutPresenter()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(FlyoutPresenter), new FrameworkPropertyMetadata(typeof(FlyoutPresenter)));
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600028D RID: 653 RVA: 0x00008E2B File Offset: 0x0000702B
		// (set) Token: 0x0600028E RID: 654 RVA: 0x00008E3D File Offset: 0x0000703D
		public CornerRadius CornerRadius
		{
			get
			{
				return (CornerRadius)base.GetValue(FlyoutPresenter.CornerRadiusProperty);
			}
			set
			{
				base.SetValue(FlyoutPresenter.CornerRadiusProperty, value);
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600028F RID: 655 RVA: 0x00008E50 File Offset: 0x00007050
		// (set) Token: 0x06000290 RID: 656 RVA: 0x00008E62 File Offset: 0x00007062
		public bool IsDefaultShadowEnabled
		{
			get
			{
				return (bool)base.GetValue(FlyoutPresenter.IsDefaultShadowEnabledProperty);
			}
			set
			{
				base.SetValue(FlyoutPresenter.IsDefaultShadowEnabledProperty, value);
			}
		}

		// Token: 0x06000291 RID: 657 RVA: 0x00008E78 File Offset: 0x00007078
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (e.Key == 13)
			{
				Popup popup = base.Parent as Popup;
				if (popup != null && popup.IsOpen)
				{
					popup.SetCurrentValue(Popup.IsOpenProperty, false);
					e.Handled = true;
				}
			}
		}

		// Token: 0x06000292 RID: 658 RVA: 0x00008EC8 File Offset: 0x000070C8
		protected override void OnDpiChanged(DpiScale oldDpi, DpiScale newDpi)
		{
			base.OnDpiChanged(oldDpi, newDpi);
			BitmapCache bitmapCache = base.CacheMode as BitmapCache;
			if (bitmapCache != null)
			{
				bitmapCache.RenderAtScale = newDpi.PixelsPerDip;
			}
		}

		// Token: 0x040000FE RID: 254
		public static readonly DependencyProperty CornerRadiusProperty = ControlHelper.CornerRadiusProperty.AddOwner(typeof(FlyoutPresenter));

		// Token: 0x040000FF RID: 255
		public static readonly DependencyProperty IsDefaultShadowEnabledProperty = DependencyProperty.Register("IsDefaultShadowEnabled", typeof(bool), typeof(FlyoutPresenter), new PropertyMetadata(true));
	}
}
