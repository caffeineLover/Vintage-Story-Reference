using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Fonts;
using UglyToad.PdfPig.PdfFonts.Cmap;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.PdfFonts.Parser.Parts
{
	// Token: 0x020000A0 RID: 160
	internal class BaseFontRangeParser : ICidFontPartParser<NumericToken>
	{
		// Token: 0x060002BE RID: 702 RVA: 0x00012094 File Offset: 0x00010294
		public void Parse(NumericToken numberOfOperations, ITokenScanner scanner, CharacterMapBuilder builder)
		{
			for (int i = 0; i < numberOfOperations.Int; i++)
			{
				HexToken hexToken;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (!scanner.TryReadToken<HexToken>(ref hexToken))
				{
					defaultInterpolatedStringHandler..ctor(41, 1);
					defaultInterpolatedStringHandler.AppendLiteral("bfrange was missing the low source code: ");
					defaultInterpolatedStringHandler.AppendFormatted<IToken>(scanner.CurrentToken);
					throw new InvalidFontFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				HexToken hexToken2;
				if (!scanner.TryReadToken<HexToken>(ref hexToken2))
				{
					defaultInterpolatedStringHandler..ctor(42, 1);
					defaultInterpolatedStringHandler.AppendLiteral("bfrange was missing the high source code: ");
					defaultInterpolatedStringHandler.AppendFormatted<IToken>(scanner.CurrentToken);
					throw new InvalidFontFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				if (!scanner.MoveNext())
				{
					throw new InvalidFontFormatException("bfrange ended unexpectedly after the high source code.");
				}
				List<byte> list = null;
				ArrayToken arrayToken = null;
				IToken currentToken = scanner.CurrentToken;
				ArrayToken arrayToken2 = currentToken as ArrayToken;
				if (arrayToken2 == null)
				{
					HexToken hexToken3 = currentToken as HexToken;
					if (hexToken3 == null)
					{
						if (!(currentToken is NumericToken))
						{
							throw new InvalidOperationException();
						}
						throw new NotImplementedException("From the spec it seems this possible but the meaning is unclear...");
					}
					else
					{
						list = Enumerable.ToList<byte>(hexToken3.Bytes);
					}
				}
				else
				{
					arrayToken = arrayToken2;
				}
				bool flag = false;
				List<byte> list2 = new List<byte>(hexToken.Bytes);
				IReadOnlyList<byte> bytes = hexToken2.Bytes;
				if (arrayToken != null)
				{
					int num = 0;
					while (!flag)
					{
						if (BaseFontRangeParser.Compare(list2, bytes) >= 0)
						{
							flag = true;
						}
						IToken token = arrayToken.Data[num];
						NameToken nameToken = token as NameToken;
						if (nameToken != null)
						{
							builder.AddBaseFontCharacter(list2, nameToken.Data);
						}
						else
						{
							HexToken hexToken4 = token as HexToken;
							if (hexToken4 != null)
							{
								builder.AddBaseFontCharacter(list2, hexToken4.Bytes);
							}
						}
						BaseFontRangeParser.Increment(list2, list2.Count - 1);
						num++;
					}
				}
				else
				{
					while (!flag)
					{
						if (BaseFontRangeParser.Compare(list2, bytes) >= 0)
						{
							flag = true;
						}
						builder.AddBaseFontCharacter(list2, list);
						BaseFontRangeParser.Increment(list2, list2.Count - 1);
						BaseFontRangeParser.Increment(list, list.Count - 1);
					}
				}
			}
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0001226A File Offset: 0x0001046A
		private static void Increment(IList<byte> data, int position)
		{
			if (position > 0 && (data[position] & 255) == 255)
			{
				data[position] = 0;
				BaseFontRangeParser.Increment(data, position - 1);
				return;
			}
			data[position] += 1;
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x000122A8 File Offset: 0x000104A8
		private static int Compare(IReadOnlyList<byte> first, IReadOnlyList<byte> second)
		{
			int i = 0;
			while (i < first.Count)
			{
				if (first[i] != second[i])
				{
					if ((first[i] & 255) < (second[i] & 255))
					{
						return -1;
					}
					return 1;
				}
				else
				{
					i++;
				}
			}
			return 0;
		}
	}
}
