using System;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.PdfFonts
{
	// Token: 0x02000094 RID: 148
	internal interface IFontFactory
	{
		// Token: 0x0600026A RID: 618
		IFont Get(DictionaryToken dictionary);
	}
}
