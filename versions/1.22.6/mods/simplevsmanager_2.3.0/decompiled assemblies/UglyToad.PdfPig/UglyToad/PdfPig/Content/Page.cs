using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UglyToad.PdfPig.Annotations;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Geometry;
using UglyToad.PdfPig.Graphics;
using UglyToad.PdfPig.Graphics.Operations;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.Content
{
	// Token: 0x020001EC RID: 492
	public class Page
	{
		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06000BD4 RID: 3028 RVA: 0x0003A8CF File Offset: 0x00038ACF
		public DictionaryToken Dictionary { get; }

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06000BD5 RID: 3029 RVA: 0x0003A8D7 File Offset: 0x00038AD7
		public int Number { get; }

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06000BD6 RID: 3030 RVA: 0x0003A8DF File Offset: 0x00038ADF
		public CropBox CropBox { get; }

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06000BD7 RID: 3031 RVA: 0x0003A8E7 File Offset: 0x00038AE7
		public MediaBox MediaBox { get; }

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06000BD8 RID: 3032 RVA: 0x0003A8EF File Offset: 0x00038AEF
		internal PageContent Content { get; }

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06000BD9 RID: 3033 RVA: 0x0003A8F7 File Offset: 0x00038AF7
		public PageRotationDegrees Rotation { get; }

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06000BDA RID: 3034 RVA: 0x0003A8FF File Offset: 0x00038AFF
		public IReadOnlyList<Letter> Letters
		{
			get
			{
				PageContent content = this.Content;
				return ((content != null) ? content.Letters : null) ?? new Letter[0];
			}
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06000BDB RID: 3035 RVA: 0x0003A91D File Offset: 0x00038B1D
		public string Text
		{
			get
			{
				return this.textLazy.Value;
			}
		}

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06000BDC RID: 3036 RVA: 0x0003A92A File Offset: 0x00038B2A
		public double Width { get; }

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06000BDD RID: 3037 RVA: 0x0003A932 File Offset: 0x00038B32
		public double Height { get; }

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06000BDE RID: 3038 RVA: 0x0003A93A File Offset: 0x00038B3A
		public PageSize Size { get; }

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06000BDF RID: 3039 RVA: 0x0003A942 File Offset: 0x00038B42
		public int NumberOfImages
		{
			get
			{
				return this.Content.NumberOfImages;
			}
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06000BE0 RID: 3040 RVA: 0x0003A94F File Offset: 0x00038B4F
		public IReadOnlyList<IGraphicsStateOperation> Operations
		{
			get
			{
				return this.Content.GraphicsStateOperations;
			}
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06000BE1 RID: 3041 RVA: 0x0003A95C File Offset: 0x00038B5C
		[NotNull]
		public Page.Experimental ExperimentalAccess { get; }

		// Token: 0x06000BE2 RID: 3042 RVA: 0x0003A964 File Offset: 0x00038B64
		internal Page(int number, DictionaryToken dictionary, MediaBox mediaBox, CropBox cropBox, PageRotationDegrees rotation, PageContent content, AnnotationProvider annotationProvider, IPdfTokenScanner pdfScanner)
		{
			if (number <= 0)
			{
				throw new ArgumentOutOfRangeException("number", "Page number cannot be 0 or negative.");
			}
			if (dictionary == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			this.Dictionary = dictionary;
			this.Number = number;
			this.MediaBox = mediaBox;
			this.CropBox = cropBox;
			this.Rotation = rotation;
			this.Content = content;
			this.textLazy = new Lazy<string>(() => Page.GetText(this.Content));
			PdfRectangle rectangle = mediaBox.Bounds.Intersect(cropBox.Bounds) ?? cropBox.Bounds;
			this.Width = (rotation.SwapsAxis ? rectangle.Height : rectangle.Width);
			this.Height = (rotation.SwapsAxis ? rectangle.Width : rectangle.Height);
			this.Size = rectangle.GetPageSize();
			this.ExperimentalAccess = new Page.Experimental(this, annotationProvider);
			this.annotationProvider = annotationProvider;
			if (pdfScanner == null)
			{
				throw new ArgumentNullException("pdfScanner");
			}
			this.pdfScanner = pdfScanner;
		}

		// Token: 0x06000BE3 RID: 3043 RVA: 0x0003AA84 File Offset: 0x00038C84
		private static string GetText(PageContent content)
		{
			if (((content != null) ? content.Letters : null) == null)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < content.Letters.Count; i++)
			{
				stringBuilder.Append(content.Letters[i].Value);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000BE4 RID: 3044 RVA: 0x0003AADF File Offset: 0x00038CDF
		public IEnumerable<Word> GetWords()
		{
			return this.GetWords(DefaultWordExtractor.Instance);
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x0003AAEC File Offset: 0x00038CEC
		public IEnumerable<Word> GetWords(IWordExtractor wordExtractor)
		{
			return (wordExtractor ?? DefaultWordExtractor.Instance).GetWords(this.Letters);
		}

		// Token: 0x06000BE6 RID: 3046 RVA: 0x0003AB03 File Offset: 0x00038D03
		public IReadOnlyList<Hyperlink> GetHyperlinks()
		{
			return HyperlinkFactory.GetHyperlinks(this, this.pdfScanner, this.annotationProvider);
		}

		// Token: 0x06000BE7 RID: 3047 RVA: 0x0003AB17 File Offset: 0x00038D17
		public IEnumerable<IPdfImage> GetImages()
		{
			return this.Content.GetImages();
		}

		// Token: 0x06000BE8 RID: 3048 RVA: 0x0003AB24 File Offset: 0x00038D24
		public IReadOnlyList<MarkedContentElement> GetMarkedContents()
		{
			return this.Content.GetMarkedContents();
		}

		// Token: 0x0400062E RID: 1582
		internal readonly AnnotationProvider annotationProvider;

		// Token: 0x0400062F RID: 1583
		internal readonly IPdfTokenScanner pdfScanner;

		// Token: 0x04000630 RID: 1584
		private readonly Lazy<string> textLazy;

		// Token: 0x020002C1 RID: 705
		public class Experimental
		{
			// Token: 0x1700041F RID: 1055
			// (get) Token: 0x06000F1B RID: 3867 RVA: 0x00043F06 File Offset: 0x00042106
			public IReadOnlyList<PdfPath> Paths
			{
				get
				{
					PageContent content = this.page.Content;
					return ((content != null) ? content.Paths : null) ?? new List<PdfPath>();
				}
			}

			// Token: 0x06000F1C RID: 3868 RVA: 0x00043F28 File Offset: 0x00042128
			internal Experimental(Page page, AnnotationProvider annotationProvider)
			{
				this.page = page;
				this.annotationProvider = annotationProvider;
			}

			// Token: 0x06000F1D RID: 3869 RVA: 0x00043F3E File Offset: 0x0004213E
			public IEnumerable<Annotation> GetAnnotations()
			{
				return this.annotationProvider.GetAnnotations();
			}

			// Token: 0x06000F1E RID: 3870 RVA: 0x00043F4C File Offset: 0x0004214C
			public IReadOnlyDictionary<string, IReadOnlyList<OptionalContentGroupElement>> GetOptionalContents()
			{
				List<OptionalContentGroupElement> list = new List<OptionalContentGroupElement>();
				PageContent content = this.page.Content;
				this.GetOptionalContentsRecursively((content != null) ? content.GetMarkedContents() : null, ref list);
				return Enumerable.ToDictionary<IGrouping<string, OptionalContentGroupElement>, string, IReadOnlyList<OptionalContentGroupElement>>(Enumerable.GroupBy<OptionalContentGroupElement, string>(list, (OptionalContentGroupElement oc) => oc.Name), (IGrouping<string, OptionalContentGroupElement> g) => g.Key, (IGrouping<string, OptionalContentGroupElement> g) => Enumerable.ToList<OptionalContentGroupElement>(g));
			}

			// Token: 0x06000F1F RID: 3871 RVA: 0x00043FE8 File Offset: 0x000421E8
			private void GetOptionalContentsRecursively(IReadOnlyList<MarkedContentElement> markedContentElements, ref List<OptionalContentGroupElement> mcesOptional)
			{
				if (markedContentElements.Count == 0)
				{
					return;
				}
				foreach (MarkedContentElement markedContentElement in markedContentElements)
				{
					if (markedContentElement.Tag == "OC")
					{
						mcesOptional.Add(new OptionalContentGroupElement(markedContentElement, this.page.pdfScanner));
					}
					else
					{
						IReadOnlyList<MarkedContentElement> children = markedContentElement.Children;
						if (children != null && children.Count > 0)
						{
							this.GetOptionalContentsRecursively(markedContentElement.Children, ref mcesOptional);
						}
					}
				}
			}

			// Token: 0x040008F8 RID: 2296
			private readonly Page page;

			// Token: 0x040008F9 RID: 2297
			private readonly AnnotationProvider annotationProvider;
		}
	}
}
