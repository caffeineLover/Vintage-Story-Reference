using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Input;
using ModernWpf.Automation.Peers;
using ModernWpf.Controls.Primitives;
using ModernWpf.Input;

namespace ModernWpf.Controls
{
	// Token: 0x02000063 RID: 99
	public class NavigationViewItem : NavigationViewItemBase
	{
		// Token: 0x0600048D RID: 1165 RVA: 0x000119E8 File Offset: 0x0000FBE8
		static NavigationViewItem()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationViewItem), new FrameworkPropertyMetadata(typeof(NavigationViewItem)));
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x00011BF1 File Offset: 0x0000FDF1
		public NavigationViewItem()
		{
			base.SetValue(NavigationViewItem.MenuItemsPropertyKey, new ObservableCollection<object>());
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x00011C14 File Offset: 0x0000FE14
		internal void UpdateVisualStateNoTransition()
		{
			this.UpdateVisualState(false);
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x00011C1D File Offset: 0x0000FE1D
		private protected override void OnNavigationViewItemBaseDepthChanged()
		{
			this.UpdateItemIndentation();
			this.PropagateDepthToChildren(base.Depth + 1);
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x00011C33 File Offset: 0x0000FE33
		private protected override void OnNavigationViewItemBaseIsSelectedChanged()
		{
			this.UpdateVisualStateForPointer();
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x00011C3B File Offset: 0x0000FE3B
		private protected override void OnNavigationViewItemBasePositionChanged()
		{
			this.UpdateVisualStateNoTransition();
			this.ReparentRepeater();
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00011C4C File Offset: 0x0000FE4C
		public override void OnApplyTemplate()
		{
			this.m_appliedTemplate = false;
			this.UnhookEventsAndClearFields();
			base.OnApplyTemplate();
			this.m_helper.Init(this);
			Grid templateChildT = CppWinRTHelpers.GetTemplateChildT<Grid>("NVIRootGrid", this);
			if (templateChildT != null)
			{
				this.m_rootGrid = templateChildT;
				FlyoutBase attachedFlyout = FlyoutBase.GetAttachedFlyout(templateChildT);
				if (attachedFlyout != null)
				{
					this.m_flyoutClosingRevoker = new FlyoutBaseClosingRevoker(attachedFlyout, new TypedEventHandler<FlyoutBase, FlyoutBaseClosingEventArgs>(this.OnFlyoutClosing));
				}
			}
			this.HookInputEvents(this);
			base.IsEnabledChanged += new DependencyPropertyChangedEventHandler(this.OnIsEnabledChanged);
			this.m_toolTip = CppWinRTHelpers.GetTemplateChildT<ToolTip>("ToolTip", this);
			SplitView splitView = base.GetSplitView();
			if (splitView != null)
			{
				splitView.IsPaneOpenChanged += this.OnSplitViewPropertyChanged;
				splitView.DisplayModeChanged += this.OnSplitViewPropertyChanged;
				splitView.CompactPaneLengthChanged += this.OnSplitViewPropertyChanged;
				this.UpdateCompactPaneLength();
				this.UpdateIsClosedCompact();
			}
			NavigationView navigationView = base.GetNavigationView();
			if (navigationView != null)
			{
				ItemsRepeater templateChildT2 = CppWinRTHelpers.GetTemplateChildT<ItemsRepeater>("NavigationViewItemMenuItemsHost", this);
				if (templateChildT2 != null)
				{
					this.m_repeater = templateChildT2;
					this.m_repeaterElementPreparedRevoker = new ItemsRepeaterElementPreparedRevoker(templateChildT2, new TypedEventHandler<ItemsRepeater, ItemsRepeaterElementPreparedEventArgs>(navigationView.OnRepeaterElementPrepared));
					this.m_repeaterElementClearingRevoker = new ItemsRepeaterElementClearingRevoker(templateChildT2, new TypedEventHandler<ItemsRepeater, ItemsRepeaterElementClearingEventArgs>(navigationView.OnRepeaterElementClearing));
					templateChildT2.ItemTemplate = navigationView.GetNavigationViewItemsFactory();
				}
				this.UpdateRepeaterItemsSource();
			}
			FlyoutBase templateChildT3 = CppWinRTHelpers.GetTemplateChildT<FlyoutBase>("ChildrenFlyout", this);
			if (templateChildT3 != null)
			{
				templateChildT3.Offset = 0.0;
			}
			this.m_flyoutContentGrid = CppWinRTHelpers.GetTemplateChildT<Grid>("FlyoutContentGrid", this);
			this.m_appliedTemplate = true;
			this.UpdateItemIndentation();
			this.UpdateVisualStateNoTransition();
			this.ReparentRepeater();
			if (!this.ShouldRepeaterShowInFlyout())
			{
				this.ShowHideChildren();
			}
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00011DEC File Offset: 0x0000FFEC
		private void UpdateRepeaterItemsSource()
		{
			ItemsRepeater repeater = this.m_repeater;
			if (repeater != null)
			{
				object itemsSource = this.<UpdateRepeaterItemsSource>g__init|23_0();
				ItemsSourceView.CollectionChangedRevoker itemsSourceViewCollectionChangedRevoker = this.m_itemsSourceViewCollectionChangedRevoker;
				if (itemsSourceViewCollectionChangedRevoker != null)
				{
					itemsSourceViewCollectionChangedRevoker.Revoke();
				}
				repeater.ItemsSource = itemsSource;
				this.m_itemsSourceViewCollectionChangedRevoker = new ItemsSourceView.CollectionChangedRevoker(repeater.ItemsSourceView, new NotifyCollectionChangedEventHandler(this.OnItemsSourceViewChanged));
			}
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x00011E3F File Offset: 0x0001003F
		private void OnItemsSourceViewChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			this.UpdateVisualStateForChevron();
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x00011E48 File Offset: 0x00010048
		internal UIElement GetSelectionIndicator()
		{
			UIElement selectionIndicator = this.m_helper.GetSelectionIndicator();
			NavigationViewItemPresenter presenter = this.GetPresenter();
			if (presenter != null)
			{
				selectionIndicator = presenter.GetSelectionIndicator();
			}
			return selectionIndicator;
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x00011E73 File Offset: 0x00010073
		private void OnSplitViewPropertyChanged(DependencyObject sender, DependencyProperty args)
		{
			if (args == SplitView.CompactPaneLengthProperty)
			{
				this.UpdateCompactPaneLength();
				return;
			}
			if (args == SplitView.IsPaneOpenProperty || args == SplitView.DisplayModeProperty)
			{
				this.UpdateIsClosedCompact();
				this.ReparentRepeater();
			}
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x00011EA0 File Offset: 0x000100A0
		private void UpdateCompactPaneLength()
		{
			SplitView splitView = base.GetSplitView();
			if (splitView != null)
			{
				base.SetValue(NavigationViewItem.CompactPaneLengthPropertyKey, splitView.CompactPaneLength);
				NavigationViewItemPresenter presenter = this.GetPresenter();
				if (presenter != null)
				{
					presenter.UpdateCompactPaneLength(splitView.CompactPaneLength, this.IsOnLeftNav());
				}
			}
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x00011EEC File Offset: 0x000100EC
		internal void UpdateIsClosedCompact()
		{
			SplitView splitView = base.GetSplitView();
			if (splitView != null)
			{
				this.m_isClosedCompact = (!splitView.IsPaneOpen && (splitView.DisplayMode == SplitViewDisplayMode.CompactOverlay || splitView.DisplayMode == SplitViewDisplayMode.CompactInline));
				this.UpdateVisualState(true);
				NavigationViewItemPresenter presenter = this.GetPresenter();
				if (presenter != null)
				{
					presenter.UpdateClosedCompactVisualState(base.IsTopLevelItem, this.m_isClosedCompact);
				}
			}
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x00011F4C File Offset: 0x0001014C
		private void UpdateNavigationViewItemToolTip()
		{
			object toolTip = ToolTipService.GetToolTip(this);
			if (toolTip == null || toolTip == this.m_suggestedToolTipContent)
			{
				if (this.ShouldEnableToolTip())
				{
					ToolTipService.SetToolTip(this, this.m_suggestedToolTipContent);
					return;
				}
				ToolTipService.SetToolTip(this, null);
			}
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x00011F88 File Offset: 0x00010188
		private void SuggestedToolTipChanged(object newContent)
		{
			bool flag = newContent != null && newContent is string;
			object suggestedToolTipContent = null;
			if (flag)
			{
				suggestedToolTipContent = newContent;
			}
			object toolTip = ToolTipService.GetToolTip(this);
			object suggestedToolTipContent2 = this.m_suggestedToolTipContent;
			if (suggestedToolTipContent2 != null && suggestedToolTipContent2 == toolTip)
			{
				ToolTipService.SetToolTip(this, null);
			}
			this.m_suggestedToolTipContent = suggestedToolTipContent;
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x00011FD0 File Offset: 0x000101D0
		private void OnIsExpandedPropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			AutomationPeer automationPeer = UIElementAutomationPeer.FromElement(this);
			if (automationPeer != null)
			{
				((NavigationViewItemAutomationPeer)automationPeer).RaiseExpandCollapseAutomationEvent(this.IsExpanded ? 1 : 0);
			}
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x00011FFE File Offset: 0x000101FE
		private void OnIconPropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			this.UpdateVisualStateNoTransition();
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x00012006 File Offset: 0x00010206
		private void OnMenuItemsPropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			this.UpdateRepeaterItemsSource();
			this.UpdateVisualStateForChevron();
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x00012014 File Offset: 0x00010214
		private void OnMenuItemsSourcePropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			this.UpdateRepeaterItemsSource();
			this.UpdateVisualStateForChevron();
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x00012022 File Offset: 0x00010222
		private void OnHasUnrealizedChildrenPropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			this.UpdateVisualStateForChevron();
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x0001202C File Offset: 0x0001022C
		private void ShowSelectionIndicator(bool visible)
		{
			UIElement selectionIndicator = this.GetSelectionIndicator();
			if (selectionIndicator != null)
			{
				selectionIndicator.Opacity = (visible ? 1.0 : 0.0);
			}
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x00012060 File Offset: 0x00010260
		private void UpdateVisualStateForIconAndContent(bool showIcon, bool showContent)
		{
			NavigationViewItemPresenter navigationViewItemPresenter = this.m_navigationViewItemPresenter;
			if (navigationViewItemPresenter != null)
			{
				string text = showIcon ? (showContent ? "IconOnLeft" : "IconOnly") : "ContentOnly";
				VisualStateManager.GoToState(navigationViewItemPresenter, text, false);
			}
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x0001209C File Offset: 0x0001029C
		private void UpdateVisualStateForNavigationViewPositionChange()
		{
			NavigationViewRepeaterPosition position = base.Position;
			string text = "OnLeftNavigation";
			bool flag = false;
			switch (position)
			{
			case NavigationViewRepeaterPosition.LeftNav:
			case NavigationViewRepeaterPosition.LeftFooter:
				if (SharedHelpers.IsRS4OrHigher())
				{
				}
				break;
			case NavigationViewRepeaterPosition.TopPrimary:
			case NavigationViewRepeaterPosition.TopFooter:
				SharedHelpers.IsRS4OrHigher();
				text = "OnTopNavigationPrimary";
				break;
			case NavigationViewRepeaterPosition.TopOverflow:
				text = "OnTopNavigationOverflow";
				break;
			}
			if (!flag)
			{
				VisualStateManager.GoToState(this, text, false);
			}
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x00012100 File Offset: 0x00010300
		private void UpdateVisualStateForKeyboardFocusedState()
		{
			string text = "KeyboardNormal";
			if (this.m_hasKeyboardFocus)
			{
				text = "KeyboardFocused";
			}
			VisualStateManager.GoToState(this, text, false);
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x0001212C File Offset: 0x0001032C
		private void UpdateVisualStateForToolTip()
		{
			ToolTip toolTip = this.m_toolTip;
			if (toolTip == null)
			{
				this.UpdateNavigationViewItemToolTip();
				return;
			}
			bool flag = this.ShouldEnableToolTip();
			object suggestedToolTipContent = this.m_suggestedToolTipContent;
			if (flag && suggestedToolTipContent != null)
			{
				toolTip.Content = suggestedToolTipContent;
				toolTip.IsEnabled = true;
				return;
			}
			toolTip.Content = null;
			toolTip.IsEnabled = false;
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x0001217C File Offset: 0x0001037C
		private void UpdateVisualStateForPointer()
		{
			string text = "Enabled";
			bool isSelected = base.IsSelected;
			string text2 = "Normal";
			if (base.IsEnabled)
			{
				if (isSelected)
				{
					if (this.m_isPressed)
					{
						text2 = "PressedSelected";
					}
					else if (this.m_isPointerOver)
					{
						text2 = "PointerOverSelected";
					}
					else
					{
						text2 = "Selected";
					}
				}
				else if (this.m_isPointerOver)
				{
					if (this.m_isPressed)
					{
						text2 = "Pressed";
					}
					else
					{
						text2 = "PointerOver";
					}
				}
				else if (this.m_isPressed)
				{
					text2 = "Pressed";
				}
			}
			else
			{
				text = "Disabled";
				if (isSelected)
				{
					text2 = "Selected";
				}
			}
			NavigationViewItemPresenter navigationViewItemPresenter = this.m_navigationViewItemPresenter;
			if (navigationViewItemPresenter != null)
			{
				VisualStateManager.GoToState(navigationViewItemPresenter, text, true);
				VisualStateManager.GoToState(navigationViewItemPresenter, text2, true);
				return;
			}
			VisualStateManager.GoToState(this, text, true);
			VisualStateManager.GoToState(this, text2, true);
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x00012240 File Offset: 0x00010440
		private void UpdateVisualState(bool useTransitions)
		{
			if (!this.m_appliedTemplate)
			{
				return;
			}
			this.UpdateVisualStateForPointer();
			this.UpdateVisualStateForNavigationViewPositionChange();
			bool flag = this.ShouldShowIcon();
			bool showContent = this.ShouldShowContent();
			if (this.IsOnLeftNav())
			{
				NavigationViewItemPresenter navigationViewItemPresenter = this.m_navigationViewItemPresenter;
				if (navigationViewItemPresenter != null)
				{
					VisualStateManager.GoToState(navigationViewItemPresenter, flag ? "IconVisible" : "IconCollapsed", useTransitions);
				}
			}
			this.UpdateVisualStateForToolTip();
			this.UpdateVisualStateForIconAndContent(flag, showContent);
			this.UpdateVisualStateForKeyboardFocusedState();
			this.UpdateVisualStateForChevron();
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x000122B4 File Offset: 0x000104B4
		private void UpdateVisualStateForChevron()
		{
			NavigationViewItemPresenter navigationViewItemPresenter = this.m_navigationViewItemPresenter;
			if (navigationViewItemPresenter != null)
			{
				string text = (this.HasChildren() && (!this.m_isClosedCompact || !this.ShouldRepeaterShowInFlyout())) ? (this.IsExpanded ? "ChevronVisibleOpen" : "ChevronVisibleClosed") : "ChevronHidden";
				VisualStateManager.GoToState(navigationViewItemPresenter, text, true);
			}
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x00012308 File Offset: 0x00010508
		internal bool HasChildren()
		{
			return this.MenuItems.Count > 0 || (this.MenuItemsSource != null && this.m_repeater != null && this.m_repeater.ItemsSourceView.Count > 0) || this.HasUnrealizedChildren;
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x00012343 File Offset: 0x00010543
		private bool ShouldShowIcon()
		{
			return this.Icon != null;
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x0001234E File Offset: 0x0001054E
		private bool ShouldEnableToolTip()
		{
			return this.IsOnLeftNav() && this.m_isClosedCompact;
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x00012360 File Offset: 0x00010560
		private bool ShouldShowContent()
		{
			return base.Content != null;
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x0001236C File Offset: 0x0001056C
		private bool IsOnLeftNav()
		{
			NavigationViewRepeaterPosition position = base.Position;
			return position == NavigationViewRepeaterPosition.LeftNav || position == NavigationViewRepeaterPosition.LeftFooter;
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x00012389 File Offset: 0x00010589
		private bool IsOnTopPrimary()
		{
			return base.Position == NavigationViewRepeaterPosition.TopPrimary;
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x00012394 File Offset: 0x00010594
		private UIElement GetPresenterOrItem()
		{
			NavigationViewItemPresenter navigationViewItemPresenter = this.m_navigationViewItemPresenter;
			if (navigationViewItemPresenter != null)
			{
				return navigationViewItemPresenter;
			}
			return this;
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x000123B0 File Offset: 0x000105B0
		private NavigationViewItemPresenter GetPresenter()
		{
			NavigationViewItemPresenter result = null;
			if (this.m_navigationViewItemPresenter != null)
			{
				result = this.m_navigationViewItemPresenter;
			}
			return result;
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x000123CF File Offset: 0x000105CF
		internal ItemsRepeater GetRepeater()
		{
			return this.m_repeater;
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x000123D8 File Offset: 0x000105D8
		internal void ShowHideChildren()
		{
			ItemsRepeater repeater = this.m_repeater;
			if (repeater != null)
			{
				bool isExpanded = this.IsExpanded;
				Visibility visibility = isExpanded ? 0 : 2;
				repeater.Visibility = visibility;
				if (this.ShouldRepeaterShowInFlyout())
				{
					if (isExpanded)
					{
						if (!this.m_isRepeaterParentedToFlyout)
						{
							this.ReparentRepeater();
						}
						SharedHelpers.QueueCallbackForCompositionRendering(delegate
						{
							FlyoutBase.ShowAttachedFlyout(this.m_rootGrid);
						});
						return;
					}
					FlyoutBase attachedFlyout = FlyoutBase.GetAttachedFlyout(this.m_rootGrid);
					if (attachedFlyout != null)
					{
						attachedFlyout.Hide();
					}
				}
			}
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x00012448 File Offset: 0x00010648
		private void ReparentRepeater()
		{
			if (this.HasChildren())
			{
				ItemsRepeater repeater = this.m_repeater;
				if (repeater != null)
				{
					if (this.ShouldRepeaterShowInFlyout() && !this.m_isRepeaterParentedToFlyout)
					{
						this.m_rootGrid.Children.Remove(repeater);
						this.m_flyoutContentGrid.Children.Add(repeater);
						this.m_isRepeaterParentedToFlyout = true;
						this.PropagateDepthToChildren(0);
						return;
					}
					if (!this.ShouldRepeaterShowInFlyout() && this.m_isRepeaterParentedToFlyout)
					{
						this.m_flyoutContentGrid.Children.Remove(repeater);
						this.m_rootGrid.Children.Add(repeater);
						this.m_isRepeaterParentedToFlyout = false;
						this.PropagateDepthToChildren(1);
					}
				}
			}
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x000124F0 File Offset: 0x000106F0
		internal bool ShouldRepeaterShowInFlyout()
		{
			return (this.m_isClosedCompact && base.IsTopLevelItem) || this.IsOnTopPrimary();
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x0001250C File Offset: 0x0001070C
		internal bool IsRepeaterVisible()
		{
			ItemsRepeater repeater = this.m_repeater;
			return repeater != null && repeater.Visibility == 0;
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x00012530 File Offset: 0x00010730
		private void UpdateItemIndentation()
		{
			NavigationViewItemPresenter navigationViewItemPresenter = this.m_navigationViewItemPresenter;
			if (navigationViewItemPresenter != null)
			{
				int num = base.Depth * 25;
				navigationViewItemPresenter.UpdateContentLeftIndentation((double)num);
			}
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x0001255C File Offset: 0x0001075C
		internal void PropagateDepthToChildren(int depth)
		{
			ItemsRepeater repeater = this.m_repeater;
			if (repeater != null)
			{
				int count = repeater.ItemsSourceView.Count;
				for (int i = 0; i < count; i++)
				{
					UIElement uielement = repeater.TryGetElement(i);
					if (uielement != null)
					{
						NavigationViewItemBase navigationViewItemBase = uielement as NavigationViewItemBase;
						if (navigationViewItemBase != null)
						{
							navigationViewItemBase.Depth = depth;
						}
					}
				}
			}
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x000125AA File Offset: 0x000107AA
		internal void OnExpandCollapseChevronTapped(object sender, TappedRoutedEventArgs args)
		{
			this.IsExpanded = !this.IsExpanded;
			args.Handled = true;
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x000125C2 File Offset: 0x000107C2
		private void OnFlyoutClosing(object sender, FlyoutBaseClosingEventArgs args)
		{
			this.IsExpanded = false;
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x000125CB File Offset: 0x000107CB
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new NavigationViewItemAutomationPeer(this);
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x000125D4 File Offset: 0x000107D4
		protected override void OnContentChanged(object oldContent, object newContent)
		{
			base.OnContentChanged(oldContent, newContent);
			this.SuggestedToolTipChanged(newContent);
			this.UpdateVisualStateNoTransition();
			if (!this.IsOnLeftNav())
			{
				NavigationView navigationView = base.GetNavigationView();
				if (navigationView != null)
				{
					navigationView.TopNavigationViewItemContentChanged();
				}
			}
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x00012610 File Offset: 0x00010810
		protected override void OnGotFocus(RoutedEventArgs e)
		{
			base.OnGotFocus(e);
			Control control = e.OriginalSource as Control;
			if (control != null && control.IsKeyboardFocused && InputManager.Current.MostRecentInputDevice is KeyboardDevice)
			{
				this.m_hasKeyboardFocus = true;
				this.UpdateVisualStateNoTransition();
			}
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x00012659 File Offset: 0x00010859
		protected override void OnLostFocus(RoutedEventArgs e)
		{
			base.OnLostFocus(e);
			if (this.m_hasKeyboardFocus)
			{
				this.m_hasKeyboardFocus = false;
				this.UpdateVisualStateNoTransition();
			}
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x00012677 File Offset: 0x00010877
		private void OnPresenterPointerPressed(object sender, MouseEventArgs args)
		{
			this.m_isPressed = (args.LeftButton == 1 || args.RightButton == 1);
			if (this.GetPresenterOrItem().CaptureMouse())
			{
				this.m_isMouseCaptured = true;
			}
			this.UpdateVisualState(true);
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x000126AF File Offset: 0x000108AF
		private void OnPresenterPointerReleased(object sender, MouseEventArgs args)
		{
			if (this.m_isPressed)
			{
				this.m_isPressed = false;
				if (this.m_isMouseCaptured)
				{
					this.GetPresenterOrItem().ReleaseMouseCapture();
				}
			}
			this.UpdateVisualState(true);
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x000126DA File Offset: 0x000108DA
		private void OnPresenterPointerEntered(object sender, MouseEventArgs args)
		{
			this.ProcessPointerOver(args);
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x000126E3 File Offset: 0x000108E3
		private void OnPresenterPointerMoved(object sender, MouseEventArgs args)
		{
			this.ProcessPointerOver(args);
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x000126EC File Offset: 0x000108EC
		private void OnPresenterPointerExited(object sender, MouseEventArgs args)
		{
			this.m_isPointerOver = false;
			this.UpdateVisualState(true);
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x000126FC File Offset: 0x000108FC
		private void OnPresenterPointerCanceled(object sender, MouseEventArgs args)
		{
			this.ProcessPointerCanceled(args);
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x00012705 File Offset: 0x00010905
		private void OnPresenterPointerCaptureLost(object sender, MouseEventArgs args)
		{
			this.ProcessPointerCanceled(args);
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x0001270E File Offset: 0x0001090E
		private void OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs args)
		{
			if (!base.IsEnabled)
			{
				this.m_isPressed = false;
				this.m_isPointerOver = false;
				if (this.m_isMouseCaptured)
				{
					this.GetPresenterOrItem().ReleaseMouseCapture();
					this.m_isMouseCaptured = false;
				}
			}
			this.UpdateVisualState(true);
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x00012748 File Offset: 0x00010948
		internal void RotateExpandCollapseChevron(bool isExpanded)
		{
			NavigationViewItemPresenter presenter = this.GetPresenter();
			if (presenter != null)
			{
				presenter.RotateExpandCollapseChevron(isExpanded);
			}
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x00012766 File Offset: 0x00010966
		private void ProcessPointerCanceled(MouseEventArgs args)
		{
			this.m_isPressed = false;
			this.m_isPointerOver = false;
			this.m_isMouseCaptured = false;
			this.UpdateVisualState(true);
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x00012784 File Offset: 0x00010984
		private void ProcessPointerOver(MouseEventArgs args)
		{
			if (!this.m_isPointerOver)
			{
				this.m_isPointerOver = true;
				this.UpdateVisualState(true);
			}
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x0001279C File Offset: 0x0001099C
		private void HookInputEvents(IControlProtected controlProtected)
		{
			NavigationViewItem.<>c__DisplayClass76_0 CS$<>8__locals1;
			CS$<>8__locals1.controlProtected = controlProtected;
			CS$<>8__locals1.<>4__this = this;
			UIElement uielement = this.<HookInputEvents>g__init|76_0(ref CS$<>8__locals1);
			uielement.MouseDown += new MouseButtonEventHandler(this.OnPresenterPointerPressed);
			uielement.MouseEnter += new MouseEventHandler(this.OnPresenterPointerEntered);
			uielement.MouseMove += new MouseEventHandler(this.OnPresenterPointerMoved);
			uielement.AddHandler(UIElement.MouseUpEvent, new MouseButtonEventHandler(this.OnPresenterPointerReleased), true);
			uielement.AddHandler(UIElement.MouseLeaveEvent, new MouseEventHandler(this.OnPresenterPointerExited), true);
			uielement.AddHandler(UIElement.LostMouseCaptureEvent, new MouseEventHandler(this.OnPresenterPointerCaptureLost), true);
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x00012840 File Offset: 0x00010A40
		private void UnhookInputEvents()
		{
			NavigationViewItemPresenter navigationViewItemPresenter = this.m_navigationViewItemPresenter ?? this;
			navigationViewItemPresenter.MouseDown -= new MouseButtonEventHandler(this.OnPresenterPointerPressed);
			navigationViewItemPresenter.MouseEnter -= new MouseEventHandler(this.OnPresenterPointerEntered);
			navigationViewItemPresenter.MouseMove -= new MouseEventHandler(this.OnPresenterPointerMoved);
			navigationViewItemPresenter.RemoveHandler(UIElement.MouseUpEvent, new MouseButtonEventHandler(this.OnPresenterPointerReleased));
			navigationViewItemPresenter.RemoveHandler(UIElement.MouseLeaveEvent, new MouseEventHandler(this.OnPresenterPointerExited));
			navigationViewItemPresenter.RemoveHandler(UIElement.LostMouseCaptureEvent, new MouseEventHandler(this.OnPresenterPointerCaptureLost));
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x000128D4 File Offset: 0x00010AD4
		private void UnhookEventsAndClearFields()
		{
			this.UnhookInputEvents();
			FlyoutBaseClosingRevoker flyoutClosingRevoker = this.m_flyoutClosingRevoker;
			if (flyoutClosingRevoker != null)
			{
				flyoutClosingRevoker.Revoke();
			}
			SplitViewIsPaneOpenChangedRevoker splitViewIsPaneOpenChangedRevoker = this.m_splitViewIsPaneOpenChangedRevoker;
			if (splitViewIsPaneOpenChangedRevoker != null)
			{
				splitViewIsPaneOpenChangedRevoker.Revoke();
			}
			SplitViewDisplayModeChangedRevoker splitViewDisplayModeChangedRevoker = this.m_splitViewDisplayModeChangedRevoker;
			if (splitViewDisplayModeChangedRevoker != null)
			{
				splitViewDisplayModeChangedRevoker.Revoke();
			}
			SplitViewCompactPaneLengthChangedRevoker splitViewCompactPaneLengthChangedRevoker = this.m_splitViewCompactPaneLengthChangedRevoker;
			if (splitViewCompactPaneLengthChangedRevoker != null)
			{
				splitViewCompactPaneLengthChangedRevoker.Revoke();
			}
			ItemsRepeaterElementPreparedRevoker repeaterElementPreparedRevoker = this.m_repeaterElementPreparedRevoker;
			if (repeaterElementPreparedRevoker != null)
			{
				repeaterElementPreparedRevoker.Revoke();
			}
			ItemsRepeaterElementClearingRevoker repeaterElementClearingRevoker = this.m_repeaterElementClearingRevoker;
			if (repeaterElementClearingRevoker != null)
			{
				repeaterElementClearingRevoker.Revoke();
			}
			base.IsEnabledChanged -= new DependencyPropertyChangedEventHandler(this.OnIsEnabledChanged);
			ItemsSourceView.CollectionChangedRevoker itemsSourceViewCollectionChangedRevoker = this.m_itemsSourceViewCollectionChangedRevoker;
			if (itemsSourceViewCollectionChangedRevoker != null)
			{
				itemsSourceViewCollectionChangedRevoker.Revoke();
			}
			this.m_rootGrid = null;
			this.m_navigationViewItemPresenter = null;
			this.m_toolTip = null;
			this.m_repeater = null;
			this.m_flyoutContentGrid = null;
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060004CC RID: 1228 RVA: 0x00012993 File Offset: 0x00010B93
		// (set) Token: 0x060004CD RID: 1229 RVA: 0x000129A5 File Offset: 0x00010BA5
		public IconElement Icon
		{
			get
			{
				return (IconElement)base.GetValue(NavigationViewItem.IconProperty);
			}
			set
			{
				base.SetValue(NavigationViewItem.IconProperty, value);
			}
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x000129B3 File Offset: 0x00010BB3
		private static void OnIconPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NavigationViewItem)sender).OnIconPropertyChanged(args);
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060004CF RID: 1231 RVA: 0x000129C1 File Offset: 0x00010BC1
		// (set) Token: 0x060004D0 RID: 1232 RVA: 0x000129D3 File Offset: 0x00010BD3
		public double CompactPaneLength
		{
			get
			{
				return (double)base.GetValue(NavigationViewItem.CompactPaneLengthProperty);
			}
			private set
			{
				base.SetValue(NavigationViewItem.CompactPaneLengthPropertyKey, value);
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060004D1 RID: 1233 RVA: 0x000129E6 File Offset: 0x00010BE6
		// (set) Token: 0x060004D2 RID: 1234 RVA: 0x000129F8 File Offset: 0x00010BF8
		public bool SelectsOnInvoked
		{
			get
			{
				return (bool)base.GetValue(NavigationViewItem.SelectsOnInvokedProperty);
			}
			set
			{
				base.SetValue(NavigationViewItem.SelectsOnInvokedProperty, value);
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060004D3 RID: 1235 RVA: 0x00012A0B File Offset: 0x00010C0B
		// (set) Token: 0x060004D4 RID: 1236 RVA: 0x00012A1D File Offset: 0x00010C1D
		public bool IsExpanded
		{
			get
			{
				return (bool)base.GetValue(NavigationViewItem.IsExpandedProperty);
			}
			set
			{
				base.SetValue(NavigationViewItem.IsExpandedProperty, value);
			}
		}

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x060004D5 RID: 1237 RVA: 0x00012A30 File Offset: 0x00010C30
		// (remove) Token: 0x060004D6 RID: 1238 RVA: 0x00012A68 File Offset: 0x00010C68
		internal event DependencyPropertyChangedCallback IsExpandedChanged;

		// Token: 0x060004D7 RID: 1239 RVA: 0x00012AA0 File Offset: 0x00010CA0
		private static void OnIsExpandedPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			NavigationViewItem navigationViewItem = (NavigationViewItem)sender;
			navigationViewItem.OnIsExpandedPropertyChanged(args);
			DependencyPropertyChangedCallback isExpandedChanged = navigationViewItem.IsExpandedChanged;
			if (isExpandedChanged == null)
			{
				return;
			}
			isExpandedChanged(navigationViewItem, args.Property);
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060004D8 RID: 1240 RVA: 0x00012AD3 File Offset: 0x00010CD3
		// (set) Token: 0x060004D9 RID: 1241 RVA: 0x00012AE5 File Offset: 0x00010CE5
		public bool HasUnrealizedChildren
		{
			get
			{
				return (bool)base.GetValue(NavigationViewItem.HasUnrealizedChildrenProperty);
			}
			set
			{
				base.SetValue(NavigationViewItem.HasUnrealizedChildrenProperty, value);
			}
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x00012AF8 File Offset: 0x00010CF8
		private static void OnHasUnrealizedChildrenPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NavigationViewItem)sender).OnHasUnrealizedChildrenPropertyChanged(args);
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060004DB RID: 1243 RVA: 0x00012B06 File Offset: 0x00010D06
		// (set) Token: 0x060004DC RID: 1244 RVA: 0x00012B18 File Offset: 0x00010D18
		public bool IsChildSelected
		{
			get
			{
				return (bool)base.GetValue(NavigationViewItem.IsChildSelectedProperty);
			}
			set
			{
				base.SetValue(NavigationViewItem.IsChildSelectedProperty, value);
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x00012B2B File Offset: 0x00010D2B
		public IList MenuItems
		{
			get
			{
				return (IList)base.GetValue(NavigationViewItem.MenuItemsProperty);
			}
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x00012B3D File Offset: 0x00010D3D
		private static void OnMenuItemsPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NavigationViewItem)sender).OnMenuItemsPropertyChanged(args);
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060004DF RID: 1247 RVA: 0x00012B4B File Offset: 0x00010D4B
		// (set) Token: 0x060004E0 RID: 1248 RVA: 0x00012B58 File Offset: 0x00010D58
		public object MenuItemsSource
		{
			get
			{
				return base.GetValue(NavigationViewItem.MenuItemsSourceProperty);
			}
			set
			{
				base.SetValue(NavigationViewItem.MenuItemsSourceProperty, value);
			}
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x00012B66 File Offset: 0x00010D66
		private static void OnMenuItemsSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NavigationViewItem)sender).OnMenuItemsSourcePropertyChanged(args);
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060004E2 RID: 1250 RVA: 0x00012B74 File Offset: 0x00010D74
		// (set) Token: 0x060004E3 RID: 1251 RVA: 0x00012B86 File Offset: 0x00010D86
		public CornerRadius CornerRadius
		{
			get
			{
				return (CornerRadius)base.GetValue(NavigationViewItem.CornerRadiusProperty);
			}
			set
			{
				base.SetValue(NavigationViewItem.CornerRadiusProperty, value);
			}
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x00012B9C File Offset: 0x00010D9C
		[CompilerGenerated]
		private object <UpdateRepeaterItemsSource>g__init|23_0()
		{
			object menuItemsSource = this.MenuItemsSource;
			if (menuItemsSource != null)
			{
				return menuItemsSource;
			}
			return this.MenuItems;
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x00012BC8 File Offset: 0x00010DC8
		[CompilerGenerated]
		private UIElement <HookInputEvents>g__init|76_0(ref NavigationViewItem.<>c__DisplayClass76_0 A_1)
		{
			NavigationViewItemPresenter templateChildT = CppWinRTHelpers.GetTemplateChildT<NavigationViewItemPresenter>("NavigationViewItemPresenter", A_1.controlProtected);
			if (templateChildT != null)
			{
				this.m_navigationViewItemPresenter = templateChildT;
				return templateChildT;
			}
			return this;
		}

		// Token: 0x0400021F RID: 543
		private const string c_navigationViewItemPresenterName = "NavigationViewItemPresenter";

		// Token: 0x04000220 RID: 544
		private const string c_repeater = "NavigationViewItemMenuItemsHost";

		// Token: 0x04000221 RID: 545
		private const string c_rootGrid = "NVIRootGrid";

		// Token: 0x04000222 RID: 546
		private const string c_childrenFlyout = "ChildrenFlyout";

		// Token: 0x04000223 RID: 547
		private const string c_flyoutContentGrid = "FlyoutContentGrid";

		// Token: 0x04000224 RID: 548
		private const string c_pressedSelected = "PressedSelected";

		// Token: 0x04000225 RID: 549
		private const string c_pointerOverSelected = "PointerOverSelected";

		// Token: 0x04000226 RID: 550
		private const string c_selected = "Selected";

		// Token: 0x04000227 RID: 551
		private const string c_pressed = "Pressed";

		// Token: 0x04000228 RID: 552
		private const string c_pointerOver = "PointerOver";

		// Token: 0x04000229 RID: 553
		private const string c_disabled = "Disabled";

		// Token: 0x0400022A RID: 554
		private const string c_enabled = "Enabled";

		// Token: 0x0400022B RID: 555
		private const string c_normal = "Normal";

		// Token: 0x0400022C RID: 556
		private const string c_chevronHidden = "ChevronHidden";

		// Token: 0x0400022D RID: 557
		private const string c_chevronVisibleOpen = "ChevronVisibleOpen";

		// Token: 0x0400022E RID: 558
		private const string c_chevronVisibleClosed = "ChevronVisibleClosed";

		// Token: 0x0400022F RID: 559
		private SplitViewIsPaneOpenChangedRevoker m_splitViewIsPaneOpenChangedRevoker;

		// Token: 0x04000230 RID: 560
		private SplitViewDisplayModeChangedRevoker m_splitViewDisplayModeChangedRevoker;

		// Token: 0x04000231 RID: 561
		private SplitViewCompactPaneLengthChangedRevoker m_splitViewCompactPaneLengthChangedRevoker;

		// Token: 0x04000232 RID: 562
		private ItemsRepeaterElementPreparedRevoker m_repeaterElementPreparedRevoker;

		// Token: 0x04000233 RID: 563
		private ItemsRepeaterElementClearingRevoker m_repeaterElementClearingRevoker;

		// Token: 0x04000234 RID: 564
		private ItemsSourceView.CollectionChangedRevoker m_itemsSourceViewCollectionChangedRevoker;

		// Token: 0x04000235 RID: 565
		private FlyoutBaseClosingRevoker m_flyoutClosingRevoker;

		// Token: 0x04000236 RID: 566
		private ToolTip m_toolTip;

		// Token: 0x04000237 RID: 567
		private NavigationViewItemHelper<NavigationViewItem> m_helper = new NavigationViewItemHelper<NavigationViewItem>();

		// Token: 0x04000238 RID: 568
		private NavigationViewItemPresenter m_navigationViewItemPresenter;

		// Token: 0x04000239 RID: 569
		private object m_suggestedToolTipContent;

		// Token: 0x0400023A RID: 570
		private ItemsRepeater m_repeater;

		// Token: 0x0400023B RID: 571
		private Grid m_flyoutContentGrid;

		// Token: 0x0400023C RID: 572
		private Grid m_rootGrid;

		// Token: 0x0400023D RID: 573
		private bool m_isClosedCompact;

		// Token: 0x0400023E RID: 574
		private bool m_appliedTemplate;

		// Token: 0x0400023F RID: 575
		private bool m_hasKeyboardFocus;

		// Token: 0x04000240 RID: 576
		private bool m_isMouseCaptured;

		// Token: 0x04000241 RID: 577
		private bool m_isPressed;

		// Token: 0x04000242 RID: 578
		private bool m_isPointerOver;

		// Token: 0x04000243 RID: 579
		private bool m_isRepeaterParentedToFlyout;

		// Token: 0x04000244 RID: 580
		public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(IconElement), typeof(NavigationViewItem), new PropertyMetadata(new PropertyChangedCallback(NavigationViewItem.OnIconPropertyChanged)));

		// Token: 0x04000245 RID: 581
		private static readonly DependencyPropertyKey CompactPaneLengthPropertyKey = DependencyProperty.RegisterReadOnly("CompactPaneLength", typeof(double), typeof(NavigationViewItem), new PropertyMetadata(48.0));

		// Token: 0x04000246 RID: 582
		public static readonly DependencyProperty CompactPaneLengthProperty = NavigationViewItem.CompactPaneLengthPropertyKey.DependencyProperty;

		// Token: 0x04000247 RID: 583
		public static readonly DependencyProperty SelectsOnInvokedProperty = DependencyProperty.Register("SelectsOnInvoked", typeof(bool), typeof(NavigationViewItem), new PropertyMetadata(true));

		// Token: 0x04000248 RID: 584
		public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(NavigationViewItem), new PropertyMetadata(false, new PropertyChangedCallback(NavigationViewItem.OnIsExpandedPropertyChanged)));

		// Token: 0x0400024A RID: 586
		public static readonly DependencyProperty HasUnrealizedChildrenProperty = DependencyProperty.Register("HasUnrealizedChildren", typeof(bool), typeof(NavigationViewItem), new PropertyMetadata(false, new PropertyChangedCallback(NavigationViewItem.OnHasUnrealizedChildrenPropertyChanged)));

		// Token: 0x0400024B RID: 587
		public static readonly DependencyProperty IsChildSelectedProperty = DependencyProperty.Register("IsChildSelected", typeof(bool), typeof(NavigationViewItem), new PropertyMetadata(false));

		// Token: 0x0400024C RID: 588
		private static readonly DependencyPropertyKey MenuItemsPropertyKey = DependencyProperty.RegisterReadOnly("MenuItems", typeof(IList), typeof(NavigationViewItem), new PropertyMetadata(new PropertyChangedCallback(NavigationViewItem.OnMenuItemsPropertyChanged)));

		// Token: 0x0400024D RID: 589
		public static readonly DependencyProperty MenuItemsProperty = NavigationViewItem.MenuItemsPropertyKey.DependencyProperty;

		// Token: 0x0400024E RID: 590
		public static readonly DependencyProperty MenuItemsSourceProperty = DependencyProperty.Register("MenuItemsSource", typeof(object), typeof(NavigationViewItem), new PropertyMetadata(new PropertyChangedCallback(NavigationViewItem.OnMenuItemsSourcePropertyChanged)));

		// Token: 0x0400024F RID: 591
		public static readonly DependencyProperty CornerRadiusProperty = ControlHelper.CornerRadiusProperty.AddOwner(typeof(NavigationViewItem));
	}
}
