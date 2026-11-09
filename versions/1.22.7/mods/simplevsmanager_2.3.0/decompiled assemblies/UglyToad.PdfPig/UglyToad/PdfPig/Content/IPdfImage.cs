using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Graphics.Colors;
using UglyToad.PdfPig.Graphics.Core;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Content
{
	// Token: 0x020001E6 RID: 486
	public interface IPdfImage
	{
		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06000B8F RID: 2959
		PdfRectangle Bounds { get; }

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06000B90 RID: 2960
		int WidthInSamples { get; }

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06000B91 RID: 2961
		int HeightInSamples { get; }

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000B92 RID: 2962
		int BitsPerComponent { get; }

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06000B93 RID: 2963
		IReadOnlyList<byte> RawBytes { get; }

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06000B94 RID: 2964
		RenderingIntent RenderingIntent { get; }

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06000B95 RID: 2965
		bool IsImageMask { get; }

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06000B96 RID: 2966
		IReadOnlyList<decimal> Decode { get; }

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06000B97 RID: 2967
		bool Interpolate { get; }

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06000B98 RID: 2968
		bool IsInlineImage { get; }

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06000B99 RID: 2969
		DictionaryToken ImageDictionary { get; }

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06000B9A RID: 2970
		ColorSpaceDetails ColorSpaceDetails { get; }

		// Token: 0x06000B9B RID: 2971
		bool TryGetBytes(out IReadOnlyList<byte> bytes);

		// Token: 0x06000B9C RID: 2972
		bool TryGetPng(out byte[] bytes);
	}
}
