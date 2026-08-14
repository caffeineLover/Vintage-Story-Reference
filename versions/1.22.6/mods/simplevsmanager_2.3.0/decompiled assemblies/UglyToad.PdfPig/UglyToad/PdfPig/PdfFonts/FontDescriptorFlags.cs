using System;

namespace UglyToad.PdfPig.PdfFonts
{
	// Token: 0x0200008E RID: 142
	[Flags]
	public enum FontDescriptorFlags
	{
		// Token: 0x0400012C RID: 300
		None = 0,
		// Token: 0x0400012D RID: 301
		FixedPitch = 1,
		// Token: 0x0400012E RID: 302
		Serif = 2,
		// Token: 0x0400012F RID: 303
		Symbolic = 4,
		// Token: 0x04000130 RID: 304
		Script = 8,
		// Token: 0x04000131 RID: 305
		NonSymbolic = 32,
		// Token: 0x04000132 RID: 306
		Italic = 64,
		// Token: 0x04000133 RID: 307
		AllCap = 65536,
		// Token: 0x04000134 RID: 308
		SmallCap = 131072,
		// Token: 0x04000135 RID: 309
		ForceBold = 262144
	}
}
