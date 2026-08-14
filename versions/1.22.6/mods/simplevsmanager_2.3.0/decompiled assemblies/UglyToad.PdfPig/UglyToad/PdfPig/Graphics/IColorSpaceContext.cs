using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Graphics.Colors;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Graphics
{
	// Token: 0x02000107 RID: 263
	public interface IColorSpaceContext : IDeepCloneable<IColorSpaceContext>
	{
		// Token: 0x1700015B RID: 347
		// (get) Token: 0x0600057A RID: 1402
		ColorSpaceDetails CurrentStrokingColorSpace { get; }

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x0600057B RID: 1403
		ColorSpaceDetails CurrentNonStrokingColorSpace { get; }

		// Token: 0x0600057C RID: 1404
		void SetStrokingColorspace(NameToken colorspace, DictionaryToken dictionary = null);

		// Token: 0x0600057D RID: 1405
		void SetNonStrokingColorspace(NameToken colorspace, DictionaryToken dictionary = null);

		// Token: 0x0600057E RID: 1406
		void SetStrokingColor(IReadOnlyList<decimal> operands, NameToken patternName = null);

		// Token: 0x0600057F RID: 1407
		void SetStrokingColorGray(decimal gray);

		// Token: 0x06000580 RID: 1408
		void SetStrokingColorRgb(decimal r, decimal g, decimal b);

		// Token: 0x06000581 RID: 1409
		void SetStrokingColorCmyk(decimal c, decimal m, decimal y, decimal k);

		// Token: 0x06000582 RID: 1410
		void SetNonStrokingColor(IReadOnlyList<decimal> operands, NameToken patternName = null);

		// Token: 0x06000583 RID: 1411
		void SetNonStrokingColorGray(decimal gray);

		// Token: 0x06000584 RID: 1412
		void SetNonStrokingColorRgb(decimal r, decimal g, decimal b);

		// Token: 0x06000585 RID: 1413
		void SetNonStrokingColorCmyk(decimal c, decimal m, decimal y, decimal k);
	}
}
