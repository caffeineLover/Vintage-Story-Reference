using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.PdfFonts
{
	// Token: 0x02000093 RID: 147
	public interface IFont
	{
		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000261 RID: 609
		NameToken Name { get; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000262 RID: 610
		bool IsVertical { get; }

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000263 RID: 611
		FontDetails Details { get; }

		// Token: 0x06000264 RID: 612
		int ReadCharacterCode(IInputBytes bytes, out int codeLength);

		// Token: 0x06000265 RID: 613
		bool TryGetUnicode(int characterCode, out string value);

		// Token: 0x06000266 RID: 614
		CharacterBoundingBox GetBoundingBox(int characterCode);

		// Token: 0x06000267 RID: 615
		TransformationMatrix GetFontMatrix();

		// Token: 0x06000268 RID: 616
		bool TryGetPath(int characterCode, out IReadOnlyList<PdfSubpath> path);

		// Token: 0x06000269 RID: 617
		bool TryGetNormalisedPath(int characterCode, out IReadOnlyList<PdfSubpath> path);
	}
}
