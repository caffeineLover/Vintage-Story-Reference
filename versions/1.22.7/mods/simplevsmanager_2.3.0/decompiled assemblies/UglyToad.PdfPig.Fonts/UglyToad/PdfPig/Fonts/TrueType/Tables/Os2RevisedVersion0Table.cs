using System;
using System.Collections.Generic;
using System.IO;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.TrueType.Tables
{
	// Token: 0x02000041 RID: 65
	public class Os2RevisedVersion0Table : Os2Table
	{
		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060001D0 RID: 464 RVA: 0x00009FE0 File Offset: 0x000081E0
		public short TypographicAscender { get; }

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x00009FE8 File Offset: 0x000081E8
		public short TypographicDescender { get; }

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060001D2 RID: 466 RVA: 0x00009FF0 File Offset: 0x000081F0
		public short TypographicLineGap { get; }

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x00009FF8 File Offset: 0x000081F8
		public ushort WindowsAscent { get; }

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060001D4 RID: 468 RVA: 0x0000A000 File Offset: 0x00008200
		public ushort WindowsDescent { get; }

		// Token: 0x060001D5 RID: 469 RVA: 0x0000A008 File Offset: 0x00008208
		public Os2RevisedVersion0Table(TrueTypeHeaderTable directoryTable, ushort version, short xAverageCharacterWidth, ushort weightClass, ushort widthClass, ushort typeFlags, short ySubscriptXSize, short ySubscriptYSize, short ySubscriptXOffset, short ySubscriptYOffset, short ySuperscriptXSize, short ySuperscriptYSize, short ySuperscriptXOffset, short ySuperscriptYOffset, short yStrikeoutSize, short yStrikeoutPosition, short familyClass, IReadOnlyList<byte> panose, IReadOnlyList<uint> unicodeRanges, string vendorId, ushort fontSelectionFlags, ushort firstCharacterIndex, ushort lastCharacterIndex, short typographicAscender, short typographicDescender, short typographicLineGap, ushort windowsAscent, ushort windowsDescent) : base(directoryTable, version, xAverageCharacterWidth, weightClass, widthClass, typeFlags, ySubscriptXSize, ySubscriptYSize, ySubscriptXOffset, ySubscriptYOffset, ySuperscriptXSize, ySuperscriptYSize, ySuperscriptXOffset, ySuperscriptYOffset, yStrikeoutSize, yStrikeoutPosition, familyClass, panose, unicodeRanges, vendorId, fontSelectionFlags, firstCharacterIndex, lastCharacterIndex)
		{
			this.TypographicAscender = typographicAscender;
			this.TypographicDescender = typographicDescender;
			this.TypographicLineGap = typographicLineGap;
			this.WindowsAscent = windowsAscent;
			this.WindowsDescent = windowsDescent;
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000A070 File Offset: 0x00008270
		public override void Write(Stream stream)
		{
			base.Write(stream);
			WritingExtensions.WriteShort(stream, this.TypographicAscender);
			WritingExtensions.WriteShort(stream, this.TypographicDescender);
			WritingExtensions.WriteShort(stream, this.TypographicLineGap);
			WritingExtensions.WriteUShort(stream, this.WindowsAscent);
			WritingExtensions.WriteUShort(stream, this.WindowsDescent);
		}
	}
}
