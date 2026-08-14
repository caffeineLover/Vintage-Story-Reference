using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Filters
{
	// Token: 0x020001C6 RID: 454
	internal class Jbig2DecodeFilter : IFilter
	{
		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000ACF RID: 2767 RVA: 0x0003690F File Offset: 0x00034B0F
		public bool IsSupported { get; }

		// Token: 0x06000AD0 RID: 2768 RVA: 0x00036917 File Offset: 0x00034B17
		public byte[] Decode(IReadOnlyList<byte> input, DictionaryToken streamDictionary, int filterIndex)
		{
			throw new NotSupportedException("The JBIG2 Filter for monochrome image data is not currently supported. Try accessing the raw compressed data directly.");
		}
	}
}
