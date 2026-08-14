using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Graphics.Operations.PathConstruction
{
	// Token: 0x02000146 RID: 326
	public class AppendStraightLineSegment : IGraphicsStateOperation
	{
		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x0600075E RID: 1886 RVA: 0x00024C1D File Offset: 0x00022E1D
		public string Operator
		{
			get
			{
				return "l";
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x0600075F RID: 1887 RVA: 0x00024C24 File Offset: 0x00022E24
		public decimal X { get; }

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000760 RID: 1888 RVA: 0x00024C2C File Offset: 0x00022E2C
		public decimal Y { get; }

		// Token: 0x06000761 RID: 1889 RVA: 0x00024C34 File Offset: 0x00022E34
		public AppendStraightLineSegment(decimal x, decimal y)
		{
			this.X = x;
			this.Y = y;
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x00024C4A File Offset: 0x00022E4A
		public void Run(IOperationContext operationContext)
		{
			operationContext.LineTo((double)this.X, (double)this.Y);
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x00024C6A File Offset: 0x00022E6A
		public void Write(Stream stream)
		{
			stream.WriteDecimal(this.X);
			stream.WriteWhiteSpace();
			stream.WriteDecimal(this.Y);
			stream.WriteWhiteSpace();
			stream.WriteText("l", false);
			stream.WriteWhiteSpace();
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x00024CA4 File Offset: 0x00022EA4
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(2, 3);
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.X);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.Y);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted("l");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0400038B RID: 907
		public const string Symbol = "l";
	}
}
