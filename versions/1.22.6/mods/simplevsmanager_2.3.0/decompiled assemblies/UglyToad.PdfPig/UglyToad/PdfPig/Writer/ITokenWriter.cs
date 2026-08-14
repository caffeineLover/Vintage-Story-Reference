using System;
using System.Collections.Generic;
using System.IO;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Writer
{
	// Token: 0x0200000F RID: 15
	public interface ITokenWriter
	{
		// Token: 0x06000057 RID: 87
		void WriteToken(IToken token, Stream outputStream);

		// Token: 0x06000058 RID: 88
		void WriteObject(long objectNumber, int generation, byte[] data, Stream outputStream);

		// Token: 0x06000059 RID: 89
		void WriteCrossReferenceTable(IReadOnlyDictionary<IndirectReference, long> objectOffsets, IndirectReference catalogToken, Stream outputStream, IndirectReference? documentInformationReference);
	}
}
