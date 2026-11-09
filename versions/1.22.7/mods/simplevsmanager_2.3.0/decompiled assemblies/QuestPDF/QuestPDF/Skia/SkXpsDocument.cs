using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace QuestPDF.Skia
{
	// Token: 0x02000023 RID: 35
	internal static class SkXpsDocument
	{
		// Token: 0x060000BA RID: 186 RVA: 0x0000371D File Offset: 0x0000191D
		[NullableContext(1)]
		public static SkDocument Create(SkWriteStream stream, float dpi)
		{
			return new SkDocument(SkXpsDocument.API.xps_document_create(stream.Instance, dpi));
		}

		// Token: 0x02000145 RID: 325
		private static class API
		{
			// Token: 0x06000B24 RID: 2852
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern IntPtr xps_document_create(IntPtr stream, float dpi);
		}
	}
}
