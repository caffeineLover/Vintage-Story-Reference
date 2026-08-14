using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts;
using UglyToad.PdfPig.Fonts.AdobeFontMetrics;
using UglyToad.PdfPig.Fonts.Encodings;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.PdfFonts.Simple
{
	// Token: 0x02000099 RID: 153
	internal class Type1Standard14Font : IFont
	{
		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000297 RID: 663 RVA: 0x000111FD File Offset: 0x0000F3FD
		public NameToken Name { get; }

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000298 RID: 664 RVA: 0x00011205 File Offset: 0x0000F405
		public bool IsVertical { get; }

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000299 RID: 665 RVA: 0x0001120D File Offset: 0x0000F40D
		public FontDetails Details { get; }

		// Token: 0x0600029A RID: 666 RVA: 0x00011218 File Offset: 0x0000F418
		public Type1Standard14Font(AdobeFontMetrics standardFontMetrics, Encoding overrideEncoding = null)
		{
			if (standardFontMetrics == null)
			{
				throw new ArgumentNullException("standardFontMetrics");
			}
			this.standardFontMetrics = standardFontMetrics;
			this.encoding = (overrideEncoding ?? new AdobeFontMetricsEncoding(standardFontMetrics));
			this.Name = NameToken.Create(standardFontMetrics.FontName);
			this.IsVertical = 0;
			this.Details = new FontDetails(this.Name.Data, standardFontMetrics.Weight == "Bold", (standardFontMetrics.Weight == "Bold") ? 700 : 500, standardFontMetrics.ItalicAngle != 0m);
		}

		// Token: 0x0600029B RID: 667 RVA: 0x000112FF File Offset: 0x0000F4FF
		public int ReadCharacterCode(IInputBytes bytes, out int codeLength)
		{
			codeLength = 1;
			return (int)bytes.CurrentByte;
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0001130C File Offset: 0x0000F50C
		public bool TryGetUnicode(int characterCode, out string value)
		{
			string name = this.encoding.GetName(characterCode);
			if (string.Equals(name, ".notdef", 5))
			{
				value = null;
				return false;
			}
			if (this.encoding is ZapfDingbatsEncoding)
			{
				string text = GlyphList.ZapfDingbats.NameToUnicode(name);
				value = text;
				return true;
			}
			if (this.encoding is StandardEncoding || this.encoding is SymbolEncoding)
			{
				string text2 = GlyphList.AdobeGlyphList.NameToUnicode(name);
				value = text2;
				return true;
			}
			string text3 = GlyphList.AdobeGlyphList.NameToUnicode(name);
			value = text3;
			return true;
		}

		// Token: 0x0600029D RID: 669 RVA: 0x00011394 File Offset: 0x0000F594
		public CharacterBoundingBox GetBoundingBox(int characterCode)
		{
			PdfRectangle pdfRectangle = this.GetBoundingBoxInGlyphSpace(characterCode);
			pdfRectangle = this.fontMatrix.Transform(pdfRectangle);
			return new CharacterBoundingBox(pdfRectangle, pdfRectangle.Width);
		}

		// Token: 0x0600029E RID: 670 RVA: 0x000113C8 File Offset: 0x0000F5C8
		private PdfRectangle GetBoundingBoxInGlyphSpace(int characterCode)
		{
			string name = this.encoding.GetName(characterCode);
			AdobeFontMetricsIndividualCharacterMetric adobeFontMetricsIndividualCharacterMetric;
			if (!this.standardFontMetrics.CharacterMetrics.TryGetValue(name, ref adobeFontMetricsIndividualCharacterMetric))
			{
				return new PdfRectangle(0, 0, 250, 0);
			}
			double num = adobeFontMetricsIndividualCharacterMetric.Width.X;
			double num2 = adobeFontMetricsIndividualCharacterMetric.Width.Y;
			if (adobeFontMetricsIndividualCharacterMetric.Width.X == 0.0 && adobeFontMetricsIndividualCharacterMetric.BoundingBox.Width > 0.0)
			{
				num = adobeFontMetricsIndividualCharacterMetric.BoundingBox.Width;
			}
			if (adobeFontMetricsIndividualCharacterMetric.Width.Y == 0.0 && adobeFontMetricsIndividualCharacterMetric.BoundingBox.Height > 0.0)
			{
				num2 = adobeFontMetricsIndividualCharacterMetric.BoundingBox.Height;
			}
			return new PdfRectangle(0.0, 0.0, num, num2);
		}

		// Token: 0x0600029F RID: 671 RVA: 0x000114C6 File Offset: 0x0000F6C6
		public TransformationMatrix GetFontMatrix()
		{
			return this.fontMatrix;
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x000114CE File Offset: 0x0000F6CE
		public bool TryGetPath(int characterCode, out IReadOnlyList<PdfSubpath> path)
		{
			path = null;
			return false;
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x000114D4 File Offset: 0x0000F6D4
		public bool TryGetNormalisedPath(int characterCode, out IReadOnlyList<PdfSubpath> path)
		{
			return this.TryGetPath(characterCode, out path);
		}

		// Token: 0x0400016A RID: 362
		private readonly AdobeFontMetrics standardFontMetrics;

		// Token: 0x0400016B RID: 363
		private readonly Encoding encoding;

		// Token: 0x0400016F RID: 367
		private readonly TransformationMatrix fontMatrix = TransformationMatrix.FromValues(0.001, 0.0, 0.0, 0.001, 0.0, 0.0);
	}
}
