using System;
using System.IO;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Graphics.Operations.PathConstruction
{
	// Token: 0x02000148 RID: 328
	public class CloseSubpath : IGraphicsStateOperation
	{
		// Token: 0x170001FA RID: 506
		// (get) Token: 0x0600076C RID: 1900 RVA: 0x00024DDF File Offset: 0x00022FDF
		public string Operator
		{
			get
			{
				return "h";
			}
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x00024DE6 File Offset: 0x00022FE6
		private CloseSubpath()
		{
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x00024DF0 File Offset: 0x00022FF0
		public void Run(IOperationContext operationContext)
		{
			PdfPoint? pdfPoint = operationContext.CloseSubpath();
			if (pdfPoint != null)
			{
				operationContext.CurrentPosition = pdfPoint.Value;
			}
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x00024E1A File Offset: 0x0002301A
		public void Write(Stream stream)
		{
			stream.WriteText("h", false);
			stream.WriteNewLine();
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x00024E2E File Offset: 0x0002302E
		public override string ToString()
		{
			return "h";
		}

		// Token: 0x04000391 RID: 913
		public const string Symbol = "h";

		// Token: 0x04000392 RID: 914
		public static readonly CloseSubpath Value = new CloseSubpath();
	}
}
