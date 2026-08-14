using System;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Images.Png
{
	// Token: 0x020000F2 RID: 242
	internal static class Decoder
	{
		// Token: 0x0600048A RID: 1162 RVA: 0x0001B71C File Offset: 0x0001991C
		[return: TupleElementNames(new string[]
		{
			"bytesPerPixel",
			"samplesPerPixel"
		})]
		public static ValueTuple<byte, byte> GetBytesAndSamplesPerPixel(ImageHeader header)
		{
			int num = (int)((header.BitDepth + 7) / 8);
			byte b = Decoder.SamplesPerPixel(header);
			return new ValueTuple<byte, byte>((byte)((int)b * num), b);
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x0001B748 File Offset: 0x00019948
		public static byte[] Decode(byte[] decompressedData, ImageHeader header, byte bytesPerPixel, byte samplesPerPixel)
		{
			InterlaceMethod interlaceMethod = header.InterlaceMethod;
			if (interlaceMethod == InterlaceMethod.None)
			{
				int num = Decoder.BytesPerScanline(header, samplesPerPixel);
				int num2 = 1;
				for (int i = 0; i < header.Height; i++)
				{
					FilterType type = (FilterType)decompressedData[num2 - 1];
					int previousRowStartByteAbsolute = i + num * (i - 1);
					int num3 = num2 + num;
					for (int j = num2; j < num3; j++)
					{
						Decoder.ReverseFilter(decompressedData, type, previousRowStartByteAbsolute, num2, j, j - num2, (int)bytesPerPixel);
					}
					num2 += num + 1;
				}
				return decompressedData;
			}
			if (interlaceMethod != InterlaceMethod.Adam7)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(27, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid interlace method: ");
				defaultInterpolatedStringHandler.AppendFormatted<InterlaceMethod>(header.InterlaceMethod);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new ArgumentOutOfRangeException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			int num4 = header.Width * (int)bytesPerPixel;
			byte[] array = new byte[header.Height * num4];
			int num5 = 0;
			int previousRowStartByteAbsolute2 = -1;
			for (int k = 0; k < 7; k++)
			{
				int numberOfScanlinesInPass = Adam7.GetNumberOfScanlinesInPass(header, k);
				int pixelsPerScanlineInPass = Adam7.GetPixelsPerScanlineInPass(header, k);
				if (numberOfScanlinesInPass > 0 && pixelsPerScanlineInPass > 0)
				{
					for (int l = 0; l < numberOfScanlinesInPass; l++)
					{
						FilterType type2 = (FilterType)decompressedData[num5++];
						int num6 = num5;
						for (int m = 0; m < pixelsPerScanlineInPass; m++)
						{
							ValueTuple<int, int> pixelIndexForScanlineInPass = Adam7.GetPixelIndexForScanlineInPass(header, k, l, m);
							for (int n = 0; n < (int)bytesPerPixel; n++)
							{
								int rowByteIndex = m * (int)bytesPerPixel + n;
								Decoder.ReverseFilter(decompressedData, type2, previousRowStartByteAbsolute2, num6, num5, rowByteIndex, (int)bytesPerPixel);
								num5++;
							}
							int num7 = num4 * pixelIndexForScanlineInPass.Item2 + pixelIndexForScanlineInPass.Item1 * (int)bytesPerPixel;
							Array.ConstrainedCopy(decompressedData, num6 + m * (int)bytesPerPixel, array, num7, (int)bytesPerPixel);
						}
						previousRowStartByteAbsolute2 = num6;
					}
				}
			}
			return array;
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x0001B904 File Offset: 0x00019B04
		private static byte SamplesPerPixel(ImageHeader header)
		{
			switch (header.ColorType)
			{
			case ColorType.None:
				return 1;
			case ColorType.PaletteUsed:
				return 1;
			case ColorType.ColorUsed:
				return 3;
			case ColorType.AlphaChannelUsed:
				return 2;
			case ColorType.ColorUsed | ColorType.AlphaChannelUsed:
				return 4;
			}
			return 0;
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x0001B948 File Offset: 0x00019B48
		private static int BytesPerScanline(ImageHeader header, byte samplesPerPixel)
		{
			int width = header.Width;
			byte bitDepth = header.BitDepth;
			switch (bitDepth)
			{
			case 1:
				return (width + 7) / 8;
			case 2:
				return (width + 3) / 4;
			case 3:
				break;
			case 4:
				return (width + 1) / 2;
			default:
				if (bitDepth == 8 || bitDepth == 16)
				{
					return width * (int)samplesPerPixel * (int)(header.BitDepth / 8);
				}
				break;
			}
			return 0;
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x0001B9AC File Offset: 0x00019BAC
		private static void ReverseFilter(byte[] data, FilterType type, int previousRowStartByteAbsolute, int rowStartByteAbsolute, int byteAbsolute, int rowByteIndex, int bytesPerPixel)
		{
			Decoder.<>c__DisplayClass4_0 CS$<>8__locals1;
			CS$<>8__locals1.rowByteIndex = rowByteIndex;
			CS$<>8__locals1.bytesPerPixel = bytesPerPixel;
			CS$<>8__locals1.data = data;
			CS$<>8__locals1.rowStartByteAbsolute = rowStartByteAbsolute;
			CS$<>8__locals1.previousRowStartByteAbsolute = previousRowStartByteAbsolute;
			if (type == FilterType.Up)
			{
				int num = CS$<>8__locals1.previousRowStartByteAbsolute + CS$<>8__locals1.rowByteIndex;
				if (num < 0)
				{
					return;
				}
				byte[] data2 = CS$<>8__locals1.data;
				data2[byteAbsolute] += CS$<>8__locals1.data[num];
				return;
			}
			else
			{
				if (type != FilterType.Sub)
				{
					switch (type)
					{
					case FilterType.None:
						return;
					case FilterType.Average:
					{
						byte[] data3 = CS$<>8__locals1.data;
						data3[byteAbsolute] += (Decoder.<ReverseFilter>g__GetLeftByteValue|4_0(ref CS$<>8__locals1) + Decoder.<ReverseFilter>g__GetAboveByteValue|4_1(ref CS$<>8__locals1)) / 2;
						return;
					}
					case FilterType.Paeth:
					{
						byte a = Decoder.<ReverseFilter>g__GetLeftByteValue|4_0(ref CS$<>8__locals1);
						byte b = Decoder.<ReverseFilter>g__GetAboveByteValue|4_1(ref CS$<>8__locals1);
						byte c = Decoder.<ReverseFilter>g__GetAboveLeftByteValue|4_2(ref CS$<>8__locals1);
						byte[] data4 = CS$<>8__locals1.data;
						data4[byteAbsolute] += Decoder.GetPaethValue(a, b, c);
						return;
					}
					}
					throw new ArgumentOutOfRangeException("type", type, null);
				}
				int num2 = CS$<>8__locals1.rowByteIndex - CS$<>8__locals1.bytesPerPixel;
				if (num2 < 0)
				{
					return;
				}
				byte[] data5 = CS$<>8__locals1.data;
				data5[byteAbsolute] += CS$<>8__locals1.data[CS$<>8__locals1.rowStartByteAbsolute + num2];
				return;
			}
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x0001BAD8 File Offset: 0x00019CD8
		private static byte GetPaethValue(byte a, byte b, byte c)
		{
			byte b2 = a + b - c;
			int num = Math.Abs((int)(b2 - a));
			int num2 = Math.Abs((int)(b2 - b));
			int num3 = Math.Abs((int)(b2 - c));
			if (num <= num2 && num <= num3)
			{
				return a;
			}
			if (num2 > num3)
			{
				return c;
			}
			return b;
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x0001BB18 File Offset: 0x00019D18
		[CompilerGenerated]
		internal static byte <ReverseFilter>g__GetLeftByteValue|4_0(ref Decoder.<>c__DisplayClass4_0 A_0)
		{
			int num = A_0.rowByteIndex - A_0.bytesPerPixel;
			if (num < 0)
			{
				return 0;
			}
			return A_0.data[A_0.rowStartByteAbsolute + num];
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x0001BB48 File Offset: 0x00019D48
		[CompilerGenerated]
		internal static byte <ReverseFilter>g__GetAboveByteValue|4_1(ref Decoder.<>c__DisplayClass4_0 A_0)
		{
			int num = A_0.previousRowStartByteAbsolute + A_0.rowByteIndex;
			if (num < 0)
			{
				return 0;
			}
			return A_0.data[num];
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x0001BB74 File Offset: 0x00019D74
		[CompilerGenerated]
		internal static byte <ReverseFilter>g__GetAboveLeftByteValue|4_2(ref Decoder.<>c__DisplayClass4_0 A_0)
		{
			int num = A_0.previousRowStartByteAbsolute + A_0.rowByteIndex - A_0.bytesPerPixel;
			if (num >= A_0.previousRowStartByteAbsolute && A_0.previousRowStartByteAbsolute >= 0)
			{
				return A_0.data[num];
			}
			return 0;
		}
	}
}
