using System;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Encryption
{
	// Token: 0x020001D4 RID: 468
	internal class NoOpEncryptionHandler : IEncryptionHandler
	{
		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000B1A RID: 2842 RVA: 0x00038D81 File Offset: 0x00036F81
		public static NoOpEncryptionHandler Instance { get; } = new NoOpEncryptionHandler();

		// Token: 0x06000B1B RID: 2843 RVA: 0x00038D88 File Offset: 0x00036F88
		private NoOpEncryptionHandler()
		{
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x00038D90 File Offset: 0x00036F90
		public IToken Decrypt(IndirectReference reference, IToken token)
		{
			return token;
		}
	}
}
