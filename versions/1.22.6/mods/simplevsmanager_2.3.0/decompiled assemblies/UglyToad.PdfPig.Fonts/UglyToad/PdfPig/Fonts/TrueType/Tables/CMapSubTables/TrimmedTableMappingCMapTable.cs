using System;
using System.IO;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.TrueType.Tables.CMapSubTables
{
	// Token: 0x0200004E RID: 78
	internal class TrimmedTableMappingCMapTable : ICMapSubTable, IWriteable
	{
		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000230 RID: 560 RVA: 0x0000AE6D File Offset: 0x0000906D
		public TrueTypeCMapPlatform PlatformId { get; }

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000231 RID: 561 RVA: 0x0000AE75 File Offset: 0x00009075
		public ushort EncodingId { get; }

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000232 RID: 562 RVA: 0x0000AE7D File Offset: 0x0000907D
		public int FirstCharacterCode { get; }

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000233 RID: 563 RVA: 0x0000AE85 File Offset: 0x00009085
		public int LastCharacterCode { get; }

		// Token: 0x06000234 RID: 564 RVA: 0x0000AE90 File Offset: 0x00009090
		public TrimmedTableMappingCMapTable(TrueTypeCMapPlatform platformId, ushort encodingId, int firstCharacterCode, int entryCount, ushort[] glyphIndices)
		{
			this.FirstCharacterCode = firstCharacterCode;
			this.entryCount = entryCount;
			if (glyphIndices == null)
			{
				throw new ArgumentNullException("glyphIndices");
			}
			this.glyphIndices = glyphIndices;
			this.LastCharacterCode = firstCharacterCode + entryCount - 1;
			this.PlatformId = platformId;
			this.EncodingId = encodingId;
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000AEE4 File Offset: 0x000090E4
		public int CharacterCodeToGlyphIndex(int characterCode)
		{
			if (characterCode < this.FirstCharacterCode || characterCode > this.FirstCharacterCode + this.entryCount)
			{
				return 0;
			}
			int num = characterCode - this.FirstCharacterCode;
			if (num < 0 || num >= this.glyphIndices.Length)
			{
				return 0;
			}
			return (int)this.glyphIndices[num];
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000AF30 File Offset: 0x00009130
		public static TrimmedTableMappingCMapTable Load(TrueTypeDataBytes data, TrueTypeCMapPlatform platformId, ushort encodingId)
		{
			data.ReadUnsignedShort();
			data.ReadUnsignedShort();
			ushort firstCharacterCode = data.ReadUnsignedShort();
			ushort length = data.ReadUnsignedShort();
			ushort[] array = data.ReadUnsignedShortArray((int)length);
			return new TrimmedTableMappingCMapTable(platformId, encodingId, (int)firstCharacterCode, (int)length, array);
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000AF6C File Offset: 0x0000916C
		public void Write(Stream stream)
		{
			WritingExtensions.WriteUShort(stream, 6);
			ushort num = (ushort)(10 + 2 * this.glyphIndices.Length);
			WritingExtensions.WriteUShort(stream, num);
			WritingExtensions.WriteUShort(stream, 0);
			WritingExtensions.WriteUShort(stream, this.FirstCharacterCode);
			WritingExtensions.WriteUShort(stream, this.glyphIndices.Length);
			for (int i = 0; i < this.glyphIndices.Length; i++)
			{
				WritingExtensions.WriteUShort(stream, this.glyphIndices[i]);
			}
		}

		// Token: 0x040001C6 RID: 454
		private const ushort Format = 6;

		// Token: 0x040001C7 RID: 455
		private const ushort DefaultLanguageId = 0;

		// Token: 0x040001C8 RID: 456
		private readonly int entryCount;

		// Token: 0x040001C9 RID: 457
		private readonly ushort[] glyphIndices;
	}
}
