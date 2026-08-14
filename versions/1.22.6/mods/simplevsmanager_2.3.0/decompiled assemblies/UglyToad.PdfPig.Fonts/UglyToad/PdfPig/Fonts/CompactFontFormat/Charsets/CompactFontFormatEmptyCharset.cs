using System;

namespace UglyToad.PdfPig.Fonts.CompactFontFormat.Charsets
{
	// Token: 0x020000A4 RID: 164
	internal class CompactFontFormatEmptyCharset : ICompactFontFormatCharset
	{
		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x060003EF RID: 1007 RVA: 0x0001B978 File Offset: 0x00019B78
		public bool IsCidCharset { get; } = 1;

		// Token: 0x060003F0 RID: 1008 RVA: 0x0001B980 File Offset: 0x00019B80
		public CompactFontFormatEmptyCharset(int numberOfCharstrings)
		{
			this.numberOfCharstrings = numberOfCharstrings;
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0001B996 File Offset: 0x00019B96
		public string GetNameByGlyphId(int glyphId)
		{
			throw new NotSupportedException("Cid Charsets do not support named glyphs.");
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0001B9A2 File Offset: 0x00019BA2
		public string GetNameByStringId(int stringId)
		{
			throw new NotSupportedException("Cid Charsets do not support named glyphs.");
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0001B9AE File Offset: 0x00019BAE
		public int GetStringIdByGlyphId(int glyphId)
		{
			throw new NotSupportedException("Cid Charsets do not support named glyphs.");
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0001B9BA File Offset: 0x00019BBA
		public int GetGlyphIdByName(string characterName)
		{
			return 0;
		}

		// Token: 0x04000371 RID: 881
		private readonly int numberOfCharstrings;
	}
}
