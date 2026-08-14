using System;
using System.Collections.Generic;

namespace UglyToad.PdfPig.Fonts.TrueType.Tables.CMapSubTables
{
	// Token: 0x0200004C RID: 76
	internal class HighByteMappingCMapTable : ICMapSubTable
	{
		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000228 RID: 552 RVA: 0x0000ACC6 File Offset: 0x00008EC6
		public TrueTypeCMapPlatform PlatformId { get; }

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000229 RID: 553 RVA: 0x0000ACCE File Offset: 0x00008ECE
		public ushort EncodingId { get; }

		// Token: 0x0600022A RID: 554 RVA: 0x0000ACD6 File Offset: 0x00008ED6
		private HighByteMappingCMapTable(TrueTypeCMapPlatform platformId, ushort encodingId, IReadOnlyDictionary<int, int> characterCodesToGlyphIndices)
		{
			if (characterCodesToGlyphIndices == null)
			{
				throw new ArgumentNullException("characterCodesToGlyphIndices");
			}
			this.characterCodesToGlyphIndices = characterCodesToGlyphIndices;
			this.PlatformId = platformId;
			this.EncodingId = encodingId;
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000AD04 File Offset: 0x00008F04
		public int CharacterCodeToGlyphIndex(int characterCode)
		{
			int result;
			if (!this.characterCodesToGlyphIndices.TryGetValue(characterCode, ref result))
			{
				return 0;
			}
			return result;
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000AD24 File Offset: 0x00008F24
		public static HighByteMappingCMapTable Load(TrueTypeDataBytes data, int numberOfGlyphs, TrueTypeCMapPlatform platformId, ushort encodingId)
		{
			data.ReadUnsignedShort();
			data.ReadUnsignedShort();
			int[] array = new int[256];
			int num = 0;
			for (int i = 0; i < 256; i++)
			{
				ushort num2 = data.ReadUnsignedShort();
				num = Math.Max(num, (int)(num2 / 8));
				array[i] = (int)num2;
			}
			int num3 = num + 1;
			HighByteMappingCMapTable.SubHeader[] array2 = new HighByteMappingCMapTable.SubHeader[num3];
			for (int j = 0; j < num3; j++)
			{
				ushort firstCode = data.ReadUnsignedShort();
				ushort entryCount = data.ReadUnsignedShort();
				short idDelta = data.ReadSignedShort();
				int idRangeOffset = (int)data.ReadUnsignedShort() - (num3 - j - 1) * 8 - 2;
				array2[j] = new HighByteMappingCMapTable.SubHeader((int)firstCode, (int)entryCount, idDelta, idRangeOffset);
			}
			long position = data.Position;
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			for (int k = 0; k < num3; k++)
			{
				HighByteMappingCMapTable.SubHeader subHeader = array2[k];
				data.Seek(position + (long)subHeader.IdRangeOffset);
				for (int l = 0; l < subHeader.EntryCount; l++)
				{
					int num4 = (k << 8) + (subHeader.FirstCode + l);
					int num5 = (int)data.ReadUnsignedShort();
					if (num5 > 0)
					{
						num5 = (num5 + (int)subHeader.IdDelta) % 65536;
					}
					if (num5 < numberOfGlyphs)
					{
						dictionary[num4] = num5;
					}
				}
			}
			return new HighByteMappingCMapTable(platformId, encodingId, dictionary);
		}

		// Token: 0x040001C3 RID: 451
		private readonly IReadOnlyDictionary<int, int> characterCodesToGlyphIndices;

		// Token: 0x020000CA RID: 202
		public struct SubHeader
		{
			// Token: 0x17000237 RID: 567
			// (get) Token: 0x0600050A RID: 1290 RVA: 0x0001F7BA File Offset: 0x0001D9BA
			public readonly int FirstCode { get; }

			// Token: 0x17000238 RID: 568
			// (get) Token: 0x0600050B RID: 1291 RVA: 0x0001F7C2 File Offset: 0x0001D9C2
			public readonly int EntryCount { get; }

			// Token: 0x17000239 RID: 569
			// (get) Token: 0x0600050C RID: 1292 RVA: 0x0001F7CA File Offset: 0x0001D9CA
			public readonly short IdDelta { get; }

			// Token: 0x1700023A RID: 570
			// (get) Token: 0x0600050D RID: 1293 RVA: 0x0001F7D2 File Offset: 0x0001D9D2
			public readonly int IdRangeOffset { get; }

			// Token: 0x0600050E RID: 1294 RVA: 0x0001F7DA File Offset: 0x0001D9DA
			public SubHeader(int firstCode, int entryCount, short idDelta, int idRangeOffset)
			{
				this.FirstCode = firstCode;
				this.EntryCount = entryCount;
				this.IdDelta = idDelta;
				this.IdRangeOffset = idRangeOffset;
			}
		}
	}
}
