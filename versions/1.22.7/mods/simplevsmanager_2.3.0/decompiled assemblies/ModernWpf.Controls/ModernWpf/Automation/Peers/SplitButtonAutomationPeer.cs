using System;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using ModernWpf.Controls;

namespace ModernWpf.Automation.Peers
{
	// Token: 0x02000011 RID: 17
	public class SplitButtonAutomationPeer : FrameworkElementAutomationPeer, IExpandCollapseProvider, IInvokeProvider
	{
		// Token: 0x06000076 RID: 118 RVA: 0x00002E31 File Offset: 0x00001031
		public SplitButtonAutomationPeer(SplitButton owner) : base(owner)
		{
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00002E3A File Offset: 0x0000103A
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == 6 || patternInterface == null)
			{
				return this;
			}
			return base.GetPattern(patternInterface);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00002E4C File Offset: 0x0000104C
		protected override string GetClassNameCore()
		{
			return "SplitButton";
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00002E53 File Offset: 0x00001053
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return 31;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00002E58 File Offset: 0x00001058
		private SplitButton GetImpl()
		{
			SplitButton result = null;
			SplitButton splitButton = base.Owner as SplitButton;
			if (splitButton != null)
			{
				result = splitButton;
			}
			return result;
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00002E7C File Offset: 0x0000107C
		public ExpandCollapseState ExpandCollapseState
		{
			get
			{
				ExpandCollapseState result = 0;
				SplitButton impl = this.GetImpl();
				if (impl != null && impl.IsFlyoutOpen)
				{
					result = 1;
				}
				return result;
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00002EA0 File Offset: 0x000010A0
		public void Expand()
		{
			SplitButton impl = this.GetImpl();
			if (impl != null)
			{
				impl.OpenFlyout();
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00002EC0 File Offset: 0x000010C0
		public void Collapse()
		{
			SplitButton impl = this.GetImpl();
			if (impl != null)
			{
				impl.CloseFlyout();
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00002EE0 File Offset: 0x000010E0
		public void Invoke()
		{
			SplitButton impl = this.GetImpl();
			if (impl != null)
			{
				impl.OnClickPrimary(null, null);
			}
		}
	}
}
