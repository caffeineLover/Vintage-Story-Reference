using System;
using System.Collections.Generic;

namespace UglyToad.PdfPig.PdfFonts.CidFonts
{
	// Token: 0x020000B8 RID: 184
	internal class CharacterIdentifierToGlyphIndexMap
	{
		// Token: 0x06000356 RID: 854 RVA: 0x000153F5 File Offset: 0x000135F5
		public CharacterIdentifierToGlyphIndexMap()
		{
			this.isIdentity = true;
			this.map = null;
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0001540C File Offset: 0x0001360C
		public CharacterIdentifierToGlyphIndexMap(IReadOnlyList<byte> streamBytes)
		{
			int num = streamBytes.Count / 2;
			this.map = new int[num];
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				int num3 = (int)streamBytes[num2] << 8 | (int)streamBytes[num2 + 1];
				this.map[i] = num3;
				num2 += 2;
			}
		}

		// Token: 0x06000358 RID: 856 RVA: 0x00015463 File Offset: 0x00013663
		public int? GetGlyphIndex(int characterIdentifier)
		{
			if (this.isIdentity)
			{
				return new int?(characterIdentifier);
			}
			if (characterIdentifier >= this.map.Length || characterIdentifier < 0)
			{
				return new int?(0);
			}
			return new int?(this.map[characterIdentifier]);
		}

		// Token: 0x040001C9 RID: 457
		private readonly bool isIdentity;

		// Token: 0x040001CA RID: 458
		private readonly int[] map;
	}
}
