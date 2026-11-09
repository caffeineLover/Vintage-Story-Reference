using System;

namespace ImGuiNET
{
	// Token: 0x02000060 RID: 96
	[Flags]
	public enum ImGuiTabItemFlags
	{
		// Token: 0x0400068F RID: 1679
		None = 0,
		// Token: 0x04000690 RID: 1680
		UnsavedDocument = 1,
		// Token: 0x04000691 RID: 1681
		SetSelected = 2,
		// Token: 0x04000692 RID: 1682
		NoCloseWithMiddleMouseButton = 4,
		// Token: 0x04000693 RID: 1683
		NoPushId = 8,
		// Token: 0x04000694 RID: 1684
		NoTooltip = 16,
		// Token: 0x04000695 RID: 1685
		NoReorder = 32,
		// Token: 0x04000696 RID: 1686
		Leading = 64,
		// Token: 0x04000697 RID: 1687
		Trailing = 128
	}
}
