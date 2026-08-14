using System;

namespace ImGuiNET
{
	// Token: 0x02000038 RID: 56
	[Flags]
	public enum ImGuiFocusedFlags
	{
		// Token: 0x04000192 RID: 402
		None = 0,
		// Token: 0x04000193 RID: 403
		ChildWindows = 1,
		// Token: 0x04000194 RID: 404
		RootWindow = 2,
		// Token: 0x04000195 RID: 405
		AnyWindow = 4,
		// Token: 0x04000196 RID: 406
		NoPopupHierarchy = 8,
		// Token: 0x04000197 RID: 407
		DockHierarchy = 16,
		// Token: 0x04000198 RID: 408
		RootAndChildWindows = 3
	}
}
