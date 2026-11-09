using System;

namespace ImGuiNET
{
	// Token: 0x02000075 RID: 117
	[Flags]
	public enum ImGuiWindowFlags
	{
		// Token: 0x04000730 RID: 1840
		None = 0,
		// Token: 0x04000731 RID: 1841
		NoTitleBar = 1,
		// Token: 0x04000732 RID: 1842
		NoResize = 2,
		// Token: 0x04000733 RID: 1843
		NoMove = 4,
		// Token: 0x04000734 RID: 1844
		NoScrollbar = 8,
		// Token: 0x04000735 RID: 1845
		NoScrollWithMouse = 16,
		// Token: 0x04000736 RID: 1846
		NoCollapse = 32,
		// Token: 0x04000737 RID: 1847
		AlwaysAutoResize = 64,
		// Token: 0x04000738 RID: 1848
		NoBackground = 128,
		// Token: 0x04000739 RID: 1849
		NoSavedSettings = 256,
		// Token: 0x0400073A RID: 1850
		NoMouseInputs = 512,
		// Token: 0x0400073B RID: 1851
		MenuBar = 1024,
		// Token: 0x0400073C RID: 1852
		HorizontalScrollbar = 2048,
		// Token: 0x0400073D RID: 1853
		NoFocusOnAppearing = 4096,
		// Token: 0x0400073E RID: 1854
		NoBringToFrontOnFocus = 8192,
		// Token: 0x0400073F RID: 1855
		AlwaysVerticalScrollbar = 16384,
		// Token: 0x04000740 RID: 1856
		AlwaysHorizontalScrollbar = 32768,
		// Token: 0x04000741 RID: 1857
		AlwaysUseWindowPadding = 65536,
		// Token: 0x04000742 RID: 1858
		NoNavInputs = 262144,
		// Token: 0x04000743 RID: 1859
		NoNavFocus = 524288,
		// Token: 0x04000744 RID: 1860
		UnsavedDocument = 1048576,
		// Token: 0x04000745 RID: 1861
		NoDocking = 2097152,
		// Token: 0x04000746 RID: 1862
		NoNav = 786432,
		// Token: 0x04000747 RID: 1863
		NoDecoration = 43,
		// Token: 0x04000748 RID: 1864
		NoInputs = 786944,
		// Token: 0x04000749 RID: 1865
		NavFlattened = 8388608,
		// Token: 0x0400074A RID: 1866
		ChildWindow = 16777216,
		// Token: 0x0400074B RID: 1867
		Tooltip = 33554432,
		// Token: 0x0400074C RID: 1868
		Popup = 67108864,
		// Token: 0x0400074D RID: 1869
		Modal = 134217728,
		// Token: 0x0400074E RID: 1870
		ChildMenu = 268435456,
		// Token: 0x0400074F RID: 1871
		DockNodeHost = 536870912
	}
}
