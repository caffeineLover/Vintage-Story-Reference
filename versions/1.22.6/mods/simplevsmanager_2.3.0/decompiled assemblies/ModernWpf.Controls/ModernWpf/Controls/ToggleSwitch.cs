using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using ModernWpf.Automation.Peers;
using ModernWpf.Controls.Primitives;

namespace ModernWpf.Controls
{
	// Token: 0x020000E0 RID: 224
	[ContentProperty("Header")]
	[TemplatePart(Name = "HeaderContentPresenter", Type = typeof(ContentPresenter))]
	[TemplatePart(Name = "SwitchKnobBounds", Type = typeof(FrameworkElement))]
	[TemplatePart(Name = "SwitchKnob", Type = typeof(FrameworkElement))]
	[TemplatePart(Name = "KnobTranslateTransform", Type = typeof(TranslateTransform))]
	[TemplatePart(Name = "SwitchThumb", Type = typeof(Thumb))]
	[TemplateVisualState(GroupName = "CommonStates", Name = "Normal")]
	[TemplateVisualState(GroupName = "CommonStates", Name = "MouseOver")]
	[TemplateVisualState(GroupName = "CommonStates", Name = "Pressed")]
	[TemplateVisualState(GroupName = "CommonStates", Name = "Disabled")]
	[TemplateVisualState(GroupName = "ContentStates", Name = "OffContent")]
	[TemplateVisualState(GroupName = "ContentStates", Name = "OnContent")]
	[TemplateVisualState(GroupName = "ToggleStates", Name = "Dragging")]
	[TemplateVisualState(GroupName = "ToggleStates", Name = "Off")]
	[TemplateVisualState(GroupName = "ToggleStates", Name = "On")]
	public class ToggleSwitch : Control
	{
		// Token: 0x06000B12 RID: 2834 RVA: 0x00025CDC File Offset: 0x00023EDC
		static ToggleSwitch()
		{
			ToggleSwitch.ToggledEvent = EventManager.RegisterRoutedEvent("Toggled", 1, typeof(RoutedEventHandler), typeof(ToggleSwitch));
			ToggleSwitch.HeaderProperty = ControlHelper.HeaderProperty.AddOwner(typeof(ToggleSwitch), new FrameworkPropertyMetadata(new PropertyChangedCallback(ToggleSwitch.OnHeaderChanged)));
			ToggleSwitch.HeaderTemplateProperty = ControlHelper.HeaderTemplateProperty.AddOwner(typeof(ToggleSwitch), new FrameworkPropertyMetadata(new PropertyChangedCallback(ToggleSwitch.OnHeaderTemplateChanged)));
			ToggleSwitch.IsOnProperty = DependencyProperty.Register("IsOn", typeof(bool), typeof(ToggleSwitch), new FrameworkPropertyMetadata(false, 1280, new PropertyChangedCallback(ToggleSwitch.OnIsOnChanged)));
			ToggleSwitch.OffContentProperty = DependencyProperty.Register("OffContent", typeof(object), typeof(ToggleSwitch), new FrameworkPropertyMetadata(new PropertyChangedCallback(ToggleSwitch.OnOffContentChanged)));
			ToggleSwitch.OffContentTemplateProperty = DependencyProperty.Register("OffContentTemplate", typeof(DataTemplate), typeof(ToggleSwitch), null);
			ToggleSwitch.OnContentProperty = DependencyProperty.Register("OnContent", typeof(object), typeof(ToggleSwitch), new FrameworkPropertyMetadata(new PropertyChangedCallback(ToggleSwitch.OnOnContentChanged)));
			ToggleSwitch.OnContentTemplateProperty = DependencyProperty.Register("OnContentTemplate", typeof(DataTemplate), typeof(ToggleSwitch), null);
			ToggleSwitch.UseSystemFocusVisualsProperty = FocusVisualHelper.UseSystemFocusVisualsProperty.AddOwner(typeof(ToggleSwitch));
			ToggleSwitch.FocusVisualMarginProperty = FocusVisualHelper.FocusVisualMarginProperty.AddOwner(typeof(ToggleSwitch));
			ToggleSwitch.CornerRadiusProperty = ControlHelper.CornerRadiusProperty.AddOwner(typeof(ToggleSwitch));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ToggleSwitch), new FrameworkPropertyMetadata(typeof(ToggleSwitch)));
			EventManager.RegisterClassHandler(typeof(ToggleSwitch), UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(ToggleSwitch.OnMouseLeftButtonDown), true);
		}

		// Token: 0x06000B13 RID: 2835 RVA: 0x00025EDF File Offset: 0x000240DF
		public ToggleSwitch()
		{
			base.SetCurrentValue(ToggleSwitch.OffContentProperty, Strings.ToggleSwitchOff);
			base.SetCurrentValue(ToggleSwitch.OnContentProperty, Strings.ToggleSwitchOn);
			base.IsEnabledChanged += new DependencyPropertyChangedEventHandler(this.OnIsEnabledChanged);
		}

		// Token: 0x1400003F RID: 63
		// (add) Token: 0x06000B14 RID: 2836 RVA: 0x00025F19 File Offset: 0x00024119
		// (remove) Token: 0x06000B15 RID: 2837 RVA: 0x00025F27 File Offset: 0x00024127
		public event RoutedEventHandler Toggled
		{
			add
			{
				base.AddHandler(ToggleSwitch.ToggledEvent, value);
			}
			remove
			{
				base.RemoveHandler(ToggleSwitch.ToggledEvent, value);
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000B16 RID: 2838 RVA: 0x00025F35 File Offset: 0x00024135
		// (set) Token: 0x06000B17 RID: 2839 RVA: 0x00025F42 File Offset: 0x00024142
		public object Header
		{
			get
			{
				return base.GetValue(ToggleSwitch.HeaderProperty);
			}
			set
			{
				base.SetValue(ToggleSwitch.HeaderProperty, value);
			}
		}

		// Token: 0x06000B18 RID: 2840 RVA: 0x00025F50 File Offset: 0x00024150
		private static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ToggleSwitch toggleSwitch = (ToggleSwitch)d;
			toggleSwitch.UpdateHeaderContentPresenterVisibility();
			toggleSwitch.OnHeaderChanged(e.OldValue, e.NewValue);
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x00025F71 File Offset: 0x00024171
		protected virtual void OnHeaderChanged(object oldContent, object newContent)
		{
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000B1A RID: 2842 RVA: 0x00025F73 File Offset: 0x00024173
		// (set) Token: 0x06000B1B RID: 2843 RVA: 0x00025F85 File Offset: 0x00024185
		public DataTemplate HeaderTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(ToggleSwitch.HeaderTemplateProperty);
			}
			set
			{
				base.SetValue(ToggleSwitch.HeaderTemplateProperty, value);
			}
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x00025F93 File Offset: 0x00024193
		private static void OnHeaderTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ToggleSwitch)d).UpdateHeaderContentPresenterVisibility();
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000B1D RID: 2845 RVA: 0x00025FA0 File Offset: 0x000241A0
		// (set) Token: 0x06000B1E RID: 2846 RVA: 0x00025FB2 File Offset: 0x000241B2
		public bool IsOn
		{
			get
			{
				return (bool)base.GetValue(ToggleSwitch.IsOnProperty);
			}
			set
			{
				base.SetValue(ToggleSwitch.IsOnProperty, value);
			}
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x00025FC5 File Offset: 0x000241C5
		private static void OnIsOnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ToggleSwitch toggleSwitch = (ToggleSwitch)d;
			toggleSwitch.OnToggled();
			toggleSwitch.UpdateVisualStates(true);
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000B20 RID: 2848 RVA: 0x00025FD9 File Offset: 0x000241D9
		// (set) Token: 0x06000B21 RID: 2849 RVA: 0x00025FE6 File Offset: 0x000241E6
		public object OffContent
		{
			get
			{
				return base.GetValue(ToggleSwitch.OffContentProperty);
			}
			set
			{
				base.SetValue(ToggleSwitch.OffContentProperty, value);
			}
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x00025FF4 File Offset: 0x000241F4
		private static void OnOffContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ToggleSwitch)d).OnOffContentChanged(e.OldValue, e.NewValue);
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x0002600F File Offset: 0x0002420F
		protected virtual void OnOffContentChanged(object oldContent, object newContent)
		{
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000B24 RID: 2852 RVA: 0x00026011 File Offset: 0x00024211
		// (set) Token: 0x06000B25 RID: 2853 RVA: 0x00026023 File Offset: 0x00024223
		public DataTemplate OffContentTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(ToggleSwitch.OffContentTemplateProperty);
			}
			set
			{
				base.SetValue(ToggleSwitch.OffContentTemplateProperty, value);
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000B26 RID: 2854 RVA: 0x00026031 File Offset: 0x00024231
		// (set) Token: 0x06000B27 RID: 2855 RVA: 0x0002603E File Offset: 0x0002423E
		public object OnContent
		{
			get
			{
				return base.GetValue(ToggleSwitch.OnContentProperty);
			}
			set
			{
				base.SetValue(ToggleSwitch.OnContentProperty, value);
			}
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x0002604C File Offset: 0x0002424C
		private static void OnOnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ToggleSwitch)d).OnOffContentChanged(e.OldValue, e.NewValue);
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x00026067 File Offset: 0x00024267
		protected virtual void OnOnContentChanged(object oldContent, object newContent)
		{
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000B2A RID: 2858 RVA: 0x00026069 File Offset: 0x00024269
		// (set) Token: 0x06000B2B RID: 2859 RVA: 0x0002607B File Offset: 0x0002427B
		public DataTemplate OnContentTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(ToggleSwitch.OnContentTemplateProperty);
			}
			set
			{
				base.SetValue(ToggleSwitch.OnContentTemplateProperty, value);
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000B2C RID: 2860 RVA: 0x00026089 File Offset: 0x00024289
		// (set) Token: 0x06000B2D RID: 2861 RVA: 0x0002609B File Offset: 0x0002429B
		public bool UseSystemFocusVisuals
		{
			get
			{
				return (bool)base.GetValue(ToggleSwitch.UseSystemFocusVisualsProperty);
			}
			set
			{
				base.SetValue(ToggleSwitch.UseSystemFocusVisualsProperty, value);
			}
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000B2E RID: 2862 RVA: 0x000260AE File Offset: 0x000242AE
		// (set) Token: 0x06000B2F RID: 2863 RVA: 0x000260C0 File Offset: 0x000242C0
		public Thickness FocusVisualMargin
		{
			get
			{
				return (Thickness)base.GetValue(ToggleSwitch.FocusVisualMarginProperty);
			}
			set
			{
				base.SetValue(ToggleSwitch.FocusVisualMarginProperty, value);
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000B30 RID: 2864 RVA: 0x000260D3 File Offset: 0x000242D3
		// (set) Token: 0x06000B31 RID: 2865 RVA: 0x000260E5 File Offset: 0x000242E5
		public CornerRadius CornerRadius
		{
			get
			{
				return (CornerRadius)base.GetValue(ToggleSwitch.CornerRadiusProperty);
			}
			set
			{
				base.SetValue(ToggleSwitch.CornerRadiusProperty, value);
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000B32 RID: 2866 RVA: 0x000260F8 File Offset: 0x000242F8
		// (set) Token: 0x06000B33 RID: 2867 RVA: 0x00026100 File Offset: 0x00024300
		private ContentPresenter HeaderContentPresenter { get; set; }

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000B34 RID: 2868 RVA: 0x00026109 File Offset: 0x00024309
		// (set) Token: 0x06000B35 RID: 2869 RVA: 0x00026111 File Offset: 0x00024311
		private FrameworkElement SwitchKnobBounds { get; set; }

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000B36 RID: 2870 RVA: 0x0002611A File Offset: 0x0002431A
		// (set) Token: 0x06000B37 RID: 2871 RVA: 0x00026122 File Offset: 0x00024322
		private FrameworkElement SwitchKnob { get; set; }

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000B38 RID: 2872 RVA: 0x0002612B File Offset: 0x0002432B
		// (set) Token: 0x06000B39 RID: 2873 RVA: 0x00026133 File Offset: 0x00024333
		private TranslateTransform KnobTranslateTransform { get; set; }

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000B3A RID: 2874 RVA: 0x0002613C File Offset: 0x0002433C
		// (set) Token: 0x06000B3B RID: 2875 RVA: 0x00026144 File Offset: 0x00024344
		private Thumb SwitchThumb { get; set; }

		// Token: 0x06000B3C RID: 2876 RVA: 0x00026150 File Offset: 0x00024350
		public override void OnApplyTemplate()
		{
			if (this.SwitchKnobBounds != null && this.SwitchKnob != null && this.KnobTranslateTransform != null && this.SwitchThumb != null)
			{
				this.SwitchThumb.DragStarted -= new DragStartedEventHandler(this.OnSwitchThumbDragStarted);
				this.SwitchThumb.DragDelta -= new DragDeltaEventHandler(this.OnSwitchThumbDragDelta);
				this.SwitchThumb.DragCompleted -= new DragCompletedEventHandler(this.OnSwitchThumbDragCompleted);
				this.SwitchThumb.ClearValue(UIElement.CacheModeProperty);
			}
			base.OnApplyTemplate();
			this.HeaderContentPresenter = (base.GetTemplateChild("HeaderContentPresenter") as ContentPresenter);
			this.SwitchKnobBounds = (base.GetTemplateChild("SwitchKnobBounds") as FrameworkElement);
			this.SwitchKnob = (base.GetTemplateChild("SwitchKnob") as FrameworkElement);
			this.KnobTranslateTransform = (base.GetTemplateChild("KnobTranslateTransform") as TranslateTransform);
			this.SwitchThumb = (base.GetTemplateChild("SwitchThumb") as Thumb);
			if (this.SwitchKnobBounds != null && this.SwitchKnob != null && this.KnobTranslateTransform != null && this.SwitchThumb != null)
			{
				this.SwitchThumb.DragStarted += new DragStartedEventHandler(this.OnSwitchThumbDragStarted);
				this.SwitchThumb.DragDelta += new DragDeltaEventHandler(this.OnSwitchThumbDragDelta);
				this.SwitchThumb.DragCompleted += new DragCompletedEventHandler(this.OnSwitchThumbDragCompleted);
				if (this._bitmapCache == null)
				{
					this._bitmapCache = new BitmapCache(VisualTreeHelper.GetDpi(this).PixelsPerDip);
				}
				this.SwitchThumb.CacheMode = this._bitmapCache;
			}
			this.UpdateHeaderContentPresenterVisibility();
			this.UpdateVisualStates(false);
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x000262F0 File Offset: 0x000244F0
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ToggleSwitchAutomationPeer(this);
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x000262F8 File Offset: 0x000244F8
		protected virtual void OnToggled()
		{
			base.RaiseEvent(new RoutedEventArgs(ToggleSwitch.ToggledEvent));
			AutomationPeer automationPeer = UIElementAutomationPeer.FromElement(this);
			if (automationPeer != null)
			{
				ToggleState toggleState = this.IsOn ? 1 : 0;
				ToggleState toggleState2 = (toggleState == 1) ? 0 : 1;
				automationPeer.RaisePropertyChangedEvent(TogglePatternIdentifiers.ToggleStateProperty, toggleState2, toggleState);
			}
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x0002634C File Offset: 0x0002454C
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == UIElement.IsMouseOverProperty)
			{
				this.UpdateVisualStates(true);
			}
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x0002636C File Offset: 0x0002456C
		protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
		{
			base.OnRenderSizeChanged(sizeInfo);
			if (this.SwitchKnobBounds != null && this.SwitchKnob != null)
			{
				this._onTranslation = this.SwitchKnobBounds.ActualWidth - this.SwitchKnob.ActualWidth - this.SwitchKnob.Margin.Left - this.SwitchKnob.Margin.Right;
			}
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x000263D5 File Offset: 0x000245D5
		protected override void OnDpiChanged(DpiScale oldDpi, DpiScale newDpi)
		{
			base.OnDpiChanged(oldDpi, newDpi);
			if (this._bitmapCache != null)
			{
				this._bitmapCache.RenderAtScale = newDpi.PixelsPerDip;
			}
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x000263F9 File Offset: 0x000245F9
		protected override void OnKeyUp(KeyEventArgs e)
		{
			if (e.Key == 18)
			{
				e.Handled = true;
				this.Toggle();
			}
			base.OnKeyUp(e);
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x0002641C File Offset: 0x0002461C
		private static void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			ToggleSwitch toggleSwitch = (ToggleSwitch)sender;
			if (!toggleSwitch.IsKeyboardFocusWithin)
			{
				e.Handled = (toggleSwitch.Focus() || e.Handled);
			}
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x0002644F File Offset: 0x0002464F
		private void OnSwitchThumbDragStarted(object sender, DragStartedEventArgs e)
		{
			e.Handled = true;
			this._startTranslation = this.KnobTranslateTransform.X;
			this.UpdateVisualStates(true);
			this.KnobTranslateTransform.X = this._startTranslation;
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x00026484 File Offset: 0x00024684
		private void OnSwitchThumbDragDelta(object sender, DragDeltaEventArgs e)
		{
			e.Handled = true;
			if (e.HorizontalChange != 0.0)
			{
				this._wasDragged = true;
				double num = this._startTranslation + e.HorizontalChange;
				this.KnobTranslateTransform.X = Math.Max(0.0, Math.Min(this._onTranslation, num));
			}
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x000264E4 File Offset: 0x000246E4
		private void OnSwitchThumbDragCompleted(object sender, DragCompletedEventArgs e)
		{
			e.Handled = true;
			bool flag = false;
			if (this._wasDragged)
			{
				double num = this.IsOn ? this._onTranslation : 0.0;
				if (this.KnobTranslateTransform.X != num)
				{
					flag = true;
				}
			}
			else
			{
				flag = true;
			}
			if (flag)
			{
				this.Toggle();
			}
			this._wasDragged = false;
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x00026540 File Offset: 0x00024740
		private void OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.UpdateVisualStates(true);
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x0002654C File Offset: 0x0002474C
		private void UpdateHeaderContentPresenterVisibility()
		{
			if (this.HeaderContentPresenter != null)
			{
				bool flag = !ControlHelper.IsNullOrEmptyString(this.Header) || this.HeaderTemplate != null;
				this.HeaderContentPresenter.Visibility = (flag ? 0 : 2);
			}
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x00026590 File Offset: 0x00024790
		private void UpdateVisualStates(bool useTransitions)
		{
			string text;
			if (!base.IsEnabled)
			{
				text = "Disabled";
			}
			else if (base.IsMouseOver)
			{
				text = "MouseOver";
			}
			else
			{
				text = "Normal";
			}
			VisualStateManager.GoToState(this, text, useTransitions);
			if (this.SwitchThumb != null && this.SwitchThumb.IsDragging)
			{
				text = "Dragging";
			}
			else
			{
				text = (this.IsOn ? "On" : "Off");
			}
			VisualStateManager.GoToState(this, text, useTransitions);
			VisualStateManager.GoToState(this, this.IsOn ? "OnContent" : "OffContent", useTransitions);
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x00026623 File Offset: 0x00024823
		internal void Toggle()
		{
			base.SetCurrentValue(ToggleSwitch.IsOnProperty, !this.IsOn);
		}

		// Token: 0x04000485 RID: 1157
		private const string ContentStatesGroup = "ContentStates";

		// Token: 0x04000486 RID: 1158
		private const string OffContentState = "OffContent";

		// Token: 0x04000487 RID: 1159
		private const string OnContentState = "OnContent";

		// Token: 0x04000488 RID: 1160
		private const string ToggleStatesGroup = "ToggleStates";

		// Token: 0x04000489 RID: 1161
		private const string DraggingState = "Dragging";

		// Token: 0x0400048A RID: 1162
		private const string OffState = "Off";

		// Token: 0x0400048B RID: 1163
		private const string OnState = "On";

		// Token: 0x0400048C RID: 1164
		private const double _offTranslation = 0.0;

		// Token: 0x0400048D RID: 1165
		private double _onTranslation;

		// Token: 0x0400048E RID: 1166
		private double _startTranslation;

		// Token: 0x0400048F RID: 1167
		private bool _wasDragged;

		// Token: 0x04000490 RID: 1168
		private BitmapCache _bitmapCache;

		// Token: 0x04000492 RID: 1170
		public static readonly DependencyProperty HeaderProperty;

		// Token: 0x04000493 RID: 1171
		public static readonly DependencyProperty HeaderTemplateProperty;

		// Token: 0x04000494 RID: 1172
		public static readonly DependencyProperty IsOnProperty;

		// Token: 0x04000495 RID: 1173
		public static readonly DependencyProperty OffContentProperty;

		// Token: 0x04000496 RID: 1174
		public static readonly DependencyProperty OffContentTemplateProperty;

		// Token: 0x04000497 RID: 1175
		public static readonly DependencyProperty OnContentProperty;

		// Token: 0x04000498 RID: 1176
		public static readonly DependencyProperty OnContentTemplateProperty;

		// Token: 0x04000499 RID: 1177
		public static readonly DependencyProperty UseSystemFocusVisualsProperty;

		// Token: 0x0400049A RID: 1178
		public static readonly DependencyProperty FocusVisualMarginProperty;

		// Token: 0x0400049B RID: 1179
		public static readonly DependencyProperty CornerRadiusProperty;
	}
}
