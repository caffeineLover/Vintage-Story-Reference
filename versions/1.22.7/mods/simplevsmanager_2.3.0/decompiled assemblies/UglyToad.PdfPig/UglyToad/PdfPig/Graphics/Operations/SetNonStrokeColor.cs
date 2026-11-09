using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UglyToad.PdfPig.Graphics.Operations
{
	// Token: 0x02000115 RID: 277
	public class SetNonStrokeColor : IGraphicsStateOperation
	{
		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000605 RID: 1541 RVA: 0x000222AE File Offset: 0x000204AE
		public string Operator
		{
			get
			{
				return "sc";
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000606 RID: 1542 RVA: 0x000222B5 File Offset: 0x000204B5
		public IReadOnlyList<decimal> Operands { get; }

		// Token: 0x06000607 RID: 1543 RVA: 0x000222BD File Offset: 0x000204BD
		public SetNonStrokeColor(decimal[] operands)
		{
			this.Operands = operands;
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x000222CC File Offset: 0x000204CC
		public void Run(IOperationContext operationContext)
		{
			operationContext.GetCurrentState().ColorSpaceContext.SetNonStrokingColor(this.Operands, null);
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x000222E8 File Offset: 0x000204E8
		public void Write(Stream stream)
		{
			foreach (decimal value in this.Operands)
			{
				stream.WriteDecimal(value);
				stream.WriteWhiteSpace();
			}
			stream.WriteText("sc", false);
			stream.WriteNewLine();
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00022350 File Offset: 0x00020550
		public override string ToString()
		{
			return string.Join(" ", Enumerable.Select<decimal, string>(this.Operands, (decimal x) => x.ToString("N"))) + " sc";
		}

		// Token: 0x040002FC RID: 764
		public const string Symbol = "sc";
	}
}
