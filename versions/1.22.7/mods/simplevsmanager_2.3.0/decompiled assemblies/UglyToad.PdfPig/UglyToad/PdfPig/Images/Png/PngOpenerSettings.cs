using System;

namespace UglyToad.PdfPig.Images.Png
{
	// Token: 0x020000FF RID: 255
	internal class PngOpenerSettings
	{
		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060004D2 RID: 1234 RVA: 0x0001D209 File Offset: 0x0001B409
		// (set) Token: 0x060004D3 RID: 1235 RVA: 0x0001D211 File Offset: 0x0001B411
		public IChunkVisitor ChunkVisitor { get; set; }

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060004D4 RID: 1236 RVA: 0x0001D21A File Offset: 0x0001B41A
		// (set) Token: 0x060004D5 RID: 1237 RVA: 0x0001D222 File Offset: 0x0001B422
		public bool DisallowTrailingData { get; set; }
	}
}
