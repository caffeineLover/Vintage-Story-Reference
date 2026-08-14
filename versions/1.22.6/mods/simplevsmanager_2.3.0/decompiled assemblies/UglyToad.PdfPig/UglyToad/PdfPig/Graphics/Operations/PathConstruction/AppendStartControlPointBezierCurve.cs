using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Graphics.Operations.PathConstruction
{
	// Token: 0x02000145 RID: 325
	public class AppendStartControlPointBezierCurve : IGraphicsStateOperation
	{
		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000755 RID: 1877 RVA: 0x00024AA1 File Offset: 0x00022CA1
		public string Operator
		{
			get
			{
				return "v";
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000756 RID: 1878 RVA: 0x00024AA8 File Offset: 0x00022CA8
		public decimal X2 { get; }

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000757 RID: 1879 RVA: 0x00024AB0 File Offset: 0x00022CB0
		public decimal Y2 { get; }

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000758 RID: 1880 RVA: 0x00024AB8 File Offset: 0x00022CB8
		public decimal X3 { get; }

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000759 RID: 1881 RVA: 0x00024AC0 File Offset: 0x00022CC0
		public decimal Y3 { get; }

		// Token: 0x0600075A RID: 1882 RVA: 0x00024AC8 File Offset: 0x00022CC8
		public AppendStartControlPointBezierCurve(decimal x2, decimal y2, decimal x3, decimal y3)
		{
			this.X2 = x2;
			this.Y2 = y2;
			this.X3 = x3;
			this.Y3 = y3;
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x00024AED File Offset: 0x00022CED
		public void Run(IOperationContext operationContext)
		{
			operationContext.BezierCurveTo((double)this.X2, (double)this.Y2, (double)this.X3, (double)this.Y3);
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x00024B28 File Offset: 0x00022D28
		public void Write(Stream stream)
		{
			stream.WriteDecimal(this.X2);
			stream.WriteWhiteSpace();
			stream.WriteDecimal(this.Y2);
			stream.WriteWhiteSpace();
			stream.WriteDecimal(this.X3);
			stream.WriteWhiteSpace();
			stream.WriteDecimal(this.Y3);
			stream.WriteWhiteSpace();
			stream.WriteText("v", false);
			stream.WriteNewLine();
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x00024B90 File Offset: 0x00022D90
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(4, 5);
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.X2);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.Y2);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.X3);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.Y3);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted("v");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04000386 RID: 902
		public const string Symbol = "v";
	}
}
