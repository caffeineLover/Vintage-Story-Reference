using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Core
{
	// Token: 0x0200000D RID: 13
	public struct PdfPoint
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000033 RID: 51 RVA: 0x00002708 File Offset: 0x00000908
		public static PdfPoint Origin { get; } = new PdfPoint(0m, 0m);

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000034 RID: 52 RVA: 0x0000270F File Offset: 0x0000090F
		public readonly double X { get; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000035 RID: 53 RVA: 0x00002717 File Offset: 0x00000917
		public readonly double Y { get; }

		// Token: 0x06000036 RID: 54 RVA: 0x0000271F File Offset: 0x0000091F
		[DebuggerStepThrough]
		public PdfPoint(decimal x, decimal y)
		{
			this.X = (double)x;
			this.Y = (double)y;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x0000273B File Offset: 0x0000093B
		[DebuggerStepThrough]
		public PdfPoint(int x, int y)
		{
			this.X = (double)x;
			this.Y = (double)y;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x0000274D File Offset: 0x0000094D
		[DebuggerStepThrough]
		public PdfPoint(double x, double y)
		{
			this.X = x;
			this.Y = y;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x0000275D File Offset: 0x0000095D
		public PdfPoint MoveX(double dx)
		{
			return new PdfPoint(this.X + dx, this.Y);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002772 File Offset: 0x00000972
		public PdfPoint MoveY(double dy)
		{
			return new PdfPoint(this.X, this.Y + dy);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002787 File Offset: 0x00000987
		public PdfPoint Translate(double dx, double dy)
		{
			return new PdfPoint(this.X + dx, this.Y + dy);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000027A0 File Offset: 0x000009A0
		public override bool Equals(object obj)
		{
			if (obj is PdfPoint)
			{
				PdfPoint pdfPoint = (PdfPoint)obj;
				return pdfPoint.X == this.X && pdfPoint.Y == this.Y;
			}
			return false;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000027E0 File Offset: 0x000009E0
		public override int GetHashCode()
		{
			return new ValueTuple<double, double>(this.X, this.Y).GetHashCode();
		}

		// Token: 0x0600003E RID: 62 RVA: 0x0000280C File Offset: 0x00000A0C
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(8, 2);
			defaultInterpolatedStringHandler.AppendLiteral("(x:");
			defaultInterpolatedStringHandler.AppendFormatted(this.X.ToString(CultureInfo.InvariantCulture));
			defaultInterpolatedStringHandler.AppendLiteral(", y:");
			defaultInterpolatedStringHandler.AppendFormatted(this.Y.ToString(CultureInfo.InvariantCulture));
			defaultInterpolatedStringHandler.AppendLiteral(")");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
