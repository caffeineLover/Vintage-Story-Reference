using System;
using System.Collections.Generic;
using System.Text;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Tokenization
{
	// Token: 0x02000009 RID: 9
	internal class NameTokenizer : ITokenizer
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002588 File Offset: 0x00000788
		public bool ReadsNextByte { get; } = 1;

		// Token: 0x0600001B RID: 27 RVA: 0x00002590 File Offset: 0x00000790
		public bool TryTokenize(byte currentByte, IInputBytes inputBytes, out IToken token)
		{
			token = null;
			if (currentByte != 47)
			{
				return false;
			}
			List<byte> list = NameTokenizer.ListPool.Borrow();
			bool flag = false;
			int num = 0;
			char[] array = new char[2];
			while (inputBytes.MoveNext())
			{
				byte currentByte2 = inputBytes.CurrentByte;
				if (currentByte2 == 35)
				{
					flag = true;
				}
				else if (flag)
				{
					if (ReadHelper.IsHex((char)currentByte2))
					{
						array[num] = (char)currentByte2;
						num++;
						if (num == 2)
						{
							byte b = (byte)Convert.ToInt32(new string(array), 16);
							list.Add(b);
							flag = false;
							num = 0;
						}
					}
					else
					{
						list.Add(35);
						if (num == 1)
						{
							list.Add((byte)array[0]);
						}
						if (ReadHelper.IsEndOfName((int)currentByte2))
						{
							break;
						}
						if (currentByte2 == 35)
						{
							flag = true;
							num = 0;
						}
						else
						{
							list.Add(currentByte2);
							flag = false;
							num = 0;
						}
					}
				}
				else
				{
					if (ReadHelper.IsEndOfName((int)currentByte2))
					{
						break;
					}
					list.Add(currentByte2);
				}
			}
			byte[] array2 = list.ToArray();
			NameTokenizer.ListPool.Return(list);
			string text = ReadHelper.IsValidUtf8(array2) ? Encoding.UTF8.GetString(array2) : Encoding.GetEncoding("windows-1252").GetString(array2);
			token = NameToken.Create(text);
			return true;
		}

		// Token: 0x0400000A RID: 10
		private static readonly ListPool<byte> ListPool = new ListPool<byte>(10);
	}
}
