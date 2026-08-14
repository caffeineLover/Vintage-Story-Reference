using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Logging;
using UglyToad.PdfPig.Parser.Parts;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util;

namespace UglyToad.PdfPig.Content
{
	// Token: 0x020001F0 RID: 496
	internal class PagesFactory
	{
		// Token: 0x06000C01 RID: 3073 RVA: 0x0003B0A8 File Offset: 0x000392A8
		public static Pages Create(IndirectReference pagesReference, DictionaryToken pagesDictionary, IPdfTokenScanner scanner, IPageFactory pageFactory, ILog log, bool isLenientParsing)
		{
			PagesFactory.PageCounter pageNumber = new PagesFactory.PageCounter();
			PageTreeNode pageTreeNode = PagesFactory.ProcessPagesNode(pagesReference, pagesDictionary, new IndirectReference(1L, 0), true, scanner, isLenientParsing, pageNumber);
			if (!pageTreeNode.IsRoot)
			{
				throw new ArgumentException("Page tree must be the root page tree node.", "pageTree");
			}
			Dictionary<int, PageTreeNode> dictionary = new Dictionary<int, PageTreeNode>();
			PagesFactory.PopulatePageByNumberDictionary(pageTreeNode, dictionary);
			int intOrDefault = pagesDictionary.GetIntOrDefault(NameToken.Count, 0);
			if (intOrDefault != dictionary.Count)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(62, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Dictionary Page Count ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(intOrDefault);
				defaultInterpolatedStringHandler.AppendLiteral(" different to discovered pages ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(dictionary.Count);
				defaultInterpolatedStringHandler.AppendLiteral(". Using ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(dictionary.Count);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				log.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return new Pages(pageFactory, scanner, pageTreeNode, dictionary);
		}

		// Token: 0x06000C02 RID: 3074 RVA: 0x0003B180 File Offset: 0x00039380
		private static PageTreeNode ProcessPagesNode(IndirectReference referenceInput, DictionaryToken nodeDictionaryInput, IndirectReference parentReferenceInput, bool isRoot, IPdfTokenScanner pdfTokenScanner, bool isLenientParsing, PagesFactory.PageCounter pageNumber)
		{
			if (PagesFactory.CheckIfIsPage(nodeDictionaryInput, parentReferenceInput, isRoot, pdfTokenScanner, isLenientParsing))
			{
				pageNumber.Increment();
				return new PageTreeNode(nodeDictionaryInput, referenceInput, true, new int?(pageNumber.PageCount)).WithChildren(EmptyArray<PageTreeNode>.Instance);
			}
			Dictionary<long, HashSet<int>> dictionary = new Dictionary<long, HashSet<int>>();
			Queue<ValueTuple<long, int>> queue = new Queue<ValueTuple<long, int>>(1000);
			Queue<ValueTuple<PageTreeNode, IndirectReference, DictionaryToken, IndirectReference, List<PageTreeNode>>> queue2 = new Queue<ValueTuple<PageTreeNode, IndirectReference, DictionaryToken, IndirectReference, List<PageTreeNode>>>();
			PageTreeNode firstPage = new PageTreeNode(nodeDictionaryInput, referenceInput, false, default(int?));
			List<Action> list = new List<Action>();
			List<PageTreeNode> firstPageChildren = new List<PageTreeNode>();
			list.Add(delegate()
			{
				firstPage.WithChildren(firstPageChildren);
			});
			queue2.Enqueue(new ValueTuple<PageTreeNode, IndirectReference, DictionaryToken, IndirectReference, List<PageTreeNode>>(firstPage, referenceInput, nodeDictionaryInput, parentReferenceInput, firstPageChildren));
			ValueTuple<PageTreeNode, IndirectReference, DictionaryToken, IndirectReference, List<PageTreeNode>> valueTuple;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			for (;;)
			{
				valueTuple = queue2.Dequeue();
				long objectNumber = valueTuple.Item2.ObjectNumber;
				int generation = valueTuple.Item2.Generation;
				if (dictionary.ContainsKey(objectNumber))
				{
					HashSet<int> hashSet = dictionary[objectNumber];
					if (!hashSet.Contains(generation))
					{
						hashSet.Add(generation);
						dictionary[objectNumber] = hashSet;
						goto IL_1A5;
					}
					List<ValueTuple<long, int>> list2 = Enumerable.ToList<ValueTuple<long, int>>(queue);
					Math.Abs(list2.IndexOf(new ValueTuple<long, int>(objectNumber, generation)) - list2.Count);
				}
				else
				{
					Dictionary<long, HashSet<int>> dictionary2 = dictionary;
					long num = objectNumber;
					HashSet<int> hashSet2 = new HashSet<int>();
					hashSet2.Add(generation);
					dictionary2.Add(num, hashSet2);
					queue.Enqueue(new ValueTuple<long, int>(objectNumber, generation));
					if (queue.Count < 1000)
					{
						goto IL_1A5;
					}
					ValueTuple<long, int> valueTuple2 = queue.Dequeue();
					long item = valueTuple2.Item1;
					int item2 = valueTuple2.Item2;
					HashSet<int> hashSet3 = dictionary[item];
					hashSet3.Remove(item2);
					if (hashSet3.Count == 0)
					{
						dictionary.Remove(item);
						goto IL_1A5;
					}
					dictionary[item] = hashSet3;
					goto IL_1A5;
				}
				IL_39B:
				if (queue2.Count <= 0)
				{
					goto Block_9;
				}
				continue;
				IL_1A5:
				ArrayToken arrayToken;
				if (!valueTuple.Item3.TryGet(NameToken.Kids, pdfTokenScanner, out arrayToken))
				{
					if (!isLenientParsing)
					{
						break;
					}
					arrayToken = new ArrayToken(EmptyArray<IToken>.Instance);
				}
				foreach (IToken token in arrayToken.Data)
				{
					IndirectReferenceToken indirectReferenceToken = token as IndirectReferenceToken;
					if (indirectReferenceToken == null)
					{
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(66, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Kids array contained invalid entry (must be indirect reference): ");
						defaultInterpolatedStringHandler.AppendFormatted<IToken>(token);
						defaultInterpolatedStringHandler.AppendLiteral(".");
						throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					DictionaryToken dictionaryToken;
					if (!DirectObjectFinder.TryGet<DictionaryToken>(indirectReferenceToken, pdfTokenScanner, out dictionaryToken))
					{
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(74, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Could not find dictionary associated with reference in pages kids array: ");
						defaultInterpolatedStringHandler.AppendFormatted<IndirectReferenceToken>(indirectReferenceToken);
						defaultInterpolatedStringHandler.AppendLiteral(".");
						throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					if (PagesFactory.CheckIfIsPage(dictionaryToken, valueTuple.Item2, false, pdfTokenScanner, isLenientParsing))
					{
						PageTreeNode pageTreeNode = new PageTreeNode(dictionaryToken, indirectReferenceToken.Data, true, new int?(pageNumber.PageCount)).WithChildren(EmptyArray<PageTreeNode>.Instance);
						valueTuple.Item5.Add(pageTreeNode);
					}
					else
					{
						PageTreeNode kidChildNode = new PageTreeNode(dictionaryToken, indirectReferenceToken.Data, false, default(int?));
						List<PageTreeNode> kidChildren = new List<PageTreeNode>();
						queue2.Enqueue(new ValueTuple<PageTreeNode, IndirectReference, DictionaryToken, IndirectReference, List<PageTreeNode>>(kidChildNode, indirectReferenceToken.Data, dictionaryToken, valueTuple.Item2, kidChildren));
						list.Add(delegate()
						{
							kidChildNode.WithChildren(kidChildren);
						});
						valueTuple.Item5.Add(kidChildNode);
					}
				}
				goto IL_39B;
			}
			defaultInterpolatedStringHandler..ctor(68, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Pages node in the document pages tree did not define a kids array: ");
			defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(valueTuple.Item3);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			Block_9:
			foreach (Action action in list)
			{
				action.Invoke();
			}
			foreach (PageTreeNode pageTreeNode2 in Enumerable.Where<PageTreeNode>(firstPage.Children.ToRecursiveOrderList((PageTreeNode x) => x.Children), (PageTreeNode child) => child.IsPage))
			{
				pageNumber.Increment();
				pageTreeNode2.PageNumber = new int?(pageNumber.PageCount);
			}
			return firstPage;
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x0003B668 File Offset: 0x00039868
		private static bool CheckIfIsPage(DictionaryToken nodeDictionary, IndirectReference parentReference, bool isRoot, IPdfTokenScanner pdfTokenScanner, bool isLenientParsing)
		{
			bool flag = false;
			NameToken nameToken;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (!nodeDictionary.TryGet(NameToken.Type, pdfTokenScanner, out nameToken))
			{
				if (!isLenientParsing)
				{
					defaultInterpolatedStringHandler..ctor(56, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Node in the document pages tree did not define a type: ");
					defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(nodeDictionary);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				ArrayToken arrayToken;
				if (!nodeDictionary.TryGet(NameToken.Kids, pdfTokenScanner, out arrayToken))
				{
					flag = true;
				}
			}
			else
			{
				flag = nameToken.Equals(NameToken.Page);
				if (!flag && !nameToken.Equals(NameToken.Pages) && !isLenientParsing)
				{
					defaultInterpolatedStringHandler..ctor(55, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Node in the document pages tree defined invalid type: ");
					defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(nodeDictionary);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			if (!isLenientParsing && !isRoot)
			{
				IndirectReferenceToken indirectReferenceToken;
				if (!nodeDictionary.TryGet(NameToken.Parent, pdfTokenScanner, out indirectReferenceToken))
				{
					defaultInterpolatedStringHandler..ctor(68, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Could not find parent indirect reference token on pages tree node: ");
					defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(nodeDictionary);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				if (!indirectReferenceToken.Data.Equals(parentReference))
				{
					defaultInterpolatedStringHandler..ctor(63, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Pages tree node parent reference ");
					defaultInterpolatedStringHandler.AppendFormatted<IndirectReference>(indirectReferenceToken.Data);
					defaultInterpolatedStringHandler.AppendLiteral(" did not match actual parent ");
					defaultInterpolatedStringHandler.AppendFormatted<IndirectReference>(parentReference);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			return flag;
		}

		// Token: 0x06000C04 RID: 3076 RVA: 0x0003B7F4 File Offset: 0x000399F4
		private static void PopulatePageByNumberDictionary(PageTreeNode node, Dictionary<int, PageTreeNode> result)
		{
			if (!node.IsPage)
			{
				foreach (PageTreeNode node2 in node.Children)
				{
					PagesFactory.PopulatePageByNumberDictionary(node2, result);
				}
				return;
			}
			if (node.PageNumber == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(45, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Node was page but did not have page number: ");
				defaultInterpolatedStringHandler.AppendFormatted<PageTreeNode>(node);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			result[node.PageNumber.Value] = node;
		}

		// Token: 0x020002C3 RID: 707
		private class PageCounter
		{
			// Token: 0x17000422 RID: 1058
			// (get) Token: 0x06000F29 RID: 3881 RVA: 0x00044257 File Offset: 0x00042457
			// (set) Token: 0x06000F2A RID: 3882 RVA: 0x0004425F File Offset: 0x0004245F
			public int PageCount { get; private set; }

			// Token: 0x06000F2B RID: 3883 RVA: 0x00044268 File Offset: 0x00042468
			public void Increment()
			{
				int pageCount = this.PageCount;
				this.PageCount = pageCount + 1;
			}
		}
	}
}
