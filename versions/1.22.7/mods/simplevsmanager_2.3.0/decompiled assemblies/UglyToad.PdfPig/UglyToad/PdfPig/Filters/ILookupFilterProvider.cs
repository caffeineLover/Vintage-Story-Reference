using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Filters
{
	// Token: 0x020001C5 RID: 453
	internal interface ILookupFilterProvider : IFilterProvider
	{
		// Token: 0x06000ACE RID: 2766
		IReadOnlyList<IFilter> GetFilters(DictionaryToken dictionary, IPdfTokenScanner scanner);
	}
}
