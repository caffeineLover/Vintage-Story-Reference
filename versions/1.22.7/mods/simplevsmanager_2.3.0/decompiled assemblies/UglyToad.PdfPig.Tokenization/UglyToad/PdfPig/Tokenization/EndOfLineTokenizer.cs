using System;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Tokenization
{
	// Token: 0x02000005 RID: 5
	public class EndOfLineTokenizer : ITokenizer
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000E RID: 14 RVA: 0x000023C0 File Offset: 0x000005C0
		public bool ReadsNextByte { get; }

		// Token: 0x0600000F RID: 15 RVA: 0x000023C8 File Offset: 0x000005C8
		public bool TryTokenize(byte currentByte, IInputBytes inputBytes, out IToken token)
		{
			token = null;
			if (currentByte != 13 && currentByte != 10)
			{
				return false;
			}
			token = EndOfLineToken.Token;
			return true;
		}
	}
}
