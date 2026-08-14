using System;
using System.Collections.Generic;
using System.IO;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Filters
{
	// Token: 0x020001B9 RID: 441
	internal class AsciiHexDecodeFilter : IFilter
	{
		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000A98 RID: 2712 RVA: 0x00035022 File Offset: 0x00033222
		public bool IsSupported { get; } = 1;

		// Token: 0x06000A99 RID: 2713 RVA: 0x0003502C File Offset: 0x0003322C
		public byte[] Decode(IReadOnlyList<byte> input, DictionaryToken streamDictionary, int filterIndex)
		{
			byte[] array = new byte[2];
			int num = 0;
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					int num2 = 0;
					while (num2 < input.Count && input[num2] != 62)
					{
						if (!AsciiHexDecodeFilter.IsWhitespace(input[num2]) && input[num2] != 60)
						{
							array[num] = input[num2];
							num++;
							if (num == 2)
							{
								AsciiHexDecodeFilter.WriteHexToByte(array, binaryWriter);
								num = 0;
							}
						}
						num2++;
					}
					if (num > 0)
					{
						if (num == 1)
						{
							array[1] = 48;
						}
						AsciiHexDecodeFilter.WriteHexToByte(array, binaryWriter);
					}
					binaryWriter.Flush();
					result = memoryStream.ToArray();
				}
			}
			return result;
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x00035100 File Offset: 0x00033300
		private static void WriteHexToByte(byte[] hexBytes, BinaryWriter writer)
		{
			short num = AsciiHexDecodeFilter.ReverseHex[(int)hexBytes[0]];
			short num2 = AsciiHexDecodeFilter.ReverseHex[(int)hexBytes[1]];
			if (num == -1)
			{
				string text = "Invalid character encountered in hex encoded stream: ";
				char c = (char)hexBytes[0];
				throw new InvalidOperationException(text + c.ToString());
			}
			if (num2 == -1)
			{
				string text2 = "Invalid character encountered in hex encoded stream: ";
				char c = (char)hexBytes[0];
				throw new InvalidOperationException(text2 + c.ToString());
			}
			byte b = (byte)(num * 16 + num2);
			writer.Write(b);
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x0003516C File Offset: 0x0003336C
		private static bool IsWhitespace(byte c)
		{
			return c == 0 || c == 9 || c == 10 || c == 12 || c == 13 || c == 32;
		}

		// Token: 0x04000552 RID: 1362
		private static readonly short[] ReverseHex = new short[]
		{
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			0,
			1,
			2,
			3,
			4,
			5,
			6,
			7,
			8,
			9,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			10,
			11,
			12,
			13,
			14,
			15,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			10,
			11,
			12,
			13,
			14,
			15
		};
	}
}
