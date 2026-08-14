using System;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using ModernWpf.Controls;

namespace ModernWpf.Automation.Peers
{
	// Token: 0x02000013 RID: 19
	public class ToggleSwitchAutomationPeer : FrameworkElementAutomationPeer, IToggleProvider
	{
		// Token: 0x06000089 RID: 137 RVA: 0x00002FF1 File Offset: 0x000011F1
		public ToggleSwitchAutomationPeer(ToggleSwitch owner) : base(owner)
		{
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00002FFA File Offset: 0x000011FA
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == 15)
			{
				return this;
			}
			return base.GetPattern(patternInterface);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x0000300A File Offset: 0x0000120A
		protected override string GetClassNameCore()
		{
			return "ToggleSwitch";
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00003014 File Offset: 0x00001214
		protected override string GetNameCore()
		{
			string text = base.GetNameCore();
			if (string.IsNullOrEmpty(text))
			{
				ToggleSwitch impl = this.GetImpl();
				object header = impl.Header;
				string text2 = (header != null) ? header.ToString() : null;
				if (!string.IsNullOrEmpty(text2))
				{
					text = text2;
				}
				object obj = impl.IsOn ? impl.OnContent : impl.OffContent;
				string text3 = (obj != null) ? obj.ToString() : null;
				if (!string.IsNullOrEmpty(text3))
				{
					if (!string.IsNullOrEmpty(text))
					{
						text += " ";
					}
					text += text3;
				}
			}
			return text ?? string.Empty;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000030A5 File Offset: 0x000012A5
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return 0;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x000030A8 File Offset: 0x000012A8
		protected override string GetLocalizedControlTypeCore()
		{
			return "toggle switch";
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600008F RID: 143 RVA: 0x000030AF File Offset: 0x000012AF
		public ToggleState ToggleState
		{
			get
			{
				if (!this.GetImpl().IsOn)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x06000090 RID: 144 RVA: 0x000030C1 File Offset: 0x000012C1
		public void Toggle()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			this.GetImpl().Toggle();
		}

		// Token: 0x06000091 RID: 145 RVA: 0x000030DC File Offset: 0x000012DC
		private ToggleSwitch GetImpl()
		{
			return (ToggleSwitch)base.Owner;
		}
	}
}
