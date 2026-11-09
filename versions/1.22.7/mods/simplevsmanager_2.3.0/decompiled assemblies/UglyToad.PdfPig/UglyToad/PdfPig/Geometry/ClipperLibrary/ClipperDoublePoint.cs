using System;

namespace UglyToad.PdfPig.Geometry.ClipperLibrary
{
	// Token: 0x020001A1 RID: 417
	internal struct ClipperDoublePoint
	{
		// Token: 0x06000A25 RID: 2597 RVA: 0x000335F8 File Offset: 0x000317F8
		public ClipperDoublePoint(double x, double y)
		{
			this.X = x;
			this.Y = y;
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x00033608 File Offset: 0x00031808
		public ClipperDoublePoint(ClipperDoublePoint dp)
		{
			this = new ClipperDoublePoint(dp.X, dp.Y);
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x0003361C File Offset: 0x0003181C
		public ClipperDoublePoint(ClipperIntPoint ip)
		{
			this = new ClipperDoublePoint((double)ip.X, (double)ip.Y);
		}

		// Token: 0x040004E2 RID: 1250
		public double X;

		// Token: 0x040004E3 RID: 1251
		public double Y;
	}
}
