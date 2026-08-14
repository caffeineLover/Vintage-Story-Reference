using System;
using System.IO;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Graphics.Operations.TextPositioning
{
	// Token: 0x0200012F RID: 303
	public class MoveToNextLineWithOffset : IGraphicsStateOperation
	{
		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x060006C4 RID: 1732 RVA: 0x00023AC6 File Offset: 0x00021CC6
		public string Operator
		{
			get
			{
				return "Td";
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x060006C5 RID: 1733 RVA: 0x00023ACD File Offset: 0x00021CCD
		public decimal Tx { get; }

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x060006C6 RID: 1734 RVA: 0x00023AD5 File Offset: 0x00021CD5
		public decimal Ty { get; }

		// Token: 0x060006C7 RID: 1735 RVA: 0x00023ADD File Offset: 0x00021CDD
		public MoveToNextLineWithOffset(decimal tx, decimal ty)
		{
			this.Tx = tx;
			this.Ty = ty;
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x00023AF4 File Offset: 0x00021CF4
		public void Run(IOperationContext operationContext)
		{
			TransformationMatrix textLineMatrix = operationContext.TextMatrices.TextLineMatrix;
			TransformationMatrix transformationMatrix = TransformationMatrix.FromValues(1.0, 0.0, 0.0, 1.0, (double)this.Tx, (double)this.Ty).Multiply(textLineMatrix);
			operationContext.TextMatrices.TextLineMatrix = transformationMatrix;
			operationContext.TextMatrices.TextMatrix = transformationMatrix;
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x00023B70 File Offset: 0x00021D70
		public void Write(Stream stream)
		{
			stream.WriteDecimal(this.Tx);
			stream.WriteWhiteSpace();
			stream.WriteDecimal(this.Ty);
			stream.WriteWhiteSpace();
			stream.WriteText("Td", false);
			stream.WriteNewLine();
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x00023BA8 File Offset: 0x00021DA8
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(2, 3);
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.Tx);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.Ty);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted("Td");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0400034D RID: 845
		public const string Symbol = "Td";
	}
}
