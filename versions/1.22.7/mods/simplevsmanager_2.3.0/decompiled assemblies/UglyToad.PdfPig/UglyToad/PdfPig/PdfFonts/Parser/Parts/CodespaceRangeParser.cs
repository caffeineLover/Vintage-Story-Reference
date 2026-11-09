using System;
using System.Collections.Generic;
using UglyToad.PdfPig.PdfFonts.Cmap;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.PdfFonts.Parser.Parts
{
	// Token: 0x020000A5 RID: 165
	internal class CodespaceRangeParser : ICidFontPartParser<NumericToken>
	{
		// Token: 0x060002D2 RID: 722 RVA: 0x00012FB4 File Offset: 0x000111B4
		public void Parse(NumericToken numeric, ITokenScanner tokenScanner, CharacterMapBuilder builder)
		{
			List<CodespaceRange> list = new List<CodespaceRange>(numeric.Int);
			int i = 0;
			while (i < numeric.Int)
			{
				if (!tokenScanner.MoveNext())
				{
					throw new InvalidOperationException("Codespace range have reach an unexpected end");
				}
				OperatorToken operatorToken = tokenScanner.CurrentToken as OperatorToken;
				if (operatorToken != null && operatorToken.Data == "endcodespacerange")
				{
					break;
				}
				HexToken hexToken = tokenScanner.CurrentToken as HexToken;
				if (hexToken == null)
				{
					string text = "Codespace range contains an unexpected token: ";
					IToken currentToken = tokenScanner.CurrentToken;
					throw new InvalidOperationException(text + ((currentToken != null) ? currentToken.ToString() : null));
				}
				if (tokenScanner.MoveNext())
				{
					HexToken hexToken2 = tokenScanner.CurrentToken as HexToken;
					if (hexToken2 != null)
					{
						list.Add(new CodespaceRange(hexToken.Bytes, hexToken2.Bytes));
						i++;
						continue;
					}
				}
				string text2 = "Codespace range contains an unexpected token: ";
				IToken currentToken2 = tokenScanner.CurrentToken;
				throw new InvalidOperationException(text2 + ((currentToken2 != null) ? currentToken2.ToString() : null));
			}
			builder.CodespaceRanges = list;
		}
	}
}
