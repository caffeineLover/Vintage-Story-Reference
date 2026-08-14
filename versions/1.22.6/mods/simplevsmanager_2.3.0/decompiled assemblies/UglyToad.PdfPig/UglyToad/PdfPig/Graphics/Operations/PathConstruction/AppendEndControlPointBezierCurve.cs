using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Graphics.Operations.PathConstruction
{
	// Token: 0x02000143 RID: 323
	public class AppendEndControlPointBezierCurve : IGraphicsStateOperation
	{
		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000743 RID: 1859 RVA: 0x00024787 File Offset: 0x00022987
		public string Operator
		{
			get
			{
				return "y";
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000744 RID: 1860 RVA: 0x0002478E File Offset: 0x0002298E
		public decimal X1 { get; }

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000745 RID: 1861 RVA: 0x00024796 File Offset: 0x00022996
		public decimal Y1 { get; }

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000746 RID: 1862 RVA: 0x0002479E File Offset: 0x0002299E
		public decimal X3 { get; }

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000747 RID: 1863 RVA: 0x000247A6 File Offset: 0x000229A6
		public decimal Y3 { get; }

		// Token: 0x06000748 RID: 1864 RVA: 0x000247AE File Offset: 0x000229AE
		public AppendEndControlPointBezierCurve(decimal x1, decimal y1, decimal x3, decimal y3)
		{
			this.X1 = x1;
			this.Y1 = y1;
			this.X3 = x3;
			this.Y3 = y3;
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x000247D4 File Offset: 0x000229D4
		public void Run(IOperationContext operationContext)
		{
			operationContext.BezierCurveTo((double)this.X1, (double)this.Y1, (double)this.X3, (double)this.Y3, (double)this.X3, (double)this.Y3);
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x00024830 File Offset: 0x00022A30
		public void Write(Stream stream)
		{
			stream.WriteDecimal(this.X1);
			stream.WriteWhiteSpace();
			stream.WriteDecimal(this.Y1);
			stream.WriteWhiteSpace();
			stream.WriteDecimal(this.X3);
			stream.WriteWhiteSpace();
			stream.WriteDecimal(this.Y3);
			stream.WriteWhiteSpace();
			stream.WriteText("y", false);
			stream.WriteNewLine();
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x00024898 File Offset: 0x00022A98
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(4, 5);
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.X1);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.Y1);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.X3);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.Y3);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted("y");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0400037C RID: 892
		public const string Symbol = "y";
	}
}
