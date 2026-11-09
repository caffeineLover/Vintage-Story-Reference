using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Filters
{
	// Token: 0x020001C3 RID: 451
	public interface IFilter
	{
		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000AC9 RID: 2761
		bool IsSupported { get; }

		// Token: 0x06000ACA RID: 2762
		byte[] Decode(IReadOnlyList<byte> input, DictionaryToken streamDictionary, int filterIndex);
	}
}
