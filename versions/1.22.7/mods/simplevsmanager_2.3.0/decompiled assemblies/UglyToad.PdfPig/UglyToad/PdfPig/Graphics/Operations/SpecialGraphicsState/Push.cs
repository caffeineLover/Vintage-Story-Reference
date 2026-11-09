using System;
using System.IO;

namespace UglyToad.PdfPig.Graphics.Operations.SpecialGraphicsState
{
	// Token: 0x02000136 RID: 310
	public class Push : IGraphicsStateOperation
	{
		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x060006F0 RID: 1776 RVA: 0x00024211 File Offset: 0x00022411
		public string Operator
		{
			get
			{
				return "q";
			}
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x00024218 File Offset: 0x00022418
		private Push()
		{
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x00024220 File Offset: 0x00022420
		public void Run(IOperationContext context)
		{
			context.PushState();
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x00024228 File Offset: 0x00022428
		public void Write(Stream stream)
		{
			stream.WriteText("q", false);
			stream.WriteNewLine();
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x0002423C File Offset: 0x0002243C
		public override string ToString()
		{
			return "q";
		}

		// Token: 0x0400035D RID: 861
		public const string Symbol = "q";

		// Token: 0x0400035E RID: 862
		public static readonly Push Value = new Push();
	}
}
