using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.PdfFonts.CidFonts
{
	// Token: 0x020000BB RID: 187
	internal interface ICidFontProgram
	{
		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600036A RID: 874
		FontDetails Details { get; }

		// Token: 0x0600036B RID: 875
		bool TryGetBoundingBox(int characterIdentifier, out PdfRectangle boundingBox);

		// Token: 0x0600036C RID: 876
		bool TryGetBoundingBox(int characterIdentifier, Func<int, int?> characterCodeToGlyphId, out PdfRectangle boundingBox);

		// Token: 0x0600036D RID: 877
		bool TryGetBoundingAdvancedWidth(int characterIdentifier, Func<int, int?> characterCodeToGlyphId, out double width);

		// Token: 0x0600036E RID: 878
		bool TryGetBoundingAdvancedWidth(int characterIdentifier, out double width);

		// Token: 0x0600036F RID: 879
		bool TryGetPath(int characterName, out IReadOnlyList<PdfSubpath> path);

		// Token: 0x06000370 RID: 880
		bool TryGetPath(int characterCode, Func<int, int?> characterCodeToGlyphId, out IReadOnlyList<PdfSubpath> path);

		// Token: 0x06000371 RID: 881
		int GetFontMatrixMultiplier();
	}
}
