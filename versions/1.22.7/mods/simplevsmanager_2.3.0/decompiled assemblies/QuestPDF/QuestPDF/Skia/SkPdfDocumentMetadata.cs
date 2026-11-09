using System;
using System.Runtime.InteropServices;

namespace QuestPDF.Skia
{
	// Token: 0x02000011 RID: 17
	internal struct SkPdfDocumentMetadata
	{
		// Token: 0x04000031 RID: 49
		public IntPtr Title;

		// Token: 0x04000032 RID: 50
		public IntPtr Author;

		// Token: 0x04000033 RID: 51
		public IntPtr Subject;

		// Token: 0x04000034 RID: 52
		public IntPtr Keywords;

		// Token: 0x04000035 RID: 53
		public IntPtr Creator;

		// Token: 0x04000036 RID: 54
		public IntPtr Producer;

		// Token: 0x04000037 RID: 55
		public IntPtr Language;

		// Token: 0x04000038 RID: 56
		public SkDateTime CreationDate;

		// Token: 0x04000039 RID: 57
		public SkDateTime ModificationDate;

		// Token: 0x0400003A RID: 58
		public PDFA_Conformance PDFA_Conformance;

		// Token: 0x0400003B RID: 59
		public PDFUA_Conformance PDFUA_Conformance;

		// Token: 0x0400003C RID: 60
		[MarshalAs(3)]
		public bool CompressDocument;

		// Token: 0x0400003D RID: 61
		public float RasterDPI;

		// Token: 0x0400003E RID: 62
		public IntPtr SemanticNodeRoot;
	}
}
