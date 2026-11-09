using System;

namespace ModernWpf.Controls
{
	// Token: 0x020000DE RID: 222
	public sealed class SplitViewPaneClosingEventArgs : EventArgs
	{
		// Token: 0x06000B0F RID: 2831 RVA: 0x00025CC3 File Offset: 0x00023EC3
		internal SplitViewPaneClosingEventArgs()
		{
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000B10 RID: 2832 RVA: 0x00025CCB File Offset: 0x00023ECB
		// (set) Token: 0x06000B11 RID: 2833 RVA: 0x00025CD3 File Offset: 0x00023ED3
		public bool Cancel { get; set; }
	}
}
