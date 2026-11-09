using System;

namespace ModernWpf.Controls
{
	// Token: 0x020000CE RID: 206
	internal struct SelectedItemInfo
	{
		// Token: 0x06000A2A RID: 2602 RVA: 0x000229FC File Offset: 0x00020BFC
		public SelectedItemInfo(SelectionNode node, IndexPath path)
		{
			this.Node = new WeakReference<SelectionNode>(node);
			this.Path = path;
		}

		// Token: 0x04000429 RID: 1065
		public WeakReference<SelectionNode> Node;

		// Token: 0x0400042A RID: 1066
		public IndexPath Path;
	}
}
