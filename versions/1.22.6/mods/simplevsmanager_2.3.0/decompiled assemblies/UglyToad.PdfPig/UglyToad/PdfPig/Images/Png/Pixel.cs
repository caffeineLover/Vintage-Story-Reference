using System;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Images.Png
{
	// Token: 0x020000FA RID: 250
	internal readonly struct Pixel
	{
		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060004B0 RID: 1200 RVA: 0x0001C171 File Offset: 0x0001A371
		public byte R { get; }

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060004B1 RID: 1201 RVA: 0x0001C179 File Offset: 0x0001A379
		public byte G { get; }

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060004B2 RID: 1202 RVA: 0x0001C181 File Offset: 0x0001A381
		public byte B { get; }

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060004B3 RID: 1203 RVA: 0x0001C189 File Offset: 0x0001A389
		public byte A { get; }

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060004B4 RID: 1204 RVA: 0x0001C191 File Offset: 0x0001A391
		public bool IsGrayscale { get; }

		// Token: 0x060004B5 RID: 1205 RVA: 0x0001C199 File Offset: 0x0001A399
		public Pixel(byte r, byte g, byte b, byte a, bool isGrayscale)
		{
			this.R = r;
			this.G = g;
			this.B = b;
			this.A = a;
			this.IsGrayscale = isGrayscale;
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x0001C1C0 File Offset: 0x0001A3C0
		public Pixel(byte r, byte g, byte b)
		{
			this.R = r;
			this.G = g;
			this.B = b;
			this.A = 255;
			this.IsGrayscale = 0;
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x0001C1E9 File Offset: 0x0001A3E9
		public Pixel(byte grayscale)
		{
			this.R = grayscale;
			this.G = grayscale;
			this.B = grayscale;
			this.A = 255;
			this.IsGrayscale = 1;
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x0001C214 File Offset: 0x0001A414
		public override bool Equals(object obj)
		{
			if (obj is Pixel)
			{
				Pixel pixel = (Pixel)obj;
				return this.IsGrayscale == pixel.IsGrayscale && this.A == pixel.A && this.R == pixel.R && this.G == pixel.G && this.B == pixel.B;
			}
			return false;
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x0001C280 File Offset: 0x0001A480
		public bool Equals(Pixel other)
		{
			return this.R == other.R && this.G == other.G && this.B == other.B && this.A == other.A && this.IsGrayscale == other.IsGrayscale;
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x0001C2DC File Offset: 0x0001A4DC
		public override int GetHashCode()
		{
			return (((this.R.GetHashCode() * 397 ^ this.G.GetHashCode()) * 397 ^ this.B.GetHashCode()) * 397 ^ this.A.GetHashCode()) * 397 ^ this.IsGrayscale.GetHashCode();
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x0001C34C File Offset: 0x0001A54C
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(8, 4);
			defaultInterpolatedStringHandler.AppendLiteral("(");
			defaultInterpolatedStringHandler.AppendFormatted<byte>(this.R);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<byte>(this.G);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<byte>(this.B);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<byte>(this.A);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
