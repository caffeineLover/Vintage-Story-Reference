using System;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using ModernWpf.Controls;

namespace ModernWpf.Automation.Peers
{
	// Token: 0x02000008 RID: 8
	public class DropDownButtonAutomationPeer : ButtonAutomationPeer, IExpandCollapseProvider
	{
		// Token: 0x06000017 RID: 23 RVA: 0x000021F1 File Offset: 0x000003F1
		public DropDownButtonAutomationPeer(DropDownButton owner) : base(owner)
		{
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000021FA File Offset: 0x000003FA
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == 6)
			{
				return this;
			}
			return base.GetPattern(patternInterface);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002209 File Offset: 0x00000409
		protected override string GetClassNameCore()
		{
			return "DropDownButton";
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002210 File Offset: 0x00000410
		private DropDownButton GetImpl()
		{
			DropDownButton result = null;
			DropDownButton dropDownButton = base.Owner as DropDownButton;
			if (dropDownButton != null)
			{
				result = dropDownButton;
			}
			return result;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00002234 File Offset: 0x00000434
		public ExpandCollapseState ExpandCollapseState
		{
			get
			{
				ExpandCollapseState result = 0;
				DropDownButton impl = this.GetImpl();
				if (impl != null && impl.IsFlyoutOpen)
				{
					result = 1;
				}
				return result;
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002258 File Offset: 0x00000458
		public void Expand()
		{
			DropDownButton impl = this.GetImpl();
			if (impl != null)
			{
				impl.OpenFlyout();
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002278 File Offset: 0x00000478
		public void Collapse()
		{
			DropDownButton impl = this.GetImpl();
			if (impl != null)
			{
				impl.CloseFlyout();
			}
		}
	}
}
