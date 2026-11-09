using System;
using System.Windows.Automation.Peers;
using ModernWpf.Controls;

namespace ModernWpf.Automation.Peers
{
	// Token: 0x0200000D RID: 13
	public class PersonPictureAutomationPeer : FrameworkElementAutomationPeer
	{
		// Token: 0x06000057 RID: 87 RVA: 0x000029FC File Offset: 0x00000BFC
		public PersonPictureAutomationPeer(PersonPicture owner) : base(owner)
		{
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002A05 File Offset: 0x00000C05
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return 20;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002A09 File Offset: 0x00000C09
		protected override string GetClassNameCore()
		{
			return "PersonPicture";
		}
	}
}
