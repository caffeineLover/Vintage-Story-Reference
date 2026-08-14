using System;
using UglyToad.PdfPig.PdfFonts.Cmap;
using UglyToad.PdfPig.Tokenization.Scanner;

namespace UglyToad.PdfPig.PdfFonts.Parser.Parts
{
	// Token: 0x020000A7 RID: 167
	internal interface ICidFontPartParser<in TToken>
	{
		// Token: 0x060002DD RID: 733
		void Parse(TToken previous, ITokenScanner tokenScanner, CharacterMapBuilder builder);
	}
}
