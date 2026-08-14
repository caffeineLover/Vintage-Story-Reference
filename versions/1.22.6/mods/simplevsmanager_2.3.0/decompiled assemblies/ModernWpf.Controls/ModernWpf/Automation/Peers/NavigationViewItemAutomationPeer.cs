using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using ModernWpf.Controls;

namespace ModernWpf.Automation.Peers
{
	// Token: 0x0200000B RID: 11
	public class NavigationViewItemAutomationPeer : FrameworkElementAutomationPeer, IInvokeProvider, ISelectionItemProvider, IExpandCollapseProvider
	{
		// Token: 0x0600002A RID: 42 RVA: 0x00002380 File Offset: 0x00000580
		public NavigationViewItemAutomationPeer(NavigationViewItem owner) : base(owner)
		{
		}

		// Token: 0x0600002B RID: 43 RVA: 0x0000238C File Offset: 0x0000058C
		protected override string GetNameCore()
		{
			string text = base.GetNameCore();
			if (string.IsNullOrEmpty(text))
			{
				NavigationViewItem navigationViewItem = base.Owner as NavigationViewItem;
				if (navigationViewItem != null)
				{
					text = SharedHelpers.TryGetStringRepresentationFromObject(navigationViewItem.Content);
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				text = NavigationViewItemAutomationPeer.ResourceAccessor.GetLocalizedStringResource("NavigationViewItemDefaultControlName");
			}
			return text;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000023DC File Offset: 0x000005DC
		public override object GetPattern(PatternInterface pattern)
		{
			if (pattern == 11 || (pattern == 6 && this.HasChildren()))
			{
				return this;
			}
			return base.GetPattern(pattern);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000023F8 File Offset: 0x000005F8
		protected override string GetClassNameCore()
		{
			return "NavigationViewItem";
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000023FF File Offset: 0x000005FF
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			if (this.IsOnTopNavigation())
			{
				return 19;
			}
			return 7;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002410 File Offset: 0x00000610
		protected override int GetPositionInSetCore()
		{
			int result;
			if (this.IsOnTopNavigation() && !this.IsOnFooterNavigation())
			{
				result = this.GetPositionOrSetCountInTopNavHelper(NavigationViewItemAutomationPeer.AutomationOutput.Position);
			}
			else
			{
				result = this.GetPositionOrSetCountInLeftNavHelper(NavigationViewItemAutomationPeer.AutomationOutput.Position);
			}
			return result;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002444 File Offset: 0x00000644
		protected override int GetSizeOfSetCore()
		{
			int result = 0;
			if (this.IsOnTopNavigation() && !this.IsOnFooterNavigation())
			{
				if (this.GetParentNavigationView() != null)
				{
					result = this.GetPositionOrSetCountInTopNavHelper(NavigationViewItemAutomationPeer.AutomationOutput.Size);
				}
			}
			else
			{
				result = this.GetPositionOrSetCountInLeftNavHelper(NavigationViewItemAutomationPeer.AutomationOutput.Size);
			}
			return result;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002480 File Offset: 0x00000680
		void IInvokeProvider.Invoke()
		{
			NavigationView parentNavigationView = this.GetParentNavigationView();
			if (parentNavigationView != null)
			{
				NavigationViewItem navigationViewItem = base.Owner as NavigationViewItem;
				if (navigationViewItem != null)
				{
					if (navigationViewItem == parentNavigationView.SettingsItem)
					{
						parentNavigationView.OnSettingsInvoked();
						return;
					}
					parentNavigationView.OnNavigationViewItemInvoked(navigationViewItem);
				}
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000032 RID: 50 RVA: 0x000024C0 File Offset: 0x000006C0
		ExpandCollapseState IExpandCollapseProvider.ExpandCollapseState
		{
			get
			{
				ExpandCollapseState result = 3;
				NavigationViewItem navigationViewItem = base.Owner as NavigationViewItem;
				if (navigationViewItem != null)
				{
					result = (navigationViewItem.IsExpanded ? 1 : 0);
				}
				return result;
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000024EC File Offset: 0x000006EC
		void IExpandCollapseProvider.Collapse()
		{
			NavigationView parentNavigationView = this.GetParentNavigationView();
			if (parentNavigationView != null)
			{
				NavigationViewItem navigationViewItem = base.Owner as NavigationViewItem;
				if (navigationViewItem != null)
				{
					parentNavigationView.Collapse(navigationViewItem);
					this.RaiseExpandCollapseAutomationEvent(0);
				}
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002520 File Offset: 0x00000720
		void IExpandCollapseProvider.Expand()
		{
			NavigationView parentNavigationView = this.GetParentNavigationView();
			if (parentNavigationView != null)
			{
				NavigationViewItem navigationViewItem = base.Owner as NavigationViewItem;
				if (navigationViewItem != null)
				{
					parentNavigationView.Expand(navigationViewItem);
					this.RaiseExpandCollapseAutomationEvent(1);
				}
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002554 File Offset: 0x00000754
		internal void RaiseExpandCollapseAutomationEvent(ExpandCollapseState newState)
		{
			if (AutomationPeer.ListenerExists(13))
			{
				ExpandCollapseState expandCollapseState = (newState == 1) ? 0 : 1;
				base.RaisePropertyChangedEvent(ExpandCollapsePatternIdentifiers.ExpandCollapseStateProperty, expandCollapseState, newState);
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x0000258C File Offset: 0x0000078C
		private NavigationView GetParentNavigationView()
		{
			NavigationView result = null;
			NavigationViewItemBase navigationViewItemBase = base.Owner as NavigationViewItemBase;
			if (navigationViewItemBase != null)
			{
				result = navigationViewItemBase.GetNavigationView();
			}
			return result;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000025B4 File Offset: 0x000007B4
		private int GetNavigationViewItemCountInPrimaryList()
		{
			int result = 0;
			NavigationView parentNavigationView = this.GetParentNavigationView();
			if (parentNavigationView != null)
			{
				result = parentNavigationView.GetNavigationViewItemCountInPrimaryList();
			}
			return result;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000025D8 File Offset: 0x000007D8
		private int GetNavigationViewItemCountInTopNav()
		{
			int result = 0;
			NavigationView parentNavigationView = this.GetParentNavigationView();
			if (parentNavigationView != null)
			{
				result = parentNavigationView.GetNavigationViewItemCountInTopNav();
			}
			return result;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000025FC File Offset: 0x000007FC
		private bool IsSettingsItem()
		{
			NavigationView parentNavigationView = this.GetParentNavigationView();
			if (parentNavigationView != null)
			{
				NavigationViewItem navigationViewItem = base.Owner as NavigationViewItem;
				object settingsItem = parentNavigationView.SettingsItem;
				if (navigationViewItem != null && settingsItem != null && (navigationViewItem == settingsItem || navigationViewItem.Content == settingsItem))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x0000263C File Offset: 0x0000083C
		private bool IsOnTopNavigation()
		{
			NavigationViewRepeaterPosition navigationViewRepeaterPosition = this.GetNavigationViewRepeaterPosition();
			return navigationViewRepeaterPosition != NavigationViewRepeaterPosition.LeftNav && navigationViewRepeaterPosition != NavigationViewRepeaterPosition.LeftFooter;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x0000265C File Offset: 0x0000085C
		internal bool IsOnTopNavigationOverflow()
		{
			return this.GetNavigationViewRepeaterPosition() == NavigationViewRepeaterPosition.TopOverflow;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002668 File Offset: 0x00000868
		private bool IsOnFooterNavigation()
		{
			NavigationViewRepeaterPosition navigationViewRepeaterPosition = this.GetNavigationViewRepeaterPosition();
			return navigationViewRepeaterPosition == NavigationViewRepeaterPosition.LeftFooter || navigationViewRepeaterPosition == NavigationViewRepeaterPosition.TopFooter;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002688 File Offset: 0x00000888
		private NavigationViewRepeaterPosition GetNavigationViewRepeaterPosition()
		{
			NavigationViewItemBase navigationViewItemBase = base.Owner as NavigationViewItemBase;
			if (navigationViewItemBase != null)
			{
				return navigationViewItemBase.Position;
			}
			return NavigationViewRepeaterPosition.LeftNav;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000026AC File Offset: 0x000008AC
		private ItemsRepeater GetParentItemsRepeater()
		{
			NavigationView parentNavigationView = this.GetParentNavigationView();
			if (parentNavigationView != null)
			{
				NavigationViewItemBase navigationViewItemBase = base.Owner as NavigationViewItemBase;
				if (navigationViewItemBase != null)
				{
					return parentNavigationView.GetParentItemsRepeaterForContainer(navigationViewItemBase);
				}
			}
			return null;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000026DC File Offset: 0x000008DC
		private int GetPositionOrSetCountInLeftNavHelper(NavigationViewItemAutomationPeer.AutomationOutput automationOutput)
		{
			int num = 0;
			ItemsRepeater parentItemsRepeater = this.GetParentItemsRepeater();
			if (parentItemsRepeater != null)
			{
				AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(parentItemsRepeater);
				if (automationPeer != null)
				{
					List<AutomationPeer> children = automationPeer.GetChildren();
					if (children != null)
					{
						int num2 = 0;
						bool flag = false;
						foreach (AutomationPeer automationPeer2 in children)
						{
							UIElement uielement = parentItemsRepeater.TryGetElement(num2);
							if (uielement != null)
							{
								if (uielement is NavigationViewItemHeader)
								{
									if (automationOutput == NavigationViewItemAutomationPeer.AutomationOutput.Size && flag)
									{
										break;
									}
									num = 0;
								}
								else
								{
									NavigationViewItem navigationViewItem = uielement as NavigationViewItem;
									if (navigationViewItem != null && navigationViewItem.Visibility == null)
									{
										num++;
										if (UIElementAutomationPeer.FromElement(navigationViewItem) == this)
										{
											if (automationOutput == NavigationViewItemAutomationPeer.AutomationOutput.Position)
											{
												break;
											}
											flag = true;
										}
									}
								}
							}
							num2++;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000027AC File Offset: 0x000009AC
		private int GetPositionOrSetCountInTopNavHelper(NavigationViewItemAutomationPeer.AutomationOutput automationOutput)
		{
			int num = 0;
			bool flag = false;
			ItemsRepeater parentItemsRepeater = this.GetParentItemsRepeater();
			if (parentItemsRepeater != null)
			{
				ItemsSourceView itemsSourceView = parentItemsRepeater.ItemsSourceView;
				if (itemsSourceView != null)
				{
					int count = itemsSourceView.Count;
					for (int i = 0; i < count; i++)
					{
						UIElement uielement = parentItemsRepeater.TryGetElement(i);
						if (uielement != null)
						{
							if (uielement is NavigationViewItemHeader)
							{
								if (automationOutput == NavigationViewItemAutomationPeer.AutomationOutput.Size && flag)
								{
									break;
								}
								num = 0;
							}
							else
							{
								NavigationViewItem navigationViewItem = uielement as NavigationViewItem;
								if (navigationViewItem != null && navigationViewItem.Visibility == null)
								{
									num++;
									if (UIElementAutomationPeer.FromElement(navigationViewItem) == this)
									{
										if (automationOutput == NavigationViewItemAutomationPeer.AutomationOutput.Position)
										{
											break;
										}
										flag = true;
									}
								}
							}
						}
					}
				}
			}
			return num;
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00002838 File Offset: 0x00000A38
		bool ISelectionItemProvider.IsSelected
		{
			get
			{
				NavigationViewItem navigationViewItem = base.Owner as NavigationViewItem;
				return navigationViewItem != null && navigationViewItem.IsSelected;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000042 RID: 66 RVA: 0x0000285C File Offset: 0x00000A5C
		IRawElementProviderSimple ISelectionItemProvider.SelectionContainer
		{
			get
			{
				NavigationView parentNavigationView = this.GetParentNavigationView();
				if (parentNavigationView != null)
				{
					AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(parentNavigationView);
					if (automationPeer != null)
					{
						return base.ProviderFromPeer(automationPeer);
					}
				}
				return null;
			}
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002886 File Offset: 0x00000A86
		void ISelectionItemProvider.AddToSelection()
		{
			this.ChangeSelection(true);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x0000288F File Offset: 0x00000A8F
		void ISelectionItemProvider.Select()
		{
			this.ChangeSelection(true);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002898 File Offset: 0x00000A98
		void ISelectionItemProvider.RemoveFromSelection()
		{
			this.ChangeSelection(false);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000028A4 File Offset: 0x00000AA4
		private void ChangeSelection(bool isSelected)
		{
			NavigationViewItem navigationViewItem = base.Owner as NavigationViewItem;
			if (navigationViewItem != null)
			{
				navigationViewItem.IsSelected = isSelected;
			}
		}

		// Token: 0x06000047 RID: 71 RVA: 0x000028C8 File Offset: 0x00000AC8
		private bool HasChildren()
		{
			NavigationViewItem navigationViewItem = base.Owner as NavigationViewItem;
			return navigationViewItem != null && navigationViewItem.HasChildren();
		}

		// Token: 0x04000005 RID: 5
		private static readonly ResourceAccessor ResourceAccessor = new ResourceAccessor(typeof(NavigationView));

		// Token: 0x020000F4 RID: 244
		private enum AutomationOutput
		{
			// Token: 0x04000553 RID: 1363
			Position,
			// Token: 0x04000554 RID: 1364
			Size
		}
	}
}
