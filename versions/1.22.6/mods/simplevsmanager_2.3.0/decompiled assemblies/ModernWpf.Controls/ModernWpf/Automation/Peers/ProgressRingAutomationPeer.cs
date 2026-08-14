using System;
using System.Windows.Automation.Peers;
using ModernWpf.Controls;

namespace ModernWpf.Automation.Peers
{
	// Token: 0x0200000E RID: 14
	public class ProgressRingAutomationPeer : FrameworkElementAutomationPeer
	{
		// Token: 0x0600005A RID: 90 RVA: 0x00002A10 File Offset: 0x00000C10
		public ProgressRingAutomationPeer(ProgressRing owner) : base(owner)
		{
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002A19 File Offset: 0x00000C19
		protected override string GetClassNameCore()
		{
			return "ProgressRing";
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00002A20 File Offset: 0x00000C20
		protected override string GetNameCore()
		{
			string nameCore = base.GetNameCore();
			ProgressRing progressRing = base.Owner as ProgressRing;
			if (progressRing != null && progressRing.IsActive)
			{
				return ProgressRingAutomationPeer.ResourceAccessor.GetLocalizedStringResource("ProgressRingIndeterminateStatus") + nameCore;
			}
			return nameCore;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00002A62 File Offset: 0x00000C62
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return 12;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00002A66 File Offset: 0x00000C66
		protected override string GetLocalizedControlTypeCore()
		{
			return ProgressRingAutomationPeer.ResourceAccessor.GetLocalizedStringResource("ProgressRingName");
		}

		// Token: 0x04000006 RID: 6
		private static readonly ResourceAccessor ResourceAccessor = new ResourceAccessor(typeof(ProgressRing));
	}
}
