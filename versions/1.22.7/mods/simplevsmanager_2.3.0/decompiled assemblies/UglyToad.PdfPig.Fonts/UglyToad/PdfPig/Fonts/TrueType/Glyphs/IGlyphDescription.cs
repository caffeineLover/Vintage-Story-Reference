using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.TrueType.Glyphs
{
	// Token: 0x02000069 RID: 105
	internal interface IGlyphDescription : IMergeableGlyph, ITransformableGlyph
	{
		// Token: 0x1700014A RID: 330
		// (get) Token: 0x0600029F RID: 671
		bool IsSimple { get; }

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060002A0 RID: 672
		PdfRectangle Bounds { get; }

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060002A1 RID: 673
		byte[] Instructions { get; }

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060002A2 RID: 674
		ushort[] EndPointsOfContours { get; }

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060002A3 RID: 675
		GlyphPoint[] Points { get; }

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060002A4 RID: 676
		bool IsEmpty { get; }

		// Token: 0x060002A5 RID: 677
		bool TryGetGlyphPath(out IReadOnlyList<PdfSubpath> subpaths);

		// Token: 0x060002A6 RID: 678
		IGlyphDescription DeepClone();
	}
}
