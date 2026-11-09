using System;

namespace ImPlotNET
{
	// Token: 0x0200002D RID: 45
	[Flags]
	public enum ImPlotSubplotFlags
	{
		// Token: 0x04000131 RID: 305
		None = 0,
		// Token: 0x04000132 RID: 306
		NoTitle = 1,
		// Token: 0x04000133 RID: 307
		NoLegend = 2,
		// Token: 0x04000134 RID: 308
		NoMenus = 4,
		// Token: 0x04000135 RID: 309
		NoResize = 8,
		// Token: 0x04000136 RID: 310
		NoAlign = 16,
		// Token: 0x04000137 RID: 311
		ShareItems = 32,
		// Token: 0x04000138 RID: 312
		LinkRows = 64,
		// Token: 0x04000139 RID: 313
		LinkCols = 128,
		// Token: 0x0400013A RID: 314
		LinkAllX = 256,
		// Token: 0x0400013B RID: 315
		LinkAllY = 512,
		// Token: 0x0400013C RID: 316
		ColMajor = 1024
	}
}
