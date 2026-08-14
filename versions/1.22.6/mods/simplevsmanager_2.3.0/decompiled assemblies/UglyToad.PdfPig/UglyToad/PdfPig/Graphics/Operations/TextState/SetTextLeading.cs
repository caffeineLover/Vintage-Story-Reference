using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Graphics.Operations.TextState
{
	// Token: 0x02000124 RID: 292
	public class SetTextLeading : IGraphicsStateOperation
	{
		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000672 RID: 1650 RVA: 0x00023126 File Offset: 0x00021326
		public string Operator
		{
			get
			{
				return "TL";
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000673 RID: 1651 RVA: 0x0002312D File Offset: 0x0002132D
		public decimal Leading { get; }

		// Token: 0x06000674 RID: 1652 RVA: 0x00023135 File Offset: 0x00021335
		public SetTextLeading(decimal leading)
		{
			this.Leading = leading;
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x00023144 File Offset: 0x00021344
		public void Run(IOperationContext operationContext)
		{
			operationContext.SetTextLeading((double)this.Leading);
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x00023158 File Offset: 0x00021358
		public void Write(Stream stream)
		{
			stream.WriteNumberText(this.Leading, "TL");
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x0002316C File Offset: 0x0002136C
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.Leading);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted("TL");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0400032B RID: 811
		public const string Symbol = "TL";
	}
}
