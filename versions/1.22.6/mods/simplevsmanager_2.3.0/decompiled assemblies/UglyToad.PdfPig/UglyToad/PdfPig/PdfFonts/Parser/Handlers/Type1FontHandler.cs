using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Filters;
using UglyToad.PdfPig.Fonts;
using UglyToad.PdfPig.Fonts.AdobeFontMetrics;
using UglyToad.PdfPig.Fonts.CompactFontFormat;
using UglyToad.PdfPig.Fonts.Encodings;
using UglyToad.PdfPig.Fonts.Standard14Fonts;
using UglyToad.PdfPig.Fonts.Type1;
using UglyToad.PdfPig.Fonts.Type1.Parser;
using UglyToad.PdfPig.Parser.Parts;
using UglyToad.PdfPig.PdfFonts.Cmap;
using UglyToad.PdfPig.PdfFonts.Simple;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.PdfFonts.Parser.Handlers
{
	// Token: 0x020000AB RID: 171
	internal class Type1FontHandler : IFontHandler
	{
		// Token: 0x060002E8 RID: 744 RVA: 0x00013E6A File Offset: 0x0001206A
		public Type1FontHandler(IPdfTokenScanner pdfScanner, ILookupFilterProvider filterProvider, IEncodingReader encodingReader)
		{
			this.pdfScanner = pdfScanner;
			this.filterProvider = filterProvider;
			this.encodingReader = encodingReader;
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x00013E88 File Offset: 0x00012088
		public IFont Generate(DictionaryToken dictionary)
		{
			bool flag = !dictionary.ContainsKey(NameToken.FirstChar) || !dictionary.ContainsKey(NameToken.Widths);
			if (flag)
			{
				IToken token;
				if (dictionary.TryGet(NameToken.BaseFont, ref token))
				{
					NameToken nameToken = token as NameToken;
					if (nameToken != null)
					{
						AdobeFontMetrics adobeFontMetrics = Standard14.GetAdobeFontMetrics(nameToken.Data);
						if (adobeFontMetrics != null)
						{
							Encoding overrideEncoding = this.encodingReader.Read(dictionary, null, null);
							return new Type1Standard14Font(adobeFontMetrics, overrideEncoding);
						}
						goto IL_95;
					}
				}
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(103, 1);
				defaultInterpolatedStringHandler.AppendLiteral("The Type 1 font did not contain a first character entry but also did not reference a standard 14 font: ");
				defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(dictionary);
				throw new InvalidFontFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			IL_95:
			int firstChar;
			int lastChar;
			double[] widths;
			if (!flag)
			{
				firstChar = FontDictionaryAccessHelper.GetFirstCharacter(dictionary);
				lastChar = FontDictionaryAccessHelper.GetLastCharacter(dictionary);
				widths = FontDictionaryAccessHelper.GetWidths(this.pdfScanner, dictionary);
			}
			else
			{
				firstChar = 0;
				lastChar = 0;
				widths = EmptyArray<double>.Instance;
			}
			IToken token2;
			IToken token3;
			NameToken nameToken2;
			if (!dictionary.TryGet(NameToken.FontDescriptor, ref token2) && dictionary.TryGet(NameToken.BaseFont, ref token3) && DirectObjectFinder.TryGet<NameToken>(token3, this.pdfScanner, out nameToken2))
			{
				AdobeFontMetrics adobeFontMetrics2 = Standard14.GetAdobeFontMetrics(nameToken2.Data);
				Encoding overrideEncoding2 = this.encodingReader.Read(dictionary, null, null);
				return new Type1Standard14Font(adobeFontMetrics2, overrideEncoding2);
			}
			FontDescriptor fontDescriptor = FontDictionaryAccessHelper.GetFontDescriptor(this.pdfScanner, dictionary);
			Union<Type1Font, CompactFontFormatFontCollection> union = this.ParseFontProgram(fontDescriptor);
			NameToken name = FontDictionaryAccessHelper.GetName(this.pdfScanner, dictionary, fontDescriptor);
			CMap toUnicodeCMap = null;
			IToken token4;
			if (dictionary.TryGet(NameToken.ToUnicode, ref token4))
			{
				StreamToken streamToken = DirectObjectFinder.Get<StreamToken>(token4, this.pdfScanner);
				IReadOnlyList<byte> readOnlyList = (streamToken != null) ? streamToken.Decode(this.filterProvider, this.pdfScanner) : null;
				if (readOnlyList != null)
				{
					toUnicodeCMap = CMapCache.Parse(new ByteArrayInputBytes(readOnlyList));
				}
			}
			Encoding fontEncoding = null;
			if (union != null)
			{
				Type1Font type1Font;
				CompactFontFormatFontCollection compactFontFormatFontCollection;
				if (union.TryGetFirst(ref type1Font))
				{
					fontEncoding = ((type1Font.Encoding != null) ? new BuiltInEncoding(type1Font.Encoding) : null);
				}
				else if (union.TryGetSecond(ref compactFontFormatFontCollection))
				{
					CompactFontFormatFont firstFont = compactFontFormatFontCollection.FirstFont;
					fontEncoding = ((firstFont != null) ? firstFont.Encoding : null);
				}
			}
			Encoding encoding = this.encodingReader.Read(dictionary, fontDescriptor, fontEncoding);
			Type1Font type1Font2;
			if (encoding == null && union != null && union.TryGetFirst(ref type1Font2))
			{
				encoding = new BuiltInEncoding(type1Font2.Encoding);
			}
			return new Type1FontSimple(name, firstChar, lastChar, widths, fontDescriptor, encoding, toUnicodeCMap, union);
		}

		// Token: 0x060002EA RID: 746 RVA: 0x000140AC File Offset: 0x000122AC
		private Union<Type1Font, CompactFontFormatFontCollection> ParseFontProgram(FontDescriptor descriptor)
		{
			if (((descriptor != null) ? descriptor.FontFile : null) == null)
			{
				return null;
			}
			if (descriptor.FontFile.ObjectKey.Data.ObjectNumber == 0L)
			{
				return null;
			}
			try
			{
				ObjectToken objectToken = this.pdfScanner.Get(descriptor.FontFile.ObjectKey.Data);
				StreamToken streamToken = ((objectToken != null) ? objectToken.Data : null) as StreamToken;
				if (streamToken == null)
				{
					return null;
				}
				IReadOnlyList<byte> readOnlyList = streamToken.Decode(this.filterProvider, this.pdfScanner);
				NameToken nameToken;
				if (streamToken.StreamDictionary.TryGet<NameToken>(NameToken.Subtype, ref nameToken) && NameToken.Type1C.Equals(nameToken))
				{
					return Union<Type1Font, CompactFontFormatFontCollection>.Two(CompactFontFormatParser.Parse(new CompactFontFormatData(readOnlyList)));
				}
				NumericToken numericToken = streamToken.StreamDictionary.Get(NameToken.Length1, this.pdfScanner);
				NumericToken numericToken2 = streamToken.StreamDictionary.Get(NameToken.Length2, this.pdfScanner);
				return Union<Type1Font, CompactFontFormatFontCollection>.One(Type1FontParser.Parse(new ByteArrayInputBytes(readOnlyList), numericToken.Int, numericToken2.Int));
			}
			catch
			{
			}
			return null;
		}

		// Token: 0x0400018D RID: 397
		private readonly IPdfTokenScanner pdfScanner;

		// Token: 0x0400018E RID: 398
		private readonly ILookupFilterProvider filterProvider;

		// Token: 0x0400018F RID: 399
		private readonly IEncodingReader encodingReader;
	}
}
