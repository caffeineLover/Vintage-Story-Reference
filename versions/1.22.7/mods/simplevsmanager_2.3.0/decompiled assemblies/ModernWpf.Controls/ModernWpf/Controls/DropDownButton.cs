using System;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using ModernWpf.Automation.Peers;
using ModernWpf.Controls.Primitives;

namespace ModernWpf.Controls
{
	// Token: 0x0200003F RID: 63
	public class DropDownButton : Button
	{
		// Token: 0x06000260 RID: 608 RVA: 0x00008414 File Offset: 0x00006614
		static DropDownButton()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DropDownButton), new FrameworkPropertyMetadata(typeof(DropDownButton)));
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000262 RID: 610 RVA: 0x000084C1 File Offset: 0x000066C1
		// (set) Token: 0x06000263 RID: 611 RVA: 0x000084D3 File Offset: 0x000066D3
		public CornerRadius CornerRadius
		{
			get
			{
				return (CornerRadius)base.GetValue(DropDownButton.CornerRadiusProperty);
			}
			set
			{
				base.SetValue(DropDownButton.CornerRadiusProperty, value);
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000264 RID: 612 RVA: 0x000084E6 File Offset: 0x000066E6
		// (set) Token: 0x06000265 RID: 613 RVA: 0x000084F8 File Offset: 0x000066F8
		public bool UseSystemFocusVisuals
		{
			get
			{
				return (bool)base.GetValue(DropDownButton.UseSystemFocusVisualsProperty);
			}
			set
			{
				base.SetValue(DropDownButton.UseSystemFocusVisualsProperty, value);
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000266 RID: 614 RVA: 0x0000850B File Offset: 0x0000670B
		// (set) Token: 0x06000267 RID: 615 RVA: 0x0000851D File Offset: 0x0000671D
		public Thickness FocusVisualMargin
		{
			get
			{
				return (Thickness)base.GetValue(DropDownButton.FocusVisualMarginProperty);
			}
			set
			{
				base.SetValue(DropDownButton.FocusVisualMarginProperty, value);
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000268 RID: 616 RVA: 0x00008530 File Offset: 0x00006730
		// (set) Token: 0x06000269 RID: 617 RVA: 0x00008542 File Offset: 0x00006742
		public FlyoutBase Flyout
		{
			get
			{
				return (FlyoutBase)base.GetValue(DropDownButton.FlyoutProperty);
			}
			set
			{
				base.SetValue(DropDownButton.FlyoutProperty, value);
			}
		}

		// Token: 0x0600026A RID: 618 RVA: 0x00008550 File Offset: 0x00006750
		private static void OnFlyoutPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DropDownButton)d).OnFlyoutPropertyChanged(e);
		}

		// Token: 0x0600026B RID: 619 RVA: 0x00008560 File Offset: 0x00006760
		private void OnFlyoutPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			FlyoutBase flyoutBase = e.OldValue as FlyoutBase;
			if (flyoutBase != null)
			{
				flyoutBase.Opened -= new EventHandler<object>(this.OnFlyoutOpened);
				flyoutBase.Closed -= new EventHandler<object>(this.OnFlyoutClosed);
			}
			FlyoutBase flyoutBase2 = e.NewValue as FlyoutBase;
			if (flyoutBase2 != null)
			{
				flyoutBase2.Opened += new EventHandler<object>(this.OnFlyoutOpened);
				flyoutBase2.Closed += new EventHandler<object>(this.OnFlyoutClosed);
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600026C RID: 620 RVA: 0x000085D5 File Offset: 0x000067D5
		internal bool IsFlyoutOpen
		{
			get
			{
				return this.m_isFlyoutOpen;
			}
		}

		// Token: 0x0600026D RID: 621 RVA: 0x000085DD File Offset: 0x000067DD
		internal void OpenFlyout()
		{
			FlyoutBase flyout = this.Flyout;
			if (flyout == null)
			{
				return;
			}
			flyout.ShowAt(this);
		}

		// Token: 0x0600026E RID: 622 RVA: 0x000085F0 File Offset: 0x000067F0
		internal void CloseFlyout()
		{
			FlyoutBase flyout = this.Flyout;
			if (flyout == null)
			{
				return;
			}
			flyout.Hide();
		}

		// Token: 0x0600026F RID: 623 RVA: 0x00008602 File Offset: 0x00006802
		private void OnFlyoutOpened(object sender, object e)
		{
			this.m_isFlyoutOpen = true;
			SharedHelpers.RaiseAutomationPropertyChangedEvent(this, 0, 1);
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000861D File Offset: 0x0000681D
		private void OnFlyoutClosed(object sender, object e)
		{
			this.m_isFlyoutOpen = false;
			SharedHelpers.RaiseAutomationPropertyChangedEvent(this, 1, 0);
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00008638 File Offset: 0x00006838
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new DropDownButtonAutomationPeer(this);
		}

		// Token: 0x040000EC RID: 236
		public static readonly DependencyProperty CornerRadiusProperty = ControlHelper.CornerRadiusProperty.AddOwner(typeof(DropDownButton));

		// Token: 0x040000ED RID: 237
		public static readonly DependencyProperty UseSystemFocusVisualsProperty = FocusVisualHelper.UseSystemFocusVisualsProperty.AddOwner(typeof(DropDownButton));

		// Token: 0x040000EE RID: 238
		public static readonly DependencyProperty FocusVisualMarginProperty = FocusVisualHelper.FocusVisualMarginProperty.AddOwner(typeof(DropDownButton));

		// Token: 0x040000EF RID: 239
		public static readonly DependencyProperty FlyoutProperty = FlyoutService.FlyoutProperty.AddOwner(typeof(DropDownButton), new FrameworkPropertyMetadata(new PropertyChangedCallback(DropDownButton.OnFlyoutPropertyChanged)));

		// Token: 0x040000F0 RID: 240
		private bool m_isFlyoutOpen;
	}
}
