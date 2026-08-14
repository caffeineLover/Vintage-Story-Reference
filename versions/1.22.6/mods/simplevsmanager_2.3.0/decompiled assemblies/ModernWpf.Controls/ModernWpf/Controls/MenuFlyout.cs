using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using ModernWpf.Controls.Primitives;

namespace ModernWpf.Controls
{
	// Token: 0x02000051 RID: 81
	[ContentProperty("Items")]
	public class MenuFlyout : FlyoutBase
	{
		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000301 RID: 769 RVA: 0x00009FB1 File Offset: 0x000081B1
		public ItemCollection Items
		{
			get
			{
				this.EnsurePresenter();
				return this.m_presenter.Items;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000302 RID: 770 RVA: 0x00009FC4 File Offset: 0x000081C4
		// (set) Token: 0x06000303 RID: 771 RVA: 0x00009FD6 File Offset: 0x000081D6
		public Style MenuFlyoutPresenterStyle
		{
			get
			{
				return (Style)base.GetValue(MenuFlyout.MenuFlyoutPresenterStyleProperty);
			}
			set
			{
				base.SetValue(MenuFlyout.MenuFlyoutPresenterStyleProperty, value);
			}
		}

		// Token: 0x06000304 RID: 772 RVA: 0x00009FE4 File Offset: 0x000081E4
		private static void OnMenuFlyoutPresenterStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((MenuFlyout)d).OnMenuFlyoutPresenterStyleChanged(e);
		}

		// Token: 0x06000305 RID: 773 RVA: 0x00009FF2 File Offset: 0x000081F2
		private void OnMenuFlyoutPresenterStyleChanged(DependencyPropertyChangedEventArgs e)
		{
			if (this.m_presenter != null)
			{
				this.m_presenter.Style = (Style)e.NewValue;
			}
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000A013 File Offset: 0x00008213
		protected override Control CreatePresenter()
		{
			throw new InvalidOperationException();
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000A01A File Offset: 0x0000821A
		internal override void ShowAtCore(FrameworkElement placementTarget, bool showAsContextFlyout = false)
		{
			if (showAsContextFlyout)
			{
				this.Show(placementTarget, 8);
				return;
			}
			this.Show(placementTarget, 11);
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000A031 File Offset: 0x00008231
		internal override void HideCore()
		{
			if (this.m_presenter != null && this.m_presenter.IsOpen)
			{
				this.m_presenter.IsOpen = false;
			}
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000A054 File Offset: 0x00008254
		internal override void OnIsOpenChanged()
		{
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000A056 File Offset: 0x00008256
		internal override void UpdateIsOpen()
		{
			base.IsOpen = (this.m_presenter != null && this.m_presenter.IsOpen);
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000A074 File Offset: 0x00008274
		internal override void OnAreOpenCloseAnimationsEnabledChanged(DependencyPropertyChangedEventArgs e)
		{
			MenuFlyoutPresenter presenter = this.m_presenter;
			if (presenter == null)
			{
				return;
			}
			presenter.UpdatePopupAnimation();
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000A088 File Offset: 0x00008288
		private void Show(FrameworkElement placementTarget, PlacementMode placement = 11)
		{
			if (this.m_presenter != null && this.m_presenter.IsOpen && this.m_presenter.PlacementTarget == placementTarget && this.m_presenter.Placement == placement)
			{
				FlyoutPlacementMode? currentPlacement = this.m_currentPlacement;
				FlyoutPlacementMode placement2 = base.Placement;
				if (currentPlacement.GetValueOrDefault() == placement2 & currentPlacement != null)
				{
					return;
				}
			}
			this.EnsurePresenter();
			if (this.m_presenter.IsOpen)
			{
				this.m_presenter.IsOpen = false;
			}
			this.m_presenter.Placement = placement;
			this.m_presenter.PlacementTarget = placementTarget;
			if (placement == 11)
			{
				this.m_presenter.PlacementRectangle = base.GetPlacementRectangle(placementTarget);
			}
			else
			{
				this.m_presenter.ClearValue(Popup.PlacementRectangleProperty);
			}
			this.m_currentPlacement = new FlyoutPlacementMode?(base.Placement);
			this.OnOpening();
			this.m_presenter.IsOpen = true;
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000A16C File Offset: 0x0000836C
		private CustomPopupPlacement[] PositionPopup(Size popupSize, Size targetSize, Point offset)
		{
			return base.PositionPopup(popupSize, targetSize, offset, this.m_presenter);
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000A180 File Offset: 0x00008380
		private void EnsurePresenter()
		{
			if (this.m_presenter == null)
			{
				MenuFlyoutPresenter menuFlyoutPresenter = new MenuFlyoutPresenter
				{
					Style = this.MenuFlyoutPresenterStyle,
					Placement = 11,
					CustomPopupPlacementCallback = new CustomPopupPlacementCallback(this.PositionPopup),
					StaysOpen = false
				};
				menuFlyoutPresenter.SetOwningFlyout(this);
				base.BindPlacement(menuFlyoutPresenter);
				menuFlyoutPresenter.UpdatePopupAnimation();
				menuFlyoutPresenter.Opened += new RoutedEventHandler(this.OnPresenterOpened);
				menuFlyoutPresenter.Closed += new RoutedEventHandler(this.OnPresenterClosed);
				menuFlyoutPresenter.IsOpenChanged += new EventHandler<DependencyPropertyChangedEventArgs>(this.OnPresenterIsOpenChanged);
				this.m_presenter = menuFlyoutPresenter;
			}
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000A21C File Offset: 0x0000841C
		private void OnPresenterOpened(object sender, RoutedEventArgs e)
		{
			this.OnOpened();
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000A224 File Offset: 0x00008424
		private void OnPresenterClosed(object sender, RoutedEventArgs e)
		{
			if (!this.m_presenter.IsOpen)
			{
				this.m_presenter.ClearValue(ContextMenu.PlacementProperty);
				this.m_presenter.ClearValue(ContextMenu.PlacementTargetProperty);
				this.m_presenter.ClearValue(ContextMenu.PlacementRectangleProperty);
				this.m_currentPlacement = default(FlyoutPlacementMode?);
			}
			this.OnClosed();
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0000A280 File Offset: 0x00008480
		private void OnPresenterIsOpenChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.UpdateIsOpen();
		}

		// Token: 0x0400011A RID: 282
		public static readonly DependencyProperty MenuFlyoutPresenterStyleProperty = DependencyProperty.Register("MenuFlyoutPresenterStyle", typeof(Style), typeof(MenuFlyout), new PropertyMetadata(new PropertyChangedCallback(MenuFlyout.OnMenuFlyoutPresenterStyleChanged)));

		// Token: 0x0400011B RID: 283
		private MenuFlyoutPresenter m_presenter;

		// Token: 0x0400011C RID: 284
		private FlyoutPlacementMode? m_currentPlacement;
	}
}
