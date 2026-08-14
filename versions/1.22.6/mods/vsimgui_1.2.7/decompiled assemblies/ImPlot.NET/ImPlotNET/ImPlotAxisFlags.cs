using System;

namespace ImPlotNET
{
	// Token: 0x02000004 RID: 4
	[Flags]
	public enum ImPlotAxisFlags
	{
		// Token: 0x0400000A RID: 10
		None = 0,
		// Token: 0x0400000B RID: 11
		NoLabel = 1,
		// Token: 0x0400000C RID: 12
		NoGridLines = 2,
		// Token: 0x0400000D RID: 13
		NoTickMarks = 4,
		// Token: 0x0400000E RID: 14
		NoTickLabels = 8,
		// Token: 0x0400000F RID: 15
		NoInitialFit = 16,
		// Token: 0x04000010 RID: 16
		NoMenus = 32,
		// Token: 0x04000011 RID: 17
		NoSideSwitch = 64,
		// Token: 0x04000012 RID: 18
		NoHighlight = 128,
		// Token: 0x04000013 RID: 19
		Opposite = 256,
		// Token: 0x04000014 RID: 20
		Foreground = 512,
		// Token: 0x04000015 RID: 21
		Invert = 1024,
		// Token: 0x04000016 RID: 22
		AutoFit = 2048,
		// Token: 0x04000017 RID: 23
		RangeFit = 4096,
		// Token: 0x04000018 RID: 24
		PanStretch = 8192,
		// Token: 0x04000019 RID: 25
		LockMin = 16384,
		// Token: 0x0400001A RID: 26
		LockMax = 32768,
		// Token: 0x0400001B RID: 27
		Lock = 49152,
		// Token: 0x0400001C RID: 28
		NoDecorations = 15,
		// Token: 0x0400001D RID: 29
		AuxDefault = 258
	}
}
