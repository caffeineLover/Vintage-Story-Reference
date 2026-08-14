using System;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.PdfFonts.Parser.Handlers
{
	// Token: 0x020000A8 RID: 168
	internal interface IFontHandler
	{
		// Token: 0x060002DE RID: 734
		IFont Generate(DictionaryToken dictionary);
	}
}
