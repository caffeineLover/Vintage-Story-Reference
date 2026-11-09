using System;

namespace UglyToad.PdfPig.Fonts.TrueType.Glyphs
{
	// Token: 0x0200006B RID: 107
	internal interface ITransformableGlyph
	{
		// Token: 0x060002A8 RID: 680
		IGlyphDescription Transform(CompositeTransformMatrix3By2 matrix);
	}
}
