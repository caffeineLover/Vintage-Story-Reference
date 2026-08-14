using System;
using System.IO;

namespace UglyToad.PdfPig.Graphics.Operations.PathPainting
{
	// Token: 0x0200013D RID: 317
	public class FillPathEvenOddRuleAndStroke : IGraphicsStateOperation
	{
		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x0600071A RID: 1818 RVA: 0x00024433 File Offset: 0x00022633
		public string Operator
		{
			get
			{
				return "B*";
			}
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x0002443A File Offset: 0x0002263A
		private FillPathEvenOddRuleAndStroke()
		{
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x00024442 File Offset: 0x00022642
		public void Run(IOperationContext operationContext)
		{
			operationContext.FillStrokePath(1, false);
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x0002444C File Offset: 0x0002264C
		public void Write(Stream stream)
		{
			stream.WriteText("B*", false);
			stream.WriteNewLine();
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x00024460 File Offset: 0x00022660
		public override string ToString()
		{
			return "B*";
		}

		// Token: 0x0400036B RID: 875
		public const string Symbol = "B*";

		// Token: 0x0400036C RID: 876
		public static readonly FillPathEvenOddRuleAndStroke Value = new FillPathEvenOddRuleAndStroke();
	}
}
