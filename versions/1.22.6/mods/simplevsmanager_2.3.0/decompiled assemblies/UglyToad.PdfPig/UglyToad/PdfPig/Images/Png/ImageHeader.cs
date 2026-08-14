using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Images.Png
{
	// Token: 0x020000F7 RID: 247
	internal readonly struct ImageHeader
	{
		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060004A0 RID: 1184 RVA: 0x0001BD95 File Offset: 0x00019F95
		public int Width { get; }

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060004A1 RID: 1185 RVA: 0x0001BD9D File Offset: 0x00019F9D
		public int Height { get; }

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x060004A2 RID: 1186 RVA: 0x0001BDA5 File Offset: 0x00019FA5
		public byte BitDepth { get; }

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060004A3 RID: 1187 RVA: 0x0001BDAD File Offset: 0x00019FAD
		public ColorType ColorType { get; }

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060004A4 RID: 1188 RVA: 0x0001BDB5 File Offset: 0x00019FB5
		public CompressionMethod CompressionMethod { get; }

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060004A5 RID: 1189 RVA: 0x0001BDBD File Offset: 0x00019FBD
		public FilterMethod FilterMethod { get; }

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060004A6 RID: 1190 RVA: 0x0001BDC5 File Offset: 0x00019FC5
		public InterlaceMethod InterlaceMethod { get; }

		// Token: 0x060004A7 RID: 1191 RVA: 0x0001BDD0 File Offset: 0x00019FD0
		public ImageHeader(int width, int height, byte bitDepth, ColorType colorType, CompressionMethod compressionMethod, FilterMethod filterMethod, InterlaceMethod interlaceMethod)
		{
			if (width == 0)
			{
				throw new ArgumentOutOfRangeException("width", "Invalid width (0) for image.");
			}
			if (height == 0)
			{
				throw new ArgumentOutOfRangeException("height", "Invalid height (0) for image.");
			}
			HashSet<byte> hashSet;
			if (!ImageHeader.PermittedBitDepths.TryGetValue(colorType, ref hashSet) || !hashSet.Contains(bitDepth))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(48, 2);
				defaultInterpolatedStringHandler.AppendLiteral("The bit depth ");
				defaultInterpolatedStringHandler.AppendFormatted<byte>(bitDepth);
				defaultInterpolatedStringHandler.AppendLiteral(" is not permitted for color type ");
				defaultInterpolatedStringHandler.AppendFormatted<ColorType>(colorType);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.Width = width;
			this.Height = height;
			this.BitDepth = bitDepth;
			this.ColorType = colorType;
			this.CompressionMethod = compressionMethod;
			this.FilterMethod = filterMethod;
			this.InterlaceMethod = interlaceMethod;
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x0001BEA0 File Offset: 0x0001A0A0
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(72, 7);
			defaultInterpolatedStringHandler.AppendLiteral("w: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Width);
			defaultInterpolatedStringHandler.AppendLiteral(", h: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Height);
			defaultInterpolatedStringHandler.AppendLiteral(", bitDepth: ");
			defaultInterpolatedStringHandler.AppendFormatted<byte>(this.BitDepth);
			defaultInterpolatedStringHandler.AppendLiteral(", colorType: ");
			defaultInterpolatedStringHandler.AppendFormatted<ColorType>(this.ColorType);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendLiteral("compression: ");
			defaultInterpolatedStringHandler.AppendFormatted<CompressionMethod>(this.CompressionMethod);
			defaultInterpolatedStringHandler.AppendLiteral(", filter: ");
			defaultInterpolatedStringHandler.AppendFormatted<FilterMethod>(this.FilterMethod);
			defaultInterpolatedStringHandler.AppendLiteral(", interlace: ");
			defaultInterpolatedStringHandler.AppendFormatted<InterlaceMethod>(this.InterlaceMethod);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x0001BF88 File Offset: 0x0001A188
		// Note: this type is marked as 'beforefieldinit'.
		static ImageHeader()
		{
			Dictionary<ColorType, HashSet<byte>> dictionary = new Dictionary<ColorType, HashSet<byte>>();
			ColorType colorType = ColorType.None;
			HashSet<byte> hashSet = new HashSet<byte>();
			hashSet.Add(1);
			hashSet.Add(2);
			hashSet.Add(4);
			hashSet.Add(8);
			hashSet.Add(16);
			dictionary.Add(colorType, hashSet);
			ColorType colorType2 = ColorType.ColorUsed;
			HashSet<byte> hashSet2 = new HashSet<byte>();
			hashSet2.Add(8);
			hashSet2.Add(16);
			dictionary.Add(colorType2, hashSet2);
			ColorType colorType3 = ColorType.PaletteUsed | ColorType.ColorUsed;
			HashSet<byte> hashSet3 = new HashSet<byte>();
			hashSet3.Add(1);
			hashSet3.Add(2);
			hashSet3.Add(4);
			hashSet3.Add(8);
			dictionary.Add(colorType3, hashSet3);
			ColorType colorType4 = ColorType.AlphaChannelUsed;
			HashSet<byte> hashSet4 = new HashSet<byte>();
			hashSet4.Add(8);
			hashSet4.Add(16);
			dictionary.Add(colorType4, hashSet4);
			ColorType colorType5 = ColorType.ColorUsed | ColorType.AlphaChannelUsed;
			HashSet<byte> hashSet5 = new HashSet<byte>();
			hashSet5.Add(8);
			hashSet5.Add(16);
			dictionary.Add(colorType5, hashSet5);
			ImageHeader.PermittedBitDepths = dictionary;
		}

		// Token: 0x0400027D RID: 637
		internal static readonly byte[] HeaderBytes = new byte[]
		{
			73,
			72,
			68,
			82
		};

		// Token: 0x0400027E RID: 638
		private static readonly IReadOnlyDictionary<ColorType, HashSet<byte>> PermittedBitDepths;
	}
}
