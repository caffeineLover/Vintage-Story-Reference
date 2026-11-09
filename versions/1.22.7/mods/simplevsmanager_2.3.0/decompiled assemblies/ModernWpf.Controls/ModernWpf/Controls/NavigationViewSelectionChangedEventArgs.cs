using System;
using ModernWpf.Media.Animation;

namespace ModernWpf.Controls
{
	// Token: 0x0200006C RID: 108
	public sealed class NavigationViewSelectionChangedEventArgs : EventArgs
	{
		// Token: 0x0600052F RID: 1327 RVA: 0x00013562 File Offset: 0x00011762
		internal NavigationViewSelectionChangedEventArgs()
		{
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000530 RID: 1328 RVA: 0x0001356A File Offset: 0x0001176A
		// (set) Token: 0x06000531 RID: 1329 RVA: 0x00013572 File Offset: 0x00011772
		public object SelectedItem { get; internal set; }

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000532 RID: 1330 RVA: 0x0001357B File Offset: 0x0001177B
		// (set) Token: 0x06000533 RID: 1331 RVA: 0x00013583 File Offset: 0x00011783
		public bool IsSettingsSelected { get; internal set; }

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000534 RID: 1332 RVA: 0x0001358C File Offset: 0x0001178C
		// (set) Token: 0x06000535 RID: 1333 RVA: 0x00013594 File Offset: 0x00011794
		public NavigationViewItemBase SelectedItemContainer { get; internal set; }

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000536 RID: 1334 RVA: 0x0001359D File Offset: 0x0001179D
		// (set) Token: 0x06000537 RID: 1335 RVA: 0x000135A5 File Offset: 0x000117A5
		public NavigationTransitionInfo RecommendedNavigationTransitionInfo { get; internal set; }
	}
}
