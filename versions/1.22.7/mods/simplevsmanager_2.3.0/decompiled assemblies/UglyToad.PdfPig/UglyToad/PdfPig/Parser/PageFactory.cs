using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Annotations;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Filters;
using UglyToad.PdfPig.Geometry;
using UglyToad.PdfPig.Graphics;
using UglyToad.PdfPig.Graphics.Operations;
using UglyToad.PdfPig.Logging;
using UglyToad.PdfPig.Outline;
using UglyToad.PdfPig.Parser.Parts;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util;

namespace UglyToad.PdfPig.Parser
{
	// Token: 0x020000C6 RID: 198
	internal class PageFactory : IPageFactory
	{
		// Token: 0x060003C3 RID: 963 RVA: 0x0001678F File Offset: 0x0001498F
		public PageFactory(IPdfTokenScanner pdfScanner, IResourceStore resourceStore, ILookupFilterProvider filterProvider, IPageContentParser pageContentParser, ILog log)
		{
			this.resourceStore = resourceStore;
			this.filterProvider = filterProvider;
			this.pageContentParser = pageContentParser;
			this.pdfScanner = pdfScanner;
			this.log = log;
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x000167BC File Offset: 0x000149BC
		public Page Create(int number, DictionaryToken dictionary, PageTreeMembers pageTreeMembers, NamedDestinations namedDestinations, InternalParsingOptions parsingOptions)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			NameToken nameOrDefault = dictionary.GetNameOrDefault(NameToken.Type);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (nameOrDefault != null && !nameOrDefault.Equals(NameToken.Page))
			{
				ILog logger = parsingOptions.Logger;
				defaultInterpolatedStringHandler..ctor(52, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Page ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(number);
				defaultInterpolatedStringHandler.AppendLiteral(" had its type specified as ");
				defaultInterpolatedStringHandler.AppendFormatted<NameToken>(nameOrDefault);
				defaultInterpolatedStringHandler.AppendLiteral(" rather than 'Page'.");
				logger.Error(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			MediaBox mediaBox = this.GetMediaBox(number, dictionary, pageTreeMembers);
			CropBox cropBox = this.GetCropBox(dictionary, pageTreeMembers, mediaBox);
			PageRotationDegrees rotation = new PageRotationDegrees(pageTreeMembers.Rotation);
			NumericToken numericToken;
			if (dictionary.TryGet(NameToken.Rotate, this.pdfScanner, out numericToken))
			{
				rotation = new PageRotationDegrees(numericToken.Int);
			}
			int num = 0;
			while (pageTreeMembers.ParentResources.Count > 0)
			{
				DictionaryToken resourceDictionary = pageTreeMembers.ParentResources.Dequeue();
				this.resourceStore.LoadResourceDictionary(resourceDictionary, parsingOptions);
				num++;
			}
			DictionaryToken resourceDictionary2;
			if (dictionary.TryGet(NameToken.Resources, this.pdfScanner, out resourceDictionary2))
			{
				this.resourceStore.LoadResourceDictionary(resourceDictionary2, parsingOptions);
				num++;
			}
			UserSpaceUnit userSpaceUnits = PageFactory.GetUserSpaceUnits(dictionary);
			IToken token;
			PageContent content;
			ArrayToken arrayToken;
			if (!dictionary.TryGet(NameToken.Contents, ref token))
			{
				content = new PageContent(EmptyArray<IGraphicsStateOperation>.Instance, EmptyArray<Letter>.Instance, EmptyArray<PdfPath>.Instance, EmptyArray<Union<XObjectContentRecord, InlineImage>>.Instance, EmptyArray<MarkedContentElement>.Instance, this.pdfScanner, this.filterProvider, this.resourceStore);
			}
			else if (DirectObjectFinder.TryGet<ArrayToken>(token, this.pdfScanner, out arrayToken))
			{
				List<byte> list = new List<byte>();
				for (int i = 0; i < arrayToken.Data.Count; i++)
				{
					IToken token2 = arrayToken.Data[i];
					IndirectReferenceToken indirectReferenceToken = token2 as IndirectReferenceToken;
					if (indirectReferenceToken == null)
					{
						defaultInterpolatedStringHandler..ctor(71, 1);
						defaultInterpolatedStringHandler.AppendLiteral("The contents contained something which was not an indirect reference: ");
						defaultInterpolatedStringHandler.AppendFormatted<IToken>(token2);
						defaultInterpolatedStringHandler.AppendLiteral(".");
						throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					StreamToken streamToken = DirectObjectFinder.Get<StreamToken>(indirectReferenceToken, this.pdfScanner);
					if (streamToken == null)
					{
						defaultInterpolatedStringHandler..ctor(40, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Could not find the contents for object ");
						defaultInterpolatedStringHandler.AppendFormatted<IndirectReferenceToken>(indirectReferenceToken);
						defaultInterpolatedStringHandler.AppendLiteral(".");
						throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					list.AddRange(streamToken.Decode(this.filterProvider, this.pdfScanner));
					if (i < arrayToken.Data.Count - 1)
					{
						list.Add(10);
					}
				}
				content = this.GetContent(number, list, cropBox, userSpaceUnits, rotation, mediaBox, parsingOptions);
			}
			else
			{
				StreamToken streamToken2 = DirectObjectFinder.Get<StreamToken>(token, this.pdfScanner);
				if (streamToken2 == null)
				{
					throw new InvalidOperationException("Failed to parse the content for the page: " + number.ToString());
				}
				IReadOnlyList<byte> contentBytes = streamToken2.Decode(this.filterProvider, this.pdfScanner);
				content = this.GetContent(number, contentBytes, cropBox, userSpaceUnits, rotation, mediaBox, parsingOptions);
			}
			TransformationMatrix initialMatrix = ContentStreamProcessor.GetInitialMatrix(userSpaceUnits, mediaBox, cropBox, rotation, this.log);
			AnnotationProvider annotationProvider = new AnnotationProvider(this.pdfScanner, dictionary, initialMatrix, namedDestinations, this.log);
			Page result = new Page(number, dictionary, mediaBox, cropBox, rotation, content, annotationProvider, this.pdfScanner);
			for (int j = 0; j < num; j++)
			{
				this.resourceStore.UnloadResourceDictionary();
			}
			return result;
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x00016B0C File Offset: 0x00014D0C
		private PageContent GetContent(int pageNumber, IReadOnlyList<byte> contentBytes, CropBox cropBox, UserSpaceUnit userSpaceUnit, PageRotationDegrees rotation, MediaBox mediaBox, InternalParsingOptions parsingOptions)
		{
			IReadOnlyList<IGraphicsStateOperation> operations = this.pageContentParser.Parse(pageNumber, new ByteArrayInputBytes(contentBytes), parsingOptions.Logger);
			return new ContentStreamProcessor(pageNumber, this.resourceStore, userSpaceUnit, mediaBox, cropBox, rotation, this.pdfScanner, this.pageContentParser, this.filterProvider, parsingOptions).Process(pageNumber, operations);
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x00016B64 File Offset: 0x00014D64
		private static UserSpaceUnit GetUserSpaceUnits(DictionaryToken dictionary)
		{
			UserSpaceUnit @default = UserSpaceUnit.Default;
			IToken token;
			if (dictionary.TryGet(NameToken.UserUnit, ref token))
			{
				NumericToken numericToken = token as NumericToken;
				if (numericToken != null)
				{
					@default = new UserSpaceUnit(numericToken.Int);
				}
			}
			return @default;
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x00016BA0 File Offset: 0x00014DA0
		private CropBox GetCropBox(DictionaryToken dictionary, PageTreeMembers pageTreeMembers, MediaBox mediaBox)
		{
			IToken token;
			ArrayToken arrayToken;
			CropBox result;
			if (dictionary.TryGet(NameToken.CropBox, ref token) && DirectObjectFinder.TryGet<ArrayToken>(token, this.pdfScanner, out arrayToken))
			{
				if (arrayToken.Length != 4)
				{
					ILog log = this.log;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(82, 2);
					defaultInterpolatedStringHandler.AppendLiteral("The CropBox was the wrong length in the dictionary: ");
					defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(dictionary);
					defaultInterpolatedStringHandler.AppendLiteral(". Array was: ");
					defaultInterpolatedStringHandler.AppendFormatted<ArrayToken>(arrayToken);
					defaultInterpolatedStringHandler.AppendLiteral(". Using MediaBox.");
					log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
					return new CropBox(mediaBox.Bounds);
				}
				result = new CropBox(arrayToken.ToRectangle(this.pdfScanner));
			}
			else
			{
				result = (pageTreeMembers.GetCropBox() ?? new CropBox(mediaBox.Bounds));
			}
			return result;
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x00016C64 File Offset: 0x00014E64
		private MediaBox GetMediaBox(int number, DictionaryToken dictionary, PageTreeMembers pageTreeMembers)
		{
			IToken token;
			ArrayToken arrayToken;
			MediaBox mediaBox;
			if (dictionary.TryGet(NameToken.MediaBox, ref token) && DirectObjectFinder.TryGet<ArrayToken>(token, this.pdfScanner, out arrayToken))
			{
				if (arrayToken.Length != 4)
				{
					ILog log = this.log;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(92, 2);
					defaultInterpolatedStringHandler.AppendLiteral("The MediaBox was the wrong length in the dictionary: ");
					defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(dictionary);
					defaultInterpolatedStringHandler.AppendLiteral(". Array was: ");
					defaultInterpolatedStringHandler.AppendFormatted<ArrayToken>(arrayToken);
					defaultInterpolatedStringHandler.AppendLiteral(". Defaulting to US Letter.");
					log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
					return MediaBox.Letter;
				}
				mediaBox = new MediaBox(new PdfRectangle?(arrayToken.ToRectangle(this.pdfScanner)));
			}
			else
			{
				mediaBox = pageTreeMembers.MediaBox;
				if (mediaBox == null)
				{
					ILog log2 = this.log;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("The MediaBox was the wrong missing for page ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(number);
					defaultInterpolatedStringHandler.AppendLiteral(". Using US Letter.");
					log2.Error(defaultInterpolatedStringHandler.ToStringAndClear());
					mediaBox = MediaBox.Letter;
				}
			}
			return mediaBox;
		}

		// Token: 0x040001EF RID: 495
		private readonly IPdfTokenScanner pdfScanner;

		// Token: 0x040001F0 RID: 496
		private readonly IResourceStore resourceStore;

		// Token: 0x040001F1 RID: 497
		private readonly ILookupFilterProvider filterProvider;

		// Token: 0x040001F2 RID: 498
		private readonly IPageContentParser pageContentParser;

		// Token: 0x040001F3 RID: 499
		private readonly ILog log;
	}
}
