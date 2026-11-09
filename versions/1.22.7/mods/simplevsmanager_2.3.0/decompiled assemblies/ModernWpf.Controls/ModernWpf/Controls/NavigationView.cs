using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shell;
using ModernWpf.Automation.Peers;
using ModernWpf.Controls.Primitives;
using ModernWpf.Input;
using ModernWpf.Media.Animation;

namespace ModernWpf.Controls
{
	// Token: 0x0200005B RID: 91
	public class NavigationView : ContentControl, IControlProtected
	{
		// Token: 0x06000322 RID: 802 RVA: 0x0000A57E File Offset: 0x0000877E
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new NavigationViewAutomationPeer(this);
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000A588 File Offset: 0x00008788
		private void UnhookEventsAndClearFields(bool isFromDestructor = false)
		{
			if (this.m_coreTitleBar != null)
			{
				this.m_coreTitleBar.LayoutMetricsChanged -= new TypedEventHandler<CoreApplicationViewTitleBar, object>(this.OnTitleBarMetricsChanged);
				this.m_coreTitleBar.IsVisibleChanged -= new TypedEventHandler<CoreApplicationViewTitleBar, object>(this.OnTitleBarIsVisibleChanged);
			}
			if (this.m_paneToggleButton != null)
			{
				this.m_paneToggleButton.Click -= new RoutedEventHandler(this.OnPaneToggleButtonClick);
			}
			this.m_settingsItem = null;
			if (this.m_paneSearchButton != null)
			{
				this.m_paneSearchButton.Click -= new RoutedEventHandler(this.OnPaneSearchButtonClick);
				this.m_paneSearchButton = null;
			}
			this.m_paneHeaderOnTopPane = null;
			this.m_paneTitleOnTopPane = null;
			FrameworkElementSizeChangedRevoker itemsContainerSizeChangedRevoker = this.m_itemsContainerSizeChangedRevoker;
			if (itemsContainerSizeChangedRevoker != null)
			{
				itemsContainerSizeChangedRevoker.Revoke();
			}
			if (this.m_paneTitleHolderFrameworkElement != null)
			{
				this.m_paneTitleHolderFrameworkElement.SizeChanged -= new SizeChangedEventHandler(this.OnPaneTitleHolderSizeChanged);
				this.m_paneTitleHolderFrameworkElement = null;
			}
			this.m_paneTitleFrameworkElement = null;
			this.m_paneTitlePresenter = null;
			this.m_paneHeaderCloseButtonColumn = null;
			this.m_paneHeaderToggleButtonColumn = null;
			this.m_paneHeaderContentBorderRow = null;
			if (this.m_leftNavRepeater != null)
			{
				this.m_leftNavRepeater.ElementPrepared -= new TypedEventHandler<ItemsRepeater, ItemsRepeaterElementPreparedEventArgs>(this.OnRepeaterElementPrepared);
				this.m_leftNavRepeater.ElementClearing -= new TypedEventHandler<ItemsRepeater, ItemsRepeaterElementClearingEventArgs>(this.OnRepeaterElementClearing);
				this.m_leftNavRepeater.IsVisibleChanged -= new DependencyPropertyChangedEventHandler(this.OnRepeaterIsVisibleChanged);
				GettingFocusHelper leftNavRepeaterGettingFocusHelper = this.m_leftNavRepeaterGettingFocusHelper;
				if (leftNavRepeaterGettingFocusHelper != null)
				{
					leftNavRepeaterGettingFocusHelper.Dispose();
				}
				this.m_leftNavRepeater = null;
			}
			if (this.m_topNavRepeater != null)
			{
				this.m_topNavRepeater.ElementPrepared -= new TypedEventHandler<ItemsRepeater, ItemsRepeaterElementPreparedEventArgs>(this.OnRepeaterElementPrepared);
				this.m_topNavRepeater.ElementClearing -= new TypedEventHandler<ItemsRepeater, ItemsRepeaterElementClearingEventArgs>(this.OnRepeaterElementClearing);
				this.m_topNavRepeater.IsVisibleChanged -= new DependencyPropertyChangedEventHandler(this.OnRepeaterIsVisibleChanged);
				GettingFocusHelper topNavRepeaterGettingFocusHelper = this.m_topNavRepeaterGettingFocusHelper;
				if (topNavRepeaterGettingFocusHelper != null)
				{
					topNavRepeaterGettingFocusHelper.Dispose();
				}
				this.m_topNavRepeater = null;
			}
			if (this.m_leftNavFooterMenuRepeater != null)
			{
				this.m_leftNavFooterMenuRepeater.ElementPrepared -= new TypedEventHandler<ItemsRepeater, ItemsRepeaterElementPreparedEventArgs>(this.OnRepeaterElementPrepared);
				this.m_leftNavFooterMenuRepeater.ElementClearing -= new TypedEventHandler<ItemsRepeater, ItemsRepeaterElementClearingEventArgs>(this.OnRepeaterElementClearing);
				this.m_leftNavFooterMenuRepeater.IsVisibleChanged -= new DependencyPropertyChangedEventHandler(this.OnRepeaterIsVisibleChanged);
				GettingFocusHelper leftNavFooterMenuRepeaterGettingFocusHelper = this.m_leftNavFooterMenuRepeaterGettingFocusHelper;
				if (leftNavFooterMenuRepeaterGettingFocusHelper != null)
				{
					leftNavFooterMenuRepeaterGettingFocusHelper.Dispose();
				}
				this.m_leftNavFooterMenuRepeater = null;
			}
			if (this.m_topNavFooterMenuRepeater != null)
			{
				this.m_topNavFooterMenuRepeater.ElementPrepared -= new TypedEventHandler<ItemsRepeater, ItemsRepeaterElementPreparedEventArgs>(this.OnRepeaterElementPrepared);
				this.m_topNavFooterMenuRepeater.ElementClearing -= new TypedEventHandler<ItemsRepeater, ItemsRepeaterElementClearingEventArgs>(this.OnRepeaterElementClearing);
				this.m_topNavFooterMenuRepeater.IsVisibleChanged -= new DependencyPropertyChangedEventHandler(this.OnRepeaterIsVisibleChanged);
				GettingFocusHelper topNavFooterMenuRepeaterGettingFocusHelper = this.m_topNavFooterMenuRepeaterGettingFocusHelper;
				if (topNavFooterMenuRepeaterGettingFocusHelper != null)
				{
					topNavFooterMenuRepeaterGettingFocusHelper.Dispose();
				}
				this.m_topNavFooterMenuRepeater = null;
			}
			ItemsSourceView.CollectionChangedRevoker footerItemsCollectionChangedRevoker = this.m_footerItemsCollectionChangedRevoker;
			if (footerItemsCollectionChangedRevoker != null)
			{
				footerItemsCollectionChangedRevoker.Revoke();
			}
			ItemsSourceView.CollectionChangedRevoker menuItemsCollectionChangedRevoker = this.m_menuItemsCollectionChangedRevoker;
			if (menuItemsCollectionChangedRevoker != null)
			{
				menuItemsCollectionChangedRevoker.Revoke();
			}
			if (this.m_topNavRepeaterOverflowView != null)
			{
				this.m_topNavRepeaterOverflowView.ElementPrepared -= new TypedEventHandler<ItemsRepeater, ItemsRepeaterElementPreparedEventArgs>(this.OnRepeaterElementPrepared);
				this.m_topNavRepeaterOverflowView.ElementClearing -= new TypedEventHandler<ItemsRepeater, ItemsRepeaterElementClearingEventArgs>(this.OnRepeaterElementClearing);
				this.m_topNavRepeaterOverflowView = null;
			}
			ItemsSourceView.CollectionChangedRevoker topNavOverflowItemsCollectionChangedRevoker = this.m_topNavOverflowItemsCollectionChangedRevoker;
			if (topNavOverflowItemsCollectionChangedRevoker != null)
			{
				topNavOverflowItemsCollectionChangedRevoker.Revoke();
			}
			if (isFromDestructor)
			{
				this.m_selectionModel.SelectionChanged -= new TypedEventHandler<SelectionModel, SelectionModelSelectionChangedEventArgs>(this.OnSelectionModelSelectionChanged);
			}
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000A8A0 File Offset: 0x00008AA0
		static NavigationView()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationView), new FrameworkPropertyMetadata(typeof(NavigationView)));
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000B23C File Offset: 0x0000943C
		public NavigationView()
		{
			base.SetValue(NavigationView.TemplateSettingsPropertyKey, new NavigationViewTemplateSettings());
			base.SizeChanged += new SizeChangedEventHandler(this.OnSizeChanged);
			this.m_selectionModelSource = new List<object>(2);
			this.m_selectionModelSource.Add(null);
			this.m_selectionModelSource.Add(null);
			ObservableCollection<object> observableCollection = new ObservableCollection<object>();
			base.SetValue(NavigationView.MenuItemsPropertyKey, observableCollection);
			ObservableCollection<object> observableCollection2 = new ObservableCollection<object>();
			base.SetValue(NavigationView.FooterMenuItemsPropertyKey, observableCollection2);
			WeakReference<NavigationView> weakThis = new WeakReference<NavigationView>(this);
			this.m_topDataProvider.OnRawDataChanged(delegate(NotifyCollectionChangedEventArgs args)
			{
				NavigationView navigationView;
				if (weakThis.TryGetTarget(ref navigationView))
				{
					navigationView.OnTopNavDataSourceChanged(args);
				}
			});
			base.Unloaded += new RoutedEventHandler(this.OnUnloaded);
			base.Loaded += new RoutedEventHandler(this.OnLoaded);
			this.m_selectionModel.SingleSelect = true;
			this.m_selectionModel.Source = this.m_selectionModelSource;
			this.m_selectionModel.SelectionChanged += new TypedEventHandler<SelectionModel, SelectionModelSelectionChangedEventArgs>(this.OnSelectionModelSelectionChanged);
			this.m_selectionModel.ChildrenRequested += new TypedEventHandler<SelectionModel, SelectionModelChildrenRequestedEventArgs>(this.OnSelectionModelChildrenRequested);
			this.m_navigationViewItemsFactory = new NavigationViewItemsFactory();
			this.m_bitmapCache = new BitmapCache();
			this.m_bitmapCache.RenderAtScale = VisualTreeHelper.GetDpi(this).PixelsPerDip;
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000B3A8 File Offset: 0x000095A8
		private void OnSelectionModelChildrenRequested(SelectionModel selectionModel, SelectionModelChildrenRequestedEventArgs e)
		{
			if (e.SourceIndex.GetSize() == 1)
			{
				e.Children = e.Source;
				return;
			}
			NavigationViewItem navigationViewItem = e.Source as NavigationViewItem;
			if (navigationViewItem != null)
			{
				e.Children = this.GetChildren(navigationViewItem);
				return;
			}
			object childrenForItemInIndexPath = this.GetChildrenForItemInIndexPath(e.SourceIndex, true);
			if (childrenForItemInIndexPath != null)
			{
				e.Children = childrenForItemInIndexPath;
			}
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000B405 File Offset: 0x00009605
		private void OnFooterItemsSourceCollectionChanged(object sender, object e)
		{
			this.UpdateFooterRepeaterItemsSource(false, true);
			this.UpdatePaneLayout();
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000B415 File Offset: 0x00009615
		private void OnOverflowItemsSourceCollectionChanged(object sender, object e)
		{
			if (this.m_topNavRepeaterOverflowView.ItemsSourceView.Count == 0)
			{
				this.SetOverflowButtonVisibility(2);
			}
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000B430 File Offset: 0x00009630
		private void OnSelectionModelSelectionChanged(SelectionModel selectionModel, SelectionModelSelectionChangedEventArgs e)
		{
			NavigationView.<>c__DisplayClass64_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			object selectedItem = selectionModel.SelectedItem;
			if (this.m_shouldIgnoreNextSelectionChange || selectedItem == this.SelectedItem || !this.m_appliedTemplate)
			{
				return;
			}
			bool flag = true;
			CS$<>8__locals1.selectedIndex = selectionModel.SelectedIndex;
			if (this.IsTopNavigationView() && (CS$<>8__locals1.selectedIndex != null && CS$<>8__locals1.selectedIndex.GetSize() > 1 && CS$<>8__locals1.selectedIndex.GetAt(0) == 0 && !this.m_topDataProvider.IsItemInPrimaryList(CS$<>8__locals1.selectedIndex.GetAt(1))))
			{
				if (this.<OnSelectionModelSelectionChanged>g__init|64_0(ref CS$<>8__locals1))
				{
					this.SelectandMoveOverflowItem(selectedItem, CS$<>8__locals1.selectedIndex, true);
					flag = false;
				}
				else
				{
					this.m_moveTopNavOverflowItemOnFlyoutClose = true;
				}
			}
			if (flag)
			{
				this.SetSelectedItemAndExpectItemInvokeWhenSelectionChangedIfNotInvokedFromAPI(selectedItem);
			}
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000B4F0 File Offset: 0x000096F0
		private void SelectandMoveOverflowItem(object selectedItem, IndexPath selectedIndex, bool closeFlyout)
		{
			try
			{
				this.m_selectionChangeFromOverflowMenu = true;
				if (closeFlyout)
				{
					this.CloseTopNavigationViewFlyout();
				}
				if (!this.IsSelectionSuppressed(selectedItem))
				{
					this.SelectOverflowItem(selectedItem, selectedIndex);
				}
			}
			finally
			{
				this.m_selectionChangeFromOverflowMenu = false;
			}
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000B538 File Offset: 0x00009738
		private void CloseFlyoutIfRequired(NavigationViewItem selectedItem)
		{
			IndexPath selectedIndex = this.m_selectionModel.SelectedIndex;
			if (this.<CloseFlyoutIfRequired>g__init|66_0() && selectedIndex != null && !this.DoesNavigationViewItemHaveChildren(selectedItem))
			{
				UIElement containerForIndex = this.GetContainerForIndex(selectedIndex.GetAt(1), selectedIndex.GetAt(0) == 1);
				if (containerForIndex != null)
				{
					NavigationViewItem navigationViewItem = containerForIndex as NavigationViewItem;
					if (navigationViewItem != null && navigationViewItem.ShouldRepeaterShowInFlyout())
					{
						navigationViewItem.IsExpanded = false;
					}
				}
			}
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000B59C File Offset: 0x0000979C
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.m_appliedTemplate = false;
			this.UnhookEventsAndClearFields(false);
			Button button = base.GetTemplateChild("TogglePaneButton") as Button;
			if (button != null)
			{
				this.m_paneToggleButton = button;
				button.Click += new RoutedEventHandler(this.OnPaneToggleButtonClick);
				this.SetPaneToggleButtonAutomationName();
				WindowChrome.SetIsHitTestVisibleInChrome(button, true);
			}
			this.m_leftNavPaneHeaderContentBorder = (base.GetTemplateChild("PaneHeaderContentBorder") as ContentControl);
			this.m_leftNavPaneCustomContentBorder = (base.GetTemplateChild("PaneCustomContentBorder") as ContentControl);
			this.m_leftNavFooterContentBorder = (base.GetTemplateChild("FooterContentBorder") as ContentControl);
			this.m_paneHeaderOnTopPane = (base.GetTemplateChild("PaneHeaderOnTopPane") as ContentControl);
			this.m_paneTitleOnTopPane = (base.GetTemplateChild("PaneTitleOnTopPane") as ContentControl);
			this.m_paneCustomContentOnTopPane = (base.GetTemplateChild("PaneCustomContentOnTopPane") as ContentControl);
			this.m_paneFooterOnTopPane = (base.GetTemplateChild("PaneFooterOnTopPane") as ContentControl);
			SplitView splitView = base.GetTemplateChild("RootSplitView") as SplitView;
			if (splitView != null)
			{
				this.m_rootSplitView = splitView;
				splitView.IsPaneOpenChanged += this.OnSplitViewClosedCompactChanged;
				splitView.DisplayModeChanged += this.OnSplitViewClosedCompactChanged;
				if (SharedHelpers.IsRS3OrHigher())
				{
					splitView.PaneClosed += new TypedEventHandler<SplitView, object>(this.OnSplitViewPaneClosed);
					splitView.PaneClosing += new TypedEventHandler<SplitView, SplitViewPaneClosingEventArgs>(this.OnSplitViewPaneClosing);
					splitView.PaneOpened += new TypedEventHandler<SplitView, object>(this.OnSplitViewPaneOpened);
					splitView.PaneOpening += new TypedEventHandler<SplitView, object>(this.OnSplitViewPaneOpening);
				}
				this.UpdateIsClosedCompact();
			}
			this.m_topNavGrid = (base.GetTemplateChild("TopNavGrid") as Grid);
			ItemsRepeater itemsRepeater = base.GetTemplateChild("MenuItemsHost") as ItemsRepeater;
			if (itemsRepeater != null)
			{
				this.m_leftNavRepeater = itemsRepeater;
				StackLayout stackLayout = itemsRepeater.Layout as StackLayout;
				if (stackLayout != null)
				{
					stackLayout.DisableVirtualization = true;
				}
				itemsRepeater.ElementPrepared += new TypedEventHandler<ItemsRepeater, ItemsRepeaterElementPreparedEventArgs>(this.OnRepeaterElementPrepared);
				itemsRepeater.ElementClearing += new TypedEventHandler<ItemsRepeater, ItemsRepeaterElementClearingEventArgs>(this.OnRepeaterElementClearing);
				itemsRepeater.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.OnRepeaterIsVisibleChanged);
				this.m_leftNavRepeaterGettingFocusHelper = new GettingFocusHelper(itemsRepeater);
				this.m_leftNavRepeaterGettingFocusHelper.GettingFocus += new TypedEventHandler<UIElement, GettingFocusEventArgs>(this.OnRepeaterGettingFocus);
				itemsRepeater.ItemTemplate = this.m_navigationViewItemsFactory;
			}
			ItemsRepeater itemsRepeater2 = base.GetTemplateChild("TopNavMenuItemsHost") as ItemsRepeater;
			if (itemsRepeater2 != null)
			{
				this.m_topNavRepeater = itemsRepeater2;
				StackLayout stackLayout2 = itemsRepeater2.Layout as StackLayout;
				if (stackLayout2 != null)
				{
					stackLayout2.DisableVirtualization = true;
				}
				itemsRepeater2.ElementPrepared += new TypedEventHandler<ItemsRepeater, ItemsRepeaterElementPreparedEventArgs>(this.OnRepeaterElementPrepared);
				itemsRepeater2.ElementClearing += new TypedEventHandler<ItemsRepeater, ItemsRepeaterElementClearingEventArgs>(this.OnRepeaterElementClearing);
				itemsRepeater2.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.OnRepeaterIsVisibleChanged);
				this.m_topNavRepeaterGettingFocusHelper = new GettingFocusHelper(itemsRepeater2);
				this.m_topNavRepeaterGettingFocusHelper.GettingFocus += new TypedEventHandler<UIElement, GettingFocusEventArgs>(this.OnRepeaterGettingFocus);
				itemsRepeater2.ItemTemplate = this.m_navigationViewItemsFactory;
			}
			ItemsRepeater itemsRepeater3 = base.GetTemplateChild("TopNavMenuItemsOverflowHost") as ItemsRepeater;
			if (itemsRepeater3 != null)
			{
				this.m_topNavRepeaterOverflowView = itemsRepeater3;
				StackLayout stackLayout3 = itemsRepeater3.Layout as StackLayout;
				if (stackLayout3 != null)
				{
					stackLayout3.DisableVirtualization = true;
				}
				itemsRepeater3.ElementPrepared += new TypedEventHandler<ItemsRepeater, ItemsRepeaterElementPreparedEventArgs>(this.OnRepeaterElementPrepared);
				itemsRepeater3.ElementClearing += new TypedEventHandler<ItemsRepeater, ItemsRepeaterElementClearingEventArgs>(this.OnRepeaterElementClearing);
				itemsRepeater3.ItemTemplate = this.m_navigationViewItemsFactory;
			}
			Button button2 = base.GetTemplateChild("TopNavOverflowButton") as Button;
			if (button2 != null)
			{
				this.m_topNavOverflowButton = button2;
				AutomationProperties.SetName(button2, NavigationView.ResourceAccessor.GetLocalizedStringResource("NavigationOverflowButtonName"));
				button2.Content = NavigationView.ResourceAccessor.GetLocalizedStringResource("NavigationOverflowButtonText");
				if (ToolTipService.GetToolTip(button2) == null)
				{
					ToolTipService.SetToolTip(button2, new ToolTip
					{
						Content = NavigationView.ResourceAccessor.GetLocalizedStringResource("NavigationOverflowButtonToolTip")
					});
				}
				FlyoutBase flyout = FlyoutService.GetFlyout(button2);
				if (flyout != null)
				{
					flyout.Closing += new TypedEventHandler<FlyoutBase, FlyoutBaseClosingEventArgs>(this.OnFlyoutClosing);
					flyout.Offset = 0.0;
				}
			}
			ItemsRepeater templateChildT = CppWinRTHelpers.GetTemplateChildT<ItemsRepeater>("FooterMenuItemsHost", this);
			if (templateChildT != null)
			{
				this.m_leftNavFooterMenuRepeater = templateChildT;
				StackLayout stackLayout4 = templateChildT.Layout as StackLayout;
				if (stackLayout4 != null)
				{
					stackLayout4.DisableVirtualization = true;
				}
				templateChildT.ElementPrepared += new TypedEventHandler<ItemsRepeater, ItemsRepeaterElementPreparedEventArgs>(this.OnRepeaterElementPrepared);
				templateChildT.ElementClearing += new TypedEventHandler<ItemsRepeater, ItemsRepeaterElementClearingEventArgs>(this.OnRepeaterElementClearing);
				templateChildT.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.OnRepeaterIsVisibleChanged);
				this.m_leftNavFooterMenuRepeaterGettingFocusHelper = new GettingFocusHelper(templateChildT);
				this.m_leftNavFooterMenuRepeaterGettingFocusHelper.GettingFocus += new TypedEventHandler<UIElement, GettingFocusEventArgs>(this.OnRepeaterGettingFocus);
				templateChildT.ItemTemplate = this.m_navigationViewItemsFactory;
			}
			ItemsRepeater templateChildT2 = CppWinRTHelpers.GetTemplateChildT<ItemsRepeater>("TopFooterMenuItemsHost", this);
			if (templateChildT2 != null)
			{
				this.m_topNavFooterMenuRepeater = templateChildT2;
				StackLayout stackLayout5 = templateChildT2.Layout as StackLayout;
				if (stackLayout5 != null)
				{
					stackLayout5.DisableVirtualization = true;
				}
				templateChildT2.ElementPrepared += new TypedEventHandler<ItemsRepeater, ItemsRepeaterElementPreparedEventArgs>(this.OnRepeaterElementPrepared);
				templateChildT2.ElementClearing += new TypedEventHandler<ItemsRepeater, ItemsRepeaterElementClearingEventArgs>(this.OnRepeaterElementClearing);
				templateChildT2.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.OnRepeaterIsVisibleChanged);
				this.m_topNavFooterMenuRepeaterGettingFocusHelper = new GettingFocusHelper(templateChildT2);
				this.m_topNavFooterMenuRepeaterGettingFocusHelper.GettingFocus += new TypedEventHandler<UIElement, GettingFocusEventArgs>(this.OnRepeaterGettingFocus);
				templateChildT2.ItemTemplate = this.m_navigationViewItemsFactory;
			}
			this.m_topNavContentOverlayAreaGrid = (base.GetTemplateChild("TopNavContentOverlayAreaGrid") as Border);
			this.m_leftNavPaneAutoSuggestBoxPresenter = (base.GetTemplateChild("PaneAutoSuggestBoxPresenter") as ContentControl);
			this.m_topNavPaneAutoSuggestBoxPresenter = (base.GetTemplateChild("TopPaneAutoSuggestBoxPresenter") as ContentControl);
			this.m_paneContentGrid = (base.GetTemplateChild("PaneContentGrid") as UIElement);
			this.m_contentLeftPadding = (base.GetTemplateChild("ContentLeftPadding") as FrameworkElement);
			this.m_paneHeaderCloseButtonColumn = (base.GetTemplateChild("PaneHeaderCloseButtonColumn") as ColumnDefinition);
			this.m_paneHeaderToggleButtonColumn = (base.GetTemplateChild("PaneHeaderToggleButtonColumn") as ColumnDefinition);
			this.m_paneHeaderContentBorderRow = (base.GetTemplateChild("PaneHeaderContentBorderRow") as RowDefinition);
			this.m_paneTitleFrameworkElement = (base.GetTemplateChild("PaneTitleTextBlock") as FrameworkElement);
			this.m_paneTitlePresenter = (base.GetTemplateChild("PaneTitlePresenter") as ContentControl);
			FrameworkElement frameworkElement = base.GetTemplateChild("PaneTitleHolder") as FrameworkElement;
			if (frameworkElement != null)
			{
				this.m_paneTitleHolderFrameworkElement = frameworkElement;
				frameworkElement.SizeChanged += new SizeChangedEventHandler(this.OnPaneTitleHolderSizeChanged);
			}
			Button button3 = base.GetTemplateChild("PaneAutoSuggestButton") as Button;
			if (button3 != null)
			{
				this.m_paneSearchButton = button3;
				button3.Click += new RoutedEventHandler(this.OnPaneSearchButtonClick);
				string localizedStringResource = NavigationView.ResourceAccessor.GetLocalizedStringResource("NavigationViewSearchButtonName");
				AutomationProperties.SetName(button3, localizedStringResource);
				ToolTipService.SetToolTip(button3, new ToolTip
				{
					Content = localizedStringResource
				});
			}
			Button button4 = base.GetTemplateChild("NavigationViewBackButton") as Button;
			if (button4 != null)
			{
				this.m_backButton = button4;
				button4.Click += new RoutedEventHandler(this.OnBackButtonClicked);
				string localizedStringResource2 = NavigationView.ResourceAccessor.GetLocalizedStringResource("NavigationBackButtonName");
				AutomationProperties.SetName(button4, localizedStringResource2);
				WindowChrome.SetIsHitTestVisibleInChrome(button4, true);
			}
			CoreApplicationViewTitleBar titleBar = CoreApplicationViewTitleBar.GetTitleBar(this);
			if (titleBar != null)
			{
				this.m_coreTitleBar = titleBar;
				titleBar.LayoutMetricsChanged += new TypedEventHandler<CoreApplicationViewTitleBar, object>(this.OnTitleBarMetricsChanged);
				titleBar.IsVisibleChanged += new TypedEventHandler<CoreApplicationViewTitleBar, object>(this.OnTitleBarIsVisibleChanged);
				if (this.ShouldPreserveNavigationViewRS4Behavior())
				{
					this.m_togglePaneTopPadding = (base.GetTemplateChild("TogglePaneTopPadding") as FrameworkElement);
					this.m_contentPaneTopPadding = (base.GetTemplateChild("ContentPaneTopPadding") as FrameworkElement);
				}
			}
			ToolTip toolTip = base.GetTemplateChild("NavigationViewBackButtonToolTip") as ToolTip;
			if (toolTip != null)
			{
				string localizedStringResource3 = NavigationView.ResourceAccessor.GetLocalizedStringResource("NavigationBackButtonToolTip");
				toolTip.Content = localizedStringResource3;
			}
			Button button5 = base.GetTemplateChild("NavigationViewCloseButton") as Button;
			if (button5 != null)
			{
				this.m_closeButton = button5;
				button5.Click += new RoutedEventHandler(this.OnPaneToggleButtonClick);
				string localizedStringResource4 = NavigationView.ResourceAccessor.GetLocalizedStringResource("NavigationCloseButtonName");
				AutomationProperties.SetName(button5, localizedStringResource4);
				WindowChrome.SetIsHitTestVisibleInChrome(button5, true);
			}
			ToolTip toolTip2 = base.GetTemplateChild("NavigationViewCloseButtonToolTip") as ToolTip;
			if (toolTip2 != null)
			{
				string localizedStringResource5 = NavigationView.ResourceAccessor.GetLocalizedStringResource("NavigationButtonOpenName");
				toolTip2.Content = localizedStringResource5;
			}
			this.m_itemsContainerRow = CppWinRTHelpers.GetTemplateChildT<RowDefinition>("ItemsContainerRow", this);
			this.m_menuItemsScrollViewer = CppWinRTHelpers.GetTemplateChildT<FrameworkElement>("MenuItemsScrollViewer", this);
			this.m_footerItemsScrollViewer = CppWinRTHelpers.GetTemplateChildT<FrameworkElement>("FooterItemsScrollViewer", this);
			this.m_visualItemsSeparator = CppWinRTHelpers.GetTemplateChildT<FrameworkElement>("VisualItemsSeparator", this);
			FrameworkElementSizeChangedRevoker itemsContainerSizeChangedRevoker = this.m_itemsContainerSizeChangedRevoker;
			if (itemsContainerSizeChangedRevoker != null)
			{
				itemsContainerSizeChangedRevoker.Revoke();
			}
			FrameworkElement templateChildT3 = CppWinRTHelpers.GetTemplateChildT<FrameworkElement>("ItemsContainerGrid", this);
			if (templateChildT3 != null)
			{
				this.m_itemsContainerSizeChangedRevoker = new FrameworkElementSizeChangedRevoker(templateChildT3, new SizeChangedEventHandler(this.OnItemsContainerSizeChanged));
			}
			if (SharedHelpers.IsRS2OrHigher())
			{
				Grid templateChildT4 = CppWinRTHelpers.GetTemplateChildT<Grid>("RootGrid", this);
				if (templateChildT4 != null)
				{
					KeyboardNavigation.SetDirectionalNavigation(templateChildT4, 4);
				}
				Grid templateChildT5 = CppWinRTHelpers.GetTemplateChildT<Grid>("ContentGrid", this);
				if (templateChildT5 != null)
				{
					KeyboardNavigation.SetDirectionalNavigation(templateChildT5, 3);
				}
			}
			this.UpdatePaneShadow();
			this.m_appliedTemplate = true;
			this.UpdatePaneDisplayMode();
			this.UpdateHeaderVisibility();
			this.UpdatePaneTitleFrameworkElementParents();
			this.UpdateTitleBarPadding();
			this.UpdatePaneTabFocusNavigation();
			this.UpdateBackAndCloseButtonsVisibility();
			this.UpdateSingleSelectionFollowsFocusTemplateSetting();
			this.UpdatePaneVisibility();
			this.UpdateVisualState(false);
			this.UpdatePaneTitleMargins();
			this.UpdatePaneLayout();
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000BEC8 File Offset: 0x0000A0C8
		private void UpdateRepeaterItemsSource(bool forceSelectionModelUpdate)
		{
			object obj = this.<UpdateRepeaterItemsSource>g__init|68_0();
			if (forceSelectionModelUpdate)
			{
				this.m_selectionModelSource[0] = obj;
			}
			ItemsSourceView.CollectionChangedRevoker menuItemsCollectionChangedRevoker = this.m_menuItemsCollectionChangedRevoker;
			if (menuItemsCollectionChangedRevoker != null)
			{
				menuItemsCollectionChangedRevoker.Revoke();
			}
			this.m_menuItemsSource = new InspectingDataSource(obj);
			this.m_menuItemsCollectionChangedRevoker = new ItemsSourceView.CollectionChangedRevoker(this.m_menuItemsSource, new NotifyCollectionChangedEventHandler(this.OnMenuItemsSourceCollectionChanged));
			if (this.IsTopNavigationView())
			{
				this.UpdateLeftRepeaterItemSource(null);
				this.UpdateTopNavRepeatersItemSource(obj);
				this.InvalidateTopNavPrimaryLayout();
				return;
			}
			this.UpdateTopNavRepeatersItemSource(null);
			this.UpdateLeftRepeaterItemSource(obj);
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000BF51 File Offset: 0x0000A151
		private void UpdateLeftRepeaterItemSource(object items)
		{
			this.UpdateItemsRepeaterItemsSource(this.m_leftNavRepeater, items);
			this.UpdatePaneLayout();
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0000BF66 File Offset: 0x0000A166
		private void UpdateTopNavRepeatersItemSource(object items)
		{
			this.m_topDataProvider.SetDataSource(items);
			this.UpdateTopNavPrimaryRepeaterItemsSource(items);
			this.UpdateTopNavOverflowRepeaterItemsSource(items);
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0000BF82 File Offset: 0x0000A182
		private void UpdateTopNavPrimaryRepeaterItemsSource(object items)
		{
			if (items != null)
			{
				this.UpdateItemsRepeaterItemsSource(this.m_topNavRepeater, this.m_topDataProvider.GetPrimaryItems());
				return;
			}
			this.UpdateItemsRepeaterItemsSource(this.m_topNavRepeater, null);
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0000BFAC File Offset: 0x0000A1AC
		private void UpdateTopNavOverflowRepeaterItemsSource(object items)
		{
			ItemsSourceView.CollectionChangedRevoker topNavOverflowItemsCollectionChangedRevoker = this.m_topNavOverflowItemsCollectionChangedRevoker;
			if (topNavOverflowItemsCollectionChangedRevoker != null)
			{
				topNavOverflowItemsCollectionChangedRevoker.Revoke();
			}
			ItemsRepeater topNavRepeaterOverflowView = this.m_topNavRepeaterOverflowView;
			if (topNavRepeaterOverflowView != null)
			{
				if (items != null)
				{
					IList overflowItems = this.m_topDataProvider.GetOverflowItems();
					topNavRepeaterOverflowView.ItemsSource = overflowItems;
					this.m_topNavOverflowItemsCollectionChangedRevoker = new ItemsSourceView.CollectionChangedRevoker(topNavRepeaterOverflowView.ItemsSourceView, new NotifyCollectionChangedEventHandler(this.OnOverflowItemsSourceCollectionChanged));
					return;
				}
				topNavRepeaterOverflowView.ItemsSource = null;
			}
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0000C00F File Offset: 0x0000A20F
		private void UpdateItemsRepeaterItemsSource(ItemsRepeater ir, object itemsSource)
		{
			if (ir != null)
			{
				ir.ItemsSource = itemsSource;
			}
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0000C01C File Offset: 0x0000A21C
		private void UpdateFooterRepeaterItemsSource(bool sourceCollectionReset, bool sourceCollectionChanged)
		{
			if (!this.m_appliedTemplate)
			{
				return;
			}
			object source = this.<UpdateFooterRepeaterItemsSource>g__init|74_0();
			this.UpdateItemsRepeaterItemsSource(this.m_leftNavFooterMenuRepeater, null);
			this.UpdateItemsRepeaterItemsSource(this.m_topNavFooterMenuRepeater, null);
			if (this.m_settingsItem == null || sourceCollectionChanged || sourceCollectionReset)
			{
				List<object> list = new List<object>();
				if (this.m_settingsItem == null)
				{
					this.m_settingsItem = new NavigationViewItem();
					NavigationViewItem settingsItem = this.m_settingsItem;
					settingsItem.Name = "SettingsItem";
					this.m_navigationViewItemsFactory.SettingsItem(settingsItem);
				}
				if (sourceCollectionReset)
				{
					if (this.m_footerItemsSource != null)
					{
						this.m_footerItemsSource.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnFooterItemsSourceCollectionChanged);
					}
					this.m_footerItemsSource = null;
				}
				if (this.m_footerItemsSource == null)
				{
					this.m_footerItemsSource = new InspectingDataSource(source);
					this.m_footerItemsCollectionChangedRevoker = new ItemsSourceView.CollectionChangedRevoker(this.m_footerItemsSource, new NotifyCollectionChangedEventHandler(this.OnFooterItemsSourceCollectionChanged));
				}
				if (this.m_footerItemsSource != null)
				{
					NavigationViewItem settingsItem2 = this.m_settingsItem;
					int count = this.m_footerItemsSource.Count;
					for (int i = 0; i < count; i++)
					{
						object at = this.m_footerItemsSource.GetAt(i);
						list.Add(at);
					}
					if (this.IsSettingsVisible)
					{
						this.CreateAndHookEventsToSettings();
						list.Add(settingsItem2);
					}
				}
				this.m_selectionModelSource[1] = list;
			}
			if (this.IsTopNavigationView())
			{
				this.UpdateItemsRepeaterItemsSource(this.m_topNavFooterMenuRepeater, this.m_selectionModelSource[1]);
				return;
			}
			ItemsRepeater leftNavFooterMenuRepeater = this.m_leftNavFooterMenuRepeater;
			if (leftNavFooterMenuRepeater != null)
			{
				this.UpdateItemsRepeaterItemsSource(this.m_leftNavFooterMenuRepeater, this.m_selectionModelSource[1]);
				leftNavFooterMenuRepeater.InvalidateMeasure();
				leftNavFooterMenuRepeater.UpdateLayout();
				this.UpdatePaneLayout();
			}
			NavigationViewItem settingsItem3 = this.m_settingsItem;
			if (settingsItem3 != null)
			{
				settingsItem3.BringIntoView();
			}
		}

		// Token: 0x06000334 RID: 820 RVA: 0x0000C1C8 File Offset: 0x0000A3C8
		private void OnFlyoutClosing(object sender, FlyoutBaseClosingEventArgs args)
		{
			if (this.m_moveTopNavOverflowItemOnFlyoutClose && !this.m_selectionChangeFromOverflowMenu)
			{
				this.m_moveTopNavOverflowItemOnFlyoutClose = false;
				IndexPath selectedIndex = this.m_selectionModel.SelectedIndex;
				if (selectedIndex.GetSize() > 0)
				{
					UIElement containerForIndex = this.GetContainerForIndex(selectedIndex.GetAt(1), false);
					if (containerForIndex != null)
					{
						NavigationViewItem navigationViewItem = containerForIndex as NavigationViewItem;
						if (navigationViewItem != null)
						{
							navigationViewItem.IsExpanded = false;
						}
					}
					this.SelectandMoveOverflowItem(this.SelectedItem, selectedIndex, false);
				}
			}
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0000C234 File Offset: 0x0000A434
		private void OnNavigationViewItemIsSelectedPropertyChanged(DependencyObject sender, DependencyProperty args)
		{
			NavigationViewItem navigationViewItem = sender as NavigationViewItem;
			if (navigationViewItem != null)
			{
				bool flag = this.IsContainerTheSelectedItemInTheSelectionModel(navigationViewItem);
				bool isSelected = navigationViewItem.IsSelected;
				if (isSelected && !flag)
				{
					IndexPath indexPathForContainer = this.GetIndexPathForContainer(navigationViewItem);
					this.UpdateSelectionModelSelection(indexPathForContainer);
				}
				else if (!isSelected && flag)
				{
					IndexPath indexPathForContainer2 = this.GetIndexPathForContainer(navigationViewItem);
					IndexPath selectedIndex = this.m_selectionModel.SelectedIndex;
					if (selectedIndex != null && indexPathForContainer2.CompareTo(selectedIndex) == 0)
					{
						this.m_selectionModel.DeselectAt(indexPathForContainer2);
					}
				}
				if (isSelected)
				{
					navigationViewItem.IsChildSelected = false;
				}
			}
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000C2B8 File Offset: 0x0000A4B8
		private void OnNavigationViewItemExpandedPropertyChanged(DependencyObject sender, DependencyProperty args)
		{
			NavigationViewItem navigationViewItem = sender as NavigationViewItem;
			if (navigationViewItem != null)
			{
				if (navigationViewItem.IsExpanded)
				{
					this.RaiseExpandingEvent(navigationViewItem);
				}
				this.ShowHideChildrenItemsRepeater(navigationViewItem);
				if (!navigationViewItem.IsExpanded)
				{
					this.RaiseCollapsedEvent(navigationViewItem);
				}
			}
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0000C2F4 File Offset: 0x0000A4F4
		private void RaiseItemInvokedForNavigationViewItem(NavigationViewItem nvi)
		{
			NavigationView.<>c__DisplayClass78_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.nvi = nvi;
			object item = null;
			CS$<>8__locals1.prevItem = this.SelectedItem;
			CS$<>8__locals1.parentIR = this.GetParentItemsRepeaterForContainer(CS$<>8__locals1.nvi);
			ItemsSourceView itemsSourceView = CS$<>8__locals1.parentIR.ItemsSourceView;
			if (itemsSourceView != null)
			{
				InspectingDataSource inspectingDataSource = (InspectingDataSource)itemsSourceView;
				int elementIndex = CS$<>8__locals1.parentIR.GetElementIndex(CS$<>8__locals1.nvi);
				if (elementIndex != -1)
				{
					item = inspectingDataSource.GetAt(elementIndex);
				}
			}
			NavigationRecommendedTransitionDirection recommendedDirection = this.<RaiseItemInvokedForNavigationViewItem>g__init|78_0(ref CS$<>8__locals1);
			this.RaiseItemInvoked(item, this.IsSettingsItem(CS$<>8__locals1.nvi), CS$<>8__locals1.nvi, recommendedDirection);
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0000C390 File Offset: 0x0000A590
		internal void OnNavigationViewItemInvoked(NavigationViewItem nvi)
		{
			this.m_shouldRaiseItemInvokedAfterSelection = true;
			object selectedItem = this.SelectedItem;
			bool flag = this.m_selectionModel != null && nvi.SelectsOnInvoked;
			if (flag)
			{
				IndexPath indexPathForContainer = this.GetIndexPathForContainer(nvi);
				if (this.DoesNavigationViewItemHaveChildren(nvi))
				{
					this.m_shouldIgnoreUIASelectionRaiseAsExpandCollapseWillRaise = true;
				}
				this.UpdateSelectionModelSelection(indexPathForContainer);
			}
			if (selectedItem == this.SelectedItem)
			{
				this.RaiseItemInvokedForNavigationViewItem(nvi);
			}
			this.ToggleIsExpandedNavigationViewItem(nvi);
			this.ClosePaneIfNeccessaryAfterItemIsClicked(nvi);
			if (flag)
			{
				this.CloseFlyoutIfRequired(nvi);
			}
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0000C405 File Offset: 0x0000A605
		private bool IsRootItemsRepeater(DependencyObject element)
		{
			return element != null && (element == this.m_topNavRepeater || element == this.m_leftNavRepeater || element == this.m_topNavRepeaterOverflowView || element == this.m_leftNavFooterMenuRepeater || element == this.m_topNavFooterMenuRepeater);
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0000C43C File Offset: 0x0000A63C
		private bool IsRootGridOfFlyout(DependencyObject element)
		{
			Grid grid = element as Grid;
			return grid != null && grid.Name == "FlyoutRootGrid";
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0000C468 File Offset: 0x0000A668
		private ItemsRepeater GetParentRootItemsRepeaterForContainer(NavigationViewItemBase nvib)
		{
			ItemsRepeater parentItemsRepeaterForContainer = this.GetParentItemsRepeaterForContainer(nvib);
			NavigationViewItemBase navigationViewItemBase = nvib;
			while (!this.IsRootItemsRepeater(parentItemsRepeaterForContainer))
			{
				navigationViewItemBase = this.GetParentNavigationViewItemForContainer(navigationViewItemBase);
				if (navigationViewItemBase == null)
				{
					return null;
				}
				parentItemsRepeaterForContainer = this.GetParentItemsRepeaterForContainer(navigationViewItemBase);
			}
			return parentItemsRepeaterForContainer;
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0000C4A0 File Offset: 0x0000A6A0
		internal ItemsRepeater GetParentItemsRepeaterForContainer(NavigationViewItemBase nvib)
		{
			DependencyObject parent = VisualTreeHelper.GetParent(nvib);
			if (parent != null)
			{
				ItemsRepeater itemsRepeater = parent as ItemsRepeater;
				if (itemsRepeater != null)
				{
					return itemsRepeater;
				}
			}
			return null;
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0000C4C4 File Offset: 0x0000A6C4
		private NavigationViewItem GetParentNavigationViewItemForContainer(NavigationViewItemBase nvib)
		{
			DependencyObject dependencyObject = this.GetParentItemsRepeaterForContainer(nvib);
			if (!this.IsRootItemsRepeater(dependencyObject))
			{
				while (dependencyObject != null)
				{
					dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
					NavigationViewItem navigationViewItem = dependencyObject as NavigationViewItem;
					if (navigationViewItem != null)
					{
						return navigationViewItem;
					}
				}
			}
			return null;
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0000C4FC File Offset: 0x0000A6FC
		private IndexPath GetIndexPathForContainer(NavigationViewItemBase nvib)
		{
			List<int> list = new List<int>();
			DependencyObject dependencyObject = nvib;
			DependencyObject dependencyObject2 = VisualTreeHelper.GetParent(dependencyObject);
			if (dependencyObject2 == null)
			{
				return IndexPath.CreateFromIndices(list);
			}
			while (dependencyObject2 != null && !this.IsRootItemsRepeater(dependencyObject2) && !this.IsRootGridOfFlyout(dependencyObject2))
			{
				ItemsRepeater itemsRepeater = dependencyObject2 as ItemsRepeater;
				if (itemsRepeater != null)
				{
					UIElement uielement = dependencyObject as UIElement;
					if (uielement != null)
					{
						list.Insert(0, itemsRepeater.GetElementIndex(uielement));
					}
				}
				dependencyObject = dependencyObject2;
				dependencyObject2 = VisualTreeHelper.GetParent(dependencyObject2);
			}
			if (this.IsRootGridOfFlyout(dependencyObject2))
			{
				NavigationViewItem lastItemExpandedIntoFlyout = this.m_lastItemExpandedIntoFlyout;
				if (lastItemExpandedIntoFlyout != null)
				{
					dependencyObject = lastItemExpandedIntoFlyout;
					dependencyObject2 = (this.IsTopNavigationView() ? this.m_topNavRepeater : this.m_leftNavRepeater);
				}
			}
			if (dependencyObject2 == this.m_topNavRepeaterOverflowView)
			{
				int elementIndex = this.m_topNavRepeaterOverflowView.GetElementIndex(dependencyObject as UIElement);
				object obj = this.m_topDataProvider.GetOverflowItems()[elementIndex];
				int num = this.m_topDataProvider.IndexOf(obj);
				list.Insert(0, num);
			}
			else if (dependencyObject2 == this.m_topNavRepeater)
			{
				int elementIndex2 = this.m_topNavRepeater.GetElementIndex(dependencyObject as UIElement);
				object obj2 = this.m_topDataProvider.GetPrimaryItems()[elementIndex2];
				int num2 = this.m_topDataProvider.IndexOf(obj2);
				list.Insert(0, num2);
			}
			else
			{
				ItemsRepeater itemsRepeater2 = dependencyObject2 as ItemsRepeater;
				if (itemsRepeater2 != null)
				{
					list.Insert(0, itemsRepeater2.GetElementIndex(dependencyObject as UIElement));
				}
			}
			bool flag = dependencyObject2 == this.m_leftNavFooterMenuRepeater || dependencyObject2 == this.m_topNavFooterMenuRepeater;
			list.Insert(0, flag ? 1 : 0);
			return IndexPath.CreateFromIndices(list);
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0000C678 File Offset: 0x0000A878
		internal void OnRepeaterElementPrepared(ItemsRepeater ir, ItemsRepeaterElementPreparedEventArgs args)
		{
			NavigationView.<>c__DisplayClass86_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.ir = ir;
			NavigationViewItemBase navigationViewItemBase = args.Element as NavigationViewItemBase;
			if (navigationViewItemBase != null)
			{
				NavigationView.<>c__DisplayClass86_1 CS$<>8__locals2;
				CS$<>8__locals2.nvibImpl = navigationViewItemBase;
				CS$<>8__locals2.nvibImpl.SetNavigationViewParent(this);
				CS$<>8__locals2.nvibImpl.IsTopLevelItem = this.IsTopLevelItem(navigationViewItemBase);
				CS$<>8__locals2.position = this.<OnRepeaterElementPrepared>g__init|86_0(ref CS$<>8__locals1);
				CS$<>8__locals2.nvibImpl.Position = CS$<>8__locals2.position;
				NavigationViewItem parentNavigationViewItemForContainer = this.GetParentNavigationViewItemForContainer(navigationViewItemBase);
				if (parentNavigationViewItemForContainer != null)
				{
					NavigationViewItem navigationViewItem = parentNavigationViewItemForContainer;
					int depth = navigationViewItem.ShouldRepeaterShowInFlyout() ? 0 : (navigationViewItem.Depth + 1);
					CS$<>8__locals2.nvibImpl.Depth = depth;
				}
				else
				{
					CS$<>8__locals2.nvibImpl.Depth = 0;
				}
				this.ApplyCustomMenuItemContainerStyling(navigationViewItemBase, CS$<>8__locals1.ir, args.Index);
				NavigationViewItem navigationViewItem2 = args.Element as NavigationViewItem;
				if (navigationViewItem2 != null)
				{
					int depth2 = NavigationView.<OnRepeaterElementPrepared>g__init|86_1(ref CS$<>8__locals2);
					navigationViewItem2.PropagateDepthToChildren(depth2);
					InputHelper.AddTappedHandler(navigationViewItem2, new TappedEventHandler(this.OnNavigationViewItemTapped));
					navigationViewItem2.KeyDown += new KeyEventHandler(this.OnNavigationViewItemKeyDown);
					navigationViewItem2.GotFocus += new RoutedEventHandler(this.OnNavigationViewItemOnGotFocus);
					navigationViewItem2.IsSelectedChanged += this.OnNavigationViewItemIsSelectedPropertyChanged;
					navigationViewItem2.IsExpandedChanged += this.OnNavigationViewItemExpandedPropertyChanged;
				}
			}
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0000C7C4 File Offset: 0x0000A9C4
		private void ApplyCustomMenuItemContainerStyling(NavigationViewItemBase nvib, ItemsRepeater ir, int index)
		{
			Style menuItemContainerStyle = this.MenuItemContainerStyle;
			if (menuItemContainerStyle != null)
			{
				nvib.Style = menuItemContainerStyle;
				return;
			}
			StyleSelector menuItemContainerStyleSelector = this.MenuItemContainerStyleSelector;
			if (menuItemContainerStyleSelector != null)
			{
				ItemsSourceView itemsSourceView = ir.ItemsSourceView;
				if (itemsSourceView != null)
				{
					object at = itemsSourceView.GetAt(index);
					if (at != null)
					{
						Style style = menuItemContainerStyleSelector.SelectStyle(at, nvib);
						if (style != null)
						{
							nvib.Style = style;
						}
					}
				}
			}
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0000C818 File Offset: 0x0000AA18
		internal void OnRepeaterElementClearing(ItemsRepeater ir, ItemsRepeaterElementClearingEventArgs args)
		{
			NavigationViewItemBase navigationViewItemBase = args.Element as NavigationViewItemBase;
			if (navigationViewItemBase != null)
			{
				NavigationViewItemBase navigationViewItemBase2 = navigationViewItemBase;
				navigationViewItemBase2.Depth = 0;
				navigationViewItemBase2.IsTopLevelItem = false;
				NavigationViewItem navigationViewItem = navigationViewItemBase as NavigationViewItem;
				if (navigationViewItem != null)
				{
					InputHelper.RemoveTappedHandler(navigationViewItem, new TappedEventHandler(this.OnNavigationViewItemTapped));
					navigationViewItem.KeyDown -= new KeyEventHandler(this.OnNavigationViewItemKeyDown);
					navigationViewItem.GotFocus -= new RoutedEventHandler(this.OnNavigationViewItemOnGotFocus);
					navigationViewItem.IsSelectedChanged -= this.OnNavigationViewItemIsSelectedPropertyChanged;
					navigationViewItem.IsExpandedChanged -= this.OnNavigationViewItemExpandedPropertyChanged;
				}
			}
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0000C8A6 File Offset: 0x0000AAA6
		internal NavigationViewItemsFactory GetNavigationViewItemsFactory()
		{
			return this.m_navigationViewItemsFactory;
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0000C8B0 File Offset: 0x0000AAB0
		private void CreateAndHookEventsToSettings()
		{
			if (this.m_settingsItem == null)
			{
				return;
			}
			NavigationViewItem settingsItem = this.m_settingsItem;
			SymbolIcon icon = new SymbolIcon(57621);
			settingsItem.Icon = icon;
			string localizedStringResource = NavigationView.ResourceAccessor.GetLocalizedStringResource("SettingsButtonName");
			AutomationProperties.SetName(settingsItem, localizedStringResource);
			settingsItem.Tag = localizedStringResource;
			this.UpdateSettingsItemToolTip();
			if (!this.IsTopNavigationView())
			{
				settingsItem.Content = localizedStringResource;
			}
			else
			{
				settingsItem.Content = null;
			}
			base.SetValue(NavigationView.SettingsItemPropertyKey, settingsItem);
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000C928 File Offset: 0x0000AB28
		protected override Size MeasureOverride(Size availableSize)
		{
			if (this.IsTopNavigationView() && this.IsTopPrimaryListVisible())
			{
				if (double.IsInfinity(availableSize.Width))
				{
					this.m_topDataProvider.MoveAllItemsToPrimaryList();
				}
				else
				{
					this.HandleTopNavigationMeasureOverride(availableSize);
				}
			}
			base.LayoutUpdated -= new EventHandler(this.OnLayoutUpdated);
			base.LayoutUpdated += new EventHandler(this.OnLayoutUpdated);
			this.m_layoutUpdatedToken = true;
			return base.MeasureOverride(availableSize);
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000C99C File Offset: 0x0000AB9C
		private void OnLayoutUpdated(object sender, object e)
		{
			base.LayoutUpdated -= new EventHandler(this.OnLayoutUpdated);
			this.m_layoutUpdatedToken = false;
			object lastSelectedItemInTopNav = this.m_lastSelectedItemPendingAnimationInTopNav;
			if (lastSelectedItemInTopNav != null)
			{
				this.m_lastSelectedItemPendingAnimationInTopNav = null;
				base.Dispatcher.BeginInvoke(delegate()
				{
					this.AnimateSelectionChanged(lastSelectedItemInTopNav);
				}, 10, Array.Empty<object>());
			}
			if (this.m_OrientationChangedPendingAnimation)
			{
				this.m_OrientationChangedPendingAnimation = false;
				this.AnimateSelectionChanged(this.SelectedItem);
			}
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000CA24 File Offset: 0x0000AC24
		private void OnSizeChanged(object sender, SizeChangedEventArgs args)
		{
			double width = args.NewSize.Width;
			this.UpdateAdaptiveLayout(width, false);
			this.UpdateTitleBarPadding();
			this.UpdateBackAndCloseButtonsVisibility();
			this.UpdatePaneLayout();
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000CA5A File Offset: 0x0000AC5A
		private void OnItemsContainerSizeChanged(object sender, SizeChangedEventArgs e)
		{
			this.UpdatePaneLayout();
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000CA64 File Offset: 0x0000AC64
		private void UpdateAdaptiveLayout(double width, bool forceSetDisplayMode = false)
		{
			if (this.IsTopNavigationView())
			{
				return;
			}
			if (this.m_rootSplitView == null)
			{
				return;
			}
			this.m_initialListSizeStateSet = false;
			NavigationViewDisplayMode navigationViewDisplayMode = NavigationViewDisplayMode.Compact;
			NavigationViewPaneDisplayMode paneDisplayMode = this.PaneDisplayMode;
			if (paneDisplayMode == NavigationViewPaneDisplayMode.Auto)
			{
				if (width >= this.ExpandedModeThresholdWidth)
				{
					navigationViewDisplayMode = NavigationViewDisplayMode.Expanded;
				}
				else if (width < this.CompactModeThresholdWidth)
				{
					navigationViewDisplayMode = NavigationViewDisplayMode.Minimal;
				}
			}
			else if (paneDisplayMode == NavigationViewPaneDisplayMode.Left)
			{
				navigationViewDisplayMode = NavigationViewDisplayMode.Expanded;
			}
			else if (paneDisplayMode == NavigationViewPaneDisplayMode.LeftCompact)
			{
				navigationViewDisplayMode = NavigationViewDisplayMode.Compact;
			}
			else if (paneDisplayMode == NavigationViewPaneDisplayMode.LeftMinimal)
			{
				navigationViewDisplayMode = NavigationViewDisplayMode.Minimal;
			}
			else
			{
				Environment.FailFast(null);
			}
			if (!forceSetDisplayMode && this.m_InitialNonForcedModeUpdate)
			{
				if (navigationViewDisplayMode == NavigationViewDisplayMode.Minimal || navigationViewDisplayMode == NavigationViewDisplayMode.Compact)
				{
					this.ClosePane();
				}
				this.m_InitialNonForcedModeUpdate = false;
			}
			int displayMode = (int)this.DisplayMode;
			this.SetDisplayMode(navigationViewDisplayMode, forceSetDisplayMode);
			if (navigationViewDisplayMode == NavigationViewDisplayMode.Expanded && this.IsPaneVisible && !this.m_wasForceClosed)
			{
				this.OpenPane();
			}
			if (displayMode == 2 && navigationViewDisplayMode == NavigationViewDisplayMode.Compact)
			{
				this.m_initialListSizeStateSet = false;
				this.ClosePane();
			}
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0000CB2C File Offset: 0x0000AD2C
		private void UpdatePaneLayout()
		{
			if (!this.IsTopNavigationView())
			{
				NavigationView.<>c__DisplayClass96_0 CS$<>8__locals1;
				CS$<>8__locals1.totalAvailableHeight = this.<UpdatePaneLayout>g__init|96_0();
				if (CS$<>8__locals1.totalAvailableHeight > 0.0)
				{
					NavigationView.<>c__DisplayClass96_1 CS$<>8__locals2;
					CS$<>8__locals2.totalAvailableHeightHalf = CS$<>8__locals1.totalAvailableHeight / 2.0;
					double maxHeight = this.<UpdatePaneLayout>g__init|96_1(ref CS$<>8__locals1, ref CS$<>8__locals2);
					FrameworkElement menuItemsScrollViewer = this.m_menuItemsScrollViewer;
					if (menuItemsScrollViewer != null)
					{
						menuItemsScrollViewer.MaxHeight = maxHeight;
					}
				}
			}
		}

		// Token: 0x0600034A RID: 842 RVA: 0x0000CB92 File Offset: 0x0000AD92
		private void OnPaneToggleButtonClick(object sender, RoutedEventArgs args)
		{
			if (this.IsPaneOpen)
			{
				this.m_wasForceClosed = true;
				this.ClosePane();
				return;
			}
			this.m_wasForceClosed = false;
			this.OpenPane();
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0000CBB8 File Offset: 0x0000ADB8
		private void OnPaneSearchButtonClick(object sender, RoutedEventArgs args)
		{
			this.m_wasForceClosed = false;
			this.OpenPane();
			AutoSuggestBox autoSuggestBox = this.AutoSuggestBox;
			if (autoSuggestBox != null)
			{
				base.Dispatcher.BeginInvoke(delegate()
				{
					autoSuggestBox.Focus();
				}, 6, Array.Empty<object>());
			}
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000CC0A File Offset: 0x0000AE0A
		private void OnPaneTitleHolderSizeChanged(object sender, SizeChangedEventArgs args)
		{
			this.UpdateBackAndCloseButtonsVisibility();
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000CC14 File Offset: 0x0000AE14
		private void OpenPane()
		{
			try
			{
				this.m_isOpenPaneForInteraction = true;
				this.IsPaneOpen = true;
			}
			finally
			{
				this.m_isOpenPaneForInteraction = false;
			}
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000CC4C File Offset: 0x0000AE4C
		private void ClosePane()
		{
			this.CollapseMenuItemsInRepeater(this.m_leftNavRepeater);
			try
			{
				this.m_isOpenPaneForInteraction = true;
				this.IsPaneOpen = false;
			}
			finally
			{
				this.m_isOpenPaneForInteraction = false;
			}
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000CC90 File Offset: 0x0000AE90
		private bool AttemptClosePaneLightly()
		{
			NavigationViewPaneClosingEventArgs navigationViewPaneClosingEventArgs = new NavigationViewPaneClosingEventArgs();
			TypedEventHandler<NavigationView, NavigationViewPaneClosingEventArgs> paneClosing = this.PaneClosing;
			if (paneClosing != null)
			{
				paneClosing.Invoke(this, navigationViewPaneClosingEventArgs);
			}
			if (!navigationViewPaneClosingEventArgs.Cancel || this.m_wasForceClosed)
			{
				this.m_blockNextClosingEvent = true;
				this.ClosePane();
				return true;
			}
			return false;
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0000CCD6 File Offset: 0x0000AED6
		private void OnSplitViewClosedCompactChanged(DependencyObject sender, DependencyProperty args)
		{
			if (args == SplitView.IsPaneOpenProperty || args == SplitView.DisplayModeProperty)
			{
				this.UpdateIsClosedCompact();
			}
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0000CCEE File Offset: 0x0000AEEE
		private void OnSplitViewPaneClosed(DependencyObject sender, object obj)
		{
			TypedEventHandler<NavigationView, object> paneClosed = this.PaneClosed;
			if (paneClosed == null)
			{
				return;
			}
			paneClosed.Invoke(this, null);
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000CD04 File Offset: 0x0000AF04
		private void OnSplitViewPaneClosing(DependencyObject sender, SplitViewPaneClosingEventArgs args)
		{
			bool flag = false;
			if (this.PaneClosing != null)
			{
				if (!this.m_blockNextClosingEvent)
				{
					NavigationViewPaneClosingEventArgs navigationViewPaneClosingEventArgs = new NavigationViewPaneClosingEventArgs();
					navigationViewPaneClosingEventArgs.SplitViewClosingArgs(args);
					this.PaneClosing.Invoke(this, navigationViewPaneClosingEventArgs);
					flag = navigationViewPaneClosingEventArgs.Cancel;
				}
				else
				{
					this.m_blockNextClosingEvent = false;
				}
			}
			if (!flag)
			{
				SplitView rootSplitView = this.m_rootSplitView;
				if (rootSplitView != null && this.m_leftNavRepeater != null && (rootSplitView.DisplayMode == SplitViewDisplayMode.CompactOverlay || rootSplitView.DisplayMode == SplitViewDisplayMode.CompactInline))
				{
					VisualStateManager.GoToState(this, "ListSizeCompact", true);
					this.UpdatePaneToggleSize();
				}
			}
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000CD87 File Offset: 0x0000AF87
		private void OnSplitViewPaneOpened(DependencyObject sender, object obj)
		{
			TypedEventHandler<NavigationView, object> paneOpened = this.PaneOpened;
			if (paneOpened == null)
			{
				return;
			}
			paneOpened.Invoke(this, null);
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000CD9B File Offset: 0x0000AF9B
		private void OnSplitViewPaneOpening(DependencyObject sender, object obj)
		{
			if (this.m_leftNavRepeater != null)
			{
				VisualStateManager.GoToState(this, "ListSizeFull", true);
			}
			TypedEventHandler<NavigationView, object> paneOpening = this.PaneOpening;
			if (paneOpening == null)
			{
				return;
			}
			paneOpening.Invoke(this, null);
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0000CDC4 File Offset: 0x0000AFC4
		private void UpdateIsClosedCompact()
		{
			SplitView rootSplitView = this.m_rootSplitView;
			if (rootSplitView != null)
			{
				SplitViewDisplayMode displayMode = rootSplitView.DisplayMode;
				this.m_isClosedCompact = (!rootSplitView.IsPaneOpen && (displayMode == SplitViewDisplayMode.CompactOverlay || displayMode == SplitViewDisplayMode.CompactInline));
				VisualStateManager.GoToState(this, this.m_isClosedCompact ? "ClosedCompact" : "NotClosedCompact", true);
				if (!this.m_initialListSizeStateSet)
				{
					this.m_initialListSizeStateSet = true;
					VisualStateManager.GoToState(this, this.m_isClosedCompact ? "ListSizeCompact" : "ListSizeFull", true);
				}
				this.UpdateTitleBarPadding();
				this.UpdateBackAndCloseButtonsVisibility();
				this.UpdatePaneTitleMargins();
				this.UpdatePaneToggleSize();
			}
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000CE60 File Offset: 0x0000B060
		private void UpdatePaneButtonsWidths()
		{
			double num = this.<UpdatePaneButtonsWidths>g__init|109_0();
			Button backButton = this.m_backButton;
			if (backButton != null)
			{
				backButton.Width = num;
			}
			Button paneToggleButton = this.m_paneToggleButton;
			if (paneToggleButton != null)
			{
				paneToggleButton.MinWidth = num;
				ColumnDefinition templateChild = ControlExtensions.GetTemplateChild<ColumnDefinition>(paneToggleButton, "PaneToggleButtonIconWidthColumn");
				if (templateChild != null)
				{
					templateChild.Width = new GridLength(num);
				}
			}
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000CEB4 File Offset: 0x0000B0B4
		private void OnBackButtonClicked(object sender, RoutedEventArgs args)
		{
			NavigationViewBackRequestedEventArgs navigationViewBackRequestedEventArgs = new NavigationViewBackRequestedEventArgs();
			TypedEventHandler<NavigationView, NavigationViewBackRequestedEventArgs> backRequested = this.BackRequested;
			if (backRequested == null)
			{
				return;
			}
			backRequested.Invoke(this, navigationViewBackRequestedEventArgs);
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000CEDC File Offset: 0x0000B0DC
		private bool IsOverlay()
		{
			SplitView rootSplitView = this.m_rootSplitView;
			return rootSplitView != null && rootSplitView.DisplayMode == SplitViewDisplayMode.Overlay;
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000CF00 File Offset: 0x0000B100
		private bool IsLightDismissible()
		{
			SplitView rootSplitView = this.m_rootSplitView;
			return rootSplitView != null && rootSplitView.DisplayMode != SplitViewDisplayMode.Inline && rootSplitView.DisplayMode != SplitViewDisplayMode.CompactInline;
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000CF30 File Offset: 0x0000B130
		private bool ShouldShowBackButton()
		{
			return this.m_backButton != null && !this.ShouldPreserveNavigationViewRS3Behavior() && (this.DisplayMode != NavigationViewDisplayMode.Minimal || !this.IsPaneOpen) && this.ShouldShowBackOrCloseButton();
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000CF5C File Offset: 0x0000B15C
		private bool ShouldShowCloseButton()
		{
			if (this.m_backButton == null || this.ShouldPreserveNavigationViewRS3Behavior() || this.m_closeButton == null)
			{
				return false;
			}
			if (!this.IsPaneOpen)
			{
				return false;
			}
			NavigationViewPaneDisplayMode paneDisplayMode = this.PaneDisplayMode;
			return (paneDisplayMode == NavigationViewPaneDisplayMode.LeftMinimal || (paneDisplayMode == NavigationViewPaneDisplayMode.Auto && this.DisplayMode == NavigationViewDisplayMode.Minimal)) && this.ShouldShowBackOrCloseButton();
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000CFAC File Offset: 0x0000B1AC
		private bool ShouldShowBackOrCloseButton()
		{
			NavigationViewBackButtonVisible isBackButtonVisible = this.IsBackButtonVisible;
			return isBackButtonVisible == NavigationViewBackButtonVisible.Visible || (isBackButtonVisible == NavigationViewBackButtonVisible.Auto && !SharedHelpers.IsOnXbox());
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000CFD4 File Offset: 0x0000B1D4
		private void SetPaneToggleButtonAutomationName()
		{
			string localizedStringResource;
			if (this.IsPaneOpen)
			{
				localizedStringResource = NavigationView.ResourceAccessor.GetLocalizedStringResource("NavigationButtonOpenName");
			}
			else
			{
				localizedStringResource = NavigationView.ResourceAccessor.GetLocalizedStringResource("NavigationButtonClosedName");
			}
			Button paneToggleButton = this.m_paneToggleButton;
			if (paneToggleButton != null)
			{
				AutomationProperties.SetName(paneToggleButton, localizedStringResource);
				ToolTipService.SetToolTip(paneToggleButton, new ToolTip
				{
					Content = localizedStringResource
				});
			}
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000D030 File Offset: 0x0000B230
		private void UpdateSettingsItemToolTip()
		{
			NavigationViewItem settingsItem = this.m_settingsItem;
			if (settingsItem != null)
			{
				if (!this.IsTopNavigationView() && this.IsPaneOpen)
				{
					ToolTipService.SetToolTip(settingsItem, null);
					return;
				}
				string localizedStringResource = NavigationView.ResourceAccessor.GetLocalizedStringResource("SettingsButtonName");
				ToolTipService.SetToolTip(settingsItem, new ToolTip
				{
					Content = localizedStringResource
				});
			}
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0000D084 File Offset: 0x0000B284
		private void UpdatePaneTitleFrameworkElementParents()
		{
			FrameworkElement paneTitleHolderFrameworkElement = this.m_paneTitleHolderFrameworkElement;
			if (paneTitleHolderFrameworkElement != null)
			{
				bool isPaneToggleButtonVisible = this.IsPaneToggleButtonVisible;
				bool flag = this.IsTopNavigationView();
				paneTitleHolderFrameworkElement.Visibility = ((isPaneToggleButtonVisible || flag || this.PaneTitle.Length == 0 || (this.PaneDisplayMode == NavigationViewPaneDisplayMode.LeftMinimal && !this.IsPaneOpen)) ? 2 : 0);
				FrameworkElement paneTitleFrameworkElement = this.m_paneTitleFrameworkElement;
				if (paneTitleFrameworkElement != null)
				{
					Action action = this.SetPaneTitleFrameworkElementParent(this.m_paneToggleButton, paneTitleFrameworkElement, flag || !isPaneToggleButtonVisible);
					Action action2 = this.SetPaneTitleFrameworkElementParent(this.m_paneTitlePresenter, paneTitleFrameworkElement, flag || isPaneToggleButtonVisible);
					Action action3 = this.SetPaneTitleFrameworkElementParent(this.m_paneTitleOnTopPane, paneTitleFrameworkElement, !flag || isPaneToggleButtonVisible);
					Action action4 = action;
					Action action5;
					if ((action5 = action4) == null)
					{
						action5 = (action2 ?? action3);
					}
					Action action6 = action5;
					if (action6 == null)
					{
						return;
					}
					action6.Invoke();
				}
			}
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000D13C File Offset: 0x0000B33C
		private Action SetPaneTitleFrameworkElementParent(ContentControl parent, FrameworkElement paneTitle, bool shouldNotContainPaneTitle)
		{
			if (parent != null && parent.Content == paneTitle == shouldNotContainPaneTitle)
			{
				if (!shouldNotContainPaneTitle)
				{
					return delegate()
					{
						parent.Content = paneTitle;
					};
				}
				parent.Content = null;
			}
			return null;
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000D19A File Offset: 0x0000B39A
		private void AnimateSelectionChangedToItem(object selectedItem)
		{
			if (selectedItem != null && !this.IsSelectionSuppressed(selectedItem))
			{
				this.AnimateSelectionChanged(selectedItem);
			}
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000D1B0 File Offset: 0x0000B3B0
		private void AnimateSelectionChanged(object nextItem)
		{
			if (this.m_lastSelectedItemPendingAnimationInTopNav != null)
			{
				return;
			}
			UIElement activeIndicator = this.m_activeIndicator;
			UIElement uielement = this.FindSelectionIndicator(nextItem);
			bool flag = false;
			if (this.m_prevIndicator != null || this.m_nextIndicator != null)
			{
				if (uielement != null && this.m_nextIndicator == uielement)
				{
					if (activeIndicator != null && activeIndicator != this.m_prevIndicator)
					{
						this.ResetElementAnimationProperties(activeIndicator, 0.0);
					}
					flag = true;
				}
				else
				{
					this.OnAnimationComplete(null, null);
				}
			}
			if (!flag)
			{
				UIElement paneContentGrid = this.m_paneContentGrid;
				if (activeIndicator != uielement && paneContentGrid != null && activeIndicator != null && uielement != null && SharedHelpers.IsAnimationsEnabled)
				{
					this.ResetElementAnimationProperties(activeIndicator, 1.0);
					this.ResetElementAnimationProperties(uielement, 1.0);
					Point point;
					point..ctor(0.0, 0.0);
					Point point2 = Extensions.SafeTransformToVisual(activeIndicator, paneContentGrid).Transform(point);
					Point point3 = Extensions.SafeTransformToVisual(uielement, paneContentGrid).Transform(point);
					Size renderSize = activeIndicator.RenderSize;
					Size renderSize2 = uielement.RenderSize;
					double num;
					double num2;
					bool flag2;
					if (this.IsTopNavigationView())
					{
						num = point2.X;
						num2 = point3.X;
						flag2 = (point2.Y == point3.Y);
					}
					else
					{
						num = point2.Y;
						num2 = point3.Y;
						flag2 = (point2.X == point3.X);
					}
					Storyboard storyboard = new Storyboard
					{
						FillBehavior = 1
					};
					if (!flag2)
					{
						bool flag3 = point2.Y < point3.Y;
						if (activeIndicator.RenderSize.Height > activeIndicator.RenderSize.Width)
						{
							this.PlayIndicatorNonSameLevelAnimations(activeIndicator, true, !flag3, storyboard.Children);
						}
						else
						{
							this.PlayIndicatorNonSameLevelTopPrimaryAnimation(activeIndicator, true, storyboard.Children);
						}
						if (uielement.RenderSize.Height > uielement.RenderSize.Width)
						{
							this.PlayIndicatorNonSameLevelAnimations(uielement, false, flag3, storyboard.Children);
						}
						else
						{
							this.PlayIndicatorNonSameLevelTopPrimaryAnimation(uielement, false, storyboard.Children);
						}
					}
					else
					{
						double to = num2 - num;
						double from = num - num2;
						this.PlayIndicatorAnimations(activeIndicator, 0.0, to, renderSize, renderSize2, true, storyboard.Children);
						this.PlayIndicatorAnimations(uielement, from, 0.0, renderSize, renderSize2, false, storyboard.Children);
					}
					this.m_prevIndicator = activeIndicator;
					this.m_nextIndicator = uielement;
					storyboard.Completed += new EventHandler(this.OnAnimationComplete);
					storyboard.Begin(this, true);
					storyboard.Pause(this);
					storyboard.SeekAlignedToLastTick(this, TimeSpan.Zero, 0);
					base.Dispatcher.BeginInvoke(delegate()
					{
						storyboard.Resume(this);
					}, 6, Array.Empty<object>());
				}
				else
				{
					this.ResetElementAnimationProperties(activeIndicator, 0.0);
					this.ResetElementAnimationProperties(uielement, 1.0);
				}
				this.m_activeIndicator = uielement;
			}
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000D4D0 File Offset: 0x0000B6D0
		private void PlayIndicatorNonSameLevelAnimations(UIElement indicator, bool isOutgoing, bool fromTop, TimelineCollection animations)
		{
			double num = isOutgoing ? 1.0 : 0.0;
			double num2 = isOutgoing ? 0.0 : 1.0;
			DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames = new DoubleAnimationUsingKeyFrames();
			doubleAnimationUsingKeyFrames.KeyFrames.Add(new DiscreteDoubleKeyFrame(num, KeyTime.FromPercent(0.0)));
			doubleAnimationUsingKeyFrames.KeyFrames.Add(new SplineDoubleKeyFrame(num2, KeyTime.FromPercent(1.0), new KeySpline(new Point(0.8, 0.0), NavigationView.c_frame2point2)));
			doubleAnimationUsingKeyFrames.Duration = TimeSpan.FromMilliseconds(600.0);
			DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames2 = doubleAnimationUsingKeyFrames;
			animations.Add(doubleAnimationUsingKeyFrames2);
			Size renderSize = indicator.RenderSize;
			double num3 = this.IsTopNavigationView() ? renderSize.Width : renderSize.Height;
			double y = fromTop ? 0.0 : num3;
			Point point = default(Point);
			point.Y = y;
			Storyboard.SetTarget(doubleAnimationUsingKeyFrames2, indicator);
			Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames2, NavigationView.s_scaleYPath);
			this.PrepareIndicatorForAnimation(indicator, new Point?(point));
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000D5FC File Offset: 0x0000B7FC
		private void PlayIndicatorNonSameLevelTopPrimaryAnimation(UIElement indicator, bool isOutgoing, TimelineCollection animations)
		{
			double num = isOutgoing ? 1.0 : 0.0;
			double num2 = isOutgoing ? 0.0 : 1.0;
			DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames = new DoubleAnimationUsingKeyFrames();
			doubleAnimationUsingKeyFrames.KeyFrames.Add(new DiscreteDoubleKeyFrame(num, KeyTime.FromPercent(0.0)));
			doubleAnimationUsingKeyFrames.KeyFrames.Add(new SplineDoubleKeyFrame(num2, KeyTime.FromPercent(1.0), new KeySpline(new Point(0.8, 0.0), NavigationView.c_frame2point2)));
			doubleAnimationUsingKeyFrames.Duration = TimeSpan.FromMilliseconds(600.0);
			DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames2 = doubleAnimationUsingKeyFrames;
			animations.Add(doubleAnimationUsingKeyFrames2);
			double y = indicator.RenderSize.Width / 2.0;
			Point point = default(Point);
			point.Y = y;
			Storyboard.SetTarget(doubleAnimationUsingKeyFrames2, indicator);
			Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames2, NavigationView.s_scaleXPath);
			this.PrepareIndicatorForAnimation(indicator, new Point?(point));
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000D70C File Offset: 0x0000B90C
		private void PlayIndicatorAnimations(UIElement indicator, double from, double to, Size beginSize, Size endSize, bool isOutgoing, TimelineCollection animations)
		{
			Size renderSize = indicator.RenderSize;
			double num = this.IsTopNavigationView() ? renderSize.Width : renderSize.Height;
			double num2 = 1.0;
			double num3 = 1.0;
			if (this.IsTopNavigationView() && Math.Abs(renderSize.Width) > 0.0010000000474974513)
			{
				num2 = beginSize.Width / renderSize.Width;
				num3 = endSize.Width / renderSize.Width;
			}
			DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames = new DoubleAnimationUsingKeyFrames();
			doubleAnimationUsingKeyFrames.KeyFrames.Add(new DiscreteDoubleKeyFrame((from < to) ? from : (from + num * (num2 - 1.0)), KeyTime.FromPercent(0.0)));
			doubleAnimationUsingKeyFrames.KeyFrames.Add(new DiscreteDoubleKeyFrame((from < to) ? (to + num * (num3 - 1.0)) : to, KeyTime.FromPercent(0.333)));
			doubleAnimationUsingKeyFrames.Duration = TimeSpan.FromMilliseconds(600.0);
			DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames2 = doubleAnimationUsingKeyFrames;
			Storyboard.SetTarget(doubleAnimationUsingKeyFrames2, indicator);
			animations.Add(doubleAnimationUsingKeyFrames2);
			DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames3 = new DoubleAnimationUsingKeyFrames();
			doubleAnimationUsingKeyFrames3.KeyFrames.Add(new DiscreteDoubleKeyFrame(num2, KeyTime.FromPercent(0.0)));
			doubleAnimationUsingKeyFrames3.KeyFrames.Add(new SplineDoubleKeyFrame(Math.Abs(to - from) / num + ((from < to) ? num3 : num2), KeyTime.FromPercent(0.333), new KeySpline(NavigationView.c_frame1point1, NavigationView.c_frame1point2)));
			doubleAnimationUsingKeyFrames3.KeyFrames.Add(new SplineDoubleKeyFrame(num3, KeyTime.FromPercent(1.0), new KeySpline(NavigationView.c_frame2point1, NavigationView.c_frame2point2)));
			doubleAnimationUsingKeyFrames3.Duration = TimeSpan.FromMilliseconds(600.0);
			DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames4 = doubleAnimationUsingKeyFrames3;
			Storyboard.SetTarget(doubleAnimationUsingKeyFrames4, indicator);
			animations.Add(doubleAnimationUsingKeyFrames4);
			DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames5 = new DoubleAnimationUsingKeyFrames();
			doubleAnimationUsingKeyFrames5.KeyFrames.Add(new DiscreteDoubleKeyFrame((from < to) ? 0.0 : num, KeyTime.FromPercent(0.0)));
			doubleAnimationUsingKeyFrames5.KeyFrames.Add(new DiscreteDoubleKeyFrame((from < to) ? num : 0.0, KeyTime.FromPercent(1.0)));
			doubleAnimationUsingKeyFrames5.Duration = TimeSpan.FromMilliseconds(200.0);
			DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames6 = doubleAnimationUsingKeyFrames5;
			Storyboard.SetTarget(doubleAnimationUsingKeyFrames6, indicator);
			animations.Add(doubleAnimationUsingKeyFrames6);
			if (isOutgoing)
			{
				DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames7 = new DoubleAnimationUsingKeyFrames();
				doubleAnimationUsingKeyFrames7.KeyFrames.Add(new DiscreteDoubleKeyFrame(1.0, KeyTime.FromPercent(0.0)));
				doubleAnimationUsingKeyFrames7.KeyFrames.Add(new DiscreteDoubleKeyFrame(1.0, KeyTime.FromPercent(0.333)));
				doubleAnimationUsingKeyFrames7.KeyFrames.Add(new SplineDoubleKeyFrame(0.0, KeyTime.FromPercent(1.0), new KeySpline(NavigationView.c_frame2point1, NavigationView.c_frame2point2)));
				doubleAnimationUsingKeyFrames7.Duration = TimeSpan.FromMilliseconds(600.0);
				DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames8 = doubleAnimationUsingKeyFrames7;
				Storyboard.SetTarget(doubleAnimationUsingKeyFrames8, indicator);
				Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames8, NavigationView.s_opacityPath);
				animations.Add(doubleAnimationUsingKeyFrames8);
			}
			if (this.IsTopNavigationView())
			{
				Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames2, NavigationView.s_translateXPath);
				Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames4, NavigationView.s_scaleXPath);
				Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames6, NavigationView.s_centerXPath);
			}
			else
			{
				Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames2, NavigationView.s_translateYPath);
				Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames4, NavigationView.s_scaleYPath);
				Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames6, NavigationView.s_centerYPath);
			}
			this.PrepareIndicatorForAnimation(indicator, default(Point?));
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000DAA8 File Offset: 0x0000BCA8
		private void PrepareIndicatorForAnimation(UIElement indicator, Point? centerPoint = null)
		{
			TransformGroup transformGroup = indicator.RenderTransform as TransformGroup;
			if (transformGroup == null || transformGroup.Children.Count != 2 || !(transformGroup.Children[0] is ScaleTransform) || !(transformGroup.Children[1] is TranslateTransform))
			{
				TransformGroup transformGroup2 = new TransformGroup();
				transformGroup2.Children.Add(new ScaleTransform());
				transformGroup2.Children.Add(new TranslateTransform());
				indicator.RenderTransform = transformGroup2;
			}
			if (centerPoint != null)
			{
				ScaleTransform scaleTransform = (ScaleTransform)((TransformGroup)indicator.RenderTransform).Children[0];
				scaleTransform.CenterX = centerPoint.Value.X;
				scaleTransform.CenterY = centerPoint.Value.Y;
			}
			if (indicator.CacheMode == null)
			{
				indicator.CacheMode = this.m_bitmapCache;
			}
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0000DB84 File Offset: 0x0000BD84
		private void OnAnimationComplete(object sender, EventArgs args)
		{
			UIElement element = this.m_prevIndicator;
			this.ResetElementAnimationProperties(element, 0.0);
			this.m_prevIndicator = null;
			element = this.m_nextIndicator;
			this.ResetElementAnimationProperties(element, 1.0);
			this.m_nextIndicator = null;
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0000DBD0 File Offset: 0x0000BDD0
		private void ResetElementAnimationProperties(UIElement element, double desiredOpacity)
		{
			if (element != null)
			{
				element.Opacity = desiredOpacity;
				TransformGroup transformGroup = element.RenderTransform as TransformGroup;
				if (transformGroup != null && transformGroup.Children.Count == 2)
				{
					ScaleTransform scaleTransform = transformGroup.Children[0] as ScaleTransform;
					if (scaleTransform != null)
					{
						TranslateTransform translateTransform = transformGroup.Children[1] as TranslateTransform;
						if (translateTransform != null)
						{
							translateTransform.BeginAnimation(TranslateTransform.XProperty, null);
							translateTransform.BeginAnimation(TranslateTransform.YProperty, null);
							scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, null);
							scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, null);
							scaleTransform.ClearValue(ScaleTransform.CenterXProperty);
							scaleTransform.ClearValue(ScaleTransform.CenterYProperty);
							goto IL_AA;
						}
					}
				}
				element.ClearValue(UIElement.RenderTransformProperty);
				IL_AA:
				element.BeginAnimation(UIElement.OpacityProperty, null);
			}
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000DC93 File Offset: 0x0000BE93
		private NavigationViewItemBase NavigationViewItemBaseOrSettingsContentFromData(object data)
		{
			return this.GetContainerForData<NavigationViewItemBase>(data);
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000DC9C File Offset: 0x0000BE9C
		private NavigationViewItem NavigationViewItemOrSettingsContentFromData(object data)
		{
			return this.GetContainerForData<NavigationViewItem>(data);
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000DCA8 File Offset: 0x0000BEA8
		private bool IsSelectionSuppressed(object item)
		{
			if (item != null)
			{
				NavigationViewItem navigationViewItem = this.NavigationViewItemOrSettingsContentFromData(item);
				if (navigationViewItem != null)
				{
					return !navigationViewItem.SelectsOnInvoked;
				}
			}
			return false;
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000DCCE File Offset: 0x0000BECE
		private bool ShouldPreserveNavigationViewRS4Behavior()
		{
			return this.m_topNavGrid == null;
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0000DCD9 File Offset: 0x0000BED9
		private bool ShouldPreserveNavigationViewRS3Behavior()
		{
			return this.m_backButton == null;
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000DCE4 File Offset: 0x0000BEE4
		private UIElement FindSelectionIndicator(object item)
		{
			if (item != null)
			{
				NavigationViewItem navigationViewItem = this.NavigationViewItemOrSettingsContentFromData(item);
				if (navigationViewItem != null)
				{
					UIElement selectionIndicator = navigationViewItem.GetSelectionIndicator();
					if (selectionIndicator != null)
					{
						return selectionIndicator;
					}
					navigationViewItem.UpdateLayout();
					return navigationViewItem.GetSelectionIndicator();
				}
			}
			return null;
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0000DD1C File Offset: 0x0000BF1C
		private void RaiseSelectionChangedEvent(object nextItem, bool isSettingsItem, NavigationRecommendedTransitionDirection recommendedDirection = NavigationRecommendedTransitionDirection.Default)
		{
			NavigationViewSelectionChangedEventArgs navigationViewSelectionChangedEventArgs = new NavigationViewSelectionChangedEventArgs();
			navigationViewSelectionChangedEventArgs.SelectedItem = nextItem;
			navigationViewSelectionChangedEventArgs.IsSettingsSelected = isSettingsItem;
			NavigationViewItemBase navigationViewItemBase = this.NavigationViewItemBaseOrSettingsContentFromData(nextItem);
			if (navigationViewItemBase != null)
			{
				navigationViewSelectionChangedEventArgs.SelectedItemContainer = navigationViewItemBase;
			}
			navigationViewSelectionChangedEventArgs.RecommendedNavigationTransitionInfo = this.CreateNavigationTransitionInfo(recommendedDirection);
			TypedEventHandler<NavigationView, NavigationViewSelectionChangedEventArgs> selectionChanged = this.SelectionChanged;
			if (selectionChanged == null)
			{
				return;
			}
			selectionChanged.Invoke(this, navigationViewSelectionChangedEventArgs);
		}

		// Token: 0x06000370 RID: 880 RVA: 0x0000DD70 File Offset: 0x0000BF70
		private void ChangeSelection(object prevItem, object nextItem)
		{
			NavigationView.<>c__DisplayClass139_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.prevItem = prevItem;
			CS$<>8__locals1.nextItem = nextItem;
			bool flag = this.IsSettingsItem(CS$<>8__locals1.nextItem);
			if (this.IsSelectionSuppressed(CS$<>8__locals1.nextItem))
			{
				this.UndoSelectionAndRevertSelectionTo(CS$<>8__locals1.prevItem, CS$<>8__locals1.nextItem);
				this.RaiseItemInvoked(CS$<>8__locals1.nextItem, flag, null, NavigationRecommendedTransitionDirection.Default);
				return;
			}
			NavigationRecommendedTransitionDirection recommendedDirection = this.<ChangeSelection>g__init|139_0(ref CS$<>8__locals1);
			object selectedItem = this.SelectedItem;
			if (this.m_shouldRaiseItemInvokedAfterSelection)
			{
				this.m_shouldRaiseItemInvokedAfterSelection = false;
				this.RaiseItemInvoked(CS$<>8__locals1.nextItem, flag, this.NavigationViewItemOrSettingsContentFromData(CS$<>8__locals1.nextItem), recommendedDirection);
			}
			if (selectedItem != this.SelectedItem)
			{
				return;
			}
			this.UnselectPrevItem(CS$<>8__locals1.prevItem, CS$<>8__locals1.nextItem);
			this.ChangeSelectStatusForItem(CS$<>8__locals1.nextItem, true);
			try
			{
				if (!this.m_shouldIgnoreUIASelectionRaiseAsExpandCollapseWillRaise)
				{
					AutomationPeer automationPeer = UIElementAutomationPeer.FromElement(this);
					if (automationPeer != null)
					{
						((NavigationViewAutomationPeer)automationPeer).RaiseSelectionChangedEvent(CS$<>8__locals1.prevItem, CS$<>8__locals1.nextItem);
					}
				}
			}
			finally
			{
				this.m_shouldIgnoreUIASelectionRaiseAsExpandCollapseWillRaise = false;
			}
			this.RaiseSelectionChangedEvent(CS$<>8__locals1.nextItem, flag, recommendedDirection);
			this.AnimateSelectionChanged(CS$<>8__locals1.nextItem);
			NavigationViewItem navigationViewItem = this.NavigationViewItemOrSettingsContentFromData(CS$<>8__locals1.nextItem);
			if (navigationViewItem != null)
			{
				this.ClosePaneIfNeccessaryAfterItemIsClicked(navigationViewItem);
			}
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0000DEAC File Offset: 0x0000C0AC
		private void UpdateSelectionModelSelection(IndexPath ip)
		{
			IndexPath selectedIndex = this.m_selectionModel.SelectedIndex;
			this.m_selectionModel.SelectAt(ip);
			this.UpdateIsChildSelected(selectedIndex, ip);
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0000DED9 File Offset: 0x0000C0D9
		private void UpdateIsChildSelected(IndexPath prevIP, IndexPath nextIP)
		{
			if (prevIP != null && prevIP.GetSize() > 0)
			{
				this.UpdateIsChildSelectedForIndexPath(prevIP, false);
			}
			if (nextIP != null && nextIP.GetSize() > 0)
			{
				this.UpdateIsChildSelectedForIndexPath(nextIP, true);
			}
		}

		// Token: 0x06000373 RID: 883 RVA: 0x0000DF04 File Offset: 0x0000C104
		private void UpdateIsChildSelectedForIndexPath(IndexPath ip, bool isChildSelected)
		{
			UIElement uielement = this.GetContainerForIndex(ip.GetAt(1), ip.GetAt(0) == 1);
			int num = 2;
			while (uielement != null)
			{
				NavigationViewItem navigationViewItem = uielement as NavigationViewItem;
				if (navigationViewItem != null)
				{
					navigationViewItem.IsChildSelected = isChildSelected;
					ItemsRepeater repeater = navigationViewItem.GetRepeater();
					if (repeater != null && num < ip.GetSize() - 1)
					{
						uielement = repeater.TryGetElement(ip.GetAt(num));
						num++;
						continue;
					}
				}
				uielement = null;
			}
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0000DF6C File Offset: 0x0000C16C
		private void RaiseItemInvoked(object item, bool isSettings, NavigationViewItemBase container = null, NavigationRecommendedTransitionDirection recommendedDirection = NavigationRecommendedTransitionDirection.Default)
		{
			object invokedItem = item;
			NavigationViewItemBase invokedItemContainer = container;
			NavigationViewItemInvokedEventArgs navigationViewItemInvokedEventArgs = new NavigationViewItemInvokedEventArgs();
			if (container != null)
			{
				invokedItem = container.Content;
			}
			else if (!isSettings)
			{
				NavigationViewItemBase navigationViewItemBase = this.NavigationViewItemBaseOrSettingsContentFromData(item);
				if (navigationViewItemBase != null)
				{
					invokedItem = navigationViewItemBase.Content;
					invokedItemContainer = navigationViewItemBase;
				}
			}
			else
			{
				invokedItemContainer = (item as NavigationViewItemBase);
			}
			navigationViewItemInvokedEventArgs.InvokedItem = invokedItem;
			navigationViewItemInvokedEventArgs.InvokedItemContainer = invokedItemContainer;
			navigationViewItemInvokedEventArgs.IsSettingsInvoked = isSettings;
			navigationViewItemInvokedEventArgs.RecommendedNavigationTransitionInfo = this.CreateNavigationTransitionInfo(recommendedDirection);
			TypedEventHandler<NavigationView, NavigationViewItemInvokedEventArgs> itemInvoked = this.ItemInvoked;
			if (itemInvoked == null)
			{
				return;
			}
			itemInvoked.Invoke(this, navigationViewItemInvokedEventArgs);
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0000DFE4 File Offset: 0x0000C1E4
		private void SetDisplayMode(NavigationViewDisplayMode displayMode, bool forceSetDisplayMode = false)
		{
			this.UpdateVisualStateForDisplayModeGroup(displayMode);
			if (forceSetDisplayMode || this.DisplayMode != displayMode)
			{
				this.UpdateHeaderVisibility(displayMode);
				this.UpdatePaneTabFocusNavigation();
				this.UpdatePaneToggleSize();
				this.RaiseDisplayModeChanged(displayMode);
			}
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0000E014 File Offset: 0x0000C214
		private NavigationViewVisualStateDisplayMode GetVisualStateDisplayMode(NavigationViewDisplayMode displayMode)
		{
			NavigationViewPaneDisplayMode paneDisplayMode = this.PaneDisplayMode;
			if (this.IsTopNavigationView())
			{
				return NavigationViewVisualStateDisplayMode.Minimal;
			}
			if (paneDisplayMode == NavigationViewPaneDisplayMode.Left || (paneDisplayMode == NavigationViewPaneDisplayMode.Auto && displayMode == NavigationViewDisplayMode.Expanded))
			{
				return NavigationViewVisualStateDisplayMode.Expanded;
			}
			if (paneDisplayMode == NavigationViewPaneDisplayMode.LeftCompact || (paneDisplayMode == NavigationViewPaneDisplayMode.Auto && displayMode == NavigationViewDisplayMode.Compact))
			{
				return NavigationViewVisualStateDisplayMode.Compact;
			}
			if (this.ShouldShowBackButton() || this.ShouldShowCloseButton())
			{
				return NavigationViewVisualStateDisplayMode.MinimalWithBackButton;
			}
			return NavigationViewVisualStateDisplayMode.Minimal;
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0000E060 File Offset: 0x0000C260
		private void UpdateVisualStateForDisplayModeGroup(NavigationViewDisplayMode displayMode)
		{
			SplitView rootSplitView = this.m_rootSplitView;
			if (rootSplitView != null)
			{
				NavigationViewVisualStateDisplayMode visualStateDisplayMode = this.GetVisualStateDisplayMode(displayMode);
				string text = "";
				SplitViewDisplayMode displayMode2 = SplitViewDisplayMode.Overlay;
				string text2 = "Minimal";
				switch (visualStateDisplayMode)
				{
				case NavigationViewVisualStateDisplayMode.Compact:
					text = "Compact";
					displayMode2 = SplitViewDisplayMode.CompactOverlay;
					break;
				case NavigationViewVisualStateDisplayMode.Expanded:
					text = "Expanded";
					displayMode2 = SplitViewDisplayMode.CompactInline;
					break;
				case NavigationViewVisualStateDisplayMode.Minimal:
					text = text2;
					displayMode2 = SplitViewDisplayMode.Overlay;
					break;
				case NavigationViewVisualStateDisplayMode.MinimalWithBackButton:
					text = "MinimalWithBackButton";
					displayMode2 = SplitViewDisplayMode.Overlay;
					break;
				}
				if (!this.IsPaneVisible)
				{
					displayMode2 = SplitViewDisplayMode.CompactOverlay;
				}
				bool flag = false;
				if (text == text2 && this.IsTopNavigationView())
				{
					flag = VisualStateManager.GoToState(this, "TopNavigationMinimal", false);
				}
				if (!flag)
				{
					VisualStateManager.GoToState(this, text, false);
				}
				rootSplitView.DisplayMode = displayMode2;
			}
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0000E110 File Offset: 0x0000C310
		private void OnNavigationViewItemTapped(object sender, TappedRoutedEventArgs args)
		{
			NavigationViewItem navigationViewItem = sender as NavigationViewItem;
			if (navigationViewItem != null)
			{
				this.OnNavigationViewItemInvoked(navigationViewItem);
				navigationViewItem.Focus();
				args.Handled = true;
			}
		}

		// Token: 0x06000379 RID: 889 RVA: 0x0000E13C File Offset: 0x0000C33C
		private void OnNavigationViewItemKeyDown(object sender, KeyEventArgs args)
		{
			if ((args.Key == 6 || args.Key == 18) && args.IsRepeat)
			{
				return;
			}
			NavigationViewItem navigationViewItem = sender as NavigationViewItem;
			if (navigationViewItem != null)
			{
				this.HandleKeyEventForNavigationViewItem(navigationViewItem, args);
			}
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0000E178 File Offset: 0x0000C378
		private void HandleKeyEventForNavigationViewItem(NavigationViewItem nvi, KeyEventArgs args)
		{
			Key key = args.Key;
			if (key != 6)
			{
				switch (key)
				{
				case 18:
					goto IL_39;
				case 19:
				case 20:
				case 23:
					break;
				case 21:
					args.Handled = true;
					this.KeyboardFocusLastItemFromItem(nvi);
					return;
				case 22:
					args.Handled = true;
					this.KeyboardFocusFirstItemFromItem(nvi);
					return;
				case 24:
					this.FocusNextUpItem(nvi, args);
					return;
				case 25:
					this.FocusNextRightItem(nvi, args);
					break;
				case 26:
					this.FocusNextDownItem(nvi, args);
					return;
				default:
					return;
				}
				return;
			}
			IL_39:
			args.Handled = true;
			this.OnNavigationViewItemInvoked(nvi);
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0000E208 File Offset: 0x0000C408
		private void FocusNextUpItem(NavigationViewItem nvi, KeyEventArgs args)
		{
			if (args.OriginalSource != nvi)
			{
				return;
			}
			bool flag = true;
			NavigationViewItem navigationViewItem = FocusManagerEx.FindNextFocusableElement(6) as NavigationViewItem;
			if (navigationViewItem != null)
			{
				NavigationViewItem navigationViewItem2 = navigationViewItem;
				if (navigationViewItem2.Depth == nvi.Depth)
				{
					if (this.DoesNavigationViewItemHaveChildren(navigationViewItem))
					{
						ItemsRepeater repeater = navigationViewItem2.GetRepeater();
						if (repeater != null)
						{
							if (repeater.MoveFocus(new TraversalRequest(3)))
							{
								args.Handled = true;
							}
							else
							{
								args.Handled = navigationViewItem2.Focus();
							}
						}
					}
					else
					{
						flag = false;
					}
				}
			}
			if (flag && !args.Handled && nvi.Depth > 0)
			{
				NavigationViewItem parentNavigationViewItemForContainer = this.GetParentNavigationViewItemForContainer(nvi);
				if (parentNavigationViewItemForContainer != null)
				{
					args.Handled = parentNavigationViewItemForContainer.Focus();
				}
			}
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0000E2B0 File Offset: 0x0000C4B0
		private void FocusNextDownItem(NavigationViewItem nvi, KeyEventArgs args)
		{
			if (args.OriginalSource != nvi)
			{
				return;
			}
			if (this.DoesNavigationViewItemHaveChildren(nvi))
			{
				ItemsRepeater repeater = nvi.GetRepeater();
				if (repeater != null)
				{
					args.Handled = repeater.MoveFocus(new TraversalRequest(2));
				}
			}
			if (!args.Handled)
			{
				args.Handled = nvi.MoveFocus(new TraversalRequest(7));
			}
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0000E306 File Offset: 0x0000C506
		private void FocusNextRightItem(NavigationViewItem nvi, KeyEventArgs args)
		{
			args.Handled = nvi.MoveFocus(new TraversalRequest(5));
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0000E31C File Offset: 0x0000C51C
		private void KeyboardFocusFirstItemFromItem(NavigationViewItemBase nvib)
		{
			NavigationView.<>c__DisplayClass153_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.nvib = nvib;
			Control control = this.<KeyboardFocusFirstItemFromItem>g__init|153_0(ref CS$<>8__locals1) as Control;
			if (control != null)
			{
				control.Focus();
			}
		}

		// Token: 0x0600037F RID: 895 RVA: 0x0000E354 File Offset: 0x0000C554
		private void KeyboardFocusLastItemFromItem(NavigationViewItemBase nvib)
		{
			ItemsRepeater parentRootItemsRepeaterForContainer = this.GetParentRootItemsRepeaterForContainer(nvib);
			ItemsSourceView itemsSourceView = parentRootItemsRepeaterForContainer.ItemsSourceView;
			if (itemsSourceView != null)
			{
				int index = itemsSourceView.Count - 1;
				UIElement uielement = parentRootItemsRepeaterForContainer.TryGetElement(index);
				if (uielement != null)
				{
					Control control = uielement as Control;
					if (control != null)
					{
						control.Focus();
					}
				}
			}
		}

		// Token: 0x06000380 RID: 896 RVA: 0x0000E39C File Offset: 0x0000C59C
		private void OnRepeaterGettingFocus(object sender, GettingFocusEventArgs args)
		{
			if (this.m_TabKeyPrecedesFocusChange && args.InputDevice == 4 && this.m_selectionModel.SelectedIndex != null)
			{
				NavigationView.<>c__DisplayClass155_0 CS$<>8__locals1;
				CS$<>8__locals1.oldFocusedElement = args.OldFocusedElement;
				if (CS$<>8__locals1.oldFocusedElement != null)
				{
					NavigationView.<>c__DisplayClass155_1 CS$<>8__locals2;
					CS$<>8__locals2.newRootItemsRepeater = (sender as ItemsRepeater);
					if (CS$<>8__locals2.newRootItemsRepeater != null)
					{
						bool flag = this.<OnRepeaterGettingFocus>g__init|155_0(ref CS$<>8__locals1, ref CS$<>8__locals2);
						object obj = this.<OnRepeaterGettingFocus>g__init|155_1();
						if (args != null && (CS$<>8__locals2.newRootItemsRepeater == obj && flag))
						{
							NavigationViewItemBase containerForIndexPath = this.GetContainerForIndexPath(this.m_selectionModel.SelectedIndex, true);
							if (args.TrySetNewFocusedElement(containerForIndexPath))
							{
								args.Handled = true;
							}
						}
					}
				}
			}
			this.m_TabKeyPrecedesFocusChange = false;
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0000E44C File Offset: 0x0000C64C
		private void OnNavigationViewItemOnGotFocus(object sender, RoutedEventArgs e)
		{
			NavigationViewItem navigationViewItem = sender as NavigationViewItem;
			if (navigationViewItem != null && this.IsNavigationViewListSingleSelectionFollowsFocus() && navigationViewItem.SelectsOnInvoked && !navigationViewItem.IsSelected)
			{
				if (this.IsTopNavigationView())
				{
					ItemsRepeater parentItemsRepeaterForContainer = this.GetParentItemsRepeaterForContainer(navigationViewItem);
					if (parentItemsRepeaterForContainer != null && parentItemsRepeaterForContainer != this.m_topNavRepeaterOverflowView)
					{
						this.OnNavigationViewItemInvoked(navigationViewItem);
						return;
					}
				}
				else
				{
					this.OnNavigationViewItemInvoked(navigationViewItem);
				}
			}
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0000E4A8 File Offset: 0x0000C6A8
		internal void OnSettingsInvoked()
		{
			NavigationViewItem settingsItem = this.m_settingsItem;
			if (settingsItem != null)
			{
				this.OnNavigationViewItemInvoked(settingsItem);
			}
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0000E4C6 File Offset: 0x0000C6C6
		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			this.m_TabKeyPrecedesFocusChange = false;
			base.OnPreviewKeyDown(e);
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0000E4D8 File Offset: 0x0000C6D8
		protected override void OnKeyDown(KeyEventArgs e)
		{
			Key key = e.Key;
			bool handled = false;
			this.m_TabKeyPrecedesFocusChange = false;
			if (key != 3)
			{
				if (key == 23)
				{
					if (Keyboard.Modifiers.HasFlag(1) && this.IsPaneOpen && this.IsLightDismissible())
					{
						handled = this.AttemptClosePaneLightly();
					}
				}
			}
			else
			{
				this.m_TabKeyPrecedesFocusChange = true;
			}
			e.Handled = handled;
			base.OnKeyDown(e);
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0000E548 File Offset: 0x0000C748
		private bool SelectSelectableItemWithOffset(int startIndex, int offset, ItemsRepeater repeater, int repeaterCollectionSize)
		{
			startIndex += offset;
			while (startIndex > -1 && startIndex < repeaterCollectionSize)
			{
				NavigationViewItem navigationViewItem = repeater.TryGetElement(startIndex) as NavigationViewItem;
				if (navigationViewItem != null && navigationViewItem.SelectsOnInvoked)
				{
					navigationViewItem.IsSelected = true;
					return true;
				}
				startIndex += offset;
			}
			return false;
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0000E58C File Offset: 0x0000C78C
		internal object MenuItemFromContainer(DependencyObject container)
		{
			if (container != null)
			{
				NavigationViewItemBase navigationViewItemBase = container as NavigationViewItemBase;
				if (navigationViewItemBase != null)
				{
					ItemsRepeater parentItemsRepeaterForContainer = this.GetParentItemsRepeaterForContainer(navigationViewItemBase);
					if (parentItemsRepeaterForContainer != null)
					{
						int elementIndex = parentItemsRepeaterForContainer.GetElementIndex(navigationViewItemBase);
						if (elementIndex >= 0)
						{
							return this.GetItemFromIndex(parentItemsRepeaterForContainer, elementIndex);
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0000E5C7 File Offset: 0x0000C7C7
		internal DependencyObject ContainerFromMenuItem(object item)
		{
			if (item != null)
			{
				return this.NavigationViewItemBaseOrSettingsContentFromData(item);
			}
			return null;
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0000E5D5 File Offset: 0x0000C7D5
		private void OnTopNavDataSourceChanged(NotifyCollectionChangedEventArgs args)
		{
			this.CloseTopNavigationViewFlyout();
			if (this.m_topNavigationMode != TopNavigationViewLayoutState.Uninitialized)
			{
				this.m_topDataProvider.MoveAllItemsToPrimaryList();
			}
			this.m_lastSelectedItemPendingAnimationInTopNav = null;
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0000E5F7 File Offset: 0x0000C7F7
		internal int GetNavigationViewItemCountInPrimaryList()
		{
			return this.m_topDataProvider.GetNavigationViewItemCountInPrimaryList();
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0000E604 File Offset: 0x0000C804
		internal int GetNavigationViewItemCountInTopNav()
		{
			return this.m_topDataProvider.GetNavigationViewItemCountInTopNav();
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0000E611 File Offset: 0x0000C811
		internal SplitView GetSplitView()
		{
			return this.m_rootSplitView;
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0000E619 File Offset: 0x0000C819
		internal TopNavigationViewDataProvider GetTopDataProvider()
		{
			return this.m_topDataProvider;
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0000E621 File Offset: 0x0000C821
		internal void TopNavigationViewItemContentChanged()
		{
			if (this.m_appliedTemplate)
			{
				this.m_topDataProvider.InvalidWidthCache();
				base.InvalidateMeasure();
			}
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0000E63C File Offset: 0x0000C83C
		private NavigationTransitionInfo CreateNavigationTransitionInfo(NavigationRecommendedTransitionDirection recommendedTransitionDirection)
		{
			if (recommendedTransitionDirection == NavigationRecommendedTransitionDirection.FromOverflow)
			{
				recommendedTransitionDirection = NavigationRecommendedTransitionDirection.FromRight;
			}
			if ((recommendedTransitionDirection == NavigationRecommendedTransitionDirection.FromLeft || recommendedTransitionDirection == NavigationRecommendedTransitionDirection.FromRight) && SharedHelpers.IsRS5OrHigher())
			{
				SlideNavigationTransitionInfo slideNavigationTransitionInfo = new SlideNavigationTransitionInfo();
				SlideNavigationTransitionEffect effect = (recommendedTransitionDirection == NavigationRecommendedTransitionDirection.FromRight) ? 2 : 1;
				if (slideNavigationTransitionInfo != null)
				{
					slideNavigationTransitionInfo.Effect = effect;
				}
				return slideNavigationTransitionInfo;
			}
			return new EntranceNavigationTransitionInfo();
		}

		// Token: 0x0600038F RID: 911 RVA: 0x0000E680 File Offset: 0x0000C880
		private NavigationRecommendedTransitionDirection GetRecommendedTransitionDirection(DependencyObject prev, DependencyObject next)
		{
			NavigationRecommendedTransitionDirection result = NavigationRecommendedTransitionDirection.Default;
			ItemsRepeater topNavRepeater = this.m_topNavRepeater;
			if (prev != null && next != null && topNavRepeater != null)
			{
				IndexPath indexPathForContainer = this.GetIndexPathForContainer(prev as NavigationViewItemBase);
				IndexPath indexPathForContainer2 = this.GetIndexPathForContainer(next as NavigationViewItemBase);
				int num = indexPathForContainer.CompareTo(indexPathForContainer2);
				if (num != -1)
				{
					if (num != 1)
					{
						result = NavigationRecommendedTransitionDirection.Default;
					}
					else
					{
						result = NavigationRecommendedTransitionDirection.FromLeft;
					}
				}
				else
				{
					result = NavigationRecommendedTransitionDirection.FromRight;
				}
			}
			return result;
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0000E6D4 File Offset: 0x0000C8D4
		private NavigationViewTemplateSettings GetTemplateSettings()
		{
			return this.TemplateSettings;
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0000E6DC File Offset: 0x0000C8DC
		private bool IsNavigationViewListSingleSelectionFollowsFocus()
		{
			return this.SelectionFollowsFocus == NavigationViewSelectionFollowsFocus.Enabled;
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0000E6E7 File Offset: 0x0000C8E7
		private void UpdateSingleSelectionFollowsFocusTemplateSetting()
		{
			this.GetTemplateSettings().SingleSelectionFollowsFocus = this.IsNavigationViewListSingleSelectionFollowsFocus();
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0000E6FC File Offset: 0x0000C8FC
		private void OnMenuItemsSourceCollectionChanged(object sender, object args)
		{
			if (!this.IsTopNavigationView())
			{
				ItemsRepeater leftNavRepeater = this.m_leftNavRepeater;
				if (leftNavRepeater != null)
				{
					leftNavRepeater.UpdateLayout();
				}
				this.UpdatePaneLayout();
			}
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0000E728 File Offset: 0x0000C928
		private void OnSelectedItemPropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			object newValue = args.NewValue;
			object oldValue = args.OldValue;
			this.ChangeSelection(oldValue, newValue);
			if (this.m_appliedTemplate && this.IsTopNavigationView() && (!this.m_layoutUpdatedToken || (newValue != null && this.m_topDataProvider.IndexOf(newValue) != -1 && this.m_topDataProvider.IndexOf(newValue, NavigationViewSplitVectorID.PrimaryList) == -1)))
			{
				this.InvalidateTopNavPrimaryLayout();
			}
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0000E78D File Offset: 0x0000C98D
		private void SetSelectedItemAndExpectItemInvokeWhenSelectionChangedIfNotInvokedFromAPI(object item)
		{
			this.SelectedItem = item;
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0000E798 File Offset: 0x0000C998
		private void ChangeSelectStatusForItem(object item, bool selected)
		{
			NavigationViewItem navigationViewItem = this.NavigationViewItemOrSettingsContentFromData(item);
			if (navigationViewItem != null)
			{
				navigationViewItem.IsSelected = selected;
				return;
			}
			if (selected)
			{
				IndexPath indexPathOfItem = this.GetIndexPathOfItem(item);
				if (indexPathOfItem != null && indexPathOfItem.GetSize() > 0)
				{
					try
					{
						this.m_shouldIgnoreNextSelectionChange = true;
						this.UpdateSelectionModelSelection(indexPathOfItem);
					}
					finally
					{
						this.m_shouldIgnoreNextSelectionChange = false;
					}
				}
			}
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0000E7F8 File Offset: 0x0000C9F8
		private bool IsSettingsItem(object item)
		{
			bool result = false;
			if (item != null)
			{
				NavigationViewItem settingsItem = this.m_settingsItem;
				if (settingsItem != null)
				{
					result = (settingsItem == item || settingsItem.Content == item);
				}
			}
			return result;
		}

		// Token: 0x06000398 RID: 920 RVA: 0x0000E828 File Offset: 0x0000CA28
		private void UnselectPrevItem(object prevItem, object nextItem)
		{
			if (prevItem != null && prevItem != nextItem)
			{
				bool flag = !this.m_shouldIgnoreNextSelectionChange;
				try
				{
					this.m_shouldIgnoreNextSelectionChange = true;
					this.ChangeSelectStatusForItem(prevItem, false);
				}
				finally
				{
					if (flag)
					{
						this.m_shouldIgnoreNextSelectionChange = false;
					}
				}
			}
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0000E874 File Offset: 0x0000CA74
		private void UndoSelectionAndRevertSelectionTo(object prevSelectedItem, object nextItem)
		{
			object selectedItem = null;
			if (prevSelectedItem != null)
			{
				if (this.IsSelectionSuppressed(prevSelectedItem))
				{
					this.AnimateSelectionChanged(null);
				}
				else
				{
					this.ChangeSelectStatusForItem(prevSelectedItem, true);
					this.AnimateSelectionChangedToItem(prevSelectedItem);
					selectedItem = prevSelectedItem;
				}
			}
			else
			{
				this.ChangeSelectStatusForItem(nextItem, false);
			}
			this.SelectedItem = selectedItem;
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0000E8BC File Offset: 0x0000CABC
		private void CloseTopNavigationViewFlyout()
		{
			Button topNavOverflowButton = this.m_topNavOverflowButton;
			if (topNavOverflowButton != null)
			{
				FlyoutBase flyoutBase = topNavOverflowButton.Flyout();
				if (flyoutBase != null)
				{
					flyoutBase.Hide();
				}
			}
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0000E8E4 File Offset: 0x0000CAE4
		private void UpdateVisualState(bool useTransitions = false)
		{
			if (this.m_appliedTemplate)
			{
				VisualStateManager.GoToState(this, (this.AutoSuggestBox != null) ? "AutoSuggestBoxVisible" : "AutoSuggestBoxCollapsed", false);
				VisualStateManager.GoToState(this, this.IsSettingsVisible ? "SettingsVisible" : "SettingsCollapsed", false);
				if (this.IsTopNavigationView())
				{
					this.UpdateVisualStateForOverflowButton();
					return;
				}
				this.UpdateLeftNavigationOnlyVisualState(useTransitions);
			}
		}

		// Token: 0x0600039C RID: 924 RVA: 0x0000E94C File Offset: 0x0000CB4C
		private void UpdateVisualStateForOverflowButton()
		{
			string text = (this.OverflowLabelMode == NavigationViewOverflowLabelMode.MoreLabel) ? "OverflowButtonWithLabel" : "OverflowButtonNoLabel";
			VisualStateManager.GoToState(this, text, false);
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0000E978 File Offset: 0x0000CB78
		private void UpdateLeftNavigationOnlyVisualState(bool useTransitions)
		{
			VisualStateManager.GoToState(this, this.IsPaneToggleButtonVisible ? "TogglePaneButtonVisible" : "TogglePaneButtonCollapsed", false);
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0000E9A3 File Offset: 0x0000CBA3
		private void InvalidateTopNavPrimaryLayout()
		{
			if (this.m_appliedTemplate && this.IsTopNavigationView())
			{
				base.InvalidateMeasure();
			}
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0000E9BB File Offset: 0x0000CBBB
		private double MeasureTopNavigationViewDesiredWidth(Size availableSize)
		{
			return LayoutUtils.MeasureAndGetDesiredWidthFor(this.m_topNavGrid, availableSize);
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0000E9C9 File Offset: 0x0000CBC9
		private double MeasureTopNavMenuItemsHostDesiredWidth(Size availableSize)
		{
			return LayoutUtils.MeasureAndGetDesiredWidthFor(this.m_topNavRepeater, availableSize);
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0000E9D7 File Offset: 0x0000CBD7
		private double GetTopNavigationViewActualWidth()
		{
			return LayoutUtils.GetActualWidthFor(this.m_topNavGrid);
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0000E9E4 File Offset: 0x0000CBE4
		private bool HasTopNavigationViewItemNotInPrimaryList()
		{
			return this.m_topDataProvider.GetPrimaryListSize() != this.m_topDataProvider.Size();
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0000EA01 File Offset: 0x0000CC01
		private void ResetAndRearrangeTopNavItems(Size availableSize)
		{
			if (this.HasTopNavigationViewItemNotInPrimaryList())
			{
				this.m_topDataProvider.MoveAllItemsToPrimaryList();
			}
			this.ArrangeTopNavItems(availableSize);
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0000EA1D File Offset: 0x0000CC1D
		private void HandleTopNavigationMeasureOverride(Size availableSize)
		{
			if (this.HasTopNavigationViewItemNotInPrimaryList())
			{
				this.HandleTopNavigationMeasureOverrideOverflow(availableSize);
			}
			else
			{
				this.HandleTopNavigationMeasureOverrideNormal(availableSize);
			}
			if (this.m_topNavigationMode == TopNavigationViewLayoutState.Uninitialized)
			{
				this.m_topNavigationMode = TopNavigationViewLayoutState.Initialized;
			}
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x0000EA46 File Offset: 0x0000CC46
		private void HandleTopNavigationMeasureOverrideNormal(Size availableSize)
		{
			if (this.MeasureTopNavigationViewDesiredWidth(NavigationView.c_infSize) > availableSize.Width)
			{
				this.ResetAndRearrangeTopNavItems(availableSize);
			}
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0000EA64 File Offset: 0x0000CC64
		private void HandleTopNavigationMeasureOverrideOverflow(Size availableSize)
		{
			double num = this.MeasureTopNavigationViewDesiredWidth(NavigationView.c_infSize);
			if (num > availableSize.Width)
			{
				this.ShrinkTopNavigationSize(num, availableSize);
				return;
			}
			if (num < availableSize.Width)
			{
				double num2 = this.m_topDataProvider.WidthRequiredToRecoveryAllItemsToPrimary();
				if (availableSize.Width >= num + num2 + (double)this.m_topNavigationRecoveryGracePeriodWidth)
				{
					this.ResetAndRearrangeTopNavItems(availableSize);
					return;
				}
				List<int> indexes = this.FindMovableItemsRecoverToPrimaryList(availableSize.Width - num, new List<int>());
				this.m_topDataProvider.MoveItemsToPrimaryList(indexes);
			}
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0000EAE4 File Offset: 0x0000CCE4
		private void ArrangeTopNavItems(Size availableSize)
		{
			this.SetOverflowButtonVisibility(2);
			double num = this.MeasureTopNavigationViewDesiredWidth(NavigationView.c_infSize);
			if (num >= availableSize.Width)
			{
				this.SetOverflowButtonVisibility(0);
				double num2 = this.MeasureTopNavigationViewDesiredWidth(NavigationView.c_infSize);
				this.m_topDataProvider.OverflowButtonWidth(num2 - num);
				this.ShrinkTopNavigationSize(num2, availableSize);
			}
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0000EB37 File Offset: 0x0000CD37
		private void SetOverflowButtonVisibility(Visibility visibility)
		{
			if (visibility != this.TemplateSettings.OverflowButtonVisibility)
			{
				this.GetTemplateSettings().OverflowButtonVisibility = visibility;
			}
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0000EB54 File Offset: 0x0000CD54
		private void SelectOverflowItem(object item, IndexPath ip)
		{
			NavigationView.<>c__DisplayClass196_0 CS$<>8__locals1;
			CS$<>8__locals1.ip = ip;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.item = item;
			object obj = this.<SelectOverflowItem>g__init|196_0(ref CS$<>8__locals1);
			int num = this.m_topDataProvider.IndexOf(obj);
			double widthForItem = this.m_topDataProvider.GetWidthForItem(num);
			bool flag = !this.m_topDataProvider.IsValidWidthForItem(num);
			if (!flag)
			{
				double topNavigationViewActualWidth = this.GetTopNavigationViewActualWidth();
				double num2 = this.MeasureTopNavigationViewDesiredWidth(NavigationView.c_infSize);
				object selectedItem = this.SelectedItem;
				if (selectedItem != null)
				{
					int num3 = this.m_topDataProvider.IndexOf(selectedItem);
					if (num3 != -1)
					{
						this.m_topDataProvider.GetWidthForItem(num3);
					}
				}
				double num4 = num2 + widthForItem - topNavigationViewActualWidth;
				List<int> list = this.FindMovableItemsToBeRemovedFromPrimaryList(num4, new List<int>());
				double num5 = this.m_topDataProvider.CalculateWidthForItems(list) - num4;
				double availableWidth = num5;
				List<int> list2 = new List<int>();
				list2.Add(num);
				List<int> list3 = this.FindMovableItemsRecoverToPrimaryList(availableWidth, list2);
				CollectionHelper.unique_push_back<int>(list3, num);
				this.m_lastSelectedItemPendingAnimationInTopNav = obj;
				if (CS$<>8__locals1.ip != null && CS$<>8__locals1.ip.GetSize() > 0)
				{
					using (List<int>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current == CS$<>8__locals1.ip.GetAt(1))
							{
								if (this.m_activeIndicator != null)
								{
									this.AnimateSelectionChanged(null);
									break;
								}
								break;
							}
						}
					}
				}
				if (this.m_topDataProvider.HasInvalidWidth(list3))
				{
					flag = true;
				}
				else
				{
					this.m_topDataProvider.MoveItemsToPrimaryList(list3);
					this.m_topDataProvider.MoveItemsOutOfPrimaryList(list);
					if (this.NeedRearrangeOfTopElementsAfterOverflowSelectionChanged(num))
					{
						flag = true;
					}
					if (!flag)
					{
						this.SetSelectedItemAndExpectItemInvokeWhenSelectionChangedIfNotInvokedFromAPI(CS$<>8__locals1.item);
						base.InvalidateMeasure();
					}
				}
			}
			if (flag)
			{
				this.m_topDataProvider.MoveAllItemsToPrimaryList();
				this.SetSelectedItemAndExpectItemInvokeWhenSelectionChangedIfNotInvokedFromAPI(CS$<>8__locals1.item);
				this.InvalidateTopNavPrimaryLayout();
			}
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000ED28 File Offset: 0x0000CF28
		private bool NeedRearrangeOfTopElementsAfterOverflowSelectionChanged(int selectedOriginalIndex)
		{
			bool flag = false;
			int count = this.m_topDataProvider.GetPrimaryItems().Count;
			int num = this.m_topDataProvider.ConvertOriginalIndexToIndex(selectedOriginalIndex);
			if (num < count - 1)
			{
				int num2 = num + 1;
				int num3 = selectedOriginalIndex + 1;
				int num4 = selectedOriginalIndex - 1;
				if (num > 0)
				{
					List<int> list = new List<int>();
					list.Add(num2 - 1);
					if (this.m_topDataProvider.ConvertPrimaryIndexToIndex(list)[0] != num4)
					{
						flag = true;
					}
				}
				while (!flag && num2 < count)
				{
					List<int> list2 = new List<int>();
					list2.Add(num2);
					List<int> list3 = this.m_topDataProvider.ConvertPrimaryIndexToIndex(list2);
					if (num3 != list3[0])
					{
						flag = true;
						break;
					}
					num2++;
					num3++;
				}
			}
			return flag;
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0000EDDC File Offset: 0x0000CFDC
		private void ShrinkTopNavigationSize(double desiredWidth, Size availableSize)
		{
			this.UpdateTopNavigationWidthCache();
			int selectedItemIndex = this.GetSelectedItemIndex();
			double num = this.MeasureTopNavMenuItemsHostDesiredWidth(NavigationView.c_infSize) - (desiredWidth - availableSize.Width);
			if (num >= 0.0)
			{
				List<int> list = this.FindMovableItemsBeyondAvailableWidth(num);
				this.KeepAtLeastOneItemInPrimaryList(list, true);
				this.m_topDataProvider.MoveItemsOutOfPrimaryList(list);
			}
			desiredWidth = this.MeasureTopNavigationViewDesiredWidth(NavigationView.c_infSize);
			double num2 = desiredWidth - availableSize.Width;
			if (num2 > 0.0)
			{
				double widthAtLeastToBeRemoved = num2;
				List<int> list2 = new List<int>();
				list2.Add(selectedItemIndex);
				List<int> list3 = this.FindMovableItemsToBeRemovedFromPrimaryList(widthAtLeastToBeRemoved, list2);
				this.KeepAtLeastOneItemInPrimaryList(list3, false);
				this.m_topDataProvider.MoveItemsOutOfPrimaryList(list3);
			}
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0000EE84 File Offset: 0x0000D084
		private List<int> FindMovableItemsRecoverToPrimaryList(double availableWidth, List<int> includeItems)
		{
			List<int> list = new List<int>();
			int num = this.m_topDataProvider.Size();
			foreach (int num2 in includeItems)
			{
				double widthForItem = this.m_topDataProvider.GetWidthForItem(num2);
				list.Add(num2);
				availableWidth -= widthForItem;
			}
			int num3 = 0;
			while (num3 < num && availableWidth > 0.0)
			{
				if (!this.m_topDataProvider.IsItemInPrimaryList(num3) && !CollectionHelper.contains<int>(includeItems, num3))
				{
					double widthForItem2 = this.m_topDataProvider.GetWidthForItem(num3);
					if (availableWidth < widthForItem2)
					{
						break;
					}
					list.Add(num3);
					availableWidth -= widthForItem2;
				}
				num3++;
			}
			if (num3 == num && !list.Empty<int>())
			{
				list.RemoveLast<int>();
			}
			return list;
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0000EF60 File Offset: 0x0000D160
		private List<int> FindMovableItemsToBeRemovedFromPrimaryList(double widthAtLeastToBeRemoved, List<int> excludeItems)
		{
			List<int> list = new List<int>();
			int num = this.m_topDataProvider.Size() - 1;
			while (num >= 0 && widthAtLeastToBeRemoved > 0.0)
			{
				if (this.m_topDataProvider.IsItemInPrimaryList(num) && !CollectionHelper.contains<int>(excludeItems, num))
				{
					double widthForItem = this.m_topDataProvider.GetWidthForItem(num);
					list.Add(num);
					widthAtLeastToBeRemoved -= widthForItem;
				}
				num--;
			}
			return list;
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0000EFC8 File Offset: 0x0000D1C8
		private List<int> FindMovableItemsBeyondAvailableWidth(double availableWidth)
		{
			List<int> list = new List<int>();
			ItemsRepeater topNavRepeater = this.m_topNavRepeater;
			if (topNavRepeater != null)
			{
				int num = this.m_topDataProvider.IndexOf(this.SelectedItem, NavigationViewSplitVectorID.PrimaryList);
				int primaryListSize = this.m_topDataProvider.GetPrimaryListSize();
				double num2 = 0.0;
				for (int i = 0; i < primaryListSize; i++)
				{
					if (i != num)
					{
						bool flag = true;
						if (num2 <= availableWidth)
						{
							UIElement uielement = topNavRepeater.TryGetElement(i);
							if (uielement != null && uielement != null)
							{
								UIElement uielement2 = uielement;
								double width = uielement2.DesiredSize.Width;
								num2 += width;
								flag = (num2 > availableWidth);
							}
						}
						if (flag)
						{
							list.Add(i);
						}
					}
				}
			}
			return this.m_topDataProvider.ConvertPrimaryIndexToIndex(list);
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0000F07D File Offset: 0x0000D27D
		private void KeepAtLeastOneItemInPrimaryList(List<int> itemInPrimaryToBeRemoved, bool shouldKeepFirst)
		{
			if (!itemInPrimaryToBeRemoved.Empty<int>() && itemInPrimaryToBeRemoved.Count == this.m_topDataProvider.GetPrimaryListSize())
			{
				if (shouldKeepFirst)
				{
					itemInPrimaryToBeRemoved.RemoveAt(0);
					return;
				}
				itemInPrimaryToBeRemoved.RemoveLast<int>();
			}
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0000F0AB File Offset: 0x0000D2AB
		private int GetSelectedItemIndex()
		{
			return this.m_topDataProvider.IndexOf(this.SelectedItem);
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0000F0BE File Offset: 0x0000D2BE
		private double GetPaneToggleButtonWidth()
		{
			return (double)SharedHelpers.FindResource("PaneToggleButtonWidth", this, 40.0);
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0000F0DE File Offset: 0x0000D2DE
		private double GetPaneToggleButtonHeight()
		{
			return (double)SharedHelpers.FindResource("PaneToggleButtonHeight", this, 40.0);
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0000F100 File Offset: 0x0000D300
		private void UpdateTopNavigationWidthCache()
		{
			int primaryListSize = this.m_topDataProvider.GetPrimaryListSize();
			ItemsRepeater topNavRepeater = this.m_topNavRepeater;
			if (topNavRepeater != null)
			{
				for (int i = 0; i < primaryListSize; i++)
				{
					UIElement uielement = topNavRepeater.TryGetElement(i);
					if (uielement == null)
					{
						break;
					}
					if (uielement != null)
					{
						UIElement uielement2 = uielement;
						double width = uielement2.DesiredSize.Width;
						this.m_topDataProvider.UpdateWidthForPrimaryItem(i, width);
					}
				}
			}
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0000F160 File Offset: 0x0000D360
		private bool IsTopNavigationView()
		{
			return this.PaneDisplayMode == NavigationViewPaneDisplayMode.Top;
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x0000F16B File Offset: 0x0000D36B
		private bool IsTopPrimaryListVisible()
		{
			return this.m_topNavRepeater != null && this.TemplateSettings.TopPaneVisibility == 0;
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0000F185 File Offset: 0x0000D385
		private void CoerceToGreaterThanZero(ref double value)
		{
			value = Math.Max(value, 0.0);
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0000F19C File Offset: 0x0000D39C
		private void PropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			DependencyProperty property = args.Property;
			if (property == NavigationView.IsPaneOpenProperty)
			{
				this.OnIsPaneOpenChanged();
				this.UpdateVisualStateForDisplayModeGroup(this.DisplayMode);
				return;
			}
			if (property == NavigationView.CompactModeThresholdWidthProperty || property == NavigationView.ExpandedModeThresholdWidthProperty)
			{
				this.UpdateAdaptiveLayout(base.ActualWidth, false);
				return;
			}
			if (property == NavigationView.AlwaysShowHeaderProperty || property == NavigationView.HeaderProperty)
			{
				this.UpdateHeaderVisibility();
				return;
			}
			if (property == NavigationView.SelectedItemProperty)
			{
				this.OnSelectedItemPropertyChanged(args);
				return;
			}
			if (property == NavigationView.PaneTitleProperty)
			{
				this.UpdatePaneTitleFrameworkElementParents();
				this.UpdateBackAndCloseButtonsVisibility();
				this.UpdatePaneToggleSize();
				return;
			}
			if (property == NavigationView.IsBackButtonVisibleProperty)
			{
				this.UpdateBackAndCloseButtonsVisibility();
				this.UpdateAdaptiveLayout(base.ActualWidth, false);
				if (this.IsTopNavigationView())
				{
					this.InvalidateTopNavPrimaryLayout();
				}
				Button backButton = this.m_backButton;
				if (backButton != null)
				{
					backButton.UpdateLayout();
				}
				this.UpdatePaneLayout();
				return;
			}
			if (property == NavigationView.MenuItemsSourceProperty)
			{
				this.UpdateRepeaterItemsSource(true);
				return;
			}
			if (property == NavigationView.MenuItemsProperty)
			{
				this.UpdateRepeaterItemsSource(true);
				return;
			}
			if (property == NavigationView.FooterMenuItemsSourceProperty)
			{
				this.UpdateFooterRepeaterItemsSource(true, true);
				return;
			}
			if (property == NavigationView.FooterMenuItemsProperty)
			{
				this.UpdateFooterRepeaterItemsSource(true, true);
				return;
			}
			if (property == NavigationView.PaneDisplayModeProperty)
			{
				this.m_wasForceClosed = false;
				this.CollapseTopLevelMenuItems((NavigationViewPaneDisplayMode)args.OldValue);
				this.UpdatePaneToggleButtonVisibility();
				this.UpdatePaneDisplayMode((NavigationViewPaneDisplayMode)args.OldValue, (NavigationViewPaneDisplayMode)args.NewValue);
				this.UpdatePaneTitleFrameworkElementParents();
				this.UpdatePaneVisibility();
				this.UpdateVisualState(false);
				this.UpdatePaneButtonsWidths();
				return;
			}
			if (property == NavigationView.IsPaneVisibleProperty)
			{
				this.UpdatePaneVisibility();
				this.UpdateVisualStateForDisplayModeGroup(this.DisplayMode);
				if (!this.IsPaneVisible && this.IsPaneOpen)
				{
					this.ClosePane();
				}
				if (this.IsPaneVisible && this.DisplayMode == NavigationViewDisplayMode.Expanded && !this.IsPaneOpen)
				{
					this.OpenPane();
					return;
				}
			}
			else if (property == NavigationView.OverflowLabelModeProperty)
			{
				if (this.m_appliedTemplate)
				{
					this.UpdateVisualStateForOverflowButton();
					this.InvalidateTopNavPrimaryLayout();
					return;
				}
			}
			else
			{
				if (property == NavigationView.AutoSuggestBoxProperty)
				{
					this.InvalidateTopNavPrimaryLayout();
					return;
				}
				if (property == NavigationView.SelectionFollowsFocusProperty)
				{
					this.UpdateSingleSelectionFollowsFocusTemplateSetting();
					return;
				}
				if (property == NavigationView.IsPaneToggleButtonVisibleProperty)
				{
					this.UpdatePaneTitleFrameworkElementParents();
					this.UpdateBackAndCloseButtonsVisibility();
					this.UpdatePaneToggleButtonVisibility();
					this.UpdateVisualState(false);
					return;
				}
				if (property == NavigationView.IsSettingsVisibleProperty)
				{
					this.UpdateFooterRepeaterItemsSource(false, true);
					return;
				}
				if (property == NavigationView.CompactPaneLengthProperty)
				{
					this.UpdatePaneShadow();
					this.UpdatePaneButtonsWidths();
					return;
				}
				if (property == NavigationView.IsTitleBarAutoPaddingEnabledProperty)
				{
					this.UpdateTitleBarPadding();
					return;
				}
				if (property == NavigationView.MenuItemTemplateProperty || property == NavigationView.MenuItemTemplateSelectorProperty)
				{
					this.SyncItemTemplates();
				}
			}
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x0000F414 File Offset: 0x0000D614
		private void UpdateNavigationViewItemsFactory()
		{
			object obj = this.MenuItemTemplate;
			if (obj == null)
			{
				obj = this.MenuItemTemplateSelector;
			}
			this.m_navigationViewItemsFactory.UserElementFactory(obj);
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0000F43E File Offset: 0x0000D63E
		private void SyncItemTemplates()
		{
			this.UpdateNavigationViewItemsFactory();
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0000F448 File Offset: 0x0000D648
		private void OnRepeaterIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if ((bool)e.NewValue)
			{
				base.Dispatcher.BeginInvoke(delegate()
				{
					if (((ItemsRepeater)sender).IsLoaded)
					{
						this.OnRepeaterLoaded(sender, null);
					}
				}, 6, Array.Empty<object>());
			}
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0000F498 File Offset: 0x0000D698
		private void OnRepeaterLoaded(object sender, RoutedEventArgs args)
		{
			object selectedItem = this.SelectedItem;
			if (selectedItem != null)
			{
				if (!this.IsSelectionSuppressed(selectedItem))
				{
					NavigationViewItem navigationViewItem = this.NavigationViewItemOrSettingsContentFromData(selectedItem);
					if (navigationViewItem != null)
					{
						navigationViewItem.IsSelected = true;
					}
				}
				this.AnimateSelectionChanged(selectedItem);
			}
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0000F4D4 File Offset: 0x0000D6D4
		private void OnUnloaded(object sender, RoutedEventArgs args)
		{
			CoreApplicationViewTitleBar coreTitleBar = this.m_coreTitleBar;
			if (coreTitleBar != null)
			{
				coreTitleBar.LayoutMetricsChanged -= new TypedEventHandler<CoreApplicationViewTitleBar, object>(this.OnTitleBarMetricsChanged);
				coreTitleBar.IsVisibleChanged -= new TypedEventHandler<CoreApplicationViewTitleBar, object>(this.OnTitleBarIsVisibleChanged);
			}
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0000F510 File Offset: 0x0000D710
		private void OnLoaded(object sender, RoutedEventArgs args)
		{
			CoreApplicationViewTitleBar coreTitleBar = this.m_coreTitleBar;
			if (coreTitleBar != null)
			{
				coreTitleBar.LayoutMetricsChanged += new TypedEventHandler<CoreApplicationViewTitleBar, object>(this.OnTitleBarMetricsChanged);
				coreTitleBar.IsVisibleChanged += new TypedEventHandler<CoreApplicationViewTitleBar, object>(this.OnTitleBarIsVisibleChanged);
			}
			this.UpdatePaneButtonsWidths();
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0000F554 File Offset: 0x0000D754
		private void OnIsPaneOpenChanged()
		{
			bool isPaneOpen = this.IsPaneOpen;
			if (isPaneOpen && this.m_wasForceClosed)
			{
				this.m_wasForceClosed = false;
			}
			else if (!this.m_isOpenPaneForInteraction && !isPaneOpen)
			{
				SplitView rootSplitView = this.m_rootSplitView;
				if (rootSplitView != null)
				{
					this.m_wasForceClosed = rootSplitView.IsPaneOpen;
				}
				else
				{
					this.m_wasForceClosed = true;
				}
			}
			this.SetPaneToggleButtonAutomationName();
			this.UpdatePaneTabFocusNavigation();
			this.UpdateSettingsItemToolTip();
			this.UpdatePaneTitleFrameworkElementParents();
			if (SharedHelpers.IsThemeShadowAvailable())
			{
				SplitView rootSplitView2 = this.m_rootSplitView;
				if (rootSplitView2 != null)
				{
					SplitViewDisplayMode displayMode = rootSplitView2.DisplayMode;
					if (displayMode != SplitViewDisplayMode.Overlay)
					{
						bool flag = displayMode == SplitViewDisplayMode.CompactOverlay;
					}
					UIElement pane = rootSplitView2.Pane;
				}
			}
			this.UpdatePaneButtonsWidths();
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0000F5F0 File Offset: 0x0000D7F0
		private void UpdatePaneToggleButtonVisibility()
		{
			bool visible = this.IsPaneToggleButtonVisible && !this.IsTopNavigationView();
			this.GetTemplateSettings().PaneToggleButtonVisibility = Util.VisibilityFromBool(visible);
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0000F624 File Offset: 0x0000D824
		private void UpdatePaneDisplayMode()
		{
			if (!this.m_appliedTemplate)
			{
				return;
			}
			if (!this.IsTopNavigationView())
			{
				this.UpdateAdaptiveLayout(base.ActualWidth, true);
				this.SwapPaneHeaderContent(this.m_leftNavPaneHeaderContentBorder, this.m_paneHeaderOnTopPane, "PaneHeader");
				this.SwapPaneHeaderContent(this.m_leftNavPaneCustomContentBorder, this.m_paneCustomContentOnTopPane, "PaneCustomContent");
				this.SwapPaneHeaderContent(this.m_leftNavFooterContentBorder, this.m_paneFooterOnTopPane, "PaneFooter");
				this.CreateAndHookEventsToSettings();
			}
			else
			{
				this.ClosePane();
				this.SetDisplayMode(NavigationViewDisplayMode.Minimal, true);
				this.SwapPaneHeaderContent(this.m_paneHeaderOnTopPane, this.m_leftNavPaneHeaderContentBorder, "PaneHeader");
				this.SwapPaneHeaderContent(this.m_paneCustomContentOnTopPane, this.m_leftNavPaneCustomContentBorder, "PaneCustomContent");
				this.SwapPaneHeaderContent(this.m_paneFooterOnTopPane, this.m_leftNavFooterContentBorder, "PaneFooter");
				this.CreateAndHookEventsToSettings();
			}
			this.UpdateContentBindingsForPaneDisplayMode();
			this.UpdateRepeaterItemsSource(false);
			this.UpdateFooterRepeaterItemsSource(false, false);
			if (this.SelectedItem != null)
			{
				this.m_OrientationChangedPendingAnimation = true;
			}
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0000F719 File Offset: 0x0000D919
		private void UpdatePaneDisplayMode(NavigationViewPaneDisplayMode oldDisplayMode, NavigationViewPaneDisplayMode newDisplayMode)
		{
			if (!this.m_appliedTemplate)
			{
				return;
			}
			this.UpdatePaneDisplayMode();
			if (!this.IsTopNavigationView())
			{
				if (this.IsPaneOpen)
				{
					if (newDisplayMode == NavigationViewPaneDisplayMode.LeftMinimal)
					{
						this.ClosePane();
						return;
					}
				}
				else if (oldDisplayMode == NavigationViewPaneDisplayMode.LeftMinimal && newDisplayMode == NavigationViewPaneDisplayMode.Left)
				{
					this.OpenPane();
				}
			}
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0000F754 File Offset: 0x0000D954
		private void UpdatePaneVisibility()
		{
			NavigationViewTemplateSettings templateSettings = this.GetTemplateSettings();
			if (this.IsPaneVisible)
			{
				if (this.IsTopNavigationView())
				{
					templateSettings.LeftPaneVisibility = 2;
					templateSettings.TopPaneVisibility = 0;
				}
				else
				{
					templateSettings.TopPaneVisibility = 2;
					templateSettings.LeftPaneVisibility = 0;
				}
				VisualStateManager.GoToState(this, "PaneVisible", false);
				return;
			}
			templateSettings.TopPaneVisibility = 2;
			templateSettings.LeftPaneVisibility = 2;
			VisualStateManager.GoToState(this, "PaneCollapsed", false);
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0000F7C0 File Offset: 0x0000D9C0
		private void SwapPaneHeaderContent(ContentControl newParentTrackRef, ContentControl oldParentTrackRef, string propertyPathName)
		{
			if (newParentTrackRef != null)
			{
				if (oldParentTrackRef != null)
				{
					oldParentTrackRef.ClearValue(ContentControl.ContentProperty);
				}
				SharedHelpers.SetBinding(propertyPathName, newParentTrackRef, ContentControl.ContentProperty);
			}
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0000F7F0 File Offset: 0x0000D9F0
		private void UpdateContentBindingsForPaneDisplayMode()
		{
			UIElement uielement;
			UIElement uielement2;
			if (!this.IsTopNavigationView())
			{
				uielement = this.m_leftNavPaneAutoSuggestBoxPresenter;
				uielement2 = this.m_topNavPaneAutoSuggestBoxPresenter;
			}
			else
			{
				uielement = this.m_topNavPaneAutoSuggestBoxPresenter;
				uielement2 = this.m_leftNavPaneAutoSuggestBoxPresenter;
			}
			if (uielement != null)
			{
				if (uielement2 != null)
				{
					uielement2.ClearValue(ContentControl.ContentProperty);
				}
				SharedHelpers.SetBinding("AutoSuggestBox", uielement, ContentControl.ContentProperty);
			}
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0000F848 File Offset: 0x0000DA48
		private void UpdateHeaderVisibility()
		{
			if (!this.m_appliedTemplate)
			{
				return;
			}
			this.UpdateHeaderVisibility(this.DisplayMode);
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0000F860 File Offset: 0x0000DA60
		private void UpdateHeaderVisibility(NavigationViewDisplayMode displayMode)
		{
			bool flag = this.AlwaysShowHeader || (!this.IsTopNavigationView() && displayMode == NavigationViewDisplayMode.Minimal);
			if (SharedHelpers.IsRS5OrHigher())
			{
				flag = (this.Header != null && flag);
			}
			VisualStateManager.GoToState(this, flag ? "HeaderVisible" : "HeaderCollapsed", false);
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0000F8B4 File Offset: 0x0000DAB4
		private void UpdatePaneTabFocusNavigation()
		{
			if (!this.m_appliedTemplate)
			{
				return;
			}
			if (SharedHelpers.IsRS2OrHigher())
			{
				KeyboardNavigationMode keyboardNavigationMode = 5;
				SplitView rootSplitView = this.m_rootSplitView;
				if (rootSplitView != null && this.IsPaneOpen && (rootSplitView.DisplayMode == SplitViewDisplayMode.Overlay || rootSplitView.DisplayMode == SplitViewDisplayMode.CompactOverlay))
				{
					keyboardNavigationMode = 2;
				}
				UIElement paneContentGrid = this.m_paneContentGrid;
				if (paneContentGrid != null)
				{
					KeyboardNavigation.SetTabNavigation(paneContentGrid, keyboardNavigationMode);
				}
			}
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0000F90C File Offset: 0x0000DB0C
		private void UpdatePaneToggleSize()
		{
			if (!this.ShouldPreserveNavigationViewRS3Behavior())
			{
				SplitView rootSplitView = this.m_rootSplitView;
				if (rootSplitView != null)
				{
					double num = this.GetPaneToggleButtonWidth();
					double width = num;
					if (this.ShouldShowBackButton() && rootSplitView.DisplayMode == SplitViewDisplayMode.Overlay)
					{
						double num2 = 40.0;
						Button backButton = this.m_backButton;
						if (backButton != null)
						{
							num2 = backButton.Width;
						}
						num += num2;
					}
					if (!this.m_isClosedCompact)
					{
						string paneTitle = this.PaneTitle;
						if (paneTitle != null && paneTitle.Length > 0)
						{
							if (rootSplitView.DisplayMode == SplitViewDisplayMode.Overlay && this.IsPaneOpen)
							{
								num = this.OpenPaneLength;
								width = this.OpenPaneLength - (double)((this.ShouldShowBackButton() || this.ShouldShowCloseButton()) ? 40 : 0);
							}
							else if (rootSplitView.DisplayMode != SplitViewDisplayMode.Overlay || this.IsPaneOpen)
							{
								num = this.OpenPaneLength;
								width = this.OpenPaneLength;
							}
						}
					}
					Button paneToggleButton = this.m_paneToggleButton;
					if (paneToggleButton != null)
					{
						paneToggleButton.Width = width;
					}
				}
			}
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0000F9F4 File Offset: 0x0000DBF4
		private void UpdateBackAndCloseButtonsVisibility()
		{
			if (!this.m_appliedTemplate)
			{
				return;
			}
			bool flag = this.ShouldShowBackButton();
			Visibility visibility = Util.VisibilityFromBool(flag);
			NavigationViewVisualStateDisplayMode visualStateDisplayMode = this.GetVisualStateDisplayMode(this.DisplayMode);
			bool flag2 = (visualStateDisplayMode == NavigationViewVisualStateDisplayMode.Minimal && !this.IsTopNavigationView()) || visualStateDisplayMode == NavigationViewVisualStateDisplayMode.MinimalWithBackButton;
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			double num4 = 0.0;
			this.GetTemplateSettings().BackButtonVisibility = visibility;
			if (this.m_paneToggleButton != null && this.IsPaneToggleButtonVisible)
			{
				num4 = this.GetPaneToggleButtonHeight();
				num2 = this.GetPaneToggleButtonWidth();
				if (flag2)
				{
					num = num2;
				}
			}
			Button backButton = this.m_backButton;
			if (backButton != null)
			{
				if (this.ShouldPreserveNavigationViewRS4Behavior())
				{
					backButton.Visibility = visibility;
				}
				if (flag2 && visibility == null)
				{
					num += backButton.Width;
				}
			}
			Button closeButton = this.m_closeButton;
			if (closeButton != null)
			{
				Visibility visibility2 = Util.VisibilityFromBool(this.ShouldShowCloseButton());
				closeButton.Visibility = visibility2;
				if (visibility2 == null)
				{
					num4 = Math.Max(num4, closeButton.Height);
					if (flag2)
					{
						num3 = closeButton.Width;
						num += num3;
					}
				}
			}
			FrameworkElement contentLeftPadding = this.m_contentLeftPadding;
			if (contentLeftPadding != null)
			{
				contentLeftPadding.Width = num;
			}
			ColumnDefinition paneHeaderToggleButtonColumn = this.m_paneHeaderToggleButtonColumn;
			if (paneHeaderToggleButtonColumn != null)
			{
				paneHeaderToggleButtonColumn.Width = GridLengthHelper.FromValueAndType(num2, 1);
			}
			ColumnDefinition paneHeaderCloseButtonColumn = this.m_paneHeaderCloseButtonColumn;
			if (paneHeaderCloseButtonColumn != null)
			{
				paneHeaderCloseButtonColumn.Width = GridLengthHelper.FromValueAndType(num3, 1);
			}
			FrameworkElement paneTitleHolderFrameworkElement = this.m_paneTitleHolderFrameworkElement;
			if (paneTitleHolderFrameworkElement != null && num4 == 0.0 && paneTitleHolderFrameworkElement.Visibility == null)
			{
				num4 = paneTitleHolderFrameworkElement.ActualHeight;
			}
			RowDefinition paneHeaderContentBorderRow = this.m_paneHeaderContentBorderRow;
			if (paneHeaderContentBorderRow != null)
			{
				paneHeaderContentBorderRow.MinHeight = num4;
			}
			UIElement paneContentGrid = this.m_paneContentGrid;
			if (paneContentGrid != null)
			{
				Grid grid = paneContentGrid as Grid;
				if (grid != null)
				{
					RowDefinitionCollection rowDefinitions = grid.RowDefinitions;
					if (rowDefinitions.Count >= 1)
					{
						RowDefinition rowDefinition = rowDefinitions[1];
						int num5 = 0;
						if (!this.IsOverlay() && flag)
						{
							num5 = 40;
						}
						else if (this.ShouldPreserveNavigationViewRS3Behavior())
						{
							num5 = 56;
						}
						GridLength height = GridLengthHelper.FromPixels((double)num5);
						rowDefinition.Height = height;
					}
				}
			}
			if (!this.ShouldPreserveNavigationViewRS4Behavior())
			{
				VisualStateManager.GoToState(this, flag ? "BackButtonVisible" : "BackButtonCollapsed", false);
			}
			this.UpdateTitleBarPadding();
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0000FC28 File Offset: 0x0000DE28
		private void UpdatePaneTitleMargins()
		{
			if (this.ShouldPreserveNavigationViewRS4Behavior())
			{
				FrameworkElement paneTitleFrameworkElement = this.m_paneTitleFrameworkElement;
				if (paneTitleFrameworkElement != null)
				{
					double num = this.GetPaneToggleButtonWidth();
					if (this.ShouldShowBackButton() && this.IsOverlay())
					{
						num += 40.0;
					}
					paneTitleFrameworkElement.Margin = new Thickness(num, 0.0, 0.0, 0.0);
				}
			}
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0000FC94 File Offset: 0x0000DE94
		private void UpdateSelectionForMenuItems()
		{
			if (this.SelectedItem == null)
			{
				bool foundFirstSelected = false;
				IList menuItems = this.MenuItems;
				if (menuItems != null)
				{
					foundFirstSelected = this.UpdateSelectedItemFromMenuItems(menuItems, false);
				}
				IList footerMenuItems = this.FooterMenuItems;
				if (footerMenuItems != null)
				{
					this.UpdateSelectedItemFromMenuItems(footerMenuItems, foundFirstSelected);
				}
			}
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0000FCD4 File Offset: 0x0000DED4
		private bool UpdateSelectedItemFromMenuItems(IList menuItems, bool foundFirstSelected = false)
		{
			for (int i = 0; i < menuItems.Count; i++)
			{
				NavigationViewItem navigationViewItem = menuItems[i] as NavigationViewItem;
				if (navigationViewItem != null && navigationViewItem.IsSelected)
				{
					if (!foundFirstSelected)
					{
						try
						{
							this.m_shouldIgnoreNextSelectionChange = true;
							this.SelectedItem = navigationViewItem;
							foundFirstSelected = true;
							goto IL_41;
						}
						finally
						{
							this.m_shouldIgnoreNextSelectionChange = false;
						}
					}
					navigationViewItem.IsSelected = false;
				}
				IL_41:;
			}
			return foundFirstSelected;
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0000FD40 File Offset: 0x0000DF40
		private void OnTitleBarMetricsChanged(object sender, object args)
		{
			this.UpdateTitleBarPadding();
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0000FD48 File Offset: 0x0000DF48
		private void OnTitleBarIsVisibleChanged(CoreApplicationViewTitleBar sender, object args)
		{
			this.UpdateTitleBarPadding();
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0000FD50 File Offset: 0x0000DF50
		private void ClosePaneIfNeccessaryAfterItemIsClicked(NavigationViewItem selectedContainer)
		{
			if (this.IsPaneOpen && this.DisplayMode != NavigationViewDisplayMode.Expanded && !this.DoesNavigationViewItemHaveChildren(selectedContainer) && !this.m_shouldIgnoreNextSelectionChange)
			{
				this.ClosePane();
			}
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0000FD7A File Offset: 0x0000DF7A
		private bool NeedTopPaddingForRS5OrHigher(CoreApplicationViewTitleBar coreTitleBar)
		{
			return coreTitleBar.IsVisible && coreTitleBar.ExtendViewIntoTitleBar && !this.IsFullScreenOrTabletMode();
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0000FD98 File Offset: 0x0000DF98
		private void UpdateTitleBarPadding()
		{
			if (!this.m_appliedTemplate)
			{
				return;
			}
			double num = 0.0;
			CoreApplicationViewTitleBar coreTitleBar = this.m_coreTitleBar;
			if (coreTitleBar != null)
			{
				bool flag = false;
				if (this.IsTitleBarAutoPaddingEnabled)
				{
					if (this.ShouldPreserveNavigationViewRS3Behavior())
					{
						flag = true;
					}
					else if (this.ShouldPreserveNavigationViewRS4Behavior())
					{
						flag = !coreTitleBar.ExtendViewIntoTitleBar;
					}
					else
					{
						flag = this.NeedTopPaddingForRS5OrHigher(coreTitleBar);
					}
				}
				if (flag)
				{
					UIElement uielement = (Window.GetWindow(this) ?? Application.Current.MainWindow).Content as UIElement;
					if (Extensions.SafeTransformToVisual(this, uielement).Transform(default(Point)).Y == 0.0)
					{
						num = coreTitleBar.Height;
					}
				}
				if (this.ShouldPreserveNavigationViewRS4Behavior())
				{
					FrameworkElement togglePaneTopPadding = this.m_togglePaneTopPadding;
					if (togglePaneTopPadding != null)
					{
						togglePaneTopPadding.Height = num;
					}
					FrameworkElement contentPaneTopPadding = this.m_contentPaneTopPadding;
					if (contentPaneTopPadding != null)
					{
						contentPaneTopPadding.Height = num;
					}
				}
				FrameworkElement paneTitleHolderFrameworkElement = this.m_paneTitleHolderFrameworkElement;
				Button paneToggleButton = this.m_paneToggleButton;
				bool flag2 = paneTitleHolderFrameworkElement != null && paneTitleHolderFrameworkElement.Visibility == 0;
				bool flag3 = !flag2 && paneToggleButton != null && paneToggleButton.Visibility == 0;
				if (flag2 || flag3)
				{
					Thickness margin = ThicknessHelper.FromLengths(0.0, 0.0, 0.0, 0.0);
					if (this.ShouldShowBackButton())
					{
						if (this.IsOverlay())
						{
							margin = ThicknessHelper.FromLengths(40.0, 0.0, 0.0, 0.0);
						}
						else
						{
							margin = ThicknessHelper.FromLengths(0.0, 40.0, 0.0, 0.0);
						}
					}
					else if (this.ShouldShowCloseButton() && this.IsOverlay())
					{
						margin = ThicknessHelper.FromLengths(40.0, 0.0, 0.0, 0.0);
					}
					if (flag2)
					{
						paneTitleHolderFrameworkElement.Margin = margin;
					}
					else
					{
						paneToggleButton.Margin = margin;
					}
				}
			}
			NavigationViewTemplateSettings templateSettings = this.TemplateSettings;
			if (templateSettings != null && Math.Abs(templateSettings.TopPadding - num) > 0.1)
			{
				this.GetTemplateSettings().TopPadding = num;
			}
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0000FFD8 File Offset: 0x0000E1D8
		private void RaiseDisplayModeChanged(NavigationViewDisplayMode displayMode)
		{
			base.SetValue(NavigationView.DisplayModePropertyKey, displayMode);
			NavigationViewDisplayModeChangedEventArgs navigationViewDisplayModeChangedEventArgs = new NavigationViewDisplayModeChangedEventArgs();
			navigationViewDisplayModeChangedEventArgs.DisplayMode = displayMode;
			TypedEventHandler<NavigationView, NavigationViewDisplayModeChangedEventArgs> displayModeChanged = this.DisplayModeChanged;
			if (displayModeChanged == null)
			{
				return;
			}
			displayModeChanged.Invoke(this, navigationViewDisplayModeChangedEventArgs);
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x00010015 File Offset: 0x0000E215
		private void CreateAndAttachHeaderAnimation(Visual visual)
		{
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x00010017 File Offset: 0x0000E217
		private bool IsFullScreenOrTabletMode()
		{
			return false;
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0001001A File Offset: 0x0000E21A
		private void UpdatePaneShadow()
		{
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0001001C File Offset: 0x0000E21C
		private T GetContainerForData<T>(object data) where T : class
		{
			if (data == null)
			{
				return default(T);
			}
			T t = data as T;
			if (t != null)
			{
				return t;
			}
			NavigationViewItem settingsItem = this.m_settingsItem;
			if (settingsItem != null && (settingsItem == data || settingsItem.Content == data))
			{
				return settingsItem as T;
			}
			ItemsRepeater itemsRepeater = this.IsTopNavigationView() ? this.m_topNavRepeater : this.m_leftNavRepeater;
			int indexFromItem = this.GetIndexFromItem(itemsRepeater, data);
			if (indexFromItem >= 0)
			{
				UIElement uielement = itemsRepeater.TryGetElement(indexFromItem);
				if (uielement != null)
				{
					return uielement as T;
				}
			}
			ItemsRepeater itemsRepeater2 = this.IsTopNavigationView() ? this.m_topNavFooterMenuRepeater : this.m_leftNavFooterMenuRepeater;
			indexFromItem = this.GetIndexFromItem(itemsRepeater2, data);
			if (indexFromItem >= 0)
			{
				UIElement uielement2 = itemsRepeater2.TryGetElement(indexFromItem);
				if (uielement2 != null)
				{
					return uielement2 as T;
				}
			}
			UIElement uielement3 = this.SearchEntireTreeForContainer(itemsRepeater, data);
			if (uielement3 != null)
			{
				return uielement3 as T;
			}
			UIElement uielement4 = this.SearchEntireTreeForContainer(itemsRepeater2, data);
			if (uielement4 != null)
			{
				return uielement4 as T;
			}
			return default(T);
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x00010130 File Offset: 0x0000E330
		private UIElement SearchEntireTreeForContainer(ItemsRepeater rootRepeater, object data)
		{
			int indexFromItem = this.GetIndexFromItem(rootRepeater, data);
			if (indexFromItem != -1)
			{
				return rootRepeater.TryGetElement(indexFromItem);
			}
			for (int i = 0; i < this.GetContainerCountInRepeater(rootRepeater); i++)
			{
				UIElement uielement = rootRepeater.TryGetElement(i);
				if (uielement != null)
				{
					NavigationViewItem navigationViewItem = uielement as NavigationViewItem;
					if (navigationViewItem != null)
					{
						ItemsRepeater repeater = navigationViewItem.GetRepeater();
						if (repeater != null)
						{
							UIElement uielement2 = this.SearchEntireTreeForContainer(repeater, data);
							if (uielement2 != null)
							{
								return uielement2;
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x00010198 File Offset: 0x0000E398
		private IndexPath SearchEntireTreeForIndexPath(ItemsRepeater rootRepeater, object data, bool isFooterRepeater)
		{
			for (int i = 0; i < this.GetContainerCountInRepeater(rootRepeater); i++)
			{
				UIElement uielement = rootRepeater.TryGetElement(i);
				if (uielement != null)
				{
					NavigationViewItem navigationViewItem = uielement as NavigationViewItem;
					if (navigationViewItem != null)
					{
						List<int> list = new List<int>();
						list.Add(isFooterRepeater ? 1 : 0);
						list.Add(i);
						IndexPath ip = new IndexPath(list);
						IndexPath indexPath = this.SearchEntireTreeForIndexPath(navigationViewItem, data, ip);
						if (indexPath != null)
						{
							return indexPath;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x00010200 File Offset: 0x0000E400
		private IndexPath SearchEntireTreeForIndexPath(NavigationViewItem parentContainer, object data, IndexPath ip)
		{
			bool flag = false;
			ItemsRepeater repeater = parentContainer.GetRepeater();
			if (repeater != null && this.DoesRepeaterHaveRealizedContainers(repeater))
			{
				flag = true;
				for (int i = 0; i < this.GetContainerCountInRepeater(repeater); i++)
				{
					UIElement uielement = repeater.TryGetElement(i);
					if (uielement != null)
					{
						NavigationViewItem navigationViewItem = uielement as NavigationViewItem;
						if (navigationViewItem != null)
						{
							IndexPath indexPath = ip.CloneWithChildIndex(i);
							if (navigationViewItem.Content == data)
							{
								return indexPath;
							}
							IndexPath indexPath2 = this.SearchEntireTreeForIndexPath(navigationViewItem, data, indexPath);
							if (indexPath2 != null)
							{
								return indexPath2;
							}
						}
					}
				}
			}
			if (!flag)
			{
				object children = this.GetChildren(parentContainer);
				if (children != null)
				{
					ItemsSourceView itemsSourceView = children as ItemsSourceView;
					if (children != null && itemsSourceView == null)
					{
						itemsSourceView = new InspectingDataSource(children);
					}
					for (int j = 0; j < itemsSourceView.Count; j++)
					{
						IndexPath indexPath3 = ip.CloneWithChildIndex(j);
						object at = itemsSourceView.GetAt(j);
						if (at == data)
						{
							return indexPath3;
						}
						NavigationViewItemBase navigationViewItemBase = this.ResolveContainerForItem(at, j);
						if (navigationViewItemBase != null)
						{
							NavigationViewItem navigationViewItem2 = navigationViewItemBase as NavigationViewItem;
							if (navigationViewItem2 != null)
							{
								IndexPath indexPath4 = this.SearchEntireTreeForIndexPath(navigationViewItem2, data, indexPath3);
								if (indexPath4 != null)
								{
									return indexPath4;
								}
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0001030C File Offset: 0x0000E50C
		private NavigationViewItemBase ResolveContainerForItem(object item, int index)
		{
			ElementFactoryGetArgs elementFactoryGetArgs = new ElementFactoryGetArgs();
			elementFactoryGetArgs.Data = item;
			elementFactoryGetArgs.Index = index;
			UIElement element = this.m_navigationViewItemsFactory.GetElement(elementFactoryGetArgs);
			if (element != null)
			{
				NavigationViewItemBase navigationViewItemBase = element as NavigationViewItemBase;
				if (navigationViewItemBase != null)
				{
					return navigationViewItemBase;
				}
			}
			return null;
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0001034C File Offset: 0x0000E54C
		private void RecycleContainer(UIElement container)
		{
			ElementFactoryRecycleArgs elementFactoryRecycleArgs = new ElementFactoryRecycleArgs();
			elementFactoryRecycleArgs.Element = container;
			this.m_navigationViewItemsFactory.RecycleElement(elementFactoryRecycleArgs);
		}

		// Token: 0x060003DC RID: 988 RVA: 0x00010374 File Offset: 0x0000E574
		private int GetContainerCountInRepeater(ItemsRepeater ir)
		{
			if (ir != null)
			{
				ItemsSourceView itemsSourceView = ir.ItemsSourceView;
				if (itemsSourceView != null)
				{
					return itemsSourceView.Count;
				}
			}
			return -1;
		}

		// Token: 0x060003DD RID: 989 RVA: 0x00010396 File Offset: 0x0000E596
		private bool DoesRepeaterHaveRealizedContainers(ItemsRepeater ir)
		{
			return ir != null && ir.TryGetElement(0) != null;
		}

		// Token: 0x060003DE RID: 990 RVA: 0x000103A8 File Offset: 0x0000E5A8
		private int GetIndexFromItem(ItemsRepeater ir, object data)
		{
			if (ir != null)
			{
				ItemsSourceView itemsSourceView = ir.ItemsSourceView;
				if (itemsSourceView != null)
				{
					return itemsSourceView.IndexOf(data);
				}
			}
			return -1;
		}

		// Token: 0x060003DF RID: 991 RVA: 0x000103CC File Offset: 0x0000E5CC
		private object GetItemFromIndex(ItemsRepeater ir, int index)
		{
			if (ir != null)
			{
				ItemsSourceView itemsSourceView = ir.ItemsSourceView;
				if (itemsSourceView != null)
				{
					return itemsSourceView.GetAt(index);
				}
			}
			return null;
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x000103F0 File Offset: 0x0000E5F0
		private IndexPath GetIndexPathOfItem(object data)
		{
			NavigationViewItemBase navigationViewItemBase = data as NavigationViewItemBase;
			if (navigationViewItemBase != null)
			{
				return this.GetIndexPathForContainer(navigationViewItemBase);
			}
			if (this.IsTopNavigationView())
			{
				IndexPath indexPath = this.SearchEntireTreeForIndexPath(this.m_topNavRepeater, data, false);
				if (indexPath != null)
				{
					return indexPath;
				}
				IndexPath indexPath2 = this.SearchEntireTreeForIndexPath(this.m_topNavRepeaterOverflowView, data, false);
				if (indexPath2 != null)
				{
					return indexPath2;
				}
				IndexPath indexPath3 = this.SearchEntireTreeForIndexPath(this.m_topNavFooterMenuRepeater, data, true);
				if (indexPath3 != null)
				{
					return indexPath3;
				}
			}
			else
			{
				IndexPath indexPath4 = this.SearchEntireTreeForIndexPath(this.m_leftNavRepeater, data, false);
				if (indexPath4 != null)
				{
					return indexPath4;
				}
				IndexPath indexPath5 = this.SearchEntireTreeForIndexPath(this.m_leftNavFooterMenuRepeater, data, true);
				if (indexPath5 != null)
				{
					return indexPath5;
				}
			}
			return new IndexPath(new List<int>(0));
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0001048C File Offset: 0x0000E68C
		private UIElement GetContainerForIndex(int index, bool inFooter)
		{
			if (this.IsTopNavigationView())
			{
				ItemsRepeater itemsRepeater = inFooter ? this.m_topNavFooterMenuRepeater : (this.m_topDataProvider.IsItemInPrimaryList(index) ? this.m_topNavRepeater : this.m_topNavRepeaterOverflowView);
				int index2 = inFooter ? index : this.m_topDataProvider.ConvertOriginalIndexToIndex(index);
				UIElement uielement = itemsRepeater.TryGetElement(index2);
				if (uielement != null)
				{
					return uielement;
				}
			}
			else
			{
				UIElement uielement2 = inFooter ? this.m_leftNavFooterMenuRepeater.TryGetElement(index) : this.m_leftNavRepeater.TryGetElement(index);
				if (uielement2 != null)
				{
					return uielement2 as NavigationViewItemBase;
				}
			}
			return null;
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x00010510 File Offset: 0x0000E710
		private NavigationViewItemBase GetContainerForIndexPath(IndexPath ip, bool lastVisible = false)
		{
			if (ip != null && ip.GetSize() > 0)
			{
				UIElement containerForIndex = this.GetContainerForIndex(ip.GetAt(1), ip.GetAt(0) == 1);
				if (containerForIndex != null)
				{
					if (lastVisible)
					{
						NavigationViewItem navigationViewItem = containerForIndex as NavigationViewItem;
						if (navigationViewItem != null && !navigationViewItem.IsExpanded)
						{
							return navigationViewItem;
						}
					}
					return this.GetContainerForIndexPath(containerForIndex, ip, lastVisible);
				}
			}
			return null;
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x00010568 File Offset: 0x0000E768
		private NavigationViewItemBase GetContainerForIndexPath(UIElement firstContainer, IndexPath ip, bool lastVisible)
		{
			UIElement uielement = firstContainer;
			if (ip.GetSize() > 2)
			{
				for (int i = 2; i < ip.GetSize(); i++)
				{
					bool flag = false;
					NavigationViewItem navigationViewItem = uielement as NavigationViewItem;
					if (navigationViewItem != null)
					{
						if (lastVisible && !navigationViewItem.IsExpanded)
						{
							return navigationViewItem;
						}
						ItemsRepeater repeater = navigationViewItem.GetRepeater();
						if (repeater != null)
						{
							UIElement uielement2 = repeater.TryGetElement(ip.GetAt(i));
							if (uielement2 != null)
							{
								uielement = uielement2;
								flag = true;
							}
						}
					}
					if (!flag)
					{
						return null;
					}
				}
			}
			return uielement as NavigationViewItemBase;
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x000105DC File Offset: 0x0000E7DC
		private bool IsContainerTheSelectedItemInTheSelectionModel(NavigationViewItemBase nvib)
		{
			object selectedItem = this.m_selectionModel.SelectedItem;
			if (selectedItem != null)
			{
				NavigationViewItemBase navigationViewItemBase = selectedItem as NavigationViewItemBase;
				if (navigationViewItemBase == null)
				{
					navigationViewItemBase = this.GetContainerForIndexPath(this.m_selectionModel.SelectedIndex, false);
				}
				return navigationViewItemBase == nvib;
			}
			return false;
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0001061B File Offset: 0x0000E81B
		internal ItemsRepeater LeftNavRepeater()
		{
			return this.m_leftNavRepeater;
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x00010624 File Offset: 0x0000E824
		internal NavigationViewItem GetSelectedContainer()
		{
			object selectedItem = this.SelectedItem;
			if (selectedItem == null)
			{
				return null;
			}
			NavigationViewItem navigationViewItem = selectedItem as NavigationViewItem;
			if (navigationViewItem != null)
			{
				return navigationViewItem;
			}
			return this.NavigationViewItemOrSettingsContentFromData(selectedItem);
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x00010650 File Offset: 0x0000E850
		internal void Expand(NavigationViewItem item)
		{
			this.ChangeIsExpandedNavigationViewItem(item, true);
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0001065A File Offset: 0x0000E85A
		internal void Collapse(NavigationViewItem item)
		{
			this.ChangeIsExpandedNavigationViewItem(item, false);
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00010664 File Offset: 0x0000E864
		private bool DoesNavigationViewItemHaveChildren(NavigationViewItem nvi)
		{
			return nvi.MenuItems.Count > 0 || nvi.MenuItemsSource != null || nvi.HasUnrealizedChildren;
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x00010684 File Offset: 0x0000E884
		private void ToggleIsExpandedNavigationViewItem(NavigationViewItem nvi)
		{
			this.ChangeIsExpandedNavigationViewItem(nvi, !nvi.IsExpanded);
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x00010696 File Offset: 0x0000E896
		private void ChangeIsExpandedNavigationViewItem(NavigationViewItem nvi, bool isExpanded)
		{
			if (this.DoesNavigationViewItemHaveChildren(nvi))
			{
				nvi.IsExpanded = isExpanded;
			}
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x000106A8 File Offset: 0x0000E8A8
		private NavigationViewItem FindLowestLevelContainerToDisplaySelectionIndicator()
		{
			int num = 1;
			IndexPath selectedIndex = this.m_selectionModel.SelectedIndex;
			if (selectedIndex != null && selectedIndex.GetSize() > 1)
			{
				UIElement containerForIndex = this.GetContainerForIndex(selectedIndex.GetAt(num), selectedIndex.GetAt(0) == 1);
				if (containerForIndex != null)
				{
					NavigationViewItem navigationViewItem = containerForIndex as NavigationViewItem;
					if (navigationViewItem != null)
					{
						NavigationViewItem navigationViewItem2 = navigationViewItem;
						bool flag = navigationViewItem2.IsRepeaterVisible();
						while (navigationViewItem != null && flag && !navigationViewItem.IsSelected && navigationViewItem.IsChildSelected)
						{
							num++;
							flag = false;
							ItemsRepeater repeater = navigationViewItem2.GetRepeater();
							if (repeater != null)
							{
								UIElement uielement = repeater.TryGetElement(selectedIndex.GetAt(num));
								if (uielement != null)
								{
									navigationViewItem = (uielement as NavigationViewItem);
									navigationViewItem2 = navigationViewItem;
									flag = navigationViewItem2.IsRepeaterVisible();
								}
							}
						}
						return navigationViewItem;
					}
				}
			}
			return null;
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x00010760 File Offset: 0x0000E960
		private void ShowHideChildrenItemsRepeater(NavigationViewItem nvi)
		{
			nvi.ShowHideChildren();
			if (nvi.ShouldRepeaterShowInFlyout())
			{
				if (nvi.IsExpanded)
				{
					this.m_lastItemExpandedIntoFlyout = nvi;
				}
				else
				{
					this.m_lastItemExpandedIntoFlyout = null;
				}
			}
			if (!nvi.IsSelected && nvi.IsChildSelected)
			{
				if (!nvi.IsRepeaterVisible() && nvi.IsChildSelected)
				{
					this.AnimateSelectionChanged(nvi);
				}
				else
				{
					this.AnimateSelectionChanged(this.FindLowestLevelContainerToDisplaySelectionIndicator());
				}
			}
			nvi.RotateExpandCollapseChevron(nvi.IsExpanded);
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x000107D6 File Offset: 0x0000E9D6
		private object GetChildren(NavigationViewItem nvi)
		{
			if (nvi.MenuItems.Count > 0)
			{
				return nvi.MenuItems;
			}
			return nvi.MenuItemsSource;
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x000107F4 File Offset: 0x0000E9F4
		private ItemsRepeater GetChildRepeaterForIndexPath(IndexPath ip)
		{
			NavigationViewItem navigationViewItem = this.GetContainerForIndexPath(ip, false) as NavigationViewItem;
			if (navigationViewItem != null)
			{
				return navigationViewItem.GetRepeater();
			}
			return null;
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0001081C File Offset: 0x0000EA1C
		private object GetChildrenForItemInIndexPath(IndexPath ip, bool forceRealize = false)
		{
			if (ip != null && ip.GetSize() > 1)
			{
				UIElement containerForIndex = this.GetContainerForIndex(ip.GetAt(1), ip.GetAt(0) == 1);
				if (containerForIndex != null)
				{
					return this.GetChildrenForItemInIndexPath(containerForIndex, ip, forceRealize);
				}
			}
			return null;
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0001085C File Offset: 0x0000EA5C
		private object GetChildrenForItemInIndexPath(UIElement firstContainer, IndexPath ip, bool forceRealize = false)
		{
			UIElement uielement = firstContainer;
			bool flag = false;
			if (ip.GetSize() > 2)
			{
				for (int i = 2; i < ip.GetSize(); i++)
				{
					bool flag2 = false;
					NavigationViewItem navigationViewItem = uielement as NavigationViewItem;
					if (navigationViewItem != null)
					{
						int at = ip.GetAt(i);
						ItemsRepeater repeater = navigationViewItem.GetRepeater();
						if (repeater != null && this.DoesRepeaterHaveRealizedContainers(repeater))
						{
							UIElement uielement2 = repeater.TryGetElement(at);
							if (uielement2 != null)
							{
								uielement = uielement2;
								flag2 = true;
							}
						}
						else if (forceRealize)
						{
							object children = this.GetChildren(navigationViewItem);
							if (children != null)
							{
								if (flag)
								{
									this.RecycleContainer(navigationViewItem);
									flag = false;
								}
								ItemsSourceView itemsSourceView = children as ItemsSourceView;
								if (children != null && itemsSourceView == null)
								{
									itemsSourceView = new InspectingDataSource(children);
								}
								object at2 = itemsSourceView.GetAt(at);
								if (at2 != null)
								{
									NavigationViewItemBase navigationViewItemBase = this.ResolveContainerForItem(at2, at);
									if (navigationViewItemBase != null)
									{
										NavigationViewItem navigationViewItem2 = navigationViewItemBase as NavigationViewItem;
										if (navigationViewItem2 != null)
										{
											uielement = navigationViewItem2;
											flag = true;
											flag2 = true;
										}
									}
								}
							}
						}
					}
					if (!flag2)
					{
						return null;
					}
				}
			}
			NavigationViewItem navigationViewItem3 = uielement as NavigationViewItem;
			if (navigationViewItem3 != null)
			{
				object children2 = this.GetChildren(navigationViewItem3);
				if (flag)
				{
					this.RecycleContainer(navigationViewItem3);
				}
				return children2;
			}
			return null;
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x00010968 File Offset: 0x0000EB68
		private void CollapseTopLevelMenuItems(NavigationViewPaneDisplayMode oldDisplayMode)
		{
			if (oldDisplayMode == NavigationViewPaneDisplayMode.Top)
			{
				this.CollapseMenuItemsInRepeater(this.m_topNavRepeater);
				this.CollapseMenuItemsInRepeater(this.m_topNavRepeaterOverflowView);
				return;
			}
			this.CollapseMenuItemsInRepeater(this.m_leftNavRepeater);
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x00010994 File Offset: 0x0000EB94
		private void CollapseMenuItemsInRepeater(ItemsRepeater ir)
		{
			for (int i = 0; i < this.GetContainerCountInRepeater(ir); i++)
			{
				UIElement uielement = ir.TryGetElement(i);
				if (uielement != null)
				{
					NavigationViewItem navigationViewItem = uielement as NavigationViewItem;
					if (navigationViewItem != null)
					{
						this.ChangeIsExpandedNavigationViewItem(navigationViewItem, false);
					}
				}
			}
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x000109D0 File Offset: 0x0000EBD0
		private void RaiseExpandingEvent(NavigationViewItemBase container)
		{
			NavigationViewItemExpandingEventArgs navigationViewItemExpandingEventArgs = new NavigationViewItemExpandingEventArgs(this);
			navigationViewItemExpandingEventArgs.ExpandingItemContainer = container;
			TypedEventHandler<NavigationView, NavigationViewItemExpandingEventArgs> expanding = this.Expanding;
			if (expanding == null)
			{
				return;
			}
			expanding.Invoke(this, navigationViewItemExpandingEventArgs);
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x00010A00 File Offset: 0x0000EC00
		private void RaiseCollapsedEvent(NavigationViewItemBase container)
		{
			NavigationViewItemCollapsedEventArgs navigationViewItemCollapsedEventArgs = new NavigationViewItemCollapsedEventArgs(this);
			navigationViewItemCollapsedEventArgs.CollapsedItemContainer = container;
			TypedEventHandler<NavigationView, NavigationViewItemCollapsedEventArgs> collapsed = this.Collapsed;
			if (collapsed == null)
			{
				return;
			}
			collapsed.Invoke(this, navigationViewItemCollapsedEventArgs);
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x00010A2D File Offset: 0x0000EC2D
		private bool IsTopLevelItem(NavigationViewItemBase nvib)
		{
			return this.IsRootItemsRepeater(this.GetParentItemsRepeaterForContainer(nvib));
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x00010A3C File Offset: 0x0000EC3C
		DependencyObject IControlProtected.GetTemplateChild(string childName)
		{
			return base.GetTemplateChild(childName);
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x00010A45 File Offset: 0x0000EC45
		protected override void OnDpiChanged(DpiScale oldDpi, DpiScale newDpi)
		{
			base.OnDpiChanged(oldDpi, newDpi);
			this.m_bitmapCache.RenderAtScale = newDpi.PixelsPerDip;
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060003F9 RID: 1017 RVA: 0x00010A61 File Offset: 0x0000EC61
		// (set) Token: 0x060003FA RID: 1018 RVA: 0x00010A73 File Offset: 0x0000EC73
		public bool IsPaneOpen
		{
			get
			{
				return (bool)base.GetValue(NavigationView.IsPaneOpenProperty);
			}
			set
			{
				base.SetValue(NavigationView.IsPaneOpenProperty, value);
			}
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x00010A86 File Offset: 0x0000EC86
		private static void OnIsPaneOpenPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NavigationView)sender).PropertyChanged(args);
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060003FC RID: 1020 RVA: 0x00010A94 File Offset: 0x0000EC94
		// (set) Token: 0x060003FD RID: 1021 RVA: 0x00010AA6 File Offset: 0x0000ECA6
		public double CompactModeThresholdWidth
		{
			get
			{
				return (double)base.GetValue(NavigationView.CompactModeThresholdWidthProperty);
			}
			set
			{
				base.SetValue(NavigationView.CompactModeThresholdWidthProperty, value);
			}
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x00010AB9 File Offset: 0x0000ECB9
		private static void OnCompactModeThresholdWidthPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NavigationView)sender).PropertyChanged(args);
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060003FF RID: 1023 RVA: 0x00010AC7 File Offset: 0x0000ECC7
		// (set) Token: 0x06000400 RID: 1024 RVA: 0x00010AD9 File Offset: 0x0000ECD9
		public double ExpandedModeThresholdWidth
		{
			get
			{
				return (double)base.GetValue(NavigationView.ExpandedModeThresholdWidthProperty);
			}
			set
			{
				base.SetValue(NavigationView.ExpandedModeThresholdWidthProperty, value);
			}
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x00010AEC File Offset: 0x0000ECEC
		private static void OnExpandedModeThresholdWidthPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NavigationView)sender).PropertyChanged(args);
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000402 RID: 1026 RVA: 0x00010AFA File Offset: 0x0000ECFA
		public IList FooterMenuItems
		{
			get
			{
				return (IList)base.GetValue(NavigationView.FooterMenuItemsProperty);
			}
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x00010B0C File Offset: 0x0000ED0C
		private static void OnFooterMenuItemsPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NavigationView)sender).PropertyChanged(args);
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000404 RID: 1028 RVA: 0x00010B1A File Offset: 0x0000ED1A
		// (set) Token: 0x06000405 RID: 1029 RVA: 0x00010B27 File Offset: 0x0000ED27
		public object FooterMenuItemsSource
		{
			get
			{
				return base.GetValue(NavigationView.FooterMenuItemsSourceProperty);
			}
			set
			{
				base.SetValue(NavigationView.FooterMenuItemsSourceProperty, value);
			}
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x00010B35 File Offset: 0x0000ED35
		private static void OnFooterMenuItemsSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NavigationView)sender).PropertyChanged(args);
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x00010B43 File Offset: 0x0000ED43
		// (set) Token: 0x06000408 RID: 1032 RVA: 0x00010B55 File Offset: 0x0000ED55
		public UIElement PaneFooter
		{
			get
			{
				return (UIElement)base.GetValue(NavigationView.PaneFooterProperty);
			}
			set
			{
				base.SetValue(NavigationView.PaneFooterProperty, value);
			}
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x00010B63 File Offset: 0x0000ED63
		private static void OnPaneFooterPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NavigationView)sender).PropertyChanged(args);
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600040A RID: 1034 RVA: 0x00010B71 File Offset: 0x0000ED71
		// (set) Token: 0x0600040B RID: 1035 RVA: 0x00010B7E File Offset: 0x0000ED7E
		public object Header
		{
			get
			{
				return base.GetValue(NavigationView.HeaderProperty);
			}
			set
			{
				base.SetValue(NavigationView.HeaderProperty, value);
			}
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x00010B8C File Offset: 0x0000ED8C
		private static void OnHeaderPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NavigationView)sender).PropertyChanged(args);
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600040D RID: 1037 RVA: 0x00010B9A File Offset: 0x0000ED9A
		// (set) Token: 0x0600040E RID: 1038 RVA: 0x00010BAC File Offset: 0x0000EDAC
		public DataTemplate HeaderTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(NavigationView.HeaderTemplateProperty);
			}
			set
			{
				base.SetValue(NavigationView.HeaderTemplateProperty, value);
			}
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x00010BBA File Offset: 0x0000EDBA
		private static void OnHeaderTemplatePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NavigationView)sender).PropertyChanged(args);
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000410 RID: 1040 RVA: 0x00010BC8 File Offset: 0x0000EDC8
		public NavigationViewDisplayMode DisplayMode
		{
			get
			{
				return (NavigationViewDisplayMode)base.GetValue(NavigationView.DisplayModeProperty);
			}
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x00010BDA File Offset: 0x0000EDDA
		private static void OnDisplayModePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NavigationView)sender).PropertyChanged(args);
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000412 RID: 1042 RVA: 0x00010BE8 File Offset: 0x0000EDE8
		// (set) Token: 0x06000413 RID: 1043 RVA: 0x00010BFA File Offset: 0x0000EDFA
		public bool IsSettingsVisible
		{
			get
			{
				return (bool)base.GetValue(NavigationView.IsSettingsVisibleProperty);
			}
			set
			{
				base.SetValue(NavigationView.IsSettingsVisibleProperty, value);
			}
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x00010C0D File Offset: 0x0000EE0D
		private static void OnIsSettingsVisiblePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NavigationView)sender).PropertyChanged(args);
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000415 RID: 1045 RVA: 0x00010C1B File Offset: 0x0000EE1B
		// (set) Token: 0x06000416 RID: 1046 RVA: 0x00010C2D File Offset: 0x0000EE2D
		public bool IsPaneToggleButtonVisible
		{
			get
			{
				return (bool)base.GetValue(NavigationView.IsPaneToggleButtonVisibleProperty);
			}
			set
			{
				base.SetValue(NavigationView.IsPaneToggleButtonVisibleProperty, value);
			}
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x00010C40 File Offset: 0x0000EE40
		private static void OnIsPaneToggleButtonVisiblePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NavigationView)sender).PropertyChanged(args);
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000418 RID: 1048 RVA: 0x00010C4E File Offset: 0x0000EE4E
		// (set) Token: 0x06000419 RID: 1049 RVA: 0x00010C60 File Offset: 0x0000EE60
		public bool AlwaysShowHeader
		{
			get
			{
				return (bool)base.GetValue(NavigationView.AlwaysShowHeaderProperty);
			}
			set
			{
				base.SetValue(NavigationView.AlwaysShowHeaderProperty, value);
			}
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x00010C73 File Offset: 0x0000EE73
		private static void OnAlwaysShowHeaderPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NavigationView)sender).PropertyChanged(args);
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600041B RID: 1051 RVA: 0x00010C81 File Offset: 0x0000EE81
		// (set) Token: 0x0600041C RID: 1052 RVA: 0x00010C93 File Offset: 0x0000EE93
		public double CompactPaneLength
		{
			get
			{
				return (double)base.GetValue(NavigationView.CompactPaneLengthProperty);
			}
			set
			{
				base.SetValue(NavigationView.CompactPaneLengthProperty, value);
			}
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x00010CA6 File Offset: 0x0000EEA6
		private static void OnCompactPaneLengthPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NavigationView)sender).PropertyChanged(args);
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x0600041E RID: 1054 RVA: 0x00010CB4 File Offset: 0x0000EEB4
		// (set) Token: 0x0600041F RID: 1055 RVA: 0x00010CC6 File Offset: 0x0000EEC6
		public double OpenPaneLength
		{
			get
			{
				return (double)base.GetValue(NavigationView.OpenPaneLengthProperty);
			}
			set
			{
				base.SetValue(NavigationView.OpenPaneLengthProperty, value);
			}
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x00010CD9 File Offset: 0x0000EED9
		private static void OnOpenPaneLengthPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NavigationView)sender).PropertyChanged(args);
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000421 RID: 1057 RVA: 0x00010CE7 File Offset: 0x0000EEE7
		// (set) Token: 0x06000422 RID: 1058 RVA: 0x00010CF9 File Offset: 0x0000EEF9
		public Style PaneToggleButtonStyle
		{
			get
			{
				return (Style)base.GetValue(NavigationView.PaneToggleButtonStyleProperty);
			}
			set
			{
				base.SetValue(NavigationView.PaneToggleButtonStyleProperty, value);
			}
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x00010D07 File Offset: 0x0000EF07
		private static void OnPaneToggleButtonStylePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NavigationView)sender).PropertyChanged(args);
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000424 RID: 1060 RVA: 0x00010D15 File Offset: 0x0000EF15
		// (set) Token: 0x06000425 RID: 1061 RVA: 0x00010D22 File Offset: 0x0000EF22
		public object SelectedItem
		{
			get
			{
				return base.GetValue(NavigationView.SelectedItemProperty);
			}
			set
			{
				base.SetValue(NavigationView.SelectedItemProperty, value);
			}
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x00010D30 File Offset: 0x0000EF30
		private static void OnSelectedItemPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NavigationView)sender).PropertyChanged(args);
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000427 RID: 1063 RVA: 0x00010D3E File Offset: 0x0000EF3E
		public IList MenuItems
		{
			get
			{
				return (IList)base.GetValue(NavigationView.MenuItemsProperty);
			}
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x00010D50 File Offset: 0x0000EF50
		private static void OnMenuItemsPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NavigationView)sender).PropertyChanged(args);
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000429 RID: 1065 RVA: 0x00010D5E File Offset: 0x0000EF5E
		// (set) Token: 0x0600042A RID: 1066 RVA: 0x00010D6B File Offset: 0x0000EF6B
		public object MenuItemsSource
		{
			get
			{
				return base.GetValue(NavigationView.MenuItemsSourceProperty);
			}
			set
			{
				base.SetValue(NavigationView.MenuItemsSourceProperty, value);
			}
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x00010D79 File Offset: 0x0000EF79
		private static void OnMenuItemsSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NavigationView)sender).PropertyChanged(args);
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600042C RID: 1068 RVA: 0x00010D87 File Offset: 0x0000EF87
		public object SettingsItem
		{
			get
			{
				return base.GetValue(NavigationView.SettingsItemProperty);
			}
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x00010D94 File Offset: 0x0000EF94
		private static void OnSettingsItemPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NavigationView)sender).PropertyChanged(args);
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600042E RID: 1070 RVA: 0x00010DA2 File Offset: 0x0000EFA2
		// (set) Token: 0x0600042F RID: 1071 RVA: 0x00010DB4 File Offset: 0x0000EFB4
		public AutoSuggestBox AutoSuggestBox
		{
			get
			{
				return (AutoSuggestBox)base.GetValue(NavigationView.AutoSuggestBoxProperty);
			}
			set
			{
				base.SetValue(NavigationView.AutoSuggestBoxProperty, value);
			}
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x00010DC2 File Offset: 0x0000EFC2
		private static void OnAutoSuggestBoxPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NavigationView)sender).PropertyChanged(args);
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000431 RID: 1073 RVA: 0x00010DD0 File Offset: 0x0000EFD0
		// (set) Token: 0x06000432 RID: 1074 RVA: 0x00010DE2 File Offset: 0x0000EFE2
		public DataTemplate MenuItemTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(NavigationView.MenuItemTemplateProperty);
			}
			set
			{
				base.SetValue(NavigationView.MenuItemTemplateProperty, value);
			}
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x00010DF0 File Offset: 0x0000EFF0
		private static void OnMenuItemTemplatePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NavigationView)sender).PropertyChanged(args);
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000434 RID: 1076 RVA: 0x00010DFE File Offset: 0x0000EFFE
		// (set) Token: 0x06000435 RID: 1077 RVA: 0x00010E10 File Offset: 0x0000F010
		public DataTemplateSelector MenuItemTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)base.GetValue(NavigationView.MenuItemTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(NavigationView.MenuItemTemplateSelectorProperty, value);
			}
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x00010E1E File Offset: 0x0000F01E
		private static void OnMenuItemTemplateSelectorPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NavigationView)sender).PropertyChanged(args);
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000437 RID: 1079 RVA: 0x00010E2C File Offset: 0x0000F02C
		// (set) Token: 0x06000438 RID: 1080 RVA: 0x00010E3E File Offset: 0x0000F03E
		public Style MenuItemContainerStyle
		{
			get
			{
				return (Style)base.GetValue(NavigationView.MenuItemContainerStyleProperty);
			}
			set
			{
				base.SetValue(NavigationView.MenuItemContainerStyleProperty, value);
			}
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00010E4C File Offset: 0x0000F04C
		private static void OnMenuItemContainerStylePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NavigationView)sender).PropertyChanged(args);
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600043A RID: 1082 RVA: 0x00010E5A File Offset: 0x0000F05A
		// (set) Token: 0x0600043B RID: 1083 RVA: 0x00010E6C File Offset: 0x0000F06C
		public StyleSelector MenuItemContainerStyleSelector
		{
			get
			{
				return (StyleSelector)base.GetValue(NavigationView.MenuItemContainerStyleSelectorProperty);
			}
			set
			{
				base.SetValue(NavigationView.MenuItemContainerStyleSelectorProperty, value);
			}
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00010E7A File Offset: 0x0000F07A
		private static void OnMenuItemContainerStyleSelectorPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NavigationView)sender).OnMenuItemContainerStyleSelectorPropertyChanged(args);
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00010E88 File Offset: 0x0000F088
		private void OnMenuItemContainerStyleSelectorPropertyChanged(DependencyPropertyChangedEventArgs args)
		{
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600043E RID: 1086 RVA: 0x00010E8A File Offset: 0x0000F08A
		// (set) Token: 0x0600043F RID: 1087 RVA: 0x00010E9C File Offset: 0x0000F09C
		public NavigationViewBackButtonVisible IsBackButtonVisible
		{
			get
			{
				return (NavigationViewBackButtonVisible)base.GetValue(NavigationView.IsBackButtonVisibleProperty);
			}
			set
			{
				base.SetValue(NavigationView.IsBackButtonVisibleProperty, value);
			}
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x00010EAF File Offset: 0x0000F0AF
		private static void OnIsBackButtonVisiblePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NavigationView)sender).PropertyChanged(args);
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000441 RID: 1089 RVA: 0x00010EBD File Offset: 0x0000F0BD
		// (set) Token: 0x06000442 RID: 1090 RVA: 0x00010ECF File Offset: 0x0000F0CF
		public bool IsBackEnabled
		{
			get
			{
				return (bool)base.GetValue(NavigationView.IsBackEnabledProperty);
			}
			set
			{
				base.SetValue(NavigationView.IsBackEnabledProperty, value);
			}
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00010EE2 File Offset: 0x0000F0E2
		private static void OnIsBackEnabledPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NavigationView)sender).PropertyChanged(args);
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000444 RID: 1092 RVA: 0x00010EF0 File Offset: 0x0000F0F0
		// (set) Token: 0x06000445 RID: 1093 RVA: 0x00010F02 File Offset: 0x0000F102
		public string PaneTitle
		{
			get
			{
				return (string)base.GetValue(NavigationView.PaneTitleProperty);
			}
			set
			{
				base.SetValue(NavigationView.PaneTitleProperty, value);
			}
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x00010F10 File Offset: 0x0000F110
		private static void OnPaneTitlePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NavigationView)sender).PropertyChanged(args);
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000447 RID: 1095 RVA: 0x00010F1E File Offset: 0x0000F11E
		// (set) Token: 0x06000448 RID: 1096 RVA: 0x00010F30 File Offset: 0x0000F130
		public NavigationViewPaneDisplayMode PaneDisplayMode
		{
			get
			{
				return (NavigationViewPaneDisplayMode)base.GetValue(NavigationView.PaneDisplayModeProperty);
			}
			set
			{
				base.SetValue(NavigationView.PaneDisplayModeProperty, value);
			}
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x00010F43 File Offset: 0x0000F143
		private static void OnPaneDisplayModePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NavigationView)sender).PropertyChanged(args);
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x0600044A RID: 1098 RVA: 0x00010F51 File Offset: 0x0000F151
		// (set) Token: 0x0600044B RID: 1099 RVA: 0x00010F63 File Offset: 0x0000F163
		public UIElement PaneHeader
		{
			get
			{
				return (UIElement)base.GetValue(NavigationView.PaneHeaderProperty);
			}
			set
			{
				base.SetValue(NavigationView.PaneHeaderProperty, value);
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x0600044C RID: 1100 RVA: 0x00010F71 File Offset: 0x0000F171
		// (set) Token: 0x0600044D RID: 1101 RVA: 0x00010F83 File Offset: 0x0000F183
		public UIElement PaneCustomContent
		{
			get
			{
				return (UIElement)base.GetValue(NavigationView.PaneCustomContentProperty);
			}
			set
			{
				base.SetValue(NavigationView.PaneCustomContentProperty, value);
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600044E RID: 1102 RVA: 0x00010F91 File Offset: 0x0000F191
		// (set) Token: 0x0600044F RID: 1103 RVA: 0x00010FA3 File Offset: 0x0000F1A3
		public UIElement ContentOverlay
		{
			get
			{
				return (UIElement)base.GetValue(NavigationView.ContentOverlayProperty);
			}
			set
			{
				base.SetValue(NavigationView.ContentOverlayProperty, value);
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000450 RID: 1104 RVA: 0x00010FB1 File Offset: 0x0000F1B1
		// (set) Token: 0x06000451 RID: 1105 RVA: 0x00010FC3 File Offset: 0x0000F1C3
		public bool IsPaneVisible
		{
			get
			{
				return (bool)base.GetValue(NavigationView.IsPaneVisibleProperty);
			}
			set
			{
				base.SetValue(NavigationView.IsPaneVisibleProperty, value);
			}
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x00010FD6 File Offset: 0x0000F1D6
		private static void OnIsPaneVisiblePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NavigationView)sender).PropertyChanged(args);
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000453 RID: 1107 RVA: 0x00010FE4 File Offset: 0x0000F1E4
		// (set) Token: 0x06000454 RID: 1108 RVA: 0x00010FF6 File Offset: 0x0000F1F6
		public NavigationViewSelectionFollowsFocus SelectionFollowsFocus
		{
			get
			{
				return (NavigationViewSelectionFollowsFocus)base.GetValue(NavigationView.SelectionFollowsFocusProperty);
			}
			set
			{
				base.SetValue(NavigationView.SelectionFollowsFocusProperty, value);
			}
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x00011009 File Offset: 0x0000F209
		private static void OnSelectionFollowsFocusPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NavigationView)sender).PropertyChanged(args);
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000456 RID: 1110 RVA: 0x00011017 File Offset: 0x0000F217
		// (set) Token: 0x06000457 RID: 1111 RVA: 0x00011029 File Offset: 0x0000F229
		public NavigationViewTemplateSettings TemplateSettings
		{
			get
			{
				return (NavigationViewTemplateSettings)base.GetValue(NavigationView.TemplateSettingsProperty);
			}
			private set
			{
				base.SetValue(NavigationView.TemplateSettingsPropertyKey, value);
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000458 RID: 1112 RVA: 0x00011037 File Offset: 0x0000F237
		// (set) Token: 0x06000459 RID: 1113 RVA: 0x00011049 File Offset: 0x0000F249
		public NavigationViewShoulderNavigationEnabled ShoulderNavigationEnabled
		{
			get
			{
				return (NavigationViewShoulderNavigationEnabled)base.GetValue(NavigationView.ShoulderNavigationEnabledProperty);
			}
			set
			{
				base.SetValue(NavigationView.ShoulderNavigationEnabledProperty, value);
			}
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x0001105C File Offset: 0x0000F25C
		private static void OnShoulderNavigationEnabledPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NavigationView)sender).PropertyChanged(args);
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600045B RID: 1115 RVA: 0x0001106A File Offset: 0x0000F26A
		// (set) Token: 0x0600045C RID: 1116 RVA: 0x0001107C File Offset: 0x0000F27C
		public NavigationViewOverflowLabelMode OverflowLabelMode
		{
			get
			{
				return (NavigationViewOverflowLabelMode)base.GetValue(NavigationView.OverflowLabelModeProperty);
			}
			set
			{
				base.SetValue(NavigationView.OverflowLabelModeProperty, value);
			}
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x0001108F File Offset: 0x0000F28F
		private static void OnOverflowLabelModePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NavigationView)sender).PropertyChanged(args);
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600045E RID: 1118 RVA: 0x0001109D File Offset: 0x0000F29D
		// (set) Token: 0x0600045F RID: 1119 RVA: 0x000110AF File Offset: 0x0000F2AF
		public bool IsTitleBarAutoPaddingEnabled
		{
			get
			{
				return (bool)base.GetValue(NavigationView.IsTitleBarAutoPaddingEnabledProperty);
			}
			set
			{
				base.SetValue(NavigationView.IsTitleBarAutoPaddingEnabledProperty, value);
			}
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x000110C2 File Offset: 0x0000F2C2
		private static void OnIsTitleBarAutoPaddingEnabledPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NavigationView)sender).PropertyChanged(args);
		}

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x06000461 RID: 1121 RVA: 0x000110D0 File Offset: 0x0000F2D0
		// (remove) Token: 0x06000462 RID: 1122 RVA: 0x00011108 File Offset: 0x0000F308
		public event TypedEventHandler<NavigationView, NavigationViewSelectionChangedEventArgs> SelectionChanged;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06000463 RID: 1123 RVA: 0x00011140 File Offset: 0x0000F340
		// (remove) Token: 0x06000464 RID: 1124 RVA: 0x00011178 File Offset: 0x0000F378
		public event TypedEventHandler<NavigationView, NavigationViewItemInvokedEventArgs> ItemInvoked;

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06000465 RID: 1125 RVA: 0x000111B0 File Offset: 0x0000F3B0
		// (remove) Token: 0x06000466 RID: 1126 RVA: 0x000111E8 File Offset: 0x0000F3E8
		public event TypedEventHandler<NavigationView, NavigationViewDisplayModeChangedEventArgs> DisplayModeChanged;

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06000467 RID: 1127 RVA: 0x00011220 File Offset: 0x0000F420
		// (remove) Token: 0x06000468 RID: 1128 RVA: 0x00011258 File Offset: 0x0000F458
		public event TypedEventHandler<NavigationView, NavigationViewBackRequestedEventArgs> BackRequested;

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06000469 RID: 1129 RVA: 0x00011290 File Offset: 0x0000F490
		// (remove) Token: 0x0600046A RID: 1130 RVA: 0x000112C8 File Offset: 0x0000F4C8
		public event TypedEventHandler<NavigationView, object> PaneClosed;

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x0600046B RID: 1131 RVA: 0x00011300 File Offset: 0x0000F500
		// (remove) Token: 0x0600046C RID: 1132 RVA: 0x00011338 File Offset: 0x0000F538
		public event TypedEventHandler<NavigationView, NavigationViewPaneClosingEventArgs> PaneClosing;

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x0600046D RID: 1133 RVA: 0x00011370 File Offset: 0x0000F570
		// (remove) Token: 0x0600046E RID: 1134 RVA: 0x000113A8 File Offset: 0x0000F5A8
		public event TypedEventHandler<NavigationView, object> PaneOpened;

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x0600046F RID: 1135 RVA: 0x000113E0 File Offset: 0x0000F5E0
		// (remove) Token: 0x06000470 RID: 1136 RVA: 0x00011418 File Offset: 0x0000F618
		public event TypedEventHandler<NavigationView, object> PaneOpening;

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x06000471 RID: 1137 RVA: 0x00011450 File Offset: 0x0000F650
		// (remove) Token: 0x06000472 RID: 1138 RVA: 0x00011488 File Offset: 0x0000F688
		public event TypedEventHandler<NavigationView, NavigationViewItemExpandingEventArgs> Expanding;

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x06000473 RID: 1139 RVA: 0x000114C0 File Offset: 0x0000F6C0
		// (remove) Token: 0x06000474 RID: 1140 RVA: 0x000114F8 File Offset: 0x0000F6F8
		public event TypedEventHandler<NavigationView, NavigationViewItemCollapsedEventArgs> Collapsed;

		// Token: 0x06000475 RID: 1141 RVA: 0x00011530 File Offset: 0x0000F730
		private static object CoerceToGreaterThanZero(DependencyObject d, object baseValue)
		{
			if (baseValue is double)
			{
				double num = (double)baseValue;
				((NavigationView)d).CoerceToGreaterThanZero(ref num);
				return num;
			}
			return baseValue;
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x00011564 File Offset: 0x0000F764
		[CompilerGenerated]
		private bool <OnSelectionModelSelectionChanged>g__init|64_0(ref NavigationView.<>c__DisplayClass64_0 A_1)
		{
			NavigationViewItemBase containerForIndexPath = this.GetContainerForIndexPath(A_1.selectedIndex, false);
			if (containerForIndexPath != null)
			{
				NavigationViewItem navigationViewItem = containerForIndexPath as NavigationViewItem;
				if (navigationViewItem != null && this.DoesNavigationViewItemHaveChildren(navigationViewItem))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x00011598 File Offset: 0x0000F798
		[CompilerGenerated]
		private bool <CloseFlyoutIfRequired>g__init|66_0()
		{
			SplitView rootSplitView = this.m_rootSplitView;
			if (rootSplitView != null)
			{
				SplitViewDisplayMode displayMode = rootSplitView.DisplayMode;
				return (!rootSplitView.IsPaneOpen && (displayMode == SplitViewDisplayMode.CompactOverlay || displayMode == SplitViewDisplayMode.CompactInline)) || this.PaneDisplayMode == NavigationViewPaneDisplayMode.Top;
			}
			return false;
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x000115D4 File Offset: 0x0000F7D4
		[CompilerGenerated]
		private object <UpdateRepeaterItemsSource>g__init|68_0()
		{
			object menuItemsSource = this.MenuItemsSource;
			if (menuItemsSource != null)
			{
				return menuItemsSource;
			}
			this.UpdateSelectionForMenuItems();
			return this.MenuItems;
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x000115FC File Offset: 0x0000F7FC
		[CompilerGenerated]
		private object <UpdateFooterRepeaterItemsSource>g__init|74_0()
		{
			object footerMenuItemsSource = this.FooterMenuItemsSource;
			if (footerMenuItemsSource != null)
			{
				return footerMenuItemsSource;
			}
			this.UpdateSelectionForMenuItems();
			return this.FooterMenuItems;
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00011624 File Offset: 0x0000F824
		[CompilerGenerated]
		private NavigationRecommendedTransitionDirection <RaiseItemInvokedForNavigationViewItem>g__init|78_0(ref NavigationView.<>c__DisplayClass78_0 A_1)
		{
			if (this.IsTopNavigationView() && A_1.nvi.SelectsOnInvoked)
			{
				if (A_1.parentIR == this.m_topNavRepeaterOverflowView)
				{
					return NavigationRecommendedTransitionDirection.FromOverflow;
				}
				if (A_1.prevItem != null)
				{
					return this.GetRecommendedTransitionDirection(this.NavigationViewItemBaseOrSettingsContentFromData(A_1.prevItem), A_1.nvi);
				}
			}
			return NavigationRecommendedTransitionDirection.Default;
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x0001167A File Offset: 0x0000F87A
		[CompilerGenerated]
		private NavigationViewRepeaterPosition <OnRepeaterElementPrepared>g__init|86_0(ref NavigationView.<>c__DisplayClass86_0 A_1)
		{
			if (this.IsTopNavigationView())
			{
				if (A_1.ir == this.m_topNavRepeater)
				{
					return NavigationViewRepeaterPosition.TopPrimary;
				}
				if (A_1.ir == this.m_topNavFooterMenuRepeater)
				{
					return NavigationViewRepeaterPosition.TopFooter;
				}
				return NavigationViewRepeaterPosition.TopOverflow;
			}
			else
			{
				if (A_1.ir == this.m_leftNavFooterMenuRepeater)
				{
					return NavigationViewRepeaterPosition.LeftFooter;
				}
				return NavigationViewRepeaterPosition.LeftNav;
			}
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x000116B7 File Offset: 0x0000F8B7
		[CompilerGenerated]
		internal static int <OnRepeaterElementPrepared>g__init|86_1(ref NavigationView.<>c__DisplayClass86_1 A_0)
		{
			if (A_0.position == NavigationViewRepeaterPosition.TopPrimary)
			{
				return 0;
			}
			return A_0.nvibImpl.Depth + 1;
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x000116D4 File Offset: 0x0000F8D4
		[CompilerGenerated]
		private double <UpdatePaneLayout>g__init|96_0()
		{
			RowDefinition itemsContainerRow = this.m_itemsContainerRow;
			if (itemsContainerRow == null)
			{
				return 0.0;
			}
			ContentControl leftNavFooterContentBorder = this.m_leftNavFooterContentBorder;
			if (leftNavFooterContentBorder != null)
			{
				return itemsContainerRow.ActualHeight - 29.0 - leftNavFooterContentBorder.ActualHeight;
			}
			return itemsContainerRow.ActualHeight - 29.0;
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00011728 File Offset: 0x0000F928
		[CompilerGenerated]
		private double <UpdatePaneLayout>g__init|96_1(ref NavigationView.<>c__DisplayClass96_0 A_1, ref NavigationView.<>c__DisplayClass96_1 A_2)
		{
			FrameworkElement footerItemsScrollViewer = this.m_footerItemsScrollViewer;
			if (footerItemsScrollViewer != null)
			{
				ItemsRepeater leftNavFooterMenuRepeater = this.m_leftNavFooterMenuRepeater;
				if (leftNavFooterMenuRepeater != null)
				{
					ItemsRepeater leftNavRepeater = this.m_leftNavRepeater;
					if (leftNavRepeater == null)
					{
						return A_1.totalAvailableHeight - leftNavFooterMenuRepeater.ActualHeight;
					}
					double actualHeight = leftNavFooterMenuRepeater.ActualHeight;
					double actualHeight2 = leftNavRepeater.ActualHeight;
					if (A_1.totalAvailableHeight > actualHeight2 + actualHeight)
					{
						footerItemsScrollViewer.MaxHeight = actualHeight;
						FrameworkElement visualItemsSeparator = this.m_visualItemsSeparator;
						if (visualItemsSeparator != null)
						{
							visualItemsSeparator.Visibility = 2;
						}
						return A_1.totalAvailableHeight - actualHeight;
					}
					if (actualHeight2 <= A_2.totalAvailableHeightHalf)
					{
						footerItemsScrollViewer.MaxHeight = A_1.totalAvailableHeight - actualHeight2;
						FrameworkElement visualItemsSeparator2 = this.m_visualItemsSeparator;
						if (visualItemsSeparator2 != null)
						{
							visualItemsSeparator2.Visibility = 0;
						}
						return actualHeight2;
					}
					if (actualHeight <= A_2.totalAvailableHeightHalf)
					{
						footerItemsScrollViewer.MaxHeight = actualHeight;
						FrameworkElement visualItemsSeparator3 = this.m_visualItemsSeparator;
						if (visualItemsSeparator3 != null)
						{
							visualItemsSeparator3.Visibility = 0;
						}
						return A_1.totalAvailableHeight - actualHeight;
					}
					footerItemsScrollViewer.MaxHeight = A_2.totalAvailableHeightHalf;
					FrameworkElement visualItemsSeparator4 = this.m_visualItemsSeparator;
					if (visualItemsSeparator4 != null)
					{
						visualItemsSeparator4.Visibility = 0;
					}
					return A_2.totalAvailableHeightHalf;
				}
				else
				{
					footerItemsScrollViewer.MaxHeight = A_2.totalAvailableHeightHalf;
				}
			}
			return A_2.totalAvailableHeightHalf;
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x0001183F File Offset: 0x0000FA3F
		[CompilerGenerated]
		private double <UpdatePaneButtonsWidths>g__init|109_0()
		{
			if (this.DisplayMode == NavigationViewDisplayMode.Minimal)
			{
				return 40.0;
			}
			return this.CompactPaneLength;
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x0001185C File Offset: 0x0000FA5C
		[CompilerGenerated]
		private NavigationRecommendedTransitionDirection <ChangeSelection>g__init|139_0(ref NavigationView.<>c__DisplayClass139_0 A_1)
		{
			if (this.IsTopNavigationView())
			{
				if (this.m_selectionChangeFromOverflowMenu)
				{
					return NavigationRecommendedTransitionDirection.FromOverflow;
				}
				if (A_1.prevItem != null && A_1.nextItem != null)
				{
					return this.GetRecommendedTransitionDirection(this.NavigationViewItemBaseOrSettingsContentFromData(A_1.prevItem), this.NavigationViewItemBaseOrSettingsContentFromData(A_1.nextItem));
				}
			}
			return NavigationRecommendedTransitionDirection.Default;
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x000118AB File Offset: 0x0000FAAB
		[CompilerGenerated]
		private UIElement <KeyboardFocusFirstItemFromItem>g__init|153_0(ref NavigationView.<>c__DisplayClass153_0 A_1)
		{
			return this.GetParentRootItemsRepeaterForContainer(A_1.nvib).TryGetElement(0);
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x000118C0 File Offset: 0x0000FAC0
		[CompilerGenerated]
		private bool <OnRepeaterGettingFocus>g__init|155_0(ref NavigationView.<>c__DisplayClass155_0 A_1, ref NavigationView.<>c__DisplayClass155_1 A_2)
		{
			bool result = true;
			for (DependencyObject dependencyObject = A_1.oldFocusedElement; dependencyObject != null; dependencyObject = VisualTreeHelper.GetParent(dependencyObject))
			{
				NavigationViewItemBase navigationViewItemBase = dependencyObject as NavigationViewItemBase;
				if (navigationViewItemBase != null)
				{
					result = (this.GetParentRootItemsRepeaterForContainer(navigationViewItemBase) != A_2.newRootItemsRepeater);
					break;
				}
			}
			return result;
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x00011904 File Offset: 0x0000FB04
		[CompilerGenerated]
		private object <OnRepeaterGettingFocus>g__init|155_1()
		{
			if (this.IsTopNavigationView())
			{
				if (this.m_selectionModel.SelectedIndex.GetAt(0) != 0)
				{
					return this.m_topNavFooterMenuRepeater;
				}
				return this.m_topNavRepeater;
			}
			else
			{
				if (this.m_selectionModel.SelectedIndex.GetAt(0) != 0)
				{
					return this.m_leftNavFooterMenuRepeater;
				}
				return this.m_leftNavRepeater;
			}
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x0001195A File Offset: 0x0000FB5A
		[CompilerGenerated]
		private object <SelectOverflowItem>g__init|196_0(ref NavigationView.<>c__DisplayClass196_0 A_1)
		{
			if (A_1.ip.GetSize() > 2)
			{
				return this.GetItemFromIndex(this.m_topNavRepeaterOverflowView, this.m_topDataProvider.ConvertOriginalIndexToIndex(A_1.ip.GetAt(1)));
			}
			return A_1.item;
		}

		// Token: 0x04000142 RID: 322
		private const string c_togglePaneButtonName = "TogglePaneButton";

		// Token: 0x04000143 RID: 323
		private const string c_paneTitleHolderFrameworkElement = "PaneTitleHolder";

		// Token: 0x04000144 RID: 324
		private const string c_paneTitleFrameworkElement = "PaneTitleTextBlock";

		// Token: 0x04000145 RID: 325
		private const string c_rootSplitViewName = "RootSplitView";

		// Token: 0x04000146 RID: 326
		private const string c_menuItemsHost = "MenuItemsHost";

		// Token: 0x04000147 RID: 327
		private const string c_footerMenuItemsHost = "FooterMenuItemsHost";

		// Token: 0x04000148 RID: 328
		private const string c_selectionIndicatorName = "SelectionIndicator";

		// Token: 0x04000149 RID: 329
		private const string c_paneContentGridName = "PaneContentGrid";

		// Token: 0x0400014A RID: 330
		private const string c_rootGridName = "RootGrid";

		// Token: 0x0400014B RID: 331
		private const string c_contentGridName = "ContentGrid";

		// Token: 0x0400014C RID: 332
		private const string c_searchButtonName = "PaneAutoSuggestButton";

		// Token: 0x0400014D RID: 333
		private const string c_paneToggleButtonIconGridColumnName = "PaneToggleButtonIconWidthColumn";

		// Token: 0x0400014E RID: 334
		private const string c_togglePaneTopPadding = "TogglePaneTopPadding";

		// Token: 0x0400014F RID: 335
		private const string c_contentPaneTopPadding = "ContentPaneTopPadding";

		// Token: 0x04000150 RID: 336
		private const string c_contentLeftPadding = "ContentLeftPadding";

		// Token: 0x04000151 RID: 337
		private const string c_navViewBackButton = "NavigationViewBackButton";

		// Token: 0x04000152 RID: 338
		private const string c_navViewBackButtonToolTip = "NavigationViewBackButtonToolTip";

		// Token: 0x04000153 RID: 339
		private const string c_navViewCloseButton = "NavigationViewCloseButton";

		// Token: 0x04000154 RID: 340
		private const string c_navViewCloseButtonToolTip = "NavigationViewCloseButtonToolTip";

		// Token: 0x04000155 RID: 341
		private const string c_paneShadowReceiverCanvas = "PaneShadowReceiver";

		// Token: 0x04000156 RID: 342
		private const string c_flyoutRootGrid = "FlyoutRootGrid";

		// Token: 0x04000157 RID: 343
		private const string c_topNavMenuItemsHost = "TopNavMenuItemsHost";

		// Token: 0x04000158 RID: 344
		private const string c_topNavFooterMenuItemsHost = "TopFooterMenuItemsHost";

		// Token: 0x04000159 RID: 345
		private const string c_topNavOverflowButton = "TopNavOverflowButton";

		// Token: 0x0400015A RID: 346
		private const string c_topNavMenuItemsOverflowHost = "TopNavMenuItemsOverflowHost";

		// Token: 0x0400015B RID: 347
		private const string c_topNavGrid = "TopNavGrid";

		// Token: 0x0400015C RID: 348
		private const string c_topNavContentOverlayAreaGrid = "TopNavContentOverlayAreaGrid";

		// Token: 0x0400015D RID: 349
		private const string c_leftNavPaneAutoSuggestBoxPresenter = "PaneAutoSuggestBoxPresenter";

		// Token: 0x0400015E RID: 350
		private const string c_topNavPaneAutoSuggestBoxPresenter = "TopPaneAutoSuggestBoxPresenter";

		// Token: 0x0400015F RID: 351
		private const string c_paneTitlePresenter = "PaneTitlePresenter";

		// Token: 0x04000160 RID: 352
		private const string c_leftNavFooterContentBorder = "FooterContentBorder";

		// Token: 0x04000161 RID: 353
		private const string c_leftNavPaneHeaderContentBorder = "PaneHeaderContentBorder";

		// Token: 0x04000162 RID: 354
		private const string c_leftNavPaneCustomContentBorder = "PaneCustomContentBorder";

		// Token: 0x04000163 RID: 355
		private const string c_itemsContainer = "ItemsContainerGrid";

		// Token: 0x04000164 RID: 356
		private const string c_itemsContainerRow = "ItemsContainerRow";

		// Token: 0x04000165 RID: 357
		private const string c_visualItemsSeparator = "VisualItemsSeparator";

		// Token: 0x04000166 RID: 358
		private const string c_menuItemsScrollViewer = "MenuItemsScrollViewer";

		// Token: 0x04000167 RID: 359
		private const string c_footerItemsScrollViewer = "FooterItemsScrollViewer";

		// Token: 0x04000168 RID: 360
		private const string c_paneHeaderOnTopPane = "PaneHeaderOnTopPane";

		// Token: 0x04000169 RID: 361
		private const string c_paneTitleOnTopPane = "PaneTitleOnTopPane";

		// Token: 0x0400016A RID: 362
		private const string c_paneCustomContentOnTopPane = "PaneCustomContentOnTopPane";

		// Token: 0x0400016B RID: 363
		private const string c_paneFooterOnTopPane = "PaneFooterOnTopPane";

		// Token: 0x0400016C RID: 364
		private const string c_paneHeaderCloseButtonColumn = "PaneHeaderCloseButtonColumn";

		// Token: 0x0400016D RID: 365
		private const string c_paneHeaderToggleButtonColumn = "PaneHeaderToggleButtonColumn";

		// Token: 0x0400016E RID: 366
		private const string c_paneHeaderContentBorderRow = "PaneHeaderContentBorderRow";

		// Token: 0x0400016F RID: 367
		private const int c_backButtonHeight = 40;

		// Token: 0x04000170 RID: 368
		private const int c_backButtonWidth = 40;

		// Token: 0x04000171 RID: 369
		private const int c_paneToggleButtonHeight = 40;

		// Token: 0x04000172 RID: 370
		private const int c_paneToggleButtonWidth = 40;

		// Token: 0x04000173 RID: 371
		private const int c_toggleButtonHeightWhenShouldPreserveNavigationViewRS3Behavior = 56;

		// Token: 0x04000174 RID: 372
		private const int c_backButtonRowDefinition = 1;

		// Token: 0x04000175 RID: 373
		private const float c_paneElevationTranslationZ = 32f;

		// Token: 0x04000176 RID: 374
		private const int c_mainMenuBlockIndex = 0;

		// Token: 0x04000177 RID: 375
		private const int c_footerMenuBlockIndex = 1;

		// Token: 0x04000178 RID: 376
		private const int s_itemNotFound = -1;

		// Token: 0x04000179 RID: 377
		private static readonly Size c_infSize = new Size(double.PositiveInfinity, double.PositiveInfinity);

		// Token: 0x0400017A RID: 378
		private static readonly ResourceAccessor ResourceAccessor = new ResourceAccessor(typeof(NavigationView));

		// Token: 0x0400017B RID: 379
		private static readonly Point c_frame1point1 = new Point(0.9, 0.1);

		// Token: 0x0400017C RID: 380
		private static readonly Point c_frame1point2 = new Point(1.0, 0.2);

		// Token: 0x0400017D RID: 381
		private static readonly Point c_frame2point1 = new Point(0.1, 0.9);

		// Token: 0x0400017E RID: 382
		private static readonly Point c_frame2point2 = new Point(0.2, 1.0);

		// Token: 0x0400017F RID: 383
		private bool m_InitialNonForcedModeUpdate = true;

		// Token: 0x04000180 RID: 384
		private NavigationViewItemsFactory m_navigationViewItemsFactory;

		// Token: 0x04000181 RID: 385
		private Button m_paneToggleButton;

		// Token: 0x04000182 RID: 386
		private SplitView m_rootSplitView;

		// Token: 0x04000183 RID: 387
		private NavigationViewItem m_settingsItem;

		// Token: 0x04000184 RID: 388
		private RowDefinition m_itemsContainerRow;

		// Token: 0x04000185 RID: 389
		private FrameworkElement m_menuItemsScrollViewer;

		// Token: 0x04000186 RID: 390
		private FrameworkElement m_footerItemsScrollViewer;

		// Token: 0x04000187 RID: 391
		private UIElement m_paneContentGrid;

		// Token: 0x04000188 RID: 392
		private ColumnDefinition m_paneToggleButtonIconGridColumn;

		// Token: 0x04000189 RID: 393
		private FrameworkElement m_paneTitleHolderFrameworkElement;

		// Token: 0x0400018A RID: 394
		private FrameworkElement m_paneTitleFrameworkElement;

		// Token: 0x0400018B RID: 395
		private FrameworkElement m_visualItemsSeparator;

		// Token: 0x0400018C RID: 396
		private Button m_paneSearchButton;

		// Token: 0x0400018D RID: 397
		private Button m_backButton;

		// Token: 0x0400018E RID: 398
		private Button m_closeButton;

		// Token: 0x0400018F RID: 399
		private ItemsRepeater m_leftNavRepeater;

		// Token: 0x04000190 RID: 400
		private ItemsRepeater m_topNavRepeater;

		// Token: 0x04000191 RID: 401
		private ItemsRepeater m_leftNavFooterMenuRepeater;

		// Token: 0x04000192 RID: 402
		private ItemsRepeater m_topNavFooterMenuRepeater;

		// Token: 0x04000193 RID: 403
		private Button m_topNavOverflowButton;

		// Token: 0x04000194 RID: 404
		private ItemsRepeater m_topNavRepeaterOverflowView;

		// Token: 0x04000195 RID: 405
		private Grid m_topNavGrid;

		// Token: 0x04000196 RID: 406
		private Border m_topNavContentOverlayAreaGrid;

		// Token: 0x04000197 RID: 407
		private UIElement m_prevIndicator;

		// Token: 0x04000198 RID: 408
		private UIElement m_nextIndicator;

		// Token: 0x04000199 RID: 409
		private UIElement m_activeIndicator;

		// Token: 0x0400019A RID: 410
		private object m_lastSelectedItemPendingAnimationInTopNav;

		// Token: 0x0400019B RID: 411
		private FrameworkElement m_togglePaneTopPadding;

		// Token: 0x0400019C RID: 412
		private FrameworkElement m_contentPaneTopPadding;

		// Token: 0x0400019D RID: 413
		private FrameworkElement m_contentLeftPadding;

		// Token: 0x0400019E RID: 414
		private CoreApplicationViewTitleBar m_coreTitleBar;

		// Token: 0x0400019F RID: 415
		private ContentControl m_leftNavPaneAutoSuggestBoxPresenter;

		// Token: 0x040001A0 RID: 416
		private ContentControl m_topNavPaneAutoSuggestBoxPresenter;

		// Token: 0x040001A1 RID: 417
		private ContentControl m_leftNavPaneHeaderContentBorder;

		// Token: 0x040001A2 RID: 418
		private ContentControl m_leftNavPaneCustomContentBorder;

		// Token: 0x040001A3 RID: 419
		private ContentControl m_leftNavFooterContentBorder;

		// Token: 0x040001A4 RID: 420
		private ContentControl m_paneHeaderOnTopPane;

		// Token: 0x040001A5 RID: 421
		private ContentControl m_paneTitleOnTopPane;

		// Token: 0x040001A6 RID: 422
		private ContentControl m_paneCustomContentOnTopPane;

		// Token: 0x040001A7 RID: 423
		private ContentControl m_paneFooterOnTopPane;

		// Token: 0x040001A8 RID: 424
		private ContentControl m_paneTitlePresenter;

		// Token: 0x040001A9 RID: 425
		private ColumnDefinition m_paneHeaderCloseButtonColumn;

		// Token: 0x040001AA RID: 426
		private ColumnDefinition m_paneHeaderToggleButtonColumn;

		// Token: 0x040001AB RID: 427
		private RowDefinition m_paneHeaderContentBorderRow;

		// Token: 0x040001AC RID: 428
		private NavigationViewItem m_lastItemExpandedIntoFlyout;

		// Token: 0x040001AD RID: 429
		private bool m_layoutUpdatedToken;

		// Token: 0x040001AE RID: 430
		private FrameworkElementSizeChangedRevoker m_itemsContainerSizeChangedRevoker;

		// Token: 0x040001AF RID: 431
		private ItemsSourceView.CollectionChangedRevoker m_menuItemsCollectionChangedRevoker;

		// Token: 0x040001B0 RID: 432
		private ItemsSourceView.CollectionChangedRevoker m_footerItemsCollectionChangedRevoker;

		// Token: 0x040001B1 RID: 433
		private ItemsSourceView.CollectionChangedRevoker m_topNavOverflowItemsCollectionChangedRevoker;

		// Token: 0x040001B2 RID: 434
		private bool m_wasForceClosed;

		// Token: 0x040001B3 RID: 435
		private bool m_isClosedCompact;

		// Token: 0x040001B4 RID: 436
		private bool m_blockNextClosingEvent;

		// Token: 0x040001B5 RID: 437
		private bool m_initialListSizeStateSet;

		// Token: 0x040001B6 RID: 438
		private TopNavigationViewDataProvider m_topDataProvider = new TopNavigationViewDataProvider();

		// Token: 0x040001B7 RID: 439
		private SelectionModel m_selectionModel = new SelectionModel();

		// Token: 0x040001B8 RID: 440
		private List<object> m_selectionModelSource;

		// Token: 0x040001B9 RID: 441
		private ItemsSourceView m_menuItemsSource;

		// Token: 0x040001BA RID: 442
		private ItemsSourceView m_footerItemsSource;

		// Token: 0x040001BB RID: 443
		private bool m_appliedTemplate;

		// Token: 0x040001BC RID: 444
		private bool m_shouldIgnoreNextSelectionChange;

		// Token: 0x040001BD RID: 445
		private bool m_shouldIgnoreNextSelectionChangeBecauseSettingsRestore;

		// Token: 0x040001BE RID: 446
		private bool m_selectionChangeFromOverflowMenu;

		// Token: 0x040001BF RID: 447
		private bool m_shouldRaiseItemInvokedAfterSelection;

		// Token: 0x040001C0 RID: 448
		private TopNavigationViewLayoutState m_topNavigationMode;

		// Token: 0x040001C1 RID: 449
		private float m_topNavigationRecoveryGracePeriodWidth = 5f;

		// Token: 0x040001C2 RID: 450
		private bool m_isOpenPaneForInteraction;

		// Token: 0x040001C3 RID: 451
		private bool m_moveTopNavOverflowItemOnFlyoutClose;

		// Token: 0x040001C4 RID: 452
		private bool m_shouldIgnoreUIASelectionRaiseAsExpandCollapseWillRaise;

		// Token: 0x040001C5 RID: 453
		private bool m_OrientationChangedPendingAnimation;

		// Token: 0x040001C6 RID: 454
		private bool m_TabKeyPrecedesFocusChange;

		// Token: 0x040001C7 RID: 455
		private GettingFocusHelper m_leftNavRepeaterGettingFocusHelper;

		// Token: 0x040001C8 RID: 456
		private GettingFocusHelper m_topNavRepeaterGettingFocusHelper;

		// Token: 0x040001C9 RID: 457
		private GettingFocusHelper m_leftNavFooterMenuRepeaterGettingFocusHelper;

		// Token: 0x040001CA RID: 458
		private GettingFocusHelper m_topNavFooterMenuRepeaterGettingFocusHelper;

		// Token: 0x040001CB RID: 459
		private readonly BitmapCache m_bitmapCache;

		// Token: 0x040001CC RID: 460
		private static readonly PropertyPath s_opacityPath = new PropertyPath(UIElement.OpacityProperty);

		// Token: 0x040001CD RID: 461
		private static readonly PropertyPath s_centerXPath = new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.CenterX)", Array.Empty<object>());

		// Token: 0x040001CE RID: 462
		private static readonly PropertyPath s_centerYPath = new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.CenterY)", Array.Empty<object>());

		// Token: 0x040001CF RID: 463
		private static readonly PropertyPath s_scaleXPath = new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)", Array.Empty<object>());

		// Token: 0x040001D0 RID: 464
		private static readonly PropertyPath s_scaleYPath = new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)", Array.Empty<object>());

		// Token: 0x040001D1 RID: 465
		private static readonly PropertyPath s_translateXPath = new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[1].(TranslateTransform.X)", Array.Empty<object>());

		// Token: 0x040001D2 RID: 466
		private static readonly PropertyPath s_translateYPath = new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[1].(TranslateTransform.Y)", Array.Empty<object>());

		// Token: 0x040001D3 RID: 467
		public static readonly DependencyProperty IsPaneOpenProperty = DependencyProperty.Register("IsPaneOpen", typeof(bool), typeof(NavigationView), new PropertyMetadata(true, new PropertyChangedCallback(NavigationView.OnIsPaneOpenPropertyChanged)));

		// Token: 0x040001D4 RID: 468
		public static readonly DependencyProperty CompactModeThresholdWidthProperty = DependencyProperty.Register("CompactModeThresholdWidth", typeof(double), typeof(NavigationView), new PropertyMetadata(641.0, new PropertyChangedCallback(NavigationView.OnCompactModeThresholdWidthPropertyChanged), new CoerceValueCallback(NavigationView.CoerceToGreaterThanZero)));

		// Token: 0x040001D5 RID: 469
		public static readonly DependencyProperty ExpandedModeThresholdWidthProperty = DependencyProperty.Register("ExpandedModeThresholdWidth", typeof(double), typeof(NavigationView), new PropertyMetadata(1008.0, new PropertyChangedCallback(NavigationView.OnExpandedModeThresholdWidthPropertyChanged), new CoerceValueCallback(NavigationView.CoerceToGreaterThanZero)));

		// Token: 0x040001D6 RID: 470
		private static readonly DependencyPropertyKey FooterMenuItemsPropertyKey = DependencyProperty.RegisterReadOnly("FooterMenuItems", typeof(IList), typeof(NavigationView), new PropertyMetadata(new PropertyChangedCallback(NavigationView.OnFooterMenuItemsPropertyChanged)));

		// Token: 0x040001D7 RID: 471
		private static readonly DependencyProperty FooterMenuItemsProperty = NavigationView.FooterMenuItemsPropertyKey.DependencyProperty;

		// Token: 0x040001D8 RID: 472
		public static readonly DependencyProperty FooterMenuItemsSourceProperty = DependencyProperty.Register("FooterMenuItemsSource", typeof(object), typeof(NavigationView), new PropertyMetadata(new PropertyChangedCallback(NavigationView.OnFooterMenuItemsSourcePropertyChanged)));

		// Token: 0x040001D9 RID: 473
		public static readonly DependencyProperty PaneFooterProperty = DependencyProperty.Register("PaneFooter", typeof(UIElement), typeof(NavigationView), new PropertyMetadata(new PropertyChangedCallback(NavigationView.OnPaneFooterPropertyChanged)));

		// Token: 0x040001DA RID: 474
		public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(object), typeof(NavigationView), new PropertyMetadata(new PropertyChangedCallback(NavigationView.OnHeaderPropertyChanged)));

		// Token: 0x040001DB RID: 475
		public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(NavigationView), new PropertyMetadata(new PropertyChangedCallback(NavigationView.OnHeaderTemplatePropertyChanged)));

		// Token: 0x040001DC RID: 476
		private static readonly DependencyPropertyKey DisplayModePropertyKey = DependencyProperty.RegisterReadOnly("DisplayMode", typeof(NavigationViewDisplayMode), typeof(NavigationView), new PropertyMetadata(NavigationViewDisplayMode.Minimal, new PropertyChangedCallback(NavigationView.OnDisplayModePropertyChanged)));

		// Token: 0x040001DD RID: 477
		public static readonly DependencyProperty DisplayModeProperty = NavigationView.DisplayModePropertyKey.DependencyProperty;

		// Token: 0x040001DE RID: 478
		public static readonly DependencyProperty IsSettingsVisibleProperty = DependencyProperty.Register("IsSettingsVisible", typeof(bool), typeof(NavigationView), new PropertyMetadata(true, new PropertyChangedCallback(NavigationView.OnIsSettingsVisiblePropertyChanged)));

		// Token: 0x040001DF RID: 479
		public static readonly DependencyProperty IsPaneToggleButtonVisibleProperty = DependencyProperty.Register("IsPaneToggleButtonVisible", typeof(bool), typeof(NavigationView), new PropertyMetadata(true, new PropertyChangedCallback(NavigationView.OnIsPaneToggleButtonVisiblePropertyChanged)));

		// Token: 0x040001E0 RID: 480
		public static readonly DependencyProperty AlwaysShowHeaderProperty = DependencyProperty.Register("AlwaysShowHeader", typeof(bool), typeof(NavigationView), new PropertyMetadata(true, new PropertyChangedCallback(NavigationView.OnAlwaysShowHeaderPropertyChanged)));

		// Token: 0x040001E1 RID: 481
		public static readonly DependencyProperty CompactPaneLengthProperty = DependencyProperty.Register("CompactPaneLength", typeof(double), typeof(NavigationView), new PropertyMetadata(48.0, new PropertyChangedCallback(NavigationView.OnCompactPaneLengthPropertyChanged), new CoerceValueCallback(NavigationView.CoerceToGreaterThanZero)));

		// Token: 0x040001E2 RID: 482
		public static readonly DependencyProperty OpenPaneLengthProperty = DependencyProperty.Register("OpenPaneLength", typeof(double), typeof(NavigationView), new PropertyMetadata(320.0, new PropertyChangedCallback(NavigationView.OnOpenPaneLengthPropertyChanged), new CoerceValueCallback(NavigationView.CoerceToGreaterThanZero)));

		// Token: 0x040001E3 RID: 483
		public static readonly DependencyProperty PaneToggleButtonStyleProperty = DependencyProperty.Register("PaneToggleButtonStyle", typeof(Style), typeof(NavigationView), new PropertyMetadata(new PropertyChangedCallback(NavigationView.OnPaneToggleButtonStylePropertyChanged)));

		// Token: 0x040001E4 RID: 484
		public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(NavigationView), new PropertyMetadata(new PropertyChangedCallback(NavigationView.OnSelectedItemPropertyChanged)));

		// Token: 0x040001E5 RID: 485
		private static readonly DependencyPropertyKey MenuItemsPropertyKey = DependencyProperty.RegisterReadOnly("MenuItems", typeof(IList), typeof(NavigationView), new PropertyMetadata(new PropertyChangedCallback(NavigationView.OnMenuItemsPropertyChanged)));

		// Token: 0x040001E6 RID: 486
		private static readonly DependencyProperty MenuItemsProperty = NavigationView.MenuItemsPropertyKey.DependencyProperty;

		// Token: 0x040001E7 RID: 487
		public static readonly DependencyProperty MenuItemsSourceProperty = DependencyProperty.Register("MenuItemsSource", typeof(object), typeof(NavigationView), new PropertyMetadata(new PropertyChangedCallback(NavigationView.OnMenuItemsSourcePropertyChanged)));

		// Token: 0x040001E8 RID: 488
		private static readonly DependencyPropertyKey SettingsItemPropertyKey = DependencyProperty.RegisterReadOnly("SettingsItem", typeof(object), typeof(NavigationView), new PropertyMetadata(new PropertyChangedCallback(NavigationView.OnSettingsItemPropertyChanged)));

		// Token: 0x040001E9 RID: 489
		public static readonly DependencyProperty SettingsItemProperty = NavigationView.SettingsItemPropertyKey.DependencyProperty;

		// Token: 0x040001EA RID: 490
		public static readonly DependencyProperty AutoSuggestBoxProperty = DependencyProperty.Register("AutoSuggestBox", typeof(AutoSuggestBox), typeof(NavigationView), new PropertyMetadata(new PropertyChangedCallback(NavigationView.OnAutoSuggestBoxPropertyChanged)));

		// Token: 0x040001EB RID: 491
		public static readonly DependencyProperty MenuItemTemplateProperty = DependencyProperty.Register("MenuItemTemplate", typeof(DataTemplate), typeof(NavigationView), new PropertyMetadata(new PropertyChangedCallback(NavigationView.OnMenuItemTemplatePropertyChanged)));

		// Token: 0x040001EC RID: 492
		public static readonly DependencyProperty MenuItemTemplateSelectorProperty = DependencyProperty.Register("MenuItemTemplateSelector", typeof(DataTemplateSelector), typeof(NavigationView), new PropertyMetadata(new PropertyChangedCallback(NavigationView.OnMenuItemTemplateSelectorPropertyChanged)));

		// Token: 0x040001ED RID: 493
		public static readonly DependencyProperty MenuItemContainerStyleProperty = DependencyProperty.Register("MenuItemContainerStyle", typeof(Style), typeof(NavigationView), new PropertyMetadata(new PropertyChangedCallback(NavigationView.OnMenuItemContainerStylePropertyChanged)));

		// Token: 0x040001EE RID: 494
		public static readonly DependencyProperty MenuItemContainerStyleSelectorProperty = DependencyProperty.Register("MenuItemContainerStyleSelector", typeof(StyleSelector), typeof(NavigationView), new PropertyMetadata(new PropertyChangedCallback(NavigationView.OnMenuItemContainerStyleSelectorPropertyChanged)));

		// Token: 0x040001EF RID: 495
		public static readonly DependencyProperty IsBackButtonVisibleProperty = DependencyProperty.Register("IsBackButtonVisible", typeof(NavigationViewBackButtonVisible), typeof(NavigationView), new PropertyMetadata(NavigationViewBackButtonVisible.Auto, new PropertyChangedCallback(NavigationView.OnIsBackButtonVisiblePropertyChanged)));

		// Token: 0x040001F0 RID: 496
		public static readonly DependencyProperty IsBackEnabledProperty = DependencyProperty.Register("IsBackEnabled", typeof(bool), typeof(NavigationView), new PropertyMetadata(new PropertyChangedCallback(NavigationView.OnIsBackEnabledPropertyChanged)));

		// Token: 0x040001F1 RID: 497
		public static readonly DependencyProperty PaneTitleProperty = DependencyProperty.Register("PaneTitle", typeof(string), typeof(NavigationView), new PropertyMetadata(string.Empty, new PropertyChangedCallback(NavigationView.OnPaneTitlePropertyChanged)));

		// Token: 0x040001F2 RID: 498
		public static readonly DependencyProperty PaneDisplayModeProperty = DependencyProperty.Register("PaneDisplayMode", typeof(NavigationViewPaneDisplayMode), typeof(NavigationView), new PropertyMetadata(NavigationViewPaneDisplayMode.Auto, new PropertyChangedCallback(NavigationView.OnPaneDisplayModePropertyChanged)));

		// Token: 0x040001F3 RID: 499
		public static readonly DependencyProperty PaneHeaderProperty = DependencyProperty.Register("PaneHeader", typeof(UIElement), typeof(NavigationView), null);

		// Token: 0x040001F4 RID: 500
		public static readonly DependencyProperty PaneCustomContentProperty = DependencyProperty.Register("PaneCustomContent", typeof(UIElement), typeof(NavigationView), null);

		// Token: 0x040001F5 RID: 501
		public static readonly DependencyProperty ContentOverlayProperty = DependencyProperty.Register("ContentOverlay", typeof(UIElement), typeof(NavigationView), null);

		// Token: 0x040001F6 RID: 502
		public static readonly DependencyProperty IsPaneVisibleProperty = DependencyProperty.Register("IsPaneVisible", typeof(bool), typeof(NavigationView), new PropertyMetadata(true, new PropertyChangedCallback(NavigationView.OnIsPaneVisiblePropertyChanged)));

		// Token: 0x040001F7 RID: 503
		public static readonly DependencyProperty SelectionFollowsFocusProperty = DependencyProperty.Register("SelectionFollowsFocus", typeof(NavigationViewSelectionFollowsFocus), typeof(NavigationView), new PropertyMetadata(NavigationViewSelectionFollowsFocus.Disabled, new PropertyChangedCallback(NavigationView.OnSelectionFollowsFocusPropertyChanged)));

		// Token: 0x040001F8 RID: 504
		private static readonly DependencyPropertyKey TemplateSettingsPropertyKey = DependencyProperty.RegisterReadOnly("TemplateSettings", typeof(NavigationViewTemplateSettings), typeof(NavigationView), null);

		// Token: 0x040001F9 RID: 505
		public static readonly DependencyProperty TemplateSettingsProperty = NavigationView.TemplateSettingsPropertyKey.DependencyProperty;

		// Token: 0x040001FA RID: 506
		public static readonly DependencyProperty ShoulderNavigationEnabledProperty = DependencyProperty.Register("ShoulderNavigationEnabled", typeof(NavigationViewShoulderNavigationEnabled), typeof(NavigationView), new PropertyMetadata(NavigationViewShoulderNavigationEnabled.Never, new PropertyChangedCallback(NavigationView.OnShoulderNavigationEnabledPropertyChanged)));

		// Token: 0x040001FB RID: 507
		public static readonly DependencyProperty OverflowLabelModeProperty = DependencyProperty.Register("OverflowLabelMode", typeof(NavigationViewOverflowLabelMode), typeof(NavigationView), new PropertyMetadata(NavigationViewOverflowLabelMode.MoreLabel, new PropertyChangedCallback(NavigationView.OnOverflowLabelModePropertyChanged)));

		// Token: 0x040001FC RID: 508
		public static readonly DependencyProperty IsTitleBarAutoPaddingEnabledProperty = DependencyProperty.Register("IsTitleBarAutoPaddingEnabled", typeof(bool), typeof(NavigationView), new PropertyMetadata(true, new PropertyChangedCallback(NavigationView.OnIsTitleBarAutoPaddingEnabledPropertyChanged)));
	}
}
