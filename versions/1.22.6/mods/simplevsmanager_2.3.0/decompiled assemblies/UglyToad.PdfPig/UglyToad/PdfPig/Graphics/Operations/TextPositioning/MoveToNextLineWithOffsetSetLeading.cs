using System;
using System.IO;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Graphics.Operations.TextState;

namespace UglyToad.PdfPig.Graphics.Operations.TextPositioning
{
	// Token: 0x02000130 RID: 304
	public class MoveToNextLineWithOffsetSetLeading : IGraphicsStateOperation
	{
		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x060006CB RID: 1739 RVA: 0x00023C03 File Offset: 0x00021E03
		public string Operator
		{
			get
			{
				return "TD";
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x060006CC RID: 1740 RVA: 0x00023C0A File Offset: 0x00021E0A
		public decimal Tx { get; }

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x060006CD RID: 1741 RVA: 0x00023C12 File Offset: 0x00021E12
		public decimal Ty { get; }

		// Token: 0x060006CE RID: 1742 RVA: 0x00023C1A File Offset: 0x00021E1A
		public MoveToNextLineWithOffsetSetLeading(decimal tx, decimal ty)
		{
			this.Tx = tx;
			this.Ty = ty;
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x00023C30 File Offset: 0x00021E30
		public void Run(IOperationContext operationContext)
		{
			new SetTextLeading(-this.Ty).Run(operationContext);
			new MoveToNextLineWithOffset(this.Tx, this.Ty).Run(operationContext);
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x00023C5F File Offset: 0x00021E5F
		public void Write(Stream stream)
		{
			stream.WriteDecimal(this.Tx);
			stream.WriteWhiteSpace();
			stream.WriteDecimal(this.Ty);
			stream.WriteWhiteSpace();
			stream.WriteText("TD", false);
			stream.WriteNewLine();
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x00023C98 File Offset: 0x00021E98
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(2, 3);
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.Tx);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.Ty);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted("TD");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04000350 RID: 848
		public const string Symbol = "TD";
	}
}
