using System;

namespace UglyToad.PdfPig.AcroForms.Fields
{
	// Token: 0x0200020F RID: 527
	[Flags]
	public enum AcroButtonFieldFlags : uint
	{
		// Token: 0x040006E6 RID: 1766
		ReadOnly = 1U,
		// Token: 0x040006E7 RID: 1767
		Required = 2U,
		// Token: 0x040006E8 RID: 1768
		NoExport = 4U,
		// Token: 0x040006E9 RID: 1769
		NoToggleToOff = 16384U,
		// Token: 0x040006EA RID: 1770
		Radio = 32768U,
		// Token: 0x040006EB RID: 1771
		PushButton = 65536U,
		// Token: 0x040006EC RID: 1772
		RadiosInUnison = 33554432U
	}
}
