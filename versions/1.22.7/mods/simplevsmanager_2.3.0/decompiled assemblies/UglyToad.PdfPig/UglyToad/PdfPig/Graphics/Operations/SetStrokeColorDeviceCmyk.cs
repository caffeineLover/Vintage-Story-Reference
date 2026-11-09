using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Graphics.Operations
{
	// Token: 0x0200011D RID: 285
	public class SetStrokeColorDeviceCmyk : IGraphicsStateOperation
	{
		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000641 RID: 1601 RVA: 0x00022B43 File Offset: 0x00020D43
		public string Operator
		{
			get
			{
				return "K";
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000642 RID: 1602 RVA: 0x00022B4A File Offset: 0x00020D4A
		public decimal C { get; }

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000643 RID: 1603 RVA: 0x00022B52 File Offset: 0x00020D52
		public decimal M { get; }

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000644 RID: 1604 RVA: 0x00022B5A File Offset: 0x00020D5A
		public decimal Y { get; }

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000645 RID: 1605 RVA: 0x00022B62 File Offset: 0x00020D62
		public decimal K { get; }

		// Token: 0x06000646 RID: 1606 RVA: 0x00022B6A File Offset: 0x00020D6A
		public SetStrokeColorDeviceCmyk(decimal c, decimal m, decimal y, decimal k)
		{
			this.C = c;
			this.M = m;
			this.Y = y;
			this.K = k;
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x00022B8F File Offset: 0x00020D8F
		public void Run(IOperationContext operationContext)
		{
			operationContext.GetCurrentState().ColorSpaceContext.SetStrokingColorCmyk(this.C, this.M, this.Y, this.K);
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x00022BBC File Offset: 0x00020DBC
		public void Write(Stream stream)
		{
			stream.WriteDecimal(this.C);
			stream.WriteWhiteSpace();
			stream.WriteDecimal(this.M);
			stream.WriteWhiteSpace();
			stream.WriteDecimal(this.Y);
			stream.WriteWhiteSpace();
			stream.WriteDecimal(this.K);
			stream.WriteWhiteSpace();
			stream.WriteText("K", false);
			stream.WriteNewLine();
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x00022C24 File Offset: 0x00020E24
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(4, 5);
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.C);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.M);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.Y);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.K);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted("K");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04000316 RID: 790
		public const string Symbol = "K";
	}
}
