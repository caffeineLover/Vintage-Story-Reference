using System;

namespace ModernWpf.Controls
{
	// Token: 0x0200005D RID: 93
	public sealed class NavigationViewDisplayModeChangedEventArgs : EventArgs
	{
		// Token: 0x06000486 RID: 1158 RVA: 0x0001199C File Offset: 0x0000FB9C
		internal NavigationViewDisplayModeChangedEventArgs()
		{
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000487 RID: 1159 RVA: 0x000119A4 File Offset: 0x0000FBA4
		// (set) Token: 0x06000488 RID: 1160 RVA: 0x000119AC File Offset: 0x0000FBAC
		public NavigationViewDisplayMode DisplayMode { get; internal set; }
	}
}
