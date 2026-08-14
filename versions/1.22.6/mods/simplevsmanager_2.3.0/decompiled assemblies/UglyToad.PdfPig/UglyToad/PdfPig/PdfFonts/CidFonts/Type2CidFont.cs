using System;
using System.Collections.Generic;
using System.Linq;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Geometry;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.PdfFonts.CidFonts
{
	// Token: 0x020000BF RID: 191
	internal class Type2CidFont : ICidFont
	{
		// Token: 0x170000DE RID: 222
		// (get) Token: 0x0600039D RID: 925 RVA: 0x00015B6E File Offset: 0x00013D6E
		public NameToken Type { get; }

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600039E RID: 926 RVA: 0x00015B76 File Offset: 0x00013D76
		public NameToken SubType { get; }

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600039F RID: 927 RVA: 0x00015B7E File Offset: 0x00013D7E
		public NameToken BaseFont { get; }

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060003A0 RID: 928 RVA: 0x00015B86 File Offset: 0x00013D86
		public CharacterIdentifierSystemInfo SystemInfo { get; }

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060003A1 RID: 929 RVA: 0x00015B8E File Offset: 0x00013D8E
		public TransformationMatrix FontMatrix { get; }

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060003A2 RID: 930 RVA: 0x00015B96 File Offset: 0x00013D96
		public CidFontType CidFontType
		{
			get
			{
				return CidFontType.Type2;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x00015B99 File Offset: 0x00013D99
		public FontDescriptor Descriptor { get; }

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x00015BA4 File Offset: 0x00013DA4
		public FontDetails Details
		{
			get
			{
				ICidFontProgram cidFontProgram = this.fontProgram;
				FontDetails result;
				if ((result = ((cidFontProgram != null) ? cidFontProgram.Details : null)) == null)
				{
					FontDescriptor descriptor = this.Descriptor;
					FontDetails fontDetails;
					if (descriptor == null)
					{
						fontDetails = null;
					}
					else
					{
						NameToken baseFont = this.BaseFont;
						fontDetails = descriptor.ToDetails((baseFont != null) ? baseFont.Data : null);
					}
					if ((result = fontDetails) == null)
					{
						NameToken baseFont2 = this.BaseFont;
						result = FontDetails.GetDefault((baseFont2 != null) ? baseFont2.Data : null);
					}
				}
				return result;
			}
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x00015C08 File Offset: 0x00013E08
		public Type2CidFont(NameToken type, NameToken subType, NameToken baseFont, CharacterIdentifierSystemInfo systemInfo, FontDescriptor descriptor, ICidFontProgram fontProgram, VerticalWritingMetrics verticalWritingMetrics, IReadOnlyDictionary<int, double> widths, double? defaultWidth, CharacterIdentifierToGlyphIndexMap cidToGid)
		{
			this.Type = type;
			this.SubType = subType;
			this.BaseFont = baseFont;
			this.SystemInfo = systemInfo;
			this.Descriptor = descriptor;
			this.fontProgram = fontProgram;
			this.verticalWritingMetrics = verticalWritingMetrics;
			this.widths = widths;
			this.defaultWidth = defaultWidth;
			this.cidToGid = cidToGid;
			double num = 1.0 / (double)((fontProgram != null) ? fontProgram.GetFontMatrixMultiplier() : 1000);
			this.FontMatrix = TransformationMatrix.FromValues(num, 0.0, 0.0, num, 0.0, 0.0);
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x00015CB8 File Offset: 0x00013EB8
		public double GetWidthFromFont(int characterIdentifier)
		{
			if (this.fontProgram == null)
			{
				return this.GetWidthFromDictionary(characterIdentifier);
			}
			double result;
			if (this.fontProgram.TryGetBoundingAdvancedWidth(characterIdentifier, new Func<int, int?>(this.cidToGid.GetGlyphIndex), out result))
			{
				return result;
			}
			return this.GetWidthFromDictionary(characterIdentifier);
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x00015D00 File Offset: 0x00013F00
		public double GetWidthFromDictionary(int characterIdentifier)
		{
			double result;
			if (this.widths.TryGetValue(characterIdentifier, ref result))
			{
				return result;
			}
			if (this.defaultWidth != null)
			{
				return this.defaultWidth.Value;
			}
			FontDescriptor descriptor = this.Descriptor;
			return (double)((descriptor != null) ? descriptor.MissingWidth : 1000m);
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x00015D5C File Offset: 0x00013F5C
		public PdfRectangle GetBoundingBox(int characterIdentifier)
		{
			if (this.fontProgram == null)
			{
				return this.Descriptor.BoundingBox;
			}
			PdfRectangle result;
			if (this.fontProgram.TryGetBoundingBox(characterIdentifier, new Func<int, int?>(this.cidToGid.GetGlyphIndex), out result))
			{
				return result;
			}
			return this.Descriptor.BoundingBox;
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x00015DAC File Offset: 0x00013FAC
		public PdfVector GetPositionVector(int characterIdentifier)
		{
			double widthFromFont = this.GetWidthFromFont(characterIdentifier);
			return this.verticalWritingMetrics.GetPositionVector(characterIdentifier, widthFromFont);
		}

		// Token: 0x060003AA RID: 938 RVA: 0x00015DCE File Offset: 0x00013FCE
		public PdfVector GetDisplacementVector(int characterIdentifier)
		{
			return this.verticalWritingMetrics.GetDisplacementVector(characterIdentifier);
		}

		// Token: 0x060003AB RID: 939 RVA: 0x00015DDC File Offset: 0x00013FDC
		public bool TryGetPath(int characterCode, out IReadOnlyList<PdfSubpath> path)
		{
			return this.TryGetPath(characterCode, new Func<int, int?>(this.cidToGid.GetGlyphIndex), out path);
		}

		// Token: 0x060003AC RID: 940 RVA: 0x00015DF7 File Offset: 0x00013FF7
		public bool TryGetPath(int characterCode, Func<int, int?> characterCodeToGlyphId, out IReadOnlyList<PdfSubpath> path)
		{
			path = null;
			return this.fontProgram != null && this.fontProgram.TryGetPath(characterCode, characterCodeToGlyphId, out path);
		}

		// Token: 0x060003AD RID: 941 RVA: 0x00015E14 File Offset: 0x00014014
		public bool TryGetNormalisedPath(int characterCode, out IReadOnlyList<PdfSubpath> path)
		{
			if (!this.TryGetPath(characterCode, out path))
			{
				return false;
			}
			path = Enumerable.ToList<PdfSubpath>(this.FontMatrix.Transform(path));
			return true;
		}

		// Token: 0x060003AE RID: 942 RVA: 0x00015E45 File Offset: 0x00014045
		public bool TryGetNormalisedPath(int characterCode, Func<int, int?> characterCodeToGlyphId, out IReadOnlyList<PdfSubpath> path)
		{
			throw new NotImplementedException();
		}

		// Token: 0x040001DD RID: 477
		private readonly ICidFontProgram fontProgram;

		// Token: 0x040001DE RID: 478
		private readonly VerticalWritingMetrics verticalWritingMetrics;

		// Token: 0x040001DF RID: 479
		private readonly IReadOnlyDictionary<int, double> widths;

		// Token: 0x040001E0 RID: 480
		private readonly double? defaultWidth;

		// Token: 0x040001E1 RID: 481
		private readonly CharacterIdentifierToGlyphIndexMap cidToGid;
	}
}
