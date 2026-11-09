using System;
using System.Collections.Generic;
using System.IO;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.TrueType.Tables
{
	// Token: 0x02000045 RID: 69
	public class Os2Version5OpenTypeTable : Os2Version2To4OpenTypeTable
	{
		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060001FC RID: 508 RVA: 0x0000A520 File Offset: 0x00008720
		public ushort LowerOpticalPointSize { get; }

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060001FD RID: 509 RVA: 0x0000A528 File Offset: 0x00008728
		public ushort UpperOpticalPointSize { get; }

		// Token: 0x060001FE RID: 510 RVA: 0x0000A530 File Offset: 0x00008730
		public Os2Version5OpenTypeTable(TrueTypeHeaderTable directoryTable, ushort version, short xAverageCharacterWidth, ushort weightClass, ushort widthClass, ushort typeFlags, short ySubscriptXSize, short ySubscriptYSize, short ySubscriptXOffset, short ySubscriptYOffset, short ySuperscriptXSize, short ySuperscriptYSize, short ySuperscriptXOffset, short ySuperscriptYOffset, short yStrikeoutSize, short yStrikeoutPosition, short familyClass, IReadOnlyList<byte> panose, IReadOnlyList<uint> unicodeRanges, string vendorId, ushort fontSelectionFlags, ushort firstCharacterIndex, ushort lastCharacterIndex, short typographicAscender, short typographicDescender, short typographicLineGap, ushort windowsAscent, ushort windowsDescent, uint codePage1, uint codePage2, short xHeight, short capHeight, ushort defaultCharacter, ushort breakCharacter, ushort maximumContext, ushort lowerOpticalPointSize, ushort upperOpticalPointSize) : base(directoryTable, version, xAverageCharacterWidth, weightClass, widthClass, typeFlags, ySubscriptXSize, ySubscriptYSize, ySubscriptXOffset, ySubscriptYOffset, ySuperscriptXSize, ySuperscriptYSize, ySuperscriptXOffset, ySuperscriptYOffset, yStrikeoutSize, yStrikeoutPosition, familyClass, panose, unicodeRanges, vendorId, fontSelectionFlags, firstCharacterIndex, lastCharacterIndex, typographicAscender, typographicDescender, typographicLineGap, windowsAscent, windowsDescent, codePage1, codePage2, xHeight, capHeight, defaultCharacter, breakCharacter, maximumContext)
		{
			this.LowerOpticalPointSize = lowerOpticalPointSize;
			this.UpperOpticalPointSize = upperOpticalPointSize;
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000A596 File Offset: 0x00008796
		public override void Write(Stream stream)
		{
			base.Write(stream);
			WritingExtensions.WriteUShort(stream, this.LowerOpticalPointSize);
			WritingExtensions.WriteUShort(stream, this.UpperOpticalPointSize);
		}
	}
}
