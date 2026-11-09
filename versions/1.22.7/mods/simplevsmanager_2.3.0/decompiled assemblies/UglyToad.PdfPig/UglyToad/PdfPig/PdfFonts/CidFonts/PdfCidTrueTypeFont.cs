using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts.TrueType;
using UglyToad.PdfPig.Fonts.TrueType.Tables;

namespace UglyToad.PdfPig.PdfFonts.CidFonts
{
	// Token: 0x020000BD RID: 189
	internal class PdfCidTrueTypeFont : ICidFontProgram
	{
		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000381 RID: 897 RVA: 0x000156BC File Offset: 0x000138BC
		public FontDetails Details { get; }

		// Token: 0x06000382 RID: 898 RVA: 0x000156C4 File Offset: 0x000138C4
		public PdfCidTrueTypeFont(TrueTypeFont font)
		{
			if (font == null)
			{
				throw new ArgumentNullException("font");
			}
			this.font = font;
			HeaderTable headerTable = font.TableRegister.HeaderTable;
			bool flag = headerTable.MacStyle.HasFlag(1);
			this.Details = new FontDetails(font.Name, flag, flag ? 700 : 500, headerTable.MacStyle.HasFlag(2));
		}

		// Token: 0x06000383 RID: 899 RVA: 0x00015747 File Offset: 0x00013947
		public bool TryGetBoundingBox(int characterIdentifier, out PdfRectangle boundingBox)
		{
			return this.TryGetBoundingBox(characterIdentifier, null, out boundingBox);
		}

		// Token: 0x06000384 RID: 900 RVA: 0x00015752 File Offset: 0x00013952
		public bool TryGetBoundingBox(int characterIdentifier, Func<int, int?> characterCodeToGlyphId, out PdfRectangle boundingBox)
		{
			return this.font.TryGetBoundingBox(characterIdentifier, characterCodeToGlyphId, ref boundingBox);
		}

		// Token: 0x06000385 RID: 901 RVA: 0x00015762 File Offset: 0x00013962
		public bool TryGetBoundingAdvancedWidth(int characterIdentifier, out double width)
		{
			return this.TryGetBoundingAdvancedWidth(characterIdentifier, null, out width);
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0001576D File Offset: 0x0001396D
		public bool TryGetBoundingAdvancedWidth(int characterIdentifier, Func<int, int?> characterCodeToGlyphId, out double width)
		{
			return this.font.TryGetAdvanceWidth(characterIdentifier, characterCodeToGlyphId, ref width);
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0001577D File Offset: 0x0001397D
		public int GetFontMatrixMultiplier()
		{
			return this.font.GetUnitsPerEm();
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0001578A File Offset: 0x0001398A
		public bool TryGetPath(int characterCode, out IReadOnlyList<PdfSubpath> path)
		{
			return this.font.TryGetPath(characterCode, ref path);
		}

		// Token: 0x06000389 RID: 905 RVA: 0x00015799 File Offset: 0x00013999
		public bool TryGetPath(int characterCode, Func<int, int?> characterCodeToGlyphId, out IReadOnlyList<PdfSubpath> path)
		{
			return this.font.TryGetPath(characterCode, characterCodeToGlyphId, ref path);
		}

		// Token: 0x040001D0 RID: 464
		private readonly TrueTypeFont font;
	}
}
