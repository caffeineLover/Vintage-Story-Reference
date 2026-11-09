using System;

namespace ImGuiNET
{
	// Token: 0x0200006F RID: 111
	[Flags]
	public enum ImGuiTreeNodeFlags
	{
		// Token: 0x040006F4 RID: 1780
		None = 0,
		// Token: 0x040006F5 RID: 1781
		Selected = 1,
		// Token: 0x040006F6 RID: 1782
		Framed = 2,
		// Token: 0x040006F7 RID: 1783
		AllowOverlap = 4,
		// Token: 0x040006F8 RID: 1784
		NoTreePushOnOpen = 8,
		// Token: 0x040006F9 RID: 1785
		NoAutoOpenOnLog = 16,
		// Token: 0x040006FA RID: 1786
		DefaultOpen = 32,
		// Token: 0x040006FB RID: 1787
		OpenOnDoubleClick = 64,
		// Token: 0x040006FC RID: 1788
		OpenOnArrow = 128,
		// Token: 0x040006FD RID: 1789
		Leaf = 256,
		// Token: 0x040006FE RID: 1790
		Bullet = 512,
		// Token: 0x040006FF RID: 1791
		FramePadding = 1024,
		// Token: 0x04000700 RID: 1792
		SpanAvailWidth = 2048,
		// Token: 0x04000701 RID: 1793
		SpanFullWidth = 4096,
		// Token: 0x04000702 RID: 1794
		NavLeftJumpsBackHere = 8192,
		// Token: 0x04000703 RID: 1795
		CollapsingHeader = 26
	}
}
