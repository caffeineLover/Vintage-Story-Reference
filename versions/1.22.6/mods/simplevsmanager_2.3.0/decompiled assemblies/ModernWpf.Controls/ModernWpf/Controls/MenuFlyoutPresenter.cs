using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using ModernWpf.Controls.Primitives;

namespace ModernWpf.Controls
{
	// Token: 0x02000052 RID: 82
	public class MenuFlyoutPresenter : ContextMenu
	{
		// Token: 0x06000313 RID: 787 RVA: 0x0000A2C0 File Offset: 0x000084C0
		static MenuFlyoutPresenter()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(MenuFlyoutPresenter), new FrameworkPropertyMetadata(typeof(MenuFlyoutPresenter)));
			ContextMenu.IsOpenProperty.OverrideMetadata(typeof(MenuFlyoutPresenter), new FrameworkPropertyMetadata(new PropertyChangedCallback(MenuFlyoutPresenter.OnIsOpenChanged)));
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000315 RID: 789 RVA: 0x0000A364 File Offset: 0x00008564
		// (set) Token: 0x06000316 RID: 790 RVA: 0x0000A376 File Offset: 0x00008576
		public CornerRadius CornerRadius
		{
			get
			{
				return (CornerRadius)base.GetValue(MenuFlyoutPresenter.CornerRadiusProperty);
			}
			set
			{
				base.SetValue(MenuFlyoutPresenter.CornerRadiusProperty, value);
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000317 RID: 791 RVA: 0x0000A389 File Offset: 0x00008589
		// (set) Token: 0x06000318 RID: 792 RVA: 0x0000A39B File Offset: 0x0000859B
		public bool IsDefaultShadowEnabled
		{
			get
			{
				return (bool)base.GetValue(MenuFlyoutPresenter.IsDefaultShadowEnabledProperty);
			}
			set
			{
				base.SetValue(MenuFlyoutPresenter.IsDefaultShadowEnabledProperty, value);
			}
		}

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06000319 RID: 793 RVA: 0x0000A3B0 File Offset: 0x000085B0
		// (remove) Token: 0x0600031A RID: 794 RVA: 0x0000A3E8 File Offset: 0x000085E8
		internal event EventHandler<DependencyPropertyChangedEventArgs> IsOpenChanged;

		// Token: 0x0600031B RID: 795 RVA: 0x0000A41D File Offset: 0x0000861D
		protected override void OnVisualParentChanged(DependencyObject oldParent)
		{
			base.OnVisualParentChanged(oldParent);
			if (this._parentPopup == null)
			{
				this.HookupParentPopup();
			}
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0000A434 File Offset: 0x00008634
		internal void SetOwningFlyout(MenuFlyout owningFlyout)
		{
			this.m_owningFlyout = new WeakReference<MenuFlyout>(owningFlyout);
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0000A444 File Offset: 0x00008644
		internal void UpdatePopupAnimation()
		{
			MenuFlyout menuFlyout;
			if (this._parentPopup != null && this.m_owningFlyout.TryGetTarget(ref menuFlyout))
			{
				if (menuFlyout.AreOpenCloseAnimationsEnabled)
				{
					this._parentPopup.Resources.Remove(SystemParameters.MenuPopupAnimationKey);
					return;
				}
				this._parentPopup.Resources[SystemParameters.MenuPopupAnimationKey] = 0;
			}
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0000A4A1 File Offset: 0x000086A1
		private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((MenuFlyoutPresenter)d).OnIsOpenChanged(e);
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0000A4AF File Offset: 0x000086AF
		private void OnIsOpenChanged(DependencyPropertyChangedEventArgs e)
		{
			EventHandler<DependencyPropertyChangedEventArgs> isOpenChanged = this.IsOpenChanged;
			if (isOpenChanged != null)
			{
				isOpenChanged.Invoke(this, e);
			}
			if ((bool)e.NewValue && this._parentPopup == null)
			{
				this.HookupParentPopup();
			}
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0000A4E0 File Offset: 0x000086E0
		private void HookupParentPopup()
		{
			this._parentPopup = (base.Parent as Popup);
			if (this._parentPopup != null)
			{
				this._parentPopup.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(this.HandlePopupMouseButtonEvent);
				this._parentPopup.PreviewMouseRightButtonDown += new MouseButtonEventHandler(this.HandlePopupMouseButtonEvent);
				this._parentPopup.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(this.HandlePopupMouseButtonEvent);
				this._parentPopup.PreviewMouseRightButtonUp += new MouseButtonEventHandler(this.HandlePopupMouseButtonEvent);
				this.UpdatePopupAnimation();
			}
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0000A568 File Offset: 0x00008768
		private void HandlePopupMouseButtonEvent(object sender, MouseButtonEventArgs e)
		{
			if (!this._parentPopup.IsOpen)
			{
				e.Handled = true;
			}
		}

		// Token: 0x0400011D RID: 285
		public static readonly DependencyProperty CornerRadiusProperty = ControlHelper.CornerRadiusProperty.AddOwner(typeof(MenuFlyoutPresenter));

		// Token: 0x0400011E RID: 286
		public static readonly DependencyProperty IsDefaultShadowEnabledProperty = DependencyProperty.Register("IsDefaultShadowEnabled", typeof(bool), typeof(MenuFlyoutPresenter), new PropertyMetadata(true));

		// Token: 0x04000120 RID: 288
		private Popup _parentPopup;

		// Token: 0x04000121 RID: 289
		private WeakReference<MenuFlyout> m_owningFlyout;
	}
}
