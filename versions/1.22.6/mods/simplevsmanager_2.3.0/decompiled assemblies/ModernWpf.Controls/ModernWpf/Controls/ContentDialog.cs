using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace ModernWpf.Controls
{
	// Token: 0x02000035 RID: 53
	[TemplatePart(Name = "Container", Type = typeof(Border))]
	[TemplatePart(Name = "LayoutRoot", Type = typeof(FrameworkElement))]
	[TemplatePart(Name = "PrimaryButton", Type = typeof(Button))]
	[TemplatePart(Name = "SecondaryButton", Type = typeof(Button))]
	[TemplatePart(Name = "CloseButton", Type = typeof(Button))]
	[TemplateVisualState(GroupName = "DialogShowingStates", Name = "DialogHidden")]
	[TemplateVisualState(GroupName = "DialogShowingStates", Name = "DialogShowing")]
	[TemplateVisualState(GroupName = "DialogShowingStates", Name = "DialogShowingWithoutSmokeLayer")]
	[TemplateVisualState(GroupName = "DialogSizingStates", Name = "DefaultDialogSizing")]
	[TemplateVisualState(GroupName = "DialogSizingStates", Name = "FullDialogSizing")]
	[TemplateVisualState(GroupName = "ButtonsVisibilityStates", Name = "AllVisible")]
	[TemplateVisualState(GroupName = "ButtonsVisibilityStates", Name = "NoneVisible")]
	[TemplateVisualState(GroupName = "ButtonsVisibilityStates", Name = "PrimaryVisible")]
	[TemplateVisualState(GroupName = "ButtonsVisibilityStates", Name = "SecondaryVisible")]
	[TemplateVisualState(GroupName = "ButtonsVisibilityStates", Name = "CloseVisible")]
	[TemplateVisualState(GroupName = "ButtonsVisibilityStates", Name = "PrimaryAndSecondaryVisible")]
	[TemplateVisualState(GroupName = "ButtonsVisibilityStates", Name = "PrimaryAndCloseVisible")]
	[TemplateVisualState(GroupName = "ButtonsVisibilityStates", Name = "SecondaryAndCloseVisible")]
	[TemplateVisualState(GroupName = "DefaultButtonStates", Name = "NoDefaultButton")]
	[TemplateVisualState(GroupName = "DefaultButtonStates", Name = "PrimaryAsDefaultButton")]
	[TemplateVisualState(GroupName = "DefaultButtonStates", Name = "SecondaryAsDefaultButton")]
	[TemplateVisualState(GroupName = "DefaultButtonStates", Name = "CloseAsDefaultButton")]
	[TemplateVisualState(GroupName = "DialogBorderStates", Name = "NoBorder")]
	[TemplateVisualState(GroupName = "DialogBorderStates", Name = "AccentColorBorder")]
	[StyleTypedProperty(Property = "PrimaryButtonStyle", StyleTargetType = typeof(Button))]
	[StyleTypedProperty(Property = "SecondaryButtonStyle", StyleTargetType = typeof(Button))]
	[StyleTypedProperty(Property = "CloseButtonStyle", StyleTargetType = typeof(Button))]
	public class ContentDialog : ContentControl
	{
		// Token: 0x060001DA RID: 474 RVA: 0x00006D90 File Offset: 0x00004F90
		static ContentDialog()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentDialog), new FrameworkPropertyMetadata(typeof(ContentDialog)));
			EventManager.RegisterClassHandler(typeof(Window), TitleBar.BackRequestedEvent, new EventHandler<BackRequestedEventArgs>(ContentDialog.OnBackRequested));
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00007150 File Offset: 0x00005350
		public ContentDialog()
		{
			this.m_closeTimer = new DispatcherTimer
			{
				Interval = TimeSpan.FromSeconds(0.6)
			};
			this.m_closeTimer.Tick += new EventHandler(this.OnCloseTimerTick);
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001DC RID: 476 RVA: 0x0000718E File Offset: 0x0000538E
		// (set) Token: 0x060001DD RID: 477 RVA: 0x0000719B File Offset: 0x0000539B
		public object Title
		{
			get
			{
				return base.GetValue(ContentDialog.TitleProperty);
			}
			set
			{
				base.SetValue(ContentDialog.TitleProperty, value);
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001DE RID: 478 RVA: 0x000071A9 File Offset: 0x000053A9
		// (set) Token: 0x060001DF RID: 479 RVA: 0x000071BB File Offset: 0x000053BB
		public DataTemplate TitleTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(ContentDialog.TitleTemplateProperty);
			}
			set
			{
				base.SetValue(ContentDialog.TitleTemplateProperty, value);
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x000071C9 File Offset: 0x000053C9
		// (set) Token: 0x060001E1 RID: 481 RVA: 0x000071DB File Offset: 0x000053DB
		public string PrimaryButtonText
		{
			get
			{
				return (string)base.GetValue(ContentDialog.PrimaryButtonTextProperty);
			}
			set
			{
				base.SetValue(ContentDialog.PrimaryButtonTextProperty, value);
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001E2 RID: 482 RVA: 0x000071E9 File Offset: 0x000053E9
		// (set) Token: 0x060001E3 RID: 483 RVA: 0x000071FB File Offset: 0x000053FB
		public ICommand PrimaryButtonCommand
		{
			get
			{
				return (ICommand)base.GetValue(ContentDialog.PrimaryButtonCommandProperty);
			}
			set
			{
				base.SetValue(ContentDialog.PrimaryButtonCommandProperty, value);
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x00007209 File Offset: 0x00005409
		// (set) Token: 0x060001E5 RID: 485 RVA: 0x00007216 File Offset: 0x00005416
		public object PrimaryButtonCommandParameter
		{
			get
			{
				return base.GetValue(ContentDialog.PrimaryButtonCommandParameterProperty);
			}
			set
			{
				base.SetValue(ContentDialog.PrimaryButtonCommandParameterProperty, value);
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x00007224 File Offset: 0x00005424
		// (set) Token: 0x060001E7 RID: 487 RVA: 0x00007236 File Offset: 0x00005436
		public Style PrimaryButtonStyle
		{
			get
			{
				return (Style)base.GetValue(ContentDialog.PrimaryButtonStyleProperty);
			}
			set
			{
				base.SetValue(ContentDialog.PrimaryButtonStyleProperty, value);
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x00007244 File Offset: 0x00005444
		// (set) Token: 0x060001E9 RID: 489 RVA: 0x00007256 File Offset: 0x00005456
		public bool IsPrimaryButtonEnabled
		{
			get
			{
				return (bool)base.GetValue(ContentDialog.IsPrimaryButtonEnabledProperty);
			}
			set
			{
				base.SetValue(ContentDialog.IsPrimaryButtonEnabledProperty, value);
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060001EA RID: 490 RVA: 0x00007269 File Offset: 0x00005469
		// (set) Token: 0x060001EB RID: 491 RVA: 0x0000727B File Offset: 0x0000547B
		public string SecondaryButtonText
		{
			get
			{
				return (string)base.GetValue(ContentDialog.SecondaryButtonTextProperty);
			}
			set
			{
				base.SetValue(ContentDialog.SecondaryButtonTextProperty, value);
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060001EC RID: 492 RVA: 0x00007289 File Offset: 0x00005489
		// (set) Token: 0x060001ED RID: 493 RVA: 0x0000729B File Offset: 0x0000549B
		public ICommand SecondaryButtonCommand
		{
			get
			{
				return (ICommand)base.GetValue(ContentDialog.SecondaryButtonCommandProperty);
			}
			set
			{
				base.SetValue(ContentDialog.SecondaryButtonCommandProperty, value);
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060001EE RID: 494 RVA: 0x000072A9 File Offset: 0x000054A9
		// (set) Token: 0x060001EF RID: 495 RVA: 0x000072B6 File Offset: 0x000054B6
		public object SecondaryButtonCommandParameter
		{
			get
			{
				return base.GetValue(ContentDialog.SecondaryButtonCommandParameterProperty);
			}
			set
			{
				base.SetValue(ContentDialog.SecondaryButtonCommandParameterProperty, value);
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x000072C4 File Offset: 0x000054C4
		// (set) Token: 0x060001F1 RID: 497 RVA: 0x000072D6 File Offset: 0x000054D6
		public Style SecondaryButtonStyle
		{
			get
			{
				return (Style)base.GetValue(ContentDialog.SecondaryButtonStyleProperty);
			}
			set
			{
				base.SetValue(ContentDialog.SecondaryButtonStyleProperty, value);
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x000072E4 File Offset: 0x000054E4
		// (set) Token: 0x060001F3 RID: 499 RVA: 0x000072F6 File Offset: 0x000054F6
		public bool IsSecondaryButtonEnabled
		{
			get
			{
				return (bool)base.GetValue(ContentDialog.IsSecondaryButtonEnabledProperty);
			}
			set
			{
				base.SetValue(ContentDialog.IsSecondaryButtonEnabledProperty, value);
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x00007309 File Offset: 0x00005509
		// (set) Token: 0x060001F5 RID: 501 RVA: 0x0000731B File Offset: 0x0000551B
		public string CloseButtonText
		{
			get
			{
				return (string)base.GetValue(ContentDialog.CloseButtonTextProperty);
			}
			set
			{
				base.SetValue(ContentDialog.CloseButtonTextProperty, value);
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x00007329 File Offset: 0x00005529
		// (set) Token: 0x060001F7 RID: 503 RVA: 0x0000733B File Offset: 0x0000553B
		public ICommand CloseButtonCommand
		{
			get
			{
				return (ICommand)base.GetValue(ContentDialog.CloseButtonCommandProperty);
			}
			set
			{
				base.SetValue(ContentDialog.CloseButtonCommandProperty, value);
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x00007349 File Offset: 0x00005549
		// (set) Token: 0x060001F9 RID: 505 RVA: 0x00007356 File Offset: 0x00005556
		public object CloseButtonCommandParameter
		{
			get
			{
				return base.GetValue(ContentDialog.CloseButtonCommandParameterProperty);
			}
			set
			{
				base.SetValue(ContentDialog.CloseButtonCommandParameterProperty, value);
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060001FA RID: 506 RVA: 0x00007364 File Offset: 0x00005564
		// (set) Token: 0x060001FB RID: 507 RVA: 0x00007376 File Offset: 0x00005576
		public Style CloseButtonStyle
		{
			get
			{
				return (Style)base.GetValue(ContentDialog.CloseButtonStyleProperty);
			}
			set
			{
				base.SetValue(ContentDialog.CloseButtonStyleProperty, value);
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060001FC RID: 508 RVA: 0x00007384 File Offset: 0x00005584
		// (set) Token: 0x060001FD RID: 509 RVA: 0x00007396 File Offset: 0x00005596
		public ContentDialogButton DefaultButton
		{
			get
			{
				return (ContentDialogButton)base.GetValue(ContentDialog.DefaultButtonProperty);
			}
			set
			{
				base.SetValue(ContentDialog.DefaultButtonProperty, value);
			}
		}

		// Token: 0x060001FE RID: 510 RVA: 0x000073A9 File Offset: 0x000055A9
		private static void OnDefaultButtonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ContentDialog)d).UpdateDefaultButtonStates(true);
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060001FF RID: 511 RVA: 0x000073B7 File Offset: 0x000055B7
		// (set) Token: 0x06000200 RID: 512 RVA: 0x000073C9 File Offset: 0x000055C9
		public bool FullSizeDesired
		{
			get
			{
				return (bool)base.GetValue(ContentDialog.FullSizeDesiredProperty);
			}
			set
			{
				base.SetValue(ContentDialog.FullSizeDesiredProperty, value);
			}
		}

		// Token: 0x06000201 RID: 513 RVA: 0x000073DC File Offset: 0x000055DC
		private static void OnFullSizeDesiredChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ContentDialog)d).UpdateVisualStates(true);
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000202 RID: 514 RVA: 0x000073EA File Offset: 0x000055EA
		// (set) Token: 0x06000203 RID: 515 RVA: 0x000073FC File Offset: 0x000055FC
		public CornerRadius CornerRadius
		{
			get
			{
				return (CornerRadius)base.GetValue(ContentDialog.CornerRadiusProperty);
			}
			set
			{
				base.SetValue(ContentDialog.CornerRadiusProperty, value);
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000204 RID: 516 RVA: 0x0000740F File Offset: 0x0000560F
		// (set) Token: 0x06000205 RID: 517 RVA: 0x00007421 File Offset: 0x00005621
		public bool IsShadowEnabled
		{
			get
			{
				return (bool)base.GetValue(ContentDialog.IsShadowEnabledProperty);
			}
			set
			{
				base.SetValue(ContentDialog.IsShadowEnabledProperty, value);
			}
		}

		// Token: 0x06000206 RID: 518 RVA: 0x00007434 File Offset: 0x00005634
		private static ContentDialog GetOpenDialog(Window window)
		{
			return (ContentDialog)window.GetValue(ContentDialog.OpenDialogProperty);
		}

		// Token: 0x06000207 RID: 519 RVA: 0x00007446 File Offset: 0x00005646
		private static void SetOpenDialog(Window window, ContentDialog value)
		{
			window.SetValue(ContentDialog.OpenDialogProperty, value);
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000208 RID: 520 RVA: 0x00007454 File Offset: 0x00005654
		// (set) Token: 0x06000209 RID: 521 RVA: 0x0000745C File Offset: 0x0000565C
		public Window Owner { get; set; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600020A RID: 522 RVA: 0x00007465 File Offset: 0x00005665
		private Window ActualOwner
		{
			get
			{
				return this.Owner ?? SharedHelpers.GetActiveWindow();
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600020B RID: 523 RVA: 0x00007476 File Offset: 0x00005676
		// (set) Token: 0x0600020C RID: 524 RVA: 0x0000747E File Offset: 0x0000567E
		private Border Container { get; set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600020D RID: 525 RVA: 0x00007487 File Offset: 0x00005687
		// (set) Token: 0x0600020E RID: 526 RVA: 0x0000748F File Offset: 0x0000568F
		private FrameworkElement LayoutRoot { get; set; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600020F RID: 527 RVA: 0x00007498 File Offset: 0x00005698
		// (set) Token: 0x06000210 RID: 528 RVA: 0x000074A0 File Offset: 0x000056A0
		private Button PrimaryButton { get; set; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000211 RID: 529 RVA: 0x000074A9 File Offset: 0x000056A9
		// (set) Token: 0x06000212 RID: 530 RVA: 0x000074B1 File Offset: 0x000056B1
		private Button SecondaryButton { get; set; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000213 RID: 531 RVA: 0x000074BA File Offset: 0x000056BA
		// (set) Token: 0x06000214 RID: 532 RVA: 0x000074C2 File Offset: 0x000056C2
		private Button CloseButton { get; set; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000215 RID: 533 RVA: 0x000074CB File Offset: 0x000056CB
		// (set) Token: 0x06000216 RID: 534 RVA: 0x000074D4 File Offset: 0x000056D4
		private bool IsShowing
		{
			get
			{
				return this.m_isShowing;
			}
			set
			{
				if (this.m_isShowing != value)
				{
					this.m_isShowing = value;
					this.m_opening = this.m_isShowing;
					if (this.m_isShowing)
					{
						if (Keyboard.FocusedElement != null)
						{
							this.m_weakRefToPreviousFocus = new WeakReference<IInputElement>(Keyboard.FocusedElement);
						}
					}
					else
					{
						if (this.m_isShowingInPlace)
						{
							this.m_isShowingInPlace = false;
						}
						else if (this.m_openDialogOwner != null)
						{
							this.m_openDialogOwner.ClearValue(ContentDialog.OpenDialogProperty);
							this.m_openDialogOwner = null;
						}
						this.m_closeTimer.Start();
						if (this.m_weakRefToPreviousFocus != null)
						{
							IInputElement inputElement;
							if (this.m_weakRefToPreviousFocus.TryGetTarget(ref inputElement))
							{
								inputElement.Focus();
							}
							this.m_weakRefToPreviousFocus = null;
						}
					}
					this.UpdateDialogShowingStates(true);
				}
			}
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000217 RID: 535 RVA: 0x00007588 File Offset: 0x00005788
		// (remove) Token: 0x06000218 RID: 536 RVA: 0x000075C0 File Offset: 0x000057C0
		public event TypedEventHandler<ContentDialog, ContentDialogOpenedEventArgs> Opened;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000219 RID: 537 RVA: 0x000075F8 File Offset: 0x000057F8
		// (remove) Token: 0x0600021A RID: 538 RVA: 0x00007630 File Offset: 0x00005830
		public event TypedEventHandler<ContentDialog, ContentDialogClosingEventArgs> Closing;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x0600021B RID: 539 RVA: 0x00007668 File Offset: 0x00005868
		// (remove) Token: 0x0600021C RID: 540 RVA: 0x000076A0 File Offset: 0x000058A0
		public event TypedEventHandler<ContentDialog, ContentDialogClosedEventArgs> Closed;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x0600021D RID: 541 RVA: 0x000076D8 File Offset: 0x000058D8
		// (remove) Token: 0x0600021E RID: 542 RVA: 0x00007710 File Offset: 0x00005910
		public event TypedEventHandler<ContentDialog, ContentDialogButtonClickEventArgs> PrimaryButtonClick;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x0600021F RID: 543 RVA: 0x00007748 File Offset: 0x00005948
		// (remove) Token: 0x06000220 RID: 544 RVA: 0x00007780 File Offset: 0x00005980
		public event TypedEventHandler<ContentDialog, ContentDialogButtonClickEventArgs> SecondaryButtonClick;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000221 RID: 545 RVA: 0x000077B8 File Offset: 0x000059B8
		// (remove) Token: 0x06000222 RID: 546 RVA: 0x000077F0 File Offset: 0x000059F0
		public event TypedEventHandler<ContentDialog, ContentDialogButtonClickEventArgs> CloseButtonClick;

		// Token: 0x06000223 RID: 547 RVA: 0x00007828 File Offset: 0x00005A28
		public Task<ContentDialogResult> ShowAsync()
		{
			ContentDialog.<ShowAsync>d__134 <ShowAsync>d__;
			<ShowAsync>d__.<>t__builder = AsyncTaskMethodBuilder<ContentDialogResult>.Create();
			<ShowAsync>d__.<>4__this = this;
			<ShowAsync>d__.<>1__state = -1;
			<ShowAsync>d__.<>t__builder.Start<ContentDialog.<ShowAsync>d__134>(ref <ShowAsync>d__);
			return <ShowAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000786B File Offset: 0x00005A6B
		public Task<ContentDialogResult> ShowAsync(ContentDialogPlacement placement)
		{
			if (placement == ContentDialogPlacement.InPlace && base.Parent != null)
			{
				if (this.IsShowing)
				{
					ContentDialog.ThrowAlreadyOpenException();
				}
				this.RemovePopup();
				this.IsShowing = true;
				this.m_isShowingInPlace = true;
				return this.CreateAsyncOperation();
			}
			return this.ShowAsync();
		}

		// Token: 0x06000225 RID: 549 RVA: 0x000078A7 File Offset: 0x00005AA7
		public void Hide()
		{
			this.Hide(ContentDialogResult.None);
		}

		// Token: 0x06000226 RID: 550 RVA: 0x000078B0 File Offset: 0x00005AB0
		public override void OnApplyTemplate()
		{
			if (this.LayoutRoot != null)
			{
				this.LayoutRoot.IsVisibleChanged -= new DependencyPropertyChangedEventHandler(this.OnLayoutRootIsVisibleChanged);
				this.LayoutRoot.Loaded -= new RoutedEventHandler(this.OnLayoutRootLoaded);
				this.LayoutRoot.KeyDown -= new KeyEventHandler(this.OnLayoutRootKeyDown);
			}
			if (this.PrimaryButton != null)
			{
				this.PrimaryButton.Click -= new RoutedEventHandler(this.OnButtonClick);
			}
			if (this.SecondaryButton != null)
			{
				this.SecondaryButton.Click -= new RoutedEventHandler(this.OnButtonClick);
			}
			if (this.CloseButton != null)
			{
				this.CloseButton.Click -= new RoutedEventHandler(this.OnButtonClick);
			}
			base.OnApplyTemplate();
			this.Container = (base.GetTemplateChild("Container") as Border);
			this.LayoutRoot = (base.GetTemplateChild("LayoutRoot") as FrameworkElement);
			this.PrimaryButton = (base.GetTemplateChild("PrimaryButton") as Button);
			this.SecondaryButton = (base.GetTemplateChild("SecondaryButton") as Button);
			this.CloseButton = (base.GetTemplateChild("CloseButton") as Button);
			if (this.LayoutRoot != null)
			{
				this.LayoutRoot.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.OnLayoutRootIsVisibleChanged);
				this.LayoutRoot.Loaded += new RoutedEventHandler(this.OnLayoutRootLoaded);
				this.LayoutRoot.KeyDown += new KeyEventHandler(this.OnLayoutRootKeyDown);
			}
			if (this.PrimaryButton != null)
			{
				this.PrimaryButton.Click += new RoutedEventHandler(this.OnButtonClick);
			}
			if (this.SecondaryButton != null)
			{
				this.SecondaryButton.Click += new RoutedEventHandler(this.OnButtonClick);
			}
			if (this.CloseButton != null)
			{
				this.CloseButton.Click += new RoutedEventHandler(this.OnButtonClick);
			}
			this.UpdateVisualStates(false);
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00007A8C File Offset: 0x00005C8C
		protected override void OnKeyDown(KeyEventArgs e)
		{
			this.HandleKeyDown(e);
			base.OnKeyDown(e);
		}

		// Token: 0x06000228 RID: 552 RVA: 0x00007A9C File Offset: 0x00005C9C
		private void Hide(ContentDialogResult result)
		{
			if (this.IsShowing)
			{
				this.OnOpened();
				TypedEventHandler<ContentDialog, ContentDialogClosingEventArgs> closing = this.Closing;
				if (closing != null)
				{
					ContentDialogClosingEventArgs args = new ContentDialogClosingEventArgs(result);
					ContentDialogClosingDeferral deferral = new ContentDialogClosingDeferral(delegate
					{
						if (!args.Cancel)
						{
							this.m_result = result;
							this.IsShowing = false;
						}
					});
					args.SetDeferral(deferral);
					args.IncrementDeferralCount();
					closing.Invoke(this, args);
					args.DecrementDeferralCount();
					return;
				}
				this.m_result = result;
				this.IsShowing = false;
			}
		}

		// Token: 0x06000229 RID: 553 RVA: 0x00007B3C File Offset: 0x00005D3C
		private void OnButtonClick(object sender, RoutedEventArgs e)
		{
			if (sender == this.PrimaryButton)
			{
				this.HandleButtonClick(this.PrimaryButtonClick, this.PrimaryButtonCommand, this.PrimaryButtonCommandParameter, ContentDialogResult.Primary);
				return;
			}
			if (sender == this.SecondaryButton)
			{
				this.HandleButtonClick(this.SecondaryButtonClick, this.SecondaryButtonCommand, this.SecondaryButtonCommandParameter, ContentDialogResult.Secondary);
				return;
			}
			if (sender == this.CloseButton)
			{
				this.HandleButtonClick(this.CloseButtonClick, this.CloseButtonCommand, this.CloseButtonCommandParameter, ContentDialogResult.None);
			}
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00007BB4 File Offset: 0x00005DB4
		private void HandleButtonClick(TypedEventHandler<ContentDialog, ContentDialogButtonClickEventArgs> handler, ICommand command, object commandParameter, ContentDialogResult result)
		{
			if (!this.IsShowing)
			{
				return;
			}
			if (handler != null)
			{
				ContentDialogButtonClickEventArgs args = new ContentDialogButtonClickEventArgs();
				ContentDialogButtonClickDeferral deferral = new ContentDialogButtonClickDeferral(delegate
				{
					if (!args.Cancel)
					{
						ContentDialog.TryExecuteCommand(command, commandParameter);
						this.Hide(result);
					}
				});
				args.SetDeferral(deferral);
				args.IncrementDeferralCount();
				handler.Invoke(this, args);
				args.DecrementDeferralCount();
				return;
			}
			ContentDialog.TryExecuteCommand(command, commandParameter);
			this.Hide(result);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x00007C5A File Offset: 0x00005E5A
		private void OnLayoutRootLoaded(object sender, RoutedEventArgs e)
		{
			this.UpdateVisualStates(true);
		}

		// Token: 0x0600022C RID: 556 RVA: 0x00007C64 File Offset: 0x00005E64
		private void OnLayoutRootIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if ((bool)e.NewValue)
			{
				if (this.LayoutRoot.Parent is Popup)
				{
					this.LayoutRoot.Focusable = true;
					this.LayoutRoot.Focus();
				}
				else
				{
					this.LayoutRoot.Focusable = false;
					base.Focus();
				}
				this.OnOpened();
				return;
			}
			this.m_closeTimer.Stop();
			this.OnClosed();
		}

		// Token: 0x0600022D RID: 557 RVA: 0x00007CD6 File Offset: 0x00005ED6
		private void OnLayoutRootKeyDown(object sender, KeyEventArgs e)
		{
			this.HandleKeyDown(e);
		}

		// Token: 0x0600022E RID: 558 RVA: 0x00007CDF File Offset: 0x00005EDF
		private void OnCloseTimerTick(object sender, EventArgs e)
		{
			this.m_closeTimer.Stop();
			this.UpdateVisualStates(false);
			this.OnClosed();
		}

		// Token: 0x0600022F RID: 559 RVA: 0x00007CF9 File Offset: 0x00005EF9
		private void OnOpened()
		{
			if (this.m_opening)
			{
				this.m_opening = false;
				TypedEventHandler<ContentDialog, ContentDialogOpenedEventArgs> opened = this.Opened;
				if (opened == null)
				{
					return;
				}
				opened.Invoke(this, new ContentDialogOpenedEventArgs());
			}
		}

		// Token: 0x06000230 RID: 560 RVA: 0x00007D20 File Offset: 0x00005F20
		private void OnClosed()
		{
			if (this.m_adornerLayer != null)
			{
				ContentDialog.RestoreKeyboardNavigation(this.m_adorner.AdornedElement);
				this.m_adornerLayer.Remove(this.m_adorner);
				this.m_adornerLayer = null;
			}
			if (this.m_showTcs != null)
			{
				TypedEventHandler<ContentDialog, ContentDialogClosedEventArgs> closed = this.Closed;
				if (closed != null)
				{
					closed.Invoke(this, new ContentDialogClosedEventArgs(this.m_result));
				}
				this.m_showTcs.TrySetResult(this.m_result);
				this.m_showTcs = null;
				this.m_result = ContentDialogResult.None;
			}
		}

		// Token: 0x06000231 RID: 561 RVA: 0x00007DA4 File Offset: 0x00005FA4
		private void HandleKeyDown(KeyEventArgs e)
		{
			Key key = e.Key;
			if (key != 6)
			{
				if (key != 13)
				{
					return;
				}
				this.Hide();
				e.Handled = true;
			}
			else if (this.IsShowing)
			{
				Button button = null;
				switch (this.DefaultButton)
				{
				case ContentDialogButton.Primary:
					button = this.PrimaryButton;
					break;
				case ContentDialogButton.Secondary:
					button = this.SecondaryButton;
					break;
				case ContentDialogButton.Close:
					button = this.CloseButton;
					break;
				}
				if (button == null)
				{
					Button button2;
					if ((button2 = this.PrimaryButton) == null)
					{
						button2 = (this.SecondaryButton ?? this.CloseButton);
					}
					button = button2;
				}
				if (button != null && button.IsEnabled)
				{
					this.OnButtonClick(button, null);
					e.Handled = true;
					return;
				}
			}
		}

		// Token: 0x06000232 RID: 562 RVA: 0x00007E48 File Offset: 0x00006048
		private void UpdateVisualStates(bool useTransitions)
		{
			this.UpdateDialogShowingStates(useTransitions);
			VisualStateManager.GoToState(this, this.FullSizeDesired ? "FullDialogSizing" : "DefaultDialogSizing", useTransitions);
			this.UpdateButtonsVisibilityStates(useTransitions);
			this.UpdateDefaultButtonStates(useTransitions);
		}

		// Token: 0x06000233 RID: 563 RVA: 0x00007E7C File Offset: 0x0000607C
		private void UpdateDialogShowingStates(bool useTransitions)
		{
			string text = (this.IsShowing && base.IsLoaded) ? "DialogShowing" : "DialogHidden";
			if (DesignerProperties.GetIsInDesignMode(this))
			{
				text = "DialogShowing";
			}
			VisualStateManager.GoToState(this, text, useTransitions);
		}

		// Token: 0x06000234 RID: 564 RVA: 0x00007EC0 File Offset: 0x000060C0
		private void UpdateButtonsVisibilityStates(bool useTransitions)
		{
			bool flag = !string.IsNullOrEmpty(this.PrimaryButtonText);
			bool flag2 = !string.IsNullOrEmpty(this.SecondaryButtonText);
			bool flag3 = !string.IsNullOrEmpty(this.CloseButtonText);
			string text;
			if (flag && flag2 && flag3)
			{
				text = "AllVisible";
			}
			else if (!flag && !flag2 && !flag3)
			{
				text = "NoneVisible";
			}
			else if (flag && flag2)
			{
				text = "PrimaryAndSecondaryVisible";
			}
			else if (flag && flag3)
			{
				text = "PrimaryAndCloseVisible";
			}
			else if (flag2 && flag3)
			{
				text = "SecondaryAndCloseVisible";
			}
			else if (flag)
			{
				text = "PrimaryVisible";
			}
			else if (flag2)
			{
				text = "SecondaryVisible";
			}
			else if (flag3)
			{
				text = "CloseVisible";
			}
			else
			{
				text = "AllVisible";
			}
			VisualStateManager.GoToState(this, text, useTransitions);
		}

		// Token: 0x06000235 RID: 565 RVA: 0x00007F74 File Offset: 0x00006174
		private void UpdateDefaultButtonStates(bool useTransitions)
		{
			string text = "NoDefaultButton";
			switch (this.DefaultButton)
			{
			case ContentDialogButton.Primary:
				text = "PrimaryAsDefaultButton";
				break;
			case ContentDialogButton.Secondary:
				text = "SecondaryAsDefaultButton";
				break;
			case ContentDialogButton.Close:
				text = "CloseAsDefaultButton";
				break;
			}
			VisualStateManager.GoToState(this, text, useTransitions);
		}

		// Token: 0x06000236 RID: 566 RVA: 0x00007FC3 File Offset: 0x000061C3
		private void EnsureAdornerLayer(ContentPresenter contentPresenter)
		{
			this.m_adornerLayer = AdornerLayer.GetAdornerLayer(contentPresenter);
			if (this.m_adornerLayer == null)
			{
				throw new InvalidOperationException("AdornerLayer not found.");
			}
		}

		// Token: 0x06000237 RID: 567 RVA: 0x00007FE4 File Offset: 0x000061E4
		private void DisconnectAdornerChild()
		{
			if (this.m_adorner != null)
			{
				this.m_adorner.Child = null;
			}
		}

		// Token: 0x06000238 RID: 568 RVA: 0x00007FFA File Offset: 0x000061FA
		private void EnsureAdornerChild(ContentPresenter cp, UIElement child)
		{
			if (this.m_adorner == null)
			{
				this.m_adorner = new ContentDialog.ContentDialogAdorner(cp, child);
				return;
			}
			this.m_adorner.Child = child;
		}

		// Token: 0x06000239 RID: 569 RVA: 0x00008020 File Offset: 0x00006220
		private void AddPopup()
		{
			if (this.m_popup == null && this.Container != null && this.LayoutRoot != null)
			{
				this.Container.Child = null;
				this.m_popup = new Popup
				{
					Child = this.LayoutRoot
				};
				this.Container.Child = this.m_popup;
			}
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000807C File Offset: 0x0000627C
		private void RemovePopup()
		{
			if (this.m_popup != null && this.Container != null && this.LayoutRoot != null)
			{
				this.m_popup.Child = null;
				this.m_popup = null;
				this.DisconnectAdornerChild();
				this.Container.Child = this.LayoutRoot;
			}
		}

		// Token: 0x0600023B RID: 571 RVA: 0x000080CC File Offset: 0x000062CC
		private static void OnBackRequested(object sender, BackRequestedEventArgs e)
		{
			Window window = e.Source as Window;
			if (window != null)
			{
				ContentDialog openDialog = ContentDialog.GetOpenDialog(window);
				if (openDialog != null)
				{
					e.Handled = true;
					openDialog.Hide();
				}
			}
		}

		// Token: 0x0600023C RID: 572 RVA: 0x000080FF File Offset: 0x000062FF
		private void OnApplicationActivated(object sender, EventArgs e)
		{
			Application.Current.Activated -= new EventHandler(this.OnApplicationActivated);
			if (this.m_activatedTcs != null)
			{
				this.m_activatedTcs.TrySetResult(true);
				this.m_activatedTcs = null;
			}
		}

		// Token: 0x0600023D RID: 573 RVA: 0x00008133 File Offset: 0x00006333
		private void OnOwnerActivated(object sender, EventArgs e)
		{
			((Window)sender).Activated -= new EventHandler(this.OnOwnerActivated);
			if (this.m_activatedTcs != null)
			{
				this.m_activatedTcs.TrySetResult(true);
				this.m_activatedTcs = null;
			}
		}

		// Token: 0x0600023E RID: 574 RVA: 0x00008168 File Offset: 0x00006368
		private Task WaitUntilApplicationActivated()
		{
			this.m_activatedTcs = new TaskCompletionSource<bool>();
			Application.Current.Activated += new EventHandler(this.OnApplicationActivated);
			return this.m_activatedTcs.Task;
		}

		// Token: 0x0600023F RID: 575 RVA: 0x00008196 File Offset: 0x00006396
		private Task WaitUntilOwnerActivated(Window owner)
		{
			this.m_activatedTcs = new TaskCompletionSource<bool>();
			owner.Activated += new EventHandler(this.OnOwnerActivated);
			return this.m_activatedTcs.Task;
		}

		// Token: 0x06000240 RID: 576 RVA: 0x000081C0 File Offset: 0x000063C0
		private static void OnButtonTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ContentDialog)d).UpdateButtonsVisibilityStates(true);
		}

		// Token: 0x06000241 RID: 577 RVA: 0x000081CE File Offset: 0x000063CE
		private static void TryExecuteCommand(ICommand command, object parameter)
		{
			if (command != null && command.CanExecute(parameter))
			{
				command.Execute(parameter);
			}
		}

		// Token: 0x06000242 RID: 578 RVA: 0x000081E3 File Offset: 0x000063E3
		private void ThrowIfHasOpenDialog(Window owner)
		{
			if (ContentDialog.GetOpenDialog(owner) != null)
			{
				ContentDialog.ThrowAlreadyOpenException();
			}
		}

		// Token: 0x06000243 RID: 579 RVA: 0x000081F2 File Offset: 0x000063F2
		private static void ThrowAlreadyOpenException()
		{
			throw new InvalidOperationException("Only a single ContentDialog can be open at any time.");
		}

		// Token: 0x06000244 RID: 580 RVA: 0x00008200 File Offset: 0x00006400
		private static ContentPresenter FindContentPresenter(Window window)
		{
			ContentPresenter contentPresenter = null;
			UIElement uielement = window.Content as UIElement;
			if (uielement != null)
			{
				contentPresenter = (VisualTreeHelper.GetParent(uielement) as ContentPresenter);
			}
			if (contentPresenter == null)
			{
				AdornerDecorator adornerDecorator = VisualTree.FindDescendant<AdornerDecorator>(window);
				if (adornerDecorator != null)
				{
					contentPresenter = VisualTree.FindDescendant<ContentPresenter>(adornerDecorator);
				}
			}
			return contentPresenter;
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000823F File Offset: 0x0000643F
		private Task<ContentDialogResult> CreateAsyncOperation()
		{
			this.m_showTcs = new TaskCompletionSource<ContentDialogResult>();
			return this.m_showTcs.Task;
		}

		// Token: 0x06000246 RID: 582 RVA: 0x00008257 File Offset: 0x00006457
		private static void DisableKeyboardNavigation(DependencyObject element)
		{
			KeyboardNavigation.SetDirectionalNavigation(element, 3);
			KeyboardNavigation.SetTabNavigation(element, 3);
			KeyboardNavigation.SetControlTabNavigation(element, 3);
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000826E File Offset: 0x0000646E
		private static void RestoreKeyboardNavigation(UIElement element)
		{
			element.ClearValue(KeyboardNavigation.DirectionalNavigationProperty);
			element.ClearValue(KeyboardNavigation.TabNavigationProperty);
			element.ClearValue(KeyboardNavigation.ControlTabNavigationProperty);
		}

		// Token: 0x04000091 RID: 145
		public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(object), typeof(ContentDialog), null);

		// Token: 0x04000092 RID: 146
		public static readonly DependencyProperty TitleTemplateProperty = DependencyProperty.Register("TitleTemplate", typeof(DataTemplate), typeof(ContentDialog), null);

		// Token: 0x04000093 RID: 147
		public static readonly DependencyProperty PrimaryButtonTextProperty = DependencyProperty.Register("PrimaryButtonText", typeof(string), typeof(ContentDialog), new PropertyMetadata(string.Empty, new PropertyChangedCallback(ContentDialog.OnButtonTextChanged)));

		// Token: 0x04000094 RID: 148
		public static readonly DependencyProperty PrimaryButtonCommandProperty = DependencyProperty.Register("PrimaryButtonCommand", typeof(ICommand), typeof(ContentDialog), null);

		// Token: 0x04000095 RID: 149
		public static readonly DependencyProperty PrimaryButtonCommandParameterProperty = DependencyProperty.Register("PrimaryButtonCommandParameter", typeof(object), typeof(ContentDialog), null);

		// Token: 0x04000096 RID: 150
		public static readonly DependencyProperty PrimaryButtonStyleProperty = DependencyProperty.Register("PrimaryButtonStyle", typeof(Style), typeof(ContentDialog), null);

		// Token: 0x04000097 RID: 151
		public static readonly DependencyProperty IsPrimaryButtonEnabledProperty = DependencyProperty.Register("IsPrimaryButtonEnabled", typeof(bool), typeof(ContentDialog), new PropertyMetadata(true));

		// Token: 0x04000098 RID: 152
		public static readonly DependencyProperty SecondaryButtonTextProperty = DependencyProperty.Register("SecondaryButtonText", typeof(string), typeof(ContentDialog), new PropertyMetadata(string.Empty, new PropertyChangedCallback(ContentDialog.OnButtonTextChanged)));

		// Token: 0x04000099 RID: 153
		public static readonly DependencyProperty SecondaryButtonCommandProperty = DependencyProperty.Register("SecondaryButtonCommand", typeof(ICommand), typeof(ContentDialog), null);

		// Token: 0x0400009A RID: 154
		public static readonly DependencyProperty SecondaryButtonCommandParameterProperty = DependencyProperty.Register("SecondaryButtonCommandParameter", typeof(object), typeof(ContentDialog), null);

		// Token: 0x0400009B RID: 155
		public static readonly DependencyProperty SecondaryButtonStyleProperty = DependencyProperty.Register("SecondaryButtonStyle", typeof(Style), typeof(ContentDialog), null);

		// Token: 0x0400009C RID: 156
		public static readonly DependencyProperty IsSecondaryButtonEnabledProperty = DependencyProperty.Register("IsSecondaryButtonEnabled", typeof(bool), typeof(ContentDialog), new PropertyMetadata(true));

		// Token: 0x0400009D RID: 157
		public static readonly DependencyProperty CloseButtonTextProperty = DependencyProperty.Register("CloseButtonText", typeof(string), typeof(ContentDialog), new PropertyMetadata(string.Empty, new PropertyChangedCallback(ContentDialog.OnButtonTextChanged)));

		// Token: 0x0400009E RID: 158
		public static readonly DependencyProperty CloseButtonCommandProperty = DependencyProperty.Register("CloseButtonCommand", typeof(ICommand), typeof(ContentDialog), null);

		// Token: 0x0400009F RID: 159
		public static readonly DependencyProperty CloseButtonCommandParameterProperty = DependencyProperty.Register("CloseButtonCommandParameter", typeof(object), typeof(ContentDialog), null);

		// Token: 0x040000A0 RID: 160
		public static readonly DependencyProperty CloseButtonStyleProperty = DependencyProperty.Register("CloseButtonStyle", typeof(Style), typeof(ContentDialog), null);

		// Token: 0x040000A1 RID: 161
		public static readonly DependencyProperty DefaultButtonProperty = DependencyProperty.Register("DefaultButton", typeof(ContentDialogButton), typeof(ContentDialog), new PropertyMetadata(new PropertyChangedCallback(ContentDialog.OnDefaultButtonChanged)));

		// Token: 0x040000A2 RID: 162
		public static readonly DependencyProperty FullSizeDesiredProperty = DependencyProperty.Register("FullSizeDesired", typeof(bool), typeof(ContentDialog), new PropertyMetadata(new PropertyChangedCallback(ContentDialog.OnFullSizeDesiredChanged)));

		// Token: 0x040000A3 RID: 163
		public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(ContentDialog), null);

		// Token: 0x040000A4 RID: 164
		public static readonly DependencyProperty IsShadowEnabledProperty = DependencyProperty.Register("IsShadowEnabled", typeof(bool), typeof(ContentDialog), new FrameworkPropertyMetadata(false));

		// Token: 0x040000A5 RID: 165
		private static readonly DependencyProperty OpenDialogProperty = DependencyProperty.RegisterAttached("OpenDialog", typeof(ContentDialog), typeof(ContentDialog));

		// Token: 0x040000B2 RID: 178
		private const string DialogShowingStatesGroup = "DialogShowingStates";

		// Token: 0x040000B3 RID: 179
		private const string DialogHiddenState = "DialogHidden";

		// Token: 0x040000B4 RID: 180
		private const string DialogShowingState = "DialogShowing";

		// Token: 0x040000B5 RID: 181
		private const string DialogShowingWithoutSmokeLayerState = "DialogShowingWithoutSmokeLayer";

		// Token: 0x040000B6 RID: 182
		private const string DialogSizingStatesGroup = "DialogSizingStates";

		// Token: 0x040000B7 RID: 183
		private const string DefaultDialogSizingState = "DefaultDialogSizing";

		// Token: 0x040000B8 RID: 184
		private const string FullDialogSizingState = "FullDialogSizing";

		// Token: 0x040000B9 RID: 185
		private const string ButtonsVisibilityStatesGroup = "ButtonsVisibilityStates";

		// Token: 0x040000BA RID: 186
		private const string AllVisibleState = "AllVisible";

		// Token: 0x040000BB RID: 187
		private const string NoneVisibleState = "NoneVisible";

		// Token: 0x040000BC RID: 188
		private const string PrimaryVisibleState = "PrimaryVisible";

		// Token: 0x040000BD RID: 189
		private const string SecondaryVisibleState = "SecondaryVisible";

		// Token: 0x040000BE RID: 190
		private const string CloseVisibleState = "CloseVisible";

		// Token: 0x040000BF RID: 191
		private const string PrimaryAndSecondaryVisibleState = "PrimaryAndSecondaryVisible";

		// Token: 0x040000C0 RID: 192
		private const string PrimaryAndCloseVisibleState = "PrimaryAndCloseVisible";

		// Token: 0x040000C1 RID: 193
		private const string SecondaryAndCloseVisibleState = "SecondaryAndCloseVisible";

		// Token: 0x040000C2 RID: 194
		private const string DefaultButtonStatesGroup = "DefaultButtonStates";

		// Token: 0x040000C3 RID: 195
		private const string NoDefaultButtonState = "NoDefaultButton";

		// Token: 0x040000C4 RID: 196
		private const string PrimaryAsDefaultButtonState = "PrimaryAsDefaultButton";

		// Token: 0x040000C5 RID: 197
		private const string SecondaryAsDefaultButtonState = "SecondaryAsDefaultButton";

		// Token: 0x040000C6 RID: 198
		private const string CloseAsDefaultButtonState = "CloseAsDefaultButton";

		// Token: 0x040000C7 RID: 199
		private const string DialogBorderStatesGroup = "DialogBorderStates";

		// Token: 0x040000C8 RID: 200
		private const string NoBorderState = "NoBorder";

		// Token: 0x040000C9 RID: 201
		private const string AccentColorBorderState = "AccentColorBorder";

		// Token: 0x040000CA RID: 202
		private TaskCompletionSource<ContentDialogResult> m_showTcs;

		// Token: 0x040000CB RID: 203
		private TaskCompletionSource<bool> m_activatedTcs;

		// Token: 0x040000CC RID: 204
		private ContentDialog.ContentDialogAdorner m_adorner;

		// Token: 0x040000CD RID: 205
		private AdornerLayer m_adornerLayer;

		// Token: 0x040000CE RID: 206
		private Popup m_popup;

		// Token: 0x040000CF RID: 207
		private bool m_opening;

		// Token: 0x040000D0 RID: 208
		private bool m_isShowing;

		// Token: 0x040000D1 RID: 209
		private bool m_isShowingInPlace;

		// Token: 0x040000D2 RID: 210
		private Window m_openDialogOwner;

		// Token: 0x040000D3 RID: 211
		private ContentDialogResult m_result;

		// Token: 0x040000D4 RID: 212
		private readonly DispatcherTimer m_closeTimer;

		// Token: 0x040000D5 RID: 213
		private WeakReference<IInputElement> m_weakRefToPreviousFocus;

		// Token: 0x020000F7 RID: 247
		private class ContentDialogAdorner : Adorner
		{
			// Token: 0x06000C96 RID: 3222 RVA: 0x0002AEED File Offset: 0x000290ED
			public ContentDialogAdorner(UIElement adornedElement, UIElement child) : base(adornedElement)
			{
				if (child == null)
				{
					throw new ArgumentNullException("child");
				}
				this.Child = child;
			}

			// Token: 0x17000230 RID: 560
			// (get) Token: 0x06000C97 RID: 3223 RVA: 0x0002AF0C File Offset: 0x0002910C
			// (set) Token: 0x06000C98 RID: 3224 RVA: 0x0002AF14 File Offset: 0x00029114
			public UIElement Child
			{
				get
				{
					return this._child;
				}
				set
				{
					if (this._child != value)
					{
						if (this._child != null)
						{
							base.RemoveVisualChild(this._child);
						}
						this._child = value;
						if (this._child != null)
						{
							base.AddVisualChild(this._child);
						}
					}
				}
			}

			// Token: 0x17000231 RID: 561
			// (get) Token: 0x06000C99 RID: 3225 RVA: 0x0002AF4E File Offset: 0x0002914E
			protected override int VisualChildrenCount
			{
				get
				{
					if (this._child == null)
					{
						return 0;
					}
					return 1;
				}
			}

			// Token: 0x06000C9A RID: 3226 RVA: 0x0002AF5B File Offset: 0x0002915B
			protected override Visual GetVisualChild(int index)
			{
				if (index == 0 && this._child != null)
				{
					return this._child;
				}
				throw new ArgumentOutOfRangeException("index");
			}

			// Token: 0x06000C9B RID: 3227 RVA: 0x0002AF79 File Offset: 0x00029179
			protected override Size MeasureOverride(Size constraint)
			{
				Size result = constraint = base.AdornedElement.RenderSize;
				UIElement child = this.Child;
				if (child == null)
				{
					return result;
				}
				child.Measure(constraint);
				return result;
			}

			// Token: 0x06000C9C RID: 3228 RVA: 0x0002AF9C File Offset: 0x0002919C
			protected override Size ArrangeOverride(Size size)
			{
				Size size2 = base.ArrangeOverride(size);
				UIElement child = this.Child;
				if (child != null)
				{
					child.Arrange(new Rect(default(Point), size2));
				}
				return size2;
			}

			// Token: 0x04000558 RID: 1368
			private UIElement _child;
		}
	}
}
