using System;
using System.IO;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Graphics.Operations.TextObjects
{
	// Token: 0x02000132 RID: 306
	public class BeginText : IGraphicsStateOperation
	{
		// Token: 0x170001CC RID: 460
		// (get) Token: 0x060006D8 RID: 1752 RVA: 0x00023EFB File Offset: 0x000220FB
		public string Operator
		{
			get
			{
				return "BT";
			}
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x00023F02 File Offset: 0x00022102
		private BeginText()
		{
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x00023F0A File Offset: 0x0002210A
		public void Run(IOperationContext operationContext)
		{
			operationContext.TextMatrices.TextMatrix = TransformationMatrix.Identity;
			operationContext.TextMatrices.TextLineMatrix = TransformationMatrix.Identity;
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x00023F2C File Offset: 0x0002212C
		public void Write(Stream stream)
		{
			stream.WriteText("BT", false);
			stream.WriteNewLine();
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x00023F40 File Offset: 0x00022140
		public override string ToString()
		{
			return "BT";
		}

		// Token: 0x04000355 RID: 853
		public const string Symbol = "BT";

		// Token: 0x04000356 RID: 854
		public static readonly BeginText Value = new BeginText();
	}
}
