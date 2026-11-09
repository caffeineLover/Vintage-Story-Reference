using System;
using System.Collections.Generic;
using UglyToad.PdfPig.PdfFonts.Cmap;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.PdfFonts.Parser.Parts
{
	// Token: 0x020000A1 RID: 161
	internal class CidCharacterParser : ICidFontPartParser<NumericToken>
	{
		// Token: 0x060002C2 RID: 706 RVA: 0x00012300 File Offset: 0x00010500
		public void Parse(NumericToken numeric, ITokenScanner scanner, CharacterMapBuilder builder)
		{
			List<CidCharacterMapping> list = new List<CidCharacterMapping>();
			for (int i = 0; i < numeric.Int; i++)
			{
				HexToken hexToken;
				if (!scanner.TryReadToken<HexToken>(ref hexToken))
				{
					string text = "The first token in a line for Cid Characters should be a hex, instead it was: ";
					IToken currentToken = scanner.CurrentToken;
					throw new InvalidOperationException(text + ((currentToken != null) ? currentToken.ToString() : null));
				}
				NumericToken numericToken;
				if (!scanner.TryReadToken<NumericToken>(ref numericToken))
				{
					string text2 = "The destination token in a line for Cid Character should be an integer, instead it was: ";
					IToken currentToken2 = scanner.CurrentToken;
					throw new InvalidOperationException(text2 + ((currentToken2 != null) ? currentToken2.ToString() : null));
				}
				int sourceCharacterCode = hexToken.Bytes.ToInt(hexToken.Bytes.Count);
				CidCharacterMapping cidCharacterMapping = new CidCharacterMapping(sourceCharacterCode, numericToken.Int);
				list.Add(cidCharacterMapping);
			}
			builder.CidCharacterMappings = list;
		}
	}
}
