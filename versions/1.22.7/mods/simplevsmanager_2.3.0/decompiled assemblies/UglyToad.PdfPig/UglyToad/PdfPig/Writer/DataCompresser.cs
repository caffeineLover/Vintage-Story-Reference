using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UglyToad.PdfPig.Filters;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Writer
{
	// Token: 0x0200000D RID: 13
	internal static class DataCompresser
	{
		// Token: 0x0600004B RID: 75 RVA: 0x00002F1D File Offset: 0x0000111D
		public static byte[] CompressBytes(IReadOnlyList<byte> bytes)
		{
			return DataCompresser.CompressBytes(Enumerable.ToArray<byte>(bytes));
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002F2C File Offset: 0x0000112C
		public static byte[] CompressBytes(byte[] bytes)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(bytes))
			{
				DictionaryToken streamDictionary = new DictionaryToken(new Dictionary<NameToken, IToken>());
				result = new FlateFilter().Encode(memoryStream, streamDictionary, 0);
			}
			return result;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002F78 File Offset: 0x00001178
		public static StreamToken CompressToStream(IReadOnlyList<byte> bytes)
		{
			return DataCompresser.CompressToStream(Enumerable.ToArray<byte>(bytes));
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002F88 File Offset: 0x00001188
		public static StreamToken CompressToStream(byte[] bytes)
		{
			byte[] array = DataCompresser.CompressBytes(bytes);
			Dictionary<NameToken, IToken> dictionary = new Dictionary<NameToken, IToken>();
			dictionary.Add(NameToken.Length, new NumericToken(array.Length));
			dictionary.Add(NameToken.Length1, new NumericToken(bytes.Length));
			dictionary.Add(NameToken.Filter, new ArrayToken(new NameToken[]
			{
				NameToken.FlateDecode
			}));
			return new StreamToken(new DictionaryToken(dictionary), array);
		}
	}
}
