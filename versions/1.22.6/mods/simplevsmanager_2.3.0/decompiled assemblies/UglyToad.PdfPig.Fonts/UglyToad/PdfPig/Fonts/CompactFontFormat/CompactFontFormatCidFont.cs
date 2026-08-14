using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts.CompactFontFormat.Charsets;
using UglyToad.PdfPig.Fonts.CompactFontFormat.CharStrings;
using UglyToad.PdfPig.Fonts.CompactFontFormat.Dictionaries;
using UglyToad.PdfPig.Fonts.Type1.CharStrings;

namespace UglyToad.PdfPig.Fonts.CompactFontFormat
{
	// Token: 0x02000087 RID: 135
	internal class CompactFontFormatCidFont : CompactFontFormatFont
	{
		// Token: 0x17000170 RID: 368
		// (get) Token: 0x0600031B RID: 795 RVA: 0x0001660C File Offset: 0x0001480C
		public IReadOnlyList<CompactFontFormatTopLevelDictionary> FontDictionaries { get; }

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x0600031C RID: 796 RVA: 0x00016614 File Offset: 0x00014814
		public IReadOnlyList<CompactFontFormatPrivateDictionary> PrivateDictionaries { get; }

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x0600031D RID: 797 RVA: 0x0001661C File Offset: 0x0001481C
		public ICompactFontFormatFdSelect FdSelect { get; }

		// Token: 0x0600031E RID: 798 RVA: 0x00016624 File Offset: 0x00014824
		public CompactFontFormatCidFont(CompactFontFormatTopLevelDictionary topDictionary, CompactFontFormatPrivateDictionary privateDictionary, ICompactFontFormatCharset charset, Union<Type1CharStrings, Type2CharStrings> charStrings, IReadOnlyList<CompactFontFormatTopLevelDictionary> fontDictionaries, IReadOnlyList<CompactFontFormatPrivateDictionary> privateDictionaries, ICompactFontFormatFdSelect fdSelect) : base(topDictionary, privateDictionary, charset, charStrings, null)
		{
			this.FontDictionaries = fontDictionaries;
			this.PrivateDictionaries = privateDictionaries;
			this.FdSelect = fdSelect;
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0001664C File Offset: 0x0001484C
		protected override decimal GetDefaultWidthX(string characterName)
		{
			CompactFontFormatPrivateDictionary compactFontFormatPrivateDictionary;
			if (!this.TryGetPrivateDictionaryForCharacter(characterName, out compactFontFormatPrivateDictionary))
			{
				return 1000m;
			}
			return compactFontFormatPrivateDictionary.DefaultWidthX;
		}

		// Token: 0x06000320 RID: 800 RVA: 0x00016678 File Offset: 0x00014878
		protected override decimal GetNominalWidthX(string characterName)
		{
			CompactFontFormatPrivateDictionary compactFontFormatPrivateDictionary;
			if (!this.TryGetPrivateDictionaryForCharacter(characterName, out compactFontFormatPrivateDictionary))
			{
				return 0m;
			}
			return compactFontFormatPrivateDictionary.NominalWidthX;
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0001669C File Offset: 0x0001489C
		private bool TryGetPrivateDictionaryForCharacter(string characterName, out CompactFontFormatPrivateDictionary dictionary)
		{
			dictionary = null;
			int glyphIdByName = base.Charset.GetGlyphIdByName(characterName);
			int fontDictionaryIndex = this.FdSelect.GetFontDictionaryIndex(glyphIdByName);
			if (fontDictionaryIndex == -1)
			{
				return false;
			}
			dictionary = this.PrivateDictionaries[fontDictionaryIndex];
			return true;
		}
	}
}
