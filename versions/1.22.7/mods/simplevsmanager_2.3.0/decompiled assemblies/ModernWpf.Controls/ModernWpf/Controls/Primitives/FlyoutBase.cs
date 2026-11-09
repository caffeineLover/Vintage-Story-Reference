using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x020000ED RID: 237
	public abstract class FlyoutBase : DependencyObject
	{
		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000C1E RID: 3102 RVA: 0x00029692 File Offset: 0x00027892
		// (set) Token: 0x06000C1F RID: 3103 RVA: 0x000296A4 File Offset: 0x000278A4
		public FlyoutPlacementMode Placement
		{
			get
			{
				return (FlyoutPlacementMode)base.GetValue(FlyoutBase.PlacementProperty);
			}
			set
			{
				base.SetValue(FlyoutBase.PlacementProperty, value);
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000C20 RID: 3104 RVA: 0x000296B7 File Offset: 0x000278B7
		// (set) Token: 0x06000C21 RID: 3105 RVA: 0x000296C9 File Offset: 0x000278C9
		public bool AreOpenCloseAnimationsEnabled
		{
			get
			{
				return (bool)base.GetValue(FlyoutBase.AreOpenCloseAnimationsEnabledProperty);
			}
			set
			{
				base.SetValue(FlyoutBase.AreOpenCloseAnimationsEnabledProperty, value);
			}
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x000296DC File Offset: 0x000278DC
		private static void OnAreOpenCloseAnimationsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((FlyoutBase)d).OnAreOpenCloseAnimationsEnabledChanged(e);
		}

		// Token: 0x06000C23 RID: 3107 RVA: 0x000296EA File Offset: 0x000278EA
		internal virtual void OnAreOpenCloseAnimationsEnabledChanged(DependencyPropertyChangedEventArgs e)
		{
			this.UpdatePopupAnimation();
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000C24 RID: 3108 RVA: 0x000296F2 File Offset: 0x000278F2
		// (set) Token: 0x06000C25 RID: 3109 RVA: 0x00029704 File Offset: 0x00027904
		public bool IsOpen
		{
			get
			{
				return (bool)base.GetValue(FlyoutBase.IsOpenProperty);
			}
			internal set
			{
				base.SetValue(FlyoutBase.IsOpenPropertyKey, value);
			}
		}

		// Token: 0x06000C26 RID: 3110 RVA: 0x00029717 File Offset: 0x00027917
		private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((FlyoutBase)d).OnIsOpenChanged();
		}

		// Token: 0x06000C27 RID: 3111 RVA: 0x00029724 File Offset: 0x00027924
		internal virtual void OnIsOpenChanged()
		{
			if (!this.IsOpen)
			{
				if (this.m_weakRefToPreviousFocus != null)
				{
					IInputElement inputElement;
					if (this.m_weakRefToPreviousFocus.TryGetTarget(ref inputElement))
					{
						inputElement.Focus();
					}
					this.m_weakRefToPreviousFocus = null;
				}
				return;
			}
			if (Keyboard.FocusedElement != null)
			{
				this.m_weakRefToPreviousFocus = new WeakReference<IInputElement>(Keyboard.FocusedElement);
			}
			Control presenter = this.m_presenter;
			if (presenter == null)
			{
				return;
			}
			presenter.Focus();
		}

		// Token: 0x06000C28 RID: 3112 RVA: 0x00029787 File Offset: 0x00027987
		internal virtual void UpdateIsOpen()
		{
			this.IsOpen = (this.m_popup != null && this.m_popup.IsOpen);
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000C29 RID: 3113 RVA: 0x000297A5 File Offset: 0x000279A5
		// (set) Token: 0x06000C2A RID: 3114 RVA: 0x000297B7 File Offset: 0x000279B7
		public FlyoutShowMode ShowMode
		{
			get
			{
				return (FlyoutShowMode)base.GetValue(FlyoutBase.ShowModeProperty);
			}
			set
			{
				base.SetValue(FlyoutBase.ShowModeProperty, value);
			}
		}

		// Token: 0x06000C2B RID: 3115 RVA: 0x000297CA File Offset: 0x000279CA
		public static FlyoutBase GetAttachedFlyout(FrameworkElement element)
		{
			return (FlyoutBase)element.GetValue(FlyoutBase.AttachedFlyoutProperty);
		}

		// Token: 0x06000C2C RID: 3116 RVA: 0x000297DC File Offset: 0x000279DC
		public static void SetAttachedFlyout(FrameworkElement element, FlyoutBase value)
		{
			element.SetValue(FlyoutBase.AttachedFlyoutProperty, value);
		}

		// Token: 0x06000C2D RID: 3117 RVA: 0x000297EC File Offset: 0x000279EC
		public static void ShowAttachedFlyout(FrameworkElement flyoutOwner)
		{
			FlyoutBase attachedFlyout = FlyoutBase.GetAttachedFlyout(flyoutOwner);
			if (attachedFlyout != null)
			{
				attachedFlyout.ShowAt(flyoutOwner);
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000C2E RID: 3118 RVA: 0x0002980A File Offset: 0x00027A0A
		internal virtual PopupAnimation DesiredPopupAnimation
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000C2F RID: 3119 RVA: 0x0002980D File Offset: 0x00027A0D
		internal PopupEx InternalPopup
		{
			get
			{
				return this.m_popup;
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000C30 RID: 3120 RVA: 0x00029815 File Offset: 0x00027A15
		// (set) Token: 0x06000C31 RID: 3121 RVA: 0x0002981D File Offset: 0x00027A1D
		internal double Offset { get; set; } = 4.0;

		// Token: 0x14000046 RID: 70
		// (add) Token: 0x06000C32 RID: 3122 RVA: 0x00029828 File Offset: 0x00027A28
		// (remove) Token: 0x06000C33 RID: 3123 RVA: 0x00029860 File Offset: 0x00027A60
		public event EventHandler<object> Opening;

		// Token: 0x14000047 RID: 71
		// (add) Token: 0x06000C34 RID: 3124 RVA: 0x00029898 File Offset: 0x00027A98
		// (remove) Token: 0x06000C35 RID: 3125 RVA: 0x000298D0 File Offset: 0x00027AD0
		public event EventHandler<object> Opened;

		// Token: 0x14000048 RID: 72
		// (add) Token: 0x06000C36 RID: 3126 RVA: 0x00029908 File Offset: 0x00027B08
		// (remove) Token: 0x06000C37 RID: 3127 RVA: 0x00029940 File Offset: 0x00027B40
		public event EventHandler<object> Closed;

		// Token: 0x14000049 RID: 73
		// (add) Token: 0x06000C38 RID: 3128 RVA: 0x00029978 File Offset: 0x00027B78
		// (remove) Token: 0x06000C39 RID: 3129 RVA: 0x000299B0 File Offset: 0x00027BB0
		internal event TypedEventHandler<FlyoutBase, FlyoutBaseClosingEventArgs> Closing;

		// Token: 0x06000C3A RID: 3130 RVA: 0x000299E5 File Offset: 0x00027BE5
		public void ShowAt(FrameworkElement placementTarget)
		{
			if (placementTarget == null)
			{
				throw new ArgumentNullException("placementTarget");
			}
			this.ShowAtCore(placementTarget, false);
		}

		// Token: 0x06000C3B RID: 3131 RVA: 0x000299FD File Offset: 0x00027BFD
		public void Hide()
		{
			this.CancelAsyncShow();
			this.HideCore();
		}

		// Token: 0x06000C3C RID: 3132
		protected abstract Control CreatePresenter();

		// Token: 0x06000C3D RID: 3133 RVA: 0x00029A0B File Offset: 0x00027C0B
		internal void ShowAsContextFlyout(FrameworkElement placementTarget)
		{
			if (placementTarget == null)
			{
				throw new ArgumentNullException("placementTarget");
			}
			this.ShowAtCore(placementTarget, true);
		}

		// Token: 0x06000C3E RID: 3134 RVA: 0x00029A24 File Offset: 0x00027C24
		internal virtual void ShowAtCore(FrameworkElement placementTarget, bool showAsContextFlyout)
		{
			this.CancelAsyncShow();
			if (this.m_popup != null && this.m_popup.IsOpen && this.m_target == placementTarget && this.m_showingAsContextFlyout == showAsContextFlyout)
			{
				return;
			}
			if (this.m_closing)
			{
				this.m_pendingShow = delegate()
				{
					this.ShowAtCore(placementTarget, showAsContextFlyout);
				};
				return;
			}
			this.PreparePopup(placementTarget, showAsContextFlyout);
			this.m_target = placementTarget;
			this.m_showingAsContextFlyout = showAsContextFlyout;
			this.OnOpening();
			this.m_popup.IsOpen = true;
		}

		// Token: 0x06000C3F RID: 3135 RVA: 0x00029ADB File Offset: 0x00027CDB
		internal virtual void HideCore()
		{
			if (this.m_popup != null && this.m_popup.IsOpen)
			{
				this.m_popup.IsOpen = false;
			}
		}

		// Token: 0x06000C40 RID: 3136 RVA: 0x00029AFE File Offset: 0x00027CFE
		internal virtual void OnOpening()
		{
			EventHandler<object> opening = this.Opening;
			if (opening == null)
			{
				return;
			}
			opening.Invoke(this, null);
		}

		// Token: 0x06000C41 RID: 3137 RVA: 0x00029B12 File Offset: 0x00027D12
		internal virtual void OnOpened()
		{
			EventHandler<object> opened = this.Opened;
			if (opened == null)
			{
				return;
			}
			opened.Invoke(this, null);
		}

		// Token: 0x06000C42 RID: 3138 RVA: 0x00029B28 File Offset: 0x00027D28
		internal virtual void OnClosed()
		{
			EventHandler<object> closed = this.Closed;
			if (closed != null)
			{
				closed.Invoke(this, null);
			}
			Action pendingShow = this.m_pendingShow;
			this.CancelAsyncShow();
			if (pendingShow != null)
			{
				this.m_asyncShow = base.Dispatcher.BeginInvoke(pendingShow, Array.Empty<object>());
			}
		}

		// Token: 0x06000C43 RID: 3139 RVA: 0x00029B6F File Offset: 0x00027D6F
		internal void BindPlacement(Control presenter)
		{
			presenter.SetBinding(CustomPopupPlacementHelper.PlacementProperty, new Binding
			{
				Path = new PropertyPath(FlyoutBase.PlacementProperty),
				Source = this,
				Converter = FlyoutBase.s_placementConverter
			});
		}

		// Token: 0x06000C44 RID: 3140 RVA: 0x00029BA4 File Offset: 0x00027DA4
		private void EnsurePresenter()
		{
			if (this.m_presenter == null)
			{
				this.m_presenter = this.CreatePresenter();
				this.BindPlacement(this.m_presenter);
			}
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x00029BC8 File Offset: 0x00027DC8
		private void EnsurePopup()
		{
			if (this.m_popup == null)
			{
				this.EnsurePresenter();
				this.m_popup = new PopupEx
				{
					Child = this.m_presenter,
					StaysOpen = false,
					AllowsTransparency = true,
					CustomPopupPlacementCallback = new CustomPopupPlacementCallback(this.PositionPopup)
				};
				this.m_popup.Opened += new EventHandler(this.OnPopupOpened);
				this.m_popup.Closing += new EventHandler(this.OnPopupClosing);
				this.m_popup.Closed += new EventHandler(this.OnPopupClosed);
				this.m_popup.IsOpenChanged += new EventHandler(this.OnPopupIsOpenChanged);
			}
		}

		// Token: 0x06000C46 RID: 3142 RVA: 0x00029C7C File Offset: 0x00027E7C
		private void PreparePopup(FrameworkElement placementTarget, bool showAsContextFlyout)
		{
			this.EnsurePopup();
			if (this.m_popup.IsOpen)
			{
				this.m_popup.IsOpen = false;
			}
			this.UpdatePopupAnimation();
			if (showAsContextFlyout)
			{
				this.m_presenter.ClearValue(FrameworkElement.WidthProperty);
				this.m_presenter.ClearValue(FrameworkElement.HeightProperty);
				this.m_popup.Placement = 8;
				this.m_popup.PlacementTarget = placementTarget;
				this.m_popup.ClearValue(Popup.PlacementRectangleProperty);
				return;
			}
			if (this.Placement == FlyoutPlacementMode.Full)
			{
				Window window = Window.GetWindow(placementTarget);
				if (window != null)
				{
					AdornerDecorator adornerDecorator = VisualTree.FindDescendant<AdornerDecorator>(window);
					if (adornerDecorator != null)
					{
						placementTarget = adornerDecorator;
						this.m_presenter.SetBinding(FrameworkElement.WidthProperty, new Binding
						{
							Path = new PropertyPath(FrameworkElement.ActualWidthProperty),
							Source = adornerDecorator
						});
						this.m_presenter.SetBinding(FrameworkElement.HeightProperty, new Binding
						{
							Path = new PropertyPath(FrameworkElement.ActualHeightProperty),
							Source = adornerDecorator
						});
					}
					else
					{
						placementTarget = window;
						FrameworkElement presenter = this.m_presenter;
						DependencyProperty widthProperty = FrameworkElement.WidthProperty;
						MultiBinding multiBinding = new MultiBinding();
						multiBinding.Converter = FlyoutBase.s_fullPlacementWidthConverter;
						multiBinding.Bindings.Add(new Binding
						{
							Path = new PropertyPath(FrameworkElement.ActualWidthProperty),
							Source = window
						});
						multiBinding.Bindings.Add(new Binding
						{
							Path = new PropertyPath(Control.BorderThicknessProperty),
							Source = window
						});
						presenter.SetBinding(widthProperty, multiBinding);
						FrameworkElement presenter2 = this.m_presenter;
						DependencyProperty heightProperty = FrameworkElement.HeightProperty;
						MultiBinding multiBinding2 = new MultiBinding();
						multiBinding2.Converter = FlyoutBase.s_fullPlacementHeightConverter;
						multiBinding2.Bindings.Add(new Binding
						{
							Path = new PropertyPath(FrameworkElement.ActualHeightProperty),
							Source = window
						});
						multiBinding2.Bindings.Add(new Binding
						{
							Path = new PropertyPath(Control.BorderThicknessProperty),
							Source = window
						});
						presenter2.SetBinding(heightProperty, multiBinding2);
					}
					this.m_popup.Placement = 3;
					this.m_popup.PlacementTarget = placementTarget;
					this.m_popup.ClearValue(Popup.PlacementRectangleProperty);
					return;
				}
			}
			this.m_presenter.ClearValue(FrameworkElement.WidthProperty);
			this.m_presenter.ClearValue(FrameworkElement.HeightProperty);
			this.m_popup.Placement = 11;
			this.m_popup.PlacementTarget = placementTarget;
			this.m_popup.PlacementRectangle = this.GetPlacementRectangle(placementTarget);
		}

		// Token: 0x06000C47 RID: 3143 RVA: 0x00029ED4 File Offset: 0x000280D4
		private void UpdatePopupAnimation()
		{
			if (this.m_popup != null)
			{
				this.m_popup.PopupAnimation = ((this.AreOpenCloseAnimationsEnabled && SharedHelpers.IsAnimationsEnabled) ? this.DesiredPopupAnimation : 0);
			}
		}

		// Token: 0x06000C48 RID: 3144 RVA: 0x00029F04 File Offset: 0x00028104
		internal Rect GetPlacementRectangle(UIElement target)
		{
			Rect empty = Rect.Empty;
			if (target != null)
			{
				Size renderSize = target.RenderSize;
				switch (this.Placement)
				{
				case FlyoutPlacementMode.Top:
				case FlyoutPlacementMode.Bottom:
				case FlyoutPlacementMode.TopEdgeAlignedLeft:
				case FlyoutPlacementMode.TopEdgeAlignedRight:
				case FlyoutPlacementMode.BottomEdgeAlignedLeft:
				case FlyoutPlacementMode.BottomEdgeAlignedRight:
					empty..ctor(new Point(0.0, -this.Offset), new Point(renderSize.Width, renderSize.Height + this.Offset));
					break;
				case FlyoutPlacementMode.Left:
				case FlyoutPlacementMode.Right:
				case FlyoutPlacementMode.LeftEdgeAlignedTop:
				case FlyoutPlacementMode.LeftEdgeAlignedBottom:
				case FlyoutPlacementMode.RightEdgeAlignedTop:
				case FlyoutPlacementMode.RightEdgeAlignedBottom:
					empty..ctor(new Point(-this.Offset, 0.0), new Point(renderSize.Width + this.Offset, renderSize.Height));
					break;
				}
			}
			return empty;
		}

		// Token: 0x06000C49 RID: 3145 RVA: 0x00029FD6 File Offset: 0x000281D6
		private void OnPopupOpened(object sender, EventArgs e)
		{
			this.OnOpened();
		}

		// Token: 0x06000C4A RID: 3146 RVA: 0x00029FDE File Offset: 0x000281DE
		private void OnPopupClosing(object sender, EventArgs e)
		{
			TypedEventHandler<FlyoutBase, FlyoutBaseClosingEventArgs> closing = this.Closing;
			if (closing != null)
			{
				closing.Invoke(this, new FlyoutBaseClosingEventArgs());
			}
			this.m_closing = true;
		}

		// Token: 0x06000C4B RID: 3147 RVA: 0x0002A000 File Offset: 0x00028200
		private void OnPopupClosed(object sender, EventArgs e)
		{
			this.m_closing = false;
			if (!this.m_popup.IsOpen)
			{
				this.m_popup.ClearValue(Popup.PlacementProperty);
				this.m_popup.ClearValue(Popup.PlacementTargetProperty);
				this.m_popup.ClearValue(Popup.PlacementRectangleProperty);
				this.m_popup.ClearValue(FrameworkElement.WidthProperty);
				this.m_popup.ClearValue(FrameworkElement.HeightProperty);
				this.m_target = null;
				this.m_showingAsContextFlyout = false;
			}
			this.OnClosed();
		}

		// Token: 0x06000C4C RID: 3148 RVA: 0x0002A085 File Offset: 0x00028285
		private void OnPopupIsOpenChanged(object sender, EventArgs e)
		{
			this.UpdateIsOpen();
		}

		// Token: 0x06000C4D RID: 3149 RVA: 0x0002A08D File Offset: 0x0002828D
		private CustomPopupPlacement[] PositionPopup(Size popupSize, Size targetSize, Point offset)
		{
			return this.PositionPopup(popupSize, targetSize, offset, this.m_presenter);
		}

		// Token: 0x06000C4E RID: 3150 RVA: 0x0002A09E File Offset: 0x0002829E
		internal CustomPopupPlacement[] PositionPopup(Size popupSize, Size targetSize, Point offset, FrameworkElement child)
		{
			return CustomPopupPlacementHelper.PositionPopup(this.Placement, popupSize, targetSize, offset, child);
		}

		// Token: 0x06000C4F RID: 3151 RVA: 0x0002A0B0 File Offset: 0x000282B0
		private void CancelAsyncShow()
		{
			this.m_pendingShow = null;
			if (this.m_asyncShow != null)
			{
				this.m_asyncShow.Abort();
				this.m_asyncShow = null;
			}
		}

		// Token: 0x040004FC RID: 1276
		public static readonly DependencyProperty PlacementProperty = DependencyProperty.Register("Placement", typeof(FlyoutPlacementMode), typeof(FlyoutBase), new PropertyMetadata(FlyoutPlacementMode.Top));

		// Token: 0x040004FD RID: 1277
		public static readonly DependencyProperty AreOpenCloseAnimationsEnabledProperty = DependencyProperty.Register("AreOpenCloseAnimationsEnabled", typeof(bool), typeof(FlyoutBase), new PropertyMetadata(true, new PropertyChangedCallback(FlyoutBase.OnAreOpenCloseAnimationsEnabledChanged)));

		// Token: 0x040004FE RID: 1278
		private static readonly DependencyPropertyKey IsOpenPropertyKey = DependencyProperty.RegisterReadOnly("IsOpen", typeof(bool), typeof(FlyoutBase), new PropertyMetadata(false, new PropertyChangedCallback(FlyoutBase.OnIsOpenChanged)));

		// Token: 0x040004FF RID: 1279
		public static readonly DependencyProperty IsOpenProperty = FlyoutBase.IsOpenPropertyKey.DependencyProperty;

		// Token: 0x04000500 RID: 1280
		public static readonly DependencyProperty ShowModeProperty = DependencyProperty.Register("ShowMode", typeof(FlyoutShowMode), typeof(FlyoutBase), new PropertyMetadata(FlyoutShowMode.Standard));

		// Token: 0x04000501 RID: 1281
		public static readonly DependencyProperty AttachedFlyoutProperty = DependencyProperty.RegisterAttached("AttachedFlyout", typeof(FlyoutBase), typeof(FlyoutBase));

		// Token: 0x04000507 RID: 1287
		private static readonly IMultiValueConverter s_fullPlacementWidthConverter = new FlyoutBase.FullPlacementWidthConverter();

		// Token: 0x04000508 RID: 1288
		private static readonly IMultiValueConverter s_fullPlacementHeightConverter = new FlyoutBase.FullPlacementHeightConverter();

		// Token: 0x04000509 RID: 1289
		private static readonly IValueConverter s_placementConverter = new FlyoutBase.PlacementConverter();

		// Token: 0x0400050A RID: 1290
		private const double s_offset = 4.0;

		// Token: 0x0400050B RID: 1291
		private Control m_presenter;

		// Token: 0x0400050C RID: 1292
		private PopupEx m_popup;

		// Token: 0x0400050D RID: 1293
		private FrameworkElement m_target;

		// Token: 0x0400050E RID: 1294
		private bool m_showingAsContextFlyout;

		// Token: 0x0400050F RID: 1295
		private WeakReference<IInputElement> m_weakRefToPreviousFocus;

		// Token: 0x04000510 RID: 1296
		private bool m_closing;

		// Token: 0x04000511 RID: 1297
		private Action m_pendingShow;

		// Token: 0x04000512 RID: 1298
		private DispatcherOperation m_asyncShow;

		// Token: 0x02000130 RID: 304
		private class FullPlacementWidthConverter : IMultiValueConverter
		{
			// Token: 0x06000D12 RID: 3346 RVA: 0x0002BD04 File Offset: 0x00029F04
			public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
			{
				double num = (double)values[0];
				Thickness thickness = (Thickness)values[1];
				return num - thickness.Left - thickness.Right;
			}

			// Token: 0x06000D13 RID: 3347 RVA: 0x0002BD37 File Offset: 0x00029F37
			public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x02000131 RID: 305
		private class FullPlacementHeightConverter : IMultiValueConverter
		{
			// Token: 0x06000D15 RID: 3349 RVA: 0x0002BD48 File Offset: 0x00029F48
			public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
			{
				double num = (double)values[0];
				Thickness thickness = (Thickness)values[1];
				return num - thickness.Top - thickness.Bottom;
			}

			// Token: 0x06000D16 RID: 3350 RVA: 0x0002BD7B File Offset: 0x00029F7B
			public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x02000132 RID: 306
		private class PlacementConverter : IValueConverter
		{
			// Token: 0x06000D18 RID: 3352 RVA: 0x0002BD8A File Offset: 0x00029F8A
			public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
			{
				return (CustomPlacementMode)value;
			}

			// Token: 0x06000D19 RID: 3353 RVA: 0x0002BD97 File Offset: 0x00029F97
			public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			{
				throw new NotImplementedException();
			}
		}
	}
}
