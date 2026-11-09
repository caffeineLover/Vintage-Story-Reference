using System;

namespace ImGuiNET
{
	// Token: 0x02000073 RID: 115
	public struct ImGuiWindowClass
	{
		// Token: 0x04000726 RID: 1830
		public uint ClassId;

		// Token: 0x04000727 RID: 1831
		public uint ParentViewportId;

		// Token: 0x04000728 RID: 1832
		public ImGuiViewportFlags ViewportFlagsOverrideSet;

		// Token: 0x04000729 RID: 1833
		public ImGuiViewportFlags ViewportFlagsOverrideClear;

		// Token: 0x0400072A RID: 1834
		public ImGuiTabItemFlags TabItemFlagsOverrideSet;

		// Token: 0x0400072B RID: 1835
		public ImGuiDockNodeFlags DockNodeFlagsOverrideSet;

		// Token: 0x0400072C RID: 1836
		public byte DockingAlwaysTabBar;

		// Token: 0x0400072D RID: 1837
		public byte DockingAllowUnclassed;
	}
}
