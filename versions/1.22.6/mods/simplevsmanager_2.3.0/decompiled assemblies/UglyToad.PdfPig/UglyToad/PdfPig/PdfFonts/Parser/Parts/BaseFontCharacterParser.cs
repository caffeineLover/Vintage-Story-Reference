using System;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.PdfFonts.Cmap;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.PdfFonts.Parser.Parts
{
	// Token: 0x0200009F RID: 159
	internal class BaseFontCharacterParser : ICidFontPartParser<NumericToken>
	{
		// Token: 0x060002BC RID: 700 RVA: 0x00011EFC File Offset: 0x000100FC
		public void Parse(NumericToken numeric, ITokenScanner tokenScanner, CharacterMapBuilder builder)
		{
			int i = 0;
			while (i < numeric.Int)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (tokenScanner.MoveNext())
				{
					HexToken hexToken = tokenScanner.CurrentToken as HexToken;
					if (hexToken != null)
					{
						if (!tokenScanner.MoveNext())
						{
							defaultInterpolatedStringHandler..ctor(65, 2);
							defaultInterpolatedStringHandler.AppendLiteral("Base font characters definition contains invalid item at index ");
							defaultInterpolatedStringHandler.AppendFormatted<int>(i);
							defaultInterpolatedStringHandler.AppendLiteral(": ");
							defaultInterpolatedStringHandler.AppendFormatted<IToken>(tokenScanner.CurrentToken);
							throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						NameToken nameToken = tokenScanner.CurrentToken as NameToken;
						if (nameToken != null)
						{
							builder.AddBaseFontCharacter(hexToken.Bytes, nameToken.Data);
						}
						else
						{
							HexToken hexToken2 = tokenScanner.CurrentToken as HexToken;
							if (hexToken2 == null)
							{
								defaultInterpolatedStringHandler..ctor(65, 2);
								defaultInterpolatedStringHandler.AppendLiteral("Base font characters definition contains invalid item at index ");
								defaultInterpolatedStringHandler.AppendFormatted<int>(i);
								defaultInterpolatedStringHandler.AppendLiteral(": ");
								defaultInterpolatedStringHandler.AppendFormatted<IToken>(tokenScanner.CurrentToken);
								throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
							}
							builder.AddBaseFontCharacter(hexToken.Bytes, hexToken2.Bytes);
						}
						i++;
						continue;
					}
				}
				OperatorToken operatorToken = tokenScanner.CurrentToken as OperatorToken;
				if (operatorToken != null && (string.Equals(operatorToken.Data, "endbfchar", 5) || string.Equals(operatorToken.Data, "endcmap", 5)))
				{
					return;
				}
				defaultInterpolatedStringHandler..ctor(65, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Base font characters definition contains invalid item at index ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(i);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted<IToken>(tokenScanner.CurrentToken);
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}
	}
}
