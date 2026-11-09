using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Parser.Parts
{
	// Token: 0x020000CB RID: 203
	internal static class NameTreeParser
	{
		// Token: 0x060003D8 RID: 984 RVA: 0x00017814 File Offset: 0x00015A14
		public static IReadOnlyDictionary<string, TResult> FlattenNameTreeToDictionary<TResult>(DictionaryToken nameTreeNodeDictionary, IPdfTokenScanner pdfScanner, Func<IToken, TResult> valuesFactory) where TResult : class
		{
			Dictionary<string, TResult> result = new Dictionary<string, TResult>();
			NameTreeParser.FlattenNameTree<TResult>(nameTreeNodeDictionary, pdfScanner, valuesFactory, result);
			return result;
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x00017834 File Offset: 0x00015A34
		public static void FlattenNameTree<TResult>(DictionaryToken nameTreeNodeDictionary, IPdfTokenScanner pdfScanner, Func<IToken, TResult> valuesFactory, Dictionary<string, TResult> result) where TResult : class
		{
			ArrayToken arrayToken;
			if (nameTreeNodeDictionary.TryGet(NameToken.Names, pdfScanner, out arrayToken))
			{
				for (int i = 0; i < arrayToken.Length; i += 2)
				{
					IDataToken<string> dataToken = arrayToken[i] as IDataToken<string>;
					if (dataToken != null)
					{
						IToken token = arrayToken[i + 1];
						TResult tresult = valuesFactory.Invoke(token);
						if (tresult != null)
						{
							result[dataToken.Data] = tresult;
						}
					}
				}
			}
			ArrayToken arrayToken2;
			if (nameTreeNodeDictionary.TryGet(NameToken.Kids, pdfScanner, out arrayToken2))
			{
				using (IEnumerator<IToken> enumerator = arrayToken2.Data.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						DictionaryToken nameTreeNodeDictionary2;
						if (DirectObjectFinder.TryGet<DictionaryToken>(enumerator.Current, pdfScanner, out nameTreeNodeDictionary2))
						{
							NameTreeParser.FlattenNameTree<TResult>(nameTreeNodeDictionary2, pdfScanner, valuesFactory, result);
						}
					}
				}
			}
		}
	}
}
