using System;
using System.Collections.Generic;
using System.Linq;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Geometry;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util;

namespace UglyToad.PdfPig.Annotations
{
	// Token: 0x02000201 RID: 513
	internal static class HyperlinkFactory
	{
		// Token: 0x06000C5E RID: 3166 RVA: 0x0003DB90 File Offset: 0x0003BD90
		public static IReadOnlyList<Hyperlink> GetHyperlinks(Page page, IPdfTokenScanner pdfScanner, AnnotationProvider annotationProvider)
		{
			List<Hyperlink> list = new List<Hyperlink>();
			foreach (Annotation annotation in annotationProvider.GetAnnotations())
			{
				DictionaryToken dictionary;
				NameToken nameToken;
				IDataToken<string> dataToken;
				if (annotation.Type == AnnotationType.Link && annotation.AnnotationDictionary.TryGet(NameToken.A, pdfScanner, out dictionary) && dictionary.TryGet(NameToken.S, pdfScanner, out nameToken) && !(nameToken != NameToken.Uri) && dictionary.TryGet(NameToken.Uri, pdfScanner, out dataToken))
				{
					PdfRectangle rectangle = annotation.Rectangle;
					PdfRectangle rectangle2;
					rectangle2..ctor(rectangle.BottomLeft.Translate(-0.5, -0.5), rectangle.TopRight.Translate(0.5, 0.5));
					List<Letter> list2 = new List<Letter>();
					foreach (Letter letter in page.Letters)
					{
						if (rectangle2.Contains(letter.Location, true))
						{
							list2.Add(letter);
						}
					}
					IEnumerable<Word> words = DefaultWordExtractor.Instance.GetWords(list2);
					string text = string.Join(" ", Enumerable.Select<Word, string>(words, (Word x) => x.Text));
					list.Add(new Hyperlink(rectangle, list2, text, dataToken.Data, annotation));
				}
			}
			return list;
		}
	}
}
