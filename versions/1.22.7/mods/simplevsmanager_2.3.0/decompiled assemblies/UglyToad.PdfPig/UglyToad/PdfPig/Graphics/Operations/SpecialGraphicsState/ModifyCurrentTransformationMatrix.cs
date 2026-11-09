using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Graphics.Operations.SpecialGraphicsState
{
	// Token: 0x02000134 RID: 308
	public class ModifyCurrentTransformationMatrix : IGraphicsStateOperation
	{
		// Token: 0x170001CE RID: 462
		// (get) Token: 0x060006E4 RID: 1764 RVA: 0x00023FAB File Offset: 0x000221AB
		public string Operator
		{
			get
			{
				return "cm";
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x060006E5 RID: 1765 RVA: 0x00023FB2 File Offset: 0x000221B2
		public decimal[] Value { get; }

		// Token: 0x060006E6 RID: 1766 RVA: 0x00023FBA File Offset: 0x000221BA
		public ModifyCurrentTransformationMatrix(decimal[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (value.Length != 6)
			{
				throw new ArgumentException("The cm operator must pass 6 numbers. Instead got: " + ((value != null) ? value.ToString() : null));
			}
			this.Value = value;
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x00023FFA File Offset: 0x000221FA
		public void Run(IOperationContext operationContext)
		{
			operationContext.ModifyCurrentTransformationMatrix(Array.ConvertAll<decimal, double>(this.Value, (decimal x) => (double)x));
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x0002402C File Offset: 0x0002222C
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
			stream.WriteText("cm", false);
			stream.WriteNewLine();
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x000240DC File Offset: 0x000222DC
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
			defaultInterpolatedStringHandler.AppendFormatted("cm");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04000359 RID: 857
		public const string Symbol = "cm";
	}
}
