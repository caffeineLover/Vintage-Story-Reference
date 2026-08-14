using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util;

namespace UglyToad.PdfPig.Filters
{
	// Token: 0x020001BC RID: 444
	internal class CcittFaxDecodeFilter : IFilter
	{
		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000AA0 RID: 2720 RVA: 0x000352BC File Offset: 0x000334BC
		public bool IsSupported { get; } = 1;

		// Token: 0x06000AA1 RID: 2721 RVA: 0x000352C4 File Offset: 0x000334C4
		public byte[] Decode(IReadOnlyList<byte> input, DictionaryToken streamDictionary, int filterIndex)
		{
			DictionaryToken filterParameters = DecodeParameterResolver.GetFilterParameters(streamDictionary, filterIndex);
			int intOrDefault = filterParameters.GetIntOrDefault(NameToken.Columns, 1728);
			int num = filterParameters.GetIntOrDefault(NameToken.Rows, 0);
			int intOrDefault2 = streamDictionary.GetIntOrDefault(NameToken.Height, NameToken.H, 0);
			if (num > 0 && intOrDefault2 > 0)
			{
				num = intOrDefault2;
			}
			else
			{
				num = Math.Max(num, intOrDefault2);
			}
			int intOrDefault3 = filterParameters.GetIntOrDefault(NameToken.K, 0);
			bool booleanOrDefault = filterParameters.GetBooleanOrDefault(NameToken.EncodedByteAlign, false);
			CcittFaxCompressionType type = CcittFaxDecodeFilter.DetermineCompressionType(input, intOrDefault3);
			byte[] result;
			using (CcittFaxDecoderStream ccittFaxDecoderStream = new CcittFaxDecoderStream(new MemoryStream(Enumerable.ToArray<byte>(input)), intOrDefault, type, booleanOrDefault))
			{
				byte[] array = new byte[(intOrDefault + 7) / 8 * num];
				CcittFaxDecodeFilter.ReadFromDecoderStream(ccittFaxDecoderStream, array);
				if (!filterParameters.GetBooleanOrDefault(NameToken.BlackIs1, false))
				{
					CcittFaxDecodeFilter.InvertBitmap(array);
				}
				result = array;
			}
			return result;
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x000353AC File Offset: 0x000335AC
		private static CcittFaxCompressionType DetermineCompressionType(IReadOnlyList<byte> input, int k)
		{
			if (k == 0)
			{
				CcittFaxCompressionType result = CcittFaxCompressionType.Group3_1D;
				if (input.Count < 20)
				{
					throw new InvalidOperationException("The format is invalid");
				}
				if (input[0] != 0 || (input[1] >> 4 != 1 && input[1] != 1))
				{
					result = CcittFaxCompressionType.ModifiedHuffman;
					short num = (short)(((int)input[0] << 8) + (int)(input[1] & byte.MaxValue) >> 4);
					for (int i = 12; i < 160; i++)
					{
						num = (short)(((int)num << 1) + (input[i / 8] >> 7 - i % 8 & 1));
						if ((num & 4095) == 1)
						{
							return CcittFaxCompressionType.Group3_1D;
						}
					}
				}
				return result;
			}
			else
			{
				if (k > 0)
				{
					return CcittFaxCompressionType.Group3_2D;
				}
				return CcittFaxCompressionType.Group4_2D;
			}
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x00035454 File Offset: 0x00033654
		private static void ReadFromDecoderStream(CcittFaxDecoderStream decoderStream, byte[] result)
		{
			int num = 0;
			int num2;
			while ((num2 = decoderStream.Read(result, num, result.Length - num)) > -1)
			{
				num += num2;
				if (num >= result.Length)
				{
					break;
				}
			}
			decoderStream.Close();
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x00035488 File Offset: 0x00033688
		private static void InvertBitmap(byte[] bufferData)
		{
			int i = 0;
			int num = bufferData.Length;
			while (i < num)
			{
				bufferData[i] = (~bufferData[i] & byte.MaxValue);
				i++;
			}
		}
	}
}
