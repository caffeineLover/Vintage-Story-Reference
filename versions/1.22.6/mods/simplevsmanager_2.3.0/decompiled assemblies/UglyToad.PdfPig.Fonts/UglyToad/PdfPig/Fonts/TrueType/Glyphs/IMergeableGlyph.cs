using System;

namespace UglyToad.PdfPig.Fonts.TrueType.Glyphs
{
	// Token: 0x0200006A RID: 106
	internal interface IMergeableGlyph
	{
		// Token: 0x060002A7 RID: 679
		IGlyphDescription Merge(IGlyphDescription glyph);
	}
}
