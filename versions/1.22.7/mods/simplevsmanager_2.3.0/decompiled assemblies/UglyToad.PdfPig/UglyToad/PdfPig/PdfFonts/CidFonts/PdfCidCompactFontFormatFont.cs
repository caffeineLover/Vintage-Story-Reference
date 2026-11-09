using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts.CompactFontFormat;

namespace UglyToad.PdfPig.PdfFonts.CidFonts
{
	// Token: 0x020000BC RID: 188
	internal class PdfCidCompactFontFormatFont : ICidFontProgram
	{
		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000372 RID: 882 RVA: 0x00015497 File Offset: 0x00013697
		public FontDetails Details { get; }

		// Token: 0x06000373 RID: 883 RVA: 0x0001549F File Offset: 0x0001369F
		public PdfCidCompactFontFormatFont(CompactFontFormatFontCollection fontCollection)
		{
			this.fontCollection = fontCollection;
			this.Details = PdfCidCompactFontFormatFont.GetDetails((fontCollection != null) ? fontCollection.FirstFont : null);
		}

		// Token: 0x06000374 RID: 884 RVA: 0x000154C8 File Offset: 0x000136C8
		private static FontDetails GetDetails(CompactFontFormatFont font)
		{
			PdfCidCompactFontFormatFont.<>c__DisplayClass5_0 CS$<>8__locals1;
			CS$<>8__locals1.font = font;
			if (CS$<>8__locals1.font == null)
			{
				return FontDetails.GetDefault(null);
			}
			string weight = CS$<>8__locals1.font.Weight;
			string text = (weight != null) ? weight.ToLowerInvariant() : null;
			if (text == "light")
			{
				return PdfCidCompactFontFormatFont.<GetDetails>g__WithWeightValues|5_0(false, 300, ref CS$<>8__locals1);
			}
			if (text == "semibold")
			{
				return PdfCidCompactFontFormatFont.<GetDetails>g__WithWeightValues|5_0(true, 600, ref CS$<>8__locals1);
			}
			if (text == "bold")
			{
				return PdfCidCompactFontFormatFont.<GetDetails>g__WithWeightValues|5_0(true, 700, ref CS$<>8__locals1);
			}
			if (!(text == "black"))
			{
				return PdfCidCompactFontFormatFont.<GetDetails>g__WithWeightValues|5_0(false, 500, ref CS$<>8__locals1);
			}
			return PdfCidCompactFontFormatFont.<GetDetails>g__WithWeightValues|5_0(true, 900, ref CS$<>8__locals1);
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0001557F File Offset: 0x0001377F
		public TransformationMatrix GetFontTransformationMatrix()
		{
			return this.fontCollection.GetFirstTransformationMatrix();
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0001558C File Offset: 0x0001378C
		public PdfRectangle? GetCharacterBoundingBox(string characterName)
		{
			return this.fontCollection.GetCharacterBoundingBox(characterName);
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0001559C File Offset: 0x0001379C
		public bool TryGetBoundingBox(int characterIdentifier, out PdfRectangle boundingBox)
		{
			boundingBox = new PdfRectangle(0, 0, 500, 0);
			CompactFontFormatFont font = this.GetFont();
			if (font.Encoding == null)
			{
				return false;
			}
			boundingBox = (font.GetCharacterBoundingBox(this.GetCharacterName(characterIdentifier)) ?? new PdfRectangle(0, 0, 500, 0));
			return true;
		}

		// Token: 0x06000378 RID: 888 RVA: 0x00015603 File Offset: 0x00013803
		public bool TryGetBoundingBox(int characterIdentifier, Func<int, int?> characterCodeToGlyphId, out PdfRectangle boundingBox)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000379 RID: 889 RVA: 0x0001560A File Offset: 0x0001380A
		public bool TryGetBoundingAdvancedWidth(int characterIdentifier, Func<int, int?> characterCodeToGlyphId, out double width)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600037A RID: 890 RVA: 0x00015611 File Offset: 0x00013811
		public bool TryGetBoundingAdvancedWidth(int characterIdentifier, out double width)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600037B RID: 891 RVA: 0x00015618 File Offset: 0x00013818
		public int GetFontMatrixMultiplier()
		{
			return 1000;
		}

		// Token: 0x0600037C RID: 892 RVA: 0x00015620 File Offset: 0x00013820
		public string GetCharacterName(int characterCode)
		{
			CompactFontFormatFont font = this.GetFont();
			if (font.Encoding != null)
			{
				return font.Encoding.GetName(characterCode);
			}
			return ".notdef";
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0001564E File Offset: 0x0001384E
		private CompactFontFormatFont GetFont()
		{
			return this.fontCollection.FirstFont;
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0001565C File Offset: 0x0001385C
		public bool TryGetPath(int characterCode, out IReadOnlyList<PdfSubpath> path)
		{
			path = EmptyArray<PdfSubpath>.Instance;
			CompactFontFormatFont font = this.GetFont();
			if (font.Encoding == null)
			{
				return false;
			}
			string characterName = this.GetCharacterName(characterCode);
			return font.TryGetPath(characterName, ref path);
		}

		// Token: 0x0600037F RID: 895 RVA: 0x00015696 File Offset: 0x00013896
		public bool TryGetPath(int characterCode, Func<int, int?> characterCodeToGlyphId, out IReadOnlyList<PdfSubpath> path)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000380 RID: 896 RVA: 0x0001569D File Offset: 0x0001389D
		[CompilerGenerated]
		internal static FontDetails <GetDetails>g__WithWeightValues|5_0(bool isbold, int weight, ref PdfCidCompactFontFormatFont.<>c__DisplayClass5_0 A_2)
		{
			return new FontDetails(null, isbold, weight, A_2.font.ItalicAngle != 0m);
		}

		// Token: 0x040001CE RID: 462
		private readonly CompactFontFormatFontCollection fontCollection;
	}
}
