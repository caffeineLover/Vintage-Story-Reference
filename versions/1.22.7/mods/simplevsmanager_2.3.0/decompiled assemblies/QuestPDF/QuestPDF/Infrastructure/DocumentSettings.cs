using System;
using System.Runtime.CompilerServices;

namespace QuestPDF.Infrastructure
{
	// Token: 0x0200003D RID: 61
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DocumentSettings
	{
		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600018C RID: 396 RVA: 0x00004DEB File Offset: 0x00002FEB
		// (set) Token: 0x0600018D RID: 397 RVA: 0x00004DF6 File Offset: 0x00002FF6
		[Obsolete("Please use the ConformanceLevel property instead.")]
		public bool PdfA
		{
			get
			{
				return this.PDFA_Conformance > PDFA_Conformance.None;
			}
			set
			{
				this.PDFA_Conformance = (value ? PDFA_Conformance.PDFA_3B : PDFA_Conformance.None);
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600018E RID: 398 RVA: 0x00004E05 File Offset: 0x00003005
		// (set) Token: 0x0600018F RID: 399 RVA: 0x00004E0D File Offset: 0x0000300D
		public PDFA_Conformance PDFA_Conformance { get; set; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000190 RID: 400 RVA: 0x00004E16 File Offset: 0x00003016
		// (set) Token: 0x06000191 RID: 401 RVA: 0x00004E1E File Offset: 0x0000301E
		public PDFUA_Conformance PDFUA_Conformance { get; set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000192 RID: 402 RVA: 0x00004E27 File Offset: 0x00003027
		// (set) Token: 0x06000193 RID: 403 RVA: 0x00004E2F File Offset: 0x0000302F
		public bool CompressDocument { get; set; } = true;

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000194 RID: 404 RVA: 0x00004E38 File Offset: 0x00003038
		// (set) Token: 0x06000195 RID: 405 RVA: 0x00004E40 File Offset: 0x00003040
		public ImageCompressionQuality ImageCompressionQuality { get; set; } = ImageCompressionQuality.High;

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000196 RID: 406 RVA: 0x00004E49 File Offset: 0x00003049
		// (set) Token: 0x06000197 RID: 407 RVA: 0x00004E51 File Offset: 0x00003051
		public int ImageRasterDpi { get; set; } = 288;

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000198 RID: 408 RVA: 0x00004E5A File Offset: 0x0000305A
		// (set) Token: 0x06000199 RID: 409 RVA: 0x00004E62 File Offset: 0x00003062
		public ContentDirection ContentDirection { get; set; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600019A RID: 410 RVA: 0x00004E6B File Offset: 0x0000306B
		public static DocumentSettings Default
		{
			get
			{
				return new DocumentSettings();
			}
		}

		// Token: 0x040000D4 RID: 212
		public const int DefaultRasterDpi = 72;
	}
}
