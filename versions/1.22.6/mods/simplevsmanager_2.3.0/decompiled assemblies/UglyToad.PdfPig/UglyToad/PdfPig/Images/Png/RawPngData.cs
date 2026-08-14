using System;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Images.Png
{
	// Token: 0x02000101 RID: 257
	internal class RawPngData
	{
		// Token: 0x060004E6 RID: 1254 RVA: 0x0001D35C File Offset: 0x0001B55C
		public RawPngData(byte[] data, int bytesPerPixel, Palette palette, ImageHeader imageHeader)
		{
			if (this.width < 0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(47, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Width must be greater than or equal to 0, got ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.width);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new ArgumentOutOfRangeException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			this.data = data;
			this.bytesPerPixel = bytesPerPixel;
			this.palette = palette;
			this.width = imageHeader.Width;
			this.colorType = imageHeader.ColorType;
			this.rowOffset = ((imageHeader.InterlaceMethod == InterlaceMethod.Adam7) ? 0 : 1);
			this.bitDepth = (int)imageHeader.BitDepth;
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x0001D414 File Offset: 0x0001B614
		public Pixel GetPixel(int x, int y)
		{
			if (this.palette == null)
			{
				int num = this.rowOffset + this.rowOffset * y + this.bytesPerPixel * this.width * y + this.bytesPerPixel * x;
				byte b = this.data[num];
				switch (this.bytesPerPixel)
				{
				case 1:
					return new Pixel(b, b, b, byte.MaxValue, true);
				case 2:
					if (this.colorType == ColorType.None)
					{
						byte second = this.data[num + 1];
						byte b2 = RawPngData.ToSingleByte(b, second);
						return new Pixel(b2, b2, b2, byte.MaxValue, true);
					}
					return new Pixel(b, b, b, this.data[num + 1], true);
				case 3:
					return new Pixel(b, this.data[num + 1], this.data[num + 2], byte.MaxValue, false);
				case 4:
					if (this.colorType == ColorType.AlphaChannelUsed)
					{
						byte second2 = this.data[num + 1];
						byte first = this.data[num + 2];
						byte second3 = this.data[num + 3];
						byte b3 = RawPngData.ToSingleByte(b, second2);
						byte a = RawPngData.ToSingleByte(first, second3);
						return new Pixel(b3, b3, b3, a, true);
					}
					return new Pixel(b, this.data[num + 1], this.data[num + 2], this.data[num + 3], false);
				case 6:
					return new Pixel(b, this.data[num + 2], this.data[num + 4], byte.MaxValue, false);
				case 8:
				{
					int num2 = num;
					byte r = RawPngData.ToSingleByte(this.data[num2++], this.data[num2++]);
					byte g = RawPngData.ToSingleByte(this.data[num2++], this.data[num2++]);
					byte b4 = RawPngData.ToSingleByte(this.data[num2++], this.data[num2++]);
					byte a2 = RawPngData.ToSingleByte(this.data[num2++], this.data[num2++]);
					return new Pixel(r, g, b4, a2, false);
				}
				}
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(40, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unreconized number of bytes per pixel: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.bytesPerPixel);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			int num3 = 8 / this.bitDepth;
			int num4 = 1 + this.width / num3;
			int num5 = x / num3;
			int num6 = 1 + y * num4 + num5;
			byte b5 = this.data[num6];
			if (this.bitDepth == 8)
			{
				return this.palette.GetPixel((int)b5);
			}
			int num7 = x % num3;
			int num8 = 8 - (num7 + 1) * this.bitDepth;
			int index = b5 >> num8 & (1 << this.bitDepth) - 1;
			return this.palette.GetPixel(index);
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x0001D6EC File Offset: 0x0001B8EC
		private static byte ToSingleByte(byte first, byte second)
		{
			int num = ((int)first << 8) + (int)second;
			return (byte)Math.Round((double)(255 * num) / 65535.0);
		}

		// Token: 0x0400029E RID: 670
		private readonly byte[] data;

		// Token: 0x0400029F RID: 671
		private readonly int bytesPerPixel;

		// Token: 0x040002A0 RID: 672
		private readonly int width;

		// Token: 0x040002A1 RID: 673
		private readonly Palette palette;

		// Token: 0x040002A2 RID: 674
		private readonly ColorType colorType;

		// Token: 0x040002A3 RID: 675
		private readonly int rowOffset;

		// Token: 0x040002A4 RID: 676
		private readonly int bitDepth;
	}
}
