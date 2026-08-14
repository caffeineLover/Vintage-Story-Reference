using System;
using System.Collections.Generic;
using System.Linq;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts;
using UglyToad.PdfPig.Fonts.CompactFontFormat;
using UglyToad.PdfPig.Fonts.Encodings;
using UglyToad.PdfPig.Fonts.Type1;
using UglyToad.PdfPig.PdfFonts.Cmap;
using UglyToad.PdfPig.PdfFonts.Composite;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.PdfFonts.Simple
{
	// Token: 0x02000098 RID: 152
	internal class Type1FontSimple : IFont
	{
		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600028A RID: 650 RVA: 0x00010D59 File Offset: 0x0000EF59
		public NameToken Name { get; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600028B RID: 651 RVA: 0x00010D61 File Offset: 0x0000EF61
		public bool IsVertical { get; }

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600028C RID: 652 RVA: 0x00010D69 File Offset: 0x0000EF69
		public FontDetails Details { get; }

		// Token: 0x0600028D RID: 653 RVA: 0x00010D74 File Offset: 0x0000EF74
		public Type1FontSimple(NameToken name, int firstChar, int lastChar, double[] widths, FontDescriptor fontDescriptor, Encoding encoding, CMap toUnicodeCMap, Union<Type1Font, CompactFontFormatFontCollection> fontProgram)
		{
			this.firstChar = firstChar;
			this.lastChar = lastChar;
			this.widths = widths;
			this.fontDescriptor = fontDescriptor;
			this.encoding = encoding;
			this.fontProgram = fontProgram;
			this.toUnicodeCMap = new ToUnicodeCMap(toUnicodeCMap);
			TransformationMatrix transformationMatrix = Type1FontSimple.DefaultTransformationMatrix;
			if (fontProgram != null)
			{
				Type1Font type1Font;
				CompactFontFormatFontCollection compactFontFormatFontCollection;
				if (fontProgram.TryGetFirst(ref type1Font))
				{
					transformationMatrix = type1Font.FontMatrix;
				}
				else if (fontProgram.TryGetSecond(ref compactFontFormatFontCollection))
				{
					transformationMatrix = compactFontFormatFontCollection.GetFirstTransformationMatrix();
				}
			}
			this.fontMatrix = transformationMatrix;
			this.Name = name;
			FontDetails fontDetails;
			if ((fontDetails = ((fontDescriptor != null) ? fontDescriptor.ToDetails((name != null) ? name.Data : null) : null)) == null)
			{
				fontDetails = FontDetails.GetDefault((name != null) ? name.Data : null);
			}
			this.Details = fontDetails;
		}

		// Token: 0x0600028E RID: 654 RVA: 0x00010E40 File Offset: 0x0000F040
		public int ReadCharacterCode(IInputBytes bytes, out int codeLength)
		{
			codeLength = 1;
			return (int)bytes.CurrentByte;
		}

		// Token: 0x0600028F RID: 655 RVA: 0x00010E4C File Offset: 0x0000F04C
		public bool TryGetUnicode(int characterCode, out string value)
		{
			if (this.toUnicodeCMap.CanMapToUnicode)
			{
				return this.toUnicodeCMap.TryGet(characterCode, out value);
			}
			value = null;
			if (this.encoding == null)
			{
				try
				{
					value = char.ConvertFromUtf32(characterCode);
					return true;
				}
				catch
				{
					if (this.fontProgram == null)
					{
						return false;
					}
					bool result = false;
					Type1Font type1Font;
					if (this.fontProgram.TryGetFirst(ref type1Font))
					{
						result = type1Font.Encoding.TryGetValue(characterCode, ref value);
					}
					return result;
				}
			}
			string name = this.encoding.GetName(characterCode);
			try
			{
				value = GlyphList.AdobeGlyphList.NameToUnicode(name);
			}
			catch
			{
				return false;
			}
			return true;
		}

		// Token: 0x06000290 RID: 656 RVA: 0x00010EFC File Offset: 0x0000F0FC
		public CharacterBoundingBox GetBoundingBox(int characterCode)
		{
			CharacterBoundingBox result;
			if (this.cachedBoundingBoxes.TryGetValue(characterCode, ref result))
			{
				return result;
			}
			PdfRectangle pdfRectangle = this.GetBoundingBoxInGlyphSpace(characterCode);
			pdfRectangle = this.fontMatrix.Transform(pdfRectangle);
			double width = this.GetWidth(characterCode, pdfRectangle);
			CharacterBoundingBox characterBoundingBox = new CharacterBoundingBox(pdfRectangle, width / 1000.0);
			this.cachedBoundingBoxes[characterCode] = characterBoundingBox;
			return characterBoundingBox;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x00010F60 File Offset: 0x0000F160
		private double GetWidth(int characterCode, PdfRectangle boundingBox)
		{
			int num = characterCode - this.firstChar;
			if (num >= 0 && num < this.widths.Length)
			{
				return this.widths[num];
			}
			FontDescriptor fontDescriptor = this.fontDescriptor;
			bool flag;
			if (fontDescriptor == null)
			{
				flag = false;
			}
			else
			{
				decimal missingWidth = fontDescriptor.MissingWidth;
				flag = true;
			}
			if (flag)
			{
				return (double)this.fontDescriptor.MissingWidth;
			}
			return boundingBox.Width;
		}

		// Token: 0x06000292 RID: 658 RVA: 0x00010FC0 File Offset: 0x0000F1C0
		private PdfRectangle GetBoundingBoxInGlyphSpace(int characterCode)
		{
			if (characterCode < this.firstChar || characterCode > this.lastChar)
			{
				return new PdfRectangle(0, 0, 250, 0);
			}
			if (this.fontProgram == null)
			{
				return new PdfRectangle(0.0, 0.0, this.widths[characterCode - this.firstChar], 0.0);
			}
			PdfRectangle? pdfRectangle = default(PdfRectangle?);
			Type1Font type1Font;
			CompactFontFormatFontCollection compactFontFormatFontCollection;
			if (this.fontProgram.TryGetFirst(ref type1Font))
			{
				string name = this.encoding.GetName(characterCode);
				pdfRectangle = type1Font.GetCharacterBoundingBox(name);
			}
			else if (this.fontProgram.TryGetSecond(ref compactFontFormatFontCollection))
			{
				CompactFontFormatFont firstFont = compactFontFormatFontCollection.FirstFont;
				string text;
				if (this.encoding != null)
				{
					text = this.encoding.GetName(characterCode);
				}
				else
				{
					text = compactFontFormatFontCollection.GetCharacterName(characterCode);
				}
				pdfRectangle = firstFont.GetCharacterBoundingBox(text);
			}
			if (pdfRectangle == null)
			{
				return new PdfRectangle(0.0, 0.0, this.widths[characterCode - this.firstChar], 0.0);
			}
			return pdfRectangle.Value;
		}

		// Token: 0x06000293 RID: 659 RVA: 0x000110D1 File Offset: 0x0000F2D1
		public TransformationMatrix GetFontMatrix()
		{
			return this.fontMatrix;
		}

		// Token: 0x06000294 RID: 660 RVA: 0x000110DC File Offset: 0x0000F2DC
		public bool TryGetPath(int characterCode, out IReadOnlyList<PdfSubpath> path)
		{
			path = null;
			IReadOnlyList<PdfSubpath> readOnlyList = null;
			if (characterCode < this.firstChar || characterCode > this.lastChar)
			{
				return false;
			}
			if (this.fontProgram == null)
			{
				return false;
			}
			Type1Font type1Font;
			CompactFontFormatFontCollection compactFontFormatFontCollection;
			if (this.fontProgram.TryGetFirst(ref type1Font))
			{
				string name = this.encoding.GetName(characterCode);
				readOnlyList = type1Font.GetCharacterPath(name);
			}
			else if (this.fontProgram.TryGetSecond(ref compactFontFormatFontCollection))
			{
				CompactFontFormatFont firstFont = compactFontFormatFontCollection.FirstFont;
				string text;
				if (this.encoding != null)
				{
					text = this.encoding.GetName(characterCode);
				}
				else
				{
					text = compactFontFormatFontCollection.GetCharacterName(characterCode);
				}
				readOnlyList = firstFont.GetCharacterPath(text);
			}
			if (readOnlyList != null)
			{
				path = readOnlyList;
				return true;
			}
			return false;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0001117C File Offset: 0x0000F37C
		public bool TryGetNormalisedPath(int characterCode, out IReadOnlyList<PdfSubpath> path)
		{
			if (this.TryGetPath(characterCode, out path))
			{
				path = Enumerable.ToList<PdfSubpath>(this.fontMatrix.Transform(path));
				return true;
			}
			return false;
		}

		// Token: 0x0400015D RID: 349
		private static readonly TransformationMatrix DefaultTransformationMatrix = TransformationMatrix.FromValues(0.001, 0.0, 0.0, 0.001, 0.0, 0.0);

		// Token: 0x0400015E RID: 350
		private readonly Dictionary<int, CharacterBoundingBox> cachedBoundingBoxes = new Dictionary<int, CharacterBoundingBox>();

		// Token: 0x0400015F RID: 351
		private readonly int firstChar;

		// Token: 0x04000160 RID: 352
		private readonly int lastChar;

		// Token: 0x04000161 RID: 353
		private readonly double[] widths;

		// Token: 0x04000162 RID: 354
		private readonly FontDescriptor fontDescriptor;

		// Token: 0x04000163 RID: 355
		private readonly Encoding encoding;

		// Token: 0x04000164 RID: 356
		[CanBeNull]
		private readonly Union<Type1Font, CompactFontFormatFontCollection> fontProgram;

		// Token: 0x04000165 RID: 357
		private readonly ToUnicodeCMap toUnicodeCMap;

		// Token: 0x04000166 RID: 358
		private readonly TransformationMatrix fontMatrix;
	}
}
