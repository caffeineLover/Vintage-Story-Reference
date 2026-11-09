using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Writer.Colors;
using UglyToad.PdfPig.Writer.Xmp;

namespace UglyToad.PdfPig.Writer
{
	// Token: 0x02000012 RID: 18
	internal static class PdfABaselineRuleBuilder
	{
		// Token: 0x0600005F RID: 95 RVA: 0x00003218 File Offset: 0x00001418
		public static void Obey(Dictionary<NameToken, IToken> catalog, Func<IToken, IndirectReferenceToken> writerFunc, PdfDocumentBuilder.DocumentInformationBuilder documentInformationBuilder, PdfAStandard archiveStandard, decimal version)
		{
			catalog[NameToken.OutputIntents] = OutputIntentsFactory.GetOutputIntentsArray(writerFunc);
			StreamToken streamToken = XmpWriter.GenerateXmpStream(documentInformationBuilder, version, archiveStandard);
			IndirectReferenceToken indirectReferenceToken = writerFunc.Invoke(streamToken);
			catalog[NameToken.Metadata] = indirectReferenceToken;
		}
	}
}
