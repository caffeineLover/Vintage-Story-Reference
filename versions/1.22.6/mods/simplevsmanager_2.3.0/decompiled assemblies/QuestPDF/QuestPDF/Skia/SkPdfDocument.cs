using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace QuestPDF.Skia
{
	// Token: 0x02000014 RID: 20
	internal static class SkPdfDocument
	{
		// Token: 0x06000073 RID: 115 RVA: 0x00002DF8 File Offset: 0x00000FF8
		[NullableContext(1)]
		public static SkDocument Create(SkWriteStream stream, SkPdfDocumentMetadata metadata)
		{
			return new SkDocument(SkPdfDocument.API.pdf_document_create(stream.Instance, metadata));
		}

		// Token: 0x0200013A RID: 314
		private static class API
		{
			// Token: 0x06000B07 RID: 2823
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern IntPtr pdf_document_create(IntPtr stream, SkPdfDocumentMetadata metadata);
		}
	}
}
