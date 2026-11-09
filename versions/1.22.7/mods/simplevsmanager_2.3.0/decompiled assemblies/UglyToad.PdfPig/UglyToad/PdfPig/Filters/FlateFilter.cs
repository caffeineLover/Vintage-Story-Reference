using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using UglyToad.PdfPig.Fonts;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util;

namespace UglyToad.PdfPig.Filters
{
	// Token: 0x020001C2 RID: 450
	internal class FlateFilter : IFilter
	{
		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000AC4 RID: 2756 RVA: 0x000366A8 File Offset: 0x000348A8
		public bool IsSupported { get; } = 1;

		// Token: 0x06000AC5 RID: 2757 RVA: 0x000366B0 File Offset: 0x000348B0
		public byte[] Decode(IReadOnlyList<byte> input, DictionaryToken streamDictionary, int filterIndex)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			DictionaryToken filterParameters = DecodeParameterResolver.GetFilterParameters(streamDictionary, filterIndex);
			int intOrDefault = filterParameters.GetIntOrDefault(NameToken.Predictor, -1);
			byte[] array = Enumerable.ToArray<byte>(input);
			try
			{
				byte[] array2 = this.Decompress(array);
				if (intOrDefault == -1)
				{
					return array2;
				}
				int colors = Math.Min(filterParameters.GetIntOrDefault(NameToken.Colors, 1), 32);
				int intOrDefault2 = filterParameters.GetIntOrDefault(NameToken.BitsPerComponent, 8);
				int intOrDefault3 = filterParameters.GetIntOrDefault(NameToken.Columns, 1);
				return PngPredictor.Decode(array2, intOrDefault, colors, intOrDefault2, intOrDefault3);
			}
			catch
			{
			}
			return array;
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x00036754 File Offset: 0x00034954
		private byte[] Decompress(byte[] input)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(input))
			{
				using (MemoryStream memoryStream2 = new MemoryStream())
				{
					memoryStream.ReadByte();
					memoryStream.ReadByte();
					try
					{
						using (DeflateStream deflateStream = new DeflateStream(memoryStream, 0))
						{
							deflateStream.CopyTo(memoryStream2);
							result = memoryStream2.ToArray();
						}
					}
					catch (InvalidDataException ex)
					{
						throw new CorruptCompressedDataException("Invalid Flate compressed stream encountered", ex);
					}
				}
			}
			return result;
		}

		// Token: 0x06000AC7 RID: 2759 RVA: 0x000367F8 File Offset: 0x000349F8
		public byte[] Encode(Stream input, DictionaryToken streamDictionary, int index)
		{
			byte[] array;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				input.CopyTo(memoryStream);
				array = memoryStream.ToArray();
			}
			byte[] result;
			using (MemoryStream memoryStream2 = new MemoryStream())
			{
				using (DeflateStream deflateStream = new DeflateStream(memoryStream2, 1))
				{
					deflateStream.Write(array, 0, array.Length);
					deflateStream.Close();
					byte[] array2 = memoryStream2.ToArray();
					byte[] array3 = new byte[2 + array2.Length + 4];
					array3[0] = 120;
					array3[1] = 1;
					Array.Copy(array2, 0, array3, 2, array2.Length);
					int num = Adler32Checksum.Calculate(array);
					int num2 = 2 + array2.Length;
					array3[num2++] = (byte)(num >> 24);
					array3[num2++] = (byte)(num >> 16);
					array3[num2++] = (byte)(num >> 8);
					array3[num2] = (byte)num;
					result = array3;
				}
			}
			return result;
		}

		// Token: 0x0400057C RID: 1404
		private const int DefaultColors = 1;

		// Token: 0x0400057D RID: 1405
		private const int DefaultBitsPerComponent = 8;

		// Token: 0x0400057E RID: 1406
		private const int DefaultColumns = 1;

		// Token: 0x0400057F RID: 1407
		private const byte Deflate32KbWindow = 120;

		// Token: 0x04000580 RID: 1408
		private const byte ChecksumBits = 1;
	}
}
