using System;
using UglyToad.PdfPig.Outline;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Content
{
	// Token: 0x020001E5 RID: 485
	internal interface IPageFactory
	{
		// Token: 0x06000B8E RID: 2958
		Page Create(int number, DictionaryToken dictionary, PageTreeMembers pageTreeMembers, NamedDestinations annotationProvider, InternalParsingOptions parsingOptions);
	}
}
