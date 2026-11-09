using System;
using System.Collections.Generic;
using System.IO;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Filters
{
	// Token: 0x020001CA RID: 458
	internal class RunLengthFilter : IFilter
	{
		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06000ADD RID: 2781 RVA: 0x000370C8 File Offset: 0x000352C8
		public bool IsSupported { get; } = 1;

		// Token: 0x06000ADE RID: 2782 RVA: 0x000370D0 File Offset: 0x000352D0
		public byte[] Decode(IReadOnlyList<byte> input, DictionaryToken streamDictionary, int filterIndex)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					int i = 0;
					while (i < input.Count)
					{
						byte b = input[i];
						if (b == 128)
						{
							break;
						}
						if (b <= 127)
						{
							for (int j = (int)(b + 1); j > 0; j--)
							{
								i++;
								binaryWriter.Write(input[i]);
							}
							i++;
						}
						else
						{
							int num = 257 - (int)b;
							byte b2 = input[i + 1];
							for (int k = 0; k < num; k++)
							{
								binaryWriter.Write(b2);
							}
							i += 2;
						}
					}
					binaryWriter.Flush();
					result = memoryStream.ToArray();
				}
			}
			return result;
		}

		// Token: 0x0400058D RID: 1421
		private const byte EndOfDataLength = 128;
	}
}
