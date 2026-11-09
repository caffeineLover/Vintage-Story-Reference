using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts;
using UglyToad.PdfPig.Fonts.Encodings;
using UglyToad.PdfPig.PdfFonts.Cmap;
using UglyToad.PdfPig.PdfFonts.Composite;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.PdfFonts.Simple
{
	// Token: 0x0200009A RID: 154
	internal class Type3Font : IFont
	{
		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060002A2 RID: 674 RVA: 0x000114DE File Offset: 0x0000F6DE
		public NameToken Name { get; }

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060002A3 RID: 675 RVA: 0x000114E6 File Offset: 0x0000F6E6
		public bool IsVertical { get; }

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060002A4 RID: 676 RVA: 0x000114EE File Offset: 0x0000F6EE
		public FontDetails Details { get; }

		// Token: 0x060002A5 RID: 677 RVA: 0x000114F8 File Offset: 0x0000F6F8
		public Type3Font(NameToken name, PdfRectangle boundingBox, TransformationMatrix fontMatrix, Encoding encoding, int firstChar, int lastChar, double[] widths, CMap toUnicodeCMap)
		{
			this.Name = name;
			this.boundingBox = boundingBox;
			this.fontMatrix = fontMatrix;
			this.encoding = encoding;
			this.firstChar = firstChar;
			this.lastChar = lastChar;
			this.widths = widths;
			this.toUnicodeCMap = new ToUnicodeCMap(toUnicodeCMap);
			this.Details = FontDetails.GetDefault((name != null) ? name.Data : null);
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x00011564 File Offset: 0x0000F764
		public int ReadCharacterCode(IInputBytes bytes, out int codeLength)
		{
			codeLength = 1;
			return (int)bytes.CurrentByte;
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x00011570 File Offset: 0x0000F770
		public bool TryGetUnicode(int characterCode, out string value)
		{
			if (this.toUnicodeCMap.CanMapToUnicode)
			{
				return this.toUnicodeCMap.TryGet(characterCode, out value);
			}
			string name = this.encoding.GetName(characterCode);
			value = GlyphList.AdobeGlyphList.NameToUnicode(name);
			return true;
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x000115B4 File Offset: 0x0000F7B4
		public CharacterBoundingBox GetBoundingBox(int characterCode)
		{
			PdfRectangle pdfRectangle = this.GetBoundingBoxInGlyphSpace(characterCode);
			pdfRectangle = this.fontMatrix.Transform(pdfRectangle);
			double width = this.fontMatrix.TransformX(this.widths[characterCode - this.firstChar]);
			return new CharacterBoundingBox(pdfRectangle, width);
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x00011600 File Offset: 0x0000F800
		private PdfRectangle GetBoundingBoxInGlyphSpace(int characterCode)
		{
			if (characterCode < this.firstChar || characterCode > this.lastChar)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(59, 1);
				defaultInterpolatedStringHandler.AppendLiteral("The character code was not contained in the widths array: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(characterCode);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new InvalidFontFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return new PdfRectangle(0.0, 0.0, this.widths[characterCode - this.firstChar], this.boundingBox.Height);
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0001168A File Offset: 0x0000F88A
		public TransformationMatrix GetFontMatrix()
		{
			return this.fontMatrix;
		}

		// Token: 0x060002AB RID: 683 RVA: 0x00011692 File Offset: 0x0000F892
		public bool TryGetPath(int characterCode, out IReadOnlyList<PdfSubpath> path)
		{
			path = null;
			return false;
		}

		// Token: 0x060002AC RID: 684 RVA: 0x00011698 File Offset: 0x0000F898
		public bool TryGetNormalisedPath(int characterCode, out IReadOnlyList<PdfSubpath> path)
		{
			return this.TryGetPath(characterCode, out path);
		}

		// Token: 0x04000170 RID: 368
		private readonly PdfRectangle boundingBox;

		// Token: 0x04000171 RID: 369
		private readonly TransformationMatrix fontMatrix;

		// Token: 0x04000172 RID: 370
		private readonly Encoding encoding;

		// Token: 0x04000173 RID: 371
		private readonly int firstChar;

		// Token: 0x04000174 RID: 372
		private readonly int lastChar;

		// Token: 0x04000175 RID: 373
		private readonly double[] widths;

		// Token: 0x04000176 RID: 374
		private readonly ToUnicodeCMap toUnicodeCMap;
	}
}
