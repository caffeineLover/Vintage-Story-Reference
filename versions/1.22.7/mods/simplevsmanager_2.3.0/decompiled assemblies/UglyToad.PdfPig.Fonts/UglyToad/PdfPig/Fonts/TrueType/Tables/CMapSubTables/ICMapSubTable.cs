using System;

namespace UglyToad.PdfPig.Fonts.TrueType.Tables.CMapSubTables
{
	// Token: 0x0200004D RID: 77
	public interface ICMapSubTable
	{
		// Token: 0x17000122 RID: 290
		// (get) Token: 0x0600022D RID: 557
		TrueTypeCMapPlatform PlatformId { get; }

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x0600022E RID: 558
		ushort EncodingId { get; }

		// Token: 0x0600022F RID: 559
		int CharacterCodeToGlyphIndex(int characterCode);
	}
}
