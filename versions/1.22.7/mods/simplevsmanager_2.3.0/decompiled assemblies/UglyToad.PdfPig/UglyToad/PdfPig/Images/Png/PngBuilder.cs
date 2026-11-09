using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace UglyToad.PdfPig.Images.Png
{
	// Token: 0x020000FC RID: 252
	internal class PngBuilder
	{
		// Token: 0x060004C5 RID: 1221 RVA: 0x0001C500 File Offset: 0x0001A700
		public static PngBuilder Create(int width, int height, bool hasAlphaChannel)
		{
			int num = hasAlphaChannel ? 4 : 3;
			return new PngBuilder(new byte[height * width * num + height], hasAlphaChannel, width, height, num);
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x0001C52A File Offset: 0x0001A72A
		private PngBuilder(byte[] rawData, bool hasAlphaChannel, int width, int height, int bytesPerPixel)
		{
			this.rawData = rawData;
			this.hasAlphaChannel = hasAlphaChannel;
			this.width = width;
			this.height = height;
			this.bytesPerPixel = bytesPerPixel;
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x0001C557 File Offset: 0x0001A757
		public PngBuilder SetPixel(byte r, byte g, byte b, int x, int y)
		{
			return this.SetPixel(new Pixel(r, g, b), x, y);
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x0001C56C File Offset: 0x0001A76C
		public PngBuilder SetPixel(Pixel pixel, int x, int y)
		{
			int num = y * (this.width * this.bytesPerPixel + 1) + 1 + x * this.bytesPerPixel;
			this.rawData[num++] = pixel.R;
			this.rawData[num++] = pixel.G;
			this.rawData[num++] = pixel.B;
			if (this.hasAlphaChannel)
			{
				this.rawData[num] = pixel.A;
			}
			return this;
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x0001C5E8 File Offset: 0x0001A7E8
		public byte[] Save()
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				this.Save(memoryStream);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x0001C628 File Offset: 0x0001A828
		public void Save(Stream outputStream)
		{
			outputStream.Write(HeaderValidationResult.ExpectedHeader, 0, HeaderValidationResult.ExpectedHeader.Length);
			PngStreamWriteHelper pngStreamWriteHelper = new PngStreamWriteHelper(outputStream);
			pngStreamWriteHelper.WriteChunkLength(13);
			pngStreamWriteHelper.WriteChunkHeader(ImageHeader.HeaderBytes);
			StreamHelper.WriteBigEndianInt32(pngStreamWriteHelper, this.width);
			StreamHelper.WriteBigEndianInt32(pngStreamWriteHelper, this.height);
			pngStreamWriteHelper.WriteByte(8);
			ColorType colorType = ColorType.ColorUsed;
			if (this.hasAlphaChannel)
			{
				colorType |= ColorType.AlphaChannelUsed;
			}
			pngStreamWriteHelper.WriteByte((byte)colorType);
			pngStreamWriteHelper.WriteByte(0);
			pngStreamWriteHelper.WriteByte(0);
			pngStreamWriteHelper.WriteByte(0);
			pngStreamWriteHelper.WriteCrc();
			byte[] array = PngBuilder.Compress(this.rawData);
			pngStreamWriteHelper.WriteChunkLength(array.Length);
			pngStreamWriteHelper.WriteChunkHeader(Encoding.ASCII.GetBytes("IDAT"));
			pngStreamWriteHelper.Write(array, 0, array.Length);
			pngStreamWriteHelper.WriteCrc();
			pngStreamWriteHelper.WriteChunkLength(0);
			pngStreamWriteHelper.WriteChunkHeader(Encoding.ASCII.GetBytes("IEND"));
			pngStreamWriteHelper.WriteCrc();
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x0001C70C File Offset: 0x0001A90C
		private static byte[] Compress(byte[] data)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (DeflateStream deflateStream = new DeflateStream(memoryStream, 1, true))
				{
					deflateStream.Write(data, 0, data.Length);
					deflateStream.Close();
					memoryStream.Seek(0L, 0);
					byte[] array = new byte[2L + memoryStream.Length + 4L];
					array[0] = 120;
					array[1] = 1;
					int num = 0;
					int num2;
					while ((num2 = memoryStream.ReadByte()) != -1)
					{
						array[2 + num] = (byte)num2;
						num++;
					}
					int num3 = Adler32Checksum.Calculate(data);
					long num4 = 2L + memoryStream.Length;
					byte[] array2 = array;
					long num5 = num4;
					num4 = num5 + 1L;
					array2[(int)(checked((IntPtr)num5))] = (byte)(num3 >> 24);
					byte[] array3 = array;
					long num6 = num4;
					num4 = num6 + 1L;
					array3[(int)(checked((IntPtr)num6))] = (byte)(num3 >> 16);
					byte[] array4 = array;
					long num7 = num4;
					num4 = num7 + 1L;
					array4[(int)(checked((IntPtr)num7))] = (byte)(num3 >> 8);
					array[(int)(checked((IntPtr)num4))] = (byte)num3;
					result = array;
				}
			}
			return result;
		}

		// Token: 0x04000293 RID: 659
		private const byte Deflate32KbWindow = 120;

		// Token: 0x04000294 RID: 660
		private const byte ChecksumBits = 1;

		// Token: 0x04000295 RID: 661
		private readonly byte[] rawData;

		// Token: 0x04000296 RID: 662
		private readonly bool hasAlphaChannel;

		// Token: 0x04000297 RID: 663
		private readonly int width;

		// Token: 0x04000298 RID: 664
		private readonly int height;

		// Token: 0x04000299 RID: 665
		private readonly int bytesPerPixel;
	}
}
