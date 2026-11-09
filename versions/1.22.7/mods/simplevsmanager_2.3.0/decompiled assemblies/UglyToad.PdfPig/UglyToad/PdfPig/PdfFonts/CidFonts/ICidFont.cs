using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Geometry;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.PdfFonts.CidFonts
{
	// Token: 0x020000BA RID: 186
	internal interface ICidFont
	{
		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000359 RID: 857
		NameToken Type { get; }

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x0600035A RID: 858
		NameToken SubType { get; }

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600035B RID: 859
		NameToken BaseFont { get; }

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600035C RID: 860
		CharacterIdentifierSystemInfo SystemInfo { get; }

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600035D RID: 861
		FontDetails Details { get; }

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600035E RID: 862
		TransformationMatrix FontMatrix { get; }

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600035F RID: 863
		CidFontType CidFontType { get; }

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000360 RID: 864
		FontDescriptor Descriptor { get; }

		// Token: 0x06000361 RID: 865
		double GetWidthFromDictionary(int cid);

		// Token: 0x06000362 RID: 866
		double GetWidthFromFont(int characterIdentifier);

		// Token: 0x06000363 RID: 867
		PdfRectangle GetBoundingBox(int characterIdentifier);

		// Token: 0x06000364 RID: 868
		PdfVector GetPositionVector(int characterIdentifier);

		// Token: 0x06000365 RID: 869
		PdfVector GetDisplacementVector(int characterIdentifier);

		// Token: 0x06000366 RID: 870
		bool TryGetPath(int characterCode, out IReadOnlyList<PdfSubpath> path);

		// Token: 0x06000367 RID: 871
		bool TryGetPath(int characterCode, Func<int, int?> characterCodeToGlyphId, out IReadOnlyList<PdfSubpath> path);

		// Token: 0x06000368 RID: 872
		bool TryGetNormalisedPath(int characterCode, out IReadOnlyList<PdfSubpath> path);

		// Token: 0x06000369 RID: 873
		bool TryGetNormalisedPath(int characterCode, Func<int, int?> characterCodeToGlyphId, out IReadOnlyList<PdfSubpath> path);
	}
}
