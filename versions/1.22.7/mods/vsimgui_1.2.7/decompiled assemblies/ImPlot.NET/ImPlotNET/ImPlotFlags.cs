using System;

namespace ImPlotNET
{
	// Token: 0x02000010 RID: 16
	[Flags]
	public enum ImPlotFlags
	{
		// Token: 0x04000069 RID: 105
		None = 0,
		// Token: 0x0400006A RID: 106
		NoTitle = 1,
		// Token: 0x0400006B RID: 107
		NoLegend = 2,
		// Token: 0x0400006C RID: 108
		NoMouseText = 4,
		// Token: 0x0400006D RID: 109
		NoInputs = 8,
		// Token: 0x0400006E RID: 110
		NoMenus = 16,
		// Token: 0x0400006F RID: 111
		NoBoxSelect = 32,
		// Token: 0x04000070 RID: 112
		NoChild = 64,
		// Token: 0x04000071 RID: 113
		NoFrame = 128,
		// Token: 0x04000072 RID: 114
		Equal = 256,
		// Token: 0x04000073 RID: 115
		Crosshairs = 512,
		// Token: 0x04000074 RID: 116
		CanvasOnly = 55
	}
}
