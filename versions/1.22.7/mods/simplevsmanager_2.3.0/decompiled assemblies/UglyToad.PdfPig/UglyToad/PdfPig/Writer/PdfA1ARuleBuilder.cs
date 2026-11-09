using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Writer
{
	// Token: 0x02000011 RID: 17
	internal static class PdfA1ARuleBuilder
	{
		// Token: 0x0600005D RID: 93 RVA: 0x000031B4 File Offset: 0x000013B4
		public static void Obey(Dictionary<NameToken, IToken> catalog)
		{
			DictionaryToken dictionaryToken = PdfA1ARuleBuilder.GenerateStructTree();
			catalog[NameToken.StructTreeRoot] = dictionaryToken;
			Dictionary<NameToken, IToken> dictionary = new Dictionary<NameToken, IToken>();
			dictionary.Add(NameToken.Marked, BooleanToken.True);
			DictionaryToken dictionaryToken2 = new DictionaryToken(dictionary);
			catalog[NameToken.MarkInfo] = dictionaryToken2;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000031FA File Offset: 0x000013FA
		private static DictionaryToken GenerateStructTree()
		{
			Dictionary<NameToken, IToken> dictionary = new Dictionary<NameToken, IToken>();
			dictionary.Add(NameToken.Type, NameToken.StructTreeRoot);
			return new DictionaryToken(dictionary);
		}
	}
}
