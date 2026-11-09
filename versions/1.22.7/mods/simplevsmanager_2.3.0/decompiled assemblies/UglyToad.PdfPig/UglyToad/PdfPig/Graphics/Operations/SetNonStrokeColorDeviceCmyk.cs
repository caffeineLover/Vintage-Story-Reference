using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Graphics.Operations
{
	// Token: 0x02000117 RID: 279
	public class SetNonStrokeColorDeviceCmyk : IGraphicsStateOperation
	{
		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000614 RID: 1556 RVA: 0x00022507 File Offset: 0x00020707
		public string Operator
		{
			get
			{
				return "k";
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000615 RID: 1557 RVA: 0x0002250E File Offset: 0x0002070E
		public decimal C { get; }

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000616 RID: 1558 RVA: 0x00022516 File Offset: 0x00020716
		public decimal M { get; }

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000617 RID: 1559 RVA: 0x0002251E File Offset: 0x0002071E
		public decimal Y { get; }

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000618 RID: 1560 RVA: 0x00022526 File Offset: 0x00020726
		public decimal K { get; }

		// Token: 0x06000619 RID: 1561 RVA: 0x0002252E File Offset: 0x0002072E
		public SetNonStrokeColorDeviceCmyk(decimal c, decimal m, decimal y, decimal k)
		{
			this.C = c;
			this.M = m;
			this.Y = y;
			this.K = k;
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x00022553 File Offset: 0x00020753
		public void Run(IOperationContext operationContext)
		{
			operationContext.GetCurrentState().ColorSpaceContext.SetNonStrokingColorCmyk(this.C, this.M, this.Y, this.K);
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x00022580 File Offset: 0x00020780
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
			stream.WriteText("k", false);
			stream.WriteNewLine();
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x000225E8 File Offset: 0x000207E8
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
			defaultInterpolatedStringHandler.AppendFormatted("k");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04000302 RID: 770
		public const string Symbol = "k";
	}
}
