using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util;

namespace UglyToad.PdfPig.Filters
{
	// Token: 0x020001C8 RID: 456
	internal class LzwFilter : IFilter
	{
		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000AD5 RID: 2773 RVA: 0x00036947 File Offset: 0x00034B47
		public bool IsSupported { get; } = 1;

		// Token: 0x06000AD6 RID: 2774 RVA: 0x00036950 File Offset: 0x00034B50
		public byte[] Decode(IReadOnlyList<byte> input, DictionaryToken streamDictionary, int filterIndex)
		{
			DictionaryToken filterParameters = DecodeParameterResolver.GetFilterParameters(streamDictionary, filterIndex);
			int intOrDefault = filterParameters.GetIntOrDefault(NameToken.Predictor, -1);
			int intOrDefault2 = filterParameters.GetIntOrDefault(NameToken.EarlyChange, 1);
			if (intOrDefault > 1)
			{
				byte[] inputBytes = LzwFilter.Decode(input, intOrDefault2 == 1);
				int colors = Math.Min(filterParameters.GetIntOrDefault(NameToken.Colors, 1), 32);
				int intOrDefault3 = filterParameters.GetIntOrDefault(NameToken.BitsPerComponent, 8);
				int intOrDefault4 = filterParameters.GetIntOrDefault(NameToken.Columns, 1);
				return PngPredictor.Decode(inputBytes, intOrDefault, colors, intOrDefault3, intOrDefault4);
			}
			return LzwFilter.Decode(input, intOrDefault2 == 1);
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x000369D4 File Offset: 0x00034BD4
		private static byte[] Decode(IReadOnlyList<byte> input, bool isEarlyChange)
		{
			List<byte> list = new List<byte>((int)((double)input.Count * 1.5));
			Dictionary<int, byte[]> defaultTable = LzwFilter.GetDefaultTable();
			int numberOfBits = 9;
			BitStream bitStream = new BitStream(input);
			int num = isEarlyChange ? 0 : 1;
			int num2 = -1;
			for (;;)
			{
				int num3 = bitStream.Get(numberOfBits);
				if (num3 == 257)
				{
					break;
				}
				if (num3 == 256)
				{
					defaultTable = LzwFilter.GetDefaultTable();
					num2 = -1;
					numberOfBits = 9;
				}
				else
				{
					byte[] array;
					if (defaultTable.TryGetValue(num3, ref array))
					{
						list.AddRange(array);
						if (num2 >= 0)
						{
							byte[] array2 = defaultTable[num2];
							byte[] array3 = new byte[array2.Length + 1];
							Array.Copy(array2, array3, array2.Length);
							array3[array2.Length] = array[0];
							defaultTable[defaultTable.Count] = array3;
						}
					}
					else
					{
						byte[] array4 = defaultTable[num2];
						byte[] array5 = new byte[array4.Length + 1];
						Array.Copy(array4, array5, array4.Length);
						array5[array4.Length] = array4[0];
						list.AddRange(array5);
						defaultTable[defaultTable.Count] = array5;
					}
					num2 = num3;
					if (defaultTable.Count >= 2047 + num)
					{
						numberOfBits = 12;
					}
					else if (defaultTable.Count >= 1023 + num)
					{
						numberOfBits = 11;
					}
					else if (defaultTable.Count >= 511 + num)
					{
						numberOfBits = 10;
					}
					else
					{
						numberOfBits = 9;
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x00036B3C File Offset: 0x00034D3C
		private static Dictionary<int, byte[]> GetDefaultTable()
		{
			Dictionary<int, byte[]> dictionary = new Dictionary<int, byte[]>();
			for (int i = 0; i < 256; i++)
			{
				dictionary[i] = new byte[]
				{
					(byte)i
				};
			}
			dictionary[256] = null;
			dictionary[257] = null;
			return dictionary;
		}

		// Token: 0x04000584 RID: 1412
		private const int DefaultColors = 1;

		// Token: 0x04000585 RID: 1413
		private const int DefaultBitsPerComponent = 8;

		// Token: 0x04000586 RID: 1414
		private const int DefaultColumns = 1;

		// Token: 0x04000587 RID: 1415
		private const int ClearTable = 256;

		// Token: 0x04000588 RID: 1416
		private const int EodMarker = 257;

		// Token: 0x04000589 RID: 1417
		private const int NineBitBoundary = 511;

		// Token: 0x0400058A RID: 1418
		private const int TenBitBoundary = 1023;

		// Token: 0x0400058B RID: 1419
		private const int ElevenBitBoundary = 2047;
	}
}
