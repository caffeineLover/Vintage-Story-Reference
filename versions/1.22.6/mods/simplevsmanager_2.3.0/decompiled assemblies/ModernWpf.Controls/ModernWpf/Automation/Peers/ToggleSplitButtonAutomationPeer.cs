using System;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using ModernWpf.Controls;

namespace ModernWpf.Automation.Peers
{
	// Token: 0x02000012 RID: 18
	public class ToggleSplitButtonAutomationPeer : FrameworkElementAutomationPeer, IExpandCollapseProvider, IToggleProvider
	{
		// Token: 0x0600007F RID: 127 RVA: 0x00002EFF File Offset: 0x000010FF
		public ToggleSplitButtonAutomationPeer(ToggleSplitButton owner) : base(owner)
		{
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00002F08 File Offset: 0x00001108
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == 6 || patternInterface == 15)
			{
				return this;
			}
			return base.GetPattern(patternInterface);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00002F1C File Offset: 0x0000111C
		protected override string GetClassNameCore()
		{
			return "ToggleSplitButton";
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00002F23 File Offset: 0x00001123
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return 31;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00002F28 File Offset: 0x00001128
		private ToggleSplitButton GetImpl()
		{
			ToggleSplitButton result = null;
			ToggleSplitButton toggleSplitButton = base.Owner as ToggleSplitButton;
			if (toggleSplitButton != null)
			{
				result = toggleSplitButton;
			}
			return result;
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00002F4C File Offset: 0x0000114C
		public ExpandCollapseState ExpandCollapseState
		{
			get
			{
				ExpandCollapseState result = 0;
				ToggleSplitButton impl = this.GetImpl();
				if (impl != null && impl.IsFlyoutOpen)
				{
					result = 1;
				}
				return result;
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00002F70 File Offset: 0x00001170
		public void Expand()
		{
			ToggleSplitButton impl = this.GetImpl();
			if (impl != null)
			{
				impl.OpenFlyout();
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00002F90 File Offset: 0x00001190
		public void Collapse()
		{
			ToggleSplitButton impl = this.GetImpl();
			if (impl != null)
			{
				impl.CloseFlyout();
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000087 RID: 135 RVA: 0x00002FB0 File Offset: 0x000011B0
		public ToggleState ToggleState
		{
			get
			{
				ToggleState result = 0;
				ToggleSplitButton impl = this.GetImpl();
				if (impl != null && impl.IsChecked)
				{
					result = 1;
				}
				return result;
			}
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00002FD4 File Offset: 0x000011D4
		public void Toggle()
		{
			ToggleSplitButton impl = this.GetImpl();
			if (impl != null)
			{
				impl.Toggle();
			}
		}
	}
}
