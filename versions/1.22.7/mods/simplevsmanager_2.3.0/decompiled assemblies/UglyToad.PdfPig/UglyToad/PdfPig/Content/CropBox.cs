using System;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Content
{
	// Token: 0x020001DF RID: 479
	public class CropBox
	{
		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000B58 RID: 2904 RVA: 0x00039813 File Offset: 0x00037A13
		public PdfRectangle Bounds { get; }

		// Token: 0x06000B59 RID: 2905 RVA: 0x0003981B File Offset: 0x00037A1B
		public CropBox(PdfRectangle bounds)
		{
			this.Bounds = bounds;
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x0003982C File Offset: 0x00037A2C
		public override string ToString()
		{
			return this.Bounds.ToString();
		}
	}
}
