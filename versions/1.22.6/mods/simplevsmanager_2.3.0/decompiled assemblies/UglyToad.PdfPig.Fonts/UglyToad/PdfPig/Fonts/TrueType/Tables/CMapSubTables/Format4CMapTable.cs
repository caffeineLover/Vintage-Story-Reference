using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Fonts.TrueType.Tables.CMapSubTables
{
	// Token: 0x0200004B RID: 75
	internal class Format4CMapTable : ICMapSubTable
	{
		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000220 RID: 544 RVA: 0x0000AAE7 File Offset: 0x00008CE7
		public TrueTypeCMapPlatform PlatformId { get; }

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000221 RID: 545 RVA: 0x0000AAEF File Offset: 0x00008CEF
		public ushort EncodingId { get; }

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000222 RID: 546 RVA: 0x0000AAF7 File Offset: 0x00008CF7
		public ushort Language { get; }

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000223 RID: 547 RVA: 0x0000AAFF File Offset: 0x00008CFF
		public IReadOnlyList<Format4CMapTable.Segment> Segments { get; }

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000224 RID: 548 RVA: 0x0000AB07 File Offset: 0x00008D07
		public IReadOnlyList<ushort> GlyphIds { get; }

		// Token: 0x06000225 RID: 549 RVA: 0x0000AB10 File Offset: 0x00008D10
		public Format4CMapTable(TrueTypeCMapPlatform platformId, ushort encodingId, ushort language, IReadOnlyList<Format4CMapTable.Segment> segments, IReadOnlyList<ushort> glyphIds)
		{
			this.PlatformId = platformId;
			this.EncodingId = encodingId;
			this.Language = language;
			if (segments == null)
			{
				throw new ArgumentNullException("segments");
			}
			this.Segments = segments;
			if (glyphIds == null)
			{
				throw new ArgumentNullException("glyphIds");
			}
			this.GlyphIds = glyphIds;
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000AB68 File Offset: 0x00008D68
		public int CharacterCodeToGlyphIndex(int characterCode)
		{
			int i = 0;
			while (i < this.Segments.Count)
			{
				Format4CMapTable.Segment segment = this.Segments[i];
				if (segment.EndCode >= characterCode && segment.StartCode <= characterCode)
				{
					if (segment.IdRangeOffset == 0)
					{
						return characterCode + segment.IdDelta & 65535;
					}
					int num = segment.IdRangeOffset / 2 + (characterCode - segment.StartCode);
					return (int)this.GlyphIds[num - this.Segments.Count + i];
				}
				else
				{
					i++;
				}
			}
			return 0;
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000ABF8 File Offset: 0x00008DF8
		public static Format4CMapTable Load(TrueTypeDataBytes data, TrueTypeCMapPlatform platformId, ushort encodingId)
		{
			int num = (int)data.ReadUnsignedShort();
			ushort language = data.ReadUnsignedShort();
			int num2 = (int)(data.ReadUnsignedShort() / 2);
			data.ReadUnsignedShort();
			data.ReadUnsignedShort();
			data.ReadUnsignedShort();
			ushort[] array = data.ReadUnsignedShortArray(num2);
			data.ReadUnsignedShort();
			ushort[] array2 = data.ReadUnsignedShortArray(num2);
			short[] array3 = data.ReadShortArray(num2);
			ushort[] array4 = data.ReadUnsignedShortArray(num2);
			int length = (num - (16 + 8 * num2)) / 2;
			ushort[] glyphIds = data.ReadUnsignedShortArray(length);
			Format4CMapTable.Segment[] array5 = new Format4CMapTable.Segment[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				ushort startCode = array2[i];
				ushort endCode = array[i];
				short idDelta = array3[i];
				ushort idRangeOffset = array4[i];
				array5[i] = new Format4CMapTable.Segment((int)startCode, (int)endCode, (int)idDelta, (int)idRangeOffset);
			}
			return new Format4CMapTable(platformId, encodingId, language, array5, glyphIds);
		}

		// Token: 0x020000C9 RID: 201
		public struct Segment
		{
			// Token: 0x17000233 RID: 563
			// (get) Token: 0x06000504 RID: 1284 RVA: 0x0001F6F9 File Offset: 0x0001D8F9
			public readonly int StartCode { get; }

			// Token: 0x17000234 RID: 564
			// (get) Token: 0x06000505 RID: 1285 RVA: 0x0001F701 File Offset: 0x0001D901
			public readonly int EndCode { get; }

			// Token: 0x17000235 RID: 565
			// (get) Token: 0x06000506 RID: 1286 RVA: 0x0001F709 File Offset: 0x0001D909
			public readonly int IdDelta { get; }

			// Token: 0x17000236 RID: 566
			// (get) Token: 0x06000507 RID: 1287 RVA: 0x0001F711 File Offset: 0x0001D911
			public readonly int IdRangeOffset { get; }

			// Token: 0x06000508 RID: 1288 RVA: 0x0001F719 File Offset: 0x0001D919
			public Segment(int startCode, int endCode, int idDelta, int idRangeOffset)
			{
				this.StartCode = startCode;
				this.EndCode = endCode;
				this.IdDelta = idDelta;
				this.IdRangeOffset = idRangeOffset;
			}

			// Token: 0x06000509 RID: 1289 RVA: 0x0001F738 File Offset: 0x0001D938
			public override string ToString()
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(33, 4);
				defaultInterpolatedStringHandler.AppendLiteral("Start: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.StartCode);
				defaultInterpolatedStringHandler.AppendLiteral(", End: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.EndCode);
				defaultInterpolatedStringHandler.AppendLiteral(", Delta: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.IdDelta);
				defaultInterpolatedStringHandler.AppendLiteral(", Offset: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.IdRangeOffset);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}
	}
}
