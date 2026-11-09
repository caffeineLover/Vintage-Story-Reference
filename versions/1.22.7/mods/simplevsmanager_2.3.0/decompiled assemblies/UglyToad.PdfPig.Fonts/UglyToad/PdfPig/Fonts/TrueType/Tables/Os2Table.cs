using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.TrueType.Tables
{
	// Token: 0x02000042 RID: 66
	public class Os2Table : ITrueTypeTable, IWriteable
	{
		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x0000A0C0 File Offset: 0x000082C0
		public string Tag
		{
			get
			{
				return "OS/2";
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060001D8 RID: 472 RVA: 0x0000A0C7 File Offset: 0x000082C7
		public TrueTypeHeaderTable DirectoryTable { get; }

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x0000A0CF File Offset: 0x000082CF
		public ushort Version { get; }

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060001DA RID: 474 RVA: 0x0000A0D7 File Offset: 0x000082D7
		public short XAverageCharacterWidth { get; }

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060001DB RID: 475 RVA: 0x0000A0DF File Offset: 0x000082DF
		public ushort WeightClass { get; }

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060001DC RID: 476 RVA: 0x0000A0E7 File Offset: 0x000082E7
		public ushort WidthClass { get; }

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060001DD RID: 477 RVA: 0x0000A0EF File Offset: 0x000082EF
		public ushort TypeFlags { get; }

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060001DE RID: 478 RVA: 0x0000A0F7 File Offset: 0x000082F7
		public short YSubscriptXSize { get; }

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060001DF RID: 479 RVA: 0x0000A0FF File Offset: 0x000082FF
		public short YSubscriptYSize { get; }

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x0000A107 File Offset: 0x00008307
		public short YSubscriptXOffset { get; }

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060001E1 RID: 481 RVA: 0x0000A10F File Offset: 0x0000830F
		public short YSubscriptYOffset { get; }

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060001E2 RID: 482 RVA: 0x0000A117 File Offset: 0x00008317
		public short YSuperscriptXSize { get; }

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x0000A11F File Offset: 0x0000831F
		public short YSuperscriptYSize { get; }

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x0000A127 File Offset: 0x00008327
		public short YSuperscriptXOffset { get; }

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x0000A12F File Offset: 0x0000832F
		public short YSuperscriptYOffset { get; }

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x0000A137 File Offset: 0x00008337
		public short YStrikeoutSize { get; }

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x0000A13F File Offset: 0x0000833F
		public short YStrikeoutPosition { get; }

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x0000A147 File Offset: 0x00008347
		public short FamilyClass { get; }

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x0000A14F File Offset: 0x0000834F
		public IReadOnlyList<byte> Panose { get; }

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060001EA RID: 490 RVA: 0x0000A157 File Offset: 0x00008357
		public IReadOnlyList<uint> UnicodeRanges { get; }

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060001EB RID: 491 RVA: 0x0000A15F File Offset: 0x0000835F
		public string VendorId { get; }

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060001EC RID: 492 RVA: 0x0000A167 File Offset: 0x00008367
		public ushort FontSelectionFlags { get; }

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060001ED RID: 493 RVA: 0x0000A16F File Offset: 0x0000836F
		public ushort FirstCharacterIndex { get; }

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060001EE RID: 494 RVA: 0x0000A177 File Offset: 0x00008377
		public ushort LastCharacterIndex { get; }

		// Token: 0x060001EF RID: 495 RVA: 0x0000A180 File Offset: 0x00008380
		public Os2Table(TrueTypeHeaderTable directoryTable, ushort version, short xAverageCharacterWidth, ushort weightClass, ushort widthClass, ushort typeFlags, short ySubscriptXSize, short ySubscriptYSize, short ySubscriptXOffset, short ySubscriptYOffset, short ySuperscriptXSize, short ySuperscriptYSize, short ySuperscriptXOffset, short ySuperscriptYOffset, short yStrikeoutSize, short yStrikeoutPosition, short familyClass, IReadOnlyList<byte> panose, IReadOnlyList<uint> unicodeRanges, string vendorId, ushort fontSelectionFlags, ushort firstCharacterIndex, ushort lastCharacterIndex)
		{
			this.DirectoryTable = directoryTable;
			this.Version = version;
			this.XAverageCharacterWidth = xAverageCharacterWidth;
			this.WeightClass = weightClass;
			this.WidthClass = widthClass;
			this.TypeFlags = typeFlags;
			this.YSubscriptXSize = ySubscriptXSize;
			this.YSubscriptYSize = ySubscriptYSize;
			this.YSubscriptXOffset = ySubscriptXOffset;
			this.YSubscriptYOffset = ySubscriptYOffset;
			this.YSuperscriptXSize = ySuperscriptXSize;
			this.YSuperscriptYSize = ySuperscriptYSize;
			this.YSuperscriptXOffset = ySuperscriptXOffset;
			this.YSuperscriptYOffset = ySuperscriptYOffset;
			this.YStrikeoutSize = yStrikeoutSize;
			this.YStrikeoutPosition = yStrikeoutPosition;
			this.FamilyClass = familyClass;
			this.Panose = panose;
			this.UnicodeRanges = unicodeRanges;
			this.VendorId = vendorId;
			this.FontSelectionFlags = fontSelectionFlags;
			this.FirstCharacterIndex = firstCharacterIndex;
			this.LastCharacterIndex = lastCharacterIndex;
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000A248 File Offset: 0x00008448
		public virtual void Write(Stream stream)
		{
			WritingExtensions.WriteUShort(stream, this.Version);
			WritingExtensions.WriteShort(stream, this.XAverageCharacterWidth);
			WritingExtensions.WriteUShort(stream, this.WeightClass);
			WritingExtensions.WriteUShort(stream, this.WidthClass);
			WritingExtensions.WriteShort(stream, this.TypeFlags);
			WritingExtensions.WriteShort(stream, this.YSubscriptXSize);
			WritingExtensions.WriteShort(stream, this.YSubscriptYSize);
			WritingExtensions.WriteShort(stream, this.YSubscriptXOffset);
			WritingExtensions.WriteShort(stream, this.YSubscriptYOffset);
			WritingExtensions.WriteShort(stream, this.YSuperscriptXSize);
			WritingExtensions.WriteShort(stream, this.YSuperscriptYSize);
			WritingExtensions.WriteShort(stream, this.YSuperscriptXOffset);
			WritingExtensions.WriteShort(stream, this.YSuperscriptYOffset);
			WritingExtensions.WriteShort(stream, this.YStrikeoutSize);
			WritingExtensions.WriteShort(stream, this.YStrikeoutPosition);
			WritingExtensions.WriteShort(stream, this.FamilyClass);
			stream.Write(Enumerable.ToArray<byte>(this.Panose), 0, this.Panose.Count);
			for (int i = 0; i < this.UnicodeRanges.Count; i++)
			{
				WritingExtensions.WriteUInt(stream, this.UnicodeRanges[i]);
			}
			for (int j = 0; j < this.VendorId.Length; j++)
			{
				stream.WriteByte((byte)this.VendorId.get_Chars(j));
			}
			WritingExtensions.WriteUShort(stream, this.FontSelectionFlags);
			WritingExtensions.WriteUShort(stream, this.FirstCharacterIndex);
			WritingExtensions.WriteUShort(stream, this.LastCharacterIndex);
		}
	}
}
