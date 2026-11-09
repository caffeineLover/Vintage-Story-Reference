using System;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Graphics
{
	// Token: 0x0200010F RID: 271
	public class TextMatrices
	{
		// Token: 0x1700016D RID: 365
		// (get) Token: 0x060005E4 RID: 1508 RVA: 0x00021FDE File Offset: 0x000201DE
		// (set) Token: 0x060005E5 RID: 1509 RVA: 0x00021FE6 File Offset: 0x000201E6
		public TransformationMatrix TextMatrix { get; set; }

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x060005E6 RID: 1510 RVA: 0x00021FEF File Offset: 0x000201EF
		// (set) Token: 0x060005E7 RID: 1511 RVA: 0x00021FF7 File Offset: 0x000201F7
		public TransformationMatrix TextLineMatrix { get; set; }
	}
}
