using System;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x0200009A RID: 154
	public sealed class ItemsRepeaterElementPreparedEventArgs : EventArgs
	{
		// Token: 0x060007A4 RID: 1956 RVA: 0x0001B44E File Offset: 0x0001964E
		internal ItemsRepeaterElementPreparedEventArgs(UIElement element, int index)
		{
			this.Update(element, index);
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060007A5 RID: 1957 RVA: 0x0001B45E File Offset: 0x0001965E
		// (set) Token: 0x060007A6 RID: 1958 RVA: 0x0001B466 File Offset: 0x00019666
		public UIElement Element { get; private set; }

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x060007A7 RID: 1959 RVA: 0x0001B46F File Offset: 0x0001966F
		// (set) Token: 0x060007A8 RID: 1960 RVA: 0x0001B477 File Offset: 0x00019677
		public int Index { get; private set; }

		// Token: 0x060007A9 RID: 1961 RVA: 0x0001B480 File Offset: 0x00019680
		internal void Update(UIElement element, int index)
		{
			this.Element = element;
			this.Index = index;
		}
	}
}
