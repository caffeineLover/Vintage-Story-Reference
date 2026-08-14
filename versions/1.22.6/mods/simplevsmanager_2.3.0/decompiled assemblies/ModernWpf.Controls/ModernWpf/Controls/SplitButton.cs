using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using ModernWpf.Automation.Peers;
using ModernWpf.Controls.Primitives;

namespace ModernWpf.Controls
{
	// Token: 0x020000D5 RID: 213
	public class SplitButton : ContentControl, ICommandSource
	{
		// Token: 0x06000A92 RID: 2706 RVA: 0x00024430 File Offset: 0x00022630
		static SplitButton()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(SplitButton), new FrameworkPropertyMetadata(typeof(SplitButton)));
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x00024638 File Offset: 0x00022838
		public SplitButton()
		{
			base.KeyDown += new KeyEventHandler(this.OnSplitButtonKeyDown);
			base.KeyUp += new KeyEventHandler(this.OnSplitButtonKeyUp);
			base.InputBindings.Add(new KeyBinding(new SplitButton.OpenFlyoutCommand(this), 26, 1));
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000A94 RID: 2708 RVA: 0x00024694 File Offset: 0x00022894
		// (set) Token: 0x06000A95 RID: 2709 RVA: 0x000246A6 File Offset: 0x000228A6
		public CornerRadius CornerRadius
		{
			get
			{
				return (CornerRadius)base.GetValue(SplitButton.CornerRadiusProperty);
			}
			set
			{
				base.SetValue(SplitButton.CornerRadiusProperty, value);
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000A96 RID: 2710 RVA: 0x000246B9 File Offset: 0x000228B9
		// (set) Token: 0x06000A97 RID: 2711 RVA: 0x000246CB File Offset: 0x000228CB
		public bool UseSystemFocusVisuals
		{
			get
			{
				return (bool)base.GetValue(SplitButton.UseSystemFocusVisualsProperty);
			}
			set
			{
				base.SetValue(SplitButton.UseSystemFocusVisualsProperty, value);
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000A98 RID: 2712 RVA: 0x000246DE File Offset: 0x000228DE
		// (set) Token: 0x06000A99 RID: 2713 RVA: 0x000246F0 File Offset: 0x000228F0
		public Thickness FocusVisualMargin
		{
			get
			{
				return (Thickness)base.GetValue(SplitButton.FocusVisualMarginProperty);
			}
			set
			{
				base.SetValue(SplitButton.FocusVisualMarginProperty, value);
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000A9A RID: 2714 RVA: 0x00024703 File Offset: 0x00022903
		// (set) Token: 0x06000A9B RID: 2715 RVA: 0x00024715 File Offset: 0x00022915
		public ICommand Command
		{
			get
			{
				return (ICommand)base.GetValue(SplitButton.CommandProperty);
			}
			set
			{
				base.SetValue(SplitButton.CommandProperty, value);
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000A9C RID: 2716 RVA: 0x00024723 File Offset: 0x00022923
		// (set) Token: 0x06000A9D RID: 2717 RVA: 0x00024730 File Offset: 0x00022930
		public object CommandParameter
		{
			get
			{
				return base.GetValue(SplitButton.CommandParameterProperty);
			}
			set
			{
				base.SetValue(SplitButton.CommandParameterProperty, value);
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000A9E RID: 2718 RVA: 0x0002473E File Offset: 0x0002293E
		// (set) Token: 0x06000A9F RID: 2719 RVA: 0x00024750 File Offset: 0x00022950
		public IInputElement CommandTarget
		{
			get
			{
				return (IInputElement)base.GetValue(SplitButton.CommandTargetProperty);
			}
			set
			{
				base.SetValue(SplitButton.CommandTargetProperty, value);
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000AA0 RID: 2720 RVA: 0x0002475E File Offset: 0x0002295E
		// (set) Token: 0x06000AA1 RID: 2721 RVA: 0x00024770 File Offset: 0x00022970
		public FlyoutBase Flyout
		{
			get
			{
				return (FlyoutBase)base.GetValue(SplitButton.FlyoutProperty);
			}
			set
			{
				base.SetValue(SplitButton.FlyoutProperty, value);
			}
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x0002477E File Offset: 0x0002297E
		private static void OnFlyoutChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((SplitButton)d).OnFlyoutChanged((FlyoutBase)e.OldValue, (FlyoutBase)e.NewValue);
		}

		// Token: 0x14000036 RID: 54
		// (add) Token: 0x06000AA3 RID: 2723 RVA: 0x000247A4 File Offset: 0x000229A4
		// (remove) Token: 0x06000AA4 RID: 2724 RVA: 0x000247DC File Offset: 0x000229DC
		public event TypedEventHandler<SplitButton, SplitButtonClickEventArgs> Click;

		// Token: 0x06000AA5 RID: 2725 RVA: 0x00024814 File Offset: 0x00022A14
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.UnregisterEvents();
			this.m_primaryButton = (base.GetTemplateChild("PrimaryButton") as Button);
			this.m_secondaryButton = (base.GetTemplateChild("SecondaryButton") as Button);
			if (this.m_primaryButton != null)
			{
				this.m_primaryButton.Click += new RoutedEventHandler(this.OnClickPrimary);
				this.SetBinding(SplitButton.PrimaryButtonIsPressedProperty, ButtonBase.IsPressedProperty, this.m_primaryButton);
				this.SetBinding(SplitButton.PrimaryButtonIsMouseOverProperty, UIElement.IsMouseOverProperty, this.m_primaryButton);
			}
			if (this.m_secondaryButton != null)
			{
				string localizedStringResource = SplitButton.ResourceAccessor.GetLocalizedStringResource("SplitButtonSecondaryButtonName");
				AutomationProperties.SetName(this.m_secondaryButton, localizedStringResource);
				this.m_secondaryButton.Click += new RoutedEventHandler(this.OnClickSecondary);
				this.SetBinding(SplitButton.SecondaryButtonIsPressedProperty, ButtonBase.IsPressedProperty, this.m_secondaryButton);
				this.SetBinding(SplitButton.SecondaryButtonIsMouseOverProperty, UIElement.IsMouseOverProperty, this.m_secondaryButton);
			}
			this.UpdateVisualStates(true);
			this.m_hasLoaded = true;
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x0002491E File Offset: 0x00022B1E
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new SplitButtonAutomationPeer(this);
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x00024926 File Offset: 0x00022B26
		private void OnFlyoutChanged(FlyoutBase oldFlyout, FlyoutBase newFlyout)
		{
			this.RegisterFlyoutEvents(oldFlyout, newFlyout);
			this.UpdateVisualStates(true);
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x00024938 File Offset: 0x00022B38
		private void RegisterFlyoutEvents(FlyoutBase oldFlyout, FlyoutBase newFlyout)
		{
			if (oldFlyout != null)
			{
				oldFlyout.Opened -= new EventHandler<object>(this.OnFlyoutOpened);
				oldFlyout.Closed -= new EventHandler<object>(this.OnFlyoutClosed);
				base.ClearValue(SplitButton.FlyoutPlacementProperty);
			}
			if (newFlyout != null)
			{
				newFlyout.Opened += new EventHandler<object>(this.OnFlyoutOpened);
				newFlyout.Closed += new EventHandler<object>(this.OnFlyoutClosed);
				this.SetBinding(SplitButton.FlyoutPlacementProperty, FlyoutBase.PlacementProperty, newFlyout);
			}
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x000249B0 File Offset: 0x00022BB0
		private static void OnVisualPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((SplitButton)d).UpdateVisualStates(true);
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x000249C0 File Offset: 0x00022BC0
		internal void UpdateVisualStates(bool useTransitions = true)
		{
			if (this.m_isKeyDown)
			{
				VisualStateManager.GoToState(this, "SecondaryButtonSpan", useTransitions);
			}
			else
			{
				VisualStateManager.GoToState(this, "SecondaryButtonRight", useTransitions);
			}
			Button primaryButton = this.m_primaryButton;
			Button secondaryButton = this.m_secondaryButton;
			if (primaryButton != null && this.m_secondaryButton != null)
			{
				if (this.m_isFlyoutOpen)
				{
					VisualStateManager.GoToState(this, "FlyoutOpen", useTransitions);
					return;
				}
				if (this.InternalIsChecked)
				{
					if (this.m_isKeyDown)
					{
						if (primaryButton.IsPressed || secondaryButton.IsPressed || this.m_isKeyDown)
						{
							VisualStateManager.GoToState(this, "CheckedTouchPressed", useTransitions);
							return;
						}
						VisualStateManager.GoToState(this, "Checked", useTransitions);
						return;
					}
					else
					{
						if (primaryButton.IsPressed)
						{
							VisualStateManager.GoToState(this, "CheckedPrimaryPressed", useTransitions);
							return;
						}
						if (primaryButton.IsMouseOver)
						{
							VisualStateManager.GoToState(this, "CheckedPrimaryPointerOver", useTransitions);
							return;
						}
						if (secondaryButton.IsPressed)
						{
							VisualStateManager.GoToState(this, "CheckedSecondaryPressed", useTransitions);
							return;
						}
						if (secondaryButton.IsMouseOver)
						{
							VisualStateManager.GoToState(this, "CheckedSecondaryPointerOver", useTransitions);
							return;
						}
						VisualStateManager.GoToState(this, "Checked", useTransitions);
						return;
					}
				}
				else if (this.m_isKeyDown)
				{
					if (primaryButton.IsPressed || secondaryButton.IsPressed || this.m_isKeyDown)
					{
						VisualStateManager.GoToState(this, "TouchPressed", useTransitions);
						return;
					}
					VisualStateManager.GoToState(this, "Normal", useTransitions);
					return;
				}
				else
				{
					if (primaryButton.IsPressed)
					{
						VisualStateManager.GoToState(this, "PrimaryPressed", useTransitions);
						return;
					}
					if (primaryButton.IsMouseOver)
					{
						VisualStateManager.GoToState(this, "PrimaryPointerOver", useTransitions);
						return;
					}
					if (secondaryButton.IsPressed)
					{
						VisualStateManager.GoToState(this, "SecondaryPressed", useTransitions);
						return;
					}
					if (secondaryButton.IsMouseOver)
					{
						VisualStateManager.GoToState(this, "SecondaryPointerOver", useTransitions);
						return;
					}
					VisualStateManager.GoToState(this, "Normal", useTransitions);
				}
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000AAB RID: 2731 RVA: 0x00024B74 File Offset: 0x00022D74
		internal bool IsFlyoutOpen
		{
			get
			{
				return this.m_isFlyoutOpen;
			}
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x00024B7C File Offset: 0x00022D7C
		internal void OpenFlyout()
		{
			FlyoutBase flyout = this.Flyout;
			if (flyout != null)
			{
				flyout.ShowAt(this);
			}
		}

		// Token: 0x06000AAD RID: 2733 RVA: 0x00024B9C File Offset: 0x00022D9C
		internal void CloseFlyout()
		{
			FlyoutBase flyout = this.Flyout;
			if (flyout != null)
			{
				flyout.Hide();
			}
		}

		// Token: 0x06000AAE RID: 2734 RVA: 0x00024BBC File Offset: 0x00022DBC
		internal virtual void OnClickPrimary(object sender, RoutedEventArgs e)
		{
			TypedEventHandler<SplitButton, SplitButtonClickEventArgs> click = this.Click;
			if (click != null)
			{
				click.Invoke(this, new SplitButtonClickEventArgs());
			}
			AutomationPeer automationPeer = UIElementAutomationPeer.FromElement(this);
			if (automationPeer != null)
			{
				automationPeer.RaiseAutomationEvent(5);
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000AAF RID: 2735 RVA: 0x00024BF1 File Offset: 0x00022DF1
		internal virtual bool InternalIsChecked
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x00024BF4 File Offset: 0x00022DF4
		private void OnFlyoutOpened(object sender, object e)
		{
			this.m_isFlyoutOpen = true;
			this.UpdateVisualStates(true);
			SharedHelpers.RaiseAutomationPropertyChangedEvent(this, 0, 1);
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x00024C16 File Offset: 0x00022E16
		private void OnFlyoutClosed(object sender, object e)
		{
			this.m_isFlyoutOpen = false;
			this.UpdateVisualStates(true);
			SharedHelpers.RaiseAutomationPropertyChangedEvent(this, 1, 0);
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x00024C38 File Offset: 0x00022E38
		private static void OnFlyoutPlacementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((SplitButton)d).UpdateVisualStates(true);
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x00024C46 File Offset: 0x00022E46
		private void OnClickSecondary(object sender, RoutedEventArgs e)
		{
			this.OpenFlyout();
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x00024C50 File Offset: 0x00022E50
		private void OnSplitButtonKeyDown(object sender, KeyEventArgs args)
		{
			Key key = args.Key;
			if (key == 18 || key == 6)
			{
				this.m_isKeyDown = true;
				this.UpdateVisualStates(true);
			}
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x00024C7C File Offset: 0x00022E7C
		private void OnSplitButtonKeyUp(object sender, KeyEventArgs args)
		{
			Key key = args.Key;
			if (key == 18 || key == 6)
			{
				this.m_isKeyDown = false;
				this.UpdateVisualStates(true);
				if (base.IsEnabled)
				{
					this.OnClickPrimary(null, null);
					args.Handled = true;
					return;
				}
			}
			else if (key == 93 && base.IsEnabled)
			{
				this.OpenFlyout();
				args.Handled = true;
			}
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x00024CDC File Offset: 0x00022EDC
		private void UnregisterEvents()
		{
			if (this.m_primaryButton != null)
			{
				this.m_primaryButton.Click -= new RoutedEventHandler(this.OnClickPrimary);
				base.ClearValue(SplitButton.PrimaryButtonIsPressedProperty);
				base.ClearValue(SplitButton.PrimaryButtonIsMouseOverProperty);
			}
			if (this.m_secondaryButton != null)
			{
				this.m_secondaryButton.Click -= new RoutedEventHandler(this.OnClickSecondary);
				base.ClearValue(SplitButton.SecondaryButtonIsPressedProperty);
				base.ClearValue(SplitButton.SecondaryButtonIsMouseOverProperty);
			}
		}

		// Token: 0x04000448 RID: 1096
		private static readonly ResourceAccessor ResourceAccessor = new ResourceAccessor(typeof(SplitButton));

		// Token: 0x04000449 RID: 1097
		public static readonly DependencyProperty CornerRadiusProperty = ControlHelper.CornerRadiusProperty.AddOwner(typeof(SplitButton));

		// Token: 0x0400044A RID: 1098
		public static readonly DependencyProperty UseSystemFocusVisualsProperty = FocusVisualHelper.UseSystemFocusVisualsProperty.AddOwner(typeof(SplitButton));

		// Token: 0x0400044B RID: 1099
		public static readonly DependencyProperty FocusVisualMarginProperty = FocusVisualHelper.FocusVisualMarginProperty.AddOwner(typeof(SplitButton));

		// Token: 0x0400044C RID: 1100
		public static readonly DependencyProperty CommandProperty = ButtonBase.CommandProperty.AddOwner(typeof(SplitButton));

		// Token: 0x0400044D RID: 1101
		public static readonly DependencyProperty CommandParameterProperty = ButtonBase.CommandParameterProperty.AddOwner(typeof(SplitButton));

		// Token: 0x0400044E RID: 1102
		public static readonly DependencyProperty CommandTargetProperty = ButtonBase.CommandTargetProperty.AddOwner(typeof(SplitButton));

		// Token: 0x0400044F RID: 1103
		public static readonly DependencyProperty FlyoutProperty = DependencyProperty.Register("Flyout", typeof(FlyoutBase), typeof(SplitButton), new PropertyMetadata(new PropertyChangedCallback(SplitButton.OnFlyoutChanged)));

		// Token: 0x04000450 RID: 1104
		private static readonly DependencyProperty PrimaryButtonIsPressedProperty = DependencyProperty.Register("PrimaryButtonIsPressed", typeof(bool), typeof(SplitButton), new FrameworkPropertyMetadata(new PropertyChangedCallback(SplitButton.OnVisualPropertyChanged)));

		// Token: 0x04000451 RID: 1105
		private static readonly DependencyProperty PrimaryButtonIsMouseOverProperty = DependencyProperty.Register("PrimaryButtonIsMouseOver", typeof(bool), typeof(SplitButton), new FrameworkPropertyMetadata(new PropertyChangedCallback(SplitButton.OnVisualPropertyChanged)));

		// Token: 0x04000452 RID: 1106
		private static readonly DependencyProperty SecondaryButtonIsPressedProperty = DependencyProperty.Register("SecondaryButtonIsPressed", typeof(bool), typeof(SplitButton), new FrameworkPropertyMetadata(new PropertyChangedCallback(SplitButton.OnVisualPropertyChanged)));

		// Token: 0x04000453 RID: 1107
		private static readonly DependencyProperty SecondaryButtonIsMouseOverProperty = DependencyProperty.Register("SecondaryButtonIsMouseOver", typeof(bool), typeof(SplitButton), new FrameworkPropertyMetadata(new PropertyChangedCallback(SplitButton.OnVisualPropertyChanged)));

		// Token: 0x04000454 RID: 1108
		private static readonly DependencyProperty FlyoutPlacementProperty = FlyoutBase.PlacementProperty.AddOwner(typeof(SplitButton), new FrameworkPropertyMetadata(new PropertyChangedCallback(SplitButton.OnFlyoutPlacementChanged)));

		// Token: 0x04000456 RID: 1110
		internal bool m_hasLoaded;

		// Token: 0x04000457 RID: 1111
		private Button m_primaryButton;

		// Token: 0x04000458 RID: 1112
		private Button m_secondaryButton;

		// Token: 0x04000459 RID: 1113
		private bool m_isFlyoutOpen;

		// Token: 0x0400045A RID: 1114
		private bool m_isKeyDown;

		// Token: 0x0400045B RID: 1115
		private readonly CornerRadiusFilterConverter m_cornerRadiusFilterConverter = new CornerRadiusFilterConverter();

		// Token: 0x0200012B RID: 299
		private class OpenFlyoutCommand : ICommand
		{
			// Token: 0x06000D04 RID: 3332 RVA: 0x0002BAFD File Offset: 0x00029CFD
			public OpenFlyoutCommand(SplitButton owner)
			{
				this.m_owner = owner;
			}

			// Token: 0x1400004B RID: 75
			// (add) Token: 0x06000D05 RID: 3333 RVA: 0x0002BB0C File Offset: 0x00029D0C
			// (remove) Token: 0x06000D06 RID: 3334 RVA: 0x0002BB44 File Offset: 0x00029D44
			public event EventHandler CanExecuteChanged;

			// Token: 0x06000D07 RID: 3335 RVA: 0x0002BB79 File Offset: 0x00029D79
			public bool CanExecute(object parameter)
			{
				return true;
			}

			// Token: 0x06000D08 RID: 3336 RVA: 0x0002BB7C File Offset: 0x00029D7C
			public void Execute(object parameter)
			{
				this.m_owner.OpenFlyout();
			}

			// Token: 0x040005D6 RID: 1494
			private readonly SplitButton m_owner;
		}
	}
}
