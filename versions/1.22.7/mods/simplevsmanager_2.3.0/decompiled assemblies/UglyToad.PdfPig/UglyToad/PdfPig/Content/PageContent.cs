using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Filters;
using UglyToad.PdfPig.Graphics;
using UglyToad.PdfPig.Graphics.Operations;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.XObjects;

namespace UglyToad.PdfPig.Content
{
	// Token: 0x020001ED RID: 493
	internal class PageContent
	{
		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06000BEA RID: 3050 RVA: 0x0003AB3E File Offset: 0x00038D3E
		internal IReadOnlyList<IGraphicsStateOperation> GraphicsStateOperations { get; }

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06000BEB RID: 3051 RVA: 0x0003AB46 File Offset: 0x00038D46
		public IReadOnlyList<Letter> Letters { get; }

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06000BEC RID: 3052 RVA: 0x0003AB4E File Offset: 0x00038D4E
		public IReadOnlyList<PdfPath> Paths { get; }

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06000BED RID: 3053 RVA: 0x0003AB56 File Offset: 0x00038D56
		public int NumberOfImages
		{
			get
			{
				return this.images.Count;
			}
		}

		// Token: 0x06000BEE RID: 3054 RVA: 0x0003AB64 File Offset: 0x00038D64
		internal PageContent(IReadOnlyList<IGraphicsStateOperation> graphicsStateOperations, IReadOnlyList<Letter> letters, IReadOnlyList<PdfPath> paths, IReadOnlyList<Union<XObjectContentRecord, InlineImage>> images, IReadOnlyList<MarkedContentElement> markedContents, IPdfTokenScanner pdfScanner, ILookupFilterProvider filterProvider, IResourceStore resourceStore)
		{
			this.GraphicsStateOperations = graphicsStateOperations;
			this.Letters = letters;
			this.Paths = paths;
			this.images = images;
			this.markedContents = markedContents;
			if (pdfScanner == null)
			{
				throw new ArgumentNullException("pdfScanner");
			}
			this.pdfScanner = pdfScanner;
			if (filterProvider == null)
			{
				throw new ArgumentNullException("filterProvider");
			}
			this.filterProvider = filterProvider;
			if (resourceStore == null)
			{
				throw new ArgumentNullException("resourceStore");
			}
			this.resourceStore = resourceStore;
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x0003ABE1 File Offset: 0x00038DE1
		public IEnumerable<IPdfImage> GetImages()
		{
			foreach (Union<XObjectContentRecord, InlineImage> union in this.images)
			{
				XObjectContentRecord xObject;
				InlineImage inlineImage;
				if (union.TryGetFirst(ref xObject))
				{
					yield return XObjectFactory.ReadImage(xObject, this.pdfScanner, this.filterProvider, this.resourceStore);
				}
				else if (union.TryGetSecond(ref inlineImage))
				{
					yield return inlineImage;
				}
			}
			IEnumerator<Union<XObjectContentRecord, InlineImage>> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x0003ABF1 File Offset: 0x00038DF1
		public IReadOnlyList<MarkedContentElement> GetMarkedContents()
		{
			return this.markedContents;
		}

		// Token: 0x0400063B RID: 1595
		private readonly IReadOnlyList<Union<XObjectContentRecord, InlineImage>> images;

		// Token: 0x0400063C RID: 1596
		private readonly IReadOnlyList<MarkedContentElement> markedContents;

		// Token: 0x0400063D RID: 1597
		private readonly IPdfTokenScanner pdfScanner;

		// Token: 0x0400063E RID: 1598
		private readonly ILookupFilterProvider filterProvider;

		// Token: 0x0400063F RID: 1599
		private readonly IResourceStore resourceStore;
	}
}
