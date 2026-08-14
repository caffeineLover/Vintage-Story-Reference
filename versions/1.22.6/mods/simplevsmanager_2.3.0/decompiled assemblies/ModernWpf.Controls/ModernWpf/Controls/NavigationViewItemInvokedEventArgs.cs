using System;
using ModernWpf.Media.Animation;

namespace ModernWpf.Controls
{
	// Token: 0x02000068 RID: 104
	public sealed class NavigationViewItemInvokedEventArgs : EventArgs
	{
		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000512 RID: 1298 RVA: 0x000130BE File Offset: 0x000112BE
		// (set) Token: 0x06000513 RID: 1299 RVA: 0x000130C6 File Offset: 0x000112C6
		public object InvokedItem { get; internal set; }

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000514 RID: 1300 RVA: 0x000130CF File Offset: 0x000112CF
		// (set) Token: 0x06000515 RID: 1301 RVA: 0x000130D7 File Offset: 0x000112D7
		public bool IsSettingsInvoked { get; internal set; }

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000516 RID: 1302 RVA: 0x000130E0 File Offset: 0x000112E0
		// (set) Token: 0x06000517 RID: 1303 RVA: 0x000130E8 File Offset: 0x000112E8
		public NavigationViewItemBase InvokedItemContainer { get; internal set; }

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000518 RID: 1304 RVA: 0x000130F1 File Offset: 0x000112F1
		// (set) Token: 0x06000519 RID: 1305 RVA: 0x000130F9 File Offset: 0x000112F9
		public NavigationTransitionInfo RecommendedNavigationTransitionInfo { get; internal set; }
	}
}
