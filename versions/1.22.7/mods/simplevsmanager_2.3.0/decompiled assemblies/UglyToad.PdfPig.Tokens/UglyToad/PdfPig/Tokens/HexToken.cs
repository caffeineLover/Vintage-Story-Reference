using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UglyToad.PdfPig.Tokens
{
	// Token: 0x02000007 RID: 7
	public class HexToken : IDataToken<string>, IToken, IEquatable<IToken>
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000025 RID: 37 RVA: 0x000026CA File Offset: 0x000008CA
		public string Data { get; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000026 RID: 38 RVA: 0x000026D2 File Offset: 0x000008D2
		public IReadOnlyList<byte> Bytes { get; }

		// Token: 0x06000027 RID: 39 RVA: 0x000026DC File Offset: 0x000008DC
		public HexToken(IReadOnlyList<char> characters)
		{
			if (characters == null)
			{
				throw new ArgumentNullException("characters");
			}
			List<byte> list = new List<byte>();
			for (int i = 0; i < characters.Count; i += 2)
			{
				char high = characters[i];
				char low;
				if (i == characters.Count - 1)
				{
					low = '0';
				}
				else
				{
					low = characters[i + 1];
				}
				byte b = HexToken.Convert(high, low);
				list.Add(b);
			}
			if (list.Count >= 2 && list[0] == 254 && list[1] == 255)
			{
				this.Data = Encoding.BigEndianUnicode.GetString(list.ToArray(), 2, list.Count - 2);
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (byte b2 in list)
				{
					if (b2 != 0)
					{
						stringBuilder.Append((char)b2);
					}
				}
				this.Data = stringBuilder.ToString();
			}
			this.Bytes = list;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000027F0 File Offset: 0x000009F0
		public static byte Convert(char high, char low)
		{
			byte b = HexToken.HexMap[high];
			byte b2 = HexToken.HexMap[low];
			return b << 4 | b2;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x0000281C File Offset: 0x00000A1C
		public static int ConvertHexBytesToInt(HexToken token)
		{
			if (token == null)
			{
				throw new ArgumentNullException("token");
			}
			IReadOnlyList<byte> bytes = token.Bytes;
			int num = (int)(bytes[0] & byte.MaxValue);
			if (bytes.Count == 2)
			{
				num <<= 8;
				num += (int)(bytes[1] & byte.MaxValue);
			}
			return num;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x0000286C File Offset: 0x00000A6C
		public bool Equals(IToken obj)
		{
			if (this == obj)
			{
				return true;
			}
			HexToken hexToken = obj as HexToken;
			return hexToken != null && this.Data == hexToken.Data;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x0000289C File Offset: 0x00000A9C
		public string GetHexString()
		{
			return BitConverter.ToString(Enumerable.ToArray<byte>(this.Bytes)).Replace("-", string.Empty);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000028C0 File Offset: 0x00000AC0
		// Note: this type is marked as 'beforefieldinit'.
		static HexToken()
		{
			Dictionary<char, byte> dictionary = new Dictionary<char, byte>();
			dictionary.Add('0', 0);
			dictionary.Add('1', 1);
			dictionary.Add('2', 2);
			dictionary.Add('3', 3);
			dictionary.Add('4', 4);
			dictionary.Add('5', 5);
			dictionary.Add('6', 6);
			dictionary.Add('7', 7);
			dictionary.Add('8', 8);
			dictionary.Add('9', 9);
			dictionary.Add('A', 10);
			dictionary.Add('a', 10);
			dictionary.Add('B', 11);
			dictionary.Add('b', 11);
			dictionary.Add('C', 12);
			dictionary.Add('c', 12);
			dictionary.Add('D', 13);
			dictionary.Add('d', 13);
			dictionary.Add('E', 14);
			dictionary.Add('e', 14);
			dictionary.Add('F', 15);
			dictionary.Add('f', 15);
			HexToken.HexMap = dictionary;
		}

		// Token: 0x04000009 RID: 9
		private static readonly Dictionary<char, byte> HexMap;
	}
}
