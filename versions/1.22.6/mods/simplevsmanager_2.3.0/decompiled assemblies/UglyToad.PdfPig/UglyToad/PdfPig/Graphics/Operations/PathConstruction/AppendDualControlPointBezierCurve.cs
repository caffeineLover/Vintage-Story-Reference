using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Graphics.Operations.PathConstruction
{
	// Token: 0x02000142 RID: 322
	public class AppendDualControlPointBezierCurve : IGraphicsStateOperation
	{
		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000738 RID: 1848 RVA: 0x00024572 File Offset: 0x00022772
		public string Operator
		{
			get
			{
				return "c";
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000739 RID: 1849 RVA: 0x00024579 File Offset: 0x00022779
		public decimal X1 { get; }

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x0600073A RID: 1850 RVA: 0x00024581 File Offset: 0x00022781
		public decimal Y1 { get; }

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x0600073B RID: 1851 RVA: 0x00024589 File Offset: 0x00022789
		public decimal X2 { get; }

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x0600073C RID: 1852 RVA: 0x00024591 File Offset: 0x00022791
		public decimal Y2 { get; }

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x0600073D RID: 1853 RVA: 0x00024599 File Offset: 0x00022799
		public decimal X3 { get; }

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x0600073E RID: 1854 RVA: 0x000245A1 File Offset: 0x000227A1
		public decimal Y3 { get; }

		// Token: 0x0600073F RID: 1855 RVA: 0x000245A9 File Offset: 0x000227A9
		public AppendDualControlPointBezierCurve(decimal x1, decimal y1, decimal x2, decimal y2, decimal x3, decimal y3)
		{
			this.X1 = x1;
			this.Y1 = y1;
			this.X2 = x2;
			this.Y2 = y2;
			this.X3 = x3;
			this.Y3 = y3;
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x000245E0 File Offset: 0x000227E0
		public void Run(IOperationContext operationContext)
		{
			operationContext.BezierCurveTo((double)this.X1, (double)this.Y1, (double)this.X2, (double)this.Y2, (double)this.X3, (double)this.Y3);
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x0002463C File Offset: 0x0002283C
		public void Write(Stream stream)
		{
			stream.WriteDecimal(this.X1);
			stream.WriteWhiteSpace();
			stream.WriteDecimal(this.Y1);
			stream.WriteWhiteSpace();
			stream.WriteDecimal(this.X2);
			stream.WriteWhiteSpace();
			stream.WriteDecimal(this.Y2);
			stream.WriteWhiteSpace();
			stream.WriteDecimal(this.X3);
			stream.WriteWhiteSpace();
			stream.WriteDecimal(this.Y3);
			stream.WriteWhiteSpace();
			stream.WriteText("c", false);
			stream.WriteNewLine();
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x000246C8 File Offset: 0x000228C8
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(6, 7);
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.X1);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.Y1);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.X2);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.Y2);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.X3);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.Y3);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted("c");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04000375 RID: 885
		public const string Symbol = "c";
	}
}
