using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Actions;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Logging;
using UglyToad.PdfPig.Outline.Destinations;
using UglyToad.PdfPig.Parser.Parts;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util;

namespace UglyToad.PdfPig.Outline
{
	// Token: 0x020000D9 RID: 217
	internal class BookmarksProvider
	{
		// Token: 0x06000412 RID: 1042 RVA: 0x0001997F File Offset: 0x00017B7F
		public BookmarksProvider(ILog log, IPdfTokenScanner pdfScanner)
		{
			this.log = log;
			this.pdfScanner = pdfScanner;
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x00019998 File Offset: 0x00017B98
		public Bookmarks GetBookmarks(Catalog catalog)
		{
			DictionaryToken dictionary;
			if (!catalog.CatalogDictionary.TryGet(NameToken.Outlines, this.pdfScanner, out dictionary))
			{
				return null;
			}
			NameToken nameToken;
			if (dictionary.TryGet(NameToken.Type, this.pdfScanner, out nameToken) && nameToken != NameToken.Outlines)
			{
				ILog log = this.log;
				if (log != null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(70, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Outlines (bookmarks) dictionary did not have correct type specified: ");
					defaultInterpolatedStringHandler.AppendFormatted<NameToken>(nameToken);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			DictionaryToken dictionaryToken;
			if (!dictionary.TryGet(NameToken.First, this.pdfScanner, out dictionaryToken))
			{
				return null;
			}
			List<BookmarkNode> list = new List<BookmarkNode>();
			HashSet<IndirectReference> hashSet = new HashSet<IndirectReference>();
			while (dictionaryToken != null)
			{
				this.ReadBookmarksRecursively(dictionaryToken, 0, false, hashSet, catalog.NamedDestinations, list);
				IndirectReferenceToken indirectReferenceToken;
				if (!dictionaryToken.TryGet<IndirectReferenceToken>(NameToken.Next, ref indirectReferenceToken) || !hashSet.Add(indirectReferenceToken.Data))
				{
					break;
				}
				dictionaryToken = DirectObjectFinder.Get<DictionaryToken>(indirectReferenceToken, this.pdfScanner);
			}
			return new Bookmarks(list);
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x00019A94 File Offset: 0x00017C94
		private void ReadBookmarksRecursively(DictionaryToken nodeDictionary, int level, bool readSiblings, HashSet<IndirectReference> seen, NamedDestinations namedDestinations, List<BookmarkNode> list)
		{
			string title;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (!nodeDictionary.TryGetOptionalStringDirect(NameToken.Title, this.pdfScanner, out title))
			{
				defaultInterpolatedStringHandler..ctor(44, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid title for outline (bookmark) node: ");
				defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(nodeDictionary);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			List<BookmarkNode> list2 = new List<BookmarkNode>();
			DictionaryToken nodeDictionary2;
			if (nodeDictionary.TryGet(NameToken.First, this.pdfScanner, out nodeDictionary2))
			{
				this.ReadBookmarksRecursively(nodeDictionary2, level + 1, true, seen, namedDestinations, list2);
			}
			ExplicitDestination destination;
			BookmarkNode bookmarkNode;
			if (DestinationProvider.TryGetDestination(nodeDictionary, NameToken.Dest, namedDestinations, this.pdfScanner, this.log, false, out destination))
			{
				bookmarkNode = new DocumentBookmarkNode(title, level, destination, list2);
			}
			else
			{
				PdfAction pdfAction;
				if (!ActionProvider.TryGetAction(nodeDictionary, namedDestinations, this.pdfScanner, this.log, out pdfAction))
				{
					ILog log = this.log;
					defaultInterpolatedStringHandler..ctor(63, 1);
					defaultInterpolatedStringHandler.AppendLiteral("No /Dest(ination) or /A(ction) entry found for bookmark node: ");
					defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(nodeDictionary);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
					return;
				}
				GoToRAction goToRAction = pdfAction as GoToRAction;
				if (goToRAction != null)
				{
					bookmarkNode = new ExternalBookmarkNode(title, level, goToRAction.Destination, list2, goToRAction.Filename);
				}
				else
				{
					GoToAction goToAction = pdfAction as GoToAction;
					if (goToAction != null)
					{
						bookmarkNode = new DocumentBookmarkNode(title, level, goToAction.Destination, list2);
					}
					else
					{
						UriAction uriAction = pdfAction as UriAction;
						if (uriAction == null)
						{
							return;
						}
						bookmarkNode = new UriBookmarkNode(title, level, uriAction.Uri, list2);
					}
				}
			}
			list.Add(bookmarkNode);
			if (!readSiblings)
			{
				return;
			}
			DictionaryToken dictionaryToken = nodeDictionary;
			IndirectReferenceToken indirectReferenceToken;
			while (dictionaryToken.TryGet<IndirectReferenceToken>(NameToken.Next, ref indirectReferenceToken) && seen.Add(indirectReferenceToken.Data))
			{
				dictionaryToken = DirectObjectFinder.Get<DictionaryToken>(indirectReferenceToken, this.pdfScanner);
				if (dictionaryToken == null)
				{
					break;
				}
				this.ReadBookmarksRecursively(dictionaryToken, level, false, seen, namedDestinations, list);
			}
		}

		// Token: 0x04000212 RID: 530
		private readonly ILog log;

		// Token: 0x04000213 RID: 531
		private readonly IPdfTokenScanner pdfScanner;
	}
}
