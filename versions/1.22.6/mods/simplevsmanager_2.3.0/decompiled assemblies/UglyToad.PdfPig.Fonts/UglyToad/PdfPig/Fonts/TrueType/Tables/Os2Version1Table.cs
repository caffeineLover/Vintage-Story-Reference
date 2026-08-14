using System;
using System.Collections.Generic;
using System.IO;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.TrueType.Tables
{
	// Token: 0x02000043 RID: 67
	public class Os2Version1Table : Os2RevisedVersion0Table
	{
		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x0000A3A7 File Offset: 0x000085A7
		public uint CodePage1 { get; }

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x0000A3AF File Offset: 0x000085AF
		public uint CodePage2 { get; }

		// Token: 0x060001F3 RID: 499 RVA: 0x0000A3B8 File Offset: 0x000085B8
		public Os2Version1Table(TrueTypeHeaderTable directoryTable, ushort version, short xAverageCharacterWidth, ushort weightClass, ushort widthClass, ushort typeFlags, short ySubscriptXSize, short ySubscriptYSize, short ySubscriptXOffset, short ySubscriptYOffset, short ySuperscriptXSize, short ySuperscriptYSize, short ySuperscriptXOffset, short ySuperscriptYOffset, short yStrikeoutSize, short yStrikeoutPosition, short familyClass, IReadOnlyList<byte> panose, IReadOnlyList<uint> unicodeRanges, string vendorId, ushort fontSelectionFlags, ushort firstCharacterIndex, ushort lastCharacterIndex, short typographicAscender, short typographicDescender, short typographicLineGap, ushort windowsAscent, ushort windowsDescent, uint codePage1, uint codePage2) : base(directoryTable, version, xAverageCharacterWidth, weightClass, widthClass, typeFlags, ySubscriptXSize, ySubscriptYSize, ySubscriptXOffset, ySubscriptYOffset, ySuperscriptXSize, ySuperscriptYSize, ySuperscriptXOffset, ySuperscriptYOffset, yStrikeoutSize, yStrikeoutPosition, familyClass, panose, unicodeRanges, vendorId, fontSelectionFlags, firstCharacterIndex, lastCharacterIndex, typographicAscender, typographicDescender, typographicLineGap, windowsAscent, windowsDescent)
		{
			this.CodePage1 = codePage1;
			this.CodePage2 = codePage2;
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000A410 File Offset: 0x00008610
		public override void Write(Stream stream)
		{
			base.Write(stream);
			WritingExtensions.WriteUInt(stream, this.CodePage1);
			WritingExtensions.WriteUInt(stream, this.CodePage2);
		}
	}
}
