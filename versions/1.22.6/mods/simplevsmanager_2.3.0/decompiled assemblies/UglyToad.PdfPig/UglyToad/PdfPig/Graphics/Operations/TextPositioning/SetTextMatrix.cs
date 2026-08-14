using System;
using System.IO;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Graphics.Operations.TextPositioning
{
	// Token: 0x02000131 RID: 305
	public class SetTextMatrix : IGraphicsStateOperation
	{
		// Token: 0x170001CA RID: 458
		// (get) Token: 0x060006D2 RID: 1746 RVA: 0x00023CF3 File Offset: 0x00021EF3
		public string Operator
		{
			get
			{
				return "Tm";
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x060006D3 RID: 1747 RVA: 0x00023CFA File Offset: 0x00021EFA
		public decimal[] Value { get; }

		// Token: 0x060006D4 RID: 1748 RVA: 0x00023D02 File Offset: 0x00021F02
		public SetTextMatrix(decimal[] value)
		{
			if (value.Length != 6)
			{
				throw new ArgumentException("Text matrix must provide 6 values. Instead got: " + ((value != null) ? value.ToString() : null));
			}
			this.Value = value;
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x00023D34 File Offset: 0x00021F34
		public void Run(IOperationContext operationContext)
		{
			TransformationMatrix transformationMatrix = TransformationMatrix.FromArray(this.Value);
			operationContext.TextMatrices.TextMatrix = transformationMatrix;
			operationContext.TextMatrices.TextLineMatrix = transformationMatrix;
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x00023D68 File Offset: 0x00021F68
		public void Write(Stream stream)
		{
			stream.WriteDecimal(this.Value[0]);
			stream.WriteWhiteSpace();
			stream.WriteDecimal(this.Value[1]);
			stream.WriteWhiteSpace();
			stream.WriteDecimal(this.Value[2]);
			stream.WriteWhiteSpace();
			stream.WriteDecimal(this.Value[3]);
			stream.WriteWhiteSpace();
			stream.WriteDecimal(this.Value[4]);
			stream.WriteWhiteSpace();
			stream.WriteDecimal(this.Value[5]);
			stream.WriteWhiteSpace();
			stream.WriteText("Tm", false);
			stream.WriteNewLine();
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x00023E18 File Offset: 0x00022018
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(6, 7);
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.Value[0]);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.Value[1]);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.Value[2]);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.Value[3]);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.Value[4]);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.Value[5]);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted("Tm");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04000353 RID: 851
		public const string Symbol = "Tm";
	}
}
