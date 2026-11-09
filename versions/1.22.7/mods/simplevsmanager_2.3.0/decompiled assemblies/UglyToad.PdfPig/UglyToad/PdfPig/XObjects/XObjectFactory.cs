using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Filters;
using UglyToad.PdfPig.Graphics;
using UglyToad.PdfPig.Graphics.Colors;
using UglyToad.PdfPig.Graphics.Core;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util;

namespace UglyToad.PdfPig.XObjects
{
	// Token: 0x0200000A RID: 10
	internal static class XObjectFactory
	{
		// Token: 0x06000039 RID: 57 RVA: 0x0000296C File Offset: 0x00000B6C
		public static XObjectImage ReadImage(XObjectContentRecord xObject, IPdfTokenScanner pdfScanner, ILookupFilterProvider filterProvider, IResourceStore resourceStore)
		{
			if (xObject == null)
			{
				throw new ArgumentNullException("xObject");
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (xObject.Type != XObjectType.Image)
			{
				defaultInterpolatedStringHandler..ctor(51, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Cannot create an image from an XObject with type: ");
				defaultInterpolatedStringHandler.AppendFormatted<XObjectType>(xObject.Type);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			DictionaryToken dictionaryToken = xObject.Stream.StreamDictionary;
			PdfRectangle bounds = xObject.AppliedTransformation.Transform(new PdfRectangle(new PdfPoint(0, 0), new PdfPoint(1, 1)));
			int @int = dictionaryToken.Get(NameToken.Width, pdfScanner).Int;
			int int2 = dictionaryToken.Get(NameToken.Height, pdfScanner).Int;
			BooleanToken booleanToken;
			bool flag = dictionaryToken.TryGet(NameToken.ImageMask, pdfScanner, out booleanToken) && booleanToken.Data;
			IToken token;
			bool flag2;
			if (dictionaryToken.TryGet(NameToken.Filter, ref token))
			{
				NameToken nameToken = token as NameToken;
				if (nameToken != null)
				{
					flag2 = nameToken.Equals(NameToken.JpxDecode);
					goto IL_110;
				}
			}
			flag2 = false;
			IL_110:
			bool flag3 = flag2;
			int bitsPerComponent = 0;
			if (!flag && !flag3)
			{
				NumericToken numericToken;
				if (!dictionaryToken.TryGet(NameToken.BitsPerComponent, pdfScanner, out numericToken))
				{
					defaultInterpolatedStringHandler..ctor(42, 1);
					defaultInterpolatedStringHandler.AppendLiteral("No bits per component defined for image: ");
					defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(dictionaryToken);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bitsPerComponent = numericToken.Int;
			}
			else if (flag)
			{
				bitsPerComponent = 1;
			}
			RenderingIntent renderingIntent = xObject.DefaultRenderingIntent;
			NameToken nameToken2;
			if (dictionaryToken.TryGet<NameToken>(NameToken.Intent, ref nameToken2))
			{
				renderingIntent = nameToken2.Data.ToRenderingIntent();
			}
			BooleanToken booleanToken2;
			bool interpolate = dictionaryToken.TryGet(NameToken.Interpolate, pdfScanner, out booleanToken2) && booleanToken2.Data;
			DictionaryToken dictionaryToken2 = xObject.Stream.StreamDictionary;
			IToken token2;
			if (xObject.Stream.StreamDictionary.TryGet(NameToken.Filter, ref token2) && token2 is IndirectReferenceToken)
			{
				ArrayToken arrayToken;
				NameToken nameToken3;
				if (dictionaryToken2.TryGet(NameToken.Filter, pdfScanner, out arrayToken))
				{
					dictionaryToken2 = dictionaryToken2.With(NameToken.Filter, arrayToken);
				}
				else if (dictionaryToken2.TryGet(NameToken.Filter, pdfScanner, out nameToken3))
				{
					dictionaryToken2 = dictionaryToken2.With(NameToken.Filter, nameToken3);
				}
				else
				{
					dictionaryToken2 = null;
				}
			}
			bool flag4 = dictionaryToken2 != null;
			if (dictionaryToken2 != null)
			{
				using (IEnumerator<IFilter> enumerator = filterProvider.GetFilters(dictionaryToken2, pdfScanner).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.IsSupported)
						{
							flag4 = false;
							break;
						}
					}
				}
			}
			IndirectReferenceToken indirectReferenceToken = dictionaryToken.GetObjectOrDefault(NameToken.DecodeParms, NameToken.Dp) as IndirectReferenceToken;
			if (indirectReferenceToken != null)
			{
				dictionaryToken = dictionaryToken.With(NameToken.DecodeParms, pdfScanner.Get(indirectReferenceToken.Data).Data);
			}
			StreamToken streamToken = new StreamToken(dictionaryToken, xObject.Stream.Data);
			Lazy<IReadOnlyList<byte>> bytes = flag4 ? new Lazy<IReadOnlyList<byte>>(() => streamToken.Decode(filterProvider, pdfScanner)) : null;
			decimal[] decode = EmptyArray<decimal>.Instance;
			ArrayToken arrayToken2;
			if (dictionaryToken.TryGet(NameToken.Decode, pdfScanner, out arrayToken2))
			{
				decode = Enumerable.ToArray<decimal>(Enumerable.Select<NumericToken, decimal>(Enumerable.OfType<NumericToken>(arrayToken2.Data), (NumericToken x) => x.Data));
			}
			ColorSpaceDetails colorSpaceDetails = null;
			if (!flag)
			{
				NameToken name;
				if (dictionaryToken.TryGet(NameToken.ColorSpace, pdfScanner, out name))
				{
					colorSpaceDetails = resourceStore.GetColorSpaceDetails(name, dictionaryToken);
				}
				else
				{
					ArrayToken arrayToken3;
					if (dictionaryToken.TryGet(NameToken.ColorSpace, pdfScanner, out arrayToken3) && arrayToken3.Length > 0)
					{
						NameToken nameToken4 = arrayToken3.Data[0] as NameToken;
						if (nameToken4 != null)
						{
							colorSpaceDetails = resourceStore.GetColorSpaceDetails(nameToken4, dictionaryToken);
							goto IL_3DB;
						}
					}
					if (!flag3)
					{
						colorSpaceDetails = xObject.DefaultColorSpace;
					}
				}
			}
			else
			{
				colorSpaceDetails = resourceStore.GetColorSpaceDetails(null, dictionaryToken);
			}
			IL_3DB:
			return new XObjectImage(bounds, @int, int2, bitsPerComponent, flag3, flag, renderingIntent, interpolate, decode, dictionaryToken, xObject.Stream.Data, bytes, colorSpaceDetails);
		}
	}
}
