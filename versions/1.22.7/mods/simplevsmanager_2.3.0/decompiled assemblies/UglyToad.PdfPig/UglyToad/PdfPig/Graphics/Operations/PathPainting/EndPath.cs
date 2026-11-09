using System;
using System.IO;

namespace UglyToad.PdfPig.Graphics.Operations.PathPainting
{
	// Token: 0x0200013B RID: 315
	public class EndPath : IGraphicsStateOperation
	{
		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x0600070E RID: 1806 RVA: 0x000243B5 File Offset: 0x000225B5
		public string Operator
		{
			get
			{
				return "n";
			}
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x000243BC File Offset: 0x000225BC
		private EndPath()
		{
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x000243C4 File Offset: 0x000225C4
		public void Run(IOperationContext operationContext)
		{
			operationContext.EndPath();
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x000243CC File Offset: 0x000225CC
		public void Write(Stream stream)
		{
			stream.WriteText("n", false);
			stream.WriteNewLine();
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x000243E0 File Offset: 0x000225E0
		public override string ToString()
		{
			return "n";
		}

		// Token: 0x04000367 RID: 871
		public const string Symbol = "n";

		// Token: 0x04000368 RID: 872
		public static readonly EndPath Value = new EndPath();
	}
}
