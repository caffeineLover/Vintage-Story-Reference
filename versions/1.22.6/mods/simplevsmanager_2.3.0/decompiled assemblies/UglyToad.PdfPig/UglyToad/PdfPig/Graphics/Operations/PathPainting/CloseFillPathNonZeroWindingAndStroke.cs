using System;
using System.IO;

namespace UglyToad.PdfPig.Graphics.Operations.PathPainting
{
	// Token: 0x0200013A RID: 314
	public class CloseFillPathNonZeroWindingAndStroke : IGraphicsStateOperation
	{
		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000708 RID: 1800 RVA: 0x00024375 File Offset: 0x00022575
		public string Operator
		{
			get
			{
				return "b";
			}
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x0002437C File Offset: 0x0002257C
		private CloseFillPathNonZeroWindingAndStroke()
		{
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x00024384 File Offset: 0x00022584
		public void Run(IOperationContext operationContext)
		{
			operationContext.FillStrokePath(2, true);
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x0002438E File Offset: 0x0002258E
		public void Write(Stream stream)
		{
			stream.WriteText("b", false);
			stream.WriteNewLine();
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x000243A2 File Offset: 0x000225A2
		public override string ToString()
		{
			return "b";
		}

		// Token: 0x04000365 RID: 869
		public const string Symbol = "b";

		// Token: 0x04000366 RID: 870
		public static readonly CloseFillPathNonZeroWindingAndStroke Value = new CloseFillPathNonZeroWindingAndStroke();
	}
}
