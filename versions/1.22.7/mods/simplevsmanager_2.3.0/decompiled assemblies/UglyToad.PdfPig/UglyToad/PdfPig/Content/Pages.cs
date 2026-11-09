using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Logging;
using UglyToad.PdfPig.Outline;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util;

namespace UglyToad.PdfPig.Content
{
	// Token: 0x020001EF RID: 495
	internal class Pages
	{
		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06000BFB RID: 3067 RVA: 0x0003ADE0 File Offset: 0x00038FE0
		public int Count
		{
			get
			{
				return this.pagesByNumber.Count;
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06000BFC RID: 3068 RVA: 0x0003ADED File Offset: 0x00038FED
		public PageTreeNode PageTree { get; }

		// Token: 0x06000BFD RID: 3069 RVA: 0x0003ADF8 File Offset: 0x00038FF8
		internal Pages(IPageFactory pageFactory, IPdfTokenScanner pdfScanner, PageTreeNode pageTree, Dictionary<int, PageTreeNode> pagesByNumber)
		{
			if (pageFactory == null)
			{
				throw new ArgumentNullException("pageFactory");
			}
			this.pageFactory = pageFactory;
			if (pdfScanner == null)
			{
				throw new ArgumentNullException("pdfScanner");
			}
			this.pdfScanner = pdfScanner;
			this.pagesByNumber = pagesByNumber;
			this.PageTree = pageTree;
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x0003AE48 File Offset: 0x00039048
		internal Page GetPage(int pageNumber, NamedDestinations namedDestinations, InternalParsingOptions parsingOptions)
		{
			if (pageNumber <= 0 || pageNumber > this.Count)
			{
				ILog logger = parsingOptions.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(36, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Page ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(pageNumber);
				defaultInterpolatedStringHandler.AppendLiteral(" requested but is out of range.");
				logger.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				string text = "pageNumber";
				defaultInterpolatedStringHandler..ctor(45, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Page number ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(pageNumber);
				defaultInterpolatedStringHandler.AppendLiteral(" invalid, must be between 1 and ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.Count);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new ArgumentOutOfRangeException(text, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			PageTreeNode pageNode = this.GetPageNode(pageNumber);
			Stack<PageTreeNode> stack = new Stack<PageTreeNode>();
			for (PageTreeNode pageTreeNode = pageNode; pageTreeNode != null; pageTreeNode = pageTreeNode.Parent)
			{
				stack.Push(pageTreeNode);
			}
			PageTreeMembers pageTreeMembers = new PageTreeMembers();
			while (stack.Count > 0)
			{
				PageTreeNode pageTreeNode = stack.Pop();
				DictionaryToken dictionaryToken;
				if (pageTreeNode.NodeDictionary.TryGet(NameToken.Resources, this.pdfScanner, out dictionaryToken))
				{
					pageTreeMembers.ParentResources.Enqueue(dictionaryToken);
				}
				ArrayToken array;
				if (pageTreeNode.NodeDictionary.TryGet(NameToken.MediaBox, this.pdfScanner, out array))
				{
					pageTreeMembers.MediaBox = new MediaBox(new PdfRectangle?(array.ToRectangle(this.pdfScanner)));
				}
				NumericToken numericToken;
				if (pageTreeNode.NodeDictionary.TryGet(NameToken.Rotate, this.pdfScanner, out numericToken))
				{
					pageTreeMembers.Rotation = numericToken.Int;
				}
			}
			return this.pageFactory.Create(pageNumber, pageNode.NodeDictionary, pageTreeMembers, namedDestinations, parsingOptions);
		}

		// Token: 0x06000BFF RID: 3071 RVA: 0x0003AFD4 File Offset: 0x000391D4
		internal PageTreeNode GetPageNode(int pageNumber)
		{
			PageTreeNode result;
			if (!this.pagesByNumber.TryGetValue(pageNumber, ref result))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(41, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Could not find page node by number for: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(pageNumber);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return result;
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x0003B02C File Offset: 0x0003922C
		internal PageTreeNode GetPageByReference(IndirectReference reference)
		{
			foreach (KeyValuePair<int, PageTreeNode> keyValuePair in this.pagesByNumber)
			{
				if (keyValuePair.Value.Reference.Equals(reference))
				{
					return keyValuePair.Value;
				}
			}
			return null;
		}

		// Token: 0x04000644 RID: 1604
		private readonly IPageFactory pageFactory;

		// Token: 0x04000645 RID: 1605
		private readonly IPdfTokenScanner pdfScanner;

		// Token: 0x04000646 RID: 1606
		private readonly Dictionary<int, PageTreeNode> pagesByNumber;
	}
}
