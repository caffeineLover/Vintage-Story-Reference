using System;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Tokenization.Scanner
{
	// Token: 0x02000010 RID: 16
	public interface ITokenScanner
	{
		// Token: 0x06000043 RID: 67
		bool MoveNext();

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000044 RID: 68
		IToken CurrentToken { get; }

		// Token: 0x06000045 RID: 69
		bool TryReadToken<T>(out T token) where T : class, IToken;
	}
}
