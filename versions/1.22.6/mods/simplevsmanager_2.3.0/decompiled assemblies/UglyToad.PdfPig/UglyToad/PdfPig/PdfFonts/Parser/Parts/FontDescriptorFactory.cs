using System;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.PdfFonts.Parser.Parts
{
	// Token: 0x020000A6 RID: 166
	internal static class FontDescriptorFactory
	{
		// Token: 0x060002D4 RID: 724 RVA: 0x000130B0 File Offset: 0x000112B0
		public static FontDescriptor Generate(DictionaryToken dictionary, IPdfTokenScanner pdfScanner)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			NameToken fontName = FontDescriptorFactory.GetFontName(dictionary, pdfScanner);
			string fontFamily = FontDescriptorFactory.GetFontFamily(dictionary);
			FontStretch fontStretch = FontDescriptorFactory.GetFontStretch(dictionary);
			FontDescriptorFlags flags = FontDescriptorFactory.GetFlags(dictionary);
			PdfRectangle boundingBox = FontDescriptorFactory.GetBoundingBox(dictionary, pdfScanner);
			string charSet = FontDescriptorFactory.GetCharSet(dictionary);
			DescriptorFontFile fontFile = FontDescriptorFactory.GetFontFile(dictionary);
			return new FontDescriptor.Builder(fontName, flags)
			{
				FontFamily = fontFamily,
				Stretch = fontStretch,
				FontWeight = FontDescriptorFactory.GetDecimalOrDefault(dictionary, NameToken.FontWeight),
				BoundingBox = boundingBox,
				ItalicAngle = FontDescriptorFactory.GetDecimalOrDefault(dictionary, NameToken.ItalicAngle),
				Ascent = FontDescriptorFactory.GetDecimalOrDefault(dictionary, NameToken.Ascent),
				Descent = FontDescriptorFactory.GetDecimalOrDefault(dictionary, NameToken.Descent),
				Leading = FontDescriptorFactory.GetDecimalOrDefault(dictionary, NameToken.Leading),
				CapHeight = Math.Abs(FontDescriptorFactory.GetDecimalOrDefault(dictionary, NameToken.CapHeight)),
				XHeight = Math.Abs(FontDescriptorFactory.GetDecimalOrDefault(dictionary, NameToken.Xheight)),
				StemVertical = FontDescriptorFactory.GetDecimalOrDefault(dictionary, NameToken.StemV),
				StemHorizontal = FontDescriptorFactory.GetDecimalOrDefault(dictionary, NameToken.StemH),
				AverageWidth = FontDescriptorFactory.GetDecimalOrDefault(dictionary, NameToken.AvgWidth),
				MaxWidth = FontDescriptorFactory.GetDecimalOrDefault(dictionary, NameToken.MaxWidth),
				MissingWidth = FontDescriptorFactory.GetDecimalOrDefault(dictionary, NameToken.MissingWidth),
				FontFile = fontFile,
				CharSet = charSet
			}.Build();
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x00013208 File Offset: 0x00011408
		private static decimal GetDecimalOrDefault(DictionaryToken dictionary, NameToken name)
		{
			IToken token;
			if (dictionary.TryGet(name, ref token))
			{
				NumericToken numericToken = token as NumericToken;
				if (numericToken != null)
				{
					return numericToken.Data;
				}
			}
			return 0m;
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x00013238 File Offset: 0x00011438
		private static NameToken GetFontName(DictionaryToken dictionary, IPdfTokenScanner scanner)
		{
			NameToken result;
			if (!dictionary.TryGet(NameToken.FontName, scanner, out result))
			{
				result = NameToken.Create(string.Empty);
			}
			return result;
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x00013264 File Offset: 0x00011464
		private static string GetFontFamily(DictionaryToken dictionary)
		{
			IToken token;
			if (dictionary.TryGet(NameToken.FontFamily, ref token))
			{
				StringToken stringToken = token as StringToken;
				if (stringToken != null)
				{
					return stringToken.Data;
				}
			}
			return string.Empty;
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x00013298 File Offset: 0x00011498
		private static FontStretch GetFontStretch(DictionaryToken dictionary)
		{
			IToken token;
			if (dictionary.TryGet(NameToken.FontStretch, ref token))
			{
				NameToken nameToken = token as NameToken;
				if (nameToken != null)
				{
					return nameToken.ConvertToFontStretch();
				}
			}
			return FontStretch.Normal;
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x000132C8 File Offset: 0x000114C8
		private static FontDescriptorFlags GetFlags(DictionaryToken dictionary)
		{
			int num = dictionary.GetIntOrDefault(NameToken.Flags, -1);
			if (num == -1)
			{
				num = 0;
			}
			return (FontDescriptorFlags)num;
		}

		// Token: 0x060002DA RID: 730 RVA: 0x000132EC File Offset: 0x000114EC
		private static PdfRectangle GetBoundingBox(DictionaryToken dictionary, IPdfTokenScanner pdfScanner)
		{
			IToken token;
			if (dictionary.TryGet(NameToken.FontBbox, ref token))
			{
				ArrayToken arrayToken = token as ArrayToken;
				if (arrayToken != null)
				{
					if (arrayToken.Data.Count != 4)
					{
						return new PdfRectangle(0, 0, 0, 0);
					}
					return arrayToken.ToRectangle(pdfScanner);
				}
			}
			return new PdfRectangle(0, 0, 0, 0);
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0001333C File Offset: 0x0001153C
		private static string GetCharSet(DictionaryToken dictionary)
		{
			IToken token;
			if (dictionary.TryGet(NameToken.CharSet, ref token))
			{
				NameToken nameToken = token as NameToken;
				if (nameToken != null)
				{
					return nameToken.Data;
				}
			}
			return null;
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0001336C File Offset: 0x0001156C
		[CanBeNull]
		private static DescriptorFontFile GetFontFile(DictionaryToken dictionary)
		{
			IToken token;
			if (dictionary.TryGet(NameToken.FontFile, ref token))
			{
				IndirectReferenceToken indirectReferenceToken = token as IndirectReferenceToken;
				if (indirectReferenceToken == null)
				{
					throw new NotSupportedException("We currently expect the FontFile to be an object reference.");
				}
				return new DescriptorFontFile(indirectReferenceToken, DescriptorFontFile.FontFileType.Type1);
			}
			else if (dictionary.TryGet(NameToken.FontFile2, ref token))
			{
				IndirectReferenceToken indirectReferenceToken2 = token as IndirectReferenceToken;
				if (indirectReferenceToken2 == null)
				{
					throw new NotSupportedException("We currently expect the FontFile2 to be an object reference.");
				}
				return new DescriptorFontFile(indirectReferenceToken2, DescriptorFontFile.FontFileType.TrueType);
			}
			else
			{
				if (!dictionary.TryGet(NameToken.FontFile3, ref token))
				{
					return null;
				}
				IndirectReferenceToken indirectReferenceToken3 = token as IndirectReferenceToken;
				if (indirectReferenceToken3 == null)
				{
					throw new NotSupportedException("We currently expect the FontFile3 to be an object reference.");
				}
				return new DescriptorFontFile(indirectReferenceToken3, DescriptorFontFile.FontFileType.FromSubtype);
			}
		}
	}
}
