using System;

namespace UglyToad.PdfPig.Geometry.ClipperLibrary
{
	// Token: 0x0200019A RID: 410
	internal struct ClipperIntRect
	{
		// Token: 0x06000A14 RID: 2580 RVA: 0x00033230 File Offset: 0x00031430
		public ClipperIntRect(long l, long t, long r, long b)
		{
			this.Left = l;
			this.Top = t;
			this.Right = r;
			this.Bottom = b;
		}

		// Token: 0x06000A15 RID: 2581 RVA: 0x0003324F File Offset: 0x0003144F
		public ClipperIntRect(ClipperIntRect ir)
		{
			this = new ClipperIntRect(ir.Left, ir.Top, ir.Right, ir.Bottom);
		}

		// Token: 0x040004C4 RID: 1220
		public long Left;

		// Token: 0x040004C5 RID: 1221
		public long Top;

		// Token: 0x040004C6 RID: 1222
		public long Right;

		// Token: 0x040004C7 RID: 1223
		public long Bottom;
	}
}
