using System;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x02000098 RID: 152
	public sealed class ItemsRepeaterElementClearingEventArgs : EventArgs
	{
		// Token: 0x06000798 RID: 1944 RVA: 0x0001B3CA File Offset: 0x000195CA
		internal ItemsRepeaterElementClearingEventArgs(UIElement element)
		{
			this.Update(element);
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000799 RID: 1945 RVA: 0x0001B3D9 File Offset: 0x000195D9
		// (set) Token: 0x0600079A RID: 1946 RVA: 0x0001B3E1 File Offset: 0x000195E1
		public UIElement Element { get; private set; }

		// Token: 0x0600079B RID: 1947 RVA: 0x0001B3EA File Offset: 0x000195EA
		internal void Update(UIElement element)
		{
			this.Element = element;
		}
	}
}
