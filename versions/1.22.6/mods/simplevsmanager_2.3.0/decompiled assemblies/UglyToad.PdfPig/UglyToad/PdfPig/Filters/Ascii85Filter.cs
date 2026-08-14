using System;
using System.Collections.Generic;
using System.IO;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Filters
{
	// Token: 0x020001B8 RID: 440
	internal class Ascii85Filter : IFilter
	{
		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000A92 RID: 2706 RVA: 0x00034DF3 File Offset: 0x00032FF3
		public bool IsSupported { get; } = 1;

		// Token: 0x06000A93 RID: 2707 RVA: 0x00034DFC File Offset: 0x00032FFC
		public byte[] Decode(IReadOnlyList<byte> input, DictionaryToken streamDictionary, int filterIndex)
		{
			byte[] array = new byte[5];
			int num = 0;
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					for (int i = 0; i < input.Count; i++)
					{
						byte b = input[i];
						if (!Ascii85Filter.IsWhiteSpace(b))
						{
							if (b == Ascii85Filter.EndOfDataBytes[0] && (i == input.Count - 1 || input[i + 1] == Ascii85Filter.EndOfDataBytes[1]))
							{
								if (num > 0)
								{
									Ascii85Filter.WriteData(array, num, binaryWriter);
								}
								num = 0;
								break;
							}
							if (b == 122)
							{
								if (num > 0)
								{
									throw new InvalidOperationException("Encountered z within a 5 character block");
								}
								for (int j = 0; j < 4; j++)
								{
									binaryWriter.Write(0);
								}
								num = 0;
							}
							else
							{
								array[num] = b - 33;
								num++;
							}
							if (num == 5)
							{
								Ascii85Filter.WriteData(array, num, binaryWriter);
								num = 0;
							}
						}
					}
					if (num > 0)
					{
						Ascii85Filter.WriteData(array, num, binaryWriter);
					}
					binaryWriter.Flush();
					result = memoryStream.ToArray();
				}
			}
			return result;
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x00034F24 File Offset: 0x00033124
		private static void WriteData(byte[] ascii, int index, BinaryWriter writer)
		{
			if (index < 2)
			{
				throw new ArgumentOutOfRangeException("index", "Cannot convert a block padded by 4 'u' characters.");
			}
			for (int i = index; i < 5; i++)
			{
				ascii[i] = 84;
			}
			int num = 0;
			num += (int)ascii[0] * Ascii85Filter.PowerByIndex[4];
			num += (int)ascii[1] * Ascii85Filter.PowerByIndex[3];
			num += (int)ascii[2] * Ascii85Filter.PowerByIndex[2];
			num += (int)ascii[3] * Ascii85Filter.PowerByIndex[1];
			num += (int)ascii[4] * Ascii85Filter.PowerByIndex[0];
			writer.Write((byte)(num >> 24));
			if (index > 2)
			{
				writer.Write((byte)(num >> 16));
			}
			if (index > 3)
			{
				writer.Write((byte)(num >> 8));
			}
			if (index > 4)
			{
				writer.Write((byte)num);
			}
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x00034FD2 File Offset: 0x000331D2
		private static bool IsWhiteSpace(byte b)
		{
			return b == 13 || b == 10 || b == 32;
		}

		// Token: 0x0400054C RID: 1356
		private const byte EmptyBlock = 122;

		// Token: 0x0400054D RID: 1357
		private const byte Offset = 33;

		// Token: 0x0400054E RID: 1358
		private const byte EmptyCharacterPadding = 117;

		// Token: 0x0400054F RID: 1359
		private static readonly byte[] EndOfDataBytes = new byte[]
		{
			126,
			62
		};

		// Token: 0x04000550 RID: 1360
		private static readonly int[] PowerByIndex = new int[]
		{
			1,
			85,
			7225,
			614125,
			52200625
		};
	}
}
