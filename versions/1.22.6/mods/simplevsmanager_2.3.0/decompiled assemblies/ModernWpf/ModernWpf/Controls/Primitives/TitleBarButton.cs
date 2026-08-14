using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interop;
using System.Windows.Media;
using Standard;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x020002B0 RID: 688
	public class TitleBarButton : Button
	{
		// Token: 0x060014E5 RID: 5349 RVA: 0x0003E508 File Offset: 0x0003C708
		static TitleBarButton()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TitleBarButton), new FrameworkPropertyMetadata(typeof(TitleBarButton)));
		}

		// Token: 0x060014E6 RID: 5350 RVA: 0x0003E6B8 File Offset: 0x0003C8B8
		public TitleBarButton()
		{
			if (OSVersionHelper.IsWindows11OrGreater)
			{
				base.Loaded += new RoutedEventHandler(this.OnLoaded);
				base.Unloaded += new RoutedEventHandler(this.OnUnloaded);
			}
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x060014E7 RID: 5351 RVA: 0x0003E6EB File Offset: 0x0003C8EB
		// (set) Token: 0x060014E8 RID: 5352 RVA: 0x0003E6FD File Offset: 0x0003C8FD
		public bool IsActive
		{
			get
			{
				return (bool)base.GetValue(TitleBarButton.IsActiveProperty);
			}
			set
			{
				base.SetValue(TitleBarButton.IsActiveProperty, value);
			}
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x060014E9 RID: 5353 RVA: 0x0003E710 File Offset: 0x0003C910
		// (set) Token: 0x060014EA RID: 5354 RVA: 0x0003E722 File Offset: 0x0003C922
		public Brush InactiveBackground
		{
			get
			{
				return (Brush)base.GetValue(TitleBarButton.InactiveBackgroundProperty);
			}
			set
			{
				base.SetValue(TitleBarButton.InactiveBackgroundProperty, value);
			}
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x060014EB RID: 5355 RVA: 0x0003E730 File Offset: 0x0003C930
		// (set) Token: 0x060014EC RID: 5356 RVA: 0x0003E742 File Offset: 0x0003C942
		public Brush InactiveForeground
		{
			get
			{
				return (Brush)base.GetValue(TitleBarButton.InactiveForegroundProperty);
			}
			set
			{
				base.SetValue(TitleBarButton.InactiveForegroundProperty, value);
			}
		}

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x060014ED RID: 5357 RVA: 0x0003E750 File Offset: 0x0003C950
		// (set) Token: 0x060014EE RID: 5358 RVA: 0x0003E762 File Offset: 0x0003C962
		public Brush HoverBackground
		{
			get
			{
				return (Brush)base.GetValue(TitleBarButton.HoverBackgroundProperty);
			}
			set
			{
				base.SetValue(TitleBarButton.HoverBackgroundProperty, value);
			}
		}

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x060014EF RID: 5359 RVA: 0x0003E770 File Offset: 0x0003C970
		// (set) Token: 0x060014F0 RID: 5360 RVA: 0x0003E782 File Offset: 0x0003C982
		public Brush HoverForeground
		{
			get
			{
				return (Brush)base.GetValue(TitleBarButton.HoverForegroundProperty);
			}
			set
			{
				base.SetValue(TitleBarButton.HoverForegroundProperty, value);
			}
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x060014F1 RID: 5361 RVA: 0x0003E790 File Offset: 0x0003C990
		// (set) Token: 0x060014F2 RID: 5362 RVA: 0x0003E7A2 File Offset: 0x0003C9A2
		public Brush PressedBackground
		{
			get
			{
				return (Brush)base.GetValue(TitleBarButton.PressedBackgroundProperty);
			}
			set
			{
				base.SetValue(TitleBarButton.PressedBackgroundProperty, value);
			}
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x060014F3 RID: 5363 RVA: 0x0003E7B0 File Offset: 0x0003C9B0
		// (set) Token: 0x060014F4 RID: 5364 RVA: 0x0003E7C2 File Offset: 0x0003C9C2
		public Brush PressedForeground
		{
			get
			{
				return (Brush)base.GetValue(TitleBarButton.PressedForegroundProperty);
			}
			set
			{
				base.SetValue(TitleBarButton.PressedForegroundProperty, value);
			}
		}

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x060014F5 RID: 5365 RVA: 0x0003E7D0 File Offset: 0x0003C9D0
		// (set) Token: 0x060014F6 RID: 5366 RVA: 0x0003E7E2 File Offset: 0x0003C9E2
		public bool IsMouseReallyOver
		{
			get
			{
				return (bool)base.GetValue(TitleBarButton.IsMouseReallyOverProperty);
			}
			private set
			{
				base.SetValue(TitleBarButton.IsMouseReallyOverPropertyKey, value);
			}
		}

		// Token: 0x060014F7 RID: 5367 RVA: 0x0003E7F5 File Offset: 0x0003C9F5
		private void UpdateIsMouseReallyOver()
		{
			this.IsMouseReallyOver = (base.IsMouseOver || this.IsNCMouseOver);
		}

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x060014F8 RID: 5368 RVA: 0x0003E80E File Offset: 0x0003CA0E
		// (set) Token: 0x060014F9 RID: 5369 RVA: 0x0003E820 File Offset: 0x0003CA20
		public bool IsReallyPressed
		{
			get
			{
				return (bool)base.GetValue(TitleBarButton.IsReallyPressedProperty);
			}
			private set
			{
				base.SetValue(TitleBarButton.IsReallyPressedPropertyKey, value);
			}
		}

		// Token: 0x060014FA RID: 5370 RVA: 0x0003E833 File Offset: 0x0003CA33
		private void UpdateIsReallyPressed()
		{
			this.IsReallyPressed = (base.IsPressed || this.IsNCPressed);
		}

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x060014FB RID: 5371 RVA: 0x0003E84C File Offset: 0x0003CA4C
		// (set) Token: 0x060014FC RID: 5372 RVA: 0x0003E854 File Offset: 0x0003CA54
		internal uint? HitTestCode { get; set; }

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x060014FD RID: 5373 RVA: 0x0003E85D File Offset: 0x0003CA5D
		// (set) Token: 0x060014FE RID: 5374 RVA: 0x0003E865 File Offset: 0x0003CA65
		private bool IsNCMouseOver
		{
			get
			{
				return this._isNCMouseOver;
			}
			set
			{
				if (this._isNCMouseOver != value)
				{
					this._isNCMouseOver = value;
					this.OnIsNCMouseOverChanged();
				}
			}
		}

		// Token: 0x060014FF RID: 5375 RVA: 0x0003E87D File Offset: 0x0003CA7D
		private void OnIsNCMouseOverChanged()
		{
			this.UpdateIsMouseReallyOver();
			if (!this.IsNCMouseOver)
			{
				this.IsNCPressed = false;
			}
		}

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06001500 RID: 5376 RVA: 0x0003E894 File Offset: 0x0003CA94
		// (set) Token: 0x06001501 RID: 5377 RVA: 0x0003E89C File Offset: 0x0003CA9C
		private bool IsNCPressed
		{
			get
			{
				return this._isNCPressed;
			}
			set
			{
				if (this._isNCPressed != value)
				{
					this._isNCPressed = value;
					this.OnIsNCPressedChanged();
				}
			}
		}

		// Token: 0x06001502 RID: 5378 RVA: 0x0003E8B4 File Offset: 0x0003CAB4
		private void OnIsNCPressedChanged()
		{
			this.UpdateIsReallyPressed();
		}

		// Token: 0x06001503 RID: 5379 RVA: 0x0003E8BC File Offset: 0x0003CABC
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == UIElement.IsMouseOverProperty)
			{
				this.UpdateIsMouseReallyOver();
				return;
			}
			if (e.Property == ButtonBase.IsPressedProperty)
			{
				this.UpdateIsReallyPressed();
			}
		}

		// Token: 0x06001504 RID: 5380 RVA: 0x0003E8EE File Offset: 0x0003CAEE
		internal void DoClick()
		{
			this.OnClick();
		}

		// Token: 0x06001505 RID: 5381 RVA: 0x0003E8F8 File Offset: 0x0003CAF8
		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			if (this.HitTestCode != null)
			{
				this._parentHwndSource = (PresentationSource.FromVisual(this) as HwndSource);
				if (this._parentHwndSource != null)
				{
					this._parentHwndSource.AddHook(new HwndSourceHook(this.TitleBarButtonFilterMessage));
				}
			}
		}

		// Token: 0x06001506 RID: 5382 RVA: 0x0003E945 File Offset: 0x0003CB45
		private void OnUnloaded(object sender, RoutedEventArgs e)
		{
			if (this._parentHwndSource != null)
			{
				this._parentHwndSource.RemoveHook(new HwndSourceHook(this.TitleBarButtonFilterMessage));
				this._parentHwndSource = null;
			}
		}

		// Token: 0x06001507 RID: 5383 RVA: 0x0003E970 File Offset: 0x0003CB70
		private IntPtr TitleBarButtonFilterMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if (msg <= 161)
			{
				if (msg != 132)
				{
					if (msg == 161)
					{
						if (this.IsNCMouseOver)
						{
							this.IsNCPressed = true;
							handled = true;
						}
					}
				}
				else
				{
					if (this.IsMousePositionWithin(lParam))
					{
						this.IsNCMouseOver = true;
						handled = true;
						return (IntPtr)((long)((ulong)this.HitTestCode.Value));
					}
					this.IsNCMouseOver = false;
				}
			}
			else if (msg != 162)
			{
				if (msg == 674)
				{
					this.IsNCMouseOver = false;
				}
			}
			else if (this.IsNCPressed)
			{
				this.IsNCPressed = false;
				if (this.IsMousePositionWithin(lParam))
				{
					this.OnClick();
				}
				handled = true;
			}
			return IntPtr.Zero;
		}

		// Token: 0x06001508 RID: 5384 RVA: 0x0003EA28 File Offset: 0x0003CC28
		private bool IsMousePositionWithin(IntPtr lParam)
		{
			Point point;
			point..ctor((double)Utility.GET_X_LPARAM(lParam), (double)Utility.GET_Y_LPARAM(lParam));
			Rect rect;
			rect..ctor(default(Point), base.RenderSize);
			Point point2 = base.PointFromScreen(point);
			return rect.Contains(point2);
		}

		// Token: 0x04000712 RID: 1810
		private HwndSource _parentHwndSource;

		// Token: 0x04000713 RID: 1811
		public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive", typeof(bool), typeof(TitleBarButton), new PropertyMetadata(false));

		// Token: 0x04000714 RID: 1812
		public static readonly DependencyProperty InactiveBackgroundProperty = DependencyProperty.Register("InactiveBackground", typeof(Brush), typeof(TitleBarButton), null);

		// Token: 0x04000715 RID: 1813
		public static readonly DependencyProperty InactiveForegroundProperty = DependencyProperty.Register("InactiveForeground", typeof(Brush), typeof(TitleBarButton), null);

		// Token: 0x04000716 RID: 1814
		public static readonly DependencyProperty HoverBackgroundProperty = DependencyProperty.Register("HoverBackground", typeof(Brush), typeof(TitleBarButton), null);

		// Token: 0x04000717 RID: 1815
		public static readonly DependencyProperty HoverForegroundProperty = DependencyProperty.Register("HoverForeground", typeof(Brush), typeof(TitleBarButton), null);

		// Token: 0x04000718 RID: 1816
		public static readonly DependencyProperty PressedBackgroundProperty = DependencyProperty.Register("PressedBackground", typeof(Brush), typeof(TitleBarButton), null);

		// Token: 0x04000719 RID: 1817
		public static readonly DependencyProperty PressedForegroundProperty = DependencyProperty.Register("PressedForeground", typeof(Brush), typeof(TitleBarButton), null);

		// Token: 0x0400071A RID: 1818
		private static readonly DependencyPropertyKey IsMouseReallyOverPropertyKey = DependencyProperty.RegisterReadOnly("IsMouseReallyOver", typeof(bool), typeof(TitleBarButton), new PropertyMetadata(false));

		// Token: 0x0400071B RID: 1819
		public static readonly DependencyProperty IsMouseReallyOverProperty = TitleBarButton.IsMouseReallyOverPropertyKey.DependencyProperty;

		// Token: 0x0400071C RID: 1820
		private static readonly DependencyPropertyKey IsReallyPressedPropertyKey = DependencyProperty.RegisterReadOnly("IsReallyPressed", typeof(bool), typeof(TitleBarButton), new PropertyMetadata(false));

		// Token: 0x0400071D RID: 1821
		public static readonly DependencyProperty IsReallyPressedProperty = TitleBarButton.IsReallyPressedPropertyKey.DependencyProperty;

		// Token: 0x0400071F RID: 1823
		private bool _isNCMouseOver;

		// Token: 0x04000720 RID: 1824
		private bool _isNCPressed;
	}
}
