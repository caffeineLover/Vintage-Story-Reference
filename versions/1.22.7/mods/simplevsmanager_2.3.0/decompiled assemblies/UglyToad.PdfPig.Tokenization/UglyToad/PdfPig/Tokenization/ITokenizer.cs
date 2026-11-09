using System;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Tokenization
{
	// Token: 0x02000007 RID: 7
	public interface ITokenizer
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000015 RID: 21
		bool ReadsNextByte { get; }

		// Token: 0x06000016 RID: 22
		bool TryTokenize(byte currentByte, IInputBytes inputBytes, out IToken token);
	}
}
