using System;
using System.IO;

namespace UglyToad.PdfPig.Graphics.Operations.PathPainting
{
	// Token: 0x02000141 RID: 321
	public class StrokePath : IGraphicsStateOperation
	{
		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000732 RID: 1842 RVA: 0x00024533 File Offset: 0x00022733
		public string Operator
		{
			get
			{
				return "S";
			}
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x0002453A File Offset: 0x0002273A
		private StrokePath()
		{
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x00024542 File Offset: 0x00022742
		public void Run(IOperationContext operationContext)
		{
			operationContext.StrokePath(false);
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x0002454B File Offset: 0x0002274B
		public void Write(Stream stream)
		{
			stream.WriteText("S", false);
			stream.WriteNewLine();
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x0002455F File Offset: 0x0002275F
		public override string ToString()
		{
			return "S";
		}

		// Token: 0x04000373 RID: 883
		public const string Symbol = "S";

		// Token: 0x04000374 RID: 884
		public static readonly StrokePath Value = new StrokePath();
	}
}
