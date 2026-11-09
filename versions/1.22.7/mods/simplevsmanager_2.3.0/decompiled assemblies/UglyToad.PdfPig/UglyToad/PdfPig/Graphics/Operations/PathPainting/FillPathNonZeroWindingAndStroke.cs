using System;
using System.IO;

namespace UglyToad.PdfPig.Graphics.Operations.PathPainting
{
	// Token: 0x0200013F RID: 319
	public class FillPathNonZeroWindingAndStroke : IGraphicsStateOperation
	{
		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000726 RID: 1830 RVA: 0x000244B3 File Offset: 0x000226B3
		public string Operator
		{
			get
			{
				return "B";
			}
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x000244BA File Offset: 0x000226BA
		private FillPathNonZeroWindingAndStroke()
		{
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x000244C2 File Offset: 0x000226C2
		public void Run(IOperationContext operationContext)
		{
			operationContext.FillStrokePath(2, false);
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x000244CC File Offset: 0x000226CC
		public void Write(Stream stream)
		{
			stream.WriteText("B", false);
			stream.WriteNewLine();
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x000244E0 File Offset: 0x000226E0
		public override string ToString()
		{
			return "B";
		}

		// Token: 0x0400036F RID: 879
		public const string Symbol = "B";

		// Token: 0x04000370 RID: 880
		public static readonly FillPathNonZeroWindingAndStroke Value = new FillPathNonZeroWindingAndStroke();
	}
}
