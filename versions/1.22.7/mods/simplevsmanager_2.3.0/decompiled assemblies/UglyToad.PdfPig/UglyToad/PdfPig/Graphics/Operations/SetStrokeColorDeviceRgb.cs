using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Graphics.Operations
{
	// Token: 0x0200011F RID: 287
	public class SetStrokeColorDeviceRgb : IGraphicsStateOperation
	{
		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000650 RID: 1616 RVA: 0x00022D3E File Offset: 0x00020F3E
		public string Operator
		{
			get
			{
				return "RG";
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000651 RID: 1617 RVA: 0x00022D45 File Offset: 0x00020F45
		public decimal R { get; }

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000652 RID: 1618 RVA: 0x00022D4D File Offset: 0x00020F4D
		public decimal G { get; }

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000653 RID: 1619 RVA: 0x00022D55 File Offset: 0x00020F55
		public decimal B { get; }

		// Token: 0x06000654 RID: 1620 RVA: 0x00022D5D File Offset: 0x00020F5D
		public SetStrokeColorDeviceRgb(decimal r, decimal g, decimal b)
		{
			this.R = r;
			this.G = g;
			this.B = b;
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x00022D7A File Offset: 0x00020F7A
		public void Run(IOperationContext operationContext)
		{
			operationContext.GetCurrentState().ColorSpaceContext.SetStrokingColorRgb(this.R, this.G, this.B);
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x00022DA0 File Offset: 0x00020FA0
		public void Write(Stream stream)
		{
			stream.WriteDecimal(this.R);
			stream.WriteWhiteSpace();
			stream.WriteDecimal(this.G);
			stream.WriteWhiteSpace();
			stream.WriteDecimal(this.B);
			stream.WriteWhiteSpace();
			stream.WriteText("RG", false);
			stream.WriteNewLine();
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x00022DF8 File Offset: 0x00020FF8
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(3, 4);
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.R);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.G);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.B);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted("RG");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0400031D RID: 797
		public const string Symbol = "RG";
	}
}
