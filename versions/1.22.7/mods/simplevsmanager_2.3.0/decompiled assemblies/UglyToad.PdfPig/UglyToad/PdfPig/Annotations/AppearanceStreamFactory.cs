using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Annotations
{
	// Token: 0x02000200 RID: 512
	internal static class AppearanceStreamFactory
	{
		// Token: 0x06000C5D RID: 3165 RVA: 0x0003DA98 File Offset: 0x0003BC98
		public static bool TryCreate(DictionaryToken appearanceDictionary, NameToken name, IPdfTokenScanner tokenScanner, out AppearanceStream appearanceStream)
		{
			IndirectReferenceToken indirectReferenceToken;
			if (appearanceDictionary.TryGet<IndirectReferenceToken>(name, ref indirectReferenceToken))
			{
				ObjectToken objectToken = tokenScanner.Get(indirectReferenceToken.Data);
				StreamToken streamToken = ((objectToken != null) ? objectToken.Data : null) as StreamToken;
				appearanceStream = new AppearanceStream(streamToken);
				return true;
			}
			DictionaryToken dictionaryToken;
			if (appearanceDictionary.TryGet<DictionaryToken>(name, ref dictionaryToken))
			{
				Dictionary<string, StreamToken> dictionary = new Dictionary<string, StreamToken>();
				foreach (string text in dictionaryToken.Data.Keys)
				{
					IToken token;
					if (dictionaryToken.Data.TryGetValue(text, ref token))
					{
						IndirectReferenceToken indirectReferenceToken2 = token as IndirectReferenceToken;
						if (indirectReferenceToken2 != null)
						{
							ObjectToken objectToken2 = tokenScanner.Get(indirectReferenceToken2.Data);
							StreamToken streamToken2 = ((objectToken2 != null) ? objectToken2.Data : null) as StreamToken;
							dictionary[text] = streamToken2;
						}
					}
				}
				if (dictionary.Count > 0)
				{
					appearanceStream = new AppearanceStream(dictionary);
					return true;
				}
			}
			appearanceStream = null;
			return false;
		}
	}
}
