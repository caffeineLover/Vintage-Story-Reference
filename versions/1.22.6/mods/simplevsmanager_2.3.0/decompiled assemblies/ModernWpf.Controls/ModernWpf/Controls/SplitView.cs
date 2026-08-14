using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using ModernWpf.Controls.Primitives;

namespace ModernWpf.Controls
{
	// Token: 0x020000DC RID: 220
	[ContentProperty("Content")]
	public class SplitView : Control
	{
		// Token: 0x06000ACE RID: 2766 RVA: 0x00024F58 File Offset: 0x00023158
		static SplitView()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(SplitView), new FrameworkPropertyMetadata(typeof(SplitView)));
		}

		// Token: 0x06000ACF RID: 2767 RVA: 0x0002516E File Offset: 0x0002336E
		public SplitView()
		{
			this.TemplateSettings = new SplitViewTemplateSettings();
			base.SizeChanged += new SizeChangedEventHandler(this.OnSizeChanged);
			base.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.OnIsVisibleChanged);
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000AD0 RID: 2768 RVA: 0x000251A8 File Offset: 0x000233A8
		private bool IsLightDismissible
		{
			get
			{
				SplitViewDisplayMode displayMode = this.DisplayMode;
				return displayMode == SplitViewDisplayMode.Overlay || displayMode == SplitViewDisplayMode.CompactOverlay;
			}
		}

		// Token: 0x14000038 RID: 56
		// (add) Token: 0x06000AD1 RID: 2769 RVA: 0x000251C8 File Offset: 0x000233C8
		// (remove) Token: 0x06000AD2 RID: 2770 RVA: 0x00025200 File Offset: 0x00023400
		public event TypedEventHandler<SplitView, object> PaneOpening;

		// Token: 0x14000039 RID: 57
		// (add) Token: 0x06000AD3 RID: 2771 RVA: 0x00025238 File Offset: 0x00023438
		// (remove) Token: 0x06000AD4 RID: 2772 RVA: 0x00025270 File Offset: 0x00023470
		public event TypedEventHandler<SplitView, object> PaneOpened;

		// Token: 0x1400003A RID: 58
		// (add) Token: 0x06000AD5 RID: 2773 RVA: 0x000252A8 File Offset: 0x000234A8
		// (remove) Token: 0x06000AD6 RID: 2774 RVA: 0x000252E0 File Offset: 0x000234E0
		public event TypedEventHandler<SplitView, SplitViewPaneClosingEventArgs> PaneClosing;

		// Token: 0x1400003B RID: 59
		// (add) Token: 0x06000AD7 RID: 2775 RVA: 0x00025318 File Offset: 0x00023518
		// (remove) Token: 0x06000AD8 RID: 2776 RVA: 0x00025350 File Offset: 0x00023550
		public event TypedEventHandler<SplitView, object> PaneClosed;

		// Token: 0x1400003C RID: 60
		// (add) Token: 0x06000AD9 RID: 2777 RVA: 0x00025388 File Offset: 0x00023588
		// (remove) Token: 0x06000ADA RID: 2778 RVA: 0x000253C0 File Offset: 0x000235C0
		internal event DependencyPropertyChangedCallback IsPaneOpenChanged;

		// Token: 0x1400003D RID: 61
		// (add) Token: 0x06000ADB RID: 2779 RVA: 0x000253F8 File Offset: 0x000235F8
		// (remove) Token: 0x06000ADC RID: 2780 RVA: 0x00025430 File Offset: 0x00023630
		internal event DependencyPropertyChangedCallback DisplayModeChanged;

		// Token: 0x1400003E RID: 62
		// (add) Token: 0x06000ADD RID: 2781 RVA: 0x00025468 File Offset: 0x00023668
		// (remove) Token: 0x06000ADE RID: 2782 RVA: 0x000254A0 File Offset: 0x000236A0
		internal event DependencyPropertyChangedCallback CompactPaneLengthChanged;

		// Token: 0x06000ADF RID: 2783 RVA: 0x000254D8 File Offset: 0x000236D8
		public override void OnApplyTemplate()
		{
			if (this._displayModeStates != null)
			{
				this._displayModeStates.CurrentStateChanging -= new EventHandler<VisualStateChangedEventArgs>(this.OnDisplayModeStatesCurrentStateChanging);
				this._displayModeStates.CurrentStateChanged -= new EventHandler<VisualStateChangedEventArgs>(this.OnDisplayModeStatesCurrentStateChanged);
			}
			base.OnApplyTemplate();
			this._templateRoot = ControlExtensions.GetTemplateRoot(this);
			this._paneRoot = (base.GetTemplateChild("PaneRoot") as FrameworkElement);
			this._displayModeStates = (base.GetTemplateChild("DisplayModeStates") as VisualStateGroup);
			this._paneClipRectangle = (base.GetTemplateChild("PaneClipRectangle") as RectangleGeometry);
			if (this._displayModeStates != null)
			{
				this._displayModeStates.CurrentStateChanging += new EventHandler<VisualStateChangedEventArgs>(this.OnDisplayModeStatesCurrentStateChanging);
				this._displayModeStates.CurrentStateChanged += new EventHandler<VisualStateChangedEventArgs>(this.OnDisplayModeStatesCurrentStateChanged);
				AnimationHelper.DeferTransitions(this._displayModeStates);
			}
			this.UpdateTemplateSettings();
			this.UpdatePaneClipRectangle();
			this.UpdateVisualState(false);
			base.Dispatcher.BeginInvoke(delegate()
			{
				this.ReapplyDisplayModeState(false);
			}, 8, Array.Empty<object>());
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x000255E1 File Offset: 0x000237E1
		private void OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			this.UpdatePaneClipRectangle();
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x000255E9 File Offset: 0x000237E9
		private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.UpdateIsLightDismissActive();
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x000255F1 File Offset: 0x000237F1
		private void OnDisplayModeStatesCurrentStateChanging(object sender, VisualStateChangedEventArgs e)
		{
			this._isDisplayModeStateChanging = true;
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x000255FC File Offset: 0x000237FC
		private void OnDisplayModeStatesCurrentStateChanged(object sender, VisualStateChangedEventArgs e)
		{
			this._isDisplayModeStateChanging = false;
			if (!this._isPaneOpening)
			{
				if (this._isPaneClosing)
				{
					this._isPaneClosing = false;
					TypedEventHandler<SplitView, object> paneClosed = this.PaneClosed;
					if (paneClosed == null)
					{
						return;
					}
					paneClosed.Invoke(this, null);
				}
				return;
			}
			this._isPaneOpening = false;
			TypedEventHandler<SplitView, object> paneOpened = this.PaneOpened;
			if (paneOpened == null)
			{
				return;
			}
			paneOpened.Invoke(this, null);
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x00025654 File Offset: 0x00023854
		private void OnWindowPreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			if (this._paneRoot != null)
			{
				Point position = e.GetPosition(this._paneRoot);
				if (position.X >= 0.0 && position.X <= this._paneRoot.ActualWidth && position.Y >= 0.0 && position.Y <= this._paneRoot.ActualHeight)
				{
					return;
				}
			}
			UIElement uielement = e.OriginalSource as UIElement;
			if (uielement != null && TitleBarControl.GetInsideTitleBar(uielement))
			{
				return;
			}
			if (this.IsPaneOpen)
			{
				e.Handled = true;
				this.ClosePane();
			}
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x000256F0 File Offset: 0x000238F0
		private void OpenPane()
		{
			if (this._isPaneOpening)
			{
				return;
			}
			TypedEventHandler<SplitView, object> paneOpening = this.PaneOpening;
			if (paneOpening != null)
			{
				paneOpening.Invoke(this, null);
			}
			if (this.UpdateDisplayModeState(true))
			{
				this._isPaneOpening = true;
				return;
			}
			TypedEventHandler<SplitView, object> paneOpened = this.PaneOpened;
			if (paneOpened == null)
			{
				return;
			}
			paneOpened.Invoke(this, null);
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x0002573C File Offset: 0x0002393C
		private void ClosePane()
		{
			if (this._isPaneClosing)
			{
				return;
			}
			TypedEventHandler<SplitView, SplitViewPaneClosingEventArgs> paneClosing = this.PaneClosing;
			if (paneClosing != null)
			{
				SplitViewPaneClosingEventArgs splitViewPaneClosingEventArgs = new SplitViewPaneClosingEventArgs();
				paneClosing.Invoke(this, splitViewPaneClosingEventArgs);
				if (splitViewPaneClosingEventArgs.Cancel && this.IsPaneOpen)
				{
					return;
				}
			}
			this._isPaneClosing = true;
			if (this.IsPaneOpen)
			{
				base.SetCurrentValue(SplitView.IsPaneOpenProperty, false);
			}
			if (!this.UpdateDisplayModeState(true))
			{
				this._isPaneClosing = false;
				TypedEventHandler<SplitView, object> paneClosed = this.PaneClosed;
				if (paneClosed == null)
				{
					return;
				}
				paneClosed.Invoke(this, null);
			}
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x000257C0 File Offset: 0x000239C0
		private void UpdateTemplateSettings()
		{
			double compactPaneLength = this.CompactPaneLength;
			double openPaneLength = this.OpenPaneLength;
			double num = openPaneLength - compactPaneLength;
			SplitViewTemplateSettings templateSettings = this.TemplateSettings;
			templateSettings.CompactPaneGridLength = new GridLength(compactPaneLength);
			templateSettings.NegativeOpenPaneLength = -openPaneLength;
			templateSettings.NegativeOpenPaneLengthMinusCompactLength = -num;
			templateSettings.OpenPaneGridLength = new GridLength(openPaneLength);
			templateSettings.OpenPaneLength = openPaneLength;
			templateSettings.OpenPaneLengthMinusCompactLength = num;
			this.ReapplyDisplayModeState(true);
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x00025821 File Offset: 0x00023A21
		private void UpdatePaneClipRectangle()
		{
			if (this._paneClipRectangle != null)
			{
				this._paneClipRectangle.Rect = new Rect(0.0, 0.0, this.OpenPaneLength, base.ActualHeight);
			}
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x0002585C File Offset: 0x00023A5C
		private void UpdateIsLightDismissActive()
		{
			bool flag = base.IsVisible && this.IsPaneOpen && this.IsLightDismissible;
			if (this._isLightDismissActive != flag)
			{
				this._isLightDismissActive = flag;
				if (this._isLightDismissActive)
				{
					this._window = Window.GetWindow(this);
					if (this._window != null)
					{
						this._window.PreviewMouseDown += new MouseButtonEventHandler(this.OnWindowPreviewMouseDown);
						return;
					}
				}
				else if (this._window != null)
				{
					this._window.PreviewMouseDown -= new MouseButtonEventHandler(this.OnWindowPreviewMouseDown);
					this._window = null;
				}
			}
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x000258F0 File Offset: 0x00023AF0
		private bool UpdateDisplayModeState(bool useTransitions = true)
		{
			string text = null;
			SplitViewDisplayMode displayMode = this.DisplayMode;
			if (this.IsPaneOpen)
			{
				if (displayMode == SplitViewDisplayMode.Overlay)
				{
					if (this.PanePlacement == SplitViewPanePlacement.Left)
					{
						text = "OpenOverlayLeft";
					}
					else
					{
						text = "OpenOverlayRight";
					}
				}
				else if (displayMode == SplitViewDisplayMode.Inline || displayMode == SplitViewDisplayMode.CompactInline)
				{
					if (this.PanePlacement == SplitViewPanePlacement.Left)
					{
						text = "OpenInlineLeft";
					}
					else
					{
						text = "OpenInlineRight";
					}
				}
				else if (displayMode == SplitViewDisplayMode.CompactOverlay)
				{
					if (this.PanePlacement == SplitViewPanePlacement.Left)
					{
						text = "OpenCompactOverlayLeft";
					}
					else
					{
						text = "OpenCompactOverlayRight";
					}
				}
			}
			else if (displayMode == SplitViewDisplayMode.CompactOverlay || displayMode == SplitViewDisplayMode.CompactInline)
			{
				if (this.PanePlacement == SplitViewPanePlacement.Left)
				{
					text = "ClosedCompactLeft";
				}
				else
				{
					text = "ClosedCompactRight";
				}
			}
			else
			{
				text = "Closed";
			}
			return VisualStateManager.GoToState(this, text, useTransitions);
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x00025994 File Offset: 0x00023B94
		private void UpdateOverlayVisibilityState(bool useTransitions = true)
		{
			string text;
			if (this.IsPaneOpen && this.IsLightDismissible && this.LightDismissOverlayMode == LightDismissOverlayMode.On)
			{
				text = "OverlayVisible";
			}
			else
			{
				text = "OverlayNotVisible";
			}
			VisualStateManager.GoToState(this, text, useTransitions);
		}

		// Token: 0x06000AEC RID: 2796 RVA: 0x000259D1 File Offset: 0x00023BD1
		private void UpdateVisualState(bool useTransitions = true)
		{
			this.UpdateDisplayModeState(useTransitions);
			this.UpdateOverlayVisibilityState(useTransitions);
		}

		// Token: 0x06000AED RID: 2797 RVA: 0x000259E4 File Offset: 0x00023BE4
		private void ReapplyDisplayModeState(bool waitForDataBinding = true)
		{
			if (!this._isDisplayModeStateChanging)
			{
				VisualStateGroup displayModeStates = this._displayModeStates;
				Storyboard storyboard;
				if (displayModeStates == null)
				{
					storyboard = null;
				}
				else
				{
					VisualState currentState = displayModeStates.CurrentState;
					storyboard = ((currentState != null) ? currentState.Storyboard : null);
				}
				Storyboard storyboard2 = storyboard;
				if (storyboard2 != null && this._templateRoot != null && !storyboard2.CanFreeze)
				{
					if (waitForDataBinding)
					{
						DispatcherHelper.DoEvents(8);
					}
					storyboard2.Begin(this._templateRoot, true);
				}
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000AEE RID: 2798 RVA: 0x00025A41 File Offset: 0x00023C41
		// (set) Token: 0x06000AEF RID: 2799 RVA: 0x00025A53 File Offset: 0x00023C53
		public double CompactPaneLength
		{
			get
			{
				return (double)base.GetValue(SplitView.CompactPaneLengthProperty);
			}
			set
			{
				base.SetValue(SplitView.CompactPaneLengthProperty, value);
			}
		}

		// Token: 0x06000AF0 RID: 2800 RVA: 0x00025A66 File Offset: 0x00023C66
		private static void OnCompactPaneLengthPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((SplitView)sender).OnCompactPaneLengthPropertyChanged(args);
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x00025A74 File Offset: 0x00023C74
		private void OnCompactPaneLengthPropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			this.UpdateTemplateSettings();
			DependencyPropertyChangedCallback compactPaneLengthChanged = this.CompactPaneLengthChanged;
			if (compactPaneLengthChanged == null)
			{
				return;
			}
			compactPaneLengthChanged(this, args.Property);
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000AF2 RID: 2802 RVA: 0x00025A94 File Offset: 0x00023C94
		// (set) Token: 0x06000AF3 RID: 2803 RVA: 0x00025AA6 File Offset: 0x00023CA6
		public UIElement Content
		{
			get
			{
				return (UIElement)base.GetValue(SplitView.ContentProperty);
			}
			set
			{
				base.SetValue(SplitView.ContentProperty, value);
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000AF4 RID: 2804 RVA: 0x00025AB4 File Offset: 0x00023CB4
		// (set) Token: 0x06000AF5 RID: 2805 RVA: 0x00025AC6 File Offset: 0x00023CC6
		public SplitViewDisplayMode DisplayMode
		{
			get
			{
				return (SplitViewDisplayMode)base.GetValue(SplitView.DisplayModeProperty);
			}
			set
			{
				base.SetValue(SplitView.DisplayModeProperty, value);
			}
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x00025AD9 File Offset: 0x00023CD9
		private static void OnDisplayModePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((SplitView)sender).OnDisplayModePropertyChanged(args);
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x00025AE7 File Offset: 0x00023CE7
		private void OnDisplayModePropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			this.UpdateIsLightDismissActive();
			this.UpdateVisualState(true);
			DependencyPropertyChangedCallback displayModeChanged = this.DisplayModeChanged;
			if (displayModeChanged == null)
			{
				return;
			}
			displayModeChanged(this, args.Property);
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000AF8 RID: 2808 RVA: 0x00025B0E File Offset: 0x00023D0E
		// (set) Token: 0x06000AF9 RID: 2809 RVA: 0x00025B20 File Offset: 0x00023D20
		public bool IsPaneOpen
		{
			get
			{
				return (bool)base.GetValue(SplitView.IsPaneOpenProperty);
			}
			set
			{
				base.SetValue(SplitView.IsPaneOpenProperty, value);
			}
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x00025B33 File Offset: 0x00023D33
		private static void OnIsPaneOpenPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((SplitView)sender).OnIsPaneOpenPropertyChanged(args);
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x00025B44 File Offset: 0x00023D44
		private void OnIsPaneOpenPropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			if ((bool)args.NewValue)
			{
				this._isPaneClosing = false;
				this.OpenPane();
			}
			else
			{
				this._isPaneOpening = false;
				this.ClosePane();
			}
			this.UpdateIsLightDismissActive();
			this.UpdateOverlayVisibilityState(true);
			DependencyPropertyChangedCallback isPaneOpenChanged = this.IsPaneOpenChanged;
			if (isPaneOpenChanged == null)
			{
				return;
			}
			isPaneOpenChanged(this, args.Property);
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000AFC RID: 2812 RVA: 0x00025BA0 File Offset: 0x00023DA0
		// (set) Token: 0x06000AFD RID: 2813 RVA: 0x00025BB2 File Offset: 0x00023DB2
		public double OpenPaneLength
		{
			get
			{
				return (double)base.GetValue(SplitView.OpenPaneLengthProperty);
			}
			set
			{
				base.SetValue(SplitView.OpenPaneLengthProperty, value);
			}
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x00025BC5 File Offset: 0x00023DC5
		private static void OnOpenPaneLengthPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((SplitView)sender).OnOpenPaneLengthPropertyChanged(args);
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x00025BD3 File Offset: 0x00023DD3
		private void OnOpenPaneLengthPropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			this.UpdateTemplateSettings();
			this.UpdatePaneClipRectangle();
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000B00 RID: 2816 RVA: 0x00025BE1 File Offset: 0x00023DE1
		// (set) Token: 0x06000B01 RID: 2817 RVA: 0x00025BF3 File Offset: 0x00023DF3
		public Brush PaneBackground
		{
			get
			{
				return (Brush)base.GetValue(SplitView.PaneBackgroundProperty);
			}
			set
			{
				base.SetValue(SplitView.PaneBackgroundProperty, value);
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000B02 RID: 2818 RVA: 0x00025C01 File Offset: 0x00023E01
		// (set) Token: 0x06000B03 RID: 2819 RVA: 0x00025C13 File Offset: 0x00023E13
		public SplitViewPanePlacement PanePlacement
		{
			get
			{
				return (SplitViewPanePlacement)base.GetValue(SplitView.PanePlacementProperty);
			}
			set
			{
				base.SetValue(SplitView.PanePlacementProperty, value);
			}
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x00025C26 File Offset: 0x00023E26
		private static void OnPanePlacementPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((SplitView)sender).OnPanePlacementPropertyChanged(args);
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x00025C34 File Offset: 0x00023E34
		private void OnPanePlacementPropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			this.UpdateDisplayModeState(true);
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000B06 RID: 2822 RVA: 0x00025C3E File Offset: 0x00023E3E
		// (set) Token: 0x06000B07 RID: 2823 RVA: 0x00025C50 File Offset: 0x00023E50
		public UIElement Pane
		{
			get
			{
				return (UIElement)base.GetValue(SplitView.PaneProperty);
			}
			set
			{
				base.SetValue(SplitView.PaneProperty, value);
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000B08 RID: 2824 RVA: 0x00025C5E File Offset: 0x00023E5E
		// (set) Token: 0x06000B09 RID: 2825 RVA: 0x00025C70 File Offset: 0x00023E70
		public SplitViewTemplateSettings TemplateSettings
		{
			get
			{
				return (SplitViewTemplateSettings)base.GetValue(SplitView.TemplateSettingsProperty);
			}
			private set
			{
				base.SetValue(SplitView.TemplateSettingsPropertyKey, value);
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000B0A RID: 2826 RVA: 0x00025C7E File Offset: 0x00023E7E
		// (set) Token: 0x06000B0B RID: 2827 RVA: 0x00025C90 File Offset: 0x00023E90
		public LightDismissOverlayMode LightDismissOverlayMode
		{
			get
			{
				return (LightDismissOverlayMode)base.GetValue(SplitView.LightDismissOverlayModeProperty);
			}
			set
			{
				base.SetValue(SplitView.LightDismissOverlayModeProperty, value);
			}
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x00025CA3 File Offset: 0x00023EA3
		private static void OnLightDismissOverlayModePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((SplitView)sender).OnLightDismissOverlayModePropertyChanged(args);
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x00025CB1 File Offset: 0x00023EB1
		private void OnLightDismissOverlayModePropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			this.UpdateOverlayVisibilityState(true);
		}

		// Token: 0x04000465 RID: 1125
		private FrameworkElement _templateRoot;

		// Token: 0x04000466 RID: 1126
		private VisualStateGroup _displayModeStates;

		// Token: 0x04000467 RID: 1127
		private FrameworkElement _paneRoot;

		// Token: 0x04000468 RID: 1128
		private RectangleGeometry _paneClipRectangle;

		// Token: 0x04000469 RID: 1129
		private Window _window;

		// Token: 0x0400046A RID: 1130
		private bool _isLightDismissActive;

		// Token: 0x0400046B RID: 1131
		private bool _isPaneOpening;

		// Token: 0x0400046C RID: 1132
		private bool _isPaneClosing;

		// Token: 0x0400046D RID: 1133
		private bool _isDisplayModeStateChanging;

		// Token: 0x0400046E RID: 1134
		private const string PaneRootName = "PaneRoot";

		// Token: 0x0400046F RID: 1135
		private const string DisplayModeStatesName = "DisplayModeStates";

		// Token: 0x04000470 RID: 1136
		private const string PaneClipRectangleName = "PaneClipRectangle";

		// Token: 0x04000471 RID: 1137
		public static readonly DependencyProperty CompactPaneLengthProperty = DependencyProperty.Register("CompactPaneLength", typeof(double), typeof(SplitView), new PropertyMetadata(new PropertyChangedCallback(SplitView.OnCompactPaneLengthPropertyChanged)));

		// Token: 0x04000472 RID: 1138
		public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(UIElement), typeof(SplitView));

		// Token: 0x04000473 RID: 1139
		public static readonly DependencyProperty DisplayModeProperty = DependencyProperty.Register("DisplayMode", typeof(SplitViewDisplayMode), typeof(SplitView), new PropertyMetadata(SplitViewDisplayMode.Overlay, new PropertyChangedCallback(SplitView.OnDisplayModePropertyChanged)));

		// Token: 0x04000474 RID: 1140
		public static readonly DependencyProperty IsPaneOpenProperty = DependencyProperty.Register("IsPaneOpen", typeof(bool), typeof(SplitView), new PropertyMetadata(new PropertyChangedCallback(SplitView.OnIsPaneOpenPropertyChanged)));

		// Token: 0x04000475 RID: 1141
		public static readonly DependencyProperty OpenPaneLengthProperty = DependencyProperty.Register("OpenPaneLength", typeof(double), typeof(SplitView), new PropertyMetadata(new PropertyChangedCallback(SplitView.OnOpenPaneLengthPropertyChanged)));

		// Token: 0x04000476 RID: 1142
		public static readonly DependencyProperty PaneBackgroundProperty = DependencyProperty.Register("PaneBackground", typeof(Brush), typeof(SplitView));

		// Token: 0x04000477 RID: 1143
		public static readonly DependencyProperty PanePlacementProperty = DependencyProperty.Register("PanePlacement", typeof(SplitViewPanePlacement), typeof(SplitView), new PropertyMetadata(SplitViewPanePlacement.Left, new PropertyChangedCallback(SplitView.OnPanePlacementPropertyChanged)));

		// Token: 0x04000478 RID: 1144
		public static readonly DependencyProperty PaneProperty = DependencyProperty.Register("Pane", typeof(UIElement), typeof(SplitView));

		// Token: 0x04000479 RID: 1145
		private static readonly DependencyPropertyKey TemplateSettingsPropertyKey = DependencyProperty.RegisterReadOnly("TemplateSettings", typeof(SplitViewTemplateSettings), typeof(SplitView), null);

		// Token: 0x0400047A RID: 1146
		public static readonly DependencyProperty TemplateSettingsProperty = SplitView.TemplateSettingsPropertyKey.DependencyProperty;

		// Token: 0x0400047B RID: 1147
		public static readonly DependencyProperty LightDismissOverlayModeProperty = DependencyProperty.Register("LightDismissOverlayMode", typeof(LightDismissOverlayMode), typeof(SplitView), new PropertyMetadata(LightDismissOverlayMode.Auto, new PropertyChangedCallback(SplitView.OnLightDismissOverlayModePropertyChanged)));
	}
}
