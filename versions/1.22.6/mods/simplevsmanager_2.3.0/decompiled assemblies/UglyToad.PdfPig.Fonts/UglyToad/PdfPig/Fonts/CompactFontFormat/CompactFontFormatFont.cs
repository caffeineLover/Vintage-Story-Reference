using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts.CompactFontFormat.Charsets;
using UglyToad.PdfPig.Fonts.CompactFontFormat.CharStrings;
using UglyToad.PdfPig.Fonts.CompactFontFormat.Dictionaries;
using UglyToad.PdfPig.Fonts.Encodings;
using UglyToad.PdfPig.Fonts.Type1.CharStrings;

namespace UglyToad.PdfPig.Fonts.CompactFontFormat
{
	// Token: 0x02000086 RID: 134
	public class CompactFontFormatFont
	{
		// Token: 0x17000168 RID: 360
		// (get) Token: 0x0600030D RID: 781 RVA: 0x000163DC File Offset: 0x000145DC
		internal CompactFontFormatTopLevelDictionary TopDictionary { get; }

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x0600030E RID: 782 RVA: 0x000163E4 File Offset: 0x000145E4
		internal CompactFontFormatPrivateDictionary PrivateDictionary { get; }

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x0600030F RID: 783 RVA: 0x000163EC File Offset: 0x000145EC
		internal ICompactFontFormatCharset Charset { get; }

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000310 RID: 784 RVA: 0x000163F4 File Offset: 0x000145F4
		internal Union<Type1CharStrings, Type2CharStrings> CharStrings { get; }

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000311 RID: 785 RVA: 0x000163FC File Offset: 0x000145FC
		public Encoding Encoding { get; }

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000312 RID: 786 RVA: 0x00016404 File Offset: 0x00014604
		public TransformationMatrix FontMatrix
		{
			get
			{
				return this.TopDictionary.FontMatrix;
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000313 RID: 787 RVA: 0x00016411 File Offset: 0x00014611
		public string Weight
		{
			get
			{
				CompactFontFormatTopLevelDictionary topDictionary = this.TopDictionary;
				if (topDictionary == null)
				{
					return null;
				}
				return topDictionary.Weight;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000314 RID: 788 RVA: 0x00016424 File Offset: 0x00014624
		public decimal ItalicAngle
		{
			get
			{
				CompactFontFormatTopLevelDictionary topDictionary = this.TopDictionary;
				if (topDictionary == null)
				{
					return 0m;
				}
				return topDictionary.ItalicAngle;
			}
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0001643B File Offset: 0x0001463B
		internal CompactFontFormatFont(CompactFontFormatTopLevelDictionary topDictionary, CompactFontFormatPrivateDictionary privateDictionary, ICompactFontFormatCharset charset, Union<Type1CharStrings, Type2CharStrings> charStrings, Encoding fontEncoding)
		{
			this.TopDictionary = topDictionary;
			this.PrivateDictionary = privateDictionary;
			this.Charset = charset;
			this.CharStrings = charStrings;
			this.Encoding = fontEncoding;
		}

		// Token: 0x06000316 RID: 790 RVA: 0x00016468 File Offset: 0x00014668
		public PdfRectangle? GetCharacterBoundingBox(string characterName)
		{
			decimal defaultWidthX = this.GetDefaultWidthX(characterName);
			decimal nominalWidthX = this.GetNominalWidthX(characterName);
			Type1CharStrings type1CharStrings;
			if (this.CharStrings.TryGetFirst(ref type1CharStrings))
			{
				throw new NotImplementedException("Type 1 CharStrings in a CFF font are currently unsupported.");
			}
			Type2CharStrings type2CharStrings;
			if (!this.CharStrings.TryGetSecond(ref type2CharStrings))
			{
				return default(PdfRectangle?);
			}
			Type2Glyph type2Glyph = type2CharStrings.Generate(characterName, (double)defaultWidthX, (double)nominalWidthX);
			PdfRectangle? boundingRectangle = PdfSubpath.GetBoundingRectangle(type2Glyph.Path);
			if (boundingRectangle != null)
			{
				return boundingRectangle;
			}
			PdfRectangle fontBoundingBox = this.TopDictionary.FontBoundingBox;
			return new PdfRectangle?(new PdfRectangle(0.0, 0.0, type2Glyph.Width.GetValueOrDefault(), fontBoundingBox.Height));
		}

		// Token: 0x06000317 RID: 791 RVA: 0x00016528 File Offset: 0x00014728
		public bool TryGetPath(string characterName, out IReadOnlyList<PdfSubpath> path)
		{
			decimal defaultWidthX = this.GetDefaultWidthX(characterName);
			decimal nominalWidthX = this.GetNominalWidthX(characterName);
			Type1CharStrings type1CharStrings;
			if (this.CharStrings.TryGetFirst(ref type1CharStrings))
			{
				throw new NotImplementedException("Type 1 CharStrings in a CFF font are currently unsupported.");
			}
			Type2CharStrings type2CharStrings;
			if (!this.CharStrings.TryGetSecond(ref type2CharStrings))
			{
				path = null;
				return false;
			}
			path = type2CharStrings.Generate(characterName, (double)defaultWidthX, (double)nominalWidthX).Path;
			return true;
		}

		// Token: 0x06000318 RID: 792 RVA: 0x00016590 File Offset: 0x00014790
		public IReadOnlyList<PdfSubpath> GetCharacterPath(string characterName)
		{
			decimal defaultWidthX = this.GetDefaultWidthX(characterName);
			decimal nominalWidthX = this.GetNominalWidthX(characterName);
			Type1CharStrings type1CharStrings;
			if (this.CharStrings.TryGetFirst(ref type1CharStrings))
			{
				throw new NotImplementedException("Type 1 CharStrings in a CFF font are currently unsupported.");
			}
			Type2CharStrings type2CharStrings;
			if (!this.CharStrings.TryGetSecond(ref type2CharStrings))
			{
				return null;
			}
			return type2CharStrings.Generate(characterName, (double)defaultWidthX, (double)nominalWidthX).Path;
		}

		// Token: 0x06000319 RID: 793 RVA: 0x000165F2 File Offset: 0x000147F2
		protected virtual decimal GetDefaultWidthX(string characterName)
		{
			return this.PrivateDictionary.DefaultWidthX;
		}

		// Token: 0x0600031A RID: 794 RVA: 0x000165FF File Offset: 0x000147FF
		protected virtual decimal GetNominalWidthX(string characterName)
		{
			return this.PrivateDictionary.NominalWidthX;
		}
	}
}
