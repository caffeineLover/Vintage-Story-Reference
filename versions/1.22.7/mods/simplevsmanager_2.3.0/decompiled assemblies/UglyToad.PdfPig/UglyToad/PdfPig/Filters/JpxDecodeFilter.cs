using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Filters
{
	// Token: 0x020001C7 RID: 455
	internal class JpxDecodeFilter : IFilter
	{
		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000AD2 RID: 2770 RVA: 0x0003692B File Offset: 0x00034B2B
		public bool IsSupported { get; }

		// Token: 0x06000AD3 RID: 2771 RVA: 0x00036933 File Offset: 0x00034B33
		public byte[] Decode(IReadOnlyList<byte> input, DictionaryToken streamDictionary, int filterIndex)
		{
			throw new NotSupportedException("The JPX Filter (JPEG2000) for image data is not currently supported. Try accessing the raw compressed data directly.");
		}
	}
}
