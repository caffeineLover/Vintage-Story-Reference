using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Graphics.Operations.General
{
	// Token: 0x02000152 RID: 338
	public class SetFlatnessTolerance : IGraphicsStateOperation
	{
		// Token: 0x1700020F RID: 527
		// (get) Token: 0x060007B0 RID: 1968 RVA: 0x000253A6 File Offset: 0x000235A6
		public string Operator
		{
			get
			{
				return "i";
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x060007B1 RID: 1969 RVA: 0x000253AD File Offset: 0x000235AD
		public decimal Tolerance { get; }

		// Token: 0x060007B2 RID: 1970 RVA: 0x000253B5 File Offset: 0x000235B5
		public SetFlatnessTolerance(decimal tolerance)
		{
			this.Tolerance = tolerance;
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x000253C4 File Offset: 0x000235C4
		public void Run(IOperationContext operationContext)
		{
			operationContext.SetFlatnessTolerance(this.Tolerance);
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x000253D2 File Offset: 0x000235D2
		public void Write(Stream stream)
		{
			stream.WriteNumberText(this.Tolerance, "i");
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x000253E8 File Offset: 0x000235E8
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.Tolerance);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted("i");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x040003AD RID: 941
		public const string Symbol = "i";
	}
}
