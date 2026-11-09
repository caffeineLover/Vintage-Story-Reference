using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UglyToad.PdfPig.Graphics.Operations
{
	// Token: 0x0200011B RID: 283
	public class SetStrokeColor : IGraphicsStateOperation
	{
		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000632 RID: 1586 RVA: 0x000228EA File Offset: 0x00020AEA
		public string Operator
		{
			get
			{
				return "SC";
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000633 RID: 1587 RVA: 0x000228F1 File Offset: 0x00020AF1
		public IReadOnlyList<decimal> Operands { get; }

		// Token: 0x06000634 RID: 1588 RVA: 0x000228F9 File Offset: 0x00020AF9
		public SetStrokeColor(decimal[] operands)
		{
			this.Operands = operands;
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x00022908 File Offset: 0x00020B08
		public void Run(IOperationContext operationContext)
		{
			operationContext.GetCurrentState().ColorSpaceContext.SetStrokingColor(this.Operands, null);
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x00022924 File Offset: 0x00020B24
		public void Write(Stream stream)
		{
			foreach (decimal value in this.Operands)
			{
				stream.WriteDecimal(value);
				stream.WriteWhiteSpace();
			}
			stream.WriteText("SC", false);
			stream.WriteNewLine();
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x0002298C File Offset: 0x00020B8C
		public override string ToString()
		{
			return string.Join(" ", Enumerable.Select<decimal, string>(this.Operands, (decimal x) => x.ToString("N"))) + " SC";
		}

		// Token: 0x04000310 RID: 784
		public const string Symbol = "SC";
	}
}
