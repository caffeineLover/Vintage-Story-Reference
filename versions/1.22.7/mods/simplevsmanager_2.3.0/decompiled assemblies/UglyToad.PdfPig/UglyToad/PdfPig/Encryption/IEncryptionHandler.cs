using System;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Encryption
{
	// Token: 0x020001D3 RID: 467
	internal interface IEncryptionHandler
	{
		// Token: 0x06000B19 RID: 2841
		IToken Decrypt(IndirectReference reference, IToken token);
	}
}
