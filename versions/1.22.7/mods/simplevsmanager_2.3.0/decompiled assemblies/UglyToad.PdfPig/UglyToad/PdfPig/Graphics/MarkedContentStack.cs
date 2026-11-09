using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Filters;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.XObjects;

namespace UglyToad.PdfPig.Graphics
{
	// Token: 0x0200010B RID: 267
	internal class MarkedContentStack
	{
		// Token: 0x17000162 RID: 354
		// (get) Token: 0x060005BB RID: 1467 RVA: 0x0001FE60 File Offset: 0x0001E060
		public bool CanPop
		{
			get
			{
				return this.top != null;
			}
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x0001FE6C File Offset: 0x0001E06C
		public void Push(NameToken name, DictionaryToken properties)
		{
			if (this.builderStack.Count == 0)
			{
				this.number++;
			}
			this.top = new MarkedContentStack.MarkedContentElementActiveBuilder(this.number, name, properties);
			this.builderStack.Push(this.top);
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x0001FEB8 File Offset: 0x0001E0B8
		public MarkedContentElement Pop(IPdfTokenScanner pdfScanner)
		{
			MarkedContentElement markedContentElement = this.builderStack.Pop().Build(pdfScanner);
			if (this.builderStack.Count > 0)
			{
				this.top = this.builderStack.Peek();
				this.top.Children.Add(markedContentElement);
				return null;
			}
			this.top = null;
			return markedContentElement;
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x0001FF11 File Offset: 0x0001E111
		public void AddLetter(Letter letter)
		{
			MarkedContentStack.MarkedContentElementActiveBuilder markedContentElementActiveBuilder = this.top;
			if (markedContentElementActiveBuilder == null)
			{
				return;
			}
			markedContentElementActiveBuilder.AddLetter(letter);
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x0001FF24 File Offset: 0x0001E124
		public void AddPath(PdfPath path)
		{
			MarkedContentStack.MarkedContentElementActiveBuilder markedContentElementActiveBuilder = this.top;
			if (markedContentElementActiveBuilder == null)
			{
				return;
			}
			markedContentElementActiveBuilder.AddPath(path);
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x0001FF37 File Offset: 0x0001E137
		public void AddImage(IPdfImage image)
		{
			MarkedContentStack.MarkedContentElementActiveBuilder markedContentElementActiveBuilder = this.top;
			if (markedContentElementActiveBuilder == null)
			{
				return;
			}
			markedContentElementActiveBuilder.AddImage(image);
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x0001FF4C File Offset: 0x0001E14C
		public void AddXObject(XObjectContentRecord xObject, IPdfTokenScanner scanner, ILookupFilterProvider filterProvider, IResourceStore resourceStore)
		{
			if (this.top != null && xObject.Type == XObjectType.Image)
			{
				XObjectImage image = XObjectFactory.ReadImage(xObject, scanner, filterProvider, resourceStore);
				MarkedContentStack.MarkedContentElementActiveBuilder markedContentElementActiveBuilder = this.top;
				if (markedContentElementActiveBuilder == null)
				{
					return;
				}
				markedContentElementActiveBuilder.AddImage(image);
			}
		}

		// Token: 0x040002E0 RID: 736
		private readonly Stack<MarkedContentStack.MarkedContentElementActiveBuilder> builderStack = new Stack<MarkedContentStack.MarkedContentElementActiveBuilder>();

		// Token: 0x040002E1 RID: 737
		private int number = -1;

		// Token: 0x040002E2 RID: 738
		private MarkedContentStack.MarkedContentElementActiveBuilder top;

		// Token: 0x02000265 RID: 613
		private class MarkedContentElementActiveBuilder
		{
			// Token: 0x1700040E RID: 1038
			// (get) Token: 0x06000DF3 RID: 3571 RVA: 0x000417E2 File Offset: 0x0003F9E2
			public List<MarkedContentElement> Children { get; } = new List<MarkedContentElement>();

			// Token: 0x06000DF4 RID: 3572 RVA: 0x000417EC File Offset: 0x0003F9EC
			public MarkedContentElementActiveBuilder(int number, NameToken name, DictionaryToken properties)
			{
				this.number = number;
				this.name = name;
				this.properties = (properties ?? new DictionaryToken(new Dictionary<NameToken, IToken>()));
			}

			// Token: 0x06000DF5 RID: 3573 RVA: 0x0004184E File Offset: 0x0003FA4E
			public void AddLetter(Letter letter)
			{
				this.letters.Add(letter);
			}

			// Token: 0x06000DF6 RID: 3574 RVA: 0x0004185C File Offset: 0x0003FA5C
			public void AddImage(IPdfImage image)
			{
				this.images.Add(image);
			}

			// Token: 0x06000DF7 RID: 3575 RVA: 0x0004186A File Offset: 0x0003FA6A
			public void AddPath(PdfPath path)
			{
				this.paths.Add(path);
			}

			// Token: 0x06000DF8 RID: 3576 RVA: 0x00041878 File Offset: 0x0003FA78
			public MarkedContentElement Build(IPdfTokenScanner pdfScanner)
			{
				int markedContentIdentifier = -1;
				NumericToken numericToken;
				if (this.properties.TryGet(NameToken.Mcid, pdfScanner, out numericToken))
				{
					markedContentIdentifier = numericToken.Int;
				}
				string optional = this.GetOptional(NameToken.Lang, pdfScanner);
				string optional2 = this.GetOptional(NameToken.ActualText, pdfScanner);
				string optional3 = this.GetOptional(NameToken.Alternate, pdfScanner);
				string optional4 = this.GetOptional(NameToken.E, pdfScanner);
				if (this.name != NameToken.Artifact)
				{
					return new MarkedContentElement(markedContentIdentifier, this.name, this.properties, optional, optional2, optional3, optional4, false, this.Children, this.letters, this.paths, this.images, this.number);
				}
				ArtifactMarkedContentElement.ArtifactType artifactType = ArtifactMarkedContentElement.ArtifactType.Unknown;
				IDataToken<string> dataToken;
				ArtifactMarkedContentElement.ArtifactType artifactType2;
				if (this.properties.TryGet(NameToken.Type, pdfScanner, out dataToken) && Enum.TryParse<ArtifactMarkedContentElement.ArtifactType>(dataToken.Data, true, ref artifactType2))
				{
					artifactType = artifactType2;
				}
				string optional5 = this.GetOptional(NameToken.Subtype, pdfScanner);
				string optional6 = this.GetOptional(NameToken.O, pdfScanner);
				PdfRectangle? boundingBox = default(PdfRectangle?);
				ArrayToken arrayToken;
				if (this.properties.TryGet(NameToken.Bbox, pdfScanner, out arrayToken))
				{
					NumericToken numericToken2 = null;
					NumericToken numericToken3 = null;
					NumericToken numericToken4 = null;
					NumericToken numericToken5 = null;
					if (arrayToken.Length == 4)
					{
						numericToken2 = (arrayToken[0] as NumericToken);
						numericToken3 = (arrayToken[1] as NumericToken);
						numericToken4 = (arrayToken[2] as NumericToken);
						numericToken5 = (arrayToken[3] as NumericToken);
					}
					else if (arrayToken.Length == 6)
					{
						numericToken2 = (arrayToken[2] as NumericToken);
						numericToken3 = (arrayToken[3] as NumericToken);
						numericToken4 = (arrayToken[4] as NumericToken);
						numericToken5 = (arrayToken[5] as NumericToken);
					}
					if (numericToken2 != null && numericToken3 != null && numericToken4 != null && numericToken5 != null)
					{
						boundingBox = new PdfRectangle?(new PdfRectangle(numericToken2.Double, numericToken3.Double, numericToken4.Double, numericToken5.Double));
					}
				}
				List<NameToken> list = new List<NameToken>();
				ArrayToken arrayToken2;
				if (this.properties.TryGet<ArrayToken>(NameToken.Attached, ref arrayToken2))
				{
					foreach (IToken token in arrayToken2.Data)
					{
						NameToken nameToken = token as NameToken;
						if (nameToken != null)
						{
							list.Add(nameToken);
						}
					}
				}
				return new ArtifactMarkedContentElement(markedContentIdentifier, this.name, this.properties, optional, optional2, optional3, optional4, artifactType, optional5, optional6, boundingBox, list, this.Children, this.letters, this.paths, this.images, this.number);
			}

			// Token: 0x06000DF9 RID: 3577 RVA: 0x00041B0C File Offset: 0x0003FD0C
			private string GetOptional(NameToken optionName, IPdfTokenScanner pdfScanner)
			{
				string result = null;
				IDataToken<string> dataToken;
				if (this.properties.TryGet(optionName, pdfScanner, out dataToken))
				{
					result = dataToken.Data;
				}
				return result;
			}

			// Token: 0x0400083A RID: 2106
			private readonly int number;

			// Token: 0x0400083B RID: 2107
			private readonly NameToken name;

			// Token: 0x0400083C RID: 2108
			private readonly DictionaryToken properties;

			// Token: 0x0400083D RID: 2109
			private readonly List<Letter> letters = new List<Letter>();

			// Token: 0x0400083E RID: 2110
			private readonly List<PdfPath> paths = new List<PdfPath>();

			// Token: 0x0400083F RID: 2111
			private readonly List<IPdfImage> images = new List<IPdfImage>();
		}
	}
}
