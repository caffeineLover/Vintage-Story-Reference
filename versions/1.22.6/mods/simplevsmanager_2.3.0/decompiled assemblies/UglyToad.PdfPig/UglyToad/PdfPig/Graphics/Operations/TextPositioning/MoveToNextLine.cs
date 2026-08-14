using System;
using System.IO;

namespace UglyToad.PdfPig.Graphics.Operations.TextPositioning
{
	// Token: 0x0200012E RID: 302
	public class MoveToNextLine : IGraphicsStateOperation
	{
		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x060006BE RID: 1726 RVA: 0x00023A88 File Offset: 0x00021C88
		public string Operator
		{
			get
			{
				return "T*";
			}
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x00023A8F File Offset: 0x00021C8F
		private MoveToNextLine()
		{
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x00023A97 File Offset: 0x00021C97
		public void Run(IOperationContext operationContext)
		{
			operationContext.MoveToNextLineWithOffset();
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x00023A9F File Offset: 0x00021C9F
		public void Write(Stream stream)
		{
			stream.WriteText("T*", false);
			stream.WriteNewLine();
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x00023AB3 File Offset: 0x00021CB3
		public override string ToString()
		{
			return "T*";
		}

		// Token: 0x0400034B RID: 843
		public const string Symbol = "T*";

		// Token: 0x0400034C RID: 844
		public static readonly MoveToNextLine Value = new MoveToNextLine();
	}
}
