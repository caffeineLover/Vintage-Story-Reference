using System;

namespace ModernWpf.Controls
{
	// Token: 0x0200006B RID: 107
	public sealed class NavigationViewPaneClosingEventArgs : EventArgs
	{
		// Token: 0x0600052B RID: 1323 RVA: 0x00013521 File Offset: 0x00011721
		internal NavigationViewPaneClosingEventArgs()
		{
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x0600052C RID: 1324 RVA: 0x00013529 File Offset: 0x00011729
		// (set) Token: 0x0600052D RID: 1325 RVA: 0x00013534 File Offset: 0x00011734
		public bool Cancel
		{
			get
			{
				return this.m_cancelled;
			}
			set
			{
				this.m_cancelled = value;
				SplitViewPaneClosingEventArgs splitViewClosingArgs = this.m_splitViewClosingArgs;
				if (splitViewClosingArgs != null)
				{
					splitViewClosingArgs.Cancel = value;
				}
			}
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x00013559 File Offset: 0x00011759
		internal void SplitViewClosingArgs(SplitViewPaneClosingEventArgs value)
		{
			this.m_splitViewClosingArgs = value;
		}

		// Token: 0x0400026D RID: 621
		private SplitViewPaneClosingEventArgs m_splitViewClosingArgs;

		// Token: 0x0400026E RID: 622
		private bool m_cancelled;
	}
}
