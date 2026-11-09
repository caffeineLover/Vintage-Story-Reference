using System;
using UglyToad.PdfPig.Fonts.Encodings;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.PdfFonts.Parser
{
	// Token: 0x0200009E RID: 158
	internal interface IEncodingReader
	{
		// Token: 0x060002BB RID: 699
		Encoding Read(DictionaryToken fontDictionary, FontDescriptor descriptor = null, Encoding fontEncoding = null);
	}
}
