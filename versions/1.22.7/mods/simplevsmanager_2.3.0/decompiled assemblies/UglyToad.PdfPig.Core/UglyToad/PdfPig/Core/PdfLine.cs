using System;

namespace UglyToad.PdfPig.Core
{
	// Token: 0x0200000C RID: 12
	public struct PdfLine
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00002554 File Offset: 0x00000754
		public double Length
		{
			get
			{
				double num = this.Point1.X - this.Point2.X;
				double num2 = this.Point1.Y - this.Point2.Y;
				return Math.Sqrt(num * num + num2 * num2);
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600002C RID: 44 RVA: 0x000025A7 File Offset: 0x000007A7
		public readonly PdfPoint Point1 { get; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600002D RID: 45 RVA: 0x000025AF File Offset: 0x000007AF
		public readonly PdfPoint Point2 { get; }

		// Token: 0x0600002E RID: 46 RVA: 0x000025B7 File Offset: 0x000007B7
		public PdfLine(double x1, double y1, double x2, double y2)
		{
			this = new PdfLine(new PdfPoint(x1, y1), new PdfPoint(x2, y2));
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000025CE File Offset: 0x000007CE
		public PdfLine(PdfPoint point1, PdfPoint point2)
		{
			this.Point1 = point1;
			this.Point2 = point2;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000025E0 File Offset: 0x000007E0
		public PdfRectangle GetBoundingRectangle()
		{
			return new PdfRectangle(Math.Min(this.Point1.X, this.Point2.X), Math.Min(this.Point1.Y, this.Point2.Y), Math.Max(this.Point1.X, this.Point2.X), Math.Max(this.Point1.Y, this.Point2.Y));
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002678 File Offset: 0x00000878
		public override bool Equals(object obj)
		{
			if (obj is PdfLine)
			{
				PdfLine pdfLine = (PdfLine)obj;
				return pdfLine.Point1.Equals(this.Point1) && pdfLine.Point2.Equals(this.Point2);
			}
			return false;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000026DC File Offset: 0x000008DC
		public override int GetHashCode()
		{
			return new ValueTuple<PdfPoint, PdfPoint>(this.Point1, this.Point2).GetHashCode();
		}
	}
}
