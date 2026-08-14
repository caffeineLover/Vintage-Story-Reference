using System;
using System.IO;

namespace UglyToad.PdfPig.Graphics.Operations.PathPainting
{
	// Token: 0x02000140 RID: 320
	public class FillPathNonZeroWindingCompatibility : IGraphicsStateOperation
	{
		// Token: 0x170001DC RID: 476
		// (get) Token: 0x0600072C RID: 1836 RVA: 0x000244F3 File Offset: 0x000226F3
		public string Operator
		{
			get
			{
				return "F";
			}
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x000244FA File Offset: 0x000226FA
		private FillPathNonZeroWindingCompatibility()
		{
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x00024502 File Offset: 0x00022702
		public void Run(IOperationContext operationContext)
		{
			operationContext.FillPath(2, false);
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x0002450C File Offset: 0x0002270C
		public void Write(Stream stream)
		{
			stream.WriteText("f", false);
			stream.WriteNewLine();
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x00024520 File Offset: 0x00022720
		public override string ToString()
		{
			return "F";
		}

		// Token: 0x04000371 RID: 881
		public const string Symbol = "F";

		// Token: 0x04000372 RID: 882
		public static readonly FillPathNonZeroWindingCompatibility Value = new FillPathNonZeroWindingCompatibility();
	}
}
