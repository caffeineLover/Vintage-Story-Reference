using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Geometry;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.PdfFonts.CidFonts
{
	// Token: 0x020000BE RID: 190
	internal class Type0CidFont : ICidFont
	{
		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x0600038A RID: 906 RVA: 0x000157A9 File Offset: 0x000139A9
		public NameToken Type { get; }

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x0600038B RID: 907 RVA: 0x000157B1 File Offset: 0x000139B1
		public NameToken SubType { get; }

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x0600038C RID: 908 RVA: 0x000157B9 File Offset: 0x000139B9
		public NameToken BaseFont { get; }

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x0600038D RID: 909 RVA: 0x000157C1 File Offset: 0x000139C1
		public CharacterIdentifierSystemInfo SystemInfo { get; }

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x0600038E RID: 910 RVA: 0x000157CC File Offset: 0x000139CC
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

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x0600038F RID: 911 RVA: 0x0001582E File Offset: 0x00013A2E
		public TransformationMatrix FontMatrix { get; }

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000390 RID: 912 RVA: 0x00015836 File Offset: 0x00013A36
		public CidFontType CidFontType
		{
			get
			{
				return CidFontType.Type0;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000391 RID: 913 RVA: 0x00015839 File Offset: 0x00013A39
		public FontDescriptor Descriptor { get; }

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000392 RID: 914 RVA: 0x00015841 File Offset: 0x00013A41
		public IReadOnlyDictionary<int, double> Widths { get; }

		// Token: 0x06000393 RID: 915 RVA: 0x0001584C File Offset: 0x00013A4C
		public Type0CidFont(ICidFontProgram fontProgram, NameToken type, NameToken subType, NameToken baseFont, CharacterIdentifierSystemInfo systemInfo, FontDescriptor descriptor, VerticalWritingMetrics verticalWritingMetrics, IReadOnlyDictionary<int, double> widths, double? defaultWidth)
		{
			this.fontProgram = fontProgram;
			this.verticalWritingMetrics = verticalWritingMetrics;
			this.defaultWidth = defaultWidth;
			this.scale = 1.0 / (double)((fontProgram != null) ? fontProgram.GetFontMatrixMultiplier() : 1000);
			this.Type = type;
			this.SubType = subType;
			this.BaseFont = baseFont;
			this.SystemInfo = systemInfo;
			this.FontMatrix = TransformationMatrix.FromValues(this.scale, 0.0, 0.0, this.scale, 0.0, 0.0);
			this.Descriptor = descriptor;
			this.Widths = widths;
		}

		// Token: 0x06000394 RID: 916 RVA: 0x00015900 File Offset: 0x00013B00
		public double GetWidthFromFont(int characterCode)
		{
			return this.GetWidthFromDictionary(characterCode);
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0001590C File Offset: 0x00013B0C
		public double GetWidthFromDictionary(int cid)
		{
			if (cid < 0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(43, 1);
				defaultInterpolatedStringHandler.AppendLiteral("The provided character code was negative: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(cid);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			double result;
			if (this.Widths.TryGetValue(cid, ref result))
			{
				return result;
			}
			if (this.defaultWidth != null)
			{
				return this.defaultWidth.Value;
			}
			if (this.Descriptor == null)
			{
				return 1000.0;
			}
			return (double)this.Descriptor.MissingWidth;
		}

		// Token: 0x06000396 RID: 918 RVA: 0x000159A4 File Offset: 0x00013BA4
		public PdfRectangle GetBoundingBox(int characterIdentifier)
		{
			if (characterIdentifier < 0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(49, 1);
				defaultInterpolatedStringHandler.AppendLiteral("The provided character identifier was negative: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(characterIdentifier);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (this.fontProgram == null)
			{
				FontDescriptor descriptor = this.Descriptor;
				if (descriptor == null)
				{
					return new PdfRectangle(0.0, 0.0, 1000.0, 1.0 / this.scale);
				}
				return descriptor.BoundingBox;
			}
			else
			{
				PdfRectangle result;
				if (this.fontProgram.TryGetBoundingBox(characterIdentifier, out result))
				{
					return result;
				}
				double num;
				if (this.Widths.TryGetValue(characterIdentifier, ref num))
				{
					return new PdfRectangle(0.0, 0.0, num, 1.0 / this.scale);
				}
				if (this.defaultWidth != null)
				{
					return new PdfRectangle(0.0, 0.0, this.defaultWidth.Value, 1.0 / this.scale);
				}
				return new PdfRectangle(0.0, 0.0, 1000.0, 1.0 / this.scale);
			}
		}

		// Token: 0x06000397 RID: 919 RVA: 0x00015AF0 File Offset: 0x00013CF0
		public PdfVector GetPositionVector(int characterIdentifier)
		{
			double widthFromFont = this.GetWidthFromFont(characterIdentifier);
			return this.verticalWritingMetrics.GetPositionVector(characterIdentifier, widthFromFont);
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00015B12 File Offset: 0x00013D12
		public PdfVector GetDisplacementVector(int characterIdentifier)
		{
			return this.verticalWritingMetrics.GetDisplacementVector(characterIdentifier);
		}

		// Token: 0x06000399 RID: 921 RVA: 0x00015B20 File Offset: 0x00013D20
		public bool TryGetPath(int characterCode, out IReadOnlyList<PdfSubpath> path)
		{
			path = null;
			return this.fontProgram != null && this.fontProgram.TryGetPath(characterCode, out path);
		}

		// Token: 0x0600039A RID: 922 RVA: 0x00015B3C File Offset: 0x00013D3C
		public bool TryGetPath(int characterCode, Func<int, int?> characterCodeToGlyphId, out IReadOnlyList<PdfSubpath> path)
		{
			path = null;
			return this.fontProgram != null && this.fontProgram.TryGetPath(characterCode, characterCodeToGlyphId, out path);
		}

		// Token: 0x0600039B RID: 923 RVA: 0x00015B59 File Offset: 0x00013D59
		public bool TryGetNormalisedPath(int characterCode, out IReadOnlyList<PdfSubpath> path)
		{
			return this.TryGetPath(characterCode, out path);
		}

		// Token: 0x0600039C RID: 924 RVA: 0x00015B63 File Offset: 0x00013D63
		public bool TryGetNormalisedPath(int characterCode, Func<int, int?> characterCodeToGlyphId, out IReadOnlyList<PdfSubpath> path)
		{
			return this.TryGetPath(characterCode, characterCodeToGlyphId, out path);
		}

		// Token: 0x040001D2 RID: 466
		private readonly ICidFontProgram fontProgram;

		// Token: 0x040001D3 RID: 467
		private readonly VerticalWritingMetrics verticalWritingMetrics;

		// Token: 0x040001D4 RID: 468
		private readonly double? defaultWidth;

		// Token: 0x040001D5 RID: 469
		private readonly double scale;
	}
}
