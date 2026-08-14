using System;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Geometry
{
	// Token: 0x0200018B RID: 395
	internal struct PdfVector
	{
		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000969 RID: 2409 RVA: 0x0002C282 File Offset: 0x0002A482
		public readonly double X { get; }

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x0600096A RID: 2410 RVA: 0x0002C28A File Offset: 0x0002A48A
		public readonly double Y { get; }

		// Token: 0x0600096B RID: 2411 RVA: 0x0002C292 File Offset: 0x0002A492
		public PdfVector(double x, double y)
		{
			this.X = x;
			this.Y = y;
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x0002C2A2 File Offset: 0x0002A4A2
		public PdfVector Scale(double scale)
		{
			return new PdfVector(this.X * scale, this.Y * scale);
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x0002C2BC File Offset: 0x0002A4BC
		public double GetMagnitude()
		{
			double x = this.X;
			double y = this.Y;
			return Math.Sqrt(x * x + y * y);
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x0002C2E1 File Offset: 0x0002A4E1
		public PdfVector Subtract(PdfVector vector)
		{
			return new PdfVector(this.X - vector.X, this.Y - vector.Y);
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x0002C304 File Offset: 0x0002A504
		public PdfPoint ToPoint()
		{
			return new PdfPoint(this.X, this.Y);
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x0002C318 File Offset: 0x0002A518
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(4, 2);
			defaultInterpolatedStringHandler.AppendLiteral("(");
			defaultInterpolatedStringHandler.AppendFormatted<double>(this.X);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(this.Y);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
