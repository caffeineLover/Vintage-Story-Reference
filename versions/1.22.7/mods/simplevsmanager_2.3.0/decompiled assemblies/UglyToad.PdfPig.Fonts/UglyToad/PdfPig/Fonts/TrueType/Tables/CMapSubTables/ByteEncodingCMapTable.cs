using System;
using System.IO;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.TrueType.Tables.CMapSubTables
{
	// Token: 0x0200004A RID: 74
	internal class ByteEncodingCMapTable : ICMapSubTable, IWriteable
	{
		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000219 RID: 537 RVA: 0x0000AA19 File Offset: 0x00008C19
		public TrueTypeCMapPlatform PlatformId { get; }

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x0600021A RID: 538 RVA: 0x0000AA21 File Offset: 0x00008C21
		public ushort EncodingId { get; }

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x0600021B RID: 539 RVA: 0x0000AA29 File Offset: 0x00008C29
		public ushort LanguageId { get; }

		// Token: 0x0600021C RID: 540 RVA: 0x0000AA31 File Offset: 0x00008C31
		public ByteEncodingCMapTable(TrueTypeCMapPlatform platformId, ushort encodingId, ushort languageId, byte[] glyphMapping)
		{
			this.glyphMapping = glyphMapping;
			this.PlatformId = platformId;
			this.EncodingId = encodingId;
			this.LanguageId = languageId;
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000AA58 File Offset: 0x00008C58
		public static ByteEncodingCMapTable Load(TrueTypeDataBytes data, TrueTypeCMapPlatform platformId, ushort encodingId)
		{
			ushort num = data.ReadUnsignedShort();
			ushort languageId = data.ReadUnsignedShort();
			byte[] array = data.ReadByteArray((int)(num - 6));
			return new ByteEncodingCMapTable(platformId, encodingId, languageId, array);
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000AA86 File Offset: 0x00008C86
		public int CharacterCodeToGlyphIndex(int characterCode)
		{
			if (characterCode < 0 || characterCode >= 256)
			{
				return 0;
			}
			return (int)this.glyphMapping[characterCode];
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000AAA0 File Offset: 0x00008CA0
		public void Write(Stream stream)
		{
			WritingExtensions.WriteUShort(stream, 0);
			WritingExtensions.WriteUShort(stream, 262);
			WritingExtensions.WriteUShort(stream, 0);
			for (int i = 0; i < this.glyphMapping.Length; i++)
			{
				stream.WriteByte(this.glyphMapping[i]);
			}
		}

		// Token: 0x040001B6 RID: 438
		private const ushort Format = 0;

		// Token: 0x040001B7 RID: 439
		private const ushort DefaultLanguageId = 0;

		// Token: 0x040001B8 RID: 440
		private const int SizeOfShort = 2;

		// Token: 0x040001B9 RID: 441
		private const int GlyphMappingLength = 256;

		// Token: 0x040001BA RID: 442
		private readonly byte[] glyphMapping;
	}
}
