using System;

namespace ModernWpf.Controls
{
	// Token: 0x02000065 RID: 101
	public sealed class NavigationViewItemCollapsedEventArgs : EventArgs
	{
		// Token: 0x06000501 RID: 1281 RVA: 0x00012E80 File Offset: 0x00011080
		internal NavigationViewItemCollapsedEventArgs(NavigationView navigationView)
		{
			this.m_navigationView = navigationView;
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000502 RID: 1282 RVA: 0x00012E8F File Offset: 0x0001108F
		// (set) Token: 0x06000503 RID: 1283 RVA: 0x00012E97 File Offset: 0x00011097
		public NavigationViewItemBase CollapsedItemContainer { get; internal set; }

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000504 RID: 1284 RVA: 0x00012EA0 File Offset: 0x000110A0
		public object CollapsedItem
		{
			get
			{
				if (this.m_collapsedItem != null)
				{
					return this.m_collapsedItem;
				}
				NavigationView navigationView = this.m_navigationView;
				if (navigationView != null)
				{
					this.m_collapsedItem = navigationView.MenuItemFromContainer(this.CollapsedItemContainer);
					return this.m_collapsedItem;
				}
				return null;
			}
		}

		// Token: 0x0400025A RID: 602
		private object m_collapsedItem;

		// Token: 0x0400025B RID: 603
		private NavigationView m_navigationView;
	}
}
