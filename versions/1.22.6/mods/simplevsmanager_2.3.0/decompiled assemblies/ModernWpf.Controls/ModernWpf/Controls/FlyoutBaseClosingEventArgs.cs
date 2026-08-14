using System;

namespace ModernWpf.Controls
{
	// Token: 0x02000042 RID: 66
	internal sealed class FlyoutBaseClosingEventArgs : EventArgs
	{
		// Token: 0x06000288 RID: 648 RVA: 0x00008D99 File Offset: 0x00006F99
		internal FlyoutBaseClosingEventArgs()
		{
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000289 RID: 649 RVA: 0x00008DA1 File Offset: 0x00006FA1
		// (set) Token: 0x0600028A RID: 650 RVA: 0x00008DA4 File Offset: 0x00006FA4
		public bool Cancel
		{
			get
			{
				return false;
			}
			set
			{
				throw new NotImplementedException();
			}
		}
	}
}
