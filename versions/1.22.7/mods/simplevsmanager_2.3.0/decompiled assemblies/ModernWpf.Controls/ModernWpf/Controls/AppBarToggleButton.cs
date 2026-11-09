using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using ModernWpf.Controls.Primitives;

namespace ModernWpf.Controls
{
	// Token: 0x02000021 RID: 33
	public class AppBarToggleButton : ToggleButton, ICommandBarElement, IAppBarElement
	{
		// Token: 0x0600014F RID: 335 RVA: 0x00005704 File Offset: 0x00003904
		static AppBarToggleButton()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(AppBarToggleButton), new FrameworkPropertyMetadata(typeof(AppBarToggleButton)));
			UIElement.IsEnabledProperty.OverrideMetadata(typeof(AppBarToggleButton), new FrameworkPropertyMetadata(new PropertyChangedCallback(AppBarToggleButton.OnIsEnabledChanged)));
			ButtonBase.CommandProperty.OverrideMetadata(typeof(AppBarToggleButton), new FrameworkPropertyMetadata(new PropertyChangedCallback(AppBarToggleButton.OnCommandPropertyChanged)));
			ToggleButton.IsCheckedProperty.OverrideMetadata(typeof(AppBarToggleButton), new FrameworkPropertyMetadata(new PropertyChangedCallback(AppBarToggleButton.OnIsCheckedChanged)));
			FrameworkElement.ToolTipProperty.OverrideMetadata(typeof(AppBarToggleButton), new FrameworkPropertyMetadata
			{
				CoerceValueCallback = new CoerceValueCallback(AppBarElementProperties.CoerceToolTip)
			});
			ToolBar.OverflowModeProperty.OverrideMetadata(typeof(AppBarToggleButton), new FrameworkPropertyMetadata(new PropertyChangedCallback(AppBarToggleButton.OnOverflowModePropertyChanged)));
			CommandBarToolBar.DefaultLabelPositionProperty.OverrideMetadata(typeof(AppBarToggleButton), new FrameworkPropertyMetadata(new PropertyChangedCallback(AppBarToggleButton.OnDefaultLabelPositionPropertyChanged)));
			AppBarElementProperties.IsInOverflowPropertyKey.OverrideMetadata(typeof(AppBarToggleButton), new FrameworkPropertyMetadata(new PropertyChangedCallback(AppBarToggleButton.OnIsInOverflowChanged)));
			AppBarElementProperties.ShowKeyboardAcceleratorTextProperty.OverrideMetadata(typeof(AppBarToggleButton), new FrameworkPropertyMetadata(new PropertyChangedCallback(AppBarToggleButton.OnShowKeyboardAcceleratorTextPropertyChanged)));
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0000595C File Offset: 0x00003B5C
		public AppBarToggleButton()
		{
			base.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.OnIsVisibleChanged);
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000151 RID: 337 RVA: 0x00005976 File Offset: 0x00003B76
		// (set) Token: 0x06000152 RID: 338 RVA: 0x00005988 File Offset: 0x00003B88
		public bool UseSystemFocusVisuals
		{
			get
			{
				return (bool)base.GetValue(AppBarToggleButton.UseSystemFocusVisualsProperty);
			}
			set
			{
				base.SetValue(AppBarToggleButton.UseSystemFocusVisualsProperty, value);
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000153 RID: 339 RVA: 0x0000599B File Offset: 0x00003B9B
		// (set) Token: 0x06000154 RID: 340 RVA: 0x000059AD File Offset: 0x00003BAD
		public Thickness FocusVisualMargin
		{
			get
			{
				return (Thickness)base.GetValue(AppBarToggleButton.FocusVisualMarginProperty);
			}
			set
			{
				base.SetValue(AppBarToggleButton.FocusVisualMarginProperty, value);
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000155 RID: 341 RVA: 0x000059C0 File Offset: 0x00003BC0
		// (set) Token: 0x06000156 RID: 342 RVA: 0x000059D2 File Offset: 0x00003BD2
		public CornerRadius CornerRadius
		{
			get
			{
				return (CornerRadius)base.GetValue(AppBarToggleButton.CornerRadiusProperty);
			}
			set
			{
				base.SetValue(AppBarToggleButton.CornerRadiusProperty, value);
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000157 RID: 343 RVA: 0x000059E5 File Offset: 0x00003BE5
		// (set) Token: 0x06000158 RID: 344 RVA: 0x000059F7 File Offset: 0x00003BF7
		public IconElement Icon
		{
			get
			{
				return (IconElement)base.GetValue(AppBarToggleButton.IconProperty);
			}
			set
			{
				base.SetValue(AppBarToggleButton.IconProperty, value);
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000159 RID: 345 RVA: 0x00005A05 File Offset: 0x00003C05
		// (set) Token: 0x0600015A RID: 346 RVA: 0x00005A17 File Offset: 0x00003C17
		public string Label
		{
			get
			{
				return (string)base.GetValue(AppBarToggleButton.LabelProperty);
			}
			set
			{
				base.SetValue(AppBarToggleButton.LabelProperty, value);
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600015B RID: 347 RVA: 0x00005A25 File Offset: 0x00003C25
		// (set) Token: 0x0600015C RID: 348 RVA: 0x00005A37 File Offset: 0x00003C37
		public CommandBarLabelPosition LabelPosition
		{
			get
			{
				return (CommandBarLabelPosition)base.GetValue(AppBarToggleButton.LabelPositionProperty);
			}
			set
			{
				base.SetValue(AppBarToggleButton.LabelPositionProperty, value);
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600015D RID: 349 RVA: 0x00005A4A File Offset: 0x00003C4A
		// (set) Token: 0x0600015E RID: 350 RVA: 0x00005A5C File Offset: 0x00003C5C
		public bool IsCompact
		{
			get
			{
				return (bool)base.GetValue(AppBarToggleButton.IsCompactProperty);
			}
			set
			{
				base.SetValue(AppBarToggleButton.IsCompactProperty, value);
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600015F RID: 351 RVA: 0x00005A6F File Offset: 0x00003C6F
		public bool IsInOverflow
		{
			get
			{
				return (bool)base.GetValue(AppBarToggleButton.IsInOverflowProperty);
			}
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00005A81 File Offset: 0x00003C81
		private static void OnIsInOverflowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((AppBarToggleButton)d).UpdateCommonState(true);
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000161 RID: 353 RVA: 0x00005A8F File Offset: 0x00003C8F
		private AppBarElementApplicationViewState ApplicationViewState
		{
			get
			{
				return (AppBarElementApplicationViewState)base.GetValue(AppBarToggleButton.ApplicationViewStateProperty);
			}
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00005AA4 File Offset: 0x00003CA4
		private void UpdateApplicationViewState()
		{
			AppBarElementApplicationViewState appBarElementApplicationViewState;
			if (this.IsInOverflow && base.IsVisible)
			{
				CommandBarOverflowPanel commandBarOverflowPanel = base.VisualParent as CommandBarOverflowPanel;
				if (commandBarOverflowPanel != null)
				{
					appBarElementApplicationViewState = this.ComputeApplicationViewStateInOverflow(commandBarOverflowPanel.HasMenuIcon);
					goto IL_77;
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
			IL_77:
			base.SetValue(AppBarElementProperties.ApplicationViewStatePropertyKey, appBarElementApplicationViewState);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00005B39 File Offset: 0x00003D39
		private AppBarElementApplicationViewState ComputeApplicationViewStateInOverflow(bool hasMenuIcon)
		{
			if (!hasMenuIcon)
			{
				return AppBarElementApplicationViewState.Overflow;
			}
			return AppBarElementApplicationViewState.OverflowWithMenuIcons;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00005B44 File Offset: 0x00003D44
		private void ApplyApplicationViewState(bool useTransitions = true)
		{
			VisualStateManager.GoToState(this, this.ApplicationViewState.ToString(), useTransitions);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00005B6D File Offset: 0x00003D6D
		void IAppBarElement.UpdateApplicationViewState()
		{
			this.UpdateApplicationViewState();
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00005B75 File Offset: 0x00003D75
		void IAppBarElement.ApplyApplicationViewState()
		{
			this.ApplyApplicationViewState(true);
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000167 RID: 359 RVA: 0x00005B7E File Offset: 0x00003D7E
		// (set) Token: 0x06000168 RID: 360 RVA: 0x00005B90 File Offset: 0x00003D90
		public string InputGestureText
		{
			get
			{
				return (string)base.GetValue(AppBarToggleButton.InputGestureTextProperty);
			}
			set
			{
				base.SetValue(AppBarToggleButton.InputGestureTextProperty, value);
			}
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00005BA0 File Offset: 0x00003DA0
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

		// Token: 0x0600016A RID: 362 RVA: 0x00005BDB File Offset: 0x00003DDB
		protected override void OnVisualParentChanged(DependencyObject oldParent)
		{
			base.OnVisualParentChanged(oldParent);
			this.UpdateApplicationViewState();
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00005BEA File Offset: 0x00003DEA
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

		// Token: 0x0600016C RID: 364 RVA: 0x00005C1D File Offset: 0x00003E1D
		protected override void OnIsPressedChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnIsPressedChanged(e);
			this.UpdateCommonState(true);
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00005C2D File Offset: 0x00003E2D
		private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((AppBarToggleButton)d).UpdateCommonState(true);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00005C3B File Offset: 0x00003E3B
		private static void OnCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			AppBarToggleButton appBarToggleButton = (AppBarToggleButton)d;
			appBarToggleButton.CoerceValue(AppBarToggleButton.LabelProperty);
			appBarToggleButton.CoerceValue(AppBarToggleButton.InputGestureTextProperty);
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00005C58 File Offset: 0x00003E58
		private static void OnIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((AppBarToggleButton)d).UpdateCommonState(true);
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00005C66 File Offset: 0x00003E66
		private static void OnOverflowModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			AppBarElementProperties.UpdateIsInOverflow(d);
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00005C6E File Offset: 0x00003E6E
		private static void OnDefaultLabelPositionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((AppBarToggleButton)d).UpdateApplicationViewState();
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00005C7B File Offset: 0x00003E7B
		private static void OnShowKeyboardAcceleratorTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((AppBarToggleButton)d).UpdateKeyboardAcceleratorTextVisibility(true);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00005C89 File Offset: 0x00003E89
		private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.UpdateApplicationViewState();
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00005C91 File Offset: 0x00003E91
		private void UpdateVisualState(bool useTransitions = true)
		{
			this.ApplyApplicationViewState(useTransitions);
			this.UpdateCommonState(useTransitions);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00005CA4 File Offset: 0x00003EA4
		private void UpdateCommonState(bool useTransitions = true)
		{
			if (this._vsm == null)
			{
				return;
			}
			bool isEnabled = base.IsEnabled;
			bool? isChecked = base.IsChecked;
			bool flag = false;
			bool flag2 = !(isChecked.GetValueOrDefault() == flag & isChecked != null);
			string text;
			if (!isEnabled)
			{
				text = "Disabled";
			}
			else if (base.IsPressed)
			{
				text = "Pressed";
			}
			else if (base.IsMouseOver)
			{
				text = "PointerOver";
			}
			else if (!flag2)
			{
				text = "Normal";
			}
			else
			{
				text = string.Empty;
			}
			if (flag2)
			{
				text = "Checked" + text;
			}
			if (isEnabled && this.IsInOverflow)
			{
				text = "Overflow" + text;
			}
			this._vsm.CanChangeCommonState = true;
			VisualStateManager.GoToState(this, text, useTransitions);
			this._vsm.CanChangeCommonState = false;
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00005D64 File Offset: 0x00003F64
		private void UpdateKeyboardAcceleratorTextVisibility(bool useTransitions = true)
		{
			string text = AppBarElementProperties.GetShowKeyboardAcceleratorText(this) ? "KeyboardAcceleratorTextVisible" : "KeyboardAcceleratorTextCollapsed";
			VisualStateManager.GoToState(this, text, useTransitions);
		}

		// Token: 0x0400005F RID: 95
		public static readonly DependencyProperty UseSystemFocusVisualsProperty = FocusVisualHelper.UseSystemFocusVisualsProperty.AddOwner(typeof(AppBarToggleButton));

		// Token: 0x04000060 RID: 96
		public static readonly DependencyProperty FocusVisualMarginProperty = FocusVisualHelper.FocusVisualMarginProperty.AddOwner(typeof(AppBarToggleButton));

		// Token: 0x04000061 RID: 97
		public static readonly DependencyProperty CornerRadiusProperty = ControlHelper.CornerRadiusProperty.AddOwner(typeof(AppBarToggleButton));

		// Token: 0x04000062 RID: 98
		public static readonly DependencyProperty IconProperty = AppBarElementProperties.IconProperty.AddOwner(typeof(AppBarToggleButton));

		// Token: 0x04000063 RID: 99
		public static readonly DependencyProperty LabelProperty = AppBarElementProperties.LabelProperty.AddOwner(typeof(AppBarToggleButton));

		// Token: 0x04000064 RID: 100
		public static readonly DependencyProperty LabelPositionProperty = AppBarElementProperties.LabelPositionProperty.AddOwner(typeof(AppBarToggleButton));

		// Token: 0x04000065 RID: 101
		public static readonly DependencyProperty IsCompactProperty = AppBarElementProperties.IsCompactProperty.AddOwner(typeof(AppBarToggleButton));

		// Token: 0x04000066 RID: 102
		public static readonly DependencyProperty IsInOverflowProperty = AppBarElementProperties.IsInOverflowProperty.AddOwner(typeof(AppBarToggleButton));

		// Token: 0x04000067 RID: 103
		private static readonly DependencyProperty ApplicationViewStateProperty = AppBarElementProperties.ApplicationViewStateProperty.AddOwner(typeof(AppBarToggleButton));

		// Token: 0x04000068 RID: 104
		public static readonly DependencyProperty InputGestureTextProperty = AppBarElementProperties.InputGestureTextProperty.AddOwner(typeof(AppBarToggleButton));

		// Token: 0x04000069 RID: 105
		private AppBarElementVisualStateManager _vsm;
	}
}
