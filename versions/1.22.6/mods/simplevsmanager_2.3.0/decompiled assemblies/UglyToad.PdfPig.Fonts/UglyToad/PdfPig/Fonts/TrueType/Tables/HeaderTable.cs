using System;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.TrueType.Tables
{
	// Token: 0x02000038 RID: 56
	public class HeaderTable : ITrueTypeTable
	{
		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600017B RID: 379 RVA: 0x000094F9 File Offset: 0x000076F9
		public string Tag
		{
			get
			{
				return "head";
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600017C RID: 380 RVA: 0x00009500 File Offset: 0x00007700
		public TrueTypeHeaderTable DirectoryTable { get; }

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600017D RID: 381 RVA: 0x00009508 File Offset: 0x00007708
		public float Version { get; }

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600017E RID: 382 RVA: 0x00009510 File Offset: 0x00007710
		public float Revision { get; }

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x0600017F RID: 383 RVA: 0x00009518 File Offset: 0x00007718
		public uint CheckSumAdjustment { get; }

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000180 RID: 384 RVA: 0x00009520 File Offset: 0x00007720
		public uint MagicNumber { get; }

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000181 RID: 385 RVA: 0x00009528 File Offset: 0x00007728
		public ushort Flags { get; }

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000182 RID: 386 RVA: 0x00009530 File Offset: 0x00007730
		public ushort UnitsPerEm { get; }

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000183 RID: 387 RVA: 0x00009538 File Offset: 0x00007738
		public DateTime Created { get; }

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000184 RID: 388 RVA: 0x00009540 File Offset: 0x00007740
		public DateTime Modified { get; }

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000185 RID: 389 RVA: 0x00009548 File Offset: 0x00007748
		public PdfRectangle Bounds { get; }

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000186 RID: 390 RVA: 0x00009550 File Offset: 0x00007750
		public HeaderTable.HeaderMacStyle MacStyle { get; }

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000187 RID: 391 RVA: 0x00009558 File Offset: 0x00007758
		public ushort LowestRecommendedPpem { get; }

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000188 RID: 392 RVA: 0x00009560 File Offset: 0x00007760
		public HeaderTable.FontDirection FontDirectionHint { get; }

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000189 RID: 393 RVA: 0x00009568 File Offset: 0x00007768
		public IndexToLocationTable.EntryFormat IndexToLocFormat { get; }

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x0600018A RID: 394 RVA: 0x00009570 File Offset: 0x00007770
		public short GlyphDataFormat { get; }

		// Token: 0x0600018B RID: 395 RVA: 0x00009578 File Offset: 0x00007778
		public HeaderTable(TrueTypeHeaderTable directoryTable, float version, float revision, uint checkSumAdjustment, uint magicNumber, ushort flags, ushort unitsPerEm, DateTime created, DateTime modified, short xMin, short yMin, short xMax, short yMax, ushort macStyle, ushort lowestRecommendedPpem, short fontDirectionHint, IndexToLocationTable.EntryFormat indexToLocFormat, short glyphDataFormat)
		{
			this.DirectoryTable = directoryTable;
			this.Version = version;
			this.Revision = revision;
			this.CheckSumAdjustment = checkSumAdjustment;
			this.MagicNumber = magicNumber;
			this.Flags = flags;
			this.UnitsPerEm = unitsPerEm;
			this.Created = created;
			this.Modified = modified;
			this.Bounds = new PdfRectangle(xMin, yMin, xMax, yMax);
			this.MacStyle = macStyle;
			this.LowestRecommendedPpem = lowestRecommendedPpem;
			this.FontDirectionHint = fontDirectionHint;
			this.IndexToLocFormat = indexToLocFormat;
			this.GlyphDataFormat = glyphDataFormat;
		}

		// Token: 0x0600018C RID: 396 RVA: 0x0000960C File Offset: 0x0000780C
		public static HeaderTable Load(TrueTypeDataBytes data, TrueTypeHeaderTable table)
		{
			data.Seek((long)((ulong)table.Offset));
			float version = data.Read32Fixed();
			float revision = data.Read32Fixed();
			uint checkSumAdjustment = data.ReadUnsignedInt();
			uint num = data.ReadUnsignedInt();
			if (num != 1594834165U)
			{
				throw new InvalidOperationException("The magic number for this TrueType font was incorrect. Value was: " + num.ToString());
			}
			ushort flags = data.ReadUnsignedShort();
			ushort num2 = data.ReadUnsignedShort();
			if (num2 < 16 || num2 > 16384)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(110, 1);
				defaultInterpolatedStringHandler.AppendLiteral("The units per em for this TrueType font was incorrect, value should be between 16 and 16384 but found ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(num2);
				defaultInterpolatedStringHandler.AppendLiteral(" istead.");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			DateTime created;
			try
			{
				created = data.ReadInternationalDate();
			}
			catch (InvalidFontFormatException)
			{
				created = DateTime.MinValue;
			}
			DateTime modified;
			try
			{
				modified = data.ReadInternationalDate();
			}
			catch (InvalidFontFormatException)
			{
				modified = DateTime.MinValue;
			}
			short xMin = data.ReadSignedShort();
			short yMin = data.ReadSignedShort();
			short xMax = data.ReadSignedShort();
			short yMax = data.ReadSignedShort();
			ushort macStyle = data.ReadUnsignedShort();
			ushort lowestRecommendedPpem = data.ReadUnsignedShort();
			short fontDirectionHint = data.ReadSignedShort();
			IndexToLocationTable.EntryFormat indexToLocFormat = (IndexToLocationTable.EntryFormat)data.ReadSignedShort();
			short glyphDataFormat = data.ReadSignedShort();
			return new HeaderTable(table, version, revision, checkSumAdjustment, num, flags, num2, created, modified, xMin, yMin, xMax, yMax, macStyle, lowestRecommendedPpem, fontDirectionHint, indexToLocFormat, glyphDataFormat);
		}

		// Token: 0x020000C6 RID: 198
		public enum FontDirection : short
		{
			// Token: 0x04000462 RID: 1122
			StronglyRightToLeftWithNeutrals = -2,
			// Token: 0x04000463 RID: 1123
			StronglyRightToLeft,
			// Token: 0x04000464 RID: 1124
			FullyMixedDirectional,
			// Token: 0x04000465 RID: 1125
			StronglyLeftToRight,
			// Token: 0x04000466 RID: 1126
			StronglyLeftToRightWithNeutrals
		}

		// Token: 0x020000C7 RID: 199
		[Flags]
		public enum HeaderMacStyle : ushort
		{
			// Token: 0x04000468 RID: 1128
			None = 0,
			// Token: 0x04000469 RID: 1129
			Bold = 1,
			// Token: 0x0400046A RID: 1130
			Italic = 2,
			// Token: 0x0400046B RID: 1131
			Underline = 4,
			// Token: 0x0400046C RID: 1132
			Outline = 8,
			// Token: 0x0400046D RID: 1133
			Shadow = 16,
			// Token: 0x0400046E RID: 1134
			Condensed = 32,
			// Token: 0x0400046F RID: 1135
			Extended = 64
		}
	}
}
