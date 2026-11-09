using System;
using System.Collections.Generic;
using System.Linq;
using UglyToad.PdfPig.Actions;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Logging;
using UglyToad.PdfPig.Outline;
using UglyToad.PdfPig.Outline.Destinations;
using UglyToad.PdfPig.Parser.Parts;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util;

namespace UglyToad.PdfPig.Annotations
{
	// Token: 0x020001FD RID: 509
	internal class AnnotationProvider
	{
		// Token: 0x06000C54 RID: 3156 RVA: 0x0003D88C File Offset: 0x0003BA8C
		public AnnotationProvider(IPdfTokenScanner tokenScanner, DictionaryToken pageDictionary, TransformationMatrix matrix, NamedDestinations namedDestinations, ILog log)
		{
			this.matrix = matrix;
			if (tokenScanner == null)
			{
				throw new ArgumentNullException("tokenScanner");
			}
			this.tokenScanner = tokenScanner;
			if (pageDictionary == null)
			{
				throw new ArgumentNullException("pageDictionary");
			}
			this.pageDictionary = pageDictionary;
			this.namedDestinations = namedDestinations;
			this.log = log;
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x0003D8E2 File Offset: 0x0003BAE2
		public IEnumerable<Annotation> GetAnnotations()
		{
			Dictionary<IndirectReference, Annotation> lookupAnnotations = new Dictionary<IndirectReference, Annotation>();
			ArrayToken arrayToken;
			if (!this.pageDictionary.TryGet(NameToken.Annots, this.tokenScanner, out arrayToken))
			{
				yield break;
			}
			foreach (IToken token in arrayToken.Data)
			{
				DictionaryToken dictionaryToken;
				if (DirectObjectFinder.TryGet<DictionaryToken>(token, this.tokenScanner, out dictionaryToken))
				{
					Annotation inReplyTo = null;
					IndirectReferenceToken indirectReferenceToken;
					Annotation annotation;
					if (dictionaryToken.TryGet<IndirectReferenceToken>(NameToken.Irt, ref indirectReferenceToken) && lookupAnnotations.TryGetValue(indirectReferenceToken.Data, ref annotation))
					{
						inReplyTo = annotation;
					}
					AnnotationType type = dictionaryToken.Get(NameToken.Subtype, this.tokenScanner).ToAnnotationType();
					PdfAction action = this.GetAction(dictionaryToken);
					PdfRectangle rectangle = this.matrix.Transform(dictionaryToken.Get(NameToken.Rect, this.tokenScanner).ToRectangle(this.tokenScanner));
					string namedString = this.GetNamedString(NameToken.Contents, dictionaryToken);
					string namedString2 = this.GetNamedString(NameToken.Nm, dictionaryToken);
					string namedString3 = this.GetNamedString(NameToken.M, dictionaryToken);
					AnnotationFlags flags = (AnnotationFlags)0;
					IToken token2;
					NumericToken numericToken;
					if (dictionaryToken.TryGet(NameToken.F, ref token2) && DirectObjectFinder.TryGet<NumericToken>(token2, this.tokenScanner, out numericToken))
					{
						flags = (AnnotationFlags)numericToken.Int;
					}
					AnnotationBorder border = AnnotationBorder.Default;
					IToken token3;
					ArrayToken arrayToken2;
					if (dictionaryToken.TryGet(NameToken.Border, ref token3) && DirectObjectFinder.TryGet<ArrayToken>(token3, this.tokenScanner, out arrayToken2) && arrayToken2.Length >= 3)
					{
						decimal data = arrayToken2.GetNumeric(0).Data;
						decimal data2 = arrayToken2.GetNumeric(1).Data;
						decimal data3 = arrayToken2.GetNumeric(2).Data;
						IReadOnlyList<decimal> lineDashPattern = null;
						if (arrayToken2.Length == 4)
						{
							ArrayToken arrayToken3 = arrayToken2.Data[4] as ArrayToken;
							if (arrayToken3 != null)
							{
								lineDashPattern = Enumerable.ToList<decimal>(Enumerable.Select<NumericToken, decimal>(Enumerable.OfType<NumericToken>(arrayToken3.Data), (NumericToken x) => x.Data));
							}
						}
						border = new AnnotationBorder(data, data2, data3, lineDashPattern);
					}
					List<QuadPointsQuadrilateral> list = new List<QuadPointsQuadrilateral>();
					ArrayToken arrayToken4;
					if (dictionaryToken.TryGet(NameToken.Quadpoints, this.tokenScanner, out arrayToken4))
					{
						List<decimal> list2 = new List<decimal>();
						for (int i = 0; i < arrayToken4.Length; i++)
						{
							NumericToken numericToken2 = arrayToken4[i] as NumericToken;
							if (numericToken2 != null)
							{
								list2.Add(numericToken2.Data);
								if (list2.Count == 8)
								{
									list.Add(new QuadPointsQuadrilateral(new PdfPoint[]
									{
										this.matrix.Transform(new PdfPoint(list2[0], list2[1])),
										this.matrix.Transform(new PdfPoint(list2[2], list2[3])),
										this.matrix.Transform(new PdfPoint(list2[4], list2[5])),
										this.matrix.Transform(new PdfPoint(list2[6], list2[7]))
									}));
									list2.Clear();
								}
							}
						}
					}
					AppearanceStream normalAppearanceStream = null;
					AppearanceStream downAppearanceStream = null;
					AppearanceStream rollOverAppearanceStream = null;
					DictionaryToken appearanceDictionary;
					if (dictionaryToken.TryGet<DictionaryToken>(NameToken.Ap, ref appearanceDictionary))
					{
						AppearanceStream appearanceStream;
						if (AppearanceStreamFactory.TryCreate(appearanceDictionary, NameToken.N, this.tokenScanner, out appearanceStream))
						{
							normalAppearanceStream = appearanceStream;
						}
						if (AppearanceStreamFactory.TryCreate(appearanceDictionary, NameToken.R, this.tokenScanner, out appearanceStream))
						{
							rollOverAppearanceStream = appearanceStream;
						}
						if (AppearanceStreamFactory.TryCreate(appearanceDictionary, NameToken.D, this.tokenScanner, out appearanceStream))
						{
							downAppearanceStream = appearanceStream;
						}
					}
					string appearanceState = null;
					NameToken nameToken;
					if (dictionaryToken.TryGet<NameToken>(NameToken.As, ref nameToken))
					{
						appearanceState = nameToken.Data;
					}
					Annotation annotation2 = new Annotation(dictionaryToken, type, rectangle, namedString, namedString2, namedString3, flags, border, list, action, normalAppearanceStream, rollOverAppearanceStream, downAppearanceStream, appearanceState, inReplyTo);
					IndirectReferenceToken indirectReferenceToken2 = token as IndirectReferenceToken;
					if (indirectReferenceToken2 != null)
					{
						lookupAnnotations[indirectReferenceToken2.Data] = annotation2;
					}
					yield return annotation2;
				}
			}
			IEnumerator<IToken> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x0003D8F4 File Offset: 0x0003BAF4
		internal PdfAction GetAction(DictionaryToken annotationDictionary)
		{
			ExplicitDestination destination;
			if (DestinationProvider.TryGetDestination(annotationDictionary, NameToken.Dest, this.namedDestinations, this.tokenScanner, this.log, false, out destination))
			{
				return new GoToAction(destination);
			}
			PdfAction result;
			if (ActionProvider.TryGetAction(annotationDictionary, this.namedDestinations, this.tokenScanner, this.log, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06000C57 RID: 3159 RVA: 0x0003D94C File Offset: 0x0003BB4C
		private string GetNamedString(NameToken name, DictionaryToken dictionary)
		{
			string result = null;
			IToken token;
			if (dictionary.TryGet(name, ref token))
			{
				StringToken stringToken = token as StringToken;
				if (stringToken != null)
				{
					result = stringToken.Data;
				}
				else
				{
					HexToken hexToken = token as HexToken;
					StringToken stringToken2;
					if (hexToken != null)
					{
						result = hexToken.Data;
					}
					else if (DirectObjectFinder.TryGet<StringToken>(token, this.tokenScanner, out stringToken2))
					{
						result = stringToken2.Data;
					}
				}
			}
			return result;
		}

		// Token: 0x0400069E RID: 1694
		private readonly IPdfTokenScanner tokenScanner;

		// Token: 0x0400069F RID: 1695
		private readonly DictionaryToken pageDictionary;

		// Token: 0x040006A0 RID: 1696
		private readonly NamedDestinations namedDestinations;

		// Token: 0x040006A1 RID: 1697
		private readonly ILog log;

		// Token: 0x040006A2 RID: 1698
		private readonly TransformationMatrix matrix;
	}
}
