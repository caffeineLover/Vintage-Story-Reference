using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Tokenization
{
	// Token: 0x02000006 RID: 6
	internal class HexTokenizer : ITokenizer
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000011 RID: 17 RVA: 0x000023E9 File Offset: 0x000005E9
		public bool ReadsNextByte { get; }

		// Token: 0x06000012 RID: 18 RVA: 0x000023F4 File Offset: 0x000005F4
		public bool TryTokenize(byte currentByte, IInputBytes inputBytes, out IToken token)
		{
			token = null;
			if (currentByte != 60)
			{
				return false;
			}
			List<char> list = new List<char>();
			while (inputBytes.MoveNext())
			{
				byte currentByte2 = inputBytes.CurrentByte;
				if (!ReadHelper.IsWhitespace(currentByte2))
				{
					if (currentByte2 == 62)
					{
						break;
					}
					if (!HexTokenizer.IsValidHexCharacter(currentByte2))
					{
						return false;
					}
					list.Add((char)currentByte2);
				}
			}
			token = new HexToken(list);
			return true;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002449 File Offset: 0x00000649
		private static bool IsValidHexCharacter(byte b)
		{
			return (b >= 48 && b <= 57) || (b >= 97 && b <= 102) || (b >= 65 && b <= 70);
		}
	}
}
