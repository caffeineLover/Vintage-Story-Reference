using System;
using System.IO;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Writer
{
	// Token: 0x0200000E RID: 14
	internal interface IPdfStreamWriter : IDisposable
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600004F RID: 79
		// (set) Token: 0x06000050 RID: 80
		bool AttemptDeduplication { get; set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000051 RID: 81
		Stream Stream { get; }

		// Token: 0x06000052 RID: 82
		IndirectReferenceToken WriteToken(IToken token);

		// Token: 0x06000053 RID: 83
		IndirectReferenceToken WriteToken(IToken token, IndirectReferenceToken indirectReference);

		// Token: 0x06000054 RID: 84
		IndirectReferenceToken ReserveObjectNumber();

		// Token: 0x06000055 RID: 85
		void InitializePdf(decimal version);

		// Token: 0x06000056 RID: 86
		void CompletePdf(IndirectReferenceToken catalogReference, IndirectReferenceToken documentInformationReference = null);
	}
}
