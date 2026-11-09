using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Fonts.CompactFontFormat.Dictionaries;

namespace UglyToad.PdfPig.Fonts.CompactFontFormat
{
	// Token: 0x02000090 RID: 144
	internal class CompactFontFormat0FdSelect : ICompactFontFormatFdSelect
	{
		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000345 RID: 837 RVA: 0x000172A2 File Offset: 0x000154A2
		public RegistryOrderingSupplement RegistryOrderingSupplement { get; }

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000346 RID: 838 RVA: 0x000172AA File Offset: 0x000154AA
		public IReadOnlyList<int> FontDictionaries { get; }

		// Token: 0x06000347 RID: 839 RVA: 0x000172B2 File Offset: 0x000154B2
		public CompactFontFormat0FdSelect(RegistryOrderingSupplement registryOrderingSupplement, IReadOnlyList<int> fontDictionaries)
		{
			if (registryOrderingSupplement == null)
			{
				throw new ArgumentNullException("registryOrderingSupplement");
			}
			this.RegistryOrderingSupplement = registryOrderingSupplement;
			if (fontDictionaries == null)
			{
				throw new ArgumentNullException("fontDictionaries");
			}
			this.FontDictionaries = fontDictionaries;
		}

		// Token: 0x06000348 RID: 840 RVA: 0x000172E6 File Offset: 0x000154E6
		public int GetFontDictionaryIndex(int glyphId)
		{
			if (glyphId < this.FontDictionaries.Count && glyphId >= 0)
			{
				return this.FontDictionaries[glyphId];
			}
			return 0;
		}
	}
}
