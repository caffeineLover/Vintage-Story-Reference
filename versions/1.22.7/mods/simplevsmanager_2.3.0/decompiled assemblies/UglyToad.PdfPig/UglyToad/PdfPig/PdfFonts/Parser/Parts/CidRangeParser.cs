using System;
using UglyToad.PdfPig.Fonts;
using UglyToad.PdfPig.PdfFonts.Cmap;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.PdfFonts.Parser.Parts
{
	// Token: 0x020000A4 RID: 164
	internal class CidRangeParser : ICidFontPartParser<NumericToken>
	{
		// Token: 0x060002D0 RID: 720 RVA: 0x00012F24 File Offset: 0x00011124
		public void Parse(NumericToken numeric, ITokenScanner scanner, CharacterMapBuilder builder)
		{
			for (int i = 0; i < numeric.Int; i++)
			{
				HexToken hexToken;
				if (!scanner.TryReadToken<HexToken>(ref hexToken))
				{
					throw new InvalidFontFormatException("Could not find the starting hex token for the CIDRange in this font.");
				}
				HexToken hexToken2;
				if (!scanner.TryReadToken<HexToken>(ref hexToken2))
				{
					throw new InvalidFontFormatException("Could not find the end hex token for the CIDRange in this font.");
				}
				NumericToken numericToken;
				if (!scanner.TryReadToken<NumericToken>(ref numericToken))
				{
					throw new InvalidFontFormatException("Could not find the starting CID numeric token for the CIDRange in this font.");
				}
				int firstCharacterCode = HexToken.ConvertHexBytesToInt(hexToken);
				int lastCharacterCode = HexToken.ConvertHexBytesToInt(hexToken2);
				CidRange range = new CidRange(firstCharacterCode, lastCharacterCode, numericToken.Int);
				builder.AddCidRange(range);
			}
		}
	}
}
