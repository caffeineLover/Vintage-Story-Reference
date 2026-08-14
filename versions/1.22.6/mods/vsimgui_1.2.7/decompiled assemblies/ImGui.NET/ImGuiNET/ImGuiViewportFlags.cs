using System;

namespace ImGuiNET
{
	// Token: 0x02000072 RID: 114
	[Flags]
	public enum ImGuiViewportFlags
	{
		// Token: 0x04000717 RID: 1815
		None = 0,
		// Token: 0x04000718 RID: 1816
		IsPlatformWindow = 1,
		// Token: 0x04000719 RID: 1817
		IsPlatformMonitor = 2,
		// Token: 0x0400071A RID: 1818
		OwnedByApp = 4,
		// Token: 0x0400071B RID: 1819
		NoDecoration = 8,
		// Token: 0x0400071C RID: 1820
		NoTaskBarIcon = 16,
		// Token: 0x0400071D RID: 1821
		NoFocusOnAppearing = 32,
		// Token: 0x0400071E RID: 1822
		NoFocusOnClick = 64,
		// Token: 0x0400071F RID: 1823
		NoInputs = 128,
		// Token: 0x04000720 RID: 1824
		NoRendererClear = 256,
		// Token: 0x04000721 RID: 1825
		NoAutoMerge = 512,
		// Token: 0x04000722 RID: 1826
		TopMost = 1024,
		// Token: 0x04000723 RID: 1827
		CanHostOtherWindows = 2048,
		// Token: 0x04000724 RID: 1828
		IsMinimized = 4096,
		// Token: 0x04000725 RID: 1829
		IsFocused = 8192
	}
}
