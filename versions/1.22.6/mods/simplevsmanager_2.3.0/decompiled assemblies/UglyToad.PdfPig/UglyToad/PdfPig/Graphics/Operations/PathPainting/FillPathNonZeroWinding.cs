using System;
using System.IO;

namespace UglyToad.PdfPig.Graphics.Operations.PathPainting
{
	// Token: 0x0200013E RID: 318
	public class FillPathNonZeroWinding : IGraphicsStateOperation
	{
		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000720 RID: 1824 RVA: 0x00024473 File Offset: 0x00022673
		public string Operator
		{
			get
			{
				return "f";
			}
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x0002447A File Offset: 0x0002267A
		private FillPathNonZeroWinding()
		{
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x00024482 File Offset: 0x00022682
		public void Run(IOperationContext operationContext)
		{
			operationContext.FillPath(2, false);
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x0002448C File Offset: 0x0002268C
		public void Write(Stream stream)
		{
			stream.WriteText("f", false);
			stream.WriteNewLine();
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x000244A0 File Offset: 0x000226A0
		public override string ToString()
		{
			return "f";
		}

		// Token: 0x0400036D RID: 877
		public const string Symbol = "f";

		// Token: 0x0400036E RID: 878
		public static readonly FillPathNonZeroWinding Value = new FillPathNonZeroWinding();
	}
}
