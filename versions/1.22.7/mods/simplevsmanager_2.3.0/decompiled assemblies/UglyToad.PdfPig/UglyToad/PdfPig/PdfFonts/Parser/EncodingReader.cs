using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Fonts;
using UglyToad.PdfPig.Fonts.Encodings;
using UglyToad.PdfPig.Parser.Parts;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.PdfFonts.Parser
{
	// Token: 0x0200009C RID: 156
	internal class EncodingReader : IEncodingReader
	{
		// Token: 0x060002B1 RID: 689 RVA: 0x00011A26 File Offset: 0x0000FC26
		public EncodingReader(IPdfTokenScanner pdfScanner)
		{
			this.pdfScanner = pdfScanner;
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x00011A38 File Offset: 0x0000FC38
		public Encoding Read(DictionaryToken fontDictionary, FontDescriptor descriptor = null, Encoding fontEncoding = null)
		{
			IToken token;
			if (!fontDictionary.TryGet(NameToken.Encoding, ref token))
			{
				return null;
			}
			NameToken encodingName;
			if (DirectObjectFinder.TryGet<NameToken>(token, this.pdfScanner, out encodingName))
			{
				Encoding result;
				if (EncodingReader.TryGetNamedEncoding(descriptor, encodingName, out result))
				{
					return result;
				}
				NameToken nameToken;
				if (fontDictionary.TryGet(NameToken.BaseFont, this.pdfScanner, out nameToken))
				{
					if (string.Equals(nameToken.Data, "ZapfDingbats", 5))
					{
						return ZapfDingbatsEncoding.Instance;
					}
					if (string.Equals(nameToken.Data, "Symbol", 5))
					{
						return SymbolEncoding.Instance;
					}
					return WinAnsiEncoding.Instance;
				}
			}
			DictionaryToken encodingDictionary = DirectObjectFinder.Get<DictionaryToken>(token, this.pdfScanner);
			return this.ReadEncodingDictionary(encodingDictionary, fontEncoding);
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x00011AD8 File Offset: 0x0000FCD8
		private Encoding ReadEncodingDictionary(DictionaryToken encodingDictionary, Encoding fontEncoding)
		{
			if (encodingDictionary == null)
			{
				return null;
			}
			IToken token;
			Encoding encoding;
			if (encodingDictionary.TryGet(NameToken.BaseEncoding, ref token))
			{
				NameToken nameToken = token as NameToken;
				if (nameToken != null)
				{
					if (!Encoding.TryGetNamedEncoding(nameToken, ref encoding))
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
						defaultInterpolatedStringHandler..ctor(53, 1);
						defaultInterpolatedStringHandler.AppendLiteral("No encoding found with name ");
						defaultInterpolatedStringHandler.AppendFormatted<NameToken>(nameToken);
						defaultInterpolatedStringHandler.AppendLiteral(" to use as base encoding.");
						throw new InvalidFontFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					goto IL_6A;
				}
			}
			encoding = (fontEncoding ?? StandardEncoding.Instance);
			IL_6A:
			IToken token2;
			if (!encodingDictionary.TryGet(NameToken.Differences, ref token2))
			{
				return encoding;
			}
			IReadOnlyList<ValueTuple<int, string>> readOnlyList = EncodingReader.ProcessDifferences(DirectObjectFinder.Get<ArrayToken>(token2, this.pdfScanner));
			return new DifferenceBasedEncoding(encoding, readOnlyList);
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x00011B7C File Offset: 0x0000FD7C
		private static IReadOnlyList<ValueTuple<int, string>> ProcessDifferences(ArrayToken differenceArray)
		{
			List<ValueTuple<int, string>> list = new List<ValueTuple<int, string>>();
			if (differenceArray.Length == 0)
			{
				return list;
			}
			int num = -1;
			foreach (IToken token in differenceArray.Data)
			{
				NumericToken numericToken = token as NumericToken;
				if (numericToken != null)
				{
					num = numericToken.Int;
				}
				else
				{
					NameToken nameToken = token as NameToken;
					if (nameToken == null)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(44, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unexpected entry in the differences array: ");
						defaultInterpolatedStringHandler.AppendFormatted<ArrayToken>(differenceArray);
						defaultInterpolatedStringHandler.AppendLiteral(".");
						throw new InvalidFontFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					list.Add(new ValueTuple<int, string>(num, nameToken.Data));
					num++;
				}
			}
			return list;
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x00011C4C File Offset: 0x0000FE4C
		private static bool TryGetNamedEncoding(FontDescriptor descriptor, NameToken encodingName, out Encoding encoding)
		{
			encoding = null;
			if (descriptor != null && descriptor.Flags.HasFlag(FontDescriptorFlags.Symbolic))
			{
				encoding = StandardEncoding.Instance;
			}
			return Encoding.TryGetNamedEncoding(encodingName, ref encoding);
		}

		// Token: 0x04000180 RID: 384
		private readonly IPdfTokenScanner pdfScanner;
	}
}
