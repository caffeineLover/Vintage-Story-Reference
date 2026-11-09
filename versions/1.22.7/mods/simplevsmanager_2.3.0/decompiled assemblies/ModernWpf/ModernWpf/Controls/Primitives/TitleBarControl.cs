using System;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x020002B1 RID: 689
	[TemplatePart(Name = "PART_BackButton", Type = typeof(Button))]
	[TemplatePart(Name = "PART_MaximizeRestoreButton", Type = typeof(TitleBarButton))]
	[TemplatePart(Name = "PART_LeftSystemOverlay", Type = typeof(FrameworkElement))]
	[TemplatePart(Name = "PART_RightSystemOverlay", Type = typeof(FrameworkElement))]
	[StyleTypedProperty(Property = "ButtonStyle", StyleTargetType = typeof(TitleBarButton))]
	[StyleTypedProperty(Property = "BackButtonStyle", StyleTargetType = typeof(TitleBarButton))]
	public class TitleBarControl : Control
	{
		// Token: 0x06001509 RID: 5385 RVA: 0x0003EA70 File Offset: 0x0003CC70
		static TitleBarControl()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TitleBarControl), new FrameworkPropertyMetadata(typeof(TitleBarControl)));
		}

		// Token: 0x0600150A RID: 5386 RVA: 0x0003ECA8 File Offset: 0x0003CEA8
		public TitleBarControl()
		{
			base.CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, new ExecutedRoutedEventHandler(this.MinimizeWindow)));
			base.CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, new ExecutedRoutedEventHandler(this.MaximizeWindow)));
			base.CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, new ExecutedRoutedEventHandler(this.RestoreWindow)));
			base.CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, new ExecutedRoutedEventHandler(this.CloseWindow)));
			TitleBarControl.SetInsideTitleBar(this, true);
		}

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x0600150B RID: 5387 RVA: 0x0003ED4A File Offset: 0x0003CF4A
		// (set) Token: 0x0600150C RID: 5388 RVA: 0x0003ED5C File Offset: 0x0003CF5C
		public bool IsActive
		{
			get
			{
				return (bool)base.GetValue(TitleBarControl.IsActiveProperty);
			}
			set
			{
				base.SetValue(TitleBarControl.IsActiveProperty, value);
			}
		}

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x0600150D RID: 5389 RVA: 0x0003ED6F File Offset: 0x0003CF6F
		// (set) Token: 0x0600150E RID: 5390 RVA: 0x0003ED81 File Offset: 0x0003CF81
		public Brush InactiveBackground
		{
			get
			{
				return (Brush)base.GetValue(TitleBarControl.InactiveBackgroundProperty);
			}
			set
			{
				base.SetValue(TitleBarControl.InactiveBackgroundProperty, value);
			}
		}

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x0600150F RID: 5391 RVA: 0x0003ED8F File Offset: 0x0003CF8F
		// (set) Token: 0x06001510 RID: 5392 RVA: 0x0003EDA1 File Offset: 0x0003CFA1
		public Brush InactiveForeground
		{
			get
			{
				return (Brush)base.GetValue(TitleBarControl.InactiveForegroundProperty);
			}
			set
			{
				base.SetValue(TitleBarControl.InactiveForegroundProperty, value);
			}
		}

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x06001511 RID: 5393 RVA: 0x0003EDAF File Offset: 0x0003CFAF
		// (set) Token: 0x06001512 RID: 5394 RVA: 0x0003EDC1 File Offset: 0x0003CFC1
		public Style ButtonStyle
		{
			get
			{
				return (Style)base.GetValue(TitleBarControl.ButtonStyleProperty);
			}
			set
			{
				base.SetValue(TitleBarControl.ButtonStyleProperty, value);
			}
		}

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x06001513 RID: 5395 RVA: 0x0003EDCF File Offset: 0x0003CFCF
		// (set) Token: 0x06001514 RID: 5396 RVA: 0x0003EDE1 File Offset: 0x0003CFE1
		public string Title
		{
			get
			{
				return (string)base.GetValue(TitleBarControl.TitleProperty);
			}
			set
			{
				base.SetValue(TitleBarControl.TitleProperty, value);
			}
		}

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06001515 RID: 5397 RVA: 0x0003EDEF File Offset: 0x0003CFEF
		// (set) Token: 0x06001516 RID: 5398 RVA: 0x0003EE01 File Offset: 0x0003D001
		public ImageSource Icon
		{
			get
			{
				return (ImageSource)base.GetValue(TitleBarControl.IconProperty);
			}
			set
			{
				base.SetValue(TitleBarControl.IconProperty, value);
			}
		}

		// Token: 0x06001517 RID: 5399 RVA: 0x0003EE0F File Offset: 0x0003D00F
		private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((TitleBarControl)d).UpdateActualIcon();
		}

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06001518 RID: 5400 RVA: 0x0003EE1C File Offset: 0x0003D01C
		// (set) Token: 0x06001519 RID: 5401 RVA: 0x0003EE2E File Offset: 0x0003D02E
		public ImageSource ActualIcon
		{
			get
			{
				return (ImageSource)base.GetValue(TitleBarControl.ActualIconProperty);
			}
			private set
			{
				base.SetValue(TitleBarControl.ActualIconPropertyKey, value);
			}
		}

		// Token: 0x0600151A RID: 5402 RVA: 0x0003EE3C File Offset: 0x0003D03C
		private void UpdateActualIcon()
		{
			if (this.Icon != null)
			{
				this.ActualIcon = this.Icon;
				return;
			}
			ImageSource actualIcon = null;
			IntPtr[] array = new IntPtr[1];
			IconHelper.GetDefaultIconHandles(null, array);
			IntPtr intPtr = array[0];
			if (intPtr != IntPtr.Zero)
			{
				try
				{
					actualIcon = Imaging.CreateBitmapSourceFromHIcon(intPtr, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
				}
				finally
				{
					IconHelper.DestroyIcon(intPtr);
				}
			}
			this.ActualIcon = actualIcon;
		}

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x0600151B RID: 5403 RVA: 0x0003EEB4 File Offset: 0x0003D0B4
		// (set) Token: 0x0600151C RID: 5404 RVA: 0x0003EEC6 File Offset: 0x0003D0C6
		public bool IsIconVisible
		{
			get
			{
				return (bool)base.GetValue(TitleBarControl.IsIconVisibleProperty);
			}
			set
			{
				base.SetValue(TitleBarControl.IsIconVisibleProperty, value);
			}
		}

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x0600151D RID: 5405 RVA: 0x0003EED9 File Offset: 0x0003D0D9
		// (set) Token: 0x0600151E RID: 5406 RVA: 0x0003EEEB File Offset: 0x0003D0EB
		public bool IsBackButtonVisible
		{
			get
			{
				return (bool)base.GetValue(TitleBarControl.IsBackButtonVisibleProperty);
			}
			set
			{
				base.SetValue(TitleBarControl.IsBackButtonVisibleProperty, value);
			}
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x0600151F RID: 5407 RVA: 0x0003EEFE File Offset: 0x0003D0FE
		// (set) Token: 0x06001520 RID: 5408 RVA: 0x0003EF10 File Offset: 0x0003D110
		public bool IsBackEnabled
		{
			get
			{
				return (bool)base.GetValue(TitleBarControl.IsBackEnabledProperty);
			}
			set
			{
				base.SetValue(TitleBarControl.IsBackEnabledProperty, value);
			}
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06001521 RID: 5409 RVA: 0x0003EF23 File Offset: 0x0003D123
		// (set) Token: 0x06001522 RID: 5410 RVA: 0x0003EF35 File Offset: 0x0003D135
		public ICommand BackButtonCommand
		{
			get
			{
				return (ICommand)base.GetValue(TitleBarControl.BackButtonCommandProperty);
			}
			set
			{
				base.SetValue(TitleBarControl.BackButtonCommandProperty, value);
			}
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06001523 RID: 5411 RVA: 0x0003EF43 File Offset: 0x0003D143
		// (set) Token: 0x06001524 RID: 5412 RVA: 0x0003EF50 File Offset: 0x0003D150
		public object BackButtonCommandParameter
		{
			get
			{
				return base.GetValue(TitleBarControl.BackButtonCommandParameterProperty);
			}
			set
			{
				base.SetValue(TitleBarControl.BackButtonCommandParameterProperty, value);
			}
		}

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x06001525 RID: 5413 RVA: 0x0003EF5E File Offset: 0x0003D15E
		// (set) Token: 0x06001526 RID: 5414 RVA: 0x0003EF70 File Offset: 0x0003D170
		public IInputElement BackButtonCommandTarget
		{
			get
			{
				return (IInputElement)base.GetValue(TitleBarControl.BackButtonCommandTargetProperty);
			}
			set
			{
				base.SetValue(TitleBarControl.BackButtonCommandTargetProperty, value);
			}
		}

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x06001527 RID: 5415 RVA: 0x0003EF7E File Offset: 0x0003D17E
		// (set) Token: 0x06001528 RID: 5416 RVA: 0x0003EF90 File Offset: 0x0003D190
		public Style BackButtonStyle
		{
			get
			{
				return (Style)base.GetValue(TitleBarControl.BackButtonStyleProperty);
			}
			set
			{
				base.SetValue(TitleBarControl.BackButtonStyleProperty, value);
			}
		}

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x06001529 RID: 5417 RVA: 0x0003EF9E File Offset: 0x0003D19E
		// (set) Token: 0x0600152A RID: 5418 RVA: 0x0003EFB0 File Offset: 0x0003D1B0
		public bool ExtendViewIntoTitleBar
		{
			get
			{
				return (bool)base.GetValue(TitleBarControl.ExtendViewIntoTitleBarProperty);
			}
			set
			{
				base.SetValue(TitleBarControl.ExtendViewIntoTitleBarProperty, value);
			}
		}

		// Token: 0x0600152B RID: 5419 RVA: 0x0003EFC3 File Offset: 0x0003D1C3
		internal static bool GetInsideTitleBar(UIElement element)
		{
			return (bool)element.GetValue(TitleBarControl.InsideTitleBarProperty);
		}

		// Token: 0x0600152C RID: 5420 RVA: 0x0003EFD5 File Offset: 0x0003D1D5
		internal static void SetInsideTitleBar(UIElement element, bool value)
		{
			element.SetValue(TitleBarControl.InsideTitleBarProperty, value);
		}

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x0600152D RID: 5421 RVA: 0x0003EFE8 File Offset: 0x0003D1E8
		// (set) Token: 0x0600152E RID: 5422 RVA: 0x0003EFF0 File Offset: 0x0003D1F0
		private Button BackButton { get; set; }

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x0600152F RID: 5423 RVA: 0x0003EFF9 File Offset: 0x0003D1F9
		// (set) Token: 0x06001530 RID: 5424 RVA: 0x0003F001 File Offset: 0x0003D201
		private TitleBarButton MaximizeRestoreButton { get; set; }

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x06001531 RID: 5425 RVA: 0x0003F00A File Offset: 0x0003D20A
		// (set) Token: 0x06001532 RID: 5426 RVA: 0x0003F012 File Offset: 0x0003D212
		private FrameworkElement LeftSystemOverlay { get; set; }

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x06001533 RID: 5427 RVA: 0x0003F01B File Offset: 0x0003D21B
		// (set) Token: 0x06001534 RID: 5428 RVA: 0x0003F023 File Offset: 0x0003D223
		private FrameworkElement RightSystemOverlay { get; set; }

		// Token: 0x06001535 RID: 5429 RVA: 0x0003F02C File Offset: 0x0003D22C
		public override void OnApplyTemplate()
		{
			if (this.BackButton != null)
			{
				this.BackButton.Click -= new RoutedEventHandler(this.OnBackButtonClick);
			}
			if (this.LeftSystemOverlay != null)
			{
				this.LeftSystemOverlay.SizeChanged -= new SizeChangedEventHandler(this.OnLeftSystemOverlaySizeChanged);
			}
			if (this.RightSystemOverlay != null)
			{
				this.RightSystemOverlay.SizeChanged -= new SizeChangedEventHandler(this.OnRightSystemOverlaySizeChanged);
			}
			base.OnApplyTemplate();
			this.BackButton = (base.GetTemplateChild("PART_BackButton") as Button);
			this.MaximizeRestoreButton = (base.GetTemplateChild("PART_MaximizeRestoreButton") as TitleBarButton);
			this.LeftSystemOverlay = (base.GetTemplateChild("PART_LeftSystemOverlay") as FrameworkElement);
			this.RightSystemOverlay = (base.GetTemplateChild("PART_RightSystemOverlay") as FrameworkElement);
			if (this.BackButton != null)
			{
				this.BackButton.Click += new RoutedEventHandler(this.OnBackButtonClick);
			}
			if (this.MaximizeRestoreButton != null)
			{
				this.MaximizeRestoreButton.HitTestCode = new uint?(9U);
			}
			if (this.LeftSystemOverlay != null)
			{
				this.LeftSystemOverlay.SizeChanged += new SizeChangedEventHandler(this.OnLeftSystemOverlaySizeChanged);
				this.UpdateSystemOverlayLeftInset(this.LeftSystemOverlay.ActualWidth);
			}
			if (this.RightSystemOverlay != null)
			{
				this.RightSystemOverlay.SizeChanged += new SizeChangedEventHandler(this.OnRightSystemOverlaySizeChanged);
				this.UpdateSystemOverlayRightInset(this.RightSystemOverlay.ActualWidth);
			}
		}

		// Token: 0x06001536 RID: 5430 RVA: 0x0003F18D File Offset: 0x0003D38D
		protected override void OnInitialized(EventArgs e)
		{
			this.UpdateActualIcon();
			base.OnInitialized(e);
		}

		// Token: 0x06001537 RID: 5431 RVA: 0x0003F19C File Offset: 0x0003D39C
		protected override void OnVisualParentChanged(DependencyObject oldParent)
		{
			if (this._parentWindow != null && this._altLeftBinding != null)
			{
				this._parentWindow.InputBindings.Remove(this._altLeftBinding);
				this._altLeftBinding = null;
			}
			base.OnVisualParentChanged(oldParent);
			this._parentWindow = (base.TemplatedParent as Window);
			if (this._parentWindow != null)
			{
				this._altLeftBinding = new KeyBinding(new TitleBarControl.GoBackCommand(this), 23, 1);
				this._parentWindow.InputBindings.Add(this._altLeftBinding);
			}
		}

		// Token: 0x06001538 RID: 5432 RVA: 0x0003F224 File Offset: 0x0003D424
		protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
		{
			base.OnRenderSizeChanged(sizeInfo);
			Window window = base.TemplatedParent as Window;
			if (window != null)
			{
				TitleBar.SetHeight(window, sizeInfo.NewSize.Height);
			}
		}

		// Token: 0x06001539 RID: 5433 RVA: 0x0003F25C File Offset: 0x0003D45C
		private void OnBackButtonClick(object sender, RoutedEventArgs e)
		{
			Window window = base.TemplatedParent as Window;
			if (window != null)
			{
				TitleBar.RaiseBackRequested(window);
			}
		}

		// Token: 0x0600153A RID: 5434 RVA: 0x0003F280 File Offset: 0x0003D480
		private void OnLeftSystemOverlaySizeChanged(object sender, SizeChangedEventArgs e)
		{
			this.UpdateSystemOverlayLeftInset(e.NewSize.Width);
		}

		// Token: 0x0600153B RID: 5435 RVA: 0x0003F2A4 File Offset: 0x0003D4A4
		private void OnRightSystemOverlaySizeChanged(object sender, SizeChangedEventArgs e)
		{
			this.UpdateSystemOverlayRightInset(e.NewSize.Width);
		}

		// Token: 0x0600153C RID: 5436 RVA: 0x0003F2C8 File Offset: 0x0003D4C8
		private void UpdateSystemOverlayLeftInset(double value)
		{
			Window window = base.TemplatedParent as Window;
			if (window != null)
			{
				TitleBar.SetSystemOverlayLeftInset(window, value);
			}
		}

		// Token: 0x0600153D RID: 5437 RVA: 0x0003F2EC File Offset: 0x0003D4EC
		private void UpdateSystemOverlayRightInset(double value)
		{
			Window window = base.TemplatedParent as Window;
			if (window != null)
			{
				TitleBar.SetSystemOverlayRightInset(window, value);
			}
		}

		// Token: 0x0600153E RID: 5438 RVA: 0x0003F310 File Offset: 0x0003D510
		private void MinimizeWindow(object sender, ExecutedRoutedEventArgs e)
		{
			Window window = base.TemplatedParent as Window;
			if (window != null)
			{
				SystemCommands.MinimizeWindow(window);
			}
		}

		// Token: 0x0600153F RID: 5439 RVA: 0x0003F334 File Offset: 0x0003D534
		private void MaximizeWindow(object sender, ExecutedRoutedEventArgs e)
		{
			Window window = base.TemplatedParent as Window;
			if (window != null)
			{
				SystemCommands.MaximizeWindow(window);
			}
		}

		// Token: 0x06001540 RID: 5440 RVA: 0x0003F358 File Offset: 0x0003D558
		private void RestoreWindow(object sender, ExecutedRoutedEventArgs e)
		{
			Window window = base.TemplatedParent as Window;
			if (window != null)
			{
				SystemCommands.RestoreWindow(window);
			}
		}

		// Token: 0x06001541 RID: 5441 RVA: 0x0003F37C File Offset: 0x0003D57C
		private void CloseWindow(object sender, ExecutedRoutedEventArgs e)
		{
			Window window = base.TemplatedParent as Window;
			if (window != null)
			{
				SystemCommands.CloseWindow(window);
			}
		}

		// Token: 0x06001542 RID: 5442 RVA: 0x0003F39E File Offset: 0x0003D59E
		private void InvokeBack()
		{
			TitleBarControl.InvokeButton(this.BackButton);
		}

		// Token: 0x06001543 RID: 5443 RVA: 0x0003F3AC File Offset: 0x0003D5AC
		private static void InvokeButton(Button button)
		{
			if (button != null && button.IsEnabled)
			{
				TitleBarButton titleBarButton = button as TitleBarButton;
				if (titleBarButton != null)
				{
					titleBarButton.DoClick();
					return;
				}
				AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(button);
				if (automationPeer != null)
				{
					IInvokeProvider invokeProvider = automationPeer.GetPattern(0) as IInvokeProvider;
					if (invokeProvider != null)
					{
						invokeProvider.Invoke();
					}
				}
			}
		}

		// Token: 0x04000721 RID: 1825
		private const string BackButtonName = "PART_BackButton";

		// Token: 0x04000722 RID: 1826
		private const string MaximizeRestoreButtonName = "PART_MaximizeRestoreButton";

		// Token: 0x04000723 RID: 1827
		private const string LeftSystemOverlayName = "PART_LeftSystemOverlay";

		// Token: 0x04000724 RID: 1828
		private const string RightSystemOverlayName = "PART_RightSystemOverlay";

		// Token: 0x04000725 RID: 1829
		private Window _parentWindow;

		// Token: 0x04000726 RID: 1830
		private KeyBinding _altLeftBinding;

		// Token: 0x04000727 RID: 1831
		public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive", typeof(bool), typeof(TitleBarControl), new PropertyMetadata(false));

		// Token: 0x04000728 RID: 1832
		public static readonly DependencyProperty InactiveBackgroundProperty = TitleBar.InactiveBackgroundProperty.AddOwner(typeof(TitleBarControl));

		// Token: 0x04000729 RID: 1833
		public static readonly DependencyProperty InactiveForegroundProperty = TitleBar.InactiveForegroundProperty.AddOwner(typeof(TitleBarControl));

		// Token: 0x0400072A RID: 1834
		public static readonly DependencyProperty ButtonStyleProperty = TitleBar.ButtonStyleProperty.AddOwner(typeof(TitleBarControl));

		// Token: 0x0400072B RID: 1835
		public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(TitleBarControl), new PropertyMetadata(string.Empty));

		// Token: 0x0400072C RID: 1836
		public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(ImageSource), typeof(TitleBarControl), new PropertyMetadata(new PropertyChangedCallback(TitleBarControl.OnIconChanged)));

		// Token: 0x0400072D RID: 1837
		private static readonly DependencyPropertyKey ActualIconPropertyKey = DependencyProperty.RegisterReadOnly("ActualIcon", typeof(ImageSource), typeof(TitleBarControl), null);

		// Token: 0x0400072E RID: 1838
		public static readonly DependencyProperty ActualIconProperty = TitleBarControl.ActualIconPropertyKey.DependencyProperty;

		// Token: 0x0400072F RID: 1839
		public static readonly DependencyProperty IsIconVisibleProperty = TitleBar.IsIconVisibleProperty.AddOwner(typeof(TitleBarControl));

		// Token: 0x04000730 RID: 1840
		public static readonly DependencyProperty IsBackButtonVisibleProperty = TitleBar.IsBackButtonVisibleProperty.AddOwner(typeof(TitleBarControl));

		// Token: 0x04000731 RID: 1841
		public static readonly DependencyProperty IsBackEnabledProperty = TitleBar.IsBackEnabledProperty.AddOwner(typeof(TitleBarControl));

		// Token: 0x04000732 RID: 1842
		public static readonly DependencyProperty BackButtonCommandProperty = TitleBar.BackButtonCommandProperty.AddOwner(typeof(TitleBarControl));

		// Token: 0x04000733 RID: 1843
		public static readonly DependencyProperty BackButtonCommandParameterProperty = TitleBar.BackButtonCommandParameterProperty.AddOwner(typeof(TitleBarControl));

		// Token: 0x04000734 RID: 1844
		public static readonly DependencyProperty BackButtonCommandTargetProperty = TitleBar.BackButtonCommandTargetProperty.AddOwner(typeof(TitleBarControl));

		// Token: 0x04000735 RID: 1845
		public static readonly DependencyProperty BackButtonStyleProperty = TitleBar.BackButtonStyleProperty.AddOwner(typeof(TitleBarControl));

		// Token: 0x04000736 RID: 1846
		public static readonly DependencyProperty ExtendViewIntoTitleBarProperty = TitleBar.ExtendViewIntoTitleBarProperty.AddOwner(typeof(TitleBarControl));

		// Token: 0x04000737 RID: 1847
		internal static readonly DependencyProperty InsideTitleBarProperty = DependencyProperty.RegisterAttached("InsideTitleBar", typeof(bool), typeof(TitleBarControl), new FrameworkPropertyMetadata(false, 32));

		// Token: 0x02000423 RID: 1059
		private class GoBackCommand : ICommand
		{
			// Token: 0x06001C2C RID: 7212 RVA: 0x0004B792 File Offset: 0x00049992
			public GoBackCommand(TitleBarControl owner)
			{
				this._owner = owner;
			}

			// Token: 0x14000026 RID: 38
			// (add) Token: 0x06001C2D RID: 7213 RVA: 0x0004B7A4 File Offset: 0x000499A4
			// (remove) Token: 0x06001C2E RID: 7214 RVA: 0x0004B7DC File Offset: 0x000499DC
			public event EventHandler CanExecuteChanged;

			// Token: 0x06001C2F RID: 7215 RVA: 0x0004B811 File Offset: 0x00049A11
			public bool CanExecute(object parameter)
			{
				return true;
			}

			// Token: 0x06001C30 RID: 7216 RVA: 0x0004B814 File Offset: 0x00049A14
			public void Execute(object parameter)
			{
				this._owner.InvokeBack();
			}

			// Token: 0x04000A3E RID: 2622
			private readonly TitleBarControl _owner;
		}
	}
}
