using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x020000EA RID: 234
	[TemplatePart(Name = "OverflowPopup", Type = typeof(Popup))]
	public class CommandBarToolBar : ToolBar
	{
		// Token: 0x06000BEC RID: 3052 RVA: 0x00028D04 File Offset: 0x00026F04
		static CommandBarToolBar()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(CommandBarToolBar), new FrameworkPropertyMetadata(typeof(CommandBarToolBar)));
			ToolBar.IsOverflowOpenProperty.OverrideMetadata(typeof(CommandBarToolBar), new FrameworkPropertyMetadata(new PropertyChangedCallback(CommandBarToolBar.OnIsOverflowOpenChanged)));
			KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(CommandBarToolBar), new FrameworkPropertyMetadata(4));
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(CommandBarToolBar), new FrameworkPropertyMetadata(0));
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x00028EF4 File Offset: 0x000270F4
		public CommandBarToolBar()
		{
			this.m_moreButtonClosedToolTip = new ToolTip
			{
				Content = Strings.AppBarMoreButtonClosedToolTip
			};
			this.m_moreButtonOpenToolTip = new ToolTip
			{
				Content = Strings.AppBarMoreButtonOpenToolTip
			};
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000BEE RID: 3054 RVA: 0x00028F28 File Offset: 0x00027128
		// (set) Token: 0x06000BEF RID: 3055 RVA: 0x00028F3A File Offset: 0x0002713A
		public CornerRadius CornerRadius
		{
			get
			{
				return (CornerRadius)base.GetValue(CommandBarToolBar.CornerRadiusProperty);
			}
			set
			{
				base.SetValue(CommandBarToolBar.CornerRadiusProperty, value);
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000BF0 RID: 3056 RVA: 0x00028F4D File Offset: 0x0002714D
		// (set) Token: 0x06000BF1 RID: 3057 RVA: 0x00028F5F File Offset: 0x0002715F
		public CommandBarDefaultLabelPosition DefaultLabelPosition
		{
			get
			{
				return (CommandBarDefaultLabelPosition)base.GetValue(CommandBarToolBar.DefaultLabelPositionProperty);
			}
			set
			{
				base.SetValue(CommandBarToolBar.DefaultLabelPositionProperty, value);
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000BF2 RID: 3058 RVA: 0x00028F72 File Offset: 0x00027172
		// (set) Token: 0x06000BF3 RID: 3059 RVA: 0x00028F84 File Offset: 0x00027184
		public bool IsDynamicOverflowEnabled
		{
			get
			{
				return (bool)base.GetValue(CommandBarToolBar.IsDynamicOverflowEnabledProperty);
			}
			set
			{
				base.SetValue(CommandBarToolBar.IsDynamicOverflowEnabledProperty, value);
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000BF4 RID: 3060 RVA: 0x00028F97 File Offset: 0x00027197
		// (set) Token: 0x06000BF5 RID: 3061 RVA: 0x00028FA9 File Offset: 0x000271A9
		public CommandBarOverflowButtonVisibility OverflowButtonVisibility
		{
			get
			{
				return (CommandBarOverflowButtonVisibility)base.GetValue(CommandBarToolBar.OverflowButtonVisibilityProperty);
			}
			set
			{
				base.SetValue(CommandBarToolBar.OverflowButtonVisibilityProperty, value);
			}
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x00028FBC File Offset: 0x000271BC
		private static void OnOverflowButtonVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((CommandBarToolBar)d).UpdateEffectiveOverflowButtonVisibility();
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000BF7 RID: 3063 RVA: 0x00028FC9 File Offset: 0x000271C9
		// (set) Token: 0x06000BF8 RID: 3064 RVA: 0x00028FDB File Offset: 0x000271DB
		public Style OverflowPresenterStyle
		{
			get
			{
				return (Style)base.GetValue(CommandBarToolBar.OverflowPresenterStyleProperty);
			}
			set
			{
				base.SetValue(CommandBarToolBar.OverflowPresenterStyleProperty, value);
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000BF9 RID: 3065 RVA: 0x00028FE9 File Offset: 0x000271E9
		// (set) Token: 0x06000BFA RID: 3066 RVA: 0x00028FFB File Offset: 0x000271FB
		public double OverflowContentMaxHeight
		{
			get
			{
				return (double)base.GetValue(CommandBarToolBar.OverflowContentMaxHeightProperty);
			}
			private set
			{
				base.SetValue(CommandBarToolBar.OverflowContentMaxHeightPropertyKey, value);
			}
		}

		// Token: 0x06000BFB RID: 3067 RVA: 0x0002900E File Offset: 0x0002720E
		private void UpdateOverflowContentMaxHeight()
		{
			this.OverflowContentMaxHeight = CommandBarToolBar.CalculateOverflowContentMaxHeight();
		}

		// Token: 0x06000BFC RID: 3068 RVA: 0x0002901B File Offset: 0x0002721B
		private static double CalculateOverflowContentMaxHeight()
		{
			return SystemParameters.PrimaryScreenHeight / 2.0 + 20.0;
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000BFD RID: 3069 RVA: 0x00029036 File Offset: 0x00027236
		// (set) Token: 0x06000BFE RID: 3070 RVA: 0x00029048 File Offset: 0x00027248
		public Visibility EffectiveOverflowButtonVisibility
		{
			get
			{
				return (Visibility)base.GetValue(CommandBarToolBar.EffectiveOverflowButtonVisibilityProperty);
			}
			private set
			{
				base.SetValue(CommandBarToolBar.EffectiveOverflowButtonVisibilityPropertyKey, value);
			}
		}

		// Token: 0x06000BFF RID: 3071 RVA: 0x0002905B File Offset: 0x0002725B
		private static void OnEffectiveOverflowButtonVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((CommandBarToolBar)d).OnEffectiveOverflowButtonVisibilityChanged();
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x00029068 File Offset: 0x00027268
		private void OnEffectiveOverflowButtonVisibilityChanged()
		{
			this.InvalidateLayout();
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x00029070 File Offset: 0x00027270
		private void UpdateEffectiveOverflowButtonVisibility()
		{
			bool flag = true;
			CommandBarOverflowButtonVisibility overflowButtonVisibility = this.OverflowButtonVisibility;
			if (overflowButtonVisibility != CommandBarOverflowButtonVisibility.Auto)
			{
				if (overflowButtonVisibility == CommandBarOverflowButtonVisibility.Collapsed)
				{
					flag = false;
				}
			}
			else
			{
				flag = base.HasOverflowItems;
			}
			this.EffectiveOverflowButtonVisibility = (flag ? 0 : 2);
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000C02 RID: 3074 RVA: 0x000290A7 File Offset: 0x000272A7
		internal Popup OverflowPopup
		{
			get
			{
				return this.m_overflowPopup;
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000C03 RID: 3075 RVA: 0x000290AF File Offset: 0x000272AF
		internal bool HasPrimaryCommands
		{
			get
			{
				return this.m_toolBarPanel != null && this.m_toolBarPanel.HasChildren;
			}
		}

		// Token: 0x14000042 RID: 66
		// (add) Token: 0x06000C04 RID: 3076 RVA: 0x000290C8 File Offset: 0x000272C8
		// (remove) Token: 0x06000C05 RID: 3077 RVA: 0x00029100 File Offset: 0x00027300
		internal event EventHandler OverflowOpened;

		// Token: 0x14000043 RID: 67
		// (add) Token: 0x06000C06 RID: 3078 RVA: 0x00029138 File Offset: 0x00027338
		// (remove) Token: 0x06000C07 RID: 3079 RVA: 0x00029170 File Offset: 0x00027370
		internal event EventHandler OverflowClosed;

		// Token: 0x14000044 RID: 68
		// (add) Token: 0x06000C08 RID: 3080 RVA: 0x000291A8 File Offset: 0x000273A8
		// (remove) Token: 0x06000C09 RID: 3081 RVA: 0x000291E0 File Offset: 0x000273E0
		internal event EventHandler HasPrimaryCommandsChanged;

		// Token: 0x14000045 RID: 69
		// (add) Token: 0x06000C0A RID: 3082 RVA: 0x00029218 File Offset: 0x00027418
		// (remove) Token: 0x06000C0B RID: 3083 RVA: 0x00029250 File Offset: 0x00027450
		internal event EventHandler HasOverflowItemsChanged;

		// Token: 0x06000C0C RID: 3084 RVA: 0x00029288 File Offset: 0x00027488
		public override void OnApplyTemplate()
		{
			if (this.m_moreButton != null)
			{
				this.m_moreButton.ClearValue(FrameworkElement.ToolTipProperty);
			}
			if (this.m_overflowPopup != null)
			{
				this.m_overflowPopup.ClearValue(Popup.CustomPopupPlacementCallbackProperty);
				this.m_overflowPopup.ClearValue(CustomPopupPlacementHelper.PlacementProperty);
				this.m_overflowPopup.Opened -= new EventHandler(this.OnOverflowPopupOpened);
				this.m_overflowPopup.Closed -= new EventHandler(this.OnOverflowPopupClosed);
			}
			if (this.m_toolBarPanel != null)
			{
				this.m_toolBarPanel.HasChildrenChanged -= new EventHandler(this.OnToolBarPanelHasChildrenChanged);
			}
			base.OnApplyTemplate();
			this.m_layoutRoot = ControlExtensions.GetTemplateRoot(this);
			this.m_moreButton = (base.GetTemplateChild("MoreButton") as ButtonBase);
			this.m_overflowPopup = (base.GetTemplateChild("OverflowPopup") as Popup);
			this.m_toolBarPanel = (base.GetTemplateChild("PART_ToolBarPanel") as CommandBarPanel);
			this.m_toolBarOverflowPanel = (base.GetTemplateChild("PART_ToolBarOverflowPanel") as CommandBarOverflowPanel);
			if (this.m_moreButton != null)
			{
				AutomationProperties.SetName(this.m_moreButton, Strings.AppBarMoreButtonName);
				this.UpdateMoreButtonTooTip();
			}
			if (this.m_overflowPopup != null)
			{
				this.m_overflowPopup.CustomPopupPlacementCallback = new CustomPopupPlacementCallback(this.PositionOverflowPopup);
				this.m_overflowPopup.SetValue(CustomPopupPlacementHelper.PlacementProperty, 8);
				this.m_overflowPopup.Opened += new EventHandler(this.OnOverflowPopupOpened);
				this.m_overflowPopup.Closed += new EventHandler(this.OnOverflowPopupClosed);
			}
			if (this.m_toolBarPanel != null)
			{
				this.m_toolBarPanel.HasChildrenChanged += new EventHandler(this.OnToolBarPanelHasChildrenChanged);
			}
			CommandBar commandBar = base.TemplatedParent as CommandBar;
			if (commandBar != null)
			{
				commandBar.UpdateVisualState(false);
			}
			this.InvalidateLayout();
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x00029448 File Offset: 0x00027648
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			if (element is AppBarButton || element is AppBarToggleButton)
			{
				((FrameworkElement)element).SetBinding(CommandBarToolBar.DefaultLabelPositionProperty, CommandBarToolBar.DefaultLabelPositionProperty, this);
			}
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x00029479 File Offset: 0x00027679
		protected override void ClearContainerForItemOverride(DependencyObject element, object item)
		{
			if (element is AppBarButton || element is AppBarToggleButton)
			{
				element.ClearValue(CommandBarToolBar.DefaultLabelPositionProperty);
			}
			base.ClearContainerForItemOverride(element, item);
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x0002949E File Offset: 0x0002769E
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == ToolBar.HasOverflowItemsProperty)
			{
				this.UpdateEffectiveOverflowButtonVisibility();
				EventHandler hasOverflowItemsChanged = this.HasOverflowItemsChanged;
				if (hasOverflowItemsChanged == null)
				{
					return;
				}
				hasOverflowItemsChanged.Invoke(this, EventArgs.Empty);
			}
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x000294D4 File Offset: 0x000276D4
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.Key == 13)
			{
				ToolBarOverflowPanel toolBarOverflowPanel = this.m_toolBarOverflowPanel;
				if (toolBarOverflowPanel != null && toolBarOverflowPanel.IsKeyboardFocusWithin)
				{
					this.MoveFocus(new TraversalRequest(3));
				}
				else
				{
					Keyboard.Focus(null);
				}
				base.SetCurrentValue(ToolBar.IsOverflowOpenProperty, false);
			}
			base.OnKeyDown(e);
		}

		// Token: 0x06000C11 RID: 3089 RVA: 0x0002952B File Offset: 0x0002772B
		private static void OnIsOverflowOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((CommandBarToolBar)d).UpdateMoreButtonTooTip();
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x00029538 File Offset: 0x00027738
		private void InvalidateLayout()
		{
			FrameworkElement layoutRoot = this.m_layoutRoot;
			if (layoutRoot != null)
			{
				base.Dispatcher.BeginInvoke(delegate()
				{
					if (layoutRoot.ActualHeight > 0.0)
					{
						layoutRoot.Height = layoutRoot.ActualHeight;
						layoutRoot.UpdateLayout();
						layoutRoot.ClearValue(FrameworkElement.HeightProperty);
					}
				}, Array.Empty<object>());
			}
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x0002957C File Offset: 0x0002777C
		private void OnOverflowPopupOpened(object sender, EventArgs e)
		{
			this.UpdateOverflowContentMaxHeight();
			EventHandler overflowOpened = this.OverflowOpened;
			if (overflowOpened == null)
			{
				return;
			}
			overflowOpened.Invoke(this, EventArgs.Empty);
		}

		// Token: 0x06000C14 RID: 3092 RVA: 0x0002959A File Offset: 0x0002779A
		private void OnOverflowPopupClosed(object sender, EventArgs e)
		{
			EventHandler overflowClosed = this.OverflowClosed;
			if (overflowClosed == null)
			{
				return;
			}
			overflowClosed.Invoke(this, EventArgs.Empty);
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x000295B2 File Offset: 0x000277B2
		private void OnToolBarPanelHasChildrenChanged(object sender, EventArgs e)
		{
			EventHandler hasPrimaryCommandsChanged = this.HasPrimaryCommandsChanged;
			if (hasPrimaryCommandsChanged == null)
			{
				return;
			}
			hasPrimaryCommandsChanged.Invoke(this, e);
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x000295C6 File Offset: 0x000277C6
		private CustomPopupPlacement[] PositionOverflowPopup(Size popupSize, Size targetSize, Point offset)
		{
			return CustomPopupPlacementHelper.PositionPopup(8, popupSize, targetSize, offset, this.m_overflowPopup.Child as FrameworkElement);
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x000295E1 File Offset: 0x000277E1
		private void UpdateMoreButtonTooTip()
		{
			if (this.m_moreButton != null)
			{
				this.m_moreButton.ToolTip = (base.IsOverflowOpen ? this.m_moreButtonOpenToolTip : this.m_moreButtonClosedToolTip);
			}
		}

		// Token: 0x040004E5 RID: 1253
		public static readonly DependencyProperty CornerRadiusProperty = ControlHelper.CornerRadiusProperty.AddOwner(typeof(CommandBarToolBar));

		// Token: 0x040004E6 RID: 1254
		public static readonly DependencyProperty DefaultLabelPositionProperty = DependencyProperty.RegisterAttached("DefaultLabelPosition", typeof(CommandBarDefaultLabelPosition), typeof(CommandBarToolBar), new PropertyMetadata(CommandBarDefaultLabelPosition.Right));

		// Token: 0x040004E7 RID: 1255
		public static readonly DependencyProperty IsDynamicOverflowEnabledProperty = DependencyProperty.Register("IsDynamicOverflowEnabled", typeof(bool), typeof(CommandBarToolBar), new PropertyMetadata(true));

		// Token: 0x040004E8 RID: 1256
		public static readonly DependencyProperty OverflowButtonVisibilityProperty = DependencyProperty.Register("OverflowButtonVisibility", typeof(CommandBarOverflowButtonVisibility), typeof(CommandBarToolBar), new PropertyMetadata(CommandBarOverflowButtonVisibility.Auto, new PropertyChangedCallback(CommandBarToolBar.OnOverflowButtonVisibilityChanged)));

		// Token: 0x040004E9 RID: 1257
		public static readonly DependencyProperty OverflowPresenterStyleProperty = DependencyProperty.Register("OverflowPresenterStyle", typeof(Style), typeof(CommandBarToolBar), null);

		// Token: 0x040004EA RID: 1258
		private static readonly DependencyPropertyKey OverflowContentMaxHeightPropertyKey = DependencyProperty.RegisterReadOnly("OverflowContentMaxHeight", typeof(double), typeof(CommandBarToolBar), new PropertyMetadata(CommandBarToolBar.CalculateOverflowContentMaxHeight()));

		// Token: 0x040004EB RID: 1259
		public static readonly DependencyProperty OverflowContentMaxHeightProperty = CommandBarToolBar.OverflowContentMaxHeightPropertyKey.DependencyProperty;

		// Token: 0x040004EC RID: 1260
		private static readonly DependencyPropertyKey EffectiveOverflowButtonVisibilityPropertyKey = DependencyProperty.RegisterReadOnly("EffectiveOverflowButtonVisibility", typeof(Visibility), typeof(CommandBarToolBar), new PropertyMetadata(2, new PropertyChangedCallback(CommandBarToolBar.OnEffectiveOverflowButtonVisibilityChanged)));

		// Token: 0x040004ED RID: 1261
		public static readonly DependencyProperty EffectiveOverflowButtonVisibilityProperty = CommandBarToolBar.EffectiveOverflowButtonVisibilityPropertyKey.DependencyProperty;

		// Token: 0x040004F2 RID: 1266
		private FrameworkElement m_layoutRoot;

		// Token: 0x040004F3 RID: 1267
		private ButtonBase m_moreButton;

		// Token: 0x040004F4 RID: 1268
		private Popup m_overflowPopup;

		// Token: 0x040004F5 RID: 1269
		private CommandBarPanel m_toolBarPanel;

		// Token: 0x040004F6 RID: 1270
		private CommandBarOverflowPanel m_toolBarOverflowPanel;

		// Token: 0x040004F7 RID: 1271
		private readonly ToolTip m_moreButtonClosedToolTip;

		// Token: 0x040004F8 RID: 1272
		private readonly ToolTip m_moreButtonOpenToolTip;

		// Token: 0x040004F9 RID: 1273
		private const string OverflowPopupName = "OverflowPopup";

		// Token: 0x040004FA RID: 1274
		private const string ToolBarPanelName = "PART_ToolBarPanel";

		// Token: 0x040004FB RID: 1275
		private const string ToolBarOverflowPanelName = "PART_ToolBarOverflowPanel";
	}
}
