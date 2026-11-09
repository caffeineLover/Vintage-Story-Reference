using System;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x020000A9 RID: 169
	internal interface IRepeaterScrollingSurface
	{
		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000838 RID: 2104
		bool IsHorizontallyScrollable { get; }

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000839 RID: 2105
		bool IsVerticallyScrollable { get; }

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x0600083A RID: 2106
		UIElement AnchorElement { get; }

		// Token: 0x1400002E RID: 46
		// (add) Token: 0x0600083B RID: 2107
		// (remove) Token: 0x0600083C RID: 2108
		event ConfigurationChangedEventHandler ConfigurationChanged;

		// Token: 0x1400002F RID: 47
		// (add) Token: 0x0600083D RID: 2109
		// (remove) Token: 0x0600083E RID: 2110
		event PostArrangeEventHandler PostArrange;

		// Token: 0x14000030 RID: 48
		// (add) Token: 0x0600083F RID: 2111
		// (remove) Token: 0x06000840 RID: 2112
		event ViewportChangedEventHandler ViewportChanged;

		// Token: 0x06000841 RID: 2113
		void RegisterAnchorCandidate(UIElement element);

		// Token: 0x06000842 RID: 2114
		void UnregisterAnchorCandidate(UIElement element);

		// Token: 0x06000843 RID: 2115
		Rect GetRelativeViewport(UIElement child);
	}
}
