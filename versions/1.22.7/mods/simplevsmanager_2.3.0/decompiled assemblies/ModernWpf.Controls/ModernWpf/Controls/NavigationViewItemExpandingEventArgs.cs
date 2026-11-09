using System;

namespace ModernWpf.Controls
{
	// Token: 0x02000066 RID: 102
	public sealed class NavigationViewItemExpandingEventArgs : EventArgs
	{
		// Token: 0x06000505 RID: 1285 RVA: 0x00012EE0 File Offset: 0x000110E0
		internal NavigationViewItemExpandingEventArgs(NavigationView navigationView)
		{
			this.m_navigationView = navigationView;
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000506 RID: 1286 RVA: 0x00012EEF File Offset: 0x000110EF
		// (set) Token: 0x06000507 RID: 1287 RVA: 0x00012EF7 File Offset: 0x000110F7
		public NavigationViewItemBase ExpandingItemContainer { get; internal set; }

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000508 RID: 1288 RVA: 0x00012F00 File Offset: 0x00011100
		public object ExpandingItem
		{
			get
			{
				if (this.m_expandingItem != null)
				{
					return this.m_expandingItem;
				}
				NavigationView navigationView = this.m_navigationView;
				if (navigationView != null)
				{
					this.m_expandingItem = navigationView.MenuItemFromContainer(this.ExpandingItemContainer);
					return this.m_expandingItem;
				}
				return null;
			}
		}

		// Token: 0x0400025D RID: 605
		private object m_expandingItem;

		// Token: 0x0400025E RID: 606
		private NavigationView m_navigationView;
	}
}
