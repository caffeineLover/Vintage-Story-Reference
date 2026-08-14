using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Graphics.Operations.TextState
{
	// Token: 0x02000127 RID: 295
	public class SetWordSpacing : IGraphicsStateOperation
	{
		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000685 RID: 1669 RVA: 0x000232CE File Offset: 0x000214CE
		public string Operator
		{
			get
			{
				return "Tw";
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000686 RID: 1670 RVA: 0x000232D5 File Offset: 0x000214D5
		public decimal Spacing { get; }

		// Token: 0x06000687 RID: 1671 RVA: 0x000232DD File Offset: 0x000214DD
		public SetWordSpacing(decimal spacing)
		{
			this.Spacing = spacing;
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x000232EC File Offset: 0x000214EC
		public void Run(IOperationContext operationContext)
		{
			operationContext.SetWordSpacing((double)this.Spacing);
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x00023300 File Offset: 0x00021500
		public void Write(Stream stream)
		{
			stream.WriteNumberText(this.Spacing, "Tw");
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x00023314 File Offset: 0x00021514
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.Spacing);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted("Tw");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04000331 RID: 817
		public const string Symbol = "Tw";
	}
}
