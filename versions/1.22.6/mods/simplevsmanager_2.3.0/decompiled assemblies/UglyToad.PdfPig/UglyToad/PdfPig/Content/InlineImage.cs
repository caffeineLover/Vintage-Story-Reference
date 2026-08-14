using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Filters;
using UglyToad.PdfPig.Graphics.Colors;
using UglyToad.PdfPig.Graphics.Core;
using UglyToad.PdfPig.Images.Png;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.Content
{
	// Token: 0x020001E4 RID: 484
	public class InlineImage : IPdfImage
	{
		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000B7E RID: 2942 RVA: 0x00039C7C File Offset: 0x00037E7C
		public PdfRectangle Bounds { get; }

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000B7F RID: 2943 RVA: 0x00039C84 File Offset: 0x00037E84
		public int WidthInSamples { get; }

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000B80 RID: 2944 RVA: 0x00039C8C File Offset: 0x00037E8C
		public int HeightInSamples { get; }

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000B81 RID: 2945 RVA: 0x00039C94 File Offset: 0x00037E94
		public int BitsPerComponent { get; }

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06000B82 RID: 2946 RVA: 0x00039C9C File Offset: 0x00037E9C
		public bool IsImageMask { get; }

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06000B83 RID: 2947 RVA: 0x00039CA4 File Offset: 0x00037EA4
		public IReadOnlyList<decimal> Decode { get; }

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06000B84 RID: 2948 RVA: 0x00039CAC File Offset: 0x00037EAC
		public bool IsInlineImage { get; } = 1;

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06000B85 RID: 2949 RVA: 0x00039CB4 File Offset: 0x00037EB4
		[NotNull]
		public DictionaryToken ImageDictionary { get; }

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06000B86 RID: 2950 RVA: 0x00039CBC File Offset: 0x00037EBC
		public RenderingIntent RenderingIntent { get; }

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06000B87 RID: 2951 RVA: 0x00039CC4 File Offset: 0x00037EC4
		public bool Interpolate { get; }

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06000B88 RID: 2952 RVA: 0x00039CCC File Offset: 0x00037ECC
		public IReadOnlyList<byte> RawBytes { get; }

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06000B89 RID: 2953 RVA: 0x00039CD4 File Offset: 0x00037ED4
		public ColorSpaceDetails ColorSpaceDetails { get; }

		// Token: 0x06000B8A RID: 2954 RVA: 0x00039CDC File Offset: 0x00037EDC
		internal InlineImage(PdfRectangle bounds, int widthInSamples, int heightInSamples, int bitsPerComponent, bool isImageMask, RenderingIntent renderingIntent, bool interpolate, IReadOnlyList<decimal> decode, IReadOnlyList<byte> bytes, IReadOnlyList<IFilter> filters, DictionaryToken streamDictionary, ColorSpaceDetails colorSpaceDetails)
		{
			this.Bounds = bounds;
			this.WidthInSamples = widthInSamples;
			this.HeightInSamples = heightInSamples;
			this.Decode = decode;
			this.BitsPerComponent = bitsPerComponent;
			this.IsImageMask = isImageMask;
			this.RenderingIntent = renderingIntent;
			this.Interpolate = interpolate;
			this.ImageDictionary = streamDictionary;
			this.RawBytes = bytes;
			this.ColorSpaceDetails = colorSpaceDetails;
			bool flag = true;
			using (IEnumerator<IFilter> enumerator = filters.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.IsSupported)
					{
						flag = false;
						break;
					}
				}
			}
			this.bytesFactory = (flag ? new Lazy<IReadOnlyList<byte>>(delegate()
			{
				byte[] array = Enumerable.ToArray<byte>(bytes);
				for (int i = 0; i < filters.Count; i++)
				{
					array = filters[i].Decode(array, streamDictionary, i);
				}
				return array;
			}) : null);
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x00039DD4 File Offset: 0x00037FD4
		public bool TryGetBytes(out IReadOnlyList<byte> bytes)
		{
			bytes = null;
			if (this.bytesFactory == null)
			{
				return false;
			}
			bytes = this.bytesFactory.Value;
			return true;
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x00039DF1 File Offset: 0x00037FF1
		public bool TryGetPng(out byte[] bytes)
		{
			return PngFromPdfImageFactory.TryGenerate(this, out bytes);
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x00039DFC File Offset: 0x00037FFC
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(21, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Inline Image (w ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(this.Bounds.Width);
			defaultInterpolatedStringHandler.AppendLiteral(", h ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(this.Bounds.Height);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x040005F5 RID: 1525
		private readonly Lazy<IReadOnlyList<byte>> bytesFactory;
	}
}
