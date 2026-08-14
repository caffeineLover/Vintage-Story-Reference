using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Geometry;
using UglyToad.PdfPig.PdfFonts.CidFonts;
using UglyToad.PdfPig.PdfFonts.Cmap;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.PdfFonts.Composite
{
	// Token: 0x020000AE RID: 174
	internal class Type0Font : IFont, IVerticalWritingSupported
	{
		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060002F5 RID: 757 RVA: 0x0001452A File Offset: 0x0001272A
		public NameToken Name
		{
			get
			{
				return this.BaseFont;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x00014532 File Offset: 0x00012732
		[NotNull]
		public NameToken BaseFont { get; }

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060002F7 RID: 759 RVA: 0x0001453A File Offset: 0x0001273A
		[NotNull]
		public ICidFont CidFont { get; }

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x00014542 File Offset: 0x00012742
		[NotNull]
		public CMap CMap { get; }

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060002F9 RID: 761 RVA: 0x0001454A File Offset: 0x0001274A
		[NotNull]
		public ToUnicodeCMap ToUnicode { get; }

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060002FA RID: 762 RVA: 0x00014552 File Offset: 0x00012752
		public bool IsVertical
		{
			get
			{
				return this.CMap.WritingMode == WritingMode.Vertical;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060002FB RID: 763 RVA: 0x00014562 File Offset: 0x00012762
		public FontDetails Details { get; }

		// Token: 0x060002FC RID: 764 RVA: 0x0001456C File Offset: 0x0001276C
		public Type0Font(NameToken baseFont, ICidFont cidFont, CMap cmap, CMap toUnicodeCMap, CMap ucs2CMap, bool isChineseJapaneseOrKorean)
		{
			this.ucs2CMap = ucs2CMap;
			this.isChineseJapaneseOrKorean = isChineseJapaneseOrKorean;
			if (baseFont == null)
			{
				throw new ArgumentNullException("baseFont");
			}
			this.BaseFont = baseFont;
			if (cidFont == null)
			{
				throw new ArgumentNullException("cidFont");
			}
			this.CidFont = cidFont;
			if (cmap == null)
			{
				throw new ArgumentNullException("cmap");
			}
			this.CMap = cmap;
			this.ToUnicode = new ToUnicodeCMap(toUnicodeCMap);
			FontDetails details = cidFont.Details;
			this.Details = (((details != null) ? details.WithName(this.Name.Data) : null) ?? FontDetails.GetDefault(this.Name.Data));
		}

		// Token: 0x060002FD RID: 765 RVA: 0x00014620 File Offset: 0x00012820
		public int ReadCharacterCode(IInputBytes bytes, out int codeLength)
		{
			long currentOffset = bytes.CurrentOffset;
			int result = this.CMap.ReadCode(bytes);
			codeLength = (int)(bytes.CurrentOffset - currentOffset);
			return result;
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0001464C File Offset: 0x0001284C
		public bool TryGetUnicode(int characterCode, out string value)
		{
			value = null;
			if (!this.ToUnicode.CanMapToUnicode)
			{
				bool flag = this.ucs2CMap != null;
				if (flag)
				{
					int num = this.CMap.ConvertToCid(characterCode);
					if (num == 0)
					{
						return false;
					}
					if (this.ucs2CMap.TryConvertToUnicode(num, out value))
					{
						return value != null;
					}
				}
				if (flag && this.ucs2CMap.TryConvertToUnicode(characterCode, out value))
				{
					return value != null;
				}
			}
			if (this.ToUnicode.IsUsingIdentityAsUnicodeMap)
			{
				value = new string((char)characterCode, 1);
				return true;
			}
			return this.ToUnicode.TryGet(characterCode, out value);
		}

		// Token: 0x060002FF RID: 767 RVA: 0x000146E0 File Offset: 0x000128E0
		public CharacterBoundingBox GetBoundingBox(int characterCode)
		{
			CharacterBoundingBox result;
			if (this.boundingBoxCache.TryGetValue(characterCode, ref result))
			{
				return result;
			}
			TransformationMatrix fontMatrix = this.GetFontMatrix();
			PdfRectangle pdfRectangle = this.GetBoundingBoxInGlyphSpace(characterCode);
			pdfRectangle = fontMatrix.Transform(pdfRectangle);
			int characterIdentifier = this.CMap.ConvertToCid(characterCode);
			double widthFromFont = this.CidFont.GetWidthFromFont(characterIdentifier);
			double width = fontMatrix.TransformX(widthFromFont);
			CharacterBoundingBox characterBoundingBox = new CharacterBoundingBox(pdfRectangle, width);
			this.boundingBoxCache[characterCode] = characterBoundingBox;
			return characterBoundingBox;
		}

		// Token: 0x06000300 RID: 768 RVA: 0x00014758 File Offset: 0x00012958
		public PdfRectangle GetBoundingBoxInGlyphSpace(int characterCode)
		{
			int characterIdentifier = this.CMap.ConvertToCid(characterCode);
			return this.CidFont.GetBoundingBox(characterIdentifier);
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0001477E File Offset: 0x0001297E
		public TransformationMatrix GetFontMatrix()
		{
			return this.CidFont.FontMatrix;
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0001478C File Offset: 0x0001298C
		public PdfVector GetPositionVector(int characterCode)
		{
			int characterIdentifier = this.CMap.ConvertToCid(characterCode);
			return this.CidFont.GetPositionVector(characterIdentifier).Scale(-0.001);
		}

		// Token: 0x06000303 RID: 771 RVA: 0x000147C4 File Offset: 0x000129C4
		public PdfVector GetDisplacementVector(int characterCode)
		{
			int characterIdentifier = this.CMap.ConvertToCid(characterCode);
			return this.CidFont.GetDisplacementVector(characterIdentifier).Scale(0.001);
		}

		// Token: 0x06000304 RID: 772 RVA: 0x000147FB File Offset: 0x000129FB
		public bool TryGetPath(int characterCode, out IReadOnlyList<PdfSubpath> path)
		{
			return this.CidFont.TryGetPath(characterCode, out path);
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0001480A File Offset: 0x00012A0A
		public bool TryGetNormalisedPath(int characterCode, out IReadOnlyList<PdfSubpath> path)
		{
			return this.CidFont.TryGetNormalisedPath(characterCode, out path);
		}

		// Token: 0x04000195 RID: 405
		private readonly CMap ucs2CMap;

		// Token: 0x04000196 RID: 406
		private readonly bool isChineseJapaneseOrKorean;

		// Token: 0x04000197 RID: 407
		private readonly Dictionary<int, CharacterBoundingBox> boundingBoxCache = new Dictionary<int, CharacterBoundingBox>();
	}
}
