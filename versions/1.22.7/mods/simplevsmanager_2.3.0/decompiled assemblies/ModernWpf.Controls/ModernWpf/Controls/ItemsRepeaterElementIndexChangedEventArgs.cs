using System;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x02000099 RID: 153
	public sealed class ItemsRepeaterElementIndexChangedEventArgs : EventArgs
	{
		// Token: 0x0600079C RID: 1948 RVA: 0x0001B3F3 File Offset: 0x000195F3
		internal ItemsRepeaterElementIndexChangedEventArgs(UIElement element, int oldIndex, int newIndex)
		{
			this.Update(element, oldIndex, newIndex);
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x0600079D RID: 1949 RVA: 0x0001B404 File Offset: 0x00019604
		// (set) Token: 0x0600079E RID: 1950 RVA: 0x0001B40C File Offset: 0x0001960C
		public UIElement Element { get; private set; }

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x0600079F RID: 1951 RVA: 0x0001B415 File Offset: 0x00019615
		// (set) Token: 0x060007A0 RID: 1952 RVA: 0x0001B41D File Offset: 0x0001961D
		public int OldIndex { get; private set; }

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060007A1 RID: 1953 RVA: 0x0001B426 File Offset: 0x00019626
		// (set) Token: 0x060007A2 RID: 1954 RVA: 0x0001B42E File Offset: 0x0001962E
		public int NewIndex { get; private set; }

		// Token: 0x060007A3 RID: 1955 RVA: 0x0001B437 File Offset: 0x00019637
		internal void Update(UIElement element, int oldIndex, int newIndex)
		{
			this.Element = element;
			this.OldIndex = oldIndex;
			this.NewIndex = newIndex;
		}
	}
}
