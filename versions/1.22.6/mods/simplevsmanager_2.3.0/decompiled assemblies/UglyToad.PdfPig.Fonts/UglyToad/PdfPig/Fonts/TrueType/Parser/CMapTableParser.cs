using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Fonts.TrueType.Tables;
using UglyToad.PdfPig.Fonts.TrueType.Tables.CMapSubTables;

namespace UglyToad.PdfPig.Fonts.TrueType.Parser
{
	// Token: 0x02000055 RID: 85
	internal class CMapTableParser : ITrueTypeTableParser<CMapTable>
	{
		// Token: 0x06000255 RID: 597 RVA: 0x0000BF34 File Offset: 0x0000A134
		public CMapTable Parse(TrueTypeHeaderTable header, TrueTypeDataBytes data, TableRegister.Builder register)
		{
			data.Seek((long)((ulong)header.Offset));
			ushort version = data.ReadUnsignedShort();
			ushort num = data.ReadUnsignedShort();
			CMapTableParser.SubTableHeaderEntry[] array = new CMapTableParser.SubTableHeaderEntry[(int)num];
			for (int i = 0; i < (int)num; i++)
			{
				TrueTypeCMapPlatform platformId = (TrueTypeCMapPlatform)data.ReadUnsignedShort();
				ushort encodingId = data.ReadUnsignedShort();
				uint num2 = data.ReadUnsignedInt();
				array[i] = new CMapTableParser.SubTableHeaderEntry(platformId, encodingId, (long)((ulong)num2));
			}
			List<ICMapSubTable> list = new List<ICMapSubTable>((int)num);
			int numberOfGlyphs = register.MaximumProfileTable.NumberOfGlyphs;
			foreach (CMapTableParser.SubTableHeaderEntry subTableHeaderEntry in array)
			{
				data.Seek((long)((ulong)header.Offset + (ulong)subTableHeaderEntry.Offset));
				switch (data.ReadUnsignedShort())
				{
				case 0:
				{
					ByteEncodingCMapTable byteEncodingCMapTable = ByteEncodingCMapTable.Load(data, subTableHeaderEntry.PlatformId, subTableHeaderEntry.EncodingId);
					list.Add(byteEncodingCMapTable);
					break;
				}
				case 2:
				{
					HighByteMappingCMapTable highByteMappingCMapTable = HighByteMappingCMapTable.Load(data, numberOfGlyphs, subTableHeaderEntry.PlatformId, subTableHeaderEntry.EncodingId);
					list.Add(highByteMappingCMapTable);
					break;
				}
				case 4:
				{
					Format4CMapTable format4CMapTable = Format4CMapTable.Load(data, subTableHeaderEntry.PlatformId, subTableHeaderEntry.EncodingId);
					list.Add(format4CMapTable);
					break;
				}
				case 6:
				{
					TrimmedTableMappingCMapTable trimmedTableMappingCMapTable = TrimmedTableMappingCMapTable.Load(data, subTableHeaderEntry.PlatformId, subTableHeaderEntry.EncodingId);
					list.Add(trimmedTableMappingCMapTable);
					break;
				}
				}
			}
			return new CMapTable(version, header, list);
		}

		// Token: 0x020000D0 RID: 208
		private class SubTableHeaderEntry
		{
			// Token: 0x1700024A RID: 586
			// (get) Token: 0x06000526 RID: 1318 RVA: 0x0001F995 File Offset: 0x0001DB95
			public TrueTypeCMapPlatform PlatformId { get; }

			// Token: 0x1700024B RID: 587
			// (get) Token: 0x06000527 RID: 1319 RVA: 0x0001F99D File Offset: 0x0001DB9D
			public ushort EncodingId { get; }

			// Token: 0x1700024C RID: 588
			// (get) Token: 0x06000528 RID: 1320 RVA: 0x0001F9A5 File Offset: 0x0001DBA5
			public long Offset { get; }

			// Token: 0x06000529 RID: 1321 RVA: 0x0001F9AD File Offset: 0x0001DBAD
			public SubTableHeaderEntry(TrueTypeCMapPlatform platformId, ushort encodingId, long offset)
			{
				this.PlatformId = platformId;
				this.EncodingId = encodingId;
				this.Offset = offset;
			}
		}
	}
}
