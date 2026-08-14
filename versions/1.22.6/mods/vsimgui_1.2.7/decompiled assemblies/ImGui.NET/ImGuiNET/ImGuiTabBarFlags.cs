using System;

namespace ImGuiNET
{
	// Token: 0x0200005F RID: 95
	[Flags]
	public enum ImGuiTabBarFlags
	{
		// Token: 0x04000683 RID: 1667
		None = 0,
		// Token: 0x04000684 RID: 1668
		Reorderable = 1,
		// Token: 0x04000685 RID: 1669
		AutoSelectNewTabs = 2,
		// Token: 0x04000686 RID: 1670
		TabListPopupButton = 4,
		// Token: 0x04000687 RID: 1671
		NoCloseWithMiddleMouseButton = 8,
		// Token: 0x04000688 RID: 1672
		NoTabListScrollingButtons = 16,
		// Token: 0x04000689 RID: 1673
		NoTooltip = 32,
		// Token: 0x0400068A RID: 1674
		FittingPolicyResizeDown = 64,
		// Token: 0x0400068B RID: 1675
		FittingPolicyScroll = 128,
		// Token: 0x0400068C RID: 1676
		FittingPolicyMask = 192,
		// Token: 0x0400068D RID: 1677
		FittingPolicyDefault = 64
	}
}
