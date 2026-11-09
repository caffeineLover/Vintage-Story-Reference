using System;

namespace UglyToad.PdfPig.Fonts.TrueType.Glyphs
{
	// Token: 0x0200006C RID: 108
	[Flags]
	public enum SimpleGlyphFlags : byte
	{
		// Token: 0x040002C2 RID: 706
		OnCurve = 1,
		// Token: 0x040002C3 RID: 707
		XSingleByte = 2,
		// Token: 0x040002C4 RID: 708
		YSingleByte = 4,
		// Token: 0x040002C5 RID: 709
		Repeat = 8,
		// Token: 0x040002C6 RID: 710
		ThisXIsTheSame = 16,
		// Token: 0x040002C7 RID: 711
		ThisYIsTheSame = 32
	}
}
