using System;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Tokenization.Scanner
{
	// Token: 0x02000082 RID: 130
	internal interface IObjectLocationProvider
	{
		// Token: 0x06000206 RID: 518
		bool TryGetOffset(IndirectReference reference, out long offset);

		// Token: 0x06000207 RID: 519
		void UpdateOffset(IndirectReference reference, long offset);

		// Token: 0x06000208 RID: 520
		bool TryGetCached(IndirectReference reference, out ObjectToken objectToken);

		// Token: 0x06000209 RID: 521
		void Cache(ObjectToken objectToken, bool force = false);
	}
}
