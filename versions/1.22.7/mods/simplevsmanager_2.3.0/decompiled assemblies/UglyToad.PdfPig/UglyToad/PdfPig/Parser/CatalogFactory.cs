using System;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Logging;
using UglyToad.PdfPig.Outline;
using UglyToad.PdfPig.Parser.Parts;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Parser
{
	// Token: 0x020000C2 RID: 194
	internal static class CatalogFactory
	{
		// Token: 0x060003BC RID: 956 RVA: 0x00015FCC File Offset: 0x000141CC
		public static Catalog Create(IndirectReference rootReference, DictionaryToken dictionary, IPdfTokenScanner scanner, PageFactory pageFactory, ILog log, bool isLenientParsing)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			IToken token;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (dictionary.TryGet(NameToken.Type, ref token) && token != NameToken.Catalog)
			{
				defaultInterpolatedStringHandler..ctor(53, 1);
				defaultInterpolatedStringHandler.AppendLiteral("The type of the catalog dictionary was not Catalog: ");
				defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(dictionary);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			IToken token2;
			if (!dictionary.TryGet(NameToken.Pages, ref token2))
			{
				defaultInterpolatedStringHandler..ctor(53, 1);
				defaultInterpolatedStringHandler.AppendLiteral("No pages entry was found in the catalog dictionary: ");
				defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(dictionary);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			IndirectReference pagesReference = rootReference;
			IndirectReferenceToken indirectReferenceToken = token2 as IndirectReferenceToken;
			DictionaryToken pagesDictionary;
			if (indirectReferenceToken != null)
			{
				pagesReference = indirectReferenceToken.Data;
				pagesDictionary = DirectObjectFinder.Get<DictionaryToken>(indirectReferenceToken, scanner);
			}
			else
			{
				DictionaryToken dictionaryToken = token2 as DictionaryToken;
				if (dictionaryToken != null)
				{
					pagesDictionary = dictionaryToken;
				}
				else
				{
					pagesDictionary = DirectObjectFinder.Get<DictionaryToken>(token2, scanner);
				}
			}
			Pages pages = PagesFactory.Create(pagesReference, pagesDictionary, scanner, pageFactory, log, isLenientParsing);
			NamedDestinations namedDestinations = NamedDestinationsProvider.Read(dictionary, scanner, pages, null);
			return new Catalog(dictionary, pages, namedDestinations);
		}
	}
}
