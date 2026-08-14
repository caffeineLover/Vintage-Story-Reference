using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Filters
{
	// Token: 0x020001BE RID: 446
	internal class DctDecodeFilter : IFilter
	{
		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000AB5 RID: 2741 RVA: 0x00036100 File Offset: 0x00034300
		public bool IsSupported { get; }

		// Token: 0x06000AB6 RID: 2742 RVA: 0x00036108 File Offset: 0x00034308
		public byte[] Decode(IReadOnlyList<byte> input, DictionaryToken streamDictionary, int filterIndex)
		{
			throw new NotSupportedException("The DST (Discrete Cosine Transform) Filter indicates data is encoded in JPEG format. This filter is not currently supported but the raw data can be supplied to JPEG supporting libraries.");
		}
	}
}
