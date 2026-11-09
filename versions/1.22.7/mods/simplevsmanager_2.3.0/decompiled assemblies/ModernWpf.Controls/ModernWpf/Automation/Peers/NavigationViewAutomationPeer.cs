using System;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using ModernWpf.Controls;

namespace ModernWpf.Automation.Peers
{
	// Token: 0x0200000A RID: 10
	internal class NavigationViewAutomationPeer : FrameworkElementAutomationPeer, ISelectionProvider
	{
		// Token: 0x06000024 RID: 36 RVA: 0x000022D9 File Offset: 0x000004D9
		public NavigationViewAutomationPeer(NavigationView owner) : base(owner)
		{
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000022E2 File Offset: 0x000004E2
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == 1)
			{
				return this;
			}
			return base.GetPattern(patternInterface);
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000026 RID: 38 RVA: 0x000022F1 File Offset: 0x000004F1
		public bool CanSelectMultiple
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000027 RID: 39 RVA: 0x000022F4 File Offset: 0x000004F4
		public bool IsSelectionRequired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000022F8 File Offset: 0x000004F8
		public IRawElementProviderSimple[] GetSelection()
		{
			NavigationView navigationView = base.Owner as NavigationView;
			if (navigationView != null)
			{
				NavigationViewItem selectedContainer = navigationView.GetSelectedContainer();
				if (selectedContainer != null)
				{
					AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(selectedContainer);
					if (automationPeer != null)
					{
						return new IRawElementProviderSimple[]
						{
							base.ProviderFromPeer(automationPeer)
						};
					}
				}
			}
			return new IRawElementProviderSimple[0];
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002340 File Offset: 0x00000540
		internal void RaiseSelectionChangedEvent(object oldSelection, object newSelecttion)
		{
			if (AutomationPeer.ListenerExists(9))
			{
				NavigationView navigationView = base.Owner as NavigationView;
				if (navigationView != null)
				{
					NavigationViewItem selectedContainer = navigationView.GetSelectedContainer();
					if (selectedContainer != null)
					{
						AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(selectedContainer);
						if (automationPeer != null)
						{
							automationPeer.RaiseAutomationEvent(8);
						}
					}
				}
			}
		}
	}
}
