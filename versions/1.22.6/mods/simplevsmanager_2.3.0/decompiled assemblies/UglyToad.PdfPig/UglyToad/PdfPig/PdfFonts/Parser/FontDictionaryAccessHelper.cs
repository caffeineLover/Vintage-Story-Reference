using System;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Fonts;
using UglyToad.PdfPig.Parser.Parts;
using UglyToad.PdfPig.PdfFonts.Parser.Parts;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.PdfFonts.Parser
{
	// Token: 0x0200009D RID: 157
	internal static class FontDictionaryAccessHelper
	{
		// Token: 0x060002B6 RID: 694 RVA: 0x00011C80 File Offset: 0x0000FE80
		public static int GetFirstCharacter(DictionaryToken dictionary)
		{
			IToken token;
			if (dictionary.TryGet(NameToken.FirstChar, ref token))
			{
				NumericToken numericToken = token as NumericToken;
				if (numericToken != null)
				{
					return numericToken.Int;
				}
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(83, 1);
			defaultInterpolatedStringHandler.AppendLiteral("No first character entry was found in the font dictionary for this TrueType font: ");
			defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(dictionary);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			throw new InvalidFontFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x00011CE4 File Offset: 0x0000FEE4
		public static int GetLastCharacter(DictionaryToken dictionary)
		{
			IToken token;
			if (dictionary.TryGet(NameToken.LastChar, ref token))
			{
				NumericToken numericToken = token as NumericToken;
				if (numericToken != null)
				{
					return numericToken.Int;
				}
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(83, 1);
			defaultInterpolatedStringHandler.AppendLiteral("No first character entry was found in the font dictionary for this TrueType font: ");
			defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(dictionary);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			throw new InvalidFontFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x00011D48 File Offset: 0x0000FF48
		public static double[] GetWidths(IPdfTokenScanner pdfScanner, DictionaryToken dictionary)
		{
			IToken token;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (!dictionary.TryGet(NameToken.Widths, ref token))
			{
				defaultInterpolatedStringHandler..ctor(37, 1);
				defaultInterpolatedStringHandler.AppendLiteral("No widths array found for the font: ");
				defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(dictionary);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new InvalidFontFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			ArrayToken arrayToken = DirectObjectFinder.Get<ArrayToken>(token, pdfScanner);
			double[] array = new double[arrayToken.Data.Count];
			for (int i = 0; i < arrayToken.Data.Count; i++)
			{
				IToken token2 = arrayToken.Data[i];
				NumericToken numericToken = token2 as NumericToken;
				if (numericToken == null)
				{
					defaultInterpolatedStringHandler..ctor(57, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Token which was not a number found in the widths array: ");
					defaultInterpolatedStringHandler.AppendFormatted<IToken>(token2);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new InvalidFontFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				array[i] = numericToken.Double;
			}
			return array;
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x00011E30 File Offset: 0x00010030
		public static FontDescriptor GetFontDescriptor(IPdfTokenScanner pdfScanner, DictionaryToken dictionary)
		{
			DictionaryToken dictionary2;
			if (!dictionary.TryGet(NameToken.FontDescriptor, pdfScanner, out dictionary2))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(67, 1);
				defaultInterpolatedStringHandler.AppendLiteral("No font descriptor indirect reference found in the TrueType font: ");
				defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(dictionary);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new InvalidFontFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return FontDescriptorFactory.Generate(dictionary2, pdfScanner);
		}

		// Token: 0x060002BA RID: 698 RVA: 0x00011E8C File Offset: 0x0001008C
		public static NameToken GetName(IPdfTokenScanner pdfScanner, DictionaryToken dictionary, FontDescriptor descriptor)
		{
			IToken token;
			if (dictionary.TryGet(NameToken.BaseFont, ref token))
			{
				return DirectObjectFinder.Get<NameToken>(token, pdfScanner);
			}
			if (descriptor.FontName != null)
			{
				return descriptor.FontName;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(37, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Could not find a name for this font ");
			defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(dictionary);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			throw new InvalidFontFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
		}
	}
}
