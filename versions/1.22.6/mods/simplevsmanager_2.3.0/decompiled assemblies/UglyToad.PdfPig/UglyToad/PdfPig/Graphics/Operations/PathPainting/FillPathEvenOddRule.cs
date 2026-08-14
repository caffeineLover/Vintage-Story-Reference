using System;
using System.IO;

namespace UglyToad.PdfPig.Graphics.Operations.PathPainting
{
	// Token: 0x0200013C RID: 316
	public class FillPathEvenOddRule : IGraphicsStateOperation
	{
		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000714 RID: 1812 RVA: 0x000243F3 File Offset: 0x000225F3
		public string Operator
		{
			get
			{
				return "f*";
			}
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x000243FA File Offset: 0x000225FA
		private FillPathEvenOddRule()
		{
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x00024402 File Offset: 0x00022602
		public void Run(IOperationContext operationContext)
		{
			operationContext.FillPath(1, false);
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x0002440C File Offset: 0x0002260C
		public void Write(Stream stream)
		{
			stream.WriteText("f*", false);
			stream.WriteNewLine();
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x00024420 File Offset: 0x00022620
		public override string ToString()
		{
			return "f*";
		}

		// Token: 0x04000369 RID: 873
		public const string Symbol = "f*";

		// Token: 0x0400036A RID: 874
		public static readonly FillPathEvenOddRule Value = new FillPathEvenOddRule();
	}
}
