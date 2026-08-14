using System;
using System.IO;

namespace UglyToad.PdfPig.Graphics.Operations.PathPainting
{
	// Token: 0x02000138 RID: 312
	public class CloseAndStrokePath : IGraphicsStateOperation
	{
		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x060006FC RID: 1788 RVA: 0x000242F6 File Offset: 0x000224F6
		public string Operator
		{
			get
			{
				return "s";
			}
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x000242FD File Offset: 0x000224FD
		private CloseAndStrokePath()
		{
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x00024305 File Offset: 0x00022505
		public void Run(IOperationContext operationContext)
		{
			operationContext.StrokePath(true);
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x0002430E File Offset: 0x0002250E
		public void Write(Stream stream)
		{
			stream.WriteText("s", false);
			stream.WriteNewLine();
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x00024322 File Offset: 0x00022522
		public override string ToString()
		{
			return "s";
		}

		// Token: 0x04000361 RID: 865
		public const string Symbol = "s";

		// Token: 0x04000362 RID: 866
		public static readonly CloseAndStrokePath Value = new CloseAndStrokePath();
	}
}
