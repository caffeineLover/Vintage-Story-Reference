using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Filters
{
	// Token: 0x020001C4 RID: 452
	public interface IFilterProvider
	{
		// Token: 0x06000ACB RID: 2763
		IReadOnlyList<IFilter> GetFilters(DictionaryToken dictionary);

		// Token: 0x06000ACC RID: 2764
		IReadOnlyList<IFilter> GetNamedFilters(IReadOnlyList<NameToken> names);

		// Token: 0x06000ACD RID: 2765
		IReadOnlyList<IFilter> GetAllFilters();
	}
}
