using System;
using System.Collections.Generic;
using System.IO;
using UglyToad.PdfPig.Graphics.Operations;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util;

namespace UglyToad.PdfPig.Writer.Fonts
{
	// Token: 0x02000021 RID: 33
	internal static class ToUnicodeCMapBuilder
	{
		// Token: 0x06000105 RID: 261 RVA: 0x00009B04 File Offset: 0x00007D04
		public static IReadOnlyList<byte> ConvertToCMapStream(IReadOnlyDictionary<char, byte> unicodeToCharacterCode)
		{
			IReadOnlyList<byte> result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				ToUnicodeCMapBuilder.TokenWriter.WriteToken(NameToken.CidInit, memoryStream);
				ToUnicodeCMapBuilder.TokenWriter.WriteToken(NameToken.ProcSet, memoryStream);
				memoryStream.WriteText("findresource", true);
				memoryStream.WriteText("begin", false);
				memoryStream.WriteNewLine();
				memoryStream.WriteDecimal(12m);
				memoryStream.WriteWhiteSpace();
				memoryStream.WriteText("dict", true);
				memoryStream.WriteText("begin", false);
				memoryStream.WriteNewLine();
				memoryStream.WriteText("begincmap", false);
				memoryStream.WriteNewLine();
				ToUnicodeCMapBuilder.TokenWriter.WriteToken(NameToken.CidSystemInfo, memoryStream);
				Dictionary<NameToken, IToken> dictionary = new Dictionary<NameToken, IToken>();
				dictionary.Add(NameToken.Registry, new StringToken("Adobe", 0));
				dictionary.Add(NameToken.Ordering, new StringToken("UCS", 0));
				dictionary.Add(NameToken.Supplement, new NumericToken(0m));
				DictionaryToken token = new DictionaryToken(dictionary);
				ToUnicodeCMapBuilder.TokenWriter.WriteToken(token, memoryStream);
				memoryStream.WriteWhiteSpace();
				memoryStream.WriteText("def", false);
				memoryStream.WriteNewLine();
				ToUnicodeCMapBuilder.TokenWriter.WriteToken(NameToken.Cmapname, memoryStream);
				ToUnicodeCMapBuilder.TokenWriter.WriteToken(NameToken.Create("Adobe-Identity-UCS"), memoryStream);
				memoryStream.WriteText("def", false);
				memoryStream.WriteNewLine();
				ToUnicodeCMapBuilder.TokenWriter.WriteToken(NameToken.CmapType, memoryStream);
				memoryStream.WriteNumberText(2m, "def");
				memoryStream.WriteNumberText(1m, "begincodespacerange");
				ToUnicodeCMapBuilder.TokenWriter.WriteToken(new HexToken(new char[]
				{
					'0',
					'0'
				}), memoryStream);
				ToUnicodeCMapBuilder.TokenWriter.WriteToken(new HexToken(new char[]
				{
					'F',
					'F'
				}), memoryStream);
				memoryStream.WriteNewLine();
				memoryStream.WriteText("endcodespacerange", false);
				memoryStream.WriteNewLine();
				memoryStream.WriteNumberText(unicodeToCharacterCode.Count, "beginbfchar");
				foreach (KeyValuePair<char, byte> keyValuePair in unicodeToCharacterCode)
				{
					char key = keyValuePair.Key;
					byte b = (byte)key;
					byte b2 = (byte)(key >> 8);
					string @string = Hex.GetString(new byte[]
					{
						keyValuePair.Value
					});
					string string2 = Hex.GetString(new byte[]
					{
						b2,
						b
					});
					ToUnicodeCMapBuilder.TokenWriter.WriteToken(new HexToken(@string.ToCharArray()), memoryStream);
					ToUnicodeCMapBuilder.TokenWriter.WriteToken(new HexToken(string2.ToCharArray()), memoryStream);
					memoryStream.WriteNewLine();
				}
				memoryStream.WriteText("endbfchar", false);
				memoryStream.WriteNewLine();
				memoryStream.WriteText("endcmap", false);
				memoryStream.WriteNewLine();
				memoryStream.WriteText("CMapName currentdict /CMap defineresource pop", false);
				memoryStream.WriteNewLine();
				memoryStream.WriteText("end", false);
				memoryStream.WriteNewLine();
				memoryStream.WriteText("end", false);
				memoryStream.WriteNewLine();
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x04000099 RID: 153
		private const string BeginToken = "begin";

		// Token: 0x0400009A RID: 154
		private const string BeginCMapToken = "begincmap";

		// Token: 0x0400009B RID: 155
		private const string DefToken = "def";

		// Token: 0x0400009C RID: 156
		private const string DictToken = "dict";

		// Token: 0x0400009D RID: 157
		private const string FindResourceToken = "findresource";

		// Token: 0x0400009E RID: 158
		private static readonly TokenWriter TokenWriter = new TokenWriter();
	}
}
