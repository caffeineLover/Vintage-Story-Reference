using System;
using System.Collections.Generic;
using System.IO;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.TrueType.Tables
{
	// Token: 0x02000044 RID: 68
	public class Os2Version2To4OpenTypeTable : Os2Version1Table
	{
		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x0000A431 File Offset: 0x00008631
		public short XHeight { get; }

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x0000A439 File Offset: 0x00008639
		public short CapHeight { get; }

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x0000A441 File Offset: 0x00008641
		public ushort DefaultCharacter { get; }

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x0000A449 File Offset: 0x00008649
		public ushort BreakCharacter { get; }

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x0000A451 File Offset: 0x00008651
		public ushort MaximumContext { get; }

		// Token: 0x060001FA RID: 506 RVA: 0x0000A45C File Offset: 0x0000865C
		public Os2Version2To4OpenTypeTable(TrueTypeHeaderTable directoryTable, ushort version, short xAverageCharacterWidth, ushort weightClass, ushort widthClass, ushort typeFlags, short ySubscriptXSize, short ySubscriptYSize, short ySubscriptXOffset, short ySubscriptYOffset, short ySuperscriptXSize, short ySuperscriptYSize, short ySuperscriptXOffset, short ySuperscriptYOffset, short yStrikeoutSize, short yStrikeoutPosition, short familyClass, IReadOnlyList<byte> panose, IReadOnlyList<uint> unicodeRanges, string vendorId, ushort fontSelectionFlags, ushort firstCharacterIndex, ushort lastCharacterIndex, short typographicAscender, short typographicDescender, short typographicLineGap, ushort windowsAscent, ushort windowsDescent, uint codePage1, uint codePage2, short xHeight, short capHeight, ushort defaultCharacter, ushort breakCharacter, ushort maximumContext) : base(directoryTable, version, xAverageCharacterWidth, weightClass, widthClass, typeFlags, ySubscriptXSize, ySubscriptYSize, ySubscriptXOffset, ySubscriptYOffset, ySuperscriptXSize, ySuperscriptYSize, ySuperscriptXOffset, ySuperscriptYOffset, yStrikeoutSize, yStrikeoutPosition, familyClass, panose, unicodeRanges, vendorId, fontSelectionFlags, firstCharacterIndex, lastCharacterIndex, typographicAscender, typographicDescender, typographicLineGap, windowsAscent, windowsDescent, codePage1, codePage2)
		{
			this.XHeight = xHeight;
			this.CapHeight = capHeight;
			this.DefaultCharacter = defaultCharacter;
			this.BreakCharacter = breakCharacter;
			this.MaximumContext = maximumContext;
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000A4D0 File Offset: 0x000086D0
		public override void Write(Stream stream)
		{
			base.Write(stream);
			WritingExtensions.WriteShort(stream, this.XHeight);
			WritingExtensions.WriteShort(stream, this.CapHeight);
			WritingExtensions.WriteUShort(stream, this.DefaultCharacter);
			WritingExtensions.WriteUShort(stream, this.BreakCharacter);
			WritingExtensions.WriteUShort(stream, this.MaximumContext);
		}
	}
}
