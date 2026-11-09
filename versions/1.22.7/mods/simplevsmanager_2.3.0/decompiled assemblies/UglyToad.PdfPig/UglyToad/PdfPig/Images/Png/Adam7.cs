using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Images.Png
{
	// Token: 0x020000EC RID: 236
	internal static class Adam7
	{
		// Token: 0x06000479 RID: 1145 RVA: 0x0001B204 File Offset: 0x00019404
		public static int GetNumberOfScanlinesInPass(ImageHeader header, int pass)
		{
			int[] array = Adam7.PassToScanlineGridIndex[pass + 1];
			int num = header.Height % 8;
			if (num == 0)
			{
				return array.Length * (header.Height / 8);
			}
			int num2 = 0;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] < num)
				{
					num2++;
				}
			}
			return array.Length * (header.Height / 8) + num2;
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x0001B268 File Offset: 0x00019468
		public static int GetPixelsPerScanlineInPass(ImageHeader header, int pass)
		{
			int[] array = Adam7.PassToScanlineColumnIndex[pass + 1];
			int num = header.Width % 8;
			if (num == 0)
			{
				return array.Length * (header.Width / 8);
			}
			int num2 = 0;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] < num)
				{
					num2++;
				}
			}
			return array.Length * (header.Width / 8) + num2;
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x0001B2CC File Offset: 0x000194CC
		[return: TupleElementNames(new string[]
		{
			"x",
			"y"
		})]
		public static ValueTuple<int, int> GetPixelIndexForScanlineInPass(ImageHeader header, int pass, int scanlineIndex, int indexInScanline)
		{
			int[] array = Adam7.PassToScanlineColumnIndex[pass + 1];
			int[] array2 = Adam7.PassToScanlineGridIndex[pass + 1];
			int num = scanlineIndex % array2.Length;
			int num2 = indexInScanline % array.Length;
			int num3 = 8 * (scanlineIndex / array2.Length);
			return new ValueTuple<int, int>(8 * (indexInScanline / array.Length) + array[num2], num3 + array2[num]);
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x0001B320 File Offset: 0x00019520
		// Note: this type is marked as 'beforefieldinit'.
		static Adam7()
		{
			Dictionary<int, int[]> dictionary = new Dictionary<int, int[]>();
			dictionary.Add(1, new int[1]);
			dictionary.Add(2, new int[1]);
			dictionary.Add(3, new int[]
			{
				4
			});
			dictionary.Add(4, new int[]
			{
				default(int),
				4
			});
			dictionary.Add(5, new int[]
			{
				2,
				6
			});
			dictionary.Add(6, new int[]
			{
				0,
				2,
				4,
				6
			});
			dictionary.Add(7, new int[]
			{
				1,
				3,
				5,
				7
			});
			Adam7.PassToScanlineGridIndex = dictionary;
			dictionary = new Dictionary<int, int[]>();
			dictionary.Add(1, new int[1]);
			dictionary.Add(2, new int[]
			{
				4
			});
			dictionary.Add(3, new int[]
			{
				default(int),
				4
			});
			dictionary.Add(4, new int[]
			{
				2,
				6
			});
			dictionary.Add(5, new int[]
			{
				0,
				2,
				4,
				6
			});
			dictionary.Add(6, new int[]
			{
				1,
				3,
				5,
				7
			});
			dictionary.Add(7, new int[]
			{
				0,
				1,
				2,
				3,
				4,
				5,
				6,
				7
			});
			Adam7.PassToScanlineColumnIndex = dictionary;
		}

		// Token: 0x0400025C RID: 604
		private static readonly IReadOnlyDictionary<int, int[]> PassToScanlineGridIndex;

		// Token: 0x0400025D RID: 605
		private static readonly IReadOnlyDictionary<int, int[]> PassToScanlineColumnIndex;
	}
}
