using System;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using ModernWpf.Controls;

namespace ModernWpf.Automation.Peers
{
	// Token: 0x02000009 RID: 9
	public class HyperlinkButtonAutomationPeer : ButtonBaseAutomationPeer, IInvokeProvider
	{
		// Token: 0x0600001E RID: 30 RVA: 0x00002295 File Offset: 0x00000495
		public HyperlinkButtonAutomationPeer(HyperlinkButton owner) : base(owner)
		{
		}

		// Token: 0x0600001F RID: 31 RVA: 0x0000229E File Offset: 0x0000049E
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == null)
			{
				return this;
			}
			return base.GetPattern(patternInterface);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000022AC File Offset: 0x000004AC
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return 5;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000022AF File Offset: 0x000004AF
		protected override string GetClassNameCore()
		{
			return "Hyperlink";
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000022B6 File Offset: 0x000004B6
		protected override bool IsControlElementCore()
		{
			return true;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000022B9 File Offset: 0x000004B9
		void IInvokeProvider.Invoke()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			((HyperlinkButton)base.Owner).AutomationButtonBaseClick();
		}
	}
}
