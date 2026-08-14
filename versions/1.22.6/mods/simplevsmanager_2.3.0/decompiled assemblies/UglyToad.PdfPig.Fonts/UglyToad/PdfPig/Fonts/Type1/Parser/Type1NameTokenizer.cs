using System;
using System.Text;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokenization;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Fonts.Type1.Parser
{
	// Token: 0x0200000E RID: 14
	public class Type1NameTokenizer : ITokenizer
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000064 RID: 100 RVA: 0x00004C5A File Offset: 0x00002E5A
		public bool ReadsNextByte { get; } = 1;

		// Token: 0x06000065 RID: 101 RVA: 0x00004C64 File Offset: 0x00002E64
		public bool TryTokenize(byte currentByte, IInputBytes inputBytes, out IToken token)
		{
			token = null;
			if (currentByte != 47)
			{
				return false;
			}
			StringBuilder stringBuilder = new StringBuilder();
			while (inputBytes.MoveNext() && !ReadHelper.IsWhitespace(inputBytes.CurrentByte) && inputBytes.CurrentByte != 123 && inputBytes.CurrentByte != 60 && inputBytes.CurrentByte != 47 && inputBytes.CurrentByte != 91 && inputBytes.CurrentByte != 40)
			{
				stringBuilder.Append((char)inputBytes.CurrentByte);
			}
			token = NameToken.Create(stringBuilder.ToString());
			return true;
		}
	}
}
