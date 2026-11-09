using System;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.PdfFonts
{
	// Token: 0x02000089 RID: 137
	public class CharacterBoundingBox
	{
		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600022D RID: 557 RVA: 0x0000FB73 File Offset: 0x0000DD73
		public PdfRectangle GlyphBounds { get; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600022E RID: 558 RVA: 0x0000FB7B File Offset: 0x0000DD7B
		public double Width { get; }

		// Token: 0x0600022F RID: 559 RVA: 0x0000FB83 File Offset: 0x0000DD83
		internal CharacterBoundingBox(PdfRectangle bounds, double width)
		{
			this.GlyphBounds = bounds;
			this.Width = width;
		}
	}
}
