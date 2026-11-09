using System;

namespace ImPlotNET
{
	// Token: 0x02000012 RID: 18
	[Flags]
	public enum ImPlotHistogramFlags
	{
		// Token: 0x04000079 RID: 121
		None = 0,
		// Token: 0x0400007A RID: 122
		Horizontal = 1024,
		// Token: 0x0400007B RID: 123
		Cumulative = 2048,
		// Token: 0x0400007C RID: 124
		Density = 4096,
		// Token: 0x0400007D RID: 125
		NoOutliers = 8192,
		// Token: 0x0400007E RID: 126
		ColMajor = 16384
	}
}
