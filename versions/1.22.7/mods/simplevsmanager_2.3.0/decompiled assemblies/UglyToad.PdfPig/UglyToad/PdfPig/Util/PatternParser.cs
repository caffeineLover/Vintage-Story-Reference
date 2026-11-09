using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Filters;
using UglyToad.PdfPig.Graphics.Colors;
using UglyToad.PdfPig.Parser.Parts;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Util
{
	// Token: 0x02000033 RID: 51
	internal static class PatternParser
	{
		// Token: 0x06000146 RID: 326 RVA: 0x0000C5F4 File Offset: 0x0000A7F4
		public static PatternColor Create(IToken pattern, IPdfTokenScanner scanner, IResourceStore resourceStore, ILookupFilterProvider filterProvider)
		{
			StreamToken patternStream = null;
			StreamToken streamToken;
			DictionaryToken dictionaryToken;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (DirectObjectFinder.TryGet<StreamToken>(pattern, scanner, out streamToken))
			{
				dictionaryToken = streamToken.StreamDictionary;
				patternStream = new StreamToken(streamToken.StreamDictionary, streamToken.Decode(filterProvider, scanner));
			}
			else
			{
				DictionaryToken dictionaryToken2;
				if (!DirectObjectFinder.TryGet<DictionaryToken>(pattern, scanner, out dictionaryToken2))
				{
					defaultInterpolatedStringHandler..ctor(64, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Invalid Pattern token encountered in page resource dictionary: ");
					defaultInterpolatedStringHandler.AppendFormatted<IToken>(pattern);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				dictionaryToken = dictionaryToken2;
			}
			if (!dictionaryToken.Data.ContainsKey(NameToken.PatternType))
			{
				throw new Exception("TODO");
			}
			int @int = (dictionaryToken.Data[NameToken.PatternType] as NumericToken).Int;
			ArrayToken arrayToken;
			TransformationMatrix matrix;
			if (dictionaryToken.Data.ContainsKey(NameToken.Matrix) && DirectObjectFinder.TryGet<ArrayToken>(dictionaryToken.Data[NameToken.Matrix], scanner, out arrayToken))
			{
				matrix = TransformationMatrix.FromArray(Enumerable.ToArray<decimal>(Enumerable.Select<NumericToken, decimal>(Enumerable.OfType<NumericToken>(arrayToken.Data), (NumericToken n) => n.Data)));
			}
			else
			{
				decimal[] array = new decimal[6];
				array[0] = 1m;
				array[3] = 1m;
				matrix = TransformationMatrix.FromArray(array);
			}
			DictionaryToken patternExtGState = null;
			if (dictionaryToken.Data.ContainsKey(NameToken.ExtGState))
			{
				DirectObjectFinder.TryGet<DictionaryToken>(dictionaryToken.Data[NameToken.ExtGState], scanner, out patternExtGState);
			}
			if (@int == 1)
			{
				return PatternParser.CreateTilingPattern(patternStream, patternExtGState, matrix, scanner);
			}
			if (@int != 2)
			{
				defaultInterpolatedStringHandler..ctor(63, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid Pattern type encountered in page resource dictionary: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(@int);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return PatternParser.CreateShadingPattern(dictionaryToken, patternExtGState, matrix, scanner, resourceStore, filterProvider);
		}

		// Token: 0x06000147 RID: 327 RVA: 0x0000C7E0 File Offset: 0x0000A9E0
		private static PatternColor CreateTilingPattern(StreamToken patternStream, DictionaryToken patternExtGState, TransformationMatrix matrix, IPdfTokenScanner scanner)
		{
			NumericToken numericToken;
			if (!patternStream.StreamDictionary.TryGet(NameToken.PaintType, scanner, out numericToken))
			{
				throw new PdfDocumentFormatException("Invalid Pattern token encountered.");
			}
			NumericToken numericToken2;
			if (!patternStream.StreamDictionary.TryGet(NameToken.TilingType, scanner, out numericToken2))
			{
				throw new PdfDocumentFormatException("Invalid Pattern token encountered.");
			}
			ArrayToken array;
			if (!patternStream.StreamDictionary.TryGet(NameToken.Bbox, scanner, out array))
			{
				throw new PdfDocumentFormatException("Invalid Pattern token encountered.");
			}
			NumericToken numericToken3;
			if (!patternStream.StreamDictionary.TryGet(NameToken.XStep, scanner, out numericToken3))
			{
				throw new PdfDocumentFormatException("Invalid Pattern token encountered.");
			}
			NumericToken numericToken4;
			if (!patternStream.StreamDictionary.TryGet(NameToken.YStep, scanner, out numericToken4))
			{
				throw new PdfDocumentFormatException("Invalid Pattern token encountered.");
			}
			DictionaryToken resources;
			if (!patternStream.StreamDictionary.TryGet(NameToken.Resources, scanner, out resources))
			{
				throw new PdfDocumentFormatException("Invalid Pattern token encountered.");
			}
			return new TilingPatternColor(matrix, patternExtGState, patternStream, (PatternPaintType)numericToken.Int, (PatternTilingType)numericToken2.Int, array.ToRectangle(scanner), numericToken3.Double, numericToken4.Double, resources, patternStream.Data);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x0000C8E0 File Offset: 0x0000AAE0
		private static PatternColor CreateShadingPattern(DictionaryToken patternDictionary, DictionaryToken patternExtGState, TransformationMatrix matrix, IPdfTokenScanner scanner, IResourceStore resourceStore, ILookupFilterProvider filterProvider)
		{
			IToken token = patternDictionary.Data[NameToken.Shading];
			DictionaryToken shading;
			Shading shading2;
			if (DirectObjectFinder.TryGet<DictionaryToken>(token, scanner, out shading))
			{
				shading2 = ShadingParser.Create(shading, scanner, resourceStore, filterProvider);
			}
			else
			{
				StreamToken shading3;
				if (!DirectObjectFinder.TryGet<StreamToken>(token, scanner, out shading3))
				{
					throw new ArgumentException("TODO");
				}
				shading2 = ShadingParser.Create(shading3, scanner, resourceStore, filterProvider);
			}
			return new ShadingPatternColor(matrix, patternExtGState, patternDictionary, shading2);
		}
	}
}
