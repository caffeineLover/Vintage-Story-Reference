using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Graphics.Colors;
using UglyToad.PdfPig.Graphics.Core;
using UglyToad.PdfPig.Images.Png;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.XObjects
{
	// Token: 0x0200000B RID: 11
	public class XObjectImage : IPdfImage
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00002D8C File Offset: 0x00000F8C
		public PdfRectangle Bounds { get; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00002D94 File Offset: 0x00000F94
		public int WidthInSamples { get; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00002D9C File Offset: 0x00000F9C
		public int HeightInSamples { get; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00002DA4 File Offset: 0x00000FA4
		public int BitsPerComponent { get; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00002DAC File Offset: 0x00000FAC
		public bool IsJpxEncoded { get; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00002DB4 File Offset: 0x00000FB4
		public RenderingIntent RenderingIntent { get; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00002DBC File Offset: 0x00000FBC
		public bool IsImageMask { get; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00002DC4 File Offset: 0x00000FC4
		public IReadOnlyList<decimal> Decode { get; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00002DCC File Offset: 0x00000FCC
		public bool Interpolate { get; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00002DD4 File Offset: 0x00000FD4
		public bool IsInlineImage { get; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00002DDC File Offset: 0x00000FDC
		[NotNull]
		public DictionaryToken ImageDictionary { get; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00002DE4 File Offset: 0x00000FE4
		public IReadOnlyList<byte> RawBytes { get; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00002DEC File Offset: 0x00000FEC
		public ColorSpaceDetails ColorSpaceDetails { get; }

		// Token: 0x06000047 RID: 71 RVA: 0x00002DF4 File Offset: 0x00000FF4
		internal XObjectImage(PdfRectangle bounds, int widthInSamples, int heightInSamples, int bitsPerComponent, bool isJpxEncoded, bool isImageMask, RenderingIntent renderingIntent, bool interpolate, IReadOnlyList<decimal> decode, DictionaryToken imageDictionary, IReadOnlyList<byte> rawBytes, Lazy<IReadOnlyList<byte>> bytes, ColorSpaceDetails colorSpaceDetails)
		{
			this.Bounds = bounds;
			this.WidthInSamples = widthInSamples;
			this.HeightInSamples = heightInSamples;
			this.BitsPerComponent = bitsPerComponent;
			this.IsJpxEncoded = isJpxEncoded;
			this.IsImageMask = isImageMask;
			this.RenderingIntent = renderingIntent;
			this.Interpolate = interpolate;
			this.Decode = decode;
			if (imageDictionary == null)
			{
				throw new ArgumentNullException("imageDictionary");
			}
			this.ImageDictionary = imageDictionary;
			this.RawBytes = rawBytes;
			this.ColorSpaceDetails = colorSpaceDetails;
			this.bytesFactory = bytes;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002E7B File Offset: 0x0000107B
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

		// Token: 0x06000049 RID: 73 RVA: 0x00002E98 File Offset: 0x00001098
		public bool TryGetPng(out byte[] bytes)
		{
			return PngFromPdfImageFactory.TryGenerate(this, out bytes);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002EA4 File Offset: 0x000010A4
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(24, 3);
			defaultInterpolatedStringHandler.AppendLiteral("XObject Image (w ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(this.Bounds.Width);
			defaultInterpolatedStringHandler.AppendLiteral(", h ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(this.Bounds.Height);
			defaultInterpolatedStringHandler.AppendLiteral("): ");
			defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(this.ImageDictionary);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04000024 RID: 36
		[CanBeNull]
		private readonly Lazy<IReadOnlyList<byte>> bytesFactory;
	}
}
