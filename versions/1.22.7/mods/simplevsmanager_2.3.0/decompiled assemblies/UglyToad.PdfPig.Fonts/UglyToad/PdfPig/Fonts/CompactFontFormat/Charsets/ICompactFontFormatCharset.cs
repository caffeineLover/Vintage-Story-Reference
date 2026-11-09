using System;

namespace UglyToad.PdfPig.Fonts.CompactFontFormat.Charsets
{
	// Token: 0x020000AB RID: 171
	internal interface ICompactFontFormatCharset
	{
		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000410 RID: 1040
		bool IsCidCharset { get; }

		// Token: 0x06000411 RID: 1041
		string GetNameByGlyphId(int glyphId);

		// Token: 0x06000412 RID: 1042
		string GetNameByStringId(int stringId);

		// Token: 0x06000413 RID: 1043
		int GetStringIdByGlyphId(int glyphId);

		// Token: 0x06000414 RID: 1044
		int GetGlyphIdByName(string characterName);
	}
}
