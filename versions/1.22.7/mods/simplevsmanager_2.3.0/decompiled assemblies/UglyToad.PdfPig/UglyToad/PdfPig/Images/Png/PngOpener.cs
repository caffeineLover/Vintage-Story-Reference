using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Text;

namespace UglyToad.PdfPig.Images.Png
{
	// Token: 0x020000FE RID: 254
	internal static class PngOpener
	{
		// Token: 0x060004CD RID: 1229 RVA: 0x0001CB3C File Offset: 0x0001AD3C
		public static Png Open(Stream stream, IChunkVisitor chunkVisitor = null)
		{
			return PngOpener.Open(stream, new PngOpenerSettings
			{
				ChunkVisitor = chunkVisitor
			});
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x0001CB50 File Offset: 0x0001AD50
		public static Png Open(Stream stream, PngOpenerSettings settings)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (!stream.CanRead)
			{
				throw new ArgumentException("The provided stream of type " + stream.GetType().FullName + " was not readable.");
			}
			HeaderValidationResult headerValidationResult = PngOpener.HasValidHeader(stream);
			if (!headerValidationResult.IsValid)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(60, 1);
				defaultInterpolatedStringHandler.AppendLiteral("The provided stream did not start with the PNG header. Got ");
				defaultInterpolatedStringHandler.AppendFormatted<HeaderValidationResult>(headerValidationResult);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			byte[] array = new byte[4];
			ImageHeader imageHeader = PngOpener.ReadImageHeader(stream, array);
			bool flag = false;
			Palette palette = null;
			Png result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (MemoryStream memoryStream2 = new MemoryStream())
				{
					ChunkHeader chunkHeader;
					while (PngOpener.TryReadChunkHeader(stream, out chunkHeader))
					{
						if (flag)
						{
							if (settings != null && settings.DisallowTrailingData)
							{
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(58, 1);
								defaultInterpolatedStringHandler.AppendLiteral("Found another chunk ");
								defaultInterpolatedStringHandler.AppendFormatted<ChunkHeader>(chunkHeader);
								defaultInterpolatedStringHandler.AppendLiteral(" after already reading the IEND chunk.");
								throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
							}
							break;
						}
						else
						{
							byte[] array2 = new byte[chunkHeader.Length];
							int num = stream.Read(array2, 0, array2.Length);
							if (num != array2.Length)
							{
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(50, 3);
								defaultInterpolatedStringHandler.AppendLiteral("Did not read ");
								defaultInterpolatedStringHandler.AppendFormatted<int>(chunkHeader.Length);
								defaultInterpolatedStringHandler.AppendLiteral(" bytes for the ");
								defaultInterpolatedStringHandler.AppendFormatted<ChunkHeader>(chunkHeader);
								defaultInterpolatedStringHandler.AppendLiteral(" header, only found: ");
								defaultInterpolatedStringHandler.AppendFormatted<int>(num);
								defaultInterpolatedStringHandler.AppendLiteral(".");
								throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
							}
							if (chunkHeader.IsCritical)
							{
								string name = chunkHeader.Name;
								if (!(name == "PLTE"))
								{
									if (!(name == "IDAT"))
									{
										if (!(name == "IEND"))
										{
											DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(54, 1);
											defaultInterpolatedStringHandler.AppendLiteral("Encountered critical header ");
											defaultInterpolatedStringHandler.AppendFormatted<ChunkHeader>(chunkHeader);
											defaultInterpolatedStringHandler.AppendLiteral(" which was not recognised.");
											throw new NotSupportedException(defaultInterpolatedStringHandler.ToStringAndClear());
										}
										flag = true;
									}
									else
									{
										memoryStream2.Write(array2, 0, array2.Length);
									}
								}
								else
								{
									if (chunkHeader.Length % 3 != 0)
									{
										DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(41, 1);
										defaultInterpolatedStringHandler.AppendLiteral("Palette data must be multiple of 3, got ");
										defaultInterpolatedStringHandler.AppendFormatted<int>(chunkHeader.Length);
										defaultInterpolatedStringHandler.AppendLiteral(".");
										throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
									}
									if (imageHeader.ColorType.HasFlag(ColorType.PaletteUsed))
									{
										palette = new Palette(array2);
									}
								}
							}
							else if (chunkHeader.Name == "tRNS" && palette != null)
							{
								palette.SetAlphaValues(array2);
							}
							num = stream.Read(array, 0, array.Length);
							if (num != 4)
							{
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(47, 1);
								defaultInterpolatedStringHandler.AppendLiteral("Did not read 4 bytes for the CRC, only found: ");
								defaultInterpolatedStringHandler.AppendFormatted<int>(num);
								defaultInterpolatedStringHandler.AppendLiteral(".");
								throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
							}
							int num2 = (int)Crc32.Calculate(Encoding.ASCII.GetBytes(chunkHeader.Name), array2);
							int num3 = ((int)array[0] << 24) + ((int)array[1] << 16) + ((int)array[2] << 8) + (int)array[3];
							if (num2 != num3)
							{
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 3);
								defaultInterpolatedStringHandler.AppendLiteral("CRC calculated ");
								defaultInterpolatedStringHandler.AppendFormatted<int>(num2);
								defaultInterpolatedStringHandler.AppendLiteral(" did not match file ");
								defaultInterpolatedStringHandler.AppendFormatted<int>(num3);
								defaultInterpolatedStringHandler.AppendLiteral(" for chunk: ");
								defaultInterpolatedStringHandler.AppendFormatted(chunkHeader.Name);
								defaultInterpolatedStringHandler.AppendLiteral(".");
								throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
							}
							if (settings != null)
							{
								IChunkVisitor chunkVisitor = settings.ChunkVisitor;
								if (chunkVisitor != null)
								{
									chunkVisitor.Visit(stream, imageHeader, chunkHeader, array2, array);
								}
							}
						}
					}
					memoryStream2.Flush();
					memoryStream2.Seek(2L, 0);
					using (DeflateStream deflateStream = new DeflateStream(memoryStream2, 0))
					{
						deflateStream.CopyTo(memoryStream);
						deflateStream.Close();
					}
				}
				byte[] array3 = memoryStream.ToArray();
				ValueTuple<byte, byte> bytesAndSamplesPerPixel = Decoder.GetBytesAndSamplesPerPixel(imageHeader);
				byte item = bytesAndSamplesPerPixel.Item1;
				byte item2 = bytesAndSamplesPerPixel.Item2;
				array3 = Decoder.Decode(array3, imageHeader, item, item2);
				result = new Png(imageHeader, new RawPngData(array3, (int)item, palette, imageHeader), palette != null && palette.HasAlphaValues);
			}
			return result;
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x0001D004 File Offset: 0x0001B204
		private static HeaderValidationResult HasValidHeader(Stream stream)
		{
			return new HeaderValidationResult(stream.ReadByte(), stream.ReadByte(), stream.ReadByte(), stream.ReadByte(), stream.ReadByte(), stream.ReadByte(), stream.ReadByte(), stream.ReadByte());
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x0001D03C File Offset: 0x0001B23C
		private static bool TryReadChunkHeader(Stream stream, out ChunkHeader chunkHeader)
		{
			chunkHeader = default(ChunkHeader);
			long position = stream.Position;
			byte[] array;
			if (!StreamHelper.TryReadHeaderBytes(stream, out array))
			{
				return false;
			}
			int length = StreamHelper.ReadBigEndianInt32(array, 0);
			string @string = Encoding.ASCII.GetString(array, 4, 4);
			chunkHeader = new ChunkHeader(position, length, @string);
			return true;
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x0001D088 File Offset: 0x0001B288
		private static ImageHeader ReadImageHeader(Stream stream, byte[] crc)
		{
			ChunkHeader chunkHeader;
			if (!PngOpener.TryReadChunkHeader(stream, out chunkHeader))
			{
				throw new ArgumentException("The provided stream did not contain a single chunk.");
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (chunkHeader.Name != "IHDR")
			{
				defaultInterpolatedStringHandler..ctor(41, 1);
				defaultInterpolatedStringHandler.AppendLiteral("The first chunk was not the IHDR chunk: ");
				defaultInterpolatedStringHandler.AppendFormatted<ChunkHeader>(chunkHeader);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (chunkHeader.Length != 13)
			{
				defaultInterpolatedStringHandler..ctor(52, 1);
				defaultInterpolatedStringHandler.AppendLiteral("The first chunk did not have a length of 13 bytes: ");
				defaultInterpolatedStringHandler.AppendFormatted<ChunkHeader>(chunkHeader);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			byte[] array = new byte[13];
			int num = stream.Read(array, 0, array.Length);
			if (num != 13)
			{
				defaultInterpolatedStringHandler..ctor(49, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Did not read 13 bytes for the IHDR, only found: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(num);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			num = stream.Read(crc, 0, crc.Length);
			if (num != 4)
			{
				defaultInterpolatedStringHandler..ctor(47, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Did not read 4 bytes for the CRC, only found: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(num);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			int width = StreamHelper.ReadBigEndianInt32(array, 0);
			int height = StreamHelper.ReadBigEndianInt32(array, 4);
			byte bitDepth = array[8];
			byte colorType = array[9];
			byte compressionMethod = array[10];
			byte filterMethod = array[11];
			byte interlaceMethod = array[12];
			return new ImageHeader(width, height, bitDepth, (ColorType)colorType, (CompressionMethod)compressionMethod, (FilterMethod)filterMethod, (InterlaceMethod)interlaceMethod);
		}
	}
}
