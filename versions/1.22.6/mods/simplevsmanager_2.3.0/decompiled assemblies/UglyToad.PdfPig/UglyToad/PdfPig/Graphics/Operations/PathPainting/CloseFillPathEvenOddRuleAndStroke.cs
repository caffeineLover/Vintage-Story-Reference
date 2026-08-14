using System;
using System.IO;

namespace UglyToad.PdfPig.Graphics.Operations.PathPainting
{
	// Token: 0x02000139 RID: 313
	public class CloseFillPathEvenOddRuleAndStroke : IGraphicsStateOperation
	{
		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000702 RID: 1794 RVA: 0x00024335 File Offset: 0x00022535
		public string Operator
		{
			get
			{
				return "b*";
			}
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x0002433C File Offset: 0x0002253C
		private CloseFillPathEvenOddRuleAndStroke()
		{
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x00024344 File Offset: 0x00022544
		public void Run(IOperationContext operationContext)
		{
			operationContext.FillStrokePath(1, true);
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x0002434E File Offset: 0x0002254E
		public void Write(Stream stream)
		{
			stream.WriteText("b*", false);
			stream.WriteNewLine();
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x00024362 File Offset: 0x00022562
		public override string ToString()
		{
			return "b*";
		}

		// Token: 0x04000363 RID: 867
		public const string Symbol = "b*";

		// Token: 0x04000364 RID: 868
		public static readonly CloseFillPathEvenOddRuleAndStroke Value = new CloseFillPathEvenOddRuleAndStroke();
	}
}
