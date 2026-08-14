using System;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Tokenization.Scanner
{
	// Token: 0x02000083 RID: 131
	internal interface IPdfTokenScanner : ISeekableTokenScanner, ITokenScanner, IDisposable
	{
		// Token: 0x0600020A RID: 522
		ObjectToken Get(IndirectReference reference);

		// Token: 0x0600020B RID: 523
		void ReplaceToken(IndirectReference reference, IToken token);
	}
}
