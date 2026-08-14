using System;

namespace ImPlotNET
{
	// Token: 0x02000019 RID: 25
	[Flags]
	public enum ImPlotLineFlags
	{
		// Token: 0x0400009F RID: 159
		None = 0,
		// Token: 0x040000A0 RID: 160
		Segments = 1024,
		// Token: 0x040000A1 RID: 161
		Loop = 2048,
		// Token: 0x040000A2 RID: 162
		SkipNaN = 4096,
		// Token: 0x040000A3 RID: 163
		NoClip = 8192,
		// Token: 0x040000A4 RID: 164
		Shaded = 16384
	}
}
