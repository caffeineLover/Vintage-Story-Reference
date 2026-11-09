using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Filters;
using UglyToad.PdfPig.Fonts;
using UglyToad.PdfPig.Fonts.Encodings;
using UglyToad.PdfPig.Parser.Parts;
using UglyToad.PdfPig.PdfFonts.Cmap;
using UglyToad.PdfPig.PdfFonts.Simple;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util;

namespace UglyToad.PdfPig.PdfFonts.Parser.Handlers
{
	// Token: 0x020000AC RID: 172
	internal class Type3FontHandler : IFontHandler
	{
		// Token: 0x060002EB RID: 747 RVA: 0x000141D0 File Offset: 0x000123D0
		public Type3FontHandler(IPdfTokenScanner scanner, ILookupFilterProvider filterProvider, IEncodingReader encodingReader)
		{
			this.filterProvider = filterProvider;
			this.encodingReader = encodingReader;
			this.scanner = scanner;
		}

		// Token: 0x060002EC RID: 748 RVA: 0x000141F0 File Offset: 0x000123F0
		public IFont Generate(DictionaryToken dictionary)
		{
			PdfRectangle boundingBox = Type3FontHandler.GetBoundingBox(dictionary);
			TransformationMatrix fontMatrix = this.GetFontMatrix(dictionary);
			if (boundingBox.Left == 0.0 && boundingBox.Bottom == 0.0 && boundingBox.Height == 0.0 && boundingBox.Width == 0.0 && fontMatrix.A != 0.0 && fontMatrix.D != 0.0)
			{
				boundingBox..ctor(0.0, 0.0, 1.0 / fontMatrix.A, 1.0 / fontMatrix.D);
			}
			int firstCharacter = FontDictionaryAccessHelper.GetFirstCharacter(dictionary);
			int lastCharacter = FontDictionaryAccessHelper.GetLastCharacter(dictionary);
			double[] widths = FontDictionaryAccessHelper.GetWidths(this.scanner, dictionary);
			Encoding encoding = this.encodingReader.Read(dictionary, null, null);
			CMap toUnicodeCMap = null;
			IToken token;
			if (dictionary.TryGet(NameToken.ToUnicode, ref token))
			{
				StreamToken streamToken = DirectObjectFinder.Get<StreamToken>(token, this.scanner);
				IReadOnlyList<byte> readOnlyList = (streamToken != null) ? streamToken.Decode(this.filterProvider, this.scanner) : null;
				if (readOnlyList != null)
				{
					toUnicodeCMap = CMapCache.Parse(new ByteArrayInputBytes(readOnlyList));
				}
			}
			return new Type3Font(this.GetFontName(dictionary), boundingBox, fontMatrix, encoding, firstCharacter, lastCharacter, widths, toUnicodeCMap);
		}

		// Token: 0x060002ED RID: 749 RVA: 0x00014340 File Offset: 0x00012540
		private NameToken GetFontName(DictionaryToken dictionary)
		{
			NameToken result;
			if (dictionary.TryGet(NameToken.Name, this.scanner, out result))
			{
				return result;
			}
			return NameToken.Type3;
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0001436C File Offset: 0x0001256C
		private TransformationMatrix GetFontMatrix(DictionaryToken dictionary)
		{
			IToken token;
			if (!dictionary.TryGet(NameToken.FontMatrix, ref token))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(23, 1);
				defaultInterpolatedStringHandler.AppendLiteral("No font matrix found: ");
				defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(dictionary);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new InvalidFontFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			ArrayToken array = DirectObjectFinder.Get<ArrayToken>(token, this.scanner);
			return TransformationMatrix.FromValues(array.GetNumeric(0).Double, array.GetNumeric(1).Double, array.GetNumeric(2).Double, array.GetNumeric(3).Double, array.GetNumeric(4).Double, array.GetNumeric(5).Double);
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0001441C File Offset: 0x0001261C
		private static PdfRectangle GetBoundingBox(DictionaryToken dictionary)
		{
			IToken token;
			if (!dictionary.TryGet(NameToken.FontBbox, ref token))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(48, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Type 3 font was invalid. No Font Bounding Box: ");
				defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(dictionary);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new InvalidFontFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			ArrayToken arrayToken = token as ArrayToken;
			if (arrayToken != null)
			{
				return new PdfRectangle(arrayToken.GetNumeric(0).Double, arrayToken.GetNumeric(1).Double, arrayToken.GetNumeric(2).Double, arrayToken.GetNumeric(3).Double);
			}
			return new PdfRectangle(0, 0, 0, 0);
		}

		// Token: 0x04000190 RID: 400
		private readonly ILookupFilterProvider filterProvider;

		// Token: 0x04000191 RID: 401
		private readonly IEncodingReader encodingReader;

		// Token: 0x04000192 RID: 402
		private readonly IPdfTokenScanner scanner;
	}
}
