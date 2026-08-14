using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using ModernWpf.Controls.Primitives;

namespace ModernWpf.Controls
{
	// Token: 0x0200001A RID: 26
	public class AppBarButton : Button, ICommandBarElement, IAppBarElement
	{
		// Token: 0x060000F7 RID: 247 RVA: 0x00004814 File Offset: 0x00002A14
		static AppBarButton()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(AppBarButton), new FrameworkPropertyMetadata(typeof(AppBarButton)));
			UIElement.IsEnabledProperty.OverrideMetadata(typeof(AppBarButton), new FrameworkPropertyMetadata(new PropertyChangedCallback(AppBarButton.OnIsEnabledChanged)));
			ButtonBase.CommandProperty.OverrideMetadata(typeof(AppBarButton), new FrameworkPropertyMetadata(new PropertyChangedCallback(AppBarButton.OnCommandPropertyChanged)));
			FrameworkElement.ToolTipProperty.OverrideMetadata(typeof(AppBarButton), new FrameworkPropertyMetadata
			{
				CoerceValueCallback = new CoerceValueCallback(AppBarElementProperties.CoerceToolTip)
			});
			ToolBar.OverflowModeProperty.OverrideMetadata(typeof(AppBarButton), new FrameworkPropertyMetadata(new PropertyChangedCallback(AppBarButton.OnOverflowModePropertyChanged)));
			CommandBarToolBar.DefaultLabelPositionProperty.OverrideMetadata(typeof(AppBarButton), new FrameworkPropertyMetadata(new PropertyChangedCallback(AppBarButton.OnDefaultLabelPositionPropertyChanged)));
			AppBarElementProperties.IsInOverflowPropertyKey.OverrideMetadata(typeof(AppBarButton), new FrameworkPropertyMetadata(new PropertyChangedCallback(AppBarButton.OnIsInOverflowChanged)));
			AppBarElementProperties.ShowKeyboardAcceleratorTextProperty.OverrideMetadata(typeof(AppBarButton), new FrameworkPropertyMetadata(new PropertyChangedCallback(AppBarButton.OnShowKeyboardAcceleratorTextPropertyChanged)));
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00004A71 File Offset: 0x00002C71
		public AppBarButton()
		{
			base.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.OnIsVisibleChanged);
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x00004A8B File Offset: 0x00002C8B
		// (set) Token: 0x060000FA RID: 250 RVA: 0x00004A9D File Offset: 0x00002C9D
		public bool UseSystemFocusVisuals
		{
			get
			{
				return (bool)base.GetValue(AppBarButton.UseSystemFocusVisualsProperty);
			}
			set
			{
				base.SetValue(AppBarButton.UseSystemFocusVisualsProperty, value);
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000FB RID: 251 RVA: 0x00004AB0 File Offset: 0x00002CB0
		// (set) Token: 0x060000FC RID: 252 RVA: 0x00004AC2 File Offset: 0x00002CC2
		public Thickness FocusVisualMargin
		{
			get
			{
				return (Thickness)base.GetValue(AppBarButton.FocusVisualMarginProperty);
			}
			set
			{
				base.SetValue(AppBarButton.FocusVisualMarginProperty, value);
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000FD RID: 253 RVA: 0x00004AD5 File Offset: 0x00002CD5
		// (set) Token: 0x060000FE RID: 254 RVA: 0x00004AE7 File Offset: 0x00002CE7
		public CornerRadius CornerRadius
		{
			get
			{
				return (CornerRadius)base.GetValue(AppBarButton.CornerRadiusProperty);
			}
			set
			{
				base.SetValue(AppBarButton.CornerRadiusProperty, value);
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000FF RID: 255 RVA: 0x00004AFA File Offset: 0x00002CFA
		// (set) Token: 0x06000100 RID: 256 RVA: 0x00004B0C File Offset: 0x00002D0C
		public FlyoutBase Flyout
		{
			get
			{
				return (FlyoutBase)base.GetValue(AppBarButton.FlyoutProperty);
			}
			set
			{
				base.SetValue(AppBarButton.FlyoutProperty, value);
			}
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00004B1A File Offset: 0x00002D1A
		private static void OnFlyoutChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((AppBarButton)d).OnFlyoutChanged();
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00004B27 File Offset: 0x00002D27
		private void OnFlyoutChanged()
		{
			this.UpdateVisualState(true);
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000103 RID: 259 RVA: 0x00004B30 File Offset: 0x00002D30
		// (set) Token: 0x06000104 RID: 260 RVA: 0x00004B42 File Offset: 0x00002D42
		public IconElement Icon
		{
			get
			{
				return (IconElement)base.GetValue(AppBarButton.IconProperty);
			}
			set
			{
				base.SetValue(AppBarButton.IconProperty, value);
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000105 RID: 261 RVA: 0x00004B50 File Offset: 0x00002D50
		// (set) Token: 0x06000106 RID: 262 RVA: 0x00004B62 File Offset: 0x00002D62
		public string Label
		{
			get
			{
				return (string)base.GetValue(AppBarButton.LabelProperty);
			}
			set
			{
				base.SetValue(AppBarButton.LabelProperty, value);
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000107 RID: 263 RVA: 0x00004B70 File Offset: 0x00002D70
		// (set) Token: 0x06000108 RID: 264 RVA: 0x00004B82 File Offset: 0x00002D82
		public CommandBarLabelPosition LabelPosition
		{
			get
			{
				return (CommandBarLabelPosition)base.GetValue(AppBarButton.LabelPositionProperty);
			}
			set
			{
				base.SetValue(AppBarButton.LabelPositionProperty, value);
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00004B95 File Offset: 0x00002D95
		// (set) Token: 0x0600010A RID: 266 RVA: 0x00004BA7 File Offset: 0x00002DA7
		public bool IsCompact
		{
			get
			{
				return (bool)base.GetValue(AppBarButton.IsCompactProperty);
			}
			set
			{
				base.SetValue(AppBarButton.IsCompactProperty, value);
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00004BBA File Offset: 0x00002DBA
		public bool IsInOverflow
		{
			get
			{
				return (bool)base.GetValue(AppBarButton.IsInOverflowProperty);
			}
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00004BCC File Offset: 0x00002DCC
		private static void OnIsInOverflowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((AppBarButton)d).UpdateCommonState(true);
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600010D RID: 269 RVA: 0x00004BDA File Offset: 0x00002DDA
		private AppBarElementApplicationViewState ApplicationViewState
		{
			get
			{
				return (AppBarElementApplicationViewState)base.GetValue(AppBarButton.ApplicationViewStateProperty);
			}
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00004BEC File Offset: 0x00002DEC
		private void UpdateApplicationViewState()
		{
			AppBarElementApplicationViewState appBarElementApplicationViewState;
			if (this.IsInOverflow && base.IsVisible)
			{
				CommandBarOverflowPanel commandBarOverflowPanel = base.VisualParent as CommandBarOverflowPanel;
				if (commandBarOverflowPanel != null)
				{
					appBarElementApplicationViewState = this.ComputeApplicationViewStateInOverflow(commandBarOverflowPanel.HasToggleButton, commandBarOverflowPanel.HasMenuIcon);
					goto IL_7D;
				}
			}
			CommandBarDefaultLabelPosition commandBarDefaultLabelPosition;
			if (base.VisualParent is ToolBarPanel)
			{
				commandBarDefaultLabelPosition = (CommandBarDefaultLabelPosition)base.GetValue(CommandBarToolBar.DefaultLabelPositionProperty);
			}
			else
			{
				commandBarDefaultLabelPosition = CommandBarDefaultLabelPosition.Bottom;
			}
			if (this.LabelPosition == CommandBarLabelPosition.Collapsed || commandBarDefaultLabelPosition == CommandBarDefaultLabelPosition.Collapsed)
			{
				appBarElementApplicationViewState = AppBarElementApplicationViewState.LabelCollapsed;
			}
			else if (commandBarDefaultLabelPosition == CommandBarDefaultLabelPosition.Right)
			{
				appBarElementApplicationViewState = AppBarElementApplicationViewState.LabelOnRight;
			}
			else if (this.IsCompact)
			{
				appBarElementApplicationViewState = AppBarElementApplicationViewState.Compact;
			}
			else
			{
				appBarElementApplicationViewState = AppBarElementApplicationViewState.FullSize;
			}
			IL_7D:
			base.SetValue(AppBarElementProperties.ApplicationViewStatePropertyKey, appBarElementApplicationViewState);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00004C87 File Offset: 0x00002E87
		private AppBarElementApplicationViewState ComputeApplicationViewStateInOverflow(bool hasToggleButton, bool hasMenuIcon)
		{
			if (hasToggleButton && hasMenuIcon)
			{
				return AppBarElementApplicationViewState.OverflowWithToggleButtonsAndMenuIcons;
			}
			if (hasToggleButton)
			{
				return AppBarElementApplicationViewState.OverflowWithToggleButtons;
			}
			if (hasMenuIcon)
			{
				return AppBarElementApplicationViewState.OverflowWithMenuIcons;
			}
			return AppBarElementApplicationViewState.Overflow;
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00004C9C File Offset: 0x00002E9C
		private void ApplyApplicationViewState(bool useTransitions = true)
		{
			VisualStateManager.GoToState(this, this.ApplicationViewState.ToString(), useTransitions);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00004CC5 File Offset: 0x00002EC5
		void IAppBarElement.UpdateApplicationViewState()
		{
			this.UpdateApplicationViewState();
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00004CCD File Offset: 0x00002ECD
		void IAppBarElement.ApplyApplicationViewState()
		{
			this.ApplyApplicationViewState(true);
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000113 RID: 275 RVA: 0x00004CD6 File Offset: 0x00002ED6
		// (set) Token: 0x06000114 RID: 276 RVA: 0x00004CE8 File Offset: 0x00002EE8
		public string InputGestureText
		{
			get
			{
				return (string)base.GetValue(AppBarButton.InputGestureTextProperty);
			}
			set
			{
				base.SetValue(AppBarButton.InputGestureTextProperty, value);
			}
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00004CF8 File Offset: 0x00002EF8
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			FrameworkElement templateRoot = ControlExtensions.GetTemplateRoot(this);
			if (templateRoot != null)
			{
				this._vsm = new AppBarElementVisualStateManager();
				VisualStateManager.SetCustomVisualStateManager(templateRoot, this._vsm);
			}
			this.UpdateVisualState(false);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00004D33 File Offset: 0x00002F33
		protected override void OnVisualParentChanged(DependencyObject oldParent)
		{
			base.OnVisualParentChanged(oldParent);
			this.UpdateApplicationViewState();
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00004D42 File Offset: 0x00002F42
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == UIElement.IsMouseOverProperty)
			{
				this.UpdateCommonState(true);
				return;
			}
			if (e.Property == ToolBar.IsOverflowItemProperty)
			{
				AppBarElementProperties.UpdateIsInOverflow(this);
			}
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00004D75 File Offset: 0x00002F75
		protected override void OnIsPressedChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnIsPressedChanged(e);
			this.UpdateCommonState(true);
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00004D85 File Offset: 0x00002F85
		private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((AppBarButton)d).UpdateCommonState(true);
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00004D93 File Offset: 0x00002F93
		private static void OnCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			AppBarButton appBarButton = (AppBarButton)d;
			appBarButton.CoerceValue(AppBarButton.LabelProperty);
			appBarButton.CoerceValue(AppBarButton.InputGestureTextProperty);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00004DB0 File Offset: 0x00002FB0
		private static void OnOverflowModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			AppBarElementProperties.UpdateIsInOverflow(d);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00004DB8 File Offset: 0x00002FB8
		private static void OnDefaultLabelPositionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((AppBarButton)d).UpdateApplicationViewState();
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00004DC5 File Offset: 0x00002FC5
		private static void OnShowKeyboardAcceleratorTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((AppBarButton)d).UpdateKeyboardAcceleratorTextVisibility(true);
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00004DD3 File Offset: 0x00002FD3
		private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.UpdateApplicationViewState();
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00004DDB File Offset: 0x00002FDB
		private void UpdateVisualState(bool useTransitions = true)
		{
			this.ApplyApplicationViewState(useTransitions);
			this.UpdateCommonState(useTransitions);
			this.UpdateKeyboardAcceleratorTextVisibility(useTransitions);
			this.UpdateFlyoutState(useTransitions);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00004DFC File Offset: 0x00002FFC
		private void UpdateCommonState(bool useTransitions = true)
		{
			if (this._vsm == null)
			{
				return;
			}
			string text;
			if (!base.IsEnabled)
			{
				text = "Disabled";
			}
			else
			{
				if (base.IsPressed)
				{
					text = "Pressed";
				}
				else if (base.IsMouseOver)
				{
					text = "PointerOver";
				}
				else
				{
					text = "Normal";
				}
				if (this.IsInOverflow)
				{
					text = "Overflow" + text;
				}
			}
			this._vsm.CanChangeCommonState = true;
			VisualStateManager.GoToState(this, text, useTransitions);
			this._vsm.CanChangeCommonState = false;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00004E80 File Offset: 0x00003080
		private void UpdateKeyboardAcceleratorTextVisibility(bool useTransitions = true)
		{
			string text = AppBarElementProperties.GetShowKeyboardAcceleratorText(this) ? "KeyboardAcceleratorTextVisible" : "KeyboardAcceleratorTextCollapsed";
			VisualStateManager.GoToState(this, text, useTransitions);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00004EAC File Offset: 0x000030AC
		private void UpdateFlyoutState(bool useTransitions = true)
		{
			VisualStateManager.GoToState(this, (this.Flyout != null && !ToolBar.GetIsOverflowItem(this)) ? "HasFlyout" : "NoFlyout", useTransitions);
		}

		// Token: 0x04000038 RID: 56
		public static readonly DependencyProperty UseSystemFocusVisualsProperty = FocusVisualHelper.UseSystemFocusVisualsProperty.AddOwner(typeof(AppBarButton));

		// Token: 0x04000039 RID: 57
		public static readonly DependencyProperty FocusVisualMarginProperty = FocusVisualHelper.FocusVisualMarginProperty.AddOwner(typeof(AppBarButton));

		// Token: 0x0400003A RID: 58
		public static readonly DependencyProperty CornerRadiusProperty = ControlHelper.CornerRadiusProperty.AddOwner(typeof(AppBarButton));

		// Token: 0x0400003B RID: 59
		public static readonly DependencyProperty FlyoutProperty = FlyoutService.FlyoutProperty.AddOwner(typeof(AppBarButton), new FrameworkPropertyMetadata(new PropertyChangedCallback(AppBarButton.OnFlyoutChanged)));

		// Token: 0x0400003C RID: 60
		public static readonly DependencyProperty IconProperty = AppBarElementProperties.IconProperty.AddOwner(typeof(AppBarButton));

		// Token: 0x0400003D RID: 61
		public static readonly DependencyProperty LabelProperty = AppBarElementProperties.LabelProperty.AddOwner(typeof(AppBarButton));

		// Token: 0x0400003E RID: 62
		public static readonly DependencyProperty LabelPositionProperty = AppBarElementProperties.LabelPositionProperty.AddOwner(typeof(AppBarButton));

		// Token: 0x0400003F RID: 63
		public static readonly DependencyProperty IsCompactProperty = AppBarElementProperties.IsCompactProperty.AddOwner(typeof(AppBarButton));

		// Token: 0x04000040 RID: 64
		public static readonly DependencyProperty IsInOverflowProperty = AppBarElementProperties.IsInOverflowProperty.AddOwner(typeof(AppBarButton));

		// Token: 0x04000041 RID: 65
		private static readonly DependencyProperty ApplicationViewStateProperty = AppBarElementProperties.ApplicationViewStateProperty.AddOwner(typeof(AppBarButton));

		// Token: 0x04000042 RID: 66
		public static readonly DependencyProperty InputGestureTextProperty = AppBarElementProperties.InputGestureTextProperty.AddOwner(typeof(AppBarButton));

		// Token: 0x04000043 RID: 67
		private AppBarElementVisualStateManager _vsm;
	}
}
